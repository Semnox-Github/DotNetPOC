/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption value converter
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
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class RedemptionValueConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int redemptionRuleId = (int)value;
            List<object> collection = parameter as List<object>;
            decimal redeemPoints;
            decimal.TryParse(collection[1].ToString(), out redeemPoints);
            List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTOs = collection[2] as List<LoyaltyRedemptionRuleContainerDTO>;
            string amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(collection[0] as ExecutionContext, "AMOUNT_FORMAT");
            LoyaltyRedemptionRuleContainerDTO loyaltyRedemptionRuleContainerDTO = loyaltyRedemptionRuleContainerDTOs.FirstOrDefault(r => r.RedemptionRuleId == redemptionRuleId);
            if (loyaltyRedemptionRuleContainerDTO != null)
            {
                decimal redemptionValue = loyaltyRedemptionRuleContainerDTO.RedemptionValue;
                decimal loyaltyPoints = loyaltyRedemptionRuleContainerDTO.LoyaltyPoints;
                decimal x;
                if(loyaltyRedemptionRuleContainerDTO.MultiplesOnly == 'Y')
                {
                    decimal n;
                    decimal d;
                    decimal m;
                    if (redeemPoints < loyaltyRedemptionRuleContainerDTO.MinimumPoints)
                    {
                        n = 0;
                    }
                    else
                    {
                        n = redeemPoints;
                    }
                    if(loyaltyRedemptionRuleContainerDTO.MinimumPoints == 0)
                    {
                        d = 1;
                        m = 1;
                    }
                    else
                    {
                        d = loyaltyRedemptionRuleContainerDTO.MinimumPoints;
                        m = loyaltyRedemptionRuleContainerDTO.MinimumPoints;
                    }
                    x = (int)(n / d) * m;
                    if(loyaltyRedemptionRuleContainerDTO.LoyaltyPoints == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return ((loyaltyRedemptionRuleContainerDTO.RedemptionValue * x) / loyaltyRedemptionRuleContainerDTO.LoyaltyPoints).ToString(amountFormat); ;
                    }
                }
                else
                {
                    if (redeemPoints < loyaltyRedemptionRuleContainerDTO.MinimumPoints)
                    {
                        x = 0;
                    }
                    else
                    {
                        x = redeemPoints;
                    }
                    if (loyaltyRedemptionRuleContainerDTO.LoyaltyPoints == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return ((loyaltyRedemptionRuleContainerDTO.RedemptionValue * x) / loyaltyRedemptionRuleContainerDTO.LoyaltyPoints).ToString(amountFormat);
                    }
                }
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
