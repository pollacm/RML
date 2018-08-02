using System.Collections.Generic;
using System.Linq;

namespace RML.PlayerComparer
{
    public class PlayerComparerHelper
    {
        private readonly List<SitePlayer> _sitePlayers;

        public PlayerComparerHelper(List<SitePlayer> sitePlayers)
        {
            _sitePlayers = sitePlayers;
        }

        public SitePlayer PlayerInUpgradedPosition(RmlPlayer rmlPlayer)
        {
            return _sitePlayers.SingleOrDefault(c => c.EspnPrimaryFreeSafety == rmlPlayer.Name || c.EspnPrimaryStrongSafety == rmlPlayer.Name ||
                                              c.EspnSecondaryFreeSafety == rmlPlayer.Name || c.EspnSecondaryStrongSafety == rmlPlayer.Name ||
                                              c.EspnTertiaryFreeSafety == rmlPlayer.Name || c.EspnTertiaryStrongSafety == rmlPlayer.Name ||
                                              c.YahooPrimaryFreeSafety == rmlPlayer.Name || c.YahooPrimaryStrongSafety == rmlPlayer.Name ||
                                              c.YahooSecondaryFreeSafety == rmlPlayer.Name || c.YahooSecondaryStrongSafety == rmlPlayer.Name ||
                                              c.YahooTertiaryFreeSafety == rmlPlayer.Name || c.YahooTertiaryStrongSafety == rmlPlayer.Name);
        }

        public RmlPlayer.PositionEnum GetPosition(RmlPlayer rmlPlayer, SitePlayer sitePlayer)
        {
            if (sitePlayer.EspnPrimaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.EspnSecondaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.EspnTertiaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.YahooPrimaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.YahooPrimaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.YahooPrimaryFreeSafety == rmlPlayer.Name)
                return RmlPlayer.PositionEnum.FR;
            return RmlPlayer.PositionEnum.SS;
        }

        public RmlPlayer.DepthChartEnum GetDepthChartSpot(RmlPlayer rmlPlayer, SitePlayer sitePlayer)
        {
            if (sitePlayer.EspnPrimaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.YahooPrimaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.EspnPrimaryStrongSafety == rmlPlayer.Name ||
                sitePlayer.YahooPrimaryStrongSafety == rmlPlayer.Name)
            {
                return RmlPlayer.DepthChartEnum.Starter;
            }

            if (sitePlayer.EspnSecondaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.YahooSecondaryFreeSafety == rmlPlayer.Name ||
                sitePlayer.EspnSecondaryStrongSafety == rmlPlayer.Name ||
                sitePlayer.YahooSecondaryStrongSafety == rmlPlayer.Name)
            {
                return RmlPlayer.DepthChartEnum.Secondary;
            }

            return RmlPlayer.DepthChartEnum.Tertiary;
        }
    }
}
