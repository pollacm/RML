using System.Linq;

namespace RML.CornersAndSafeties
{
    public class SiteCorner
    {
        public SiteCorner()
        {
            this.YahooPrimaryStrongSafety = string.Empty;
            this.YahooSecondaryStrongSafety = string.Empty;
            this.YahooTertiaryStrongSafety = string.Empty;
            this.YahooPrimaryFreeSafety = string.Empty;
            this.YahooSecondaryFreeSafety = string.Empty;
            this.YahooTertiaryFreeSafety = string.Empty;
            this.EspnPrimaryStrongSafety = string.Empty;
            this.EspnSecondaryStrongSafety = string.Empty;
            this.EspnTertiaryStrongSafety = string.Empty;
            this.EspnPrimaryFreeSafety = string.Empty;
            this.EspnSecondaryFreeSafety = string.Empty;
            this.EspnTertiaryFreeSafety = string.Empty;
        }

        public string YahooPrimaryStrongSafety { get; set; }
        public string YahooSecondaryStrongSafety { get; set; }
        public string YahooTertiaryStrongSafety { get; set; }
        public string YahooPrimaryFreeSafety { get; set; }
        public string YahooSecondaryFreeSafety { get; set; }
        public string YahooTertiaryFreeSafety { get; set; }

        public string EspnPrimaryStrongSafety { get; set; }
        public string EspnSecondaryStrongSafety { get; set; }
        public string EspnTertiaryStrongSafety { get; set; }
        public string EspnPrimaryFreeSafety { get; set; }
        public string EspnSecondaryFreeSafety { get; set; }
        public string EspnTertiaryFreeSafety { get; set; }

        public string Team { get; set; }

    }
}