/* Project Name - MembershipRewardsBL
* Description  - Business call object of the MembershipRewards
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.60.2      20-May-2019    Jagan Mohana Rao        Added new Validate() for validation
*2.70.2        19-Jul-2019    Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
*2.70.2        21-Aug-2019    Laster Menezes          Added new method IsRecurringReward()
********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient; 
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Membership
{
    public class MembershipRewardsBL
    {
        private MembershipRewardsDTO membershipRewardsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        

        /// <summary>
        ///Constructor will fetch the MembershipRewards DTO based on the membershipRewards id passed 
        /// </summary>
        /// <param name="membershipRewardsId">MembershipRewards id</param>
        public MembershipRewardsBL(ExecutionContext executionContext, int membershipRewardsId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(membershipRewardsId, executionContext);
            this.executionContext = executionContext;
            MembershipRewardsDataHandler membershipRewardsDataHandler = new MembershipRewardsDataHandler(sqlTransaction);
            membershipRewardsDTO = membershipRewardsDataHandler.GetMembershipRewards(membershipRewardsId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates membershipRewards object using the MembershipRewardsDTO
        /// </summary>
        /// <param name="membershipRewards">MembershipRewardsDTO object</param>
        public MembershipRewardsBL(ExecutionContext executionContext, MembershipRewardsDTO membershipRewards)
        {
            log.LogMethodEntry(membershipRewards, executionContext);
            this.membershipRewardsDTO = membershipRewards;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the membershipRewards record
        /// Checks if the MembershipRewardsId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MembershipRewardsDataHandler membershipRewardsDataHandler = new MembershipRewardsDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (membershipRewardsDTO.MembershipRewardsId < 0)
            {
                membershipRewardsDTO = membershipRewardsDataHandler.InsertMembershipRewards(membershipRewardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                membershipRewardsDTO.AcceptChanges();
            }
            else
            {
                if (membershipRewardsDTO.IsChanged)
                {
                    membershipRewardsDTO = membershipRewardsDataHandler.UpdateMembershipRewards(membershipRewardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    membershipRewardsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (membershipRewardsDTO != null)
            {
                if (string.IsNullOrEmpty(membershipRewardsDTO.RewardName))
                {
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Membership Name"), MessageContainerList.GetMessage(executionContext, 1856)); /// Please enter valid value for Membership Reward Name
                    validationErrorList.Add(validationError);
                }
                if (membershipRewardsDTO.RewardProductID == -1 && string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                {
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Reward Attribute"), MessageContainerList.GetMessage(executionContext, 1484)); /// Please select Reward Product or Reward Attribute for the reward entry
                    validationErrorList.Add(validationError);
                }
                if (membershipRewardsDTO.RewardProductID != -1 && !string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                {
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Reward Attribute"), MessageContainerList.GetMessage(executionContext, 1485)); /// Please select Reward Product or Reward Attribute for the reward entry. Both are not allowed on same reward entry
                    validationErrorList.Add(validationError);
                }
                if (!string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                {
                    if (membershipRewardsDTO.RewardAttributePercent == 0)
                    {
                        ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Reward Attribute Percent"), MessageContainerList.GetMessage(executionContext, 1486)); /// Please set Reward Attribute Percentage value
                        validationErrorList.Add(validationError);
                    }
                    if (string.IsNullOrEmpty(membershipRewardsDTO.RewardFunction))
                    {
                        ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Reward Function"), MessageContainerList.GetMessage(executionContext, 1487)); /// Please set Reward Function value for the attribute
                        validationErrorList.Add(validationError);
                    }
                    else
                    {
                        if (membershipRewardsDTO.RewardFunction == "TOPRD" && (membershipRewardsDTO.RewardFunctionPeriod == 0 || string.IsNullOrEmpty(membershipRewardsDTO.UnitOfRewardFunctionPeriod)))
                        {
                            ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Reward Function Period"), MessageContainerList.GetMessage(executionContext, 1488)); /// Please set Reward function period details
                            validationErrorList.Add(validationError);
                        }
                    }
                }
                if (membershipRewardsDTO.RewardFrequency != 0 && string.IsNullOrEmpty(membershipRewardsDTO.UnitOfRewardFrequency))
                {
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Membership"), MessageContainerList.GetMessage(executionContext, "Unit Of Reward Function Period"), MessageContainerList.GetMessage(executionContext, 1489)); /// Please set Reward frequence period details
                    validationErrorList.Add(validationError);
                }
                if (membershipRewardsDTO.RewardProductID != -1 && string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                {
                    if (membershipRewardsDTO.RewardFrequency == 0)
                        membershipRewardsDTO.UnitOfRewardFrequency = null;
                    membershipRewardsDTO.RewardAttributePercent = 0;
                    membershipRewardsDTO.RewardFunction = null;
                    membershipRewardsDTO.RewardFunctionPeriod = 0;
                    membershipRewardsDTO.UnitOfRewardFunctionPeriod = null;
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MembershipRewardsDTO getMembershipRewardsDTO { get { return membershipRewardsDTO; } }

        /// <summary>
        /// Is Recurring Reward
        /// </summary>
        /// <returns> isRecurringReward</returns>
        public bool IsRecurringReward()
        {
            log.LogMethodEntry();
            bool isRecurringReward = false;
            if (membershipRewardsDTO != null)
            {
                if(membershipRewardsDTO.RewardFrequency != 0 && !string.IsNullOrEmpty(membershipRewardsDTO.UnitOfRewardFrequency))
                {
                    isRecurringReward = true;
                }
            }
            log.LogMethodExit();
            return isRecurringReward;
        }

    }

    /// <summary>
    /// Manages the list of membershipRewards
    /// </summary>
    public class MembershipRewardsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the membershipRewards list
        /// </summary>
        public List<MembershipRewardsDTO> GetAllMembershipRewards(List<KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>> searchParameters ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MembershipRewardsDataHandler membershipRewardsDataHandler = new MembershipRewardsDataHandler(sqlTransaction);
            List<MembershipRewardsDTO> membershipRewardsDTOList =  membershipRewardsDataHandler.GetAllMembershipRewardsList(searchParameters);
            log.LogMethodExit(membershipRewardsDTOList);
            return membershipRewardsDTOList;
        }
    }
}
