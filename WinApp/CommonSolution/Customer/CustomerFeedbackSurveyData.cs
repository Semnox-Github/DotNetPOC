/********************************************************************************************
 * Project Name - Customer Feedback Survey DataSet
 * Description  - A high level structure created to classify the Customer Feedback Survey DataSet 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera          Created 
 *2.70.2      19-Jul-2019   Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.3       21-02-2020     Girish Kundar     Modified : 3 tier Changes for REST API
 *2.80        24-Feb-2020   Mushahid Faizan  Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackSurveyData
    /// </summary>
    public class CustomerFeedbackSurveyData
    {
        private CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyData class
        /// </summary>
        private CustomerFeedbackSurveyData(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CustomerFeedbackSurveyData(ExecutionContext executionContext,
                                           CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.customerFeedbackSurveyDataDTO = customerFeedbackSurveyDataDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Survey DataSet id as the parameter
        /// Would fetch the Customer Feedback Survey DataSet object from the database based on the id passed. 
        /// </summary>
        /// <param name="customerFeedbackSurveyDataId">Customer Feedback Survey DataSet id</param>
        public CustomerFeedbackSurveyData(ExecutionContext executionContext, int customerFeedbackSurveyDataId,
                                          SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataId, sqlTransaction);
            CustomerFeedbackSurveyDataDataHandler customerFeedbackSurveyDataDataHandler = new CustomerFeedbackSurveyDataDataHandler(sqlTransaction);
            customerFeedbackSurveyDataDTO = customerFeedbackSurveyDataDataHandler.GetCustomerFeedbackSurveyData(customerFeedbackSurveyDataId);
            if (customerFeedbackSurveyDataDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "customerFeedbackSurveyDataDTO", customerFeedbackSurveyDataId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Saves the Customer Feedback Survey DataSet
        /// Checks if the CustomerFeedbackSurveyData id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDataDTO.IsChanged == false && customerFeedbackSurveyDataDTO.CustFbSurveyDataId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CustomerFeedbackSurveyDataDataHandler customerFeedbackSurveyDataDataHandler = new CustomerFeedbackSurveyDataDataHandler(sqlTransaction);
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (customerFeedbackSurveyDataDTO.CustFbSurveyDataId < 0)
                {
                    customerFeedbackSurveyDataDTO = customerFeedbackSurveyDataDataHandler.InsertCustomerFeedbackSurveyData(customerFeedbackSurveyDataDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    customerFeedbackSurveyDataDTO.AcceptChanges();
                }
                else if (customerFeedbackSurveyDataDTO.IsChanged)
                {
                    customerFeedbackSurveyDataDTO = customerFeedbackSurveyDataDataHandler.UpdateCustomerFeedbackSurveyData(customerFeedbackSurveyDataDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    customerFeedbackSurveyDataDTO.AcceptChanges();
                }
            log.LogMethodExit();
        }


        /// <summary>
        /// Validate the customerFeedbackQuestion details
        /// </summary>
        /// <returns></returns>
        private List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerFeedbackSurveyDataDTO GetCustomerFeedbackSurveyData { get { return customerFeedbackSurveyDataDTO; } }
    }

    /// <summary>
    /// Manages the list of Customer Feedback Survey DataSets
    /// </summary>
    public class CustomerFeedbackSurveyDataList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList = new List<CustomerFeedbackSurveyDataDTO>();

        public CustomerFeedbackSurveyDataList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CustomerFeedbackSurveyDataList(ExecutionContext executionContext, List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerFeedbackSurveyDataDTOList);
            this.customerFeedbackSurveyDataDTOList = customerFeedbackSurveyDataDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Customer Feedback Survey DataSet used in the Gateway
        /// </summary>
        public CustomerFeedbackSurveyDataDTO GetCustomerFeedbackSurveyData(int CustFbSurveyDataSetId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(CustFbSurveyDataSetId, sqlTransaction);
            CustomerFeedbackSurveyDataDataHandler customerFeedbackSurveyDataDataHandler = new CustomerFeedbackSurveyDataDataHandler(sqlTransaction);
            CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataDTO = customerFeedbackSurveyDataDataHandler.GetCustomerFeedbackSurveyData(CustFbSurveyDataSetId);
            log.LogMethodExit(customerFeedbackSurveyDataDTO);
            return customerFeedbackSurveyDataDTO;
        }

        /// <summary>
        /// Returns the Customer Feedback Survey DataSet list
        /// </summary>
        public List<CustomerFeedbackSurveyDataDTO> GetAllCustomerFeedbackSurveyData(List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerFeedbackSurveyDataDataHandler customerFeedbackSurveyDataDataHandler = new CustomerFeedbackSurveyDataDataHandler(sqlTransaction);
            this.customerFeedbackSurveyDataDTOList = customerFeedbackSurveyDataDataHandler.GetCustomerFeedbackSurveyDataList(searchParameters);
            log.LogMethodExit(customerFeedbackSurveyDataDTOList);
            return customerFeedbackSurveyDataDTOList;
        }

        /// <summary>
        /// Saves the  list of customerFeedbackSurveyDataDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyDataDTOList == null ||
                customerFeedbackSurveyDataDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < customerFeedbackSurveyDataDTOList.Count; i++)
            {
                var customerFeedbackSurveyDataDTO = customerFeedbackSurveyDataDTOList[i];
                if (customerFeedbackSurveyDataDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    CustomerFeedbackSurveyData customerFeedbackSurveyData = new CustomerFeedbackSurveyData(executionContext, customerFeedbackSurveyDataDTO);
                    customerFeedbackSurveyData.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackSurveyDataDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackSurveyDataDTO", customerFeedbackSurveyDataDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
