/********************************************************************************************
 * Project Name - Users
 * Description  - Data structure of the UserViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Data structure of the UserViewContainer class 
    /// </summary>
    public class UserIdentificationTagContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private string cardNumber;
        private int cardId;
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserIdentificationTagContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public UserIdentificationTagContainerDTO(int id, string cardNumber, int cardId, DateTime startDate, DateTime endDate)
        {
            log.LogMethodEntry(id, cardNumber, cardId);
            this.id = id;
            this.cardNumber = cardNumber;
            this.cardId = cardId;
            this.startDate = startDate;
            this.endDate = endDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of id field
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of cardNumber field
        /// </summary>
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of cardId field 
        /// </summary>
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

        /// <summary>
        /// Get/Set method of startDate field 
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of endDate field 
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
            }
        }
    }
}
