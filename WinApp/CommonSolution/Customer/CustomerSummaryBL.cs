/********************************************************************************************
 * Project Name - Customer
 * Description  - CustomerSummaryBL class to get the CustomerSummary Data.  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.120.0     15-Mar-2021      Prajwal S                 Created
 ********************************************************************************************/
using Semnox.Core;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class CustomerSummaryBL
    {
        CustomerBL customerBL;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private CustomerSummaryDTO customerSummaryDTO;
        private CustomerSummaryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CustomerSummaryBL(ExecutionContext executionContext, int customerId)
            : this(executionContext)
        {
            log.LogMethodEntry(customerId);
            customerSummaryDTO = new CustomerSummaryDTO();
            customerBL = new CustomerBL(executionContext, customerId, true, true);
            AccountListBL accountListBL = new AccountListBL(executionContext);
            List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerBL.CustomerDTO.Id.ToString()));
            List<AccountDTO> accountList = accountListBL.GetAccountDTOList(searchParameters, false, true, null);
            if (accountList != null && accountList.Count > 0)
            {
                AccountBuilderBL accountBuilder = new AccountBuilderBL(executionContext);
                accountBuilder.Build(accountList, true);
            }
            customerBL.CustomerDTO.AccountDTOList = accountList;
            if (customerBL.CustomerDTO.CustomerMembershipProgressionDTOList != null && customerBL.CustomerDTO.CustomerMembershipProgressionDTOList.Count == 0)
            {
                customerBL.LoadCustomerRewards();
            }
            customerSummaryDTO.MembershipId = customerBL.CustomerDTO.MembershipId;
            customerSummaryDTO.MemberShipName = customerBL.GetMembershipName();
            customerSummaryDTO.MembershipValidity = customerBL.GetCurrentMembershipEffectiveToDate();
            customerSummaryDTO.MembershipTotalPoints = customerBL.GetEarnedActiveLoyaltyPoints();
            double expiringPointsForMembership = customerBL.GetExpiringPointsForMembership(3);
            if (expiringPointsForMembership > 0)
            {
                if (customerSummaryDTO.MembershipValidity != null && customerSummaryDTO.MembershipValidity > DateTime.Now.AddMonths(3))
                {
                    customerSummaryDTO.MembershipPointDetails = "\u2022 " + MessageContainerList.GetMessage(executionContext, 2231, expiringPointsForMembership, DateTime.Now.AddMonths(3).ToString()) + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    customerSummaryDTO.MembershipPointDetails = "\u2022 " + MessageContainerList.GetMessage(executionContext, 2231, expiringPointsForMembership, Convert.ToDateTime(customerSummaryDTO.MembershipValidity).ToString()) + Environment.NewLine + Environment.NewLine;
                }
            }
            bool isStandAloneMembership = false;
            if (customerBL.CustomerDTO.MembershipId > -1)
            {
                MembershipBL membershipBL = new MembershipBL(executionContext, customerBL.CustomerDTO.MembershipId);
                isStandAloneMembership = membershipBL.IsStandAloneMembership();
            }
            if (!isStandAloneMembership)
            {
                double requiredPointsToRetainMembership = customerBL.GetRequiredPointsToRetainMembership();
                if (customerBL.CustomerDTO.MembershipId < 0)
                {
                    customerSummaryDTO.MembershipPointDetails += "\u2022 " + MessageContainerList.GetMessage(executionContext, 2289, customerSummaryDTO.MemberShipName) + Environment.NewLine + Environment.NewLine;
                }
                else if (requiredPointsToRetainMembership != 0)
                {
                    customerSummaryDTO.MembershipPointDetails += "\u2022 " + MessageContainerList.GetMessage(executionContext, 2232, requiredPointsToRetainMembership, Convert.ToDateTime(customerSummaryDTO.MembershipValidity).ToString(), customerSummaryDTO.MemberShipName) + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    customerSummaryDTO.MembershipPointDetails += "\u2022 " + MessageContainerList.GetMessage(executionContext, 2350, customerSummaryDTO.MemberShipName) + Environment.NewLine + Environment.NewLine;
                }
            }
            double redemptionDiscountForMembership = 0;
            if (customerBL.CustomerDTO.MembershipId > -1)
            {
                MembershipBL membershipBLs = new MembershipBL(executionContext, customerBL.CustomerDTO.MembershipId);
                redemptionDiscountForMembership = membershipBLs.getMembershipDTO.RedemptionDiscount;
            }
            string gameRewards = GetGameRewards();
            string discountRewards = GetDiscountRewards();
            customerSummaryDTO.MembershipRewardsDetails = gameRewards;
            customerSummaryDTO.MembershipRewardsDetails += discountRewards;
            if (redemptionDiscountForMembership != 0)
            {
                customerSummaryDTO.MembershipRewardsDetails += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(executionContext, 2234, redemptionDiscountForMembership) + Environment.NewLine;
            }

            customerSummaryDTO.MembershipCard = customerBL.GetMembershipCardNumber();
            customerSummaryDTO.CustomerId = customerBL.CustomerDTO.Id;
            customerSummaryDTO.Name = customerBL.CustomerDTO.FirstName;
            log.LogMethodExit();
        }
        public CustomerSummaryDTO CustomerSummaryDTO
        {
            get { return customerSummaryDTO; }
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
                    if (cardGamesDTO.ExpiryDate == null || cardGamesDTO.ExpiryDate > DateTime.Now)
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

                            MembershipRewardsBL membershipRewardsBL = new MembershipRewardsBL(executionContext, cardGamesDTO.MembershipRewardsId);
                            bool isRecurringReward = membershipRewardsBL.IsRecurringReward();
                            string gameName = string.Empty;
                            if (cardGamesDTO.GameId != -1)
                            {
                                Game.Game game = new Game.Game(cardGamesDTO.GameId, executionContext);
                                gameName = game.GetGameDTO.GameName;
                            }
                            else if (cardGamesDTO.GameProfileId != -1)
                            {
                                GameProfile gameProfile = new GameProfile(cardGamesDTO.GameProfileId, executionContext);
                                gameName = gameProfile.GetGameProfileDTO.ProfileName;
                            }

                            if (cardGamesDTO.GameProfileId == -1 && cardGamesDTO.GameId == -1)
                            {
                                if (cardGamesDTO.Frequency == "N")
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(executionContext, 2286, cardGamesDTO.BalanceGames);
                                }
                                else
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(executionContext, 2349, cardGamesDTO.BalanceGames, MessageContainerList.GetMessage(executionContext, cardGamesFrequency));
                                }
                            }
                            else if (cardGamesDTO.GameId != -1 || cardGamesDTO.GameProfileId != -1)
                            {
                                if (cardGamesDTO.Frequency == "N")
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(executionContext, 2242, cardGamesDTO.BalanceGames, gameName);
                                }
                                else
                                {
                                    gameRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(executionContext, 2338, cardGamesDTO.BalanceGames, MessageContainerList.GetMessage(executionContext, cardGamesFrequency), gameName);
                                }
                            }

                            string membershipCard = customerBL.GetMembershipCardNumber();
                            List<AccountDTO> cardCoreDTO = new List<AccountDTO>();
                            cardCoreDTO = customerBL.CustomerDTO.AccountDTOList.Where(t => t.AccountId == cardGamesDTO.AccountId).ToList();
                            if (cardCoreDTO != null)
                            {
                                if (!string.IsNullOrEmpty(cardCoreDTO[0].TagNumber) && cardCoreDTO[0].TagNumber != membershipCard)
                                {
                                    if (customerBL.CustomerDTO != null)
                                    {
                                        string cardNumber = cardCoreDTO[0].TagNumber;
                                        gameRewards += " " + MessageContainerList.GetMessage(executionContext, 2287, cardNumber);
                                    }
                                }
                            }

                            if (cardGamesDTO.MembershipId != -1)
                            {
                                MembershipBL membershipBL = new MembershipBL(executionContext, cardGamesDTO.MembershipId);
                                string membershipName = membershipBL.getMembershipDTO.MembershipName;
                                if (!string.IsNullOrEmpty(membershipName))
                                {
                                    gameRewards += " " + MessageContainerList.GetMessage(executionContext, 2288, membershipName);
                                }
                            }

                            if (cardGamesDTO.ExpiryDate != null && cardGamesDTO.ExpiryDate != DateTime.MinValue)
                            {
                                gameRewards += " " + MessageContainerList.GetMessage(executionContext, 2237, Convert.ToDateTime(cardGamesDTO.ExpiryDate).ToString());
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
                                if (cardGamesDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                                {
                                    gameRewards += " " + MessageContainerList.GetMessage(executionContext, 2238, membershipRewardsBL.getMembershipRewardsDTO.RewardFrequency, MessageContainerList.GetMessage(executionContext, rewardFrequencyUnit));
                                }
                            }

                            if (cardGamesDTO.ExpireWithMembership == true && cardGamesDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                            {
                                gameRewards += ", " + MessageContainerList.GetMessage(executionContext, 2239);
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
                        if (cardDiscountsDTO.ExpiryDate == null || cardDiscountsDTO.ExpiryDate > DateTime.Now)
                        {
                            DiscountsBL discountsBL;
                            using (UnitOfWork unitOfWork = new UnitOfWork())
                            {
                                discountsBL = new DiscountsBL(executionContext, unitOfWork, cardDiscountsDTO.DiscountId, false);
                            }
                            
                            string discountName = discountsBL.DiscountsDTO.DiscountName;
                            double discountValue = 0;
                            MembershipRewardsBL membershipRewardsBL = new MembershipRewardsBL(executionContext, cardDiscountsDTO.MembershipRewardsId);
                            bool isRecurringReward = membershipRewardsBL.IsRecurringReward();

                            string membershipName = string.Empty;
                            if (cardDiscountsDTO.MembershipId != -1)
                            {
                                MembershipBL membershipBL = new MembershipBL(executionContext, cardDiscountsDTO.MembershipId);
                                membershipName = membershipBL.getMembershipDTO.MembershipName;
                            }

                            List<AccountDTO> cardCoreDTO = new List<AccountDTO>();
                            cardCoreDTO = customerBL.CustomerDTO.AccountDTOList.Where(t => t.AccountId == cardDiscountsDTO.AccountId).ToList();
                            string cardNumber = cardCoreDTO[0].TagNumber;
                            string membershipCard = customerBL.GetMembershipCardNumber();

                            discountRewards += Environment.NewLine + "\u2022 " + MessageContainerList.GetMessage(executionContext, 2644, discountName);
                            if (!string.IsNullOrEmpty(cardNumber) && cardNumber != membershipCard)
                            {
                                discountRewards += " " + MessageContainerList.GetMessage(executionContext, 2287, cardNumber);
                            }
                            if (!string.IsNullOrEmpty(membershipName))
                            {
                                discountRewards += " " + MessageContainerList.GetMessage(executionContext, 2288, membershipName);
                            }

                            if (discountsBL.DiscountsDTO.DiscountAmount != null)
                            {
                                discountValue = Convert.ToDouble(discountsBL.DiscountsDTO.DiscountAmount);
                                discountRewards += " " + MessageContainerList.GetMessage(executionContext, 2236, discountName, discountValue.ToString());
                            }
                            else if (discountsBL.DiscountsDTO.DiscountPercentage != null)
                            {
                                discountValue = Convert.ToDouble(discountsBL.DiscountsDTO.DiscountPercentage);
                                discountRewards += " " + MessageContainerList.GetMessage(executionContext, 2235, discountName, discountValue);
                            }

                            if (cardDiscountsDTO.ExpiryDate != null && cardDiscountsDTO.ExpiryDate != DateTime.MinValue)
                            {
                                discountRewards += " " + MessageContainerList.GetMessage(executionContext, 2237, Convert.ToDateTime(cardDiscountsDTO.ExpiryDate).ToString());
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
                                    discountRewards += " " + MessageContainerList.GetMessage(executionContext, 2238, membershipRewardsBL.getMembershipRewardsDTO.RewardFrequency, MessageContainerList.GetMessage(executionContext, rewardFrequencyUnit));
                                }
                            }

                            if (cardDiscountsDTO.ExpireWithMembership == "Y" && cardDiscountsDTO.MembershipId == customerBL.CustomerDTO.MembershipId)
                            {
                                discountRewards += ", " + MessageContainerList.GetMessage(executionContext, 2239);
                            }
                        }
                    }
                }
            }

            log.LogMethodExit(discountRewards);
            return discountRewards;
        }
    }
}
