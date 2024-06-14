
/********************************************************************************************
 * Project Name - Staff Card Management DTO
 * Description  - Data Objects of the Staff Card Management DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Amaresh         Created 
 ********************************************************************************************
 *1.00        24-Apr-2017   Suneetha        Modified 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the StaffCardManagementDTO data object class. This acts as data holder for the StaffCardManagement. this hold the data of StaffCardManagement xml.
    /// </summary>
    public class StaffCardManagementDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        int userId;
        int cardId;
        string cardNumber;
        string userFirstName;
        string userLastName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public StaffCardManagementDTO()
        {
            log.Debug("Starts-StaffCardManagementDTO() default constructor.");
            userId = -1;
            cardId = -1;
            log.Debug("Starts-StaffCardManagementDTO() default constructor.");
        }

        /// <summary>
        /// argument constructor
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardId"></param>
        /// <param name="cardNumber"></param>
        /// <param name="userFirstName"></param>
        /// <param name="userLastName"></param>
        public StaffCardManagementDTO(int userId, int cardId, string cardNumber, string userFirstName, string userLastName)
        {
            log.Debug("Starts-StaffCardManagementDTO() argument constructor.");
            this.userId = userId;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.userFirstName = userFirstName;
            this.userLastName = userLastName;
            log.Debug("Starts-StaffCardManagementDTO() argument constructor.");
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        public int UserId { get { return userId; } set { userId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("CardNumber")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("CardNumber")]
        public string UserFirstName { get { return userFirstName; } set { userFirstName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UserLastName field
        /// </summary>
        [DisplayName("UserLastName")]
        public string UserLastName { get { return userLastName; } set { userLastName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || userId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }


        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
