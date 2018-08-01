using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.CornersAndSafeties
{
    public class Corner
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public int PreviousRank { get; set; }
        public decimal PreviousPoints { get; set; }
        public decimal PreviousAverage { get; set; }
    }
}
