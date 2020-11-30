﻿using OpenQA.Selenium;
//using OpenQA.Selenium.Appium;
using System;
using ChromeDriver = OpenQA.Selenium.Chrome.ChromeDriver;
using ChromeOptions = OpenQA.Selenium.Chrome.ChromeOptions;
using System.Configuration;
using System.Diagnostics;
//using DesiredCapabilities = OpenQA.Selenium.Remote.DesiredCapabilities;
using RemoteWebDriver = OpenQA.Selenium.Remote.RemoteWebDriver;
using NUnit.Framework;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.Enums;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium.Support.Extensions;

namespace EquipmentReturn.Automation.Accelerators
{
    [TestFixture]
    public class TestEngine
    {
        /*Read framework specific settings from App.config */
        //public string browser = null;
        //public string device = null;
        public string environment = null;
       
        public string devicenamelocal = string.Empty;
        public string devicenameBS = string.Empty;
        public string devicenameiphone = string.Empty;
        public string deviceNameAndroid = string.Empty;
        public string configuration = string.Empty;
        public string EquipmentReturnNonProdURL = ConfigurationManager.AppSettings.Get("nonProd");
        public string EquipmentReturnProdURL = ConfigurationManager.AppSettings.Get("Prod");
        public string SessionId = string.Empty;

        public static string MobileMode = ConfigurationManager.AppSettings.Get("MobileMode");

        public static string Device { get; set; }

        public static string isNonProd = ConfigurationManager.AppSettings["isNonProd"];
        public static string isProd = ConfigurationManager.AppSettings["isProd"];

        public static string browserstack_user = ConfigurationManager.AppSettings["browserstackuser"];
        public static string browserstack_key = ConfigurationManager.AppSettings["browserstackkey"];


        //EquipmentReturn URL of all environments

        public static string ProdURL = ConfigurationManager.AppSettings["ProdUrl"];
        public static string NonProdURL = ConfigurationManager.AppSettings["NonProdUrl"];

        //Environment setup         
        public static string WebuiURL = isNonProd == "true" ? NonProdURL : ProdURL;

        //device setup
        public static string devicemode = ConfigurationManager.AppSettings["DeviceMode"]; // false
        public static string androiddevice = ConfigurationManager.AppSettings["androiddevice"]; //android
        public static string iosdevice = ConfigurationManager.AppSettings["iosdevice"]; // android
        public static string DeviceType = devicemode == "true" ? androiddevice : iosdevice;

        ResultDbHelper _dbresults = new ResultDbHelper();

        public bool newrunflag { get; set; }
        public string runguid { get; set; }
        public string Env { get; set; } = "";
        public string Browser { get; set; } = "";

        //public bool Config => configurationbool;
        public bool Config { get; set; }

        public IWebDriver driver { get; set; }

        private bool configurationbool;

        /*Report*/
       // public CReporter reporter = null;

        public enum browsertype
        {
            firefox,
            chrome,
            ie,
            android,
            ios,
            phantomjs,
            safari
        }

        public IWebDriver TestSetup(string strMobileDevice)
        {
            browserstack_user = ConfigurationManager.AppSettings.Get("browserstackuser");
            browserstack_key = ConfigurationManager.AppSettings.Get("browserstackkey");

            //Code in TestFixture
            DebugBSChrome();
            DebugBSMobile(strMobileDevice);
            DebugLocalMobile(strMobileDevice);
            DebugLocalChrome();

            //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(180));
            VerifyPageIsLoaded();
            driver.Navigate().GoToUrl(WebuiURL);
            ExtentReport.ReportPass("Successfully navigated to URL " + WebuiURL);
            // Update Env build and browser in DB
            ResultDbHelper _result = new ResultDbHelper();
            //_result.UpdateEnvdetails(Env, Browser, "");

            return driver;
        }

        public IWebDriver TestSetup(string strMobileDevice, string nr)
        {
            browserstack_user = ConfigurationManager.AppSettings.Get("browserstackuser");
            browserstack_key = ConfigurationManager.AppSettings.Get("browserstackkey");

            //Code in TestFixture
            DebugBSChrome();
            DebugBSMobile(strMobileDevice);
            DebugLocalMobile(strMobileDevice);
            DebugLocalChrome();
            
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(120));
            VerifyPageIsLoaded();
            driver.Navigate().GoToUrl(WebuiURL);

            ResultDbHelper _result = new ResultDbHelper();
            //_result.UpdateEnvdetails(Env, Browser, "");

            return driver;
        }

        public void VerifyPageIsLoaded()
        {
            var pageLoaded = false;

            for (var i = 0; i < 120000; i++)
            {
                Thread.Sleep(1000);

                if (driver.ExecuteJavaScript<string>("return document.readyState").Equals("complete"))
                //jQuery.active might cause problems on some browser or browserstack so I commented it out
                //&& WebDriver.ExecuteJavaScript<bool>("return jQuery.active == 0").Equals(true))
                {
                    pageLoaded = true;
                    break;
                }

                Thread.Sleep(1000);
            }

            if (!pageLoaded)
            {
                throw new Exception("Page was not with complete state)!");
            }
        }

        //  [TestFixtureTearDown]
        //public void afterSuite()
        //{
        //    //this.reporter.calculateSuiteExecutionTime();

        //    //this.reporter.createHtmlSummaryReport(ReporterConstants.APP_BASE_URL, true);
        //    //this.reporter.closeSummaryReport();
        //    //driver.Close();
        //    //driver.Quit();
        //}

        [Conditional("DEBUGLOCALCHROME")]
        public void DebugLocalChrome()
        {
            try
            {
                //set configuration to be picked up and set Locator
                Config = false;
                Browser = "Chrome";
                Env = "Desktop";

                string path = TestUtility.GetAssemblyDirectory() + "\\chromedriver.exe";
                Environment.SetEnvironmentVariable("webdriver.chrome.driver", path);

                ChromeOptions chrOpts = new ChromeOptions();
                chrOpts.AddArguments("test-type");
                //chrOpts.AddArguments("--disable-extensions");
                chrOpts.AddArguments("no-sandbox");
                //chrOpts.AddArgument("incognito");
                chrOpts.AddUserProfilePreference("download.prompt_for_download", ConfigurationManager.AppSettings["ShowBrowserDownloadPrompt"]);
                driver = new ChromeDriver(chrOpts);
                driver.Manage().Window.Maximize();
                //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("ElementPageLoad"))));
                ExtentReport.StartTest(TestContext.CurrentContext.Test.Name);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Conditional("DEBUGLOCALMOBILE")]
        public void DebugLocalMobile(string strMobileDevice)
        {
            if (strMobileDevice == browsertype.android.ToString())
            {
                Browser = "Android";
                Env = "Android";
                Config = true;
                try
                {
                    //devicenamelocal = ConfigurationManager.AppSettings.Get("androiddeviceNameLocal");
                    //configurationbool = true;
                    //DesiredCapabilities capabilities = new DesiredCapabilities();
                    //capabilities.SetCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Chrome);
                    //// capabilities.SetCapability(MobileCapabilityType.DeviceName, devicenamelocal);
                    //capabilities.SetCapability(MobileCapabilityType.DeviceName, "192.168.99.101:5555");
                    //capabilities.SetCapability(MobileCapabilityType.PlatformName, MobilePlatform.Android);
                    //capabilities.SetCapability("newCommandTimeout", TimeSpan.FromSeconds(200));
                    //driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);
                    //// driver = new RemoteWebDriver(capabilities);
                    //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("ElementPageLoad"))));
                    //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(200));


                    //-------------- local chrome DEVICE Mode for chrome ------------------------

                    string path = TestUtility.GetAssemblyDirectory() + "\\chromedriver.exe";
                    Environment.SetEnvironmentVariable("webdriver.chrome.driver", path);

                    DesiredCapabilities capabilities = new DesiredCapabilities();
                    capabilities.SetCapability(MobileCapabilityType.Orientation, MobileSelector.AndroidUIAutomator);
                    capabilities.SetCapability(MobileCapabilityType.BrowserName, "Android");
                    capabilities.SetCapability(MobileCapabilityType.DeviceName, "Galaxy S5");
                    capabilities.SetCapability(MobileCapabilityType.PlatformName, "Android");
                    capabilities.SetCapability(MobileCapabilityType.PlatformVersion, "5.0.2");
                    capabilities.SetCapability(MobileBrowserType.Browser, "Android");


                    //Dictionary<String, String> mobileEmulation = new Dictionary<String, String>();
                    //mobileEmulation.Add("deviceName", "Galaxy S5");
                    //mobileEmulation.Add("height", "640");
                    //mobileEmulation.Add("width", "360");
                    //mobileEmulation.Add("pixelRatio", "3");
                    //mobileEmulation.Add("browserName", "Android");
                    //mobileEmulation.Add("platformName", MobilePlatform.Android);
                    ChromeOptions chrOpts = new ChromeOptions();
                    // chrOpts.AddAdditionalCapability("capabilityName", mobileEmulation);
                    chrOpts.EnableMobileEmulation("Galaxy S5");
                    // chrOpts.AddArguments("test-type");
                    // chrOpts.AddArguments("--disable-extensions");
                    chrOpts.AddArgument("incognito");
                    //chrOpts.AddUserProfilePreference("download.prompt_for_download", ConfigurationManager.AppSettings["ShowBrowserDownloadPrompt"]);
                    driver = new ChromeDriver(chrOpts);
                    driver.Manage().Window.Maximize();
                    driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("ElementPageLoad"))));

                    // Map<String, String> mobileEmulation = new HashMap<>();
                    // mobileEmulation.put("deviceName", "Nexus 5");
                    // ChromeOptions chromeOptions = new ChromeOptions();
                    // chromeOptions.setExperimentalOption("mobileEmulation", mobileEmulation);
                    //WebDriver driver = new ChromeDriver(chromeOptions);



                    //------------- Genymotion Local---------------------

                    //DesiredCapabilities capabilities = new DesiredCapabilities();
                    //capabilities.SetCapability("device", "Android");
                    //capabilities.SetCapability("browserName", "chrome");
                    //capabilities.SetCapability("deviceName", "192.168.99.101:5555");
                    //capabilities.SetCapability("platformName", "Android");
                    //capabilities.SetCapability("platformVersion", "5.0.2");

                    //driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities, TimeSpan.FromSeconds(180));
                    //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("ElementPageLoad"))));
                    //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(200));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (strMobileDevice == browsertype.ios.ToString())
            {
                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability("platformName", "ios");
                capabilities.SetCapability("platformVersion", "11.0");
                capabilities.SetCapability("", "Mac");
                capabilities.SetCapability("deviceName", "iPhone SE");
                capabilities.SetCapability("automationplatformName", "XCUITest");
                capabilities.SetCapability("browserName", "Safari");

                Uri serverUri = new Uri("http://192.168.10.59:4723/wd/hub"); //192.168.10.41  192.168.1.6 192.168.10.46 10.37.110.228 192.168.10.60
                                                                             // driver = new IOSDriver<IWebElement>(serverUri, capabilities, TimeSpan.FromSeconds(500));
                driver = new RemoteWebDriver(serverUri, capabilities, TimeSpan.FromSeconds(1000));
            }
            else
            {
                driver = null;
            }
        }

        [Conditional("DEBUGBSMOBILE")]
        public void DebugBSMobile(string strMobileDevice)
        {
            Config = true;
            configurationbool = true;

            if (strMobileDevice == browsertype.android.ToString())
            {
                // ********************** sauce lab capabilities start **********************

                //Thread.Sleep(4000);
                //devicenameBS = ConfigurationManager.AppSettings.Get("androiddeviceNameBS");
                //this.deviceNameAndroid = ConfigurationManager.AppSettings.Get("androiddevice");
                //this.environment = ConfigurationManager.AppSettings.Get("environment");
                //DesiredCapabilities capability = DesiredCapabilities.Android();              
                //capability.SetCapability("browserName", "Chrome");
                //capability.SetCapability("platformVersion", "6.1");
                //capability.SetCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Chrome);             
                //capability.SetCapability("username", "Sunil21318"); // supply sauce labs username
                //capability.SetCapability("accessKey", "d056215a-a397-414c-923f-e42e0be236b6");  // supply sauce labs account key
                //capability.SetCapability("deviceName", "Android Emulator");
                //capability.SetCapability("tunnel-identifier", Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER"));
                //capability.SetCapability("build", Environment.GetEnvironmentVariable("TRAVIS_BUILD_NUMBER"));               
                //capability.SetCapability("deviceReadyTimeout", TimeSpan.FromSeconds(120));
                //capability.SetCapability("androidDeviceReadyTimeout", TimeSpan.FromSeconds(120));
                //capability.SetCapability("avdLaunchTimeout", TimeSpan.FromSeconds(120));
                //capability.SetCapability("appWaitDuration", TimeSpan.FromSeconds(120));
                //capability.SetCapability("avdReadyTimeout", TimeSpan.FromSeconds(120));              
                //capability.SetCapability("idleTimeout", 120);
                //driver = new RemoteWebDriver(new Uri("http://Sunil21318:d056215a-a397-414c-923f-e42e0be236b6@ondemand.saucelabs.com/wd/hub"), capability, TimeSpan.FromSeconds(120));
                //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(200));
                //driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(200));

                // ********************** sauce lab capabilities end **********************



                //  ********************** browser stack capabilities start **********************

                Browser = "Chrome";
                Env = "Android";

                devicenameBS = ConfigurationManager.AppSettings.Get("androiddeviceNameBS");
                this.deviceNameAndroid = ConfigurationManager.AppSettings.Get("androiddevice");
                this.environment = ConfigurationManager.AppSettings.Get("environment");
                DesiredCapabilities capability = DesiredCapabilities.Android();
                ChromeOptions chrOpts = new ChromeOptions();
                chrOpts.AddArguments("test-type");
                chrOpts.AddArguments("--disable-extensions");
                chrOpts.AddArgument("incognito");
                chrOpts.AddArgument("no-sandbox");
                capability = (DesiredCapabilities)chrOpts.ToCapabilities();
                capability.SetCapability("browserName", "android");
                capability.SetCapability("browserstack.user", browserstack_user); // browser stack username
                capability.SetCapability("browserstack.key", browserstack_key); // browser stack account key
                capability.SetCapability("deviceName", devicenameBS);
                capability.SetCapability("browserName", "chrome");
                capability.SetCapability("platform", MobilePlatform.Android);
                capability.SetCapability("browserstack.debug", true);
                capability.SetCapability("project", Environment.GetEnvironmentVariable("BS_AUTOMATE_PROJECT"));
                capability.SetCapability("build", Environment.GetEnvironmentVariable("BS_AUTOMATE_BUILD"));
                capability.SetCapability("browserstack.localIdentifier", Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL_IDENTIFIER"));
                capability.SetCapability("deviceReadyTimeout", TimeSpan.FromSeconds(200));
                capability.SetCapability("androidDeviceReadyTimeout", TimeSpan.FromSeconds(200));
                capability.SetCapability("avdLaunchTimeout", TimeSpan.FromSeconds(200));
                capability.SetCapability("appWaitDuration", TimeSpan.FromSeconds(200));
                capability.SetCapability("avdReadyTimeout", TimeSpan.FromSeconds(200));
                capability.SetCapability("idleTimeout", 300);
                driver = new RemoteWebDriver(new Uri(ConfigurationManager.AppSettings.Get("server")), capability, TimeSpan.FromSeconds(200));
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(200));

                // ********************** browser stack capabilities end **********************



            }
            else if (strMobileDevice == browsertype.ios.ToString())
            {
                Browser = "Safari";
                Env = "IOS";
                Thread.Sleep(4000);
                devicenameBS = ConfigurationManager.AppSettings.Get("iosdeviceNameBS");
                this.deviceNameAndroid = ConfigurationManager.AppSettings.Get("iosdevice");
                this.environment = ConfigurationManager.AppSettings.Get("environment");
                DesiredCapabilities desiredCap = DesiredCapabilities.IPhone();
                desiredCap.SetCapability("browserstack.user", browserstack_user);
                desiredCap.SetCapability("browserstack.key", browserstack_key);
                desiredCap.SetCapability("platform", "MAC");
                desiredCap.SetCapability("browserName", "iPhone");
                desiredCap.SetCapability("browserstack.debug", true);
                desiredCap.SetCapability("device", devicenameiphone);
                desiredCap.SetCapability("project", Environment.GetEnvironmentVariable("BS_AUTOMATE_PROJECT"));
                desiredCap.SetCapability("build", Environment.GetEnvironmentVariable("BS_AUTOMATE_BUILD"));
                desiredCap.SetCapability("browserstack.localIdentifier", Environment.GetEnvironmentVariable("BROWSERSTACK_LOCAL_IDENTIFIER"));
                desiredCap.SetCapability("newCommandTimeout", TimeSpan.FromSeconds(120));
                desiredCap.SetCapability("idleTimeout", TimeSpan.FromSeconds(120));
                driver = new RemoteWebDriver(new Uri(ConfigurationManager.AppSettings.Get("server")), desiredCap, TimeSpan.FromSeconds(120));
                // driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(200));
            }
        }

        [Conditional("DEBUGBSCHROME")]
        public void DebugBSChrome()
        {
            Browser = "Chrome";
            Env = "BS Desktop";
            Config = false;
            configurationbool = false;
            DesiredCapabilities desiredCap = DesiredCapabilities.Chrome();
            ChromeOptions chrOpts = new ChromeOptions();
            chrOpts.AddArguments("test-type");
            chrOpts.AddArguments("--disable-extensions");
            //chrOpts.AddArgument("incognito");
            //desiredCap.SetCapability(ChromeOptions.Capability, chrOpts);    // updated                   
            desiredCap = (DesiredCapabilities)chrOpts.ToCapabilities();


            desiredCap.SetCapability("os", "Windows");
            desiredCap.SetCapability("os_version", "10");
            desiredCap.SetCapability("browser", "Chrome");
            desiredCap.SetCapability("browser_version", "81");
            desiredCap.SetCapability("browserstack.user", browserstack_user);
            desiredCap.SetCapability("browserstack.key", browserstack_key);
            
            // desiredCap.SetCapability("build", Environment.GetEnvironmentVariable("BS_AUTOMATE_BUILD"));
            desiredCap.SetCapability("browserstack.debug", true);
            // string strTestName = TestContext.CurrentContext.Test.Name.ToString();
            desiredCap.SetCapability("project", Environment.GetEnvironmentVariable("BS_AUTOMATE_PROJECT"));
            desiredCap.SetCapability("build", Environment.GetEnvironmentVariable("BS_AUTOMATE_BUILD"));

            driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), desiredCap);

            SessionId = ((RemoteWebDriver)driver).SessionId.ToString();

            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("ElementPageLoad"))));
            driver.Manage().Window.Maximize();
        }

        [Conditional("DEBUG")]
        public void Debug()
        {
            //DebugBSMobile();
        }

        [Conditional("RELEASE")]
        public void Release()
        {
            //DebugBSMobile();
        }

        [Conditional("DEBUGLOCALSAFARI")]
        public void DebugLocalSafari()
        {
            try
            {
                //set configuration to be picked up and set Locator
                Config = false;
                Browser = "Safari";
                Env = "Desktop";

                string path = TestUtility.GetAssemblyDirectory() + "\\chromedriver.exe";
                Environment.SetEnvironmentVariable("webdriver.chrome.driver", path);

                ChromeOptions chrOpts = new ChromeOptions();
                chrOpts.AddArguments("test-type");
                chrOpts.AddArguments("--disable-extensions");
                // chrOpts.AddArgument("incognito");
                chrOpts.AddUserProfilePreference("download.prompt_for_download", ConfigurationManager.AppSettings["ShowBrowserDownloadPrompt"]);
                driver = new ChromeDriver(chrOpts);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings.Get("ElementPageLoad"))));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Conditional("DEBUGMOBILESAFARI")]
        public void DebugMobileSafari(string strMobileDevice)
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("platformName", "ios");
            capabilities.SetCapability("platformVersion", "11.0");
            capabilities.SetCapability("", "Mac");
            capabilities.SetCapability("deviceName", "iPhone 6");
            capabilities.SetCapability("automationplatformName", "XCUITest");
            capabilities.SetCapability("browserName", "Safari");

            Uri serverUri = new Uri("http://192.168.10.60:4723/wd/hub"); //192.168.10.41  192.168.1.6 192.168.10.46 192.168.10.60
                                                                         // driver = new IOSDriver<IWebElement>(serverUri, capabilities, TimeSpan.FromSeconds(500));
            driver = new RemoteWebDriver(serverUri, capabilities, TimeSpan.FromSeconds(500));
        }

        public bool GetPlatform(IWebDriver driver)
        {
            bool flag = false;
            if (MobileMode == "true")
            {
                flag = true;
            }
            else
            {
                string strval = ((OpenQA.Selenium.Remote.DesiredCapabilities)((OpenQA.Selenium.Remote.RemoteWebDriver)driver).Capabilities).Platform.PlatformType.ToString();
                if (strval == "Android" || strval == "Mac")
                    flag = true;
                else flag = false;
            }
            return flag;
        }
    }

    public class ChangeSessionStatus
    {
        public void RestAPI(string outcome, string sessionid)
        {
            //Completed, Error or Timeout
            string result = (outcome == "Passed") ? "Completed" : "Failed";
            string reqString = string.Empty;
            // string sessionid = TestContext.CurrentContext.Test.Name.ToString();
            if ((outcome == "Passed"))
            {
                reqString = "{\"status\":\"completed\", \"reason\":\"horrey Passed\"}";
            }
            else
            {
                reqString = "{\"status\":\"error\", \"reason\":\"horrey Passed\"}";
            }

            byte[] requestData = Encoding.UTF8.GetBytes(reqString);
            Uri myUri = new Uri(string.Format("https://www.browserstack.com/automate/sessions/" + sessionid + ".json"));
            WebRequest myWebRequest = HttpWebRequest.Create(myUri);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myWebRequest.ContentType = "application/json";
            myWebRequest.Method = "PUT";
            myWebRequest.ContentLength = requestData.Length;
            using (Stream st = myWebRequest.GetRequestStream()) st.Write(requestData, 0, requestData.Length);

            NetworkCredential myNetworkCredential = new NetworkCredential("sunil406", "v6NkgMhsa78ukX7gcLAQ");
            CredentialCache myCredentialCache = new CredentialCache();
            myCredentialCache.Add(myUri, "Basic", myNetworkCredential);
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Credentials = myCredentialCache;
            //myHttpWebRequest.ContinueTimeout = 5000;
            myWebRequest.GetResponse().Close();

        }

        public string GetSessionID(IWebDriver driver)
        {
            string sessionid = string.Empty;
            var sessionIdProperty = typeof(RemoteWebDriver).GetProperty("SessionId", BindingFlags.Instance | BindingFlags.NonPublic);
            if (sessionIdProperty != null)
            {
                SessionId sessionId = sessionIdProperty.GetValue(driver, null) as SessionId;
                if (sessionId == null)
                {
                    Console.WriteLine("Could not obtain SessionId.");
                }
                else
                {
                    Console.WriteLine("SessionId is " + sessionId.ToString());
                }
                sessionid = sessionId.ToString();
            }
            return sessionid;
        }

        // public string GetBuildID()
    }

    public class Base
    {
        /** IWebDriver Instance **/
        public bool configurationbool { get; set; }
    }
}



