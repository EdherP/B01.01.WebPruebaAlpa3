using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class General
    {
    }

    public class DocIngresoSAP
    {
        public Nullable<int> U_DocNum_EM { get; set; }
        public Nullable<int> U_DocEntry_EM { get; set; }
        public Nullable<int> U_DocNum_TR { get; set; }
    }
    public class EstadoPedido
    {
        public string U_Code { get; set; }
        public string U_Nombre { get; set; }
    }

    public class SituacionPedido
    {
        public int U_Code { get; set; }
        public string U_Nombre { get; set; }
    }

    public class TipoInventario
    {
        public int U_Code { get; set; }
        public string U_Nombre { get; set; }
    }

    public class RptaRequest
    {
        public int EstadoRpta { get; set; }

        public bool RptaTrans { get; set; }

        public string Rpta { get; set; }
        public bool EnableSuccess { get; set; }
    }

    public class RptaOperacion
    {
        public bool Exito { get; set; }

        public string Rpta { get; set; }

    }
    public class TipoBusquedaInv
    {
        public int U_Code { get; set; }
        public string U_Nombre { get; set; }
    }
    public class TipoStock
    {
        public int U_Code { get; set; }
        public string U_Nombre { get; set; }
    }

    public class DirectorioImg_Productos
    {
        public string U_Archivo { get; set; }
        public string U_NomArchivo { get; set; }
        public string U_extension { get; set; }
        public string U_DireccionImg { get; set; }
        public string U_CarpetaImg { get; set; }
        public string U_CarpetaSAP { get; set; }
    }

    public class RptaTransaccion
    {
        public string U_Titulo { get; set; }
        public string U_Mensaje { get; set; }
        public bool U_Exito { get; set; }
    }
}