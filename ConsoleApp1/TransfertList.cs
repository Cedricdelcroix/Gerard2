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
    class TransfertList : Transfert
    {
        public static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        WebDriverWait wait;

        public TransfertList(IWebDriver webDriver) : base(webDriver)
        {
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
        }

        public List<Player> GetPlayersOnTransfertList()
        {
            GoToMarcketList();
            List<Player> players = new List<Player>();
            if(IsSomethingToList())
            {
                reListAll();
            }

            if(IsSomethingToErase())
            {
                eraseAll();
            }
            ICollection<IWebElement> elemTable = wait.Until<ICollection<IWebElement>>(d => d.FindElements(By.XPath("//ul[contains(@class,'itemList')]/li[contains(@class,'listFUTItem')]")));


            foreach (IWebElement elem in elemTable)
            {
                IWebElement nameElement = wait.Until<IWebElement>(d => d.FindElement(By.CssSelector(".name")));
                IWebElement priceElement = wait.Until<IWebElement>(d => d.FindElement(By.XPath("//span[contains(text(),'Achat imm')]/following-sibling::span")));
                if (!priceElement.Text.Equals("0"))
                {
                    players.Add(new Player(nameElement.Text, Utils.convertPrice(priceElement.Text)));
                }
            }
            return players;

        }

        public void reListAll()
        {
            IWebElement reList = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'Re-lister tout')]")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains (@id,'ClickShield')]")));
            reList.Click();
            IWebElement yesButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'Oui')]")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains (@id,'ClickShield')]")));

            yesButton.Click();
        }

        public void eraseAll()
        {
            IWebElement listerElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Effacer')]")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains (@id,'ClickShield')]")));
            listerElement.Click();
        }

        public void GoToMarcketList()
        {
            GoToTransfert();
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//h1[text() = 'Liste de transferts']")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[contains (@id,'ClickShield')]")));
            element.Click();
            _log.Info("Enter On transfer list");
        }

        public bool IsSomethingToErase()
        {
            try
            {
                Thread.Sleep(300);
                IWebElement reList = driver.FindElement(By.XPath("//button[contains(text(), 'Effacer') and not(contains(@style, 'display: none'))]"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsSomethingToList()
        {
            try
            {
                Thread.Sleep(300);
                IWebElement reList = driver.FindElement(By.XPath("//button[contains(text(), 'Re-lister tout') and not(contains(@style, 'display: none'))]"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
