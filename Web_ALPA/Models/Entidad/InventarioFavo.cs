using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class InventarioFavo
    {
    }
    public class CeldaUbicacion
    {
        public Nullable<int> Linea { get; set; }
        public string CodigoDeCelda { get; set; }
        public string IDColumna { get; set; }
        public string IDNivel { get; set; }
        public bool CheckLinea { get; set; }

        //campo agregado para consulta

        public bool CheckError { get; set; }
    }
    public class Ubicacion
    { 
        public string CodigoDeCelda { get; set; } 
    }
    public class CeldaUbicacion_Stock
    {
        public string CodigoDeCelda { get; set; }
        public string CodigoDeArticulo { get; set; }
        public string CodigoDeLote { get; set; }
        public Nullable<System.DateTime> FechaDeCaducidad { get; set; }
        public Nullable<double> Stock { get; set; }
    }

    public class ProgInventarioFavo_Cab
    {
        public Nullable<int> Nro { get; set; }
        public long U_NroDocumento { get; set; }
        public string U_FechaProgramacion { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public string U_FechaCierre { get; set; }
        public System.DateTime U_FechaRegistro { get; set; }
        public string U_UsuarioCierre { get; set; }
        public string U_UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> U_FechaModificacion { get; set; }
        public string U_UsuarioCancelacion { get; set; }
        public Nullable<System.DateTime> U_FechaCancelacion { get; set; }
        public string U_Comentario { get; set; }
        public Nullable<bool> U_FlagCancelado { get; set; }
        public Nullable<bool> U_ProgParcial { get; set; }
        public string U_Parcial { get; set; }
        public Nullable<bool> U_Cerrado { get; set; }
        public string U_Estado { get; set; }
    }

    public class ProgInventarioFavo_LS
    {
        public Nullable<int> Nro { get; set; }
        public long U_NroDocumento { get; set; }
        public string U_CodProducto { get; set; }
        public string U_CodUbicacion { get; set; }
        public string U_LoteSerie { get; set; }
        public decimal U_CantOperacion { get; set; }
        public System.DateTime U_Producto_FecVcto { get; set; }
        public string U_UM { get; set; }
        public System.DateTime U_FechaRegistro { get; set; }
        public string U_UsuarioRegistro { get; set; }
    }


    public class BE_SalidaDeJabas
    {
        public string Fecha { get; set; }
        public int Code { get; set; }
        public System.DateTime U_FechaDespacho { get; set; }
        public string U_CodJaba { get; set; }
        public string U_CodCliente { get; set; }
        public string U_NomCliente { get; set; }
        public string U_Ruta { get; set; }
        public string U_Secuencia { get; set; }
        public Nullable<System.DateTime> U_FechaRegistro { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public Nullable<System.DateTime> U_FechaCancelacion { get; set; }
        public string U_UsuarioCancelacion { get; set; }
        public Nullable<bool> U_FlagCancelado { get; set; }
        public string U_BusqGral { get; set; }
    }
    public class BE_DevolucionCliente
    {
        public string Fecha { get; set; }
        public int Code { get; set; }
        public string U_CodArticulo { get; set; }
        public string U_CodBarras { get; set; }
        public string U_DescArticulo { get; set; }
        public System.DateTime U_FechaRegistro { get; set; }
        public string U_UsuarioRegistro { get; set; }
        public Nullable<System.DateTime> U_FechaCancelacion { get; set; }
        public string U_UsuarioCancelacion { get; set; }
        public Nullable<bool> U_FlagCancelado { get; set; }
        public decimal U_Cantidad { get; set; }
    }
}