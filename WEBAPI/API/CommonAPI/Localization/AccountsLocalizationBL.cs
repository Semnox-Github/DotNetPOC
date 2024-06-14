/********************************************************************************************
 * Project Name - Cards Module
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        25-Feb-2019   Mushahid Faizan          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Localization
{
    public class AccountsLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();
        /// <summary>
        ///   Default Constructor for Cards Localization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public AccountsLocalizationBL(ExecutionContext executionContext, string entityName)
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
                localizedValue = MessageContainerList.GetMessage(executionContext, literalsOrMessages);
                if (!listHeadersList.ContainsKey(literalsOrMessages))
                {
                    listHeadersList.Add(literalsOrMessages, localizedValue);
                }
            }
        }
        public string GetLocalizedLabelsAndHeaders()
        {
            log.LogMethodEntry();
            string literalsMessagesList = string.Empty;
            if (!string.IsNullOrEmpty(entityName))
            {
                List<string> literalsOrMessage = GetLiterals(entityName);
                GetLiteralsAndMessages(entityName, literalsOrMessage);
                if (listHeadersList.Count != 0)
                {
                    literalsMessagesList = JsonConvert.SerializeObject(listHeadersList, Formatting.Indented);
                }
            }
            log.LogMethodExit(literalsMessagesList);
            return literalsMessagesList;
        }

        public enum AccountsEntityNameLookup
        {
            VIEWCARDS,
            CARDSCREDITPLUS,
            CARDGAMES,
            CARDDISCOUNTS,
            MEMBERSHIP,
            MEMBERSHIPRULES,
            MEMBERSHIPEXCLUSIONRULES,
            CARDSMANAGEMENT,
            BULKUPLOADCARDS,
            CARDACTIVITY,
            AUDITTRAIL,
            CUSTOMERDETAILS,
            CUSTOMERLOOKUP,
            ADDTOCARD,
            IDPROOF
        }
        public List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);

            try
            {
                log.LogMethodEntry();
                List<string> literals = new List<string>();
                AccountsEntityNameLookup accountsEntityNameLookupValue = (AccountsEntityNameLookup)Enum.Parse(typeof(AccountsEntityNameLookup), entityName);
                switch (accountsEntityNameLookupValue)
                {                    
                    case AccountsEntityNameLookup.VIEWCARDS:
                        literals.Add("View Cards Details");
                        literals.Add("Issue New Card");
                        literals.Add("New Card");
                        literals.Add("Update Card");
                        literals.Add("View Cards");
                        literals.Add("Valid Cards Only");
                        literals.Add("VIP Customer");
                        literals.Add("Card Number");
                        literals.Add("Issue Date");
                        literals.Add("Incl. Roaming Cards");
                        literals.Add("Technician Cards");
                        literals.Add("Customer Name");
                        literals.Add("Advanced");
                        literals.Add("Clear");
                        literals.Add("Page 0 of 0");
                        literals.Add("Cards");
                        literals.Add("Card Id");
                        literals.Add("Card Number");
                        literals.Add("Customer");
                        literals.Add("Issue Date");
                        literals.Add("Membership");
                        literals.Add("Deposit");
                        literals.Add("Time");
                        literals.Add("Ticket Count");
                        literals.Add("Loyalty Points");
                        literals.Add("Credits Played");
                        literals.Add("CreditPlus");
                        literals.Add("Real Ticket Mode");
                        literals.Add("VIP Customer");
                        literals.Add("Ticket Allowed");
                        literals.Add("Tech Card?");
                        literals.Add("Timer Reset Card");
                        literals.Add("Tech Games");
                        literals.Add("Valid Flag");
                        literals.Add("Refund Flag");
                        literals.Add("Refund Amount");
                        literals.Add("Refund Date");
                        literals.Add("Expiry Date");
                        literals.Add("Start Time");
                        literals.Add("Last Played Time");
                        literals.Add("Notes");
                        literals.Add("Primary Card");
                        literals.Add("Card Identifier");
                        literals.Add("Site Id");
                        literals.Add("Last Update Time");
                        literals.Add("Last Updated By");
                        //literals.Add("Update Card");
                        literals.Add("New Card");
                        literals.Add("Card Activity");
                        literals.Add("Customer");
                        literals.Add("Export To Excel");
                        literals.Add("Value");
                        literals.Add("Tech Games #");
                        literals.Add("Main");
                        literals.Add("Games");
                        literals.Add("Discounts");
                        literals.Add("Add To Card");
                        literals.Add("Bulk Upload");
                        literals.Add("Activites");
                        literals.Add("Audit Trail");
                        literals.Add("Refund / Expiry");
                        literals.Add("Miscellaneous");
                        literals.Add("Ticket");
                        literals.Add("Game Time");
                        literals.Add("Refunded");
                        literals.Add("Technician");
                        literals.Add("Tech Card Type");
                        literals.Add("Face Value");
                        literals.Add("Select");
                        literals.Add("Credit Plus");
                        literals.Add("End");
                        literals.Add("Rs");
                        literals.Add("To");
                        literals.Add("Last Play Time");
                        break;
                    case AccountsEntityNameLookup.CARDSCREDITPLUS:
                        literals.Add("Extended Credits (CreditPlus)");
                        literals.Add("CreditPlus Summary");
                        literals.Add("Card Balance");
                        literals.Add("Cards CreditPlus Details");
                        literals.Add("Item Purchase");
                        literals.Add("Tickets");
                        literals.Add("Refundable");
                        literals.Add("Loyalty");
                        literals.Add("Card Credit Plus");
                        literals.Add("CreditPlus Type");
                        literals.Add("CreditPlus Loaded");
                        literals.Add("CreditPlus Balance");
                        literals.Add("Period From");
                        literals.Add("Period To");
                        literals.Add("Extend On Reload");
                        //literals.Add("Refundable");
                        literals.Add("Time From");
                        literals.Add("Time To");
                        literals.Add("Monday");
                        literals.Add("Tuesday");
                        literals.Add("Wednesday");
                        literals.Add("Thursday");
                        literals.Add("Friday");
                        literals.Add("Saturday");
                        literals.Add("Sunday");
                        literals.Add("Ticket Allowed");
                        literals.Add("Pause Allowed");
                        literals.Add("Remarks");
                        literals.Add("Expire With Membership");
                        literals.Add("Credits Plus Consumption by POS / Game");
                        literals.Add("POS Counter");
                        literals.Add("Category");
                        literals.Add("Product");
                        literals.Add("Order Type");
                        literals.Add("Game Profile");
                        literals.Add("Games");
                        literals.Add("Discount Percentage");
                        literals.Add("Discount Amount");
                        literals.Add("Discounted Price");
                        literals.Add("Balance");
                        literals.Add("Daily Limit");
                        break;
                    case AccountsEntityNameLookup.CARDGAMES:
                        literals.Add("Add or Remove Games / Entitlements from Card");
                        literals.Add("Card Games");
                        literals.Add("Card Games Details");
                        literals.Add("Game Profile");
                        literals.Add("Game");
                        literals.Add("Frequency");
                        literals.Add("Play Count / Entt. Value");
                        literals.Add("Balance Games");
                        literals.Add("Ticket Allowed");
                        literals.Add("From Date");
                        literals.Add("Expiry Date");
                        literals.Add("Monday");
                        literals.Add("Tuesday");
                        literals.Add("Wednesday");
                        literals.Add("Thursday");
                        literals.Add("Friday");
                        literals.Add("Saturday");
                        literals.Add("Sunday");
                        literals.Add("Entitlement Type");
                        literals.Add("Last Played Time");
                        literals.Add("Optional Attribute");
                        literals.Add("Custom");
                        literals.Add("Expire With Membership");
                        literals.Add("Extended Inclusion / Exclusions");
                        literals.Add("Game Profile");
                        literals.Add("Game");
                        literals.Add("Exclude?");
                        literals.Add("Play Limit Per Game");
                        break;
                    case AccountsEntityNameLookup.CARDDISCOUNTS:
                        literals.Add("Card Discounts");
                        literals.Add("Card Discounts Details");
                        literals.Add("Discount");
                        literals.Add("Discounts Available for Card");
                        literals.Add("Id");
                        literals.Add("Discount Name");
                        literals.Add("Expiry Date");
                        literals.Add("Last Updated User");
                        literals.Add("Last Updated Date");
                        break;
                    case AccountsEntityNameLookup.MEMBERSHIP:
                        literals.Add("Membership");
                        literals.Add("Membership Details");
                        literals.Add("Membership ID");
                        literals.Add("Membership Name");
                        literals.Add("Description");
                        literals.Add("VIP");
                        literals.Add("Base Membership");
                        literals.Add("Membership Rule");
                        literals.Add("Redemption Discount");
                        literals.Add("Price List");
                        literals.Add("Membership Rewards");
                        literals.Add("Rewards Id");
                        literals.Add("Reward Name");
                        literals.Add("Description");
                        literals.Add("Reward Product");
                        literals.Add("Reward Attribute");
                        literals.Add("Reward Attribute Percent");
                        literals.Add("Reward Function");
                        literals.Add("Reward Function Period");
                        literals.Add("Unit Of Reward Function Period");
                        literals.Add("Reward Frequency");
                        literals.Add("Unit Of Reward Frequency");
                        literals.Add("Expire With Membership");
                        literals.Add("Exclusion Rules");
                        break;
                    case AccountsEntityNameLookup.MEMBERSHIPRULES:
                        literals.Add("Membership Rules");
                        literals.Add("Membership Rules Details");
                        literals.Add("Rule ID");
                        literals.Add("Rule Name");
                        literals.Add("Description");
                        literals.Add("Qualifying Points");
                        literals.Add("Qualification Window");
                        literals.Add("Unit Of Qualification Window");
                        literals.Add("Retention Points");
                        literals.Add("Retention Window");
                        literals.Add("Unit Of Retention Window");
                        break;
                    case AccountsEntityNameLookup.MEMBERSHIPEXCLUSIONRULES:
                        literals.Add("Membership Exclusion Rules");
                        literals.Add("Membership Exclusion Rules Details");
                        literals.Add("Product");
                        literals.Add("Game");
                        literals.Add("Game Profile");
                        literals.Add("Disallowed");
                        literals.Add("Qualification Window");
                        literals.Add("Last Updated By");
                        literals.Add("Last Updated Date");
                        break;
                    case AccountsEntityNameLookup.CARDSMANAGEMENT:
                        literals.Add("Cards Management");
                        literals.Add("Cards Management Details");
                        literals.Add("Add Cards");
                        literals.Add("User");
                        literals.Add("Received Date");
                        literals.Add("From Ser #");
                        literals.Add("To Ser #");
                        literals.Add("Number of Cards");
                        literals.Add("Card Inventory Till Date");
                        literals.Add("Cards Issued Till Date");
                        literals.Add("Card Balance In Stock");
                        literals.Add("Delete Cards");
                        literals.Add("Date");
                        literals.Add("View Inventory");
                        literals.Add("Till Date");
                        literals.Add("For Period");
                        literals.Add("From");
                        literals.Add("To");
                        literals.Add("Generate");
                        literals.Add("Save to Disk");
                        literals.Add("From Serial Number");
                        literals.Add("To Serial Number");
                        literals.Add("Action");
                        literals.Add("Action Date");
                        literals.Add("Action By");
                        literals.Add("Token/Card Inventory");
                        literals.Add("Location");
                        literals.Add("Week Ending");
                        literals.Add("Technician Name");
                        literals.Add("Token Collected from Kiosk");
                        literals.Add("Token Collected from Pos");
                        literals.Add("Remaining on Hand Token");
                        literals.Add("Transferred Tokens");
                        literals.Add("Total Cards on Hand");
                        literals.Add("Cards Purchased");
                        literals.Add("Cancel");
                        break;
                    case AccountsEntityNameLookup.BULKUPLOADCARDS:
                        literals.Add("Bulk Upload Cards");
                        literals.Add("Bulk Upload Cards Details");
                        literals.Add("Upload File");
                        literals.Add("Choose File");
                        literals.Add("Progress");
                        literals.Add("Serial Number");
                        literals.Add("Card Number");
                        literals.Add("Status");
                        literals.Add("Message");
                        literals.Add("Clear");
                        literals.Add("Upload");
                        literals.Add("File Format");
                        break;
                    case AccountsEntityNameLookup.CARDACTIVITY:
                        literals.Add("Product");
                        literals.Add("GameplayId");
                        literals.Add("MachineId");
                        literals.Add("CardId");
                        literals.Add("CardNumber");
                        literals.Add("Credits");
                        literals.Add("Courtesy");
                        literals.Add("Bonus");
                        literals.Add("PlayDate");
                        literals.Add("Notes");
                        literals.Add("TicketCount");
                        literals.Add("TicketMode");
                        literals.Add("Guid");
                        literals.Add("SiteId");
                        literals.Add("SynchStatus");
                        literals.Add("CardGame");
                        literals.Add("CPCardBalance");
                        literals.Add("CPCredits");
                        literals.Add("CPBonus");
                        literals.Add("CardGameId");
                        literals.Add("PayoutCost");
                        literals.Add("MasterEntityId");
                        literals.Add("Game");
                        literals.Add("ETickets");
                        literals.Add("ManualTickets");
                        literals.Add("TicketEaterTickets");
                        literals.Add("Mode");
                        literals.Add("Site");
                        literals.Add("TaskId");
                        literals.Add("IsChanged");
                        literals.Add("Card Activity for Card Number");
                        literals.Add("Purchases and Tasks");
                        literals.Add("Show Extended");
                        literals.Add("Consolidated View");
                        literals.Add("Export To Excel");
                        literals.Add("AccountId");
                        literals.Add("Date");
                        literals.Add("Amount");
                        literals.Add("Credits");
                        literals.Add("Courtesy");
                        literals.Add("Bonus");
                        literals.Add("Time");
                        literals.Add("Tokens");
                        literals.Add("Tickets");
                        literals.Add("LoyaltyPoints");
                        literals.Add("Site");
                        literals.Add("POS");
                        literals.Add("UserName");
                        literals.Add("Quantity");
                        literals.Add("Price");
                        literals.Add("RefId");
                        literals.Add("ActivityType");
                        literals.Add("Game Plays");
                        literals.Add("Hide Extended");
                        break;
                    case AccountsEntityNameLookup.AUDITTRAIL:
                        literals.Add("Audit Trail for Card");
                        literals.Add("Card Audit Id");
                        literals.Add("Card Id");
                        literals.Add("Card Number");
                        literals.Add("Customer");
                        literals.Add("Issue Date");
                        literals.Add("Deposit");
                        literals.Add("Time");
                        literals.Add("Loyalty Points");
                        literals.Add("Ticket Count");
                        literals.Add("Credits Played");
                        literals.Add("Real Ticket Mode");
                        literals.Add("VIP Customer");
                        literals.Add("Ticket Allowed");
                        literals.Add("Tech Card?");
                        literals.Add("Timer Reset Card");
                        literals.Add("Tech Games");
                        literals.Add("Valid Flag");
                        literals.Add("Refund Flag");
                        literals.Add("Refund Amount");
                        literals.Add("Refund Date");
                        literals.Add("Expiry Date");
                        literals.Add("Start Time");
                        literals.Add("Last Played Time");
                        literals.Add("Notes");
                        literals.Add("Last Update Time");
                        literals.Add("Last Updated By");
                        literals.Add("Primary Card");
                        literals.Add("Card Identifier");
                        literals.Add("Customer Id");
                        literals.Add("Upload Site Id");
                        literals.Add("Upload Time");
                        literals.Add("Download Batch Id");
                        literals.Add("Refresh From HQ Time");
                        literals.Add("Site Id");
                        break;
                    case AccountsEntityNameLookup.CUSTOMERDETAILS:
                        literals.Add("Customer Details");
                        literals.Add("Name");
                        literals.Add("Title");
                        literals.Add("Membership");
                        literals.Add("Tax Code");
                        literals.Add("Ext. Sys. Ref");
                        literals.Add("Address");
                        literals.Add("Address Type");
                        literals.Add("Verified");
                        literals.Add("Team User");
                        literals.Add("Contact");
                        literals.Add("Contact Type");
                        literals.Add("Attribute1");
                        literals.Add("Attribute2");
                        literals.Add("Lookup");
                        literals.Add("OK");
                        literals.Add("Id");
                        literals.Add("First Name");
                        literals.Add("Middle Name");
                        literals.Add("Username");
                        break;
                    case AccountsEntityNameLookup.CUSTOMERLOOKUP:
                        literals.Add("Customer Lookup");
                        literals.Add("Customer Lookup Details");
                        literals.Add("Customer Name");
                        literals.Add("Unique Identifier");
                        literals.Add("Membership");
                        literals.Add("Phone");
                        literals.Add("E-Mail");
                        literals.Add("Site");
                        literals.Add("Page 1 of 1");
                        literals.Add("Select");
                        literals.Add("Customer Id");
                        literals.Add("Title");
                        literals.Add("First Name");
                        literals.Add("Middle Name");
                        literals.Add("Card Number");
                        literals.Add("Tax Code");
                        literals.Add("Team User");
                        literals.Add("Verified");
                        literals.Add("External System Reference");
                        literals.Add("Channel");
                        literals.Add("Custom");
                        literals.Add("Last Update Date");
                        literals.Add("Last Updated User");
                        literals.Add("Addresses");
                        literals.Add("Address Type");
                        literals.Add("Contacts");
                        literals.Add("Contact Type");
                        literals.Add("Attribute1");
                        literals.Add("Attribute2");
                        break;
                    case AccountsEntityNameLookup.ADDTOCARD:
                        literals.Add("Add To Card");
                        literals.Add("Game Time");
                        literals.Add("Tech Games #");
                        literals.Add("Ticket Count");
                        literals.Add("Card Games");
                        literals.Add("Game Profile");
                        literals.Add("Frequency");
                        literals.Add("Entt. Type");
                        literals.Add("Entt. Value");
                        literals.Add("Optional Attribute");
                        literals.Add("X");
                        literals.Add("Repeat Count");
                        literals.Add("Add");
                        literals.Add("Cancel");
                        literals.Add("Games");
                        break;
                    case AccountsEntityNameLookup.IDPROOF:
                        literals.Add("Id Proof");
                        literals.Add("Browse");
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