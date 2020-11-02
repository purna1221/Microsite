using EquipmentReturn.Automation.Accelerators;
using EquipmentReturn.Automation.Repository.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentReturn.Automation.Repository
{
    public class HomeScreen2 : TestEngine
    {
        private IFormScreen2 _homescreen2Loc;

        private ActionEngine _act = null;
        private IWebDriver _driver = null;

        public HomeScreen2(IWebDriver driver, string strUserType)
        {
            if (GetPlatform(driver))
                _homescreen2Loc = new FormScreen2MobileLoc();
            else
                _homescreen2Loc = new FormScreen2DesktopLoc();
            _act = new ActionEngine(driver);
            _driver = driver;
        }

        public bool VerifyThankYouLabel()
        {
            bool flag = false;
            _act.waitForVisibilityOfElement(_homescreen2Loc.ThankYouLabel, 60);
            if (_act.isElementPresent(_homescreen2Loc.ThankYouLabel))
            {
                if (_act.isElementDisplayed(_driver.FindElement(_homescreen2Loc.ThankYouLabel)))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        public string GetConfirmationMsg()
        {
            _act.waitForVisibilityOfElement(_homescreen2Loc.ConfirmationMsg, 120);
            string confirmationmsg = _act.getText(_homescreen2Loc.ConfirmationMsg, "ConfirmationMsg");
            return confirmationmsg;
        }

        public bool VerifyRequestNumber()
        {
            bool flag = false;
            _act.waitForVisibilityOfElement(_homescreen2Loc.ReqNumber, 60);
            if (_act.isElementPresent(_homescreen2Loc.ReqNumber))
            {
                if (_act.isElementDisplayed(_driver.FindElement(_homescreen2Loc.ReqNumber)))
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        public string GetReqNumber()
        {
            _act.waitForVisibilityOfElement(_homescreen2Loc.ReqNumber, 120);
            string reqnumber = _act.getText(_homescreen2Loc.ReqNumber, "ReqNumber");
            return reqnumber;
        }

    }
}
