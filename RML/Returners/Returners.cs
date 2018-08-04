using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RML.SiteCodes;

namespace RML.Returners
{
    public class Returners
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

    public Returners(ChromeDriver driver)
    {
        _driver = driver;
    }

    public void GenerateReturners()
    {
            var returners = new List<Returner>();
            var espnUrlTemplate = "http://www.espn.com/nfl/team/depth/_/name/{0}/formation/special-teams";
            var yahooUrlTemplate = "https://sports.yahoo.com/nfl/teams/{0}/roster/";
            var ourladsUrlTemplate = "http://www.ourlads.com/nfldepthcharts/depthchart/{0}";
            var count = 0;
            foreach (var siteCode in siteCodes)
            {
                //yahoo
                _driver.Navigate().GoToUrl(string.Format(yahooUrlTemplate, siteCode.YahooCode));
                _driver.FindElement(By.XPath("//li[contains(.,'Specialists')]")).Click();
                System.Threading.Thread.Sleep(1000);

                var returner = new Returner();
                returner.Team = siteCode.TeamCode;

                var yahooKickReturnersElement = _driver.FindElements(By.XPath("//div/h4[contains(.,'Kick Returner')]"));
                if (yahooKickReturnersElement.Count == 1)
                {
                    var yahooKickReturners = yahooKickReturnersElement[0].FindElements(By.XPath("./parent::div/ul/li/div/a"));

                    if (yahooKickReturners.Count > 0)
                    {
                        returner.YahooPrimaryKickReturner = yahooKickReturners[0].Text;
                    }
                    if (yahooKickReturners.Count > 1)
                    {
                        if (yahooKickReturners[0].Text == string.Empty)
                        {
                            returner.YahooPrimaryKickReturner = yahooKickReturners[1].Text;
                        }
                        else
                        {
                            returner.YahooSecondaryKickReturner = yahooKickReturners[1].Text;
                        }
                    }
                    if (yahooKickReturners.Count > 2)
                    {
                        returner.YahooTertiaryKickReturner = yahooKickReturners[2].Text;
                    }
                }

                var yahooPuntReturnersElement = _driver.FindElements(By.XPath("//div/h4[contains(.,'Punt Returner')]"));
                if (yahooPuntReturnersElement.Count == 1)
                {
                    var yahooPuntReturners = yahooPuntReturnersElement[0].FindElements(By.XPath("./parent::div/ul/li/div/a"));

                    if (yahooPuntReturners.Count > 0)
                    {
                        returner.YahooPrimaryPuntReturner = yahooPuntReturners[0].Text;
                    }
                    if (yahooPuntReturners.Count > 1)
                    {
                        if (yahooPuntReturners[0].Text == string.Empty)
                        {
                            returner.YahooPrimaryPuntReturner = yahooPuntReturners[1].Text;
                        }
                        else
                        {
                            returner.YahooSecondaryPuntReturner = yahooPuntReturners[1].Text;
                        }
                    }
                    if (yahooPuntReturners.Count > 2)
                    {
                        returner.YahooTertiaryPuntReturner = yahooPuntReturners[2].Text;
                    }
                }

                //espn
                _driver.Navigate().GoToUrl(string.Format(espnUrlTemplate, siteCode.EspnCode));
                var espnKickReturnersElement = _driver.FindElements(By.XPath("//table/tbody/tr/td[contains(.,'KR')]"));
                if (espnKickReturnersElement.Count == 1)
                {
                    var espnKickReturners = espnKickReturnersElement[0].FindElements(By.XPath("./parent::tr/td"));

                    if (espnKickReturners.Count > 1)
                    {
                        returner.EspnPrimaryKickReturner = espnKickReturners[1].Text;
                    }
                    if (espnKickReturners.Count > 2)
                    {
                        if (espnKickReturners[0].Text == string.Empty)
                        {
                            returner.EspnPrimaryKickReturner = espnKickReturners[2].Text;
                        }
                        else
                        {
                            returner.EspnSecondaryKickReturner = espnKickReturners[2].Text;
                        }
                    }
                    if (espnKickReturners.Count > 3)
                    {
                        returner.EspnTertiaryKickReturner = espnKickReturners[3].Text;
                    }
                }

                var espnPuntReturnersElement = _driver.FindElements(By.XPath("//table/tbody/tr/td[contains(.,'PR')]"));
                if (espnPuntReturnersElement.Count == 1)
                {
                    var espnPuntReturners = espnPuntReturnersElement[0].FindElements(By.XPath("./parent::tr/td"));

                    if (espnPuntReturners.Count > 1)
                    {
                        returner.EspnPrimaryPuntReturner = espnPuntReturners[1].Text;
                    }
                    if (espnPuntReturners.Count > 2)
                    {
                        if (espnPuntReturners[0].Text == string.Empty)
                        {
                            returner.EspnPrimaryPuntReturner = espnPuntReturners[2].Text;
                        }
                        else
                        {
                            returner.EspnSecondaryPuntReturner = espnPuntReturners[2].Text;
                        }
                    }
                    if (espnPuntReturners.Count > 3)
                    {
                        returner.EspnTertiaryPuntReturner = espnPuntReturners[3].Text;
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

                returners.Add(returner);

                count++;

                if (count % 5 == 0)
                {
                    Console.WriteLine($"******************************************");
                    Console.WriteLine($"Writing Returners: {count} processed.");
                    Console.WriteLine($"******************************************");
                }
            }

            //Console.WriteLine(returners);

            Console.WriteLine("Writing returner File:");

            new PrintReturnerService(returners).WriteReturnerFile();
            Console.WriteLine("Writing returner File COMPLETE");
        }

}

}