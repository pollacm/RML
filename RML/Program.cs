using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RML.PowerRankings;

namespace RML
{
    class Program
    {
        private static string year = "2017";
        private static int week = 4;
        static void Main(string[] args)
        {
            var driver = new ChromeDriver();

            //login

            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/standings?leagueId=127291&seasonId={year}");
            driver.SwitchTo().Frame("disneyid-iframe");
            var userNameBox = driver.FindElement(By.CssSelector("div.field-username-email input"));
            userNameBox.SendKeys(Keys.ArrowDown);
            userNameBox.SendKeys("pollacm@gmail.com");
            
            var passwordBox = driver.FindElement(By.CssSelector("div.field-password input"));
            passwordBox.SendKeys(Keys.ArrowDown);
            passwordBox.SendKeys("grip1334");
            passwordBox.SendKeys(Keys.Enter);
            System.Threading.Thread.Sleep(2000);
            //var submitButton = driver.FindElement(By.XPath("//button[contains(., 'Log In')]"));
            //submitButton.Click();



            //get teams
            //var teamAnchors = driver.FindElements(By.CssSelector("div.games-fullcol table:nth-child(1) a"));

            //var teams = new List<string>();
            //foreach (var teamAnchor in teamAnchors)
            //{
            //    teams.Add(teamAnchor.Text);
            //}

            //driver.WaitUntilElementExists(By.CssSelector("table.tableBody"));

            //OP
            //driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&seasonId={year}");
            //driver.WaitUntilElementExists(By.Id("playerTableContainerDiv"));

            //var opLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'OP')]"));
            //opLink.Click();

            //System.Threading.Thread.Sleep(2000);

            //var lastLink = driver.FindElement(By.XPath("//tr[contains(@class, 'playerTableBgRowSubhead')]/td/a[contains(.,'LAST')]"));
            //lastLink.Click();

            //System.Threading.Thread.Sleep(5000);

            //TODO: Need to account for more than one
            //var opRow = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr")).First();
            //var opPlayerName = opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
            //var opPlayerTeam = opRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
            //var opPlayerTeamAbbreviation = opRow.FindElement(By.XPath("./td[3]/a")).Text;
            //var opPlayerId = opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid");
            //var opPlayerPoints = opRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text;

            //DP
            //var dpLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'DP')]"));
            //dpLink.Click();

            //System.Threading.Thread.Sleep(2000);

            //var dpRow = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr")).First();
            //var dpPlayerName = dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
            //var dpPlayerTeam = dpRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
            //var dpPlayerTeamAbbreviation = dpRow.FindElement(By.XPath("./td[3]/a")).Text;
            //var dpPlayerId = dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid");
            //var dpPlayerPoints = dpRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text;

            //TODO: Power Rankings
            GetPowerRankings(driver);
            //TODO: Assign Trophies
            AssignTrophies();
            var x = 1;
        }

        private static void AssignTrophies()
        {
            throw new System.NotImplementedException();
        }

        private static void GetPowerRankings(ChromeDriver driver)
        {
            var currentWeek = week;
            List<Score>[] scoresForPowerRankings = new List<Score>[4];
            //TODO: Get previous power ranking
            for (int i = 3; i >= 0; i--)
            {
                scoresForPowerRankings[i] = GetScores(currentWeek - i, driver);
            }

            var powerRankingGenerator = new PowerRankingGenerator(scoresForPowerRankings, currentWeek);

            var x = 1;
        }

        private static List<Score> GetScores(int currentWeek, ChromeDriver driver)
        {
            var scores = new List<Score>();
            
            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/leagueoffice?leagueId=127291&seasonId={year}");
            driver.WaitUntilElementExists(By.ClassName("games-nav"));

            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/scoreboard?leagueId=127291&matchupPeriodId={currentWeek}");
            driver.WaitUntilElementExists(By.ClassName("ptsBased"));

            var matchups = driver.FindElements(By.XPath("//table[@class='ptsBased matchup']"));
            foreach (var matchup in matchups)
            {
                scores.Add(BuildMatchup(matchup));
            }
            
            return scores;
        }

        private static Score BuildMatchup(IWebElement matchupElement)
        {
            var score = new Score();
            var teams = matchupElement.FindElements(By.XPath("./tbody//tr"));
            var awayTeam = true;
            foreach (var team in teams)
            {
                if (awayTeam) //new score; build away
                {
                    score = new Score();
                    score.AwayTeam = BuildScore(team);
                    awayTeam = false;
                }
                else //build home team
                {
                    score.HomeTeam = BuildScore(team);
                    break;
                }
            }

            return score;
        }

        private static Team BuildScore(IWebElement teamElement)
        {
            var team = new Team();

            team.TeamName = teamElement.FindElement(By.XPath("./td[@class='team']/div[@class='name']/a")).Text;
            team.TeamAbbreviation = teamElement.FindElement(By.XPath("./td[@class='team']/div[@class='name']/span")).Text.Replace("(","").Replace(")","");
            team.TeamPoints = decimal.Parse(teamElement.FindElement(By.XPath("./td[contains(@class, 'score')]")).Text);
            team.Win = teamElement.FindElements(By.XPath("./td[2][contains(@class, 'winning')]")).Any();

            return team;
        }
    }


}
