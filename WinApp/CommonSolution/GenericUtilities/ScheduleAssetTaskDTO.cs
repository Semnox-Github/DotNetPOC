/********************************************************************************************
 * Project Name - Schedule Asset Task DTO
 * Description  - Data object of Schedule Asset Task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Dec-2015   Raghuveera          Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class ScheduleAssetTaskDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByScheduleAssetTaskParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByScheduleAssetTaskParameters
        {
            /// <summary>
            /// Search by MAINT_SCH_ASSET_TASK_ID field
            /// </summary>
            MAINT_SCH_ASSET_TASK_ID = 0,
            /// <summary>
            /// Search by MAINT_SCHEDULE_ID field
            /// </summary>
            MAINT_SCHEDULE_ID = 1,
            /// <summary>
            /// Search by ASSET_GROUP_ID field
            /// </summary>
            ASSET_GROUP_ID = 2,
            /// <summary>
            /// Search by ASSET_TYPE_ID field
            /// </summary>
            ASSET_TYPE_ID = 3,
            /// <summary>
            /// Search by ASSET_ID field
            /// </summary>
            ASSET_ID = 4,
            /// <summary>
            /// Search by MAINT_TASK_GROUP_ID field
            /// </summary>
            MAINT_TASK_GROUP_ID = 5,
            /// <summary>
            /// Search by MAINT_TASK_ID field
            /// </summary>
            MAINT_TASK_ID = 6,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG = 7,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 8,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 9
        }

        int maintSchAssetTaskId;
        int maintScheduleId;
        int assetGroupId;
        int assetTypeId;
        int assetID;
        int maintTaskGroupId;
        int maintTaskId;
        string isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;//Modification on 18-Jul-2016 for publish feature

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleAssetTaskDTO()
        {
            log.Debug("Starts-ScheduleAssetTaskDTO() default constructor.");
            maintSchAssetTaskId = -1;
            maintScheduleId = -1;
            assetGroupId = -1;
            assetTypeId = -1;
            assetID = -1;
            maintTaskGroupId = -1;
            maintTaskId = -1;
            isActive = "Y";
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.Debug("Ends-ScheduleAssetTaskDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleAssetTaskDTO(int maintSchAssetTaskId, int maintScheduleId, int assetGroupId, int assetTypeId,
                                    int assetID, int maintTaskGroupId, int maintTaskId, string isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                    int siteId, bool synchStatus, int masterEntityId)//Modification on 18-Jul-2016 for publish feature
        {
            log.Debug("Starts-ScheduleAssetTaskDTO(with all the data fields) Parameterized constructor.");
            this.maintSchAssetTaskId = maintSchAssetTaskId;
            this.maintScheduleId = maintScheduleId;
            this.assetGroupId = assetGroupId;
            this.assetTypeId = assetTypeId;
            this.assetID = assetID;
            this.maintTaskGroupId = maintTaskGroupId;
            this.maintTaskId = maintTaskId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;//Modification on 18-Jul-2016 for publish feature
            log.Debug("Ends-ScheduleAssetTaskDTO(with all the data fields) Parameterized constructor.");
        }


        /// <summary>
        /// Get/Set method of the MaintSchAssetTaskId field
        /// </summary>
        [DisplayName("MaintSch Asset Task Id")]
        [ReadOnly(true)]
        public int MaintSchAssetTaskId { get { return maintSchAssetTaskId; } set { maintSchAssetTaskId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintScheduleId field
        /// </summary>
        [DisplayName("Maint Schedule")]
        public int MaintScheduleId { get { return maintScheduleId; } set { maintScheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the assetGroupId field
        /// </summary>
        [DisplayName("Asset Group")]
        public int AssetGroupId { get { return assetGroupId; } set { assetGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        [DisplayName("Asset Type")]
        public int AssetTypeId { get { return assetTypeId; } set { assetTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the assetID field
        /// </summary>
        [DisplayName("Asset")]
        public int AssetID { get { return assetID; } set { assetID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintTaskGroupId field
        /// </summary>
        [DisplayName("Maint Task Group")]
        public int MaintTaskGroupId { get { return maintTaskGroupId; } set { maintTaskGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintTaskId field
        /// </summary>
        [DisplayName("Maint Task")]
        public int MaintTaskId { get { return maintTaskId; } set { maintTaskId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive == "Y" ? true : false; } set { isActive = (value == true) ? "Y" : "N"; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }//Ends:Modification on 18-Jul-2016 for publish feature


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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
