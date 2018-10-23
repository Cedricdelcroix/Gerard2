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
        public int nbOfTry { get; set; }

        public Player()
        {
        }

        public Player(string name, int price, int numberMaxItem = 1, int nbOfTry = 1)
        {
            this.name = name;
            this.price = price;
            this.numberMaxItem = numberMaxItem;
            this.nbOfTry = nbOfTry;
        }
    }
}
