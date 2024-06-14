///********************************************************************************************
// * Project Name - MaintenanceLocalizationBL
// * Description  - Created to fetch localized text and messages for Maintenance Module.
// *  
// **************
// **Version Log
// **************
// *Version     Date          Modified By    Remarks          
// *********************************************************************************************
// *2.60.02        13-June-2019   Muhammed Mehraj   Created.
// ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    public class MaintenanceLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();
        /// <summary>
        ///   Default Constructor for Maintenance Localization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public MaintenanceLocalizationBL(ExecutionContext executionContext, string entityName)
        {
            log.LogMethodEntry(executionContext, entityName);
            this.executionContext = executionContext;
            this.entityName = entityName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Getting lable messageno and headers
        /// </summary>
        /// <returns>json</returns>
        private void GetLiteralsAndMessages(string entity, List<string> literalsOrMessageList)
        {
            string localizedValue = string.Empty;
            foreach (string literalsOrMessages in literalsOrMessageList)
            {
                if (entity == "COMMONMESSAGES")
                {
                    localizedValue = MessageContainerList.GetMessage(executionContext, Convert.ToInt32(literalsOrMessages));
                }
                else
                {
                    localizedValue = MessageContainerList.GetMessage(executionContext, literalsOrMessages);
                }
                if (!listHeadersList.ContainsKey(literalsOrMessages))
                {
                    listHeadersList.Add(literalsOrMessages, localizedValue);
                }
            }
        }
        public string GetLocalizedLabelsAndHeaders()
        {
            log.LogMethodEntry();
            List<string> entities = new List<string>();
            entities.Add(entityName);
            entities.Add("COMMONMESSAGES");
            entities.Add("COMMONLITERALS");
            List<string> literalsOrMessage = new List<string>();
            foreach (string entity in entities)
            {
                if (entity == "COMMONMESSAGES")
                {
                    literalsOrMessage = GetMessages(entity);
                }
                else
                {
                    literalsOrMessage = GetLiterals(entity);
                }
                GetLiteralsAndMessages(entity, literalsOrMessage);
            }

            string literalsMessagesList = string.Empty;
            if (listHeadersList.Count != 0)
            {
                literalsMessagesList = JsonConvert.SerializeObject(listHeadersList, Formatting.Indented);
            }
            log.LogMethodExit(literalsMessagesList);
            return literalsMessagesList;
        }
        private List<string> GetMessages(string entityName)
        {
            log.LogMethodEntry(entityName);
            List<string> messages = new List<string>();
            switch (entityName.ToUpper().ToString())
            {
                case "COMMONMESSAGES":
                    messages.Add("691"); // published successfully
                    messages.Add("566"); // Do you want to save the changes? 
                    messages.Add("977");
                    messages.Add("1764"); // Record has been saved successfully
                    messages.Add("1765");
                    messages.Add("1766");
                    messages.Add("1751"); // Required
                    messages.Add("1749");
                    messages.Add("1750");
                    messages.Add("1752");
                    messages.Add("1753");
                    messages.Add("1754");
                    messages.Add("1755");
                    messages.Add("1756");
                    messages.Add("1757");
                    messages.Add("1758");
                    messages.Add("1759");
                    messages.Add("1760");
                    messages.Add("1761"); // Accepts only Alphanumeric, hyphen(-) and colon(:)
                    messages.Add("1762");
                    messages.Add("1763");
                    messages.Add("1769"); // Do you want to shutdown the  computer : &1
                    messages.Add("1770"); // Are you sure you want to reset?
                    messages.Add("1777"); // Please enter data and save
                    messages.Add("1778"); // Deleting the existing record is not allowed. Do you want make record inative?
                    messages.Add("1448");   // Loading... Please wait...
                    messages.Add("1593");   //  Nothing to export. Please change the search criteria.
                    messages.Add("1144");   //  Please enter valid value for &1
                    messages.Add("1600");   //  Nothing to export.
                    messages.Add("882");   // No valid data found in uploaded xls
                    messages.Add("959");   //  No rows selected. Please select the rows before clicking delete.
                    messages.Add("957");   // Rows deleted succesfully
                    messages.Add("646");   // Please enter a number
                    messages.Add("122");   // Save Successful 
                    messages.Add("1425");
                    messages.Add("1426");
                    messages.Add("694");
                    messages.Add("601");
                    messages.Add("594"); //Image File Error
                    messages.Add("648");
                    messages.Add("11864");
                    messages.Add("659");
                    messages.Add("601");
                    messages.Add("741");
                    messages.Add("1357");
                    messages.Add("1358");
                    messages.Add("640");
                    messages.Add("1359");
                    messages.Add("1360");
                    messages.Add("643");
                    messages.Add("566");
                    messages.Add("1424");
                    messages.Add("628");
                    messages.Add("664");
                    messages.Add("665");
                    messages.Add("628");
                    messages.Add("371");
                    messages.Add("122");
                    messages.Add("718");
                    messages.Add("959");
                    messages.Add("958");
                    messages.Add("957");
                    messages.Add("582");
                    messages.Add("583");
                    messages.Add("602");
                    messages.Add("644");
                    messages.Add("566");
                    messages.Add("706");
                    messages.Add("601");
                    messages.Add("594");
                    messages.Add("648");
                    messages.Add("627");
                    messages.Add("1125");
                    messages.Add("728");
                    messages.Add("539");
                    messages.Add("664"); //Please save changes before changing options
                    messages.Add("665");//Please save changes before this operation
                    messages.Add("1124"); //Role and assigned manager role can not be same
                    messages.Add("1867"); //Select a record to see more actions
                    messages.Add("1868"); //Do you want to continue without saving?
                    messages.Add("1869"); //Unable to delete this record.Please check the reference record first.
                    messages.Add("654");
                    messages.Add("1887"); // Do you want to start the purge process immediately
                    messages.Add("1888"); // Purge Complete
                    messages.Add("1889"); // Past Data Purge Completed
                    messages.Add("1890"); // Invalid Licensed POS key
                    messages.Add("1891"); // Invalid features key
                    messages.Add("1892"); // Authentication is required to proceed with save
                    messages.Add("1893"); // Please enter Authkey to proceed
                    messages.Add("1894"); // Please enter Add Cards Key
                    messages.Add("1895"); // Enter a valid Add Cards Key
                    break;
            }
            return messages;

        }

        public enum MaintenanceEntityLocalization
        {
            COMMONLITERALS,
            ASSETTYPE,
            ASSETGROUP,
            ASSETDETAILED,
            ASSETLIMITED,
            ASSETGROUPASSET,
            TASKGROUP,
            MAINTENANCETASK,
            SCHEDULECALENDER,
            SCHEDULE,
            CREATEADHOCJOB,
            MAINTENANCEJOBDETAILS,
            MAINTENANCEREQUESTS,
            MAINTENANCEREQUEST,
            EMAIL

        }
        public List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            try
            {
                List<string> literals = new List<string>();

                MaintenanceEntityLocalization maintenanceEntityLocalization = (MaintenanceEntityLocalization)Enum.Parse(typeof(MaintenanceEntityLocalization), entityName);
                switch (maintenanceEntityLocalization)
                {
                    case MaintenanceEntityLocalization.COMMONLITERALS:
                        literals.Add("Filter");
                        literals.Add("First");
                        literals.Add("Previous");
                        literals.Add("Next");
                        literals.Add("Last");
                        literals.Add("Save");
                        literals.Add("Refresh");
                        literals.Add("Delete");
                        literals.Add("Close");
                        literals.Add("Creation Date");
                        literals.Add("Search");
                        literals.Add("Advanced Search");
                        literals.Add("Active?");
                        literals.Add("Is Active");
                        literals.Add("Active");
                        literals.Add("Show Active Only");
                        literals.Add("No Records Found!");
                        literals.Add("Last Page");
                        literals.Add("Next Page");
                        literals.Add("Previous Page");
                        literals.Add("First Page");
                        literals.Add("Add New");
                        literals.Add("Edit");
                        literals.Add("Reset");
                        literals.Add("Search By");
                        literals.Add("Are you sure you want to delete?");
                        literals.Add("Are you sure you want to reset?");
                        literals.Add("Please de-select the checkbox to add new/edit record");
                        literals.Add("Select All");
                        literals.Add("Active Only");
                        literals.Add("Active Flag");
                        literals.Add("Site Id");
                        literals.Add("GUID");
                        literals.Add("Synch Status");
                        literals.Add("Last Updated User");
                        literals.Add("Last Updated Date");
                        literals.Add("Last Update User");
                        literals.Add("Last Update Date");
                        literals.Add("Last Updated By");
                        literals.Add("Export To Excel");
                        literals.Add("Import Machines");
                        literals.Add("Publish To Sites");
                        literals.Add("Filter");
                        literals.Add("Show Active Entries");
                        literals.Add("Action");
                        literals.Add("Max &1 Characters");
                        literals.Add("Max &1 Digits");
                        literals.Add("Click Here");
                        literals.Add("Save & Continue");
                        literals.Add("Advance Search!");
                        literals.Add("Select a record to see more actions");
                        literals.Add("Back");
                        literals.Add("End");
                        literals.Add("Name");
                        literals.Add("Start Time");
                        literals.Add("End Time");
                        literals.Add("Start Date");
                        literals.Add("End Date");
                        literals.Add("New");
                        literals.Add("Id");
                        literals.Add("No Records Found!");
                        literals.Add("No Data");
                        literals.Add("Advance Search!");
                        literals.Add("Selected");
                        literals.Add("Custom Attributes for &1");
                        literals.Add("Items per page");
                        literals.Add("Confirmation Inactivation.");
                        literals.Add("Do you want to continue without saving ?");
                        literals.Add("Click Here");                        
                        literals.Add("Preview");
                        literals.Add("OK");
                        literals.Add("Cancel");
                        break;
                    case MaintenanceEntityLocalization.ASSETTYPE:
                        literals.Add("Asset Type");
                        literals.Add("Name");
                        literals.Add("Type Id");
                        literals.Add("Type Name");
                        break;
                    case MaintenanceEntityLocalization.ASSETGROUP:
                        literals.Add("Asset Group");
                        literals.Add("Asset Type");
                        literals.Add("Name");
                        literals.Add("Group Id");
                        literals.Add("Group Name");
                        break;
                    case MaintenanceEntityLocalization.ASSETDETAILED:
                        literals.Add("Generic Asset");
                        literals.Add("Asset Type");
                        literals.Add("Name");
                        literals.Add("URN");
                        literals.Add("Asset Status");
                        literals.Add("Location");
                        literals.Add("Asset Id");
                        literals.Add("Asset Name");
                        literals.Add("Description");
                        literals.Add("Machine Id");
                        literals.Add("Purchase Date");
                        literals.Add("Sale Date");
                        literals.Add("Scrap Date");
                        literals.Add("Tax Type");
                        literals.Add("Purchase Value");
                        literals.Add("Sale Value");
                        literals.Add("Scrap Value");
                        break;
                    case MaintenanceEntityLocalization.ASSETLIMITED:
                        literals.Add("Generic Asset");
                        literals.Add("Asset type");
                        literals.Add("Name");
                        literals.Add("URN");
                        literals.Add("Asset Status");
                        literals.Add("Location");
                        literals.Add("Asset Id");
                        literals.Add("Asset Name");
                        literals.Add("Description");
                        literals.Add("Machine Id");
                        literals.Add("Asset Type");
                        literals.Add("Asset Status");
                        break;
                    case MaintenanceEntityLocalization.ASSETGROUPASSET:
                        literals.Add("Asset Group Asset");
                        literals.Add("Asset Name");
                        literals.Add("Group Name");
                        literals.Add("Asset Group Asset Id");
                        literals.Add("Asset Group");
                        literals.Add("Asset");
                        break;
                    case MaintenanceEntityLocalization.TASKGROUP:
                        literals.Add("Maintenance Task Group");
                        literals.Add("Group Name");
                        literals.Add("Group Id");
                        break;
                    case MaintenanceEntityLocalization.MAINTENANCETASK:
                        literals.Add("Maintenance Task");
                        literals.Add("Task Id");
                        literals.Add("Task Name");
                        literals.Add("Task Group");
                        literals.Add("Validate Tag?");
                        literals.Add("Card Number");
                        literals.Add("Remarks Mandatory?");
                        break;
                    case MaintenanceEntityLocalization.SCHEDULECALENDER:
                        literals.Add("Schedule Calender");
                        literals.Add("Day");
                        literals.Add("Week");
                        literals.Add("All");
                        literals.Add("Schedule Id");
                        literals.Add("Schedule Name");
                        literals.Add("Schedule Time");
                        literals.Add("Recur Flag");
                        literals.Add("Recur Frequency");
                        literals.Add("End Date");
                        literals.Add("Recur Type");
                        literals.Add("Change Date");
                        literals.Add("Schedule");
                        literals.Add("Complete In Days");
                        literals.Add("Please select assigned to");
                        break;
                    case MaintenanceEntityLocalization.SCHEDULE:
                        literals.Add("Schedule Name");
                        literals.Add("Schedule Date");
                        literals.Add("Assigned To");
                        literals.Add("Complete In");
                        literals.Add("Days");
                        literals.Add("Time");
                        literals.Add("Recurrence");
                        literals.Add("Recur Flag");
                        literals.Add("Daily");
                        literals.Add("Weekly");
                        literals.Add("Monthly");
                        literals.Add("End Date");
                        literals.Add("MaintSch Asset Task Id");
                        literals.Add("Asset Group");
                        literals.Add("Asset Type");
                        literals.Add("Asset");
                        literals.Add("Maint Task Group");
                        literals.Add("Maint Task");
                        literals.Add("Recurrence Type");
                        literals.Add("Day");
                        literals.Add("Week Day");
                        literals.Add("Job Task Group");
                        literals.Add("Job Task");
                        literals.Add("Job Schedule Task");
                        literals.Add("Schedule");
                        literals.Add("Incl/Excl Days");
                        literals.Add("Schedule Exclusion");
                        literals.Add("Schedule Name");
                        literals.Add("Exclusion Id");
                        literals.Add("Exclusion Date");
                        literals.Add("Include Date");
                        literals.Add("Complete In Days");
                        literals.Add("Please select assigned to");
                        break;
                    case MaintenanceEntityLocalization.CREATEADHOCJOB:
                        literals.Add("Asset and Assignment");
                        literals.Add("Asset Name");
                        literals.Add("Assigned To");
                        literals.Add("Complete In");
                        literals.Add("Task Name");
                        literals.Add("Validate Tag?");
                        literals.Add("Task Card Number");
                        literals.Add("Remarks Mandatory?");
                        literals.Add("Create Adhoc Job");
                        literals.Add("Complete In Days");
                        literals.Add("Task Details");
                        break;
                    case MaintenanceEntityLocalization.MAINTENANCEJOBDETAILS:
                        literals.Add("Maintenance Job Details");
                        literals.Add("Asset Name");
                        literals.Add("Task Name");
                        literals.Add("Assigned To");
                        literals.Add("Status");
                        literals.Add("Jobs Past Due Date");
                        literals.Add("Schedule From");
                        literals.Add("Schedule To");
                        literals.Add("Task Name");
                        literals.Add("Asset Name");
                        literals.Add("Checked?");
                        literals.Add("Remarks");
                        literals.Add("Validate?");
                        literals.Add("Card Number");
                        literals.Add("Status");
                        literals.Add("Assigned User");
                        literals.Add("Raise Service Request");
                        literals.Add("Maintenance Requests");
                        literals.Add("Asset");
                        literals.Add("Status");
                        literals.Add("Request Type");
                        literals.Add("Title");
                        literals.Add("Priority");
                        literals.Add("Schedule Date From");
                        literals.Add("To");
                        literals.Add("Request Date From");
                        literals.Add("Job ID");
                        literals.Add("Asset Name");
                        literals.Add("Request Title");
                        literals.Add("Request Date");
                        literals.Add("Schedule Date");
                        literals.Add("Request Detail");
                        literals.Add("Resolution");
                        literals.Add("Comments");
                        literals.Add("Status");
                        literals.Add("Image Name");
                        literals.Add("Requested By");
                        literals.Add("Assigned To");
                        literals.Add("Contact Phone");
                        literals.Add("Contact Email Id");
                        literals.Add("Remarks");
                        literals.Add("Request Type");
                        literals.Add("Repair Cost");
                        literals.Add("Doc File Name");
                        literals.Add("Close Date");
                        literals.Add("Contact Person");
                        literals.Add("Phone");
                        literals.Add("Email");
                        literals.Add("Request");
                        break;
                    case MaintenanceEntityLocalization.MAINTENANCEREQUESTS:
                        literals.Add("Maintenance Requests");
                        literals.Add("Asset");
                        literals.Add("Status");
                        literals.Add("Request Type");
                        literals.Add("Title");
                        literals.Add("Priority");
                        literals.Add("Schedule Date From");
                        literals.Add("To");
                        literals.Add("Request Date From");
                        literals.Add("Job ID");
                        literals.Add("Asset Name");
                        literals.Add("Request Title");
                        literals.Add("Request Date");
                        literals.Add("Schedule Date");
                        literals.Add("Request Detail");
                        literals.Add("Resolution");
                        literals.Add("Comments");
                        literals.Add("Status");
                        literals.Add("Image Name");
                        literals.Add("Requested By");
                        literals.Add("Assigned To");
                        literals.Add("Contact Phone");
                        literals.Add("Contact Email Id");
                        literals.Add("Remarks");
                        literals.Add("Request Type");
                        literals.Add("Repair Cost");
                        literals.Add("Doc File Name");
                        literals.Add("Close Date");
                        literals.Add("Contact Person");
                        literals.Add("Phone");
                        literals.Add("Email");
                        literals.Add("Request");
                        literals.Add("Set Machine Out Of Service");
                        literals.Add("Upload File");
                        literals.Add("Upload Image");
                        break;
                    case MaintenanceEntityLocalization.EMAIL:
                        literals.Add("To");
                        literals.Add("CC");
                        literals.Add("Reply To");
                        literals.Add("Subject");
                        literals.Add("Body");
                        literals.Add("Attachment");
                        literals.Add("Attach");
                        literals.Add("Send");
                        literals.Add("Send Email");
                        break;
                }
                log.LogMethodExit();
                return literals;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
