/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - Data object of RedemptionCurrencyRuleContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *0.0         05-Jul-2020   Vikas Dwivedi       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleDetailContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int redemptionCurrencyRuleDetailId;
        private int redemptionCurrencyRuleId;
        private int currencyId;
        private int? quantity;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionCurrencyRuleDetailContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public RedemptionCurrencyRuleDetailContainerDTO(int redemptionCurrencyRuleDetailId, int redemptionCurrencyRuleId, int currencyId, int? quantity)
            : this()
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailId, redemptionCurrencyRuleId, currencyId, quantity);
            this.redemptionCurrencyRuleDetailId = redemptionCurrencyRuleDetailId;
            this.redemptionCurrencyRuleId = redemptionCurrencyRuleId;
            this.currencyId = currencyId;
            this.quantity = quantity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the redemptionCurrencyRuleDetailId field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleDetailId")]
        public int RedemptionCurrencyRuleDetailId { get { return redemptionCurrencyRuleDetailId; } set { redemptionCurrencyRuleDetailId = value; } }

        /// <summary>
        /// Get/Set method of the redemptionCurrencyRuleId field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleId")]
        public int RedemptionCurrencyRuleId { get { return redemptionCurrencyRuleId; } set { redemptionCurrencyRuleId = value; } }

        /// <summary>
        /// Get/Set method of the currencyId field
        /// </summary>
        [DisplayName("Currency")]
        public int CurrencyId { get { return currencyId; } set { currencyId = value; } }

        /// <summary>
        /// Get/Set method of the quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int? Quantity { get { return quantity; } set { quantity = value; } }

    }
}
