/********************************************************************************************
 * Project Name - ModifierSet DTO
 * Description  - Data object of ModifierSet
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created
 *2.60        26-Apr-2019      Akshay G            modified isActive dataType to bool(from string to bool) 
 *2.110.00    26-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    public class ModifierSetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   
            /// <summary>
            /// Search by modifierSetId
            /// </summary>
            MODIFIER_SET_ID,
            /// <summary>
            /// Search by modifierSetId
            /// </summary>
            PARENT_MODIFIER_ID,
            /// <summary>
            /// Search by modifierSetName
            /// </summary>
            MODIFIER_SET_NAME,
            /// <summary>
            /// Search by isactive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by modifierSetIdList
            /// </summary>
            MODIFIER_SET_ID_LIST,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID
        }

        private int modifierSetId;
        private string setName;
        private int minQuantity;
        private int maxQuantity;
        private int freeQuantity;
        private int parentModifierSetId;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedUser;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private List<ModifierSetDetailsDTO> modifierSetDetailsDTO;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ModifierSetDTO()
        {
            log.LogMethodEntry();
            modifierSetId = -1;
            parentModifierSetId = -1;
            isActive = true;
            masterEntityId = -1;
            site_id = -1;
            modifierSetDetailsDTO = new List<ModifierSetDetailsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ModifierSetDTO(int modifierSetId, string setName, int minQuantity, int maxQuantity, int freeQuantity,
                              int parentModifierSetId, bool isActive)
            : this()
        {
            log.LogMethodEntry(modifierSetId, setName, minQuantity, maxQuantity, freeQuantity, parentModifierSetId, isActive);
            this.ModifierSetId = modifierSetId;
            this.setName = setName;
            this.minQuantity = minQuantity;
            this.maxQuantity = maxQuantity;
            this.freeQuantity = freeQuantity;
            this.parentModifierSetId = parentModifierSetId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with all parameters
        /// </summary>
        public ModifierSetDTO(int modifierSetId, string setName, int minQuantity, int maxQuantity, int freeQuantity,
                              int parentModifierSetId, bool isActive,
                              DateTime creationDate, string createdBy, DateTime lastUpdateDate, string lastUpdatedUser,
                              int site_id, string guid, bool synchStatus, int masterEntityId)
            : this(modifierSetId, setName, minQuantity, maxQuantity, freeQuantity, parentModifierSetId, isActive)
        {
            log.LogMethodEntry(modifierSetId, setName, minQuantity, maxQuantity, freeQuantity, parentModifierSetId,
                               isActive, creationDate, createdBy, lastUpdateDate, lastUpdatedUser,
                               site_id, guid, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// COpy Constructor
        /// </summary>
        /// <param name="modifierSetDTO"></param>
        public ModifierSetDTO(ModifierSetDTO modifierSetDTO)
            :this(modifierSetDTO.modifierSetId, modifierSetDTO.setName, modifierSetDTO.minQuantity, modifierSetDTO.maxQuantity, modifierSetDTO.freeQuantity,
                              modifierSetDTO.parentModifierSetId, modifierSetDTO.isActive,
                              modifierSetDTO.creationDate, modifierSetDTO.createdBy, modifierSetDTO.lastUpdateDate, modifierSetDTO.lastUpdatedUser,
                              modifierSetDTO.site_id, modifierSetDTO.guid, modifierSetDTO.synchStatus, modifierSetDTO.masterEntityId)
        {
            log.LogMethodEntry(modifierSetDTO);
            if(modifierSetDTO.modifierSetDetailsDTO != null)
            {
                modifierSetDetailsDTO = new List<ModifierSetDetailsDTO>();
                foreach (var item in modifierSetDTO.modifierSetDetailsDTO)
                {
                    ModifierSetDetailsDTO copy = new ModifierSetDetailsDTO(item);
                    modifierSetDetailsDTO.Add(copy);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ModifierSetId field
        /// </summary>       
        public int ModifierSetId { get { return modifierSetId; } set { this.IsChanged = true; modifierSetId = value; } }

        /// <summary>
        /// Get/Set method of the setName field
        /// </summary>       
        public string SetName { get { return setName; } set { this.IsChanged = true; setName = value; } }

        /// <summary>
        /// Get/Set method of the minQuantity field
        /// </summary>        
        public int MinQuantity { get { return minQuantity; } set { this.IsChanged = true; minQuantity = value; } }

        /// <summary>
        /// Get/Set method of the ParentModifierSetId field
        /// </summary>    
        public int ParentModifierSetId { get { return parentModifierSetId; } set { this.IsChanged = true; parentModifierSetId = value; } }

        /// <summary>
        /// Get/Set method of the maxQuantity field
        /// </summary>       
        public int MaxQuantity { get { return maxQuantity; } set { this.IsChanged = true; maxQuantity = value; } }

        /// <summary>
        /// Get/Set method of the freeQuantity field
        /// </summary>       
        public int FreeQuantity { get { return freeQuantity; } set { this.IsChanged = true; freeQuantity = value; } }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>       
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>     
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_Id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string GUID { get { return guid; } set { this.IsChanged = true; guid = value; } }

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
        /// Get/Set methods for ModifierSetDetailsDTO 
        /// </summary>
        public List<ModifierSetDetailsDTO> ModifierSetDetailsDTO { get { return modifierSetDetailsDTO; } set { this.IsChanged = true; modifierSetDetailsDTO = value; } }

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
                    return notifyingObjectIsChanged || modifierSetId < 0;
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
                if (modifierSetDetailsDTO != null &&
                    modifierSetDetailsDTO.Any(x => x.IsChanged || x.IsChangedRecursive))
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
            log.LogMethodExit(null);
        }
    }

    /// <summary>
    ///  PurchasedModifierSet class. 
    /// </summary>
    public class PurchasedModifierSet : ModifierSetDTO
    {
        List<PurchasedProducts> purchasedProductsList;

        public PurchasedModifierSet()
        {

        }

        /// <summary>
        ///   Get/Set method of the PurchasedProductsList field
        /// </summary>
        public List<PurchasedProducts> PurchasedProductsList { get { return purchasedProductsList; } set { purchasedProductsList = value; } }
    }
}
