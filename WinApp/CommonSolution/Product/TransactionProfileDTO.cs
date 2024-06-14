/********************************************************************************************
 * Project Name - TransactionProfile DTO
 * Description  - Data object of TransactionProfile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        1-Dec-2017   Lakshminarayana          Created
 *2.60        17-Mar-2019   Jagan Mohana Rao      Modified - Added TransactionProfileTaxRulesDTO List.
 *2.110.00    07-Dec-2020   Prajwal S       Updated Three Tier
 ********************************************************************************************/
//using Semnox.Core.SortableBindingList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the TransactionProfile data object class. This acts as data holder for the TransactionProfile business object
    /// </summary>
    public class TransactionProfileDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TrxProfileId field
            /// </summary>
            TRANSACTION_PROFILE_ID,
            /// <summary>
            /// Search by ProfileName field
            /// </summary>
            PROFILE_NAME,
            /// <summary>
            /// Search by ActiveFlag field
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
            /// Search by verificationRequired field
            /// </summary>
            VERIFICATION_REQUIRED
        }

        int transactionProfileId;
        string profileName;
        private int priceListId;
        bool isActive;
        int siteId;
        int masterEntityId;
        bool synchStatus;
        string guid;
        bool verificationRequired;
        string createdBy;
        DateTime creationDate;
        string lastupdatedBy;
        DateTime lastupdateDate;
        List<TransactionProfileTaxRulesDTO> transactionProfileTaxRulesDTOs;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionProfileDTO()
        {
            log.LogMethodEntry();
            transactionProfileId = -1;
            masterEntityId = -1;
            priceListId = -1;
            isActive = true;
            transactionProfileTaxRulesDTOs = new List<TransactionProfileTaxRulesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public TransactionProfileDTO(int transactionProfileId, string profileName, bool verificationRequired, bool isActive)
            :this()
        {
            log.LogMethodEntry(transactionProfileId, profileName, verificationRequired, isActive);
            this.transactionProfileId = transactionProfileId;
            this.profileName = profileName;
            this.isActive = isActive;
            this.verificationRequired = verificationRequired;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionProfileDTO(int transactionProfileId, string profileName, bool isActive, int siteId,
                            int masterEntityId, bool synchStatus, string guid, bool verificationRequired, string createdBy, DateTime creationDate, string lastupdatedBy, DateTime lastupdateDate)
            :this(transactionProfileId, profileName, verificationRequired, isActive)
        {
            log.LogMethodEntry(transactionProfileId, profileName, isActive, siteId,
                             masterEntityId, synchStatus, guid, verificationRequired, createdBy, creationDate, lastupdatedBy, lastupdateDate);
            this.transactionProfileId = transactionProfileId;
            this.profileName = profileName;
            this.isActive = isActive;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.verificationRequired = verificationRequired;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastupdatedBy = lastupdatedBy;
            this.lastupdateDate = lastupdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TransactionProfileId field
        /// </summary>
        public int TransactionProfileId
        {
            get
            {
                return transactionProfileId;
            }

            set
            {
                this.IsChanged = true;
                transactionProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProfileName field
        /// </summary>
        public string ProfileName
        {
            get
            {
                return profileName;
            }

            set
            {
                this.IsChanged = true;
                profileName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the priceListId field
        /// </summary>
        public int PriceListId
        {
            get
            {
                return priceListId;
            }

            set
            {
                this.IsChanged = true;
                priceListId = value;
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
        /// Get method of the SiteId field
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
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }


        /// <summary>
        /// Get/Set method of the VerificationRequired field
        /// </summary> 
        public bool VerificationRequired
        {
            get
            {
                return verificationRequired;
            }

            set
            {
                this.IsChanged = true;
                verificationRequired = value;
            }
        }


        /// <summary>
        /// Get method of the CreatedBy field
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
        /// Get method of the CreationDate field
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastupdatedBy;
            }
            set
            {
                lastupdatedBy = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastupdateDate;
            }
            set
            {
               lastupdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set methods for TransactionProfileTaxRulesDTOList 
        /// </summary>
        public List<TransactionProfileTaxRulesDTO> TransactionProfileTaxRulesDTOList
        {
            get
            {
                return transactionProfileTaxRulesDTOs;
            }

            set
            {
                transactionProfileTaxRulesDTOs = value;
            }
        }

        /// <summary>
        /// Returns whether the taxDTO changed or any of its TransactionProfileTaxRules  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (transactionProfileTaxRulesDTOs != null &&
                   transactionProfileTaxRulesDTOs.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || transactionProfileId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

        /// <summary>
        /// Returns a string that represents the current TransactionProfileDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.Debug("Starts-ToString() method.");
            StringBuilder returnValue = new StringBuilder("\n-----------------------TransactionProfileDTO-----------------------------\n");
            returnValue.Append(" TransactionProfileId : " + TransactionProfileId);
            returnValue.Append(" ProfileName : " + ProfileName);
            
            
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.Debug("Ends-ToString() Method");
            return returnValue.ToString();

        }
    }
}
