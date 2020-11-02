#pragma warning disable 1591
using EquipmentReturn.Automation.Accelerators;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.IO;

namespace EquipmentReturn.Automation.FunctionalTest
{
    [SetUpFixture]
    public class SetUp
    {
        ResultDbHelper _dbresults = new ResultDbHelper();
        public bool newrunflag { get; set; }
        public bool resultflag { get; set; }
        public string runguid { get; set; }
        public string Env { get; set; } = "";
        public string Browser { get; set; } = "";
        public string Build { get; set; } = "";

        public string devicetype { get; set; } = "";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            newrunflag = Convert.ToBoolean(ConfigurationManager.AppSettings["NewTestRun"]);
            resultflag = Convert.ToBoolean(ConfigurationManager.AppSettings["ResultFlag"]);
            devicetype = TestEngine.DeviceType;
            XML.deletefile();
            ExtentReport.TestMethod1();

            if (newrunflag)
            {
                //Generate Run GUID
                runguid = _dbresults.ExecuteTestRun(Env, Browser, "1").ToString();
                TestData.RunGuid = runguid;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ExtentReport.Flush();
            ExtentReport.email_send();
            XML.append();

            if (newrunflag)
            {
                //Generate Run GUID
                _dbresults.UpdateEndTime(TestData.RunGuid.ToString());
            }

            if (resultflag)
            {
                _dbresults.UpdateScriptPassCount();
            }
        }
    }
}
#pragma warning restore 1591
