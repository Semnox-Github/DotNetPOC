using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClsValidations
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public clsAmountInputArgs validateAmonts(clsAmountInputArgs AllAmounts)
        {
            log.LogMethodEntry(AllAmounts);

            if (AllAmounts.Amount == "")
            {
                AllAmounts.RetMessage = PGSEMessages.PGValAmountBlank.ToString();

                log.LogMethodExit(AllAmounts);
                return AllAmounts;
            }

            string p = AllAmounts.Amount;
            if (!Regex.Match(p, @"^((\d)*(\.\d{2}?)){1}$").Success)
            {
                AllAmounts.RetMessage = PGSEMessages.PGValPurAmtformat.ToString();

                log.LogMethodExit(AllAmounts);
                return AllAmounts;
            }

            log.LogMethodExit(AllAmounts);
            return AllAmounts;
        }
    }

    /// <summary>
    /// Class clsAmountInputArgs
    /// </summary>
    public class clsAmountInputArgs
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string amount;
        private string retMessage = "";

        /// <summary>
        /// clsAmountInputArgs Method
        /// </summary>
        /// <param name="Purchaseamount"></param>
        public clsAmountInputArgs(string Purchaseamount)
        {
            log.LogMethodEntry(Purchaseamount);

            amount = Purchaseamount;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get Property for Amount
        /// </summary>
        public string Amount
        {
            get { return amount; }
        }
        /// <summary>
        /// Get/Set For RetMessage
        /// </summary>
        public string RetMessage
        {
            get { return retMessage; }
            set { retMessage = value; }
        }
    }
}
