/********************************************************************************************
 * Project Name - Server code discounts
 * Description  - Discounts 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks           
 *2.70.0      11-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Semnox.Parafait.Discounts;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.ServerCore
{
     static class Discounts
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static decimal getDiscountedCredits(long CardId, object GameId, decimal credits_played, decimal credits_required, Utilities Utilities)
        {
            log.LogMethodEntry(CardId, GameId, credits_played, credits_required);
            List<DiscountContainerDTO> applicableDiscountsDTOList = new List<DiscountContainerDTO>();
            DateTime currentTime = ServerDateTime.Now;
            
            //AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, (int)CardId);
            AccountDiscountListBL accountDiscountListBL = new AccountDiscountListBL(Utilities.ExecutionContext);
            List<KeyValuePair< AccountDiscountDTO.SearchByParameters, string >> searchParameters = new List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.ACCOUNT_ID, CardId.ToString()));
            searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.IS_ACTIVE, "Y"));
            List<AccountDiscountDTO> accountDiscountDTOList = accountDiscountListBL.GetAccountDiscountDTOList(searchParameters);
            if (accountDiscountDTOList != null && accountDiscountDTOList.Count > 0)
            {
                foreach (AccountDiscountDTO accountDiscountDTO in accountDiscountDTOList)
                {
                    DiscountContainerDTO discountContainerDTO =
                        DiscountContainerList.GetDiscountContainerDTO(Utilities.ExecutionContext, accountDiscountDTO.DiscountId);
                    if (discountContainerDTO == null)
                    {
                        continue;
                    }
                    if (DiscountContainerList.IsDiscountAvailable(Utilities.ExecutionContext, discountContainerDTO.DiscountId, currentTime) &&
                        DiscountContainerList.CheckMinimumCreditsPlayed(Utilities.ExecutionContext, discountContainerDTO.DiscountId, credits_played) &&
                        DiscountContainerList.IsDiscountedGame(Utilities.ExecutionContext, discountContainerDTO.DiscountId, Convert.ToInt32(GameId)))
                    {
                        applicableDiscountsDTOList.Add(discountContainerDTO);
                    }
                }
            }

            if (applicableDiscountsDTOList.Any() == false)
            {
                foreach (DiscountContainerDTO discountContainerDTO in DiscountContainerList.GetLoyaltyGameDiscountsBLList(Utilities.ExecutionContext))
                {
                    if (DiscountContainerList.IsDiscountAvailable(Utilities.ExecutionContext, discountContainerDTO.DiscountId, currentTime) &&
                        DiscountContainerList.CheckMinimumCreditsPlayed(Utilities.ExecutionContext, discountContainerDTO.DiscountId, credits_played) &&
                        DiscountContainerList.IsDiscountedGame(Utilities.ExecutionContext, discountContainerDTO.DiscountId, Convert.ToInt32(GameId)))
                    {
                        applicableDiscountsDTOList.Add(discountContainerDTO);
                    }
                }
            }
            double discountPercentage = 0;
            foreach (var discountsDTO in applicableDiscountsDTOList)
            {
                discountPercentage += discountsDTO.DiscountPercentage == null ? 0 : (double)discountsDTO.DiscountPercentage;
            }
            if(discountPercentage > 100)
            {
                discountPercentage = 100;
            }
            decimal result = (credits_required - (credits_required * (decimal)discountPercentage / 100.0M));
            log.LogMethodExit(result);
            return result;
        }
        
    }
}
