using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_PlacaSecuritas_Get
    {
        public class DrivinPlacaSecuritasEstado
        {
            public DrivinPlacaSecuritasResult[] items { get; set; }
        }

        public class DrivinPlacaSecuritasResult
        {
            public string id { get; set; }
            public Attributes attributes { get; set; }
        }

        public class Attributes
        {
            public string name { get; set; }
            public string type { get; set; }
            public string plateNumber { get; set; }
            public string identifier { get; set; }
        }
    }
}