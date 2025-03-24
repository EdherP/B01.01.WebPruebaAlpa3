using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using Web_ALPA.Data;
using Web_ALPA.Models.Entidad;
using Web_ALPA.Models.IngresosAlpa;

namespace Web_ALPA.Models.Administracion
{
    public class AdministracionModel
    {
        public List<Usuario> ListarUsuarios(int Code = 0)
        {
            List<Usuario> ListUsuarios = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    ListUsuarios = (db.GET_Listar_Usuarios(Code).ToList()).ConvertAll(x => new Usuario
                    {
                        Nro = x.Nro,
                        Code = x.Code,
                        Apellidos = x.Apellidos,
                        Nombres = x.Nombres,
                        E_Mail = x.E_Mail,
                        Activo = x.Activo,
                        CodUsuario = x.CodUsuario,
                        SuperUsuario = x.SuperUsuario,
                        U_FlagActivo = x.U_FlagActivo,
                        U_fechaModificacion = x.U_fechaModificacion,
                        U_fechaRegistro = x.U_fechaRegistro,
                        U_UsuarioModificacion = x.U_UsuarioModificacion,
                        U_UsuarioRegistro = x.U_UsuarioRegistro,
                        Rol = x.Rol,
                        RoleId = x.RoleId,
                        U_ApellidoNombre = x.Apellidos + " " + x.Nombres,
                        U_Web = x.U_Web,
                        U_APP = x.U_APP,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_AutorizaIngreso = x.U_AutorizaIngreso,

                    });
                }
            }
            catch
            {
                return null;
            }
            return ListUsuarios;
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

        //public List<USP_Web_Listar_Empresa_Result> ListarEmpresas()
        //{
        //    List<USP_Web_Listar_Empresa_Result> ListEmpresas;
        //    try
        //    {
        //        using (ALPA_WEBEntities db = new ALPA_WEBEntities())
        //        {
        //            ListEmpresas = db.GET_Listar_Empresas().ToList();
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    return ListRol;
        //}
        public RptaTransaccion RegistrarUsuario(Usuario oBE)
        {

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));

            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2;
            int rpta4;
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                        {
                            rpta4 = dbx.POST_Usuario(oBE.CodUsuario, oBE.Contraseña, oBE.Nombres, oBE.Apellidos, oBE.E_Mail, oBE.SuperUsuario, oBE.U_Web, oBE.U_APP,
                                                         oBE.U_UsuarioRegistro, oBE.U_CodEmpresa, rpta);
                            if (rpta4 == 0)
                            {
                                transaction.Dispose();
                                xRptaTransaccion.U_Mensaje = "Usuario no registrado.";
                                xRptaTransaccion.U_Exito = false;
                            }
                            else
                            {
                                if (rpta4 == 1)
                                {
                                    rpta2 = dbx.POST_Usuario_Rol(oBE.CodUsuario, oBE.RoleId, rpta);
                                    if (rpta2 == 1)
                                    {
                                        transaction.Complete();
                                        xRptaTransaccion.U_Mensaje = "Usuario registrado!";
                                        xRptaTransaccion.U_Exito = true;
                                    }
                                    else
                                    {
                                        transaction.Dispose();
                                        xRptaTransaccion.U_Mensaje = "Usuario no registrado.";
                                        xRptaTransaccion.U_Exito = false;
                                    }
                                }
                                else
                                {
                                    transaction.Dispose();
                                    xRptaTransaccion.U_Mensaje = "Usuario ya registrado.";
                                    xRptaTransaccion.U_Exito = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Usuario no registrado." + ex.Message;
                        xRptaTransaccion.U_Exito = false;
                        return xRptaTransaccion;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = "Usuario no registrado." + ex.Message;
                xRptaTransaccion.U_Exito = false;
                return xRptaTransaccion;
            }
            
            return xRptaTransaccion;
        }
        public List<Permisos> ListarPermisos(string RoleID, string UserID)
        {
            List<Permisos> ListPerm;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                {
                    ListPerm = (db.GET_Listar_MenuPermiso(UserID, RoleID).ToList()).ConvertAll(x => new Permisos
                    {
                        PermisoType = x.PermisoType,
                        DisplayName = x.DisplayName,
                        MenuID = x.MenuID,
                        ParentMenuID = x.ParentMenuID,
                        Permiso = Convert.ToBoolean(x.Permiso),

                    });
                }
            }
            catch
            {
                return null;
            }
            return ListPerm;
        }
        public RptaTransaccion Actualizar_Permisos(List<Permisos> oPermisos, int xPermisoType, int oCodeUsu = 0, int CodeRol = 0)
        {

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2 = -1;
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {
                        foreach (var T in oPermisos)
                        {
                            if (rpta2 != 0)
                            {
                                rpta2 = dbx.UPDATE_Permisos(T.MenuID, oCodeUsu, CodeRol, xPermisoType, Convert.ToInt32(T.Permiso), rpta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (rpta2 != 0)
                    {
                        xRptaTransaccion.U_Mensaje = "Permiso actualizado correctamente.";
                        xRptaTransaccion.U_Exito = true;
                        xRptaTransaccion.U_Titulo = "Exito";
                        transaction.Complete();
                    }
                    else
                    {
                        xRptaTransaccion.U_Mensaje = "Error : Permiso no actualizado.";
                        xRptaTransaccion.U_Exito = false;
                        xRptaTransaccion.U_Titulo = "Error";
                        transaction.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
                xRptaTransaccion.U_Titulo = "Error";
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }
        public RptaTransaccion ActualizarUsuario(Usuario oBE)
        {

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2 = 0;
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {

                        int rpta4 = dbx.UPDATE_Usuario(oBE.Code, oBE.Apellidos, oBE.Nombres, oBE.E_Mail, oBE.U_FlagActivo,
                             oBE.SuperUsuario, oBE.U_Web, oBE.U_APP, oBE.U_UsuarioRegistro, oBE.U_CodEmpresa, rpta);


                        if (rpta4 == 1)
                        {
                            rpta2 = dbx.UPDATE_Usuario_Rol(oBE.Code, oBE.RoleId, rpta);
                        }
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Usuario actualizado!.";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "No se pudo actualizar el usuario.";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = "No se pudo actualizar el usuario." + ex.Message;
                xRptaTransaccion.U_Exito = false;
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }
    }
}