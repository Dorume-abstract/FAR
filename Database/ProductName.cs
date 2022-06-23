using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FAR.Database
{
    class ProductName
    {
        [Key]
        public int Id { get; set; }
        public string AmbarName { get; set; }
        public string RealName { get; set; }
    }
}

