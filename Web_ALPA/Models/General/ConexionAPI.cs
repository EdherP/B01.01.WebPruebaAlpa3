using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Web_ALPA.Models.Entidad;

namespace Web_ALPA.Models.General
{
    public class ConexionAPI
    {
        public string Get_Data(string Url)
        {
            int ominutos = 4;
            string responseBody = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Proxy = null;
                request.Timeout = (1000 * 60 * ominutos);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return string.Empty;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return string.Empty;
            }
            return responseBody;
        }
        public string Get_Data2(string Url)
        { 
            string responseBody = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                //request.Credentials = CredentialCache.DefaultCredentials;
                //request.Proxy = null;
                request.Timeout = -1;
                
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return string.Empty;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
            return responseBody;
        }
        public RptaTransaccion Post_Data(string JsonData, string Url)
        {
            RptaTransaccion RptaTR = new RptaTransaccion();
            int aTimeoutSeconds = 480;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = aTimeoutSeconds * 1000;
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader != null)
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                RptaTR = JsonConvert.DeserializeObject<RptaTransaccion>(responseBody);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                RptaTR.U_Mensaje = ex.Message;
                RptaTR.U_Exito = false;
                return RptaTR;
            }
            return RptaTR;
        }
        public RptaTransaccion Post_Data2(string Url)
        {
            RptaTransaccion RptaTR = new RptaTransaccion();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.ContentLength = 0;
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader != null)
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                RptaTR = JsonConvert.DeserializeObject<RptaTransaccion>(responseBody);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                RptaTR.U_Mensaje = ex.Message;
                RptaTR.U_Exito = false;
                return RptaTR;
            }
            return RptaTR;
        }
    }
}