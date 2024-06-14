/********************************************************************************************
 * Project Name - CardCore project 
 * Description  - Data object of the CardCreditPlusBalanceDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00       17-May-2017     Rakshith           Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// CardCoreDTO Class
    /// </summary>
    public class CardCreditPlusBalanceDTO 
    {

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int creditPlusTickets;

        double creditPlusBalance;
        double creditPlusLoyaltyPoints;
        double creditPlusRefundableBalance;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CardCreditPlusBalanceDTO()
        {
            creditPlusTickets = 0;
            creditPlusBalance = 0;
            creditPlusLoyaltyPoints = 0;
            creditPlusRefundableBalance = 0;
        }


        public int CreditPlusTickets { get { return creditPlusTickets; } set { creditPlusTickets = value; } }
        public double CreditPlusBalance { get { return creditPlusBalance; } set { creditPlusBalance = value; } }
        public double CreditPlusLoyaltyPoints { get { return creditPlusLoyaltyPoints; } set { creditPlusLoyaltyPoints = value;  } }
        public double CreditPlusRefundableBalance { get { return creditPlusRefundableBalance; } set { creditPlusRefundableBalance = value; } }


    }
}
