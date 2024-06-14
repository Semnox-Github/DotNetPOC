/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Exchange Token
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.1     23-Mar-2021   Vikas Dwivedi           Created 
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Exchange Token data object class. This acts as data holder for the Exchange Token business object
    /// </summary>
    public class ExchangeTokenDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int cardId;
        private int managerId;
        private FromTypeEnum fromType;
        private FromTypeEnum toType;
        private decimal tokenValue;
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
            ///TOKEN
            ///</summary>
            [Description("Token")] TOKEN = 1,

            ///<summary>
            ///CREDITS
            ///</summary>
            [Description("Credits")] CREDITS = 2

        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExchangeTokenDTO()
        {
            log.LogMethodEntry();
            managerId = -1;
            tokenValue = 0;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ExchangeTokenDTO(int cardId, int managerId, FromTypeEnum fromType, FromTypeEnum toType, decimal tokenValue, string remarks)
        {
            log.LogMethodEntry();
            this.cardId = cardId;
            this.managerId = managerId;
            this.fromType = fromType;
            this.toType = toType;
            this.tokenValue = tokenValue;
            this.remarks = remarks;
            log.LogMethodExit();
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
        /// Get/Set method of the FromTypeValue field
        /// </summary>
        public decimal TokenValue
        {
            get { return tokenValue; }
            set { tokenValue = value; }
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
        /// Get/Set method of the FromTypeValue field
        /// </summary>
        public FromTypeEnum ToType
        {
            get { return toType; }
            set { toType = value; }
        }
        /// <summary>
        /// Get/Set method of the cardId field
        /// </summary>
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; }
        }
    }
}
