using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Launcher launcher = new Launcher(new ChromeDriver());
            launcher.run();
        }
    }
}
