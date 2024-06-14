/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Loyalty redeem dto
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-July-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class LoyaltyRedeemDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int cardId;
        int ruleId;
        decimal loyaltyRedeemPoints;
        string remarks;
        int managerId;

        public LoyaltyRedeemDTO()
        {
            log.LogMethodEntry();
            cardId = -1;
            ruleId = -1;
            loyaltyRedeemPoints = 0;
            remarks = string.Empty;
            managerId = -1;
            log.LogMethodExit();
        }

        public LoyaltyRedeemDTO(int cardId, int ruleId, decimal loyaltyRedeemPoints, string remarks, int managerId)
        {
            log.LogMethodEntry(cardId, ruleId, loyaltyRedeemPoints, remarks, managerId);
            this.cardId = cardId;
            this.ruleId = ruleId;
            this.loyaltyRedeemPoints = loyaltyRedeemPoints;
            this.remarks = remarks;
            this.managerId = managerId;
            log.LogMethodExit();
        }

        public int CardId
        {
            get
            {
                return cardId;
            }
            set
            {
                cardId = value;
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

        public decimal LoyaltyRedeemPoints
        {
            get
            {
                return loyaltyRedeemPoints;
            }
            set
            {
                loyaltyRedeemPoints = value;
            }
        }

        public int ManagerId
        {
            get
            {
                return managerId;
            }
            set
            {
                managerId = value;
            }
        }

        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
            }
        }
    }
}
