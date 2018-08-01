using System.Linq;

namespace RML.CornersAndSafeties
{
    public class SiteCorner
    {
        public SiteCorner()
        {
            this.YahooPrimarySafety = string.Empty;
            this.YahooSecondarySafety = string.Empty;
            this.YahooTertiarySafety = string.Empty;
            this.EspnPrimarySafety = string.Empty;
            this.EspnSecondarySafety = string.Empty;
            this.EspnTertiarySafety = string.Empty;
        }

        public string YahooPrimarySafety { get; set; }
        public string YahooSecondarySafety { get; set; }
        public string YahooTertiarySafety { get; set; }

        public string EspnPrimarySafety { get; set; }
        public string EspnSecondarySafety { get; set; }
        public string EspnTertiarySafety { get; set; }

        public string Team { get; set; }

        //returner domain
        public bool InCommonBothPrimary => this.InCommonPrimarySafeties;

        public bool InCommonSafties => this.InCommonPrimarySafeties && this.InCommonSecondaryKickReturners && InCommonTertiaryKickReturners;
        
        public bool InCommonPrimarySafeties => (YahooPrimarySafety == null && EspnPrimarySafety == null) ||
                                                    (YahooPrimarySafety != null && EspnPrimarySafety != null && YahooPrimarySafety == EspnPrimarySafety);

        public bool InCommonSecondaryKickReturners => (YahooSecondarySafety == null && EspnSecondarySafety == null) ||
                                                      (YahooSecondarySafety != null && EspnSecondarySafety != null && YahooSecondarySafety == EspnSecondarySafety);

        public bool InCommonTertiaryKickReturners => (YahooTertiarySafety == null && EspnTertiarySafety == null) ||
                                                     (YahooTertiarySafety != null && EspnTertiarySafety != null && YahooTertiarySafety ==EspnTertiarySafety);

    }
}