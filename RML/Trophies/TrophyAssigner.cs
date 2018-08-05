using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RML.Teams;
using RML.Weeks;

namespace RML.Trophies
{
    public class TrophyAssigner
    {
        private readonly ChromeDriver _driver;
        private readonly int _year;

        public TrophyAssigner(ChromeDriver driver, int year)
        {
            _driver = driver;
            _year = year;
        }
        public Trophy AssignTrophy(Week currentWeek, Team team, ITrophy trophyToAssign, string additionalInfo = "")
        {
            var trophy = new Trophy();
            
            _driver.Navigate().GoToUrl($"http://games.espn.com/ffl/trophylist?leagueId=127291");

            var winners = currentWeek.Scores.Where(s => s.AwayTeam.TeamPoints > 500).Select(s => s.AwayTeam).ToList();
            winners.AddRange(currentWeek.Scores.Where(s => s.HomeTeam.TeamPoints > 500).Select(s => s.HomeTeam));

            _driver.FindElement(By.XPath("//table/tbody/tr/td/div/div/center/b[contains(.,'" + trophyToAssign.GetTrophyName() + "')]/parent::center/parent::div/div/a[contains(.,'Assign')]")).Click();
            _driver.WaitUntilElementExists(By.Id("assignTrophyDiv"));

            var dropdown = _driver.FindElement(By.Id("assignTeamId"));
            var dropdownSelect = new SelectElement(dropdown);
            dropdownSelect.SelectByText(team.TeamName);

            _driver.FindElement(By.Name("headline")).SendKeys(trophyToAssign.GetHeadline(team, additionalInfo));
            _driver.FindElement(By.Name("reason")).SendKeys(trophyToAssign.GetReason(team, additionalInfo));

            var showcase = _driver.FindElement(By.Id("isShowcase"));
            var showcaseDropdown = new SelectElement(showcase);
            showcaseDropdown.SelectByText("Yes");

            //_driver.FindElement(By.Name("btnSubmit")).Click();
            //_driver.WaitUntilElementExists(By.ClassName("bodyCopy"));

            trophy.TrophyName = trophyToAssign.GetTrophyName();
            trophy.TeamName = team.TeamName;

            Thread.Sleep(2000);

            return trophy;
        }
    }
}
