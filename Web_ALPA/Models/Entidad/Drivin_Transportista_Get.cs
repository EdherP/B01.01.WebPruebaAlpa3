using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Transportista_Get
    {


        public class DrivinTransportistaEstado
        {
            public string status { get; set; }
            public bool success { get; set; }
            public DrivinTransportistaResultado[] response { get; set; }
        }

        public class DrivinTransportistaResultado
        {
            public string email { get; set; }
            public string phone { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string dni { get; set; }
            public string license_number { get; set; }
            public string description { get; set; }
            public string role_name { get; set; }
            public string organization { get; set; }
            public object employer_name { get; set; }
            public string profile { get; set; }
            public string deposits { get; set; }
            public string fleets { get; set; }
            public string unsuscribed_organizations { get; set; }
            public string users { get; set; }
            public string skills { get; set; }
            public bool is_active { get; set; }
            public string app_version { get; set; }
            public string last_connection { get; set; }
        }

    }
}