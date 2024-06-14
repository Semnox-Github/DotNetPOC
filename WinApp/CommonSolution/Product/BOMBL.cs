/********************************************************************************************
* Project Name - BOM BL
* Description  - Bussiness logic for BOM entity
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.60.3      14-Jun-2019      Nagesh Badiger      Added parametrized constructor and SaveUpdateProductBOM() in BOMList
*2.100.0     13-Sep-2020      Deeksha             Modified for Recipe Management Enhancement.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for BOM class.
    /// </summary>
    public class BOMBL
    {
        private BOMDTO bomDTO;
        BOMDataHandler bOMDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        public BOMBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.bomDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="bomDTO">Parameter of the type BOMDTO</param>
        public BOMBL(ExecutionContext executionContext, BOMDTO bOMDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, bOMDTO);
            this.bomDTO = bOMDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the BOM id as the parameter
        /// Would fetch the BOM object based on the ID passed. 
        /// </summary>
        /// <param name="BOMId">BOM id</param>
        public BOMBL(ExecutionContext executionContext, int BOMId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, BOMId);
            this.bomDTO = bOMDataHandler.GetBOMDTO(BOMId);
            log.LogMethodExit(bomDTO);
        }

        /// <summary>
        /// Saves the BOM
        /// BOM will be inserted if BOMId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);     
            Validate(sqlTransaction);
            BOMDataHandler bomDataHandler = new BOMDataHandler(sqlTransaction);
            if (bomDTO.BOMId < 0)
            {
                bomDTO = bomDataHandler.InsertBOM(bomDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                bomDTO.AcceptChanges();
            }
            else
            {
                if (bomDTO.IsChanged == true)
                {
                    bomDTO = bomDataHandler.UpdateBOM(bomDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    bomDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the BOMDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductList productListProduct = new ProductList(executionContext);
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, bomDTO.ProductId.ToString()));
            List<ProductDTO> productDTOListProduct = productListProduct.GetAllProducts(searchProductParameters);
            if (productDTOListProduct == null && productDTOListProduct.Count < 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 111);
                log.Error("Parent Product Not Found");
                throw new ValidationException(errorMessage);
            }
            ProductList productListChildProduct = new ProductList(executionContext);
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchChildProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, bomDTO.ChildProductId.ToString()));
            List<ProductDTO> productDTOListChildProduct = productListChildProduct.GetAllProducts(searchProductParameters);
            if (productDTOListChildProduct == null && productDTOListChildProduct.Count < 1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 111);
                log.Error("Child Product Not Found");
                throw new ValidationException(errorMessage);
            }
            if (bomDTO.ProductId == bomDTO.ChildProductId)
            {
                log.Error("Child Product cannot be same as its Parent ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 905); //Child Product cannot be same as its Parent
                throw new ValidationException(errorMessage);
            }
            if (bomDTO.Quantity <= 0)
            {
                log.Error("Please enter valid Quantity ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2360); //Please enter valid Quantity
                throw new ValidationException(errorMessage);
            }
            if (bomDTO.UOMId < 0)
            {
                log.Error("Please enter UOM ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 888); //Please enter UOM
                throw new ValidationException(errorMessage);
            }
            List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchParameters = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
            searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.PRODUCT_ID, bomDTO.ProductId.ToString()));
            searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID, bomDTO.ChildProductId.ToString()));
            BOMList bOMList = new BOMList(executionContext);
            List<BOMDTO> bOMDTOList = bOMList.GetAllBOMs(searchParameters);
            if (bOMDTOList != null && bOMDTOList.Any() && bOMDTOList.FirstOrDefault().BOMId != bomDTO.BOMId)
            {
                log.Error("Duplicate entries detail Bom Product");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Bom Product"));
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of BOM
    /// </summary>
    public class BOMList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel
        private List<BOMDTO> bOMDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public BOMList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.bOMDTOList = new List<BOMDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="bOMDTOList"></param>
        public BOMList(ExecutionContext executionContext, List<BOMDTO> bOMDTOList)
        {
            log.LogMethodEntry(executionContext, bOMDTOList);
            this.executionContext = executionContext;
            this.bOMDTOList = bOMDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the vendor list
        /// </summary>
        public List<BOMDTO> GetAllBOMs(List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            BOMDataHandler bomDataHandler = new BOMDataHandler();
            bOMDTOList = bomDataHandler.GetBOMList(searchParameters);
            log.LogMethodExit(bOMDTOList);
            return bOMDTOList;
        }
        ///<summary>
        ///Returns Datatable containing the ProductId and Code
        ///</summary>  
        public List<BOMDTO> GetProductDetails(int productId)
        {
            log.LogMethodEntry(productId);
            BOMDataHandler bomDataHandler = new BOMDataHandler();
            bOMDTOList = bomDataHandler.GetProductDetailsList(productId);
            log.LogMethodExit(bOMDTOList);
            return bOMDTOList;
        }

        /// <summary>
        /// Save and Update Bill of materials Details
        /// </summary>
        public void SaveUpdateProductBOM(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (bOMDTOList != null)
                {
                    foreach (BOMDTO bOMDTO in bOMDTOList)
                    {
                        BOMBL bOMBL = new BOMBL(executionContext, bOMDTO);
                        bOMBL.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        public List<BOMDTO> GetProductBOMDTOListOfProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            BOMDataHandler productBOMDataHandler = new BOMDataHandler(sqlTransaction);
            List<BOMDTO> bomDTOList = productBOMDataHandler.GetProductBOMDTOListOfProducts(productIdList, activeRecords);
            log.LogMethodExit(bomDTOList);
            return bomDTOList;
        }

        public List<BOMDTO> GetEventProducts(DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(fromDate, toDate);
            BOMDataHandler productBOMDataHandler = new BOMDataHandler(sqlTransaction);
            List<BOMDTO> bomDTOList = productBOMDataHandler.GetEventProductBOM(fromDate, toDate);
            log.LogMethodExit(bomDTOList);
            return bomDTOList;
        }

        /// <summary>
        /// This method is will return Sheet object for BOMDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(bool buildTemplate = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(buildTemplate);
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            BOMViewExcelDTODefination bOMViewExcelDTODefination = new BOMViewExcelDTODefination(executionContext, "");
            ///Building headers from BOMViewExcelDTODefination
            bOMViewExcelDTODefination.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);
            if (!buildTemplate)
            {
                BOMDataHandler bOMDataHandler = new BOMDataHandler(sqlTransaction);
                List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchParameters = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
                searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.SITEID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.IS_ACTIVE, "1"));
                bOMDTOList = bOMDataHandler.GetBOMList(searchParameters);
                if (bOMDTOList != null && bOMDTOList.Any())
                {
                    foreach (BOMDTO bOMDTO in bOMDTOList)
                    {
                        ProductList productListProduct = new ProductList(executionContext);
                        List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                        searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, bOMDTO.ProductId.ToString()));
                        List<ProductDTO> productDTOListProduct = productListProduct.GetAllProducts(searchProductParameters);
                        log.Error("productDTOListProduct count" + productDTOListProduct.Count());
                        ProductList productListChildProduct = new ProductList(executionContext);
                        List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchChildProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                        searchChildProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchChildProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, bOMDTO.ChildProductId.ToString()));
                        List<ProductDTO> productDTOListChildProduct = productListChildProduct.GetAllProducts(searchChildProductParameters);
                        log.Error("productDTOListChildProduct count" + productDTOListChildProduct.Count());
                        string uom = bOMDataHandler.GetUOMById(bOMDTO.UOMId);
                        if (productDTOListProduct != null && productDTOListProduct.Count > 0 && productDTOListChildProduct != null && productDTOListChildProduct.Count > 0)
                        {
                            BOMExcelDTO rowBOMExcelDTO = new BOMExcelDTO(productDTOListProduct.FirstOrDefault().Code, productDTOListProduct.FirstOrDefault().Description,
                            productDTOListChildProduct.FirstOrDefault().Code, productDTOListChildProduct.FirstOrDefault().Description, bOMDTO.Quantity, uom);
                            bOMViewExcelDTODefination.Configure(rowBOMExcelDTO);
                            Row row = new Row();
                            bOMViewExcelDTODefination.Serialize(row, rowBOMExcelDTO);
                            sheet.AddRow(row);
                        }
                    }
                }
            }
            log.LogMethodExit();
            return sheet;
        }


        public Sheet BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            BOMViewExcelDTODefination bOMViewExcelDTODefination = new BOMViewExcelDTODefination(executionContext, "");
            List<BOMDTO> rowBOMDTOList = new List<BOMDTO>();
            Sheet responseSheet = null;

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    BOMExcelDTO rowBOMExcelDTO = (BOMExcelDTO)bOMViewExcelDTODefination.Deserialize(sheet[0], sheet[i], ref index);
                    ProductList productListProduct = new ProductList(executionContext);
                    List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                    searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, rowBOMExcelDTO.ProductCode));
                    List<ProductDTO> productDTOListProduct = productListProduct.GetAllProducts(searchProductParameters);
                    if (productDTOListProduct == null && productDTOListProduct.Count < 1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 111);
                        log.Error("Parent Product Not Found");
                        throw new ValidationException(errorMessage);
                    }
                    ProductList productListChildProduct = new ProductList(executionContext);
                    List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchChildProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                    searchChildProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchChildProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, rowBOMExcelDTO.ChildProductCode));
                    List<ProductDTO> productDTOListChildProduct = productListChildProduct.GetAllProducts(searchChildProductParameters);
                    if (productDTOListChildProduct == null && productDTOListChildProduct.Count < 1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 111);
                        log.Error("Child Product Not Found");
                        throw new ValidationException(errorMessage);
                    }
                    BOMDataHandler bOMDataHandler = new BOMDataHandler(sqlTransaction);
                    BOMList bomListBL = new BOMList(executionContext);
                    List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchBOMParameters = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.SITEID, executionContext.GetSiteId().ToString()));
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.PRODUCT_ID, productDTOListProduct.First().ProductId.ToString()));
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID, productDTOListChildProduct.First().ProductId.ToString()));
                    List<BOMDTO> bOMDTOList = bomListBL.GetAllBOMs(searchBOMParameters);
                    int uomId = bOMDataHandler.GetUOMByName(rowBOMExcelDTO.UOM);
                    BOMDTO rowBOMDTO = null;
                    if (bOMDTOList != null && bOMDTOList.Any())
                    {
                        rowBOMDTO = new BOMDTO(bOMDTOList.First().BOMId, bOMDTOList.First().ProductId, bOMDTOList.First().ChildProductId,
                        rowBOMExcelDTO.Quantity, executionContext.GetSiteId(), string.Empty, false, bOMDTOList.First().MasterEntityId, true, bOMDTOList.First().PreparationRemarks, uomId);
                        rowBOMDTO.IsChanged = true;
                    }
                    else
                    {
                        rowBOMDTO = new BOMDTO(-1, productDTOListProduct.First().ProductId, productDTOListChildProduct.First().ProductId,
                       rowBOMExcelDTO.Quantity, executionContext.GetSiteId(), string.Empty, false, -1, true, string.Empty, uomId);
                        rowBOMDTO.IsChanged = true;
                    }
                    rowBOMDTOList.Add(rowBOMDTO);
                    if (rowBOMDTOList != null && rowBOMDTOList.Any())
                    {
                        BOMList bOMListBL = new BOMList(executionContext, rowBOMDTOList);
                        bOMListBL.SaveUpdateProductBOM(sqlTransaction);
                        rowBOMDTOList = new List<BOMDTO>();
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Empty;
                    string seperator = string.Empty;
                    //In case of exception we add a cell to status object with cell value saved
                    if (responseSheet == null)
                    {
                        responseSheet = new Sheet();
                        responseSheet.AddRow(sheet[0]);
                        responseSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Status")));
                    }
                    responseSheet.AddRow(sheet[i]);

                    if (ex is ValidationException)
                    {
                        foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                        {
                            errorMessage += seperator;
                            errorMessage += validationError.Message;
                            seperator = ", ";
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    responseSheet[responseSheet.Rows.Count - 1].AddCell(new Cell("Failed: " + errorMessage));
                    log.Error(errorMessage);
                    log.LogVariableState("Row", sheet[i]);
                    rowBOMDTOList = new List<BOMDTO>();
                    continue;
                }
            }
            log.LogMethodExit(responseSheet);
            return responseSheet;
        }
    }
}
