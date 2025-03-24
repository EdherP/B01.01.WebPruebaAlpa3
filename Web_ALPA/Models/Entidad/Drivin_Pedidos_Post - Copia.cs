using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Pedidos_post_1
    {

        public class Rootobject
        {
            public string description { get; set; }
            public string date { get; set; }
            public object fleet_name { get; set; }
            public string schema_name { get; set; }
            public Client[] clients { get; set; }
        }

        public class Client
        {
            public object code { get; set; }
            public string address { get; set; }
            public string reference { get; set; }
            public string city { get; set; }
            public string county { get; set; }
            public string country { get; set; }
            public float lat { get; set; }
            public float lng { get; set; }
            public string name { get; set; }
            public string client_name { get; set; }
            public object client_code { get; set; }
            public string address_type { get; set; }
            public string contact_name { get; set; }
            public string contact_phone { get; set; }
            public string contact_email { get; set; }
            public string approve_contact_name { get; set; }
            public string approve_contact_phone { get; set; }
            public string approve_contact_email { get; set; }
            public string start_contact_name { get; set; }
            public string start_contact_phone { get; set; }
            public string start_contact_email { get; set; }
            public string near_contact_name { get; set; }
            public string near_contact_phone { get; set; }
            public string near_contact_email { get; set; }
            public string delivered_contact_name { get; set; }
            public string delivered_contact_phone { get; set; }
            public string delivered_contact_email { get; set; }
            public object service_time { get; set; }
            public int priority { get; set; }
            public Time_Windows[] time_windows { get; set; }
            public object[] tags { get; set; }
            public Order[] orders { get; set; }
        }

        public class Time_Windows
        {
            public string start { get; set; }
            public string end { get; set; }
        }

        public class Order
        {
            public string code { get; set; }
            public object alt_code { get; set; }
            public string description { get; set; }
            public object units_1 { get; set; }
            public object units_2 { get; set; }
            public object units_3 { get; set; }
            public object position { get; set; }
            public string vehicle_code { get; set; }
            public string delivery_date { get; set; }
            public string billing_date { get; set; }
            public object custom_1 { get; set; }
            public object custom_2 { get; set; }
            public object custom_3 { get; set; }
            public object suppler_code { get; set; }
            public Item[] items { get; set; }
        }

        public class Item
        {
            public string code { get; set; }
            public string description { get; set; }
            public int units { get; set; }
            public int units_1 { get; set; }
            public object units_2 { get; set; }
            public object units_3 { get; set; }
        }


    }
}