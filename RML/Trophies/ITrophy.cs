using RML.Teams;

namespace RML.Trophies
{
    public interface ITrophy
    {
        string GetHeadline(Team team, string additionalInfo);
        string GetReason(Team team, string additionalInfo);
        string GetTrophyName();
    }
}