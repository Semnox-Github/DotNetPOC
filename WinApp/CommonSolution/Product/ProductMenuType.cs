/********************************************************************************************
 * Project Name - Product
 * Description  - Represents the different types of static menu types
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class ProductMenuType
    {
        /// <summary>
        /// Search by REDEMPTION
        /// </summary>
        public const string REDEMPTION = "R";
        /// <summary>
        /// Search by ORDER_SALE
        /// </summary>
        public const string ORDER_SALE = "O";
        /// <summary>
        /// Search by ORDER_SALE
        /// </summary>
        public const string RESERVATION = "B";

        public static bool IsValid(string type)
        {
            return type == REDEMPTION || type == ORDER_SALE || type == RESERVATION;
        }

        public static string GetDescription(string type)
        {
            if(type == REDEMPTION)
            {
                return "Redemption";
            }
            if (type == ORDER_SALE)
            {
                return "Order Sale";
            }
            if (type == ORDER_SALE)
            {
                return "Reservation";
            }
            throw new ArgumentException("Invalid product menu type", "type");
        }
    }
}