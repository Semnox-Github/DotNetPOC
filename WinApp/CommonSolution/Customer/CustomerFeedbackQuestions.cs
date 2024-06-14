/********************************************************************************************
 * Project Name - Customer Feedback Questions
 * Description  - A high level structure created to classify the Customer Feedback Questions 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera       Created 
 *2.70.2      19-Jul-2019   Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.2      19-Jul-2019   Girish Kundar    Modified : As part of API changes
 *2.80        24-Feb-2020   Mushahid Faizan  Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackQuestions
    /// </summary>
    public class CustomerFeedbackQuestions
    {
        private CustomerFeedbackQuestionsDTO customerFeedbackQuestionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of CustomerFeedbackQuestions class
        /// </summary>
        private CustomerFeedbackQuestions(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Questions id as the parameter
        /// Would fetch the Customer Feedback Questions object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"> executionContext</param>
        /// <param name="customerFeedbackQuestionsId">Customer Feedback Questions id</param>
        /// <param name="LanguageId">Integer value holds the languageid</param>

        public CustomerFeedbackQuestions(ExecutionContext executionContext, int customerFeedbackQuestionsId, int LanguageId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackQuestionsId, LanguageId);
            CustomerFeedbackQuestionsDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackQuestionsDataHandler(sqlTransaction);
            customerFeedbackQuestionDTO = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackQuestions(customerFeedbackQuestionsId, LanguageId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Questions object using the CustomerFeedbackQuestionsDTO
        /// </summary>
        /// <param name="executionContext"> executionContext</param>
        /// <param name="customerFeedbackQuestions">CustomerFeedbackQuestionsDTO object</param>
        public CustomerFeedbackQuestions(ExecutionContext executionContext, CustomerFeedbackQuestionsDTO customerFeedbackQuestions)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackQuestions);
            this.customerFeedbackQuestionDTO = customerFeedbackQuestions;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Customer Feedback Questions
        /// Checks if the CustomerFeedbackQuestions id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackQuestionDTO.IsChanged == false
                 && customerFeedbackQuestionDTO.CustFbQuestionId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CustomerFeedbackQuestionsDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackQuestionsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (customerFeedbackQuestionDTO.CustFbQuestionId < 0)
            {
                customerFeedbackQuestionDTO = customerFeedbackQuestionsDataHandler.InsertCustomerFeedbackQuestions(customerFeedbackQuestionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerFeedbackQuestionDTO.AcceptChanges();
            }
            else if (customerFeedbackQuestionDTO.IsChanged)
            {
                customerFeedbackQuestionDTO = customerFeedbackQuestionsDataHandler.UpdateCustomerFeedbackQuestions(customerFeedbackQuestionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerFeedbackQuestionDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the customerFeedbackQuestion details
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerFeedbackQuestionsDTO CustomerFeedbackQuestionDTO { get { return customerFeedbackQuestionDTO; } }

    }

    /// <summary>
    /// Manages the list of Customer Feedback Questions
    /// </summary>
    public class CustomerFeedbackQuestionsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList = new List<CustomerFeedbackQuestionsDTO>();

        public CustomerFeedbackQuestionsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public CustomerFeedbackQuestionsList(ExecutionContext executionContext, List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackQuestionsDTOList);
            this.customerFeedbackQuestionsDTOList = customerFeedbackQuestionsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Customer Feedback Question
        /// </summary>
        public CustomerFeedbackQuestionsDTO GetCustomerFeedbackQuestion(int CustFbQuestionId, int LanguageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(CustFbQuestionId, LanguageId, sqlTransaction);
            CustomerFeedbackQuestionsDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackQuestionsDataHandler(sqlTransaction);
            CustomerFeedbackQuestionsDTO customerFeedbackQuestionsDTO = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackQuestions(CustFbQuestionId, LanguageId);
            log.LogMethodExit(customerFeedbackQuestionsDTO);
            return customerFeedbackQuestionsDTO;
        }


        /// <summary>
        /// Returns the Customer Feedback Questions list
        /// </summary>
        public List<CustomerFeedbackQuestionsDTO> GetAllCustomerFeedbackQuestions(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchParameters, int LanguageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, LanguageId, sqlTransaction);
            CustomerFeedbackQuestionsDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackQuestionsDataHandler(sqlTransaction);
            this.customerFeedbackQuestionsDTOList = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackQuestionsList(searchParameters, LanguageId);
            log.LogMethodExit(customerFeedbackQuestionsDTOList);
            return customerFeedbackQuestionsDTOList;
        }

        /// <summary>
        /// Returns the Customer Feedback Questions list
        /// </summary>
        public List<CustomerFeedbackQuestionsDTO> GetAllCustomerFeedbackQuestions(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchParameters, int LanguageId, bool buildChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, LanguageId, buildChildRecords, activeChildRecords, sqlTransaction);
            CustomerFeedbackQuestionsDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackQuestionsDataHandler(sqlTransaction);
            this.customerFeedbackQuestionsDTOList = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackQuestionsList(searchParameters, LanguageId);
            if (this.customerFeedbackQuestionsDTOList != null && this.customerFeedbackQuestionsDTOList.Any() && buildChildRecords)
            {
                Build(this.customerFeedbackQuestionsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(customerFeedbackQuestionsDTOList);
            return customerFeedbackQuestionsDTOList;
        }

        private void Build(List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerFeedbackQuestionsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CustomerFeedbackQuestionsDTO> customerFeedbackResponseIdDictionary = new Dictionary<int, CustomerFeedbackQuestionsDTO>();
            StringBuilder sb = new StringBuilder("");
            string customerFeedbackResponseIdList;
            for (int i = 0; i < customerFeedbackQuestionsDTOList.Count; i++)
            {
                if (customerFeedbackQuestionsDTOList[i].CustFbResponseId == -1 ||
                    customerFeedbackResponseIdDictionary.ContainsKey(customerFeedbackQuestionsDTOList[i].CustFbResponseId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(customerFeedbackQuestionsDTOList[i].CustFbResponseId.ToString());
                customerFeedbackResponseIdDictionary.Add(customerFeedbackQuestionsDTOList[i].CustFbResponseId, customerFeedbackQuestionsDTOList[i]);
            }
            customerFeedbackResponseIdList = sb.ToString();
            CustomerFeedbackResponseList customerFeedbackResponseListBL = new CustomerFeedbackResponseList(executionContext);
            List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID_LIST, customerFeedbackResponseIdList.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, "1"));
            }
            List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = customerFeedbackResponseListBL.GetAllCustomerFeedbackResponseDTOList(searchParameters, true, activeChildRecords, sqlTransaction);
            if (customerFeedbackResponseDTOList != null && customerFeedbackResponseDTOList.Any())
            {
                log.LogVariableState("CustomerFeedbackResponseDTO", customerFeedbackResponseDTOList);
                foreach (var customerFeedbackResponseDTO in customerFeedbackResponseDTOList)
                {
                    if (customerFeedbackResponseIdDictionary.ContainsKey(customerFeedbackResponseDTO.CustFbResponseId))
                    {
                        if (customerFeedbackResponseIdDictionary[customerFeedbackResponseDTO.CustFbResponseId].QuestionResponse == null)
                        {
                            customerFeedbackResponseIdDictionary[customerFeedbackResponseDTO.CustFbResponseId].QuestionResponse = new CustomerFeedbackResponseDTO();
                        }
                        customerFeedbackResponseIdDictionary[customerFeedbackResponseDTO.CustFbResponseId].QuestionResponse = customerFeedbackResponseDTO;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the  list of CustomerFeedbackQuestionsDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (this.customerFeedbackQuestionsDTOList == null ||
                this.customerFeedbackQuestionsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < this.customerFeedbackQuestionsDTOList.Count; i++)
            {
                var customerFeedbackQuestionsDTO = this.customerFeedbackQuestionsDTOList[i];
                if (customerFeedbackQuestionsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CustomerFeedbackQuestions customerFeedbackQuestions = new CustomerFeedbackQuestions(executionContext, customerFeedbackQuestionsDTO);
                    customerFeedbackQuestions.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackQuestionsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackQuestionsDTO", customerFeedbackQuestionsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
