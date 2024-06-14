/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel content Business object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuPanelContentBL
    {
        private ProductMenuPanelContentDTO productMenuPanelContentDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPanelContentBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ProductMenuPanelContentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productMenuPanelContentDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductMenuPanelContentId parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductMenuPanelContentBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ProductMenuPanelContentDataHandler productMenuPanelContentDataHandler = new ProductMenuPanelContentDataHandler(sqlTransaction);
            productMenuPanelContentDTO = productMenuPanelContentDataHandler.GetProductMenuPanelContent(id);
            if (productMenuPanelContentDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPanelContent", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates ProductMenuPanelContentBL object using the ProductMenuPanelContentDTO
        /// </summary>
        /// <param name="productMenuPanelContentDTO">ProductMenuPanelContentDTO object</param>
        /// <param name="executionContext">executionContext object</param>
        public ProductMenuPanelContentBL(ExecutionContext executionContext, ProductMenuPanelContentDTO productMenuPanelContentDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productMenuPanelContentDTO);
            if (productMenuPanelContentDTO.Id < 0)
            {
                ValidateColor(productMenuPanelContentDTO.TextColor);
                ValidateFont(productMenuPanelContentDTO.Font);
                ValidateImageURL(productMenuPanelContentDTO.ImageURL);
                ValidateObjectType(productMenuPanelContentDTO.ObjectType);
                ValidateButtonType(productMenuPanelContentDTO.ButtonType);
            }
            this.productMenuPanelContentDTO = productMenuPanelContentDTO;
            log.LogMethodExit();
        }

        public void Update(ProductMenuPanelContentDTO parameterProductMenuPanelContentDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterProductMenuPanelContentDTO, sqlTransaction);
            ChangeObjectType(parameterProductMenuPanelContentDTO.ObjectType);
            ChangeObjectGuid(parameterProductMenuPanelContentDTO.ObjectGuid);
            ChangeImageURL(parameterProductMenuPanelContentDTO.ImageURL);
            ChangeBackColor(parameterProductMenuPanelContentDTO.BackColor);
            ChangeTextColor(parameterProductMenuPanelContentDTO.TextColor);
            ChangeIsActive(parameterProductMenuPanelContentDTO.IsActive);
            ChangeFont(parameterProductMenuPanelContentDTO.Font);
            ChangeButtonType(parameterProductMenuPanelContentDTO.ButtonType);
            log.LogMethodExit();

        }

        private void ChangeObjectGuid(string objectGuid)
        {
            log.LogMethodEntry(objectGuid);
            if (productMenuPanelContentDTO.ObjectGuid == objectGuid)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent objectGuid");
                return;
            }
            productMenuPanelContentDTO.ObjectGuid = objectGuid;
            log.LogMethodExit();
        }

        private void ChangeImageURL(string imageURL)
        {
            log.LogMethodEntry(imageURL);
            if (productMenuPanelContentDTO.ImageURL == imageURL)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent ImageURL");
                return;
            }
            ValidateImageURL(imageURL);
            productMenuPanelContentDTO.ImageURL = imageURL;
            log.LogMethodExit();
        }

        private void ChangeButtonType(string buttonType)
        {
            log.LogMethodEntry(buttonType);
            if (productMenuPanelContentDTO.ButtonType == buttonType)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent ButtonType");
                return;
            }
            ValidateButtonType(buttonType);
            productMenuPanelContentDTO.ButtonType = buttonType;
            log.LogMethodExit();
        }

        private void ChangeBackColor(string backColor)
        {
            log.LogMethodEntry(backColor);
            if (productMenuPanelContentDTO.BackColor == backColor)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent BackColor");
                return;
            }
            ValidateColor(backColor);
            productMenuPanelContentDTO.BackColor = backColor;
            log.LogMethodExit();
        }

        private void ChangeFont(string font)
        {
            log.LogMethodEntry(font);
            if (productMenuPanelContentDTO.Font == font)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent Font");
                return;
            }
            productMenuPanelContentDTO.Font = font;
            ValidateFont(font);
            log.LogMethodExit();
        }

        private void ChangeTextColor(string textColor)
        {
            log.LogMethodEntry(textColor);
            if (productMenuPanelContentDTO.TextColor == textColor)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent TextColor");
                return;
            }
            productMenuPanelContentDTO.TextColor = textColor;
            ValidateColor(textColor);
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (productMenuPanelContentDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent isActive");
                return;
            }
            productMenuPanelContentDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeObjectType(string objectType)
        {
            log.LogMethodEntry(objectType);
            if (productMenuPanelContentDTO.ObjectType == objectType)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanelContent objectType");
                return;
            }
            ValidateObjectType(objectType);
            productMenuPanelContentDTO.ObjectType = objectType;
            log.LogMethodExit();
        }


        private void ValidateObjectType(string objectType)
        {
            log.LogMethodEntry(objectType);
            //if (objectType == -1)
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 858);
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("productId is empty.", "ProductMenuPanelContent", "productId", errorMessage);
            //}
            //if (productId > -1)
            //{
            //    Products products = new Products(productId);
            //    ProductsDTO productsDTO = products.GetProductsDTO;
            //    if (productsDTO == null || productsDTO.ProductId == -1 || productsDTO.ActiveFlag == false)
            //    {
            //        string errorMessage = MessageContainerList.GetMessage(executionContext, 858);
            //        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //        throw new ValidationException("product is invalid.", "ProductMenuPanelContent", "productId", errorMessage);

            //    }
            //}
            log.LogMethodExit();
        }

        private void ValidateImageURL(string imageURL)
        {
            log.LogMethodEntry(imageURL);
            log.LogMethodExit();
        }

        private void ValidateButtonType(string buttonType)
        {
            log.LogMethodEntry(buttonType);
            if (string.Equals(buttonType, "S") == false &&
                string.Equals(buttonType, "N") == false &&
                string.Equals(buttonType, "L") == false)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Button Type"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Invalid button type.", "ProductMenuPanelContent", "ButtonType", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateColor(string color)
        {
            //if (Regex.Match(color, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
            //    return;
            //else
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "color"));
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("displayOrder is less than 0.", "ProductMenuPanelContent", "color", errorMessage);

            //}

        }

        private void ValidateFont(string font)
        {

        }



        /// <summary>
        /// Saves the ProductMenuPanelContent
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productMenuPanelContentDTO.IsChanged == false && productMenuPanelContentDTO.Id > -1)
            {
                log.LogMethodExit(null, "productMenuPanelContentDTO is not changed.");
                return;
            }
            ProductMenuPanelContentDataHandler productMenuPanelContentDataHandler = new ProductMenuPanelContentDataHandler(sqlTransaction);
            productMenuPanelContentDataHandler.Save(productMenuPanelContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        internal int GetLastOccupiedColumnIndex()
        {
            log.LogMethodEntry();
            int result = productMenuPanelContentDTO.ColumnIndex + ProductMenuPanelContentButtonType.FromString(productMenuPanelContentDTO.ButtonType).HorizontalCellCount - 1;
            log.LogMethodExit(result);
            return result;
        }

        internal int GetLastOccupiedRowIndex()
        {
            log.LogMethodEntry();
            int result = productMenuPanelContentDTO.RowIndex + ProductMenuPanelContentButtonType.FromString(productMenuPanelContentDTO.ButtonType).VerticalCellCount - 1;
            log.LogMethodExit(result);
            return result;
        }

        public override string ToString()
        {
            log.LogMethodEntry();
            string result = "ProductMenuPanelContent(Id : " + productMenuPanelContentDTO.Id + " Button Type: " + productMenuPanelContentDTO.ButtonType + " Object Type" + productMenuPanelContentDTO.ObjectType + " Object Guid: " + productMenuPanelContentDTO.ObjectGuid + ")";
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductMenuPanelContentDTO ProductMenuPanelContentDTO
        {
            get
            {
                return productMenuPanelContentDTO;
            }
        }

        internal bool Collides(int rowIndex, int columnIndex)
        {
            log.LogMethodEntry(rowIndex, columnIndex);
            bool result = false;
            int occupiedColumnIndex = GetLastOccupiedColumnIndex();
            int occupiedRowIndex = GetLastOccupiedRowIndex();
            if (rowIndex >= productMenuPanelContentDTO.RowIndex &&
                rowIndex <= occupiedRowIndex &&
                columnIndex >= productMenuPanelContentDTO.ColumnIndex &&
                columnIndex <= occupiedColumnIndex)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }
    }

    /// <summary>
    /// Manages the list of ProductMenuPanelContent
    /// </summary>
    public class ProductMenuPanelContentListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPanelContentListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPanelContentListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="ProductMenuPanelContentToList">ProductMenuPanelContentToList</param>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPanelContentListBL(ExecutionContext executionContext, List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productMenuPanelContentDTOList);
            this.productMenuPanelContentDTOList = productMenuPanelContentDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates and saves the ProductMenuPanelContentDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productMenuPanelContentDTOList == null ||
                productMenuPanelContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }

            ProductMenuPanelContentDataHandler productMenuPanelContentDataHandler = new ProductMenuPanelContentDataHandler(sqlTransaction);
            productMenuPanelContentDataHandler.Save(productMenuPanelContentDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductMenuPanelContentDTO List for ProductMenuPanelList
        /// </summary>
        /// <param name="panelIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPanelContentDTO</returns>
        public List<ProductMenuPanelContentDTO> GetProductMenuPanelContentDTOList(List<int> panelIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(panelIdList, activeRecords, sqlTransaction);
            ProductMenuPanelContentDataHandler productMenuPanelContentDataHandler = new ProductMenuPanelContentDataHandler(sqlTransaction);
            List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList = productMenuPanelContentDataHandler.GetProductMenuPanelContentDTOList(panelIdList, activeRecords);
            log.LogMethodExit(productMenuPanelContentDTOList);
            return productMenuPanelContentDTOList;
        }
    }
}