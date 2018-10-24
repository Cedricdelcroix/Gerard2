using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    static class Utils
    {
        public static int convertPrice(string price)
        {
            price = price.Replace(" ", "");
            if (string.Empty != price)
            {
                if (price.Contains("K"))
                {
                    double tmp;
                    double.TryParse(price.Replace("K", "").Replace(".", ","), out tmp);

                    return Convert.ToInt32(tmp * 1000);
                }
                else
                {
                    return int.Parse(price.Replace("K", ""));
                }
            }
            return 0;
        }
    }
}
