/********************************************************************************************
 * Project Name - ParentModifierDTO
 * Description  - Populates Parent modifiers grid in product modifiers form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 *2.60       18-Feb-2019      Mehraj/Guru S A        3 tier class creation
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ParentModifierDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID
            /// </summary>
            ID,
            /// <summary>
            /// Search by PARENTMODIFIERID
            /// </summary>
            PARENTMODIFIERID,
            /// <summary>
            /// Search by MODIFIERID
            /// </summary>
            MODIFIERID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by masterEntity_id
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private int modifierId;
        private int parentModifierId;
        private string parentModifierProductName;
        private double? price;
        private bool isActive;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ParentModifierDTO()
        {
            log.LogMethodEntry();
            id = -1;
            siteId = -1;
            parentModifierId = -1;
            modifierId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ParentModifierDTO(int id, int modifierId, int parentModifierId, double? price, bool isActive,string parentModifierProductName)
            : this()
        {
            log.LogMethodEntry(id, modifierId, parentModifierId, price, isActive);
            this.id = id;
            this.modifierId = modifierId;
            this.parentModifierId = parentModifierId;
            this.price = price;
            this.isActive = isActive;
            this.parentModifierProductName = parentModifierProductName;
            log.LogMethodExit();
        }

        public ParentModifierDTO(int id, int modifierid, int parentmodifiersid, string parentModifierProductName, double? price, bool isActive,string lastUpdatedBy, DateTime lastUpdateDate, string guid, bool synchStatus, int site_id,
                                 int masterEntityId, string createdBy, DateTime creationDate)
            :this(id,modifierid,parentmodifiersid,price,isActive,parentModifierProductName)
        {
            log.LogMethodEntry(id, modifierid, parentmodifiersid, parentModifierProductName, price, lastUpdatedBy, lastUpdateDate, guid, synchStatus, site_id, masterEntityId, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = site_id;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }


        public ParentModifierDTO(ParentModifierDTO parentModifierDTO)
            : this(parentModifierDTO.id, parentModifierDTO.modifierId, parentModifierDTO.parentModifierId, parentModifierDTO.parentModifierProductName, parentModifierDTO.price, parentModifierDTO.isActive, parentModifierDTO.lastUpdatedBy, parentModifierDTO.lastUpdateDate, parentModifierDTO.guid, parentModifierDTO.synchStatus, parentModifierDTO.siteId,
                                parentModifierDTO.masterEntityId, parentModifierDTO.createdBy, parentModifierDTO.creationDate)
        {
            log.LogMethodEntry(parentModifierDTO);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the ModifierId field
        /// </summary>
        [DisplayName("modifierId")]
        public int ModifierId
        {
            get
            {
                return modifierId;
            }
            set
            {
                this.IsChanged = true;
                modifierId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the ParentModifierId field
        /// </summary>
        [DisplayName("parentModifierId")]
        public int ParentModifierId
        {
            get
            {
                return parentModifierId;
            }
            set
            {
                this.IsChanged = true;
                parentModifierId = value;
            }
        }



        /// <summary>
        /// Get/Set method of the parentModifierProductName field
        /// </summary>
        public string ParentModifierProductName
        {
            get
            {
                return parentModifierProductName;
            }
            set
            {
                parentModifierProductName = value;
            }
        }



        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("price")]
        public double? Price
        {
            get
            {
                return price;
            }
            set
            {
                this.IsChanged = true;
                price = value;
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
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }


        /// <summary>
        /// Get/Set method of the LastUpdateDate field
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
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the synchStatus field
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
        /// Get/Set method of the masterEntityId field
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
                masterEntityId = value;
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
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the creationDate field
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
        /// Get/Set method of the SiteId field
        /// </summary>      

        public int SiteId { get { return siteId; } set { this.IsChanged = true; siteId = value; } }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>      
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
