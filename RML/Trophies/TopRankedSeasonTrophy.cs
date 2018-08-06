using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RML.Teams;
using RML.Weeks;

namespace RML.Trophies
{
    public class TopRankedSeasonTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.Number1Seed;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            return $"For finishing the season ranked #1!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
