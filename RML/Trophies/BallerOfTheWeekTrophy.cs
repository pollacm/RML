using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RML.Teams;
using RML.Weeks;

namespace RML.Trophies
{
    public class BallerOfTheWeekTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.LargestMarginWeekWinner;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            return $"For winning by {additionalInfo} points!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
