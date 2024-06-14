/********************************************************************************************
 * Project Name - Customer Feedback Response Values
 * Description  - A high level structure created to classify the Customer Feedback Response Values 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.80        09-Mar-2020   Mushahid Faizan  Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackResponseValues
    /// </summary>
    public class CustomerFeedbackResponseValues
    {
        private CustomerFeedbackResponseValuesDTO customerFeedbackResponseValuesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CustomerFeedbackResponseValues class
        /// </summary>
        private CustomerFeedbackResponseValues(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Response Values id as the parameter
        /// Would fetch the Customer Feedback Response Values object from the database based on the id passed. 
        /// </summary>
        /// <param name="customerFeedbackQuestionsId">Customer Feedback Response Values id</param>
        /// <param name="LanguageId">Integer value holds the languageid</param>
        public CustomerFeedbackResponseValues(ExecutionContext executionContext, int customerFeedbackQuestionsId, int LanguageId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackQuestionsId, LanguageId, sqlTransaction);
            CustomerFeedbackResponseValuesDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackResponseValuesDataHandler(sqlTransaction);
            customerFeedbackResponseValuesDTO = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackResponseValues(customerFeedbackQuestionsId, LanguageId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Response Values object using the CustomerFeedbackResponseValuesDTO
        /// </summary>
        /// <param name="customerFeedbackQuestions">CustomerFeedbackResponseValuesDTO object</param>
        public CustomerFeedbackResponseValues(ExecutionContext executionContext, CustomerFeedbackResponseValuesDTO customerFeedbackQuestions)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackQuestions);
            this.customerFeedbackResponseValuesDTO = customerFeedbackQuestions;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Customer Feedback Response Values
        /// Checks if the CustomerFeedbackResponseValues id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackResponseValuesDTO.IsChanged == false &&
                customerFeedbackResponseValuesDTO.CustFbResponseValueId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            CustomerFeedbackResponseValuesDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackResponseValuesDataHandler(sqlTransaction);
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (customerFeedbackResponseValuesDTO.CustFbResponseValueId < 0)
                {
                    customerFeedbackResponseValuesDTO = customerFeedbackQuestionsDataHandler.InsertCustomerFeedbackResponseValues(customerFeedbackResponseValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerFeedbackResponseValuesDTO.AcceptChanges();
                }
                else if (customerFeedbackResponseValuesDTO.IsChanged)
                {
                    customerFeedbackResponseValuesDTO = customerFeedbackQuestionsDataHandler.UpdateCustomerFeedbackResponseValues(customerFeedbackResponseValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerFeedbackResponseValuesDTO.AcceptChanges();
                }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validate the customerFeedbackSurveyPOSMapping details
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
        public CustomerFeedbackResponseValuesDTO GetCustomerFeedbackResponseValuesDTO { get { return customerFeedbackResponseValuesDTO; } }
    }

    /// <summary>
    /// Manages the list of Customer Feedback Response Values
    /// </summary>
    public class CustomerFeedbackResponseValuesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = new List<CustomerFeedbackResponseValuesDTO>();

        /// <summary>
        /// Returns the Customer Feedback Response Values
        /// </summary>
        public CustomerFeedbackResponseValuesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Customer Feedback Response Values
        /// </summary>
        public CustomerFeedbackResponseValuesList(ExecutionContext executionContext, List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackResponseValuesDTOList);
            this.customerFeedbackResponseValuesDTOList = customerFeedbackResponseValuesDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Customer Feedback Response Values list
        /// </summary>
        public List<CustomerFeedbackResponseValuesDTO> GetAllCustomerFeedbackResponseValues(List<KeyValuePair<CustomerFeedbackResponseValuesDTO.SearchByCustomerFeedbackResponseValuesParameters, string>> searchParameters, int LanguageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, LanguageId, sqlTransaction);
            CustomerFeedbackResponseValuesDataHandler customerFeedbackQuestionsDataHandler = new CustomerFeedbackResponseValuesDataHandler(sqlTransaction);
            if (LanguageId == -1)
            {
                List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackResponseValuesList(searchParameters);
                log.LogMethodExit(customerFeedbackResponseValuesDTOList);
                return customerFeedbackResponseValuesDTOList;
            }
            else
            {
                List<CustomerFeedbackResponseValuesDTO> customerFeedbackResponseValuesDTOList = customerFeedbackQuestionsDataHandler.GetCustomerFeedbackResponseValuesList(searchParameters, LanguageId);
                log.LogMethodExit(customerFeedbackResponseValuesDTOList);
                return customerFeedbackResponseValuesDTOList;
            }
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the customerFeedbackResponseValuesDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackResponseValuesDTOList == null ||
                customerFeedbackResponseValuesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < customerFeedbackResponseValuesDTOList.Count; i++)
            {
                var customerFeedbackResponseValuesDTO = customerFeedbackResponseValuesDTOList[i];
                if (customerFeedbackResponseValuesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CustomerFeedbackResponseValues customerFeedbackResponseValues = new CustomerFeedbackResponseValues(executionContext, customerFeedbackResponseValuesDTO);
                    customerFeedbackResponseValues.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackResponseValuesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackResponseValuesDTO", customerFeedbackResponseValuesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
