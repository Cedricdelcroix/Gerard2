using ConsoleApp1;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    class MarketPlace : Transfert
    {
        public static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Double POURCENTAGE_ACHAT = 5.00;
        private static Double POURCENTAGE_VENTE = 10.00;
        public int nbOfTry = 0;
        int count = 0;
        WebDriverWait wait;
        public MarketPlace(IWebDriver webDriver) : base(webDriver)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void GoToMarketPlace()
        {
            GoToTransfert();
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("ut-tile-transfer-market")));
            _log.Info("Enter On market list");
        }

        public void FindPlayer(Player p)
        {
            SetPlayerName(p);
            if (IsPlayerExist())
            {
                IWebElement playerselect = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//ul[contains(@class,'playerResultsList')]/button")));
                SetPriceSearch(p);
                IWebElement searchButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Rechercher')]")));
                tryBuyPlayer(p);
            }
        }

        public void SetPriceSearch(Player p)
        {
            IWebElement element = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Max')]/parent::div/following-sibling::div/div/input")));
            element.Clear();
            element.SendKeys(Convert.ToInt32(p.price / (1 + POURCENTAGE_ACHAT / 100)).ToString());
        }

        public void SetPlayerName(Player p)
        {
            IWebElement playerInput = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//input[@placeholder='Saisissez nom joueur']")));
            playerInput.Clear();
            playerInput.SendKeys(p.name);

        }

        public Boolean IsPlayerExist()
        {
            try
            {
                Thread.Sleep(300);
                driver.FindElement(By.XPath("//ul[contains(@class,'playerResultsList')]/button"));
                Console.WriteLine("Le joueur existe");
                return true;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Le joueur n'existe pas");
                return false;
            }
        }

        public bool IsPlayerFind(Player p)
        {
            // si le joueur est trouver essayer de l'acheter 
            try
            {
                Thread.Sleep(300);
                driver.FindElement(By.XPath("//div[contains(@class, 'pagingContainer')]/following-sibling::ul/li"));
                IWebElement time = driver.FindElement(By.XPath("//span[contains(@class,'time')]"));
                if (IsBuyable(time.Text))
                {
                    Console.WriteLine(p.name + " trouvé");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                Console.WriteLine(p.name + " non trouvé au prix de " + p.price);
                return false;
            }
        }

        public bool IsBuyable(string time)
        {
            List<string> buyableTime = new List<string> { "59 minutes", "1 Heure", "1 heure" };
            foreach (string s in buyableTime)
            {
                if (s == time)
                {
                    Console.WriteLine("buyable");
                    return true;
                }
            }

            //check nb of Contract
            Console.WriteLine("not buyable");
            nbOfTry = 2;
            return false;
        }

        public bool IsPlayerBuy(Player p)
        {
            try
            {
                Thread.Sleep(1000);
                driver.FindElement(By.XPath("//span[contains(text(),'Bravo')]"));
                p.numberOfItem++;
                return true;
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                return false;
            }
        }

        public void tryBuyPlayer(Player p)
        {

            if (IsPlayerFind(p))
            {
                // Buy Player
                BuyPlayer(p);
            }
            else
            {
                if (nbOfTry <= p.nbOfTry)
                {
                    Random rnd1 = new Random();
                    int nbSearch = rnd1.Next(14, 21);
                    if (count > nbSearch)
                    {
                        Random rnd = new Random();
                        int timeWaiting = rnd.Next(60000, 90000);
                        Thread.Sleep(timeWaiting);
                        count = 0;
                        nbOfTry++;
                    }
                    else
                    {
                        count++;
                    }
                    RetryBuyPlayer(p);

                }
            }
        }

        public void BuyPlayer(Player p)
        {
            IWebElement offreButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Achat immédiat')]")));
            IWebElement confirmeoffreButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Ok')]")));
            Console.WriteLine("Tentative d'achat du joueur " + p.name);
            if (IsPlayerBuy(p))
            {
                SetPlayertoSellList(p);
            }
            else
            {
                RetryBuyPlayer(p);
            }
        }

        public void RetryBuyPlayer(Player p)
        {
            //GotoMarket();
            IWebElement offreButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//h1[contains(text(), 'Résultats')]/preceding::button")));
            IWebElement minimumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/button[contains(@class,'decrement-value')]")));
            IWebElement maximumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/button[contains(@class,'increment-value')]")));
            IWebElement inputminimumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/div/input")));

            Random random = new Random();
            int randomNumber = random.Next(0, 7);
            if (randomNumber == 0 || int.Parse(minimumPrice.Text == string.Empty ? "0" : minimumPrice.Text) + p.price * POURCENTAGE_VENTE / 100 > p.price)
            {
                inputminimumPrice.Clear();
                inputminimumPrice.Click();
                inputminimumPrice.SendKeys(Convert.ToInt32(p.price / 5).ToString());
            }
            else
            {
                randomNumber = random.Next(0, 4);
                if (randomNumber == 0)
                {
                    minimumPrice.Click();
                }
                else
                {
                    maximumPrice.Click();
                }
            }

            IWebElement searchButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Rechercher')]")));
            Console.WriteLine("Joueur raté... Nouvelle recherche");
            tryBuyPlayer(p);
        }

        public void SetPlayertoSellList(Player p)
        {
                IWebElement lister = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(),'Lister sur Marché')]")));
                lister.Click();
                setMinimumPriceSold(p);
                setMaximumPriceSold(p);
                IWebElement listerElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Lister élément')]")));
                listerElement.Click();
                Console.WriteLine("joueur mis sur la liste des transferts");
        }

        public void setMinimumPriceSold(Player p)
        {
          
                IWebElement minimumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix de départ')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input")));
                IWebElement maximumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix achat immédiat')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input")));
                Thread.Sleep(500);
                minimumPrice.Click();
                minimumPrice.Click();
                minimumPrice.SendKeys((p.price + p.price * POURCENTAGE_VENTE / 100).ToString());
                maximumPrice.Click();
            
        }

        public void setMaximumPriceSold(Player p)
        {
                IWebElement maximumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix achat immédiat')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input")));
                IWebElement minimumPrice = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix de départ')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input")));
                Thread.Sleep(500);
                maximumPrice.Click();
                maximumPrice.Click();
                maximumPrice.SendKeys((p.price + p.price * POURCENTAGE_VENTE / 100).ToString());
                minimumPrice.Click();
            
        }
    }
}
