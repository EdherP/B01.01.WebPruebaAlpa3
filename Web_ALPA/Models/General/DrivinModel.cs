using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Web_ALPA.Models.Entidad;

namespace Web_ALPA.Models.General
{
    public class DrivinModel
    {
        public static string oAPI_DRIVIN = ConfigurationManager.AppSettings["oAPI_DRIVIN"];
        public static string oAPI_KEY_DRIVIN = ConfigurationManager.AppSettings["oAPI_KEY_DRIVIN"];

        //Configuracion OAuth 2.0
        private static string oTokenUrl = System.Configuration.ConfigurationManager.AppSettings["OAuth2_AccessTokenUrl"];
        private static string oClientId = System.Configuration.ConfigurationManager.AppSettings["OAuth2_ClientId"];
        private static string oClientSecret = System.Configuration.ConfigurationManager.AppSettings["OAuth2_ClientSecret"];
        private static string oUsername = System.Configuration.ConfigurationManager.AppSettings["OAuth2_Username"];
        private static string oPassword = System.Configuration.ConfigurationManager.AppSettings["OAuth2_Password"];
        private static string oScope = System.Configuration.ConfigurationManager.AppSettings["OAuth2_Scope"];        
        private static string oAPI_SECURITAS = System.Configuration.ConfigurationManager.AppSettings["oAPI_OAuth2SECURITAS"];


        public static string GetDrivin(string TipoDoc)
        {
            string oResult = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var HttpWebRequest = (HttpWebRequest)WebRequest.Create(oAPI_DRIVIN + TipoDoc);
                
                HttpWebRequest.Method = "GET";
                HttpWebRequest.ContentType = "application/json";
                HttpWebRequest.Headers.Add("X-API-Key", oAPI_KEY_DRIVIN);

                var httpResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
                var oStatus = httpResponse.StatusCode;
                if (oStatus == System.Net.HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        oResult = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                var oMsg = ex.Message;
                oResult = null;
            }
            return oResult;
        }

        public static string PostDrivin(string TipoDoc, string oJson)
        {
            string oResult =  string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var HttpWebRequest = (HttpWebRequest)WebRequest.Create(oAPI_DRIVIN + TipoDoc);
                HttpWebRequest.Method = "POST";
                HttpWebRequest.ContentType = "application/json";
                //HttpWebRequest.Accept = "application/json";
                //HttpWebRequest.ContentLength = 0;
                HttpWebRequest.Headers.Add("X-API-Key", oAPI_KEY_DRIVIN);

                using (var streamWriter = new StreamWriter(HttpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(oJson);
                }

                var httpResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
                var oStatus = httpResponse.StatusCode;
                if (oStatus == System.Net.HttpStatusCode.OK)
                {
                   // oEstado = "Ok";
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        oResult = streamReader.ReadToEnd();
                    }
                }

            }
            catch (Exception ex)
            {
                //string rpt = ex.Message;
                //return "Error";
                oResult = ex.Message;
            }
            return oResult;
        }

        public static string DeleteDrivin(string TipoDoc)
        {
            string oResult = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var HttpWebRequest = (HttpWebRequest)WebRequest.Create(oAPI_DRIVIN + TipoDoc);
                HttpWebRequest.Method = "DELETE";
                HttpWebRequest.ContentType = "application/json";
                HttpWebRequest.Headers.Add("X-API-Key", oAPI_KEY_DRIVIN);

                var httpResponse = (HttpWebResponse)HttpWebRequest.GetResponse();
                var oStatus = httpResponse.StatusCode;
                if (oStatus == System.Net.HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        oResult = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                oResult = ex.Message;
            }
            return oResult;
        }

        #region[OAuth2]
        public static async Task<string> ObtenerTokenAsync()
        {
            string tokenUrl = oTokenUrl;
            string clientId = oClientId;
            string clientSecret = oClientSecret;
            string username = oUsername;
            string password = oPassword;
            string scope = oScope;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            // Construir los parámetros de la solicitud            
            string postData = $"grant_type=password&username={WebUtility.UrlEncode(username)}&password={WebUtility.UrlEncode(password)}&scope={WebUtility.UrlEncode(scope)}";
            // Crear la solicitud HTTP POST
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tokenUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Codificar las credenciales de cliente para la cabecera de autorización
            string base64Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers[HttpRequestHeader.Authorization] = $"Basic {base64Auth}";

            // Escribir los datos POST en el cuerpo de la solicitud
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            try
            {
                // Obtener el flujo de datos de la solicitud y escribir los datos
                using (Stream dataStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
                {
                    await dataStream.WriteAsync(byteArray, 0, byteArray.Length).ConfigureAwait(false);
                }

                // Obtener la respuesta del servidor
                using (WebResponse response = await request.GetResponseAsync().ConfigureAwait(false))
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseText = await reader.ReadToEndAsync().ConfigureAwait(false);
                    Console.WriteLine(responseText); // Imprime la respuesta JSON (opcional)

                    // Deserializar la respuesta JSON para obtener el token de acceso
                    Drivin_Token_Response tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Drivin_Token_Response>(responseText);

                    // Retornar el token de acceso
                    return tokenResponse.access_token;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error en la solicitud HTTP para obtener el token: {ex.Message}");
                throw; 
            }
        }

        public static async Task<string> GetDrivinAsync(string tipoDoc)
        {
            string oResult = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                // Obtener el token de acceso de manera asíncrona                
                string token = await ObtenerTokenAsync();
                // Crear la solicitud GET con el token en la cabecera de autorización
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(oAPI_SECURITAS + tipoDoc);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

                // Obtener la respuesta del servidor
                using (var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync().ConfigureAwait(false))
                {
                    var oStatus = httpResponse.StatusCode;
                    if (oStatus == HttpStatusCode.OK)
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            oResult = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oResult = ex.Message;
            }
            return oResult;
        }

        #endregion
    }
}