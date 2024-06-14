using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public interface IRedemptionCurrencyRuleProvider
    {
        List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList();
        double GetValueInTickets(int currencyId);
    }
}
