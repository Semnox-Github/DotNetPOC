/********************************************************************************************
 * Project Name - ModifierSetDetails DTO
 * Description  - Data object of ModifierSetDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created
 *2.60        05-Mar-2019      Muhammed Mehraj      Added parentModifierDTOList as a child
              21-Mar-2019      Akshay Gulaganji     Modified isActive (from string to bool) 
  *2.110.00    27-Nov-2020      Abhishek                Modified : Modified to 3 Tier Standard              
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ModifierSetDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;
        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by ModifierSetDetailId
            /// </summary>
            MODIFIER_SET_DETAIL_ID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by ModifierSetId
            ///</summary>
            MODIFIER_SET_ID,
            ///<summary>
            ///Search by ModifierProductId
            ///</summary>
            MODIFIER_PRODUCT_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by GUID
            /// </summary>
            GUID,
            /// <summary>
            /// Search by MODIFIER_SET_ID_LIST
            /// </summary>
            MODIFIER_SET_ID_LIST
        }

        private int id;
        private int modifierSetId;
        private int modifierProductId;
        private double price;
        private int parentId;
        private bool isActive;
        private int sortOrder;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedUser;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private List<ParentModifierDTO> parentModifierDTOList;
        //ProductsDTO modifierProductDTO;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ModifierSetDetailsDTO()
        {
            log.LogMethodEntry();
            id = -1;
            modifierSetId = -1;
            modifierProductId = -1;
            parentId = -1;
            isActive = true;
            masterEntityId = -1;
            site_id = -1;
            parentModifierDTOList = new List<ParentModifierDTO>();
            log.LogMethodExit( );
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ModifierSetDetailsDTO(int id, int modifierSetId, int modifierProductId, double price, int parentId, int sortOrder,
                                     bool isActive)
            : this()
        {
            log.LogMethodEntry(id, modifierSetId, modifierProductId, price, parentId, sortOrder, isActive);
            this.id = id;
            this.ModifierSetId = modifierSetId;
            this.modifierSetId = modifierSetId;
            this.modifierProductId = modifierProductId;
            this.price = price;
            this.parentId = parentId;
            this.sortOrder = sortOrder;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifierSetId"></param>
        /// <param name="modifierProductId"></param>
        /// <param name="price"></param>
        /// <param name="parentId"></param>
        /// <param name="isActive"></param>
        /// <param name="creationDate"></param>
        /// <param name="createdBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="lastUpdatedUser"></param>
        /// <param name="site_id"></param>
        /// <param name="guid"></param>
        /// <param name="synchStatus"></param>
        /// <param name="masterEntityId"></param>
        public ModifierSetDetailsDTO(int id, int modifierSetId, int modifierProductId, double price, int parentId, int sortOrder,
                                   bool isActive, DateTime creationDate, string createdBy, DateTime lastUpdateDate,
                                   string lastUpdatedUser, int site_id, string guid, bool synchStatus, int masterEntityId)
          : this(id, modifierSetId, modifierProductId, price, parentId, sortOrder, isActive)
        {
            log.LogMethodEntry(id, modifierSetId, modifierProductId, price, parentId, isActive, creationDate, createdBy,
                               lastUpdateDate, lastUpdatedUser, site_id, guid, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.parentModifierDTOList = new List<ParentModifierDTO>();
            log.LogMethodExit( );
        }

        public ModifierSetDetailsDTO(ModifierSetDetailsDTO modifierSetDetailsDTO)
            : this(modifierSetDetailsDTO.id, modifierSetDetailsDTO.modifierSetId, modifierSetDetailsDTO.modifierProductId, modifierSetDetailsDTO.price, modifierSetDetailsDTO.parentId, modifierSetDetailsDTO.sortOrder,
                                 modifierSetDetailsDTO.isActive, modifierSetDetailsDTO.creationDate, modifierSetDetailsDTO.createdBy, modifierSetDetailsDTO.lastUpdateDate,
                                 modifierSetDetailsDTO.lastUpdatedUser, modifierSetDetailsDTO.site_id, modifierSetDetailsDTO.guid, modifierSetDetailsDTO.synchStatus, modifierSetDetailsDTO.masterEntityId)
        {
            log.LogMethodEntry(modifierSetDetailsDTO);
            if(modifierSetDetailsDTO.ParentModifierDTOList != null)
            {
                parentModifierDTOList = new List<ParentModifierDTO>();
                foreach (var item in modifierSetDetailsDTO.parentModifierDTOList)
                {
                    ParentModifierDTO copy = new ParentModifierDTO(item);
                    parentModifierDTOList.Add(copy);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int ModifierSetDetailId
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
        /// Get/Set method of the modifierSetId field
        /// </summary>
        [DisplayName("modifierSetId")]
        public int ModifierSetId
        {
            get
            {
                return modifierSetId;
            }

            set
            {
                this.IsChanged = true;
                modifierSetId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the modifierProductId field
        /// </summary>
        [DisplayName("modifierProductId")]
        public int ModifierProductId
        {
            get
            {
                return modifierProductId;
            }

            set
            {
                this.IsChanged = true;
                modifierProductId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        [DisplayName("price")]
        public double Price
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
        /// Get/Set method of the parentId field
        /// </summary>
        [DisplayName("parentId")]
        public int ParentId
        {
            get
            {
                return parentId;
            }

            set
            {
                this.IsChanged = true;
                parentId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("Sort Order")]
        public int SortOrder
        {
            get
            {
                return sortOrder;
            }

            set
            {
                this.IsChanged = true;
                sortOrder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the isActive field
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
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
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        public string LastUpdatedUser
        {
            get
            {
                return lastUpdatedUser;
            }
            set
            {
                lastUpdatedUser = value;
            }
        }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_Id
        {
            get
            {
                return site_id;
            }
            set
            {
                site_id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string GUID
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
        /// Get/Set method of the synchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the masterEntityId field
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
        /// Get/Set method of the ParentModifierDTO field
        /// </summary>
        [Browsable(false)]
        public List<ParentModifierDTO> ParentModifierDTOList
        {
            get
            {
                return parentModifierDTOList;
            }

            set
            {
                this.IsChanged = true;
                parentModifierDTOList = value;
            }
        }

        ///// <summary>
        ///// Get/Set method of the ModifierProductDTO field
        ///// </summary>
        //[Browsable(false)]
        //public ProductsDTO ModifierProductDTO
        //{
        //    get
        //    {
        //        return modifierProductDTO;
        //    }

        //    set
        //    {
        //        modifierProductDTO = value;
        //    }
        //}
        

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

        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (parentModifierDTOList != null &&
                    parentModifierDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit( );
        }
    }
}
