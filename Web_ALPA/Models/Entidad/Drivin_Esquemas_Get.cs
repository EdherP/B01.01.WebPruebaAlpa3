using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Esquemas_Get
    {


        public class DrivinEsquemasEstado
        {
            public string status { get; set; }
            public bool success { get; set; }
            public DrivinEsquemasResultado[] response { get; set; }
        }

        public class DrivinEsquemasResultado
        {
            public string code { get; set; }
            public string name { get; set; }
            public bool return_trip { get; set; }
            public bool multiple_trips { get; set; }
            public int? reload_time { get; set; }
            public int service_time { get; set; }
            public bool exclusive { get; set; }
            public int max_speed { get; set; }
            public int optimization_type { get; set; }
            public object custom_settings { get; set; }
            public object fleet { get; set; }
            public Deposit deposit { get; set; }
        }

        public class Deposit
        {
            public string name { get; set; }
            public string code { get; set; }
            public string address_1 { get; set; }
            public float lat { get; set; }
            public float lng { get; set; }
            public string country { get; set; }
            public string city { get; set; }
            public string county { get; set; }
            public string state { get; set; }
        }

    }
}