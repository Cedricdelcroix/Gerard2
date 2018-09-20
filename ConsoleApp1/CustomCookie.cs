using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class CustomCookie
    {
        public string domain { get; set;}
        public double expirationDate { get; set; }
        public bool hostOnly { get; set; }
        public bool httpOnly { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string sameSite { get; set; }
        public string secure { get; set; }
        public bool session { get; set; }
        public int storeId { get; set; }
        public string value { get; set; }
        public int id { get; set; }
    }
}
