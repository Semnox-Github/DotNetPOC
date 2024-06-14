/********************************************************************************************
 * Project Name - Game Module
 * Description  - All Formate types and Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70        22-Aug-2019   Jagan Mohana Rao          Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Helpers
{
    public class AllFormates
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for Games Locallization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public AllFormates(ExecutionContext executionContext, string entityName = null)
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
            string localizedValue = "";
            foreach (string literalsOrMessages in literalsOrMessageList)
            {
                if (entity == "CommonMessages")
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
            entities.Add("CommonMessages");
            entities.Add("CommonLiterals");
            List<string> literalsOrMessage = new List<string>();
            foreach (string entity in entities)
            {
                if (entity == "CommonMessages")
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
                    messages.Add("122");   // Save Successful 
                    messages.Add("212");   // Customer with this Unique ID already exists. Customer Name: &1
                    messages.Add("290");   //  Duplicate Unique Id
                    messages.Add("371");   //  Nothing to Save
                    messages.Add("539");
                    messages.Add("541"); // Are your sure you want to configure this hub? Address and Frequency will be reconfigured.
                    messages.Add("544");  // Cannot delete Games that are in use on Machines.
                    messages.Add("545"); // Cannot delete Hubs that have associated Machines.
                    messages.Add("548");   // Card updated successfully
                    messages.Add("549");  //Cards Added Successfully
                    messages.Add("550");  //Cards Reduced Successfully 
                    messages.Add("559"); // Custom Attributes for &1  not defined
                    messages.Add("566"); // Do you want to save the changes?
                    messages.Add("571"); //End Time should be greater than Start Time
                    messages.Add("576");
                    messages.Add("577");
                    messages.Add("579"); // Enter Machine Name
                    messages.Add("582");
                    messages.Add("583");
                    messages.Add("587");  //Error updating cards. Please enter valid values
                    messages.Add("588");  //Error updating cards. Please try again
                    messages.Add("594"); //Image File Error
                    messages.Add("596"); // Invalid Category-ParentCategory association. Make sure there are no loops.
                    messages.Add("601");
                    messages.Add("602");
                    messages.Add("611");  //Invalid value for discount percentage or discount amount
                    messages.Add("627");
                    messages.Add("628");
                    messages.Add("638");
                    messages.Add("640");
                    messages.Add("643"); // Please authenticate before purging data
                    messages.Add("644");
                    messages.Add("646");   // Please enter a number
                    messages.Add("647");   // Please enter a number >= 0
                    messages.Add("648");
                    messages.Add("652"); // Please enter unique Game Names
                    messages.Add("653"); // Please enter unique Game Profiles
                    messages.Add("654"); // Please enter unique Hub Names / Port - Mac - IP - Server - Active combinations
                    messages.Add("655");
                    messages.Add("659");
                    messages.Add("664"); //Please save changes before changing options
                    messages.Add("665"); //Please save changes before this operation
                    messages.Add("682");   //   Please use a NEW card for manual issue
                    messages.Add("688");  // Product saved successfully
                    messages.Add("691"); // published successfully
                    messages.Add("694");
                    messages.Add("706");
                    messages.Add("718");   // There was an error while saving. Please retry.
                    messages.Add("728");
                    messages.Add("741");
                    messages.Add("882");   // No valid data found in uploaded xls                    
                    messages.Add("957");   // Rows deleted succesfully
                    messages.Add("958");  //Deleting the existing record is not allowed. Do you want make record inactive?
                    messages.Add("959");   //  No rows selected. Please select the rows before clicking delete.
                    messages.Add("1000"); // Inventory Segments are not defined
                    messages.Add("1016"); // Barcode added successfully
                    messages.Add("1017"); // Please save Product before adding SKU.
                    messages.Add("1124"); //Role and assigned manager role can not be same
                    messages.Add("1125");
                    messages.Add("1134");   // Please save the record.
                    messages.Add("1137"); // Wake up on LAN command was sent successfully to &1.
                    messages.Add("1141"); // Shutdown command is sent
                    messages.Add("1144"); // Please enter valid value for &1
                    messages.Add("1151"); // Media Saved Successfully
                    messages.Add("1172"); // Restart command is sent to : &1
                    messages.Add("1200"); // Please Enter Device Name.
                    messages.Add("1201"); // Please Enter Device Type.
                    messages.Add("1202"); // Please Enter Device mode.
                    messages.Add("1250"); // Unexpected error while fetching Product Cost from BOM 
                    messages.Add("1309"); // Please select a game profile.
                    messages.Add("1328"); // Once the record is saved it cannot be edited
                    messages.Add("1329"); // There exists a configuration setup for using the Original transaction number for refund.Please check the Transaction setup
                    messages.Add("1330"); // There exists a mapping for the selected values
                    messages.Add("1331"); // Please select a valid invoice sequence
                    messages.Add("1332"); // The selected invoice series is expired
                    messages.Add("1357");
                    messages.Add("1358");
                    messages.Add("1359");
                    messages.Add("1360"); // Purge process will begin during maintenance hour.
                    messages.Add("1414"); // Resolution Recorded with Success
                    messages.Add("1424");
                    messages.Add("1425");
                    messages.Add("1426");
                    messages.Add("1448");  // Loading... Please wait...
                    messages.Add("1480");  //Please enter membership rule name
                    messages.Add("1481");  //Please enter Unit Of Qualification Window
                    messages.Add("1482");  //Please enter Retention Window
                    messages.Add("1483");  //Please enter Unit of Retention Window
                    messages.Add("1484");  //Please select Reward Product or Reward Attribute for the reward entry
                    messages.Add("1485");  //Please select Reward Product or Reward Attribute for the reward entry. Both are not allowed on same reward entry
                    messages.Add("1486");  //Please set Reward Attribute Percentage value
                    messages.Add("1487");  //Please set Reward Function value for the attribute
                    messages.Add("1488");  //Please set Reward function period details
                    messages.Add("1489");  //Please set Reward frequence period details
                    messages.Add("1490");  //Unable to load Exclusion Rule form
                    messages.Add("1512");  //Please enter data in correct format for the field
                    messages.Add("1522");   // Please choose a valid file containing customer details. Invalid Template.
                    messages.Add("1593");   //  Nothing to export. Please change the search criteria.
                    messages.Add("1598");   // Customer has another card &1 set as Primary Card. Do you want to mark this card as primary?
                    messages.Add("1600");   //  Nothing to export.
                    messages.Add("1605"); //Ticket receipt is not from the current site
                    messages.Add("1747");
                    messages.Add("1748"); // Please enter valid details                    
                    messages.Add("1749");
                    messages.Add("1750");
                    messages.Add("1751");
                    messages.Add("1752");
                    messages.Add("1753");
                    messages.Add("1754");
                    messages.Add("1755");
                    messages.Add("1756");
                    messages.Add("1757");
                    messages.Add("1758");
                    messages.Add("1759");
                    messages.Add("1760");
                    messages.Add("1761");
                    messages.Add("1762");
                    messages.Add("1763");
                    messages.Add("1764");
                    messages.Add("1765");
                    messages.Add("1766");
                    messages.Add("1769"); // Do you want to shutdown the  computer : &1
                    messages.Add("1770"); // Are you sure you want to reset?
                    messages.Add("1771"); // Please enter unique event Names
                    messages.Add("1772"); // The Parameter must be a non-negative integer
                    messages.Add("1773"); // Please enter valid value for MAC Addess
                    messages.Add("1774"); // Please enter unique Media
                    messages.Add("1775"); // Please enter valid value for parameter
                    messages.Add("1776"); // Query is not valid.Please check.
                    messages.Add("1777"); // Please enter data and save
                    messages.Add("1778"); // Deleting the existing record is not allowed. Do you want make record inative?
                    messages.Add("1779"); // Please select media type
                    messages.Add("1780"); // Please choose valid file as per media type selection
                    messages.Add("1830"); // Please enter valid value for MAC Addess
                    messages.Add("1831"); // The Parameter should not be empty.
                    messages.Add("1849"); // Are you sure you want to change the baud rate? Module may be in incommunicable if not set properly.
                    messages.Add("1851"); // Please enter valid value for Discount Name
                    messages.Add("1854"); // Please de-select the checkbox to add new/edit record
                    messages.Add("1855"); // Please enter valid value for Membership Name
                    messages.Add("1856"); // Please enter valid value for Membership Reward Name
                    messages.Add("1860"); // Custom Attributes not defined
                    messages.Add("1864");
                    messages.Add("1865");
                    messages.Add("1866");
                    messages.Add("1867"); // Select a record to see more actions
                    messages.Add("1868"); //Do you want to continue without saving?
                    messages.Add("1869"); //Unable to delete this record.Please check the reference record first.
                    messages.Add("1874"); //Total discounted products amount and discount amount should be equal
                    messages.Add("1875"); //Individual discounted products percentage can not be greater than the discount percentage.
                    messages.Add("1876"); //Percentage should not exceed 100%
                    messages.Add("1877"); //Modifer Set Name and Parent Modifier set can not be same
                    messages.Add("1885"); // Barcode exists
                    messages.Add("1886"); // Successfully loaded &1 of &2 records
                    messages.Add("1887"); // Do you want to start the purge process immediately
                    messages.Add("1888"); // Purge Complete
                    messages.Add("1889"); // Past Data Purge Completed
                    messages.Add("1890"); // Invalid Licensed POS key
                    messages.Add("1891"); // Invalid features key
                    messages.Add("1892"); // Authentication is required to proceed with save
                    messages.Add("1893"); // Please enter Authkey to proceed
                    messages.Add("1894"); // Please enter Add Cards Key
                    messages.Add("1895"); // Enter a valid Add Cards Key
                    messages.Add("1896"); // Master Schedule is linked with a facility map!!
                    messages.Add("1897"); // Please enter valid value for Coupon Expiry Date
                    messages.Add("1898"); // Please enter valid value for Resolution Date
                    messages.Add("1899"); // Please enter valid value for Expiry Date
                    messages.Add("1900"); // Duplicate pos machine not allowed
                    messages.Add("1901"); // Re-enter Password does not match password
                    messages.Add("1902"); // Site data created successfully
                    messages.Add("1903"); // Master data from HQ Master Site published successfully to
                    messages.Add("1904"); // Duplicate Postype name not allowed
                    messages.Add("1905"); // Station Id length more than 2 Characters!.Do you want to proceed?
                    messages.Add("1906"); // Ticket length should be between 3 and 6!.Do you want to proceed?
                    messages.Add("1907"); // Voucher length should be between 12 and 16!.Do you want to proceed?
                    messages.Add("1908"); // Ticket Station Id should be unique for particular Site
                    messages.Add("1909"); // Please enter valid value for Order Type Group
                    messages.Add("1910"); // This will delete all data before the selected dates. Do you want to proceed?
                    messages.Add("1911"); // Do you want to delete this Policy?
                    messages.Add("1912"); // Mac address already exists, Please enter unique mac address
                    messages.Add("1913"); // Please enter unique machine address
                    messages.Add("2079"); // Category selection is mandatory to create Inventory details. Please select category and save to proceed
                    messages.Add("2083"); // Description field is mandatory to proceed with inventory product creation
                    messages.Add("275");
                    messages.Add("274");
                    messages.Add("737");
                    messages.Add("2732"); // added 15-07-2020
                    messages.Add("2748");
                    break;
            }
            return messages;
        }

        private List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            List<string> literals = new List<string>();
            switch (entityName.ToUpper().ToString())
            {
                case "COMMONLITERALS":
                    literals.Add("Remarks");
                    literals.Add("File Format");
                    literals.Add("Clear");
                    literals.Add("Upload");
                    literals.Add("Bulk Upload");
                    literals.Add("Get By Status");
                    literals.Add("Publish To Sites");
                    literals.Add("HQ Publish"); //HQPublish
                    literals.Add("Publishing");
                    literals.Add("Publish");
                    literals.Add("Game Profiles");
                    literals.Add("Reader Configuration");
                    literals.Add("Date");
                    literals.Add("Courtesy");
                    literals.Add("Notes");
                    literals.Add("Config");
                    literals.Add("Select All");
                    literals.Add("IsActive");
                    literals.Add("Active Flag");
                    literals.Add("Theme");
                    literals.Add("Theme Calendar");
                    literals.Add("Custom");
                    literals.Add("Last Updated Date");
                    literals.Add("Last Updated User");
                    literals.Add("Save");
                    literals.Add("Refresh");
                    literals.Add("Delete");
                    literals.Add("Close");
                    literals.Add("Audit");
                    literals.Add("Database Save");
                    literals.Add("Refresh Master Data");
                    literals.Add("Close Master Data");
                    literals.Add("Data Error");
                    literals.Add("Yes");
                    literals.Add("No");
                    literals.Add("Cancel");
                    literals.Add("Last Page");
                    literals.Add("Next Page");
                    literals.Add("Previous Page");
                    literals.Add("First Page");
                    literals.Add("MAC Address");
                    literals.Add("IP Address");
                    literals.Add("TCP Port");
                    literals.Add("Add New");
                    literals.Add("Edit");
                    literals.Add("Reset");
                    literals.Add("Items per page");
                    literals.Add("Search By");
                    literals.Add("Search");
                    literals.Add("Action");
                    literals.Add("Are you sure you want to delete?");
                    literals.Add("Are you sure you want to reset?");
                    literals.Add("Selected");
                    literals.Add("Active_Flag");
                    literals.Add("created_date");
                    literals.Add("created_by");
                    literals.Add("last_updated_date");
                    literals.Add("last_updated_user");
                    literals.Add("CreatedBy");
                    literals.Add("CreationDate");
                    literals.Add("LastUpdatedBy");
                    literals.Add("LastUpdateDate");
                    literals.Add("last_updated_by");
                    literals.Add("Guid");
                    literals.Add("site_id");
                    literals.Add("SynchStatus");
                    literals.Add("MasterEntityId");
                    literals.Add("Active Only");
                    literals.Add("Search By &1");
                    literals.Add("Custom Attributes for &1");
                    literals.Add("Machines");
                    literals.Add("Reader themes");
                    literals.Add("Your session will expiry in");
                    literals.Add("You have unsaved changes! If you leave, your changes will be lost.");
                    literals.Add("Selected Site");
                    literals.Add("Select Site");
                    literals.Add("Welcome");
                    literals.Add("Logout");
                    literals.Add("Logo");
                    literals.Add("Home");
                    literals.Add("Games");
                    literals.Add("Hubs");
                    literals.Add("Game Profile");
                    literals.Add("Reader Configuration For");
                    literals.Add("Reader Hardware Version:");
                    literals.Add("Digital Signage");
                    literals.Add("Media");
                    literals.Add("Content");
                    literals.Add("List");
                    literals.Add("Event");
                    literals.Add("Setup");
                    literals.Add("Panel");
                    literals.Add("Screen Design");
                    literals.Add("Screen Transition");
                    literals.Add("Signage Schedule");
                    literals.Add("Ticker");
                    literals.Add("None");
                    literals.Add("No Records Found!");
                    literals.Add("Reader Hardware Version");
                    literals.Add("Pattern");
                    literals.Add("Product");
                    literals.Add("Advance Search!");
                    literals.Add("Back");
                    literals.Add("Max &1 Characters");
                    literals.Add("Next");
                    literals.Add("Previous");
                    literals.Add("Click Here");
                    literals.Add("Max &1 Digits");
                    literals.Add("Products");
                    literals.Add("Discounts");
                    literals.Add("Tax");
                    literals.Add("Cards");
                    literals.Add("View Cards");
                    literals.Add("New Card");
                    literals.Add("Membership");
                    literals.Add("Inventory");
                    literals.Add("ID");
                    literals.Add("Id");
                    literals.Add("Description");
                    literals.Add("Active");
                    literals.Add("Active?");
                    literals.Add("SI#");
                    literals.Add("Display Order");
                    literals.Add("Filter");
                    literals.Add("Confirmation Inactivation.");
                    literals.Add("Last updated user");
                    literals.Add("MAC Address");
                    literals.Add("Start Time");
                    literals.Add("End Time");
                    literals.Add("No Data");
                    literals.Add("First");
                    literals.Add("Last");
                    literals.Add("Advanced Search");
                    literals.Add("Is Active");
                    literals.Add("Show Active Only");
                    literals.Add("Credits");
                    literals.Add("Bonus");
                    literals.Add("Please de-select the checkbox to add new/edit record");
                    literals.Add("AND");
                    literals.Add("OR");
                    literals.Add("Field");
                    literals.Add("Condition");
                    literals.Add("Value");
                    literals.Add("Add");
                    literals.Add("Remove");
                    literals.Add("First Name");
                    literals.Add("Ok");
                    literals.Add("Name");
                    literals.Add("Image");
                    literals.Add("Last Updated By");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("Sunday");
                    literals.Add("Expiry Date");
                    literals.Add("Sort Order");
                    literals.Add("Price");
                    literals.Add("Do you want to continue without saving ?");
                    literals.Add("Save & Continue");
                    literals.Add("Select a record to see more actions");
                    literals.Add("Last Update Date");
                    literals.Add("Product Type");
                    literals.Add("Quantity");
                    literals.Add("Created By");
                    literals.Add("Creation Date");
                    literals.Add("Master Schedule");
                    literals.Add("Timestamp");
                    literals.Add("User");
                    literals.Add("Type");
                    literals.Add("Custom Attribute Data for &1");
                    literals.Add("Category");
                    literals.Add("Vendor");
                    literals.Add("UOM");
                    literals.Add("Site Id");
                    literals.Add("Synch Status");
                    literals.Add("Last Update User");
                    literals.Add("End");
                    literals.Add("Start Date");
                    literals.Add("End Date");
                    literals.Add("New");
                    literals.Add("Confirm Delete");
                    literals.Add("Preview");
                    literals.Add("Template Keywords");
                    literals.Add("Site Setup");
                    literals.Add("Configuration");
                    literals.Add("Site");
                    literals.Add("Users");
                    literals.Add("User Roles");
                    literals.Add("Task Types");
                    literals.Add("POS Management");
                    literals.Add("Print Setup");
                    literals.Add("Lookups");
                    literals.Add("Transaction Profiles");
                    literals.Add("Messages");
                    literals.Add("Custom Attributes");
                    literals.Add("ULC Keys");
                    literals.Add("Kiosk Setup");
                    literals.Add("Table Layout");
                    literals.Add("Security Policy");
                    literals.Add("Email Template");
                    literals.Add("Product Key");
                    literals.Add("Purge Data");
                    literals.Add("DB Synch");
                    literals.Add("Currency Setup");
                    literals.Add("Order Type Group");
                    literals.Add("Order Type");
                    literals.Add("Invoice Sequence Setup");
                    literals.Add("Upload");
                    literals.Add("Template");
                    literals.Add("Ticket Station");
                    literals.Add("Change Password");
                    literals.Add("Login Id");
                    literals.Add("New Password");
                    literals.Add("Re-enter");
                    literals.Add("Change");
                    break;
            }
            log.LogMethodExit(literals);
            return literals;
        }

        public Dictionary<string, string> GetFormateTypes()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, "Formats"));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParameters);
            if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count != 0)
            {
                foreach (ParafaitDefaultsDTO parafaitDefaultsDTO in parafaitDefaultsDTOList)
                {
                    if (keyValuePairs.ContainsKey(parafaitDefaultsDTO.DefaultValueName) == false)
                    {
                        keyValuePairs.Add(parafaitDefaultsDTO.DefaultValueName, parafaitDefaultsDTO.DefaultValue);
                    }
                }
            }
            return keyValuePairs;
        }
    }
}