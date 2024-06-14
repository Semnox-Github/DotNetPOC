/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - AttributeEnabledTablesDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0     13-Aug-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int attributeEnabledTableId;
        private string tableName;
        private string description;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<TableAttributeSetupDTO> tableAttributeSetupDTOList;
        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By attributeEnabledTableId
            /// </summary>
            ATTRIBUTE_ENABLED_TABLE_ID,

            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Base constructor for AttributeEnabledTablesDTO
        /// </summary>
        public AttributeEnabledTablesDTO()
        {
            log.LogMethodEntry();
            this.attributeEnabledTableId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            tableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with parameters
        /// </summary> 
        public AttributeEnabledTablesDTO(int attributeEnabledTableId, string tableName, string description, bool isActive) : this()
        {
            log.LogMethodEntry(attributeEnabledTableId, tableName, description, isActive);
            this.attributeEnabledTableId = attributeEnabledTableId;
            this.tableName = tableName;
            this.description = description;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all parameters
        /// </summary> 
        public AttributeEnabledTablesDTO(int attributeEnabledTableId, string tableName, string description, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId) 
            : this(attributeEnabledTableId, tableName, description, isActive)
        {
            log.LogMethodEntry(attributeEnabledTableId,  tableName,  description,  isActive,  createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate,  guid,  siteId,  synchStatus,  masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="attributeEnabledTablesDTO"></param>
        public AttributeEnabledTablesDTO(AttributeEnabledTablesDTO attributeEnabledTablesDTO)
            : this(attributeEnabledTablesDTO.attributeEnabledTableId,
            attributeEnabledTablesDTO.tableName,
            attributeEnabledTablesDTO.description,
            attributeEnabledTablesDTO.isActive,
            attributeEnabledTablesDTO.createdBy,
            attributeEnabledTablesDTO.creationDate,
            attributeEnabledTablesDTO.lastUpdatedBy,
            attributeEnabledTablesDTO.lastUpdateDate,
            attributeEnabledTablesDTO.guid,
            attributeEnabledTablesDTO.siteId,
            attributeEnabledTablesDTO.synchStatus,
            attributeEnabledTablesDTO.masterEntityId)
        {
            log.LogMethodEntry();
            if (attributeEnabledTablesDTO.TableAttributeSetupDTOList != null && attributeEnabledTablesDTO.TableAttributeSetupDTOList.Any())
            {
                if (this.tableAttributeSetupDTOList == null)
                {
                    this.tableAttributeSetupDTOList = new List<TableAttributeSetupDTO>();
                }
                for (int i = 0; i < attributeEnabledTablesDTO.TableAttributeSetupDTOList.Count; i++)
                {
                    this.tableAttributeSetupDTOList.Add(new TableAttributeSetupDTO(attributeEnabledTablesDTO.TableAttributeSetupDTOList[i]));
                }
            }
            log.LogMethodExit();
        }
        public int AttributeEnabledTableId
        {
            get
            {
                return attributeEnabledTableId;
            }
            set
            {
                this.IsChanged = true;
                attributeEnabledTableId = value;
            }
        }       
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                this.IsChanged = true;
                tableName = value;
            }
        }
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
        public List<TableAttributeSetupDTO> TableAttributeSetupDTOList
        {
            get
            {
                return tableAttributeSetupDTOList;
            }
            set
            {
                tableAttributeSetupDTOList = value;
            }
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
                    return notifyingObjectIsChanged || attributeEnabledTableId < 0;
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
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (tableAttributeSetupDTOList != null
                    && tableAttributeSetupDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
        public enum AttributeEnabledTableNames
        {
            /// <summary>
            /// None
            /// </summary>
            NONE,
            /// <summary>
            /// TrxPayments
            /// </summary>
            TrxPayments
        }
        /// <summary>
        /// AttributeEnabledTableNamesToString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AttributeEnabledTableNamesToString(AttributeEnabledTableNames value)
        {
            log.LogMethodEntry(value);
            string returnValue = AttributeEnabledTableNames.NONE.ToString();
            switch (value)
            {
                case AttributeEnabledTableNames.TrxPayments:
                    returnValue = AttributeEnabledTableNames.TrxPayments.ToString();
                    break; 
                default:
                    returnValue = AttributeEnabledTableNames.NONE.ToString();
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// AttributeEnabledTableNamesFromString
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static AttributeEnabledTableNames AttributeEnabledTableNamesFromString(string sourceType)
        {
            log.LogMethodEntry(sourceType);
            AttributeEnabledTableNames returnValue = AttributeEnabledTableNames.NONE;
            switch (sourceType)
            {
                case "TrxPayments":
                    returnValue = AttributeEnabledTableNames.TrxPayments;
                    break; 
                default:
                    returnValue = AttributeEnabledTableNames.NONE;
                    break;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}
