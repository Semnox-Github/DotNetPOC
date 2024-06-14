/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu panel Business object
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
    public class ProductMenuPanelBL
    {
        private ProductMenuPanelDTO productMenuPanelDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductMenuPanelBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ProductMenuPanelBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productMenuPanelDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductMenuPanelId parameter
        /// </summary>
        /// <param name="productMenuPanelId">ProductMenuPanelId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public ProductMenuPanelBL(ExecutionContext executionContext, int productMenuPanelId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(productMenuPanelId, loadChildRecords, activeChildRecords);
            LoadProductMenuPanel(productMenuPanelId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductMenuPanel id as the parameter
        /// Would fetch the ProductMenuPanel object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        private void LoadProductMenuPanel(int id, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            ProductMenuPanelDataHandler productMenuPanelDataHandler = new ProductMenuPanelDataHandler(sqlTransaction);
            productMenuPanelDTO = productMenuPanelDataHandler.GetProductMenuPanel(id);
            ThrowIfProductMenuPanelIsNull(id);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(productMenuPanelDTO);
        }

        private void ThrowIfProductMenuPanelIsNull(int menuId)
        {
            log.LogMethodEntry();
            if (productMenuPanelDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPanel", menuId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parametersproductMenuPanelDTO">sproductMenuPanelDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductMenuPanelBL(ExecutionContext executionContext, ProductMenuPanelDTO parametersproductMenuPanelDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parametersproductMenuPanelDTO, sqlTransaction);

            if (parametersproductMenuPanelDTO.PanelId > -1)
            {
                LoadProductMenuPanel(parametersproductMenuPanelDTO.PanelId, true, false, sqlTransaction);//added sql
                ThrowIfProductMenuPanelIsNull(parametersproductMenuPanelDTO.PanelId);
                Update(parametersproductMenuPanelDTO, sqlTransaction);
            }
            else
            {
                ValidateName(parametersproductMenuPanelDTO.Name);
                ValidateColumnCount(parametersproductMenuPanelDTO.ColumnCount);
                ValidateDisplayOrder(parametersproductMenuPanelDTO.DisplayOrder);
                ValidateName(parametersproductMenuPanelDTO.Name);
                ValidateCellMarginLeft(parametersproductMenuPanelDTO.CellMarginLeft);
                ValidateCellMarginRight(parametersproductMenuPanelDTO.CellMarginRight);
                ValidateCellMarginTop(parametersproductMenuPanelDTO.CellMarginTop);
                ValidateCellMarginBottom(parametersproductMenuPanelDTO.CellMarginBottom);
                ValidateRowCount(parametersproductMenuPanelDTO.RowCount);
                ValidateImageURL(parametersproductMenuPanelDTO.ImageURL);
                ValidateIsActive(parametersproductMenuPanelDTO.IsActive);
                productMenuPanelDTO = new ProductMenuPanelDTO(-1, parametersproductMenuPanelDTO.ColumnCount, parametersproductMenuPanelDTO.DisplayOrder, parametersproductMenuPanelDTO.Name, parametersproductMenuPanelDTO.CellMarginLeft, parametersproductMenuPanelDTO.CellMarginRight, parametersproductMenuPanelDTO.CellMarginTop, parametersproductMenuPanelDTO.CellMarginBottom, 
                                                              parametersproductMenuPanelDTO.RowCount, parametersproductMenuPanelDTO.ImageURL, parametersproductMenuPanelDTO.IsActive);
                if (parametersproductMenuPanelDTO.ProductMenuPanelContentDTOList != null && parametersproductMenuPanelDTO.ProductMenuPanelContentDTOList.Any())
                {
                    productMenuPanelDTO.ProductMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
                    foreach (ProductMenuPanelContentDTO parameterProductMenuPanelContentDTO in parametersproductMenuPanelDTO.ProductMenuPanelContentDTOList)
                    {
                        if (parameterProductMenuPanelContentDTO.Id > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductMenuPanelContent", parameterProductMenuPanelContentDTO.PanelId);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var productMenuPanelContentDTO = new ProductMenuPanelContentDTO(-1, -1, parameterProductMenuPanelContentDTO.ObjectType, parameterProductMenuPanelContentDTO.ObjectGuid, parameterProductMenuPanelContentDTO.ImageURL, parameterProductMenuPanelContentDTO.BackColor,
                                                                                         parameterProductMenuPanelContentDTO.TextColor, parameterProductMenuPanelContentDTO.Font, parameterProductMenuPanelContentDTO.ColumnIndex, parameterProductMenuPanelContentDTO.RowIndex, parameterProductMenuPanelContentDTO.ButtonType,parameterProductMenuPanelContentDTO.IsActive);
                        ProductMenuPanelContentBL productMenuPanelContentBL = new ProductMenuPanelContentBL(executionContext, productMenuPanelContentDTO);
                        productMenuPanelDTO.ProductMenuPanelContentDTOList.Add(productMenuPanelContentBL.ProductMenuPanelContentDTO);
                    }
                }
                ValidateProductMenuPanelContentConstaints();
            }
            log.LogMethodExit();
        }

        

        public void Update(ProductMenuPanelDTO parameterProductMenuPanelDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterProductMenuPanelDTO, sqlTransaction);
            ChangeColumnCount(parameterProductMenuPanelDTO.ColumnCount);
            ChangeCellMarginLeft(parameterProductMenuPanelDTO.CellMarginLeft);
            ChangeDisplayOrder(parameterProductMenuPanelDTO.DisplayOrder);
            ChangeName(parameterProductMenuPanelDTO.Name);
            ChangeCellMarginRight(parameterProductMenuPanelDTO.CellMarginRight);
            ChangeCellMarginTop(parameterProductMenuPanelDTO.CellMarginTop);
            ChangeCellMarginBottom(parameterProductMenuPanelDTO.CellMarginBottom);
            ChangeIsActive(parameterProductMenuPanelDTO.IsActive, sqlTransaction);
            ChangeImageURL(parameterProductMenuPanelDTO.ImageURL);
            ChangeRowCount(parameterProductMenuPanelDTO.RowCount);
            Dictionary<int, ProductMenuPanelContentDTO> productMenuPanelContentDTODictionary = new Dictionary<int, ProductMenuPanelContentDTO>();
            if (productMenuPanelDTO.ProductMenuPanelContentDTOList != null &&
                productMenuPanelDTO.ProductMenuPanelContentDTOList.Any())
            {
                foreach (var productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
                {
                    productMenuPanelContentDTODictionary.Add(productMenuPanelContentDTO.Id, productMenuPanelContentDTO);
                }
            }
            if (parameterProductMenuPanelDTO.ProductMenuPanelContentDTOList != null &&
                parameterProductMenuPanelDTO.ProductMenuPanelContentDTOList.Any())
            {
                foreach (var parameterProductMenuPanelContentDTO in parameterProductMenuPanelDTO.ProductMenuPanelContentDTOList)
                {
                    if (productMenuPanelContentDTODictionary.ContainsKey(parameterProductMenuPanelContentDTO.Id))
                    {
                        ProductMenuPanelContentBL productMenuPanelContent = new ProductMenuPanelContentBL(executionContext, productMenuPanelContentDTODictionary[parameterProductMenuPanelContentDTO.Id]);
                        productMenuPanelContent.Update(parameterProductMenuPanelContentDTO);
                    }
                    else if (parameterProductMenuPanelContentDTO.Id > -1)
                    {
                        ProductMenuPanelContentBL productMenuPanelContent = new ProductMenuPanelContentBL(executionContext, parameterProductMenuPanelContentDTO.Id, sqlTransaction);
                        if (productMenuPanelDTO.ProductMenuPanelContentDTOList == null)
                        {
                            productMenuPanelDTO.ProductMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
                        }
                        productMenuPanelDTO.ProductMenuPanelContentDTOList.Add(productMenuPanelContent.ProductMenuPanelContentDTO);
                        productMenuPanelContent.Update(parameterProductMenuPanelContentDTO);
                    }
                    else
                    {
                        ProductMenuPanelContentBL productMenuPanelContentBL = new ProductMenuPanelContentBL(executionContext, parameterProductMenuPanelContentDTO);
                        if (productMenuPanelDTO.ProductMenuPanelContentDTOList == null)
                        {
                            productMenuPanelDTO.ProductMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
                        }
                        productMenuPanelDTO.ProductMenuPanelContentDTOList.Add(productMenuPanelContentBL.ProductMenuPanelContentDTO);
                    }
                }
                ValidateProductMenuPanelContentConstaints();
                log.LogMethodExit();
            }
        }

        public void AddProductMenuPanelContentDTOList(List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList)
        {
            log.LogMethodEntry(productMenuPanelContentDTOList);
            if(productMenuPanelContentDTOList == null || productMenuPanelContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "productMenuPanelContentDTOList is empty");
                return;
            }
            if(productMenuPanelContentDTOList.Any(x => x.Id > -1))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4009);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            foreach (var productMenuPanelContentDTO in productMenuPanelContentDTOList)
            {
                AddProductMenuPanelContent(productMenuPanelContentDTO);
            }
            log.LogMethodExit();
        }

        private void AddProductMenuPanelContent(ProductMenuPanelContentDTO productMenuPanelContentDTO)
        {
            log.LogMethodEntry(productMenuPanelContentDTO);
            
            ProductMenuPanelContentBL lastProductMenuPanelContentBL = GetLastProductMenuPanelContentBL();
            int newColumnIndex = 0;
            int newRowIndex = 0;
            ProductMenuPanelContentButtonType buttonType = ProductMenuPanelContentButtonType.FromString(productMenuPanelContentDTO.ButtonType);
            if (lastProductMenuPanelContentBL != null)
            {
                if(lastProductMenuPanelContentBL.GetLastOccupiedColumnIndex() + buttonType.HorizontalCellCount < productMenuPanelDTO.ColumnCount)
                {
                    newColumnIndex = lastProductMenuPanelContentBL.GetLastOccupiedColumnIndex() + 1;
                    newRowIndex = lastProductMenuPanelContentBL.ProductMenuPanelContentDTO.RowIndex;
                }
                else
                {
                    newColumnIndex = 0;
                    newRowIndex = lastProductMenuPanelContentBL.GetLastOccupiedRowIndex() + 1;
                }
            }
            if(productMenuPanelDTO.RowCount < (newRowIndex + buttonType.VerticalCellCount))
            {
                productMenuPanelDTO.RowCount = newRowIndex + buttonType.VerticalCellCount;
            }
            if(productMenuPanelDTO.ColumnCount < (newRowIndex + buttonType.HorizontalCellCount))
            {
                productMenuPanelDTO.ColumnCount = newColumnIndex + buttonType.HorizontalCellCount;
            }
            if(productMenuPanelDTO.ProductMenuPanelContentDTOList == null)
            {
                productMenuPanelDTO.ProductMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
            }
            productMenuPanelContentDTO.RowIndex = newRowIndex;
            productMenuPanelContentDTO.ColumnIndex = newColumnIndex;
            ProductMenuPanelContentBL productMenuPanelContentBL = new ProductMenuPanelContentBL(executionContext, productMenuPanelContentDTO);
            productMenuPanelDTO.ProductMenuPanelContentDTOList.Add(productMenuPanelContentBL.ProductMenuPanelContentDTO);
            ValidateProductMenuPanelContentConstaints();
            log.LogMethodExit();
        }

        private void ValidateProductMenuPanelContentConstaints()
        {
            log.LogMethodEntry();
            if (productMenuPanelDTO.ProductMenuPanelContentDTOList == null ||
                productMenuPanelDTO.ProductMenuPanelContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "productMenuPanelDTO.ProductMenuPanelContentDTOList is empty.");
                return;
            }
            foreach (var productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
            {
                if(productMenuPanelContentDTO.IsActive == false)
                {
                    continue;
                }
                ProductMenuPanelContentBL productMenuPanelContentBL = new ProductMenuPanelContentBL(executionContext, productMenuPanelContentDTO);
                int occupiedColumnIndex = productMenuPanelContentBL.GetLastOccupiedColumnIndex();
                int occupiedRowIndex = productMenuPanelContentBL.GetLastOccupiedRowIndex();
                if (occupiedColumnIndex >= productMenuPanelDTO.ColumnCount)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Column Count"));
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    string message = productMenuPanelContentBL.ToString() +
                                     Environment.NewLine +
                                     " is having a invalid columnIndex." +
                                     Environment.NewLine +
                                      "it occupies : " + occupiedColumnIndex + " which is more than allowed : " + (productMenuPanelDTO.ColumnCount - 1);
                    throw new ValidationException(message, "ProductMenuPanel", "ColumnCount", errorMessage);
                }
                if (occupiedRowIndex >= productMenuPanelDTO.RowCount)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Row Count"));
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    string message = productMenuPanelContentBL.ToString() +
                                     Environment.NewLine +
                                     " is having a invalid columnIndex." +
                                     Environment.NewLine +
                                      "it occupies : " + occupiedColumnIndex + " which is more than allowed : " + (productMenuPanelDTO.ColumnCount - 1);
                    throw new ValidationException(message, "ProductMenuPanel", "RowCount", errorMessage);
                }
                foreach (var otherProductMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
                {
                    if (productMenuPanelContentDTO.IsActive == false || 
                        otherProductMenuPanelContentDTO == productMenuPanelContentDTO)
                    {
                        continue;
                    }
                    if(productMenuPanelContentBL.Collides(otherProductMenuPanelContentDTO.RowIndex, otherProductMenuPanelContentDTO.ColumnIndex))
                    {
                        ProductMenuPanelContentBL otherProductMenuPanelContentBL = new ProductMenuPanelContentBL(executionContext, otherProductMenuPanelContentDTO);
                        string message = productMenuPanelContentBL.ToString() + " and " + otherProductMenuPanelContentBL.ToString() +
                                         Environment.NewLine +
                                         " are colliding.";
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new ValidationException(message, "ProductMenuPanel", "RowCount", message);
                    }
                }
            }
            log.LogMethodExit();
        }

        private ProductMenuPanelContentBL GetLastProductMenuPanelContentBL()
        {
            log.LogMethodEntry();
            if(productMenuPanelDTO.ProductMenuPanelContentDTOList == null 
                || productMenuPanelDTO.ProductMenuPanelContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "productMenuPanelDTO.ProductMenuPanelContentDTOList is empty");
                return null;
            }
            ProductMenuPanelContentDTO productMenuPanelContentDTO = productMenuPanelDTO.ProductMenuPanelContentDTOList.OrderBy(x => x.RowIndex * productMenuPanelDTO.RowCount + x.ColumnIndex).Last();
            ProductMenuPanelContentBL productMenuPanelContentBL = new ProductMenuPanelContentBL(executionContext, productMenuPanelContentDTO);
            log.LogMethodExit(productMenuPanelContentBL);
            return productMenuPanelContentBL;
        }

        private void ChangeRowCount(int rowCount)
        {
            log.LogMethodEntry(rowCount);
            if (productMenuPanelDTO.RowCount == rowCount)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel rowCount");
                return;
            }
            ValidateRowCount(rowCount);
            productMenuPanelDTO.RowCount = rowCount;
            log.LogMethodExit();
        }

        private void ChangeImageURL(string imageURL)
        {
            log.LogMethodEntry(imageURL);
            if (productMenuPanelDTO.ImageURL == imageURL)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel ImageURL");
                return;
            }
            ValidateImageURL(imageURL);
            productMenuPanelDTO.ImageURL = imageURL;
            log.LogMethodExit();
        }

        private void ValidateImageURL(string imageURL)
        {
            log.LogMethodEntry(imageURL);
            log.LogMethodExit();
        }

        private void ChangeName(string name)
        {
            log.LogMethodEntry(name);
            if (productMenuPanelDTO.Name == name)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel name");
                return;
            }
            ValidateName(name);
            productMenuPanelDTO.Name = name;
            log.LogMethodExit();
        }

        private void ChangeCellMarginBottom(int cellMarginBottom)
        {
            log.LogMethodEntry(cellMarginBottom);
            if (productMenuPanelDTO.CellMarginBottom == cellMarginBottom)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel cellMarginBottom");
                return;
            }
            ValidateCellMarginBottom(cellMarginBottom);
            productMenuPanelDTO.CellMarginBottom = cellMarginBottom;
            log.LogMethodExit();
        }

        private void ChangeCellMarginTop(int cellMarginTop)
        {
            log.LogMethodEntry(cellMarginTop);
            if (productMenuPanelDTO.CellMarginTop == cellMarginTop)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel CellMarginTop");
                return;
            }
            ValidateCellMarginTop(cellMarginTop);
            productMenuPanelDTO.CellMarginTop = cellMarginTop;
            log.LogMethodExit();
        }

        private void ChangeCellMarginRight(int cellMarginRight)
        {
            log.LogMethodEntry(cellMarginRight);
            if (productMenuPanelDTO.CellMarginRight == cellMarginRight)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel cellMarginRight");
                return;
            }
            ValidateCellMarginRight(cellMarginRight);
            productMenuPanelDTO.CellMarginRight = cellMarginRight;
            log.LogMethodExit();
        }
        private void ChangeDisplayOrder(int displayOrder)
        {
            log.LogMethodEntry(displayOrder);
            if (productMenuPanelDTO.DisplayOrder == displayOrder)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel displayOrder");
                return;
            }
            ValidateDisplayOrder(displayOrder);
            productMenuPanelDTO.DisplayOrder = displayOrder;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(isActive);
            if (productMenuPanelDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel isActive");
                return;
            }
            ValidateIsActive(isActive, sqlTransaction);
            productMenuPanelDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeColumnCount(int columnCount)
        {
            log.LogMethodEntry(columnCount);
            if (productMenuPanelDTO.ColumnCount == columnCount)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel columnCount");
                return;
            }
            ValidateColumnCount(columnCount);
            productMenuPanelDTO.ColumnCount = columnCount;
            log.LogMethodExit();
        }


        public void ChangeCellMarginLeft(int cellMarginLeft)
        {
            log.LogMethodEntry(cellMarginLeft);
            if (productMenuPanelDTO.CellMarginLeft == cellMarginLeft)
            {
                log.LogMethodExit(null, "No changes to ProductMenuPanel CellMarginLeft");
                return;
            }
            ValidateCellMarginLeft(cellMarginLeft);
            productMenuPanelDTO.CellMarginLeft = cellMarginLeft;
            log.LogMethodExit();
        }

        private void ValidateDisplayOrder(int displayOrder)
        {
            log.LogMethodEntry(displayOrder);
            if (displayOrder < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "displayOrder"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("displayOrder is less than 0.", "ProductMenuPanel", "displayOrder", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateRowCount(int rowCount)
        {
            log.LogMethodEntry(rowCount);
            if (rowCount < 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Row Count"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("rowCount is less than 0.", "ProductMenuPanel", "RowCount", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateColumnCount(int columnCount)
        {
            log.LogMethodEntry(columnCount);
            if (columnCount < 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Column Count"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("columnCount is less than 0.", "ProductMenuPanel", "ColumnCount", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateIsActive(bool isActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(isActive);
            if ((productMenuPanelDTO != null && productMenuPanelDTO.PanelId > -1) && isActive == false)
            {
                ProductMenuPanelDataHandler productMenuPanelDataHandler = new ProductMenuPanelDataHandler(sqlTransaction);
                bool isRecordReferenced = productMenuPanelDataHandler.GetIsRecordReferenced(productMenuPanelDTO.PanelId);
                if (isRecordReferenced)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1869);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Unable to delete this record. Please check the reference record first.", "ProductMenu", "IsActive", errorMessage);
                }
            }
            log.LogMethodExit();
        }

        private void ValidateCellMarginRight(int cellMarginRight)
        {
            log.LogMethodEntry(cellMarginRight);
            if (cellMarginRight < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Cell Margin Right"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("cellMarginRight is less than 0.", "ProductMenuPanel", "CellMarginRight", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateCellMarginLeft(int cellMarginLeft)
        {
            log.LogMethodEntry(cellMarginLeft);
            if (cellMarginLeft < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Cell Margin Left"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("cellMarginLeft is less than 0.", "ProductMenuPanel", "CellMarginLeft", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateCellMarginTop(int cellMarginTop)
        {
            log.LogMethodEntry(cellMarginTop);
            if (cellMarginTop < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Cell Margin Top"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("cellMarginTop is less than 0.", "ProductMenuPanel", "CellMarginTop", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateCellMarginBottom(int cellMarginBottom)
        {
            log.LogMethodEntry(cellMarginBottom);
            if (cellMarginBottom < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Cell Margin Bottom"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("cellMarginBottom is less than 0.", "ProductMenuPanel", "CellMarginBottom", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name is empty.", "ProductMenuPanel", "Name", errorMessage);
            }
            if (name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Name greater than 100 characters.", "ProductMenuPanel", "Name", errorMessage);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the child records for ProductMenu object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            ProductMenuPanelContentListBL productMenuPanelContentListBL = new ProductMenuPanelContentListBL(executionContext);
            List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList = productMenuPanelContentListBL.GetProductMenuPanelContentDTOList(new List<int>() { productMenuPanelDTO.PanelId }, activeChildRecords, sqlTransaction);
            if (productMenuPanelContentDTOList.Count != 0 && productMenuPanelContentDTOList.Any())
            {
                productMenuPanelDTO.ProductMenuPanelContentDTOList = productMenuPanelContentDTOList;
            }
            log.LogMethodExit();
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves theProductMenuPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductMenuPanelDataHandler productMenuPanelDataHandler = new ProductMenuPanelDataHandler(sqlTransaction);
            if (productMenuPanelDTO.PanelId < 0)
            {
                productMenuPanelDTO = productMenuPanelDataHandler.Insert(productMenuPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productMenuPanelDTO.AcceptChanges();
            }
            else
            {
                if (productMenuPanelDTO.IsChanged)
                {
                    productMenuPanelDTO = productMenuPanelDataHandler.Update(productMenuPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productMenuPanelDTO.AcceptChanges();
                }
            }
            // Will Save the Child ProductMenuPanelDTO
            log.Debug("productMenuPanelDTO.ProductMenuPanelContentDTOList Value :" + productMenuPanelDTO.ProductMenuPanelContentDTOList);
            if (productMenuPanelDTO.ProductMenuPanelContentDTOList != null && productMenuPanelDTO.ProductMenuPanelContentDTOList.Any())
            {
                List<ProductMenuPanelContentDTO> updatedProductMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
                foreach (ProductMenuPanelContentDTO productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
                {
                    if (productMenuPanelContentDTO.PanelId != productMenuPanelDTO.PanelId)
                    {
                        productMenuPanelContentDTO.PanelId = productMenuPanelDTO.PanelId;
                    }
                    log.Debug("ProductMenuPanelDTO.IsChanged Value :" + productMenuPanelContentDTO.IsChanged);
                    if (productMenuPanelContentDTO.IsChanged)
                    {
                        updatedProductMenuPanelContentDTOList.Add(productMenuPanelContentDTO);
                    }
                }
                log.Debug("updatedProductMenuPanelDTO Value :" + updatedProductMenuPanelContentDTOList);
                if (updatedProductMenuPanelContentDTOList.Any())
                {
                    ProductMenuPanelContentListBL productMenuPanelContentListBL = new ProductMenuPanelContentListBL(executionContext, updatedProductMenuPanelContentDTOList);
                    productMenuPanelContentListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductMenuPanelDTO ProductMenuPanelDTO
        {
            get
            {
                ProductMenuPanelDTO result = new ProductMenuPanelDTO(productMenuPanelDTO);
                return result;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProductMenuPanel
    /// </summary>
    public class ProductMenuPanelListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductMenuPanelDTO> productMenuPanelDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductMenuPanelListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of ProductMenuPanelListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPanelListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="ProductMenuPanelToList">ProductMenuPanelToList</param>
        /// <param name="executionContext">executionContext</param>
        public ProductMenuPanelListBL(ExecutionContext executionContext, List<ProductMenuPanelDTO> productMenuPanelDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productMenuPanelDTOList);
            this.productMenuPanelDTOList = productMenuPanelDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be used to Save and Update the ProductMenuPanel.
        /// </summary>
        public List<ProductMenuPanelDTO> Save(List<ProductMenuPanelDTO> productMenuPanelDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ProductMenuPanelDTO> savedProductMenuPanelDTOList = new List<ProductMenuPanelDTO>();
            if (productMenuPanelDTOList == null || productMenuPanelDTOList.Any() == false)
            {
                log.LogMethodExit(savedProductMenuPanelDTOList);
                return savedProductMenuPanelDTOList;
            }
            foreach (ProductMenuPanelDTO productMenuPanelDTO in productMenuPanelDTOList)
            {
                ProductMenuPanelBL productMenuPanelBL = new ProductMenuPanelBL(executionContext, productMenuPanelDTO, sqlTransaction);
                productMenuPanelBL.Save(sqlTransaction);
                savedProductMenuPanelDTOList.Add(productMenuPanelBL.ProductMenuPanelDTO);
            }
            log.LogMethodExit(savedProductMenuPanelDTOList);
            return savedProductMenuPanelDTOList;
        }

        /// <summary>
        /// Returns the ProductMenuPanel list
        /// </summary>
        public List<ProductMenuPanelDTO> GetProductMenuPanelDTOList(List<KeyValuePair<ProductMenuPanelDTO.SearchByProductMenuPanelParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            ProductMenuPanelDataHandler productMenuPanelDataHandler = new ProductMenuPanelDataHandler(sqlTransaction);
            List<ProductMenuPanelDTO> productMenuPanelDTOsList = productMenuPanelDataHandler.GetProductMenuPanelDTOList(searchParameters, sqlTransaction);
            if (productMenuPanelDTOsList != null && productMenuPanelDTOsList.Any() && loadChildRecords)
            {
                Build(productMenuPanelDTOsList, loadActiveChildRecords, sqlTransaction);

            }
            log.LogMethodExit(productMenuPanelDTOsList);
            return productMenuPanelDTOsList;
        }


        /// <summary>
        /// Gets the ProductMenuPanelDTO List for ProductMenuPanelList
        /// </summary>
        /// <param name="menuIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductMenuPanelDTO</returns>
        public List<ProductMenuPanelDTO> GetProductMenuPanelDTOList(List<int> menuIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(menuIdList, activeRecords, sqlTransaction);
            ProductMenuPanelDataHandler productMenuPanelDataHandler = new ProductMenuPanelDataHandler(sqlTransaction);
            List<ProductMenuPanelDTO> productMenuPanelDTOList = productMenuPanelDataHandler.GetProductMenuPanelDTOList(menuIdList, activeRecords);
            if (productMenuPanelDTOList != null && productMenuPanelDTOList.Any())
            {
                Build(productMenuPanelDTOList, activeRecords, sqlTransaction);
            }

            log.LogMethodExit(productMenuPanelDTOList);
            return productMenuPanelDTOList;
        }

        private void Build(List<ProductMenuPanelDTO> productMenuPanelDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productMenuPanelDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductMenuPanelDTO> productMenuPanelDTOIdMap = new Dictionary<int, ProductMenuPanelDTO>();
            List<int> productMenuPanelIdList = new List<int>();
            for (int i = 0; i < productMenuPanelDTOList.Count; i++)
            {
                if (productMenuPanelDTOIdMap.ContainsKey(productMenuPanelDTOList[i].PanelId))
                {
                    continue;
                }
                productMenuPanelDTOIdMap.Add(productMenuPanelDTOList[i].PanelId, productMenuPanelDTOList[i]);
                productMenuPanelIdList.Add(productMenuPanelDTOList[i].PanelId);
            }

            ProductMenuPanelContentListBL productMenuPanelContentListBL = new ProductMenuPanelContentListBL(executionContext);
            List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList = productMenuPanelContentListBL.GetProductMenuPanelContentDTOList(productMenuPanelIdList, activeChildRecords, sqlTransaction);
            if (productMenuPanelContentDTOList != null && productMenuPanelContentDTOList.Any())
            {
                for (int i = 0; i < productMenuPanelContentDTOList.Count; i++)
                {
                    if (productMenuPanelDTOIdMap.ContainsKey(productMenuPanelContentDTOList[i].PanelId) == false)
                    {
                        continue;
                    }
                    ProductMenuPanelDTO productMenuPanelDTO = productMenuPanelDTOIdMap[productMenuPanelContentDTOList[i].PanelId];
                    if (productMenuPanelDTO.ProductMenuPanelContentDTOList == null)
                    {
                        productMenuPanelDTO.ProductMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
                    }
                    productMenuPanelDTO.ProductMenuPanelContentDTOList.Add(productMenuPanelContentDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
    }
}
