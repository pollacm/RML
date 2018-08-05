using RML.Teams;

namespace RML.Trophies
{
    public class LoserOfTheWeekTrophy : ITrophy
    {
        public string GetTrophyName()
        {
            return TrophyConstants.LargestMarginWeekLoser;
        }

        public string GetHeadline(Team team, string additionalInfo)
        {
            return $"For losing by {additionalInfo} points!!!!!";
        }

        public string GetReason(Team team, string additionalInfo)
        {
            return string.Empty;
        }
    }
}