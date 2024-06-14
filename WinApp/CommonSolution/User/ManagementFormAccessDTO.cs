/********************************************************************************************
 * Project Name - ManagementFormAccess DTO
 * Description  - Data object of ManagementFormAccess
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019   Girish Kundar           Modified : Added Parametrized Constructor with required fields
 *                                                             and Missed Who columns
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.ComponentModel;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.User
{
    public enum ManagementFormAccessFunctionGroup 
    {
        ManagementStudio, // = "Management Studio",
        InventoryManagement, // = "Inventory Management",
        DataAccess,
        MetricDashboard,
        POSTaskAccess,
        AppsAccess,
        WebAccess,
        Reports
    }

    public enum ManagementFormAccessMainMenu
    {
        Reports,
        Sites,
        RedemptionCurrency,
        Products,
        DeploymentTool,
        Semnox,
        ManageDashboard,
        ParafaitPOS,
        Functions,
        POSMachine,
        SiteSetup,
        Exit,
        Maintenance,
        Achievement,
        FeedbackSurvey,
        Tools,
        HR,
        Cards,
        Games,
        ManageReports,
        POSCounter,
        DigitalSignage,
        RunReports,
        Promotions,
        SemnoxOnline,
        UserRoles,
        Help
    }


    public class ManagementFormAccessDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  managementFormAccessId field
            /// </summary>
            MANAGEMENT_FORM_ACCESS_ID,
            /// <summary>
            /// roleId
            /// </summary>
            ROLE_ID,
            /// <summary>
            /// mainMenu
            /// </summary>
            MAIN_MENU,
            /// <summary>
            /// formName Id
            /// </summary>
            FORM_NAME,
            /// <summary>
            /// functionGroup
            /// </summary>
            FUNCTION_GROUP,
            /// <summary>
            /// functionGuid
            /// </summary>
            FUNCTION_GUID,
            /// <summary>
            /// accessAllowed
            /// </summary>
            ACCESS_ALLOWED,
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// roleId
            /// </summary>
            ROLE_ID_LIST,
            /// <summary>
            /// Search by  ISACTIVE field
            /// </summary>
            ISACTIVE
        }

       private int managementFormAccessId;
       private int roleId;
       private string mainMenu;
       private string formName; 
       private bool accessAllowed;
       private int functionId;
       private string functionGroup;
       private string functionGUID;
       private int siteId;
       private int masterEntityId;
       private bool synchStatus;
       private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private bool isActive;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManagementFormAccessDTO()
        {
            log.LogMethodEntry();
            managementFormAccessId = -1;
            roleId = -1;
            functionId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required  data fields
        /// </summary>
        public ManagementFormAccessDTO(int managementFormAccessId, int roleId, string mainMenu, string formName, bool accessAllowed, int functionId,
                                       string functionGroup, string functionGUID, bool isActive)
            :this()
        {
            log.LogMethodEntry(managementFormAccessId, roleId, mainMenu, formName, accessAllowed,
                               functionId, functionGroup, functionGUID);
            this.managementFormAccessId = managementFormAccessId;
            this.roleId = roleId;
            this.mainMenu = mainMenu;
            this.formName = formName;
            this.accessAllowed = accessAllowed;
            this.functionId = functionId;
            this.functionGroup = functionGroup;
            this.functionGUID = functionGUID;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ManagementFormAccessDTO(int managementFormAccessId, int roleId, string mainMenu, string formName, bool accessAllowed, int functionId, 
                                       string functionGroup, string functionGUID, int siteId, int masterEntityId, bool synchStatus, string guid,
                                       string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            :this(managementFormAccessId, roleId, mainMenu, formName, accessAllowed, functionId, functionGroup, functionGUID, isActive)
        {
            log.LogMethodEntry(managementFormAccessId, roleId, mainMenu, formName, accessAllowed, 
                               functionId, functionGroup, functionGUID, siteId, masterEntityId, synchStatus, guid,
                               createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate, isActive);
          
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the managementFormAccessId field
        /// </summary>
        [DisplayName("ManagementFormAccessId")]
        public int ManagementFormAccessId
        {
            get { return managementFormAccessId; }
            set { this.IsChanged = true; managementFormAccessId = value; }
        }

        /// <summary>
        /// Get/Set method of the roleId field
        /// </summary>
        [DisplayName("RoleId")]
        public int RoleId
        {
            get { return roleId; }
            set { this.IsChanged = true; roleId = value; }
        }

        /// <summary>
        /// Get/Set method of the MainMenu field
        /// </summary>
        [DisplayName("MainMenu")]
        public string MainMenu
        {
            get { return mainMenu; }
            set { this.IsChanged = true; mainMenu = value; }
        }

        /// <summary>
        /// Get/Set method of the FormName field
        /// </summary>
        [DisplayName("FormName")]
        public string FormName
        {
            get { return formName; }
            set { this.IsChanged = true; formName = value; }
        }

        /// <summary>
        /// Get/Set method of the AccessAllowed field
        /// </summary>
        [DisplayName("AccessAllowed")]
        public bool AccessAllowed
        {
            get { return accessAllowed; }
            set { this.IsChanged = true; accessAllowed = value; }
        }

        /// <summary>
        /// Get/Set method of the FunctionId field
        /// </summary>
        [DisplayName("FunctionId")]
        public int FunctionId
        {
            get { return functionId; }
            set { this.IsChanged = true; functionId = value; }
        }

        /// <summary>
        /// Get/Set method of the FunctionGroup field
        /// </summary>
        [DisplayName("FunctionGroup")]
        public string FunctionGroup
        {
            get { return functionGroup; }
            set { this.IsChanged = true; functionGroup = value; }
        }

        /// <summary>
        /// Get/Set method of the FunctionGUID field
        /// </summary>
        [DisplayName("FunctionGUID")]
        public string FunctionGUID
        {
            get { return functionGUID; }
            set { this.IsChanged = true; functionGUID = value; }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive
        {
            get { return isActive; }
            set { this.IsChanged = true; isActive = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set { this.IsChanged = true; siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
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
                this.IsChanged = true; guid = value;
            }
        }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
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
                    return notifyingObjectIsChanged || managementFormAccessId < 0;
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
