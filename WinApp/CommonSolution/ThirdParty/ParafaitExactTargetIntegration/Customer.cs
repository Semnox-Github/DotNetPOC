// Class developed to handle the integration with ExactTarger DB
// The integration is primarily focused on the customer registration data. 
// CEC's customers will register at the registration kiosks and this information will need to flow to ExactTarger (SFDC) 
// to manage the marketing activities. 
// Primarily the integration involves two scenarios
// 1. New customer registration - Whenever a new customer registers and does not opt out of the marketing emails, 
//    would be transmitted to ExactTarget
// 2. Updates to the customer data - The customer could update their record. Based on the type of the update, 
//    the appropriate action needs to be initiated on the  ExactTarget side. 
//
//  Date          Modification                                          Done by         Version
//  31-July-2015  Created                                               Kiran Karanki   0.00
//  03-Aug-2015   Changed the logic of the key from original email id   Kiran Karanki   0.10
//                to subscriber id
//  18-Aug-2015   Added the "Opt out" logic. In case the customer has   Kiran Karanki   0.20
//                opted out, then the subscriber has to be added to a 
//                different list
//  26-Aug-2015   Reverted the changes. Customer will not be            Kiran Karanki   0.30
//                transmitted to ExactTarget in case they don't 
//                subscribe to the email communication
//                In this process, considering that there could multiple
//                such back and forth by customer, have made the 
//                functions more generic. 
//  14-Nov-2015   If email already exists for customer, throw           Kiran Karanki   0.40
//                duplicate customer exception. 
//  24-May-2016   Adding the Store_ID field                             Kiran Karanki   1.01
//  01-Aug-2017   Adding new field - YouthOrg, New method to update     Mathew Ninan    1.02
//                Data Extension (SetUpDataExtensioninExactTarget)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Parafait.logging;
using FuelSDK;

namespace Semnox.Parafait.ThirdParty
{
    public class Customer
    {
        private const int SEARCH_USING_ID = 1;
        private const int SEARCH_USING_KEY = 2;

        public const string CUSTOMERID = "Subscriber Id";
        public const string CUSTOMER_KEY = "Subscriber Key";
        public const string FIRST_NAME = "First Name";
        public const string LAST_NAME = "Last Name";
        public const string ZIP_CODE = "Zip";
        public const string PHONE_NUMBER = "Phone Number";
        //     public const string BIRTHDAY = "Subscriber Birthday";
        public const string BIRTHDAY = "Birthday";
        public const string EMAIL_ID = "Email Id";
        public const string EMAIL = "EmailAddress";
        //      public const string STORE_ID = "Store_ID";
        public const string STORE_ID = "Store";
        public const string YOUTH_ORG = "YouthOrg";
        //      private List<string> customerAttributeNames = new List<string> { CUSTOMERID, CUSTOMER_KEY, FIRST_NAME, LAST_NAME, 
        //          ZIP_CODE, PHONE_NUMBER, BIRTHDAY, EMAIL_ID, STORE_ID };
        private List<string> customerAttributeNames = new List<string> { CUSTOMERID, CUSTOMER_KEY, FIRST_NAME, LAST_NAME,
            ZIP_CODE, PHONE_NUMBER, BIRTHDAY, EMAIL_ID, STORE_ID, EMAIL, YOUTH_ORG };

        static bool debugFlag = false;

        Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ET_Client etIntegrationHandler;
        private SubscriberStatus customerStatus;

        private Dictionary<string, string> customerAttributes;
        private Dictionary<string, int> optInListNames;
        private Dictionary<string, int> optOutListNames;

        string optInListName;
        string optOutListName;
        string dataExtensionName;

        //LogWriter writer = LogWriter.Instance;

        public Customer()
        {
            try
            {
                log.Debug("Start base constructor");
                etIntegrationHandler = new ET_Client();
                customerAttributes = new Dictionary<string, string>();
                optInListNames = new Dictionary<string, int>();
                optOutListNames = new Dictionary<string, int>();
                log.Debug("End base constructor");
            }
            catch (Exception ex)
            {
                log.Log("Exact Target client constructor: ", ex);
                throw new Exception("Exact target client could not be instantiated. " + ex.Message);
            }
        }

        [Obsolete("Customer constructor passing the parameters is deprecated, please use constructor wherein the lists are passed instead.")]
        public Customer(string firstName, string lastName, string salutation, string zipCode,
                        string emailAddress, string phoneNumber, string birthDate, bool unSubscribe,
                        string subscriberId, string optInListName, string optOutListName, string storeId,
                        string youthOrg)
            : this()
        {
            try
            {
                log.Debug("Start constructor with params " + "Name: " + firstName + " Last Name: " + lastName +
                    "Sal: " + salutation + " ZipCode: " + zipCode + " Email: " + emailAddress + " Phone: " + phoneNumber +
                    "Opt out" + unSubscribe + " Sub Id: " + subscriberId + " Birthdate: " + birthDate + " YouthOrg: " + youthOrg);

                customerAttributes.Add(FIRST_NAME, firstName);
                customerAttributes.Add(LAST_NAME, lastName);
                customerAttributes.Add(ZIP_CODE, zipCode);
                customerAttributes.Add(EMAIL_ID, emailAddress);
                customerAttributes.Add(CUSTOMERID, subscriberId);
                customerAttributes.Add(PHONE_NUMBER, phoneNumber);
                customerAttributes.Add(BIRTHDAY, birthDate);
                customerAttributes.Add(CUSTOMER_KEY, emailAddress);
                customerAttributes.Add(STORE_ID, storeId);
                customerAttributes.Add(EMAIL, emailAddress);
                customerAttributes.Add(YOUTH_ORG, youthOrg);


                if (unSubscribe == true)
                    customerStatus = SubscriberStatus.Unsubscribed;
                else
                    customerStatus = SubscriberStatus.Active;

                this.optInListName = optInListName;
                this.optOutListName = optOutListName;

                log.Debug("End constructor with params");
            }
            catch (Exception ex)
            {
                log.Log("Constructor with params hits exception ", ex);
                throw ex;
            }
        }

        public Customer(List<KeyValuePair<string, string>> customerAttributes, List<string> optInListNames, List<string> optOutListNames, string inDataExtensionName)
            : this()
        {
            try
            {
                log.Debug("Start constructor with list params");
                foreach (string optInListName in optInListNames)
                {
                    if ((optInListName != null) && (optInListName.Length > 0))
                        this.optInListNames.Add(optInListName, -1);
                }
                foreach (string optOutListName in optOutListNames)
                {
                    if ((optOutListName != null) && (optOutListName.Length > 0))
                        this.optOutListNames.Add(optOutListName, -1);
                }
                foreach (KeyValuePair<string, string> customerAttribute in customerAttributes)
                {
                    var match = customerAttributeNames.FirstOrDefault(stringToCheck => stringToCheck.Contains(customerAttribute.Key));
                    if (match == null)
                        throw new Exception("Customer attribute " + customerAttribute.Key + " does not exist");
                }
                dataExtensionName = inDataExtensionName;
                customerStatus = SubscriberStatus.Active; //Added on 29-Oct-2015 to initialize customerStatus to Active
                this.customerAttributes = customerAttributes.ToDictionary(pair => pair.Key, pair => pair.Value);
                log.Debug("End constructor with list params");
            }
            catch (Exception ex)
            {
                log.Log("Constructor with list params hits exception ", ex);
                throw;
            }
        }

        public void MakeActive()
        {
            customerStatus = SubscriberStatus.Active;
            Publish(false);
        }

        public void MakeInactive()
        {
            customerStatus = SubscriberStatus.Unsubscribed;
            Publish(false);
        }

        public static void StartDebug()
        {
            debugFlag = true;
        }

        public static void EndDebug()
        {
            debugFlag = false;
        }

        public bool Exists()
        {
            try
            {
                log.Debug("Check customer exists function");
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                                    (se, cert, chain, sslerror) =>
                                    {
                                        return true;
                                    };
                if (!customerAttributes.ContainsKey(CUSTOMERID))
                    throw new CustomerNotFoundException();
                string subscriberId = customerAttributes[CUSTOMERID];
                if (subscriberId.Trim().Length == 0)
                    throw new CustomerNotFoundException();
                ET_Subscriber subscriber = CheckCustomerExistsWithSubscriberId(subscriberId);
            }
            catch (CustomerNotFoundException)
            {
                log.Debug("End customer exists function - Customer not found");
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            log.Debug("End customer exists function");
            return true;
        }

        [Obsolete("Publish method with listname as the parameter should no longer be used. Please use the Publish(bool isDuplicateEmailAllowed) method instead. The list names that the customer subscribes to should be made passed to the constructor")]
        public string Publish(string listName)
        {
            if ((customerAttributes[CUSTOMERID] != null) && (customerAttributes[CUSTOMERID].Length > 0))
            {
                if (listName.Trim().Length == 0)
                    customerStatus = SubscriberStatus.Unsubscribed;
            }
            else if ((listName == null) || (listName.Length == 0))
                return "";

            string listToRemove;
            if (listName.CompareTo(optInListName) == 0)
                listToRemove = optOutListName;
            else
                listToRemove = optInListName;
            if ((listName != null) && (listName.Length != 0))
                optInListNames.Add(listName, -1);
            if ((listToRemove != null) && (listToRemove.Length != 0))
                optOutListNames.Add(listToRemove, -1);
            return Publish(false);
        }

        public string Publish(bool isDuplicateEmailAllowed)
        {
            try
            {
                log.Debug("Start publish function");
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                                    (se, cert, chain, sslerror) =>
                                    {
                                        return true;
                                    };
                try
                {
                    if (!customerAttributes.ContainsKey(CUSTOMERID))
                        throw new CustomerNotFoundException();
                    string subscriberId = customerAttributes[CUSTOMERID];
                    if (subscriberId.Trim().Length == 0)
                        throw new CustomerNotFoundException("Subscriber id is null");
                    ET_Subscriber subscriber = CheckCustomerExists(subscriberId, SEARCH_USING_ID);
                    string originalEmailId = subscriber.EmailAddress;
                    string emailAddress = customerAttributes[EMAIL_ID];
                    if (originalEmailId != emailAddress)
                    {
                        // In this scenario, the subscriber id was existing and further we find the particular customer in 
                        // ExactTarget. But the email address has changed. So, instead of updating the email id, the prescribed 
                        // mechanism is that the old customer is un-subscribed and the new customer is subscribed. 
                        DeleteCustomerFromExactTarget(originalEmailId);
                        try
                        {
                            ET_Subscriber existingSubscriber = CheckCustomerExists(emailAddress, SEARCH_USING_KEY);
                            // In case the customer already exists with the particular email id, then there are three choices
                            // a. Don't do anything
                            // b. Create a new customer
                            // c. Update the existing customer
                            // Currently the logic has been coded for a and b. The "c" could occur. Needs more thought on how it 
                            // should be handled. But given that the subscriber id was not passed, we should not be going and 
                            // updating the existing record
                            if (isDuplicateEmailAllowed)
                                subscriberId = SetupCustomerInExactTarget();
                        }
                        catch
                        {
                            subscriberId = SetupCustomerInExactTarget();
                        }
                    }
                    else
                    {
                        //if (optOutOfMarketing == true)
                        {
                            //customerStatus = SubscriberStatus.Unsubscribed;
                        }
                        UpdateCustomerInExactTarget();
                    }
                }
                catch
                {
                    //if (optOutOfMarketing == false)
                    {
                        try
                        {
                            string emailAddress = customerAttributes[EMAIL_ID];
                            ET_Subscriber subscriber = CheckCustomerExists(emailAddress, SEARCH_USING_KEY);
                            // If we find that the email already exists, then we have three choices
                            // a. Create a new customer with the newly entered details
                            // b. Dont do anything
                            // c. Update the customer with the new details - But because the subscriber id was not present, it
                            // might not be correct to update the customer. 
                            if (isDuplicateEmailAllowed)
                                SetupCustomerInExactTarget();
                            else
                            {
                                throw new CustomerDuplicateException("Customer already exists. Subscriber id: " + subscriber.ID.ToString());
                            }
                        }
                        catch
                        {
                            SetupCustomerInExactTarget();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Log("End publish function ", ex);
                throw ex;
            }
            log.Debug("End publish function");
            return customerAttributes[CUSTOMERID];
        }

        private ET_Subscriber CheckCustomerExistsWithSubscriberId(string subscriberKey)
        {
            log.Debug("Exec CheckCustomerExistsWithSubscriberId function");
            return CheckCustomerExists(subscriberKey, SEARCH_USING_ID);
        }

        public ET_Subscriber CheckCustomerExistsWithEmailId(string subscriberKey)
        {
            log.Debug("Exec CheckCustomerExistsWithEmailId function");
            return CheckCustomerExists(subscriberKey, SEARCH_USING_KEY);
        }

        private ET_Subscriber CheckCustomerExists(string subscriberKey, int searchCriteria)
        {
            log.Debug("Start Check customer exists function");
            ET_Subscriber currSubscriber = new ET_Subscriber();
            currSubscriber.AuthStub = etIntegrationHandler;
            currSubscriber.Props = new string[] { "SubscriberKey", "EmailAddress", "Status", "ID" };
            if (searchCriteria == SEARCH_USING_ID)
                currSubscriber.SearchFilter = new SimpleFilterPart() { Property = "ID", SimpleOperator = SimpleOperators.equals, Value = new string[] { subscriberKey } };
            else if (searchCriteria == SEARCH_USING_KEY)
                currSubscriber.SearchFilter = new SimpleFilterPart() { Property = "SubscriberKey", SimpleOperator = SimpleOperators.equals, Value = new string[] { subscriberKey } };
            GetReturn getResponse = currSubscriber.Get();
            if (getResponse.Status == false)
            {
                log.Debug("End Check customer exists function with exception " + "Failure to make the get subscriber call. " + getResponse.Message.ToString() + " . Response code" + getResponse.Code.ToString());
                throw new Exception("Failure to make the get subscriber call. " + getResponse.Message.ToString() + " . Response code" + getResponse.Code.ToString());
            }
            if (getResponse.Results.Length <= 0)
            {
                log.Debug("End Check customer exists function with exception - Customer not found");
                throw new CustomerNotFoundException("Customer does not exist");
            }
            ET_Subscriber sub = (ET_Subscriber)getResponse.Results[0];
            log.Debug("End Check customer exists function");
            return sub;
        }

        private int GetListId(string listName)
        {
            ET_List etListHandler = null;
            etListHandler = new ET_List();
            etListHandler.AuthStub = etIntegrationHandler;
            etListHandler.Props = new string[] { "ID", "ListName", "Description", "Client.ID" };
            etListHandler.SearchFilter = new SimpleFilterPart() { Property = "ListName", SimpleOperator = SimpleOperators.equals, Value = new String[] { listName } };
            GetReturn getList = etListHandler.Get();
            if ((getList.Status == false) || (getList.Results.Length == 0))
            {
                log.Debug("End Setup customer in exact target function with exception" + "Failure to get the list in ExactTarget. " + getList.Message.ToString() + " . Response code: " + getList.Code.ToString());
                throw new Exception("Failure to get the list in ExactTarget. " + getList.Message.ToString() + " . Response code: " + getList.Code.ToString());
            }
            return getList.Results[0].ID;
        }

        private string SetupCustomerInExactTarget()
        {
            log.Debug("Start Setup customer in exact target function");

            //ET_SubscriberList[] currSubscribersList = new ET_SubscriberList[optInListNames.Count];
            //int i = 0;
            //foreach (string currentListName in optInListNames.Keys.ToList())
            //{
            //    optInListNames[currentListName] = GetListId(currentListName);
            //    currSubscribersList[i] = new ET_SubscriberList() { ID = optInListNames[currentListName] };
            //    i++;
            //}
            //foreach (string currentListName in optOutListNames.Keys.ToList())
            //{
            //    optOutListNames[currentListName] = GetListId(currentListName);
            //}

            ET_Subscriber currSubscriber = new ET_Subscriber();
            currSubscriber.AuthStub = etIntegrationHandler;
            currSubscriber.EmailAddress = customerAttributes[EMAIL_ID];
            currSubscriber.SubscriberKey = customerAttributes[CUSTOMER_KEY];
            currSubscriber.Status = customerStatus;
            //if (currSubscribersList.Length > 0)
            //    currSubscriber.Lists = currSubscribersList;
            //List<FuelSDK.ET_ProfileAttribute> currCustomerAttributesList = new List<FuelSDK.ET_ProfileAttribute>();
            //foreach (KeyValuePair<string, string> currentAttribute in customerAttributes)
            //{
            //    if (!(currentAttribute.Key.Contains(EMAIL_ID) || currentAttribute.Key.Contains(CUSTOMER_KEY) || currentAttribute.Key.Contains(CUSTOMERID)))
            //        currCustomerAttributesList.Add(new ET_ProfileAttribute{Name = currentAttribute.Key, Value = currentAttribute.Value});
            //}
            //currSubscriber.Attributes = currCustomerAttributesList.ToArray();
            PostReturn postResponse = currSubscriber.Post();
            if (postResponse.Status == false)
            {
                string detailedMessage = "None";
                if (postResponse.Results.Length > 0)
                    detailedMessage = postResponse.Results[0].StatusMessage;
                log.Debug("End Setup customer in exact target function with exception " + "Failure to setup the customer in ExactTarget. " + postResponse.Message.ToString() + " . Response code: " + postResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                throw new Exception("Failure to setup the customer in ExactTarget. " + postResponse.Message.ToString() + " . Response code: " + postResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
            }
            if (postResponse.Results.Length <= 0)
            {
                string detailedMessage = "None";
                log.Debug("End Setup customer in exact target function with exception " + "Failure to setup the customer in ExactTarget. " + postResponse.Message.ToString() + " . Response code: " + postResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                throw new Exception("Failure to setup the customer in ExactTarget. " + postResponse.Message.ToString() + " . Response code: " + postResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
            }
            SetupDataExtensionInExactTarget(false);
            log.Debug("End Setup customer in exact target function with exception");
            customerAttributes[CUSTOMERID] = postResponse.Results[0].NewID.ToString();
            return customerAttributes[CUSTOMERID];
        }

        private void DeleteCustomerFromExactTarget(string subscriberKey)
        {
            log.Debug("Start delete customer in exact target function with exception");
            ET_Subscriber currSubscriber = new ET_Subscriber();
            currSubscriber.AuthStub = etIntegrationHandler;
            currSubscriber.EmailAddress = subscriberKey;
            currSubscriber.SubscriberKey = subscriberKey;
            currSubscriber.Status = SubscriberStatus.Unsubscribed;
            PatchReturn patchResponse = currSubscriber.Patch();
            if (patchResponse.Status == false)
            {
                string detailedMessage = "None";
                if (patchResponse.Results.Length > 0)
                    detailedMessage = patchResponse.Results[0].StatusMessage;
                log.Debug("End delete customer in exact target function with exception" + "Failure to make the delete the customer in ExactTarget. " + patchResponse.Message.ToString() + " . Response code: " + patchResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                throw new Exception("Failure to make the delete the customer in ExactTarget. " + patchResponse.Message.ToString() + " . Response code: " + patchResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
            }
            log.Debug("End delete customer in exact target function");

        }

        private void UpdateCustomerInExactTarget()
        {
            log.Debug("Start update customer in exact target function.");

            //ET_SubscriberList[] currSubscribersList = new ET_SubscriberList[optInListNames.Count + optOutListNames.Count];
            //int i = 0;
            //foreach (string currentListName in optInListNames.Keys.ToList())
            //{
            //    optInListNames[currentListName] = GetListId(currentListName);
            //    currSubscribersList[i] = new ET_SubscriberList() { ID = optInListNames[currentListName], Status = SubscriberStatus.Active };
            //    i++;
            //}
            //foreach (string currentListName in optOutListNames.Keys.ToList())
            //{
            //    optOutListNames[currentListName] = GetListId(currentListName);
            //    currSubscribersList[i] = new ET_SubscriberList() { ID = optOutListNames[currentListName], Status = SubscriberStatus.Unsubscribed };
            //    i++;
            //}

            ET_Subscriber currSubscriber = new ET_Subscriber();
            currSubscriber.AuthStub = etIntegrationHandler;
            currSubscriber.EmailAddress = customerAttributes[EMAIL_ID];
            currSubscriber.SubscriberKey = customerAttributes[CUSTOMER_KEY];
            currSubscriber.Status = customerStatus;
            //if (currSubscribersList.Length > 0)
            //    currSubscriber.Lists = currSubscribersList;
            //List<FuelSDK.ET_ProfileAttribute> currCustomerAttributesList = new List<FuelSDK.ET_ProfileAttribute>();
            //foreach (KeyValuePair<string, string> currentAttribute in customerAttributes)
            //{
            //    if (!(currentAttribute.Key.Contains(EMAIL_ID) || currentAttribute.Key.Contains(CUSTOMER_KEY) || currentAttribute.Key.Contains(CUSTOMERID)))
            //        currCustomerAttributesList.Add(new ET_ProfileAttribute { Name = currentAttribute.Key, Value = currentAttribute.Value });
            //}
            //currSubscriber.Attributes = currCustomerAttributesList.ToArray();
            PatchReturn patchResponse = currSubscriber.Patch();
            if (patchResponse.Status == false)
            {
                string detailedMessage = "None";
                if (patchResponse.Results.Length > 0)
                    detailedMessage = patchResponse.Results[0].StatusMessage;
                log.Debug("End update customer in exact target function with exception " + "Failure to make the update the customer in ExactTarget. " + patchResponse.Message.ToString() + " . Response code: " + patchResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                throw new Exception("Failure to make the update the customer in ExactTarget. " + patchResponse.Message.ToString() + " . Response code: " + patchResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
            }
            if (patchResponse.Results.Length <= 0)
            {
                string detailedMessage = "None";
                log.Debug("End update customer in exact target function with exception " + "Failure to make the update the customer in ExactTarget. " + patchResponse.Message.ToString() + " . Response code: " + patchResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                throw new Exception("Failure to update the customer in ExactTarget. " + patchResponse.Message.ToString() + " . Response code: " + patchResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
            }
            //SetupDataExtensionInExactTarget(true);
            ET_Subscriber sub = (ET_Subscriber)patchResponse.Results[0].Object;
            string subscriberEmail = sub.EmailAddress;
            log.Debug("End update customer in exact target function");

        }

        private void SetupDataExtensionInExactTarget(bool patch = false)
        {
            log.Debug("Start Setup data extension in exact target function using Data Extensions");
            ET_DataExtensionRow currSubscriberDataExt = new ET_DataExtensionRow();
            currSubscriberDataExt.AuthStub = etIntegrationHandler;
            foreach (KeyValuePair<string, string> currentAttribute in customerAttributes)
            {
                if (!(currentAttribute.Key.Contains(EMAIL_ID) || currentAttribute.Key.Contains(CUSTOMER_KEY) || currentAttribute.Key.Contains(CUSTOMERID)
                      || currentAttribute.Key.Contains(PHONE_NUMBER) || currentAttribute.Key.Contains(ZIP_CODE)))
                {
                    currSubscriberDataExt.ColumnValues.Add(currentAttribute.Key, currentAttribute.Value);
                }
            }
            if (patch)
            {
                currSubscriberDataExt.DataExtensionCustomerKey = dataExtensionName;
                PatchReturn subscriberDataExtPatchResp = currSubscriberDataExt.Patch();
                if (subscriberDataExtPatchResp.Status == false)
                {
                    string detailedMessage = "None";
                    if (subscriberDataExtPatchResp.Results.Length > 0)
                        detailedMessage = subscriberDataExtPatchResp.Results[0].StatusMessage;
                    log.Debug("End update data extension in exact target function with exception " + "Failure to make the update the customer in ExactTarget. " + subscriberDataExtPatchResp.Message.ToString() + " . Response code: " + subscriberDataExtPatchResp.Code.ToString() + " Detailed Message: " + detailedMessage);
                    throw new Exception("Failure to make the update the data extension in ExactTarget. " + subscriberDataExtPatchResp.Message.ToString() + " . Response code: " + subscriberDataExtPatchResp.Code.ToString() + " Detailed Message: " + detailedMessage);
                }
                if (subscriberDataExtPatchResp.Results.Length <= 0)
                {
                    string detailedMessage = "None";
                    log.Debug("End update customer in exact target function with exception " + "Failure to make the update the customer in ExactTarget. " + subscriberDataExtPatchResp.Message.ToString() + " . Response code: " + subscriberDataExtPatchResp.Code.ToString() + " Detailed Message: " + detailedMessage);
                    throw new Exception("Failure to update the customer in ExactTarget. " + subscriberDataExtPatchResp.Message.ToString() + " . Response code: " + subscriberDataExtPatchResp.Code.ToString() + " Detailed Message: " + detailedMessage);
                }
            }
            else
            {
                currSubscriberDataExt.DataExtensionCustomerKey = dataExtensionName;
                PostReturn subscriberDataExtResponse = currSubscriberDataExt.Post();
                if (subscriberDataExtResponse.Status == false)
                {
                    string detailedMessage = "None";
                    if (subscriberDataExtResponse.Results.Length > 0)
                        detailedMessage = subscriberDataExtResponse.Results[0].StatusMessage;
                    log.Debug("End insert data extension in exact target function with exception " + "Failure to make the update the data extension in ExactTarget. " + subscriberDataExtResponse.Message.ToString() + " . Response code: " + subscriberDataExtResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                    throw new Exception("Failure to insert the data extension in ExactTarget. " + subscriberDataExtResponse.Message.ToString() + " . Response code: " + subscriberDataExtResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                }
                if (subscriberDataExtResponse.Results.Length <= 0)
                {
                    string detailedMessage = "None";
                    log.Debug("End insert data extension in exact target function with exception " + "Failure to make the update the data extension in ExactTarget. " + subscriberDataExtResponse.Message.ToString() + " . Response code: " + subscriberDataExtResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                    throw new Exception("Failure to insert the data extension in ExactTarget. " + subscriberDataExtResponse.Message.ToString() + " . Response code: " + subscriberDataExtResponse.Code.ToString() + " Detailed Message: " + detailedMessage);
                }
            }
        }
    }

    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException()
            : base()
        {
        }

        public CustomerNotFoundException(string message)
            : base(message)
        {
        }

    }

    public class CustomerDuplicateException : Exception
    {
        public CustomerDuplicateException()
            : base()
        {
        }

        public CustomerDuplicateException(string message)
            : base(message)
        {
        }
    }
}
