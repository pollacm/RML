using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.PlayerComparer
{
    public static class PlayerConstants
    {
        public enum DepthChartEnum
        {
            Starter = 0,
            Secondary = 1,
            Tertiary = 2
        }

        public static Dictionary<string, SitePlayer.PositionEnum> LinebackerPositionsYahoo = new Dictionary<string, SitePlayer.PositionEnum>
        {
            { "Strong Safety", SitePlayer.PositionEnum.SS},
            { "Free Safety", SitePlayer.PositionEnum.FS},
            { "Left LineBacker", SitePlayer.PositionEnum.WLB},
            { "Middle Linebacker", SitePlayer.PositionEnum.MLB},
            { "Right Linebacker", SitePlayer.PositionEnum.SLB},
            { "Left Outside Linebacker", SitePlayer.PositionEnum.OLB},
            { "Right Outside Linebacker", SitePlayer.PositionEnum.OLB},
            { "Left Inside Linebacker", SitePlayer.PositionEnum.ILB},
            { "Right Inside Linebacker", SitePlayer.PositionEnum.ILB}
        };

        public static Dictionary<string, SitePlayer.PositionEnum> LinebackerPositionsEspn = new Dictionary<string, SitePlayer.PositionEnum>
        {
            { "SS", SitePlayer.PositionEnum.SS},
            { "FS", SitePlayer.PositionEnum.FS},
            { "WLB", SitePlayer.PositionEnum.WLB},
            { "MLB", SitePlayer.PositionEnum.MLB},
            { "SLB", SitePlayer.PositionEnum.SLB},
            { "LOLB", SitePlayer.PositionEnum.OLB},
            { "ROLB", SitePlayer.PositionEnum.OLB},
            { "LILB", SitePlayer.PositionEnum.ILB},
            { "RILB", SitePlayer.PositionEnum.ILB}
        };
    }
}
