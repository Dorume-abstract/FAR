using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Models
{
    class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public Tax[] Taxes { get; set; } //Doesn't affect the amount
        public Tax[] Rate { get; set; }
        public string GroupId { get; set; }
        public string CatalogId { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }
        public BaseUnit BaseUnit { get; set; }
        public Requisite[] Requisites { get; set; }
    }
}
