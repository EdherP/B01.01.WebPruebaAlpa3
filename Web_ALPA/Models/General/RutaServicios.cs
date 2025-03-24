using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.General
{
    public static class RutaServicios
    {
        public static string oUrlServ = ConfigurationManager.AppSettings.Get("oAPI_SERVICIO_ALPA");
        public static string UrlServicio = oUrlServ;
        public static string UrlUbiCeldas = oUrlServ + "/api/Appia/GetUbicacionCeldas";
        public static string UrlUbiStockCeldas = oUrlServ + "/api/Appia/GetUbicacionCeldas_Stock";
        public static string UrlInsertLocStock = oUrlServ + "/api/Appia/PostInsertarProgInventarioFavo_LocacionStock";

        public static string UrlSalidaDeJabas = oUrlServ + "/api/Appia/GetSalidaDeJabas";
        public static string UrlDevolucionVenta = oUrlServ + "/api/Appia/GetDevolucionVenta";
    }
}