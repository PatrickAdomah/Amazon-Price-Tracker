using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Net;
using System.Net.Mail;

namespace Amazon_Price_Tracker
{
    internal class Program
    {
        ///go on amazon and find the inital price
        ///display price and save to text file
        ///then after a set amount of time check the price again
        ///output message if price is gone up or down

        private static void Main(string[] args)
        {
            string url;
            string email;
            double target_price;

            Console.WriteLine("Amazon price tracker By Patrick Adomah");

            Console.Write("Enter amazon.co.uk url: ");
            url = Console.ReadLine();
            Console.Write("What is your target price: ");
            target_price = double.Parse(Console.ReadLine());

            //Console.Write("What is your email address: ");
            //email = Console.ReadLine();

            bool targetMet = findPrice(url, target_price);
            if (targetMet)
            {
                Console.WriteLine("The product is equal or bellow the target price");
                email_notif();
            }
        }

        static bool findPrice(string url, double target_price)
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl(url);
                try
                {
                    IWebElement product_name = wait.Until(ExpectedConditions.ElementExists(By.Id("productTitle")));
                    IWebElement inital_price = wait.Until(ExpectedConditions.ElementExists(By.Id("priceblock_ourprice")));
                    Console.WriteLine("Name: {0} Price: {1}", product_name.GetAttribute("textContent"), inital_price.GetAttribute("textContent"));

                    while (double.Parse(inital_price.GetAttribute("textContent").Substring(1)) >= target_price)
                    {
                        product_name = wait.Until(ExpectedConditions.ElementExists(By.Id("productTitle")));
                        IWebElement current_price = wait.Until(ExpectedConditions.ElementExists(By.Id("priceblock_ourprice")));
                        Console.WriteLine("Name: {0} Price: {1}", product_name.GetAttribute("textContent"), current_price.GetAttribute("textContent"));
                        System.Threading.Thread.Sleep(120000);
                    }

                    return true;
                }
                catch
                {
                    try
                    {
                        IWebElement product_name = wait.Until(ExpectedConditions.ElementExists(By.Id("productTitle")));
                        IWebElement inital_price = wait.Until(ExpectedConditions.ElementExists(By.Id("priceblock_saleprice")));
                        Console.WriteLine("Name: {0} Price: {1}", product_name.GetAttribute("textContent"), inital_price.GetAttribute("textContent"));

                        while (double.Parse(inital_price.GetAttribute("textContent").Substring(1)) >= target_price)
                        {
                            product_name = wait.Until(ExpectedConditions.ElementExists(By.Id("productTitle")));
                            IWebElement current_price = wait.Until(ExpectedConditions.ElementExists(By.Id("priceblock_saleprice")));
                            Console.WriteLine("Name: {0} Price: {1}", product_name.GetAttribute("textContent"), current_price.GetAttribute("textContent"));
                            System.Threading.Thread.Sleep(120000);
                        }

                        return true;
                    }
                    catch
                    {
                        Console.WriteLine("Invaild link");
                    }
                    return false;
                }
            }
        }

        static void email_notif()
        {

        }

    }
}