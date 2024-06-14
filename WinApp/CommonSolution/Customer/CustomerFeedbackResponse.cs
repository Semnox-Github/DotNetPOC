/********************************************************************************************
 * Project Name - Customer Feedback Response
 * Description  - A high level structure created to classify the Customer Feedback Response 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019   Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.3      19-Jul-2019   Girish Kundar       Modified : As per REST API requirement
 *2.80        09-Mar-2020   Mushahid Faizan     Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackResponse
    /// </summary>
    public class CustomerFeedbackResponse
    {
        private CustomerFeedbackResponseDTO customerFeedbackResponseDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CustomerFeedbackResponse class
        /// </summary>
        private CustomerFeedbackResponse(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Response id as the parameter
        /// Would fetch the Customer Feedback Response object from the database based on the id passed. 
        /// </summary>
        /// <param name="CustFbResponseId">Customer Feedback Response id</param>
        public CustomerFeedbackResponse(ExecutionContext executionContext, int CustFbResponseId,
                                        bool loadChildRecords = false,
                                        bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, CustFbResponseId);
            CustomerFeedbackResponseDataHandler customerFeedbackResponseDataHandler = new CustomerFeedbackResponseDataHandler(sqlTransaction);
            customerFeedbackResponseDTO = customerFeedbackResponseDataHandler.GetCustomerFeedbackResponse(CustFbResponseId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CustomerFeedbackResponseValuesList customerFeedbackSurveyDataListBL = new CustomerFeedbackResponseValuesList(executionContext);
            List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID, customerFeedbackResponseDTO.CustFbResponseId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "1"));
            }
            customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList = customerFeedbackSurveyDataListBL.GetAllCustomerFeedbackResponseValues(searchParameters, -1, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Response object using the CustomerFeedbackResponseDTO
        /// </summary>
        /// <param name="customerFeedbackResponse">CustomerFeedbackResponseDTO object</param>
        public CustomerFeedbackResponse(ExecutionContext executionContext, CustomerFeedbackResponseDTO customerFeedbackResponseDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackResponseDTO);
            this.customerFeedbackResponseDTO = customerFeedbackResponseDTO;
            log.LogMethodExit();
        }

       

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackResponseDTO.IsChangedRecursive == false && customerFeedbackResponseDTO.CustFbResponseId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CustomerFeedbackResponseDataHandler customerFeedbackResponseDataHandler = new CustomerFeedbackResponseDataHandler(sqlTransaction);
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (customerFeedbackResponseDTO.CustFbResponseId < 0)
                {
                    customerFeedbackResponseDTO = customerFeedbackResponseDataHandler.InsertCustomerFeedbackResponse(customerFeedbackResponseDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerFeedbackResponseDTO.AcceptChanges();
                }
                else if (customerFeedbackResponseDTO.IsChanged)
                {
                    customerFeedbackResponseDTO = customerFeedbackResponseDataHandler.UpdateCustomerFeedbackResponse(customerFeedbackResponseDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerFeedbackResponseDTO.AcceptChanges();
                }
                SaveFeedbackResponseValues(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : CustomerFeedbackResponseValuesDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveFeedbackResponseValues(SqlTransaction sqlTransaction)
        {
            if (customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList != null &&
                customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList.Any())
            {
                List<CustomerFeedbackResponseValuesDTO> updatedCustomerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();
                foreach (var customerFeedbackResponseValuesDTO in customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList)
                {
                    if (customerFeedbackResponseValuesDTO.CustFbResponseId != customerFeedbackResponseDTO.CustFbResponseId)
                    {
                        customerFeedbackResponseValuesDTO.CustFbResponseId = customerFeedbackResponseDTO.CustFbResponseId;
                    }
                    if (customerFeedbackResponseValuesDTO.IsChanged)
                    {
                        updatedCustomerFeedbackResponseValuesDTOList.Add(customerFeedbackResponseValuesDTO);
                    }
                }
                if (updatedCustomerFeedbackResponseValuesDTOList.Any())
                {
                    CustomerFeedbackResponseValuesList customerFeedbackResponseValuesList = new CustomerFeedbackResponseValuesList(executionContext, updatedCustomerFeedbackResponseValuesDTOList);
                    customerFeedbackResponseValuesList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the customerFeedbackResponseDTO  ,customerFeedbackResponseValuesDTO - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            // Validation Logic here

            if (customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList != null && customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList.Any())
            {
                foreach (var customerFeedbackResponseValuesDTO in customerFeedbackResponseDTO.CustomerFeedbackResponseValuesDTOList)
                {
                    if (customerFeedbackResponseValuesDTO.IsChanged)
                    {
                        CustomerFeedbackResponseValues customerFeedbackResponseValues = new CustomerFeedbackResponseValues(executionContext, customerFeedbackResponseValuesDTO);
                        validationErrorList.AddRange(customerFeedbackResponseValues.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerFeedbackResponseDTO GetCustomerFeedbackResponseDTO { get { return customerFeedbackResponseDTO; } }
    }

    /// <summary>
    /// Manages the list of Customer Feedback Responses
    /// </summary>
    public class CustomerFeedbackResponseList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = new List<CustomerFeedbackResponseDTO>();

        /// <summary>
        /// parameterized Constructor with Execution Context
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerFeedbackResponseList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        ///  parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerFeedbackResponseDTOList"></param>
        public CustomerFeedbackResponseList(ExecutionContext executionContext, List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList) : this (executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackResponseDTOList);
            this.customerFeedbackResponseDTOList = customerFeedbackResponseDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Customer Feedback Response
        /// </summary>
        public CustomerFeedbackResponseDTO GetCustomerFeedbackResponse(int CustFbResponseId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(CustFbResponseId, sqlTransaction);
            CustomerFeedbackResponseDataHandler customerFeedbackResponseDataHandler = new CustomerFeedbackResponseDataHandler(sqlTransaction);
            CustomerFeedbackResponseDTO customerFeedbackResponseDTO = customerFeedbackResponseDataHandler.GetCustomerFeedbackResponse(CustFbResponseId);
            log.LogMethodExit(customerFeedbackResponseDTO);
            return customerFeedbackResponseDTO;
        }

        /// <summary>
        /// Returns the Customer Feedback Response list
        /// </summary>
        public List<CustomerFeedbackResponseDTO> GetAllCustomerFeedbackResponse(List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerFeedbackResponseDataHandler customerFeedbackResponseDataHandler = new CustomerFeedbackResponseDataHandler();
            this.customerFeedbackResponseDTOList = customerFeedbackResponseDataHandler.GetCustomerFeedbackResponseList(searchParameters);
            log.LogMethodEntry(customerFeedbackResponseDTOList);
            return customerFeedbackResponseDTOList;
        }

        public List<CustomerFeedbackResponseDTO> GetAllCustomerFeedbackResponseDTOList(List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchParameters,
                                         bool loadChildRecords = false, bool activeChildRecords = true,
                                         SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            CustomerFeedbackResponseDataHandler customerFeedbackResponseDataHandler = new CustomerFeedbackResponseDataHandler(sqlTransaction);
            this.customerFeedbackResponseDTOList = customerFeedbackResponseDataHandler.GetCustomerFeedbackResponseList(searchParameters, sqlTransaction);
            if (customerFeedbackResponseDTOList != null && customerFeedbackResponseDTOList.Any() && loadChildRecords)
            {
                Build(customerFeedbackResponseDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerFeedbackResponseDTOList);
            return customerFeedbackResponseDTOList;
        }

        private void Build(List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerFeedbackResponseDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CustomerFeedbackResponseDTO> customerFeedbackResponseIdDictionary = new Dictionary<int, CustomerFeedbackResponseDTO>();
            StringBuilder sb = new StringBuilder("");
            string CustFbResponseIdList;
            for (int i = 0; i < customerFeedbackResponseDTOList.Count; i++)
            {
                if (customerFeedbackResponseDTOList[i].CustFbResponseId == -1 ||
                    customerFeedbackResponseIdDictionary.ContainsKey(customerFeedbackResponseDTOList[i].CustFbResponseId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(customerFeedbackResponseDTOList[i].CustFbResponseId.ToString());
                customerFeedbackResponseIdDictionary.Add(customerFeedbackResponseDTOList[i].CustFbResponseId, customerFeedbackResponseDTOList[i]);
            }
            CustFbResponseIdList = sb.ToString();
            CustomerFeedbackResponseValuesList customerFeedbackResponseValuesListBL = new CustomerFeedbackResponseValuesList(executionContext);
            List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.CUST_FB_RESPONSE_ID_LIST, CustFbResponseIdList.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>(CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters.IS_ACTIVE, "1"));
            }
            List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = customerFeedbackResponseValuesListBL.GetAllCustomerFeedbackResponseValues(searchParameters, -1, sqlTransaction);
            if (customerFeedbackResponseValuesDTOList != null && customerFeedbackResponseValuesDTOList.Any())
            {
                log.LogVariableState("customerFeedbackResponseValuesDTOList", customerFeedbackResponseValuesDTOList);
                foreach (var customerFeedbackResponseValuesDTO in customerFeedbackResponseValuesDTOList)
                {
                    if (customerFeedbackResponseIdDictionary.ContainsKey(customerFeedbackResponseValuesDTO.CustFbResponseId))
                    {
                        if (customerFeedbackResponseIdDictionary[customerFeedbackResponseValuesDTO.CustFbResponseId].CustomerFeedbackResponseValuesDTOList == null)
                        {
                            customerFeedbackResponseIdDictionary[customerFeedbackResponseValuesDTO.CustFbResponseId].CustomerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();
                        }
                        customerFeedbackResponseIdDictionary[customerFeedbackResponseValuesDTO.CustFbResponseId].CustomerFeedbackResponseValuesDTOList.Add(customerFeedbackResponseValuesDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of customerFeedbackResponseDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackResponseDTOList == null ||
                customerFeedbackResponseDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < customerFeedbackResponseDTOList.Count; i++)
            {
                var customerFeedbackResponseDTO = customerFeedbackResponseDTOList[i];
                if (customerFeedbackResponseDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CustomerFeedbackResponse customerFeedbackResponse = new CustomerFeedbackResponse(executionContext, customerFeedbackResponseDTO);
                    customerFeedbackResponse.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackResponseDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackResponseDTO", customerFeedbackResponseDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
