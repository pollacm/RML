public class Returners
{
    List<SiteCode> siteCodes = new List<SiteCode>{
    new SiteCode("Buffalo Bills", "buf", "buf"),
    new SiteCode("Dallas Cowboys", "dal", "dal"),
    new SiteCode("Miami Dolphins", "mia", "mia"),
    new SiteCode("New York Giants", "nyg", "nyg"),
    new SiteCode("New England Patriots", "ne", "nwe"),
    new SiteCode("Philadelphia Eagles", "phi", "phi"),
    new SiteCode("New York Jets", "nyj", "nyj"),
    new SiteCode("Washington Redskins", "wsh", "was"),

    new SiteCode("Denver Broncos", "den", "den"),
    new SiteCode("Arizona Cardinals", "ari", "ari"),
    new SiteCode("Kansas City Chiefs", "kc", "kan"),
    new SiteCode("Los Angeles Rams", "lar", "lar"),
    new SiteCode("Los Angeles Chargers", "lac", "lac"),
    new SiteCode("San Fransisco 49ers", "sf", "sfo"),
    new SiteCode("Oakland Raiders", "oak", "oak"),
    new SiteCode("Seattle Seahawks", "sea", "sea"),

    new SiteCode("Baltimore Ravens", "bal", "bal"),
    new SiteCode("Chicago Bears", "chi", "chi"),
    new SiteCode("Cincinnati Bengals", "cin", "cin"),
    new SiteCode("Detroit Lions", "det", "det"),
    new SiteCode("Cleveland Browns", "cle", "cle"),
    new SiteCode("Green Bay Packers", "gb", "gnb"),
    new SiteCode("Pittsburg Steelers", "pit", "pit"),
    new SiteCode("Minnesota Vikings", "min", "min"),

    new SiteCode("Houston Texans", "hou", "hou"),
    new SiteCode("Atlanta Falcons", "atl", "atl"),
    new SiteCode("Indianapolis Colts", "ind", "ind"),
    new SiteCode("Carolina Panthers", "car", "car"),
    new SiteCode("Jacksonville Jaguars", "jax", "jac"),
    new SiteCode("New Orleans Saints", "no", "nor"),
    new SiteCode("Tennessee Titans", "ten", "ten"),
    new SiteCode("Tampa Bay Buccaneers", "tb", "tam")
};



    var returners = new List<Returner>();
    var espnUrlTemplate = "http://www.espn.com/nfl/team/depth/_/name/{0}/formation/special-teams";
    var yahooUrlTemplate = "http://in.sports.yahoo.com/nfl/teams/{0}/depthchart?nfl-pos=special";
    var doc = new HtmlDocument();

foreach (var siteCode in siteCodes)
{
    var returner = new Returner();
    returner.Team = siteCode.TeamCode;
    //("SiteCode: " + siteCode.TeamCode).Dump();
    using (WebClient client = new WebClient())
    {
        var espnUrl = string.Format(espnUrlTemplate, siteCode.EspnCode);
    string espnHtml = client.DownloadString(espnUrl);
    var espnHasKickReturner = false;
    var espnHasPuntReturner = false;

    doc = new HtmlDocument();
    doc.LoadHtml(espnHtml);

        //("ESPN Punt Returner Start: ").Dump();
        var players = doc.DocumentNode
                         .Descendants("div")
                         .Where(d =>
                            d.Attributes.Contains("class")
                             &&
                             d.Attributes["class"].Value.Contains("field-group row-2")
                         ).Single().InnerHtml;
    //("ESPN Punt Returner End: ").Dump();
    espnHasKickReturner = players.Contains("Kick Returner");
        espnHasPuntReturner = players.Contains("Punt Returner");
        //("ESPN Punt Returners Set: ").Dump();
        doc = new HtmlDocument();
    doc.LoadHtml(players);
        //("ESPN Kick Returner Start: ").Dump();
        var espnReturners = doc.DocumentNode
                        .Descendants("ul")
                        .Where(d =>
                          d.Attributes.Contains("class")
                           &&
                           d.Attributes["class"].Value.Contains("players")
                        ).ToArray();
        //("ESPN Kick Returner End: ").Dump();
        if (espnHasKickReturner)
        {
            var kickReturnersNode = espnReturners[0];

    doc = new HtmlDocument();
    doc.LoadHtml(kickReturnersNode.InnerHtml);
            var kickReturners = doc.DocumentNode
                             .Descendants("a").ToArray();

            for (var index = 0; index<kickReturners.Count(); index++)
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
        //("ESPN Kick Returner Set: ").Dump();

        if (espnHasPuntReturner)
        {
            var puntReturnersNode = espnHasKickReturner ? espnReturners[1] : espnReturners[0];
doc = new HtmlDocument();
doc.LoadHtml(puntReturnersNode.InnerHtml);
            var puntReturners = doc.DocumentNode
                             .Descendants("a").ToArray();

            for (var index = 0; index<puntReturners.Count(); index++)
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
        //("Yahoo Data Loaded: ").Dump();
        var playersYahoo = doc.DocumentNode
                         .Descendants("li")
                         .Where(d =>
                            d.Attributes.Contains("class")
                             &&
                             d.Attributes["class"].Value.Contains("kick-returner")
                         ).SingleOrDefault()?.InnerHtml ?? string.Empty;
        //("Yahoo data loaded complete: ").Dump();
        if (!string.IsNullOrEmpty(playersYahoo))
        {
            doc = new HtmlDocument();
doc.LoadHtml(playersYahoo);
            //("Yahoo kick returners start: ").Dump();
            var kickReturnersYahoo = doc.DocumentNode
                                    .Descendants("a").ToArray();

            for (var index = 0; index<kickReturnersYahoo.Count(); index++)
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
            //("Yahoo Kick Returners Set: ").Dump();
        }

        doc = new HtmlDocument();
doc.LoadHtml(yahooHtml);
        //("Yahoo punt Returner Start: ").Dump();
        playersYahoo = doc.DocumentNode
                         .Descendants("li")
                         .Where(d =>
                            d.Attributes.Contains("class")
                             &&
                             d.Attributes["class"].Value.Contains("punt-returner")
                         ).Single().InnerHtml;
        //("Yahoo Punt Returner End: ").Dump();
        doc = new HtmlDocument();
doc.LoadHtml(playersYahoo);

        var puntReturnersYahoo = doc.DocumentNode
                                .Descendants("a").ToArray();

        for (var index = 0; index<puntReturnersYahoo.Count(); index++)
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
        //("Yahoo punt returners set: ").Dump();

        //returner.Dump();
        returners.Add(returner);
        if(returners.Count % 5 == 0) {
            returners.Count.Dump();
        }        
    }
}

//returners.Dump();

"Writing returner File:".Dump();

new PrintReturnerService(returners).WriteReturnerFile();
"Writing returner File COMPLETE".Dump();

public class Returner
{
    public Returner()
    {
        this.YahooPrimaryKickReturner = string.Empty;
        this.YahooSecondaryKickReturner = string.Empty;
        this.YahooTertiaryKickReturner = string.Empty;
        this.YahooPrimaryPuntReturner = string.Empty;
        this.YahooSecondaryPuntReturner = string.Empty;
        this.YahooTertiaryPuntReturner = string.Empty;
        this.EspnPrimaryKickReturner = string.Empty;
        this.EspnSecondaryKickReturner = string.Empty;
        this.EspnTertiaryKickReturner = string.Empty;
        this.EspnPrimaryPuntReturner = string.Empty;
        this.EspnSecondaryPuntReturner = string.Empty;
        this.EspnTertiaryPuntReturner = string.Empty;
    }

    public string YahooPrimaryKickReturner { get; set; }
    public string YahooSecondaryKickReturner { get; set; }
    public string YahooTertiaryKickReturner { get; set; }

    public string YahooPrimaryPuntReturner { get; set; }
    public string YahooSecondaryPuntReturner { get; set; }
    public string YahooTertiaryPuntReturner { get; set; }

    public string EspnPrimaryKickReturner { get; set; }
    public string EspnSecondaryKickReturner { get; set; }
    public string EspnTertiaryKickReturner { get; set; }

    public string EspnPrimaryPuntReturner { get; set; }
    public string EspnSecondaryPuntReturner { get; set; }
    public string EspnTertiaryPuntReturner { get; set; }

    public string Team { get; set; }
    //returner domain
    public bool InCommonBothPrimary => this.InCommonPrimaryKickReturners && this.InCommonPrimaryPuntReturners;
    public bool InCommonAndSamePlayerPrimary => this.InCommonPrimaryKickReturners && this.InCommonPrimaryPuntReturners && EspnPrimaryKickReturner != null && EspnPrimaryPuntReturner != null && EspnPrimaryKickReturner == EspnPrimaryPuntReturner;
    public bool InCommonKickReturners => this.InCommonPrimaryKickReturners && this.InCommonSecondaryKickReturners && InCommonTertiaryKickReturners;
    public bool InCommonPuntReturners => this.InCommonPrimaryPuntReturners && this.InCommonSecondaryPuntReturners && InCommonTertiaryPuntReturners;

    public bool InCommonPrimaryKickReturners => (YahooPrimaryKickReturner == null && EspnPrimaryKickReturner == null) || (YahooPrimaryKickReturner != null && EspnPrimaryKickReturner != null && YahooPrimaryKickReturner.Contains(EspnPrimaryKickReturner.Split(' ').Last()));
    public bool InCommonSecondaryKickReturners => (YahooSecondaryKickReturner == null && EspnSecondaryKickReturner == null) || (YahooSecondaryKickReturner != null && EspnSecondaryKickReturner != null && YahooSecondaryKickReturner.Contains(EspnSecondaryKickReturner.Split(' ').Last()));
    public bool InCommonTertiaryKickReturners => (YahooTertiaryKickReturner == null && EspnTertiaryKickReturner == null) || (YahooTertiaryKickReturner != null && EspnTertiaryKickReturner != null && YahooTertiaryKickReturner.Contains(EspnTertiaryKickReturner.Split(' ').Last()));

    public bool InCommonPrimaryPuntReturners => (YahooPrimaryPuntReturner == null && EspnPrimaryPuntReturner == null) || (YahooPrimaryPuntReturner != null && EspnPrimaryPuntReturner != null && YahooPrimaryPuntReturner.Contains(EspnPrimaryPuntReturner.Split(' ').Last()));
    public bool InCommonSecondaryPuntReturners => (YahooSecondaryPuntReturner == null && EspnSecondaryPuntReturner == null) || (YahooSecondaryPuntReturner != null && EspnSecondaryPuntReturner != null && YahooSecondaryPuntReturner.Contains(EspnSecondaryPuntReturner.Split(' ').Last()));
    public bool InCommonTertiaryPuntReturners => (YahooTertiaryPuntReturner == null && EspnTertiaryPuntReturner == null) || (YahooTertiaryPuntReturner != null && EspnTertiaryPuntReturner != null && YahooTertiaryPuntReturner.Contains(EspnTertiaryPuntReturner.Split(' ').Last()));
}

public class SiteCode
{
    public SiteCode(string teamCode, string espnCode, string yahooCode)
    {
        this.TeamCode = teamCode;
        this.EspnCode = espnCode;
        this.YahooCode = yahooCode;
    }
    public string TeamCode { get; set; }
    public string EspnCode { get; set; }
    public string YahooCode { get; set; }
}

public class PrintReturnerService
{
    private List<Returner> _returners;
    private string returnerFile = @"E:\Dropbox\Private\Fantasy\Football\2018\ReturnersGenerated.txt";
    public PrintReturnerService(List<Returner> returners)
    {
        _returners = returners;
    }

    public void WriteReturnerFile()
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(returnerFile))
        {
            PrintHeader(file);

            foreach (var returner in _returners)
            {
                PrintLine(file, returner);
            }
        }
    }

    private void PrintLine(System.IO.StreamWriter file, Returner returner)
    {
        file.Write(returner.Team);
        for (int i = 0; i < (6 - (int)(returner.Team.ToArray().Count() / 4)); i++)
            file.Write("\t");

        file.Write(returner.YahooPrimaryKickReturner);
        for (int i = 0; i < (6 - (int)(returner.YahooPrimaryKickReturner.ToArray().Count() / 4)); i++)
            file.Write("\t");

        file.Write(returner.YahooPrimaryPuntReturner);
        for (int i = 0; i < (8 - (int)(returner.YahooPrimaryPuntReturner.ToArray().Count() / 4)); i++)
            file.Write("\t");

        file.Write(returner.EspnPrimaryKickReturner);
        for (int i = 0; i < (8 - (int)(returner.EspnPrimaryKickReturner.ToArray().Count() / 4)); i++)
            file.Write("\t");

        file.Write(returner.EspnPrimaryPuntReturner);
        for (int i = 0; i < (7 - (int)(returner.EspnPrimaryPuntReturner.ToArray().Count() / 4)); i++)
            file.Write("\t");

        if (returner.InCommonBothPrimary)
        {
            file.Write("***\t\t");
        }

        //"Player".Dump();
        //returner.EspnPrimaryKickReturner.Dump();
        //"Split Player".Dump();
        //returner.EspnPrimaryKickReturner.Split(' ').Last().Dump();        
        //"Is Set".Dump();
        //returner.YahooPrimaryKickReturner.Contains(returner.EspnPrimaryKickReturner.Split(' ').Last()).Dump();
        //"Is Set Real".Dump();
        //returner.InCommonKickReturners.Dump();
        //returner.Dump();

        if (returner.InCommonAndSamePlayerPrimary)
        {
            file.Write("***");
        }
        file.WriteLine();
    }

    private void PrintHeader(System.IO.StreamWriter file)
    {
        file.WriteLine("\t\t\t\t\t\tYahoo\t\t\t\t\t\t\t\t\t\t\t\t\tESPN");
        file.WriteLine("TEAM\t\t\t\t\tKR\t\t\t\t\t\tPR\t\t\t\t\t\t\t\tKR\t\t\t\t\t\t\t\tPR");
        file.WriteLine("\t\t\t\t\t\t--------------------\t-------------------------\t\t------------------------------\t-------------------------");
    }

}
}
