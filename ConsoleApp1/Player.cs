using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Player
    {
        public int id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string place { get; set; }
        public int numberOfItem { get; set; }
        public int numberMaxItem { get; set; }


        public Player(string name, int price)
        {
            this.name = name;
            this.price = price;
            numberMaxItem = 1;
        }
    }
}
