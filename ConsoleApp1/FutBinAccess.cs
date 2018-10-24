using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    class FutBinAccess
    {
        public IWebDriver driver;
        public static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FutBinAccess(IWebDriver webDriver)
        {
            driver = webDriver;
        }

        public List<Player> initPlayerList(int priceStart, int priceEnd, int pageIndex)
        {
            List<Player> players = new List<Player>();
            driver.Navigate().GoToUrl("https://www.futbin.com/players?page=" + pageIndex + "&ps_price=" + priceStart + "-" + priceEnd + "&sort=likes&order=desc&version=all_nif");
            var elemTable = driver.FindElement(By.XPath("//table[@id = 'repTb']/tbody"));
            List<IWebElement> listItem = new List<IWebElement>(elemTable.FindElements(By.TagName("tr")));
            foreach (var elem in listItem)
            {
                IWebElement nameElement = elem.FindElement(By.TagName("a"));
                IWebElement priceElement = elem.FindElement(By.CssSelector(".ps4_color"));
                if (nameElement.Text.Contains(" ") && !priceElement.Text.Equals("0"))
                {
                    players.Add(new Player(nameElement.Text, Utils.convertPrice(priceElement.Text)));
                }
            }
            return players;
        }
    }

}
