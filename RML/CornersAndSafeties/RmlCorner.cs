using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.CornersAndSafeties
{
    public class RmlCorner
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public int PreviousRank { get; set; }
        public decimal PreviousPoints { get; set; }
        public decimal PreviousAverage { get; set; }
        public PositionEnum Position { get; set; }
        public DepthChartEnum DepthChart { get; set; }
        public bool Matches { get; set; }

        public enum PositionEnum
        {
            FR = 0,
            SS = 1
        }

        public enum DepthChartEnum
        {
            Starter = 0,
            Secondary = 1,
            Tertiary = 2
        }
    }
}
