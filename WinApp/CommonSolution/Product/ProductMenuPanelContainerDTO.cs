/********************************************************************************************
 * Project Name - Product
 * Description  - product menu panel container data transfer object
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Jun-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/

using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// product menu panel container data transfer object
    /// </summary>
    public class ProductMenuPanelContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int panelId;
        private bool isMainPanel;
        private int displayOrder;
        private string name;
        private int cellMarginLeft;
        private int cellMarginRight;
        private int cellMarginTop;
        private int cellMarginBottom;
        private int rowCount;
        private int columnCount;
        private string imageURL;
        private string guid;
        private List<ProductMenuPanelContentContainerDTO> productMenuPanelContentContainerDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductMenuPanelContainerDTO()
        {
            log.LogMethodEntry();
            productMenuPanelContentContainerDTOList = new List<ProductMenuPanelContentContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ProductMenuPanelContainerDTO(int panelId, bool isMainPanel, int displayOrder,
            string name, int cellMarginLeft, int cellMarginRight, int cellMarginTop, int cellMarginBottom, int rowCount,
            int columnCount, string imageURL, string guid)
            : this()
        {
            log.LogMethodEntry(panelId, isMainPanel, displayOrder, name, cellMarginLeft, cellMarginRight, cellMarginTop,
                cellMarginBottom, rowCount, columnCount, imageURL, guid);
            this.panelId = panelId;
            this.isMainPanel = isMainPanel;
            this.displayOrder = displayOrder;
            this.name = name;
            this.cellMarginLeft = cellMarginLeft; 
            this.cellMarginRight = cellMarginRight;
            this.cellMarginTop = cellMarginTop;
            this.cellMarginBottom = cellMarginBottom;
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.imageURL = imageURL;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public ProductMenuPanelContainerDTO(ProductMenuPanelContainerDTO productMenuPanelContainerDTO)
            :this(productMenuPanelContainerDTO.panelId, productMenuPanelContainerDTO.isMainPanel,
                 productMenuPanelContainerDTO.displayOrder, productMenuPanelContainerDTO.name,
                 productMenuPanelContainerDTO.cellMarginLeft, productMenuPanelContainerDTO.cellMarginRight,
                 productMenuPanelContainerDTO.cellMarginTop, productMenuPanelContainerDTO.cellMarginBottom,
                 productMenuPanelContainerDTO.rowCount, productMenuPanelContainerDTO.columnCount,
                 productMenuPanelContainerDTO.imageURL, productMenuPanelContainerDTO.guid)
        {
            log.LogMethodEntry(productMenuPanelContainerDTO);
            if(productMenuPanelContainerDTO.productMenuPanelContentContainerDTOList != null)
            {
                foreach (var productMenuPanelContentContainerDTO in productMenuPanelContainerDTO.productMenuPanelContentContainerDTOList)
                {
                    this.productMenuPanelContentContainerDTOList.Add(new ProductMenuPanelContentContainerDTO(productMenuPanelContentContainerDTO));
                }
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the panelId field
        /// </summary>
        public int PanelId
        {
            get
            {
                return panelId;
            }

            set
            {
                panelId = value;
            }
        }



        /// <summary>
        /// Get/Set method of the isMainPanel field
        /// </summary>
        public bool IsMainPanel
        {
            get
            {
                return isMainPanel;
            }

            set
            {
                isMainPanel = value;
            }
        }

        /// <summary>
        /// Get/Set method of the displayOrder field
        /// </summary>
        public int DisplayOrder
        {
            get
            {
                return displayOrder;
            }

            set
            {
                displayOrder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the cellMarginLeft field
        /// </summary>
        public int CellMarginLeft
        {
            get
            {
                return cellMarginLeft;
            }

            set
            {
                cellMarginLeft = value;
            }
        }

        /// <summary>
        /// Get/Set method of the startDate field
        /// </summary>
        public int CellMarginRight
        {
            get
            {
                return cellMarginRight;
            }

            set
            {
                cellMarginRight = value;
            }
        }

        /// <summary>
        /// Get/Set method of the cellMarginTop field
        /// </summary>
        public int CellMarginTop
        {
            get
            {
                return cellMarginTop;
            }

            set
            {
                cellMarginTop = value;
            }
        }

        /// <summary>
        /// Get/Set method of the startDate field
        /// </summary>
        public int CellMarginBottom
        {
            get
            {
                return cellMarginBottom;
            }

            set
            {
                cellMarginBottom = value;
            }
        }

        /// <summary>
        /// Get/Set method of the startDate field
        /// </summary>
        public int RowCount
        {
            get
            {
                return rowCount;
            }

            set
            {
                rowCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the columnCount field
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return columnCount;
            }

            set
            {
                columnCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the imageURL field
        /// </summary>
        public string ImageURL
        {
            get
            {
                return imageURL;
            }

            set
            {
                imageURL = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
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
        /// Get/Set method of the productMenuPanelContentContainerDTOList field
        /// </summary>
        public List<ProductMenuPanelContentContainerDTO> ProductMenuPanelContentContainerDTOList
        {
            get
            {
                return productMenuPanelContentContainerDTOList;
            }

            set
            {
                productMenuPanelContentContainerDTOList = value;
            }
        }

    }
}
