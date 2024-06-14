/********************************************************************************************
 * Project Name - ManagementFormsDTO
 * Description  - Data object of ManagementForms
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        15-Oct-2019   Jagan Mohana           Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class ManagementFormsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  MANAGEMENT_FORM_ID field
            /// </summary>
            MANAGEMENT_FORM_ID,
            /// <summary>
            /// Search by  FUNCTION_GROUP field
            /// </summary>
            FUNCTION_GROUP,
            /// <summary>
            /// Search by  GROUP_NAME field
            /// </summary>
            GROUP_NAME,
            /// <summary>
            /// Search by  FORM_NAME field
            /// </summary>
            FORM_NAME,
            /// <summary>
            /// Search by  ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int managementFormId;
        private string functionGroup;
        private string groupName;
        private string formName;
        private string formLookupTable;
        private string fontImageIcon;
        private string formTargetPath;
        private bool enableAccess;
        private int displayOrder;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default Constructor
        /// </summary>
        public ManagementFormsDTO()
        {
            log.LogMethodEntry();
            managementFormId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public ManagementFormsDTO(int managementFormId, string functionGroup, string groupName, string formName, string formLookupTable, string fontImageIcon,
                             string formTargetPath, bool enableAccess, int displayOrder, string guid, int siteId, bool synchStatus, int masterEntityId,
                             string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive)
        {
            log.LogMethodEntry(managementFormId, functionGroup, groupName, formName, formLookupTable, fontImageIcon, formTargetPath, enableAccess, displayOrder, guid,
                siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, isActive);
            this.managementFormId = managementFormId;
            this.functionGroup = functionGroup;
            this.groupName = groupName;
            this.formName = formName;
            this.formLookupTable = formLookupTable;
            this.fontImageIcon = fontImageIcon;
            this.formTargetPath = formTargetPath;
            this.enableAccess = enableAccess;
            this.displayOrder = displayOrder;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the ManagementFormId  field
        /// </summary>
        public int ManagementFormId
        {
            get { return managementFormId; }
            set { managementFormId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the FunctionGroup  field
        /// </summary>
        public string FunctionGroup
        {
            get { return functionGroup; }
            set { functionGroup = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the GroupName  field
        /// </summary>
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FormName  field
        /// </summary>
        public string FormName
        {
            get { return formName; }
            set { formName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FormLookupTable field
        /// </summary>
        public string FormLookupTable
        {
            get { return formLookupTable; }
            set { formLookupTable = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FontImageIcon field
        /// </summary>
        public string FontImageIcon
        {
            get { return fontImageIcon; }
            set { fontImageIcon = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FormTargetPath field
        /// </summary>
        public string FormTargetPath
        {
            get { return fontImageIcon; }
            set { fontImageIcon = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EnableAccess field
        /// </summary>
        public bool EnableAccess
        {
            get { return enableAccess; }
            set { enableAccess = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
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
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EnableAccess field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || managementFormId < 0;
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