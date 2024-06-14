/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Loyalty Redemption rule model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-July-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TransactionUI
{
    public class LoyaltyRedemptionRuleModel
    {
        private int ruleId;
        private decimal loyaltyPoints;
        private decimal redemptionValue;
        private string rule;

        public LoyaltyRedemptionRuleModel(string amountFormat, int ruleId, decimal loyaltyPoints, decimal redemptionValue)
        {
            this.RuleId = ruleId;
            this.LoyaltyPoints = loyaltyPoints;
            this.RedemptionValue = redemptionValue;
            this.Rule = this.RedemptionValue.ToString(amountFormat) + " for " + this.LoyaltyPoints.ToString(amountFormat);
        }

        public string Rule
        {
            get
            {
                return rule;
            }
            set
            {
                rule = value;
            }
        }
        public int RuleId
        {
            get
            {
                return ruleId;
            }
            set
            {
                ruleId = value;
            }
        }

        public decimal LoyaltyPoints
        {
            get
            {
                return loyaltyPoints;
            }
            set
            {
                loyaltyPoints = value;
            }
        }
        public decimal RedemptionValue
        {
            get
            {
                return redemptionValue;
            }
            set
            {
                redemptionValue = value;
            }
        }
    }
}
