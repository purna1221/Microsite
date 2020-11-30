using OpenQA.Selenium;

namespace EquipmentReturn.Automation.Repository.Locators
{
    public interface IFormScreen1
    {
        By EmployeeID { get; }

        By FirstName { get; }

        By LastName { get; }

        By Email { get; }

        By Phone { get; }

        By VerifyPhnBtn { get; }

        By InvalidPHNMsg { get; }

        By UserOTPTextbox { get; }

        By submitOtpBtn { get; }

        By ResendOtpLink { get; }

        By ReturnOption { get; }

        By ReturnLocation { get; }

        By Date { get; }

        By Time { get; }

        By PackMaterialYes { get; }

        By PackMaterialNo { get; }

        By AddressLine1 { get; }

        By AddressLine2 { get; }

        By City { get; }

        By StateCode { get; }

        By CountryCode { get; }

        By ZipCode { get; }

        By StatusMessage { get; }

        By VisualCaptchaRefreshButton { get; }

        By VisualCaptchaAccessibilityButton { get; }

        By AccessibilityDescription { get; }

        By AudioFieldTextfield { get; }

        By VisualCaptchaImages { get; }

        By NextBtn { get; }

        By AdditionalHelpTopicsList { get; }

        By FooterLinks { get; }

        By LouiechatButton { get; }

        By OracleIDNotFoundPopUp { get; }

        By PopupContinueBtn { get; }

        By PopupCorrectBtn { get; }

        By ReqNumber { get; }
    }

    public class FormScreen1DesktopLoc : IFormScreen1
    {
        public By EmployeeID => By.XPath("//input[@id='formEmployeeID']");

        public By FirstName => By.XPath("//input[@id='formFirstName']");

        public By LastName => By.XPath("//input[@id='formLastName']");

        public By Email => By.XPath("//input[@id='formEmail']");

        public By Phone => By.XPath("//input[@id='formPhone']");

        public By VerifyPhnBtn => By.XPath("//button[@id='VerifyPhn']");

        public By InvalidPHNMsg => By.XPath("//span[@id='InvalidPHN']");

        public By UserOTPTextbox => By.XPath("//input[@id='userOTP']");

        public By submitOtpBtn => By.XPath("//button[@id='submitOtp']");

        public By ResendOtpLink => By.XPath("//a[@id='ResendOtp']");

        public By ReturnOption => By.XPath("//select[@id='formReturnOption']");

        // For ReturnOption - Return In Person

        public By ReturnLocation => By.XPath("//select[@id='formReturnLocation']");

        public By Date => By.XPath("//input[@id='date']");

        public By Time => By.XPath("//select[@id='appointment_time']");

        // For ReturnOption - FedEx Shipping

        public By PackMaterialYes => By.XPath("//label[@id='PackMatColor']/following::div//input[@value='Yes']");

        public By PackMaterialNo => By.XPath("//label[@id='PackMatColor']/following::div//input[@value='No']");

        public By AddressLine1 => By.XPath("//input[@id='AddressLine1']");

        public By AddressLine2 => By.XPath("//input[@id='AddressLine2']");

        public By City => By.XPath("//input[@id='city']");

        public By StateCode => By.XPath("//select[@id='stateCode']");

        public By CountryCode => By.XPath("//select[@id='countryCode']");

        public By ZipCode => By.XPath("//input[@id='zipCode']");

        // Captcha

        public By StatusMessage => By.XPath("//div[@id='status-message']//p[@id='status-text']");

        public By VisualCaptchaRefreshButton => By.XPath("//div[@class='visualCaptcha-refresh-button']/a");

        public By VisualCaptchaAccessibilityButton => By.XPath("//div[@class='visualCaptcha-accessibility-button']/a");

        public By AccessibilityDescription => By.ClassName("accessibility-description");

        public By AudioFieldTextfield => By.ClassName("form-control audioField");

        public By VisualCaptchaImages => By.XPath("//div[@class='visualCaptcha-possibilities']/div/a");

        public By NextBtn => By.XPath("//button[@id='Next']");

        // Additional Help Topics Links

        public By AdditionalHelpTopicsList => By.XPath("//div[@class='d-none d-md-block col-md-5 col-lg-4 offset-lg-1 mb-3 mt-4 help-topics']/p//a");

        public By FooterLinks => By.XPath("//div[@class='col pt-3 pb-2 text-center']//a");

        public By LouiechatButton => By.XPath("//div[@id='chatButton']/img");

        // OracleIDNotFound PopUp

        public By OracleIDNotFoundPopUp => By.XPath("//div[@id='exampleModal']//div[@class='modal-header']/h5");

        public By PopupContinueBtn => By.XPath("//button[@id='Continue']");

        public By PopupCorrectBtn => By.XPath("//button[@id='Correct']");

        //Request Number

        public By ReqNumber => By.XPath("//span[@id='ErrorText']");
    }

    public class FormScreen1MobileLoc : IFormScreen1
    {
        public By EmployeeID => By.XPath("//input[@id='formEmployeeID']");

        public By FirstName => By.XPath("//input[@id='formFirstName']");

        public By LastName => By.XPath("//input[@id='formLastName']");

        public By Email => By.XPath("//input[@id='formEmail']");

        public By Phone => By.XPath("//input[@id='formPhone']");

        public By VerifyPhnBtn => By.XPath("//button[@id='VerifyPhn']");

        public By InvalidPHNMsg => By.XPath("//span[@id='InvalidPHN']");

        public By UserOTPTextbox => By.XPath("//input[@id='userOTP']");

        public By submitOtpBtn => By.XPath("//button[@id='submitOtp']");

        public By ResendOtpLink => By.XPath("//a[@id='ResendOtp']");

        public By ReturnOption => By.XPath("//select[@id='formReturnOption']");

        // For ReturnOption - Return In Person

        public By ReturnLocation => By.XPath("//select[@id='formReturnLocation']");

        public By Date => By.XPath("//input[@id='date']");

        public By Time => By.XPath("//select[@id='appointment_time']");

        // For ReturnOption - FedEx Shipping

        public By PackMaterialYes => By.XPath("//label[@id='PackMatColor']/following::div//input[@value='Yes']");

        public By PackMaterialNo => By.XPath("//label[@id='PackMatColor']/following::div//input[@value='No']");

        public By AddressLine1 => By.XPath("//input[@id='AddressLine1']");

        public By AddressLine2 => By.XPath("//input[@id='AddressLine2']");

        public By City => By.XPath("//input[@id='city']");

        public By StateCode => By.XPath("//select[@id='stateCode']");

        public By CountryCode => By.XPath("//select[@id='countryCode']");

        public By ZipCode => By.XPath("//input[@id='zipCode']");

        // Captcha

        public By StatusMessage => By.XPath("//div[@id='status-message']//p[@id='status-text']");

        public By VisualCaptchaRefreshButton => By.XPath("//div[@class='visualCaptcha-refresh-button']/a");

        public By VisualCaptchaAccessibilityButton => By.XPath("//div[@class='visualCaptcha-accessibility-button']/a");

        public By AccessibilityDescription => By.ClassName("accessibility-description");

        public By AudioFieldTextfield => By.ClassName("form-control audioField");

        public By VisualCaptchaImages => By.XPath("//div[@class='visualCaptcha-possibilities']/div/a");

        public By NextBtn => By.XPath("//button[@id='Next']");

        // Additional Help Topics Links

        public By AdditionalHelpTopicsList => By.XPath("//div[@class='d-none d-md-block col-md-5 col-lg-4 offset-lg-1 mb-3 mt-4 help-topics']/p//a");

        public By FooterLinks => By.XPath("//div[@class='col pt-3 pb-2 text-center']//a");

        public By LouiechatButton => By.XPath("//div[@id='chatButton']/img");

        // OracleIDNotFound PopUp

        public By OracleIDNotFoundPopUp => By.XPath("//div[@id='exampleModal']//div[@class='modal-header']/h5");

        public By PopupContinueBtn => By.XPath("//button[@id='Continue']");

        public By PopupCorrectBtn => By.XPath("//button[@id='Correct']");

        //Request Number

        public By ReqNumber => By.XPath("//span[@id='ErrorText']");
    }
}
