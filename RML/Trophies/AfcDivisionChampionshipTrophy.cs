using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RML.Teams;
using RML.Weeks;

namespace RML.Trophies
{
    public class AfcDivisionChampionshipTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.AfcChamp;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(additionalInfo);
            return $"For winning the AFC Division!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
