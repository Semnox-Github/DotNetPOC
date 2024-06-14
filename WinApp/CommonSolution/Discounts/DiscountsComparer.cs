/********************************************************************************************
 * Project Name - Discounts
 * Description  - class for DiscountsComparer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Girish Kundar   Added LogMethodEcnty() and LogMethodExit() and Removed Unused namespace's.
 *2.150.0       27-Apr-2021   Abhishek        Modified: POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This class used as a comparer for Discounts
    /// </summary>
    public class DiscountsComparer : IComparer<DiscountContainerDTO>
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// returns 1 if priority of discount x is less than priority of discount y,
        /// returns -1 if the priority of the discount x is greater than priority of the discount y,
        /// returns 0 if priority of the discount is same for both of them.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(DiscountContainerDTO x, DiscountContainerDTO y)
        {
            log.LogMethodEntry(x,y);
            int returnValue;
            if(x == null && y == null)
            {
                returnValue = 0;
            }
            else if(x == null && y != null)
            {
                returnValue = 1;
            }
            else if(x != null &&  y == null)
            {
                returnValue = -1;
            }
            else
            {
                if(x.DiscountPurchaseCriteriaCount > y.DiscountPurchaseCriteriaCount)
                {
                    returnValue = -1;
                }
                else if(x.DiscountPurchaseCriteriaCount < y.DiscountPurchaseCriteriaCount)
                {
                    returnValue = 1;
                }
                else
                {
                    if(x.MinimumCredits != null &&
                        x.MinimumCredits > 0 &&
                        y.MinimumCredits != null &&
                        y.MinimumCredits > 0)
                    {
                        if(x.MinimumCredits > y.MinimumCredits)
                        {
                            returnValue = -1;
                        }
                        else if(x.MinimumCredits < y.MinimumCredits)
                        {
                            returnValue = 1;
                        }
                        else
                        {
                            returnValue = 0;
                        }
                    }
                    else if(x.MinimumSaleAmount != null &&
                        x.MinimumSaleAmount > 0 &&
                        y.MinimumSaleAmount != null &&
                        y.MinimumSaleAmount > 0)
                    {
                        if(x.MinimumSaleAmount > y.MinimumSaleAmount)
                        {
                            returnValue = -1;
                        }
                        else if(x.MinimumSaleAmount < y.MinimumSaleAmount)
                        {
                            returnValue = 1;
                        }
                        else
                        {
                            returnValue = 0;
                        }
                    }
                    else
                    {
                        returnValue = 0;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
