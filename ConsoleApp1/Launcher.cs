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
        //IWebElement header = browser.FindElement(By.Id("site - header"));
        // Login("delcroix.cedric62@gmail.com","Cedric62100");
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
        playerList.Add(new Player("", 0));
        playerList.Add(new Player("", 0));
        playerList.Add(new Player("", 0));
        playerList.Add(new Player("", 0));
    }

    public void GotoMarket()
    {
        Thread.Sleep(10000);
        IWebElement transferButton = browser.FindElement(By.XPath("//button[text() = 'Transferts']"));
        transferButton.Click();
        Thread.Sleep(500);
        IWebElement marketPlace = browser.FindElement(By.ClassName("ut-tile-transfer-market"));
        Thread.Sleep(500);
        marketPlace.Click();
        Console.WriteLine("On market");
    }

    public void FindPlayer(Player p)
    {
        IWebElement playerInput = browser.FindElement(By.XPath("//input[@placeholder='Saisissez nom joueur']"));
        playerInput.SendKeys(p.name);
        SetPrice(p);
        tryBuyPlayer(p);
    }

    public void SetPrice(Player p)
    {
        IWebElement achatImmediat = browser.FindElement(By.XPath("//h1[text() ='Prix achat immédiat :']"));
        IWebElement Max = achatImmediat.FindElement(By.XPath("//following-sibling::span[text() = 'Max']"));
        IWebElement inputPrice = Max.FindElement(By.XPath("/following-sibling::input[@placeholder='Tout']"));
        
    }

    public bool IsPlayerFind(Player p)
    {
       
        // si le joueur est trouver essayer de l'acheter 
        try
        {
            //browser.FindElement(By.CssSelector).isDisplayed();
            Console.WriteLine(p.name + " find");
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
            BuyPlayer();
        }
        else
        {
            if (count < 15)
            Thread.Sleep(60000);
            tryBuyPlayer(p);
        }
    }

    public void BuyPlayer()
    {
        // click on acheter 

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
