/******************************************************************************************************
 * Project Name - POS
 * Description  - Membership Details
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ****************************************************************************************************** 
*2.80        20-Aug-2019      Laster Menezes       Membership Details View Enhancement 
*2.100       09-Oct-2020      Laster Menezes       Modified GetRedemptionDiscountForMembership method to get RedemptionDiscount from the membershipDTO
*2.130       19-Oct-2021      Laster Menezes       Fixed Round up issues while displaying Loyalty points 
********************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Game;
using System.Linq;
using Semnox.Parafait.Customer.Accounts;

namespace Parafait_POS
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmMembershipDetails : Form
    {

        Utilities Utilities = POSStatic.Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CustomerBL customerBL;       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerDTO"></param>
        public frmMembershipDetails(CustomerBL customerBl)
        {
            log.LogMethodEntry(customerBL);
            InitializeComponent();
            Utilities.setLanguage(this);
            this.customerBL = customerBl;
            log.LogMethodExit();
        }

        private void frmMembershipDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.customerBL != null && this.customerBL.CustomerDTO != null)
                {                    
                    string MembershipPointsDetails = string.Empty;

                    string membershipName = GetMembershipName();
                    txtMembershipName.Text = membershipName;

                    DateTime? MembershipValidity = GetMembershipExpiryDate();
                    txtMembershipValidity.Text = (MembershipValidity == null ? string.Empty : Convert.ToDateTime(MembershipValidity).ToString(Utilities.ParafaitEnv.DATE_FORMAT));

                    txtMembershipCard.Text = GetMembershipCardNumber();

                    double earnedActiveLoyaltyPoints = GetEarnedActiveLoyaltyPoints();
                    txtMembershipTotalPoints.Text = earnedActiveLoyaltyPoints.ToString();

                    double expiringPointsForMembership = GetExpiringPointsForMembership(3);
                    if(expiringPointsForMembership > 0)
                    {
                        if (MembershipValidity != null && MembershipValidity > Utilities.getServerTime().AddMonths(3))
                        {
                            MembershipPointsDetails = "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2231, expiringPointsForMembership, Utilities.getServerTime().AddMonths(3).ToString(Utilities.ParafaitEnv.DATE_FORMAT)) + Environment.NewLine + Environment.NewLine;
                        }
                        else
                        {
                            MembershipPointsDetails = "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2231, expiringPointsForMembership, Convert.ToDateTime(MembershipValidity).ToString(Utilities.ParafaitEnv.DATE_FORMAT)) + Environment.NewLine + Environment.NewLine;
                        }
                    }

                    if(!IsStandAloneMembership())
                    {
                        double requiredPointsToRetainMembership = GetMembershipRetentionPointsNeeded();
                        if(requiredPointsToRetainMembership != 0)
                        {
                            MembershipPointsDetails += "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2232, requiredPointsToRetainMembership, Convert.ToDateTime(MembershipValidity).ToString(Utilities.ParafaitEnv.DATE_FORMAT), membershipName) + Environment.NewLine + Environment.NewLine;
                        }
                        else
                        {
                            MembershipPointsDetails += "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2350, membershipName) + Environment.NewLine + Environment.NewLine;
                        }                       
                    }
                    
                    string nextLevelMembershipName = GetNextLevelmembershipName();
                    if(!string.IsNullOrEmpty(nextLevelMembershipName))
                    {
                        double membershipPointsRequiredForNextLevel = GetMembershipPointsRequiredForNextLevel();
                        if (membershipPointsRequiredForNextLevel != 0)
                        {
                            MembershipPointsDetails += "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2233, membershipPointsRequiredForNextLevel, nextLevelMembershipName) + Environment.NewLine;
                        }
                        else
                        {
                            MembershipPointsDetails += "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2645, nextLevelMembershipName) + Environment.NewLine + Environment.NewLine;
                        }

                    }
                    
                    lblMembershipPointsDetails.Text = MembershipPointsDetails;

                    string gameRewards = GetGameRewards();
                    string discountRewards = GetDiscountRewards();
                    double redemptionDiscount = GetRedemptionDiscountForMembership();
                    lblMemershipRewardsDetails.Text = gameRewards;
                    lblMemershipRewardsDetails.Text += discountRewards;

                    if(redemptionDiscount != 0)
                    {
                        lblMemershipRewardsDetails.Text += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2234, redemptionDiscount) + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2243));
                log.Error("Error in Membership Details Load." + ex.Message);
            }
            log.LogMethodExit();
        }


        private string GetMembershipName()
        {
            log.LogMethodEntry();
            string membershipName = string.Empty;            
            membershipName = customerBL.GetMembershipName();
            log.LogMethodExit(membershipName);
            return membershipName;
        }

        private DateTime? GetMembershipExpiryDate()
        {
            log.LogMethodEntry();
            DateTime? membershipExpiryDate = null;          
            membershipExpiryDate = customerBL.GetCurrentMembershipEffectiveToDate();
            log.LogMethodExit(membershipExpiryDate);
            return membershipExpiryDate;
        }

        private string GetMembershipCardNumber()
        {
            log.LogMethodEntry();
            string membershipCard = string.Empty;
            membershipCard = customerBL.GetMembershipCardNumber();
            log.LogMethodExit(membershipCard);
            return membershipCard;
        }

        private int GetMembershipCardId()
        {
            log.LogMethodEntry();
            int membershipCardId = -1;
            membershipCardId = customerBL.GetMembershipCardId();
            log.LogMethodExit(membershipCardId);
            return membershipCardId;
        }


        private string GetNextLevelmembershipName()
        {
            log.LogMethodEntry();
            string nextLevelmembershipName = string.Empty;
            MembershipBL membershipBL = new MembershipBL(Utilities.ExecutionContext, this.customerBL.CustomerDTO.MembershipId);
            nextLevelmembershipName = membershipBL.GetNextLevelMembershipName();
            log.LogMethodExit(nextLevelmembershipName);
            return nextLevelmembershipName;
        }


        private double GetEarnedActiveLoyaltyPoints()
        {
            log.LogMethodEntry();
            double earnedActiveLoyaltyPoints = 0;
            earnedActiveLoyaltyPoints = customerBL.GetEarnedActiveLoyaltyPoints();
            earnedActiveLoyaltyPoints = Math.Ceiling(earnedActiveLoyaltyPoints);
            log.LogMethodExit(earnedActiveLoyaltyPoints);
            return earnedActiveLoyaltyPoints;
        }

        private double GetMembershipRetentionPointsNeeded()
        {
            log.LogMethodEntry();
            double membershipRetentionPointsNeeded = 0;
            membershipRetentionPointsNeeded = customerBL.GetRequiredPointsToRetainMembership();
            if(membershipRetentionPointsNeeded < 0)
            {
                membershipRetentionPointsNeeded = 0;
            }
            membershipRetentionPointsNeeded = Math.Ceiling(membershipRetentionPointsNeeded);
            log.LogMethodExit(membershipRetentionPointsNeeded);
            return membershipRetentionPointsNeeded;
        }

        private double GetExpiringPointsForMembership(int noOfMonths)
        {
            log.LogMethodEntry(noOfMonths);
            double expiringPointsForMembership = 0;
            expiringPointsForMembership = customerBL.GetExpiringPointsForMembership(noOfMonths);
            expiringPointsForMembership = Math.Ceiling(expiringPointsForMembership);
            log.LogMethodExit(expiringPointsForMembership);
            return expiringPointsForMembership;
        }

        private double GetMembershipPointsRequiredForNextLevel()
        {
            log.LogMethodEntry();
            double membershipPointsRequiredForNextLevel = 0;
            membershipPointsRequiredForNextLevel = customerBL.GetHowManyMorePointsForNextMembership();
            if (membershipPointsRequiredForNextLevel < 0)
            {
                membershipPointsRequiredForNextLevel = 0;
            }
            membershipPointsRequiredForNextLevel = Math.Ceiling(membershipPointsRequiredForNextLevel);
            log.LogMethodExit(membershipPointsRequiredForNextLevel);
            return membershipPointsRequiredForNextLevel;
        }

        private bool IsStandAloneMembership()
        {
            log.LogMethodEntry();
            bool isStandAloneMembership = false;
            MembershipBL membershipBL = new MembershipBL(Utilities.ExecutionContext, this.customerBL.CustomerDTO.MembershipId);
            isStandAloneMembership = membershipBL.IsStandAloneMembership();
            log.LogMethodExit(isStandAloneMembership);
            return isStandAloneMembership;           
        }

        private string GetGameRewards()
        {
            log.LogMethodEntry();
            List<AccountGameDTO> cardGamesDTOList = new List<AccountGameDTO>();
            string gameRewards = string.Empty;
            cardGamesDTOList = customerBL.GetGameRewards();

            foreach (AccountGameDTO cardGamesDTO in cardGamesDTOList)
            {
                if (cardGamesDTO.MembershipRewardsId != -1)
                {
                    if (cardGamesDTO.ExpiryDate == null || cardGamesDTO.ExpiryDate > Utilities.getServerTime())
                    {
                        if (cardGamesDTO.Frequency != "N" || cardGamesDTO.BalanceGames != 0)
                        {
                            string cardGamesFrequency = string.Empty;
                            if (cardGamesDTO.Frequency == "D")
                                cardGamesFrequency = "Daily";
                            else if (cardGamesDTO.Frequency == "W")
                                cardGamesFrequency = "Weekly";
                            else if (cardGamesDTO.Frequency == "M")
                                cardGamesFrequency = "Monthly";
                            else if (cardGamesDTO.Frequency == "Y")
                                cardGamesFrequency = "Yearly";
                            else if (cardGamesDTO.Frequency == "B")
                                cardGamesFrequency = "Birthday";
                            else if (cardGamesDTO.Frequency == "A")
                                cardGamesFrequency = "Anniversary";

                            MembershipRewardsBL membershipRewardsBL = new MembershipRewardsBL(Utilities.ExecutionContext, cardGamesDTO.MembershipRewardsId);
                            bool isRecurringReward = membershipRewardsBL.IsRecurringReward();
                            string gameName = string.Empty;
                            if (cardGamesDTO.GameId != -1)
                            {
                                Game game = new Game(cardGamesDTO.GameId, Utilities.ExecutionContext);
                                gameName = game.GetGameDTO.GameName;
                            }
                            else if (cardGamesDTO.GameProfileId != -1)
                            {
                                GameProfile gameProfile = new GameProfile(cardGamesDTO.GameProfileId, Utilities.ExecutionContext);
                                gameName = gameProfile.GetGameProfileDTO.ProfileName;
                            }

                            if (cardGamesDTO.GameProfileId == -1 && cardGamesDTO.GameId == -1)
                            {
                                if (cardGamesDTO.Frequency == "N")
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2286, cardGamesDTO.BalanceGames);
                                }
                                else
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2349, cardGamesDTO.BalanceGames, MessageContainerList.GetMessage(Utilities.ExecutionContext,cardGamesFrequency));
                                }
                            }
                            else if (cardGamesDTO.GameId != -1 || cardGamesDTO.GameProfileId != -1)
                            {
                                if (cardGamesDTO.Frequency == "N")
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2242, cardGamesDTO.BalanceGames, gameName);
                                }
                                else
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2338, cardGamesDTO.BalanceGames, MessageContainerList.GetMessage(Utilities.ExecutionContext,cardGamesFrequency), gameName);
                                }
                            }

                            string membershipCard = GetMembershipCardNumber();
                            List<AccountDTO> cardCoreDTO = new List<AccountDTO>();
                            cardCoreDTO = customerBL.CustomerDTO.AccountDTOList.Where(t => t.AccountId == cardGamesDTO.AccountId).ToList();
                            if (cardCoreDTO != null)
                            {
                                if (!string.IsNullOrEmpty(cardCoreDTO[0].TagNumber) && cardCoreDTO[0].TagNumber != membershipCard)
                                {
                                    if (customerBL.CustomerDTO != null)
                                    {
                                        string cardNumber = cardCoreDTO[0].TagNumber;
                                        gameRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2287, cardNumber);
                                    }
                                }
                            }

                            if (cardGamesDTO.MembershipId != -1)
                            {
                                MembershipBL membershipBL = new MembershipBL(Utilities.ExecutionContext, cardGamesDTO.MembershipId);
                                string membershipName = membershipBL.getMembershipDTO.MembershipName;
                                if (!string.IsNullOrEmpty(membershipName))
                                {
                                    gameRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2288, membershipName);
                                }
                            }

                            if (cardGamesDTO.ExpiryDate != null && cardGamesDTO.ExpiryDate != DateTime.MinValue)
                            {
                                gameRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2237, Convert.ToDateTime(cardGamesDTO.ExpiryDate).ToString(Utilities.ParafaitEnv.DATE_FORMAT));
                            }

                            if (isRecurringReward)
                            {
                                string rewardFrequencyUnit = string.Empty;
                                if (membershipRewardsBL.getMembershipRewardsDTO.UnitOfRewardFrequency == "D")
                                    rewardFrequencyUnit = "Days";
                                if (membershipRewardsBL.getMembershipRewardsDTO.UnitOfRewardFrequency == "M")
                                    rewardFrequencyUnit = "Months";
                                if (membershipRewardsBL.getMembershipRewardsDTO.UnitOfRewardFrequency == "Y")
                                    rewardFrequencyUnit = "Years";
                                if(cardGamesDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                                {
                                    gameRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2238, membershipRewardsBL.getMembershipRewardsDTO.RewardFrequency, MessageContainerList.GetMessage(Utilities.ExecutionContext, rewardFrequencyUnit));
                                }                              
                            }

                            if (cardGamesDTO.ExpireWithMembership == true && cardGamesDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                            {
                                gameRewards += ", " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2239);
                            }


                        }
                    }
                }
            }
            log.LogMethodExit(gameRewards);
            return gameRewards;
        }


        private string GetDiscountRewards()
        {
            log.LogMethodEntry();
            List<AccountDiscountDTO> cardDiscountsDTOList = new List<AccountDiscountDTO>();
            string discountRewards = string.Empty;
            cardDiscountsDTOList = customerBL.GetDiscountRewards();

            foreach (AccountDiscountDTO cardDiscountsDTO in cardDiscountsDTOList)
            {
                if (cardDiscountsDTO.MembershipRewardsId != -1)
                {
                    if (cardDiscountsDTO.IsActive)
                    {
                        if (cardDiscountsDTO.ExpiryDate == null || cardDiscountsDTO.ExpiryDate > Utilities.getServerTime())
                        {
                            using (UnitOfWork unitOfWork = new UnitOfWork())
                            {
                                DiscountsBL discountsBL = new DiscountsBL(Utilities.ExecutionContext, unitOfWork, cardDiscountsDTO.DiscountId, false);
                                string discountName = discountsBL.DiscountsDTO.DiscountName;


                                double discountValue = 0;
                                MembershipRewardsBL membershipRewardsBL = new MembershipRewardsBL(Utilities.ExecutionContext, cardDiscountsDTO.MembershipRewardsId);
                                bool isRecurringReward = membershipRewardsBL.IsRecurringReward();

                                string membershipName = string.Empty;
                                if (cardDiscountsDTO.MembershipId != -1)
                                {
                                    MembershipBL membershipBL = new MembershipBL(Utilities.ExecutionContext, cardDiscountsDTO.MembershipId);
                                    membershipName = membershipBL.getMembershipDTO.MembershipName;
                                }

                                List<AccountDTO> cardCoreDTO = new List<AccountDTO>();
                                cardCoreDTO = customerBL.CustomerDTO.AccountDTOList.Where(t => t.AccountId == cardDiscountsDTO.AccountId).ToList();
                                string cardNumber = cardCoreDTO[0].TagNumber;
                                string membershipCard = GetMembershipCardNumber();

                                discountRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2644, discountName);
                                if (!string.IsNullOrEmpty(cardNumber) && cardNumber != membershipCard)
                                {
                                    discountRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2287, cardNumber);
                                }
                                if (!string.IsNullOrEmpty(membershipName))
                                {
                                    discountRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2288, membershipName);
                                }

                                if (discountsBL.DiscountsDTO.DiscountAmount != null)
                                {
                                    discountValue = Convert.ToDouble(discountsBL.DiscountsDTO.DiscountAmount);
                                    discountRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2236, discountName, discountValue.ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                }
                                else if (discountsBL.DiscountsDTO.DiscountPercentage != null)
                                {
                                    discountValue = Convert.ToDouble(discountsBL.DiscountsDTO.DiscountPercentage);
                                    discountRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2235, discountName, discountValue);
                                }

                                if (cardDiscountsDTO.ExpiryDate != null && cardDiscountsDTO.ExpiryDate != DateTime.MinValue)
                                {
                                    discountRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2237, Convert.ToDateTime(cardDiscountsDTO.ExpiryDate).ToString(Utilities.ParafaitEnv.DATE_FORMAT));
                                }

                                if (isRecurringReward)
                                {
                                    string rewardFrequencyUnit = string.Empty;
                                    if (membershipRewardsBL.getMembershipRewardsDTO.UnitOfRewardFrequency == "D")
                                        rewardFrequencyUnit = "Days";
                                    if (membershipRewardsBL.getMembershipRewardsDTO.UnitOfRewardFrequency == "M")
                                        rewardFrequencyUnit = "Months";
                                    if (membershipRewardsBL.getMembershipRewardsDTO.UnitOfRewardFrequency == "Y")
                                        rewardFrequencyUnit = "Years";
                                    if (cardDiscountsDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                                    {
                                        discountRewards += " " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2238, membershipRewardsBL.getMembershipRewardsDTO.RewardFrequency, MessageContainerList.GetMessage(Utilities.ExecutionContext, rewardFrequencyUnit));
                                    }
                                }

                                if (cardDiscountsDTO.ExpireWithMembership == "Y" && cardDiscountsDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                                {
                                    discountRewards += ", " + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2239);
                                }
                            }
                        }
                    }
                }
            }

            log.LogMethodExit(discountRewards);
            return discountRewards;
        }

        private double GetRedemptionDiscountForMembership()
        {
            log.LogMethodEntry();
            double redemptionDiscountForMembership = 0;
            if(this.customerBL.CustomerDTO.MembershipId > -1)
            {
                MembershipBL membershipBL = new MembershipBL(Utilities.ExecutionContext, this.customerBL.CustomerDTO.MembershipId);
                redemptionDiscountForMembership = membershipBL.getMembershipDTO.RedemptionDiscount;
            }            
            log.LogMethodExit(redemptionDiscountForMembership);
            return redemptionDiscountForMembership;
        }

        private void btnCardCreditPlusDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int primaryCardId = GetMembershipCardId();
            if (primaryCardId != -1)
            {
                CreditPlusDetails cpd = new CreditPlusDetails(primaryCardId);
                cpd.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        
    }
}
