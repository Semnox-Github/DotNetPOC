/*/********************************************************************************************
 * Project Name - LegacyCardDiscountsDTO
 * Description  - Data Object File for LegacyCardDiscountsDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    /// <summary>
    /// This is the LegacyCardDiscountsDTO data object class. This acts as data holder for the LegacyCardCreditPlus business objects
    /// </summary>
    public class LegacyCardDiscountsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search By LegacyCardCreditPlus enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Legacy Card Discount Id field
            /// </summary>
            LEGACY_CARD_DISCOUNT_ID,
            /// <summary>
            /// Search by Legacy card id field
            /// </summary>
            LEGACY_CARD_ID,
            /// <summary>
            /// Search by Discount name field
            /// </summary>
            DISCOUNT_NAME,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Card ID LIST 
            /// </summary>
            CARD_ID_LIST,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int legacyCardDiscountId;
        private int legacycard_id;
        private string discount_name;
        private DateTime? expiryDate;
        private bool isActive;
        private string createdBy;
        private DateTime? creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdatedDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of LegacyCardDiscountsDTO with required fields
        /// </summary>
        public LegacyCardDiscountsDTO()
        {
            log.LogMethodEntry();
            legacyCardDiscountId = -1;
            legacycard_id = -1;
            site_id = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardDiscountsDTO with the required fields
        /// </summary>
        public LegacyCardDiscountsDTO(int legacyCardDiscountId, int legacycard_id, string discount_name, DateTime? expiryDate)
            : this()
        {
            log.LogMethodEntry(legacyCardDiscountId, legacycard_id, discount_name, expiryDate);
            this.legacyCardDiscountId = legacyCardDiscountId;
            this.legacycard_id = legacycard_id;
            this.discount_name = discount_name;
            this.expiryDate = expiryDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardDiscountsDTO with all fields
        /// </summary>
        public LegacyCardDiscountsDTO(int legacyCardDiscountId, int legacycard_id, string discount_name, DateTime? expiryDate, string createdBy, DateTime? creationDate, string lastUpdatedBy, bool isActive, DateTime lastupdatedDate,
                                  int site_id, string guid, bool synchStatus, int masterEntityId)
        : this(legacyCardDiscountId, legacycard_id, discount_name, expiryDate)
        {
            log.LogMethodEntry(lastupdatedDate, site_id, lastUpdatedBy, guid, synchStatus, masterEntityId, isActive);
            this.lastupdatedDate = lastupdatedDate;
            this.site_id = site_id;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the legacyCardDiscountId field
        /// </summary>
        public int LegacyCardDiscountId { get { return legacyCardDiscountId; } set { legacyCardDiscountId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Legacycard_id field
        /// </summary>
        public int Legacycard_id { get { return legacycard_id; } set { legacycard_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Discount name field
        /// </summary>
        public string Discount_name { get { return discount_name; } set { discount_name = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_id { get { return site_id; } set { site_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }
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
                    return notifyingObjectIsChanged || LegacyCardDiscountId < 0;
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
