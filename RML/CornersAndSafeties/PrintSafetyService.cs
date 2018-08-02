using System.Collections.Generic;
using System.IO;
using System.Linq;
using RML.Returners;

namespace RML.CornersAndSafeties
{
    public class PrintSafetyService
    {
        private readonly List<SiteCorner> _siteCorners;
        private readonly SafetyComparer _safetyComparer;

        private string returnerFile = @"E:\Dropbox\Private\Fantasy\RML\2018\SafetiesGenerated2.txt";
        private readonly List<RmlCorner> _rmlCorners;

        public PrintSafetyService(List<SiteCorner> siteCorners, List<RmlCorner> rmlCorners)
        {
            _safetyComparer = new SafetyComparer(siteCorners);
            _rmlCorners = rmlCorners;
            _siteCorners = siteCorners;
        }

        public void WriteSafetyFile()
        {
            using (StreamWriter file = new StreamWriter(returnerFile))
            {
                PrintHeader(file);
                foreach (var rmlCorner in _rmlCorners)
                {
                    if (_siteCorners.Any(c => c.EspnPrimaryFreeSafety == rmlCorner.Name || c.EspnPrimaryStrongSafety == rmlCorner.Name ||
                                              c.EspnSecondaryFreeSafety == rmlCorner.Name || c.EspnSecondaryStrongSafety == rmlCorner.Name ||
                                              c.EspnTertiaryFreeSafety == rmlCorner.Name || c.EspnTertiaryStrongSafety == rmlCorner.Name ||
                                              c.YahooPrimaryFreeSafety == rmlCorner.Name || c.YahooPrimaryStrongSafety == rmlCorner.Name ||
                                              c.YahooSecondaryFreeSafety == rmlCorner.Name || c.YahooSecondaryStrongSafety == rmlCorner.Name ||
                                              c.YahooTertiaryFreeSafety == rmlCorner.Name || c.YahooTertiaryStrongSafety == rmlCorner.Name))
                    {
                        var siteCorner = _safetyComparer.CornerInRmlIsSafety(rmlCorner);
                        if (siteCorner != null)
                        {
                            PrintLine(file, rmlCorner, siteCorner);
                        }
                    }
                    //var siteCorner = _siteCorners.First();
                    //PrintLine(file, rmlCorner, siteCorner);
                }
            }
        }

        private void PrintLine(StreamWriter file, RmlCorner rmlCorner, SiteCorner siteCorner)
        {
            file.Write(rmlCorner.Team);
            for (int i = 0; i < (4 - (int)(rmlCorner.Team.ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlCorner.Name);
            for (int i = 0; i < (7 - (int)(rmlCorner.Name.ToArray().Count() / 4)); i++)
                file.Write("\t");

            RmlCorner.PositionEnum position = _safetyComparer.GetPosition(rmlCorner, siteCorner);
            file.Write(position);
            for (int i = 0; i < (3 - (int)(position.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            RmlCorner.DepthChartEnum depthChart = _safetyComparer.GetDepthChartSpot(rmlCorner, siteCorner);
            file.Write(depthChart);
            for (int i = 0; i < (3 - (int)(depthChart.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlCorner.PreviousRank);
            for (int i = 0; i < (3 - (int)(rmlCorner.PreviousRank.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlCorner.PreviousAverage);
            for (int i = 0; i < (8 - (int)(rmlCorner.PreviousAverage.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.WriteLine();
        }

        private void PrintHeader(StreamWriter file)
        {
            file.WriteLine("TEAM\t\t\tNAME\t\t\t\t\t\tPOSITION\tDEPTH\t\tRANK\t\tPOINTS");
        }
    }
}