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
    public class NfcDivisionChampionshipTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.NfcChamp;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(additionalInfo);
            return $"For winning the NFC Division!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}
