using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RML.Trophies
{
    public class Trophies
    {
        public string Da500Club = "Da 500 Club";
        public string Da600Club = "Da 600 Club";
        public string Da700Club = "Da 700 Club";
        public string LargestMarginWeekWinner = "Rake It Up!";
        public string LargestMarginWeekLoser = "Meek Millz!!!";
        public string OffensivePlayerOfTheWeek = "Offensive Player of the Week Award!!!!";
        public string DefensivePlayerOfTheWeek = "Defensive Player of the Week Award!!!!";
        public string Number12Ranked = "Meek Millz of the Year Award!!!";
        public string Number1Seed = "MASK OFF!!!";
        public string OffensivePlayerOfTheYear = "R.M.L. Offensive MVP";
        public string DefensivePlayerOfTheYear = "R.M.L. Defensive MVP";
        public string HighestScoringTeamOfTheYear = "Secure The Bag!!!!";
        public string AfcChamp = "AFC Division Championship Banner";
        public string NfcChamp = "NFC Division Championship Banner";
        public string Champion = "R.M.L. Championship Trophy";

        public List<Trophy> TrophyList;

        public Trophies()
        {
            TrophyList = new List<Trophy>
            {
                new Trophy
                {
                    TrophyName = Da500Club
                },
                new Trophy
                {
                    TrophyName = Da600Club
                },
                new Trophy
                {
                    TrophyName = Da700Club
                },
                new Trophy
                {
                    TrophyName = LargestMarginWeekWinner
                },
                new Trophy
                {
                    TrophyName = LargestMarginWeekLoser
                },
                new Trophy
                {
                    TrophyName = OffensivePlayerOfTheWeek
                },

                new Trophy
                {
                    TrophyName = DefensivePlayerOfTheWeek
                },
                new Trophy
                {
                    TrophyName = Number12Ranked
                },
                new Trophy
                {
                    TrophyName = Number1Seed
                },
                new Trophy
                {
                    TrophyName = OffensivePlayerOfTheYear
                },
                new Trophy
                {
                    TrophyName = DefensivePlayerOfTheYear
                },
                new Trophy
                {
                    TrophyName = HighestScoringTeamOfTheYear
                },
                new Trophy
                {
                    TrophyName = AfcChamp
                },
                new Trophy
                {
                    TrophyName = NfcChamp
                },
                new Trophy
                {
                    TrophyName = Champion
                },
            }; ;
        }
    }
}
