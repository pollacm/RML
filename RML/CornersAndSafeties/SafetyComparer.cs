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

        public SiteCorner CornerInRmlIsSafety(RmlCorner rmlCorner)
        {
            return _siteCorners.SingleOrDefault(c => c.EspnPrimaryFreeSafety == rmlCorner.Name || c.EspnPrimaryStrongSafety == rmlCorner.Name ||
                                              c.EspnSecondaryFreeSafety == rmlCorner.Name || c.EspnSecondaryStrongSafety == rmlCorner.Name ||
                                              c.EspnTertiaryFreeSafety == rmlCorner.Name || c.EspnTertiaryStrongSafety == rmlCorner.Name ||
                                              c.YahooPrimaryFreeSafety == rmlCorner.Name || c.YahooPrimaryStrongSafety == rmlCorner.Name ||
                                              c.YahooSecondaryFreeSafety == rmlCorner.Name || c.YahooSecondaryStrongSafety == rmlCorner.Name ||
                                              c.YahooTertiaryFreeSafety == rmlCorner.Name || c.YahooTertiaryStrongSafety == rmlCorner.Name);
        }

        public RmlCorner.PositionEnum GetPosition(RmlCorner rmlCorner, SiteCorner siteCorner)
        {
            if (siteCorner.EspnPrimaryFreeSafety == rmlCorner.Name ||
                siteCorner.EspnSecondaryFreeSafety == rmlCorner.Name ||
                siteCorner.EspnTertiaryFreeSafety == rmlCorner.Name ||
                siteCorner.YahooPrimaryFreeSafety == rmlCorner.Name ||
                siteCorner.YahooPrimaryFreeSafety == rmlCorner.Name ||
                siteCorner.YahooPrimaryFreeSafety == rmlCorner.Name)
                return RmlCorner.PositionEnum.FR;
            return RmlCorner.PositionEnum.SS;
        }

        public RmlCorner.DepthChartEnum GetDepthChartSpot(RmlCorner rmlCorner, SiteCorner siteCorner)
        {
            if (siteCorner.EspnPrimaryFreeSafety == rmlCorner.Name ||
                siteCorner.YahooPrimaryFreeSafety == rmlCorner.Name ||
                siteCorner.EspnPrimaryStrongSafety == rmlCorner.Name ||
                siteCorner.YahooPrimaryStrongSafety == rmlCorner.Name)
            {
                return RmlCorner.DepthChartEnum.Starter;
            }

            if (siteCorner.EspnSecondaryFreeSafety == rmlCorner.Name ||
                siteCorner.YahooSecondaryFreeSafety == rmlCorner.Name ||
                siteCorner.EspnSecondaryStrongSafety == rmlCorner.Name ||
                siteCorner.YahooSecondaryStrongSafety == rmlCorner.Name)
            {
                return RmlCorner.DepthChartEnum.Secondary;
            }

            return RmlCorner.DepthChartEnum.Tertiary;
        }
    }
}
