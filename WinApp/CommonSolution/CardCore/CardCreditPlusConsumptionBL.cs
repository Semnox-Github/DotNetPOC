
/********************************************************************************************
 * Project Name - CardCore
 * Description  - Bussiness logic of the  CardCreditPlusConsumption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       7-Nov-2017    Jeevan         Created 
 ********************************************************************************************/

using Semnox.Parafait.CardCore;
using System.Collections.Generic;
using System.Data;

namespace Semnox.Parafait.CardCore
{

    public class CardCreditPlusConsumptionBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardCreditPlusConsumptionBL()
        {
        }


        /// <summary>
        /// GetCardCreditPlusLoyaltyConsumption
        /// </summary>
        /// <param name="cardCreditPlusId"></param>
        /// <returns></returns>
        public DataTable GetCardCreditPlusLoyaltyConsumption(int cardCreditPlusId)
        {
            log.Debug("Starts-GetCardCreditPlusLoyaltyConsumption(cardCreditPlusId) method.");
            CardCreditPlusConsumptionHandler cardCreditPlusConsumptionHandler = new CardCreditPlusConsumptionHandler();
            log.Debug("Ends-GetCardCreditPlusLoyaltyConsumption(cardCreditPlusId) method by returning the result of cardCreditPlusConsumptionHandler.GetCardCreditPlusConsumption() call.");
            return cardCreditPlusConsumptionHandler.GetCardCreditPlusConsumption(cardCreditPlusId);
        }


    }



    /// <summary>
    ///  Manages the list of CardCreditPlusConsumptionList
    /// </summary>
    public class CardCreditPlusConsumptionList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
