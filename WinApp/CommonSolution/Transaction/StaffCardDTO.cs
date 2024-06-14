/********************************************************************************************
 * Project Name - StaffCardDTO
 * Description  - Data object of StaffCards
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.120.1       26-May-2021    Abhishek               Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the StaffCardDTO data object class.
    /// </summary>
    public class StaffCardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int userId;
        private int trxId;
        private string cardNumber;
        private int productId;
        private string remarks;
        private int managerId;
     
        /// <summary>
        /// Default Constructor
        /// </summary>
        public StaffCardDTO()
        {
            log.LogMethodEntry();
            userId = -1;
            trxId = -1;
            managerId = -1;
            productId = -1;
            cardNumber = string.Empty;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public StaffCardDTO(int userId, int trxId, int managerId, int productId, string cardNumber,
                            string remarks)
            :this()
        {
            log.LogMethodEntry(userId, trxId, managerId, productId, cardNumber,
                               remarks);
            this.userId = userId;
            this.trxId = trxId;
            this.managerId = managerId;
            this.productId = productId;
            this.cardNumber = cardNumber;
            this.remarks = remarks;
            log.LogMethodExit();
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
        /// Get/Set method of the productId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        /// <summary>
        /// Get method of the remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}
