using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RML.PlayerComparer
{
    public class PlayerComparer
{
    readonly List<SiteCode> siteCodes = new List<SiteCode>
    {
        new SiteCode("Buffalo Bills", "buf", "buf", "buf"),
        new SiteCode("Dallas Cowboys", "dal", "dal", "dal"),
        new SiteCode("Miami Dolphins", "mia", "mia", "mia"),
        new SiteCode("New York Giants", "nyg", "nyg", "nyg"),
        new SiteCode("New England Patriots", "ne", "nwe", "ne"),
        new SiteCode("Philadelphia Eagles", "phi", "phi", "phi"),
        new SiteCode("New York Jets", "nyj", "nyj", "nyj"),
        new SiteCode("Washington Redskins", "wsh", "was", "was"),

        new SiteCode("Denver Broncos", "den", "den", "den"),
        new SiteCode("Arizona Cardinals", "ari", "ari", "arz"),
        new SiteCode("Kansas City Chiefs", "kc", "kan", "kc"),
        new SiteCode("Los Angeles Rams", "lar", "lar", "ram"),
        new SiteCode("Los Angeles Chargers", "lac", "lac", "sd"),
        new SiteCode("San Fransisco 49ers", "sf", "sfo", "sf"),
        new SiteCode("Oakland Raiders", "oak", "oak", "oak"),
        new SiteCode("Seattle Seahawks", "sea", "sea", "sea"),

        new SiteCode("Baltimore Ravens", "bal", "bal", "bal"),
        new SiteCode("Chicago Bears", "chi", "chi", "chi"),
        new SiteCode("Cincinnati Bengals", "cin", "cin", "cin"),
        new SiteCode("Detroit Lions", "det", "det", "det"),
        new SiteCode("Cleveland Browns", "cle", "cle", "cle"),
        new SiteCode("Green Bay Packers", "gb", "gnb", "gb"),
        new SiteCode("Pittsburg Steelers", "pit", "pit", "pit"),
        new SiteCode("Minnesota Vikings", "min", "min", "min"),

        new SiteCode("Houston Texans", "hou", "hou", "hou"),
        new SiteCode("Atlanta Falcons", "atl", "atl", "atl"),
        new SiteCode("Indianapolis Colts", "ind", "ind", "ind"),
        new SiteCode("Carolina Panthers", "car", "car", "car"),
        new SiteCode("Jacksonville Jaguars", "jax", "jac", "jax"),
        new SiteCode("New Orleans Saints", "no", "nor", "no"),
        new SiteCode("Tennessee Titans", "ten", "ten", "ten"),
        new SiteCode("Tampa Bay Buccaneers", "tb", "tam", "tb")
    };

    private readonly ChromeDriver _driver;
    private readonly List<RmlPlayer> _rmlPlayers;

    public PlayerComparer(ChromeDriver driver, List<RmlPlayer> rmlPlayers)
    {
        _driver = driver;
        _rmlPlayers = rmlPlayers;
    }

    public void ComparePlayers()
    {
            var sitePlayers = new List<SitePlayer>();
            var espnUrlTemplate = "http://www.espn.com/nfl/team/depth/_/name/{0}/formation/special-teams";
            var yahooUrlTemplate = "https://sports.yahoo.com/nfl/teams/{0}/roster/";
            var ourladsUrlTemplate = "http://www.ourlads.com/nfldepthcharts/depthchart/{0}";
            var count = 0;
            foreach (var siteCode in siteCodes)
            {
                //yahoo
                _driver.NavigateToUrl(string.Format(yahooUrlTemplate, siteCode.YahooCode));
                _driver.FindElement(By.XPath("//li[contains(.,'Defense')]")).Click();
                _driver.WaitUntilElementExists(By.XPath($"//div/h4[contains(.,'Free Safety')]"));

                foreach (var linebackerPositionsYahoo in PlayerConstants.LinebackerPositionsYahoo)
                {
                    var yahooStrongSafetyElement = _driver.FindElements(By.XPath($"//div/h4[contains(.,'{linebackerPositionsYahoo.Key}')]"));
                    //var test = sitePlayers.Where(s => s.Team == "Dallas Cowboys").ToList();
                    if (yahooStrongSafetyElement.Count == 1)
                    {
                        var yahooSafeties = yahooStrongSafetyElement[0].FindElements(By.XPath("./parent::div/ul/li/div/a"));

                        if (yahooSafeties.Count > 0)
                        {
                            sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Starter, linebackerPositionsYahoo.Value, SitePlayer.SiteEnum.Yahoo, yahooSafeties[0].Text));
                        }
                        if (yahooSafeties.Count > 1)
                        {
                            if (yahooStrongSafetyElement[0].Text == string.Empty)
                            {
                                sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Starter, linebackerPositionsYahoo.Value, SitePlayer.SiteEnum.Yahoo, yahooSafeties[1].Text));
                            }
                            else
                            {
                                sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Secondary, linebackerPositionsYahoo.Value, SitePlayer.SiteEnum.Yahoo, yahooSafeties[1].Text));
                            }
                        }
                        if (yahooSafeties.Count > 2)
                        {
                            sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Tertiary, linebackerPositionsYahoo.Value, SitePlayer.SiteEnum.Yahoo, yahooSafeties[2].Text));
                        }
                    }
                }

                _driver.NavigateToUrl(string.Format(espnUrlTemplate, siteCode.EspnCode));
                _driver.FindElement(By.XPath("//div[@id='my-teams-table']/div/ul/li[2]/a")).Click();
                _driver.WaitUntilElementExists(By.XPath($"//table/tbody/tr/td[contains(.,'SS')]"));

                foreach (var linebackerPositionsEspn in PlayerConstants.LinebackerPositionsEspn)
                {
                    var espnStrongSafetyElement = _driver.FindElements(By.XPath($"//table/tbody/tr/td[contains(.,'{linebackerPositionsEspn.Key}')]"));
                    if (espnStrongSafetyElement.Count == 1)
                    {
                        var espnSafties = espnStrongSafetyElement[0].FindElements(By.XPath("./parent::tr/td"));

                        if (espnSafties.Count > 1)
                        {
                            sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Starter, linebackerPositionsEspn.Value, SitePlayer.SiteEnum.ESPN, espnSafties[1].Text));
                        }
                        if (espnSafties.Count > 2)
                        {
                            if (espnSafties[0].Text == string.Empty)
                            {
                                sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Starter, linebackerPositionsEspn.Value, SitePlayer.SiteEnum.ESPN, espnSafties[2].Text));
                            }
                            else
                            {
                                sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Secondary, linebackerPositionsEspn.Value, SitePlayer.SiteEnum.ESPN, espnSafties[2].Text));
                            }
                        }
                        if (espnSafties.Count > 3)
                        {
                            sitePlayers.Add(new SitePlayer(siteCode.TeamCode, PlayerConstants.DepthChartEnum.Tertiary, linebackerPositionsEspn.Value, SitePlayer.SiteEnum.ESPN, espnSafties[3].Text));
                        }
                    }
                }

                //ourlads
                //_driver.Navigate().GoToUrl(string.Format(ourladsUrlTemplate, siteCode.OurladsCode));
                //var ourladsKickReturnersElement = _driver.FindElements(By.XPath("//table/tbody/tr/td[contains(.,'KR')]"));
                //if (ourladsKickReturnersElement.Count == 1)
                //{
                //    var ourladsKickReturners = ourladsKickReturnersElement[0].FindElements(By.XPath("./parent::tr/td"));

                //    var splitReturnerName = ourladsKickReturners[2].Text.Split(new string[] { ", " }, StringSplitOptions.None);
                //    if (splitReturnerName.Length > 1)
                //    {
                //        returner.OurladsPrimaryKickReturner = splitReturnerName[1] + " " + splitReturnerName[0];
                //    }

                //    splitReturnerName = ourladsKickReturners[4].Text.Split(new string[] { ", " }, StringSplitOptions.None);
                //    if (splitReturnerName.Length > 1)
                //    {
                //        returner.OurladsPrimaryKickReturner = splitReturnerName[1] + " " + splitReturnerName[0];
                //    }

                //    splitReturnerName = ourladsKickReturners[6].Text.Split(new string[] { ", " }, StringSplitOptions.None);
                //    if (splitReturnerName.Length > 1)
                //    {
                //        returner.OurladsTertiaryKickReturner = splitReturnerName[1] + " " + splitReturnerName[0];
                //    }
                //}

                //var ourladsPuntReturnersElement = _driver.FindElements(By.XPath("//table/tbody/tr/td[contains(.,'PR')]"));
                //if (ourladsPuntReturnersElement.Count == 1)
                //{
                //    var ourladsPuntReturners = ourladsPuntReturnersElement[0].FindElements(By.XPath("./parent::tr/td"));

                //    var splitReturnerName = ourladsPuntReturners[2].Text.Split(new string[] { ", " }, StringSplitOptions.None);
                //    if (splitReturnerName.Length > 1)
                //    {
                //        returner.OurladsPrimaryPuntReturner = splitReturnerName[1] + " " + splitReturnerName[0];
                //    }

                //    splitReturnerName = ourladsPuntReturners[4].Text.Split(new string[] { ", " }, StringSplitOptions.None);
                //    if (splitReturnerName.Length > 1)
                //    {
                //        returner.OurladsPrimaryPuntReturner = splitReturnerName[1] + " " + splitReturnerName[0];
                //    }

                //    splitReturnerName = ourladsPuntReturners[6].Text.Split(new string[] { ", " }, StringSplitOptions.None);
                //    if (splitReturnerName.Length > 1)
                //    {
                //        returner.OurladsTertiaryPuntReturner = splitReturnerName[1] + " " + splitReturnerName[0];
                //    }
                //}

                count++;

                if (count % 5 == 0)
                {
                    Console.WriteLine($"******************************************");
                    Console.WriteLine($"Writing Player Comparer: {count} processed.");
                    Console.WriteLine($"******************************************");
                }
            }

            //Console.WriteLine(returners);

            Console.WriteLine("Writing returner File:");

            new PrintPlayerComparerService(sitePlayers.OrderBy(p => p.Name).ToList(), _rmlPlayers).WritePlayerComparerFile();
            Console.WriteLine("Writing returner File COMPLETE");
        }

}

}