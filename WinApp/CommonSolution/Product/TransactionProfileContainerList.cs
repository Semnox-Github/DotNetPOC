/********************************************************************************************
 * Project Name - Product
 * Description  - TransactionProfilesContainerList class to get the List of  of values from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.130.0     1-Sep-2021       Lakshminarayana           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Product
{
    public class TransactionProfileContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, TransactionProfileContainer> transactionProfileContainerCache = new Cache<int, TransactionProfileContainer>();
        private static Timer refreshTimer;

        static TransactionProfileContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = transactionProfileContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                TransactionProfileContainer transactionProfileContainer;
                if (transactionProfileContainerCache.TryGetValue(uniqueKey, out transactionProfileContainer))
                {
                    transactionProfileContainerCache[uniqueKey] = transactionProfileContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static TransactionProfileContainer GetTransactionProfileContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            TransactionProfileContainer result = transactionProfileContainerCache.GetOrAdd(siteId, (k) => new TransactionProfileContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static TransactionProfileContainerDTOCollection GetTransactionProfileContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            TransactionProfileContainer container = GetTransactionProfileContainer(siteId);
            TransactionProfileContainerDTOCollection result = container.GetTransactionProfileContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            TransactionProfileContainer transactionProfileContainer = GetTransactionProfileContainer(siteId);
            transactionProfileContainerCache[siteId] = transactionProfileContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TransactionProfileContainerDTO based on the site and transactionProfileId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="transactionProfileId">option value transactionProfileId</param>
        /// <returns></returns>
        public static TransactionProfileContainerDTO GetTransactionProfileContainerDTO(int siteId, int transactionProfileId)
        {
            log.LogMethodEntry(siteId, transactionProfileId);
            TransactionProfileContainer transactionProfileContainer = GetTransactionProfileContainer(siteId);
            TransactionProfileContainerDTO result = transactionProfileContainer.GetTransactionProfileContainerDTO(transactionProfileId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the TransactionProfileContainerDTOList based on the site 
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="transactionProfileId">option value transactionProfileId</param>
        /// <returns></returns>
        public static List<TransactionProfileContainerDTO> GetTransactionProfileContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            TransactionProfileContainer transactionProfileContainer = GetTransactionProfileContainer(siteId);
            var result = transactionProfileContainer.GetTransactionProfileContainerDTOList();
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the TransactionProfileContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="transactionProfileId">transactionProfileId</param>
        /// <returns></returns>
        public static TransactionProfileContainerDTO GetTransactionProfileContainerDTO(ExecutionContext executionContext, int transactionProfileId)
        {
            log.LogMethodEntry(executionContext, transactionProfileId);
            TransactionProfileContainerDTO transactionProfileContainerDTO = GetTransactionProfileContainerDTO(executionContext.GetSiteId(), transactionProfileId);
            log.LogMethodExit(transactionProfileContainerDTO);
            return transactionProfileContainerDTO;
        }
    }
}
