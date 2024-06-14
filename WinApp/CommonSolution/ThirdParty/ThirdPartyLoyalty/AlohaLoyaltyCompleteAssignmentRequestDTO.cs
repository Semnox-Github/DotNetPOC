using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    class AlohaLoyaltyCompleteAssignmentRequestDTO
    {
        public class AlohaLoyaltyRewardsAccepted
        {
            int hstRewardProgramId;
            int iter;
            int rewardProgramId;
            int tierId;
            int discountAmt;
            public AlohaLoyaltyRewardsAccepted()
            {

            }
            public int hstRewardProgramID { get { return hstRewardProgramId; } set { hstRewardProgramId = value; } }
            public int iteration { get { return iter; } set { iter = value; } }
            public int rewardProgramID { get { return rewardProgramId; } set { rewardProgramId = value; } }
            public int tierID { get { return tierId; } set { tierId = value; } }
            public int discountAmount { get { return discountAmt; } set { discountAmt = value; } }
        }

        public class AlohaLoyaltyRewardsRejected
        {
            int hstRewardProgramId;
            int iter;
            int rewardProgramId;
            int tierId;
            string rejectReason;
            public AlohaLoyaltyRewardsRejected()
            {
                rejectReason = "Assignment from PlayPass. Redemption not allowed";
            }

            public AlohaLoyaltyRewardsRejected(int hstRewardProgramId, int iter, int rewardProgramId, int tierId) : this()
            {
                this.hstRewardProgramId = hstRewardProgramId;
                this.iter = iter;
                this.rewardProgramId = rewardProgramId;
                this.tierId = tierId;
            }
            public int hstRewardProgramID { get { return hstRewardProgramId; } set { hstRewardProgramId = value; } }
            public int iteration { get { return iter; } set { iter = value; } }
            public int rewardProgramID { get { return rewardProgramId; } set { rewardProgramId = value; } }
            public int tierID { get { return tierId; } set { tierId = value; } }
            public string rejectionReason { get { return rejectReason; } set { rejectReason = value; } }

        }
        string cardNum;
        string compId;
        int storeIdentifier;
        string dateOfBuss;
        int checkIdentifier;
        bool autoAgn = false;
        string trxId = Guid.NewGuid().ToString();
        bool discardRewards = false;
        List<AlohaLoyaltyRewardsAccepted> rewardsAccpt;
        List<AlohaLoyaltyRewardsRejected> rewardsRjct;

        public AlohaLoyaltyCompleteAssignmentRequestDTO()
        {
            rewardsAccpt = new List<AlohaLoyaltyRewardsAccepted>();
            rewardsRjct = new List<AlohaLoyaltyRewardsRejected>();
        }

        public string cardNumber { get { return cardNum; } set { cardNum = value; } }
        public string companyId { get { return compId; } set { compId = value; } }
        public int storeId { get { return storeIdentifier; } set { storeIdentifier = value; } }
        public string dateOfBusiness { get { return dateOfBuss; } set { dateOfBuss = value; } }
        public int checkID { get { return checkIdentifier; } set { checkIdentifier = value; } }
        public bool autoAssign { get { return autoAgn; } set { autoAgn = value; } }
        public string transactionId { get { return trxId; } set { trxId = value; } }
        public bool discard { get { return discardRewards; } set { discardRewards = value; } }

        public List<AlohaLoyaltyRewardsAccepted> rewardsAccepted { get { return rewardsAccpt; } set { rewardsAccpt = value; } }
        public List<AlohaLoyaltyRewardsRejected> rewardsRejected { get { return rewardsRjct; } set { rewardsRjct = value; } }
    }
}
