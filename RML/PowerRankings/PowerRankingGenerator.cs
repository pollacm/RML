using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.PowerRankings
{
    public class PowerRankingGenerator
    {
        private readonly List<Score>[] _scores;
        private readonly int _currentWeek;

        public PowerRankingGenerator(List<Score>[] scores, int currentWeek)
        {
            _scores = scores;
            _currentWeek = currentWeek;
        }

        public List<PowerRanking> GeneratePowerRankings()
        {
            var currentPowerRankings = new Dictionary<string, PowerRanking>();
            var powerRankings = new List<PowerRanking>();
            //previous power rankings
            for (int i = _currentWeek - 4; i < _currentWeek - 1; i++)
            {
                foreach (var score in _scores[i])
                {
                    //HomeTeam
                    if (!currentPowerRankings.ContainsKey(score.HomeTeam.TeamName))
                    {
                        var powerRanking = new PowerRanking();
                        powerRanking.TeamName = score.HomeTeam.TeamName;
                        powerRanking.TeamAbbreviation = score.HomeTeam.TeamAbbreviation;
                        powerRanking.PreviousTotal = score.HomeTeam.GetPointsForWeek;
                        currentPowerRankings[score.HomeTeam.TeamName] = powerRanking;
                    }
                    else
                    {
                        var powerRanking = currentPowerRankings[score.HomeTeam.TeamName];
                        powerRanking.TeamName = score.HomeTeam.TeamName;
                        powerRanking.TeamAbbreviation = score.HomeTeam.TeamAbbreviation;
                        powerRanking.PreviousTotal = score.HomeTeam.GetPointsForWeek;
                        currentPowerRankings[score.HomeTeam.TeamName] = powerRanking;
                    }

                    //AwayTeam
                    if (!currentPowerRankings.ContainsKey(score.AwayTeam.TeamName))
                    {
                        var powerRanking = new PowerRanking();
                        powerRanking.TeamName = score.AwayTeam.TeamName;
                        powerRanking.TeamAbbreviation = score.AwayTeam.TeamAbbreviation;
                        powerRanking.PreviousTotal = score.AwayTeam.GetPointsForWeek;
                        currentPowerRankings[score.AwayTeam.TeamName] = powerRanking;
                    }
                    else
                    {
                        var powerRanking = currentPowerRankings[score.AwayTeam.TeamName];
                        powerRanking.TeamName = score.AwayTeam.TeamName;
                        powerRanking.TeamAbbreviation = score.AwayTeam.TeamAbbreviation;
                        powerRanking.PreviousTotal += score.AwayTeam.GetPointsForWeek;
                        currentPowerRankings[score.AwayTeam.TeamName] = powerRanking;
                    }
                }
            }

            //current power rankings
            for (int i = _currentWeek - 4; i < _currentWeek - 1; i++)
            {
                foreach (var score in _scores[i])
                {
                    //HomeTeam
                    var powerRanking = currentPowerRankings[score.HomeTeam.TeamName];
                    powerRanking.CurrentTotal += score.HomeTeam.GetPointsForWeek;
                    currentPowerRankings[score.HomeTeam.TeamName] = powerRanking;

                    //AwayTeam
                    powerRanking = currentPowerRankings[score.AwayTeam.TeamName];
                    powerRanking.CurrentTotal += score.AwayTeam.GetPointsForWeek;
                    currentPowerRankings[score.AwayTeam.TeamName] = powerRanking;
                }
            }

            var orderedPreviousTotalList = currentPowerRankings.OrderByDescending(p => p.Value.PreviousTotal);
            var point = 0m;
            var rank = 0;
            foreach (var item in orderedPreviousTotalList)
            {
                if(point != item.Value.PreviousTotal)
                {
                    rank++;                    
                }

                item.Value.PreviousPowerRanking = rank;
            }

            var orderedCurrentTotalList = currentPowerRankings.OrderByDescending(p => p.Value.CurrentTotal);
            var rankedList = new PowerRanking();
            point = 0m;
            rank = 0;
            foreach (var item in orderedCurrentTotalList)
            {
                if (point != item.Value.CurrentTotal)
                {
                    rank++;
                }

                item.Value.CurrentPowerRanking = rank;
                powerRankings.Add(item.Value);

            }

            return powerRankings;
        }
    }
}
