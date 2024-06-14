/********************************************************************************************
 * Project Name - Customer Feedback Survey Mapping
 * Description  - A high level structure created to classify the Customer Feedback Survey Mapping 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic of saving  CustomerFeedbackSurveyMapping
    /// </summary>
    public class CustomerFeedbackSurveyMapping
    {
        private CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMapping;
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyMapping class
        /// </summary>
        private CustomerFeedbackSurveyMapping(ExecutionContext executionContext)
        {
            log.LogMethodExit();
            this.executionContext = executionContext;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Survey Mapping id as the parameter
        /// Would fetch the Customer Feedback Survey Mapping object from the database based on the id passed. 
        /// </summary>
        /// <param name="customerFeedbackSurveyMappingId">Customer Feedback Survey Mapping id</param>
        public CustomerFeedbackSurveyMapping(ExecutionContext executionContext ,int customerFeedbackSurveyMappingId ,SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyMappingId, sqlTransaction);
            CustomerFeedbackSurveyMappingDataHandler customerFeedbackSurveyMappingDataHandler = new CustomerFeedbackSurveyMappingDataHandler(sqlTransaction);
            customerFeedbackSurveyMapping = customerFeedbackSurveyMappingDataHandler.GetCustomerFeedbackSurveyMapping(customerFeedbackSurveyMappingId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Survey Mapping object using the CustomerFeedbackSurveyMappingDTO
        /// </summary>
        /// <param name="customerFeedbackSurveyMapping">CustomerFeedbackSurveyMappingDTO object</param>
        public CustomerFeedbackSurveyMapping(ExecutionContext executionContext ,CustomerFeedbackSurveyMappingDTO customerFeedbackSurveyMapping)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyMapping);
            this.customerFeedbackSurveyMapping = customerFeedbackSurveyMapping;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Customer Feedback Survey Mapping
        /// Checks if the CustomerFeedbackSurveyMapping id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            CustomerFeedbackSurveyMappingDataHandler customerFeedbackSurveyMappingDataHandler = new CustomerFeedbackSurveyMappingDataHandler();
            if (customerFeedbackSurveyMapping.CustFbSurveyMapId < 0)
            {
                 customerFeedbackSurveyMapping = customerFeedbackSurveyMappingDataHandler.InsertCustomerFeedbackSurveyMapping(customerFeedbackSurveyMapping, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                customerFeedbackSurveyMapping.AcceptChanges();
            }
            else
            {
                if (customerFeedbackSurveyMapping.IsChanged)
                {
                    customerFeedbackSurveyMapping = customerFeedbackSurveyMappingDataHandler.UpdateCustomerFeedbackSurveyMapping(customerFeedbackSurveyMapping, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    customerFeedbackSurveyMapping.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerFeedbackSurveyMappingDTO GetCustomerFeedbackSurveyMapping { get { return customerFeedbackSurveyMapping; } }
    
    }

    /// <summary>
    /// Manages the list of Customer Feedback Survey Mapping
    /// </summary>
    public class CustomerFeedbackSurveyMappingList
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public CustomerFeedbackSurveyMappingList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Returns the Customer Feedback Survey Mapping list
        /// </summary>
        public List<CustomerFeedbackSurveyMappingDTO> GetAllCustomerFeedbackSurveyMapping(List<KeyValuePair<CustomerFeedbackSurveyMappingDTO.SearchByCustomerFeedbackSurveyMappingParameters, string>> searchParameters ,SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerFeedbackSurveyMappingDataHandler customerFeedbackSurveyMappingDataHandler = new CustomerFeedbackSurveyMappingDataHandler(sqlTransaction);
            List<CustomerFeedbackSurveyMappingDTO> customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyMappingDataHandler.GetCustomerFeedbackSurveyMappingList(searchParameters);
            log.LogMethodEntry(customerFeedbackSurveyMappingDTOList);
            return customerFeedbackSurveyMappingDTOList;
        }
    }

}
