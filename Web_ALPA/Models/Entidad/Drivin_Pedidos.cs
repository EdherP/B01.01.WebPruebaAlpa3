using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Pedidos
    {
        public class DrivinPedidosOk
        {
            public bool success { get; set; }
            public string status { get; set; }
            public DrivinPed[] response { get; set; }
        }

        public class DrivinPed
        {
            public string scenario_token { get; set; }
            public int addresses_count { get; set; }
            public int orders_count { get; set; }
            public int items_count { get; set; }
            public int vehicles_count { get; set; }
            public object[] added { get; set; }
            public string[] edited { get; set; }
            public object[] skipped { get; set; }
        }

    }
}