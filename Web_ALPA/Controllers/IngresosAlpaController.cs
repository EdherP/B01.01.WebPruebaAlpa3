using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Web_ALPA.Data;
using Web_ALPA.Models.IngresosAlpa;
using Web_ALPA.Models.Entidad;
using Web_ALPA.ViewModel;
using Web_ALPA.Models.General;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Web_ALPA.Models.Administracion;

namespace Web_ALPA.Controllers
{
    public class IngresosAlpaController : Controller
    {
        readonly IngresosAlpaModel objAdm;
        readonly AdministracionModel objAdmin;
        readonly ComunModel objGrl;
        private RptaTransaccion xRptaTransaccion;

        public IngresosAlpaController()
        {
            objAdm = new IngresosAlpaModel();
            objAdmin = new AdministracionModel();
            objGrl = new ComunModel();
        }

        #region General
        [HttpPost]
        public JsonResult ValidarDocIdentidad(string oConsulta, string oNroDoc)
        {
            bool oEstado = false;
            string oRUC = string.Empty;
            string oDNI = string.Empty;
            string oRazonSocial = string.Empty;
            string oNombres = string.Empty;
            string oApellidos = "";
            string oUrl = ConfigurationManager.AppSettings.Get("oAPI_SERVICIO_SUNAT");
            try
            {
                if (oConsulta == "RUC" & oNroDoc.ToString().Length == 11)
                {
                    DocRUC oDocRUC = objGrl.Verificar_RUC(oUrl + "ruc?numero=" + oNroDoc);
                    if (oDocRUC != null)
                    {
                        oEstado = true;
                        oRUC = oDocRUC.numeroDocumento ?? "";
                        oRazonSocial = oDocRUC.nombre ?? "";
                    }
                }
                if (oConsulta == "DNI" & oNroDoc.ToString().Length == 8)
                {
                    DocDNI oDocDNI = objGrl.Verificar_DNI(oUrl + "dni?numero=" + oNroDoc);
                    if (oDocDNI != null)
                    {
                        oEstado = true;
                        oDNI = oDocDNI.numeroDocumento ?? "";
                        oNombres = oDocDNI.nombres ?? "";
                        oApellidos = ((oDocDNI.apellidoPaterno + " " + oDocDNI.apellidoMaterno).Trim() ?? "").ToString();
                    }
                }
            }
            catch
            {
                return this.Json(new
                {
                    EnableSuccess = oEstado,
                });
            }
            return this.Json(new
            {
                EnableSuccess = oEstado,
                xRUC = oRUC,
                xDNI = oDNI,
                xRazonSocial = oRazonSocial,
                xNombres = oNombres,
                xApellidos = oApellidos
            });
        }

        #endregion

        #region Empresa
        // GET: IngresosAlpa
        public ActionResult Empresas()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Empresas";
            ViewBag.Menu = "Ingreso";

            return View();
        }

        public ActionResult CrearEmpresa()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Crear Empresa";
            ViewBag.Menu = "Ingreso";
            return View();
        }

        public PartialViewResult VP_Crear_Empresa(int oCodEmp = 0)
        {
            List<TipoEmpresa> ListTipoEmpresa = objAdm.ListarTipoEmpresa(0);
            SelectList lista2 = new SelectList(ListTipoEmpresa, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipo = lista2;

            if (oCodEmp != 0)
            {
                List<Empresa> oMArt = objAdm.ListarEmpresas(oCodEmp);
                if (oMArt.Count == 1)
                {
                    ViewBag.oCodEmpresa = oMArt[0].U_CodEmpresa;
                    ViewBag.oEmpresa = oMArt[0].U_Empresa;
                    ViewBag.oRUC = oMArt[0].U_RUC;
                    ViewBag.oCodTipoEmpresa = oMArt[0].U_CodTipoEmpresa;
                    ViewBag.oContacto = oMArt[0].U_Contacto;
                    ViewBag.oFonoContacto = oMArt[0].U_FonoContacto;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar datos de Empresa";
                    ViewBag.oActivo = oMArt[0].U_FlagActivo;
                    ViewBag.oCode = oMArt[0].Code;
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Creación de Empresa";
            }

            return PartialView("_VP_Crear_Empresa");
        }

        [HttpPost]
        public JsonResult RegistrarEmpresa(string oEmpresa = "", string oRUC = "",
                                       string oContacto = "", int oCodTipoEmpresa = 0, string oFonoContacto = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                Empresa ent = new Empresa
                {

                    U_Empresa = oEmpresa,
                    U_RUC = oRUC,
                    U_Contacto = oContacto ?? "",
                    U_FonoContacto = oFonoContacto ?? "",
                    U_CodTipoEmpresa = oCodTipoEmpresa,

                };
                xRptaTransaccion = objAdm.RegistrarEmpresa(ent);

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

        public PartialViewResult VP_ListarEmpresas()
        {
            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(0);
            return PartialView("_VP_ListarEmpresas", ListEmpresas);
        }

        public ActionResult EditarEmpresa(int oCode = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Empresa";
            ViewBag.Menu = "Ingreso";

            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(oCode);
            return View(ListEmpresas);
        }

        [HttpPost]
        public JsonResult ActualizarEmpresa(int oCode = 0, string oEmpresa = "", string oRUC = "",
                                            string oContacto = "", int oCodTipoEmpresa = 0, string oFonoContacto = "", string oActivoEMP = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();


                bool FlagActivo = false;


                if (oActivoEMP == "on")
                {
                    FlagActivo = true;
                }

                Empresa ent = new Empresa
                {
                    U_Empresa = oEmpresa,
                    U_RUC = oRUC,
                    U_Contacto = oContacto ?? "",
                    U_CodTipoEmpresa = oCodTipoEmpresa,
                    U_FonoContacto = oFonoContacto ?? "",
                    U_FlagActivo = FlagActivo,
                    Code = oCode,
                };
                xRptaTransaccion = objAdm.ActualizarEmpresa(ent);


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

        #region Transportista
        public ActionResult Transportistas()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Transportistas";
            ViewBag.Menu = "Ingreso";

            return View();
        }

        public ActionResult CrearTransportista()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Crear Transportista";
            ViewBag.Menu = "Ingreso";
            return View();
        }

        public PartialViewResult VP_Crear_Transportista(int oCodTra = 0)
        {
            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(0);
            SelectList lista = new SelectList(ListEmpresas, "U_CodEmpresa", "U_Empresa");
            ViewBag.ListarEmpresa = lista;

            if (oCodTra != 0)
            {
                List<Transportista> oMArt = objAdm.ListarTransportistas(oCodTra);
                if (oMArt.Count == 1)
                {
                    ViewBag.oCodTransportista = oMArt[0].U_CodTransportista;
                    ViewBag.oTransportista = oMArt[0].U_Transportista;
                    ViewBag.oRUC = oMArt[0].U_RUC;
                    ViewBag.oCodEmpresa = oMArt[0].U_CodEmpresa;
                    ViewBag.oContacto = oMArt[0].U_Contacto;
                    ViewBag.oFonoContacto = oMArt[0].U_FonoContacto;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar datos de Trasportista";
                    ViewBag.oActivo = oMArt[0].U_FlagActivo;
                    ViewBag.oCode = oMArt[0].Code;
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Creación de Transportista";
            }

            return PartialView("_VP_Crear_Transportista");
        }

        [HttpPost]
        public JsonResult RegistrarTransportista(string oTransportista = "", string oCodTransportista = "", string oRUC = "",
                                              string oContacto = "", string oCodEmpresa = "", string oFonoContacto = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                Transportista ent = new Transportista
                {

                    U_Transportista = oTransportista,
                    U_CodTransportista = oCodTransportista,
                    U_RUC = oRUC,
                    U_Contacto = oContacto ?? "",
                    U_FonoContacto = oFonoContacto ?? "",
                    U_CodEmpresa = oCodEmpresa,
                    U_UsuarioRegistro = CodUsu,

                };
                xRptaTransaccion = objAdm.RegistrarTransportista(ent);

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
        public PartialViewResult VP_ListarTransportistas()
        {
            string oparm = string.Empty;
            string oCodEmpresa = System.Web.HttpContext.Current.Session["CodEmpresa"].ToString() ?? "";
            bool oSuperUsuario = Convert.ToBoolean(System.Web.HttpContext.Current.Session["SuperUsuario"].ToString());
            if (oSuperUsuario == false)
            {
                oparm = oCodEmpresa;
            }

            List<Transportista> ListTransportistas = objAdm.ListarTransportistas(0, "", oparm);
            return PartialView("_VP_ListarTransportistas", ListTransportistas);
        }
        public ActionResult EditarTransportista(int oCode = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Transportista";
            ViewBag.Menu = "Ingreso";

            List<Transportista> ListTransportistas = objAdm.ListarTransportistas(oCode);
            return View(ListTransportistas);
        }
        [HttpPost]
        public JsonResult ActualizarTransportista(int oCode = 0, string oTransportista = "", string oCodTransportista = "", string oRUC = "",
                                                    string oContacto = "", string oCodEmpresa = "", string oFonoContacto = "", string oActivoTRA = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();


                bool FlagActivo = false;


                if (oActivoTRA == "on")
                {
                    FlagActivo = true;
                }

                Transportista ent = new Transportista
                {
                    U_Transportista = oTransportista,
                    U_CodTransportista = oCodTransportista,
                    U_RUC = oRUC,
                    U_Contacto = oContacto ?? "",
                    U_CodEmpresa = oCodEmpresa,
                    U_FonoContacto = oFonoContacto ?? "",
                    U_FlagActivo = FlagActivo,
                    Code = oCode,
                    U_UsuarioModificacion = CodUsu,
                };
                xRptaTransaccion = objAdm.ActualizarTransportista(ent);


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

        #region Vehiculo
        public ActionResult Vehiculos()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Vehiculos";
            ViewBag.Menu = "Ingreso";

            return View();
        }
        public ActionResult CrearVehiculo()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Crear Vehiculo";
            ViewBag.Menu = "Ingreso";
            return View();
        }
        public PartialViewResult VP_Crear_Vehiculo(int oCodVeh = 0)
        {
            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(0);
            SelectList lista = new SelectList(ListEmpresas, "U_CodEmpresa", "U_Empresa");
            ViewBag.ListarEmpresa = lista;

            if (oCodVeh != 0)
            {
                List<Vehiculo> oMArt = objAdm.ListarVehiculos(oCodVeh);
                if (oMArt.Count == 1)
                {
                    ViewBag.oCodEmpresa = oMArt[0].U_CodEmpresa;
                    ViewBag.oPlaca = oMArt[0].U_Placa;
                    ViewBag.oMarca = oMArt[0].U_Marca;
                    ViewBag.oAnio = oMArt[0].U_Anio;
                    ViewBag.oRutaRevTecnica = oMArt[0].U_RutaRevTecnica;
                    ViewBag.oRutaSOAT = oMArt[0].U_RutaSOAT;
                    ViewBag.oRutaTarjPropiedad = oMArt[0].U_RutaTarjPropiedad;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar datos de Vehiculo";
                    ViewBag.oIdTx = oMArt[0].U_IdTx;
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Creación de Vehiculo";
            }

            return PartialView("_VP_Crear_Vehiculo");
        }
        [HttpPost]
        public JsonResult RegistrarVehiculo(HttpPostedFileBase oCustomFile1, HttpPostedFileBase oCustomFile2, HttpPostedFileBase oCustomFile3, string oCodEmpresa = "", string oPlaca = "", string oMarca = "", string oAnio = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                string xFecha = string.Format("{0:HHmmss}", DateTime.Now);
                string oFileRevTecnica = string.Empty;
                string oFileSOAT = string.Empty;
                string oFileTarjPropiedad = string.Empty;
                string sRut = ConfigurationManager.AppSettings.Get("oRutaSaveDocs");

                if (oCustomFile1 != null)
                {
                    oFileRevTecnica = "RevisionTecnica_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile1.SaveAs(sRut + oFileRevTecnica);
                }
                if (oCustomFile2 != null)
                {
                    oFileSOAT = "SOAT_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile2.SaveAs(sRut + oFileSOAT);
                }
                if (oCustomFile3 != null)
                {
                    oFileTarjPropiedad = "TarjetaPropiedad_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile3.SaveAs(sRut + oFileTarjPropiedad);
                }

                Vehiculo ent = new Vehiculo
                {
                    U_CodEmpresa = oCodEmpresa,
                    U_Placa = oPlaca,
                    U_Marca = oMarca,
                    U_Anio = oAnio,
                    U_RutaRevTecnica = oFileRevTecnica,
                    U_RutaSOAT = oFileSOAT,
                    U_RutaTarjPropiedad = oFileTarjPropiedad,
                    U_UsuarioRegistro = CodUsu
                };
                xRptaTransaccion = objAdm.RegistrarVehiculo(ent);

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
        public ActionResult EditarVehiculo(int oCode = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Vehiculo";
            ViewBag.Menu = "Ingreso";

            List<Vehiculo> ListVehiculos = objAdm.ListarVehiculos(oCode);
            return View(ListVehiculos);
        }
        [HttpPost]
        public JsonResult ActualizarVehiculo(HttpPostedFileBase oCustomFile1, HttpPostedFileBase oCustomFile2, HttpPostedFileBase oCustomFile3,
            int oIdTx = 0, string oCodEmpresa = "", string oMarca = "", string oAnio = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                string xFecha = string.Format("{0:HHmmss}", DateTime.Now);
                string oFileRevTecnica = string.Empty;
                string oFileSOAT = string.Empty;
                string oFileTarjPropiedad = string.Empty;
                string sRut = ConfigurationManager.AppSettings.Get("oRutaSaveDocs");

                if (oCustomFile1 != null)
                {
                    oFileRevTecnica = "RevisionTecnica_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile1.SaveAs(sRut + oFileRevTecnica);
                }
                if (oCustomFile2 != null)
                {
                    oFileSOAT = "SOAT_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile2.SaveAs(sRut + oFileSOAT);
                }
                if (oCustomFile3 != null)
                {
                    oFileTarjPropiedad = "TarjetaPropiedad_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile3.SaveAs(sRut + oFileTarjPropiedad);
                }

                Vehiculo ent = new Vehiculo
                {
                    U_NroDocumento = oIdTx,
                    U_CodEmpresa = oCodEmpresa,
                    U_Marca = oMarca,
                    U_Anio = oAnio,
                    U_RutaRevTecnica = oFileRevTecnica,
                    U_RutaSOAT = oFileSOAT,
                    U_RutaTarjPropiedad = oFileTarjPropiedad,
                    U_UsuarioModificacion = CodUsu
                };
                xRptaTransaccion = objAdm.ActualizarVehiculo(ent);


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

        public PartialViewResult VP_ListarVehiculos()
        {
            string oparm = string.Empty;
            string oCodEmpresa = System.Web.HttpContext.Current.Session["CodEmpresa"].ToString() ?? "";
            bool oSuperUsuario = Convert.ToBoolean(System.Web.HttpContext.Current.Session["SuperUsuario"].ToString());
            if (oSuperUsuario == false)
            {
                oparm = oCodEmpresa;
            }

            List<Vehiculo> ListVehiculos = objAdm.ListarVehiculos(0, "", oparm);
            return PartialView("_VP_ListarVehiculos", ListVehiculos);
        }

        #endregion

        #region Ayudante
        public ActionResult Ayudantes()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Ayudantes";
            ViewBag.Menu = "Ingreso";

            return View();
        }
        public ActionResult CrearAyudante()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Crear Ayudante";
            ViewBag.Menu = "Ingreso";
            return View();
        }
        public PartialViewResult VP_Crear_Ayudante(int oCodAyu = 0)
        {
            List<TipoDocumento> ListTipoDocumento = objAdm.ListarTipoDocumento(0);
            SelectList lista2 = new SelectList(ListTipoDocumento, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipo = lista2;

            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(0);
            SelectList lista = new SelectList(ListEmpresas, "U_CodEmpresa", "U_Empresa");
            ViewBag.ListarEmpresa = lista;

            if (oCodAyu != 0)
            {
                List<Ayudante> oMArt = objAdm.ListarAyudantes(oCodAyu);
                if (oMArt.Count == 1)
                {
                    ViewBag.oTipoDocumento = oMArt[0].U_TipoDocumento;
                    ViewBag.oDocIdentidad = oMArt[0].U_DocIdentidad;
                    ViewBag.oNombre = oMArt[0].U_Nombre;
                    ViewBag.oApellidos = oMArt[0].U_Apellidos;
                    ViewBag.oCodEmpresa = oMArt[0].U_CodEmpresa;
                    ViewBag.oRutaCuestionario = oMArt[0].U_RutaCuestionario;
                    ViewBag.oRutaVacunacion = oMArt[0].U_RutaVacunacion;
                    ViewBag.oRutaSCTR = oMArt[0].U_RutaSCTR;
                    ViewBag.oRutaInduccion = oMArt[0].U_RutaInduccion;
                    ViewBag.oActivo = oMArt[0].U_FlagActivo;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar datos de Ayudante";
                    ViewBag.oIdTx = oMArt[0].U_IdTx;
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Creación de Ayudante";
            }

            return PartialView("_VP_Crear_Ayudante");
        }
        [HttpPost]
        public JsonResult RegistrarAyudante(HttpPostedFileBase oCustomFile1, HttpPostedFileBase oCustomFile2, HttpPostedFileBase oCustomFile3, HttpPostedFileBase oCustomFile4, int oTipoDocumento = 0,
                                            string oDocIdentidad = "", string oNombre = "", string oApellidos = "", string oCodEmpresa = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                string xFecha = string.Format("{0:HHmmss}", DateTime.Now);
                string oFileCuestionario = string.Empty;
                string oFileVacunacion = string.Empty;
                string oFileSCTR = string.Empty;
                string oFileInduccion = string.Empty;
                string sRut = ConfigurationManager.AppSettings.Get("oRutaSaveDocs");

                if (oCustomFile1 != null)
                {
                    oFileCuestionario = "Cuestionario_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile1.SaveAs(sRut + oFileCuestionario);
                }
                if (oCustomFile2 != null)
                {
                    oFileVacunacion = "Vacunacion_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile2.SaveAs(sRut + oFileVacunacion);
                }
                if (oCustomFile3 != null)
                {
                    oFileSCTR = "SCTR_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile3.SaveAs(sRut + oFileSCTR);
                }

                if (oCustomFile4 != null)
                {
                    oFileInduccion = "Induccion_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile4.SaveAs(sRut + oFileInduccion);
                }

                Ayudante ent = new Ayudante
                {
                    U_TipoDocumento = oTipoDocumento,
                    U_DocIdentidad = oDocIdentidad,
                    U_Nombre = oNombre,
                    U_Apellidos = oApellidos,
                    U_CodEmpresa = oCodEmpresa,
                    U_RutaVacunacion = oFileVacunacion,
                    U_RutaInduccion = oFileInduccion,
                    U_RutaCuestionario = oFileCuestionario,
                    U_RutaSCTR = oFileSCTR,
                    U_UsuarioRegistro = CodUsu,
                };
                xRptaTransaccion = objAdm.RegistrarAyudante(ent);

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

        public ActionResult EditarAyudante(int oCode = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Ayudante";
            ViewBag.Menu = "Ingreso";

            List<Ayudante> ListAyudantes = objAdm.ListarAyudantes(oCode);
            return View(ListAyudantes);
        }
        [HttpPost]
        public JsonResult ActualizarAyudante(HttpPostedFileBase oCustomFile1, HttpPostedFileBase oCustomFile2, HttpPostedFileBase oCustomFile3, HttpPostedFileBase oCustomFile4,
                                             int oIdTx = 0, string oNombre = "", string oApellidos = "", string oActivoAYU = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                string xFecha = string.Format("{0:HHmmss}", DateTime.Now);
                string oFileCuestionario = string.Empty;
                string oFileVacunacion = string.Empty;
                string oFileSCTR = string.Empty;
                string oFileInduccion = string.Empty;
                string sRut = ConfigurationManager.AppSettings.Get("oRutaSaveDocs");
                bool FlagActivo = false;


                if (oCustomFile1 != null)
                {
                    oFileCuestionario = "Cuestionario_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile1.SaveAs(sRut + oFileCuestionario);
                }
                if (oCustomFile2 != null)
                {
                    oFileVacunacion = "Vacunacion_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile2.SaveAs(sRut + oFileVacunacion);
                }
                if (oCustomFile3 != null)
                {
                    oFileSCTR = "SCTR_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile3.SaveAs(sRut + oFileSCTR);
                }

                if (oCustomFile4 != null)
                {
                    oFileInduccion = "Induccion_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile4.SaveAs(sRut + oFileInduccion);
                }



                if (oActivoAYU == "on")
                {
                    FlagActivo = true;
                }

                Ayudante ent = new Ayudante
                {
                    U_IdTx = oIdTx,
                    U_Nombre = oNombre,
                    U_Apellidos = oApellidos,
                    U_RutaVacunacion = oFileVacunacion,
                    U_RutaInduccion = oFileInduccion,
                    U_RutaCuestionario = oFileCuestionario,
                    U_RutaSCTR = oFileSCTR,
                    U_UsuarioModificacion = CodUsu,
                    U_FlagActivo = FlagActivo,
                };
                xRptaTransaccion = objAdm.ActualizarAyudante(ent);


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

        public JsonResult ActualizarAyudanteValidacion(int oNroDocumento = 0, int oEstadoValidacion = 1, string oObservacionx = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                xRptaTransaccion = objAdm.ActualizarAyudante_Validacion(oNroDocumento, oEstadoValidacion, oObservacionx ?? "", CodUsu);

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

        public PartialViewResult VP_ListarAyudantes()
        {
            string oparm = string.Empty;
            string oCodEmpresa = System.Web.HttpContext.Current.Session["CodEmpresa"].ToString() ?? "";
            bool oSuperUsuario = Convert.ToBoolean(System.Web.HttpContext.Current.Session["SuperUsuario"].ToString());
            if (oSuperUsuario == false)
            {
                oparm = oCodEmpresa;
            }
            List<Ayudante> ListAyudantes = objAdm.ListarAyudantes(0, "", oparm);

            List<TipoInventario> oEstadoValidacion = new List<TipoInventario>
            {
                new TipoInventario() { U_Code = 1, U_Nombre = "PENDIENTE" },
                new TipoInventario() { U_Code = 2, U_Nombre = "OBSERVADO" },
                new TipoInventario() { U_Code = 3, U_Nombre = "CONFORME" }
            };
            SelectList lista2 = new SelectList(oEstadoValidacion, "U_Code", "U_Nombre");
            ViewBag.ListarValidacion = lista2;
            ViewBag.oEstadoValidacion = 1;
            return PartialView("_VP_ListarAyudantes", ListAyudantes);
        }

        #endregion

        #region Conductor
        public ActionResult Conductores()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Conductores";
            ViewBag.Menu = "Ingreso";

            return View();
        }
        public ActionResult CrearConductor()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Crear Conductor";
            ViewBag.Menu = "Ingreso";
            return View();
        }
        public PartialViewResult VP_Crear_Conductor(int oCodCon = 0)
        {
            List<TipoDocumento> ListTipoDocumento = objAdm.ListarTipoDocumento(0);
            SelectList lista2 = new SelectList(ListTipoDocumento, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipo = lista2;

            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(0);
            SelectList lista = new SelectList(ListEmpresas, "U_CodEmpresa", "U_Empresa");
            ViewBag.ListarEmpresa = lista;
            if (oCodCon != 0)
            {
                List<Conductor> oMArt = objAdm.ListarConductores(oCodCon);
                if (oMArt.Count == 1)
                {
                    ViewBag.oTipoDocumento = oMArt[0].U_TipoDocumento;
                    ViewBag.oNombre = oMArt[0].U_Nombre;
                    ViewBag.oDocIdentidad = oMArt[0].U_DocIdentidad;
                    ViewBag.oApellidos = oMArt[0].U_Apellidos;
                    ViewBag.oCodEmpresa = oMArt[0].U_CodEmpresa;
                    ViewBag.oActivo = oMArt[0].U_FlagActivo;
                    ViewBag.oRutaCuestionario = oMArt[0].U_RutaCuestionario;
                    ViewBag.oRutavacunacion = oMArt[0].U_RutaVacunacion;
                    ViewBag.oRutaSCTR = oMArt[0].U_RutaSCTR;
                    ViewBag.oRutaLicConducir = oMArt[0].U_RutaLicConducir;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar datos de Conductor";
                    ViewBag.oIdTx = oMArt[0].U_IdTx;
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Creación de Conductor";
            }

            return PartialView("_VP_Crear_Conductor");
        }
        [HttpPost]
        public JsonResult RegistrarConductor(HttpPostedFileBase oCustomFile1, HttpPostedFileBase oCustomFile2, HttpPostedFileBase oCustomFile3, HttpPostedFileBase oCustomFile4,
            int oTipoDocumento = 0, string oDocIdentidad = "", string oNombre = "", string oApellidos = "", string oCodEmpresa = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                string xFecha = string.Format("{0:HHmmss}", DateTime.Now);
                string oFileCuestionario = string.Empty;
                string oFileVacunacion = string.Empty;
                string oFileSCTR = string.Empty;
                string oFileLicenciaConducir = string.Empty;
                string sRut = ConfigurationManager.AppSettings.Get("oRutaSaveDocs");

                if (oCustomFile1 != null)
                {
                    oFileCuestionario = "Cuestionario_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile1.SaveAs(sRut + oFileCuestionario);
                }
                if (oCustomFile2 != null)
                {
                    oFileVacunacion = "Vacunacion_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile2.SaveAs(sRut + oFileVacunacion);
                }
                if (oCustomFile3 != null)
                {
                    oFileSCTR = "SCTR_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile3.SaveAs(sRut + oFileSCTR);
                }
                if (oCustomFile4 != null)
                {
                    oFileLicenciaConducir = "LicenciaConducir_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile4.SaveAs(sRut + oFileLicenciaConducir);
                }

                Conductor ent = new Conductor
                {
                    U_TipoDocumento = oTipoDocumento,
                    U_DocIdentidad = oDocIdentidad,
                    U_Nombre = oNombre,
                    U_Apellidos = oApellidos,
                    U_CodEmpresa = oCodEmpresa,
                    U_RutaVacunacion = oFileVacunacion,
                    U_RutaCuestionario = oFileCuestionario,
                    U_RutaSCTR = oFileSCTR,
                    U_RutaLicConducir = oFileLicenciaConducir,
                    U_UsuarioRegistro = CodUsu,
                };
                xRptaTransaccion = objAdm.RegistrarConductor(ent);

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

        public ActionResult EditarConductor(int oIdTx = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Conductor";
            ViewBag.Menu = "Ingreso";

            List<Conductor> ListConductores = objAdm.ListarConductores(oIdTx);
            return View(ListConductores);
        }
        [HttpPost]
        public JsonResult ActualizarConductor(HttpPostedFileBase oCustomFile1, HttpPostedFileBase oCustomFile2, HttpPostedFileBase oCustomFile3, HttpPostedFileBase oCustomFile4,
                                             int oIdTx = 0, string oNombre = "", string oApellidos = "", string oActivoCON = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                string xFecha = string.Format("{0:HHmmss}", DateTime.Now);
                string oFileCuestionario = string.Empty;
                string oFileVacunacion = string.Empty;
                string oFileSCTR = string.Empty;
                string oFileLicenciaConducir = string.Empty;
                string sRut = ConfigurationManager.AppSettings.Get("oRutaSaveDocs");
                bool FlagActivo = false;

                if (oCustomFile1 != null)
                {
                    oFileCuestionario = "Cuestionario_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile1.SaveAs(sRut + oFileCuestionario);
                }
                if (oCustomFile2 != null)
                {
                    oFileVacunacion = "Vacunacion_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile2.SaveAs(sRut + oFileVacunacion);
                }
                if (oCustomFile3 != null)
                {
                    oFileSCTR = "SCTR_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile3.SaveAs(sRut + oFileSCTR);
                }

                if (oCustomFile4 != null)
                {
                    oFileLicenciaConducir = "LicenciaConducir_" + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + string.Format("{0:HHmmss}", DateTime.Now) + ".pdf";
                    oCustomFile4.SaveAs(sRut + oFileLicenciaConducir);
                }
                if (oActivoCON == "on")
                {
                    FlagActivo = true;
                }

                Conductor ent = new Conductor
                {
                    U_IdTx = oIdTx,
                    U_Nombre = oNombre,
                    U_Apellidos = oApellidos,
                    U_RutaVacunacion = oFileVacunacion,
                    U_RutaCuestionario = oFileCuestionario,
                    U_RutaSCTR = oFileSCTR,
                    U_RutaLicConducir = oFileLicenciaConducir,
                    U_UsuarioModificacion = CodUsu,
                    U_FlagActivo = FlagActivo
                };
                xRptaTransaccion = objAdm.ActualizarConductor(ent);


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

        public JsonResult ActualizarConductorValidacion(int oNroDocumento = 0, int oEstadoValidacion = 1, string oObservacionx = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                xRptaTransaccion = objAdm.ActualizarConductor_Validacion(oNroDocumento, oEstadoValidacion, oObservacionx ?? "", CodUsu);

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

        public PartialViewResult VP_ListarConductores()
        {
            string oparm = string.Empty;
            string oCodEmpresa = System.Web.HttpContext.Current.Session["CodEmpresa"].ToString() ?? "";
            bool oSuperUsuario = Convert.ToBoolean(System.Web.HttpContext.Current.Session["SuperUsuario"].ToString());
            if (oSuperUsuario == false)
            {
                oparm = oCodEmpresa;
            }
            List<Conductor> ListConductores = objAdm.ListarConductores(0, "", oparm);

            List<TipoInventario> oEstadoValidacion = new List<TipoInventario>
            {
                new TipoInventario() { U_Code = 1, U_Nombre = "PENDIENTE" },
                new TipoInventario() { U_Code = 2, U_Nombre = "OBSERVADO" },
                new TipoInventario() { U_Code = 3, U_Nombre = "CONFORME" }
            };
            SelectList lista2 = new SelectList(oEstadoValidacion, "U_Code", "U_Nombre");
            ViewBag.ListarValidacion = lista2;
            ViewBag.oEstadoValidacion = 1;

            return PartialView("_VP_ListarConductores", ListConductores);
        }

        #endregion

        #region Personal
        public ActionResult Personal()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Personal";
            ViewBag.Menu = "Ingreso";

            return View();
        }
        public ActionResult CrearPersonal()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Crear Personal";
            ViewBag.Menu = "Ingreso";
            return View();
        }
        public PartialViewResult VP_Crear_Personal(int oCodPer = 0)
        {
            List<TipoDocumento> ListTipoDocumento = objAdm.ListarTipoDocumento(0);
            SelectList lista2 = new SelectList(ListTipoDocumento, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipo = lista2;

            List<Empresa> ListEmpresas = objAdm.ListarEmpresas(0);
            SelectList lista3 = new SelectList(ListEmpresas, "U_CodEmpresa", "U_Empresa");
            ViewBag.ListarEmpresa = lista3;

            if (oCodPer != 0)
            {
                List<Personal> oMArt = objAdm.ListarPersonal(oCodPer);
                if (oMArt.Count == 1)
                {
                    ViewBag.oTipoDocumento = oMArt[0].U_TipoDocumento;
                    ViewBag.oNombre = oMArt[0].U_Nombre;
                    ViewBag.oDocIdentidad = oMArt[0].U_DocIdentidad;
                    ViewBag.oApellidos = oMArt[0].U_Apellidos;
                    ViewBag.oCodEmpresa = oMArt[0].U_CodEmpresa;
                    ViewBag.oActivoCuestionario = oMArt[0].U_Cuestionario;
                    ViewBag.oFechaCuestionarioInicio = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaCuestionarioInicio);
                    ViewBag.oFechaCuestionarioFin = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaCuestionarioFin);
                    ViewBag.oActivoVacuna = oMArt[0].U_Vacunacion;
                    ViewBag.oActivoSCTR = oMArt[0].U_SCTR;
                    ViewBag.oNroDosis = oMArt[0].U_NroDosis;
                    ViewBag.oFechaSCTRInicio = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaSCTRInicio);
                    ViewBag.oFechaSCTRFin = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaSCTRFin);
                    ViewBag.oInduccion = oMArt[0].U_Induccion;
                    ViewBag.oActivoLicenciaConducir = oMArt[0].U_LicenciaConducir;
                    ViewBag.oFechaLicenciaInicio = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaLicenciaInicio);
                    ViewBag.oFechaLicenciaFin = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaLicenciaFin);
                    ViewBag.oEmail = oMArt[0].U_Email;
                    ViewBag.oActivoListaNegra = oMArt[0].U_ListaNegra;
                    ViewBag.oMotivoListaNegra = oMArt[0].U_MotivoListaNegra;
                    ViewBag.oFechaListaNegra = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaListaNegra);
                    ViewBag.oActivoSancionado = oMArt[0].U_Sancionado;
                    ViewBag.oFechaSancionInicio = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_FechaSancionInicio);
                    ViewBag.oFechaSancionFin = string.Format("{0:yyyy-MM-dd}", oMArt[0].U_Fecha_SancionFin);
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar datos de Personal";
                    ViewBag.oIdTx = oMArt[0].U_IdTx;
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Creación de Personal";
            }

            return PartialView("_VP_Crear_Personal");
        }
        [HttpPost]
        public JsonResult RegistrarPersonal(int oTipoDocumento = 0, string oDocIdentidad = "", string oNombre = "",
                                       string oApellidos = "", string oCodEmpresa = "", string oFechaCuestionarioInicio = "", string oFechaCuestionarioFin = "",
                                       string oVacunacion = "", string oFechaSCTRInicio = "", string oFechaSCTRFin = "", string oInduccion = "", string oFechaLicenciaInicio = "",
                                       string oFechaLicenciaFin = "", string oEmail = "", string oFechaListaNegra = "", string oListaNegra = "", string oMotivoListaNegra = "",
                                         string oSancionado = "", string oFechaSancionInicio = "", string oFechaSancionFin = "", string oCuestionario = "",
                                         int oNroDosis = 0, string oSCTR = "", string oLicenciaConducir = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                bool xVacunacion = false;
                bool xInduccion = false;
                bool xListaNegra = false;
                bool xSancionado = false;
                bool xCuestionario = false;
                bool xSCTR = false;
                bool xLicenciaConducir = false;
                if (oVacunacion == "on") { xVacunacion = true; }
                if (oInduccion == "on") { xInduccion = true; }
                if (oListaNegra == "on") { xListaNegra = true; }
                if (oSancionado == "on") { xSancionado = true; }
                if (oCuestionario == "on") { xCuestionario = true; }
                if (oSCTR == "on") { xSCTR = true; }
                if (oLicenciaConducir == "on") { xLicenciaConducir = true; }


                Personal ent = new Personal
                {
                    U_TipoDocumento = oTipoDocumento,
                    U_DocIdentidad = oDocIdentidad,
                    U_Nombre = oNombre,
                    U_Apellidos = oApellidos,
                    U_CodEmpresa = oCodEmpresa,
                    U_FechaCuestionarioInicio = oFechaCuestionarioInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaCuestionarioInicio),
                    U_FechaCuestionarioFin = oFechaCuestionarioFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaCuestionarioFin),
                    U_Vacunacion = xVacunacion,
                    U_FechaSCTRInicio = oFechaSCTRInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSCTRInicio),
                    U_FechaSCTRFin = oFechaSCTRFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSCTRFin),
                    U_Induccion = xInduccion,
                    U_FechaLicenciaInicio = oFechaLicenciaInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaLicenciaInicio),
                    U_FechaLicenciaFin = oFechaLicenciaFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaLicenciaFin),
                    U_Email = oEmail,
                    U_FechaListaNegra = oFechaListaNegra == "" ? (DateTime?)null : Convert.ToDateTime(oFechaListaNegra),
                    U_ListaNegra = xListaNegra,
                    U_MotivoListaNegra = oMotivoListaNegra,
                    U_Sancionado = xSancionado,
                    U_FechaSancionInicio = oFechaSancionInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSancionInicio),
                    U_Fecha_SancionFin = oFechaSancionFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSancionFin),
                    U_Cuestionario = xCuestionario,
                    U_NroDosis = oNroDosis,
                    U_SCTR = xSCTR,
                    U_LicenciaConducir = xLicenciaConducir,
                    U_UsuarioRegistro = CodUsu
                };
                xRptaTransaccion = objAdm.RegistrarPersonal(ent);

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

        public ActionResult EditarPersonal(int oIdTx = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Personal";
            ViewBag.Menu = "Ingreso";

            List<Personal> ListPersonal = objAdm.ListarPersonal(oIdTx);
            return View(ListPersonal);
        }
        [HttpPost]
        public JsonResult ActualizarPersonal(int oIdTx = 0, string oNombre = "",
                                       string oApellidos = "", string oCodEmpresa = "", string oFechaCuestionarioInicio = "", string oFechaCuestionarioFin = "",
                                       string oVacunacion = "", string oFechaSCTRInicio = "", string oFechaSCTRFin = "", string oInduccion = "", string oFechaLicenciaInicio = "",
                                       string oFechaLicenciaFin = "", string oEmail = "", string oFechaListaNegra = "", string oListaNegra = "", string oMotivoListaNegra = "",
                                         string oSancionado = "", string oFechaSancionInicio = "", string oFechaSancionFin = "", string oCuestionario = "",
                                         int oNroDosis = 0, string oSCTR = "", string oLicenciaConducir = "")
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                bool xVacunacion = false;
                bool xInduccion = false;
                bool xListaNegra = false;
                bool xSancionado = false;
                bool xCuestionario = false;
                bool xSCTR = false;
                bool xLicenciaConducir = false;
                if (oVacunacion == "on")
                {
                    xVacunacion = true;
                }
                if (oInduccion == "on")
                {
                    xInduccion = true;
                }
                if (oListaNegra == "on")
                {
                    xListaNegra = true;
                }
                if (oSancionado == "on")
                {
                    xSancionado = true;
                }
                if (oCuestionario == "on")
                {
                    xCuestionario = true;
                }
                if (oSCTR == "on")
                {
                    xSCTR = true;
                }
                if (oLicenciaConducir == "on")
                {
                    xLicenciaConducir = true;
                }

                Personal ent = new Personal
                {
                    U_IdTx = oIdTx,
                    U_Nombre = oNombre,
                    U_Apellidos = oApellidos,
                    U_CodEmpresa = oCodEmpresa,
                    U_FechaCuestionarioInicio = oFechaCuestionarioInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaCuestionarioInicio),
                    U_FechaCuestionarioFin = oFechaCuestionarioFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaCuestionarioFin),
                    U_Vacunacion = xVacunacion,
                    U_FechaSCTRInicio = oFechaSCTRInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSCTRInicio),
                    U_FechaSCTRFin = oFechaSCTRFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSCTRFin),
                    U_Induccion = xInduccion,
                    U_FechaLicenciaInicio = oFechaLicenciaInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaLicenciaInicio),
                    U_FechaLicenciaFin = oFechaLicenciaFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaLicenciaFin),
                    U_Email = oEmail,
                    U_FechaListaNegra = oFechaListaNegra == "" ? (DateTime?)null : Convert.ToDateTime(oFechaListaNegra),
                    U_ListaNegra = xListaNegra,
                    U_MotivoListaNegra = oMotivoListaNegra,
                    U_Sancionado = xSancionado,
                    U_FechaSancionInicio = oFechaSancionInicio == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSancionInicio),
                    U_Fecha_SancionFin = oFechaSancionFin == "" ? (DateTime?)null : Convert.ToDateTime(oFechaSancionFin),
                    U_Cuestionario = xCuestionario,
                    U_NroDosis = oNroDosis,
                    U_SCTR = xSCTR,
                    U_LicenciaConducir = xLicenciaConducir,
                    U_UsuarioModificacion = CodUsu
                };
                xRptaTransaccion = objAdm.ActualizarPersonal(ent);


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


        public PartialViewResult VP_ListarPersonal()
        {
            List<Personal> ListPersonal = objAdm.ListarPersonal(0, "");
            return PartialView("_VP_ListarPersonal", ListPersonal);
        }

        #endregion

        #region IngresosPersonal
        public ActionResult IngresosPersonal(string oFechaDesde = "", string oFechaHasta = "")
        {
            List<USP_Web_Listar_TipoIngreso_Result> ListTipo = objAdm.ListarTipoIngreso();
            SelectList lista3 = new SelectList(ListTipo, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipoIngreso = lista3;

            List<Usuario> ListUsuarios = objAdmin.ListarUsuarios(0);
            List<Usuario> ListAprobador = ListUsuarios.Where(x => x.U_AutorizaIngreso == true).ToList();
            SelectList lista4 = new SelectList(ListAprobador, "CodUsuario", "U_ApellidoNombre");
            ViewBag.ListarAprobador = lista4;

            if (oFechaDesde != "" & oFechaHasta != "")
            {
                ViewBag.oFechaDesde = string.Format("{0:yyyy-MM-dd}", oFechaDesde);
                ViewBag.oFechaHasta = string.Format("{0:yyyy-MM-dd}", oFechaHasta);
            }
            else
            {
                ViewBag.oFechaDesde = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                ViewBag.oFechaHasta = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            return View();
        }

        public PartialViewResult VP_ListarIngresoPersonal(string oFechaDesde = "", string oFechaHasta = "")
        {
            List<TipoInventario> oEstadoPr = new List<TipoInventario>
            {
                //new TipoInventario() { U_Code = 2, U_Nombre = "" },
                new TipoInventario() { U_Code = 0, U_Nombre = "No entrega llave" },
                new TipoInventario() { U_Code = 1, U_Nombre = "Entrega llave" }
            };
            SelectList lista2 = new SelectList(oEstadoPr, "U_Code", "U_Nombre");
            ViewBag.ListarEstadodev = lista2;
            ViewBag.oDevuelvellavex = 0;

            List<PersonalAsistencia> ListPersonal = objAdm.ListarPersonalAsistencia(string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaDesde)),
                string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaHasta)));
            return PartialView("_VP_ListarIngresoPersonal", ListPersonal);
        }

        public JsonResult ConsultarPersonal(string oBusqueda = "")
        {
            bool xEnableSuccess = true;
            string oCodEmpresa = string.Empty;
            string oEmpresa = string.Empty;
            string oNombre = string.Empty;
            string oApellido = string.Empty;
            string oTipoDoc = string.Empty;
            bool oCuestionario = false;
            bool oVencCuestionario = false;
            bool oVacunado = false;
            bool oSCTR = false;
            bool oVencSCTR = false;
            bool oInduccion = false;
            bool oLicencia = false;
            bool oVencLicencia = false;
            bool oListaNegra = false;
            bool oSancionado = false;
            bool oVencSancionado = false;
            int oCode = 0;
            try
            {
                List<Personal> ListPersonal = objAdm.ListarPersonal(0, oBusqueda);
                if (ListPersonal.Count == 1)
                {
                    oCodEmpresa = ListPersonal[0].U_CodEmpresa;
                    oEmpresa = ListPersonal[0].U_Empresa;
                    oNombre = ListPersonal[0].U_Nombre;
                    oApellido = ListPersonal[0].U_Apellidos;
                    oCuestionario = Convert.ToBoolean(ListPersonal[0].U_Cuestionario);
                    oVencCuestionario = Convert.ToBoolean(ListPersonal[0].U_VencimientoCuestionario);
                    oVacunado = Convert.ToBoolean(ListPersonal[0].U_Vacunacion);
                    oSCTR = Convert.ToBoolean(ListPersonal[0].U_SCTR);
                    oVencSCTR = Convert.ToBoolean(ListPersonal[0].U_VencimientoSCTR);
                    oInduccion = Convert.ToBoolean(ListPersonal[0].U_Induccion);
                    oLicencia = Convert.ToBoolean(ListPersonal[0].U_LicenciaConducir);
                    oVencLicencia = Convert.ToBoolean(ListPersonal[0].U_VencimientoLConducir);
                    oListaNegra = Convert.ToBoolean(ListPersonal[0].U_ListaNegra);
                    oSancionado = Convert.ToBoolean(ListPersonal[0].U_Sancionado);
                    oVencSancionado = Convert.ToBoolean(ListPersonal[0].U_VencimientoSancion);
                    oTipoDoc = ListPersonal[0].U_DescTipoDoc;
                    oCode = Convert.ToInt32(ListPersonal[0].U_IdTx);
                }
                else
                {
                    xEnableSuccess = false;
                }
            }
            catch
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                });
            }
            return this.Json(new
            {
                EnableSuccess = xEnableSuccess,
                TipoDoc = oTipoDoc,
                CodEmpresa = oCodEmpresa,
                Empresa = oEmpresa,
                Nombre = oNombre,
                Apellido = oApellido,
                Cuestionario = oCuestionario,
                VencCuestionario = oVencCuestionario,
                Vacunado = oVacunado,
                SCTR = oSCTR,
                VencSCTR = oVencSCTR,
                Induccion = oInduccion,
                Licencia = oLicencia,
                VencLicencia = oVencLicencia,
                ListaNegra = oListaNegra,
                Sancionado = oSancionado,
                VencSancionado = oVencSancionado,
                Code = oCode
            });

        }

        [HttpPost]
        public JsonResult RegistrarPersonalAsistencia(string oCodEmpresax = "", int oIDx = 0, int oConceptoingreso = 0, bool oEntregallave = false,
                                                      string oOficinallave = "", string oCodAutorizante = "", string oMotivoAutorizacion = "", string oObsIngreso = "")
        {
            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                xRptaTransaccion = objAdm.RegistrarAsistenciaPersonal(oCodEmpresax, oIDx, oConceptoingreso, oEntregallave, oOficinallave,
                                                                      oCodAutorizante, oMotivoAutorizacion, oObsIngreso ?? "", CodUsu);

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
        public JsonResult ActualizarPersonalAsistencia_Salida(int oNroDocumento = 0, string oObservacionSalidax = "", int oDevuelvellavex = 0)
        {
            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                xRptaTransaccion = objAdm.ActualizarAsistenciaPersonal_salida(oNroDocumento, oObservacionSalidax ?? "", Convert.ToBoolean(oDevuelvellavex), CodUsu);

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

        #region ReporteIngreso
        //public ActionResult ReporteIngreso(string oFechaDesde = "", string oFechaHasta = "")
        //{

        //}
        public ActionResult ReporteIngreso()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "ReporteIngreso";
            ViewBag.Menu = "Ingreso";

            return View();
        }
        public PartialViewResult VP_ListarUsuarios()
        {
            List<Usuario> ListUsuarios = objAdm.ListarUsuarios(0);
            return PartialView("_VP_ListarUsuarios", ListUsuarios);
        }

        public PartialViewResult VP_ListarOrdenCompra()
        {
            List<KORBER_Listar_OrdenCompra_Cab_Result> ListOrdenCompra = objAdm.KORBER_Listar_OrdenCompra("");
            return PartialView("_VP_ListarOrdenCompra", ListOrdenCompra);
        }

        public PartialViewResult VP_ListarOrdenCompraDetalle(string xCode = "")
        {
            List<KORBER_Listar_OrdenCompra_Det_Result> ListOrdenCompraDetalle = objAdm.KORBER_Listar_OrdenCompraDetalle(xCode);
            return PartialView("_VP_ListarOrdenCompraDetalle", ListOrdenCompraDetalle);
        }

        public ActionResult EditarReporteCompra(string oCode = "")
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Compra";
            ViewBag.Menu = "IngresosAlpa";

            List<Drivin_General> data1 = objAdm.ListarLineaNegocio("LINEANEGOCIO");
            SelectList lista1 = new SelectList(data1, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarLineaNegocio = lista1;

            List<Drivin_General> data2 = objAdm.ListarTipoVehiculo("TIPOVEHICULO");
            SelectList lista2 = new SelectList(data2, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipoVehiculo = lista2;

            List<Drivin_General> data3 = objAdm.ListarTipoCarga("TIPOCARGA");
            SelectList lista3 = new SelectList(data3, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipoCarga = lista3;

            List<Drivin_General> data4 = objAdm.ListarTipoCompra("TIPOCOMPRA");
            SelectList lista4 = new SelectList(data4, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarTipoCompra = lista4;

            List<Drivin_General> data5 = objAdm.ListarAlmacen("ALMACEN");
            SelectList lista5 = new SelectList(data5, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarAlmacen = lista5;

            List<Drivin_General> data6 = objAdm.ListarCliente("CLIENTE");
            SelectList lista6 = new SelectList(data6, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarCliente = lista6;

            List<Drivin_General> data7 = objAdm.ListarReporteConductor("CONDUCTOR");
            SelectList lista7 = new SelectList(data7, "U_CodTipo", "U_TipoDesc");
            ViewBag.ListarConductor = lista7;

            ViewBag.oTipo = "View";

            List<KORBER_Listar_OrdenCompra_Cab_Result> ListOrdenCompra = objAdm.KORBER_Listar_OrdenCompra(oCode);
            ViewBag.oWh_id = ListOrdenCompra[0].wh_id;
            ViewBag.oCliente_code = ListOrdenCompra[0].client_code;
            ViewBag.oDivision = ListOrdenCompra[0].division_mxt;
            ViewBag.oTipoVehiculo = ListOrdenCompra[0].id_tipo_vehiculo;
            ViewBag.oTipoCompra = ListOrdenCompra[0].id_tipo_compra;
            ViewBag.oTipoCarga = ListOrdenCompra[0].id_tipo_carga;
            return View(ListOrdenCompra);
        }


        [HttpPost]
        public JsonResult ActualizarReporteCompra(            
            DateTime oFechaRecepcion, DateTime oFechaRegistro, DateTime oFechaInforme,
            string oWh_id = "", string oCliente_code = "", string oDivision = "",
            string oConductor = "", string oNroPlaca = "", string oNroContenedor = "", 
            string oNroPrecinto = "", int oTipoVehiculo = 0, string oNroDni = "", 
            string oNroLicencia = "", string oNroSoat = "", int oTipoCompra = 0, 
            int oTipoCarga = 0, string oNroOrdenCompra = "", string oNroOrdenTraslado = "", 
            string oNroBill = "", string oNroFactura = "", string oNroGuia = ""
            )
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                KORBER_Listar_OrdenCompra_Cab_Result ent = new KORBER_Listar_OrdenCompra_Cab_Result
                {
                    wh_id = oWh_id,
                    client_code = oCliente_code,
                    division_mxt = oDivision,
                    fecha_recepcion = oFechaRecepcion,
                    fecha_registro = oFechaRegistro,
                    fecha_informe = oFechaInforme,
                    conductor = oConductor ?? "",
                    nro_placa = oNroPlaca,
                    nro_contenedor = oNroContenedor,
                    nro_precinto = oNroPrecinto,
                    id_tipo_vehiculo = oTipoVehiculo,
                    nro_dni = oNroDni,
                    nro_licencia = oNroLicencia,
                    nro_soat = oNroSoat,
                    id_tipo_compra = oTipoCompra,
                    id_tipo_carga = oTipoCarga,
                    display_po_number = oNroOrdenCompra,
                    nro_traslado = oNroOrdenTraslado,
                    nro_bill_lading = oNroBill,
                    nro_factura = oNroFactura,
                    nro_guia_remision = oNroGuia,
                };
                xRptaTransaccion = objAdm.ActualizarReporteCompra(ent);

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

        public PartialViewResult VP_Crear_Observacion(int oCodEmp = 0)
        {
            if (oCodEmp != 0)
            {
                List<KORBER_Listar_OrdenCompra_OBS_Result> oMArt = objAdm.ListarOrdenCompraOBS(oCodEmp);
                if (oMArt.Count == 1)
                {
                    ViewBag.oComentario = oMArt[0].comentario;
                    ViewBag.oConclusiones = oMArt[0].conclusiones;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar Observación";
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Crear Observación";
            }

            return PartialView("_VP_Crear_OC_Observacion");
        }

        public PartialViewResult VP_Crear_Imagenes(string oCodEmp = "")
        {
            if (oCodEmp != "")
            {
                List<KORBER_Listar_OrdenCompra_IMG_Result> oMArt = objAdm.ListarOrdenCompraIMG(oCodEmp);
                if (oMArt.Count == 1)
                {
                    ViewBag.id_imagen = oMArt[0].id_imagen;
                    ViewBag.nro_orden_compra = oMArt[0].nro_orden_compra;
                    ViewBag.nombre = oMArt[0].nombre;
                    ViewBag.descripcion = oMArt[0].descripcion;
                    ViewBag.imagen = oMArt[0].imagen;
                    ViewBag.oTipoForm = "U";
                    ViewBag.oTitulo = "Editar Observación";
                }
                else
                {
                    ViewBag.oTipoForm = "A";
                    ViewBag.oTitulo = "Crear Observación";
                }
            }
            else
            {
                ViewBag.oTipoForm = "A";
                ViewBag.oTitulo = "Crear Observación";
            }

            return PartialView("_VP_Crear_OC_Observacion");
        }

        [HttpPost]
        public JsonResult RegistrarImagenes(HttpPostedFileBase[] photo,   // Recibimos los archivos
                                      string[] photoDescription,
                                      int oIdImagen = 0,
                                      string oOrdenCompra = "",
                                      string oNombre = "",
                                      string oDescripcion = "")    // Recibimos las descripciones
        {
            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                // Verifica si se recibieron archivos
                if (photo != null && photo.Length > 0)
                {
                    // Recorrer los archivos y descripciones
                    for (int i = 0; i < photo.Length; i++)
                    {
                        var file = photo[i];
                        var description = photoDescription[i];

                        // Verifica si el archivo no es null o vacío
                        if (file != null && file.ContentLength > 0)
                        {
                            // Puedes guardar el archivo o convertirlo a byte[] según sea necesario
                            byte[] imagenBytes = null;
                            using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                            {
                                imagenBytes = binaryReader.ReadBytes(file.ContentLength);
                            }

                            // Crear el objeto que quieres insertar en la base de datos
                            KORBER_Listar_OrdenCompra_IMG_Result ent = new KORBER_Listar_OrdenCompra_IMG_Result
                            {
                                id_imagen = oIdImagen,
                                nro_orden_compra = oOrdenCompra,
                                nombre = oNombre,
                                descripcion = description,
                                imagen = imagenBytes,
                            };

                            // Registrar imagen
                            xRptaTransaccion = objAdm.RegistrarImagenes(ent);
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        EnableSuccess = false,
                        SuccessMsg = "No se recibieron imágenes."
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                });
            }

            return Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
            });
        }
        #endregion
    }
}