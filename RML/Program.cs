using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RML.CornersAndSafeties;
using RML.PowerRankings;
using RML.Returners;

namespace RML
{
    class Program
    {
        private static int year = 2017;
        private static int week = 10;
        static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");
            var driver = new ChromeDriver(options);

            //var returners = new Returners.Returners(driver);
            //returners.GenerateReturners();

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

            //var cornerBuilder = new CornerBuilder(driver, year);
            //var corners = cornerBuilder.BuildCorners();
            //TODO: Need to validate this result with ESPN/YAHOO to get a difference and put that into the report.

            //get teams
            var teamAnchors = driver.FindElements(By.CssSelector("div.games-fullcol table:nth-child(1) a"));

            //var teams = new List<string>();
            //foreach (var teamAnchor in teamAnchors)
            //{
            //    teams.Add(teamAnchor.Text);
            //}

            driver.WaitUntilElementExists(By.CssSelector("table.tableBody"));

            //OP
            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&seasonId={year}");
            driver.WaitUntilElementExists(By.Id("playerTableContainerDiv"));

            var opLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'OP')]"));
            opLink.Click();

            System.Threading.Thread.Sleep(2000);

            var lastLink = driver.FindElement(By.XPath("//tr[contains(@class, 'playerTableBgRowSubhead')]/td/a[contains(.,'LAST')]"));
            lastLink.Click();

            System.Threading.Thread.Sleep(5000);

            //TODO: Need to account for more than one
            var opRow = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr")).First();
            var opOfTheWeek = new PlayerOfTheWeek();
            opOfTheWeek.Name = opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
            opOfTheWeek.Team = opRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
            opOfTheWeek.TeamAbbreviation = opRow.FindElement(By.XPath("./td[3]/a")).Text;
            opOfTheWeek.PlayerId = Int32.Parse(opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
            opOfTheWeek.Points = decimal.Parse(opRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);

            //DP
            var dpLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'DP')]"));
            dpLink.Click();

            System.Threading.Thread.Sleep(2000);

            var dpRow = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr")).First();
            var dpOfTheWeek = new PlayerOfTheWeek();
            dpOfTheWeek.Name = dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
            dpOfTheWeek.Team = dpRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
            dpOfTheWeek.TeamAbbreviation = dpRow.FindElement(By.XPath("./td[3]/a")).Text;
            dpOfTheWeek.PlayerId = Int32.Parse(dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
            dpOfTheWeek.Points = decimal.Parse(dpRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
            
            var powerRankings = GetPowerRankings(driver);
            var currentWeek = GetWeek(week, driver);

            var weeklyPayoutTeams = string.Empty;
            for(int i = 1; i <= week; i++)
            {
                var weekForWeeklyPayouts = GetWeek(i, driver);
                var teamsForWeeklyPayouts = weekForWeeklyPayouts.Scores.Select(s => s.AwayTeam).ToList();
                teamsForWeeklyPayouts.AddRange(weekForWeeklyPayouts.Scores.Select(s => s.HomeTeam));
                weeklyPayoutTeams += (i + ". " + teamsForWeeklyPayouts.OrderByDescending(t => t.TeamPoints).First().TeamName.ToUpper() + @"
                ");
            }

            CreateLeaguePage(powerRankings, weeklyPayoutTeams, opOfTheWeek, dpOfTheWeek, currentWeek, week);
            //TODO: Assign Trophies.. Need to prompt if it should happen
            AssignTrophies(currentWeek, driver);
            var x = 1;
        }

        private static void CreateLeaguePage(List<PowerRanking> powerRankings, string weeklyPayoutTeams, PlayerOfTheWeek opOfTheWeek, PlayerOfTheWeek dpOfTheWeek, Week currentWeek, int i)
        {
            var leagueMessage = @"[b]<update> IN WEEK " + week + @"[/b]!!!!!

[image]<update>[/image]

[b]R.M.L. WEEK " + week + @" - <update>[/b]

<update>

[b]WEEK " + week + @" RECAP[/b]

" + GetRecapInfo(currentWeek) + @"

  [b]OFFENSIVE PLAYER OF THE WEEK[/b]

  [b]" + BuildPlayerOfTheWeek(opOfTheWeek) + @"[/b]

[image]<update>[/image]



  [b]DEFENSIVE PLAYER OF THE WEEK[/b]

  [b]" + BuildPlayerOfTheWeek(dpOfTheWeek) + @"[/b]

[image]<update>[/image]



  [b]THE 600 CLUB[/b]
  [b]DOUBLE TROUBLE, TOO EASY[/b]


  [b]THE 500 CLUB[/b]
  [b]NO MERCY GOONZ, !ZO, BAMA BLACKOUT, WIDE RECEIVER SAMMIE[/b]


  [b]I'M All THE WAY UP! AWARD GOES TO...[/b]
  [b]WIDE RECEIVER SAMMIE[/b]


  [b]MEEK MILLZ!!!AWARD GOES TO...[/b]
  [b]NO FLEX ZONE[/b]


  [i][b]DISCLAIMER:[/b] For the [b]POWER RANKINGS[/b], I use an algorithm to calculate how likely you are to win against another team at any given time.It's not my personal opinion of the teams. The algorithm is basically a total of your points scored over the last 3 weeks, plus 50 points for each win over that same time period. [/i]
  
  [b]WEEK " + week + @" POWER RANKINGS[/b]

  " + GeneratePowerRankings(powerRankings) + @"


  [b]WEEKLY PAYOUTS[/b]
  [b]
  " + weeklyPayoutTeams + @"
  [/b]
";
        }

        private static string GeneratePowerRankings(List<PowerRanking> powerRankings)
        {
            var powerRankingString = string.Empty;
            //1.  [b]Double Trouble(+0)[/b]

            foreach (var ranking in powerRankings.OrderBy(p => p.CurrentPowerRanking))
            {
                powerRankingString += ranking.CurrentPowerRanking + ".  [b]" + ranking.TeamName + "[/b] [i][b](" + (ranking.CurrentPowerRanking >= ranking.PreviousPowerRanking ? "+" + (ranking.CurrentPowerRanking - ranking.PreviousPowerRanking) : "-" + (ranking.PreviousPowerRanking - ranking.CurrentPowerRanking)) + ")[/b][/i]";
                powerRankingString += @"
                ";
            }

            return powerRankingString;
        }

        private static string BuildPlayerOfTheWeek(PlayerOfTheWeek playerOfTheWeek)
        {
            return @"[player#" + playerOfTheWeek.PlayerId + "]" + playerOfTheWeek.Name.ToUpper()+ "[/player] (" + playerOfTheWeek.Team.ToUpper()+ ") - " + playerOfTheWeek.Points + " POINTS";
        }

        private static string GetRecapInfo(Week currentWeek)
        {
            var recapString = string.Empty;

            foreach (var score in currentWeek.Scores)
            {
                if (score.HomeTeam.Win)
                {
                    recapString += $"[b]{score.HomeTeam.TeamName.ToUpper()}[/b] <update> [b]{score.AwayTeam.TeamName.ToUpper()}[/b]!!!!!";
                }
                else
                {
                    recapString += $"[b]{score.AwayTeam.TeamName.ToUpper()}[/b] <update> [b]{score.HomeTeam.TeamName.ToUpper()}[/b]!!!!!";
                }

                recapString += @"

                ";
            }

            return recapString;
        }

        private static void AssignTrophies(Week currentWeek, ChromeDriver driver)
        {
            //http://games.espn.com/ffl/trophylist?leagueId=127291
            //var week =
            //Assign500ClubTrophies(currentWeek, driver);
            //throw new System.NotImplementedException();
        }

        private static void Assign500ClubTrophies(Week currentWeek, ChromeDriver driver)
        {
            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/trophylist?leagueId=127291");

            var trophies = new Trophies.Trophies();
            var winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints > 500).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints > 500).Select(s => s.HomeTeam));

            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                driver.FindElement(By.XPath("//table/tbody/tr/td/div/div/center/b[contains(.," + trophies.Da500Club + ")]/parent::center/parent::div/div/a[contains(.,'Assign')]")).Click();
                driver.WaitUntilElementExists(By.Id("assignTrophyDiv"));

                var dropdown = driver.FindElement(By.Id("assignTeamId"));
                var dropdownSelect = new SelectElement(dropdown);
                dropdownSelect.SelectByText(team.TeamName);

                driver.FindElement(By.Name("headline")).SendKeys($"For putting up {team.TeamPoints} points!!!!!");

                var showcase = driver.FindElement(By.Id("isShowcase"));
                var showcaseDropdown = new SelectElement(showcase);
                showcaseDropdown.SelectByText("Yes");

                //driver.FindElement(By.Name("btnSubmit")).Click();
                //driver.WaitUntilElementExists(By.ClassName("bodyCopy"));
            }
        }

        private static List<PowerRanking> GetPowerRankings(ChromeDriver driver)
        {
            var currentWeek = week;
            List<Week> weeksForPowerRankings = new List<Week>();
            //TODO: Get previous power ranking
            //TODO: Need to account for weeks < 3
            for (int i = 3; i >= 0; i--)
            {
                if (currentWeek - i > 0)
                {
                    weeksForPowerRankings.Add(GetWeek(currentWeek - i, driver));
                }
            }

            var powerRankingGenerator = new PowerRankingGenerator(weeksForPowerRankings, currentWeek);
            return powerRankingGenerator.GeneratePowerRankings();
        }

        private static Week GetWeek(int weekNumber, ChromeDriver driver)
        {
            var week = new Week();
            var scores = new List<Score>();
            
            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/leagueoffice?leagueId=127291&seasonId={year}");
            driver.WaitUntilElementExists(By.ClassName("games-nav"));

            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/scoreboard?leagueId=127291&matchupPeriodId={weekNumber}");
            driver.WaitUntilElementExists(By.ClassName("ptsBased"));

            var matchups = driver.FindElements(By.XPath("//table[@class='ptsBased matchup']"));
            foreach (var matchup in matchups)
            {
                scores.Add(BuildScore(matchup));
            }
            week.Scores = scores;
            week.WeekNumber = weekNumber;

            return week;
        }

        private static Score BuildScore(IWebElement matchupElement)
        {
            var score = new Score();
            var teams = matchupElement.FindElements(By.XPath("./tbody//tr"));
            var awayTeam = true;
            foreach (var team in teams)
            {
                if (awayTeam) //new score; build away
                {
                    score = new Score();
                    score.AwayTeam = BuildTeam(team);
                    awayTeam = false;
                }
                else //build home team
                {
                    score.HomeTeam = BuildTeam(team);
                    break;
                }
            }

            return score;
        }

        private static Team BuildTeam(IWebElement teamElement)
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
