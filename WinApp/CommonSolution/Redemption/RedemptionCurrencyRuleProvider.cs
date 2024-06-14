using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleProvider : IRedemptionCurrencyRuleProvider
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public RedemptionCurrencyRuleProvider(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList()
        {
            log.LogMethodEntry();
            var result = RedemptionCurrencyRuleContainerList.GetRedemptionCurrencyRuleContainerDTOList(executionContext.SiteId);
            log.LogMethodExit(result);
            return result;
        }

        public double GetValueInTickets(int currencyId)
        {
            log.LogMethodEntry();
            var result = RedemptionCurrencyContainerList.GetValueInTickets(executionContext.SiteId, currencyId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
