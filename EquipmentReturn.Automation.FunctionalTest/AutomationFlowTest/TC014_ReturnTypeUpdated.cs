using AventStack.ExtentReports;
using EquipmentReturn.Automation.Accelerators;
using EquipmentReturn.Automation.Repository;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentReturn.Automation.FunctionalTest.AutomationFlowTest
{
    [TestFixture, Parallelizable, Category("RegressionTest")]
    class TC014_ReturnTypeUpdated :TestEngine
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
                case TestStatus.Passed:
                    logstatus = Status.Pass;
                    ExtentReport.test.Log(Status.Pass, "Pass");
                    break;
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
                    logstatus = Status.Info;
                    break;
            }

            ExtentReport.test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
            _driver.Quit();
        }

        private HomeScreen1 _homescreen1 = null;
        private AskNowPage _asknowpage = null;
        string strMessage;
        private IWebDriver _driver = null;
        public TestEngine _testengine = new TestEngine();
        public XML _xml = new XML();

        DateTime starttime { get; set; } = DateTime.Now;


        [TestCase("android", TestName = "TC014_ReturnTypeUpdated")]
        public void TC014_VerifyReturnTypeUpdated(string strmobiledevice)
        {
            starttime = DateTime.Now;
            strMessage = string.Format("\r\n\t " + TestContext.CurrentContext.Test.Name + " Starts");

            _driver = _testengine.TestSetup(strmobiledevice);
            _homescreen1 = new HomeScreen1(_driver, "NR");
            _asknowpage = new AskNowPage(_driver, "NR");

            DataTable AddReturnInPerson = ExcelReader.ReadExcelFile("EquipmentReturn", "ReturnTypeUpdated", true);
            int rowcount = AddReturnInPerson.Rows.Count;
            string[] empId = new string[rowcount];
            string[] isvalidRITM = new string[rowcount];

            for (int j = 0; j < rowcount; j++)
            {
                Dictionary<string, string> AddUserTestData = AddReturnInPerson.Columns
                    .Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => AddReturnInPerson.Rows[j][col].ToString());
                empId[j] = AddUserTestData["EmpId"];
                isvalidRITM[j] = AddUserTestData["isvalidRITM"];
            }

            for (int i = 0; i < rowcount; i++)
            {
                _driver.Url = WebuiURL;
                bool flag = false;
                do
                {
                    _homescreen1.EnterEmpID(empId[i]);

                    _homescreen1.PressTabonEmpID();

                    if (isvalidRITM[i] == "True")
                    {
                        Assert.IsFalse(_homescreen1.VerifyOracleIDNotFoundPopUp());

                        Assert.IsNotEmpty(_homescreen1.GetFirstNameText());

                        Assert.IsNotEmpty(_homescreen1.GetLastNameText());

                        Assert.IsNotEmpty(_homescreen1.GetEmailText());

                        Assert.IsNotEmpty(_homescreen1.GetPhoneNumberText());
                    }
                    else if (isvalidRITM[i] == "False")
                    {
                        Assert.IsTrue(_homescreen1.VerifyOracleIDNotFoundPopUp());
                    }

                    string ReqNumber = _homescreen1.FetchREQNumber();

                    _asknowpage.FetchRITMParameters(ReqNumber, 2, "Responder");

                } while (flag);
            }
        }
    }
}
