
namespace Semnox.Parafait.CardCore
{
    public class CardParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int customerId;
        int cardId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardParams()
        {
            log.LogMethodEntry();
            this.customerId = -1;
            this.cardId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CardParams(int customerId)
        {
            log.LogMethodEntry(customerId);
            this.customerId = customerId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; } }


        /// <summary>
        /// Get/Set method of the cardId field
        /// </summary>
        public int CardId { get { return cardId; } set { cardId = value; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        public string CardNumber { get ;  set; }


        /// <summary>
        /// Get/Set method of the CustomerPhoneNumber field
        /// </summary>
        public string CustomerPhoneNumber { get; set; }


    }
}
