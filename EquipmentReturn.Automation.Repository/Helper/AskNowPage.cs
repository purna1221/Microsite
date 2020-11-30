using EquipmentReturn.Automation.Accelerators;
using EquipmentReturn.Automation.Repository.Locators;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentReturn.Automation.Repository
{
    public class AskNowPage : TestEngine
    {
        private AskNowLoc _asknowLoc;

        private ActionEngine _act = null;
        private IWebDriver _driver = null;

        public AskNowPage(IWebDriver driver, string strUserType)
        {
            _asknowLoc = new AskNowLoc();
            _act = new ActionEngine(driver);
            _driver = driver;
        }

        public void AsknowLogin()
        {
            _act.waitForVisibilityOfElement(_asknowLoc.Asknowuname, 30);

            if (_act.isElementPresent(_asknowLoc.Asknowuname))
            {

                _act.EnterText(_asknowLoc.Asknowuname, "purna.mutyala@ttec.com", "Asknowuname");

                _act.EnterText(_asknowLoc.Asknowpwd, "Krishanarao@1221", "Asknowpwd");

                _act.click(_asknowLoc.Asknowloginbtn, "Asknowloginbtn");
            }
        }

        public void ClickSysSearch()
        {
            _act.waitForVisibilityOfElement(_asknowLoc.SysParamSearchIcon, 60);
            _act.click(_asknowLoc.SysParamSearchIcon, "SysSearchIcon");
        }

        public void EnterSysSearch(string Reqnumber)
        {
            _act.waitForVisibilityOfElement(_asknowLoc.SysParmSearchInput, 30);
            _act.EnterText(_asknowLoc.SysParmSearchInput, Reqnumber, "SysParmSearchInput");

            _driver.FindElement(_asknowLoc.SysParmSearchInput).SendKeys(Keys.Enter);
        }

        public void ClickRITMNumberLink()
        {
            // _act.waitForVisibilityOfElement(_asknowLoc.RITMNumberLink, 90);
            //IWebElement ele = _driver.FindElement(_asknowLoc.RITMNumberLink);
            //_act.MovetoElement(ele, "RITMNumberLink");
            _act.click(_asknowLoc.RITMNumberLink, "RITMNumberLink");
        }

        public bool VerifyCategoryStatus(string categorystatus)
        {
            bool selectedcategory = _act.Verifydropdownselectedtext(_asknowLoc.CategoryStatus, categorystatus);
            return selectedcategory;
        }

        public void FetchRITMParameters(string ReqNumber, int taskcount = 0, string CategoryStatus = "", string TaskStatus = "")
        {
            try
            {
                string AskNowUrl = "https://asknowdev.service-now.com/";

                ((IJavaScriptExecutor)_driver).ExecuteScript("window.open()");
                _driver.SwitchTo().Window(_driver.WindowHandles[1]);
                _driver.Navigate().GoToUrl(AskNowUrl);

                AsknowLogin();

                ClickSysSearch();

                EnterSysSearch(ReqNumber);

                _act.switchToFrameById("gsft_main");

                ClickRITMNumberLink();

                Assert.IsTrue(VerifyCategoryStatus(CategoryStatus));

                int tasklist = _driver.FindElements(_asknowLoc.TaskLists).Count;

                Assert.IsTrue(tasklist == taskcount);

                _driver.SwitchTo().Window(_driver.WindowHandles[1]).Close();

                _driver.SwitchTo().Window(_driver.WindowHandles[0]);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
