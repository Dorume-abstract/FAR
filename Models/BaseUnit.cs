using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Models
{
    public class BaseUnit
    {

        public string Code { get; set; }
        public string FullName { get; set; }
        public string Unit { get; set; }
        public Requisite[] Requisites { get; set; }


    }
}
