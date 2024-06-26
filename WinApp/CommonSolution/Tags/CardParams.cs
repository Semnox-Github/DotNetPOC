using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public class CardParams
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int customerId;
        int cardId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardParams()
        {
            log.Debug("Starts-CardParams() default constructor.");
            this.customerId = -1;
            this.cardId = -1;
            log.Debug("Ends-CardParams() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CardParams(int customerId)
        {
            this.customerId = customerId;
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


    }
}
