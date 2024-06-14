using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// CustomerFingerPrintDTO class
    /// </summary>
    public class CustomerFingerPrintDTO
    {

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        int id;
        int cardId;
        string template;
        bool activeFlag;
        string source;
        DateTime lastUpdatedDate;
        string lastUpdatedBy;
        int siteId;
        string guid;
        bool syncStatus;
        int masterEntityId;

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerFingerPrintDTO()
        {
            log.Debug("Starts-CustomerFingerPrintDTO() default constructor.");

            this.id = -1;
            this.cardId = -1;
            this.template = null;
            this.activeFlag = false;
            this.source = null;
            this.siteId = -1;
            this.guid = "";
            this.syncStatus = false;
            this.masterEntityId = -1;
            log.Debug("Ends-CustomerFingerPrintDTO() default constructor.");

        }

        /// <summary>
        ///parameterised constructor
        /// </summary>
        public CustomerFingerPrintDTO(int id, int cardId, string template, bool activeFlag, string source, DateTime lastUpdatedDate, string lastUpdatedBy, int siteId, string guid, bool syncStatus, int masterEntityId)
        {
            log.Debug("Starts-CustomerFingerPrintDTO() constructor.");
            this.id = id;
            this.cardId = cardId;
            this.template = template;
            this.activeFlag = activeFlag;
            this.source = source;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.syncStatus = syncStatus;
            this.masterEntityId = masterEntityId;
            log.Debug("Ends-CustomerFingerPrintDTO() constructor.");
        }

        /// <summary>
        /// Get/Set Method for Id
        /// </summary>
        public int Id { get { return id; } set { id = value; } }
        /// <summary>
        /// Get/Set Method for CardId 
        /// </summary>
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method for Template
        /// </summary>
        public string Template { get { return template; } set { template = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method for ActiveFlag
        /// </summary>
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method for Source
        /// </summary>
        public string Source { get { return source; } set { source = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method for LastUpdatedDate
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method for LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set Method for SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set Method for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set Method for SyncStatus
        /// </summary>
        public bool SyncStatus { get { return syncStatus; } set { syncStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set Method for MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
