/********************************************************************************************
 * Project Name - Product
 * Description  - product menu panel content container data transfer object
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Jun-2021      Lakshminarayana           Created : Static menu enhancement
 ********************************************************************************************/

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// product menu panel content container data transfer object
    /// </summary>
    public class ProductMenuPanelContentContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int panelId;
        private int productId;
        private int childPanelId;
        private string name;
        private int displayOrder;
        private string imageURL;
        private string backColor;
        private string textColor;
        private string font;
        private int columnIndex;
        private int rowIndex;
        private string buttonType;
        private bool isDiscounted;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductMenuPanelContentContainerDTO()
        {
            log.LogMethodEntry();
            id = -1;
            isDiscounted = false;
            name = string.Empty;
            childPanelId = -1;
            displayOrder = 0;
            panelId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public ProductMenuPanelContentContainerDTO(int id, int panelId, int productId, int childPanelId, string name, int displayOrder, string imageURL, string backColor, string textColor, string font, int columnIndex, int rowIndex, string buttonType, bool isDiscounted)
        {
            log.LogMethodEntry(id, panelId, productId, childPanelId, name, displayOrder, imageURL, backColor, textColor, font, columnIndex, rowIndex, buttonType, isDiscounted);
            this.id = id;
            this.panelId = panelId;
            this.displayOrder = displayOrder;
            this.productId = productId;
            this.childPanelId = childPanelId;
            this.name = name;
            this.imageURL = imageURL;
            this.backColor = backColor;
            this.textColor = textColor;
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            this.buttonType = buttonType;
            this.font = font;
            this.isDiscounted = isDiscounted;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="productMenuPanelContentContainerDTO"></param>
        public ProductMenuPanelContentContainerDTO(ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO)
            : this(productMenuPanelContentContainerDTO.id,
                 productMenuPanelContentContainerDTO.panelId,
                 productMenuPanelContentContainerDTO.productId, 
                 productMenuPanelContentContainerDTO.childPanelId,
                 productMenuPanelContentContainerDTO.name,
                 productMenuPanelContentContainerDTO.displayOrder,
                 productMenuPanelContentContainerDTO.imageURL,
                 productMenuPanelContentContainerDTO.backColor,
                 productMenuPanelContentContainerDTO.textColor,
                 productMenuPanelContentContainerDTO.font,
                 productMenuPanelContentContainerDTO.columnIndex,
                 productMenuPanelContentContainerDTO.rowIndex,
                 productMenuPanelContentContainerDTO.buttonType,
                 productMenuPanelContentContainerDTO.isDiscounted)
        {
            log.LogMethodEntry(productMenuPanelContentContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; } }

        /// <summary>
        /// Get/Set method of the PanelId field
        /// </summary>
        public int PanelId { get { return panelId; } set { panelId = value; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value; } }

        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; } }
        /// <summary>
        /// Get/Set method of the childPanelId field
        /// </summary>
        public int ChildPanelId { get { return childPanelId; } set { childPanelId = value; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Get/Set method of the RowIndex field
        /// </summary>
        public int RowIndex { get { return rowIndex; } set { rowIndex = value; } }

        /// <summary>
        /// Get/Set method of the ColumnIndex field
        /// </summary>
        public int ColumnIndex { get { return columnIndex; } set { columnIndex = value; } }

        /// <summary>
        /// Get/Set method of the ImageURL field
        /// </summary>
        public string ImageURL { get { return imageURL; } set { imageURL = value; } }

        /// <summary>
        /// Get/Set method of the ButtonType field
        /// </summary>
        public string ButtonType { get { return buttonType; } set { buttonType = value; } }

        /// <summary>
        /// Get/Set method of the BackColor field
        /// </summary>
        public string BackColor { get { return backColor; } set { backColor = value; } }

        /// <summary>
        /// Get/Set method of the TextColor field
        /// </summary>
        public string TextColor { get { return textColor; } set { textColor = value; } }

        /// <summary>
        /// Get/Set method of the Font field
        /// </summary>
        public string Font { get { return font; } set { font = value; } }

        /// <summary>
        /// Get/Set method of the IsDiscounted field
        /// </summary>
        public bool IsDiscounted { get { return isDiscounted; } set { isDiscounted = value; } }

    }
}