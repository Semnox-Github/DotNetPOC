/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - TableAttributeSetupDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0     23-Aug-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Semnox.Parafait.TableAttributeSetup
{
    public class TableAttributeSetupDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int tableAttributeSetupId;
        private int attributeEnabledTableId;
        private string columnName;
        private string displayName;
        private DataSourceTypeEnum dataSourceType;
        private DataTypeEnum dataType;
        private int lookupId;
        private string sQLSource;
        private string sQLDisplayMember;
        private string sQLValueMember;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<TableAttributeValidationDTO> tableAttributeValidationDTOList; 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By tableAttributeSetupId
            /// </summary>
            TABLE_ATTRIBUTE_SETUP_ID,

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
        public TableAttributeSetupDTO()
        {
            log.LogMethodEntry();
            this.tableAttributeSetupId = -1;
            this.attributeEnabledTableId = -1;
            this.lookupId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            tableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
            log.LogMethodExit();
        }
        public TableAttributeSetupDTO(int tableAttributeSetupId, int attributeEnabledTableId, string columnName, string displayName, DataSourceTypeEnum dataSourceType, DataTypeEnum dataType, int lookupId, string sQLSource, string sQLDisplayMember, string sQLValueMember, bool isActive) : this()
        {
            log.LogMethodEntry(tableAttributeSetupId,  attributeEnabledTableId,  columnName,  displayName,  dataSourceType,  dataType,  lookupId,  sQLSource,  sQLDisplayMember,  sQLValueMember,  isActive, tableAttributeSetupId,  attributeEnabledTableId,  columnName,  displayName,  dataSourceType,  dataType,  lookupId,  sQLSource,  sQLDisplayMember,  sQLValueMember,  isActive);
            this.tableAttributeSetupId = tableAttributeSetupId;
            this.attributeEnabledTableId = attributeEnabledTableId;
            this.columnName = columnName;
            this.displayName = displayName;
            this.dataSourceType = dataSourceType;
            this.dataType = dataType;
            this.lookupId = lookupId;
            this.sQLSource = sQLSource;
            this.sQLDisplayMember = sQLDisplayMember;
            this.sQLValueMember = sQLValueMember;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        public TableAttributeSetupDTO(int tableAttributeSetupId, int attributeEnabledTableId, string columnName, string displayName, DataSourceTypeEnum dataSourceType, DataTypeEnum dataType, int lookupId, string sQLSource, string sQLDisplayMember, string sQLValueMember, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId) : 
            this(tableAttributeSetupId, attributeEnabledTableId, columnName, displayName, dataSourceType, dataType, lookupId, sQLSource, sQLDisplayMember, sQLValueMember, isActive)
        {
            log.LogMethodEntry(tableAttributeSetupId, attributeEnabledTableId, columnName, displayName, dataSourceType, dataType, lookupId, sQLSource, sQLDisplayMember, sQLValueMember, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
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
        public TableAttributeSetupDTO(TableAttributeSetupDTO tableAttributeSetupDTO)
            :this (tableAttributeSetupDTO.tableAttributeSetupId,
                 tableAttributeSetupDTO.attributeEnabledTableId,
                 tableAttributeSetupDTO.columnName,
                 tableAttributeSetupDTO.displayName,
                 tableAttributeSetupDTO.dataSourceType,
                 tableAttributeSetupDTO.dataType,
                 tableAttributeSetupDTO.lookupId,
                 tableAttributeSetupDTO.sQLSource,
                 tableAttributeSetupDTO.sQLDisplayMember,
                 tableAttributeSetupDTO.sQLValueMember,
                 tableAttributeSetupDTO.isActive,
                 tableAttributeSetupDTO.createdBy,
                 tableAttributeSetupDTO.creationDate,
                 tableAttributeSetupDTO.lastUpdatedBy,
                 tableAttributeSetupDTO.lastUpdateDate,
                 tableAttributeSetupDTO.guid,
                 tableAttributeSetupDTO.siteId,
                 tableAttributeSetupDTO.synchStatus,
                 tableAttributeSetupDTO.masterEntityId)
        {
            log.LogMethodEntry();
            if(tableAttributeSetupDTO.tableAttributeValidationDTOList!=null && tableAttributeSetupDTO.TableAttributeValidationDTOList.Any())
            {
                if(tableAttributeValidationDTOList==null)
                {
                    tableAttributeValidationDTOList = new List<TableAttributeValidationDTO>();
                }
                for (int i = 0; i < tableAttributeSetupDTO.TableAttributeValidationDTOList.Count; i++)
                {
                    this.tableAttributeValidationDTOList.Add(new TableAttributeValidationDTO(tableAttributeSetupDTO.TableAttributeValidationDTOList[i]));
                }
            }
            log.LogMethodExit();
        }

        public int TableAttributeSetupId
        {
            get
            {
                return tableAttributeSetupId;
            }
            set
            {
                this.IsChanged = true;
                tableAttributeSetupId = value;
            }
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
        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                this.IsChanged = true;
                columnName = value;
            }
        }
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                this.IsChanged = true;
                displayName = value;
            }
        }
        public DataSourceTypeEnum DataSourceType
        {
            get
            {
                return dataSourceType;
            }
            set
            {
                this.IsChanged = true;
                dataSourceType = value;
            }
        }
        public DataTypeEnum DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                this.IsChanged = true;
                dataType = value;
            }
        }
        public int LookupId
        {
            get
            {
                return lookupId;
            }
            set
            {
                this.IsChanged = true;
                lookupId = value;
            }
        }
        public string SQLSource
        {
            get
            {
                return sQLSource;
            }
            set
            {
                this.IsChanged = true;
                sQLSource = value;
            }
        }
        public string SQLDisplayMember
        {
            get
            {
                return sQLDisplayMember;
            }
            set
            {
                this.IsChanged = true;
                sQLDisplayMember = value;
            }
        }
        public string SQLValueMember
        {
            get
            {
                return sQLValueMember;
            }
            set
            {
                this.IsChanged = true;
                sQLValueMember = value;
            }
        }
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
        public List<TableAttributeValidationDTO> TableAttributeValidationDTOList
        {
            get
            {
                return tableAttributeValidationDTOList;
            }
            set
            {
                tableAttributeValidationDTOList = value;
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
                    return notifyingObjectIsChanged || tableAttributeSetupId < 0;
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
                if(IsChanged)
                {
                    return true;
                }
                if(tableAttributeValidationDTOList!=null 
                    && tableAttributeValidationDTOList.Any(x=>x.IsChanged))
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
        /// <summary>
        /// DataSourceTypeEnum
        /// </summary>
        public enum DataSourceTypeEnum
        {
            /// <summary>
            /// NONE
            /// </summary>
            NONE,
            /// <summary>
            /// Constant
            /// </summary>
            CONSTANT,
            /// <summary>
            /// SQL
            /// </summary>
            SQL,
            /// <summary>
            /// LOOKUP
            /// </summary>
           LOOKUP
        }
        /// <summary>
        /// DataSourceTypeToString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DataSourceTypeToString(DataSourceTypeEnum value)
        {
            log.LogMethodEntry(value);
            string returnValue = DataSourceTypeEnum.NONE.ToString();
            switch (value)
            {
                case DataSourceTypeEnum.SQL:
                    returnValue = DataSourceTypeEnum.SQL.ToString();
                    break;
                case DataSourceTypeEnum.LOOKUP:
                    returnValue = DataSourceTypeEnum.LOOKUP.ToString();
                    break;
                case DataSourceTypeEnum.CONSTANT:
                    returnValue = DataSourceTypeEnum.CONSTANT.ToString();
                    break;
                default:
                    returnValue = DataSourceTypeEnum.NONE.ToString();
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// DataSourceTypeFromString
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static DataSourceTypeEnum DataSourceTypeFromString(string sourceType)
        {
            log.LogMethodEntry(sourceType);
            DataSourceTypeEnum returnValue = DataSourceTypeEnum.NONE;
            if (string.IsNullOrWhiteSpace(sourceType) == false)
            {
                switch (sourceType)
                {
                    case "SQL":
                        returnValue = DataSourceTypeEnum.SQL;
                        break;
                    case "LOOKUP":
                        returnValue = DataSourceTypeEnum.LOOKUP;
                        break;
                    case "CONSTANT":
                        returnValue = DataSourceTypeEnum.CONSTANT;
                        break;
                    default:
                        returnValue = DataSourceTypeEnum.NONE;
                        break;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// DataTypeEnum
        /// </summary>
        public enum DataTypeEnum
        {
            /// <summary>
            /// NONE
            /// </summary>
            NONE,
            /// <summary>
            /// TEXT
            /// </summary>
            TEXT,
            /// <summary>
            /// Number
            /// </summary>
            NUMBER,
            /// <summary>
            /// DATETIME
            /// </summary>
            DATETIME
        }
        /// <summary>
        /// DataTypeToString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DataTypeToString(DataTypeEnum value)
        {
            log.LogMethodEntry(value);
            string returnValue = DataTypeEnum.NONE.ToString();
            switch (value)
            {
                case DataTypeEnum.DATETIME:
                    returnValue = DataTypeEnum.DATETIME.ToString();
                    break;
                case DataTypeEnum.NUMBER:
                    returnValue = DataTypeEnum.NUMBER.ToString();
                    break;
                case DataTypeEnum.TEXT:
                    returnValue = DataTypeEnum.TEXT.ToString();
                    break;
                default:
                    returnValue = DataTypeEnum.NONE.ToString();
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// DataTypeFromString
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static DataTypeEnum DataTypeFromString(string dataType)
        {
            log.LogMethodEntry(dataType);
            DataTypeEnum returnValue = DataTypeEnum.NONE;
            if (string.IsNullOrWhiteSpace(dataType) == false)
            {
                switch (dataType.ToUpper())
                {
                    case "DATETIME":
                        returnValue = DataTypeEnum.DATETIME;
                        break;
                    case "NUMBER":
                        returnValue = DataTypeEnum.NUMBER;
                        break;
                    case "TEXT":
                        returnValue = DataTypeEnum.TEXT;
                        break;
                    default:
                        returnValue = DataTypeEnum.NONE;
                        break;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
