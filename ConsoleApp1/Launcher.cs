using ConsoleApp1;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

public class Launcher
{
    public static List<Player> playerList = new List<Player>();
    IWebDriver browser;
    int count = 0;
    private static Double POURCENTAGE_VENTE = 12.00;
    private static int NB_SEARCH = 2;
    private static int TIME_WAITING = 3000;
    private static System.Timers.Timer aTimer;
    Player CurrentPlayer = new Player();
    static Object lockthis = new object();
    int nbOfTry = 0;
    public Launcher(IWebDriver b)
    {
        this.browser = b;
    }


    public void initPlayerList()
    {
        //playerList.Add(new Player("Alex Oxlade-Chamberlain", 2800));
        playerList.Add(new Player("rybus", 4000,1,2));
        playerList.Add(new Player("Alex Teixeira", 700));
        /*playerList.Add(new Player("", 0));
        playerList.Add(new Player("", 0));*/
    }


    private void SetTimer()
    {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(40000);
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        lock (lockthis)
        {
            Console.WriteLine("Enter on Timer");
            ReListerTout();
            GotoMarket();
        }
    }

    public void run()
    {
        initPlayerList();
        SetTimer();
        browser.Navigate().GoToUrl("https://www.easports.com/fr/fifa/ultimate-team/web-app/");
        Console.WriteLine("End goto");
        //Login("delcroix.cedric62@gmail.com","Cedric62100");
        Thread.Sleep(15000);
        //ReListerTout()
        
        foreach (Player player in playerList)
        {
            lock (lockthis)
            {
                GotoMarket();
                CurrentPlayer = player;
                FindPlayer(player);
            }
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

    public void GotoMarket()
    {
        try
        {
            Thread.Sleep(300);
            IWebElement transferButton = browser.FindElement(By.XPath("//button[text() = 'Transferts']"));
            transferButton.Click();
            Thread.Sleep(300);
            IWebElement marketPlace = browser.FindElement(By.ClassName("ut-tile-transfer-market"));
            Thread.Sleep(300);
            marketPlace.Click();
            Console.WriteLine("Enter on market");
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            GotoMarket();
        }
    }

    public void FindPlayer(Player p)
    {
        try
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
            Console.WriteLine("Joueur a rechercher saisie : " + p.name);
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            Console.WriteLine("élément non trouver dans la saisie de recherche de : " + p.name);
            FindPlayer(p);
        }
        tryBuyPlayer(p);
        
    }

    public void SetPrice(Player p)
    {
        try
        {
            Thread.Sleep(300);
            IWebElement element = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Max')]/parent::div/following-sibling::div/div/input"));
            element.Clear();
            Thread.Sleep(300);
            element.SendKeys(p.price.ToString());
            Console.WriteLine("recherche de : " + p.name + " au prix de " + p.price);
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            Console.WriteLine("élément non trouver dans la saisie du prix de : " + p.name);
            FindPlayer(p);
        }
    }

    public bool IsPlayerFind(Player p)
    {
        // si le joueur est trouver essayer de l'acheter 
        try
        {
            Thread.Sleep(500);
            browser.FindElement(By.XPath("//span[contains(@class,'no-results-icon')]"));
            Console.WriteLine(p.name + " non trouvé au prix de " + p.price);
            return false;
        }
        catch (OpenQA.Selenium.NoSuchElementException)
        {
            Console.WriteLine(p.name + " trouvé");
            return true;
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
            if(nbOfTry < p.nbOfTry)
            {
                if (count > NB_SEARCH)
                {
                    Thread.Sleep(TIME_WAITING);
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

    public void RetryBuyPlayer(Player p)
    {
        //GotoMarket();
        try
        {

            IWebElement element = browser.FindElement(By.XPath("//button[contains(@class,'btn-navigation')][1]"));
            Thread.Sleep(300);
            element.Click();
            Thread.Sleep(300);
            IWebElement minimumPrice = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/button[contains(@class,'decrement-value')]"));
            IWebElement maximumPrice = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/button[contains(@class,'increment-value')]"));
            Random random = new Random();
            int randomNumber = random.Next(0, 2);
            if (randomNumber == 0)
            {
                minimumPrice.Click();
            }
            else
            {
                maximumPrice.Click();
            }
            Thread.Sleep(300);
            IWebElement searchButton = browser.FindElement(By.XPath("//button[contains(text(),'Rechercher')]"));
            searchButton.Click();
            tryBuyPlayer(p);
            Console.WriteLine("Joueur non trouvé... Nouvelle recherche");
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            RetryBuyPlayer(p);
        }
    }

    public void BuyPlayer(Player p)
    {
        // click on acheter 
        try
        {
            IWebElement offreButton = browser.FindElement(By.XPath("//button[contains(text(),'Achat immédiat')]"));
            Thread.Sleep(100);
            offreButton.Click();
            Thread.Sleep(300);
            IWebElement confirmeoffreButton = browser.FindElement(By.XPath("//button[contains(text(),'Ok')]"));
            confirmeoffreButton.Click();
            Console.WriteLine("Achat du joueur " + p.name);
            SetPlayertoSellList(p);
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            RetryBuyPlayer(p);
            Console.WriteLine("Erreur lors de l'achat du joueur");
        }
    }

    public bool IsSomethingToList()
    {
        try
        {
            Thread.Sleep(300);
            IWebElement reList = browser.FindElement(By.XPath("//button[contains(text(), 'Re-lister tout') and not(contains(@style, 'display: none'))]"));
            return true;
        }
        catch(NoSuchElementException)
        {
            return false;
        }
    }

    public void ReListerTout()
    {
        try
        {
            Thread.Sleep(300);
            IWebElement transferButton = browser.FindElement(By.XPath("//button[text() = 'Transferts']"));
            transferButton.Click();
            Thread.Sleep(300);
            IWebElement transferListButton = browser.FindElement(By.XPath("//h1[text() = 'Liste de transferts']"));
            transferListButton.Click();
            Thread.Sleep(300);

            if (IsSomethingToList())
            {
                IWebElement reList = browser.FindElement(By.XPath("//button[contains(text(), 'Re-lister tout')]"));
                reList.Click();
                Thread.Sleep(300);
                IWebElement yesButton = browser.FindElement(By.XPath("//button[contains(text(), 'Oui')]"));
                yesButton.Click();
                Thread.Sleep(300);
                Console.WriteLine("Tous les joueurs ont été relisté");
            }
            else
            {
                Console.WriteLine("Rien à relisté");
            }
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            Console.WriteLine("Erreur lors du relistage de tout les joueurs");
            ReListerTout();
        }
    }

    public bool IsPlayerBuy(Player p)
    {
        try
        {
            Thread.Sleep(500);
            browser.FindElement(By.XPath("//span[contains(text(),'Bravo')]"));
            p.numberOfItem++;
            return true;
        }
        catch(OpenQA.Selenium.NoSuchElementException)
        {
            return false;
        }
    }

    public void SetPlayertoSellList(Player p)
    {
        try
        {
            if (IsPlayerBuy(p))
            {
                IWebElement lister = browser.FindElement(By.XPath("//span[contains(text(),'Lister sur Marché')]"));
                lister.Click();
                Thread.Sleep(500);
                setMinimumPriceSold(p);
                setMaximumPriceSold(p);
                IWebElement listerElement = browser.FindElement(By.XPath("//span[contains(text(),'Lister élément')]"));
                lister.Click();
            }
            else
            {
                RetryBuyPlayer(p);
            }
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            SetPlayertoSellList(p);
        }
    }

    public void setMinimumPriceSold(Player p)
    {
        try
        {
            Thread.Sleep(500);
            IWebElement minimumPrice = browser.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix de départ')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input"));
            Thread.Sleep(100);
            minimumPrice.Clear();
            Thread.Sleep(100);
            minimumPrice.SendKeys(((p.price + p.price * POURCENTAGE_VENTE/100) - 100).ToString());
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            SetPlayertoSellList(p);
        }
    }

    public void setMaximumPriceSold(Player p)
    {
        try
        {
            Thread.Sleep(500);
            IWebElement maximumPrice = browser.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix achat immédiat')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input"));
            Thread.Sleep(100);
            maximumPrice.Clear();
            Thread.Sleep(100);
            maximumPrice.SendKeys((p.price + p.price * POURCENTAGE_VENTE/100).ToString());
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            SetPlayertoSellList(p);
        }
    }
}