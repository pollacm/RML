using Newtonsoft.Json;
using RML.Teams;

namespace RML.Trophies
{
    public class OffensivePlayerOfTheYearTrophy : ITrophy
    {
        public OffensivePlayerOfTheYearTrophy(Team team, string additionalInfo)
        {
            Team = team;
            AdditionalInfo = additionalInfo;
            BuildTogether = false;
        }
        public Team Team { get; set; }
        public string AdditionalInfo { get; }
        public bool BuildTogether { get; }

        public string GetTrophyName()
        {
            return TrophyConstants.OffensivePlayerOfTheYear;
        }

        public string LeaguePageText()
        {
            return "THE " + GetTrophyName().ToUpper() + @" AWARD GOES TO...";
        }

        public string GetTrophyBody()
        {
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(AdditionalInfo);
            return $"[player#{op.PlayerId}]{op.Name.ToUpper()}[/player] ({op.Team.ToUpper()} - {op.Points} POINTS!!!!!)" + @"

                [image]<update>[/image]";
        }

        public string GetHeadline(Team team)
        {
            var op = JsonConvert.DeserializeObject<PlayerOfTheWeek>(AdditionalInfo);
            return $"{op.Name.ToUpper()} - {op.Points} POINTS!!!!!";
        }

        public string GetReason(Team team)
        {
            return string.Empty;
        }
    }
}