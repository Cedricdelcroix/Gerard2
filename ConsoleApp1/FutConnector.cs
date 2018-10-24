using log4net;
using log4net.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    class FutConnector
    {
        public static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        IWebDriver driver;
        public int coins = 0;
        List<Player> playerList = new List<Player>();
        List<Player> playerOnMarket = new List<Player>();
        MarketPlace marketPlace;
        TransfertList transfertList;
        FutBinAccess futBinAccess;
        WebDriverWait wait;
        public FutConnector()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            XmlConfigurator.Configure();
            ChromeOptions chOption = new ChromeOptions();
            chOption.AddArgument("user-data-dir=D:/Profiles/cdelcroix/AppData/Local/Google/Chrome");
            driver = new ChromeDriver(chOption);
            marketPlace = new MarketPlace(driver);
            transfertList = new TransfertList(driver);
            futBinAccess = new FutBinAccess(driver);
        }

      

        public void run(int pagination = 1)
        {
            //Récupérer la liste des joueurs
            playerList = futBinAccess.initPlayerList(2000,15000,pagination);
            //Aller sur EaSport
            OpenEaFut();
            clickConnection();
            if (IsLogged())
            {

            }
            //Se connecter

            //Aller sur la liste des joueur

            //récupérer la liste des joueurs

            //récupérer les crédits.

            //verifier la taille de la liste et les crédits

            //Procedure de recherche
        }

        public void clickConnection()
        {
            IWebElement connexion = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[text() = 'Connexion']")));
            Console.WriteLine("Connexion clique");
        }

        public void OpenEaFut()
        {

            driver.Navigate().GoToUrl("https://www.easports.com/fr/fifa/ultimate-team/web-app/");
            _log.Info("Application Connected");
        }

        public String WhereIAm()
        {
            return "";
        }

        public bool IsLogged()
        {
            try
            {
                Thread.Sleep(1000);
                driver.FindElement(By.Id("btnLogin"));
                return false;
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                return true;
            }
        }

        public void Login(String mail, String pass)
        {
            IWebElement email = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//input[@type='email']")));
            email.SendKeys(mail);
            IWebElement passwd = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//input[@type='password']")));
            passwd.SendKeys(pass);
            IWebElement btnLogin = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("btnLogin")));
            _log.Info("LoginSuccess");
        }
    }
}
