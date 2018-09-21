using ConsoleApp1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Launcher
{
    public static List<Player> playerList = new List<Player>();
    IWebDriver browser;
    int count = 0;

    public Launcher(IWebDriver b)
    {
        this.browser = b;
    }

    public void run()
    {
        initPlayerList();

        browser.Navigate().GoToUrl("https://www.easports.com/fr/fifa/ultimate-team/web-app/");
        Console.WriteLine("End goto");
        //Login("delcroix.cedric62@gmail.com","Cedric62100");
        Thread.Sleep(13000);
        foreach (Player player in playerList)
        {
            GotoMarket();
            FindPlayer(player);
        }
    }

    public void Login(string mail, string pass)
    {
        Thread.Sleep(10000);
        IWebElement connexion = browser.FindElement(By.XPath("//button[text() = 'Connexion']"));
        connexion.Click();
        IWebElement email = browser.FindElement(By.XPath("//input[@type='email']"));
        email.SendKeys(mail);
        IWebElement passwd = browser.FindElement(By.XPath("//input[@type='password']"));
        passwd.SendKeys(pass);
        IWebElement btnLogin = browser.FindElement(By.Id("btnLogin"));
        btnLogin.Click();
    }

    public void initPlayerList()
    {
        //playerList.Add(new Player("Alex Oxlade-Chamberlain", 2800));
        playerList.Add(new Player("lemina", 500));
        //playerList.Add(new Player("Williams", 4700));
        /*playerList.Add(new Player("", 0));
        playerList.Add(new Player("", 0));*/
    }

    public void GotoMarket()
    {
        Thread.Sleep(300);
        IWebElement transferButton = browser.FindElement(By.XPath("//button[text() = 'Transferts']"));
        transferButton.Click();
        Thread.Sleep(300);
        IWebElement marketPlace = browser.FindElement(By.ClassName("ut-tile-transfer-market"));
        Thread.Sleep(300);
        marketPlace.Click();
        Console.WriteLine("On market");
    }

    public void FindPlayer(Player p)
    {
        IWebElement playerInput = browser.FindElement(By.XPath("//input[@placeholder='Saisissez nom joueur']"));
        playerInput.Clear();
        Thread.Sleep(300);
        playerInput.SendKeys(p.name);
        Thread.Sleep(500);
        IWebElement playerselect = browser.FindElement(By.XPath("//ul[contains(@class,'playerResultsList')]/button"));
        playerselect.Click();
        SetPrice(p);
        Thread.Sleep(500);
        IWebElement searchButton = browser.FindElement(By.XPath("//button[contains(text(),'Rechercher')]"));
        searchButton.Click();
        tryBuyPlayer(p);
    }

    public void SetPrice(Player p)
    {
        Thread.Sleep(300);
        IWebElement element = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Max')]/parent::div/following-sibling::div/div/input"));
        element.Clear();
        Thread.Sleep(300);
        element.SendKeys(p.price.ToString());
    }

    public bool IsPlayerFind(Player p)
    {

        // si le joueur est trouver essayer de l'acheter 
        try
        {
            Thread.Sleep(500);
            browser.FindElement(By.XPath("//span[contains(@class,'no-results-icon')]"));
            return false;
        }
        catch (OpenQA.Selenium.NoSuchElementException)
        {
            Console.WriteLine(p.name + " find");
            return true;
        }
    }

    public void tryBuyPlayer(Player p)
    {
        if (IsPlayerFind(p))
        {
            // Buy Player
            BuyPlayer();
        }
        else
        {
            if (count > 13)
            {
                Thread.Sleep(60000);
                count = 0;
            }
            else
            {
                count++;
            }
            RetryBuyPlayer(p);
        }
    }

    public void RetryBuyPlayer(Player p)
    {
        //GotoMarket();
        IWebElement element = browser.FindElement(By.XPath("//button[contains(@class,'btn-navigation')][1]"));
        element.Click();
        Thread.Sleep(300);
        IWebElement searchButton = browser.FindElement(By.XPath("//button[contains(text(),'Rechercher')]"));
        searchButton.Click();
        tryBuyPlayer(p);
    }

    public void BuyPlayer()
    {
        // click on acheter 
        IWebElement offreButton = browser.FindElement(By.XPath("//button[contains(text(),'Achat immédiat')]"));
        offreButton.Click();
        Thread.Sleep(300);
        IWebElement confirmeoffreButton = browser.FindElement(By.XPath("//button[contains(text(),'Ok')]"));
        confirmeoffreButton.Click();
        Console.WriteLine("Player buy");
    }

    public bool IsPlayerBuy(Player p)
    {
        return true;
    }

    public void SetPlayertoSellList(Player p)
    {
        if (IsPlayerBuy(p))
        {

        }
    }
}