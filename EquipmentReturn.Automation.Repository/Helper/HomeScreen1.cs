using EquipmentReturn.Automation.Accelerators;
using EquipmentReturn.Automation.Repository.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace EquipmentReturn.Automation.Repository
{
    public class HomeScreen1 : TestEngine
    {
        private IFormScreen1 _homescreen1Loc;

        private ActionEngine _act = null;
        private IWebDriver _driver = null;

        public HomeScreen1(IWebDriver driver, string strUserType)
        {
            if (GetPlatform(driver))
                _homescreen1Loc = new FormScreen1MobileLoc();
            else
                _homescreen1Loc = new FormScreen1DesktopLoc();
            _act = new ActionEngine(driver);
            _driver = driver;
        }

        public void ClickNextBtn()
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.NextBtn, 30);
            _act.click(_homescreen1Loc.NextBtn, "NextBtn");
        }

        public bool GetEmployeeIDFieldStyle()
        {
            bool attrvalue = _act.VerifyByAttributeValue(_homescreen1Loc.EmployeeID, "style", "border-color: red;");
            return attrvalue;
        }

        public bool GetFirstNameFieldStyle()
        {
            bool attrvalue = _act.VerifyByAttributeValue(_homescreen1Loc.FirstName, "style", "border-color: red;");
            return attrvalue;
        }

        public bool GetLastNameFieldStyle()
        {
            bool attrvalue = _act.VerifyByAttributeValue(_homescreen1Loc.LastName, "style", "border-color: red;");
            return attrvalue;
        }

        public bool GetEmailFieldStyle()
        {
            bool attrvalue = _act.VerifyByAttributeValue(_homescreen1Loc.Email, "style", "border-color: red;");
            return attrvalue;
        }

        public bool GetPhoneFieldStyle()
        {
            bool attrvalue = _act.VerifyByAttributeValue(_homescreen1Loc.Phone, "style", "border-color: red;");
            return attrvalue;
        }

        public bool GetReturnOptionFieldStyle()
        {
            bool attrvalue = _act.VerifyByAttributeValue(_homescreen1Loc.ReturnOption, "style", "border-color: red;");
            return attrvalue;
        }

        public void EnterEmpID(string empid)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.EmployeeID, 30);
            _act.EnterText(_homescreen1Loc.EmployeeID, empid, "EmployeeID");
        }

        public void PressTabonEmpID()
        {
            _act.EnterTab(_homescreen1Loc.EmployeeID);
        }

        public string GetFirstNameText()
        {
            string firstnameval = _act.getValue(_homescreen1Loc.FirstName, "FirstName");
            return firstnameval;
        }

        public string GetLastNameText()
        {
            string lastnameval = _act.getValue(_homescreen1Loc.LastName, "LastName");
            return lastnameval;
        }

        public string GetEmailText()
        {
            string emailval = _act.getValue(_homescreen1Loc.Email, "Email");
            return emailval;
        }

        public string GetPhoneNumberText()
        {
            string phonenumberval = _act.getValue(_homescreen1Loc.Phone, "Phone");
            return phonenumberval;
        }

        public string GetRequestNumber()
        {
            string phonenumberval = _act.getValue(_homescreen1Loc.Phone, "Phone");
            return phonenumberval;
        }

        public void EnterFirstName(string fname)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.FirstName, 30);
            _act.EnterText(_homescreen1Loc.FirstName, fname, "FirstName");
        }

        public void EnterLastName(string lname)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.LastName, 30);
            _act.EnterText(_homescreen1Loc.LastName, lname, "LastName");
        }

        public void EnterPersonalEmail(string email)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.Email, 30);
            _act.EnterText(_homescreen1Loc.Email, email, "Email");
        }

        public void EnterPhoneNumber(string phone)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.Phone, 30);
            _act.EnterText(_homescreen1Loc.Phone, phone, "Phone");
        }

        public void EnterDate(string date)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.Date, 30);
            _act.EnterText(_homescreen1Loc.Date, date,"Date");
        }

        public void SelectTime(string time)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.Time, 120);
            _act.selectByOptionText(_homescreen1Loc.Time, time, "Time");
        }

        public void SelectReturnType(string returntype)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.ReturnOption, 120);
            _act.selectByOptionText(_homescreen1Loc.ReturnOption, returntype, "ReturnOption");
        }

        public void SelectSiteLocation(string site)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.ReturnLocation, 120);
            _act.selectByOptionText(_homescreen1Loc.ReturnLocation, site, "ReturnLocation");
        }

        public void ClickPackMaterialYes()
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.PackMaterialYes, 30);
            _act.click(_homescreen1Loc.PackMaterialYes, "PackMaterialYes");
        }

        public void ClickPackMaterialNo()
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.PackMaterialNo, 30);
            _act.click(_homescreen1Loc.PackMaterialNo, "PackMaterialNo");
        }

        public void EnterAddressLine1(string addressline1)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.AddressLine1, 30);
            _act.EnterText(_homescreen1Loc.AddressLine1, addressline1, "AddressLine1");
        }

        public void EnterAddressLine2(string addressline2)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.AddressLine2, 30);
            _act.EnterText(_homescreen1Loc.AddressLine2, addressline2, "AddressLine2");
        }

        public void EnterCity(string city)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.City, 30);
            _act.EnterText(_homescreen1Loc.City, city,"City");
        }

        public void SelectState(string statecode)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.StateCode, 120);
            _act.selectByOptionText(_homescreen1Loc.StateCode, statecode, "StateCode");
        }

        public void SelectCountry(string country)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.CountryCode, 120);
            _act.selectByOptionText(_homescreen1Loc.CountryCode, country, "CountryCode");
        }

        public void EnterZipcode(string zipcode)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.ZipCode, 30);
            _act.EnterText(_homescreen1Loc.ZipCode, zipcode,"ZipCode");
        }

        public void ClickVerifyPhnBtn()
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.VerifyPhnBtn, 30);
            _act.click(_homescreen1Loc.VerifyPhnBtn, "VerifyPhnBtn");
        }

        public string FetchVerificationCodeTab()
        {
            string verificationcode = "";
            bool flag = false;
            try
            {
                string SMSUrl = "https://receive-sms.com/";

                ((IJavaScriptExecutor)_driver).ExecuteScript("window.open()");
                _driver.SwitchTo().Window(_driver.WindowHandles[1]);
                _driver.Navigate().GoToUrl(SMSUrl);
               
                do
                {
                    IWebElement eleMsg = _driver.FindElement(By.XPath("//table[@id='messages-table']/tbody/tr[1]//td[@class='td-message']"));
                    string[] verificationmsg = eleMsg.Text.Split(' ');
                    if (verificationmsg[1] == "TTEC")
                    {
                        verificationcode = verificationmsg[5].Replace(".", "").ToString();
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                } while (flag);
                _driver.SwitchTo().Window(_driver.WindowHandles[1]).Close();
                _driver.SwitchTo().Window(_driver.WindowHandles[0]);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return verificationcode;
        }     

        public string FetchVerificationCode()
        {
            string verificationcode = "";
            try
            {
                string SMSUrl = "https://receive-sms.com/";

                IWebDriver driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                //((IJavaScriptExecutor)_driver).ExecuteScript("window.open()");
                //_driver.SwitchTo().Window(_driver.WindowHandles[1]);
                driver.Navigate().GoToUrl(SMSUrl);

                IWebElement eleMsg = driver.FindElement(By.XPath("//table[@id='messages-table']/tbody/tr[1]//td[@class='td-message']"));

                string[] verificationmsg = eleMsg.Text.Split(' ');
                verificationcode = verificationmsg[5].Replace(".", "").ToString();
                //driver.SwitchTo().Window(driver.WindowHandles[0]);
                driver.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return verificationcode;
        }

        public string FetchREQNumber()
        {
            IWebElement errortext = _driver.FindElement(_homescreen1Loc.ReqNumber);

            string[] errortextarray = errortext.Text.Split(')');

            string[] errortextsplited = errortextarray[0].Split('(');

            string reqnum = errortextsplited[1];

            return reqnum;
        }

        public void EnterOTP(string otp)
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.UserOTPTextbox, 30);
            _act.EnterText(_homescreen1Loc.UserOTPTextbox, otp, "UserOTPTextbox");
        }

        public void ClickSubmitOTPBtn()
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.submitOtpBtn, 30);
            _act.click(_homescreen1Loc.submitOtpBtn, "submitOtpBtn");
        }

        public void ClickResendOtpLink()
        {
            _act.waitForVisibilityOfElement(_homescreen1Loc.ResendOtpLink, 30);
            _act.click(_homescreen1Loc.ResendOtpLink, "ResendOtpLink");
        }

        public bool VerifyOracleIDNotFoundPopUp()
        {
            bool flag = false;

            Thread.Sleep(2000);

            _act.waitForVisibilityOfElement(_homescreen1Loc.OracleIDNotFoundPopUp, 10);

            if (_act.isElementPresent(_homescreen1Loc.OracleIDNotFoundPopUp))
            {
                if (_act.isElementDisplayed(_driver.FindElement(_homescreen1Loc.OracleIDNotFoundPopUp)))
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

        public bool ClickpopupContinueBtn()
        {
            bool flag = false;
            _act.waitForVisibilityOfElement(_homescreen1Loc.PopupContinueBtn, 30);
            flag = _act.click(_homescreen1Loc.PopupContinueBtn, "PopupContinueBtn");
            return flag;
        }

        public bool ClickpopupCorrectBtn()
        {
            bool flag = false;
            _act.waitForVisibilityOfElement(_homescreen1Loc.PopupCorrectBtn, 30);
            flag = _act.click(_homescreen1Loc.PopupCorrectBtn, "PopupCorrectBtn");
            return flag;
        }
    }
}
