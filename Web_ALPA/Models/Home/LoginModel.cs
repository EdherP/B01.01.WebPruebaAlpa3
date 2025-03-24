using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Web_ALPA.Data;
using Web_ALPA.Models.Entidad;
using Web_ALPA.ViewModel;

namespace Web_ALPA.Models.Home
{
    public class LoginModel
    {
        internal LoginViewModel SetLogin(LoginViewModel model)
        {
            try
            {
                if (model.UserName == string.Empty || model.Password == string.Empty) //validación de vacíos
                {
                    model.Mensaje = "Debe ingresar ususario y contraseña";
                    return model;
                }
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    USP_Web_Listar_DatosInicialesUsuario_Result usu = db.GET_Listar_DatosInicialesUsuario(model.UserName, model.Password).FirstOrDefault();
                    if (usu != null)
                    {
                        if (usu.U_Web == true)
                        {
                            FormsAuthentication.SetAuthCookie(usu.Usuario, false);
                            System.Web.HttpContext.Current.Session["CodUsuario"] = usu.CodUsuario;
                            System.Web.HttpContext.Current.Session["UserID"] = usu.idUsuario;
                            System.Web.HttpContext.Current.Session["SuperUsuario"] = usu.SuperUsuario;
                            System.Web.HttpContext.Current.Session["ValidarDocs"] = usu.U_FlagValidarDocs;
                            System.Web.HttpContext.Current.Session["CodEmpresa"] = usu.U_CodEmpresa;
                            System.Web.HttpContext.Current.Session["Email"] = usu.E_Mail;
                            model.Datos = usu;
                            return model;
                        }
                        else
                        {
                            model.Mensaje = "El usuario no tiene acceso a la web ALPA.";
                        }
                    }
                    else
                    {
                        model.Mensaje = "Los Datos Usuario y/o Contraseña no son Correctos.";
                    }
                }
              
                return model;
            }
            catch (Exception ex)
            {
                model.Mensaje = "Error en Conexión : " + ex.Message;
                return model;
            }
        }

        public List<MenuTemp> ListarLoad_Menu()
        {
            List<MenuTemp> LoadMenuTemp = null;
            int UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    LoadMenuTemp = (db.GET_Listar_Menu(UserID).ToList()).ConvertAll(x => new MenuTemp
                    {
                        MenuID = x.MenuID,
                        DisplayName = x.DisplayName,
                        MenuIcon = x.MenuIcon,
                        MenuURL = x.MenuURL,
                        OrderNumber = x.OrderNumber,
                        ParentMenuID = x.ParentMenuID,
                    });
                }
            }
            catch
            {
                return null;
            }
            return LoadMenuTemp;
        }

        public List<USP_Listar_Roles_Result> ListaRoles()
        {
            List<USP_Listar_Roles_Result> ListRol;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    ListRol = db.GET_Listar_Roles().ToList();
                }
            }
            catch
            {
                return null;
            }
            return ListRol;
        }
    }
}