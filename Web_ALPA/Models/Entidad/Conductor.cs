using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Conductor
    {
        public Nullable<int> Nro { get; set; }
        public long U_IdTx { get; set; }
        public long U_NroDocumento { get; set; }
        public string U_TipoDesc { get; set; }
        public Nullable<int> U_TipoDocumento { get; set; }
        public string U_DocIdentidad { get; set; }
        public string U_CodEmpresa { get; set; }
        public string U_Empresa { get; set; }
        public string U_Nombre { get; set; }
        public string U_Apellidos { get; set; }
        public Nullable<bool> U_Cuestionario { get; set; }
        public string U_RutaCuestionario { get; set; }
        public Nullable<bool> U_Vacunacion { get; set; }
        public string U_RutaVacunacion { get; set; }
        public Nullable<bool> U_SCTR { get; set; }
        public string U_RutaSCTR { get; set; }
        public Nullable<bool> U_LicenciaConducir { get; set; }
        public string U_RutaLicConducir { get; set; }
        public Nullable<System.DateTime> U_FechaLicenciaInicio { get; set; }
        public Nullable<System.DateTime> U_FechaLicenciaFin { get; set; }
        public Nullable<bool> U_ConformeLicConducir { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public System.DateTime U_FechaRegistro { get; set; }
        public string U_UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> U_FechaModificacion { get; set; }
        public Nullable<bool> U_FlagActivo { get; set; }
        public Nullable<int> U_EstadoDocs { get; set; }
        public string U_ObsEstadoDocs { get; set; }
        public string DescEstadoDocs { get; set; }
        //AGREGADO PARA VISUALIZAR DOCs
        public string U_DirCuestionario { get; set; }
        public string U_DirVacunacion { get; set; }
        public string U_DirSCTR { get; set; }
        public string U_DirLicenciaConducir { get; set; }
        ///////////////
        ///
        public string U_ApellidoNombre { get; set; }
    }
}