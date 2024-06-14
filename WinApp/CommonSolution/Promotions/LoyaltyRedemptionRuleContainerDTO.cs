/********************************************************************************************
 * Project Name - Promotions
 * Description  -  LoyaltyRedemptionRuleContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyRedemptionRuleContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int redemptionRuleId;
        private int loyaltyAttributeId;
        private decimal loyaltyPoints;
        private decimal redemptionValue;
        private DateTime expiryDate;
        private bool activeFlag;
        private decimal minimumPoints;
        private decimal maximiumPoints;
        private char multiplesOnly;
        private decimal virtualLoyaltyPoints;
        public LoyaltyRedemptionRuleContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        /// 
        public LoyaltyRedemptionRuleContainerDTO(int redemptionRuleIdPassed, int loyaltyAttributeIdPassed, decimal loyaltyPointsPassed, decimal redemptionValuePassed, DateTime expiryDatePassed, bool activeFlagPassed,
                                                  decimal minimumPointsPassed, decimal maximiumPointsPassed, char multiplesOnlyPassed, decimal virtualLoyaltyPointsPassed) : this()
        {
            log.LogMethodEntry(redemptionRuleIdPassed, loyaltyAttributeIdPassed, loyaltyPointsPassed, redemptionValuePassed, expiryDatePassed, activeFlagPassed,
                                minimumPointsPassed, maximiumPointsPassed, multiplesOnlyPassed, virtualLoyaltyPointsPassed);
            this.redemptionRuleId= redemptionRuleIdPassed;
            this.loyaltyAttributeId= loyaltyAttributeIdPassed;
            this.loyaltyPoints= loyaltyPointsPassed;
            this.redemptionValue= redemptionValuePassed;
            this.expiryDate= expiryDatePassed;
            this.activeFlag = activeFlagPassed;
            this.minimumPoints= minimumPointsPassed;
            this.maximiumPoints= maximiumPointsPassed;
            this.multiplesOnly = multiplesOnlyPassed;
            this.virtualLoyaltyPoints = virtualLoyaltyPointsPassed;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the RedemptionRuleId field
        /// </summary>
        [DisplayName("RedemptionRuleId")]
        [ReadOnly(true)]
        public int RedemptionRuleId { get { return redemptionRuleId; } set { redemptionRuleId = value; } }
        /// <summary>
        /// Get/Set method of the LoyaltyAttributeId field
        /// </summary>
        [DisplayName("LoyaltyAttributeId")]
        [ReadOnly(true)]
        public int LoyaltyAttributeId { get { return loyaltyAttributeId; } set { loyaltyAttributeId = value; } }
        /// <summary>
        /// Get/Set method of the LoyaltyPoints field
        /// </summary>
        [DisplayName("LoyaltyPoints")]
        [ReadOnly(true)]
        public decimal LoyaltyPoints { get { return loyaltyPoints; } set { loyaltyPoints = value; } }
        /// <summary>
        /// Get/Set method of the RedemptionValue field
        /// </summary>
        [DisplayName("RedemptionValue")]
        [ReadOnly(true)]
        public decimal RedemptionValue { get { return redemptionValue; } set { redemptionValue = value; } }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("ExpiryDate")]
        [ReadOnly(true)]
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("ActiveFlag")]
        [ReadOnly(true)]
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; } }
        /// <summary>
        /// Get/Set method of the MinimumPoints field
        /// </summary>
        [DisplayName("MinimumPoints")]
        [ReadOnly(true)]
        public decimal MinimumPoints { get { return minimumPoints; } set { minimumPoints = value; } }
        /// <summary>
        /// Get/Set method of the MaximiumPoints field
        /// </summary>
        [DisplayName("MaximiumPoints")]
        [ReadOnly(true)]
        public decimal MaximiumPoints { get { return maximiumPoints; } set { maximiumPoints = value; } }
        /// <summary>
        /// Get/Set method of the MultiplesOnly field
        /// </summary>
        [DisplayName("MultiplesOnly")]
        [ReadOnly(true)]
        public char MultiplesOnly { get { return multiplesOnly; } set { multiplesOnly = value; } }
        /// <summary>
        /// Get/Set method of the VirtualLoyaltyPoints field
        /// </summary>
        [DisplayName("VirtualLoyaltyPoints")]
        [ReadOnly(true)]
        public decimal VirtualLoyaltyPoints { get { return virtualLoyaltyPoints; } set { virtualLoyaltyPoints = value; } }
    }
}

