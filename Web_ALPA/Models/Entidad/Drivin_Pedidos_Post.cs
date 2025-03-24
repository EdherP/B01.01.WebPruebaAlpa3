using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ALPA.Models.Entidad
{
    public class Drivin_Pedidos_post
    {


        public class Rootobject
        {
            public string description { get; set; }
            public string date { get; set; }
            public object fleet_name { get; set; }
            public string schema_code { get; set; }
            public Client[] clients { get; set; }
        }

        public class Client
        {
            public string code { get; set; }
            public string address { get; set; }
            public string reference { get; set; }
            public string city { get; set; }
            public string county { get; set; }
            public string country { get; set; }
            public float? lat { get; set; }
            public float? lng { get; set; }
            public string postal_code { get; set; }
            public string name { get; set; }
            public string client_name { get; set; }
            public object client_code { get; set; }
            public string address_type { get; set; }
            public string contact_name { get; set; }
            public string contact_phone { get; set; }
            public string contact_email { get; set; }
            public string additional_contact_name { get; set; }
            public string additional_contact_phone { get; set; }
            public string additional_contact_email { get; set; }
            public string start_contact_name { get; set; }
            public string start_contact_phone { get; set; }
            public string start_contact_email { get; set; }
            public string near_contact_name { get; set; }
            public string near_contact_phone { get; set; }
            public string near_contact_email { get; set; }
            public string delivered_contact_name { get; set; }
            public string delivered_contact_phone { get; set; }
            public string delivered_contact_email { get; set; }
            public string approve_contact_name { get; set; }
            public string approve_contact_phone { get; set; }
            public string approve_contact_email { get; set; }
            public int service_time { get; set; }
            public string sales_zone_code { get; set; }
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
            public string alt_code { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public object units_1 { get; set; }
            public object units_2 { get; set; }
            public object units_3 { get; set; }
            public int? position { get; set; }
            public string delivery_date { get; set; }
            public string custom_1 { get; set; }
            public object custom_2 { get; set; }
            public object custom_3 { get; set; }
            public object custom_4 { get; set; }
            public object custom_5 { get; set; }
            public object custom_6 { get; set; }
            public object custom_7 { get; set; }
            public object custom_8 { get; set; }
            public object custom_9 { get; set; }
            public object custom_10 { get; set; }
            public object custom_11 { get; set; }
            public object custom_12 { get; set; }
            public string supplier_code { get; set; }
            public string supplier_name { get; set; }
            public string deploy_date { get; set; }
            public string vehicle_code { get; set; }             
            public Item[] items { get; set; }
            public object[] pickups { get; set; }
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