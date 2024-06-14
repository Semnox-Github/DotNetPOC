/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Data object of DirectSQL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the DirectSQLDTO data object class. This acts as data holder for the DirectSQL business object
    /// </summary>
    public class DirectSQLDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DIRECT SQL ID field
            /// </summary>
            DIRECT_SQL_ID,
            /// <summary>
            /// Search by  TABLE NAME field
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }
        
        private int directSQLId;
        private string query;
        private string tableName;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DirectSQLDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            directSQLId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public DirectSQLDTO(int directSQLId, string query, string tableName)
            :this()
        {
            log.LogMethodEntry(directSQLId, query, tableName);
            this.directSQLId = directSQLId;
            this.query = query;
            this.tableName = tableName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public DirectSQLDTO(int directSQLId, string query, string tableName, DateTime lastUpdatedDate, string lastUpdatedBy,
                            int siteId,string createdBy, DateTime creationDate)
            :this(directSQLId, query, tableName)
        {
            log.LogMethodEntry(directSQLId, query, tableName, lastUpdatedDate, lastUpdatedBy, siteId,createdBy, creationDate);
            this.siteId = siteId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DirectSQLId  field
        /// </summary>
        public int DirectSQLId
        {
            get { return directSQLId; }
            set { directSQLId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Query  field
        /// </summary>
        public string Query
        {
            get { return query; }
            set { query = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TableName field
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value;  }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || directSQLId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}

