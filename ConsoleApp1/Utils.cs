using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }


}
