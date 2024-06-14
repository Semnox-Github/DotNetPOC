/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - TableAttributeValidationDTO 
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
    /// <summary>
    /// TableAttributeValidationDTO
    /// </summary>
    public class TableAttributeValidationDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int tableAttributeValidationId;
        private int tableAttributeSetupId;
        private string dataValidationRule;
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
            /// Search By attributeEnabledTableId
            /// </summary>
            ATTRIBUTE_ENABLED_VALIDATION_ID,

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
        /// TableAttributeValidationDTO constructor
        /// </summary>
        public TableAttributeValidationDTO()
        {
            log.LogMethodEntry();
            this.tableAttributeValidationId = -1;
            this.tableAttributeSetupId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// TableAttributeValidationDTO with  parameters
        /// </summary>
        /// <param name="tableAttributeValidationDTO"></param>
        public TableAttributeValidationDTO(int tableAttributeValidationId, int tableAttributeSetupId, string dataValidationRule, bool isActive):this()
        {
            log.LogMethodEntry(tableAttributeValidationId, tableAttributeSetupId, dataValidationRule, isActive);
            this.tableAttributeValidationId = tableAttributeValidationId;
            this.tableAttributeSetupId = tableAttributeSetupId;
            this.dataValidationRule = dataValidationRule;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// TableAttributeValidationDTO with all parameters
        /// </summary>
        /// <param name="tableAttributeValidationDTO"></param>
        public TableAttributeValidationDTO(int tableAttributeValidationId, int tableAttributeSetupId, string dataValidationRule, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId) : 
            this(tableAttributeValidationId, tableAttributeSetupId, dataValidationRule, isActive)
        {
            log.LogMethodEntry(tableAttributeValidationId, tableAttributeSetupId, dataValidationRule, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
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
        /// TableAttributeValidationDTO copy constructor
        /// </summary>
        /// <param name="tableAttributeValidationDTO"></param>
        public TableAttributeValidationDTO(TableAttributeValidationDTO tableAttributeValidationDTO)
            :this(tableAttributeValidationDTO.tableAttributeValidationId, tableAttributeValidationDTO.tableAttributeSetupId, tableAttributeValidationDTO.dataValidationRule,
                 tableAttributeValidationDTO.isActive, tableAttributeValidationDTO.createdBy, tableAttributeValidationDTO.creationDate, tableAttributeValidationDTO.lastUpdatedBy,
                 tableAttributeValidationDTO.lastUpdateDate, tableAttributeValidationDTO.guid, tableAttributeValidationDTO.siteId, tableAttributeValidationDTO.synchStatus,
                 tableAttributeValidationDTO.masterEntityId)
        {
            log.LogMethodEntry(tableAttributeValidationDTO); 
            log.LogMethodExit();
        }
        /// <summary>
        /// Get and Set method for TableAttributeValidationId
        /// </summary>
        public int TableAttributeValidationId
        {
            get
            {
                return tableAttributeValidationId;
            }
            set
            {
                this.IsChanged = true;
                tableAttributeValidationId = value;
            }
        }
        /// <summary>
        /// Get and Set method for TableAttributeSetupId
        /// </summary>
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
        /// <summary>
        /// Get and Set method for DataValidationRule
        /// </summary>
        public string DataValidationRule
        {
            get
            {
                return dataValidationRule;
            }
            set
            {
                this.IsChanged = true;
                dataValidationRule = value;
            }
        }
        /// <summary>
        /// Get and Set method for IsActive
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
                    return notifyingObjectIsChanged || tableAttributeValidationId < 0;
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
