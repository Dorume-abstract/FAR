using FAR.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FAR.Services
{
    public class XmlParser
    {
        public Task<Answer<Invoice[]>> Parse(string path)
        {
            XmlDocument doc = GetXmlDoc(path);
            Answer<Invoice[]> answer = new Answer<Invoice[]>();
            Invoice[] invoices = null;
            try
            {
                invoices = GetInvoice(doc);
                answer.Result = Result.Ok;
                answer.Attachment = invoices;
                answer.Description = "Ok";
            }
            catch (Exception e)
            {
                answer.Result = Result.OperationError;
                answer.Exception = e;
                answer.Description = "An error occured while reading the file";
                answer.BecauseOf = nameof(GetInvoice);
                answer.From = nameof(Parse);

            }
            answer.ServiceName = "XmlParser";

            return Task.FromResult(answer);

        }

        private Invoice[] GetInvoice(XmlDocument doc)
        {
            List<Invoice> invoices = new List<Invoice>();
            XmlNodeList documentNodes = doc.GetElementsByTagName("Документ");
            foreach (XmlNode documentNode in documentNodes)
            {
                Invoice invoice = new Invoice();
                invoice.InvoiceId = documentNode.SelectSingleNode("Ид").InnerText;
                invoice.Number = documentNode.SelectSingleNode("Номер").InnerText;
                string date = documentNode.SelectSingleNode("Дата").InnerText;
                string time = documentNode.SelectSingleNode("Время").InnerText;
                invoice.DateTime = DateTime.Parse(date + " " + time);
                invoice.Amount = decimal.Parse(documentNode.SelectSingleNode("Сумма").InnerText,
                    CultureInfo.InvariantCulture); // "34465.26" dot is a problem
                invoice.Comment = documentNode.SelectSingleNode("Комментарий").InnerText;

                foreach (XmlNode counteActor in documentNode.SelectNodes("Контрагенты/Контрагент"))
                {
                    CounterActor actor = new CounterActor()
                    {
                        Name = counteActor.SelectSingleNode("Наименование").InnerText,
                        CounterActorId = counteActor.SelectSingleNode("Ид").InnerText,
                        FullName = counteActor.SelectSingleNode("ПолноеНаименование")?.InnerText,
                        OfficialName = counteActor.SelectSingleNode("ОфициальноеНаименование")?.InnerText,
                        LegalAdress = counteActor.SelectSingleNode("ЮридическийАдрес/Представление")?.InnerText,
                        Code = counteActor.SelectSingleNode("Код")?.InnerText,
                        TIN = counteActor.SelectSingleNode("ИНН")?.InnerText,
                    };
                    if (counteActor.SelectSingleNode("Роль").InnerText == "Продавец")
                    {
                        actor.Role = Role.Seller;
                        invoice.Seller = actor;
                    }
                    else
                    {
                        actor.Role = Role.Buyer;
                        invoice.Buyer = actor;
                    }
                }

                #region Products
                List<Product> products = new List<Product>();
                foreach (XmlNode rawProduct in documentNode.SelectNodes("Товары/Товар"))
                {
                    Product product = new Product();

                    #region SingleFields
                    product.Name = rawProduct.SelectSingleNode("Наименование").InnerText;
                    product.Price = decimal.Parse(rawProduct.SelectSingleNode("ЦенаЗаЕдиницу").InnerText,
                    CultureInfo.InvariantCulture);
                    product.Amount = decimal.Parse(rawProduct.SelectSingleNode("Сумма").InnerText,
                    CultureInfo.InvariantCulture);
                    product.Count = decimal.Parse(rawProduct.SelectSingleNode("Количество").InnerText,
                    CultureInfo.InvariantCulture);
                    product.ProductId = rawProduct.SelectSingleNode("Ид").InnerText;
                    product.BarCode = rawProduct.SelectSingleNode("Штрихкод")?.InnerText;
                    product.CatalogId = rawProduct.SelectSingleNode("ИдКаталога").InnerText;
                    product.GroupId = rawProduct.SelectSingleNode("Группы/Ид").InnerText;
                    product.Unit = rawProduct.SelectSingleNode("Единица").InnerText;
                    #endregion

                    #region Taxes

                    List<Tax> taxes = new List<Tax>();
                    foreach (XmlNode rawTax in rawProduct.SelectNodes("Налоги/Налог"))
                    {
                        Tax tax = new Tax();
                        tax.Name = rawTax.SelectSingleNode("Наименование").InnerText;
                        tax.Included = bool.Parse(rawTax.SelectSingleNode("УчтеноВСумме").InnerText);
                        tax.Amount = decimal.Parse(rawTax.SelectSingleNode("Сумма").InnerText,
                    CultureInfo.InvariantCulture);
                        taxes.Add(tax);
                    }
                    product.Taxes = taxes.ToArray();

                    List<Tax> taxesRate = new List<Tax>();
                    foreach (XmlNode rawTaxRate in rawProduct.SelectNodes("СтавкиНалогов/СтавкаНалога"))
                    {
                        Tax taxRate = new Tax();
                        taxRate.Name = rawTaxRate.SelectSingleNode("Наименование").InnerText;
                        taxRate.Rate = rawTaxRate.SelectSingleNode("Ставка").InnerText;
                        taxesRate.Add(taxRate);
                    }
                    product.Rate = taxesRate.ToArray();

                    #endregion

                    #region BaseUnit
                    BaseUnit baseUnit = new BaseUnit();
                    XmlNode baseUnitNode = rawProduct.SelectSingleNode("БазоваяЕдиница");
                    baseUnit.Unit = baseUnitNode.SelectSingleNode("Пересчет/Единица").InnerText;
                    baseUnit.FullName = baseUnitNode.Attributes["НаименованиеПолное"].Value;
                    baseUnit.Code = baseUnitNode.Attributes["Код"].Value;
                    List<Requisite> requisitesBaseUnit = new List<Requisite>();
                    foreach (XmlNode rawBaseUnit in baseUnitNode.SelectNodes("Пересчет/ДополнительныеДанные/ЗначениеРеквизита"))
                    {
                        Requisite requisite = new Requisite();
                        requisite.Name = rawBaseUnit.SelectSingleNode("Наименование").InnerText;
                        requisite.Value = rawBaseUnit.SelectSingleNode("Значение").InnerText;
                        requisitesBaseUnit.Add(requisite);
                    }
                    baseUnit.Requisites = requisitesBaseUnit.ToArray();
                    product.BaseUnit = baseUnit;
                    #endregion

                    #region Requisites
                    List<Requisite> requisites = new List<Requisite>();
                    foreach (XmlNode rawRequisite in rawProduct.SelectNodes("ЗначенияРеквизитов/ЗначениеРеквизита"))
                    {
                        Requisite requisite = new Requisite();
                        requisite.Name = rawRequisite.SelectSingleNode("Наименование").InnerText;
                        requisite.Value = rawRequisite.SelectSingleNode("Значение").InnerText;
                        requisites.Add(requisite);
                    }
                    product.Requisites = requisites.ToArray();
                    #endregion

                    products.Add(product);
                }
                invoice.Products = products.ToArray();
                #endregion

                #region Warehouse
                Warehouse warehouse = new Warehouse();
                warehouse.WarehouseId = documentNode.SelectSingleNode("Склады/Ид").InnerText;
                warehouse.Name = documentNode.SelectSingleNode("Склады/Наименование").InnerText;
                invoice.Warehouse = warehouse;
                #endregion

                #region TaxesInvoice
                List<Tax> taxesInvoice = new List<Tax>();
                foreach (XmlNode rawTax in documentNode.SelectNodes("Налоги/Налог"))
                {
                    Tax tax = new Tax();
                    tax.Name = rawTax.SelectSingleNode("Наименование").InnerText;
                    tax.Included = bool.Parse(rawTax.SelectSingleNode("УчтеноВСумме").InnerText);
                    tax.Amount = decimal.Parse(rawTax.SelectSingleNode("Сумма").InnerText,
                CultureInfo.InvariantCulture);
                    taxesInvoice.Add(tax);
                }
                invoice.Taxes = taxesInvoice.ToArray(); 
                #endregion

                invoices.Add(invoice);
            }
            return invoices.ToArray();
        }

        private XmlDocument GetXmlDoc(string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            return xDoc;
        }
    }
}
