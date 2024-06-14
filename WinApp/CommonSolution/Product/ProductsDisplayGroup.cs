
/********************************************************************************************
 * Project Name - ProductsDisplayGroup
 * Description  - Data object of the ProductsDisplayGroupBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        21-Nov-2016    Amaresh           Created
 *2.50        10-Jan-2019    Jagan Mohana      Created the constructor ProductDisplayGroupList and
 *                                             added new methods SaveUpdateProductDisplayGroupList and IsExistDisplayGroup
  *2.60       15-Mar-2019    Nitin Pai         Added new search for PRODUCT_TYPE_NAME_LIST
 *            11-Apr-2019    Archana           SqlTransaction parameter is added to Save()
 *            23-Apr-2019    Lakshminarayana   Changed to handle batch save
 *2.60.2      29-May-2019    Jagan Mohan       Code merge from Development to WebManagementStudio
 *********************************************************************************************
 *2.70        29-June-2019   Indrajeet Kumar   Created DeleteProductDisplayGroupList() for delete. 
 *2.80        11-Mar-2020    Vikas Dwivedi     Modified as per the Standard for Phase 1 changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductsDisplayGroup will creates and modifies the ProductsDisplayGroup
    /// </summary>
    public class ProductsDisplayGroup
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsDisplayGroupDTO productsDisplayGroupDTO;
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductsDisplayGroup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductsDisplayGroup(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductsDisplayGroupDataHandler productsDisplayGroupDataHandler = new ProductsDisplayGroupDataHandler(sqlTransaction);
            productsDisplayGroupDTO = productsDisplayGroupDataHandler.GetProductsDisplayGroup(id);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductsDisplayGroup DTO parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="productsDisplayGroupDTO">Parameter of the type ProductsDisplayGroupDTO</param>
        public ProductsDisplayGroup(ExecutionContext executionContext, ProductsDisplayGroupDTO productsDisplayGroupDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, productsDisplayGroupDTO);
            this.productsDisplayGroupDTO = productsDisplayGroupDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ProductsDisplayGroup. if any of the field values are not valid returns a list of ValidationErrors.
        /// </summary>
        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrors = new List<ValidationError>();
            if (productsDisplayGroupDTO.DisplayGroupId < 0)
            {
                ValidationError validationError = new ValidationError("ProductsDisplayGroup", "DisplayGroupId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Group")));
                validationErrors.Add(validationError);
            }
            log.LogMethodExit(validationErrors);
            return validationErrors;
        }

        /// <summary>
        /// Saves the ProductsDisplayGroup  
        /// ProductsDisplayGroup will be inserted if id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productsDisplayGroupDTO.IsChanged == false
                && productsDisplayGroupDTO.Id > -1)
            {
                log.LogMethodExit(null, "productsDisplayGroupDTO is not changed.");
                return;
            }
            ProductsDisplayGroupDataHandler productsDisplayGroupDataHandler = new ProductsDisplayGroupDataHandler(sqlTransaction);
            productsDisplayGroupDataHandler.Save(productsDisplayGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the productsDisplayGroupDTO based on Id
        /// </summary>
        public int Delete(int id)
        {
            log.LogMethodEntry(id);
            int result = -1;
            try
            {
                ProductsDisplayGroupDataHandler productsDisplayGroupDataHandler = new ProductsDisplayGroupDataHandler();
                result = productsDisplayGroupDataHandler.DeleteProductsDisplayGroup(id);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductsDisplayGroupDTO ProductsDisplayGroupDTO
        {
            get
            {
                return productsDisplayGroupDTO;
            }
        }

        /// <summary>
        /// Valadiate the Display group exist or not based on the displayGroupId for the pariculat product
        /// </summary>
        /// <param name="displayGroupId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool IsExistDisplayGroup(int displayGroupId, int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(displayGroupId, productId);
            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParam = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
            searchParam.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productId.ToString()));
            searchParam.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, displayGroupId.ToString()));

            List<ProductsDisplayGroupDTO> productDisplayGroupListOnDisplay = productsDisplayGroupList.GetAllProductsDisplayGroup(searchParam, sqlTransaction);

            if (productDisplayGroupListOnDisplay != null && productDisplayGroupListOnDisplay.Count > 0)
            {
                return true;
            }
            log.LogMethodExit();
            return false;
        }

    }

    /// <summary>
    /// Manages the list of ProductsDisplayGroup List
    /// </summary>
    public class ProductsDisplayGroupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<ProductsDisplayGroupDTO> productsDisplayGroupList = new List<ProductsDisplayGroupDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// default constructor
        /// </summary>
        public ProductsDisplayGroupList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductsDisplayGroupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executioncontext"></param>
        /// <param name="productsDisplayGroupList"></param>
        public ProductsDisplayGroupList(ExecutionContext executioncontext, List<ProductsDisplayGroupDTO> productsDisplayGroupList)
            :this(executioncontext)
        {
            log.LogMethodEntry(executioncontext, productsDisplayGroupList);
            this.productsDisplayGroupList = productsDisplayGroupList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the ProductsDisplayGroup DTO
        /// </summary>
        public ProductsDisplayGroupDTO GetProductsDisplayGroup(int id, bool loadChildRecord = false)
        {
            log.LogMethodEntry(id, loadChildRecord);
            ProductsDisplayGroupDataHandler productsDisplayGroupDataHandler = new ProductsDisplayGroupDataHandler();
            ProductsDisplayGroupDTO result = productsDisplayGroupDataHandler.GetProductsDisplayGroupByDisplayGroupId(id, loadChildRecord);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ProductsDisplayGroupDTO List
        /// </summary>
        public List<ProductsDisplayGroupDTO> GetAllProductsDisplayGroup(List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductsDisplayGroupDataHandler productsDisplayGroupDataHandler = new ProductsDisplayGroupDataHandler(sqlTransaction);
            List<ProductsDisplayGroupDTO> result = productsDisplayGroupDataHandler.GetProductsDisplayGroupList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ProductDTO List for ManualProductId List
        /// </summary>
        /// <param name="manualProductIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<ProductsDisplayGroupDTO> GetProductsDisplayGroupDTOOfProducts(List<int> productIdList, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeChildRecords, sqlTransaction);
            ProductsDisplayGroupDataHandler productsDisplayGroupDataHandler = new ProductsDisplayGroupDataHandler(sqlTransaction);
            List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = productsDisplayGroupDataHandler.GetProductsDisplayGroupDTOListOfProducts(productIdList, activeChildRecords);
            log.LogMethodExit(productsDisplayGroupDTOList);
            return productsDisplayGroupDTOList;
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the ProductsDisplayGroup List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productsDisplayGroupList == null ||
                productsDisplayGroupList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < productsDisplayGroupList.Count; i++)
            {
                var productsDisplayGroupDTO = productsDisplayGroupList[i];
                if (productsDisplayGroupDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ProductsDisplayGroup productsDisplayGroup = new ProductsDisplayGroup(executionContext, productsDisplayGroupDTO);
                    bool isExistDisplayGroup = productsDisplayGroup.IsExistDisplayGroup(productsDisplayGroupDTO.DisplayGroupId, productsDisplayGroupDTO.ProductId);
                    if (!(productsDisplayGroupDTO.DisplayGroupId == -1 && isExistDisplayGroup))
                    {
                        productsDisplayGroup.Save(sqlTransaction);
                    }
                    else
                    {
                           throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ProductsDisplayGroupDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ProductsDisplayGroupDTO", productsDisplayGroupDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        

        /// <summary>
        /// Delete the DeleteProductDisplayGroupList details based on Id
        /// </summary>
        public void DeleteProductDisplayGroupList()
        {
            log.LogMethodEntry();
            if (productsDisplayGroupList != null && productsDisplayGroupList.Any())
            {
                foreach (ProductsDisplayGroupDTO productsDisplayGroupDTO in productsDisplayGroupList)
                {
                    if (productsDisplayGroupDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProductsDisplayGroup productsDisplayGroup = new ProductsDisplayGroup(executionContext);
                                productsDisplayGroup.Delete(productsDisplayGroupDTO.Id);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
