
/********************************************************************************************
 * Project Name - Customer Membership Rewards Log BL
 * Description  - BL for CustomerMembershipRewardsLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        19-Jul-2019    Girish Kundar Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.80          21-May-2020    Girish Kundar       Modified : Made default constructor as Private   
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for MembershipRewardsLog class.
    /// </summary>
    public class CustomerMembershipRewardsLogBL
    {
        private CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO;
        private readonly ExecutionContext executionContext;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomerMembershipRewardsLogBL class
        /// </summary>
        private CustomerMembershipRewardsLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the MembershipRewardsLog id as the parameter
        /// Would fetch the MembershipRewardsLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="membershipRewardsLogId">membershipRewardsLogId</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerMembershipRewardsLogBL(ExecutionContext executionContext, int membershipRewardsLogId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, membershipRewardsLogId, sqlTransaction);
            CustomerMembershipRewardsLogDataHandler customerMembershipRewardsLogDataHandler = new CustomerMembershipRewardsLogDataHandler(sqlTransaction);
            customerMembershipRewardsLogDTO = customerMembershipRewardsLogDataHandler.GetCustomerMembershipRewardsLogDTO(membershipRewardsLogId);
            if (customerMembershipRewardsLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "customerMembershipRewardsLog", membershipRewardsLogId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerMembershipRewardsLogBL object using the CustomerMembershipRewardsLogDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerMembershipRewardsLogDTO">CustomerMembershipRewardsLogDTO object</param>
        public CustomerMembershipRewardsLogBL(ExecutionContext executionContext, CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerMembershipRewardsLogDTO);
            this.customerMembershipRewardsLogDTO = customerMembershipRewardsLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the MembershipRewardsLog
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerMembershipRewardsLogDataHandler customerMembershipRewardsLogDataHandler = new CustomerMembershipRewardsLogDataHandler(sqlTransaction);
            if (customerMembershipRewardsLogDTO.MembershipRewardsLogId < 0)
            {
                customerMembershipRewardsLogDTO = customerMembershipRewardsLogDataHandler.InsertMembershipProgression(customerMembershipRewardsLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerMembershipRewardsLogDTO.AcceptChanges();
            }
            else
            {
                if (customerMembershipRewardsLogDTO.IsChanged)
                {
                    customerMembershipRewardsLogDTO = customerMembershipRewardsLogDataHandler.UpdateMembershipProgression(customerMembershipRewardsLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerMembershipRewardsLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerMembershipRewardsLogDTO CustomerMembershipRewardsLogDTO
        {
            get
            {
                return customerMembershipRewardsLogDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of MembershipRewardsLog
    /// </summary>
    public class CustomerMembershipRewardsLogList
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerMembershipRewardsLogList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the MembershipRewardsLog list
        /// </summary>
        public List<CustomerMembershipRewardsLogDTO> GetCustomerMembershipRewardsLogDTOList(List<KeyValuePair<CustomerMembershipRewardsLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerMembershipRewardsLogDataHandler customerMembershipRewardsLogDataHandler = new CustomerMembershipRewardsLogDataHandler(sqlTransaction);
            List<CustomerMembershipRewardsLogDTO> returnValue = customerMembershipRewardsLogDataHandler.GetCustomerMembershipRewardsLogDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public List<CustomerMembershipRewardsLogDTO> GetCustomerMembershipRewardsLogsByCustomerIds(List<int> customerIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerIdList, sqlTransaction);
            CustomerMembershipRewardsLogDataHandler customerMembershipRewardsLogDataHandler = new CustomerMembershipRewardsLogDataHandler(sqlTransaction);
            List<CustomerMembershipRewardsLogDTO> returnValue = customerMembershipRewardsLogDataHandler.GetCustomerMembershipRewardsLogsByCustomerIds(customerIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
