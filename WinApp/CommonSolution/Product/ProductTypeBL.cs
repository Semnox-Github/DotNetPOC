/********************************************************************************************
 * Project Name - ProductType DTO
 * Description  - Data object of Product Type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.0      24-Jan-2019   Deeksha                 Created 
 *2.60        08-Feb-2019   Indrajeet Kumar         Modification in Parameterized Constructor, Created Default Constructor and SaveUpdateCheckOutPricesList() Method
 *2.70.0      21-Feb-2019   Guru S A                Booking phase 2 changes
 *2.70        29-Jun-2019   Indrajeet Kumar         Created DeleteProductsList() & DeleteProductType() - Implement for Hard Deletion.
 *            10-Jul-2019   Akshay Gulaganji        modified DeleteProductsList() method
 *2.120.1     24-Jun-2021   Abhishek                Modified : added GetProductTypeModuleLastUpdateTime()             
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ProductTypeBL
    {
        private ProductTypeDTO productTypeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Constructor for executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ProductTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ProductType id as the parameter
        /// Would fetch the ProductType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productTypeId">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ProductTypeBL(ExecutionContext executionContext, int productTypeId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productTypeId, sqlTransaction);
            ProductTypeDataHandler productTypeDataHandler = new ProductTypeDataHandler(sqlTransaction);
            productTypeDTO = productTypeDataHandler.GetProductTypeDTO(productTypeId);
            if (productTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Product Type ", productTypeId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductTypeBL object using the ProductTypeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productTypeDTO">ProductTypeDTO object</param>
        public ProductTypeBL(ExecutionContext executionContext, ProductTypeDTO productTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productTypeDTO);
            this.productTypeDTO = productTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productTypeDTO.IsChanged == false
                && productTypeDTO.ProductTypeId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ProductTypeDataHandler productTypeDataHandler = new ProductTypeDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            } 
            if (productTypeDTO.ProductTypeId < 0)
            {               
                productTypeDTO = productTypeDataHandler.Insert(productTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());                
                productTypeDTO.AcceptChanges();
            }
            else
            {
                if (productTypeDTO.IsChanged)
                {
                    productTypeDTO = productTypeDataHandler.Update(productTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ProductTypeDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// Hard Deletions for ProductType
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                ProductTypeDataHandler productTypeDataHandler = new ProductTypeDataHandler(sqlTransaction);
                productTypeDataHandler.Delete(productTypeDTO.ProductTypeId);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductTypeDTO ProductTypeDTO { get { return productTypeDTO; } }
    }

    /// <summary>
    /// Manages the list of ProductType
    /// </summary>
    public class ProductTypeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductTypeDTO> productTypeDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductTypeListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProductTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and productTypeDTOList
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productTypeDTOList"></param>
        public ProductTypeListBL(ExecutionContext executionContext, List<ProductTypeDTO> productTypeDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, productTypeDTOList);
            this.productTypeDTOList = productTypeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductType list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ProductTypeDTO> GetProductTypeDTOList(List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductTypeDataHandler productTypeDataHandler = new ProductTypeDataHandler(sqlTransaction);
            List<ProductTypeDTO> productTypeDTOList = productTypeDataHandler.GetProductTypeDTOList(searchParameters);
            log.LogMethodExit(productTypeDTOList);
            return productTypeDTOList;
        }

        /// <summary>
        /// Save Update ProductType List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            { 
                if (productTypeDTOList != null)
                {
                    foreach (ProductTypeDTO productTypeDTO in productTypeDTOList)
                    {
                        ProductTypeBL productTypeBL = new ProductTypeBL(executionContext, productTypeDTO);
                        productTypeBL.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the ProductTpyeList details based on ProductTypeId
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productTypeDTOList != null && productTypeDTOList.Count > 0)
            {
                foreach (ProductTypeDTO productTypeDTO in productTypeDTOList)
                {
                    if (productTypeDTO.IsActive == false && productTypeDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProductTypeBL productTypeBL = new ProductTypeBL(executionContext,productTypeDTO);
                                productTypeBL.Delete(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 546));
                                }
                                else
                                {
                                    throw;
                                }
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

        public DateTime? GetProductTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ProductTypeDataHandler productTypeDataHandler = new ProductTypeDataHandler();
            DateTime? result = productTypeDataHandler.GetProductTypeModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
