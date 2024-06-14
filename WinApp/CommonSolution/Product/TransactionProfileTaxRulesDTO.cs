/********************************************************************************************
 * Project Name - Transaction Profile Tax Rules DTO
 * Description  - DTO of TransactionProfileTaxRulesDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        14-Mar-2016   Jagan Mohana      Created
 *2.70        05-Apr-2019   Mushahid Faizan   Added IsActive, LogMethodEntry & LogMethodExit,removed enum numbering & unused namespaces.
2.110.00    08-Dec-2020   Prajwal S       Updated Three Tier 
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class TransactionProfileTaxRulesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by TRX_PROFILE_ID field
            /// </summary>
            TRX_PROFILE_ID,
            /// <summary>
            /// Search by TAX_ID field
            /// </summary>
            TAX_ID,
            /// <summary>
            /// Search by TAX_STRUCTURE_ID field
            /// </summary>
            TAX_STRUCTURE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IsActive Field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        int id;
        int trxProfileId;
        int taxId;
        int taxStructureId;
        string exempt;
        int siteId;
        string guid;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionProfileTaxRulesDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.trxProfileId = -1;
            this.taxId = -1;
            this.taxStructureId = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public TransactionProfileTaxRulesDTO(int id, int trxProfileId, int taxId, int taxStructureId, string exempt, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, trxProfileId, taxId, taxStructureId, exempt, isActive);
            this.id = id;
            this.trxProfileId = trxProfileId;
            this.taxId = taxId;
            this.taxStructureId = taxStructureId;
            this.exempt = exempt;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public TransactionProfileTaxRulesDTO(int id, int trxProfileId, int taxId, int taxStructureId, string exempt, int siteId, string guid,
                                           bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate,
                                           string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            : this(id, trxProfileId, taxId, taxStructureId, exempt, isActive)
        {
            log.LogMethodEntry(id, trxProfileId, taxId, taxStructureId, exempt, siteId, guid, synchStatus, masterEntityId, createdBy, CreationDate, lastUpdatedBy, LastUpdateDate, isActive);
            this.id = id;
            this.trxProfileId = trxProfileId;
            this.taxId = taxId;
            this.taxStructureId = taxStructureId;
            this.exempt = exempt;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TrxProfileId field
        /// </summary>
        public int TrxProfileId { get { return trxProfileId; } set { trxProfileId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        public int TaxId { get { return taxId; } set { taxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TaxStructureId field
        /// </summary>
        public int TaxStructure { get { return taxStructureId; } set { taxStructureId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Exempt field
        /// </summary>
        public string Exempt { get { return exempt; } set { exempt = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value;} }

        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;} }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;} }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;} }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;} }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary> 
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value;} }
        
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0 ;
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