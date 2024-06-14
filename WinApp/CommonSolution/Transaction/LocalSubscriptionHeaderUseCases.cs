/********************************************************************************************
 * Project Name - LocalSubscriptionHeaderUseCases
 * Description  - LocalSubscriptionHeaderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 2.110.0      25-Jan-2021      Guru S A             For Subscription changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// LocalSubscriptionHeaderUseCases
    /// </summary>
    public class LocalSubscriptionHeaderUseCases : ISubscriptionHeaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalSubscriptionHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalSubscriptionHeaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetSubscriptionHeader
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param>
        /// <param name="loadChildren"></param>
        /// <returns></returns>
        public async Task<List<SubscriptionHeaderDTO>> GetSubscriptionHeader(List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters, Utilities utilities, bool loadChildren)
        {
            log.LogMethodEntry(searchParameters, loadChildren);
            return await Task<List<SubscriptionHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
                List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities, loadChildren);
                log.LogMethodExit(subscriptionHeaderDTOList);
                return subscriptionHeaderDTOList;
            });
        }
        /// <summary>
        /// SaveSubscriptionHeader
        /// </summary>
        /// <param name="subscriptionHeaderDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveSubscriptionHeader(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry(subscriptionHeaderDTOList);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    if (subscriptionHeaderDTOList == null)
                    {
                        throw new ValidationException("SubscriptionHeaderDTO list is empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    { 
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext, subscriptionHeaderDTOList);
                            subscriptionHeaderListBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
         
    }
}
