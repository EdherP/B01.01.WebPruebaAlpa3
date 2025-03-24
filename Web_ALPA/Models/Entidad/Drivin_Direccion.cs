using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Direccion
    {

        public class DrivinDireccion
        {
            public string status { get; set; }
            public DrivinDir[] response { get; set; }
        }

        public class DrivinDir
        {
            public string code { get; set; }
            public string name { get; set; }
            public string client { get; set; }
            public string address_type { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public object zip_code { get; set; }
            public string phone { get; set; }
            public string[] email { get; set; }
            public float? lat { get; set; }
            public float? lng { get; set; }
            public bool georeferenced { get; set; }
            public object service_time { get; set; }
            public object time_window_start { get; set; }
            public object time_window_end { get; set; }
            public object dispatch_date { get; set; }
        }

    }
}