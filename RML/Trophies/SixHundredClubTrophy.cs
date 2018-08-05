using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RML.Teams;

namespace RML.Trophies
{
    public class SixHundredClubTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.Da600Club;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            return $"For putting up {team.TeamPoints} points!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
