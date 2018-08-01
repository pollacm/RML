using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RML.Returners;

namespace RML.CornersAndSafeties
{
    public class Corners
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
    private readonly List<RmlCorner> _rmlCorners;

    public Corners(ChromeDriver driver, List<RmlCorner> rmlCorners)
    {
        _driver = driver;
        _rmlCorners = rmlCorners;
    }

    public void GenerateCorners()
    {
            var siteCorners = new List<SiteCorner>();
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

                var siteCorner = new SiteCorner();
                siteCorner.Team = siteCode.TeamCode;

                var yahooStrongSafetyElement = _driver.FindElements(By.XPath("//div/h4[contains(.,'Kick Returner')]"));
                if (yahooStrongSafetyElement.Count == 1)
                {
                    var yahooSafeties = yahooStrongSafetyElement[0].FindElements(By.XPath("./parent::div/ul/li/div/a"));

                    if (yahooSafeties.Count > 0)
                    {
                        siteCorner.YahooPrimaryStrongSafety = yahooSafeties[0].Text;
                    }
                    if (yahooSafeties.Count > 1)
                    {
                        if (yahooStrongSafetyElement[0].Text == string.Empty)
                        {
                            siteCorner.YahooPrimaryStrongSafety = yahooSafeties[1].Text;
                        }
                        else
                        {
                            siteCorner.YahooSecondaryStrongSafety = yahooSafeties[1].Text;
                        }
                    }
                    if (yahooSafeties.Count > 2)
                    {
                        siteCorner.YahooTertiaryStrongSafety = yahooSafeties[2].Text;
                    }
                }

                var yahooFreeSafetyElement = _driver.FindElements(By.XPath("//div/h4[contains(.,'Kick Returner')]"));
                if (yahooFreeSafetyElement.Count == 1)
                {
                    var yahooSafeties = yahooFreeSafetyElement[0].FindElements(By.XPath("./parent::div/ul/li/div/a"));

                    if (yahooSafeties.Count > 0)
                    {
                        siteCorner.YahooPrimaryFreeSafety = yahooSafeties[0].Text;
                    }
                    if (yahooSafeties.Count > 1)
                    {
                        if (yahooFreeSafetyElement[0].Text == string.Empty)
                        {
                            siteCorner.YahooPrimaryFreeSafety = yahooSafeties[1].Text;
                        }
                        else
                        {
                            siteCorner.YahooSecondaryFreeSafety = yahooSafeties[1].Text;
                        }
                    }
                    if (yahooSafeties.Count > 2)
                    {
                        siteCorner.YahooTertiaryFreeSafety = yahooSafeties[2].Text;
                    }
                }

                //espn
                _driver.Navigate().GoToUrl(string.Format(espnUrlTemplate, siteCode.EspnCode));
                var espnStrongSafetyElement = _driver.FindElements(By.XPath("//table/tbody/tr/td[contains(.,'KR')]"));
                if (espnStrongSafetyElement.Count == 1)
                {
                    var espnSafties = espnStrongSafetyElement[0].FindElements(By.XPath("./parent::tr/td"));

                    if (espnSafties.Count > 1)
                    {
                        siteCorner.EspnPrimaryStrongSafety = espnSafties[1].Text;
                    }
                    if (espnSafties.Count > 2)
                    {
                        if (espnSafties[0].Text == string.Empty)
                        {
                            siteCorner.EspnPrimaryStrongSafety = espnSafties[2].Text;
                        }
                        else
                        {
                            siteCorner.EspnPrimaryStrongSafety = espnSafties[2].Text;
                        }
                    }
                    if (espnSafties.Count > 3)
                    {
                        siteCorner.EspnTertiaryStrongSafety = espnSafties[3].Text;
                    }
                }

                var espnFreeSafetyElement = _driver.FindElements(By.XPath("//table/tbody/tr/td[contains(.,'KR')]"));
                if (espnFreeSafetyElement.Count == 1)
                {
                    var espnSafties = espnFreeSafetyElement[0].FindElements(By.XPath("./parent::tr/td"));

                    if (espnSafties.Count > 1)
                    {
                        siteCorner.EspnPrimaryFreeSafety = espnSafties[1].Text;
                    }
                    if (espnSafties.Count > 2)
                    {
                        if (espnSafties[0].Text == string.Empty)
                        {
                            siteCorner.EspnPrimaryFreeSafety = espnSafties[2].Text;
                        }
                        else
                        {
                            siteCorner.EspnPrimaryFreeSafety = espnSafties[2].Text;
                        }
                    }
                    if (espnSafties.Count > 3)
                    {
                        siteCorner.EspnTertiaryFreeSafety = espnSafties[3].Text;
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

                siteCorners.Add(siteCorner);

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

            new PrintSafetyService(siteCorners, _rmlCorners).WriteSafetyFile();
            Console.WriteLine("Writing returner File COMPLETE");
        }

}

}