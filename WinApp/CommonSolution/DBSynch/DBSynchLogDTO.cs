/********************************************************************************************
 * Project Name - DBSynchLog DTO
 * Description  - Data object of DBSynchLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        20-Mar-2017   Lakshminarayana          Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    /// This is the DBSynchLog data object class. This acts as data holder for the DBSynchLog business object
    /// </summary>
    public class DBSynchLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by timestamp field
            /// </summary>
            TIME_STAMP_GREATER_THAN,
            /// <summary>
            /// Search by timestamp field
            /// </summary>
            TIME_STAMP_LESSER_THAN,
            /// <summary>
            /// Search by tableName field
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search by operation field
            /// </summary>
            OPERATION,
            /// <summary>
            /// Search by Guid field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
        }
           private string operation;
           private string guid;
           private string tableName;
           private DateTime timeStamp;
           private int siteId;
           private string createdBy;
           private DateTime creationDate;
           private DateTime lastUpdateDate;
           private string lastUpdatedBy;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DBSynchLogDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DBSynchLogDTO(string operation, string guid, string tableName, DateTime timeStamp, int siteId)
            :this()
        {
            log.LogMethodEntry(operation, guid, tableName, timeStamp, siteId);
            this.operation = operation;
            this.guid = guid;
            this.tableName = tableName;
            this.timeStamp = timeStamp;
            this.siteId = siteId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DBSynchLogDTO(string operation, string guid, string tableName,DateTime timeStamp, int siteId,
                             string createdBy,DateTime creationDate,string lastUpdatedBy, DateTime lastUpdateDate)
            :this(operation, guid, tableName, timeStamp, siteId)
        {
            log.LogMethodEntry( operation,guid,tableName,timeStamp,siteId,createdBy, creationDate,lastUpdatedBy, lastUpdateDate);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this. lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the operation field
        /// </summary>
        [Browsable(false)]
        public string Operation
        {
            get
            {
                return operation;
            }

            set
            {
                this.IsChanged = true;
                operation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get method of the tableName field
        /// </summary>
        [Browsable(false)]
        public string TableName
        {
            get
            {
                return tableName;
            }

            set
            {
                this.IsChanged = true;
                tableName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the timeStamp field
        /// </summary>
        [Browsable(false)]
        public DateTime TimeStamp
        {
            get
            {
                return timeStamp;
            }

            set
            {
                this.IsChanged = true;
                timeStamp = value;
            }
        }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; this.IsChanged = true; }
        }
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
