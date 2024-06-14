using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    class AlohaLoyaltyCreateAssignmentResponseDTO
    {
        public class CheckItemDiscount
        {
            int itemDiscount;
            int checkItemId;
            public int discount { get { return itemDiscount; } set { itemDiscount = value; } }
            public int checkItemID { get { return checkItemId; } set { checkItemId = value; } }
            public CheckItemDiscount()
            { }
        }
        public class AlohaLoyaltyReward
        {
            string checkTxt;
            int rewardTyp;
            int discountId;
            int iter;
            int rewardProgId;
            int tierId;
            string rewardTierName;
            int hstRewardProgramId;
            bool prtOnVoucher;
            bool discountUpToDiscountAmnt;
            string voucherTxt1;
            string discountTxt;
            List<CheckItemDiscount> checkItmDiscounts;
            bool autoRejectRewardFlag;
            public AlohaLoyaltyReward()
            {
                checkItmDiscounts = new List<CheckItemDiscount>();
            }
            public string checkText { get { return checkTxt; } set { checkTxt = value; } }
            public int rewardType { get { return rewardTyp; } set { rewardTyp = value; } }
            public int discountID { get { return discountId; } set { discountId = value; } }
            public int iteration { get { return iter; } set { iter = value; } }
            public int rewardProgID { get { return rewardProgId; } set { rewardProgId = value; } }
            public int tierID { get { return tierId; } set { tierId = value; } }
            public string tierName { get { return rewardTierName; } set { rewardTierName = value; } }
            public int hstRewardProgramID { get { return hstRewardProgramId; } set { hstRewardProgramId = value; } }
            public bool printOnVoucher { get { return prtOnVoucher; } set { prtOnVoucher = value; } }
            public bool discountUpToDiscountAmount { get { return discountUpToDiscountAmnt; } set { discountUpToDiscountAmnt = value; } }
            public string voucherText1 { get { return voucherTxt1; } set { voucherTxt1 = value; } }
            public string discountText { get { return discountTxt; } set { discountTxt = value; } }
            public List<CheckItemDiscount> checkItemDiscounts { get { return checkItmDiscounts; } set { checkItmDiscounts = value; } }
            public bool autoRejectReward { get { return autoRejectRewardFlag; } set { autoRejectRewardFlag = value; } }
        }
        string tailCheckTxt;
        int maxRewardsToAccept;
        string voucherTxt;
        List<AlohaLoyaltyReward> rewardsReturned;
        string rejectedHeadChkTxt;
        string rejectedTailChkTxt;
        bool printRewardChkTxtFlag;
        bool printAsVoucherFlag;
        string headChkText;
        string memberTxt;
        string crdNumber;
        public AlohaLoyaltyCreateAssignmentResponseDTO()
        {
            rewardsReturned = new List<AlohaLoyaltyReward>();
        }
        public string tailCheckText { get { return tailCheckTxt; } set { tailCheckTxt = value; } }
        public int maximumRewardsToAccept { get { return maxRewardsToAccept; } set { maxRewardsToAccept = value; } }
        public string voucherText { get { return voucherTxt; } set { voucherTxt = value; } }
        public List<AlohaLoyaltyReward> rewards { get { return rewardsReturned; } set { rewardsReturned = value; } }
        public string rejectedHeadCheckText { get { return rejectedHeadChkTxt; } set { rejectedHeadChkTxt = value; } }
        public string rejectedTailCheckText { get { return rejectedTailChkTxt; } set { rejectedTailChkTxt = value; } }
        public bool printRewardCheckText { get { return printRewardChkTxtFlag; } set { printRewardChkTxtFlag = value; } }
        public bool printAsVoucher { get { return printAsVoucherFlag; } set { printAsVoucherFlag = value; } }
        public string headCheckText { get { return headChkText; } set { headChkText = value; } }
        public string memberText { get { return memberTxt; } set { memberTxt = value; } }
        public string cardNumber { get { return crdNumber; } set { crdNumber = value; } }
    }

    class AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO
    {
        int statusId;
        int statusCode;
        string statusType;
        string statusMessage;
        string statusMoreInfo;
        string statusReferenceId;
        public AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO()
        {

        }
        public AlohaLoyaltyCreateAssignmentUnsuccessfulResponseDTO(int status, int code, string type, string message, string moreInfo, string referenceId) : this()
        {
            this.statusId = status;
            this.statusCode = code;
            this.statusType = type;
            this.statusMessage = message;
            this.statusMoreInfo = moreInfo;
            this.statusReferenceId = referenceId;
        }
        public int status { get { return statusId; } set { statusId = value; } }
        public int code { get { return statusCode; } set { statusCode = value; } }
        public string type { get { return statusType; } set { statusType = value; } }
        public string message { get { return statusMessage; } set { statusMessage = value; } }
        public string moreinfo { get { return statusMoreInfo; } set { statusMoreInfo = value; } }
        public string referenceId { get { return statusReferenceId; } set { statusReferenceId = value; } }
    }
}
