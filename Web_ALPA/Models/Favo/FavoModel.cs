using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using Web_ALPA.Data;
using Web_ALPA.Models.Entidad;
using Web_ALPA.Models.General;

namespace Web_ALPA.Models.Favo
{
    public class FavoModel
    {
        readonly ConexionAPI oCNx = new ConexionAPI();
        public List<CeldaUbicacion> ListaUbicacionCeldas()
        {
            List<CeldaUbicacion> oubic;
            try
            {
                string responseBody = oCNx.Get_Data(RutaServicios.UrlUbiCeldas);
                oubic = JsonConvert.DeserializeObject<List<CeldaUbicacion>>(responseBody);
            }
            catch
            {
                return null;
            }
            return oubic;
        }
        public List<CeldaUbicacion_Stock> ListaUbicacionCeldas_Stock()
        {
            List<CeldaUbicacion_Stock> oubic;
            try
            {
                string responseBody = oCNx.Get_Data(RutaServicios.UrlUbiStockCeldas);
                oubic = JsonConvert.DeserializeObject<List<CeldaUbicacion_Stock>>(responseBody);
            }
            catch
            {
                return null;
            }
            return oubic;
        }
        public RptaTransaccion RegistrarProgInventario(List<CeldaUbicacion> Docs, bool TipoInventario, string FechaProg = "", string Comentario = "")
        {
            string CodUsuario = HttpContext.Current.Session["CodUsuario"] as string;
            int rpta3 = 1;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter u_NroDocumento = new ObjectParameter("U_NroDocumento", typeof(int));
            string oRpta = string.Empty;
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {
                        if (TipoInventario == false)
                        {
                            string responseBody = oCNx.Get_Data(RutaServicios.UrlUbiCeldas);
                            Docs = JsonConvert.DeserializeObject<List<CeldaUbicacion>>(responseBody);
                        }
                        if (Docs.Count > 0)
                        {
                            rpta3 = dbx.POST_ProgInventarioFavo_Cab(string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(FechaProg)), CodUsuario, "", Comentario, TipoInventario, u_NroDocumento, rpta);
                            if (rpta3 == 1)
                            {
                                foreach (var T in Docs)
                                {
                                    rpta3 = dbx.POST_ProgInventarioFavo_Locacion(Convert.ToInt32(u_NroDocumento.Value), T.CodigoDeCelda, rpta);
                                    if (rpta3 == 0)
                                    {
                                        break;
                                    }
                                }
                                if (rpta3 == 1)
                                {
                                    List<CeldaUbicacion_Stock> oubic = ListaUbicacionCeldas_Stock();
                                    if (oubic.Count > 0)
                                    {
                                        foreach (var T in Docs)
                                        {
                                            if (rpta3 == 1)
                                            {
                                                List<CeldaUbicacion_Stock> oTemp = oubic.Where(x => x.CodigoDeCelda.Trim() == T.CodigoDeCelda.Trim()).ToList();
                                                if (oTemp != null)
                                                {
                                                    if (oTemp.Count > 0)
                                                    {
                                                        foreach (var K in oTemp)
                                                        {
                                                            if (rpta3 == 1)
                                                            {
                                                                rpta3 = dbx.POST_ProgInventarioFavo_LocacionStock(Convert.ToInt32(u_NroDocumento.Value), K.CodigoDeCelda,
                                                                        K.CodigoDeArticulo, K.CodigoDeLote, K.FechaDeCaducidad, Convert.ToDecimal(K.Stock), CodUsuario, rpta);
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (rpta3 == 1)
                                {
                                    transaction.Complete();
                                    xRptaTransaccion.U_Mensaje = "Se registro la programación de inventario N° " + Convert.ToString(u_NroDocumento.Value);
                                    xRptaTransaccion.U_Exito = true;


                                }
                                else
                                {
                                    transaction.Dispose();
                                    xRptaTransaccion.U_Mensaje = "No se Pudo Registrar la Programación de Inventario.";
                                    xRptaTransaccion.U_Exito = false;
                                }
                            }
                            else
                            {
                                transaction.Dispose();
                                xRptaTransaccion.U_Mensaje = "No se Pudo Registrar la Programación de Inventario.";
                                xRptaTransaccion.U_Exito = false;
                            }
                        }
                        else
                        { 
                            xRptaTransaccion.U_Mensaje = "No se Pudo Registrar la Programación de Inventario, error al obtener ubicaciones de FAVO.";
                            xRptaTransaccion.U_Exito = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }

        public List<ProgInventarioFavo_Cab> ListarProgInventario_Cab(string oFechaDesde = "", string oFecHasta = "", int oEstado = 2)
        {
            List<ProgInventarioFavo_Cab> oProgCAB = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    oProgCAB = (db.GET_Listar_ProgInventarioFavo_Cab(string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaDesde)),
                    string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFecHasta)), 0, oEstado).ToList()).ConvertAll(x => new ProgInventarioFavo_Cab
                    {
                        Nro = x.Nro,
                        U_NroDocumento = x.U_NroDocumento,
                        U_FechaRegistro = x.U_FechaRegistro,
                        U_Comentario = x.U_Comentario,
                        U_FechaProgramacion = x.U_FechaProgramacion,
                        U_Parcial = x.U_Parcial,
                        U_UsuarioRegistro = x.U_UsuarioRegistro,
                        U_FechaCierre = x.U_FechaCierre,
                        U_Estado = x.U_Estado,
                        U_UsuarioCierre = x.U_UsuarioCierre,
                        U_Cerrado = x.U_Cerrado,

                    });
                }
            }
            catch
            {
                return null;
            }
            return oProgCAB;
        }

        public List<USP_Web_Listar_ProgInventarioFavo_LS_Result> ListarProgInventario_LS(int oNroDoc)
        {
            List<USP_Web_Listar_ProgInventarioFavo_LS_Result> oProgCAB = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    oProgCAB = db.GET_Listar_ProgInventarioFavo_LS(oNroDoc).ToList();                     
                }
            }
            catch
            {
                return null;
            }
            return oProgCAB;
        }
        public List<USP_Web_Listar_ProgInventarioFavo_Locacion_Result> ListarProgInventario_Locacion(int oNroDoc)
        {
            List<USP_Web_Listar_ProgInventarioFavo_Locacion_Result> oProgCAB = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    oProgCAB = db.GET_Listar_ProgInventarioFavo_Locacion(oNroDoc,"").ToList();
                }
            }
            catch
            {
                return null;
            }
            return oProgCAB;
        }

        public List<USP_Web_Listar_ProgInventarioFavo_Comparativo_Result> ListarProgInventario_Comparativo(int oNroDoc)
        {
            List<USP_Web_Listar_ProgInventarioFavo_Comparativo_Result> oProgCAB = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    oProgCAB = db.GET_Listar_ProgInventarioFavo_Comparativo(oNroDoc).ToList();
                }
            }
            catch
            {
                return null;
            }
            return oProgCAB;
        }

        public List<BE_SalidaDeJabas> ListaRegistrojabas(string oFecha)
        {
            List<BE_SalidaDeJabas> oubic;
            try
            {
                string responseBody = oCNx.Get_Data(RutaServicios.UrlSalidaDeJabas + "?oFecha=" + oFecha);
                oubic = JsonConvert.DeserializeObject<List<BE_SalidaDeJabas>>(responseBody);
            }
            catch
            {
                return null;
            }
            return oubic;
        }

        public List<BE_DevolucionCliente> ListaRegistroDevolucion(string oFecha)
        {
            List<BE_DevolucionCliente> oubic;
            try
            {
                string responseBody = oCNx.Get_Data(RutaServicios.UrlDevolucionVenta + "?oFecha=" + oFecha);
                oubic = JsonConvert.DeserializeObject<List<BE_DevolucionCliente>>(responseBody);
            }
            catch
            {
                return null;
            }
            return oubic;
        }
    }
}