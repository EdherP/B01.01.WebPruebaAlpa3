
using Newtonsoft.Json;
using System;
using System.Collections.Generic; 

namespace Web_ALPA.Models.General
{ 
    public class ComunModel
    {
        DocRUC oDatax = new DocRUC();
        DocDNI oData = new DocDNI();
        readonly ConexionAPI oCNx = new ConexionAPI();
        public DocRUC Verificar_RUC(string Urlx)
        {
           // List<RootObject> mydata;
            oDatax = new DocRUC();
            try
            {
                string responseBody = oCNx.Get_Data2(Urlx);
                oDatax = JsonConvert.DeserializeObject<DocRUC>(responseBody);  
            }
            catch
            {
                return null;
            }
            return oDatax;
        }
        public DocDNI Verificar_DNI(string Urlx)
        {
            oData = new DocDNI();
            try
            {
                string responseBody = oCNx.Get_Data2(Urlx);
                oData = JsonConvert.DeserializeObject<DocDNI>(responseBody);
            }
            catch
            {
                return null;
            }
            return oData;
        }
    }
}