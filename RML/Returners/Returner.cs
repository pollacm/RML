using System.Linq;

namespace RML.Returners
{
    public class Returner
    {
        public Returner()
        {
            this.YahooPrimaryKickReturner = string.Empty;
            this.YahooSecondaryKickReturner = string.Empty;
            this.YahooTertiaryKickReturner = string.Empty;
            this.YahooPrimaryPuntReturner = string.Empty;
            this.YahooSecondaryPuntReturner = string.Empty;
            this.YahooTertiaryPuntReturner = string.Empty;
            this.EspnPrimaryKickReturner = string.Empty;
            this.EspnSecondaryKickReturner = string.Empty;
            this.EspnTertiaryKickReturner = string.Empty;
            this.EspnPrimaryPuntReturner = string.Empty;
            this.EspnSecondaryPuntReturner = string.Empty;
            this.EspnTertiaryPuntReturner = string.Empty;
        }

        public string YahooPrimaryKickReturner { get; set; }
        public string YahooSecondaryKickReturner { get; set; }
        public string YahooTertiaryKickReturner { get; set; }

        public string YahooPrimaryPuntReturner { get; set; }
        public string YahooSecondaryPuntReturner { get; set; }
        public string YahooTertiaryPuntReturner { get; set; }

        public string EspnPrimaryKickReturner { get; set; }
        public string EspnSecondaryKickReturner { get; set; }
        public string EspnTertiaryKickReturner { get; set; }

        public string EspnPrimaryPuntReturner { get; set; }
        public string EspnSecondaryPuntReturner { get; set; }
        public string EspnTertiaryPuntReturner { get; set; }

        public string Team { get; set; }

        //returner domain
        public bool InCommonBothPrimary => this.InCommonPrimaryKickReturners && this.InCommonPrimaryPuntReturners;

        public bool InCommonAndSamePlayerPrimary => this.InCommonPrimaryKickReturners && this.InCommonPrimaryPuntReturners && EspnPrimaryKickReturner != null && EspnPrimaryPuntReturner != null &&
                                                    EspnPrimaryKickReturner == EspnPrimaryPuntReturner;

        public bool InCommonKickReturners => this.InCommonPrimaryKickReturners && this.InCommonSecondaryKickReturners && InCommonTertiaryKickReturners;
        public bool InCommonPuntReturners => this.InCommonPrimaryPuntReturners && this.InCommonSecondaryPuntReturners && InCommonTertiaryPuntReturners;

        public bool InCommonPrimaryKickReturners => (YahooPrimaryKickReturner == null && EspnPrimaryKickReturner == null) ||
                                                    (YahooPrimaryKickReturner != null && EspnPrimaryKickReturner != null && YahooPrimaryKickReturner.Contains(EspnPrimaryKickReturner.Split(' ').Last()));

        public bool InCommonSecondaryKickReturners => (YahooSecondaryKickReturner == null && EspnSecondaryKickReturner == null) ||
                                                      (YahooSecondaryKickReturner != null && EspnSecondaryKickReturner != null && YahooSecondaryKickReturner.Contains(EspnSecondaryKickReturner.Split(' ').Last()));

        public bool InCommonTertiaryKickReturners => (YahooTertiaryKickReturner == null && EspnTertiaryKickReturner == null) ||
                                                     (YahooTertiaryKickReturner != null && EspnTertiaryKickReturner != null && YahooTertiaryKickReturner.Contains(EspnTertiaryKickReturner.Split(' ').Last()));

        public bool InCommonPrimaryPuntReturners => (YahooPrimaryPuntReturner == null && EspnPrimaryPuntReturner == null) ||
                                                    (YahooPrimaryPuntReturner != null && EspnPrimaryPuntReturner != null && YahooPrimaryPuntReturner.Contains(EspnPrimaryPuntReturner.Split(' ').Last()));

        public bool InCommonSecondaryPuntReturners => (YahooSecondaryPuntReturner == null && EspnSecondaryPuntReturner == null) ||
                                                      (YahooSecondaryPuntReturner != null && EspnSecondaryPuntReturner != null && YahooSecondaryPuntReturner.Contains(EspnSecondaryPuntReturner.Split(' ').Last()));

        public bool InCommonTertiaryPuntReturners => (YahooTertiaryPuntReturner == null && EspnTertiaryPuntReturner == null) ||
                                                     (YahooTertiaryPuntReturner != null && EspnTertiaryPuntReturner != null && YahooTertiaryPuntReturner.Contains(EspnTertiaryPuntReturner.Split(' ').Last()));
    }
}