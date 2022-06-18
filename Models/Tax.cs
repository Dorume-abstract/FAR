using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Models
{
    class Tax
    {
        public string Name { get; set; }
        public bool Included { get; set; }
        public decimal Amount { get; set; }
        public string Desc { get; set; }
    }
}
