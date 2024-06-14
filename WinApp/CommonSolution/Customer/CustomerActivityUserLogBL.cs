/********************************************************************************************
 * Project Name - CustomerActivityUserLogBL
 * Description  - Entity to capture all customer related activities
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version 
 *2.130.10    08-Sep-2022      Nitin Pai      Enhanced customer activity user log table
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public class CustomerActivityUserLogBL
    {
        private CustomerActivityUserLogDTO customerActivityUserLogDTO;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Populates the CustomerActivityUserLogDTO based on the provided ActivityUserLogId
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqltransaction"></param>
        public CustomerActivityUserLogBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            CustomerActivityUserLogDataHandler customerActivityUserLogDataHandler = new CustomerActivityUserLogDataHandler(sqltransaction);
            customerActivityUserLogDTO = customerActivityUserLogDataHandler.GetCustomerActivityUserLogDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for CustomerActivityUserLogBL class
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerActivityUserLogBL(ExecutionContext executionContext, CustomerActivityUserLogDTO customerActivityUserLogDTO)
        {
            log.LogMethodEntry(executionContext);
            this.customerActivityUserLogDTO = customerActivityUserLogDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomerActivityUserLog.
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerActivityUserLogDataHandler customerActivityUserLogDataHandler = new CustomerActivityUserLogDataHandler(sqlTransaction);
            
            if (customerActivityUserLogDTO.Id < 0)
            {
                customerActivityUserLogDTO = customerActivityUserLogDataHandler.Insert(customerActivityUserLogDTO, this.executionContext.GetUserId() , executionContext.GetSiteId());
                customerActivityUserLogDTO.AcceptChanges();
            }
            else
            {
                if (customerActivityUserLogDTO.IsChanged)
                {
                    customerActivityUserLogDTO = customerActivityUserLogDataHandler.Update(customerActivityUserLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerActivityUserLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// Get method for CustomerActivityUserLogDTO
        /// </summary>
        public CustomerActivityUserLogDTO CustomerActivityUserLogDTO
        {
            get
            {
                return customerActivityUserLogDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CustomerActivityUserLog
    /// </summary>
    public class CustomerActivityUserLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        List<CustomerActivityUserLogDTO> customerActivityUserLogDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerActivityUserLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
		/// <param name="accountDTOList"></param>
        public CustomerActivityUserLogListBL(ExecutionContext executionContext, List<CustomerActivityUserLogDTO> customerActivityUserLogDTOList)
        {
            log.LogMethodEntry(executionContext, customerActivityUserLogDTOList);
            this.executionContext = executionContext;
            this.customerActivityUserLogDTOList = customerActivityUserLogDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Account list
        /// </summary>
        public List<CustomerActivityUserLogDTO> GetCustomerActivityUserLogDTOList(List<KeyValuePair<CustomerActivityUserLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CustomerActivityUserLogDataHandler customerActivityUserLogDataHandler = new CustomerActivityUserLogDataHandler(sqlTransaction);
            List<CustomerActivityUserLogDTO> returnValue = customerActivityUserLogDataHandler.GetCustomerActivityUserLogDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
