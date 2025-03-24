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
using static Web_ALPA.Models.Entidad.Drivin_Direccion;
using static Web_ALPA.Models.Entidad.Drivin_Pedidos;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using static Web_ALPA.Models.Entidad.Drivin_Pedidos_post;
using static Web_ALPA.Models.Entidad.Drivin_Vehiculos_Get;
using static Web_ALPA.Models.Entidad.Drivin_Pedidos_Response;
using static Web_ALPA.Models.Entidad.Drivin_Pedido_Response_EL;
using static Web_ALPA.Models.Entidad.Drivin_Esquemas_Get;
using static Web_ALPA.Models.Entidad.DrivinDireccionResponse;
using static Web_ALPA.Models.Entidad.Drivin_Transportista_Get;
using static Web_ALPA.Models.Entidad.Drivin_Escenarios_Get;
using static Web_ALPA.Models.Entidad.Drivin_PlacaSecuritas_Get;
using static Web_ALPA.Models.Entidad.Drivin_LicenciasSecuritas_Get;

namespace Web_ALPA.Models.Drivin
{
    public class ClienteDireccion_Model
    {
        private List<Client> listDRIVIN;
        private List<Order> listDRIVIN_Order;
        private List<Time_Windows> listDRIVIN_TimeWindows;
        // private List<DrivinPed> listDrivin_order;        

        private List<Address_Post> ListDireccionesEnviar;

        public List<DRIVIN_Listar_Clientes_Result> Drivin_ListarClientes()
        {
            List<DRIVIN_Listar_Clientes_Result> ListClientes = null;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                    ListClientes = db.DRIVIN_Listar_Clientes().ToList();
            }
            catch
            {
                return null;
            }
            return ListClientes;
        }

        public List<DRIVIN_Listar_Direcciones_Result> Drivin_ListarDirecciones()
        {
            List<DRIVIN_Listar_Direcciones_Result> ListDirecciones = null;
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            try
            {
                string responseBody = DrivinModel.GetDrivin("addresses");

                if (responseBody != null)
                {
                    DrivinDireccion ListDrivin = JsonConvert.DeserializeObject<DrivinDireccion>(responseBody);
                    if (ListDrivin.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.response != null)
                            {
                                if (ListDrivin.response.Count() > 0)
                                {
                                    using (Entities_Korber db = new Entities_Korber())
                                    {
                                        rpta2 = db.DRIVIN_Eliminar_direcciones(rpta);

                                        if (rpta2 == -1)
                                        {
                                            foreach (var T in ListDrivin.response)
                                            {
                                                rpta2 = db.DRIVIN_Insertar_direccion(T.code, T.name, T.client, T.address_type, T.address1, T.address2, T.city, T.state, T.country, Convert.ToString(T.lat), Convert.ToString(T.lng), T.georeferenced, rpta);
                                            }
                                            transaction.Complete();
                                        }
                                        else
                                        {
                                            transaction.Dispose();
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                using (Entities_Korber db2 = new Entities_Korber())
                {
                    ListDirecciones = db2.DRIVIN_Listar_Direcciones().ToList();
                }

            }
            catch
            {
                return null;
            }
            return ListDirecciones;
        }

        public RptaTransaccion RegistrarPedidosDrivin(List<DRIVIN_SAP_Listar_Pedidos_Result> oPedidosx )
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                //using (Entities_Korber dbx = new Entities_Korber())
                //{

                //    rpta2 = dbx.DRIVIN_update_Pedidos(oPedidosx., "", oBE.placa, rpta);
                //}
                // Se agrega comentario
                rpta2 = 1;

                if (rpta2 != 0) //rpta2 == 1
                    {
                        //transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Drivin Registrada!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {                       
                        //transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Drivin no Registrada.";
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

        public List<DRIVIN_SAP_Listar_Pedidos_Result> Drivin_ListarPedidos(string oAlmacen, string oFechaDesde = "", string oFechaHasta = "", int oEstadoDrivin = 0, string oFormasEnvioDrivin = "", string oBusquedaGeneral ="", string oLineaNegocioDrivin = "", string oPlacaGeneral = "")
        {
            List<DRIVIN_SAP_Listar_Pedidos_Result> ListPedidos = null;
            try
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    db.Database.CommandTimeout = 180;

                    string fechaIni = oFechaDesde;
                    string fechaFin = oFechaHasta;
                    string parametro3 = "";
                    string parametro4 = oAlmacen;
                    string parametro5 = "";
                    int parametro6 = oEstadoDrivin;
                    string parametro7 = oFormasEnvioDrivin == "0" ? "" : oFormasEnvioDrivin.ToString();
                    string parametro8 = oBusquedaGeneral;
                    string parametro9 = oLineaNegocioDrivin == "0" ? "" : oLineaNegocioDrivin.ToString();
                    string parametro10 = oPlacaGeneral;

                    ListPedidos = db.DRIVIN_SAP_Listar_Pedidos(fechaIni, fechaFin, parametro3, parametro4, parametro5, parametro6, parametro7, parametro8, parametro9, parametro10).ToList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine (ex.Message);
                return null;
            }
            #region[Probar]
           
            //string rpta2 = "";
            if (ListPedidos.Count() > 0)
            {
                using (Entities_Korber db = new Entities_Korber())
                {
                    foreach (var T in ListPedidos)
                    {
                     // string  rpta2 = T.id_direccion;
                      string responseBody = DrivinModel.GetDrivin("addresses?code=" + T.id_direccion);
                        if (responseBody != null)
                        {
                            DrivinDireccion ListDrivin = JsonConvert.DeserializeObject<DrivinDireccion>(responseBody);
                            if (ListDrivin.status == "OK")
                            {
                                using (TransactionScope transaction = new TransactionScope())
                                {
                                    if (ListDrivin.response.Count() > 0)
                                    {
                                        foreach (var D in ListDrivin.response)
                                        {
                                            T.georeferencia = D.georeferenced ? 1 : 0;
                                            T.latitud = D.lat.ToString();
                                            T.longitud = D.lng.ToString();
                                        }
                                    }
                                    else {
                                        T.georeferencia = 0;
                                    }                                   
                                    }
                                }
                            }
                        }
                }
            }
            #endregion
            return ListPedidos;
        }

        public List<DRIVIN_SAP_Listar_Pedidos_Result> Drivin_ListarPedidosPost()
        {
            List<DRIVIN_SAP_Listar_Pedidos_Result> ListPedidos = null;

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            try
            {
                string responseBody = DrivinModel.PostDrivin("orders?schema_code=011", "campo1");

                if (responseBody != "")
                {
                    DrivinPedidosOk ListDrivin = JsonConvert.DeserializeObject<DrivinPedidosOk>(responseBody);
                    if (ListDrivin.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.response != null)
                            {
                                if (ListDrivin.response.Count() > 0)
                                {
                                    transaction.Dispose();
                                }
                            }
                        }
                    }
                }

                using (Entities_Korber db2 = new Entities_Korber())
                {
                    string fechaIni = "";
                    string fechaFin = "";
                    string parametro3 = "";
                    string parametro4 = "";
                    string parametro5 = "";
                    int parametro6 = 0;
                    string parametro7 = "";
                    string parametro8 = "";
                    string parametro9 = "";
                    string parametro10 = "";
                    ListPedidos = db2.DRIVIN_SAP_Listar_Pedidos(fechaIni, fechaFin, parametro3, parametro4, parametro5, parametro6, parametro7, parametro8, parametro9, parametro10).ToList();
                }

            }
            catch
            {
                return null;
            }
            return ListPedidos;
        }

        public RptaTransaccion RegistrarPlaca(List<DRIVIN_SAP_Listar_Pedidos_Result> listaEnt)
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
                        foreach (var T in listaEnt)
                        {
                            rpta2 = dbx.DRIVIN_SAP_update_Pedidos(T.Orden, T.Docentry, T.reference, T.wh_id, T.placa, T.tiempo_servicio, T.inicio_ventana_horario, T.fin_ventana_horario, T.delivery_date, T.convoy, rpta);
                        }
                    }
                    if (rpta2 != 0 ) //rpta2 == 1
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Datos Modificados!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        //transaction.Complete();
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Datos no Modificados.";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
            }
            catch (Exception ex)
            {               
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
                //Console.WriteLine("Excepción interna: " + ex.InnerException);
                //throw;
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }

        public RptaTransaccion RegistrarPlacaIndivudual(List<DRIVIN_SAP_Listar_Pedidos_Result> listaEnt)
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
                        foreach (var T in listaEnt)
                        {
                            rpta2 = dbx.DRIVIN_SAP_Update_Placa_Guia(T.Docentry, T.placa_individual, T.prioridad, T.licencia,T.pedido_origen, rpta);
                        }
                    }
                    if (rpta2 != 0) //rpta2 == 1
                    {
                        transaction.Complete();
                        xRptaTransaccion.U_Mensaje = "Datos Modificados!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        //transaction.Complete();
                        transaction.Dispose();
                        xRptaTransaccion.U_Mensaje = "Datos no Modificados.";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
                //Console.WriteLine("Excepción interna: " + ex.InnerException);
                //throw;
                return xRptaTransaccion;
            }
            return xRptaTransaccion;
        }


        public async Task<RptaTransaccion> RegistrarPedidosDrivinAsync(List<DRIVIN_SAP_Listar_Pedidos_Result> oPedidosx)
        {
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();

            try
            {
                // Serializa la lista de pedidos a formato JSON
                string jsonPedidos = JsonConvert.SerializeObject(oPedidosx);

                // URL del API al que deseas enviar los datos
                string apiUrl = "https://app2-qa.driv.in/api/external/v2/orders?schema_code=011"; // Reemplaza con la URL real

                using (var client = new HttpClient())
                {
                    // Configura la solicitud POST
                    var content = new StringContent(jsonPedidos, Encoding.UTF8, "application/json");

                    // Realiza la solicitud POST al API
                    var response = await client.PostAsync(apiUrl, content);

                    var IsSuccessStatusCode = true; // provisional
                    // Maneja la respuesta del API
                    if (IsSuccessStatusCode) // provisional
                    //if (response.IsSuccessStatusCode)
                    {
                        // Si la solicitud fue exitosa, procesa la respuesta si es necesario
                        // Puedes parsear la respuesta JSON si el API devuelve datos
                        // Ejemplo: var jsonResponse = await response.Content.ReadAsStringAsync();
                        xRptaTransaccion.U_Mensaje = "Drivin Registrada!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else
                    {
                        // Si la solicitud falla, maneja el error
                        xRptaTransaccion.U_Mensaje = "Error al enviar los pedidos al API";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que ocurra durante el proceso
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
            }

            return xRptaTransaccion;
        }

        public RptaTransaccion RegistrarPedidosDrivin_1(List<DRIVIN_SAP_Listar_Pedidos_Result> oPedidosx,string xCodEsquema = "", string xDescripcionEsquema="", string xEscenario = "", string xPrimertoken = "")
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            listDRIVIN = new List<Client>();
           // listDRIVIN_Order = new List<Order>();

            try
            {
                foreach (var T in oPedidosx) {

                    // Order
                    listDRIVIN_Order = new List<Order>();                    
                    Order resp2 = new Order
                    {
                        code = T.code_2,
                        delivery_date = T.delivery_date,
                        vehicle_code = T.placa,
                        units_1 = T.peso,
                        units_2 = T.cantidadpedido,
                        position = (int?)T.prioridad,
                        alt_code = T.placa_individual ?? "",
                        custom_6 = T.Docnum,
                        custom_7 = T.Docentry,
                        custom_8 = T.Doctype,
                        custom_9 = T.convoy,
                        custom_10 = T.licencia,
                        custom_11 = T.nombre_conductor,
                        custom_12 = T.transporte
                    };

                    listDRIVIN_Order.Add(resp2);
                    Order[] obj5 = listDRIVIN_Order.ToArray();

                    //Time_Windows
                    listDRIVIN_TimeWindows = new List<Time_Windows>();
                    Time_Windows resp3 = new Time_Windows
                    {
                        start = T.inicio_ventana_horario,
                        end = T.fin_ventana_horario,
                    };
                    listDRIVIN_TimeWindows.Add(resp3);
                    Time_Windows[] obj6 = listDRIVIN_TimeWindows.ToArray();

                    object[] tagsArray = new object[] { T.linea_negocio };

                    //Cliente
                    Client resp = new Client
                    {                       
                        code = T.id_direccion,
                        address = T.address,
                        reference = T.reference,
                        city = T.city,
                        county = T.county,
                        country = T.country,
                        //lat = T.lat,
                        //lng = T.lng,
                        postal_code = T.postal_code,
                        name = T.name,
                        client_name = T.client_name,
                        client_code = T.id_direccion,
                        address_type = T.address_type,
                        contact_name = T.contact_name,
                        contact_phone = T.contact_phone,
                        contact_email = T.contact_email,
                        additional_contact_name = T.additional_contact_name,
                        additional_contact_phone = T.additional_contact_phone,
                        additional_contact_email = T.additional_contact_email,
                        approve_contact_name = T.approve_contact_name,
                        approve_contact_email = T.email_aprobar_ruta,
                        approve_contact_phone = T.approve_contact_phone,
                        start_contact_name = T.start_contact_name,
                        start_contact_phone = T.start_contact_phone,
                        start_contact_email = T.email_iniciar_ruta,
                        near_contact_name = T.near_contact_name,
                        near_contact_phone = T.near_contact_phone,
                        near_contact_email = T.email_en_camino,
                        delivered_contact_name = T.delivered_contact_name,
                        delivered_contact_phone = T.delivered_contact_phone,
                        delivered_contact_email = T.email_entrega_finalizada,                       
                        //service_time = T.service_time,
                        //time_windows = T.time_windows,
                        service_time = Convert.ToInt32(T.tiempo_servicio),
                        tags = tagsArray,
                        time_windows = obj6,
                        orders = obj5,
                        
                    };
                    listDRIVIN.Add(resp);
                   

                }

                Client[] obj4 = listDRIVIN.ToArray();
                
                Rootobject objJE = new Rootobject
                {
                    description = xEscenario,
                    date = DateTime.Now.ToString("dd/MM/yyyy"),                   
                    fleet_name = null,
                    schema_code = xCodEsquema,
                    clients = obj4,
                };
                
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objJE);

                //string responseBody = DrivinModel.PostDrivin("orders?schema_code=" + xCodEsquema + "&schema_name=" + xDescripcionEsquema, json);
                //string responseBody = DrivinModel.DeleteDrivin("orders/" + oCodOrder + "?token=" + oCodTokenEscenario);
                string responseBody;
                if (xPrimertoken != null && xPrimertoken !="")
                {
                     responseBody = DrivinModel.PostDrivin("orders?token=" + xPrimertoken, json);
                }
                else
                {
                     responseBody = DrivinModel.PostDrivin("scenarios", json);
                }
                //string responseBody = DrivinModel.PostDrivin("scenarios", json);

                rpta2 = 1;
                string status = "Ok";

                if (responseBody != null) //rpta2 == 1
                {
                    #region[Get Result del Insert]
                        DrivinPedidosResponse ListDrivinResult = JsonConvert.DeserializeObject<DrivinPedidosResponse>(responseBody);
                    #endregion
                    if (ListDrivinResult.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                    {
                        using (Entities_Korber db = new Entities_Korber())
                        {
                                foreach (var T in oPedidosx)
                                {
                                    rpta2 = db.DRIVIN_SAP_Update_Estado(T.Orden,T.Docentry, T.pedido_origen,T.reference ,ListDrivinResult.response.scenario_token,rpta);                                    
                                }
                        }
                            if (rpta2 == 1)
                        {
                            using (Entities_Korber db = new Entities_Korber())
                             {
                                rpta2 = db.DRIVIN_execute_job();
                             }
                            transaction.Complete();
                            xRptaTransaccion.U_Mensaje = "Drivin Registrada!";
                            xRptaTransaccion.U_Exito = true;
                        }
                        else {
                            transaction.Dispose();
                            xRptaTransaccion.U_Mensaje = "Drivin no Registrada.";
                            xRptaTransaccion.U_Exito = false;
                        }
                    }
                }
                }
                else
                {
                    //transaction.Dispose();
                    xRptaTransaccion.U_Mensaje = "Drivin no Registrada." ;
                    xRptaTransaccion.U_Mensaje += " Error";
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

        public List<Drivin_General> ListarPlacaDrivin(string CodigoPlaca = "")
        {
            List<Drivin_General> ListPlacaDrivin = null;                                
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            int rpta2 = 0;
            try
            {
                #region[Obtener Placa Drivin]
                string responseBody = DrivinModel.GetDrivin("vehicles");
                if (responseBody != null)
                {
                    DrivinVehiculosEstado ListDrivin = JsonConvert.DeserializeObject<DrivinVehiculosEstado>(responseBody);
                    if (ListDrivin.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.response.Count() > 0)
                            {
                                using (Entities_Korber db = new Entities_Korber())
                                {
                                    foreach (var T in ListDrivin.response)
                                    {
                                      rpta2 = db.DRIVIN_Insertar_vehiculo(Convert.ToString(T.id), T.code,T.description, T.capacity_1, T.capacity_2, T.capacity_3, T.fleets, rpta);
                                    }
                                    if (rpta2 == 1) {
                                        transaction.Complete();
                                    }
                                    else {
                                        transaction.Dispose();
                                    }
                                    
                                }
                           }

                        }
                    }

                }
                #endregion

                using (Entities_Korber db = new Entities_Korber())
                ListPlacaDrivin = (db.DRIVIN_Listar_General(CodigoPlaca).ToList()).ConvertAll(x => new Drivin_General
                {
                   U_CodTipo = x.code,
                   U_TipoDesc = x.descripcion
                });
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
            return ListPlacaDrivin;
        }

        #region[Eliminar Pedido]
        public RptaTransaccion EliminarPedidoDrivin(string oCodOrder = "", string oNumeroGuia = "", string oCustPoNumber = "", string oReferencia = "", string oCodTokenEscenario = "")
        {
            int rpta2 = 0;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            try
            {
                string responseBody = DrivinModel.DeleteDrivin("orders/" + oCodOrder + "?token=" + oCodTokenEscenario);

                if (responseBody != null)
                {
                    DrivinPedidoResponseEL ListDrivinResponseEl = JsonConvert.DeserializeObject<DrivinPedidoResponseEL>(responseBody);
                    if (ListDrivinResponseEl.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivinResponseEl.response.Count() > 0)
                            {
                                using (Entities_Korber db = new Entities_Korber())
                                {
                                    //foreach (var T in ListDrivinResponseEl.response)
                                    //{
                                        //limpiar token y estado en cero
                                        rpta2 = db.DRIVIN_SAP_Update_Estado(oCodOrder, oNumeroGuia, oCustPoNumber, oReferencia, "", rpta);
                                    //}
                                    if (rpta2 == 1)
                                    {
                                        transaction.Complete();
                                        xRptaTransaccion.U_Mensaje = "Drivin Pedido Desasignado!";
                                        xRptaTransaccion.U_Exito = true;
                                    }
                                    else
                                    {
                                        transaction.Dispose();
                                        xRptaTransaccion.U_Mensaje = "Drivin Pedido no Desasignado.";
                                        xRptaTransaccion.U_Exito = false;
                                    }
                                }
                            }
                            else
                            {
                                xRptaTransaccion.U_Mensaje = "Drivin Pedido no Desasignado.";
                                //xRptaTransaccion.U_Mensaje += " Error";
                                xRptaTransaccion.U_Exito = false;
                            }
                        }
                    }
                    else
                    {
                        xRptaTransaccion.U_Mensaje = "Drivin Pedido no Desasignado.";
                        //xRptaTransaccion.U_Mensaje += " Error";
                        xRptaTransaccion.U_Exito = false;
                    }
                }
                else {
                    xRptaTransaccion.U_Mensaje = "Drivin Pedido no Desasignado.";
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
        #endregion

        #region[Listar Esquema]
        public List<Drivin_General> ListarEsquemaDrivin(string CodigoEsquema = "")
        {
            List<Drivin_General> ListEsquemaDrivin = null;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            int rpta2 = 0;
            try
            {
                #region[Obtener Esquema Drivin]
                string responseBody = DrivinModel.GetDrivin("schemas");
                if (responseBody != null)
                {
                    DrivinEsquemasEstado ListDrivin = JsonConvert.DeserializeObject<DrivinEsquemasEstado>(responseBody);
                    if (ListDrivin.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.response.Count() > 0)
                            {
                                using (Entities_Korber db = new Entities_Korber())
                                {
                                    foreach (var T in ListDrivin.response)
                                    {
                                        rpta2 = db.DRIVIN_Insertar_esquema(T.code, T.name, rpta);
                                    }
                                    if (rpta2 == 1)
                                    {
                                        transaction.Complete();
                                    }
                                    else
                                    {
                                        transaction.Dispose();
                                    }

                                }
                            }

                        }
                    }

                }
                #endregion

                using (Entities_Korber db = new Entities_Korber())
                    ListEsquemaDrivin = (db.DRIVIN_Listar_General(CodigoEsquema).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
            }
            catch
            {
                return null;
            }
            return ListEsquemaDrivin;
        }

        #endregion

        #region[Actualizar Direcciones]
        public RptaTransaccion Actualizar_Direcciones_Drivin(string oCodDireccion = "",
                                                             string oDireccion = "",
                                                             string oCity = "",
                                                             string oState = "",
                                                             string oCounty = "",
                                                             string oCountry = "",
                                                             string oLatitud = "", 
                                                             string oLongitud = "",
                                                             string oNombreDireccion = "",
                                                             string oCliente = "",                                                             
                                                             string oCodCliente = "",
                                                             string oAddressType = "",
                                                              string oContactName = ""
                                                             )
        {
            int rptax = 0;
            string MsgRpta = "";

            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();
            ListDireccionesEnviar = new List<Address_Post>();

            try
            {
                Address_Post objDirecciones = new Address_Post
                {
                    code = oCodDireccion,
                    address1 = oDireccion,
                    city = oCity,
                    state = oState,
                    county = oCounty,
                    country = "Peru",
                    //zip_code = oCodDireccion,
                    lat = float.Parse(oLatitud),
                    lng = float.Parse(oLongitud),
                    name = oNombreDireccion,
                    client = oCliente,
                    client_code = oCodCliente,
                    address_type = oAddressType,
                    contact_name = oContactName,
                };
                ListDireccionesEnviar.Add(objDirecciones);
                Address_Post[] obj5 = ListDireccionesEnviar.ToArray();

                Drivin_Direcciones_Post ObjJE = new Drivin_Direcciones_Post
                {
                    addresses = obj5,
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(ObjJE);
                string responseBody = DrivinModel.PostDrivin("addresses", json);
                rptax = 1;

                if (responseBody != null) 
                {
                    #region[Result del Insert]                    
                    DireccionResponse ListDrivinResult = JsonConvert.DeserializeObject<DireccionResponse>(responseBody);
                    #endregion
                    if (ListDrivinResult.status == "OK")
                    {
                        xRptaTransaccion.U_Mensaje = "Drivin Registrada!";
                        xRptaTransaccion.U_Exito = true;
                    }
                    else {
                        xRptaTransaccion.U_Mensaje = "Drivin no Registrada.";
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

        #endregion

        #region[Listar transportistas]
        public List<Drivin_General> ListarTransportistaDrivin(string email = "")
        {
            List<Drivin_General> ListTransportistaDrivin = null;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            int rpta2 = 0;
            try
            {
                #region[Obtener Transportista Drivin]
                string responseBody = DrivinModel.GetDrivin("users");
                if (responseBody != null)
                {
                    DrivinTransportistaEstado ListDrivin = JsonConvert.DeserializeObject<DrivinTransportistaEstado>(responseBody);
                    if (ListDrivin.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.response.Count() > 0)
                            {
                                using (Entities_Korber db = new Entities_Korber())
                                {
                                    foreach (var T in ListDrivin.response)
                                    {
                                        rpta2 = db.DRIVIN_Insertar_transportista(T.email, T.phone, T.first_name, T.last_name, T.dni, T.license_number, T.role_name,T.organization, rpta);
                                    }
                                    if (rpta2 == 1)
                                    {
                                        transaction.Complete();
                                    }
                                    else
                                    {
                                        transaction.Dispose();
                                    }

                                }
                            }

                        }
                    }

                }
                #endregion

                using (Entities_Korber db = new Entities_Korber())
                    ListTransportistaDrivin = (db.DRIVIN_Listar_General(email).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
            return ListTransportistaDrivin;
        }
        #endregion

        #region[Listar Escenarios]
        public List<Drivin_General> ListarPlanEscenariosDrivin(string CodigoEscenario = "")
        {        
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();

            List<Drivin_General> ListPlanEscenarioDrivin = null;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            int rpta2 = 0;
            try
            {
                #region[Obtener Plan Escenario Drivin]
                string responseBody = DrivinModel.GetDrivin("scenarios");
                if (responseBody != null)
                {
                    DrivinEscenariosEstado ListDrivin = JsonConvert.DeserializeObject<DrivinEscenariosEstado>(responseBody);
                    if (ListDrivin.status == "OK")
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.response.Count() > 0)
                            {
                                using (Entities_Korber db = new Entities_Korber())
                                {
                                    rpta2 = db.DRIVIN_Eliminar_escenarios(rpta);                                    

                                    foreach (var T in ListDrivin.response.Where(T => T.status == "Started" || T.status == "Optimized" || T.status == "Ready").ToList())
                                    {
                                        rpta2 = db.DRIVIN_Insertar_escenarios(T.token, T.description, T.status, T.schema_name, T.schema_code, rpta);
                                    }                                   
                                    transaction.Complete();
                                    //transaction.Dispose();                                    
                                }
                            }

                        }
                    }

                }
                #endregion

                using (Entities_Korber db = new Entities_Korber())
                    ListPlanEscenarioDrivin = (db.DRIVIN_Listar_General(CodigoEscenario).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;                
            }
            
            return ListPlanEscenarioDrivin;
        }

        #endregion

        #region[Listar Placas Securitas]
        public async Task<List<Drivin_General>> ListarPlacaSecuritasDrivinAsync(string CodigoPlaca = "")
        {
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();

            List<Drivin_General> ListPlacaSecuritas = null;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            int rpta2 = 0;
            try
            {
                #region[Obtener  Placas Securitas]
                string responseBody = await DrivinModel.GetDrivinAsync("trackables?offset=0&limit=100").ConfigureAwait(false); 

                if (responseBody != null)
                {
                    DrivinPlacaSecuritasEstado ListDrivin = JsonConvert.DeserializeObject<DrivinPlacaSecuritasEstado>(responseBody);
                    //if (ListDrivin.status == "OK")
                    //{
                        using (TransactionScope transaction = new TransactionScope())
                        {
                            if (ListDrivin.items.Count() > 0)
                            {
                                using (Entities_Korber db = new Entities_Korber())
                                {                                  
                                    foreach (var T in ListDrivin.items.ToList())
                                    {                                       
                                        rpta2 = db.DRIVIN_Insertar_placa_securitas(T.attributes.plateNumber, T.attributes.type, T.attributes.name, T.attributes.identifier, rpta);
                                    }                                                                  
                                transaction.Complete();
                                //transaction.Dispose();                                    
                                }
                            }

                        }
                   // }

                }
                #endregion

                using (Entities_Korber db = new Entities_Korber())
                    ListPlacaSecuritas = (db.DRIVIN_Listar_General(CodigoPlaca).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
            }

            return ListPlacaSecuritas;
        }

        #endregion

        #region[Listar Conductor Licencias Securitas]
        public async Task<List<Drivin_General>> ListarLicenciasSecuritasDrivinAsync(string CodigoLicencias = "")
        {
            RptaTransaccion xRptaTransaccion = new RptaTransaccion();

            List<Drivin_General> ListLicenciasSecuritas = null;
            ObjectParameter rpta = new ObjectParameter("Rpta", typeof(int));
            int rpta2 = 0;
            try
            {
                #region[Obtener  Licencias Securitas]
                string responseBody = await DrivinModel.GetDrivinAsync("drivers?offset=0&limit=100").ConfigureAwait(false);

                if (responseBody != null)
                {
                    DrivinLicenciasSecuritasEstado ListDrivin = JsonConvert.DeserializeObject<DrivinLicenciasSecuritasEstado>(responseBody);
                    //if (ListDrivin.status == "OK")
                    //{
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        if (ListDrivin.items.Count() > 0)
                        {
                            using (Entities_Korber db = new Entities_Korber())
                            {
                                foreach (var T in ListDrivin.items.Where(T => !string.IsNullOrEmpty(T.identifier)).ToList())
                                    //foreach (var T in ListDrivin.items.ToList(T => T. == "Started"))
                                {
                                    rpta2 = db.DRIVIN_Insertar_licencias_securitas(T.identifier, T.name, T.description, rpta);
                                }
                                transaction.Complete();
                                //transaction.Dispose();                                    
                            }
                        }
                    }
                    // }

                }
                #endregion

                using (Entities_Korber db = new Entities_Korber())
                    ListLicenciasSecuritas = (db.DRIVIN_Listar_General(CodigoLicencias).ToList()).ConvertAll(x => new Drivin_General
                    {
                        U_CodTipo = x.code,
                        U_TipoDesc = x.descripcion
                    });
            }
            catch (Exception ex)
            {
                xRptaTransaccion.U_Mensaje = ex.Message;
                xRptaTransaccion.U_Exito = false;
            }

            return ListLicenciasSecuritas;
        }

        #endregion

    }
}