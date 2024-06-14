/********************************************************************************************
 * Project Name - CCStatusPGW DTO
 * Description  - Data object of CCStatusPGW
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        19-Jun-2017   Lakshminarayana          Created 
 *2.60        15-May-2019   Nitin pai                Modified for guest app
 *2.70.2        10-Jul-2019   Girish kundar            Modified : Added constructor for required fields .
 *                                                              Added Missed Who columns. 
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{

    /// <summary>
    /// This is the CCStatusPGW data object class. This acts as data holder for the CCStatusPGW business object
    /// </summary>
    public class CCStatusPGWDTO
    {
        /// <summary>
        /// Status is success.
        /// </summary>
        public const string STATUS_SUCCESS = "Success";
        public const string STATUS_DECLINED = "Declined";
        public const string STATUS_SUBMITTED = "Submitted";

        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  StatusID field
            /// </summary>
            STATUS_ID,
            /// <summary>
            /// Search by Status message field
            /// </summary>
            STATUS_MESSAGE,
            /// <summary>
            /// Search by SiteId field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
        }

        private int statusId;
        private string statusMessage;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CCStatusPGWDTO()
        {
            log.LogMethodEntry();
            statusId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor withRequired data fields
        /// </summary>
        public CCStatusPGWDTO(int statusId, string statusMessage)
            :this()
        {
            log.LogMethodEntry(statusId, statusMessage);
            this.statusId = statusId;
            this.statusMessage = statusMessage;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CCStatusPGWDTO(int statusId, string statusMessage, string guid, bool synchStatus,
                              int siteId, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(statusId, statusMessage)
        {
            log.LogMethodEntry(statusId, statusMessage, guid, synchStatus, siteId, masterEntityId,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the StatusID field
        /// </summary>
        public int StatusId
        {
            get
            {
                return statusId;
            }

            set
            {
                IsChanged = true;
                statusId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the StatusMessage field
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }

            set
            {
                IsChanged = true;
                statusMessage = value;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value;  }
        }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || statusId < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current CCStatusPGWDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------CCStatusPGWDTO-----------------------------\n");
            returnValue.Append(" StatusID : " + StatusId);
            returnValue.Append(" StatusMessage : " + StatusMessage);
            returnValue.Append("\n-------------------------------------------------------------\n");
            string return_value = returnValue.ToString();
            log.LogMethodExit(return_value);
            return return_value;
        }
    }
}
