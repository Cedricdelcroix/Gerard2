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
        
        browser.Navigate().GoToUrl("http://saucelabs.com");
        Console.WriteLine("End goto");
        IWebElement header = browser.FindElement(By.Id("site - header"));

        foreach (Player player in playerList)
        {
            GotoMarket();
            FindPlayer(player);
        }
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
        IWebElement header = browser.FindElement(By.Id("site - header"));
        Console.WriteLine("On market");
    }

    public void FindPlayer(Player p)
    {
        IWebElement header = browser.FindElement(By.Id("site - header"));
        tryBuyPlayer(p);
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
