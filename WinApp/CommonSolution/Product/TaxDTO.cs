/********************************************************************************************
 * Project Name -Tax DTO
 * Description  - Data object of asset tax
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       07-Jan-2016   Raghuveera          Created 
 ********************************************************************************************
 *2.60       11-Apr-2019   Girish Kundar       Modified : Added  fields notifyingObjectIsChanged,
 *                                              and  notifyingObjectIsChangedSyncRoot
 *2.70       30-Jan-2019   Mushahid Faizan     Modified -- Added IsChanged() and AcceptChanges() Method & TaxStructureDTO List.
 *2.70.2       21-Nov-2019   Girish Kundar       Modified -- Added IsChangedRecursive() method .
 *2.110.0     06-Oct-2020    Mushahid Faizan   Web Inventory UI changes.
  *2.130.0     21-May-2021   Girish Kundar   Modified: Added Attribue1  to 5 columns to the table as part of Xero integration
 *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the  Tax data object class. This acts as data holder for the  Tax business object
    /// </summary>
    public class TaxDTO
    {
        private int taxId; 
        private string taxName;
        private double taxPercentage;
        private bool activeFlag;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int masterEntityId;
        private List<TaxStructureDTO> taxStructureDTOList;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTaxParameters
        {
            /// <summary>
            /// Search by TAX_ID field
            /// </summary>
            TAX_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by TAX_NAME field
            /// </summary>
            TAX_NAME,
            /// <summary>
            /// Search by TAX_PERCENTAGE field
            /// </summary>
            TAX_PERCENTAGE,
            /// <summary>
            /// Search by TAX_NAME_EXACT field
            /// </summary>
            TAX_NAME_EXACT,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaxDTO()
        {
            log.LogMethodEntry();
            taxId = -1;
            siteId = -1;
            masterEntityId = -1;
            taxStructureDTOList = new List<TaxStructureDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the required fields
        /// </summary>
        public TaxDTO(int taxId, string taxName, double taxPercentage, bool activeFlag, string attribute1, 
                      string attribute2, string attribute3, string attribute4, string attribute5)
            : this()

        {
            log.LogMethodEntry(taxId, taxName, taxPercentage, activeFlag, attribute1, attribute2, attribute3, attribute4, attribute5);
            this.taxId = taxId;
            this.taxName = taxName;
            this.taxPercentage = taxPercentage;
            this.activeFlag = activeFlag;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TaxDTO(int taxId, string taxName, double taxPercentage, bool activeFlag,
                            string guid, int siteId, bool synchStatus, int masterEntityId,
                             string attribute1, string attribute2, string attribute3, string attribute4, string attribute5)
            : this(taxId, taxName, taxPercentage, activeFlag, attribute1, attribute2, attribute3, attribute4, attribute5)
        {
            log.LogMethodEntry(taxId, taxName, taxPercentage, activeFlag, guid, siteId, synchStatus, masterEntityId);
            this.activeFlag = activeFlag;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Tax Id field
        /// </summary>
        [ReadOnly(true)]
        public int TaxId { get { return taxId; } set { taxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Tax Name field
        /// </summary>        
        public string TaxName { get { return taxName; } set { taxName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active Flag field
        /// </summary>
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
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
            set { lastUpdatedBy = value; }
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
            set { lastUpdateDate = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1
        {
            get { return attribute1; }
            set { this.IsChanged = true; attribute1 = value; }
        }

        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2
        {
            get { return attribute2; }
            set { this.IsChanged = true; attribute2 = value; }
        }

        /// <summary>
        /// Attribute3
        /// </summary>
        public string Attribute3
        {
            get { return attribute3; }
            set { this.IsChanged = true; attribute3 = value; }
        }

        /// <summary>
        /// Attribute4
        /// </summary>
        public string Attribute4
        {
            get { return attribute4; }
            set { this.IsChanged = true; attribute4 = value; }
        }

        /// <summary>
        /// Attribute5
        /// </summary>
        public string Attribute5
        {
            get { return attribute5; }
            set { this.IsChanged = true; attribute5 = value; }
        }



        /// <summary>
        /// Get/Set method of the TaxStructureDTOList field
        /// </summary>
        public List<TaxStructureDTO> TaxStructureDTOList { get { return taxStructureDTOList; } set { this.IsChanged = true; taxStructureDTOList = value; } }
      
        
        
        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || taxId < 0;
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
        /// Returns whether the taxDTO changed or any of its taxStructureDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (taxStructureDTOList != null &&
                   taxStructureDTOList.Any(x => x.IsChanged))
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
    }
}
