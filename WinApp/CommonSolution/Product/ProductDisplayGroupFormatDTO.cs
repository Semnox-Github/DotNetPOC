
/********************************************************************************************
 * Project Name - DisplayGroup
 * Description  - Data object of the DisplayGroup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-May-2016    Amaresh          Created 
 ********************************************************************************************** 
 *2.60       14-Feb-2019   Nagesh Badiger    Added IsActive property as boolean
             17-May-2019   Mushahid Faizan   Added <ProductsDisplayGroupDTO> List.
 *2.130.0    02-Feb-2022   Fiona Lishal      Added MasterEntityId property
 *2.130.11   13-Oct-2022   Vignesh Bhat      Added BackgroundImageFileName property
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductDisplayGroupFormatDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByDisplayParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByDisplayParameters
        {
            /// <summary>
            /// Search by Display Group field 
            /// </summary>
            DISPLAY_GROUP,
            /// <summary>
            /// Search by Display Group format Id list field
            /// </summary>
            DISPLAY_GROUP_FORMAT_ID_LIST,
            /// <summary>
            /// Search by Display Group Id field
            /// </summary>
            DISPLAY_GROUP_ID,
            /// <summary>
            /// Search by POS Machine Id field
            /// </summary>
            POS_MACHINE_ID, 
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by SORT ORDER field
            /// </summary>
            SORT_ORDER,
            /// <summary>
            /// Search by IS_ACTIVE
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by EXTERNAL_REFERENCE 
            /// </summary>
            EXTERNAL_REFERENCE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID 
            /// </summary>
            MASTER_ENTITY_ID
        }

        int id;
        string displayGroup;
        string buttonColor;
        string textColor;
        string font;
        int sortOrder;
        string guid;
        bool synchStatus;
        int siteId;
        string imageFileName;
        DateTime lastUpdatedDate;
        string lastUpdatedBy;
        string description;
        bool isActive;
        private List<ProductsDisplayGroupDTO> productDisplayGroupList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private string externalSourceReference;
        private int masterEntityId;
        private string backgroundImageFileName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductDisplayGroupFormatDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            id = -1;
            sortOrder = 0;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductDisplayGroupFormatDTO(int id, string displayGroup, string buttonColor, string textColor, string font, int sortOrder,
                                     string guid, bool synchStatus, int siteId, string imageFileName, string lastUpdatedBy, DateTime lastUpdatedDate,
                                     string description, bool isActive, string externalSourceReference, int masterEntityId, string backgroundImageFileName)
        {
            log.LogMethodEntry(id, displayGroup, buttonColor, textColor, font, sortOrder, guid, synchStatus, siteId,
                               imageFileName, lastUpdatedBy, lastUpdatedDate, description, isActive, externalSourceReference, masterEntityId);
            this.id = id;
            this.displayGroup = displayGroup;
            this.buttonColor = buttonColor;
            this.textColor = textColor;
            this.font = font;
            this.sortOrder = sortOrder;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.imageFileName = imageFileName;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.description = description;
            this.isActive = isActive;
            this.externalSourceReference = externalSourceReference;
            this.masterEntityId = masterEntityId;
            this.backgroundImageFileName = backgroundImageFileName;
            productDisplayGroupList = new List<ProductsDisplayGroupDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the displayGroup
        /// </summary>
        [DisplayName("Display Group")]
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ButtonColor field
        /// </summary>
        [DisplayName("ButtonColor")]
        public string ButtonColor { get { return buttonColor; } set { buttonColor = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the TextColor field
        /// </summary>
        [DisplayName("TextColor")]
        public string TextColor { get { return textColor; } set { textColor = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        [DisplayName("Font")]
        public string Font { get { return font; } set { font = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("SortOrder")]
        public int SortOrder { get { return sortOrder; } set { sortOrder = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [Browsable(false)]
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [DisplayName("ImageFileName")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the backgroundImageFileName field
        /// </summary>
        [DisplayName("BackgroundImageFileName")]
        public string BackgroundImageFileName { get { return backgroundImageFileName; } set { backgroundImageFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("lastUpdatedBy")]
        [ReadOnly(true)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("lastUpdatedDate")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        public string ExternalSourceReference { get { return externalSourceReference; } set { externalSourceReference = value; this.IsChanged = true; } }

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
        /// Get/Set method of the ProductDisplayGroupList field
        /// </summary>
        [Browsable(false)]
        public List<ProductsDisplayGroupDTO> ProductDisplayGroupList
        {
            get
            {
                return productDisplayGroupList;
            }

            set
            {
                productDisplayGroupList = value;
            }
        }
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

        /// <summary>
        /// Returns whether the ProductDisplayFormatDTO changed or any of its ProductsDisplayGroupList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (productDisplayGroupList != null &&
                    productDisplayGroupList.Any(x => x.IsChanged))
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
            log.LogMethodExit();
        }
    }
}
