/********************************************************************************************
 * Project Name - DashboardDefinitionDTO
 * Description  - Data object of DashboardDefinition
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
    public class DashboardDefinitionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DASHBOARD_DEF_ID field
            /// </summary>
            DASHBOARD_DEF_ID,
            /// <summary>
            /// Search by  DASHBOARD_NAME field
            /// </summary>
            DASHBOARD_NAME,
            /// <summary>
            /// Search by  DASHBOARD_SCREEN_TITLE field
            /// </summary>
            DASHBOARD_SCREEN_TITLE,
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
        private int dashboardDefId;
        private string dashboardName;
        private string dashboardScreenTitle;
        private string titleAlignment;
        private string titleFont;
        private int totalRows;
        private int totalCols;
        private DateTime effectiveDate;
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
        public DashboardDefinitionDTO()
        {
            log.LogMethodEntry();
            dashboardDefId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public DashboardDefinitionDTO(int dashboardDefId, string dashboardName, string dashboardScreenTitle, string titleAlignment, string titleFont, int totalRows,
                             int totalCols, DateTime effectiveDate, string guid, int siteId, bool synchStatus, int masterEntityId,
                             string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive)
        {
            log.LogMethodEntry(dashboardDefId, dashboardName, dashboardScreenTitle, titleAlignment, titleFont, totalRows, totalCols, effectiveDate, guid,
                siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, isActive);
            this.dashboardDefId = dashboardDefId;
            this.dashboardName = dashboardName;
            this.dashboardScreenTitle = dashboardScreenTitle;
            this.titleAlignment = titleAlignment;
            this.titleFont = titleFont;
            this.totalRows = totalRows;
            this.totalCols = totalCols;
            this.effectiveDate = effectiveDate;
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
        /// Get/Set method of the DashboardDefId  field
        /// </summary>
        public int DashboardDefId
        {
            get { return dashboardDefId; }
            set { dashboardDefId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the DashboardName  field
        /// </summary>
        public string DashboardName
        {
            get { return dashboardName; }
            set { dashboardName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DashboardScreenTitle  field
        /// </summary>
        public string DashboardScreenTitle
        {
            get { return dashboardScreenTitle; }
            set { dashboardScreenTitle = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TitleAlignment  field
        /// </summary>
        public string TitleAlignment
        {
            get { return titleAlignment; }
            set { titleAlignment = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TitleFont field
        /// </summary>
        public string TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TotalRows field
        /// </summary>
        public int TotalRows
        {
            get { return totalRows; }
            set { totalRows = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TotalCols field
        /// </summary>
        public int TotalCols
        {
            get { return totalCols; }
            set { totalCols = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || dashboardDefId < 0;
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