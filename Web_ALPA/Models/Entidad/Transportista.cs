using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Transportista
    {
        public Nullable<int> Nro { get; set; }
        public long U_IdTx { get; set; }
        public int Code { get; set; }
        public string U_CodTransportista { get; set; }
        public string U_CodEmpresa { get; set; }
        public string U_Empresa { get; set; }
        public string U_Transportista { get; set; }
        public string Activo { get; set; }
        public bool U_FlagActivo { get; set; }
        public string U_RUC { get; set; }
        public string U_Contacto { get; set; }
        public string U_FonoContacto { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public Nullable<System.DateTime> U_FechaRegistro { get; set; }
        public string U_UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> U_FechaModificacion { get; set; }
    }
}