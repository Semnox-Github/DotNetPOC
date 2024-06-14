/********************************************************************************************
 * Project Name - Products Special Pricing
 * Description  - Bussiness logic of Products Special Pricing
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        29-Jan-2019   Akshay Gulaganji    Created 
 *            29-Jun-2019   Akshay Gulaganji    Added sqlTransaction, DeleteProductsSpecialPricing() and DeleteProductsSpecialPricingList() method
 ********************************************************************************************/
using Semnox.Core.Utilities;
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
    /// Products Special Pricing will creates and modifies the Products Special Pricing
    /// </summary>
    public class ProductsSpecialPricingBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsSpecialPricingDTO productsSpecialPricingDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductsSpecialPricingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productsSpecialPricingDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="productsSpecialPricingDTO">Parameter of the type ProductsSpecialPricingDTO</param>
        public ProductsSpecialPricingBL(ExecutionContext executionContext, ProductsSpecialPricingDTO productsSpecialPricingDTO)
        {
            log.LogMethodEntry(productsSpecialPricingDTO, executionContext);
            this.executionContext = executionContext;
            this.productsSpecialPricingDTO = productsSpecialPricingDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the productsSpecialPricingDTO record
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            ProductsSpecialPricingHandler productsSpecialPricingHandler = new ProductsSpecialPricingHandler(sqlTransaction);
            if (productsSpecialPricingDTO.ProductPricingId < 0)
            {
                int productPricingId = productsSpecialPricingHandler.InsertProductsSpecialPricing(productsSpecialPricingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productsSpecialPricingDTO.ProductPricingId = productPricingId;
                productsSpecialPricingDTO.AcceptChanges();
            }
            else
            {
                if (productsSpecialPricingDTO.ProductPricingId > 0 && productsSpecialPricingDTO.IsChanged == true)
                {
                    productsSpecialPricingHandler.UpdateProductsSpecialPricing(productsSpecialPricingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the ProductsSpecialPricing details based on ProductPricingId
        /// </summary>
        /// <param name="productPricingId">ProductPricingId</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>        
        public void DeleteProductsSpecialPricing(int productPricingId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productPricingId, sqlTransaction);
            try
            {
                ProductsSpecialPricingHandler productsSpecialPricingHandler = new ProductsSpecialPricingHandler(sqlTransaction);
                productsSpecialPricingHandler.DeleteProductSpecialPricing(productPricingId);
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

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if(productsSpecialPricingDTO != null)
            {
                if(productsSpecialPricingDTO.PricingId < 0)
                {
                    validationErrorList.Add(new ValidationError("ProductsSpecialPricing", "PricingId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Pricing Option"))));
                }
                List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParameters = new List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.ACTIVE_FLAG, "1"));
                searchParameters.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRICING_ID, productsSpecialPricingDTO.PricingId.ToString()));
                ProductsSpecialPricingHandler productsSpecialPricingHandler = new ProductsSpecialPricingHandler(sqlTransaction);
                List<ProductsSpecialPricingDTO> productsSpecialPricingDTOs = productsSpecialPricingHandler.GetProductsSpecialPricingList(searchParameters);
                if(productsSpecialPricingDTO.ProductPricingId == -1 && productsSpecialPricingDTOs != null && productsSpecialPricingDTOs.Any())
                {
                    log.Debug("Duplicate entries detail");
                    validationErrorList.Add(new ValidationError("ProductsSpecialPricing", "Pricing Option", MessageContainerList.GetMessage(executionContext, "Duplicate Pricing Option is not allowed", MessageContainerList.GetMessage(executionContext, "Pricing Option"))));
                }
                if (productsSpecialPricingDTO.ProductPricingId > -1 && productsSpecialPricingDTOs != null && productsSpecialPricingDTOs.Any(x=>x.ProductPricingId != productsSpecialPricingDTO.ProductPricingId))
                {
                    log.Debug("Duplicate entries detail");
                    validationErrorList.Add(new ValidationError("ProductsSpecialPricing", "Pricing Option", MessageContainerList.GetMessage(executionContext, "Duplicate Pricing Option is not allowed", MessageContainerList.GetMessage(executionContext, "Pricing Option"))));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }
    /// <summary>
    /// Manages the list of Products Special Pricing
    /// </summary>
    public class ProductsSpecialPricingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductsSpecialPricingDTO> productsSpecialPricingList;
        private ExecutionContext executionContext;
        /// <summary>
        /// Paramterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductsSpecialPricingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            this.productsSpecialPricingList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor to initialize ProductsSpecialPricingList and executionContext
        /// </summary>
        public ProductsSpecialPricingListBL(ExecutionContext executionContext, List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            log.LogMethodEntry(productsSpecialPricingList, executionContext);
            this.productsSpecialPricingList = productsSpecialPricingList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Products Special Pricing list
        /// </summary>
        public List<ProductsSpecialPricingDTO> GetAllProductsSpecialPricing(List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductsSpecialPricingHandler productsSpecialPricingHandler = new ProductsSpecialPricingHandler(sqlTransaction);
            log.LogMethodExit();
            return productsSpecialPricingHandler.GetProductsSpecialPricingList(searchParameters);
        }

        /// <summary>
        /// Saves the Products Special Pricing
        /// </summary>
        public void SaveUpdateProductsSpecialPricingList()
        {
            log.LogMethodEntry();
            try
            {
                if (productsSpecialPricingList != null && productsSpecialPricingList.Count > 0)
                {
                    foreach (ProductsSpecialPricingDTO productsSpecialPricingDto in productsSpecialPricingList)
                    {
                        ProductsSpecialPricingBL productsSpecialPricingBLObj = new ProductsSpecialPricingBL(executionContext, productsSpecialPricingDto);
                        productsSpecialPricingBLObj.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Hard Deletions for ProductsSpecialPricing
        /// </summary>
        public void DeleteProductsSpecialPricingList()
        {
            log.LogMethodEntry();
            if (productsSpecialPricingList != null && productsSpecialPricingList.Count > 0)
            {
                foreach (ProductsSpecialPricingDTO productsSpecialPricingDTO in productsSpecialPricingList)
                {
                    if (productsSpecialPricingDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProductsSpecialPricingBL productsSpecialPricingBL = new ProductsSpecialPricingBL(executionContext);
                                productsSpecialPricingBL.DeleteProductsSpecialPricing(productsSpecialPricingDTO.ProductPricingId, parafaitDBTrx.SQLTrx);
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
