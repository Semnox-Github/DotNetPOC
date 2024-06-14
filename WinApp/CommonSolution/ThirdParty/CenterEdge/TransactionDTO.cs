using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
   
    public class TransactionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TransactionDTO()
        {
            log.LogMethodEntry();
            id = 0;
            type = TransactionTypes.adjustment.ToString();
            transactionTime = string.Empty;
            cardNumber = string.Empty;
            adjustments = new List<Adjustments>();
            gameId = null;
            amount = null; // new Points();
            usedTimePlay = null;
            usedPlayPrivilege = null;
            log.LogMethodExit();
        }
        public int id { get; set; }
        public string type { get; set; }
        public string transactionTime { get; set; }
        public string cardNumber { get; set; }
        public OperatorDTO operators { get; set; }
        public List<Adjustments> adjustments { get; set; }
        public int? gameId { get; set; }
        public string gameDescription { get; set; }
        public Points amount { get; set; }
        public bool? usedTimePlay { get; set; }
        public bool? usedPlayPrivilege { get; set; }
       

    }
}
