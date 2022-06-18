using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Models
{
    internal class Invoice
    {
        public string InvoiceId { get; set; }
        public string Number { get; set; }
        public DateTime DateTime { get; set; } // Merged 2 fields
        public decimal Sum { get; set; }
        public CounterActor Buyer { get; set; }
        public CounterActor Seller { get; set; }
        public Tax[] Taxes { get; set; }
        public string Comment { get; set; }
        public Product[] Products { get; set; }


    }

}
