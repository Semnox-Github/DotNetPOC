/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - Cards class This would hold the cards related details
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class Card
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        private string accountNumber;
        private DateTime accountIssuedTime;
        private Points cardBalance;
        private List<TimePlays> cardTimePlays;
        private List<Privilege> cardPrivileges;
        //private OperatorDTO operatorDTO;
        public Card()
        {
            log.LogMethodEntry();
            accountNumber = string.Empty;
            accountIssuedTime =DateTime.MinValue;
            cardBalance = new Points();
            cardTimePlays = new List<TimePlays>();
            cardPrivileges = new List<Privilege>();
            log.LogMethodExit();
        }


        public string cardNumber { get { return accountNumber; } set { accountNumber = value; } }
        public DateTime issuedAtTime { get { return accountIssuedTime; } set { accountIssuedTime = value; } }
        public Points balance { get { return cardBalance; } set { cardBalance = value; } }
        public List<TimePlays> timePlays { get { return cardTimePlays; } set { cardTimePlays = value; } }
        public List<Privilege> privileges { get { return cardPrivileges; } set { cardPrivileges = value; } }
        //public OperatorDTO operators { get { return operatorDTO; } set { operatorDTO = value; } }
    }
}
