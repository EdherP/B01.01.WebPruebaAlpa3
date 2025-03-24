using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Web_ALPA.Data;
using Web_ALPA.Models.Drivin;
using Web_ALPA.Models.Entidad;
using Web_ALPA.ViewModel;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Web_ALPA.Controllers
{
    public class DrivinController : Controller
    {
        readonly ClienteDireccion_Model objAdm;
        readonly ClienteDireccion_Model objCli;
        private RptaTransaccion xRptaTransaccion;

        public DrivinController()
        {
            objAdm = new ClienteDireccion_Model();
            objCli = new ClienteDireccion_Model();
        }

        public class DatosRegistro
        {
            public List<string> Docentry { get; set; }
            public List<string> CheckboxesSeleccionados { get; set; }
            public string oRUC { get; set; }
            public string oCodPlaca { get; set; }
            public string oTiempoServicio { get; set; }
            public string oHoraMinutoInicio { get; set; }
            public string oHoraMinutoFin { get; set; }
            public string oFechaDespacho { get; set; }
            public string oConvoy { get; set; }
            public List<string> numeroguia { get; set; }
            public List<string> Reference { get; set; }
            public int oPrioridad { get; set; }
            public string oConductor { get; set; }
            public string oCustPoNumber { get; set; }
            public string oLineaNegocio { get; set; }
        }

        #region ClienteDireccion

        public ActionResult Drivin_ClienteDireccion()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Empresas";
            ViewBag.Menu = "Ingreso";

            return View();
        }

        public PartialViewResult VP_Drivin_Listar_Clientes()
        {
            List<DRIVIN_Listar_Clientes_Result> ListEmpresas = objCli.Drivin_ListarClientes();
            return PartialView("_VP_Drivin_Listar_Clientes", ListEmpresas);
        }

        public PartialViewResult VP_Drivin_Listar_Direcciones()
        {
            List<DRIVIN_Listar_Direcciones_Result> ListDir = objCli.Drivin_ListarDirecciones();
            return PartialView("_VP_Drivin_Listar_Direcciones", ListDir);
        }

        public PartialViewResult VP_Drivin_Listar_Pedidos(string oAlmacen = "", string oFechaDesde = "", string oFechaHasta = "", int oEstadoDrivin = 0, string oFormasEnvioDrivin = "", string oBusquedaGeneral = "", string oLineaNegocioDrivin = "", string oPlacaGeneral = "")
        {
            List<DRIVIN_SAP_Listar_Pedidos_Result> ListEmpresas = objCli.Drivin_ListarPedidos(oAlmacen, string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaDesde)),
                string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaHasta)), oEstadoDrivin, oFormasEnvioDrivin, oBusquedaGeneral, oLineaNegocioDrivin, oPlacaGeneral);
            return PartialView("_VP_Drivin_Listar_Pedidos", ListEmpresas);            

        }

        public ActionResult Drivin_PedidosPost()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Empresas";
            ViewBag.Menu = "Ingreso";

            return View();
        }

        public PartialViewResult VP_Drivin_Listar_PedidosPost()
        {
            List<DRIVIN_SAP_Listar_Pedidos_Result> ListDir = objCli.Drivin_ListarPedidosPost();
            return PartialView("_VP_Drivin_Listar_PedidosPost", ListDir);
        }

        public PartialViewResult VP_Crear_Placa(string oCodigoPlaca = "")
        {
            List<Drivin_General> ListPlacaDrivin = objAdm.ListarPlacaDrivin("PLACA");
            SelectList lista2 = new SelectList(ListPlacaDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarPlacaDrivin = lista2;

            if (oCodigoPlaca == "")
            {
                {
                    ViewBag.oTipoForm = "A";
                    ViewBag.oTitulo = "Actualizar Datos";
                }                
            }
            return PartialView("_VP_Crear_Placa");
        }

        public PartialViewResult VP_Crear_Placa_Individual(string numeroguia = "", string oCustPoNumber ="", string oLineaNegocio = "", string oCodigoPlaca = "")
        {
            if (oLineaNegocio == "CIANURO")
            {             
            List<Drivin_General> ListPlacaDrivin = objAdm.ListarPlacaDrivin("PLACA");
            SelectList lista2 = new SelectList(ListPlacaDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarPlacaDrivin = lista2;

            List<Drivin_General> ListTransportistaDrivin = objAdm.ListarTransportistaDrivin("TRANSPORTISTA");
            SelectList lista3 = new SelectList(ListTransportistaDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTransportistaDrivin = lista3;
            }
            else
            {
                ViewBag.ListarPlacaDrivin = new SelectList(new List<Drivin_General>());
            }
            //string campo1 = numeroguia;
            ViewBag.numeroguia = numeroguia;
            ViewBag.oCustPoNumber = oCustPoNumber;
            ViewBag.oLineaNegocio = oLineaNegocio;

            if (oCodigoPlaca == "")
            {
                {
                    ViewBag.oTipoForm = "A";
                    ViewBag.oTitulo = "Registrar";
                }
            }
            return PartialView("_VP_Crear_Placa_Individual");
        }
        #endregion

        public JsonResult RegistrarPlaca([FromBody] DatosRegistro datos)
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                List<DRIVIN_SAP_Listar_Pedidos_Result> listaEnt = new List<DRIVIN_SAP_Listar_Pedidos_Result>();

                for (int i = 0; i < datos.Docentry.Count; i++)
                {
                    if (i < datos.CheckboxesSeleccionados.Count)
                    {
                        var Referencia = datos.Reference[i];
                        var Docentry = datos.Docentry[i];
                        var pedido = datos.CheckboxesSeleccionados[i];

                        DRIVIN_SAP_Listar_Pedidos_Result ent = new DRIVIN_SAP_Listar_Pedidos_Result
                        {
                            placa = datos.oCodPlaca,
                            Orden = pedido,
                            Docentry = Docentry,
                            tiempo_servicio = datos.oTiempoServicio,
                            inicio_ventana_horario = datos.oHoraMinutoInicio,
                            fin_ventana_horario = datos.oHoraMinutoFin,
                            delivery_date = datos.oFechaDespacho,
                            reference = Referencia,
                            convoy = datos.oConvoy,
                        };
                        listaEnt.Add(ent);                        
                    }
                }
                xRptaTransaccion = objAdm.RegistrarPlaca(listaEnt);

            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                });
            }
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }

        public JsonResult RegistrarPlacaIndividual([FromBody] DatosRegistro datos)
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                List<DRIVIN_SAP_Listar_Pedidos_Result> listaEnt = new List<DRIVIN_SAP_Listar_Pedidos_Result>();

                DRIVIN_SAP_Listar_Pedidos_Result ent = new DRIVIN_SAP_Listar_Pedidos_Result
                {
                    Docentry = datos.numeroguia.FirstOrDefault(),
                    placa_individual = datos.oCodPlaca,
                    prioridad = datos.oPrioridad == 0 ? (short?)null : (short?)datos.oPrioridad,
                    licencia = datos.oConductor,
                    pedido_origen = datos.oCustPoNumber
                }
                ;
                listaEnt.Add(ent);
                
                xRptaTransaccion = objAdm.RegistrarPlacaIndivudual(listaEnt);

            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                });
            }
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }


        [HttpPost]
        public async Task<JsonResult> Registrar_Drivin_Pedidos(List<DRIVIN_SAP_Listar_Pedidos_Result> oliq)
        {
           
            try
            {
                xRptaTransaccion = new RptaTransaccion();
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                if (oliq != null & oliq.Count > 0)
                {
                    List<DRIVIN_SAP_Listar_Pedidos_Result> oTemp = oliq.Where(x => x.Nro1 == true).ToList();
                    if (oTemp.Count > 0)
                    {
                        //xRptaTransaccion = objAdm.RegistrarPedidosDrivin(oTemp);
                        xRptaTransaccion = await objAdm.RegistrarPedidosDrivinAsync(oTemp); 
                    }
                    else
                    {
                        xRptaTransaccion.U_Exito   = false;
                        xRptaTransaccion.U_Mensaje = "No existen documentos seleccionados a procesar";
                        
                    }
                }

            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                });
            }
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }

        //public JsonResult Registrar_Drivin_Pedidos_1(List<DRIVIN_Listar_Pedidos_Result> oliq, string oCodPlaca = "" )
        public JsonResult Registrar_Drivin_Pedidos_1(List<DRIVIN_SAP_Listar_Pedidos_Result> oliq, string xCodEsquema = "", string xDescripcionEsquema = "", string xEscenario = "", string xCodPlanEscenario = "")
        {

            try
            {
                xRptaTransaccion = new RptaTransaccion();
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                if (oliq != null & oliq.Count > 0)
                {
                    List<DRIVIN_SAP_Listar_Pedidos_Result> oTemp = oliq.Where(x => x.Nro1 == true).ToList();
                    if (oTemp.Count > 0)
                    {

                        string primerToken = xCodPlanEscenario;
                        //string primerToken = oTemp[0].token_escenario_MXT;

                        #region[validar fechas]
                        DateTime fechaDespachoBase = Convert.ToDateTime(oTemp[0].delivery_date);
                        bool fechasDiferentes = oTemp.Any(x => Convert.ToDateTime(x.delivery_date) != fechaDespachoBase);
                        #endregion
                        
                        if (fechasDiferentes)
                        {
                            xRptaTransaccion.U_Exito = false;
                            xRptaTransaccion.U_Mensaje = "Las Fechas seleccionadas son diferentes";
                            
                        }
                        else {
                            xRptaTransaccion = objAdm.RegistrarPedidosDrivin_1(oTemp, xCodEsquema, xDescripcionEsquema, xEscenario, primerToken);
                            //xRptaTransaccion =  objAdm.RegistrarPedidosDrivinAsync(oTemp);
                        }

                    }
                    else
                    {
                        xRptaTransaccion.U_Exito = false;
                        xRptaTransaccion.U_Mensaje = "No existen documentos seleccionados a procesar";

                    }
                }

            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                });
            }
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }

        #region [Eliminar Pedidos]
        public JsonResult VP_Eliminar_Pedido(string oCodOrder = "", string oNumeroGuia = "", string oCustPoNumber = "", string oReferencia = "", string oCodTokenEscenario ="")
        {
            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                xRptaTransaccion = objAdm.EliminarPedidoDrivin(oCodOrder, oNumeroGuia, oCustPoNumber, oReferencia, oCodTokenEscenario);
            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                });
            }
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }
        #endregion

        public ActionResult Drivin_Pedidos()
        {
            List<Drivin_General> ListPlacaDrivin = objAdm.ListarPlacaDrivin("PLACA");
            SelectList lista2 = new SelectList(ListPlacaDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarPlacaDrivin = lista2;

            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Empresas";
            ViewBag.Menu = "Ingreso";

            return View();
        }
        public ActionResult Drivin_Fletes()
        {
            return View();
        }

        #region[Listar Esquema]
        public PartialViewResult VP_Sincronizar(string oCodigoEsquema = "")
        {
            List<Drivin_General> ListEsquemaDrivin = objAdm.ListarEsquemaDrivin("ESQUEMA");
            SelectList lista2 = new SelectList(ListEsquemaDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListEsquemaDrivin = lista2;

            List<Drivin_General> ListPlanesEscenariosDrivin = objAdm.ListarPlanEscenariosDrivin("ESCENARIOS");
            SelectList lista3 = new SelectList(ListPlanesEscenariosDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListPlanesEscenariosDrivin = lista3;

            List<Drivin_General> ListPlacaSecuritasDrivin = Task.Run(async () =>
            {
                return await objAdm.ListarPlacaSecuritasDrivinAsync("PLACASECURITAS").ConfigureAwait(false);
            }).Result;

            SelectList lista4 = new SelectList(ListPlacaSecuritasDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListPlacaSecuritasDrivin = lista4;

            List<Drivin_General> ListLicenciasSecuritasDrivin = Task.Run(async () =>
            {
                return await objAdm.ListarLicenciasSecuritasDrivinAsync("LICENCIASECURITAS").ConfigureAwait(false);
            }).Result;

            SelectList lista5 = new SelectList(ListLicenciasSecuritasDrivin, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListLicenciasSecuritasDrivin = lista5;

            if (oCodigoEsquema == "")
            {
                {
                    ViewBag.oTipoForm = "A";
                    ViewBag.oTitulo = "Seleccionar Esquema";
                }
            }
            return PartialView("_VP_Sincronizar");
        }

        #endregion

        #region[Ver Mapa]
        public PartialViewResult VP_VerMapa(string oCodDireccion = "", string oDireccion = "", string oCity = "", string oState = "", string oCounty = "", string oCountry = "", string oLatitud = "", string oLongitud = "", string oNombreDireccion = "", string oCliente = "", string oCodCliente = "", string oAddressType = "", string oContactName = "")
        {
            ViewBag.xCodDireccion = oCodDireccion;
            ViewBag.xDireccion = oDireccion;
            ViewBag.xCity = oCity;
            ViewBag.xState = oState;
            ViewBag.xCounty = oCounty;
            ViewBag.xCountry = oCountry;
            ViewBag.xLatitud = oLatitud;
            ViewBag.xLongitud = oLongitud;
            ViewBag.xNombreDireccion = oNombreDireccion;
            ViewBag.xCliente = oCliente;
            ViewBag.xCodCliente = oCodCliente;
            ViewBag.xAddressType = oAddressType;
            ViewBag.xContactName = oContactName;


            if (oLatitud != "")
            {
                ViewBag.oLatitud = oLatitud;
            }
            else
            {
                ViewBag.oLatitud = -12.0509469;
            }

            if (oLongitud != "")
            {
                ViewBag.oLongitud = oLongitud;
            }
            else
            {
                ViewBag.oLongitud = -76.9507009;
            }

            if (oLongitud == "" & oLatitud == "")
            {
                ViewBag.xDireccionBusq = oDireccion;
            }
            else
            {
                ViewBag.xDireccionBusq = "";
            }
            return PartialView("_VP_VerMapa");
        }

        #endregion

        #region[Actualizar Direcciones]

        [HttpPost]

        public JsonResult Actualizar_Direcciones_Drivin_Post(string oCodDireccion = "",
                                                     string oDireccion = "",
                                                     string oCity    = "",
                                                     string oState   = "",
                                                     string oCounty  = "",
                                                     string oCountry = "",
                                                     string oLatitud = "", 
                                                     string oLongitud = "",
                                                     string oNombreDireccion = "",
                                                     string oCliente = "",
                                                     string oCodCliente = "",
                                                     string oAddressType = "",
                                                     string oContactName = "")
        {            

            try
            {
                xRptaTransaccion = new RptaTransaccion();
                xRptaTransaccion = objAdm.Actualizar_Direcciones_Drivin(oCodDireccion, oDireccion, oCity, oState, oCounty, oCountry,  oLatitud, oLongitud, oNombreDireccion, oCliente, oCodCliente, oAddressType, oContactName);
            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessTitle = "Error",
                    SuccessMsg = ex.Message,
                });
            }
            // Info  
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }

        #endregion

        // comiteado
    }
}