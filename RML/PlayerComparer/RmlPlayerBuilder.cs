using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RML.PlayerComparer
{
    public class RmlPlayerBuilder
    {
        private readonly ChromeDriver _driver;
        private readonly int _year;

        public RmlPlayerBuilder(ChromeDriver driver, int year)
        {
            _driver = driver;
            _year = year;
        }

        public List<RmlPlayer> BuildRmlPlayers()
        {
            var playerTypes = new List<string>
            {
                "CB",
                "DL"
            };

            var rmlPlayers = new List<RmlPlayer>();
            foreach (var playerType in playerTypes)
            {
                _driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&teamId=8&seasonId={_year}");
                var opLink = _driver.FindElement(By.XPath($"//ul[@class='filterToolsOptionSet']/li/a[contains(.,'{playerType}')]"));
                opLink.Click();

                System.Threading.Thread.Sleep(2000);
                var nextLink = _driver.FindElements(By.XPath("//div[@class='paginationNav']/a[contains(., 'NEXT')]"));

                while (nextLink.Count == 1)
                {
                    nextLink = _driver.FindElements(By.XPath("//div[@class='paginationNav']/a[contains(., 'NEXT')]"));
                    var rmlPlayerRows = _driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]"));

                    foreach (var rmlPlayerRow in rmlPlayerRows)
                    {
                        var rmlPlayer = new RmlPlayer();
                        //TODO: Need to check if the first Position is the one we are looking for (i.e. S, CB => CB)
                        //Chandler Jones, Ari LB, DE, EDR
                        try
                        {
                            rmlPlayer.Team = rmlPlayerRow.FindElement(By.XPath("./td[@class='playertablePlayerName']")).Text.Split(new string[] { ", " }, StringSplitOptions.None)[1].Split(' ')[0];
                            rmlPlayer.Name = rmlPlayerRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                            rmlPlayer.PreviousRank = int.Parse(rmlPlayerRow.FindElement(By.XPath("./td[@class='playertableData'][1]")).Text);
                            rmlPlayer.PreviousPoints = decimal.Parse(rmlPlayerRow.FindElement(By.XPath("./td[contains(@class,'playertableStat')][1]")).Text);
                            rmlPlayer.PreviousAverage = decimal.Parse(rmlPlayerRow.FindElement(By.XPath("./td[contains(@class,'playertableStat')][2]")).Text);
                        }
                        catch
                        {
                            System.Threading.Thread.Sleep(30000);
                            rmlPlayer.Team = rmlPlayerRow.FindElement(By.XPath("./td[@class='playertablePlayerName']")).Text.Split(new string[] { ", " }, StringSplitOptions.None)[1].Split(' ')[0];
                            rmlPlayer.Name = rmlPlayerRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                            rmlPlayer.PreviousRank = int.Parse(rmlPlayerRow.FindElement(By.XPath("./td[@class='playertableData'][1]")).Text);
                            rmlPlayer.PreviousPoints = decimal.Parse(rmlPlayerRow.FindElement(By.XPath("./td[contains(@class,'playertableStat')][1]")).Text);
                            rmlPlayer.PreviousAverage = decimal.Parse(rmlPlayerRow.FindElement(By.XPath("./td[contains(@class,'playertableStat')][2]")).Text);
                        }

                        rmlPlayers.Add(rmlPlayer);
                    }

                    if (nextLink.Count == 1)
                    {
                        nextLink[0].Click();
                        System.Threading.Thread.Sleep(2000);
                    }
                }
            }

            return rmlPlayers;
        }
    }
}
