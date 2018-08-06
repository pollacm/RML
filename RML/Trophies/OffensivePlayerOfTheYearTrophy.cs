﻿using Newtonsoft.Json;
using RML.Teams;

namespace RML.Trophies
{
    public class OffensivePlayerOfTheYearTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.OffensivePlayerOfTheYear;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(additionalInfo);
            return $"{op.Name.ToUpper()} - {op.Points} POINTS!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}