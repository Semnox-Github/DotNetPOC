/********************************************************************************************
 * Project Name - Customer App Configuration                                                                     
 * Description  - BL for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.130.10    08-Sep-2022      Nitin Pai      Enhanced customer activity user log table
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    public class CustomerActivityUserLogDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum ActivityCategory
        {
            PROFILE,
            CARD,
            WALLET,
            SMARTFUN,
            WEB,
            MEMBERSHIP
        }

        public enum ActivitySeverity
        {
            INFO,
            ERROR,
            FATAL
        }
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID
            /// </summary>
            ID,
            /// <summary>
            /// Search by CustomerID
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by Activity Time
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by Activity Time
            /// </summary>
            TO_DATE,
            /// <summary>
            /// Search by Active fields
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by SiteId
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Device Id
            /// </summary>
            DEVICE_ID
        }

        private int id;
        private int? customerId;
        private string deviceId;
        private string action;
        private string activity;
        private DateTime? activityTime;
        private string guid;
        private bool isActive;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private DateTime createdDate;
        private string createdBy;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string source;
        private string data;
        private string category;
        private string severity;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomerActivityUserLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            customerId = -1;
            deviceId = string.Empty;
            action = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        public CustomerActivityUserLogDTO(int id, int? customerId, string deviceId, string action, string activity, DateTime? activityTime, string source, string data, string category, string severity) : this()
        {
            log.LogMethodEntry(id, customerId, deviceId, action, activity, activityTime);
            this.id = id;
            this.customerId = customerId;
            this.deviceId = deviceId;
            this.action = action;
            this.activity = activity;
            this.activityTime = activityTime;
            this.source = source;
            this.data = data;
            this.category = category;
            this.severity = severity;
            log.LogMethodExit();
        }

        public CustomerActivityUserLogDTO(int id, int? customerId, string deviceId, string action, string activity, DateTime? activityTime,
                                            string source, string data, string category, string severity,
                                            string guid, bool isActive, bool synchStatus, int siteId, int masterEntityId, DateTime createdDate,
                                            string createdBy, string lastUpdatedBy, DateTime lastUpdatedDate)
                                        : this(id, customerId, deviceId, action, activity, activityTime, source, data, category, severity)
        {
            this.guid = guid;
            this.isActive = isActive;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdDate = createdDate;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
        }

        public int Id { get { return id; } set { id = value; IsChanged = true; } }
        public int? CustomerId { get { return customerId; } set { customerId = value; IsChanged = true; } }
        public string DeviceId { get { return deviceId; } set { deviceId = value; IsChanged = true; } }
        public string Action { get { return action; } set { action = value; IsChanged = true; } }
        public string Activity { get { return activity; } set { activity = value; IsChanged = true; } }
        public DateTime? ActivityTime { get { return activityTime; } set { activityTime = value; IsChanged = true; } }
        public string Guid { get { return guid; } set { guid = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        public int SiteId { get { return siteId; } set { siteId = value; IsChanged = true; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; IsChanged = true; } }
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; IsChanged = true; } }
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; IsChanged = true; } }
        public string Source { get { return source; } set { source = value; IsChanged = true; } }
        public string Data { get { return data; } set { data = value; IsChanged = true; } }
        public string Category { get { return category; } set { category = value; IsChanged = true; } }
        public string Severity { get { return severity; } set { severity = value; IsChanged = true; } }

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
