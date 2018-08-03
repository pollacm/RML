namespace RML.PlayerComparer
{
    public class RmlPlayer
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public int PreviousRank { get; set; }
        public decimal PreviousPoints { get; set; }
        public decimal PreviousAverage { get; set; }
        public PositionEnum Position { get; set; }
        public PlayerConstants.DepthChartEnum DepthChart { get; set; }
        public bool Matches { get; set; }

        public enum PositionEnum
        {
            FR = 0,
            SS = 1
        }
    }
}
