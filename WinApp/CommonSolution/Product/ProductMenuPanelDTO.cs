/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductMenuPanelParameters
        {   /// <summary>
            /// Search by PANEL_ID
            /// </summary>
            PANEL_ID,
            /// <summary>
            /// Search by PANEL_ID_LIST
            /// </summary>
            PANEL_ID_LIST,
            /// <summary>
            /// Search by NAME
            /// </summary>
            NAME,
            /// <summary>
            /// Search by GUID
            /// </summary>
            GUID,
            /// <summary>
            /// Search by Site Id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IsActive
            /// </summary>
            IS_ACTIVE,

        }

        private int panelId;
        private int columnCount;
        private int displayOrder;
        private string name;
        private int cellMarginLeft;
        private int cellMarginRight;
        private int cellMarginTop;
        private int cellMarginBottom;
        private int rowCount;
        private string imageURL;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList;

        public ProductMenuPanelDTO()
        {
            log.LogMethodEntry();
            panelId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            columnCount = 1;
            displayOrder = 0;
            cellMarginLeft = 0;
            cellMarginRight = 0;
            cellMarginTop = 0;
            cellMarginBottom = 0;
            rowCount = 0;
            productMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ProductMenuPanelDTO(int panelId, int columnCount, int displayOrder, string name, int cellMarginLeft, int cellMarginRight, int cellMarginTop, int cellMarginBottom, int rowCount, string imageURL, bool isActive)
            : this()
        {
            log.LogMethodEntry(panelId, columnCount, displayOrder, name, cellMarginLeft, cellMarginRight, cellMarginTop, cellMarginBottom, rowCount, imageURL, isActive);
            this.panelId = panelId;
            this.columnCount = columnCount;
            this.displayOrder = displayOrder;
            this.name = name;
            this.cellMarginLeft = cellMarginLeft;
            this.cellMarginRight = cellMarginRight;
            this.cellMarginTop = cellMarginTop;
            this.cellMarginBottom = cellMarginBottom;
            this.rowCount = rowCount;
            this.imageURL = imageURL;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductMenuPanelDTO(int panelId, int columnCount, int displayOrder, string name, int cellMarginLeft, int cellMarginRight, int cellMarginTop, int cellMarginBottom, int rowCount, string imageURL, string guid, int siteId, bool synchStatus, int masterEntityId,
                              string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            : this(panelId, columnCount, displayOrder, name, cellMarginLeft, cellMarginRight, cellMarginTop, cellMarginBottom, rowCount, imageURL, isActive)
        {
            log.LogMethodEntry(panelId, columnCount, displayOrder, name, cellMarginLeft, cellMarginRight, cellMarginTop, cellMarginBottom, rowCount, imageURL, guid, siteId, synchStatus, masterEntityId, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public ProductMenuPanelDTO(ProductMenuPanelDTO productMenuPanelDTO)
            : this()
        {
            log.LogMethodEntry(productMenuPanelDTO);
            panelId = productMenuPanelDTO.panelId;
            columnCount = productMenuPanelDTO.columnCount;
            displayOrder = productMenuPanelDTO.displayOrder;
            name = productMenuPanelDTO.name;
            cellMarginLeft = productMenuPanelDTO.cellMarginLeft;
            cellMarginRight = productMenuPanelDTO.cellMarginRight;
            cellMarginTop = productMenuPanelDTO.cellMarginTop;
            cellMarginBottom = productMenuPanelDTO.cellMarginBottom;
            rowCount = productMenuPanelDTO.rowCount;
            imageURL = productMenuPanelDTO.imageURL;
            isActive = productMenuPanelDTO.isActive;
            siteId = productMenuPanelDTO.siteId;
            synchStatus = productMenuPanelDTO.synchStatus;
            guid = productMenuPanelDTO.guid;
            lastUpdatedBy = productMenuPanelDTO.lastUpdatedBy;
            lastUpdateDate = productMenuPanelDTO.lastUpdateDate;
            createdBy = productMenuPanelDTO.createdBy;
            creationDate = productMenuPanelDTO.creationDate;
            masterEntityId = productMenuPanelDTO.masterEntityId;
            if (productMenuPanelDTO.productMenuPanelContentDTOList != null && productMenuPanelDTO.productMenuPanelContentDTOList.Any())
            {
                productMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
                foreach (var productMenuPanelContentDTO in productMenuPanelDTO.productMenuPanelContentDTOList)
                {
                    productMenuPanelContentDTOList.Add(new ProductMenuPanelContentDTO(productMenuPanelContentDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CellMarginTop field
        /// </summary>
        public int CellMarginTop { get { return cellMarginTop; } set { this.IsChanged = true; cellMarginTop = value; } }

        /// <summary>
        /// Get/Set method of the RowCount field
        /// </summary>
        public int RowCount { get { return rowCount; } set { this.IsChanged = true; rowCount = value; } }

        /// <summary>
        /// Get/Set method of the PanelId field
        /// </summary>
        public int PanelId { get { return panelId; } set { this.IsChanged = true; panelId = value; } }
        /// <summary>
        /// Get/Set method of the ColumnCount field
        /// </summary>
        public int ColumnCount { get { return columnCount; } set { this.IsChanged = true; columnCount = value; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        public int DisplayOrder { get { return displayOrder; } set { this.IsChanged = true; displayOrder = value; } }

        /// <summary>
        /// Get/Set method of the CellMarginBottom field
        /// </summary>
        public int CellMarginBottom { get { return cellMarginBottom; } set { this.IsChanged = true; cellMarginBottom = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { this.IsChanged = true; name = value; } }

        /// <summary>
        /// Get/Set method of the ImageURL field
        /// </summary>
        public string ImageURL { get { return imageURL; } set { this.IsChanged = true; imageURL = value; } }
        /// <summary>
        /// Get/Set method of the CellMarginLeft field
        /// </summary>
        public int CellMarginLeft { get { return cellMarginLeft; } set { this.IsChanged = true; cellMarginLeft = value; } }

        /// <summary>
        /// Get/Set method of the CellMarginRight field
        /// </summary>
        public int CellMarginRight { get { return cellMarginRight; } set { this.IsChanged = true; cellMarginRight = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the ProductMenuPanelContentDTOList field
        /// </summary>
        public List<ProductMenuPanelContentDTO> ProductMenuPanelContentDTOList { get { return productMenuPanelContentDTOList; } set { productMenuPanelContentDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || panelId < 0;
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
        /// Returns whether the PosMachineDTO changed or any of its posProductDisplayList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (productMenuPanelContentDTOList != null &&
                    productMenuPanelContentDTOList.Any(x => x.IsChanged))
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
