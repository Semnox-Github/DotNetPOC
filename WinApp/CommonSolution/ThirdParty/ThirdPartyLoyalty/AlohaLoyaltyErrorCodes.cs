using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{ 
    class AlohaLoyaltyErrorCodes
    {
        //Error code and failure count to be considered
        public Dictionary<string, int> AlohaLoyaltyErrorCount;
        public AlohaLoyaltyErrorCodes()
        {
            AlohaLoyaltyErrorCount = new Dictionary<string, int>();
            AlohaLoyaltyErrorCount.Add("TRANSACTION_AMOUNT_DIFFERS_FROM_REQUESTED_AMOUNT", 1);
            AlohaLoyaltyErrorCount.Add("UNKNOWN_ERROR FROM_REQUESTED_AMOUNT", 10);
            AlohaLoyaltyErrorCount.Add("INSUFFICIENT_BALANCE", 1);
            AlohaLoyaltyErrorCount.Add("ADD_VALUE_NOT_ALLOWED", 1);
            AlohaLoyaltyErrorCount.Add("CARD_NOT_FOUND", 1);
            AlohaLoyaltyErrorCount.Add("CARD_EXPIRED", 1);
            AlohaLoyaltyErrorCount.Add("STORE_NOT_FOUND", 1);
            AlohaLoyaltyErrorCount.Add("STORE_NOT_LICENSED", 10);
            AlohaLoyaltyErrorCount.Add("INVALID_USER_NAME_PASSWORD", 10);
            AlohaLoyaltyErrorCount.Add("DUPLICATE_TRANSACTION", 1);
            AlohaLoyaltyErrorCount.Add("TRANSACTION_NOT_FOUND", 1);
            AlohaLoyaltyErrorCount.Add("TRANSACTION_ALREADY_VOIDED", 0);
            AlohaLoyaltyErrorCount.Add("INVALID_TRANSACTION_TYPE", 1);
            AlohaLoyaltyErrorCount.Add("INVALID_TRANSACTION_DATE", 10);
            AlohaLoyaltyErrorCount.Add("INVALID_CARD_NUMBER", 0);
            AlohaLoyaltyErrorCount.Add("INVALID_PIN_NUMBER", 5);
            AlohaLoyaltyErrorCount.Add("INVALID_TRANSACTION_AMOUNT", 1);
            AlohaLoyaltyErrorCount.Add("INSUFFICIENT_ACCESS_PRIVILEGES", 10); 
            AlohaLoyaltyErrorCount.Add("COMPANY_NOT_FOUND", 10);
        }
    }
}
