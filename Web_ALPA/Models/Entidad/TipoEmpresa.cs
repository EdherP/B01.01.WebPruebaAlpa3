using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class TipoEmpresa
    {
        public Nullable<int> Nro { get; set; }
        public int U_CodTipo { get; set; }
        public string U_TipoDesc { get; set; }
        public bool U_FlagActivo { get; set; }
    }
}