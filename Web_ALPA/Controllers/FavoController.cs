using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Web_ALPA.Models.Entidad;
using Web_ALPA.Models.Favo;
using Web_ALPA.ViewModel;

namespace Web_ALPA.Controllers
{
    public class FavoController : Controller
    {
        readonly FavoModel objFavo;
        private RptaTransaccion xRptaTransaccion;
        private List<CeldaUbicacion> ubics;
        public FavoController()
        {
            objFavo = new FavoModel();
        }

        #region  /*   Toma de Inventario*/
        public ActionResult TomaInventarioGral(string oFechaDesde = "", string oFechaHasta = "", int oEstado = 2)
        {

            if (oFechaDesde != "" & oFechaHasta != "")
            {
                ViewBag.oFechaDesde = string.Format("{0:yyyy-MM-dd}", oFechaDesde);
                ViewBag.oFechaHasta = string.Format("{0:yyyy-MM-dd}", oFechaHasta);
            }
            else
            {
                ViewBag.oFechaDesde = string.Format("{0:yyyy-MM-01}", DateTime.Now);
                ViewBag.oFechaHasta = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }

            List<TipoInventario> oEstadoPr = new List<TipoInventario>
            {
                new TipoInventario() { U_Code = 2, U_Nombre = "" },
                new TipoInventario() { U_Code = 0, U_Nombre = "Abierto" },
                new TipoInventario() { U_Code = 1, U_Nombre = "Cerrado" }
            };
            SelectList lista2 = new SelectList(oEstadoPr, "U_Code", "U_Nombre");
            ViewBag.ListarEstado = lista2;
            ViewBag.oEstado = oEstado;
            return View();
        }

        public PartialViewResult VP_progInventario_Cab(string oFechaDesde = "", string oFechaHasta = "", int oEstado = 2)
        {

            List<ProgInventarioFavo_Cab> oMArt = objFavo.ListarProgInventario_Cab(oFechaDesde, oFechaHasta, oEstado);
            ViewBag.xFechaDesde = oFechaDesde;
            ViewBag.xFechaHasta = oFechaHasta;
            ViewBag.xEstado = oEstado;
            return PartialView("_VP_progInventario_Cab", oMArt);
        }

        public PartialViewResult VP_Crear_ProgInventario()
        {
            List<TipoInventario> oTInventario = new List<TipoInventario>
            {
                new TipoInventario() { U_Code = 0, U_Nombre = "General" },
                new TipoInventario() { U_Code = 1, U_Nombre = "Parcial" }
            };
            SelectList lista2 = new SelectList(oTInventario, "U_Code", "U_Nombre");
            ViewBag.ListarTipoInventario = lista2;

            ViewBag.FechaProg = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.Comentario = "";

            return PartialView("_VP_Crear_ProgInventario");
        }

        public PartialViewResult VP_ListarUbicacionCeldas()
        {
            List<CeldaUbicacion> Docs;
            try
            {
                Docs = objFavo.ListaUbicacionCeldas();
            }
            catch
            {
                Docs = null;
            }

            return PartialView("_VP_ListarUbicacionCeldas", Docs);
        }

        [HttpPost]
        public PartialViewResult VP_ListarUbicacionCeldas_Excel()
        {
            List<string> data = new List<string>();
            var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
            HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
            List<CeldaUbicacion> Docs;
            try
            {
                if (filebase != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                    if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                    {
                        string targetpath = HostingEnvironment.MapPath("~/Content/");
                        var filename = Path.GetFileName(filebase.FileName);
                        var path = Path.Combine(HostingEnvironment.MapPath("~/Content/"), filename);

                        string pathToExcelFile = targetpath + filename;
                        int cont = 0;
                        filebase.SaveAs(path);
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", path);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", path);
                        }

                        var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                        var ds = new DataSet();

                        adapter.Fill(ds, "ExcelTable");

                        DataTable dtable = ds.Tables["ExcelTable"];

                        string sheetName = "Sheet1";

                        var excelFile = new ExcelQueryFactory(path);
                        var artistAlbums = from a in excelFile.Worksheet<Ubicacion>(sheetName) select a;

                        Docs = objFavo.ListaUbicacionCeldas();
                        ubics = new List<CeldaUbicacion>();
                        foreach (var a in artistAlbums)
                        {
                            try
                            {
                                List<CeldaUbicacion> oTemp = Docs.Where(x => x.CodigoDeCelda.Trim() == a.CodigoDeCelda.Trim()).ToList();
                                if (oTemp.Count > 0)
                                {
                                    foreach (var T in oTemp)
                                    {
                                        cont++;
                                        CeldaUbicacion ent = new CeldaUbicacion
                                        {
                                            Linea = cont,
                                            CheckLinea = true,
                                            CodigoDeCelda = T.CodigoDeCelda,
                                            IDColumna = T.IDColumna,
                                            IDNivel = T.IDNivel,
                                            CheckError = false,
                                        };
                                        ubics.Add(ent);
                                    }
                                }
                                else
                                {
                                    cont++;
                                    CeldaUbicacion ent = new CeldaUbicacion
                                    {
                                        Linea = cont,
                                        CheckLinea = false,
                                        CodigoDeCelda = a.CodigoDeCelda,
                                        IDColumna = string.Empty,
                                        IDNivel = string.Empty,
                                        CheckError = true,
                                    };
                                    ubics.Add(ent);
                                }
                            }
                            catch (DbEntityValidationException ex)
                            {
                                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                {

                                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                                    {

                                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                    }

                                }
                            }
                        }
                        //deleting excel file from folder  
                        if ((System.IO.File.Exists(path)))
                        {
                            System.IO.File.Delete(path);
                        }

                    }
                    else
                    {
                        //alert message for invalid file format  
                        data.Add("<ul>");
                        data.Add("<li>Solo se permite el formato de archivo de Excel</li>");
                        data.Add("</ul>");
                        data.ToArray();

                    }
                }
                else
                {
                    data.Add("<ul>");
                    if (filebase == null) data.Add("<li>Elija un archivo de Excel</li>");
                    data.Add("</ul>");
                    data.ToArray();

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return PartialView("_VP_ListarUbicacionCeldas", ubics);
        }
        [HttpPost]
        public JsonResult RegistrarProgInv(List<CeldaUbicacion> Docs, int TipoInventario = 0, string FechaProg = "", string Comentario = "")
        {
            xRptaTransaccion = new RptaTransaccion();
            List<CeldaUbicacion> oTemp = new List<CeldaUbicacion>();
            try
            {

                bool xTipoInventario;
                if (TipoInventario == 0)
                {
                    xTipoInventario = false;
                }
                else
                { xTipoInventario = true; }

                if (TipoInventario == 1)
                {
                    oTemp = Docs.Where(x => x.CheckLinea == true).ToList();
                }

                xRptaTransaccion = objFavo.RegistrarProgInventario(oTemp, xTipoInventario, FechaProg, Comentario);

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

        public ActionResult DetalleProgInventarioGral(int oNroDocumento = 0, string oFechaDesde = "", string oFechaHasta = "", int oEstado = 2)
        {
            List<TipoBusquedaInv> TipoBusquedaInvx = new List<TipoBusquedaInv>
            {
                new TipoBusquedaInv() { U_Code = 0, U_Nombre = "Inventario General" },
                new TipoBusquedaInv() { U_Code = 1, U_Nombre = "Inventario Detallado" },
                new TipoBusquedaInv() { U_Code = 2, U_Nombre = "Stock Favo - Stock Inventario" }
            };
            SelectList listax = new SelectList(TipoBusquedaInvx, "U_Code", "U_Nombre");
            ViewBag.xListarBusqueda = listax;
            ViewBag.xNroDocumento = oNroDocumento;

            ViewBag.oFechaDesde = oFechaDesde;
            ViewBag.oFechaHasta = oFechaHasta;
            ViewBag.oEstado = oEstado;
            return View();
        }

        public PartialViewResult VP_ProgInventario(int EstadoBusqueda = 0, int NroProgramacion = 0)
        {
            ViewBag.EstadoBusqueda = EstadoBusqueda;
            ViewBag.NroProgramacion = NroProgramacion;

            if (EstadoBusqueda == 0)
            {
                InvFavoViewModel oInvCab = new InvFavoViewModel
                {
                    ProgInventarioFavo_Locacion = objFavo.ListarProgInventario_Locacion(NroProgramacion)
                };
                return PartialView("_VP_ProgInventario", oInvCab);
            }
            else
            {
                if (EstadoBusqueda == 1)
                {
                    InvFavoViewModel oInvCab = new InvFavoViewModel
                    {
                        ProgInventarioFavo_LS = objFavo.ListarProgInventario_LS(NroProgramacion)
                    };
                    return PartialView("_VP_ProgInventario", oInvCab);
                }
                else
                {
                    InvFavoViewModel oInvCab = new InvFavoViewModel
                    {
                        ProgInventarioFavo_Comparativo = objFavo.ListarProgInventario_Comparativo(NroProgramacion)
                    };
                    return PartialView("_VP_ProgInventario", oInvCab);
                }

            }
        }

        #endregion

        #region  /*   Consultas Favo*/

        public ActionResult OperacionesConsulta()
        {
            ViewBag.oFechaDesde1 = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.oFechaDesde2 = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

            return View();
        }

        public PartialViewResult VP_ListarRegistroJabas(string oFechaDesde1)
        {
            List<BE_SalidaDeJabas> oMArt = objFavo.ListaRegistrojabas(string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaDesde1)));
            return PartialView("_VP_ListarRegistroJabas", oMArt);
        }

        public PartialViewResult VP_ListarRegistroDevoluciones(string oFechaDesde2)
        {
            List<BE_DevolucionCliente> oMArt = objFavo.ListaRegistroDevolucion(string.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(oFechaDesde2)));
            return PartialView("_VP_ListarRegistroDevoluciones", oMArt);
        }
        #endregion
    }
}