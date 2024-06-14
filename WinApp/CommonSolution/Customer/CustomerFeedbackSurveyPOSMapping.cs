/********************************************************************************************
 * Project Name - Customer Feedback Survey POS Mapping
 * Description  - A high level structure created to classify the Customer Feedback Survey POS Mapping 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *1.00        05-Dec-2016    Raghuveera          Created 
 *2.70.2        19-Jul-2019    Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.80        09-Mar-2020   Mushahid Faizan     Modified : 3 tier Changes for REST API
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
    /// Business logic of saving  CustomerFeedbackSurveyPOSMapping
    /// </summary>
    public class CustomerFeedbackSurveyPOSMapping
    {
        private CustomerFeedbackSurveyPOSMappingDTO customerFeedbackSurveyPOSMapping;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CustomerFeedbackSurveyPOSMapping class
        /// </summary>
        private CustomerFeedbackSurveyPOSMapping(ExecutionContext ExecutionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = ExecutionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Customer Feedback Survey POS Mapping id as the parameter
        /// Would fetch the Customer Feedback Survey POS Mapping object from the database based on the id passed. 
        /// </summary>
        /// <param name="customerFeedbackSurveyPOSMappingId">Customer Feedback Survey POS Mapping id</param>
        public CustomerFeedbackSurveyPOSMapping(ExecutionContext executionContext, int customerFeedbackSurveyPOSMappingId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyPOSMappingId, sqlTransaction);
            CustomerFeedbackSurveyPOSMappingDataHandler customerFeedbackSurveyPOSMappingDataHandler = new CustomerFeedbackSurveyPOSMappingDataHandler(sqlTransaction);
            customerFeedbackSurveyPOSMapping = customerFeedbackSurveyPOSMappingDataHandler.GetCustomerFeedbackSurveyPOSMapping(customerFeedbackSurveyPOSMappingId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Customer Feedback Survey POS Mapping object using the CustomerFeedbackSurveyPOSMappingDTO
        /// </summary>
        /// <param name="customerFeedbackSurveyPOSMapping">CustomerFeedbackSurveyPOSMappingDTO object</param>
        public CustomerFeedbackSurveyPOSMapping(ExecutionContext executionContext, CustomerFeedbackSurveyPOSMappingDTO customerFeedbackSurveyPOSMapping)
            : this(executionContext)
        {
            log.LogMethodEntry(customerFeedbackSurveyPOSMapping);
            this.customerFeedbackSurveyPOSMapping = customerFeedbackSurveyPOSMapping;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Customer Feedback Survey POS Mapping
        /// Checks if the CustomerFeedbackSurveyPOSMapping id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (customerFeedbackSurveyPOSMapping.IsChanged == false &&
                customerFeedbackSurveyPOSMapping.CustFbSurveyPOSMappingId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            CustomerFeedbackSurveyPOSMappingDataHandler customerFeedbackSurveyPOSMappingDataHandler = new CustomerFeedbackSurveyPOSMappingDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (customerFeedbackSurveyPOSMapping.CustFbSurveyPOSMappingId < 0)
            {
                customerFeedbackSurveyPOSMapping = customerFeedbackSurveyPOSMappingDataHandler.InsertCustomerFeedbackSurveyPOSMapping(customerFeedbackSurveyPOSMapping, executionContext.GetUserId(), executionContext.GetSiteId());
                customerFeedbackSurveyPOSMapping.AcceptChanges();
            }
            else if (customerFeedbackSurveyPOSMapping.IsChanged)
            {
                customerFeedbackSurveyPOSMapping = customerFeedbackSurveyPOSMappingDataHandler.UpdateCustomerFeedbackSurveyPOSMapping(customerFeedbackSurveyPOSMapping, executionContext.GetUserId(), executionContext.GetSiteId());
                customerFeedbackSurveyPOSMapping.AcceptChanges();
            }
            log.LogMethodExit();
        }

        internal void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerFeedbackSurveyPOSMappingDataHandler customerFeedbackSurveyPOSMappingDataHandler = new CustomerFeedbackSurveyPOSMappingDataHandler(sqlTransaction);
            List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchParams = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
            searchParams.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyMappingDTOList = customerFeedbackSurveyPOSMappingDataHandler.GetCustomerFeedbackSurveyPOSMappingList(searchParams);
            if (customerFeedbackSurveyMappingDTOList != null && customerFeedbackSurveyMappingDTOList.Any())
            {
                if (customerFeedbackSurveyMappingDTOList.Exists(x => x.POSMachineId == customerFeedbackSurveyPOSMapping.POSMachineId && x.CustFbSurveyId == customerFeedbackSurveyPOSMapping.CustFbSurveyId))
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
            }
            log.LogMethodExit();
        }

    /// <summary>
    /// Gets the DTO
    /// </summary>
    public CustomerFeedbackSurveyPOSMappingDTO GetCustomerFeedbackSurveyPOSMapping { get { return customerFeedbackSurveyPOSMapping; } }
}

/// <summary>
/// Manages the list of Customer Feedback Survey POS Mapping
/// </summary>
public class CustomerFeedbackSurveyPOSMappingList
{
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private readonly ExecutionContext executionContext;
    private List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="executionContext"></param>
    public CustomerFeedbackSurveyPOSMappingList(ExecutionContext executionContext)
    {
        log.LogMethodEntry(executionContext);
        this.executionContext = executionContext;
        log.LogMethodExit();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="customerFeedbackSurveyPOSMappingDTOList"></param>
    public CustomerFeedbackSurveyPOSMappingList(ExecutionContext executionContext, List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList) : this(executionContext)
    {
        log.LogMethodEntry(executionContext, customerFeedbackSurveyPOSMappingDTOList);
        this.customerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingDTOList;
        log.LogMethodExit();
    }


    ///// <summary>
    ///// Returns the Customer Feedback Survey POS Mapping
    ///// </summary>
    //public CustomerFeedbackSurveyPOSMappingDTO GetCustomerFeedbackSurveyPOSMapping(int CustFbSurveyPOSMappingId ,SqlTransaction sqlTransaction = null)
    //{
    //    log.LogMethodEntry(CustFbSurveyPOSMappingId, sqlTransaction);
    //    CustomerFeedbackSurveyPOSMappingDataHandler customerFeedbackSurveyPOSMappingDataHandler = new CustomerFeedbackSurveyPOSMappingDataHandler(sqlTransaction);
    //    CustomerFeedbackSurveyPOSMappingDTO customerFeedbackSurveyPOSMappingDTO =  customerFeedbackSurveyPOSMappingDataHandler.GetCustomerFeedbackSurveyPOSMapping(CustFbSurveyPOSMappingId);
    //    log.LogMethodExit(customerFeedbackSurveyPOSMappingDTO);
    //    return customerFeedbackSurveyPOSMappingDTO;
    //}

    /// <summary>
    /// Returns the Customer Feedback Survey POS Mapping list
    /// </summary>
    public List<CustomerFeedbackSurveyPOSMappingDTO> GetAllCustomerFeedbackSurveyPOSMapping(List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(searchParameters, sqlTransaction);
        CustomerFeedbackSurveyPOSMappingDataHandler customerFeedbackSurveyPOSMappingDataHandler = new CustomerFeedbackSurveyPOSMappingDataHandler(sqlTransaction);
        this.customerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingDataHandler.GetCustomerFeedbackSurveyPOSMappingList(searchParameters);
        log.LogMethodExit(customerFeedbackSurveyPOSMappingDTOList);
        return customerFeedbackSurveyPOSMappingDTOList;
    }

    /// <summary>
    /// This method should be called from the Parent Class BL method Save().
    /// Saves the customerFeedbackSurveyPOSMappingDTO List
    /// Checks if the  id is not less than or equal to 0
    /// If it is less than or equal to 0, then inserts
    /// else updates
    /// </summary>
    /// <param name="sqlTransaction">SqlTransaction</param>
    internal void Save(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        if (customerFeedbackSurveyPOSMappingDTOList == null ||
            customerFeedbackSurveyPOSMappingDTOList.Any() == false)
        {
            log.LogMethodExit(null, "List is empty");
            return;
        }

        for (int i = 0; i < customerFeedbackSurveyPOSMappingDTOList.Count; i++)
        {
            var customerFeedbackSurveyPOSMappingDTO = customerFeedbackSurveyPOSMappingDTOList[i];
            if (customerFeedbackSurveyPOSMappingDTO.IsChanged == false)
            {
                continue;
            }
            try
            {
                CustomerFeedbackSurveyPOSMapping customerFeedbackSurveyPOSMapping = new CustomerFeedbackSurveyPOSMapping(executionContext, customerFeedbackSurveyPOSMappingDTO);
                customerFeedbackSurveyPOSMapping.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while saving customerFeedbackSurveyPOSMappingDTO.", ex);
                log.LogVariableState("Record Index ", i);
                log.LogVariableState("customerFeedbackSurveyPOSMappingDTO", customerFeedbackSurveyPOSMappingDTO);
                throw;
            }
        }
        log.LogMethodExit();
    }
}
}
