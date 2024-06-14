/********************************************************************************************
 * Project Name - Customer
 * Description  - BL for the CustomerRegistration Localization 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *0.00        16-oct-2018   Mushahid Faizan        Created 
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;


namespace Semnox.CommonAPI.Localization
{
    public class CustomerRegistrationLocalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        string entityName;
        private Dictionary<string, string> listHeadersList = new Dictionary<string, string>();

        /// <summary>
        ///   Default Constructor for Customer Registration Locallization
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="entityName"></param>
        public CustomerRegistrationLocalizationBL(ExecutionContext executionContext, string entityName)
        {
            log.LogMethodEntry(executionContext, entityName);
            this.executionContext = executionContext;
            this.entityName = entityName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Getting lable messageno and headers
        /// </summary>
        private void GetLiteralsAndMessages(string entity, List<string> literalsOrMessageList)
        {
            string localizedValue = string.Empty;
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
            //entities.Add("CommonLiterals");
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
                    messages.Add("1867"); // Select a record to see more actions
                    messages.Add("1868"); //Do you want to continue without saving?
                    messages.Add("566"); // Do you want to save the changes?  
                    messages.Add("1761"); // Accepts only Alphanumeric, hyphen(-) and colon(:)
                    messages.Add("1764"); // Record has been saved successfully
                    messages.Add("1770"); // Are you sure you want to reset?
                    messages.Add("1773"); // Please enter valid value for &1
                    messages.Add("1868"); // Do you want to continue without saving?
                    messages.Add("1751"); // Required
                    messages.Add("1448");   // Loading... Please wait...
                    messages.Add("1776"); // Query is not valid.Please check.
                    messages.Add("1748");  // Please enter valid details
                    messages.Add("1134");   // Please save the record.
                    messages.Add("646");   // Please enter a number
                    messages.Add("5");   // Invalid Username / Password. Please retry.
                    messages.Add("24");   // Invalid Username or Password
                    messages.Add("272");   // Current Password is incorrect
                    messages.Add("273");   // New Password cannot be Blank
                    messages.Add("274");   // New Password and Re-enter New Password are not matching
                    messages.Add("275");   // Password Changed
                    messages.Add("580");   // Enter new Password to Change
                    messages.Add("777");   // Password has been emailed to registered email id
                    messages.Add("788");   // Enter Password Here
                    messages.Add("790");   // Please Enter Password
                    messages.Add("794");   // Password is sent to registered Email Id
                    messages.Add("807");   // Invalid Login Id / Password. Please retry.
                    messages.Add("812");   // Password Change required
                    messages.Add("814");   // Password length should be at least &1
                    messages.Add("815");   // Password should have at least &1 alphabets
                    messages.Add("816");   // Password should have at least &1 numerals
                    messages.Add("817");   // Password should have at least &1 special characters
                    messages.Add("818");   
                    messages.Add("1752");   
                    messages.Add("1753");   
                    messages.Add("1756");   
                    messages.Add("1758");   
                    messages.Add("1759");   
                    messages.Add("1760");   
                    messages.Add("1762");   
                    messages.Add("1761");   
                    messages.Add("1757");   
                    messages.Add("1760");   
                    messages.Add("1763");   
                    messages.Add("1760");   
                    messages.Add("1963");   
                    messages.Add("1964");   
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
                    literals.Add("Clear");
                    literals.Add("Select All");
                    literals.Add("Save");
                    literals.Add("Refresh");
                    literals.Add("Delete");
                    literals.Add("Close");
                    literals.Add("Yes"); 
                    literals.Add("No"); 
                    literals.Add("Cancel"); 
                    literals.Add("Last Page");
                    literals.Add("Next Page");
                    literals.Add("Previous Page");
                    literals.Add("Back");
                    literals.Add("Next");
                    literals.Add("Edit");
                    literals.Add("Reset");
                    literals.Add("Items per page");
                    literals.Add("Alphanumeric Only");
                    literals.Add("Submit");
                    literals.Add("Sign In?");
                    literals.Add("Sign In");
                    literals.Add("Loading");
                    literals.Add("A verification email has been sent to your registered email Id $ Click the link in the verification email and verify your account");
                    literals.Add("Already have an Account? Sign In");
                    literals.Add("New Password");
                    literals.Add("Confirm Password");
                    literals.Add("Sign Up?");
                    literals.Add("Click here to View");
                    literals.Add("Click here to Download");
                    literals.Add("Click here to Preview");
                    literals.Add("No Records Found!");
                    literals.Add("Ok");
                    literals.Add("Accept");
                    literals.Add("Search");
                    literals.Add("Complete");
                    literals.Add("Password Reset Email Link Has Been Sent To Your Email!");
                    literals.Add("Password Has Been Changed Successfully!");
                    literals.Add("Passwords should match");
                    literals.Add("Saved Successfully");
                    literals.Add("Invalid Date of birth");
                    literals.Add("Please Enter Valid Phone Number");
                    literals.Add("Please Enter Valid Email Address");
                    literals.Add("Max length");
                    literals.Add("Max length &1 Characters");
                    literals.Add("Min length &1 Characters");
                    literals.Add("Select a language to view in different language");
                    literals.Add("Password should contain atleast one capital letter,number and special character");
                    literals.Add("Your account has been successfully activated.Please click on the below link to Sign-in");
                    literals.Add("Resend Activation Mail");
                    literals.Add("Account Activation");
                    literals.Add("Select a language");
                    literals.Add("Your link has expired.Please click on the below link to re-send an activation email link");
                    literals.Add("Please select a language to view the application in that language");
                    literals.Add("Please Wait");
                    literals.Add("From Date");
                    literals.Add("To Date");
                    literals.Add("Go");
                    literals.Add("Please enter required details.");
                    literals.Add("A verification email has been set to your registered email Id");
                    literals.Add("Click the link in the verification email and verify your account");
                    literals.Add("Resend verification link");
                    literals.Add("Verification link sent successfully");
                    literals.Add("Offered In");
                    literals.Add("Error Summary");
                    literals.Add("Discount");
                    literals.Add("Account Activation Link Has Been Sent To Your Registered Email");
                    literals.Add("Registration Successful");
                    literals.Add("Minimum &1 characters, at least &2 uppercase letter, &3 lowercase letter, &4 number and one special character");
                    literals.Add("Your account has been successfully activated.");
                    literals.Add("Please click on the below link to Sign-in.");
                    literals.Add("Something went wrong. Please try again.");
                    literals.Add("Account Verification");
                    literals.Add("Please Select From Date");
                    literals.Add("Please Select To Date");
                    literals.Add("Waiting For Confirmation");
                    literals.Add("Sunday");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("All Days");
                    break;

                case "LOGIN":
                    literals.Add("Sign In");
                    literals.Add("UserName");
                    literals.Add("Password");
                    literals.Add("Forgot Password?");
                    literals.Add("Please Enter Email Id");
                    literals.Add("Wrong password. Try again or click Forgot password to reset it.");
                    break;
                case "APPLICATION":
                    literals.Add("Sign In");
                    literals.Add("Select a language");
                    literals.Add("Select your preferred language");
                    break;
                case "PASSWORDRESET":
                    literals.Add("Password Reset");
                    literals.Add("Forgot Password");
                    literals.Add("Current Password");
                    literals.Add("Old Password");
                    literals.Add("Change Password");
                    literals.Add("Reset Password");
                    literals.Add("Cancel");
                    literals.Add("New Password");
                    literals.Add("Confirm Password");
                    literals.Add("Submit");
                    literals.Add("Reset");
                    literals.Add("Sign In");
                    break;

                case "FORGOTPASSWORD":
                    literals.Add("Forgot Password");
                    literals.Add("Email Id");
                    literals.Add("Current Password");
                    literals.Add("Please Enter Registered Email Id");
                    break;

                case "CHANGEPASSWORD":
                    literals.Add("Current Password");
                    literals.Add("New Password");
                    literals.Add("Confirm Password");
                    literals.Add("Change Password");
                    break;

                case "MYACCOUNT":
                    literals.Add("Customer Details");
                    literals.Add("Logout");
                    literals.Add("Personal Info");
                    literals.Add("My Cards");
                    literals.Add("My Orders");
                    literals.Add("My Loyalty Points");
                    literals.Add("Change Password");
                    literals.Add("My Account");
                    literals.Add("Current Password");
                    literals.Add("View Credit Plus Details");
                    literals.Add("Click Here To View Account Details");
             
                    //MY Cards
                    literals.Add("My Cards");
                    literals.Add("Search My Cards");
                    literals.Add("Card# / Issued On");
                    literals.Add("Name");
                    literals.Add("Credits");
                    literals.Add("Top Up");
                    literals.Add("Card Activity");
                    literals.Add("Balance Check");
                    literals.Add("Link");
                    literals.Add("Update");
                    literals.Add("Enter Card Holder Name");
                    literals.Add("Personal Info");
                    literals.Add("View Card Credit Plus Details");
                    literals.Add("Card Balance");
                    literals.Add("Issue Date");
                    literals.Add("Card Deposit");
                    literals.Add("Credits");
                    literals.Add("Courtesy");
                    literals.Add("Loyalty Points");
                    literals.Add("Games");
                    literals.Add("Balance Time");
                    literals.Add("My Loyalty Details");
                    literals.Add("Card Activity");
                    literals.Add("Card Activity For Card #:");
                    literals.Add("Search Purchase Task");
                    literals.Add("Product");
                    literals.Add("Amount");
                    literals.Add("Bonus");
                    literals.Add("Tickets");
                    literals.Add("Search Game Play");
                    literals.Add("Date ");
                    literals.Add("Game");
                    literals.Add("Time ");
                    literals.Add("Mode");
                    literals.Add("Card Number #:");
                    literals.Add("Game Plays");
                    literals.Add("Purchase & Tasks");
                    literals.Add("Click Here To View Credit Plus Details");
                    // My Orders
                    literals.Add("My Orders");
                    literals.Add("Search My Orders");
                    literals.Add("Order Placed");
                    literals.Add("Status");
                    literals.Add("Order Details");
                    literals.Add("Invoice");
                    literals.Add("Preview");
                    literals.Add("Link");
                    literals.Add("Order Number #");
                    literals.Add("Ordered On");
                    literals.Add("My Order Details");
                    literals.Add("Order Details");
                    literals.Add("Items(s)SubTotal");
                    literals.Add("Total");
                    literals.Add("Promotion Applied");
                    literals.Add("Grand Total");
                    literals.Add("Ordered Number");
                    literals.Add("Date");
                    literals.Add("Receipt");
                    literals.Add("Ticket");
                    literals.Add("Tax Total");
                    literals.Add("Transaction Id #");
                    // My Loyalty Rewards
                    literals.Add("My Loyalty Rewards - Card #:");
                    literals.Add("Loyalty Points Balance");
                    literals.Add("Membership Type");
                    literals.Add("Search My Loyalty Rewards");
                    literals.Add("Loyalty Details (Click on the row to check Redemption/Consumption)");
                    literals.Add("Card Credit Plus Id");
                    literals.Add("Name");
                    literals.Add("Type");
                    literals.Add("Amount");
                    literals.Add("Balance");
                    literals.Add("Period From");
                    literals.Add("Period To");
                    literals.Add("Category");
                    literals.Add("Product Name");
                    literals.Add("Applicable On");
                    literals.Add("Description");
                    literals.Add("Daily Limit");
                    literals.Add("Disc Percent");
                    literals.Add("Disc Amount");
                    literals.Add("Disc Price");
                    literals.Add("Discount Amount");
                    literals.Add("Discount Percentage");
                    literals.Add("You do not have a membership");
                    literals.Add("Displays list of registered cards");
                    literals.Add("Displays list of transactions");
                    literals.Add("Redemption / Consumption Details");
                    literals.Add("Membership Details");
                    literals.Add("Membership");
                    literals.Add("You need another &1 points to become &2 member");
                    literals.Add("Earn another &1 points by &2 to retain &3 membership");
                    literals.Add("&1 points will expire by &2");
                    literals.Add("Total Points");
                    literals.Add("Membership Card");
                    literals.Add("Valid Till");
                    literals.Add("Loyalty Details");
                    literals.Add("Redemption/Consumption Details");
                    literals.Add("For Reference, Refer screens below");
                    // Personal Info
                    literals.Add("Account Details");
                    literals.Add("Personal Info");
                    literals.Add("Invalid Card Number");
                    literals.Add("Fill Out Your Personal Details");
                    literals.Add("Fill Out Your Address Details");
                    literals.Add("View Card Credit Plus Details");
                    literals.Add("Fill Out Your Contact Details");
                    literals.Add("Terms and Conditions");
                    literals.Add("Already Have An Account? Sign In");
                    literals.Add("Sign Up");
                    // Link Card
                    literals.Add("Link Card");
                    literals.Add("Link Your Card");
                    literals.Add("Enter Card #");
                    literals.Add("Click here to register another card");
                    break;
                case "ALL":
                    literals.Add("Clear");
                    literals.Add("Select All");
                    literals.Add("Save");
                    literals.Add("Refresh");
                    literals.Add("Delete");
                    literals.Add("Close");
                    literals.Add("Yes");
                    literals.Add("No");
                    literals.Add("Cancel");
                    literals.Add("Last Page");
                    literals.Add("Next Page");
                    literals.Add("Previous Page");
                    literals.Add("Back");
                    literals.Add("Next");
                    literals.Add("Edit");
                    literals.Add("Reset");
                    literals.Add("Items per page");
                    literals.Add("Alphanumeric Only");
                    literals.Add("Submit");
                    literals.Add("Sign In?");
                    literals.Add("Sign In");
                    literals.Add("Loading");
                    literals.Add("A verification email has been sent to your registered email Id $ Click the link in the verification email and verify your account");
                    literals.Add("Already have an Account? Sign In");
                    literals.Add("New Password");
                    literals.Add("Confirm Password");
                    literals.Add("Sign Up?");
                    literals.Add("Click here to View");
                    literals.Add("Click here to Download");
                    literals.Add("Click here to Preview");
                    literals.Add("No Records Found!");
                    literals.Add("Ok");
                    literals.Add("Accept");
                    literals.Add("Search");
                    literals.Add("Complete");
                    literals.Add("Password Reset Email Link Has Been Sent To Your Email!");
                    literals.Add("Password Has Been Changed Successfully!");
                    literals.Add("Passwords should match");
                    literals.Add("Saved Successfully");
                    literals.Add("Invalid Date of birth");
                    literals.Add("Please Enter Valid Phone Number");
                    literals.Add("Please Enter Valid Email Address");
                    literals.Add("Max length");
                    literals.Add("Max length &1 Characters");
                    literals.Add("Min length &1 Characters");
                    literals.Add("Select a language to view in different language");
                    literals.Add("Password should contain atleast one capital letter,number and special character");
                    literals.Add("Your account has been successfully activated.Please click on the below link to Sign-in");
                    literals.Add("Resend Activation Mail");
                    literals.Add("Account Activation");
                    literals.Add("Select a language");
                    literals.Add("Your link has expired.Please click on the below link to re-send an activation email link");
                    literals.Add("Please select a language to view the application in that language");
                    literals.Add("Please Wait");
                    literals.Add("From Date");
                    literals.Add("To Date");
                    literals.Add("Go");
                    literals.Add("Please enter required details.");
                    literals.Add("A verification email has been set to your registered email Id");
                    literals.Add("Click the link in the verification email and verify your account");
                    literals.Add("Resend verification link");
                    literals.Add("Verification link sent successfully");
                    literals.Add("Offered In");
                    literals.Add("Error Summary");
                    literals.Add("Discount");
                    literals.Add("Account Activation Link Has Been Sent To Your Registered Email");
                    literals.Add("Registration Successful");
                    literals.Add("Minimum &1 characters, at least &2 uppercase letter, &3 lowercase letter, &4 number and one special character");
                    literals.Add("Your account has been successfully activated.");
                    literals.Add("Please click on the below link to Sign-in.");
                    literals.Add("Something went wrong. Please try again.");
                    literals.Add("Account Verification");
                    literals.Add("Please Select From Date");
                    literals.Add("Please Select To Date");
                    literals.Add("Waiting For Confirmation");
                    literals.Add("Sunday");
                    literals.Add("Monday");
                    literals.Add("Tuesday");
                    literals.Add("Wednesday");
                    literals.Add("Thursday");
                    literals.Add("Friday");
                    literals.Add("Saturday");
                    literals.Add("All Days");
                    literals.Add("Sign In");
                    literals.Add("UserName");
                    literals.Add("Password");
                    literals.Add("Forgot Password?");
                    literals.Add("Please Enter Email Id");
                    literals.Add("Wrong password. Try again or click Forgot password to reset it.");
                    literals.Add("Sign In");
                    literals.Add("Select a language");
                    literals.Add("Select your preferred language");
                    literals.Add("Password Reset");
                    literals.Add("Forgot Password");
                    literals.Add("Current Password");
                    literals.Add("Old Password");
                    literals.Add("Change Password");
                    literals.Add("Reset Password");
                    literals.Add("Cancel");
                    literals.Add("New Password");
                    literals.Add("Confirm Password");
                    literals.Add("Submit");
                    literals.Add("Reset");
                    literals.Add("Sign In");
                    literals.Add("Forgot Password");
                    literals.Add("Email Id");
                    literals.Add("Current Password");
                    literals.Add("Please Enter Registered Email Id");
                    literals.Add("Current Password");
                    literals.Add("New Password");
                    literals.Add("Confirm Password");
                    literals.Add("Change Password");
                    literals.Add("Customer Details");
                    literals.Add("Logout");
                    literals.Add("Personal Info");
                    literals.Add("My Cards");
                    literals.Add("My Orders");
                    literals.Add("My Loyalty Points");
                    literals.Add("Change Password");
                    literals.Add("My Account");
                    literals.Add("Current Password");
                    literals.Add("View Credit Plus Details");
                    literals.Add("Click Here To View Account Details");
                    literals.Add("My Cards");
                    literals.Add("Search My Cards");
                    literals.Add("Card# / Issued On");
                    literals.Add("Name");
                    literals.Add("Credits");
                    literals.Add("Top Up");
                    literals.Add("Card Activity");
                    literals.Add("Balance Check");
                    literals.Add("Link");
                    literals.Add("Update");
                    literals.Add("Enter Card Holder Name");
                    literals.Add("Personal Info");
                    literals.Add("View Card Credit Plus Details");
                    literals.Add("Card Balance");
                    literals.Add("Issue Date");
                    literals.Add("Card Deposit");
                    literals.Add("Credits");
                    literals.Add("Courtesy");
                    literals.Add("Loyalty Points");
                    literals.Add("Games");
                    literals.Add("Balance Time");
                    literals.Add("My Loyalty Details");
                    literals.Add("Card Activity");
                    literals.Add("Card Activity For Card #:");
                    literals.Add("Search Purchase Task");
                    literals.Add("Product");
                    literals.Add("Amount");
                    literals.Add("Bonus");
                    literals.Add("Tickets");
                    literals.Add("Search Game Play");
                    literals.Add("Date ");
                    literals.Add("Game");
                    literals.Add("Time ");
                    literals.Add("Mode");
                    literals.Add("Card Number #:");
                    literals.Add("Game Plays");
                    literals.Add("Purchase & Tasks");
                    literals.Add("Click Here To View Credit Plus Details");
                    // My Orders
                    literals.Add("My Orders");
                    literals.Add("Search My Orders");
                    literals.Add("Order Placed");
                    literals.Add("Status");
                    literals.Add("Order Details");
                    literals.Add("Invoice");
                    literals.Add("Preview");
                    literals.Add("Link");
                    literals.Add("Order Number #");
                    literals.Add("Ordered On");
                    literals.Add("My Order Details");
                    literals.Add("Order Details");
                    literals.Add("Items(s)SubTotal");
                    literals.Add("Total");
                    literals.Add("Promotion Applied");
                    literals.Add("Grand Total");
                    literals.Add("Ordered Number");
                    literals.Add("Date");
                    literals.Add("Receipt");
                    literals.Add("Ticket");
                    literals.Add("Tax Total");
                    literals.Add("Transaction Id #");
                    // My Loyalty Rewards
                    literals.Add("My Loyalty Rewards - Card #:");
                    literals.Add("Loyalty Points Balance");
                    literals.Add("Membership Type");
                    literals.Add("Search My Loyalty Rewards");
                    literals.Add("Loyalty Details (Click on the row to check Redemption/Consumption)");
                    literals.Add("Card Credit Plus Id");
                    literals.Add("Name");
                    literals.Add("Type");
                    literals.Add("Amount");
                    literals.Add("Balance");
                    literals.Add("Period From");
                    literals.Add("Period To");
                    literals.Add("Category");
                    literals.Add("Product Name");
                    literals.Add("Applicable On");
                    literals.Add("Description");
                    literals.Add("Daily Limit");
                    literals.Add("Disc Percent");
                    literals.Add("Disc Amount");
                    literals.Add("Disc Price");
                    literals.Add("Discount Amount");
                    literals.Add("Discount Percentage");
                    literals.Add("You do not have a membership");
                    literals.Add("Displays list of registered cards");
                    literals.Add("Displays list of transactions");
                    literals.Add("Redemption / Consumption Details");
                    literals.Add("Membership Details");
                    literals.Add("Membership");
                    literals.Add("You need another &1 points to become &2 member");
                    literals.Add("Earn another &1 points by &2 to retain &3 membership");
                    literals.Add("&1 points will expire by &2");
                    literals.Add("Total Points");
                    literals.Add("Membership Card");
                    literals.Add("Valid Till");
                    literals.Add("Loyalty Details");
                    literals.Add("Redemption/Consumption Details");
                    literals.Add("For Reference, Refer screens below");
                    // Personal Info
                    literals.Add("Account Details");
                    literals.Add("Personal Info");
                    literals.Add("Invalid Card Number");
                    literals.Add("Fill Out Your Personal Details");
                    literals.Add("Fill Out Your Address Details");
                    literals.Add("View Card Credit Plus Details");
                    literals.Add("Fill Out Your Contact Details");
                    literals.Add("Terms and Conditions");
                    literals.Add("Already Have An Account? Sign In");
                    literals.Add("Sign Up");
                    // Link Card
                    literals.Add("Link Card");
                    literals.Add("Link Your Card");
                    literals.Add("Enter Card #");
                    literals.Add("Click here to register another card");
                    break;
            }
            log.LogMethodExit(literals);
            return literals;
        }
    }
}