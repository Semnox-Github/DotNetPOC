/********************************************************************************************
 * Project Name - ProductModifiers DTO
 * Description  - Data object of ProductModifiers
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created 
 *2.110.00    01-Dec-2020      Abhishek            Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Product
{
    public class ProductModifiersDTO

    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by productModifierId
            /// </summary>
            PRODUCT_MODIFIER_ID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by ProductId
            ///</summary>
            PRODUCT_ID,
            ///<summary>
            ///Search by ModifierSetId
            ///</summary>
            MODIFIER_SET_ID,
            /// <summary>
            /// Search by Product id list
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }

        private int productModifierId;
        private int categoryId;
        private int productId;
        private int modifierSetId;
        private string autoShowinPOS;
        private int sortOrder;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private ModifierSetDTO modifierSetDTO;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductModifiersDTO()
        {
            log.LogMethodEntry();
            productModifierId = -1;
            categoryId = -1;
            productId = -1;
            modifierSetId = -1;
            autoShowinPOS = "Y";
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ProductModifiersDTO(int productModifierId, int categoryId, int productId, int modifierSetId, string autoShowinPOS,
                                   int sortOrder, bool isActive)
            : this()
        {
            log.LogMethodEntry(productModifierId, categoryId, productId, modifierSetId, autoShowinPOS, sortOrder, isActive);
            this.productModifierId = productModifierId;
            this.categoryId = categoryId;
            this.productId = productId;
            this.modifierSetId = modifierSetId;
            this.autoShowinPOS = autoShowinPOS;
            this.sortOrder = sortOrder;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ProductModifiersDTO(int productModifierId,int categoryId,int productId,int modifierSetId,
                                    string autoShowinPOS,int sortOrder,bool isActive,DateTime creationDate,
                                    string createdBy,DateTime lastUpdateDate,string lastUpdatedBy,int site_id,
                                    string guid,bool synchStatus,int masterEntityId) 
            :this(productModifierId, categoryId, productId, modifierSetId, autoShowinPOS, sortOrder, isActive)
        {
            log.LogMethodEntry(productModifierId,categoryId,productId,modifierSetId, autoShowinPOS,sortOrder,isActive,
                               creationDate,createdBy,lastUpdateDate, lastUpdatedBy,site_id,guid,synchStatus,masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int ProductModifierId { get { return productModifierId; } set { this.IsChanged = true; productModifierId = value; } }
       
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }
       
        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        public int CategoryId { get { return categoryId; } set { this.IsChanged = true; categoryId = value; } }
       
        /// <summary>
        /// Get/Set method of the ModifierSetId field
        /// </summary>
        public int ModifierSetId { get { return modifierSetId; } set { this.IsChanged = true; modifierSetId = value; } }
       
        /// <summary>
        /// Get/Set method of the AutoShowinPOS field
        /// </summary>
        public string AutoShowinPOS { get { return autoShowinPOS; } set { this.IsChanged = true; autoShowinPOS = value; } }
       
        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        public int SortOrder { get { return sortOrder; } set { this.IsChanged = true; sortOrder = value; } }
      
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
       
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
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }
        
        /// <summary>
        /// Get/Set method of the ModifierSetDTO field
        /// </summary>
        public ModifierSetDTO ModifierSetDTO { get { return modifierSetDTO; } set { this.IsChanged = true; modifierSetDTO = value; } }
       
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productModifierId < 0;
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
