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
            ChromeOptions chOption = new ChromeOptions();
            chOption.AddArgument("user-data-dir=D:/Profiles/cdelcroix/AppData/Local/Google/Chrome");
            //IWebDriver driver = new ChromeDriver(chOption);
            FirefoxOptions fOption = new FirefoxOptions();
            fOption.AddArgument("user-data-dir=C:/Users/cedric/AppData/Local/Mozilla/Firefox/Profiles");
            IWebDriver driver = new FirefoxDriver(fOption);
            // create file named Cookies to store Login Information		

            //foreach (CustomCookie cookie in cookies)
            //{
            //    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //    date = date.AddSeconds(cookie.expirationDate);
            //    Cookie myCookie = new Cookie(cookie.name, cookie.value, cookie.domain, cookie.path, date);
            //    driver.Manage().Cookies.AddCookie(myCookie);
            //}

            Launcher launcher = new Launcher(driver);
            launcher.run();
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
