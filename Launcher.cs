using System;

public class Launcher
{
	public Launcher()
	{
        
    }

    public void run()
    {
        initPlayerList();
        IWebDriver browser = new ChromeDriver();
        //Firefox's proxy driver executable is in a folder already
        //  on the host system's PATH environment variable.
        browser.Navigate().GoToUrl("http://saucelabs.com");
        Console.WriteLine("End goto");
        IWebElement header = browser.FindElement(By.Id("site - header"));
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
    }

    public void FindPlayer()
    {
        IWebElement header = browser.FindElement(By.Id("site - header"));
    }

    public bool IsPlayerFind()
    {
        return true;
    }

    public bool tryBuyPlayer()
    {
        return true;
    }

    public void SetPlayertoSellList()
    {

    }
}
}
