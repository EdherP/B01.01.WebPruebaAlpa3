using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Vehiculo
    {
        public long U_IdTx { get; set; }
        public Nullable<int> Nro { get; set; }
        public long U_NroDocumento { get; set; }
        public string U_Anio { get; set; }
        public string U_Marca { get; set; }
        public string U_Placa { get; set; }
        public string U_CodEmpresa { get; set; }
        public string U_Empresa { get; set; }
        public Nullable<bool> U_RevisionTecnica { get; set; }
        public string U_RutaRevTecnica { get; set; }
        public Nullable<System.DateTime> U_FechaRevTecnicaInicio { get; set; }
        public Nullable<System.DateTime> U_FechaRevTecnicaFin { get; set; }
        public Nullable<bool> U_ConformeRevTecnica { get; set; }
        public Nullable<bool> U_SOAT { get; set; }
        public Nullable<System.DateTime> U_SOAT_Inicio { get; set; }
        public Nullable<System.DateTime> U_SOAT_Fin { get; set; }
        public string U_RutaSOAT { get; set; }
        public Nullable<bool> U_ConformeSOAT { get; set; }
        public Nullable<bool> U_TarjetaPropiedad { get; set; }
        public Nullable<System.DateTime> U_TarjPropiedadInicio { get; set; }
        public Nullable<System.DateTime> U_TarjPropiedadFin { get; set; }
        public string U_RutaTarjPropiedad { get; set; }
        public Nullable<bool> U_ConformeTarjPropiedad { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public Nullable<System.DateTime> U_FechaRegistro { get; set; }
        public string U_UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> U_FechaModificacion { get; set; }
        //AGREGADO PARA VISUALIZAR DOCs
        public string U_DirRevisionTecnica { get; set; }
        public string U_DirSOAT { get; set; }
        public string U_DirTarjetaPropiedad { get; set; }
    }
}