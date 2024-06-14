/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - Data object of RedemptionCurrencyRuleContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *0.0         05-Jul-2020   Vikas Dwivedi       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int redemptionCurrencyRuleId;
        private string redemptionCurrencyRuleName;
        private string description;
        private decimal percentage;
        private decimal amount;
        private int priority;
        private bool cumulative;
        private List<RedemptionCurrencyRuleDetailContainerDTO> redemptionCurrencyRuleDetailContainerDTOList = new List<RedemptionCurrencyRuleDetailContainerDTO>();
        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionCurrencyRuleContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public RedemptionCurrencyRuleContainerDTO(int redemptionCurrencyRuleId, string redemptionCurrencyRuleName, string description, decimal percentage, decimal amount,
                                         int priority, bool cumulative)
            : this()
        {
            log.LogMethodEntry(redemptionCurrencyRuleId, redemptionCurrencyRuleName, description, percentage, amount, priority, cumulative);
            this.redemptionCurrencyRuleId = redemptionCurrencyRuleId;
            this.redemptionCurrencyRuleName = redemptionCurrencyRuleName;
            this.description = description;
            this.percentage = percentage;
            this.amount = amount;
            this.priority = priority;
            this.cumulative = cumulative;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionCurrencyRuleId field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleId")]
        public int RedemptionCurrencyRuleId { get { return redemptionCurrencyRuleId; } set { redemptionCurrencyRuleId = value; } }

        /// <summary>
        /// Get/Set method of the RedemptionCurrencyRuleName field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleName")]
        public string RedemptionCurrencyRuleName { get { return redemptionCurrencyRuleName; } set { redemptionCurrencyRuleName = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the Percentage field
        /// </summary>
        [DisplayName("Percentage")]
        public decimal Percentage { get { return percentage; } set { percentage = value; } }

        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>
        [DisplayName("Amount")]
        public decimal Amount { get { return amount; } set { amount = value; } }

        /// Get/Set method of the Priority field
        /// </summary>
        [DisplayName("Priority")]
        public int Priority { get { return priority; } set { priority = value; } }

        /// <summary>
        /// Get/Set method of the Cumulative field
        /// </summary>
        [DisplayName("Cumulative")]
        public bool Cumulative { get { return cumulative; } set { cumulative = value; } }

        /// <summary>
        /// Get/Set method of the redemptionCurrencyRuleDetailContainerDTOList field
        /// </summary>
        public List<RedemptionCurrencyRuleDetailContainerDTO> RedemptionCurrencyRuleDetailContainerDTOList { get { return redemptionCurrencyRuleDetailContainerDTOList; } set { redemptionCurrencyRuleDetailContainerDTOList = value; } }

    }
}
