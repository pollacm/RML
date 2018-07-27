using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OpenQA.Selenium.Chrome;

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
            var yahooUrlTemplate = "http://in.sports.yahoo.com/nfl/teams/{0}/depthchart?nfl-pos=special";

            foreach (var siteCode in siteCodes)
            {
                var returner = new Returner();
                returner.Team = siteCode.TeamCode;
                //Console.WriteLine("SiteCode: " + siteCode.TeamCode);
                using (WebClient client = new WebClient())
                {
                    var espnUrl = string.Format(espnUrlTemplate, siteCode.EspnCode);
                    string espnHtml = client.DownloadString(espnUrl);
                    var espnHasKickReturner = false;
                    var espnHasPuntReturner = false;

                    doc = new HtmlDocument();
                    doc.LoadHtml(espnHtml);

                    //Console.WriteLine("ESPN Punt Returner Start: ");
                    var players = doc.DocumentNode
                        .Descendants("div")
                        .Where(d =>
                                   d.Attributes.Contains("class")
                                   &&
                                   d.Attributes["class"].Value.Contains("field-group row-2")
                              ).Single().InnerHtml;
                    //Console.WriteLine("ESPN Punt Returner End: ");
                    espnHasKickReturner = players.Contains("Kick Returner");
                    espnHasPuntReturner = players.Contains("Punt Returner");
                    //Console.WriteLine("ESPN Punt Returners Set: ");
                    doc = new HtmlDocument();
                    doc.LoadHtml(players);
                    //Console.WriteLine("ESPN Kick Returner Start: ");
                    var espnReturners = doc.DocumentNode
                        .Descendants("ul")
                        .Where(d =>
                                   d.Attributes.Contains("class")
                                   &&
                                   d.Attributes["class"].Value.Contains("players")
                              ).ToArray();
                    //Console.WriteLine("ESPN Kick Returner End: ");
                    if (espnHasKickReturner)
                    {
                        var kickReturnersNode = espnReturners[0];

                        doc = new HtmlDocument();
                        doc.LoadHtml(kickReturnersNode.InnerHtml);
                        var kickReturners = doc.DocumentNode
                            .Descendants("a").ToArray();

                        for (var index = 0; index < kickReturners.Count(); index++)
                        {
                            if (index == 0)
                            {
                                returner.EspnPrimaryKickReturner = kickReturners[index].InnerText;
                            }

                            if (index == 1)
                            {
                                returner.EspnSecondaryKickReturner = kickReturners[index].InnerText;
                            }

                            if (index == 2)
                            {
                                returner.EspnTertiaryKickReturner = kickReturners[index].InnerText;
                            }
                        }
                    }
                    //Console.WriteLine("ESPN Kick Returner Set: ");

                    if (espnHasPuntReturner)
                    {
                        var puntReturnersNode = espnHasKickReturner ? espnReturners[1] : espnReturners[0];
                        doc = new HtmlDocument();
                        doc.LoadHtml(puntReturnersNode.InnerHtml);
                        var puntReturners = doc.DocumentNode
                            .Descendants("a").ToArray();

                        for (var index = 0; index < puntReturners.Count(); index++)
                        {
                            if (index == 0)
                            {
                                returner.EspnPrimaryPuntReturner = puntReturners[index].InnerText;
                            }

                            if (index == 1)
                            {
                                returner.EspnSecondaryPuntReturner = puntReturners[index].InnerText;
                            }

                            if (index == 2)
                            {
                                returner.EspnTertiaryPuntReturner = puntReturners[index].InnerText;
                            }
                        }
                    }

                    var yahooUrl = string.Format(yahooUrlTemplate, siteCode.YahooCode);
                    string yahooHtml = client.DownloadString(yahooUrl);
                    doc = new HtmlDocument();
                    doc.LoadHtml(yahooHtml);
                    //Console.WriteLine("Yahoo Data Loaded: ");
                    var playersYahoo = doc.DocumentNode
                                           .Descendants("li")
                                           .Where(d =>
                                                      d.Attributes.Contains("class")
                                                      &&
                                                      d.Attributes["class"].Value.Contains("kick-returner")
                                                 ).SingleOrDefault()?.InnerHtml ?? string.Empty;
                    //Console.WriteLine("Yahoo data loaded complete: ");
                    if (!string.IsNullOrEmpty(playersYahoo))
                    {
                        doc = new HtmlDocument();
                        doc.LoadHtml(playersYahoo);
                        //Console.WriteLine("Yahoo kick returners start: ");
                        var kickReturnersYahoo = doc.DocumentNode
                            .Descendants("a").ToArray();

                        for (var index = 0; index < kickReturnersYahoo.Count(); index++)
                        {
                            if (index == 0)
                            {
                                returner.YahooPrimaryKickReturner = kickReturnersYahoo[index].InnerText;
                            }

                            if (index == 1)
                            {
                                returner.YahooSecondaryKickReturner = kickReturnersYahoo[index].InnerText;
                            }

                            if (index == 2)
                            {
                                returner.YahooTertiaryKickReturner = kickReturnersYahoo[index].InnerText;
                            }
                        }

                        //Console.WriteLine("Yahoo Kick Returners Set: ");
                    }

                    doc = new HtmlDocument();
                    doc.LoadHtml(yahooHtml);
                    //Console.WriteLine("Yahoo punt Returner Start: ");
                    playersYahoo = doc.DocumentNode
                        .Descendants("li")
                        .Where(d =>
                                   d.Attributes.Contains("class")
                                   &&
                                   d.Attributes["class"].Value.Contains("punt-returner")
                              ).Single().InnerHtml;
                    //Console.WriteLine("Yahoo Punt Returner End: ");
                    doc = new HtmlDocument();
                    doc.LoadHtml(playersYahoo);

                    var puntReturnersYahoo = doc.DocumentNode
                        .Descendants("a").ToArray();

                    for (var index = 0; index < puntReturnersYahoo.Count(); index++)
                    {
                        if (index == 0)
                        {
                            returner.YahooPrimaryPuntReturner = puntReturnersYahoo[index].InnerText;
                        }

                        if (index == 1)
                        {
                            returner.YahooSecondaryPuntReturner = puntReturnersYahoo[index].InnerText;
                        }

                        if (index == 2)
                        {
                            returner.YahooTertiaryPuntReturner = puntReturnersYahoo[index].InnerText;
                        }
                    }
                    //Console.WriteLine("Yahoo punt returners set: ");

                    //Console.WriteLine(returner);
                    returners.Add(returner);
                    if (returners.Count % 5 == 0)
                    {
                        Console.WriteLine(returners.Count);
                    }
                }
            }

            //Console.WriteLine(returners);

            Console.WriteLine("Writing returner File:");

            new PrintReturnerService(returners).WriteReturnerFile();
            Console.WriteLine("Writing returner File COMPLETE");
        }

}

}