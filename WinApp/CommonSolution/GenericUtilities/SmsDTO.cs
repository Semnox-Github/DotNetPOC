/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - SMS object to send SMS.
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        17-Sept-2019  Mushahid Faizan     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class SmsDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string userId;
        private string password;
        private string message;
        private string mobileNumber;
        private string status;

        public SmsDTO()
        {
            log.LogMethodEntry();
            userId = "";
            password = "";
            message = "";
            mobileNumber = "";
            log.LogMethodExit();
        }
        /// <summary>
        /// constructor with required fields
        /// </summary>
        public SmsDTO(string userId, string password, string message, string mobileNumber)
            : this()
        {
            log.LogMethodEntry(userId, password, message, mobileNumber);
            this.userId = userId;
            this.password = password;
            this.message = message;
            this.mobileNumber = mobileNumber;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public string UserId { get { return userId; } set { userId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Password field
        /// </summary>
        public string Password { get { return password; } set { password = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>
        public string Message { get { return message; } set { message = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MobileNumber field
        /// </summary>
        public string MobileNumber { get { return mobileNumber; } set { mobileNumber = value; IsChanged = true; } }
        public string Status { get { return status; } set { status = value; IsChanged = true; } }


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
                    return notifyingObjectIsChanged;
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
