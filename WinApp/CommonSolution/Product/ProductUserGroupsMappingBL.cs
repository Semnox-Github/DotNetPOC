/********************************************************************************************
 * Project Name - Product
 * Description  - Business logic file for ProductUserGroupsMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.00   11-Nov-2020     Abhishek               Created 
 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for ProductUserGroupsMapping class.
    /// </summary>
    public class ProductUserGroupsMappingBL
    {
        private ProductUserGroupsMappingDTO productUserGroupsMappingDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ProductUserGroupsMapping class
        /// </summary>
        private ProductUserGroupsMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Product User Groups Mapping id as the parameter
        /// Would fetch the Product User Groups Mapping object from the database based on the id passed. 
        /// </summary>
        /// <param name="Id">Product User Groups Mapping id</param>
        public ProductUserGroupsMappingBL(ExecutionContext executionContext, int Id, SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(Id);
            ProductUserGroupsMappingDataHandler productUserGroupsMappingDataHandler = new ProductUserGroupsMappingDataHandler(sqlTransaction);
            productUserGroupsMappingDTO = productUserGroupsMappingDataHandler.GetProductUserGroupsMappingDTO(Id);
            if (productUserGroupsMappingDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Product User Groups Mapping", Id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(productUserGroupsMappingDTO);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productUserGroupsMappingDTO"></param>
        public ProductUserGroupsMappingBL(ExecutionContext executionContext, ProductUserGroupsMappingDTO productUserGroupsMappingDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(productUserGroupsMappingDTO);
            this.productUserGroupsMappingDTO = productUserGroupsMappingDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the product user groups mapping
        /// Checks if the productUserGroupsMappingDTO id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productUserGroupsMappingDTO.IsChanged == false
                   && productUserGroupsMappingDTO.ProductUserGroupsMappingId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ProductUserGroupsMappingDataHandler productUserGroupsMappingDataHandler = new ProductUserGroupsMappingDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (productUserGroupsMappingDTO.ProductUserGroupsMappingId < 0)
            {
                productUserGroupsMappingDTO = productUserGroupsMappingDataHandler.Insert(productUserGroupsMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productUserGroupsMappingDTO.AcceptChanges();
            }
            else
            {
                if (productUserGroupsMappingDTO.IsChanged)
                {
                    productUserGroupsMappingDTO = productUserGroupsMappingDataHandler.Update(productUserGroupsMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productUserGroupsMappingDTO.AcceptChanges();
                }
            }

        }

        /// <summary>
        /// Validates the productUserGroupsMappingDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationError</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }    

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductUserGroupsMappingDTO GetProductUserGroupsMappingDTO { get { return productUserGroupsMappingDTO; } }
    }

    // <summary>
    /// Manages the list of ProductUserGroupsMapping
    /// </summary>
    public class ProductUserGroupsMappingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList = new List<ProductUserGroupsMappingDTO>(); // To be initialized

        /// <summary>
        /// Parameterized constructor of ProductUserGroupsMappingListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public ProductUserGroupsMappingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="productUserGroupsMappingDTOList">ProductUserGroupsMapping DTO List as parameter </param>
        public ProductUserGroupsMappingListBL(ExecutionContext executionContext, List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productUserGroupsMappingDTOList);
            this.productUserGroupsMappingDTOList = productUserGroupsMappingDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the ProductUserGroupsMapping DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of ProductUserGroupsMappingDTO </returns>
        public List<ProductUserGroupsMappingDTO> GetProductUserGroupsMappingDTOList(List<KeyValuePair<ProductUserGroupsMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductUserGroupsMappingDataHandler productUserGroupsMappingDataHandler = new ProductUserGroupsMappingDataHandler(sqlTransaction);
            List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList = productUserGroupsMappingDataHandler.GetProductUserGroupsMappingDTOList(searchParameters);
            log.LogMethodExit(productUserGroupsMappingDTOList);
            return productUserGroupsMappingDTOList;
        }

        /// <summary>
        /// Gets the ProductUserGroupsMappingDTO List for productUserGroupsIdList 
        /// </summary>
        /// <param name="productUserGroupsIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductUserGroupsMappingDTO</returns>
        public List<ProductUserGroupsMappingDTO> GetProductUserGroupsDTOListOfRecipe(List<int> productUserGroupsIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productUserGroupsIdList, activeRecords, sqlTransaction);
            ProductUserGroupsMappingDataHandler productUserGroupsMappingDataHandler = new ProductUserGroupsMappingDataHandler(sqlTransaction);
            List<ProductUserGroupsMappingDTO> productUserGroupsMappingList = productUserGroupsMappingDataHandler.GetProductUserGroupsMappingDTOListOfProduct(productUserGroupsIdList, activeRecords);
            log.LogMethodExit(productUserGroupsMappingList);
            return productUserGroupsMappingList;
        }

        /// <summary>
        /// Gets the ProductUserGroupsMappingDTO List for productUserGroupsIdList 
        /// </summary>
        /// <param name="productUserGroupsIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductUserGroupsMappingDTO</returns>
        public List<ProductUserGroupsMappingDTO> GetProductUserGroupsDTOListOfProduct(List<int> productUserGroupsIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productUserGroupsIdList, activeRecords, sqlTransaction);
            ProductUserGroupsMappingDataHandler productUserGroupsMappingDataHandler = new ProductUserGroupsMappingDataHandler(sqlTransaction);
            List<ProductUserGroupsMappingDTO> productUserGroupsMappingList = productUserGroupsMappingDataHandler.GetProductUserGroupsMappingDTOListOfProduct(productUserGroupsIdList, activeRecords);
            log.LogMethodExit(productUserGroupsMappingList);
            return productUserGroupsMappingList;
        }

        // <summary>
        // Saves the  list of productUserGroupsMapping DTO.
        // </summary>
        // <param name = "sqlTransaction" > sqlTransaction object</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productUserGroupsMappingDTOList == null ||
                productUserGroupsMappingDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < productUserGroupsMappingDTOList.Count; i++)
            {
                var productUserGroupsMappingDTO = productUserGroupsMappingDTOList[i];
                if (productUserGroupsMappingDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ProductUserGroupsMappingBL productUserGroupsMappingBL = new ProductUserGroupsMappingBL(executionContext, productUserGroupsMappingDTO);
                    productUserGroupsMappingBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ProductUserGroupsMappingDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ProductUserGroupsMappingDTO", productUserGroupsMappingDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}



