using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RML.PlayerComparer
{
    public class PrintPlayerComparerService
    {
        private readonly List<SitePlayer> _sitePlayers;
        private readonly PlayerComparerHelper _playerComparerHelper;

        private string returnerFile = @"E:\Dropbox\Private\Fantasy\RML\2018\SafetiesGenerated2.txt";
        private readonly List<RmlPlayer> _rmlPlayers;

        public PrintPlayerComparerService(List<SitePlayer> sitePlayers, List<RmlPlayer> rmlPlayers)
        {
            _playerComparerHelper = new PlayerComparerHelper(sitePlayers);
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
                    if (_sitePlayers.Any(c => c.EspnPrimaryFreeSafety == rmlPlayer.Name || c.EspnPrimaryStrongSafety == rmlPlayer.Name ||
                                              c.EspnSecondaryFreeSafety == rmlPlayer.Name || c.EspnSecondaryStrongSafety == rmlPlayer.Name ||
                                              c.EspnTertiaryFreeSafety == rmlPlayer.Name || c.EspnTertiaryStrongSafety == rmlPlayer.Name ||
                                              c.YahooPrimaryFreeSafety == rmlPlayer.Name || c.YahooPrimaryStrongSafety == rmlPlayer.Name ||
                                              c.YahooSecondaryFreeSafety == rmlPlayer.Name || c.YahooSecondaryStrongSafety == rmlPlayer.Name ||
                                              c.YahooTertiaryFreeSafety == rmlPlayer.Name || c.YahooTertiaryStrongSafety == rmlPlayer.Name))
                    {
                        var sitePlayer = _playerComparerHelper.PlayerInUpgradedPosition(rmlPlayer);
                        if (sitePlayer != null)
                        {
                            PrintLine(file, rmlPlayer, sitePlayer);
                        }
                    }
                    //var sitePlayer = _sitePlayers.First();
                    //PrintLine(file, RmlPlayer, sitePlayer);
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

            RmlPlayer.PositionEnum position = _playerComparerHelper.GetPosition(rmlPlayer, sitePlayer);
            file.Write(position);
            for (int i = 0; i < (3 - (int)(position.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            RmlPlayer.DepthChartEnum depthChart = _playerComparerHelper.GetDepthChartSpot(rmlPlayer, sitePlayer);
            file.Write(depthChart);
            for (int i = 0; i < (3 - (int)(depthChart.ToString().ToArray().Count() / 4)); i++)
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
            file.WriteLine("TEAM\t\t\tNAME\t\t\t\t\t\tPOSITION\tDEPTH\t\tRANK\t\tPOINTS");
        }
    }
}