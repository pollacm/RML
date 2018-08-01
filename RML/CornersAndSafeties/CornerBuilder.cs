using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RML.CornersAndSafeties
{
    public class CornerBuilder
    {
        private readonly ChromeDriver _driver;
        private readonly int _year;

        public CornerBuilder(ChromeDriver driver, int year)
        {
            _driver = driver;
            _year = year;
        }

        public List<Corner> BuildCorners()
        {
            _driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&teamId=8&seasonId=2018#&seasonId={_year}");
            var opLink = _driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'CB')]"));
            opLink.Click();

            System.Threading.Thread.Sleep(2000);
            var corners = new List<Corner>();
            var nextLink = _driver.FindElements(By.XPath("//div[@class='paginationNav']/a[contains(., 'NEXT')]"));

            while (nextLink.Count == 1)
            {
                nextLink = _driver.FindElements(By.XPath("//div[@class='paginationNav']/a[contains(., 'NEXT')]"));
                var cornerRows = _driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]"));

                foreach (var cornerRow in cornerRows)
                {
                    var corner = new Corner();

                    corner.Team = cornerRow.FindElement(By.XPath("./td[@class='playertablePlayerName']")).Text.Split(new string[] { ", " }, StringSplitOptions.None)[1].Split(' ')[0];
                    corner.Name = cornerRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                    corner.PreviousRank = int.Parse(cornerRow.FindElement(By.XPath("./td[@class='playertableData'][1]")).Text);
                    corner.PreviousPoints = decimal.Parse(cornerRow.FindElement(By.XPath("./td[contains(@class,'playertableStat')][1]")).Text);
                    corner.PreviousAverage = decimal.Parse(cornerRow.FindElement(By.XPath("./td[contains(@class,'playertableStat')][2]")).Text);

                    corners.Add(corner);
                }

                if (nextLink.Count == 1)
                {
                    nextLink[0].Click();
                    System.Threading.Thread.Sleep(2000);
                }
            }

            return corners;
        }
    }
}
