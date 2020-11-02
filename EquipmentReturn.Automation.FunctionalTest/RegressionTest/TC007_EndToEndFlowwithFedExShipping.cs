using System;
using AventStack.ExtentReports;
using EquipmentReturn.Automation.Accelerators;
using EquipmentReturn.Automation.Repository;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EquipmentReturn.Automation.FunctionalTest
{
    [TestFixture, Parallelizable, Category("RegressionTest")]
    class TC007_EndToEndFlowwithFedExShipping : TestEngine
    {
        [TearDown]
        public void AfterTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    DateTime time = DateTime.Now;
                    string fileName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
                    string screenShotPath = ExtentReport.Capture(_driver, fileName);
                    ExtentReport.test.Log(Status.Fail, "Fail");
                    ExtentReport.test.Log(Status.Fail, "Snapshot below: " + ExtentReport.test.AddScreenCaptureFromPath("Screenshots\\" + fileName));
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }

            ExtentReport.test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
            _driver.Quit();
        }

        private HomeScreen1 _homescreen1 = null;
        private HomeScreen2 _homescreen2 = null;
        string strMessage;
        private IWebDriver _driver = null;
        public TestEngine _testengine = new TestEngine();
        public XML _xml = new XML();
       
        DateTime starttime { get; set; } = DateTime.Now;

        [TestCase("android", TestName = "TC007_EndToEndFlowwithFedExShipping")]
        public void TC007_VerifyingEndToEndFlowwithFedExShipping(string strmobiledevice)
        {
            starttime = DateTime.Now;
            strMessage = string.Format("\r\n\t " + TestContext.CurrentContext.Test.Name + " Starts");

            _driver = _testengine.TestSetup(strmobiledevice);
            _homescreen1 = new HomeScreen1(_driver, "NR");
            _homescreen2 = new HomeScreen2(_driver, "NR");

            DataTable AddReturnInPerson = ExcelReader.ReadExcelFile("EquipmentReturn", "FedEXShipping", true);
            int rowcount = AddReturnInPerson.Rows.Count;
            string[] empId = new string[rowcount];
            string[] isvalidEmpId = new string[rowcount];
            string[] popupContinue = new string[rowcount];
            string[] popupCorrect = new string[rowcount];
            string[] empId2 = new string[rowcount];
            string[] isvalidEmpId2 = new string[rowcount];
            string[] firstName = new string[rowcount];
            string[] lastName = new string[rowcount];
            string[] personalEmail = new string[rowcount];
            string[] phoneumber = new string[rowcount];
            string[] returnType = new string[rowcount];
            string[] packMaterial = new string[rowcount];
            string[] addressLine1 = new string[rowcount];
            string[] addressLine2 = new string[rowcount];
            string[] city = new string[rowcount];
            string[] state = new string[rowcount];
            string[] country = new string[rowcount];
            string[] zipcode = new string[rowcount];

            for (int j = 0; j < rowcount; j++)
            {
                Dictionary<string, string> AddUserTestData = AddReturnInPerson.Columns
                    .Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => AddReturnInPerson.Rows[j][col].ToString());
                empId[j] = AddUserTestData["EmpId"];
                isvalidEmpId[j] = AddUserTestData["IsValidEmpId"];
                popupContinue[j] = AddUserTestData["PopupContinue"];
                popupCorrect[j] = AddUserTestData["PopupCorrect"];
                empId2[j] = AddUserTestData["EmpId2"];
                isvalidEmpId2[j] = AddUserTestData["IsValidEmpId2"];
                firstName[j] = AddUserTestData["FirstName"];
                lastName[j] = AddUserTestData["LastName"];
                personalEmail[j] = AddUserTestData["PersonalEmail"];
                phoneumber[j] = AddUserTestData["Phoneumber"];
                returnType[j] = AddUserTestData["ReturnType"];
                packMaterial[j] = AddUserTestData["PackMaterial"];
                addressLine1[j] = AddUserTestData["AddressLine1"];
                addressLine2[j] = AddUserTestData["AddressLine2"];
                city[j] = AddUserTestData["City"];
                state[j] = AddUserTestData["State"];
                country[j] = AddUserTestData["Country"];
                zipcode[j] = AddUserTestData["Zipcode"];
            }

            for (int i = 0; i < rowcount; i++)
            {
                _driver.Url = WebuiURL;
                bool flag = false;
                do
                {
                    _homescreen1.EnterEmpID(empId[i]);

                    _homescreen1.PressTabonEmpID();

                    if (isvalidEmpId[i] == "True")
                    {
                        Assert.IsFalse(_homescreen1.VerifyOracleIDNotFoundPopUp());
                    }
                    else
                    {
                        Assert.IsTrue(_homescreen1.VerifyOracleIDNotFoundPopUp());

                        if (popupContinue[i] == "True")
                        {
                            if (_homescreen1.ClickpopupContinueBtn())
                            {
                                _homescreen1.EnterFirstName(firstName[i]);

                                _homescreen1.EnterLastName(lastName[i]);
                            }
                            flag = false;
                        }

                        if (popupCorrect[i] == "True")
                        {
                            _homescreen1.ClickpopupCorrectBtn();
                            empId[i] = empId2[i];
                            isvalidEmpId[i] = isvalidEmpId2[i];
                            flag = true;
                        }
                    }
                } while (flag);

                // Screen 1

                _homescreen1.EnterPersonalEmail(personalEmail[i]);

                _homescreen1.EnterPhoneNumber(phoneumber[i]);

                _homescreen1.ClickVerifyPhnBtn();

                // explicitly waiting until OTP sent
                Thread.Sleep(10000);

                string VerificationCode = _homescreen1.FetchVerificationCodeTab();

                _homescreen1.ClickVerifyPhnBtn();

                _homescreen1.EnterOTP(VerificationCode);

                _homescreen1.ClickSubmitOTPBtn();

                _homescreen1.SelectReturnType(returnType[i]);

                if (packMaterial[i] == "Yes")
                {
                    _homescreen1.ClickPackMaterialYes();
                }
                else
                {
                    _homescreen1.ClickPackMaterialNo();
                }

                _homescreen1.EnterAddressLine1(addressLine1[i]);

                _homescreen1.EnterAddressLine2(addressLine2[i]);

                _homescreen1.EnterCity(city[i]);

                _homescreen1.SelectState(state[i]);

                _homescreen1.SelectCountry(country[i]);

                _homescreen1.EnterZipcode(zipcode[i]);

                //click on captch manually --> need to automate
                Thread.Sleep(5000);

                _homescreen1.ClickNextBtn();

                // Screen 2

                _homescreen2.VerifyThankYouLabel();

                string FedExConfirmMsg1 = "We have received your request, you will receive an email from FedEx with further instruction to complete your drop off.";

                string FedExConfirmMsg2 = "\r\nPlease note it may take up to 24 hours for the email with your shipping details from FedEx to be delivered.";

                string FedEXConfirmMsg = string.Concat(FedExConfirmMsg1, FedExConfirmMsg2);

                string ActualConfirmMsg = _homescreen2.GetConfirmationMsg();

                Assert.IsTrue(FedEXConfirmMsg.Contains(ActualConfirmMsg));

                _homescreen2.VerifyRequestNumber();

            }
        }
    }
}
