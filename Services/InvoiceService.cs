using FAR.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FAR.Services
{
    internal class InvoiceService
    {
        #region Singleton
        private static InvoiceService instance;
        private InvoiceService(){ }
        public static InvoiceService getIstance()
        {
            if(instance == null)
            {
                instance = new InvoiceService();
            }
            return instance;
        }
        #endregion

        private XmlParser xmlParser = new XmlParser();
        
        public async Task<Answer<Invoice[]>> GetInvoice(string path)
        {
            Answer<Invoice[]> answer = await xmlParser.Parse(path);
            return answer;
        }

        //public async Task<Answer<Invoice>> SaveInvoice(string path, params Invoice[] invoce)
        //{
        //    return new Answer<Invoice>();
        //}
    }
}
