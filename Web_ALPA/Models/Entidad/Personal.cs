using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Personal
    {
        public Nullable<int> Nro { get; set; }
        public long U_IdTx { get; set; }
        public long U_NroDocumento { get; set; }
        public int U_TipoDocumento { get; set; }
        public string U_DescTipoDoc { get; set; }
        public string U_DocIdentidad { get; set; }
        public string U_Nombre { get; set; }
        public string U_Apellidos { get; set; }
        public string U_CodEmpresa { get; set; }
        public string U_Empresa { get; set; }
        public Nullable<bool> U_Cuestionario { get; set; }
        public Nullable<bool> U_VencimientoCuestionario { get; set; }
        public Nullable<System.DateTime> U_FechaCuestionarioInicio { get; set; }
        public Nullable<System.DateTime> U_FechaCuestionarioFin { get; set; }
        public Nullable<bool> U_Vacunacion { get; set; }
        public Nullable<int> U_NroDosis { get; set; }
        public Nullable<bool> U_SCTR { get; set; }
        public Nullable<bool> U_VencimientoSCTR { get; set; }
        public Nullable<System.DateTime> U_FechaSCTRInicio { get; set; }
        public Nullable<System.DateTime> U_FechaSCTRFin { get; set; }
        public Nullable<bool> U_Induccion { get; set; }
        public Nullable<bool> U_LicenciaConducir { get; set; }
        public Nullable<bool> U_VencimientoLConducir { get; set; }
        public Nullable<System.DateTime> U_FechaLicenciaInicio { get; set; }
        public Nullable<System.DateTime> U_FechaLicenciaFin { get; set; }
        public string U_Email { get; set; }
        public Nullable<bool> U_ListaNegra { get; set; }
        public string U_MotivoListaNegra { get; set; }
        public Nullable<System.DateTime> U_FechaListaNegra { get; set; }
        public Nullable<bool> U_Sancionado { get; set; }
        public Nullable<bool> U_VencimientoSancion { get; set; }
        public Nullable<System.DateTime> U_FechaSancionInicio { get; set; }
        public Nullable<System.DateTime> U_Fecha_SancionFin { get; set; }
        public System.DateTime U_FechaRegistro { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public Nullable<System.DateTime> U_FechaModificacion { get; set; }
        public string U_UsuarioModificacion { get; set; }

    }

    public class PersonalAsistencia
    {
        public Nullable<int> Nro { get; set; }
        public long U_NroDocumento { get; set; }
        public string U_CodEmpresa { get; set; }
        public string U_Empresa { get; set; }
        public long U_CodPersonal { get; set; }
        public string TipoDocIdentidad { get; set; }
        public string U_DocIdentidad { get; set; }
        public string U_Nombre { get; set; }
        public string U_Apellidos { get; set; }
        public Nullable<bool> U_Ingreso { get; set; }
        public Nullable<System.DateTime> U_FechaIngreso { get; set; }
        public string FechaIngreso { get; set; }
        public Nullable<System.DateTime> U_HoraIngreso { get; set; }
        public string Hora_Ingreso { get; set; }
        public Nullable<bool> U_Salida { get; set; }
        public Nullable<System.DateTime> U_FechaSalida { get; set; }
        public string FechaSalida { get; set; }
        public Nullable<System.DateTime> U_HoraSalida { get; set; }
        public string Hora_Salida { get; set; }
        public string U_ObservacionIngreso { get; set; }
        public string U_ObservacionSalida { get; set; }
        public string U_TipoAsistencia { get; set; }
        public string U_ConceptoIngreso { get; set; }
        public Nullable<bool> U_EntregaLlave { get; set; }
        public string U_OficinaLlave { get; set; }
        public Nullable<bool> U_DevuelveLlave { get; set; }
        public string U_CodAutorizante { get; set; }
        public string U_MotivoAutorizacion { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public System.DateTime U_FechaRegistro { get; set; }
        public string U_UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> U_FechaModificacion { get; set; }
        public bool U_FlagCancelado { get; set; }

        //Agregado
        public string U_ApellidoNombre { get; set; }
    }
}