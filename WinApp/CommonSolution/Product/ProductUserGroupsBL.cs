/********************************************************************************************
 * Project Name - Product
 * Description  - Business Logic of ProductUserGroups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.00    11-Nov-2020      Abhishek               Created 

 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Parafait.Languages;
//using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductUserGroupsBL
    {
        private ProductUserGroupsDTO productUserGroupsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ProductUserGroupsBL class
        /// </summary>
        /// <param name="executionContext"></param>
        private ProductUserGroupsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductUserGroups id as the parameter
        /// Would fetch the ProductUserGroups object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of ProductUserGroups Object </param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductUserGroupsBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
                                   bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductUserGroupsDataHandler productUserGroupsDataHandler = new ProductUserGroupsDataHandler(sqlTransaction);
            productUserGroupsDTO = productUserGroupsDataHandler.GetProductUserGroupsDTO(id);
            if (productUserGroupsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductUserGroups", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords == false ||
               productUserGroupsDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            ProductUserGroupsMappingListBL productUserGroupsMappingListBL = new ProductUserGroupsMappingListBL(executionContext);
            productUserGroupsDTO.ProductUserGroupsMappingDTOList = productUserGroupsMappingListBL.GetProductUserGroupsDTOListOfRecipe(new List<int> { id }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();         
        }

        // <summary>
        // Creates ProductUserGroupsBL object using the ProductUserGroupsDTO
        // </summary>
        // <param name = "executionContext" > ExecutionContext object is passed as parameter</param>
        // <param name = "ProductUserGroupsDTO" > ProductUserGroupsDTO object is passed as parameter</param>
        public ProductUserGroupsBL(ExecutionContext executionContext, ProductUserGroupsDTO productUserGroupsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productUserGroupsDTO);
            this.productUserGroupsDTO = productUserGroupsDTO;
            log.LogMethodExit();
        }    

        /// <summary>
        /// Saves the AppUIPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productUserGroupsDTO.IsChangedRecursive == false &&
                productUserGroupsDTO.ProductUserGroupsId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ProductUserGroupsDataHandler productUserGroupsDataHandler = new ProductUserGroupsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (productUserGroupsDTO.ProductUserGroupsId < 0)
            {
                log.LogVariableState("ProductUserGroupsDTO", productUserGroupsDTO);
                productUserGroupsDTO = productUserGroupsDataHandler.Insert(productUserGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productUserGroupsDTO.AcceptChanges();
            }
            else if(productUserGroupsDTO.IsChanged)
            {
                log.LogVariableState("ProductUserGroupsDTO", productUserGroupsDTO);
                productUserGroupsDTO = productUserGroupsDataHandler.Update(productUserGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productUserGroupsDTO.AcceptChanges();
            }

            if (productUserGroupsDTO.ProductUserGroupsMappingDTOList != null &&
                  productUserGroupsDTO.ProductUserGroupsMappingDTOList.Count != 0)
            {
                foreach (ProductUserGroupsMappingDTO productUserGroupsMappingDTO in productUserGroupsDTO.ProductUserGroupsMappingDTOList)
                {
                    productUserGroupsMappingDTO.ProductUserGroupsId = productUserGroupsDTO.ProductUserGroupsId;
                }
                ProductUserGroupsMappingListBL productUserGroupsMappingListBL = new ProductUserGroupsMappingListBL(executionContext, productUserGroupsDTO.ProductUserGroupsMappingDTOList);
                productUserGroupsMappingListBL.Save(sqlTransaction);
            }           
            log.LogMethodExit();
        }
    
        /// <summary>
        /// Validates the ProductUserGroupsDTO,ProductUserGroupsMappingDTO - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();          
            if (productUserGroupsDTO.ProductUserGroupsMappingDTOList != null)
            {
                foreach (var productUserGroupsMappingDTO in productUserGroupsDTO.ProductUserGroupsMappingDTOList)
                {
                    if (productUserGroupsMappingDTO.IsChanged)
                    {
                        ProductUserGroupsMappingBL productUserGroupsMappingBL = new ProductUserGroupsMappingBL(executionContext, productUserGroupsMappingDTO);
                        validationErrorList.AddRange(productUserGroupsMappingBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductUserGroupsDTO GetProductUserGroupsDTO { get { return productUserGroupsDTO; } }
    }

    public class ProductUserGroupsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductUserGroupsDTO> productUserGroupsDTOList = new List<ProductUserGroupsDTO>(); 
        
        /// <summary>
        /// Parameterized constructor of ProductUserGroupsListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>                                                                                                                                     
        public ProductUserGroupsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ProductUserGroupsDTOList">ProductUserGroupsDTO List is passed as parameter </param>
        public ProductUserGroupsListBL(ExecutionContext executionContext, 
                                       List<ProductUserGroupsDTO> productUserGroupsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productUserGroupsDTOList);
            this.productUserGroupsDTOList = productUserGroupsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the productUserGroupsDTODTO list
        /// </summary>
        public List<ProductUserGroupsDTO> GetAllProductUserGroupsDTOList(List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> searchParameters,
                                                                         bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            ProductUserGroupsDataHandler productUserGroupsDataHandler = new ProductUserGroupsDataHandler(sqlTransaction);
            List<ProductUserGroupsDTO> productUserGroupsDTOList = productUserGroupsDataHandler.GetProductUserGroupsDTOList(searchParameters);
            if (loadChildRecords == false ||
                productUserGroupsDTOList == null ||
                productUserGroupsDTOList.Count > 0 == false)
            {
                log.LogMethodExit(productUserGroupsDTOList, "Child records are not loaded.");
                return productUserGroupsDTOList;
            }
            BuildProductUserGroupsDTOList(productUserGroupsDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(productUserGroupsDTOList);
            return productUserGroupsDTOList;
        }

        private void BuildProductUserGroupsDTOList(List<ProductUserGroupsDTO> productUserGroupsDTOList, bool activeChildRecords,
                                                    SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(productUserGroupsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductUserGroupsDTO> productUserGroupsDTOIdMap = new Dictionary<int, ProductUserGroupsDTO>();
            List<int> productUserGroupsIdList = new List<int>();
            for (int i = 0; i < productUserGroupsDTOList.Count; i++)
            {
                if (productUserGroupsDTOIdMap.ContainsKey(productUserGroupsDTOList[i].ProductUserGroupsId))
                {
                    continue;
                }
                productUserGroupsDTOIdMap.Add(productUserGroupsDTOList[i].ProductUserGroupsId, productUserGroupsDTOList[i]);
                productUserGroupsIdList.Add(productUserGroupsDTOList[i].ProductUserGroupsId);
            }

            ProductUserGroupsMappingListBL productUserGroupsMappingListBL = new ProductUserGroupsMappingListBL(executionContext);
            List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList = productUserGroupsMappingListBL.GetProductUserGroupsDTOListOfProduct(productUserGroupsIdList, activeChildRecords, sqlTransaction);
            if (productUserGroupsMappingDTOList != null && productUserGroupsMappingDTOList.Count > 0)
            {
                for (int i = 0; i < productUserGroupsMappingDTOList.Count; i++)
                {
                    if (productUserGroupsDTOIdMap.ContainsKey(productUserGroupsMappingDTOList[i].ProductUserGroupsId) == false)
                    {
                        continue;
                    }
                    ProductUserGroupsDTO productUserGroupsDTO = productUserGroupsDTOIdMap[productUserGroupsMappingDTOList[i].ProductUserGroupsId];
                    if (productUserGroupsDTO.ProductUserGroupsMappingDTOList == null)
                    {
                        productUserGroupsDTO.ProductUserGroupsMappingDTOList = new List<ProductUserGroupsMappingDTO>();
                    }
                    productUserGroupsDTO.ProductUserGroupsMappingDTOList.Add(productUserGroupsMappingDTOList[i]);
                }
            }
            log.LogMethodExit();          
        }            
    }
}






















