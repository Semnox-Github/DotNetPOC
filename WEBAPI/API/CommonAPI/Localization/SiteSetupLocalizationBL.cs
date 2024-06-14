/********************************************************************************************
 * Project Name - Site Setup
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019   Mushahid Faizan          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Localization
{
    public class SiteSetupLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();
        /// <summary>
        ///   Default Constructor for Site Setup Localization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public SiteSetupLocalizationBL(ExecutionContext executionContext, string entityName)
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

        public enum SiteSetupEntityName
        {
            PARAFAITCONFIGURATION,
            RECEIPTTEMPLATE,
            LOOKUPS,
            SECURITYPOLICY,
            SITE,
            USERS,
            PARAFAITOPTIONS,
            USERROLES,
            DATAACCESSRULE,
            TASKTYPES,
            POSMANAGEMENT,
            PERIPHERALS,
            ATTENDANCEROLES,
            TRANSACTIONPROFILES,
            MESSAGES,
            KIOSKSETUP,
            CUSTOMATTRIBUTES,
            TABLELAYOUT,
            EMAILTEMPLATE,
            PRODUCTKEY,
            MANUALPURGE,
            DBSYNCH,
            CURRENCY,
            ORDERTYPEGROUP,
            ORDERTYPE,
            INVOICESEQUENCESETUP,
            TICKETTEMPLATE,
            ULCKEYS,
            TICKETSTATION,
            MESSAGINGCLIENT,
            PAYCONFIGURATIONMAP,
            CASHDRAWERS,
            DELIVERYCHANNELS,
            DELIVERYINTEGRATION,
            TABLEATTRIBUTESETUP,
            ATTRIBUTEENABLEDTABLE,
            ATTRIBUTEVALIDATION
        }
        public List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);
            try
            {
                List<string> literals = new List<string>();

                SiteSetupEntityName siteSetupEntityName = (SiteSetupEntityName)Enum.Parse(typeof(SiteSetupEntityName), entityName.ToUpper());
                switch (siteSetupEntityName)
                {
                    case SiteSetupEntityName.PARAFAITCONFIGURATION:
                        literals.Add("Values");
                        literals.Add("Parafait Configuration");
                        literals.Add("Settings");
                        literals.Add("Datatypes");
                        literals.Add("Default Value Name");
                        literals.Add("Description");
                        literals.Add("Default Value");
                        literals.Add("Datatype");
                        literals.Add("Screen Group");
                        literals.Add("User Level");
                        literals.Add("POS Level");
                        literals.Add("Protected?");
                        literals.Add("Datatype Id");
                        literals.Add("DataValues");
                        break;
                    case SiteSetupEntityName.RECEIPTTEMPLATE:
                        literals.Add("Receipt Template");
                        literals.Add("Printers");
                        literals.Add("Print Templates");
                        literals.Add("Help");
                        literals.Add("Printer Id");
                        literals.Add("Printer Name");
                        literals.Add("Printer Location");
                        literals.Add("IP Address");
                        literals.Add("Printer Type");
                        literals.Add("Remarks");
                        literals.Add("Display Groups");
                        literals.Add("Available Display Groups");
                        literals.Add("Print These Display Groups");
                        literals.Add("Select Products To Print");
                        literals.Add("Display Group :");
                        literals.Add("Available Products");
                        literals.Add("Print Only These Product");
                        literals.Add(">>");
                        literals.Add("<<");
                        literals.Add("Receipt And KOT Templates");
                        literals.Add("Template Id");
                        literals.Add("Template Name");
                        literals.Add("Font Name");
                        literals.Add("Font Size");
                        literals.Add("Preview");
                        literals.Add("Section");
                        literals.Add("Sequence");
                        literals.Add("Col1 Data");
                        literals.Add("Col1 Alignment");
                        literals.Add("Col2 Data");
                        literals.Add("Col2 Alignment");
                        literals.Add("Col3 Data");
                        literals.Add("Col3 Alignment");
                        literals.Add("Col4 Data");
                        literals.Add("Col4 Alignment");
                        literals.Add("Col5 Data");
                        literals.Add("Col5 Alignment");
                        literals.Add("Font Name");
                        literals.Add("Font Size");
                        literals.Add("MetaData");
                        literals.Add("Copy");
                        literals.Add("Import");
                        literals.Add("Ticket Template");
                        literals.Add("Save to File / Copy");
                        literals.Add("Select Yes to export this template to file. Select No to create a copy.");
                        literals.Add("Print Preview");
                        literals.Add("Print");
                        break;
                    case SiteSetupEntityName.TICKETTEMPLATE:
                        literals.Add("Template Keywords");
                        literals.Add("Paper Size (Inch)");
                        literals.Add("Width");
                        literals.Add("Height");
                        literals.Add("Show Scroll Bar");
                        literals.Add("Border Width");
                        literals.Add("Left");
                        literals.Add("Right");
                        literals.Add("Top");
                        literals.Add("Bottom");
                        literals.Add("Margins (1/100 Inch)");
                        literals.Add("Background Image");
                        literals.Add("Upload");
                        literals.Add("Clear");
                        literals.Add("Backside Template");
                        literals.Add("Ticket Templates");
                        literals.Add("Copy");
                        literals.Add("MAIN");
                        literals.Add("PRODUCT");
                        literals.Add("TOTALS");
                        literals.Add("CARDINFO");
                        literals.Add("DISCOUNT");
                        literals.Add("Receipt Template");
                        literals.Add("Printers");
                        literals.Add("Print Templates");
                        literals.Add("Add");
                        literals.Add("Template Name");
                        literals.Add("Add Label");
                        literals.Add("Font");
                        literals.Add("Alignment");
                        literals.Add("Rotate");
                        literals.Add("Format");
                        literals.Add("Date");
                        literals.Add("Number");
                        literals.Add("Encode Option");
                        literals.Add("Color");
                        literals.Add("Remove");
                        literals.Add("Save to File / Copy");
                        literals.Add("Select Yes to export this template to file. Select No to create a copy.");
                        literals.Add("Print Preview");
                        literals.Add("Print");
                        break;
                    case SiteSetupEntityName.LOOKUPS:
                        literals.Add("Lookups");
                        literals.Add("Payment Modes");
                        literals.Add("Sequences");
                        literals.Add("Lookup Id");
                        literals.Add("Lookup Name");
                        literals.Add("Protected?");
                        literals.Add("Lookup Values");
                        literals.Add("Lookup Value");
                        literals.Add("Description");
                        literals.Add("Payment Mode Id");
                        literals.Add("Payment Mode");
                        literals.Add("Is Cash?");
                        literals.Add("Is Debit Card?");
                        literals.Add("Is Credit Card?");
                        literals.Add("Credit Card Surcharge%");
                        literals.Add("Manager Approval?");
                        literals.Add("POS Available?");
                        literals.Add("Display Order");
                        literals.Add("Gateway");
                        literals.Add("Channel");
                        literals.Add("Coupons");
                        literals.Add("Image File");
                        literals.Add("Payment Reference Mandatory");
                        literals.Add("Payment Mode Channel");
                        literals.Add("Select Payment Channel");
                        literals.Add("Enable");
                        literals.Add("Payment Coupons");
                        literals.Add("Enter Coupon Details");
                        literals.Add("Coupon Set Id");
                        literals.Add("From Number");
                        literals.Add("To Number");
                        literals.Add("Coupon Count");
                        literals.Add("Coupon Value");
                        literals.Add("Effective Date");
                        literals.Add("Coupon Expiry Date");
                        literals.Add("Used Coupons");
                        literals.Add("Coupon Number");
                        literals.Add("Transaction Id");
                        literals.Add("Line Id");
                        literals.Add("Sequence Name");
                        literals.Add("Seed");
                        literals.Add("Increment By");
                        literals.Add("Current Value");
                        literals.Add("Maximum Value");
                        literals.Add("Prefix");
                        literals.Add("Suffix");
                        literals.Add("Width");
                        literals.Add("User Column Heading");
                        literals.Add("POS");
                        literals.Add("Order Type Group");
                        literals.Add("Invoice Sequence Mapping");
                        literals.Add("Show Active Entries");
                        literals.Add("ID");
                        literals.Add("Sequence Id");
                        literals.Add("Invoice Sequence");
                        literals.Add("Effective Date");
                        literals.Add("Invoice Type");
                        literals.Add("Series Start Number");
                        literals.Add("Series End Number");
                        literals.Add("Expiry Date");
                        literals.Add("Import");
                        break;

                    case SiteSetupEntityName.SECURITYPOLICY:
                        literals.Add("Security Policy");
                        literals.Add("Policy Name");
                        literals.Add("Policy Details");
                        literals.Add("Password Change Frequency");
                        literals.Add("Password Minimum Length");
                        literals.Add("Password Minimum Alphabets");
                        literals.Add("Password Minimum Numbers");
                        literals.Add("Password Minimum Special Characters");
                        literals.Add("Remember Passwords History");
                        literals.Add("Invalid Attempts Before Lock Out");
                        literals.Add("Lock Out Duration");
                        literals.Add("User Session Time Out");
                        literals.Add("Maximum User Inactivity Days");
                        literals.Add("Max Days To Login After User Creation");
                        break;
                    case SiteSetupEntityName.SITE:
                        literals.Add("Site");
                        literals.Add("Company");
                        literals.Add("Country / State");
                        literals.Add("Site Info");
                        literals.Add("Site Id");
                        literals.Add("Site Name");
                        literals.Add("Site Code");
                        literals.Add("Site Address");
                        literals.Add("Notes");
                        literals.Add("Site Guid");
                        literals.Add("Logo");
                        literals.Add("Initial Load Done");
                        literals.Add("Max Cards");
                        literals.Add("Version");
                        literals.Add("Customer Key");
                        literals.Add("Org Id");
                        literals.Add("Upload Logo");
                        literals.Add("Clear Logo");
                        literals.Add("Edit Site");
                        literals.Add("Upload URL");
                        literals.Add("Company Key");
                        literals.Add("Company Id");
                        literals.Add("Company Name");
                        literals.Add("Internet Login Key");
                        literals.Add("Master Site Id");
                        literals.Add("Country Id");
                        literals.Add("Country Name");
                        literals.Add("States");
                        literals.Add("State Id");
                        literals.Add("State");
                        literals.Add("Description");
                        literals.Add("Org Structure");
                        literals.Add("Organization");
                        literals.Add("Id");
                        literals.Add("Structure Name");
                        literals.Add("Parent Structure");
                        literals.Add("Auto Roam");
                        literals.Add("Org Id");
                        literals.Add("Org Name");
                        literals.Add("Structure");
                        literals.Add("Parent Org");
                        literals.Add("Tree View");
                        literals.Add("HQ Publish");
                        literals.Add("HQ Site Management");
                        literals.Add("HQ Options");
                        literals.Add("Last Upload Time");
                        literals.Add("Last Upload Message");
                        literals.Add("Last Download Time");
                        literals.Add("Product Key");
                        literals.Add("Create/Update Master Data from Master Site");
                        literals.Add("Download Data from HQ Site after Last Upload Time");
                        break;
                    case SiteSetupEntityName.USERS:
                        literals.Add("POS User Setup");
                        literals.Add("User Details");
                        literals.Add("Users");
                        literals.Add("User Id");
                        literals.Add("User Name");
                        literals.Add("Login Id");
                        literals.Add("Role");
                        literals.Add("User Status");
                        literals.Add("POS Counter");
                        literals.Add("Options");
                        literals.Add("Pwd Change Date");
                        literals.Add("Pwd Change On Next Login");
                        literals.Add("Invalid Access Attempts");
                        literals.Add("Locked Out Time");
                        literals.Add("Override Fingerprint");
                        literals.Add("Last Name");
                        literals.Add("Emp. No");
                        literals.Add("Email");
                        literals.Add("Company Admin");
                        literals.Add("Department");
                        literals.Add("Manager");
                        literals.Add("Emp. Start Date");
                        literals.Add("Emp. End Date");
                        literals.Add("Emp. End Reason");
                        literals.Add("Last Login Time");
                        literals.Add("Last Logout Time");
                        literals.Add("Creation Date");
                        literals.Add("Created By");
                        literals.Add("Created User");
                        literals.Add("Password");
                        literals.Add("Reset Password");
                        literals.Add("Re-Enter Password");
                        literals.Add("Options");
                        literals.Add("User Identification Tags");
                        literals.Add("ID Tags");
                        literals.Add("Change Password On Next Login");
                        literals.Add("Employee Details");
                        literals.Add("Status");
                        literals.Add("Override FP");
                        literals.Add("Company Admin");
                        literals.Add("Invalid Access Attempts");
                        literals.Add("Employee Details");
                        literals.Add("Status");
                        literals.Add("Play Pass#");
                        literals.Add("Attendance Reader Tag");
                        literals.Add("Finger Print");
                        literals.Add("Finger Number");
                        literals.Add("Parafait Options");
                        literals.Add("Options");
                        literals.Add("Values");
                        literals.Add("Option Name");
                        literals.Add("Option Value");
                        literals.Add("Card Number");
                        literals.Add("Date Of Birth");
                        literals.Add("Shift Configurations");
                        break;

                    case SiteSetupEntityName.USERROLES:
                        literals.Add("User Roles");
                        literals.Add("POS User Setup");
                        literals.Add("Role Id");
                        literals.Add("Role");
                        literals.Add("Role Description");
                        literals.Add("Manager Flag");
                        literals.Add("Allow Pos Access");
                        literals.Add("POS Clock In-Out");
                        literals.Add("Assigned Manager Role");
                        literals.Add("Allow Shift Open-Close");
                        literals.Add("Security Policy");
                        literals.Add("Data Access Level");
                        literals.Add("Data Access Rule");
                        literals.Add("Product Group Inclusion / Exclusions");
                        literals.Add("Price List");
                        literals.Add("Include");
                        literals.Add("Exclude");
                        literals.Add(">>");
                        literals.Add("<<");
                        literals.Add("Delete Role");
                        literals.Add("Attendance Roles");
                        literals.Add("Attendance Role");
                        literals.Add("Refresh Functions");
                        literals.Add("Data Access Management");
                        literals.Add("Access Management");
                        literals.Add("Shift Configurations");
                        break;
                    case SiteSetupEntityName.TRANSACTIONPROFILES:
                        literals.Add("Transaction Profiles");
                        literals.Add("ID");
                        literals.Add("Profile Name");
                        literals.Add("Verify?");
                        literals.Add("Tax Rule");
                        literals.Add("Tax");
                        literals.Add("Tax Structure");
                        literals.Add("Exempt?");
                        break;
                    case SiteSetupEntityName.ULCKEYS:
                        literals.Add("ULC Keys");
                        literals.Add("Manage Ulc Authentication Keys");
                        literals.Add("Key");
                        literals.Add("Current Key");
                        literals.Add("View");
                        literals.Add("Add");
                        break;
                    case SiteSetupEntityName.TASKTYPES:
                        literals.Add("Task Types");
                        literals.Add("Task Type Id");
                        literals.Add("Task Type");
                        literals.Add("Task Type Name");
                        literals.Add("Requires Manager Approval");
                        literals.Add("Display In Pos");
                        break;
                    case SiteSetupEntityName.POSMANAGEMENT:
                        literals.Add("POS Management");
                        literals.Add("POS Counters");
                        literals.Add("POS Machines");
                        literals.Add("Counter Id");
                        literals.Add("Counter Name");
                        literals.Add("Description");
                        literals.Add("Id");
                        literals.Add("Counter");
                        literals.Add("POS Name");
                        literals.Add("Computer Name");
                        literals.Add("IP Address");
                        literals.Add("Friendly Name");
                        literals.Add("Legal Entity");
                        literals.Add("Inventory Location");
                        literals.Add("Remarks");
                        literals.Add("Options");
                        literals.Add("Peripherals");
                        literals.Add("Product Group Inclusion / Exclusions");
                        literals.Add("POS Printers");
                        literals.Add("Include");
                        literals.Add("Exclude");
                        literals.Add(">>");
                        literals.Add("<<");
                        literals.Add("Printer");
                        literals.Add("Order Type Group");
                        literals.Add("Print Template");
                        literals.Add("Secondary Printer");
                        literals.Add("X");
                        break;
                    case SiteSetupEntityName.PERIPHERALS:
                        literals.Add("POS Peripherals");
                        literals.Add("Device Id");
                        literals.Add("Device Name");
                        literals.Add("Device Type");
                        literals.Add("Device SubType");
                        literals.Add("VID");
                        literals.Add("PID");
                        literals.Add("Optional String / Serial Number");
                        break;
                    case SiteSetupEntityName.PARAFAITOPTIONS:
                        literals.Add("Parafait Options");
                        literals.Add("Options");
                        literals.Add("Values");
                        literals.Add("Option Name");
                        literals.Add("Option Value");
                        literals.Add("Limits");
                        literals.Add("Card");
                        literals.Add("Card Credit Gateways");
                        literals.Add("Payment Gateway");
                        literals.Add("Server");
                        break;
                    case SiteSetupEntityName.ATTENDANCEROLES:
                        literals.Add("Attendance Roles For");
                        literals.Add("Attendance Role");
                        literals.Add("Id");
                        literals.Add("Approval Required");
                        literals.Add("AtendanceRoles MapId");
                        break;
                    case SiteSetupEntityName.DATAACCESSRULE:
                        literals.Add("Data Access Rule");
                        literals.Add("Data Access Management");
                        literals.Add("Data Access Rule Id");
                        literals.Add("First Name");
                        literals.Add("Access Rule Details");
                        literals.Add("Rule Detail Id");
                        literals.Add("Entity");
                        literals.Add("Access Level");
                        literals.Add("Access Limit");
                        literals.Add("Exclusion Detail");
                        literals.Add("Exclusion Id");
                        literals.Add("Attribute");
                        break;
                    case SiteSetupEntityName.KIOSKSETUP:
                        literals.Add("Kiosk Setup");
                        literals.Add("ID");
                        literals.Add("Note Coin Flag");
                        literals.Add("Denomination Id");
                        literals.Add("First Name");
                        literals.Add("Image");
                        literals.Add("Value");
                        literals.Add("Acceptor Hex Code");
                        literals.Add("Model Code");
                        literals.Add("Upload Image");
                        literals.Add("Clear Image");
                        break;
                    case SiteSetupEntityName.MESSAGES:
                        literals.Add("Messages");
                        literals.Add("Languages");
                        literals.Add("Message No.");
                        literals.Add("Message");
                        literals.Add("Translated Message");
                        literals.Add("Language Filter");
                        literals.Add("All Literals");
                        literals.Add("Search Messages");
                        literals.Add("Language");
                        literals.Add("Language Id");
                        literals.Add("Language Name");
                        literals.Add("Language Code");
                        literals.Add("Culture Code");
                        literals.Add("Font Name");
                        literals.Add("Font Size");
                        literals.Add("Reader Language No");
                        literals.Add("Remarks");
                        break;
                    case SiteSetupEntityName.CUSTOMATTRIBUTES:
                        literals.Add("Custom Attributes");
                        literals.Add("ID");
                        literals.Add("Attribute Name");
                        literals.Add("Sequence");
                        literals.Add("Type");
                        literals.Add("Applicability");
                        literals.Add("Access");
                        literals.Add("Attribute Values");
                        literals.Add("Value");
                        literals.Add("Default?");
                        break;
                    case SiteSetupEntityName.TABLELAYOUT:
                        literals.Add("Table Layout");
                        literals.Add("Select Facility");
                        literals.Add("Add Row");
                        literals.Add("Add Column");
                        literals.Add("Delete Last Row");
                        literals.Add("Delete Last Column");
                        literals.Add("Name");
                        literals.Add("Type");
                        literals.Add("Int Info");
                        literals.Add("Remarks");
                        literals.Add("Max Check Ins");
                        break;
                    case SiteSetupEntityName.EMAILTEMPLATE:
                        literals.Add("Email Template");
                        literals.Add("Help");
                        literals.Add("Email Template Id");
                        literals.Add("First Name");
                        literals.Add("Name");
                        literals.Add("Description");
                        literals.Add("Start Date");
                        literals.Add("End Date");
                        literals.Add("Template Keywords");
                        literals.Add("Email Message Template");
                        literals.Add("Replacement Keywords");
                        literals.Add("Displayed in the Help Tab");
                        literals.Add("[ Max 160 for SMS ]");
                        literals.Add("Email Template Preview");
                        break;
                    case SiteSetupEntityName.PRODUCTKEY:
                        literals.Add("Update Product Key");
                        literals.Add("Site Key");
                        literals.Add("License Key");
                        literals.Add("Licensed POS Key");
                        literals.Add("Features Key");
                        literals.Add("Licensed POS Machines");
                        literals.Add("Expires On");
                        literals.Add("Update");
                        literals.Add("Add-On Features");
                        literals.Add("Featured");
                        literals.Add("Installed?");
                        literals.Add("Add Cards");
                        literals.Add("Cards");
                        literals.Add("Apply");
                        literals.Add("Allow FF Till");
                        literals.Add("AuthKey");
                        literals.Add("AuthKey1");
                        literals.Add("AuthKey2");
                        literals.Add("AuthKey3");
                        break;
                    case SiteSetupEntityName.MANUALPURGE:
                        literals.Add("Manual Purge");
                        literals.Add("Purge Old Data");
                        literals.Add("Purge");
                        literals.Add("Purge Confirmation");
                        literals.Add("Purge Data Before");
                        literals.Add("Group");
                        literals.Add("Cards");
                        literals.Add("Include Active Cards With Zero Balance");
                        literals.Add("GamePlay");
                        literals.Add("Transactions");
                        literals.Add("Logs");
                        literals.Add("Warning: Purging delete all Transactions, Game Plays, refunded Cards, redemption details before the specified purge date from the local database");
                        literals.Add("Includes Cards Table");
                        literals.Add("Includes GamePlay and GamePlayInfo tables");
                        literals.Add("Includes trx_header, trx_lines, trx_TaxLines, trxDiscounts, TrxPayments, TipPayment, TrxSplitLines, TrxSplitPayments and TrxParentModifierDetails tables");
                        literals.Add("Includes EventLog and MonitorLog tables");
                        break;
                    case SiteSetupEntityName.DBSYNCH:
                        literals.Add("DBSynch");
                        literals.Add("ID");
                        literals.Add("Table Name");
                        literals.Add("Synchronize");
                        literals.Add("Insert Only");
                        literals.Add("Synch Deletes");
                        literals.Add("Ignore On Error");
                        literals.Add("Ignore Columns On Roaming");
                        literals.Add("Site Id");
                        break;
                    case SiteSetupEntityName.CURRENCY:
                        literals.Add("Currency");
                        literals.Add("Currency Details");
                        literals.Add("Currency Code");
                        literals.Add("Currency Symbol");
                        literals.Add("Description");
                        literals.Add("Buy Rate");
                        literals.Add("Sell Rate");
                        literals.Add("Effective Date");
                        literals.Add("Last Modified By");
                        break;
                    case SiteSetupEntityName.ORDERTYPEGROUP:
                        literals.Add("Order Type Group");
                        literals.Add("First Name:");
                        literals.Add("SI#");
                        literals.Add("First Name");
                        literals.Add("Description");
                        literals.Add("Precedence");
                        break;
                    case SiteSetupEntityName.ORDERTYPE:
                        literals.Add("Order Type");
                        literals.Add("SI#");
                        literals.Add("First Name");
                        literals.Add("First Name:");
                        literals.Add("Description");
                        literals.Add("Map Order Type");
                        literals.Add("Order Type Group Map:");
                        literals.Add("Order Type Group:");
                        literals.Add("Order Type Group");
                        break;
                    case SiteSetupEntityName.INVOICESEQUENCESETUP:
                        literals.Add("Invoice Sequence Setup");
                        literals.Add("Invoice Type:");
                        literals.Add("System Resolution Authorization No:");
                        literals.Add("SI#");
                        literals.Add("Invoice Type");
                        literals.Add("Prefix");
                        literals.Add("Current Value");
                        literals.Add("Series Start Number");
                        literals.Add("Series End Number");
                        literals.Add("Approved Date");
                        literals.Add("Expiry Date");
                        literals.Add("Resolution Number");
                        literals.Add("Resolution Date");
                        break;
                    case SiteSetupEntityName.TICKETSTATION:
                        literals.Add("Ticket Station Setup");
                        literals.Add("Ticket Station Id");
                        literals.Add("Ticket Station Type");
                        literals.Add("Voucher Length");
                        literals.Add("Check Digit");
                        literals.Add("Ticket Length");
                        literals.Add("Check Bit Algorithim");
                        break;
                    case SiteSetupEntityName.MESSAGINGCLIENT:
                        literals.Add("ClientName");
                        literals.Add("MessagingChannelCode");
                        literals.Add("Sender");
                        literals.Add("SmtpPort");
                        literals.Add("Host Url");
                        literals.Add("EnableSsl");
                        literals.Add(" Messaging Client");
                        literals.Add("Communication Client Setup");
                        literals.Add(" Parafait Function Mapping To Messaging Client");
                        literals.Add("Parafait Function");
                        literals.Add("Customer Function");
                        literals.Add("Message Type");
                        literals.Add("Client Name");
                        break;
                    case SiteSetupEntityName.PAYCONFIGURATIONMAP:
                        literals.Add("PayConfigurationMapId");
                        literals.Add("Pay Configurations");
                        literals.Add("Pay Configuration Map");
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