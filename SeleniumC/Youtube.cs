using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumC
{
    public class Youtube
    {
        IWebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
        }
        [Test]
        public void ElementGoldSound()
        {
            driver.Navigate().GoToUrl("https://www.youtube.com/");
            IWebElement ButtonLanguage = driver.FindElement(By.XPath(".//div[@id='button']//button[@id='button']//yt-icon"));
            ButtonLanguage.Click();
            IWebElement ChangeLanguage = driver.FindElement(By.XPath(".//a[@id='endpoint']//div[@id='primary-text-container']//yt-formatted-string[@id='label' and text()='Language:']"));
            ChangeLanguage.Click();
            IWebElement ThaiLanguage = driver.FindElement(By.XPath(".//a[@id='endpoint']//div[@id='primary-text-container']//yt-formatted-string[@id='label' and text()='ภาษาไทย']"));
            ThaiLanguage.Click();
            IWebElement inputSearch = driver.FindElement(By.XPath(".//input[@id='search']"));
            inputSearch.SendKeys("ธาตุทองซาว");
            inputSearch.SendKeys(Keys.Enter);

            var videoTitles = driver.FindElements(By.XPath(".//a[@id='video-title']"));
            // จัดเก็บ title และ href ไปยัง list (Array)
            List<(string Title, string Href)> videoDetails = new List<(string Title, string Href)>();

            //Loop 0-5 เพื่อเก็บ index - title - href
            for (int i = 0; i < videoTitles.Count && i < 5; i++)
            {
                var videoTitle = videoTitles[i];
                string href = videoTitle.GetAttribute("href");

                videoDetails.Add((videoTitle.Text, href));
                Console.WriteLine($"Video at index {i} Title: {videoTitle.Text} Href: {href}");
            }
            // เก็บตัวแปร ไว้ Verify text ในหน้าถัดไป
            string firstVideoTitle = videoTitles[0].Text;
            Console.WriteLine(firstVideoTitle);
            videoTitles[0].Click();




            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement videoPlayTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='title']//h1//yt-formatted-string")));
            Console.WriteLine("Video title: " + videoPlayTitle.Text);



            Console.WriteLine($"Video แรกที่คลิก : {firstVideoTitle}");
            Console.WriteLine($"Video ที่กำลังชมขณะนี้ : {videoPlayTitle.Text}");

            if (firstVideoTitle.Equals(videoPlayTitle.Text))
            {
                Console.WriteLine("Video ตรงกันครับ");
            }
            else
            {
                Console.WriteLine("Video ไม่ตรงกันครับ");
            }




        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}