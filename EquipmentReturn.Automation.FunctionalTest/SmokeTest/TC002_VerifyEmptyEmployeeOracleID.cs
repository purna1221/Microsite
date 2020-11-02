using System;
using EquipmentReturn.Automation.Accelerators;
using EquipmentReturn.Automation.Repository;
using NUnit.Framework;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;

namespace EquipmentReturn.Automation.FunctionalTest
{

    [TestFixture, Parallelizable, Category("SmokeTest")]
    class TC002_VerifyEmptyEmployeeOracleID : TestEngine
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

        private IWebDriver _driver = null;
        public TestEngine _testengine = new TestEngine();
        public XML _xml = new XML();
        string strMessage;
        DateTime starttime { get; set; } = DateTime.Now;

        [TestCase("android", TestName = "TC002_VerifyEmptyEmployeeOracleID")]
        public void TC002_VerifyingEmptyEmployeeOracleID(string strmobiledevice)
        {
            starttime = DateTime.Now;
            strMessage = string.Format("\r\n\t " + TestContext.CurrentContext.Test.Name + " Starts");

            try
            {
                _driver = _testengine.TestSetup(strmobiledevice);
                _homescreen1 = new HomeScreen1(_driver, "NR");

                ResultDbHelper _resul = new ResultDbHelper();

                // enter FirstName
                //_homescreen1.EnterFirstName();

                bool attrvalueempID = _homescreen1.GetEmployeeIDFieldStyle();
                Assert.IsTrue(attrvalueempID);

                bool attrvaluefName = _homescreen1.GetFirstNameFieldStyle();
                Assert.IsTrue(attrvaluefName);

                bool attrvaluelName = _homescreen1.GetLastNameFieldStyle();
                Assert.IsTrue(attrvaluelName);

                bool attrvalueemail = _homescreen1.GetEmailFieldStyle();
                Assert.IsTrue(attrvalueemail);

                bool attrvaluephone = _homescreen1.GetPhoneFieldStyle();
                Assert.IsTrue(attrvaluephone);

                bool attrvaluereturnoption = _homescreen1.GetReturnOptionFieldStyle();
                Assert.IsTrue(attrvaluereturnoption);

                // click on Next button
                _homescreen1.ClickNextBtn();

                ExtentReport.ReportPass(TestContext.CurrentContext.Test.FullName);
            }
            catch (Exception ex)
            {
                strMessage += ex.Message;
                ExtentReport.ReportFail(ex.Message);
                _xml.FailureTests(TestContext.CurrentContext.Test.FullName);
                _xml.FailurePlaylist(TestContext.CurrentContext.Test.FullName);
                Assert.Fail(ex.Message);
            }
        }
    }
}
