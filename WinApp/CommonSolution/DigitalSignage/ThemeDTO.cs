/********************************************************************************************
 * Project Name - Theme DTO
 * Description  - Data object of Theme
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2017   Lakshminarayana          Created 
 *2.70.2      31-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 *2.70.3      21-Dec-2019   Archana                  Modified to add themeNameWithThemeNumber get property
 *                                                   and TYPE_ID_LIST search parameter 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the Theme data object class. This acts as data holder for the Theme business object
    /// </summary>
    public class ThemeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  id field
            /// </summary>
            ID,
            /// <summary>
            /// Search by TypeId field
            /// </summary>
            TYPE_ID,
            /// <summary>
            /// Search by TypeList field
            /// </summary>
            TYPE_LIST,
            /// <summary>
            /// Search by Name field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Type Id list field
            /// </summary>
            TYPE_ID_LIST
        }

        private string name;
        private int typeId;
        private string description;
        private int initialScreenId;
        private int themeNumber;
        private int id;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private SortableBindingList<ScreenTransitionsDTO> screenTransitionsDTOList;
        private List<ScreenSetupDTO> screenSetupDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ThemeDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            screenTransitionsDTOList = new SortableBindingList<ScreenTransitionsDTO>();
            initialScreenId = -1;
            typeId = -1;
            siteId = -1;
            screenSetupDTOList = new List<ScreenSetupDTO>();
        log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ThemeDTO(int id, string name, int typeId, string description, int initialScreenId, int themeNumber, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, name, typeId, description, initialScreenId, themeNumber, isActive);
            this.name = name;
            this.typeId = typeId;
            this.description = description;
            this.initialScreenId = initialScreenId;
            this.themeNumber = themeNumber;
            this.id = id;
            this.isActive = isActive;
            screenTransitionsDTOList = new SortableBindingList<ScreenTransitionsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ThemeDTO(int id, string name, int typeId, string description, int initialScreenId, int themeNumber,
                        bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                        DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(id, name, typeId, description, initialScreenId, themeNumber, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Theme Name")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.IsChanged = true;
                name = value;
            }
        }

        public String ThemeDetails
        {
            get
            {
                return name + " [" + themeNumber + "]";
            }
        }

        /// <summary>
        /// Get/Set method of the TypeId field
        /// </summary>
        [DisplayName("Theme Type")]
        public int TypeId
        {
            get
            {
                return typeId;
            }

            set
            {
                this.IsChanged = true;
                typeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the InitialScreenId field
        /// </summary>
        [DisplayName("Initial Screen")]
        public int InitialScreenId
        {
            get
            {
                return initialScreenId;
            }

            set
            {
                this.IsChanged = true;
                initialScreenId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the themeNumber field
        /// </summary>
        [DisplayName("Theme Number")]
        public int ThemeNumber
        {
            get
            {
                return themeNumber;
            }

            set
            {
                this.IsChanged = true;
                themeNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.IsChanged = true;
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
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
            set
            {
                siteId = value;
            }
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

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
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
            set
            {
                synchStatus = value;
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
                this.IsChanged = true;
                guid = value;
            }
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
                    return notifyingObjectIsChanged || id < 0;
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
        /// Get/Set method of the ScreenTransitionsDTOList field
        /// </summary>
        [Browsable(false)]
        public SortableBindingList<ScreenTransitionsDTO> ScreenTransitionsDTOList
        {
            get
            {
                return screenTransitionsDTOList;
            }

            set
            {
                screenTransitionsDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ScreenSetupDTOList field
        /// </summary>
        [Browsable(false)]
        public List<ScreenSetupDTO> ScreenSetupDTOList
        {
            get
            {
                return screenSetupDTOList;
            }

            set
            {
                screenSetupDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ThemeNameWithThemeNumber field
        /// </summary>
        [DisplayName("ThemeNameWithThemeNumber")]
        public string ThemeNameWithThemeNumber
        {
            get
            {
                return ((string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name)) ? " " : Name + "[" + ThemeNumber + "]");
            }
            set { }     
        }

        /// <summary>
        /// Returns whether child DTO's are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (screenTransitionsDTOList != null &&
                   screenTransitionsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (screenSetupDTOList != null &&
                  screenSetupDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns a string that represents the current ThemeDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------ThemeDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" Name : " + Name);
            returnValue.Append(" TypeId : " + TypeId);
            returnValue.Append(" Description : " + Description);
            returnValue.Append(" InitialScreenId : " + InitialScreenId);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();

        }
    }
}
