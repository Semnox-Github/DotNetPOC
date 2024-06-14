/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - model for redemption currency rule provided
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Amitha Joy            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.Generic;

using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionCurrencyRuleViewProvider : IRedemptionCurrencyRuleProvider
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        #endregion
        #region Constructors
        public RedemptionCurrencyRuleViewProvider(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        #endregion
        #region Methods
        public List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList()
        {
            log.LogMethodEntry();
            List<RedemptionCurrencyRuleContainerDTO> result = RedemptionCurrencyRuleViewContainerList.GetRedemptionCurrencyRuleContainerDTOList(executionContext);
            log.LogMethodExit(result);
            return result;
        }
        public double GetValueInTickets(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            double result = RedemptionCurrencyViewContainerList.GetValueInTickets(executionContext.SiteId, currencyId);
            log.LogMethodExit(result);
            return result;
        }
        #endregion
    }
}
