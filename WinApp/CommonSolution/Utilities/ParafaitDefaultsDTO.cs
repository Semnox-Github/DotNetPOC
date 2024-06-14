/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data object of ParafaitDefaults
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        16-Mar-2017   Lakshminarayana     Created
 *2.60        29-Apr-2019   Mushahid Faizan     Modified IsActive from string to bool datatype 
 *2.70        3- Jul- 2019  Girish Kundar       Modified : Added Constructor with required Parameter
3.00         24-Aug-2020    Girish Kundar       Modified : POS UI Redesign with REST API  -  Added  List<ParafaitOptionValuesDTO> as child 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// This is the ParafaitDefaults data object class. This acts as data holder for the ParafaitDefaults business object
    /// </summary>
    public class ParafaitDefaultsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  defaultValueId field
            /// </summary>
            DEFAULT_VALUE_ID ,
            /// <summary>
            /// Search by defaultValueName field
            /// </summary>
            DEFAULT_VALUE_NAME ,
            /// <summary>
            /// Search by defaultValueName list field
            /// </summary>
            DEFAULT_VALUE_NAME_LIST,
            /// <summary>
            /// Search by screenGroup field
            /// </summary>
            SCREEN_GROUP ,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ACTIVE_FLAG ,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

       private int defaultValueId;
       private string defaultValueName;
       private string description;
       private string defaultValue;
       private string screenGroup;
       private int dataTypeId;
       private string userLevel;
       private string pOSLevel;
       private string isProtected;
       private bool isActive;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private int siteId;
       private int masterEntityId;
       private bool synchStatus;
       private string guid;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ParafaitDefaultsDTO()
        {
            log.LogMethodEntry();
            defaultValueId = -1;
            masterEntityId = -1;
            isActive = true;
            dataTypeId = -1;
            siteId = -1;
            parafaitOptionValuesDTOList = new List<ParafaitOptionValuesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ParafaitDefaultsDTO(int defaultValueId, string defaultValueName, string description, string defaultValue,
                                   string screenGroup, int datatypeId, string userLevel, string pOSLevel, string isProtected,
                                   bool isActive)
            :this()
        {
            log.LogMethodEntry( defaultValueId,  defaultValueName,  description,  defaultValue,
                                screenGroup,  datatypeId,  userLevel,  pOSLevel,  isProtected,isActive);
            this.defaultValueId = defaultValueId;
            this.defaultValueName = defaultValueName;
            this.description = description;
            this.defaultValue = defaultValue;
            this.screenGroup = screenGroup;
            this.dataTypeId = datatypeId;
            this.userLevel = userLevel;
            this.pOSLevel = pOSLevel;
            this.isProtected = isProtected;
            this.isActive = isActive;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ParafaitDefaultsDTO(int defaultValueId, string defaultValueName, string description, string defaultValue,
                                   string screenGroup, int datatypeId, string userLevel, string pOSLevel, string isProtected,
                                   bool isActive, string lastUpdatedBy, DateTime lastUpdatedDate,int siteId, int masterEntityId,
                                   bool synchStatus, string guid,string createdBy, DateTime creationDate )
            :this(defaultValueId, defaultValueName, description, defaultValue, screenGroup, datatypeId, userLevel, pOSLevel,
                  isProtected, isActive)
        {
            log.LogMethodEntry(defaultValueId, defaultValueName, description, defaultValue, screenGroup, datatypeId, userLevel, pOSLevel, isProtected, 
                               isActive , lastUpdatedBy, lastUpdatedDate,siteId, masterEntityId, synchStatus, guid, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DefaultValueId field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int DefaultValueId
        {
            get
            {
                return defaultValueId;
            }

            set
            {
                defaultValueId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DefaultValueName field
        /// </summary>
        [DisplayName("Name")]
        public string DefaultValueName
        {
            get
            {
                return defaultValueName;
            }

            set
            {
                defaultValueName = value;
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
        /// Get/Set method of the DefaultValue field
        /// </summary>
        [DisplayName("Default Value")]
        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }

            set
            {
                defaultValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ScreenGroup field
        /// </summary>
        [DisplayName("Screen Group")]
        public string ScreenGroup
        {
            get
            {
                return screenGroup;
            }

            set
            {
                screenGroup = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DataTypeId field
        /// </summary>
        [DisplayName("DataTypeId")]
        public int DataTypeId
        {
            get
            {
                return dataTypeId;
            }

            set
            {
                dataTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UserLevel field
        /// </summary>
        [DisplayName("UserLevel")]
        public string UserLevel
        {
            get
            {
                return userLevel;
            }

            set
            {
                userLevel = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSLevel field
        /// </summary>
        [DisplayName("POSLevel")]
        public string POSLevel
        {
            get
            {
                return pOSLevel;
            }

            set
            {
                pOSLevel = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsProtected field
        /// </summary>
        [DisplayName("IsProtected")]
        public string IsProtected
        {
            get
            {
                return isProtected;
            }

            set
            {
                isProtected = value;
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
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
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        public List<ParafaitOptionValuesDTO> ParafaitOptionValuesDTOList
        {
            get { return parafaitOptionValuesDTOList; }
            set { parafaitOptionValuesDTOList = value; }
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
                    return notifyingObjectIsChanged || defaultValueId < 0;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current ParafaitDefaultsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------ParafaitDefaultsDTO-----------------------------\n");
            returnValue.Append(" DefaultValueId : " + DefaultValueId);
            returnValue.Append(" DefaultValueName : " + DefaultValueName);
            returnValue.Append(" Description : " + Description);
            returnValue.Append(" DefaultValue : " + DefaultValue);
            returnValue.Append(" ScreenGroup : " + ScreenGroup);
            returnValue.Append(" DataTypeId : " + DataTypeId);
            returnValue.Append(" UserLevel : " + UserLevel);
            returnValue.Append(" POSLevel : " + POSLevel);
            returnValue.Append(" IsProtected : " + IsProtected);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit();
            return returnValue.ToString();

        }
    }
}
