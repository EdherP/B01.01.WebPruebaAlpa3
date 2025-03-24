using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_LicenciasSecuritas_Get
    {

        public class DrivinLicenciasSecuritasEstado
        {
            public DrivinLicenciasSecuritasResult[] items { get; set; }
            public int total { get; set; }
            public int offset { get; set; }
            public int limit { get; set; }
        }

        public class DrivinLicenciasSecuritasResult
        {
            public string id { get; set; }
            public string type { get; set; }
            public string name { get; set; }
            public DateTime created { get; set; }
            public DateTime lastModified { get; set; }
            public string description { get; set; }
            public string identifier { get; set; }
        }

    }
}