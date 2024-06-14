/********************************************************************************************
* Project Name - RedemptionLoadToCardRequestDTO
* Description  - Data Transfer Object RedemptionLoadToCardRequest
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*0.0         11-Dec-2020      Girish Kundar      Created
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionLoadToCardRequestDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int totalTickets;
        private int accountId;
        private string managerToken;
        private string status;
        private string remarks;
        private string source;
        private bool? considerForLoyalty;

        public RedemptionLoadToCardRequestDTO()
        {
            log.LogMethodEntry();
            totalTickets = 0;
            accountId = -1;
            managerToken = string.Empty;
            status = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
            remarks = string.Empty;
            source = string.Empty;
            considerForLoyalty = null;
            log.LogMethodExit();
        }

        public bool? ConsiderForLoyalty
        {
            get
            {
                return considerForLoyalty;
            }
            set
            {
                considerForLoyalty = value;
            }
        }
        public int TotalTickets
        {
            get
            {
                return totalTickets;
            }
            set
            {
                totalTickets = value;
            }
        }

        public int AccountId
        {
            get
            {
                return accountId;
            }
            set
            {
                accountId = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
            }
        }

        public string ManagerToken
        {
            get
            {
                return managerToken;
            }
            set
            {
                managerToken = value;
            }
        }
    }
}
