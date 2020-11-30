using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentReturn.Automation.Repository.Locators
{
    public class AskNowLoc
    {
        public By Asknowuname => By.XPath("//input[@id='userNameInput']");

        public By Asknowpwd => By.XPath("//input[@id='passwordInput']");

        public By Asknowloginbtn => By.XPath("//span[@id='submitButton']");

        public By SysParamSearchIcon => By.XPath("//span[@class='input-group-addon-transparent icon-search sysparm-search-icon']");

        public By SysParmSearchInput => By.XPath("//input[@class='form-control form-control-search sn-tooltip-basic focus']");        

        public By BusinessServiceinREQ => By.XPath("//input[@id='sys_display.sc_request.cmdb_ci']");

        public By BusinessServiceinRITM => By.XPath("//input[@id='sys_display.sc_req_item.cmdb_ci']");

        public By ShortDescriptionREQ => By.XPath("//input[@id='sc_request.short_description']");

        public By ShortDescriptionRITM => By.XPath("//input[@id='sc_req_item.short_description']");

        public By DescriptionREQ => By.XPath("//textarea[@id='sc_request.description']");

        public By DescriptionRITM => By.XPath("//textarea[@id='sc_req_item.description']");

        public By RITMNumberLink => By.XPath("//a[@class='linked formlink']");
        //table[@id='sc_request.sc_req_item.request_table']//tr[@class='list_row list_odd']/td/a[@class='linked formlink']

        public By CategoryStatus => By.XPath("//span[contains(text(),'Category Status')]/parent::label/parent::div/following-sibling::div/select");

        public By TaskLists => By.XPath("//table[@id='sc_req_item.sc_task.request_item_table']//td/a[@class='linked formlink']");

        public By TaskStatus => By.XPath("//select[@id='sc_task.state']");
    }
}
