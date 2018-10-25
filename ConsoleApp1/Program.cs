using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {

        public static List<CustomCookie> cookies = new List<CustomCookie>();
        public static void Main(string[] args)
        {
            //getCookies();
            //ChromeOptions chOption = new ChromeOptions();
            //chOption.AddArgument("user-data-dir=D:/Profiles/cdelcroix/AppData/Local/Google/Chrome");
            //IWebDriver driver = new ChromeDriver(chOption);

            //FirefoxProfile profile = new FirefoxProfile("D:/Profiles/cdelcroix/AppData/Local/Mozilla/Firefox/Profiles");
            //Proxy proxy = new Proxy();
            //proxy.ProxyAutoConfigUrl = "http://wpad.lill.fr.sopra/wpad.dat ";
            //FirefoxOptions firefoxOptions = new FirefoxOptions();
            //firefoxOptions.AddArguments("- profile", "D:/Profiles/cdelcroix/AppData/Local/Mozilla/Firefox/Profiles");
            //firefoxOptions.Proxy = proxy;
            ////firefoxOptions.Profile = profile;
            //IWebDriver driver = new FirefoxDriver(firefoxOptions);
            // create file named Cookies to store Login Information		

            //foreach (CustomCookie cookie in cookies)
            //{
            //    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //    date = date.AddSeconds(cookie.expirationDate);
            //    Cookie myCookie = new Cookie(cookie.name, cookie.value, cookie.domain, cookie.path, date);
            //    driver.Manage().Cookies.AddCookie(myCookie);
            //}

            FutConnector futConnector = new FutConnector();
            futConnector.run();
        }

        static void getCookies()
        {
            string path = "D:\\Profiles\\cdelcroix\\Documents\\Visual Studio 2017\\Projects\\ConsoleApp1\\ConsoleApp1\\ressource\\ea_cookies.JSON";
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                cookies = JsonConvert.DeserializeObject<List<CustomCookie>>(json);
            }
        }
    }
}
