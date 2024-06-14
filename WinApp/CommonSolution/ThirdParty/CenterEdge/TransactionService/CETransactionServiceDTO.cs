/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CETransactionServiceDTO class - This is neutral class which holds the details for the 
 * Bulk card issue and transaction POST 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge.TransactionService
{

    /// <summary>
    /// Neutral class  for CE bulk issue and transactions
    /// </summary>

    public class CETransactionServiceDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CETransactionServiceDTO()
        {
            log.LogMethodEntry();
            cardNumber = "";
            transactions = new List<TransactionDTO>();
            sinceId = null;
            startingCardNumber = "";
            cardNumbers = new List<string>();
            adjustments = new List<Adjustments>();
            operators = null;
            log.LogMethodExit();
        }

        public string cardNumber { get; set; }
        public List<TransactionDTO> transactions { get; set; }
        public int? sinceId { get; set; }
        public string startingCardNumber { get; set ;  }
        public List<string> cardNumbers { get; set; }
        public List<Adjustments> adjustments { get; set; } 
        public OperatorDTO operators { get ; set ; }
    }
}
