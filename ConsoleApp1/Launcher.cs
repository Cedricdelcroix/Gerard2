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
    private static Double POURCENTAGE_VENTE = 10.00;
    private static int nbSearch = 20;
    private int timeWaiting = 60000;
    private static System.Timers.Timer aTimer;
    Player CurrentPlayer = new Player();
    static Object lockthis = new object();
    int nbOfTry = 0;
    public Launcher(IWebDriver b)
    {
        this.browser = b;
    }


    public void initPlayerList(int priceStart, int priceEnd)
    {


        //playerList.Add(new Player("Alex Oxlade-Chamberlain", 2800));
        //playerList.Add(new Player("Joshua King", 700));
        //playerList.Add(new Player("Doucouré", 1500));
        //playerList.Add(new Player("Lerma", 600));
        //playerList.Add(new Player("Masuaku", 1100));

        //playerList.Add(new Player("ashley young", 1500));
        //playerList.Add(new Player("andreas christensen", 1500));
        //playerList.Add(new Player("manuel akanji", 1100));
        //playerList.Add(new Player("bakambu", 800));
        //playerList.Add(new Player("Yannick kessié", 1100));
        //playerList.Add(new Player("keita baldé", 1300));
        //playerList.Add(new Player("michail antonio", 1500));
        //playerList.Add(new Player("andre gray", 1300));
        //playerList.Add(new Player("Wilson", 1500));
        //playerList.Add(new Player("Giroud", 2300));
        //playerList.Add(new Player("Nathan Aké", 1700));
        //playerList.Add(new Player("Smalling", 2500));
        //playerList.Add(new Player("Ndidi", 2400));
        //playerList.Add(new Player("yedlin", 3500));
        //playerList.Add(new Player("ben yedder", 7500));
        //playerList.Add(new Player("ousmane dembélé", 8000));

        browser.Navigate().GoToUrl("https://www.futbin.com/players?page=1&ps_price=" + priceStart + "-" + priceEnd + "&sort=likes&order=desc");
        var elemTable = browser.FindElement(By.XPath("//table[@id = 'repTb']/tbody"));

        List<IWebElement> listItem = new List<IWebElement>(elemTable.FindElements(By.TagName("tr")));
        foreach (var elem in listItem)
        {
            IWebElement nameElement = elem.FindElement(By.TagName("a"));
            IWebElement priceElement = elem.FindElement(By.CssSelector(".ps4_color"));
            if (nameElement.Text.Contains(" ") && !priceElement.Text.Equals("0"))
            {
                playerList.Add(new Player(nameElement.Text, convertPrice(priceElement.Text)));
            }
        }

    }

    public int convertPrice(string price)
    {
        if (string.Empty != price)
        {
            if (price.Contains("K"))
            {
                double tmp;
                double.TryParse(price.Replace("K", "").Replace(".",","), out tmp);

                return Convert.ToInt32(tmp * 1000);
            }
            else
            {
                return int.Parse(price.Replace("K", ""));
            }
        }
        return 0;
    }


    private void SetTimer()
    {
        // Create a timer with a two second interval.
        aTimer = new System.Timers.Timer(3600000);
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
        initPlayerList(0,5000);
        //SetTimer();
        browser.Navigate().GoToUrl("https://www.easports.com/fr/fifa/ultimate-team/web-app/");
        Console.WriteLine("End goto");
        //Login("delcroix.cedric62@gmail.com","Cedric62100");
        Thread.Sleep(15000);
        //ReListerTout()
        
        foreach (Player player in playerList)
        {
            nbOfTry = 0;
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

    public Boolean IsPlayerExist()
    {
        try
        {
            browser.FindElement(By.XPath("//ul[contains(@class,'playerResultsList')]/button"));
            Console.WriteLine("Le joueur existe");
            return true;
        }
        catch(NoSuchElementException)
        {
            Console.WriteLine("Le joueur n'existe pas");
            return false;
        }
    }

    public void FindPlayer(Player p)
    {
        try
        {
            IWebElement reinitialiser = browser.FindElement(By.XPath("//button[contains(text(), 'Réinitialiser')]"));
            reinitialiser.Click();
            IWebElement playerInput = browser.FindElement(By.XPath("//input[@placeholder='Saisissez nom joueur']"));
            playerInput.Clear();
            Thread.Sleep(300);
            playerInput.SendKeys(p.name);
            Thread.Sleep(500);
            if (IsPlayerExist())
            {
                IWebElement playerselect = browser.FindElement(By.XPath("//ul[contains(@class,'playerResultsList')]/button"));
                playerselect.Click();
                SetPrice(p);
                Thread.Sleep(500);
                IWebElement searchButton = browser.FindElement(By.XPath("//button[contains(text(),'Rechercher')]"));
                searchButton.Click();
                Console.WriteLine("Joueur a rechercher saisie : " + p.name);
                tryBuyPlayer(p);
            }
        }catch(Exception e)
        {
            Console.WriteLine("Erreur dans la recherche de : " + p.name);
            FindPlayer(p);
        }

    }

    public void SetPrice(Player p)
    {
        try
        {
            Thread.Sleep(300);
            IWebElement element = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Max')]/parent::div/following-sibling::div/div/input"));
            element.Clear();
            Thread.Sleep(300);
            element.SendKeys(Convert.ToInt32(p.price / (1 + POURCENTAGE_VENTE / 100)).ToString());
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
            browser.FindElement(By.XPath("//div[contains(@class, 'pagingContainer')]/following-sibling::ul/li"));
            Console.WriteLine(p.name + " trouvé");
            return true;
        }
        catch (OpenQA.Selenium.NoSuchElementException)
        {
            Console.WriteLine(p.name + " non trouvé au prix de " + p.price);
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
            if(nbOfTry <= p.nbOfTry)
            {
                Random rnd1 = new Random();
                nbSearch = rnd1.Next(14, 21);
                if (count > nbSearch)
                {
                    Random rnd = new Random();
                    timeWaiting = rnd.Next(60000, 90000);
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

    public void RetryBuyPlayer(Player p)
    {
        //GotoMarket();
        try
        {
            IWebElement element = browser.FindElement(By.XPath("//h1[contains(text(), 'Résultats')]/preceding::button"));
            Thread.Sleep(300);
            element.Click();
            Thread.Sleep(300);
            IWebElement minimumPrice = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/button[contains(@class,'decrement-value')]"));
            IWebElement maximumPrice = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/button[contains(@class,'increment-value')]"));
            IWebElement inputminimumPrice = browser.FindElement(By.XPath("//h1[contains(text(), 'Prix achat immédiat')]/parent::div/following-sibling::div/div/span[contains(text(), 'Min')]/parent::div/following-sibling::div/div/input"));
            Random random = new Random();
            int randomNumber = random.Next(0, 7);
            if (randomNumber == 0 || int.Parse(minimumPrice.Text == string.Empty ? "0" : minimumPrice.Text)+p.price*POURCENTAGE_VENTE/100 > p.price)
            {
                inputminimumPrice.Clear();
                inputminimumPrice.Click();
                inputminimumPrice.SendKeys(Convert.ToInt32(p.price/5).ToString());
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
            Thread.Sleep(300);
            IWebElement searchButton = browser.FindElement(By.XPath("//button[contains(text(),'Rechercher')]"));
            searchButton.Click();
            tryBuyPlayer(p);
            Console.WriteLine("Joueur raté... Nouvelle recherche");
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
            Thread.Sleep(2000);
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
            IWebElement lister = browser.FindElement(By.XPath("//span[contains(text(),'Lister sur Marché')]"));
            lister.Click();
            Thread.Sleep(1000);
            setMinimumPriceSold(p);
            setMaximumPriceSold(p);
            IWebElement listerElement = browser.FindElement(By.XPath("//button[contains(text(),'Lister élément')]"));
            listerElement.Click();                
            Console.WriteLine("joueur mise sur la liste des transferts");
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
            Thread.Sleep(1000);
            IWebElement minimumPrice = browser.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix de départ')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input"));
            IWebElement maximumPrice = browser.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix achat immédiat')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input"));
            Thread.Sleep(1000);
            minimumPrice.Click();
            minimumPrice.Click();
            minimumPrice.SendKeys((p.price).ToString());
            maximumPrice.Click();
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            setMinimumPriceSold(p);
        }
    }

    public void setMaximumPriceSold(Player p)
    {
        try
        {
            Thread.Sleep(1000);
            IWebElement maximumPrice = browser.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix achat immédiat')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input"));
            IWebElement minimumPrice = browser.FindElement(By.XPath("//div[contains(@class,'panelAction')]/div/span[contains(text(),'Prix de départ')]/ancestor::div[contains(@class, 'panelActionRow')]/descendant::input"));
            Thread.Sleep(1000);
            maximumPrice.Click();
            maximumPrice.Click();
            maximumPrice.SendKeys((p.price).ToString());
            minimumPrice.Click();
        }
        catch(OpenQA.Selenium.WebDriverException)
        {
            setMaximumPriceSold(p);
        }
    }
}