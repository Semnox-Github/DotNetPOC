/********************************************************************************************
* Project Name - RedemptionActivityDTO
* Description  - Data Transfer Object
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*0.0         11-Dec-2020       Vikas Dwivedi       Created
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionActivityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool loadToCard;
        private bool printBalanceTicket;
        private string managerToken;
        private RedemptionActivityStatusEnum status;
        private string remarks;
        private string source;
        private int tickets;
        private string parentScreenNumber;
        private int turninLocationId;
        private int targetLocationId;
        private List<RedemptionGiftsDTO> reversalRedemptionGiftDTOList;
        public enum RedemptionActivityStatusEnum
        {
            ///<summary>
            ///OPEN
            ///</summary>
            [Description("Open")] OPEN,

            ///<summary>
            ///PREPARED
            ///</summary>
            [Description("Prepared")] PREPARED,

            ///<summary>
            ///DELIVERED
            ///</summary>
            [Description("Delivered")] DELIVERED,
            ///<summary>
            ///SUSPENDED
            ///</summary>
            [Description("Suspended")] SUSPENDED,
            ///<summary>
            ///ABANDONED
            ///</summary>
            [Description("Abadonned")] ABANDONED,
            ///<summary>
            ///Reverse
            ///</summary>
            [Description("Reversed")] REVERSED
        }

        public RedemptionActivityDTO()
        {
            log.LogMethodEntry();
            loadToCard = false;
            printBalanceTicket = false;
            managerToken = string.Empty;
            status = RedemptionActivityStatusEnum.OPEN;
            remarks = string.Empty;
            source = string.Empty;
            tickets = 0;
            targetLocationId = -1;
            turninLocationId = -1;
            reversalRedemptionGiftDTOList = new List<RedemptionGiftsDTO>();
            log.LogMethodExit();
        }

        public bool LoadToCard
        {
            get
            {
                return loadToCard;
            }
            set
            {
                loadToCard = value;
            }
        }
        public int Tickets
        {
            get
            {
                return tickets;
            }
            set
            {
                tickets = value;
            }
        }

        public bool PrintBalanceTicket
        {
            get
            {
                return printBalanceTicket;
            }
            set
            {
                printBalanceTicket = value;
            }
        }

        public RedemptionActivityStatusEnum Status
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
        public string ParentScreenNumber
        {
            get
            {
                return parentScreenNumber;
            }
            set
            {
                parentScreenNumber = value;
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
        public List<RedemptionGiftsDTO> ReversalRedemptionGiftDTOList
        {
            get
            {
                return reversalRedemptionGiftDTOList;
            }
            set
            {
                reversalRedemptionGiftDTOList = value;
            }
        }
       
        public int TargetLocationId
        {
            get
            {
                return targetLocationId;
            }
            set
            {
                targetLocationId = value;
            }
        }
        public int TurninLocationId
        {
            get
            {
                return turninLocationId;
            }
            set
            {
                turninLocationId = value;
            }
        }
    }
}
