/********************************************************************************************
 * Project Name - SubscriptionBillingScheduleHistoryBL 
 * Description  -BL class of the SubscriptionBillingScheduleHistory 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     14-Dec-2020    Fiona             Created for Subscription changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionBillingScheduleHistoryBL
    /// </summary>
    public class SubscriptionBillingScheduleHistoryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SubscriptionBillingScheduleHistoryDTO subscriptionBillingScheduleHistoryDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of SubscriptionBillingScheduleHistoryBL class
        /// </summary>
        private SubscriptionBillingScheduleHistoryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            subscriptionBillingScheduleHistoryDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates SubscriptionBillingScheduleHistoryBL object using the SubscriptionBillingScheduleHistoryDTO
        /// </summary>
        /// <param name="SubscriptionBillingScheduleHistoryDTO">SubscriptionBillingScheduleHistoryDTO object</param>
        /// <param name="executionContext">ExecutionContext object</param>
        public SubscriptionBillingScheduleHistoryBL(ExecutionContext executionContext, SubscriptionBillingScheduleHistoryDTO SubscriptionBillingScheduleHistoryDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(SubscriptionBillingScheduleHistoryDTO);
            this.subscriptionBillingScheduleHistoryDTO = SubscriptionBillingScheduleHistoryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the SubscriptionBillingScheduleHistory id as the parameter
        /// Would fetch the SubscriptionBillingScheduleHistory object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        /// <param name="executionContext">ExecutionContext</param>
        public SubscriptionBillingScheduleHistoryBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            SubscriptionBillingScheduleHistoryDataHandler subscriptionBillingScheduleHistoryDataHandler = new SubscriptionBillingScheduleHistoryDataHandler(sqlTransaction);
            subscriptionBillingScheduleHistoryDTO = subscriptionBillingScheduleHistoryDataHandler.GetSubscriptionBillingScheduleHistoryDTO(id);
            if (subscriptionBillingScheduleHistoryDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SubscriptionBillingScheduleHistory", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(subscriptionBillingScheduleHistoryDTO);
        }
    }
    /// <summary>
    /// SubscriptionBillingScheduleHistoryListBL
    /// </summary>
    public class SubscriptionBillingScheduleHistoryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList = new List<SubscriptionBillingScheduleHistoryDTO>();
        /// <summary>
        /// Constructor
        /// </summary>
        public SubscriptionBillingScheduleHistoryListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="subscriptionBillingScheduleHistoryDTOList">SubscriptionBillingScheduleHistoryDTOList</param>
        public SubscriptionBillingScheduleHistoryListBL(ExecutionContext executionContext,
                                             List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.subscriptionBillingScheduleHistoryDTOList = subscriptionBillingScheduleHistoryDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the SubscriptionBillingScheduleHistoryDTO list
        /// </summary>
        public List<SubscriptionBillingScheduleHistoryDTO> GetAllSubscriptionBillingScheduleHistoryDTOList(List<KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>> searchParameters,  SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters,  sqlTransaction);
            SubscriptionBillingScheduleHistoryDataHandler subscriptionBillingScheduleHistoryDataHandler = new SubscriptionBillingScheduleHistoryDataHandler(sqlTransaction);
            List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList = subscriptionBillingScheduleHistoryDataHandler.GetSubscriptionBillingScheduleHistoryDTOList(searchParameters);
            
            log.LogMethodExit(subscriptionBillingScheduleHistoryDTOList);
            return subscriptionBillingScheduleHistoryDTOList;
        }

        internal List<SubscriptionBillingScheduleHistoryDTO> GetSubscriptionBillingScheduleHistoryDTOList(List<int> IdList, bool loadChild, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(IdList, loadChild, sqlTransaction);
            SubscriptionBillingScheduleHistoryDataHandler subscriptionBillingScheduleHistoryDataHandler = new SubscriptionBillingScheduleHistoryDataHandler(sqlTransaction);
            List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList = subscriptionBillingScheduleHistoryDataHandler.GetSubscriptionBillingScheduleHistoryDTOList(IdList, loadChild);
            log.LogMethodExit(subscriptionBillingScheduleHistoryDTOList);
            return subscriptionBillingScheduleHistoryDTOList;
        }
    }
}

