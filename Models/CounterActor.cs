using System;
using System.Collections.Generic;
using System.Text;

namespace FAR.Models
{
    class CounterActor
    {

        public string CounterActorId { get; set; }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string LegalAdress { get; set; }

        #region Not reguired
        public string Code { get; set; }
        public string TIN { get; set; } //TAX ID NUMBER
        #endregion

        public Role role { get; set; }



    }
}
