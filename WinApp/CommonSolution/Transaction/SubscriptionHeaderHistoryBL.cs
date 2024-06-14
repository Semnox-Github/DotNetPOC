/********************************************************************************************
 * Project Name - SubscriptionHeaderHistoryBL 
 * Description  -BL class of the SubscriptionHeaderHistory 
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
    /// SubscriptionHeaderHistoryBL
    /// </summary>
    public class SubscriptionHeaderHistoryBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SubscriptionHeaderHistoryDTO subscriptionHeaderHistoryDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of SubscriptionHeaderHistoryBL class
        /// </summary>
        private SubscriptionHeaderHistoryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            subscriptionHeaderHistoryDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();

        }
        /// <summary>
        /// Creates SubscriptionHeaderHistoryBL object using the SubscriptionHeaderHistoryDTO
        /// </summary>
        /// <param name="subscriptionHeaderHistoryDTO">SubscriptionHeaderHistoryDTO object</param>
        /// <param name="executionContext">ExecutionContext object</param>
        public SubscriptionHeaderHistoryBL(ExecutionContext executionContext, SubscriptionHeaderHistoryDTO subscriptionHeaderHistoryDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(subscriptionHeaderHistoryDTO);
            this.subscriptionHeaderHistoryDTO = subscriptionHeaderHistoryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the SubscriptionHeaderHistory id as the parameter
        /// Would fetch the SubscriptionHeaderHistory object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        /// <param name="executionContext">ExecutionContext</param>
        public SubscriptionHeaderHistoryBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            SubscriptionHeaderHistoryDataHandler SubscriptionHeaderHistoryDataHandler = new SubscriptionHeaderHistoryDataHandler(sqlTransaction);
            subscriptionHeaderHistoryDTO = SubscriptionHeaderHistoryDataHandler.GetSubscriptionHeaderHistoryDTO(id);
            if (subscriptionHeaderHistoryDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SubscriptionHeaderHistory", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(subscriptionHeaderHistoryDTO);
        }
    }
    /// <summary>
    /// SubscriptionHeaderHistoryListBL Class
    /// </summary>
    public class SubscriptionHeaderHistoryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SubscriptionHeaderHistoryDTO> subscriptionHeaderHistoryDTOList = new List<SubscriptionHeaderHistoryDTO>();

        /// <summary>
        /// Constructor
        /// </summary>
        public SubscriptionHeaderHistoryListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="subscriptionHeaderHistoryDTOList">SubscriptionHeaderHistoryDTOList</param>
        public SubscriptionHeaderHistoryListBL(ExecutionContext executionContext,
                                             List<SubscriptionHeaderHistoryDTO> subscriptionHeaderHistoryDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.subscriptionHeaderHistoryDTOList = subscriptionHeaderHistoryDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the SubscriptionHeaderHistoryDTO list
        /// </summary>
        public List<SubscriptionHeaderHistoryDTO> GetAllSubscriptionHeaderHistoryDTOList(List<KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string>> searchParameters, bool loadChild, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChild, sqlTransaction);
            SubscriptionHeaderHistoryDataHandler subscriptionHeaderHistoryDataHandler = new SubscriptionHeaderHistoryDataHandler(sqlTransaction);
            List<SubscriptionHeaderHistoryDTO> subscriptionHeaderHistoryDTOList = subscriptionHeaderHistoryDataHandler.GetSubscriptionHeaderHistoryDTOList(searchParameters);
            if (subscriptionHeaderHistoryDTOList != null && loadChild)
            {
                LoadChildren(loadChild, sqlTransaction);
            }
            log.LogMethodExit(subscriptionHeaderHistoryDTOList);
            return subscriptionHeaderHistoryDTOList;
        }

        private void LoadChildren(bool loadChild, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            Dictionary<int, SubscriptionHeaderHistoryDTO> subscriptionHeaderHistoryDTOIdMap = new Dictionary<int, SubscriptionHeaderHistoryDTO>();
            List<int> subscriptionHeaderIdList = new List<int>();
            for (int i = 0; i < subscriptionHeaderHistoryDTOList.Count; i++)
            {
                if (subscriptionHeaderHistoryDTOIdMap.ContainsKey(subscriptionHeaderHistoryDTOList[i].SubscriptionHeaderId))
                {
                    continue;
                }
                subscriptionHeaderHistoryDTOIdMap.Add(subscriptionHeaderHistoryDTOList[i].SubscriptionHeaderId, subscriptionHeaderHistoryDTOList[i]);
                subscriptionHeaderIdList.Add(subscriptionHeaderHistoryDTOList[i].SubscriptionHeaderId);
            }
            SubscriptionBillingScheduleHistoryListBL subscriptionBillingScheduleHistoryBL = new SubscriptionBillingScheduleHistoryListBL(executionContext);
            List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistory = subscriptionBillingScheduleHistoryBL.GetSubscriptionBillingScheduleHistoryDTOList(subscriptionHeaderIdList, loadChild, sqlTransaction);
            if (subscriptionBillingScheduleHistory != null && subscriptionBillingScheduleHistory.Any())
            {
                for (int i = 0; i < subscriptionBillingScheduleHistory.Count; i++)
                {
                    if (subscriptionHeaderHistoryDTOIdMap.ContainsKey(subscriptionBillingScheduleHistory[i].SubscriptionHeaderId) == false)
                    {
                        continue;
                    }
                    SubscriptionHeaderHistoryDTO subscriptionHeaderHistoryDTO = subscriptionHeaderHistoryDTOIdMap[subscriptionBillingScheduleHistory[i].SubscriptionHeaderId];
                    if (subscriptionHeaderHistoryDTO.SubscriptionBillingScheduleHistoryDTOList == null)
                    {
                        subscriptionHeaderHistoryDTO.SubscriptionBillingScheduleHistoryDTOList = new List<SubscriptionBillingScheduleHistoryDTO>();
                    }
                    subscriptionHeaderHistoryDTO.SubscriptionBillingScheduleHistoryDTOList.Add(subscriptionBillingScheduleHistory[i]);
                }
            }
            log.LogMethodExit();
        }
    }
}
