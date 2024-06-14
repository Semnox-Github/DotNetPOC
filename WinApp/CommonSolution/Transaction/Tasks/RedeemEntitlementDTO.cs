/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Redeem Entitlement
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.1     23-Mar-2021   Vikas Dwivedi           Created 
 *2.130.2     13-Dec-2021   Deeksha                 Modified to add counter items and play credits 
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Redeem Entitlement data object class. This acts as data holder for the Redeem Entitlement business object
    /// </summary>
    public class RedeemEntitlementDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cardId;
        private int managerId;
        private FromTypeEnum fromType;
        private FromTypeEnum toType;
        private decimal fromValue;
        private string remarks;

        /// <summary>
        /// FromType Enum
        /// </summary>
        public enum FromTypeEnum
        {
            ///<summary>
            ///NONE
            ///</summary>
            [Description("None")] NONE = -1,

            ///<summary>
            ///TICKETS
            ///</summary>
            [Description("Tickets")] TICKETS = 1,

            ///<summary>
            ///CREDITS
            ///</summary>
            [Description("Credits")] CREDITS = 2,

            /// <summary>
            /// BONUS
            /// </summary>
            [Description("Bonus")]BONUS = 3,

            /// <summary>
            /// TIME
            /// </summary>
            [Description("Time")] TIME = 4,
            /// <summary>
            /// Courtesy
            /// </summary>
            [Description("Courtesy")] COURTESY = 5,

            /// <summary>
            /// Counter Items
            /// </summary>
            [Description("Counter Items")] COUNTERITEMS = 6,

            /// <summary>
            /// Play Credits
            /// </summary>
            [Description("Play Credits")] PLAYCREDITS = 7

        }

        /// <summary>
        /// ToType Enum
        /// </summary>
        public enum ToTypeEnum
        {
            ///<summary>
            ///NONE
            ///</summary>
            [Description("None")] NONE = -1,

            ///<summary>
            ///TICKETS
            ///</summary>
            [Description("Tickets")] TICKETS = 1,

            ///<summary>
            ///CREDITS
            ///</summary>
            [Description("Credits")] CREDITS = 2,

            /// <summary>
            /// BONUS
            /// </summary>
            [Description("Bonus")] BONUS = 3,

            /// <summary>
            /// TIME
            /// </summary>
            [Description("Bonus")] TIME = 4
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RedeemEntitlementDTO()
        {
            log.LogMethodEntry();
            cardId = -1;
            managerId = -1;
            fromType = FromTypeEnum.NONE;
            toType = FromTypeEnum.NONE;
            fromValue = 0;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedeemEntitlementDTO(int cardId, int managerId, FromTypeEnum fromType, FromTypeEnum toType, decimal fromValue, string remarks)
        {
            log.LogMethodEntry();
            this.cardId = cardId;
            this.managerId = managerId;
            this.fromType = fromType;
            this.toType = toType;
            this.fromValue = fromValue;
            this.remarks = remarks;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; }
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

        /// <summary>
        /// Get/Set method of the FromValue field
        /// </summary>
        public decimal FromValue
        {
            get { return fromValue; }
            set { fromValue = value; }
        }
        /// <summary>
        /// Get/Set method of the FromType field
        /// </summary>
        public FromTypeEnum FromType
        {
            get { return fromType; }
            set { fromType = value; }
        }
        /// <summary>
        /// Get/Set method of the ToType field
        /// </summary>
        public FromTypeEnum ToType
        {
            get { return toType; }
            set { toType = value; }
        }
    }
}
