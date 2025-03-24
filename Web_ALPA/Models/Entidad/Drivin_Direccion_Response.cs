using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class DrivinDireccionResponse
    {


        public class DireccionResponse
        {
            public bool success { get; set; }
            public string status { get; set; }
            public int? response { get; set; }
        }

    }
}