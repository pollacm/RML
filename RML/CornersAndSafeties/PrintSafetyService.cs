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
        }

        public void WriteSafetyFile()
        {
            using (StreamWriter file = new StreamWriter(returnerFile))
            {
                PrintHeader(file);

                foreach (var rmlCorner in _rmlCorners)
                {
                    PrintLine(file, rmlCorner);
                }
            }
        }

        private void PrintLine(StreamWriter file, RmlCorner rmlCorner)
        {
            file.Write(rmlCorner.Team);
            for (int i = 0; i < (6 - (int)(rmlCorner.Team.ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlCorner.Name);
            for (int i = 0; i < (6 - (int)(rmlCorner.Name.ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlCorner.PreviousRank);
            for (int i = 0; i < (8 - (int)(rmlCorner.PreviousRank.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            file.Write(rmlCorner.PreviousAverage);
            for (int i = 0; i < (8 - (int)(rmlCorner.PreviousAverage.ToString().ToArray().Count() / 4)); i++)
                file.Write("\t");

            //TODO: See if there's a safety marked as a corner
            if (_safetyComparer.CornerInRmlIsSafety(rmlCorner).Matches)
            {
                file.Write("***\t\t");
            }

            if (_safetyComparer.CornerInRmlIsSafety(rmlCorner).Matches)
            {
                file.Write("***");
            }

            file.WriteLine();
        }

        private void PrintHeader(StreamWriter file)
        {
            file.WriteLine("\t\t\t\t\t\tYahoo\t\t\t\t\t\t\t\t\t\t\t\t\tESPN");
            file.WriteLine("TEAM\t\t\t\t\tKR\t\t\t\t\t\tPR\t\t\t\t\t\t\t\tKR\t\t\t\t\t\t\t\tPR");
            file.WriteLine("\t\t\t\t\t\t--------------------\t-------------------------\t\t------------------------------\t-------------------------");
        }
    }
}