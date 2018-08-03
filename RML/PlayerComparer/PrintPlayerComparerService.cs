using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RML.PlayerComparer
{
    public class PrintPlayerComparerService
    {
        private readonly List<SitePlayer> _sitePlayers;

        private string returnerFile = @"E:\Dropbox\Private\Fantasy\RML\2018\PlayerComparerGenerated.txt";
        private readonly List<RmlPlayer> _rmlPlayers;

        public PrintPlayerComparerService(List<SitePlayer> sitePlayers, List<RmlPlayer> rmlPlayers)
        {
            _rmlPlayers = rmlPlayers;
            _sitePlayers = sitePlayers;
        }

        public void WritePlayerComparerFile()
        {
            using (StreamWriter file = new StreamWriter(returnerFile))
            {
                PrintHeader(file);
                foreach (var rmlPlayer in _rmlPlayers)
                {
                    var sitePlayers = _sitePlayers.Where(c => c.Name == rmlPlayer.Name);
                    if (sitePlayers.Any())
                    {
                        foreach (var sitePlayer in sitePlayers)
                        {
                            PrintLine(file, rmlPlayer, sitePlayer);
                        }
                    }
                }
            }
        }

        private void PrintLine(StreamWriter file, RmlPlayer rmlPlayer, SitePlayer sitePlayer)
        {
            file.Write(rmlPlayer.Team);
            for (int i = 0; i < (4 - (int)(rmlPlayer.Team.ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlPlayer.Name);
            for (int i = 0; i < (7 - (int)(rmlPlayer.Name.ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlPlayer.Position);
            for (int i = 0; i < (3 - (int)(rmlPlayer.Position.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(sitePlayer.DepthChart);
            for (int i = 0; i < (3 - (int)(sitePlayer.DepthChart.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(sitePlayer.Site);
            for (int i = 0; i < (3 - (int)(sitePlayer.Site.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlPlayer.PreviousRank);
            for (int i = 0; i < (3 - (int)(rmlPlayer.PreviousRank.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlPlayer.PreviousAverage);
            for (int i = 0; i < (8 - (int)(rmlPlayer.PreviousAverage.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.WriteLine();
        }

        private void PrintHeader(StreamWriter file)
        {
            file.WriteLine("TEAM\t\t\tNAME\t\t\t\t\t\tPOSITION\tDEPTH\t\tSITE\t\tRANK\t\tPOINTS");
        }
    }
}