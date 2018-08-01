using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.CornersAndSafeties
{
    public class SafetyComparer
    {
        private readonly List<SiteCorner> _siteCorners;

        public SafetyComparer(List<SiteCorner> siteCorners)
        {
            _siteCorners = siteCorners;
        }

        public RmlCorner CornerInRmlIsSafety(RmlCorner rmlCorner)
        {
            var RmlCorner = rmlCorner;
            //TODO: Add match, position, depthchart
            return rmlCorner;
        }
    }
}
