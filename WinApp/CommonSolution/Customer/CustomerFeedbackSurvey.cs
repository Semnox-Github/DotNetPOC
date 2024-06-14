/********************************************************************************************
 * Project Name - Customer Feedback Survey
 * Description  - A high level structure created to classify the Customer Feedback Survey 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019   Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.3      21-02-2020    Girish Kundar       Modified : 3 tier Changes for REST API
 *2.80        09-Mar-2020   Mushahid Faizan     Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackSurvey
    /// </summary>
    public class CustomerFeedbackSurvey
    {
        private CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of CustomerFeedbackSurvey class
        /// </summary>
        private CustomerFeedbackSurvey(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Survey id as the parameter
        /// Would fetch the Customer Feedback Survey object from the database based on the id passed. 
        /// </summary>
        /// <param name="customerFeedbackSurveyId">Customer Feedback Survey id</param>
        public CustomerFeedbackSurvey(ExecutionContext executionContext, int customerFeedbackSurveyId,
                                      bool loadChildRecords = false,
                                      bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyId);
            CustomerFeedbackSurveyDataHandler customerFeedbackSurveyDataHandler = new CustomerFeedbackSurveyDataHandler(sqlTransaction);
            customerFeedbackSurveyDTO = customerFeedbackSurveyDataHandler.GetCustomerFeedbackSurvey(customerFeedbackSurveyId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingList = new CustomerFeedbackSurveyPOSMappingList(executionContext);
            List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID, customerFeedbackSurveyDTO.CustFbSurveyId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "1"));
            }
            customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingList.GetAllCustomerFeedbackSurveyPOSMapping(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Survey object using the CustomerFeedbackSurveyDTO
        /// </summary>
        /// <param name="customerFeedbackSurvey">CustomerFeedbackSurveyDTO object</param>
        public CustomerFeedbackSurvey(ExecutionContext executionContext, CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyDTO);
            this.customerFeedbackSurveyDTO = customerFeedbackSurveyDTO;
            log.LogMethodExit();
        }


        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDTO.IsChangedRecursive == false && customerFeedbackSurveyDTO.CustFbSurveyId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CustomerFeedbackSurveyDataHandler customerFeedbackSurveyDataHandler = new CustomerFeedbackSurveyDataHandler(sqlTransaction);
            Validate();
            if (customerFeedbackSurveyDTO.CustFbSurveyId < 0)
            {
                customerFeedbackSurveyDTO = customerFeedbackSurveyDataHandler.InsertCustomerFeedbackSurvey(customerFeedbackSurveyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerFeedbackSurveyDTO.AcceptChanges();
            }
            else if (customerFeedbackSurveyDTO.IsChanged)
            {
                customerFeedbackSurveyDTO = customerFeedbackSurveyDataHandler.UpdateCustomerFeedbackSurvey(customerFeedbackSurveyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerFeedbackSurveyDTO.AcceptChanges();
            }
            SaveFeedbackSurveyPOSMapping(sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the child records : CustomerFeedbackSurveyPOSMappingDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveFeedbackSurveyPOSMapping(SqlTransaction sqlTransaction)
        {
            if (customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList != null &&
                customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList.Any())
            {
                List<CustomerFeedbackSurveyPOSMappingDTO> updatedCustomerFeedbackSurveyPOSMappingDTOList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
                foreach (var customerFeedbackSurveyPOSMappingDTO in customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList)
                {
                    if (customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId != customerFeedbackSurveyDTO.CustFbSurveyId)
                    {
                        customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId = customerFeedbackSurveyDTO.CustFbSurveyId;
                    }
                    if (customerFeedbackSurveyPOSMappingDTO.IsChanged)
                    {
                        updatedCustomerFeedbackSurveyPOSMappingDTOList.Add(customerFeedbackSurveyPOSMappingDTO);
                    }
                }
                if (updatedCustomerFeedbackSurveyPOSMappingDTOList.Any())
                {
                    CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingBL = new CustomerFeedbackSurveyPOSMappingList(executionContext, updatedCustomerFeedbackSurveyPOSMappingDTOList);
                    customerFeedbackSurveyPOSMappingBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the customerFeedbackSurveyDTO  ,CustomerFeedbackSurveyPOSMappingDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList != null && customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList.Any())
            {
                foreach (var customerFeedbackSurveyPOSMappingDTO in customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList)
                {
                    if (customerFeedbackSurveyPOSMappingDTO.IsChanged)
                    {
                        CustomerFeedbackSurveyPOSMapping customerFeedbackSurveyPOSMappingBL = new CustomerFeedbackSurveyPOSMapping(executionContext, customerFeedbackSurveyPOSMappingDTO);
                        customerFeedbackSurveyPOSMappingBL.Validate(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerFeedbackSurveyDTO GetCustomerFeedbackSurveyDTO { get { return customerFeedbackSurveyDTO; } }


    }

    /// <summary>
    /// Manages the list of Customer Feedback Surveys
    /// </summary>
    public class CustomerFeedbackSurveyList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = new List<CustomerFeedbackSurveyDTO>();
        /// <summary>
        /// Returns the Customer Feedback Survey
        /// </summary>
        public CustomerFeedbackSurveyList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CustomerFeedbackSurveyList(ExecutionContext executionContext, List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackSurveyDTOList);
            this.customerFeedbackSurveyDTOList = customerFeedbackSurveyDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Customer Feedback Survey list
        /// </summary>
        public List<CustomerFeedbackSurveyDTO> GetAllCustomerFeedbackSurvey(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchParameters,
                                               bool loadChildRecords = false, bool activeChildRecords = true,
                                               SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerFeedbackSurveyDataHandler customerFeedbackSurveyDataHandler = new CustomerFeedbackSurveyDataHandler(sqlTransaction);
            List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = customerFeedbackSurveyDataHandler.GetCustomerFeedbackSurveyList(searchParameters);
            if (customerFeedbackSurveyDTOList != null && customerFeedbackSurveyDTOList.Any() && loadChildRecords)
            {
                Build(customerFeedbackSurveyDTOList, -1, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerFeedbackSurveyDTOList);
            return customerFeedbackSurveyDTOList;
        }

        public List<CustomerFeedbackSurveyDTO> GetAllCustomerFeedbackSurvey(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchParameters, string posMachine,
                                              bool loadChildRecords = false, bool activeChildRecords = true,
                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, posMachine, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomerFeedbackSurveyDataHandler customerFeedbackSurveyDataHandler = new CustomerFeedbackSurveyDataHandler(sqlTransaction);
            List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = null;
            int posMachineId = -1;
            if (!string.IsNullOrEmpty(posMachine))
            {
                customerFeedbackSurveyDataHandler = new CustomerFeedbackSurveyDataHandler(sqlTransaction);
                posMachineId = customerFeedbackSurveyDataHandler.GetPosMachineId(posMachine, executionContext.GetSiteId());
                if (posMachineId < 0)
                {
                    string errormessage = MessageContainerList.GetMessage(executionContext, 1648, posMachine);
                    log.Error("PosMachine Not Found");
                    throw new ValidationException(errormessage);
                }
                CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingListBL = new CustomerFeedbackSurveyPOSMappingList(executionContext);
                List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchPosMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
                if (activeChildRecords)
                {
                    searchPosMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "1"));
                }
                searchPosMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID, posMachineId.ToString()));
                List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingListBL.GetAllCustomerFeedbackSurveyPOSMapping(searchPosMappingParameters, sqlTransaction);
                if (customerFeedbackSurveyPOSMappingDTOList != null && customerFeedbackSurveyPOSMappingDTOList.Any())
                {
                    Dictionary<int, CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyIdDictionary = new Dictionary<int, CustomerFeedbackSurveyPOSMappingDTO>();
                    StringBuilder sb = new StringBuilder("");
                    string customerFeedbackSurveyIdList;
                    log.LogVariableState("customerFeedbackSurveyPOSMappingDTOList", customerFeedbackSurveyPOSMappingDTOList);
                    for (int i = 0; i < customerFeedbackSurveyPOSMappingDTOList.Count; i++)
                    {
                        if (customerFeedbackSurveyPOSMappingDTOList[i].CustFbSurveyId == -1 ||
                            customerFeedbackSurveyIdDictionary.ContainsKey(customerFeedbackSurveyPOSMappingDTOList[i].CustFbSurveyId))
                        {
                            continue;
                        }
                        if (i != 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(customerFeedbackSurveyPOSMappingDTOList[i].CustFbSurveyId.ToString());
                        customerFeedbackSurveyIdDictionary.Add(customerFeedbackSurveyPOSMappingDTOList[i].CustFbSurveyId, customerFeedbackSurveyPOSMappingDTOList[i]);
                    }
                    customerFeedbackSurveyIdList = sb.ToString();
                    searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.CUST_FB_SURVEY_ID_LIST, customerFeedbackSurveyIdList.ToString()));
                    customerFeedbackSurveyDTOList = customerFeedbackSurveyDataHandler.GetCustomerFeedbackSurveyList(searchParameters);
                }
            }
            else
            {
                customerFeedbackSurveyDTOList = customerFeedbackSurveyDataHandler.GetCustomerFeedbackSurveyList(searchParameters);
            }
            if (customerFeedbackSurveyDTOList != null && customerFeedbackSurveyDTOList.Any() && loadChildRecords)
            {
                Build(customerFeedbackSurveyDTOList, posMachineId, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerFeedbackSurveyDTOList);
            return customerFeedbackSurveyDTOList;
        }

        private void Build(List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList, int posMachineId = -1, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerFeedbackSurveyDTOList, posMachineId, activeChildRecords, sqlTransaction);
            Dictionary<int, CustomerFeedbackSurveyDTO> customerFeedbackSurveyIdDictionary = new Dictionary<int, CustomerFeedbackSurveyDTO>();
            StringBuilder sb = new StringBuilder("");
            string customerFeedbackSurveyIdList;
            for (int i = 0; i < customerFeedbackSurveyDTOList.Count; i++)
            {
                if (customerFeedbackSurveyDTOList[i].CustFbSurveyId == -1 ||
                    customerFeedbackSurveyIdDictionary.ContainsKey(customerFeedbackSurveyDTOList[i].CustFbSurveyId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(customerFeedbackSurveyDTOList[i].CustFbSurveyId.ToString());
                customerFeedbackSurveyIdDictionary.Add(customerFeedbackSurveyDTOList[i].CustFbSurveyId, customerFeedbackSurveyDTOList[i]);
            }
            customerFeedbackSurveyIdList = sb.ToString();
            CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingListBL = new CustomerFeedbackSurveyPOSMappingList(executionContext);
            List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID_LIST, customerFeedbackSurveyIdList.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "1"));
            }
            if (posMachineId > -1)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID, posMachineId.ToString()));
            }
            List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingListBL.GetAllCustomerFeedbackSurveyPOSMapping(searchParameters, sqlTransaction);
            if (customerFeedbackSurveyPOSMappingDTOList != null && customerFeedbackSurveyPOSMappingDTOList.Any())
            {
                log.LogVariableState("customerFeedbackSurveyPOSMappingDTOList", customerFeedbackSurveyPOSMappingDTOList);
                foreach (var customerFeedbackSurveyPOSMappingDTO in customerFeedbackSurveyPOSMappingDTOList)
                {
                    if (customerFeedbackSurveyIdDictionary.ContainsKey(customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId))
                    {
                        if (customerFeedbackSurveyIdDictionary[customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId].CustomerFeedbackSurveyPOSMappingDTOList == null)
                        {
                            customerFeedbackSurveyIdDictionary[customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId].CustomerFeedbackSurveyPOSMappingDTOList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
                        }
                        customerFeedbackSurveyIdDictionary[customerFeedbackSurveyPOSMappingDTO.CustFbSurveyId].CustomerFeedbackSurveyPOSMappingDTOList.Add(customerFeedbackSurveyPOSMappingDTO);
                    }
                }
            }
            if (posMachineId > -1)
            {
                CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList(executionContext);
                List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                searchSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID_LIST, customerFeedbackSurveyIdList));
                if (activeChildRecords)
                {
                    searchSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                }
                List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchSurveyDetailsParameters, true, activeChildRecords, sqlTransaction);
                if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Any())
                {
                    foreach (var customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsDTOList)
                    {
                        if (customerFeedbackSurveyIdDictionary.ContainsKey(customerFeedbackSurveyDetailsDTO.CustFbSurveyId))
                        {
                            if (customerFeedbackSurveyIdDictionary[customerFeedbackSurveyDetailsDTO.CustFbSurveyId].SurveyDetails == null)
                            {
                                customerFeedbackSurveyIdDictionary[customerFeedbackSurveyDetailsDTO.CustFbSurveyId].SurveyDetails = new List<CustomerFeedbackSurveyDetailsDTO>();
                            }
                            customerFeedbackSurveyIdDictionary[customerFeedbackSurveyDetailsDTO.CustFbSurveyId].SurveyDetails.Add(customerFeedbackSurveyDetailsDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of CustomerFeedbackSurveyDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDTOList == null ||
                customerFeedbackSurveyDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < customerFeedbackSurveyDTOList.Count; i++)
            {
                var customerFeedbackSurveyDTO = customerFeedbackSurveyDTOList[i];
                if (customerFeedbackSurveyDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CustomerFeedbackSurvey customerFeedbackSurvey = new CustomerFeedbackSurvey(executionContext, customerFeedbackSurveyDTO);
                    customerFeedbackSurvey.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackSurveyDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackSurveyDTO", customerFeedbackSurveyDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// IsValidSurvey method
        /// </summary>
        /// <returns></returns>
        public bool IsValidSurvey(int surveyId, string objectName, int objectId, DateTime dtDate, string criteria, int languageId, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(surveyId, objectName, objectId, dtDate, criteria, languageId, machineUserContext);
            CustomerFeedbackSurveyMappingList customerFeedbackSurveyMappingList = new CustomerFeedbackSurveyMappingList(machineUserContext);
            List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList = new List<CustomerFeedbackSurveyMappingDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> searchByCustomerFeedbackSurveyMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>>();
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList();
            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_FEEDBACK_CRITERIA"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, criteria));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_NAME, objectName.ToString()));
            searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_ID, objectId.ToString()));
            customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingList.GetAllCustomerFeedbackSurveyMapping(searchByCustomerFeedbackSurveyMappingParameters);
            if (customerFeedbackSurveyMappingDTOList != null && customerFeedbackSurveyMappingDTOList.Any())//Survey in progress for customer
            {
                foreach (CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO in customerFeedbackSurveyMappingDTOList)
                {
                    searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID, customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId.ToString()));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                    customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                    customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);
                    if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
                    {
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                        {
                            if (lookupValuesDTOList[0].LookupValueId != customerFeedbackSurveyDetailsDTOList[0].CriteriaId)
                            {
                                return false;
                            }
                        }
                        if (dtDate.Date.Equals(customerFeedbackSurveyMappingDTO.LastVisitDate.Date) || (customerFeedbackSurveyDetailsDTOList[0].NextQuestionId == -1 && !customerFeedbackSurveyDetailsDTOList[0].IsRecur))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));

                //List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetCustomerFeedbackSurveyDetailsOfInitialLoadList(searchByCustomerFeedbackSurveyDetailsParameters);
                if (customerFeedbackSurveyDetailsDTOList != null)
                {
                    List<CustomerFeedbackSurveyDetailsDTO> populatedSurveyDetailsList = new List<CustomerFeedbackSurveyDetailsDTO>();

                    foreach (CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsDTOList)
                    {
                        if (lookupValuesDTOList[0].LookupValueId != customerFeedbackSurveyDetailsDTOList[0].CriteriaId)
                        {
                            return false;
                        }
                        CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(executionContext);
                        CustomerFeedbackQuestionsDTO customerFeedbackQuestionDTO = customerFeedbackSurveyList.GetPopulatedQuestion(machineUserContext, customerFeedbackSurveyDetailsDTO.CustFbQuestionId, languageId);
                        if (customerFeedbackQuestionDTO != null)
                        {
                            return true;
                        }
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Returns the Customer Feedback Survey list matching the parameters
        /// </summary>
        public List<CustomerFeedbackSurveyDTO> FetchSurvey(int posMachineID, string surveyType, int customerId, int languageId, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(posMachineID, surveyType, customerId, languageId, machineUserContext);
            List<CustomerFeedbackSurveyDTO> surveys = new List<CustomerFeedbackSurveyDTO>();

            CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(machineUserContext);
            CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(executionContext);
            List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = new List<CustomerFeedbackSurveyDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchBycustomerFeedbackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>>();

            //CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingList = new CustomerFeedbackSurveyPOSMappingList(machineUserContext);
            //List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
            //List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchByCustomerFeedbackSurveyPOSMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, "1"));
            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            customerFeedbackSurveyDTOList = customerFeedbackSurveyList.GetAllCustomerFeedbackSurvey(searchBycustomerFeedbackSurveyParameters, true, true);
            if (customerFeedbackSurveyDTOList != null && customerFeedbackSurveyDTOList.Count > 0)
            {
                foreach (CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO in customerFeedbackSurveyDTOList)
                {
                    //searchByCustomerFeedbackSurveyPOSMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
                    //searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "1"));
                    //searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    //searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID, customerFeedbackSurveyDTO.CustFbSurveyId.ToString()));
                    //searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID, posMachineID.ToString()));
                    //customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingList.GetAllCustomerFeedbackSurveyPOSMapping(searchByCustomerFeedbackSurveyPOSMappingParameters);

                    // Has survey details for all the POS , filter it to the requested POS.
                    if (customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList != null &&
                        customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList.Count > 0)
                    {
                        customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList.Where(x => x.POSMachineId == posMachineID).ToList();
                    }

                    if (customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList != null &&
                         customerFeedbackSurveyDTO.CustomerFeedbackSurveyPOSMappingDTOList.Count > 0)
                    {
                        if ((!customerFeedbackSurveyDTO.FromDate.Equals(DateTime.MinValue) &&
                            customerFeedbackSurveyDTO.FromDate.CompareTo(DateTime.Now) <= 0) &&
                            (customerFeedbackSurveyDTO.ToDate.Equals(DateTime.MinValue) ||
                            customerFeedbackSurveyDTO.ToDate.CompareTo(DateTime.Now) >= 0))
                        {
                            if (IsValidSurvey(customerFeedbackSurveyDTO.CustFbSurveyId, "CUSTOMER", customerId, DateTime.Today, surveyType, languageId, machineUserContext))
                            {
                                surveys.Add(customerFeedbackSurveyDTO);
                            }
                            //if (surveyType == "Transaction")
                            //{
                            //    surveys.Add(customerFeedbackSurveyDTO);
                            //}
                            //else if (surveyType != "Transaction" && !customerFeedbackSurveyDetails.IsQuestionAsked(customerFeedbackSurveyDTO.CustFbSurveyId, "CUSTOMER", customerId, DateTime.Today, "Visit Count"))
                            //{
                            //    surveys.Add(customerFeedbackSurveyDTO);
                            //}
                        }
                    }
                }
            }
            //IsCalledFromProductClick = false; //resetting flag
            //log.Debug("Ends-PerformCustomerFeedback()");
            log.LogMethodExit(surveys);
            return surveys;
        }


        /// <summary>
        /// Fetch next survey question method
        /// </summary>
        public List<CustomerFeedbackSurveyDTO> FetchNextSurveyQuestion(CustomerFeedbackSurveyMappingDTO customerFBSurveyMappingDTO, int objectId, String objectName, String criteria, int criteriaId, int posMachineId, int languageId, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(customerFBSurveyMappingDTO, objectId, objectName, criteria, criteriaId, posMachineId, languageId, machineUserContext);
            List<CustomerFeedbackSurveyDTO> CustomerFeedbackSurveyList = new List<CustomerFeedbackSurveyDTO>();
            List<CustomerFeedbackSurveyDTO> fetchedCustomerFeedbackSurveyDTOList = FetchSurvey(posMachineId, criteria, objectId, languageId, machineUserContext);
            if ((fetchedCustomerFeedbackSurveyDTOList == null) || (fetchedCustomerFeedbackSurveyDTOList.Count == 0))
                return null;

            CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(executionContext);
            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList(executionContext);

            foreach (CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO in fetchedCustomerFeedbackSurveyDTOList)
            {
                int surveyId = customerFeedbackSurveyDTO.CustFbSurveyId;

                //List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsAnswerDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                //List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = new List<CustomerFeedbackResponseDTO>();
                //List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();

                CustomerFeedbackSurveyMappingList customerFeedbackSurveyMappingList = new CustomerFeedbackSurveyMappingList(executionContext);
                List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList = new List<CustomerFeedbackSurveyMappingDTO>();
                List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> searchByCustomerFeedbackSurveyMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>>();
                searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.IS_ACTIVE, "1"));
                searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_ID, objectId.ToString()));
                searchByCustomerFeedbackSurveyMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>(CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters.OBJECT_NAME, objectName.ToString()));

                customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingList.GetAllCustomerFeedbackSurveyMapping(searchByCustomerFeedbackSurveyMappingParameters);
                if (customerFeedbackSurveyMappingDTOList != null && customerFeedbackSurveyMappingDTOList.Any())// second or next visit
                {
                    foreach (CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMappingDTO in customerFeedbackSurveyMappingDTOList)
                    {
                        if ((customerFBSurveyMappingDTO != null) && ((customerFBSurveyMappingDTO.CustFbSurveyMapId != customerFeedbackSurveyMappingDTO.CustFbSurveyMapId)))
                            continue;

                        List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                        customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetails.GetNextQuestionId(surveyId, customerFeedbackSurveyMappingDTO.LastCustFbSurveyDetailId, customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId, customerFeedbackSurveyMappingDTO.VisitCount);
                        if (customerFeedbackSurveyDetailsDTOList != null && customerFeedbackSurveyDetailsDTOList.Count > 0)
                        {
                            List<CustomerFeedbackSurveyDetailsDTO> populatedSurveyDetailsList = new List<CustomerFeedbackSurveyDetailsDTO>();
                            foreach (CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsDTOList)
                            {
                                if (customerFeedbackSurveyDetailsDTO.CriteriaId == criteriaId)//Question for the visitcount critiria if exists
                                {
                                    if (customerFeedbackSurveyDetailsDTO.NextQuestionId == -1 && customerFeedbackSurveyDetailsDTO.IsRecur && customerFeedbackSurveyMappingDTO.VisitCount >= Convert.ToInt32(customerFeedbackSurveyDetailsDTO.CriteriaValue))
                                    {
                                        CustomerFeedbackQuestionsDTO customerFeedbackQuestionDTO = GetPopulatedQuestion(machineUserContext, customerFeedbackSurveyDetailsDTO.CustFbQuestionId, languageId);
                                        if (customerFeedbackQuestionDTO != null)
                                        {
                                            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, criteriaId.ToString()));
                                            //searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, (customerFeedbackSurveyDetailsDTO.VisitCount).ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID, customerFeedbackQuestionDTO.CustFbQuestionId.ToString()));

                                            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOsList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);

                                            customerFeedbackSurveyDetailsDTOsList[0].SurveyQuestion = customerFeedbackQuestionDTO;
                                            populatedSurveyDetailsList.Add(customerFeedbackSurveyDetailsDTOsList[0]);
                                        }
                                    }
                                    else if (customerFeedbackSurveyDetailsDTO.NextQuestionId != -1)
                                    {
                                        CustomerFeedbackQuestionsDTO customerFeedbackQuestionDTO = GetPopulatedQuestion(machineUserContext, customerFeedbackSurveyDetailsDTO.NextQuestionId, languageId);
                                        if (customerFeedbackQuestionDTO != null)
                                        {
                                            List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID, criteriaId.ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE, (customerFeedbackSurveyMappingDTO.VisitCount + 1).ToString()));
                                            searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID, customerFeedbackQuestionDTO.CustFbQuestionId.ToString()));

                                            List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOsList = customerFeedbackSurveyDetailsList.GetAllCustomerFeedbackSurveyDetails(searchByCustomerFeedbackSurveyDetailsParameters);

                                            customerFeedbackSurveyDetailsDTOsList[0].SurveyQuestion = customerFeedbackQuestionDTO;
                                            populatedSurveyDetailsList.Add(customerFeedbackSurveyDetailsDTOsList[0]);
                                        }
                                    }
                                }
                            }

                            customerFeedbackSurveyDTO.SurveyDetails = populatedSurveyDetailsList;
                            CustomerFeedbackSurveyList.Add(customerFeedbackSurveyDTO);
                        }
                    }
                }
                else // First visit
                {
                    List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();

                    searchByCustomerFeedbackSurveyDetailsParameters = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, "1"));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    searchByCustomerFeedbackSurveyDetailsParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID, surveyId.ToString()));

                    List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = new List<CustomerFeedbackSurveyDetailsDTO>();
                    customerFeedbackSurveyDetailsDTOList = customerFeedbackSurveyDetailsList.GetCustomerFeedbackSurveyDetailsOfInitialLoadList(searchByCustomerFeedbackSurveyDetailsParameters);
                    if (customerFeedbackSurveyDetailsDTOList != null)
                    {
                        List<CustomerFeedbackSurveyDetailsDTO> populatedSurveyDetailsList = new List<CustomerFeedbackSurveyDetailsDTO>();

                        foreach (CustomerFeedbackSurveyDetailsDTO customerFeedbackSurveyDetailsDTO in customerFeedbackSurveyDetailsDTOList)
                        {
                            if (customerFeedbackSurveyDetailsDTO.CriteriaId == criteriaId)//Question for the visitcount critiria if exists
                            {
                                CustomerFeedbackQuestionsDTO customerFeedbackQuestionDTO = GetPopulatedQuestion(machineUserContext, customerFeedbackSurveyDetailsDTO.CustFbQuestionId, languageId);
                                if (customerFeedbackQuestionDTO != null)
                                {
                                    customerFeedbackSurveyDetailsDTO.SurveyQuestion = customerFeedbackQuestionDTO;
                                    populatedSurveyDetailsList.Add(customerFeedbackSurveyDetailsDTO);
                                }
                            }
                        }
                        customerFeedbackSurveyDTO.SurveyDetails = populatedSurveyDetailsList;
                        CustomerFeedbackSurveyList.Add(customerFeedbackSurveyDTO);
                    }
                }
            }
            log.LogMethodExit(CustomerFeedbackSurveyList);
            return CustomerFeedbackSurveyList;
        }
        /// <summary>
        /// GetPopulatedQuestion
        /// </summary>
        /// <returns></returns>
        // This should be moved to QuestionBL
        public CustomerFeedbackQuestionsDTO GetPopulatedQuestion(ExecutionContext machineUserContext, int questionId, int languageId)
        {
            log.LogMethodEntry(machineUserContext, questionId, languageId);
            CustomerFeedbackQuestionsDTO customerFeedbackQuestionDTO = null;

            List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchByCustomerFeedbackQuestionsParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID, questionId.ToString()));

            CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(machineUserContext);

            customerFeedbackQuestionDTO = customerFeedbackQuestionsList.GetCustomerFeedbackQuestion(questionId, languageId);
            //if (customerFeedbackQuestionsDTOList != null && customerFeedbackQuestionsDTOList.Count > 0)
            //{
            //    foreach (CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO in customerFeedbackQuestionsDTOList)
            //    {
            List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
            searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
            searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
            searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID, customerFeedbackQuestionDTO.CustFbResponseId.ToString()));

            CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(machineUserContext);

            List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = customerFeedbackResponseList.GetAllCustomerFeedbackResponse(searchByCustomerFeedbackResponseParameters);
            if (customerFeedbackResponseDTOList != null && customerFeedbackResponseDTOList.Count > 0)
            {
                foreach (CustomerFeedbackResponseDTO customerFeedbackResponseDTO in customerFeedbackResponseDTOList)
                {
                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOMER_RESPONSE_TYPE"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, customerFeedbackResponseDTO.ResponseTypeId.ToString()));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                    {
                        String lookupValue = lookupValuesDTOList[0].LookupValue;
                        if (lookupValue.CompareTo("MultiChoice") == 0)
                        {
                            customerFeedbackResponseDTO.ResponseType = lookupValue;
                            List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchByCustomerFeedbackResponseValuesParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
                            searchByCustomerFeedbackResponseValuesParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
                            searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "1"));
                            searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                            searchByCustomerFeedbackResponseValuesParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID, customerFeedbackResponseDTO.CustFbResponseId.ToString()));

                            CustomerFeedbackResponseValuesList customerFeedbackResponseValuesList = new CustomerFeedbackResponseValuesList(machineUserContext);
                            List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = customerFeedbackResponseValuesList.GetAllCustomerFeedbackResponseValues(searchByCustomerFeedbackResponseValuesParameters, languageId);
                            if (customerFeedbackResponseValuesDTOList != null && customerFeedbackResponseValuesDTOList.Count > 0)
                                customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList = customerFeedbackResponseValuesDTOList;
                            else
                                customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList = null;
                        }
                        else
                        {
                            customerFeedbackResponseDTO.ResponseType = lookupValue;
                            customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList = null;
                        }
                    }

                    customerFeedbackQuestionDTO.QuestionResponse = customerFeedbackResponseDTO;
                }
            }//should an exception be thrown or null be returned if no question is available
             //    }
             //}//should an exception be thrown here or null be returned if no question is available
            log.LogMethodExit(customerFeedbackQuestionDTO);
            return customerFeedbackQuestionDTO;
        }

    }
}
