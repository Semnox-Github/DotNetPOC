/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - DTO to hold card number and AccountRelationshipDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth                Created for POS UI Redesign 
 ********************************************************************************************/

namespace Semnox.Parafait.Customer.Accounts
{
    public class LinkNewCardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string cardNumber;
        private AccountRelationshipDTO accountRelationShipDTO;

        public LinkNewCardDTO()
        {
            log.LogMethodEntry();
            cardNumber = string.Empty;
            accountRelationShipDTO = new AccountRelationshipDTO();
            log.LogMethodExit();
        }
        public LinkNewCardDTO(string cardNumber, AccountRelationshipDTO accountRelationshipDTO) : this()
        {
            log.LogMethodEntry(cardNumber, accountRelationShipDTO);
            this.cardNumber = cardNumber;
            this.accountRelationShipDTO = accountRelationshipDTO;
            log.LogMethodExit();
        }

        public string CardNumber
        {
            get
            {
                return cardNumber;
            }

            set
            {
                cardNumber = value;
            }
        }

        public AccountRelationshipDTO AccountRelationShipDTO
        {
            get
            {
                return accountRelationShipDTO;
            }

            set
            {
                accountRelationShipDTO = value;
            }
        }
    }
}
