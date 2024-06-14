/********************************************************************************************
 * Project Name - Products Availability BL
 * Description  - Business logic for ProductsAvailabilityStatus functionality
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        05-Mar-2019      Nitin Pai      86-68 Created 
 *2.110.00    01-Dec-2020      Abhishek       Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using System.Globalization;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ProductsAvailabilityBL
    {
        private ProductsAvailabilityDTO productsAvailabilityDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsAvailabilityDataHandler productsAvailabilityyDataHandler;

        /// <summary>
        /// Parameterized constructor of ProductsAvailabilityBL class
        /// </summary>
        public ProductsAvailabilityBL(ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            productsAvailabilityyDataHandler = new ProductsAvailabilityDataHandler(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductsAvailabilityBL object using the ProductsAvailabilityDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productsAvailabilityDTO">ProductsAvailabilityDTO object</param>
        public ProductsAvailabilityBL(ExecutionContext executionContext, ProductsAvailabilityDTO productsAvailabilityDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext, sqlTransaction)
        {
            log.LogMethodEntry(executionContext, productsAvailabilityDTO);
            this.productsAvailabilityDTO = productsAvailabilityDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Products Availability  id as the parameter
        /// Would fetch the ProductsAvailability object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id of Products Availability  </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProductsAvailabilityBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            productsAvailabilityyDataHandler = new ProductsAvailabilityDataHandler(sqlTransaction);
            productsAvailabilityDTO = productsAvailabilityyDataHandler.GetProductsAvailabilityDTO(id);
            if (productsAvailabilityDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Products Availability ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductsAvailabilityDTO GetProductsAvailabilityDTO { get { return productsAvailabilityDTO; } }

        public string Save(string approvedBy,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productsAvailabilityDTO);
            if (productsAvailabilityDTO.IsChanged == false
                && productsAvailabilityDTO.Id > -1)
            {
                log.LogMethodExit();
                return "Nothing to save.";
            }  
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (productsAvailabilityDTO.Id < 0)
            {
                productsAvailabilityDTO = productsAvailabilityyDataHandler.Insert(productsAvailabilityDTO, approvedBy, executionContext.GetUserId(), executionContext.GetSiteId());
                productsAvailabilityDTO.LastUpdateDate = DateTime.Now;
                productsAvailabilityDTO.AcceptChanges();
            }
            else
            {
                if (productsAvailabilityDTO.IsChanged)
                {
                    productsAvailabilityDTO = productsAvailabilityyDataHandler.Update(productsAvailabilityDTO, approvedBy, executionContext.GetUserId(), executionContext.GetSiteId());
                    productsAvailabilityDTO.LastUpdateDate = DateTime.Now;
                    productsAvailabilityDTO.AcceptChanges();
                }
            }

            log.LogMethodExit();
            return "Saved Successfully";
        }

        /// <summary>
        /// Validates the ProductsAvailabilityDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        public int UpdatedExpiredProductsToAvailable()
        {
            log.LogMethodEntry();
            int rowsUpdated = 0;
            
            try
            {
                rowsUpdated = productsAvailabilityyDataHandler.UpdatedExpiredProductsToAvailable();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
            return rowsUpdated;

        }

        public int UpdateAvailableQuantityForCancelledTransaction(int TrxId)
        { 
            log.LogMethodEntry(TrxId);
            int rowsUpdated = 0;
            rowsUpdated = productsAvailabilityyDataHandler.UpdateAvailableQuantityForCancelledTransaction(TrxId, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
            return rowsUpdated;
        }

        public List<LookupValuesDTO> GetUnavailableTillLookupValue()
        {
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> SearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            SearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PRODUCTS_UNAVAILABLE_TILL"));

            List<LookupValuesDTO> lookupList = lookupValuesList.GetAllLookupValues(SearchParameters);

            LookupValuesDTO lookupValuesDTO = new LookupValuesDTO(-1, -1, "-1", "-1", "", "", false, -1, -1);
            lookupList.Add(lookupValuesDTO);
            return lookupList;
        }
       
    }

    /// <summary>
    /// Manages the list of ProductsAvailability
    /// </summary>
    public class ProductsAvailabilityListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductsAvailabilityDTO> productsAvailabilityDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProductsAvailabilityListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor for ProductsAvailabilityDTO Collection.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productsAvailabilityDTOList"></param>
        public ProductsAvailabilityListBL(ExecutionContext executionContext, List<ProductsAvailabilityDTO> productsAvailabilityDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productsAvailabilityDTOList = productsAvailabilityDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductsAvailabilityDTO list
        /// </summary>
        public List<ProductsAvailabilityDTO> ExecuteSearchQuery(List<KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductsAvailabilityDataHandler productsAvailabilityyDataHandler = new ProductsAvailabilityDataHandler(sqlTransaction);
            List<ProductsAvailabilityDTO> productsAvailabilityDTOList = productsAvailabilityyDataHandler.GetUnavailableProductsList(searchParameters);
            log.LogMethodExit(productsAvailabilityDTOList);
            return productsAvailabilityDTOList;
        }

        public ProductsAvailabilityDTO SearchUnavailableProductByProductId(int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            ProductsAvailabilityDTO unavailableProduct = null;
            List<KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>> productsSearchParameters
                = new List<KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>>();
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.IS_AVAILABLE, "0"));
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.UNAVAILABLE_TILL, currentTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.PRODUCT_ID, productId.ToString()));
            List<ProductsAvailabilityDTO> tempList = ExecuteSearchQuery(productsSearchParameters, sqlTransaction);

            if (tempList != null && tempList.Count > 0)
            {
                unavailableProduct = tempList[0];
            }
            else
            {
                // adding default product to avoid null checks in multiple places in transaction
                unavailableProduct = new ProductsAvailabilityDTO(-1, -1, true, int.MaxValue, int.MaxValue, DateTime.MinValue, "", "", "", true, "", DateTime.Now, "", DateTime.Now, -1, -1, true, "");
            }
            log.LogMethodExit(unavailableProduct);
            return unavailableProduct;
        }

        public List<ProductsAvailabilityDTO> GetAvailableProductsList(List<ProductsDisplayGroupDTO> excludedProductsDisplayGroupForRole)
        {
            log.LogMethodEntry(excludedProductsDisplayGroupForRole);
            List<ProductsAvailabilityDTO> availableProductsList = new List<ProductsAvailabilityDTO>();
            // Get list of products - isActive = 'Y', Product type = Manual-5 or Combo-48, Display In Pos = 'Y' 
            ProductsList productsList = new ProductsList(this.executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> productsSearchParameters
                = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            productsSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            productsSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));
            productsSearchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST, "'MANUAL','COMBO'"));
            List<ProductsDTO> productsDTO = productsList.GetProductsDTOList(productsSearchParameters);

            if (productsDTO != null && productsDTO.Count > 0)
            {
                foreach (ProductsDTO product in productsDTO)
                {
                    ProductsAvailabilityDTO availabilityDTO = new ProductsAvailabilityDTO(-1, product.ProductId, true, 0.0M, 0.0M, DateTime.MinValue,  "", "", product.ProductName,
                        true, "", DateTime.Now,"", DateTime.Now, -1,-1,true, "");
                    availableProductsList.Add(availabilityDTO);
                }
            }

            if (excludedProductsDisplayGroupForRole != null && excludedProductsDisplayGroupForRole.Count > 0)
            {
                availableProductsList = availableProductsList.Where(x => !excludedProductsDisplayGroupForRole.Any(y => y.ProductId == x.ProductId))
                                                     .ToList();
            }
            log.LogMethodExit(availableProductsList);
            return availableProductsList;
        }

        public List<ProductsAvailabilityDTO> GetUnAvailableProductsList(List<ProductsAvailabilityDTO> availableProductsList, List<ProductsDisplayGroupDTO> excludedProductsDisplayGroupForRole)
        {
            //Get all unavailable products
            List<ProductsAvailabilityDTO> unavailableProductsList = new List<ProductsAvailabilityDTO>();
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();

            List<KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>> productsSearchParameters
                = new List<KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>>();
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.IS_AVAILABLE, "false"));
            productsSearchParameters.Add(new KeyValuePair<ProductsAvailabilityDTO.SearchParameters, string>(ProductsAvailabilityDTO.SearchParameters.UNAVAILABLE_TILL, currentTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<ProductsAvailabilityDTO> tempList = ExecuteSearchQuery(productsSearchParameters);

            if (tempList != null && tempList.Count > 0)
            {
                unavailableProductsList = tempList.Where(x => availableProductsList.Any(y => y.ProductId == x.ProductId))
                                                             .ToList();

                // filter out products to which the user role does not have access
                if (excludedProductsDisplayGroupForRole != null && excludedProductsDisplayGroupForRole.Count > 0)
                {
                    unavailableProductsList = unavailableProductsList.Where(x => !excludedProductsDisplayGroupForRole.Any(y => y.ProductId == x.ProductId))
                                                             .ToList();
                }
            }

            return unavailableProductsList;
        }

        public List<ValidationError> Save(String approvedBy, SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> errorsList = new List<ValidationError>();
            foreach (ProductsAvailabilityDTO productsAvailabilityDTO in productsAvailabilityDTOList)
            {
                try
                {
                    ProductsAvailabilityBL productsAvailabilityBL = new ProductsAvailabilityBL(executionContext, productsAvailabilityDTO);
                    productsAvailabilityBL.Save(approvedBy);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    ValidationError validationError = new ValidationError(productsAvailabilityDTO.ProductName, "", ex.Message);
                    errorsList.Add(validationError);
                }
            }

            log.LogMethodExit(false);
            return errorsList;
        }
    }
}
