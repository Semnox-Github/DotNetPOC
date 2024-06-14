/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - EnabledAttibutesDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0     16-Aug-2021    Fiona         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class EnabledAttributesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int enabledAttibuteId;
        private string tableName;
        private string recordGuid;
        private string enabledAttributeName;
        private IsMandatoryOrOptional mandatoryOrOptional;
        private string defaultValue;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By enabledAttibuteId
            /// </summary>
            ENABLED_ATTRIBUTE_ID,
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
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search By RECORD_GUID
            /// </summary>
            RECORD_GUID,
            /// <summary>
            /// Search By ENABLED_ATTRIBUTE_NAME
            /// </summary>
            ENABLED_ATTRIBUTE_NAME,
            /// <summary>
            /// Search By TABLE_NAME
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search By MANDATORY_OR_OPTIONAL
            /// </summary>
            MANDATORY_OR_OPTIONAL
        }
        public enum IsMandatoryOrOptional
        {
            /// <summary>
            /// YES
            /// </summary>
            Mandatory,
            /// <summary>
            /// NO
            /// </summary>
            Optional,

        }
        public static string IsMandatoryOrOptionalToString(IsMandatoryOrOptional value)
        {
            log.LogMethodEntry(value);
            string returnValue = "O";
            switch (value)
            {
                case IsMandatoryOrOptional.Mandatory:
                    returnValue = "M";
                    break;
                case IsMandatoryOrOptional.Optional:
                    returnValue = "O";
                    break;
                default:
                    log.LogMethodExit();
                    returnValue = "O";
                    break;
            }
            return returnValue;
        }
        public static IsMandatoryOrOptional IsMandatoryOrOptionalFromString(string deliveryStatus)
        {
            log.LogMethodEntry(deliveryStatus);
            IsMandatoryOrOptional returnValue = IsMandatoryOrOptional.Optional;
            if (string.IsNullOrWhiteSpace(deliveryStatus) == false)
            {
                switch (deliveryStatus)
                {
                    case "M":
                        returnValue = IsMandatoryOrOptional.Mandatory;
                        break;
                    case "O":
                        returnValue = IsMandatoryOrOptional.Optional;
                        break;
                    default:
                        returnValue = IsMandatoryOrOptional.Optional;
                        break;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public EnabledAttributesDTO()
        {
            log.LogMethodEntry();
            this.enabledAttibuteId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        public EnabledAttributesDTO(int enabledAttibuteId, string tableName, string recordGuid, string enabledAttributeName, IsMandatoryOrOptional mandatoryOrOptional, string defaultValue, 
            bool isActive) : this()
        {
            log.LogMethodEntry(enabledAttibuteId, tableName, recordGuid, enabledAttributeName, mandatoryOrOptional, defaultValue, isActive);
            this.enabledAttibuteId = enabledAttibuteId;
            this.tableName = tableName;
            this.recordGuid = recordGuid;
            this.enabledAttributeName = enabledAttributeName;
            this.mandatoryOrOptional = mandatoryOrOptional;
            this.defaultValue = defaultValue;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        public EnabledAttributesDTO(int enabledAttibuteId, string tableName, string recordGuid, string enabledAttributeName, IsMandatoryOrOptional mandatoryOrOptional, string defaultValue, 
            bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId) :
            this(enabledAttibuteId, tableName, recordGuid, enabledAttributeName, mandatoryOrOptional, defaultValue, isActive)
        {
            log.LogMethodEntry(enabledAttibuteId, tableName, recordGuid, enabledAttributeName, mandatoryOrOptional, defaultValue, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
        }
        public EnabledAttributesDTO(EnabledAttributesDTO enabledAttibutesDTO)
            : this(enabledAttibutesDTO.enabledAttibuteId,
                  enabledAttibutesDTO.tableName,
                  enabledAttibutesDTO.recordGuid,
                  enabledAttibutesDTO.enabledAttributeName,
                  enabledAttibutesDTO.mandatoryOrOptional,
                  enabledAttibutesDTO.defaultValue,
                  enabledAttibutesDTO.isActive,
                  enabledAttibutesDTO.createdBy,
                  enabledAttibutesDTO.creationDate,
                  enabledAttibutesDTO.lastUpdatedBy,
                  enabledAttibutesDTO.lastUpdateDate,
                  enabledAttibutesDTO.guid,
                  enabledAttibutesDTO.siteId,
                  enabledAttibutesDTO.synchStatus,
                  enabledAttibutesDTO.masterEntityId)
        {
            log.LogMethodEntry(enabledAttibutesDTO);
            log.LogMethodExit();
        }

        public int EnabledAttibuteId
        {
            get
            {
                return enabledAttibuteId;
            }
            set
            {
                enabledAttibuteId = value;
                this.IsChanged = true;
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
                tableName = value;
                this.IsChanged = true;
            }
        }
        public string RecordGuid
        {
            get
            {
                return recordGuid;
            }
            set
            {
                recordGuid = value;
                this.IsChanged = true;
            }
        }
        public string EnabledAttributeName
        {
            get
            {
                return enabledAttributeName;
            }
            set
            {
                enabledAttributeName = value;
                this.IsChanged = true;
            }
        }
        public IsMandatoryOrOptional MandatoryOrOptional
        {
            get
            {
                return mandatoryOrOptional;
            }
            set
            {
                mandatoryOrOptional = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the DefaultValue field
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
                this.IsChanged = true;
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


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || enabledAttibuteId < 0;
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
        /// <summary>
        /// TableWithEnabledAttributes
        /// </summary>
        public enum TableWithEnabledAttributes
        {
            /// <summary>
            /// None
            /// </summary>
            NONE,
            /// <summary>
            /// PaymentMode
            /// </summary>
            PaymentMode
        }
        /// <summary>
        /// TableWithEnabledAttributesToString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TableWithEnabledAttributesToString(TableWithEnabledAttributes value)
        {
            log.LogMethodEntry(value);
            string returnValue = TableWithEnabledAttributes.NONE.ToString();
            switch (value)
            {
                case TableWithEnabledAttributes.PaymentMode:
                    returnValue = TableWithEnabledAttributes.PaymentMode.ToString();
                    break;
                default:
                    returnValue = TableWithEnabledAttributes.NONE.ToString();
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// TableWithEnabledAttributesFromString
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static TableWithEnabledAttributes TableWithEnabledAttributesFromString(string sourceType)
        {
            log.LogMethodEntry(sourceType);
            TableWithEnabledAttributes returnValue = TableWithEnabledAttributes.NONE;
            if (string.IsNullOrWhiteSpace(sourceType) == false)
            {
                switch (sourceType)
                {
                    case "PaymentMode":
                        returnValue = TableWithEnabledAttributes.PaymentMode;
                        break;
                    default:
                        returnValue = TableWithEnabledAttributes.NONE;
                        break;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// TableWithEnabledAttributesFromString
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public static AttributeEnabledTablesDTO.AttributeEnabledTableNames GetAttributeEnabledTable(TableWithEnabledAttributes enabledTable)
        {
            log.LogMethodEntry(enabledTable);
            AttributeEnabledTablesDTO.AttributeEnabledTableNames returnValue = AttributeEnabledTablesDTO.AttributeEnabledTableNames.NONE;
            switch (enabledTable.ToString())
            {
                case "PaymentMode":
                    returnValue = AttributeEnabledTablesDTO.AttributeEnabledTableNames.TrxPayments;
                    break;
                default:
                    returnValue = AttributeEnabledTablesDTO.AttributeEnabledTableNames.NONE;
                    break;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
