using FAR.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FAR.Services
{
    internal class XmlParser
    {
        public Task<Answer<Invoice[]>> Parse(string path)
        {
            XmlDocument doc = GetXmlDoc(path);
            Invoice[] invoices = GetInvoice(doc);
            Answer<Invoice[]> answer = new Answer<Invoice[]>
            {
                Result = Result.Ok,
                Attachment = invoices,
                From = nameof(Parse),
                ServiceName = nameof(XmlParser)
            };
            return Task.FromResult(answer);

        }

        private Invoice[] GetInvoice(XmlDocument doc)
        {
            List<Invoice> invoices = new List<Invoice>();
            XmlNodeList documentNodes = doc.GetElementsByTagName("Документ");
            foreach(XmlNode documentNode in documentNodes)
            {
                Invoice invoice = new Invoice();
                XmlNode number = documentNode.SelectSingleNode("Номер");



                invoice.Number = number.InnerText;
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
