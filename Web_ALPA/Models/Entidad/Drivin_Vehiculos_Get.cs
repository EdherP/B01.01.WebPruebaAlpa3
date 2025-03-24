using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Vehiculos_Get
    {

        public class DrivinVehiculosEstado
        {
            public string status { get; set; }
            public DrivinVehiculosResultado[] response { get; set; }
        }

        public class DrivinVehiculosResultado
        {
            public int id { get; set; }
            public string code { get; set; }
            public string description { get; set; }
            public string detail { get; set; }
            public string model { get; set; }
            public int? year { get; set; }
            public object peoneta_id { get; set; }
            public object assistant_2_id { get; set; }
            public object assistant_3_id { get; set; }
            public string device_number { get; set; }
            public int priority { get; set; }
            public object custom_1 { get; set; }
            public object custom_2 { get; set; }
            public bool kinesis_redirect { get; set; }
            public bool is_active { get; set; }
            public float capacity_1 { get; set; }
            public float capacity_2 { get; set; }
            public float capacity_3 { get; set; }
            public float? capacity_4 { get; set; }
            public DateTime shift_start { get; set; }
            public DateTime shift_end { get; set; }
            public int? reload_time { get; set; }
            public float speed_factor { get; set; }
            public string days { get; set; }
            public Driver driver { get; set; }
            public string[] tags { get; set; }
            public object[] cost_allocation_tags { get; set; }
            public string device_type { get; set; }
            public object vehicle_tpye { get; set; }
            public string fleets { get; set; }
        }

        public class Driver
        {
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string phone { get; set; }
            public string dni { get; set; }
        }

    }
}