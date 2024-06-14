/********************************************************************************************
 * Project Name - Category DTO
 * Description  - Data object of category DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Apr-2016   Krishnanand    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Category data object class. This acts as data holder for the Category business object
    /// </summary>
    public class CategoryDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByCategoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCategoryParameters
        {
            /// <summary>
            /// search by CATEGORY_ID field
            /// </summary>
            CATEGORY_ID = 0,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME = 1,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 2,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 3,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 4 //Added search parameter 16-May-2017
        }

        int categoryId;
        int parentCategoryId;
        string name;
        string isActive;
        string lastUpdatedUserId;
        int siteId;
        string guid;
        bool synchStatus;
        int masterEntityId;
        
        /// <summary>
        /// Default Contructor
        /// </summary>
        public CategoryDTO()
        {
            log.Debug("Starts-CategoryDTO() default constructor.");
            categoryId = -1;
            parentCategoryId = -1;
            isActive = "Y";
            siteId = -1;
            masterEntityId = -1;
            log.Debug("Ends-CategoryDTO() default constructor.");
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="parentCategoryId"></param>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <param name="lastUpdatedUserId"></param>
        /// <param name="siteId"></param>
        /// <param name="guid"></param>
        /// <param name="synchStatus"></param>
        /// <param name="masterEntityId"></param>
        public CategoryDTO(int categoryId, int parentCategoryId, string name,
                           string isActive, string lastUpdatedUserId, int siteId,
                           string guid, bool synchStatus, int masterEntityId)
        {
            log.Debug("Starts-CategoryDTO(with all the data fields) Parameterized constructor.");
            this.categoryId = categoryId;
            this.parentCategoryId = parentCategoryId;
            this.name = name;
            this.isActive = isActive;
            this.lastUpdatedUserId = lastUpdatedUserId;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.Debug("Ends-CategoryDTO(with all the data fields) Parameterized constructor.");
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
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
        public int Site_Id { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

    }
}
