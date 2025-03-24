using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ALPA.Data;
using Web_ALPA.Models.Administracion;
using Web_ALPA.Models.IngresosAlpa;
using Web_ALPA.Models.Entidad;
using Web_ALPA.ViewModel;

namespace Web_ALPA.Controllers
{
    public class AdministracionController : Controller
    {
        readonly IngresosAlpaModel objAdm1;
        readonly AdministracionModel objAdm;
        private RptaTransaccion xRptaTransaccion;

        public AdministracionController()
        {
            objAdm = new AdministracionModel();
            objAdm1= new IngresosAlpaModel();
            //objUsu = new Model_Usuario();
        }

        #region Usuarios

        // GET: Administracion
        public ActionResult Usuarios()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.SubMenu = "Usuarios";
            ViewBag.Menu = "Administración";

            return View();
        }

        public ActionResult CrearUsuario()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Registro de Usuario";
            ViewBag.Menu = "Administración";
            List<USP_Listar_Roles_Result> data2 = objAdm.ListaRoles();
            SelectList lista2 = new SelectList(data2, "Code", "Name");
            ViewBag.ListaRoles = lista2;

            List<Empresa> ListEmpresas = objAdm1.ListarEmpresas();
            SelectList lista = new SelectList(ListEmpresas, "U_CodEmpresa", "U_Empresa");
            ViewBag.ListarEmpresa = lista;
            return View();
        }

        [HttpPost]
        public JsonResult Registrarusuario(string oNombres = "", string oApellidos = "", string oEmail = "",
                                       string oActivoWEB = "", string oActivoAPP = "", string oCodUsuario = "", string oContrasena = "",
                                       string oSuperUsuario = "", int oRoles = 0,string oCodEmpresa="")
        {

            
            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();

                bool EstadoApp = false;
                bool EstadoWeb = false;
                bool SuperUsuario = false;


                if (oActivoAPP == "on")
                {
                    EstadoApp = true;
                }

                if (oActivoWEB == "on")
                {
                    EstadoWeb = true;
                }

                if (oSuperUsuario == "on")
                {
                    SuperUsuario = true;
                }



                Usuario ent = new Usuario
                {
                    Nombres = oNombres,
                    Apellidos = oApellidos,
                    E_Mail = oEmail ?? "",
                    CodUsuario = oCodUsuario,
                    Contraseña = oContrasena,
                    U_APP = EstadoApp,
                    U_Web = EstadoWeb,
                    U_UsuarioRegistro = CodUsu,
                    RoleId = oRoles,
                    SuperUsuario = SuperUsuario,
                    U_CodEmpresa = oCodEmpresa,
                };
                xRptaTransaccion = objAdm.RegistrarUsuario(ent);

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

        public PartialViewResult VP_ListarUsuarios()
        {
            List<Usuario> ListUsuarios = objAdm.ListarUsuarios(0);
            return PartialView("_VP_ListarUsuarios", ListUsuarios);
        }

        public ActionResult EditarUsuario(int oCode = 0)
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Detalle de Usuario";
            ViewBag.Menu = "Administración";

            List<USP_Listar_Roles_Result> data2 = objAdm.ListaRoles();
            SelectList lista2 = new SelectList(data2, "Code", "Name");
            ViewBag.ListaRoles = lista2;
            ViewBag.oTipo = "View";
            List<Usuario> ListUsuarios = objAdm.ListarUsuarios(oCode);
            ViewBag.oRoles = ListUsuarios[0].RoleId;
            return View(ListUsuarios);
        }

        [HttpPost]
        public JsonResult ActualizarUsuario(int oCode = 0, string oNombres = "", string oApellidos = "", string oEmail = "",
                                            string oActivo = "", string oActivoAPP = "", string oActivoWEB = "", string oSuperUsuario = "", int oRoles = 0)
        {

            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();


                bool SuperUsuario = false;
                bool Activo = false;
                bool oAPP = false;
                bool oWEB = false;

                if (oSuperUsuario == "on")
                {
                    SuperUsuario = true;
                }

                if (oActivo == "on")
                {
                    Activo = true;
                }

                if (oActivoAPP == "on")
                {
                    oAPP = true;
                }

                if (oActivoWEB == "on")
                {
                    oWEB = true;
                }

                Usuario ent = new Usuario
                {
                    Code = oCode,
                    Nombres = oNombres,
                    Apellidos = oApellidos,
                    E_Mail = oEmail ?? "",
                    U_Web = oWEB,
                    U_APP = oAPP,
                    U_UsuarioRegistro = CodUsu,
                    RoleId = oRoles,
                    SuperUsuario = SuperUsuario,
                    U_FlagActivo = Activo,
                };
                xRptaTransaccion = objAdm.ActualizarUsuario(ent);

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

        #region Permisos
        public ActionResult Permisos()
        {
            if ((LoginViewModel)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.SubMenu = "Administración de Permisos.";
            ViewBag.Menu = "Permisos";

            List<Usuario> data1 = objAdm.ListarUsuarios(0);
            SelectList lista1 = new SelectList(data1, "Code", "U_ApellidoNombre");
            ViewBag.ListaUsuarios = lista1;

            List<USP_Listar_Roles_Result> data2 = objAdm.ListaRoles();
            SelectList lista2 = new SelectList(data2, "Code", "Name");
            ViewBag.ListaRoles = lista2;

            return View();
        }

        public PartialViewResult VP_ListarPermisos(string RoleID, string UserID)
        {

            List<Permisos> ListPerm = null;
            if (RoleID == "")
            {
                RoleID = null;
            }

            if (UserID == "")
            {
                UserID = null;
            }

            if (RoleID != null || UserID != null)
            {
                ListPerm = objAdm.ListarPermisos(RoleID, UserID);
            }
            ViewBag.RoleID = RoleID;
            ViewBag.UserID = UserID;
            return PartialView("_VP_ListarPermisos", ListPerm);
        }

        [HttpPost]
        public JsonResult ActualizarPermisos(List<Permisos> oPermisos, int UserID = 0, int RoleID = 0)
        {
            xRptaTransaccion = new RptaTransaccion();
            try
            {
                string CodUsu = System.Web.HttpContext.Current.Session["CodUsuario"].ToString();
                int xPermisoType;
                if (UserID != 0)
                {
                    xPermisoType = 1;
                }
                else
                {
                    xPermisoType = 0;
                }
                xRptaTransaccion = objAdm.Actualizar_Permisos(oPermisos, xPermisoType, UserID, RoleID);

            }
            catch (Exception ex)
            {
                return this.Json(new
                {
                    EnableSuccess = false,
                    SuccessMsg = ex.Message,
                    Title = "Error",
                });
            }
            return this.Json(new
            {
                EnableSuccess = xRptaTransaccion.U_Exito,
                SuccessMsg = xRptaTransaccion.U_Mensaje,
                Title = xRptaTransaccion.U_Titulo,
            });
        }

        #endregion
    }
}