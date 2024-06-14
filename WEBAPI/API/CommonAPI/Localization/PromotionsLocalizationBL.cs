/********************************************************************************************
 * Project Name - Promotions
 * Description  - Bussiness logic of Localization for Promotions module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.0      03-Sep-2019   Mushahid Faizan      Created
 *2.80        24-Dec-2019   Vikas Dwivedi        Added Localization in Customer
 *2.80        02-Jan-2020   Vikas Dwivedi        Added Localization 
  ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;

namespace Semnox.CommonAPI.Localization
{
    public class PromotionsLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for Promotions Localization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public PromotionsLocalizationBL(ExecutionContext executionContext, string entityName)
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
                    messages.Add("643"); // Please authenticate before purging data
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
                    messages.Add("536");
                    messages.Add("1649");
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
                    messages.Add("537"); //Are you sure you want to delete this Messaging Trigger?
                    break;
            }
            return messages;
        }
        public enum PromotionsEntityName
        {
            COMMONLITERALS,
            CUSTOMER,
            LAUNCHPROMOTIONS,
            LOYALTYMANAGEMENT,
            CAMPAIGNS,
            MESSAGEMANAGEMENT
        }

        public List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            try
            {
                List<string> literals = new List<string>();

                PromotionsEntityName promotionsEntityName = (PromotionsEntityName)Enum.Parse(typeof(PromotionsEntityName), entityName);
                switch (promotionsEntityName)
                {
                    case PromotionsEntityName.COMMONLITERALS:
                        literals.Add("Filter");
                        literals.Add("First");
                        literals.Add("Previous");
                        literals.Add("Next");
                        literals.Add("Last");
                        literals.Add("Save");
                        literals.Add("Refresh");
                        literals.Add("Delete");
                        literals.Add("Creation Date");
                        literals.Add("Close");
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
                        literals.Add("Publish To Sites");
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
                        literals.Add("Save & Continue");
                        literals.Add("Publish To Sites");
                        literals.Add("Preview");
                        literals.Add("OK");
                        literals.Add("Cancel");
                        literals.Add("Clear");
                        literals.Add("Last Played Time");
                        literals.Add("Last Updated Time");
                        literals.Add("cannot add multiple event triggers under same entry.please create new entry for the second event trigger");
                        break;
                    case PromotionsEntityName.CUSTOMER:
                        literals.Add("Customer");
                        literals.Add("Customers");
                        literals.Add("Filter");
                        literals.Add("Customer Name");
                        literals.Add("Phone");
                        literals.Add("E-Mail");
                        literals.Add("Unique Identifier");
                        literals.Add("Membership");
                        literals.Add("Site");
                        literals.Add("Search");
                        literals.Add("Advanced Search");
                        literals.Add("Customer Id");
                        literals.Add("Title");
                        literals.Add("First Name");
                        literals.Add("Name");
                        literals.Add("Middle Name");
                        literals.Add("Play Pass#");
                        literals.Add("Tax Code");
                        literals.Add("Team User");
                        literals.Add("Verified");
                        literals.Add("External System Reference");
                        literals.Add("Channel");
                        literals.Add("Custom");
                        literals.Add("Addresses");
                        literals.Add("Address Type");
                        literals.Add("Active?");
                        literals.Add("Contacts");
                        literals.Add("Contact Type");
                        literals.Add("Attribute1");
                        literals.Add("Attribute2");
                        literals.Add("Customer Cards");
                        literals.Add("Issue Date");
                        literals.Add("Play Points");
                        literals.Add("Courtesy");
                        literals.Add("Bonus");
                        literals.Add("Ticket Count");
                        literals.Add("Real Ticket?");
                        literals.Add("VIP?");
                        literals.Add("Valid?");
                        literals.Add("Face Value");
                        literals.Add("Refund?");
                        literals.Add("Refund Amount");
                        literals.Add("Refund Date");
                        literals.Add("Notes");
                        literals.Add("Last Update Time");
                        literals.Add("Customer Relationship");
                        literals.Add("Relationship Type");
                        literals.Add("New Relationship");
                        literals.Add("Related Customer");
                        literals.Add("Effective Date");
                        literals.Add("Expiry Date");
                        literals.Add("Relationships");
                        literals.Add("Id");
                        literals.Add("Customer");
                        literals.Add("Related Customer");
                        literals.Add("Customer Relationship Type");
                        literals.Add("Details");
                        literals.Add("Update Membership");
                        literals.Add("Import");
                        literals.Add("Export");
                        literals.Add("Export Customer Details");
                        literals.Add("Import Customers");
                        literals.Add("Template");
                        literals.Add("Upload");
                        literals.Add("Customer Name");
                        literals.Add("Current Membership");
                        literals.Add("Assign Membership");
                        literals.Add("Associated Cards");
                        literals.Add("Customer Associated Cards");
                        literals.Add("Cards View");
                        literals.Add("Card Id");
                        literals.Add("Card Number");
                        literals.Add("Credits");
                        literals.Add("Credit Plus");
                        literals.Add("Primary Card");
                        literals.Add("Courtsey");
                        literals.Add("Bonus");
                        literals.Add("Real Ticket Mode");
                        literals.Add("Issue Date");
                        literals.Add("Vip Customer");
                        literals.Add("Ticket Allowed");
                        literals.Add("Valid Flag");
                        literals.Add("Refund Flag");
                        literals.Add("Deposit");
                        literals.Add("Credits Played");
                        literals.Add("Refund Amount");
                        literals.Add("Loyalty Points");
                        literals.Add("Ticket Count");
                        literals.Add("Time");
                        literals.Add("Refund Date");
                        literals.Add("Notes");
                        literals.Add("Last Name");
                        literals.Add("Address");
                        literals.Add("Contact");
                        literals.Add("Profile");
                        literals.Add("Save profile details");
                        break;

                    case PromotionsEntityName.LAUNCHPROMOTIONS:
                        literals.Add("Launch Promotions");
                        literals.Add("Promotion Calendar");
                        literals.Add("Change Date");
                        literals.Add("New Product Promotion");
                        literals.Add("New Game Play Promotion");
                        literals.Add("Close");
                        literals.Add("Day");
                        literals.Add("Week");
                        literals.Add("All");
                        literals.Add("Promotion Type");
                        literals.Add("Promotion Id");
                        literals.Add("Promotion Name");
                        literals.Add("Time From");
                        literals.Add("Time To");
                        literals.Add("Recurring?");
                        literals.Add("Recur Frequency");
                        literals.Add("Monthly Recur Type");
                        literals.Add("Recur End Date");
                        //  literals.Add("PROMOTIONS");
                        literals.Add("Promotions");
                        literals.Add("Recur");
                        literals.Add("Recurrence");
                        literals.Add("Daily");
                        literals.Add("Weekly");
                        literals.Add("Monthly");
                        literals.Add("End By");
                        literals.Add("Date");
                        literals.Add("Weekday");
                        literals.Add("Membership");
                        literals.Add("X");
                        literals.Add("Campaign");
                        literals.Add("Promotion Details");
                        literals.Add("Category");
                        literals.Add("Product");
                        literals.Add("Discounted Price");
                        literals.Add("Discount%");
                        literals.Add("Discount Amount");
                        literals.Add("Visualization Theme");
                        literals.Add("incl. / Excl. Dates");
                        literals.Add("Delete Promotion");
                        literals.Add("Delete Detail");
                        literals.Add("Promotion - Include / Exclude Days");
                        literals.Add("Include / Exclude Days from Promotion");
                        literals.Add("ID");
                        literals.Add("Date");
                        literals.Add("Day");
                        literals.Add("Include This Day?");
                        literals.Add("Remarks");
                        literals.Add("Game Profile");
                        literals.Add("Game Name");
                        literals.Add("Promo Credits");
                        literals.Add("Promo VIP Credits");
                        literals.Add("Disc % on Credits");
                        literals.Add("Disc % on VIP Credits");
                        literals.Add("Bonus Allowed");
                        literals.Add("Courtesy Allowed");
                        literals.Add("Time Allowed");
                        literals.Add("Ticket Allowed");
                        literals.Add("Display Theme");
                        literals.Add("Visualization Theme");
                        literals.Add("Start Time");
                        literals.Add("End Time");
                        break;
                    case PromotionsEntityName.LOYALTYMANAGEMENT:
                        literals.Add("Loyalty Management");
                        literals.Add("Loyalty Point Redemption Rules");
                        literals.Add("Not Expired");
                        literals.Add("VIP");
                        literals.Add("Type");
                        literals.Add("First Name");
                        literals.Add("Name");
                        literals.Add("Minimum Used Credits");
                        literals.Add("Maximum Used Credits");
                        literals.Add("Minimum Sale Amount");
                        literals.Add("Maximum Sale Amount");
                        literals.Add("Expiry Date");
                        literals.Add("VIP Only");
                        literals.Add("Instances");
                        literals.Add("First Instances Only");
                        literals.Add("On Different Days");
                        literals.Add("Period From");
                        literals.Add("Period To");
                        literals.Add("Time From");
                        literals.Add("Time To");
                        literals.Add("Monday");
                        literals.Add("Tuesday");
                        literals.Add("Wednesday");
                        literals.Add("Thursday");
                        literals.Add("Friday");
                        literals.Add("Saturday");
                        literals.Add("Sunday");
                        literals.Add("Number Of Days");
                        literals.Add("Membership");
                        literals.Add("Customer Count");
                        literals.Add("Customer Count Type");
                        literals.Add("Launch Loyalty Program");
                        literals.Add("On Card Purchase / Recharge");
                        literals.Add("On Purchase Of Counter Items");
                        literals.Add("On Game Play");
                        literals.Add("Launch");
                        literals.Add("( Applicable on purchase of card products, i.e., on new card purchase or recharge of cards bonus balance or specific counter items with promotional prices could be credited to cards. The additional credit could be % of transaction amount, or an absolute amount)");
                        literals.Add("( Purchase of Counter Items , External Payment Gateway transactions qualify for this type of Loyalty )");
                        literals.Add("( Applicable on game play on machines )");
                        literals.Add("Redemption Rules");
                        literals.Add("Id");
                        literals.Add("Loyalty Points");
                        literals.Add("Redemption Value");
                        literals.Add("Redemption Attribute");
                        literals.Add("Minimum Points");
                        literals.Add("Multiples Only");
                        literals.Add("Expiry Date");
                        literals.Add("Loyalty Promotions - On Card Purchase");
                        literals.Add("Promotion Name");
                        literals.Add("Qualifying Criteria");
                        literals.Add("Promo Name");
                        literals.Add("Expires on");
                        literals.Add("Only For VIP Customers");
                        literals.Add("For Membership");
                        literals.Add("For the first");
                        literals.Add("Eligible Customers");
                        literals.Add("th Customer");
                        literals.Add("have spent between");
                        literals.Add("and");
                        literals.Add("doing a Recharge between");
                        literals.Add("in");
                        literals.Add("recharge instances ( 0 = All )");
                        literals.Add("On Different Days");
                        literals.Add("excluding new card issue");
                        literals.Add("Reward Count");
                        literals.Add("and consider only First recharge instances after card purchase");
                        literals.Add("days");
                        literals.Add("days after first purchase");
                        literals.Add("from date");
                        literals.Add("to");
                        literals.Add("between hours");
                        literals.Add("on Monday");
                        literals.Add("Apply Immediate");
                        literals.Add("Include / Exclude Recharge Products");
                        literals.Add("Category");
                        literals.Add("Product");
                        literals.Add("Exclude?");
                        literals.Add("X");
                        literals.Add("Rewards");
                        literals.Add("Attribute");
                        literals.Add("Reward Amount");
                        literals.Add("or");
                        literals.Add("% of");
                        literals.Add("Valid After");
                        literals.Add("for");
                        literals.Add("Auto Extend on Reload");
                        literals.Add("For Membership Only");
                        literals.Add("on a minimum purchase of");
                        literals.Add("On purchase of these Menu Items (Purchase Criteria for claiming reward at Counter)");
                        literals.Add("Reward applicable on these Menu Items or Games (Reward Criteria)");
                        literals.Add("Counter");
                        literals.Add("Game Profile");
                        literals.Add("Game Id");
                        literals.Add("Game");
                        literals.Add("Disc. %");
                        literals.Add("Discounted Price");
                        literals.Add("New Promotion");
                        literals.Add("Delete Promotion");
                        literals.Add("Include / Exclude Game Profiles / Games");
                        literals.Add("Disc. Amount");
                        literals.Add("Include / Exclude Specific Products");
                        literals.Add("and consider only First purchase instances after card purchase");
                        literals.Add("and consider only First game play instances after card purchase");

                        break;
                    case PromotionsEntityName.CAMPAIGNS:
                        literals.Add("Campaigns");
                        literals.Add("Id");
                        literals.Add("First Name");
                        literals.Add("Name");
                        literals.Add("Description");
                        literals.Add("Start Date");
                        literals.Add("End Date");
                        literals.Add("Communication Mode");
                        literals.Add("Message Template");
                        literals.Add("Campaign Customers");
                        literals.Add("Membership");
                        literals.Add("Total Top-up");
                        literals.Add("Total Add Point");
                        literals.Add("Total Spend");
                        literals.Add("Birthday In Next Days");                        
                        literals.Add("Adv Customer Criteria");
                        literals.Add("Adv Card Criteria");
                        literals.Add("Add To Campaign");                        
                        literals.Add("Customers In Campaign &Count");
                        literals.Add("Remove Selected");
                        literals.Add("Remove All");
                        literals.Add("Send Email / SMS");
                        literals.Add("First Name");
                        literals.Add("Name");
                        literals.Add("Play Pass#");
                        literals.Add("Email");
                        literals.Add("Contact Phone1");
                        literals.Add("BirthDate");
                        literals.Add("Name");
                        literals.Add("Card Number");
                        literals.Add("Email Sent Date");
                        literals.Add("Email Status");
                        literals.Add("SMS Sent Date");
                        literals.Add("SMS Status");
                        literals.Add("Message");
                        literals.Add("Customer Found");
                        literals.Add("Campaign - Send Email / SMS");
                        literals.Add("Email");
                        literals.Add("Customers With Email Ids");
                        literals.Add("Email Subject");
                        literals.Add("SMS");
                        literals.Add("Customers With Phone Nos");
                        literals.Add("Email Subject");
                        literals.Add("No. Prefix");
                        literals.Add("Send Email");
                        literals.Add("Send SMS");
                        literals.Add("Batch Size");
                        literals.Add("Log");
                        literals.Add("Campaign Message Template");
                        literals.Add("Max 160 for SMS");
                        literals.Add("Communication Mode not defined");
                        literals.Add("Campaign Name:");
                        literals.Add("Field");
                        literals.Add("Condition");
                        literals.Add("Value");
                        break;
                    case PromotionsEntityName.MESSAGEMANAGEMENT:
                        literals.Add("Message Management");
                        literals.Add("Id");
                        literals.Add("Trigger Name");
                        literals.Add("VIP Only");
                        literals.Add("Message Type");
                        literals.Add("SMS Template");
                        literals.Add("Email Subject");
                        literals.Add("Email Template");
                        literals.Add("Minimum Sale Amount");
                        literals.Add("Minimum Message Count");
                        literals.Add("Message Customer");
                        literals.Add("SMS Numbers");
                        literals.Add("Email Ids");
                        literals.Add("Send Receipt");
                        literals.Add("Receipt Template Id");
                        literals.Add("Time Stamp");
                        literals.Add("Trigger Name");
                        literals.Add("Trigger Type");
                        literals.Add("Min. Sale Amt.");
                        literals.Add("Send Message to Customer?");
                        literals.Add("Message Type");
                        literals.Add("SMS Template");
                        literals.Add("Receipt Template");
                        literals.Add("Sale Product");
                        literals.Add("Exclude?");
                        literals.Add("New Trigger");
                        literals.Add("TypeCode");
                        literals.Add("Campaign Message Template");
                        literals.Add("MinimumTicketCount");
                        break;
                }
                log.LogMethodExit(literals);
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