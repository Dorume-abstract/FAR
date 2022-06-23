using FAR.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FAR.Services
{
    class XmlSave
    {
        public Task<Answer<Object>> Save(string fullPath, Invoice[] invoices)
        {
            if (invoices.Length == 0)
            {
                return Task.FromResult(new Answer<Object>()
                {
                    Result = Result.OperationError,
                    Description = "No elements",
                    Attachment = invoices,
                });
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            XmlWriter writer = XmlWriter.Create(fullPath, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("КоммерческаяИнформация");
            writer.WriteAttributeString("ВерсияСхемы", "2.03");
            writer.WriteAttributeString("ДатаФормирования",
                invoices[0].DateTime.Date.ToString());
            foreach (Invoice invoice in invoices)
            {
                writer.WriteStartElement("Документ");

                writer.WriteStartElement("Ид");
                writer.WriteString(invoice.InvoiceId);
                writer.WriteEndElement();

                writer.WriteStartElement("Номер");
                writer.WriteString(invoice.Number);
                writer.WriteEndElement();

                writer.WriteStartElement("Дата");
                writer.WriteString(invoice.DateTime.ToShortDateString());
                writer.WriteEndElement();

                writer.WriteStartElement("ХозОперация");
                writer.WriteString("Отпуск товара");
                writer.WriteEndElement();

                writer.WriteStartElement("Роль");
                writer.WriteString("Продавец");
                writer.WriteEndElement();

                writer.WriteStartElement("Валюта");
                writer.WriteString("грн");
                writer.WriteEndElement();

                writer.WriteStartElement("Курс");
                writer.WriteString("1");
                writer.WriteEndElement();

                writer.WriteStartElement("Сумма");
                writer.WriteString(invoice.Amount.ToString());
                writer.WriteEndElement();

                #region Counterpartys
                writer.WriteStartElement("Контрагенты");

                #region Seller

                writer.WriteStartElement("Контрагент");
                writer.WriteStartElement("Ид");
                writer.WriteString(invoice.Seller.CounterActorId);
                writer.WriteEndElement();
                writer.WriteStartElement("Наименование");
                writer.WriteString(invoice.Seller.Name);
                writer.WriteEndElement();
                writer.WriteStartElement("ОфициальноеНаименование");
                writer.WriteString(invoice.Seller.OfficialName);
                writer.WriteEndElement();
                writer.WriteStartElement("ЮридическийАдрес");
                writer.WriteStartElement("Представление");
                writer.WriteString(invoice.Seller.LegalAdress);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("Код");
                writer.WriteString(invoice.Seller.Code);
                writer.WriteEndElement();
                writer.WriteStartElement("ИНН");
                writer.WriteString(invoice.Seller.TIN);
                writer.WriteEndElement();
                writer.WriteStartElement("Роль");
                writer.WriteString("Продавец");
                writer.WriteEndElement();
                writer.WriteEndElement();

                #endregion
                #region Buyer

                writer.WriteStartElement("Контрагент");
                writer.WriteStartElement("Ид");
                writer.WriteString(invoice.Buyer.CounterActorId);
                writer.WriteEndElement();
                writer.WriteStartElement("Наименование");
                writer.WriteString(invoice.Buyer.Name);
                writer.WriteEndElement();
                writer.WriteStartElement("ПолноеНаименование");
                writer.WriteString(invoice.Buyer.FullName);
                writer.WriteEndElement();
                writer.WriteStartElement("Роль");
                writer.WriteString("Покупатель");
                writer.WriteEndElement();
                writer.WriteEndElement();

                #endregion
                writer.WriteEndElement();
                #endregion

                #region Warehouse

                writer.WriteStartElement("Склады");
                writer.WriteStartElement("Ид");
                writer.WriteString(invoice.Warehouse.WarehouseId);
                writer.WriteEndElement();
                writer.WriteStartElement("Наименование");
                writer.WriteString(invoice.Warehouse.Name);
                writer.WriteEndElement();
                writer.WriteEndElement();
                #endregion

                writer.WriteStartElement("Время");
                writer.WriteString(invoice.DateTime.ToLongTimeString());
                writer.WriteEndElement();

                writer.WriteStartElement("Налоги");
                foreach (Tax tax in invoice.Taxes)
                {
                    writer.WriteStartElement("Налог");

                    writer.WriteStartElement("Наименование");
                    writer.WriteString(tax.Name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("УчтеноВСумме");
                    writer.WriteString(tax.Included.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Сумма");
                    writer.WriteString(tax.Amount.ToString());
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                }
                writer.WriteEndElement();

                writer.WriteStartElement("Комментарий");
                writer.WriteString(invoice.Comment);
                writer.WriteEndElement();

                #region Products

                writer.WriteStartElement("Товары");
                foreach (Product product in invoice.Products)
                {
                    writer.WriteStartElement("Товар");

                    writer.WriteStartElement("Ид");
                    writer.WriteString(product.ProductId);
                    writer.WriteEndElement();

                    if (!string.IsNullOrEmpty(product.BarCode))
                    {
                        writer.WriteStartElement("Штрихкод");
                        writer.WriteString(product.BarCode);
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("Наименование");
                    writer.WriteString(product.Name);
                    writer.WriteEndElement();

                    #region BaseUnit

                    writer.WriteStartElement("БазоваяЕдиница");
                    writer.WriteAttributeString("Код", product.BaseUnit.Code);
                    writer.WriteAttributeString("НаименованиеПолное", product.BaseUnit.FullName);

                    writer.WriteStartElement("Пересчет");
                    writer.WriteStartElement("Единица");
                    writer.WriteString(product.BaseUnit.Unit);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Коэффициент");
                    writer.WriteString("1");
                    writer.WriteEndElement();

                    writer.WriteStartElement("ДополнительныеДанные");
                    foreach (Requisite requisite in product.BaseUnit.Requisites)
                    {
                        writer.WriteStartElement("ЗначениеРеквизита");
                        writer.WriteStartElement("Наименование");
                        writer.WriteString(requisite.Name);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Значение");
                        writer.WriteString(requisite.Value);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteString(product.BaseUnit.Unit);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Группы");
                    writer.WriteStartElement("Ид");
                    writer.WriteString(product.GroupId);
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("СтавкиНалогов");
                    foreach (Tax rate in product.Rate)
                    {
                        writer.WriteStartElement("СтавкаНалога");
                        writer.WriteStartElement("Наименование");
                        writer.WriteString(rate.Name);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Ставка");
                        writer.WriteString(rate.Rate);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    #endregion

                    /***/
                    writer.WriteStartElement("ЗначенияРеквизитов");
                    foreach (Requisite requisiteInvoice in product.Requisites)
                    {
                        writer.WriteStartElement("ЗначениеРеквизита");
                        writer.WriteStartElement("Наименование");
                        writer.WriteString(requisiteInvoice.Name);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Значение");
                        writer.WriteString(requisiteInvoice.Value);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    /***/
                    writer.WriteStartElement("ИдКаталога");
                    writer.WriteString(product.CatalogId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("ЦенаЗаЕдиницу");
                    writer.WriteString(product.Price.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Количество");
                    writer.WriteString(product.Count.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Сумма");
                    writer.WriteString(product.Amount.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Единица");
                    writer.WriteString(product.Unit.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Коэффициент");
                    writer.WriteString("1");
                    writer.WriteEndElement();
                    /***/
                    writer.WriteStartElement("Налоги");
                    foreach (Tax tax in product.Taxes)
                    {
                        writer.WriteStartElement("Налог");
                        writer.WriteStartElement("Наименование");
                        writer.WriteString(tax.Name);
                        writer.WriteEndElement();
                        writer.WriteStartElement("УчтеноВСумме");
                        writer.WriteString(tax.Included.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("Сумма");
                        writer.WriteString(tax.Amount.ToString());
                        writer.WriteEndElement();
                        writer.WriteEndElement();

                    }
                    writer.WriteEndElement();



                    /***/
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                #endregion


                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Close();

            return Task.FromResult(new Answer<object>()
            {
                Result = Result.Ok,
                Description = "Ok"
            });
        }
    }
}
