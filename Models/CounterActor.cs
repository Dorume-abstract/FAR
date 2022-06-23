using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Models
{
    public class CounterActor
    {

        public string CounterActorId { get; set; }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string LegalAdress { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string TIN { get; set; } //TAX 
        public Role Role { get; set; }



    }
}
