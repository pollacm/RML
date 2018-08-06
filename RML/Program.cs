using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RML.PlayerComparer;
using RML.PowerRankings;
using RML.Returners;
using RML.RmlPlayer;
using RML.SitePlayer;
using RML.Teams;
using RML.Trophies;
using RML.Weeks;

namespace RML
{
    internal class Program
    {
        private static readonly int year = 2017;
        private static readonly int week = 10;

        private static void Main(string[] args)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            var driver = new ChromeDriver(options);

            //new ReturnerBuilder(driver).GenerateReturners();

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
            Thread.Sleep(2000);

            driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 0, 0, 5);

            //var rmlPlayerBuilder = new RmlPlayerBuilder(driver, 2018);
            //var rmlPlayers = rmlPlayerBuilder.BuildRmlPlayers();
            //rmlPlayerRepository.RefreshRmlPlayers(rmlPlayers);

            //var rmlPlayerRepository = new RmlPlayerRepository();
            //var rmlPlayers = rmlPlayerRepository.GetRmlPlayers();

            //var sitePlayerBuilder = new SitePlayerBuilder(driver);
            //var sitePlayers = sitePlayerBuilder.BuildSitePlayers();
            //sitePlayerRepository.RefreshSitePlayers(sitePlayers);

            //var sitePlayerRepository = new SitePlayerRepository();
            //var sitePlayers = sitePlayerRepository.GetSitePlayers();

            //Console.WriteLine("Writing returner File:");
            //new PrintPlayerComparerService(sitePlayers.OrderBy(p => p.Name).ToList(), rmlPlayers).WritePlayerComparerFile();
            //Console.WriteLine("Writing returner File COMPLETE");

            //get teams
            var teamBuilder = new TeamBuilder(driver, year);
            var teamRepository = new TeamRepository();

            //var teams = teamBuilder.BuildTeams();
            //teamRepository.RefreshTeams(teams);
            var teams = teamRepository.GetTeams();

            driver.WaitUntilElementExists(By.CssSelector("table.tableBody"));

            //OP
            driver.Navigate().GoToUrl($"http://games.espn.com/ffl/freeagency?leagueId=127291&seasonId={year}");
            driver.WaitUntilElementExists(By.Id("playerTableContainerDiv"));

            var opLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'OP')]"));
            opLink.Click();

            Thread.Sleep(2000);

            var lastLink = driver.FindElement(By.XPath("//tr[contains(@class, 'playerTableBgRowSubhead')]/td/a[contains(.,'LAST')]"));
            lastLink.Click();

            Thread.Sleep(5000);
            
            var opRows = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr"));
            var opsOfTheWeek = new List<PlayerOfTheWeek>();
            var topPoints = 0m;
            foreach (var opRow in opRows)
            {
                var rowPoints = decimal.Parse(opRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
                if (rowPoints >= topPoints)
                {
                    topPoints = rowPoints;
                }
                else
                {
                    break;
                }

                var opOfTheWeek = new PlayerOfTheWeek();
                opOfTheWeek.Name = opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                opOfTheWeek.Team = opRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
                opOfTheWeek.TeamAbbreviation = opRow.FindElement(By.XPath("./td[3]/a")).Text;
                opOfTheWeek.PlayerId = int.Parse(opRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
                opOfTheWeek.Points = rowPoints;

                opsOfTheWeek.Add(opOfTheWeek);
            }

            //DP
            var dpLink = driver.FindElement(By.XPath("//ul[@class='filterToolsOptionSet']/li/a[contains(.,'DP')]"));
            dpLink.Click();

            Thread.Sleep(2000);

            var dpRows = driver.FindElements(By.XPath("//tr[contains(@class, 'pncPlayerRow')]/td[3][not(contains(.,'FA'))]/parent::tr"));
            var dpsOfTheWeek = new List<PlayerOfTheWeek>();
            topPoints = 0m;
            foreach (var dpRow in dpRows)
            {
                var rowPoints = decimal.Parse(dpRow.FindElement(By.XPath("./td[contains(@class, 'sortedCell')]")).Text);
                if (rowPoints >= topPoints)
                {
                    topPoints = rowPoints;
                }
                else
                {
                    break;
                }

                var dpOfTheWeek = new PlayerOfTheWeek();
                dpOfTheWeek.Name = dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).Text;
                dpOfTheWeek.Team = dpRow.FindElement(By.XPath("./td[3]/a")).GetAttribute("title");
                dpOfTheWeek.TeamAbbreviation = dpRow.FindElement(By.XPath("./td[3]/a")).Text;
                dpOfTheWeek.PlayerId = int.Parse(dpRow.FindElement(By.XPath("./td[@class='playertablePlayerName']/a")).GetAttribute("playerid"));
                dpOfTheWeek.Points = rowPoints;

                dpsOfTheWeek.Add(dpOfTheWeek);
            }

            var weekRepository = new WeekRepository();
            var powerRankings = GetPowerRankings(driver, weekRepository);
            var currentWeek = weekRepository.GetWeek(driver, week, year);

            var weeklyPayoutTeams = string.Empty;
            for (var i = 1; i <= week; i++)
            {
                var weekForWeeklyPayouts = weekRepository.GetWeek(driver, i, year);
                var teamsForWeeklyPayouts = weekForWeeklyPayouts.Scores.Select(s => s.AwayTeam).ToList();
                teamsForWeeklyPayouts.AddRange(weekForWeeklyPayouts.Scores.Select(s => s.HomeTeam));
                weeklyPayoutTeams += i + ". " + teamsForWeeklyPayouts.OrderByDescending(t => t.TeamPoints).First().TeamName.ToUpper() + @"
                ";
            }

            CreateLeaguePage(powerRankings, weeklyPayoutTeams, opsOfTheWeek, dpsOfTheWeek, currentWeek, week);
            //TODO: Assign Trophies.. Need to prompt if it should happen
            AssignTrophies(currentWeek, opsOfTheWeek, dpsOfTheWeek, driver);
            var x = 1;
        }

        private static void CreateLeaguePage(List<PowerRanking> powerRankings, string weeklyPayoutTeams, List<PlayerOfTheWeek> opsOfTheWeek, List<PlayerOfTheWeek> dpsOfTheWeek, Week currentWeek, int i)
        {
            var leagueMessage = @"[b]<update> IN WEEK " + week + @"[/b]!!!!!

[image]<update>[/image]

[b]R.M.L. WEEK " + week + @" - <update>[/b]

<update>

[b]WEEK " + week + @" RECAP[/b]

" + GetRecapInfo(currentWeek) + @"

  [b]OFFENSIVE PLAYER OF THE WEEK[/b]

  " + BuildPlayersOfTheWeek(opsOfTheWeek) + @"


  [b]DEFENSIVE PLAYER OF THE WEEK[/b]

  " + BuildPlayersOfTheWeek(dpsOfTheWeek) + @"

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
                powerRankingString += ranking.CurrentPowerRanking + ".  [b]" + ranking.TeamName + "[/b] [i][b](" + (ranking.CurrentPowerRanking >= ranking.PreviousPowerRanking
                                          ? "+" + (ranking.CurrentPowerRanking - ranking.PreviousPowerRanking)
                                          : "-" + (ranking.PreviousPowerRanking - ranking.CurrentPowerRanking)) + ")[/b][/i]";
                powerRankingString += @"
                ";
            }

            return powerRankingString;
        }

        private static string BuildPlayersOfTheWeek(List<PlayerOfTheWeek> playersOfTheWeek)
        {
            var playersOfTheWeekString = string.Empty;
            foreach (var player in playersOfTheWeek)
            {
                playersOfTheWeekString += @"
                [b] [player#" + player.PlayerId + "]" + player.Name.ToUpper() + "[/player] (" + player.Team.ToUpper() + ") - " + player.Points + @" POINTS [/b]
            
            [image]<update>[/image]
            
            ";
            }

            return playersOfTheWeekString;
        }

        private static string GetRecapInfo(Week currentWeek)
        {
            var recapString = string.Empty;

            foreach (var score in currentWeek.Scores)
            {
                if (score.HomeTeam.Win)
                    recapString += $"[b]{score.HomeTeam.TeamName.ToUpper()}[/b] <update> [b]{score.AwayTeam.TeamName.ToUpper()}[/b]!!!!!";
                else
                    recapString += $"[b]{score.AwayTeam.TeamName.ToUpper()}[/b] <update> [b]{score.HomeTeam.TeamName.ToUpper()}[/b]!!!!!";

                recapString += @"

                ";
            }

            return recapString;
        }

        //TODO: Need to finish trophies and have them dynamically get generated in the leagueMessage
        //Need to add a bool for assigntrophies and default to no so you dont accidently assign trophies
        private static List<Trophy> AssignTrophies(Week currentWeek, List<PlayerOfTheWeek> opsOfTheWeek, List<PlayerOfTheWeek> dpsOfTheWeek, ChromeDriver driver)
        {
            var trophies = new List<Trophy>();

            //500 club
            var winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints >= 500 && s.AwayTeam.TeamPoints < 600).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints >= 500 && s.AwayTeam.TeamPoints < 600).Select(s => s.HomeTeam));

            var trophyAssigner = new TrophyAssigner(driver, year);
            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new FiveHundredClubTrophy()));
            }

            //600 club
            winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints >= 600 && s.AwayTeam.TeamPoints < 700).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints >= 600 && s.AwayTeam.TeamPoints < 700).Select(s => s.HomeTeam));
            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new SixHundredClubTrophy()));
            }

            //700 club
            winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints >= 700).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints >= 700).Select(s => s.HomeTeam));
            foreach (var team in winners.OrderByDescending(t => t.TeamPoints))
            {
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new SevenHundredClubTrophy()));
            }

            //ballers/losers
            var largestMargin = currentWeek.Scores.OrderByDescending(s => s.MarginOfVictory).First().MarginOfVictory;
            var ballersAndMeeksOfTheWeek = currentWeek.Scores.Where(s => s.MarginOfVictory == largestMargin);

            foreach (var score in ballersAndMeeksOfTheWeek)
            {
                if (score.HomeTeam.TeamPoints > score.AwayTeam.TeamPoints)
                {
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.HomeTeam, new BallerOfTheWeekTrophy(), score.MarginOfVictory.ToString(CultureInfo.InvariantCulture)));
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.AwayTeam, new LoserOfTheWeekTrophy(), score.MarginOfVictory.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.AwayTeam, new BallerOfTheWeekTrophy(), score.MarginOfVictory.ToString(CultureInfo.InvariantCulture)));
                    trophies.Add(trophyAssigner.AssignTrophy(currentWeek, score.HomeTeam, new LoserOfTheWeekTrophy(), score.MarginOfVictory.ToString(CultureInfo.InvariantCulture)));
                }
            }

            Thread.Sleep(2000);
            //ops
            var teams = currentWeek.Scores.Select(s => s.HomeTeam).ToList();
            teams.AddRange(currentWeek.Scores.Select(s => s.AwayTeam));

            foreach (var op in opsOfTheWeek)
            {
                string additionalInfo = JsonConvert.SerializeObject(op);
                var team = teams.Single(t => t.TeamName == op.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new OffensivePlayerOfTheWeekTrophy(), additionalInfo));
            }

            //dps
            foreach (var dp in dpsOfTheWeek)
            {
                string additionalInfo = JsonConvert.SerializeObject(dp);
                var team = teams.Single(t => t.TeamName == dp.Team);
                trophies.Add(trophyAssigner.AssignTrophy(currentWeek, team, new DefensivePlayerOfTheWeekTrophy(), additionalInfo));
            }

            if (currentWeek.WeekNumber == 13)
            {
                //assign yearly trophies
            }

            return trophies;
        }

        private static List<PowerRanking> GetPowerRankings(ChromeDriver driver, WeekRepository weekRepository)
        {
            var currentWeek = week;
            var weeksForPowerRankings = new List<Week>();
            
            for (var i = 3; i >= 0; i--)
                if (currentWeek - i > 0)
                    weeksForPowerRankings.Add(weekRepository.GetWeek(driver, currentWeek - i, year));

            var powerRankingGenerator = new PowerRankingGenerator(weeksForPowerRankings, currentWeek);
            return powerRankingGenerator.GeneratePowerRankings();
        }
    }
}