using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_ALPA.Data;

namespace Web_ALPA.ViewModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Mensaje { get; set; } = "";
        public USP_Web_Listar_DatosInicialesUsuario_Result Datos { get; set; }
    }
}