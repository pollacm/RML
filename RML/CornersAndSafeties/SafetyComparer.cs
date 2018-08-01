using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.CornersAndSafeties
{
    public class SafetyComparer
    {
        private readonly List<RmlCorner> _corners;

        public SafetyComparer(List<RmlCorner> corners)
        {
            _corners = corners;
        }

        public bool CornerInRmlIsSafety(SiteCorner siteCorner)
        {
            return true;
        }
    }
}
