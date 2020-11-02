using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using OpenQA.Selenium;
//using iText.Html2pdf;
//using EvoPdf;

namespace EquipmentReturn.Automation.Accelerators
{
    public class ExtentReport

    {
        protected static ExtentReports extent;
        public static ExtentReports GetExtent;
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentTest test;
        private static string ResultsPath;
        private static string reportPath;
        private static string screenshotPath;

        public static string date = DateTime.Now.ToString("yyyyMMdd-HHmmss");

        public static void TestMethod1()
        {
            try
            {
                string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
                string actualPath = path.Substring(0, path.LastIndexOf("bin"));
                string projectPath = new Uri(actualPath).LocalPath;
                string ts = date;
                ResultsPath = Path.Combine(projectPath, "Result\\Reports_" + date + "\\");
                //deletefile();
                reportPath = Path.Combine(ResultsPath, "EquipmentReturnReport-" + date + ".html");
                screenshotPath = Path.Combine(ResultsPath);
                string hostName = System.Net.Dns.GetHostName();
                ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(reportPath);
                htmlReporter.Configuration().Theme = Theme.Dark;
                htmlReporter.Configuration().DocumentTitle = "EquipmentReturnDocument";
                htmlReporter.Configuration().ChartVisibilityOnOpen = true;
                htmlReporter.Configuration().ReportName = "EquipmentReturn";
                htmlReporter.AppendExisting = true;
                //htmlReporter.LoadConfig("C:\\EquipmentReturn-ui-automation\\EquipmentReturn.Automation.FunctionalTest\\extent-config.xml");
                extent = new ExtentReports();
                extent.AddSystemInfo("Host Name", hostName);
                extent.AddSystemInfo("Environment", "Non Production");
                extent.AddSystemInfo("Username", "TTEC IT - QA Team");
                extent.AttachReporter(htmlReporter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        public static void Parent(string methodname, string description)
        {

        }
        public static void StartTest(string xyz)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DisableExtentReport"])) { return; }
            test = extent.CreateTest(xyz);

        }
        public static void ReportPass(string Stepname)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DisableExtentReport"])) { return; }
            try
            {
                test.Pass(Stepname);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void ReportFail(string Stepname)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DisableExtentReport"])) { return; }
            try
            {
                test.Fail(Stepname);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void email_send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("naiduswaroopa739@gmail.com");
                string txttoemail = "purna1221@gmail.com";
                //mail.To.Add("");
                string[] array = txttoemail.Split(',');
                foreach (string xyz in array)
                {
                    mail.To.Add(new MailAddress(xyz));
                }

                mail.Subject = "Test Mail - 1";
                mail.Body = "mail with attachment";

                System.Net.Mail.Attachment attachment;


                var directory = new DirectoryInfo(ResultsPath);
                var myFile = (from f in directory.GetFiles()
                              orderby f.LastWriteTime descending
                              select f).First();

                string attach = Convert.ToString(myFile);


                attachment = new System.Net.Mail.Attachment(directory + "\\" + attach);

                mail.Attachments.Add(attachment);
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("naiduswaroopa739@gmail.com", "Test@123456");
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }

        public static void deletefile()
        {
            try
            {
                string[] fileEntries = Directory.GetFiles(ResultsPath);
                foreach (string fileName in fileEntries)

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void Flush()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DisableExtentReport"])) { return; }
            try
            {
                extent.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static string Capture(IWebDriver driver, String screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
            Directory.CreateDirectory(Path.Combine(screenshotPath, "Screenshots"));
            var finalpth = Path.Combine(screenshotPath, "Screenshots\\" + screenShotName);
            var localpath = new Uri(finalpth).LocalPath;
            screenshot.SaveAsFile(localpath, format);
            return localpath;
        }
    }
}