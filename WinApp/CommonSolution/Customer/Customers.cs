/********************************************************************************************
 * Project Name - Customers Programs 
 * Description  - Data object of the Customers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        08-June-2016   Rakshith           Created 
 *2.70.2      23-Jul-2019    Girish Kundar      Modified : Save() method() returns the DTO .
 *                                                         Added LogMethidEntry() and LogMethodExit()
 *2.90        21-May-2020    Girish Kundar    Modified : Made default constructor as Private                                                        
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customers data object class. This acts as data holder for the Customers business object
    /// </summary>
    public class Customers
    {

        private CustomersDTO customersDTO;
        private bool enableRoaming = false;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private Customers(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomersDTO constructor
        /// </summary>
        public Customers(ExecutionContext executionContext, CustomersDTO customersDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customersDTO);
            this.customersDTO = customersDTO;
            log.LogMethodExit();
        }
        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'CustomersDTO"'  DTO  ====>  ""CustomersDTO" DataHandler
        public Customers(ExecutionContext executionContext, int customerId ,SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerId,sqlTransaction);
            CustomersDatahandler customersDatahandler = new CustomersDatahandler(sqlTransaction);
            if (string.IsNullOrEmpty(CustomersList.passPhrase))
            {
                GetCustomerPassPhrase();
            }
            customersDTO = customersDatahandler.GetCustomersDTO(customerId, CustomersList.passPhrase);
            if (customersDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "customers", customerId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(customersDTO);
        }


        /// <summary>
        /// Get/Set method of the EnableRoaming field
        /// </summary>
        public bool EnableRoaming { get { return enableRoaming; } set { enableRoaming = value; } }


        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        /// <returns>returns int status</returns>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomersDatahandler customersDatahandler = new CustomersDatahandler(sqlTransaction);
            try
            {
                if (customersDTO.CustomerId <= 0)
                {
                    GetCustomerPassPhrase();
                    customersDTO = customersDatahandler.InsertCustomer(customersDTO, executionContext.GetUserId(), executionContext.GetSiteId(),CustomersList.passPhrase);
                    customersDTO.AcceptChanges();
                    if (EnableRoaming)
                    {
                        customersDatahandler.EnableRoaming(customersDTO.CustomerId);
                    }
                    log.LogMethodExit(customersDTO.CustomerId);
                    return customersDTO.CustomerId;
                }
                else
                {
                    if (customersDTO.IsChanged)
                    {
                        GetCustomerPassPhrase();
                        customersDTO = customersDatahandler.UpdateCustomer(customersDTO, executionContext.GetUserId(), executionContext.GetSiteId(), CustomersList.passPhrase);
                        customersDTO.AcceptChanges();
                    }
                    log.LogMethodExit();
                    if(EnableRoaming)
                    {
                        customersDatahandler.EnableRoaming(customersDTO.CustomerId);
                    }
                    return 0;
                }

            }
            catch (Exception expn)
            {
                log.Error("Error occurred at Save() method", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }

        }

        /// <summary>
        /// gets the GetCustomersDTO
        /// </summary>
        public CustomersDTO GetCustomersDTO
        {

            get { return customersDTO; }
        }

        /// <summary>
        ///  GetCardNumber  method
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>returns string cardnumber</returns>
        public string GetCardNumber(int customerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerId , sqlTransaction);
            try
            {
                CustomersDatahandler customersDatahandler = new CustomersDatahandler(sqlTransaction);
                return customersDatahandler.GetCardNumber(customerId);
            }
            catch (Exception expn)
            {
                log.Error("Error occurred at GetCardNumber()", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }


        /// <summary>
        /// LoginCustomer(CustomerParams customerParams) method
        /// </summary>
        /// <param name="customerParams">customerParams</param>
        /// <returns>returns CustomersDTO object</returns>
        public CustomersDTO LoginCustomer(CustomerParams customerParams ,SqlTransaction sqlTransaction =null)
        {
            try
            {
                log.LogMethodEntry(customerParams);
                CustomersDatahandler customersDatahandler = new CustomersDatahandler(sqlTransaction);
                if (string.IsNullOrEmpty(CustomersList.passPhrase))
                {
                    GetCustomerPassPhrase();
                }
                CustomersDTO customersDTO = customersDatahandler.LoginCustomer(customerParams, CustomersList.passPhrase);
                log.LogMethodExit(customersDTO);
                return customersDTO;

            }
            catch(Exception expn)
            {
                log.Error("Error occurred at LoginCustomer()", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }


        /// <summary>
        /// ForgotPassword(CustomerParams customerParams) Method
        /// </summary>
        /// <param name="customerParams">CustomerParams customerParams</param>
        /// <returns>return bool</returns>
        public bool ForgotPassword(CustomerParams customerParams , SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(customerParams , sqlTransaction);
                CustomersDatahandler customersDatahandler = new CustomersDatahandler(sqlTransaction);
                if (string.IsNullOrEmpty(CustomersList.passPhrase))
                {
                    GetCustomerPassPhrase();
                }
                bool status = customersDatahandler.ForgotPassword(customerParams, CustomersList.passPhrase);
                log.LogMethodExit(status);
                return status;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the Customer Pass Phrase
        /// </summary>
        public void GetCustomerPassPhrase()
        {
            ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParam = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParafaitDefaultsParam.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "CUSTOMER_ENCRYPTION_PASS_PHRASE"));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParam);
            if (parafaitDefaultsDTOList != null)
            {
                string encryptedPassPhrase = parafaitDefaultsDTOList[0].DefaultValue;
                CustomersList.passPhrase = Encryption.Decrypt(encryptedPassPhrase);
            }
        }

    }

    public class CustomersList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private ExecutionContext executionContext;
        public static string passPhrase;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomersList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<CustomersDTO.SearchByParameters, string>> by converting CustomerParams</returns>
        public List<KeyValuePair<CustomersDTO.SearchByParameters, string>> BuildCustomerSearchParametersList(CustomerParams customerParams)
        {
            log.LogMethodEntry(customerParams);
            List<KeyValuePair<CustomersDTO.SearchByParameters, string>> customerSearchParams = new List<KeyValuePair<CustomersDTO.SearchByParameters, string>>();
            if (customerParams != null)
            {
                if (!string.IsNullOrEmpty(customerParams.FirstName))
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.FNAME, customerParams.FirstName));
                if (!string.IsNullOrEmpty(customerParams.MiddleName))
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.MNAME, customerParams.MiddleName));
                if (!string.IsNullOrEmpty(customerParams.LastName))
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.LNAME, customerParams.LastName));
                if (!string.IsNullOrEmpty(customerParams.Email))
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.EMAIL, customerParams.Email));
                if (!string.IsNullOrEmpty(customerParams.Phone))
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.PHONE, customerParams.Phone));
                if (!string.IsNullOrEmpty(customerParams.UserName))
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.USER_NAME, customerParams.UserName));
                if (customerParams.OrderByUsername)
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.ORDER_BY_USERNAME, ""));
                if (customerParams.OrderBylastUpdatedDate)
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.ORDER_BY_LAST_UPDATED_DATE, ""));
                if (customerParams.SiteId > 0)
                    customerSearchParams.Add(new KeyValuePair<CustomersDTO.SearchByParameters, string>(CustomersDTO.SearchByParameters.SITE_ID, customerParams.SiteId.ToString()));

            }
            log.LogMethodExit(customerSearchParams);
            return customerSearchParams;
        }

        /// <summary>
        /// Returns Search Request And returns List Of CustomersDTO Class  
        /// </summary>
        public List<CustomersDTO> GetAllCustomersList(List<KeyValuePair<CustomersDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                CustomersDatahandler customersDatahandler = new CustomersDatahandler();
                if (string.IsNullOrEmpty(passPhrase))
                {
                    GetCustomerPassPhrase();
                }
                List<CustomersDTO> customersDTOList =  customersDatahandler.GetAllCustomersList(searchParameters, passPhrase);
                log.LogMethodExit(customersDTOList);
                return customersDTOList;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred at  GetAllCustomersList(SearchParameters)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }


        /// <summary>
        /// Returns Search Request And returns List Of CustomersDTO Class 
        /// <param name="customerParams"></param>
        /// <param name="passPhrase">Customer data Encryption PassPhrase</param>
        /// </summary>
        public List<CustomersDTO> GetAllCustomersList(CustomerParams customerParams)
        {
            log.LogMethodEntry(customerParams);
            try
            {
                List<KeyValuePair<CustomersDTO.SearchByParameters, string>> searchParameters = BuildCustomerSearchParametersList(customerParams);
                CustomersDatahandler customersDatahandler = new CustomersDatahandler();
                if (string.IsNullOrEmpty(passPhrase))
                {
                    GetCustomerPassPhrase();
                }
                List<CustomersDTO> customersDTOList = customersDatahandler.GetAllCustomersList(searchParameters, passPhrase);
                log.LogMethodExit(customersDTOList);
                return customersDTOList;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred at GetAllCustomersList(customerParams)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        ///  GetTopCustomer(string number, string email) 
        /// </summary>
        /// <param name="number">string number</param>
        /// <param name="email">string email</param>
        /// <returns>returns CustomersDTO object</returns>
        public CustomersDTO GetTopCustomer(string number, string email)
        {
            log.LogMethodEntry(number, email);
            try
            {
                CustomersDatahandler customersDatahandler = new CustomersDatahandler();
                if (string.IsNullOrEmpty(passPhrase))
                {
                    GetCustomerPassPhrase();
                }
                CustomersDTO customersDTO = customersDatahandler.GetTopCustomer(number, email,passPhrase);
                log.LogMethodExit(customersDTO);
                return customersDTO;

            }
            catch (Exception expn)
            {
                log.Error("Error occurred at GetTopCustomer(number, email)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }


        /// <summary>
        /// Get Customer Pass Phrase
        /// </summary>
        public void GetCustomerPassPhrase()
        {
            log.LogMethodEntry();
            ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParam = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParafaitDefaultsParam.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "CUSTOMER_ENCRYPTION_PASS_PHRASE"));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParam);
            if (parafaitDefaultsDTOList != null)
            {
                string encryptedPassPhrase = parafaitDefaultsDTOList[0].DefaultValue;
                passPhrase = Encryption.Decrypt(encryptedPassPhrase);
            }
            log.LogMethodExit(passPhrase);
        }
    }
}
