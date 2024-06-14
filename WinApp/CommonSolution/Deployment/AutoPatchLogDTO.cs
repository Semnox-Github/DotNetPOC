/********************************************************************************************
 * Project Name - Auto Patch Log DTO
 * Description  - Data object of auto patch log 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// This is the auto patch log data object class. This acts as data holder for the auto patch log business object
    /// </summary>
    public class AutoPatchLogDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        int logId;
        DateTime timeStamp;
        string objectName;
        string description;
        string type;        
        string createdBy;
        DateTime creationDate;
        string guid;
        int siteId;
        string assetName;
        bool synchStatus;
        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoPatchLogDTO()
        {
            log.Debug("Starts-AutoPatchLogDTO() default constructor.");
            logId = -1;           
            log.Debug("Ends-AutoPatchLogDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AutoPatchLogDTO(int logId, DateTime timeStamp, string objectName, string description, string type,
                                string createdBy, DateTime creationDate, string guid, int siteId, string assetName, bool synchStatus)
        {
            log.Debug("Starts-AutoPatchLogDTO(with all the data fields) Parameterized constructor.");
            this.logId = logId;
            this.timeStamp = timeStamp;
            this.objectName = objectName;
            this.description = description;
            this.type = type;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.guid = guid;
            this.siteId = siteId;
            this.assetName = assetName;
            this.synchStatus = synchStatus;
            log.Debug("Ends-AutoPatchLogDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the LogId field
        /// </summary>
        [DisplayName("Log Id")]
        [ReadOnly(true)]
        public int LogId { get { return logId; } set { logId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TimeStamp field
        /// </summary>
        [DisplayName("Time Stamp")]
        public DateTime TimeStamp { get { return timeStamp; } set { timeStamp = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ObjectName field
        /// </summary>
        [DisplayName("Object Name")]
        public string ObjectName { get { return objectName; } set { objectName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public string Type { get { return type; } set { type = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SystemName field
        /// </summary>
        [DisplayName("System Name")]
        public string SystemName { get { return assetName; } set { assetName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || logId < 0;
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
