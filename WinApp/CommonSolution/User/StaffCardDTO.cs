using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class StaffCardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int userId;
        private int trxId;
        private string cardNumber;
        private int cardId;
        private string remarks;
        private int managerId;
        private int productId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StaffCardDTO()
        {
            log.LogMethodEntry();
            userId = -1;
            trxId = -1;
            managerId = -1;
            cardId = -1;
            cardNumber = string.Empty;
            remarks = string.Empty;
            productId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public StaffCardDTO(int userId, int trxId, int managerId, int productId, string cardNumber,
                            string remarks, int cardId = -1)
            : this()
        {
            log.LogMethodEntry(userId, trxId, managerId, cardNumber,
                               remarks);
            this.userId = userId;
            this.trxId = trxId;
            this.managerId = managerId;
            this.cardNumber = cardNumber;
            this.remarks = remarks;
            this.cardId = cardId;
            this.productId = productId;
            log.LogMethodExit();
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        /// <summary>
        /// Get/Set method of the userId field
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; }
        }

        /// <summary>
        /// Get/Set method of the managerId field
        /// </summary>
        public int ManagerId
        {
            get { return managerId; }
            set { managerId = value; }
        }

        /// <summary>
        /// Get method of the cardNumber field
        /// </summary>
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }


        /// <summary>
        /// Get method of the remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public int CardId
        {
            get
            {
                return cardId;
            }
            set
            {
                cardId = value;
            }
        }
    }
}
