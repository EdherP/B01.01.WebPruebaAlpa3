using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_ALPA.Data;

namespace Web_ALPA.ViewModel
{
    public class InvFavoViewModel
    {
        public List<USP_Web_Listar_ProgInventarioFavo_LS_Result> ProgInventarioFavo_LS { get; set; }
        public List<USP_Web_Listar_ProgInventarioFavo_Locacion_Result> ProgInventarioFavo_Locacion { get; set; }
        public List<USP_Web_Listar_ProgInventarioFavo_Comparativo_Result> ProgInventarioFavo_Comparativo { get; set; }
    }
}