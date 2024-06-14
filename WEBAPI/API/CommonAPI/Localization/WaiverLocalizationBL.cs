/********************************************************************************************
 * Project Name - Cards Module
 * Description  - Localization for all Literals and messages 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.80        13-Apr-2019   Mushahid Faizan          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.Localization
{
    public class WaiverLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for Waivers Localization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public WaiverLocalizationBL(ExecutionContext executionContext, string entityName)
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
                    messages.Add("1748");  // Please enter valid details
                    messages.Add("1761"); // Accepts only Alphanumeric, hyphen(-) and colon(:)
                    messages.Add("1762");
                    messages.Add("1763");
                    messages.Add("1764"); // Record has been saved successfully
                    messages.Add("1765");
                    messages.Add("1134");   // Please save the record.
                    messages.Add("1867"); // Select a record to see more actions

                    messages.Add("328"); // No valid reservation found for the given Reservation Code: &1
                    messages.Add("684"); // Please wait...
                    messages.Add("2618"); //Transaction OTP is not valid!
                    messages.Add("2619"); //Please choose only one option to proceed with signing
                    messages.Add("2620"); //Select Waiver
                    messages.Add("2621"); //Enter either Reservation Code or proceed with waiver selection
                    messages.Add("2622"); //Enter either Transaction OTP or proceed with waiver selection
                    messages.Add("2623"); //Reservation Code cannot accommodate all selected customers. Do you want to proceed without Reservation Code ?
                    messages.Add("2624"); //Date of birth is not present. Do you want to update the Date of birth for the customer?
                    messages.Add("2625"); //Do you want to re-sign the waiver?
                    messages.Add("2626"); //Selected customer is an adult. You cannot sign for them
                    messages.Add("2627"); // You can not proceed with re-signing the waiver. Managers approval is required
                    messages.Add("2628"); // Selected customer is an adult. Please select only child records
                    messages.Add("2629"); //Please select Relationship Type for this customer
                    messages.Add("2630"); //Managers approval is required to re-sign the waiver
                    messages.Add("2631"); //Date Of Birth is not present for selected customer(s)?
                    messages.Add("2632"); //Only &1 customers can be added against the &2
                    messages.Add("2633"); //Do you want to proceed without saving customer details?
                    messages.Add("2634"); //Do you want to proceed without signing the waiver?
                    messages.Add("2635"); //You have signed waiver &1 out of &2. Waiver has been successfully saved
                    messages.Add("2636"); //You have signed waiver &1 out of &2
                    messages.Add("2637"); // Please select the terms and condition to sign the waiver
                    messages.Add("2638"); // Reservation Code is not valid!

                    break;
            }
            return messages;

        }

        public enum WaiversEntityName
        {
            COMMONLITERALS,
            WAIVERSELECTION,
            SIGNWAIVER,
            CUSTOMERDETAILS,
            CUSTOMERRELATIONSHIP,
            SIGNINGWAIVER,
            VIEWSIGNEDWAIVERS,
            THANKYOU
        }

        public List<string> GetLiterals(string entityName)
        {
            log.LogMethodEntry(entityName);

            try
            {
                log.LogMethodEntry();
                List<string> literals = new List<string>();
                WaiversEntityName WaiversEntityName = (WaiversEntityName)Enum.Parse(typeof(WaiversEntityName), entityName);
                switch (WaiversEntityName)
                {
                    case WaiversEntityName.COMMONLITERALS:
                        literals.Add("Filter");
                        literals.Add("First");
                        literals.Add("Previous");
                        literals.Add("Next");
                        literals.Add("Last");
                        literals.Add("Save");
                        literals.Add("Refresh");
                        literals.Add("Delete");
                        literals.Add("Close");
                        literals.Add("Search");
                        literals.Add("Cancel");
                        literals.Add("Ok");
                        literals.Add("No Records Found!");
                        literals.Add("Last Page");
                        literals.Add("Next Page");
                        literals.Add("Previous Page");
                        literals.Add("First Page");
                        literals.Add("Add New");
                        literals.Add("Edit");
                        literals.Add("Reset");
                        literals.Add("Items per page"); //Items per page
                        literals.Add("Search By");
                        literals.Add("Are you sure you want to delete?");
                        literals.Add("Are you sure you want to reset?");
                        literals.Add("Remove");
                        literals.Add("Last Updated By");
                        literals.Add("Last Updated Date");
                        literals.Add("Expiry Date");
                        break;
                    case WaiversEntityName.WAIVERSELECTION:
                        literals.Add("Sign Waivers");
                        literals.Add("Reservation Code");
                        literals.Add("Transaction OTP");
                        literals.Add("OR");
                        literals.Add("Pick the waiver options to proceed");
                        literals.Add("Waiver Set");
                        literals.Add("Choose");
                        literals.Add("Proceed");
                        literals.Add("Cancel");
                        literals.Add("SIGN IN/SIGN UP");
                        literals.Add("My Account");
                        literals.Add("View Signed Waivers");
                        literals.Add("Change Password");
                        literals.Add("Logout");

                        break;
                    case WaiversEntityName.SIGNWAIVER:
                        literals.Add("Sign up/Log in to proceed with signing waiver");
                        literals.Add("Sign Waiver");
                        literals.Add("Sign");
                        literals.Add("View Waiver");
                        literals.Add("SIGN IN/SIGN UP");
                        literals.Add("Availability is for &1 person(s)");
                        literals.Add("My Account");
                        literals.Add("View Signed Waivers");
                        literals.Add("Change Password");
                        literals.Add("Logout");
                        literals.Add("Validity &1 days");

                        break;
                    case WaiversEntityName.CUSTOMERDETAILS:
                        literals.Add("Please select for whom you want to sign waiver");
                        literals.Add("Waiver");
                        literals.Add("Signing Waiver: &1 of &2 for Waiver set - &3");
                        literals.Add("Name :");
                        literals.Add("Reservation Code :");
                        literals.Add("Transaction OTP :");
                        literals.Add("Relationship");
                        literals.Add("Signed Status");
                        literals.Add("Add Relation");
                        literals.Add("Proceed");
                        literals.Add("Customer Details");

                        break;
                    case WaiversEntityName.CUSTOMERRELATIONSHIP:
                        literals.Add("Customer Relationship");
                        literals.Add("Primary Customer");
                        literals.Add("Related Customer");
                        literals.Add("Relationship Type");

                        break;
                    case WaiversEntityName.SIGNINGWAIVER:
                        literals.Add("Signing Waiver");
                        literals.Add("Waiver");
                        literals.Add("Name");
                        literals.Add("Signing Waiver");
                        literals.Add("Selected customer for whom you want sign for");
                        literals.Add("Customer Name");
                        literals.Add("Email Id");
                        literals.Add("Child Name");
                        literals.Add("Child Age");
                        literals.Add("Read carefully before signing the waivers");
                        literals.Add("Sign here...");
                        literals.Add("Clear Sign");
                        literals.Add("i here by declare that i have read the terms and conditions carefully");

                        break;
                    case WaiversEntityName.VIEWSIGNEDWAIVERS:
                        literals.Add("View Signed Waivers");
                        literals.Add("Signed Waiver");
                        literals.Add("Waiver Set Name");
                        literals.Add("Waiver Name");
                        literals.Add("Signed By");
                        literals.Add("Signed Date");
                        literals.Add("Signed For");
                        literals.Add("Waiver Code");
                        literals.Add("View");

                        break;
                    case WaiversEntityName.THANKYOU:
                        literals.Add("Thank You");
                        literals.Add("Waiver has been saved successfully");
                        literals.Add("Customer Name");
                        literals.Add("Signed For");
                        literals.Add("Date");
                        literals.Add("Waiver Code is &1");
                        literals.Add("Page will be redirected to the main page automatically,if not please click on link");
                        literals.Add("home page");

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