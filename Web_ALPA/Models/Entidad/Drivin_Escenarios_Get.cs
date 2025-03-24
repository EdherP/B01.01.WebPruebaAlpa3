using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Escenarios_Get
    {
        public class DrivinEscenariosEstado
        {
            public string status { get; set; }
            public bool success { get; set; }
            public DrivinEscenariosResultado[] response { get; set; }
        }

        public class DrivinEscenariosResultado
        {
            public string token { get; set; }
            public string deploy_date { get; set; }
            public string description { get; set; }
            public string status { get; set; }
            public string schema_name { get; set; }
            public string schema_code { get; set; }
            public DateTime created_at { get; set; }
            public object[] children_scenarios { get; set; }
        }
    }
}