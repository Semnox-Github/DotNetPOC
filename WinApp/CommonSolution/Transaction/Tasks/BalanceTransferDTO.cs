/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Balance Transfer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.1     23-Mar-2021   Vikas Dwivedi           Created 
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Balance Transfer data object class. This acts as data holder for the Balance Transfer business object
    /// </summary>
    public class BalanceTransferDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int fromCardId;
        private List<TransferDetailsDTO> transferDetails;
        private int managerId;
        private string remarks;
        /// <summary>
        /// This is the Balance Transfer data object class. This acts as data holder for the Balance Transfer business object
        /// </summary>
        public class TransferDetailsDTO
        {
            private int cardId;
            private Dictionary<RedeemEntitlementDTO.FromTypeEnum, decimal> entitlements;
            public int CardId
            {
                get { return cardId; }
                set { cardId = value; }
            }
            public Dictionary<RedeemEntitlementDTO.FromTypeEnum, decimal> Entitlements
            {
                get { return entitlements; }
                set { entitlements = value; }
            }
            /// <summary>
            /// Default Constructor
            /// </summary>
            public TransferDetailsDTO()
            {
                log.LogMethodEntry();
                cardId = -1;
                entitlements = new Dictionary<RedeemEntitlementDTO.FromTypeEnum, decimal>();
                log.LogMethodExit();
            }

            /// <summary>
            /// Constructor with all the data fields
            /// </summary>
            public TransferDetailsDTO(int cardId, Dictionary<RedeemEntitlementDTO.FromTypeEnum, decimal> entitlements)
            {
                log.LogMethodEntry();
                this.cardId = cardId;
                this.entitlements = entitlements;
                log.LogMethodExit();
            }
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BalanceTransferDTO()
        {
            log.LogMethodEntry();
            fromCardId = -1;
            transferDetails = new List<TransferDetailsDTO>();
            managerId = -1;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public BalanceTransferDTO(int fromCardId, int managerId, List<TransferDetailsDTO> transferDetails, string remarks)
        {
            log.LogMethodEntry();
            this.fromCardId = fromCardId;
            this.managerId = managerId;
            this.transferDetails = transferDetails;
            this.remarks = remarks;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int FromCardId
        {
            get { return fromCardId; }
            set { fromCardId = value; }
        }
        /// <summary>
        /// Get/Set method of the transferDetails field
        /// </summary>
        public List<TransferDetailsDTO> TransferDetails
        {
            get { return transferDetails; }
            set { transferDetails = value; }
        }
        /// <summary>
        /// Get/Set method of the ManagerId field
        /// </summary>
        public int ManagerId
        {
            get { return managerId; }
            set { managerId = value; }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}
