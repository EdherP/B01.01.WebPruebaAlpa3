using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Pedido_Response_EL
    {

        public class DrivinPedidoResponseEL
        {
            public bool success { get; set; }
            public string status { get; set; }
            public DrivinPedidoResultEL[] response { get; set; }
        }

        public class DrivinPedidoResultEL
        {
            public string code { get; set; }
            public string token { get; set; }
            public string status { get; set; }
        }

    }
}