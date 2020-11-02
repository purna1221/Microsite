using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentReturn.Automation.Repository.Locators
{
    public interface IFormScreen2
    {
        By ThankYouLabel { get; }

        By ConfirmationMsg { get; }

        By ReqNumber { get; }
    }

    public class FormScreen2DesktopLoc : IFormScreen2
    {
        public By ThankYouLabel => By.XPath("//div[@class='form-screen-2 py-5']/h1");

        public By ConfirmationMsg => By.XPath("//p[@id='Requested']");

        public By ReqNumber => By.XPath("//span[@id='ReqNumber']");
    }

    public class FormScreen2MobileLoc : IFormScreen2
    {
        public By ThankYouLabel => By.XPath("//div[@class='form-screen-2 py-5']/h1");

        public By ConfirmationMsg => By.XPath("//p[@id='Requested']");

        public By ReqNumber => By.XPath("//span[@id='ReqNumber']");
    }

}
