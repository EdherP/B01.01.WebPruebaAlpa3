using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using Web_ALPA.Data;
using Web_ALPA.Models.Entidad;
using Web_ALPA.Models.General;

namespace Web_ALPA.Models.IngresosAlpa
{
    public class IngresosAlpaModel
    {

        public RptaTransaccion RegistrarEmpresa(Empresa oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {

                        rpta2 = dbx.POST_Empresa(oBE.U_Empresa, oBE.U_RUC, oBE.U_Contacto, oBE.U_CodTipoEmpresa, oBE.U_FonoContacto, oBE.U_UsuarioRegistro, rpta);
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Empresa registrada!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Empresa no registrada.";
                        xRptaTransaccion.U_Exito = false;
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
        public List<Empresa> ListarEmpresas(int Cod = 0)
        {
            List<Empresa> ListEmpresas = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListEmpresas = (db.GET_Listar_Empresa(Cod).ToList()).ConvertAll(x => new Empresa
                    {
                        Nro = x.Nro,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_Empresa = x.U_Empresa,
                        U_RUC = x.U_RUC,
                        U_Contacto = x.U_Contacto,
                        U_FonoContacto = x.U_FonoContacto,
                        U_FlagActivo = x.U_FlagActivo,
                        U_CodTipoEmpresa = x.U_CodTipoEmpresa,
                        U_TipoDesc = x.U_TipoDesc,

                        Code = x.Code,
                        Activo = x.Activo,
                    });
            }
            catch
            {
                return null;
            }
            return ListEmpresas;
        }
        public RptaTransaccion ActualizarEmpresa(Empresa oBE)
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

                        rpta2 = dbx.UPDATE_Empresa(oBE.Code, oBE.U_Empresa, oBE.U_RUC, oBE.U_Contacto, oBE.U_CodTipoEmpresa, oBE.U_FonoContacto, oBE.U_UsuarioModificacion, oBE.U_FlagActivo, rpta);

                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Empresa actualizada correctamente.";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Error : No se pudo actualizar la empresa.";
                        xRptaTransaccion.U_Exito = false;
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
        public RptaTransaccion RegistrarVehiculo(Vehiculo oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter valrpta = new ObjectParameter("ValRpta", typeof(string));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {

                        rpta2 = dbx.POST_Vehiculo(oBE.U_CodEmpresa, oBE.U_Placa, oBE.U_Anio, oBE.U_Marca, oBE.U_RutaRevTecnica, oBE.U_RutaSOAT, oBE.U_RutaTarjPropiedad, oBE.U_UsuarioRegistro, valrpta, rpta);
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Vehiculo registrado!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Vehiculo no registrado.";
                        xRptaTransaccion.U_Exito = false;
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
        public List<Vehiculo> ListarVehiculos(int Code = 0, string Busq = "", string CodEmpresa = "")
        {
            List<Vehiculo> ListVehiculos = null;
            string sRut = ConfigurationManager.AppSettings.Get("oRutaPublicaDocs");
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListVehiculos = db.GET_Listar_Vehiculo(Code, Busq, CodEmpresa).ToList().ConvertAll(x => new Vehiculo
                    {
                        Nro = x.Nro,
                        U_IdTx = x.U_IdTx,
                        U_NroDocumento = x.U_NroDocumento,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_Empresa = x.U_Empresa,
                        U_Anio = x.U_Anio,
                        U_Marca = x.U_Marca,
                        U_Placa = x.U_Placa,
                        U_RevisionTecnica = x.U_RevisionTecnica,
                        U_RutaRevTecnica = x.U_RutaRevTecnica,
                        U_FechaRevTecnicaInicio = x.U_FechaRevTecnicaInicio,
                        U_FechaRevTecnicaFin = x.U_FechaRevTecnicaFin,
                        U_ConformeRevTecnica = x.U_ConformeRevTecnica,
                        U_SOAT = x.U_SOAT,
                        U_SOAT_Inicio = x.U_SOAT_Inicio,
                        U_SOAT_Fin = x.U_SOAT_Fin,
                        U_RutaSOAT = x.U_RutaSOAT,
                        U_ConformeSOAT = x.U_ConformeSOAT,
                        U_TarjetaPropiedad = x.U_TarjetaPropiedad,
                        U_TarjPropiedadInicio = x.U_TarjPropiedadInicio,
                        U_TarjPropiedadFin = x.U_TarjPropiedadFin,
                        U_RutaTarjPropiedad = x.U_RutaTarjPropiedad,
                        U_ConformeTarjPropiedad = x.U_ConformeTarjPropiedad,
                        U_DirRevisionTecnica = sRut + x.U_RutaRevTecnica,
                        U_DirSOAT = sRut + x.U_RutaSOAT,
                        U_DirTarjetaPropiedad = sRut + x.U_RutaTarjPropiedad,

                    });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
            return ListVehiculos;
        }
        public RptaTransaccion ActualizarVehiculo(Vehiculo oBE)
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

                        rpta2 = dbx.UPDATE_Vehiculo(oBE.U_IdTx, oBE.U_CodEmpresa, oBE.U_Anio, oBE.U_Marca, oBE.U_RutaRevTecnica, oBE.U_RutaSOAT,
                             oBE.U_RutaTarjPropiedad, oBE.U_UsuarioModificacion, rpta);

                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Vehiculo actualizado correctamente.";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Error : No se pudo actualizar el Vehiculo.";
                        xRptaTransaccion.U_Exito = false;
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
        public RptaTransaccion RegistrarAyudante(Ayudante oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter valrpta = new ObjectParameter("ValRpta", typeof(string));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                {

                    rpta2 = dbx.POST_Ayudante(oBE.U_TipoDocumento, oBE.U_DocIdentidad, oBE.U_CodEmpresa, oBE.U_Nombre, oBE.U_Apellidos, oBE.U_RutaCuestionario,
                                                oBE.U_RutaVacunacion, oBE.U_RutaSCTR, oBE.U_RutaInduccion, oBE.U_UsuarioRegistro, rpta, valrpta);
                }
                if (rpta2 == 1)
                {
                    xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                    xRptaTransaccion.U_Exito = true;
                }
                else
                {
                    xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                    xRptaTransaccion.U_Exito = false;
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
        public List<Ayudante> ListarAyudantes(int Code = 0, string Busq = "", string CodEmpresa = "")
        {
            List<Ayudante> ListAyudantes = null;
            string sRut = ConfigurationManager.AppSettings.Get("oRutaPublicaDocs");
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListAyudantes = (db.GET_Listar_Ayudantes(Code, Busq, CodEmpresa).ToList()).ConvertAll(x => new Ayudante
                    {
                        Nro = x.Nro,
                        U_IdTx = x.U_IdTx,
                        U_NroDocumento = x.U_NroDocumento,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_Empresa = x.U_Empresa,
                        U_TipoDesc = x.U_TipoDesc,
                        U_TipoDocumento = x.U_TipoDocumento,
                        U_DocIdentidad = x.U_DocIdentidad,
                        U_Nombre = x.U_Nombre,
                        U_Apellidos = x.U_Apellidos,
                        U_FlagActivo = x.U_FlagActivo,
                        U_Cuestionario = x.U_Cuestionario,
                        U_RutaCuestionario = x.U_RutaCuestionario,
                        U_Vacunacion = x.U_Vacunacion,
                        U_RutaVacunacion = x.U_RutaVacunacion,
                        U_SCTR = x.U_SCTR,
                        U_RutaSCTR = x.U_RutaSCTR,
                        U_Induccion = x.U_Induccion,
                        U_RutaInduccion = x.U_RutaInduccion,
                        U_DirCuestionario = sRut + x.U_RutaCuestionario,
                        U_DirVacunacion = sRut + x.U_RutaVacunacion,
                        U_DirSCTR = sRut + x.U_RutaSCTR,
                        U_DirInduccion = sRut + x.U_RutaInduccion,
                        U_ObsEstadoDocs = x.U_ObsEstadoDocs,
                        DescEstadoDocs = x.DescEstadoDocs,
                        U_EstadoDocs = x.U_EstadoDocs,
                        U_ApellidoNombre = x.U_Apellidos + ' ' + x.U_Nombre,
                    });
            }
            catch (Exception ex)
            {
                string oRpta = ex.Message;
                return null;
            }
            return ListAyudantes;
        }
        public RptaTransaccion ActualizarAyudante(Ayudante oBE)
        {

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2 = 0;
            try
            {
                using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                {

                    rpta2 = dbx.UPDATE_Ayudante(oBE.U_IdTx, oBE.U_Nombre, oBE.U_Apellidos, oBE.U_RutaCuestionario, oBE.U_RutaVacunacion,
                                                oBE.U_RutaSCTR, oBE.U_RutaInduccion, oBE.U_UsuarioModificacion, oBE.U_FlagActivo, rpta);

                }
                if (rpta2 == 1)
                {
                    xRptaTransaccion.U_Mensaje = "Ayudante actualizado correctamente.";
                    xRptaTransaccion.U_Exito = true;
                }
                else
                {
                    xRptaTransaccion.U_Mensaje = "Error : No se pudo actualizar el Ayudante.";
                    xRptaTransaccion.U_Exito = false;
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

        public RptaTransaccion ActualizarAyudante_Validacion(int oNroDocumento, int oEstadoDocs, string oObservacion, string oUser)
        {
            string oEstado = string.Empty;
            string CorreoTI = ConfigurationManager.AppSettings.Get("oCorreoTI");
            string PswTI = ConfigurationManager.AppSettings.Get("oPswCorreoTI");
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2 = 0;
            try
            {
                switch (oEstadoDocs)
                {
                    case 1:
                        oEstado = "PENDIENTE";
                        break;

                    case 2:
                        oEstado = "OBSERVADO";
                        break;

                    case 3:
                        oEstado = "CONFORME";
                        break;
                    default:
                        break;

                }
                using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                {
                    List<USP_Web_Listar_Ayudantes_Result> ListConductores = dbx.GET_Listar_Ayudantes(oNroDocumento, "", "").ToList();
                    rpta2 = dbx.UPDATE_Ayudante_Docs(oNroDocumento, oEstadoDocs, oObservacion, oUser, rpta);


                    if (rpta2 == 1)
                    {
                        xRptaTransaccion.U_Mensaje = "Registro actualizado.";
                        xRptaTransaccion.U_Exito = true;
                        Email_ValidacionDocs(CorreoTI, PswTI, "Ayudante", ListConductores[0].U_Apellidos + ' ' + ListConductores[0].U_Nombre, ListConductores[0].U_Empresa, oEstado, oObservacion ?? "");
                    }
                    else
                    {
                        xRptaTransaccion.U_Mensaje = "No se pudo actualizar.";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = "No se pudo actualizar." + ex.Message;
                xRptaTransaccion.U_Exito = false;
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }
        public RptaTransaccion RegistrarConductor(Conductor oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter valrpta = new ObjectParameter("ValRpta", typeof(string));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {

                        rpta2 = dbx.POST_Conductor(oBE.U_TipoDocumento, oBE.U_DocIdentidad, oBE.U_CodEmpresa, oBE.U_Nombre, oBE.U_Apellidos, oBE.U_RutaCuestionario,
                                                    oBE.U_RutaVacunacion, oBE.U_RutaSCTR, oBE.U_RutaLicConducir, oBE.U_UsuarioRegistro, rpta, valrpta);
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                        xRptaTransaccion.U_Exito = false;
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
        public List<Conductor> ListarConductores(int Code = 0, string Busq = "", string CodEmpresa = "")
        {
            List<Conductor> ListConductores = null;
            string sRut = ConfigurationManager.AppSettings.Get("oRutaPublicaDocs");
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListConductores = (db.GET_Listar_Conductores(Code, Busq, CodEmpresa).ToList()).ConvertAll(x => new Conductor
                    {
                        Nro = x.Nro,
                        U_IdTx = x.U_IdTx,
                        U_NroDocumento = x.U_NroDocumento,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_Empresa = x.U_Empresa,
                        U_TipoDesc = x.U_TipoDesc,
                        U_TipoDocumento = x.U_TipoDocumento,
                        U_DocIdentidad = x.U_DocIdentidad,
                        U_Nombre = x.U_Nombre,
                        U_Apellidos = x.U_Apellidos,
                        U_FlagActivo = x.U_FlagActivo,
                        U_Cuestionario = x.U_Cuestionario,
                        U_RutaCuestionario = x.U_RutaCuestionario,
                        U_Vacunacion = x.U_Vacunacion,
                        U_RutaVacunacion = x.U_RutaVacunacion,
                        U_SCTR = x.U_SCTR,
                        U_RutaSCTR = x.U_RutaSCTR,
                        U_LicenciaConducir = x.U_LicenciaConducir,
                        U_RutaLicConducir = x.U_RutaLicConducir,
                        U_FechaLicenciaInicio = x.U_FechaLicenciaInicio,
                        U_FechaLicenciaFin = x.U_FechaLicenciaFin,
                        U_ConformeLicConducir = x.U_ConformeLicConducir,
                        U_DirCuestionario = sRut + x.U_RutaCuestionario,
                        U_DirVacunacion = sRut + x.U_RutaVacunacion,
                        U_DirSCTR = sRut + x.U_RutaSCTR,
                        U_DirLicenciaConducir = sRut + x.U_RutaLicConducir,
                        U_EstadoDocs = x.U_EstadoDocs,
                        DescEstadoDocs = x.DescEstadoDocs,
                        U_ObsEstadoDocs = x.U_ObsEstadoDocs,
                        U_ApellidoNombre = x.U_Apellidos + ' ' + x.U_Nombre,
                    });
            }
            catch
            {
                return null;
            }
            return ListConductores;
        }
        public RptaTransaccion ActualizarConductor(Conductor oBE)
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

                        rpta2 = dbx.UPDATE_Conductor(oBE.U_IdTx, oBE.U_CodEmpresa, oBE.U_Nombre, oBE.U_Apellidos, oBE.U_RutaCuestionario, oBE.U_RutaVacunacion,
                                                   oBE.U_RutaSCTR, oBE.U_RutaLicConducir, oBE.U_UsuarioModificacion, oBE.U_FlagActivo, rpta);

                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Conductor actualizado correctamente.";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Error : No se pudo actualizar el Conductor.";
                        xRptaTransaccion.U_Exito = false;
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

        public RptaTransaccion ActualizarConductor_Validacion(int oNroDocumento, int oEstadoDocs, string oObservacion, string oUser)
        {
            string oEstado = string.Empty;
            string CorreoTI = ConfigurationManager.AppSettings.Get("oCorreoTI");
            string PswTI = ConfigurationManager.AppSettings.Get("oPswCorreoTI");
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2 = 0;
            try
            {
                switch (oEstadoDocs)
                {
                    case 1:
                        oEstado = "PENDIENTE";
                        break;

                    case 2:
                        oEstado = "OBSERVADO";
                        break;

                    case 3:
                        oEstado = "CONFORME";
                        break;
                    default:
                        break;

                }
                using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                {
                    List<USP_Web_Listar_Conductores_Result> ListConductores = dbx.GET_Listar_Conductores(oNroDocumento, "", "").ToList();
                    rpta2 = dbx.UPDATE_Conductor_Docs(oNroDocumento, oEstadoDocs, oObservacion, oUser, rpta);


                    if (rpta2 == 1)
                    {
                        xRptaTransaccion.U_Mensaje = "Registro actualizado.";
                        xRptaTransaccion.U_Exito = true;
                        Email_ValidacionDocs(CorreoTI, PswTI, "Conductor", ListConductores[0].U_Apellidos + ' ' + ListConductores[0].U_Nombre, ListConductores[0].U_Empresa, oEstado, oObservacion ?? "");
                    }
                    else
                    {
                        xRptaTransaccion.U_Mensaje = "No se pudo actualizar.";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = "No se pudo actualizar." + ex.Message;
                xRptaTransaccion.U_Exito = false;
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }


        public RptaTransaccion RegistrarPersonal(Personal oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter valrpta = new ObjectParameter("ValRpta", typeof(string));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {

                        rpta2 = dbx.POST_Personal(oBE.U_TipoDocumento, oBE.U_DocIdentidad, oBE.U_Nombre, oBE.U_Apellidos, oBE.U_CodEmpresa,
                                                  oBE.U_Cuestionario, oBE.U_FechaCuestionarioInicio, oBE.U_FechaCuestionarioFin,
                                                  oBE.U_Vacunacion, oBE.U_NroDosis, oBE.U_SCTR, oBE.U_FechaSCTRInicio, oBE.U_FechaSCTRFin,
                                                  oBE.U_Induccion, oBE.U_LicenciaConducir, oBE.U_FechaLicenciaInicio, oBE.U_FechaLicenciaFin,
                                                   oBE.U_Email, oBE.U_ListaNegra, oBE.U_MotivoListaNegra,
                                                  oBE.U_FechaListaNegra, oBE.U_Sancionado, oBE.U_FechaSancionInicio, oBE.U_Fecha_SancionFin,
                                                   oBE.U_UsuarioRegistro, rpta, valrpta);
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                        xRptaTransaccion.U_Exito = false;
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
        public List<Personal> ListarPersonal(int Code = 0, string oBusqueda = "")
        {
            List<Personal> ListPersonal = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListPersonal = (db.GET_Listar_Personal(Code, oBusqueda).ToList()).ConvertAll(x => new Personal
                    {
                        Nro = x.Nro,
                        U_IdTx = x.U_IdTx,
                        U_Vacunacion = x.U_Vacunacion,
                        U_TipoDocumento = x.U_TipoDocumento,
                        U_DescTipoDoc = x.U_DescTipoDoc,
                        U_DocIdentidad = x.U_DocIdentidad,
                        U_Apellidos = x.U_Apellidos,
                        U_Nombre = x.U_Nombre,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_Empresa = x.U_Empresa,
                        U_FechaCuestionarioInicio = x.U_FechaCuestionarioInicio,
                        U_FechaCuestionarioFin = x.U_FechaCuestionarioFin,
                        U_FechaLicenciaInicio = x.U_FechaLicenciaInicio,
                        U_FechaLicenciaFin = x.U_FechaLicenciaFin,
                        U_FechaSCTRInicio = x.U_FechaSCTRInicio,
                        U_FechaSCTRFin = x.U_FechaSCTRFin,
                        U_Induccion = x.U_Induccion,
                        U_Email = x.U_Email,
                        U_ListaNegra = x.U_ListaNegra,
                        U_MotivoListaNegra = x.U_MotivoListaNegra,
                        U_FechaListaNegra = x.U_FechaListaNegra,
                        U_FechaSancionInicio = x.U_FechaSancionInicio,
                        U_Fecha_SancionFin = x.U_Fecha_SancionFin,
                        U_Sancionado = x.U_Sancionado,
                        U_Cuestionario = x.U_Cuestionario,
                        U_NroDosis = x.U_NroDosis,
                        U_SCTR = x.U_SCTR,
                        U_LicenciaConducir = x.U_LicenciaConducir,
                        U_VencimientoCuestionario = x.U_VencimientoCuestionario,
                        U_VencimientoLConducir = x.U_VencimientoLConducir,
                        U_VencimientoSancion = x.U_VencimientoSancion,
                        U_VencimientoSCTR = x.U_VencimientoSCTR,
                    });
            }
            catch
            {
                return null;
            }
            return ListPersonal;
        }
        public RptaTransaccion ActualizarPersonal(Personal oBE)
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

                        rpta2 = dbx.UPDATE_Personal(oBE.U_IdTx, oBE.U_Nombre, oBE.U_Apellidos, oBE.U_CodEmpresa,
                                                  oBE.U_FechaCuestionarioInicio, oBE.U_FechaCuestionarioFin, oBE.U_Vacunacion, oBE.U_FechaSCTRInicio, oBE.U_FechaSCTRFin,
                                                  oBE.U_Induccion, oBE.U_FechaLicenciaInicio, oBE.U_FechaLicenciaFin, oBE.U_Email, oBE.U_ListaNegra, oBE.U_MotivoListaNegra,
                                                  oBE.U_FechaListaNegra, oBE.U_Sancionado, oBE.U_FechaSancionInicio, oBE.U_Fecha_SancionFin, oBE.U_Cuestionario, oBE.U_NroDosis,
                                                  oBE.U_SCTR, oBE.U_LicenciaConducir, oBE.U_UsuarioModificacion, rpta);

                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Personal actualizado correctamente.";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Error : No se pudo actualizar el Personal.";
                        xRptaTransaccion.U_Exito = false;
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
        public List<TipoDocumento> ListarTipoDocumento(int Cod = 0)
        {
            List<TipoDocumento> ListTipoDocumento = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListTipoDocumento = (db.GET_Listar_TipoDocumento(Cod).ToList()).ConvertAll(x => new TipoDocumento
                    {
                        Nro = x.Nro,
                        U_CodTipo = x.U_CodTipo,
                        U_TipoDesc = x.U_TipoDesc,
                        U_FlagActivo = x.U_FlagActivo,
                    });
            }
            catch
            {
                return null;
            }
            return ListTipoDocumento;
        }
        public RptaTransaccion RegistrarTransportista(Transportista oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                    {

                        rpta2 = dbx.POST_Transportista(oBE.U_CodTransportista, oBE.U_Transportista, oBE.U_CodEmpresa, oBE.U_RUC, oBE.U_Contacto, oBE.U_FonoContacto, oBE.U_FlagActivo, oBE.U_UsuarioRegistro, rpta);
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Transportista registrado!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Transportista no registrado.";
                        xRptaTransaccion.U_Exito = false;
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
        public List<Transportista> ListarTransportistas(int Code = 0, string Busq = "", string CodEmpresa = "")
        {
            List<Transportista> ListTransportistas = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListTransportistas = (db.GET_Listar_Transportista(Code, Busq, CodEmpresa).ToList()).ConvertAll(x => new Transportista
                    {
                        Nro = x.Nro,
                        U_CodTransportista = x.U_CodTransportista,
                        U_Transportista = x.U_Transportista,
                        U_CodEmpresa = x.U_CodEmpresa,
                        U_Empresa = x.U_Empresa,
                        U_RUC = x.U_RUC,
                        U_Contacto = x.U_Contacto,
                        U_FonoContacto = x.U_FonoContacto,
                        U_FlagActivo = x.U_FlagActivo,
                        Code = x.Code,
                        Activo = x.Activo,
                    });
            }
            catch
            {
                return null;
            }
            return ListTransportistas;
        }
        public RptaTransaccion ActualizarTransportista(Transportista oBE)
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

                        rpta2 = dbx.UPDATE_Transportista(oBE.Code, oBE.U_Transportista, oBE.U_CodEmpresa, oBE.U_RUC, oBE.U_Contacto, oBE.U_FonoContacto, oBE.U_FlagActivo, oBE.U_UsuarioModificacion, rpta);

                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Transportista actualizada correctamente.";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Error : No se pudo actualizar el Transportista.";
                        xRptaTransaccion.U_Exito = false;
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
        public List<TipoEmpresa> ListarTipoEmpresa(int Cod = 0)
        {
            List<TipoEmpresa> ListTipoEmpresa = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListTipoEmpresa = (db.GET_Listar_TipoEmpresa(Cod).ToList()).ConvertAll(x => new TipoEmpresa
                    {
                        Nro = x.Nro,
                        U_CodTipo = x.U_CodTipo,
                        U_TipoDesc = x.U_TipoDesc,
                        U_FlagActivo = x.U_FlagActivo,
                    });
            }
            catch
            {
                return null;
            }
            return ListTipoEmpresa;
        }
        public List<USP_Web_Listar_TipoIngreso_Result> ListarTipoIngreso()
        {
            List<USP_Web_Listar_TipoIngreso_Result> ListTipo = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListTipo = db.GET_Listar_TipoIngreso().ToList();
            }
            catch
            {
                return null;
            }
            return ListTipo;
        }
        public RptaTransaccion RegistrarAsistenciaPersonal(string oCodEmpresa = "", int oCodPersonal = 0, int oConceptoingreso = 0, bool oEntregallave = false,
            string oOficinallave = "", string oCodAutorizante = "", string oMotivoAutorizacion = "", string oObsIngreso = "", string ocodUsuario = "")
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter valrpta = new ObjectParameter("ValRpta", typeof(string));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                {

                    rpta2 = dbx.POST_PersonalAsistencia_Cab(oCodEmpresa, oCodPersonal, oObsIngreso, oConceptoingreso, oEntregallave, oOficinallave,
                        oCodAutorizante, oMotivoAutorizacion, ocodUsuario, rpta, valrpta);
                }
                if (rpta2 == 1)
                {
                    xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                    xRptaTransaccion.U_Exito = true;
                }
                else
                {
                    xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                    xRptaTransaccion.U_Exito = false;
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
        public List<PersonalAsistencia> ListarPersonalAsistencia(string oFechaDesde = "", string oFechaHasta = "", string oBuscaGral = "")
        {
            List<PersonalAsistencia> ListPersonal = null;
            try
            {
                using (ALPA_WEBEntities db = new ALPA_WEBEntities())
                    ListPersonal = (db.GET_Listar_PersonalAsistencia_Cab(oFechaDesde, oFechaHasta, oBuscaGral).ToList()).ConvertAll(x => new PersonalAsistencia
                    {
                        U_NroDocumento = x.U_NroDocumento,
                        Nro = x.Nro,
                        FechaIngreso = x.FechaIngreso,
                        Hora_Ingreso = x.Hora_Ingreso,
                        FechaSalida = x.FechaSalida ?? "--",
                        Hora_Salida = x.Hora_Salida ?? "--",
                        U_ApellidoNombre = x.U_Apellidos + " " + x.U_Nombre,
                        TipoDocIdentidad = x.TipoDocIdentidad,
                        U_DocIdentidad = x.U_DocIdentidad,
                        U_Ingreso = x.U_Ingreso,
                        U_Salida = x.U_Salida,
                        U_OficinaLlave = x.U_OficinaLlave,
                        U_UsuarioRegistro = x.U_UsuarioRegistro,
                        U_TipoAsistencia = x.U_TipoAsistencia,
                        U_Empresa = x.U_Empresa,
                        U_ConceptoIngreso = x.U_ConceptoIngreso,
                    });
            }
            catch
            {
                return null;
            }
            return ListPersonal;
        }

        public RptaTransaccion ActualizarAsistenciaPersonal_salida(int oNroDocumento = 0, string oObservacionSalida = "", bool oDevuelveLlave = false, string ocodUsuario = "")
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            ObjectParameter valrpta = new ObjectParameter("ValRpta", typeof(string));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (ALPA_WEBEntities dbx = new ALPA_WEBEntities())
                {
                    rpta2 = dbx.UPDATE_PersonalAsistencia_Salida(oNroDocumento, oObservacionSalida, ocodUsuario, oDevuelveLlave, rpta, valrpta);
                }
                if (rpta2 == 1)
                {
                    xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                    xRptaTransaccion.U_Exito = true;
                }
                else
                {
                    xRptaTransaccion.U_Mensaje = Convert.ToString(valrpta.Value);
                    xRptaTransaccion.U_Exito = false;
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

        #region Email
        private void Email_ValidacionDocs(string CorreoTI, string PswTI, string oTipoPersonal, string oPersonal, string oEmpresa, string oEstado, string oObservacion)
        {
            Email oEmail = new Email();
            string xCorreos = ConfigurationManager.AppSettings.Get("oCorreoValidacionDocs") + ',' + System.Web.HttpContext.Current.Session["Email"].ToString() ?? "";

            string sEncabezado = "<tr><th colspan='1' bgcolor='#084B8A' scope='col' style='font-size:30px; color:#FFFFFF; font-family:Arial Narrow;'>" + "Validación de Documentos - " + oTipoPersonal
            + "</th></tr>";

            string sDatosCliente = "<tr>" +
            "<td colspan='2'>" +
            "<table width='80%' border='0' align='center' cellpadding='0' cellspacing='0'>" +

            "<tr>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow;'>Empresa: </td>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow; font-weight: bold;'><b>" + oEmpresa + "</td>" +
            "</tr>" +

            "<tr>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow;'>" + oTipoPersonal + "</td>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow; font-weight: bold;'><b>" + oPersonal + "</td>" +
            "</tr>" +

            "<tr>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow;'>Validación ALPA: </td>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow; font-weight: bold;'><b>" + oEstado + "</td>" +
            "</tr>" +

            "<tr>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#000000; font-family:Arial Narrow;'>Observación: </td>" +
            "<td nowrap='nowrap' align='left' style='font-size:16px; color=#D3320E; font-family:Arial Narrow; font-weight: bold;'><b>" + oObservacion + "</td>" +
            "</tr>" +

            "</table>" +
            "</td>" +
            "</tr>";

            string sHTMPBody = "<table width='80%' height='80%' border='1' cellspacing='0' cellpadding='0' align='center' style='border-color:#084B8A'>" +
            sEncabezado + sDatosCliente;

            oEmail.EnviarCorreo(CorreoTI, PswTI, xCorreos, "Validación ALPA - " + oPersonal, sHTMPBody);
        }
        #endregion

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

        public List<KORBER_Listar_OrdenCompra_Cab_Result> KORBER_Listar_OrdenCompra(string OrdenCompra = "")
        {
            List<KORBER_Listar_OrdenCompra_Cab_Result> ListOrdenCompra = null;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                    ListOrdenCompra = (db.KORBER_Listar_OrdenCompra_Cab(OrdenCompra).ToList()).ConvertAll(x=> new KORBER_Listar_OrdenCompra_Cab_Result
                    {
                        Nro = x.Nro,
                        wh_id = x.wh_id,
                        client_code = x.client_code,
                        type = x.type,
                        fecha_entrega_mxt = x.fecha_entrega_mxt,
                        display_po_number = x.display_po_number,
                        status = x.status,
                        tipo_compra_mxt = x.tipo_compra_mxt,
                        division_mxt = x.division_mxt,
                        presentacion_mxt = x.presentacion_mxt,
                        modalidad_mxt = x.modalidad_mxt,
                        fecha_registro_mxt = x.fecha_registro_mxt,
                        cantidad_esperada_mxt = x.cantidad_esperada_mxt,
                        conductor = x.conductor,
                        nro_dni = x.nro_dni,
                        nro_licencia = x.nro_licencia,
                        id_tipo_vehiculo = x.id_tipo_vehiculo,
                        id_tipo_compra = x.id_tipo_compra,
                        id_tipo_carga = x.id_tipo_carga,
                        nro_placa = x.nro_placa,
                        nro_contenedor = x.nro_contenedor,
                        nro_precinto = x.nro_precinto,
                        nro_soat = x.nro_soat,
                        nro_dua = x.nro_dua,
                        nro_traslado = x.nro_traslado,
                        nro_bill_lading = x.nro_bill_lading,
                        nro_factura = x.nro_factura,
                        nro_guia_remision = x.nro_guia_remision,
                        fecha_recepcion = x.fecha_recepcion,
                        fecha_registro = x.fecha_registro,
                        fecha_informe = x.fecha_informe
                    })
                    ;
            }
            catch
            {
                return null;
            }
            return ListOrdenCompra;
        }

        public List<KORBER_Listar_OrdenCompra_Det_Result> KORBER_Listar_OrdenCompraDetalle(string OrdenCompra = "")
        {
            List<KORBER_Listar_OrdenCompra_Det_Result> ListOrdenCompraDetalle = null;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                    ListOrdenCompraDetalle = (db.KORBER_Listar_OrdenCompra_Det(OrdenCompra).ToList()).ConvertAll(x => new KORBER_Listar_OrdenCompra_Det_Result
                    {
                        wh_id = x.wh_id,
                        po_number = x.po_number,
                        item_number = x.item_number,
                        lot_number = x.lot_number,
                        line_number = x.line_number,
                        FechaVencimiento_MXT = x.FechaVencimiento_MXT,
                        qty_received = x.qty_received,
                        cant_correcto = x.cant_correcto,
                        cant_defectuosa = x.cant_defectuosa,
                        cant_faltante = x.cant_faltante,
                        cant_sobrante = x.cant_sobrante,
                        flag_observado = x.flag_observado

                    })
                    ;
            }
            catch
            {
                return null;
            }
            return ListOrdenCompraDetalle;
        }


        public List<Drivin_General> ListarLineaNegocio(string CodigoNegocio = "")
        {
            List<Drivin_General> ListLineaNegocio; 
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListLineaNegocio = (db.DRIVIN_Listar_General(CodigoNegocio).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo= x.code,
                        U_TipoDesc = x.descripcion
                    });
                }
            }
            catch (Exception ex)
            {
                // Log el error (opcional)
                Console.WriteLine($"Error: {ex.Message}");
                // Puedes incluir el stack trace si necesitas más detalles
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null; // O maneja el error de otra forma
            }
            return ListLineaNegocio;
        }

        public List<Drivin_General> ListarTipoVehiculo(string CodigoTipoVehiculo = "")
        {
            List<Drivin_General> ListTipoVehiculo;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListTipoVehiculo = (db.DRIVIN_Listar_General(CodigoTipoVehiculo).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
                }
            }
            catch
            {
                return null;
            }
            return ListTipoVehiculo;
        }

        public List<Drivin_General> ListarTipoCompra(string CodigoTipoCompra = "")
        {
            List<Drivin_General> ListTipoCompra;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListTipoCompra = (db.DRIVIN_Listar_General(CodigoTipoCompra).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
                }
            }
            catch
            {
                return null;
            }
            return ListTipoCompra;
        }

        public List<Drivin_General> ListarAlmacen(string CodigoAlmacen = "")
        {
            List<Drivin_General> ListAlmacen;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListAlmacen = (db.DRIVIN_Listar_General(CodigoAlmacen).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
                }
            }
            catch
            {
                return null;
            }
            return ListAlmacen;
        }

        public List<Drivin_General> ListarCliente(string CodigoCliente = "")
        {
            List<Drivin_General> ListCliente;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListCliente = (db.DRIVIN_Listar_General(CodigoCliente).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
                }
            }
            catch
            {
                return null;
            }
            return ListCliente;
        }

        public List<Drivin_General> ListarTipoCarga(string CodigoTipoCarga = "")
        {
            List<Drivin_General> ListTipoCarga;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListTipoCarga = (db.DRIVIN_Listar_General(CodigoTipoCarga).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
                }
            }
            catch
            {
                return null;
            }
            return ListTipoCarga;
        }

        public List<Drivin_General> ListarReporteConductor(string CodigoConductor = "")
        {
            List<Drivin_General> ListConductor;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    ListConductor = (db.DRIVIN_Listar_General_Conductor(CodigoConductor).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion,
                        presentacion_mxt = x.dni
                        //U_Licencia = x.licencia

                    });
                }
            }
            catch
            {
                return null;
            }
            return ListConductor;
        }

        public RptaTransaccion ActualizarReporteCompra(KORBER_Listar_OrdenCompra_Cab_Result oBE)
        {

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            int rpta2 = 0;
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (Entities_Korber dbx = new Entities_Korber())
                    {

                         rpta2 = dbx.KORBER_SP_INFORME_MERCADERIA_CAB_INS(oBE.wh_id,
                             oBE.client_code,
                             oBE.display_po_number,
                             oBE.division_mxt,
                             oBE.conductor,
                             oBE.nro_dni,
                             oBE.nro_licencia,
                             oBE.id_tipo_vehiculo,
                             oBE.id_tipo_compra,
                             oBE.id_tipo_carga,
                             oBE.nro_placa,
                             oBE.nro_contenedor,
                             oBE.nro_precinto,
                             oBE.nro_soat,
                             oBE.nro_dua,
                             oBE.nro_traslado,
                             oBE.nro_bill_lading,
                             oBE.nro_factura,
                             oBE.nro_guia_remision,
                             oBE.fecha_recepcion,
                             oBE.fecha_registro,
                             oBE.fecha_informe,
                             rpta);

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

        public List<KORBER_Listar_OrdenCompra_OBS_Result> ListarOrdenCompraOBS(int Cod = 0)
        {
            List<KORBER_Listar_OrdenCompra_OBS_Result> ListEmpresas = null;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                    ListEmpresas = (db.KORBER_Listar_OrdenCompra_OBS(Cod).ToList()).ConvertAll(x => new KORBER_Listar_OrdenCompra_OBS_Result
                    {
                        id_detalle = x.id_detalle,
                        id_observado = x.id_observado,
                        comentario = x.comentario,
                        conclusiones = x.conclusiones
                        
                    });
            }
            catch
            {
                return null;
            }
            return ListEmpresas;
        }

        public List<KORBER_Listar_OrdenCompra_IMG_Result> ListarOrdenCompraIMG(string Cod = "")
        {
            List<KORBER_Listar_OrdenCompra_IMG_Result> ListEmpresas = null;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                    ListEmpresas = (db.KORBER_Listar_OrdenCompra_IMG(Cod).ToList()).ConvertAll(x => new KORBER_Listar_OrdenCompra_IMG_Result
                    {
                        id_imagen = x.id_imagen,
                        nro_orden_compra = x.nro_orden_compra,
                        nombre = x.nombre,
                        descripcion = x.descripcion,
                        imagen = x.imagen
                    });
            }
            catch
            {
                return null;
            }
            return ListEmpresas;
        }

        public RptaTransaccion RegistrarImagenes(KORBER_Listar_OrdenCompra_IMG_Result oBE)
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (Entities_Korber dbx = new Entities_Korber())
                    {

                        rpta2 = dbx.KOBER_SP_INFORME_DET_IMAGENES(oBE.id_imagen, oBE.nro_orden_compra, oBE.nombre, oBE.descripcion, oBE.imagen, rpta);
                    }
                    if (rpta2 == 1)
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Imagen registrada!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Imagen no registrada.";
                        xRptaTransaccion.U_Exito = false;
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

    }
}