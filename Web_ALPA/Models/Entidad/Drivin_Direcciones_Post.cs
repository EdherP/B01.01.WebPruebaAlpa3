using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{

    public class Drivin_Direcciones_Post
    {
        public Address_Post[] addresses { get; set; }
    }

    public class Address_Post
    {
        public string code { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string country { get; set; }
        public object zip_code { get; set; }
        public float? lat { get; set; }
        public float? lng { get; set; }
        public string name { get; set; }
        public string client { get; set; }
        public string client_code { get; set; }
        public string address_type { get; set; }
        public string contact_name { get; set; }
        public object sales_zone_code { get; set; }
        public object sales_zone_name { get; set; }
        public object supplier_code { get; set; }
        public object supplier_name { get; set; }
        public object priority { get; set; }
        public bool update_all { get; set; }
    }

}