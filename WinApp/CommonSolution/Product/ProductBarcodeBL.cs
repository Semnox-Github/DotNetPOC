/********************************************************************************************
 * Project Name - Product Barcode BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        30-Aug-2017   Indhu            Created 
 *2.60.0      02-Apr-2019   Lakshminarayana  Changed to use batch insert and update
 *2.110.00    30-Nov-2020   Abhishek         Modified : Modified to 3 Tier Standard 
 *********************************************************************************************/
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
    /// Business logic for Product Barcode class.
    /// </summary>
    public class ProductBarcodeBL
    {
        private ProductBarcodeDTO productBarcodeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of ProductBarcodeBL class
        /// </summary>
        private ProductBarcodeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the productBarcode id as the parameter
        /// Would fetch the productBarcode object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ProductBarcodeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductBarcodeDataHandler productBarcodeDataHandler = new ProductBarcodeDataHandler(sqlTransaction);
            productBarcodeDTO = productBarcodeDataHandler.GetProductBarcodeDTO(id);
            if (productBarcodeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "  Product Barcode ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductBarcodeBL object using the ProductBarcodeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productBarcodeDTO">ProductBarcodeDTO object</param>
        public ProductBarcodeBL(ExecutionContext executionContext, ProductBarcodeDTO productBarcodeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productBarcodeDTO);
            this.productBarcodeDTO = productBarcodeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ProductBarcode. if any of the field values are not valid returns a list of ValidationErrors.
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrors = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(productBarcodeDTO.BarCode))
            {
                ValidationError validationError = new ValidationError("ProductBarcode", "BarCode", MessageContainerList.GetMessage(executionContext, 1144, "BarCode"));
                validationErrors.Add(validationError);
            }
            //if (productBarcodeDTO.IsActive )
            //{
            //    ValidationError validationError = new ValidationError("ProductBarcode", "IsActive", MessageContainerList.GetMessage(executionContext, 1144, "IsActive"));
            //    validationErrors.Add(validationError);
            //}
            log.LogMethodExit(validationErrors);
            return validationErrors;
        }

        /// <summary>
        /// save and updates the record 
        /// </summary>
        /// <param name="sqlTransaction">Holds the sql transaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productBarcodeDTO.IsChanged == false && productBarcodeDTO.Id > -1)
            {
                log.LogMethodExit(null, "productBarcodeDTO is not changed.");
                return;
            }
            ProductBarcodeDataHandler productBarcodeDataHandler = new ProductBarcodeDataHandler(sqlTransaction);
            productBarcodeDataHandler.Save(productBarcodeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductBarcodeDTO GetProductBarcodeDTO { get { return productBarcodeDTO; } }
    }

    /// <summary>
    /// Manages the list of ProductBarcode
    /// </summary>
    public class ProductBarcodeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<ProductBarcodeDTO> productBarcodeDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        //public ProductBarcodeListBL() : this(ExecutionContext.GetExecutionContext())
        //{
        //    log.LogMethodEntry(executionContext);
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProductBarcodeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productBarcodeDTOList"></param>
        public ProductBarcodeListBL(ExecutionContext executionContext, List<ProductBarcodeDTO> productBarcodeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productBarcodeDTOList = productBarcodeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductBarcode list
        /// </summary>
        public List<ProductBarcodeDTO> GetProductBarcodeDTOList(List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductBarcodeDataHandler productBarcodeDataHandler = new ProductBarcodeDataHandler(sqlTransaction);
            List<ProductBarcodeDTO> productBarcodeDTOList = productBarcodeDataHandler.GetProductBarcodeDTOList(searchParameters);
            log.LogMethodExit(productBarcodeDTOList);
            return productBarcodeDTOList;
        }

        /// <summary>
        /// Gets the ProductBarcodeDTO List for Product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ProductBarcodeDTO> GetProductBarcodeDTOListOfProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList);
            ProductBarcodeDataHandler productBarcodeDataHandler = new ProductBarcodeDataHandler(sqlTransaction);
            List<ProductBarcodeDTO> productBarcodeDTOList = productBarcodeDataHandler.GetProductBarcodeDTOListOfProducts(productIdList, activeRecords);
            log.LogMethodExit(productBarcodeDTOList);
            return productBarcodeDTOList;
        }

        /// <summary>
        /// Validates and saves the productBarcodeDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productBarcodeDTOList == null ||
                productBarcodeDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
           
            ProductBarcodeDataHandler productBarcodeDataHandler = new ProductBarcodeDataHandler(sqlTransaction);
            productBarcodeDataHandler.Save(productBarcodeDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}
