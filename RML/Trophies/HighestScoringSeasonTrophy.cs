using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RML.Standings;
using RML.Teams;
using RML.Weeks;

namespace RML.Trophies
{
    public class HighestScoringSeasonTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.HighestScoringTeamOfTheYear;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            var standing = JsonConvert.DeserializeObject<Standing>(additionalInfo);
            return $"For putting up a total of {standing.PointsFor} throughout the season!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
