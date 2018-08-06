using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RML.Teams;
using RML.Weeks;

namespace RML.Trophies
{
    public class BottomRankedSeasonTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.Number12Ranked;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            return $"For finishing the season in last place!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
