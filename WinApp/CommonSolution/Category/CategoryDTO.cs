/********************************************************************************************
 * Project Name - Category
 * Description  - Data object of category DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Krishnanand    Created
 *2.60        20-Mar-2019   Akshay G       Modified isActive DataType from string to bool 
 *2.60.2      23-Apr-2019   Lakshmi        WHO column updates 
 *2.70        02-Jul-2019   Dakshakh raj   Modified : (Added Parameterized costrustor) 
 *2.110.0     09-Oct-2020   Mushahid Faizan   Added List<AccountingCodeCombinationDTO>, CategoryExcelDTODefinition class for excel sheet functionality & modified IsChanged property.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Category
{
    /// <summary>
    /// This is the Category data object class. This acts as data holder for the Category business object
    /// </summary>
    public class CategoryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByCategoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCategoryParameters
        {
            /// <summary>
            /// search by CATEGORY ID field
            /// </summary>
            CATEGORY_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,//Added search parameter 16-May-2017
             /// <summary>
             /// Search by LAST UPDATED DATE field
             /// </summary>
            LAST_UPDATED_DATE ,
            CATEGORY_ID_LIST
        }

        private int categoryId;
        private int parentCategoryId;
        private string name;
        private bool isActive;
        private string lastUpdatedUserId;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;

        private List<AccountingCodeCombinationDTO> accountingCodeCombinationDTOList;

        /// <summary>
        /// Default Contructor
        /// </summary>
        public CategoryDTO()
        {
            log.LogMethodEntry();
            categoryId = -1;
            parentCategoryId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            accountingCodeCombinationDTOList = new List<AccountingCodeCombinationDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public CategoryDTO(int categoryId, int parentCategoryId, string name, bool isActive)
            : this()
        {
            log.LogMethodEntry(categoryId, parentCategoryId, name, isActive);
            this.categoryId = categoryId;
            this.parentCategoryId = parentCategoryId;
            this.name = name;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>

        public CategoryDTO(int categoryId, int parentCategoryId, string name,
                           bool isActive, string lastUpdatedUserId, int siteId,
                           string guid, bool synchStatus, int masterEntityId,
                           string createdBy, DateTime creationDate,
                           DateTime lastUpdateDate)
            : this(categoryId, parentCategoryId, name, isActive)
        {
            log.LogMethodEntry(categoryId, parentCategoryId, name, isActive, lastUpdatedUserId, siteId,
                               guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdateDate);

            this.lastUpdatedUserId = lastUpdatedUserId;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>

        [DisplayName("Id")]
        [ReadOnly(true)]
        public int CategoryId { get { return categoryId; } set { categoryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParentCategoryId field
        /// </summary>

        [DisplayName("Parent Category")]
        //[Browsable(false)]
        public int ParentCategoryId { get { return parentCategoryId; } set { parentCategoryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>

        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>

        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("Last Updated UserId")]
        [Browsable(false)]
        public string LastUpdatedUserId { get { return lastUpdatedUserId; } set { lastUpdatedUserId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_Id field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int Site_Id { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
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

        public List<AccountingCodeCombinationDTO> AccountingCodeCombinationDTOList { get { return accountingCodeCombinationDTOList; } set { accountingCodeCombinationDTOList = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || CategoryId < 0;
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
        ///Returns whether the DTO changed or any of its accountingCodeCombinationDTOList childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (accountingCodeCombinationDTOList != null &&
                   accountingCodeCombinationDTOList.Any(x => x.IsChanged))
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
            IsChanged = false;
            log.LogMethodExit();
        }

    }
}
