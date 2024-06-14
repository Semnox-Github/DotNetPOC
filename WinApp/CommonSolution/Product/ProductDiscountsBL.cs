/********************************************************************************************
 * Project Name - ProductDiscounts BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017      Lakshminarayana     Created 
 *2.70        18-Mar-2019      Akshay Gulaganji    Modified isActive (from string to bool) and 
 *                                                 Added Constructor with ExecutionContext 
 *            26-Mar-2019      Mushahid Faizan     Modified- Author Version, SaveUpdateProductDiscountsList,added log Method Entry & Exit , removed unnecessary namespaces.
 *2.110.00    30-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard            
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for ProductDiscounts class.
    /// </summary>
    public class ProductDiscountsBL
    {
        private ProductDiscountsDTO productDiscountsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int? businessHourStartTime = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parametrized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ProductDiscountsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the productDiscounts id as the parameter
        /// Would fetch the productDiscounts object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public ProductDiscountsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductDiscountsDataHandler productDiscountsDataHandler = new ProductDiscountsDataHandler(sqlTransaction);
            productDiscountsDTO = productDiscountsDataHandler.GetProductDiscountsDTO(id);
            if (productDiscountsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Product Discounts ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(productDiscountsDTO);
        }

        /// <summary>
        /// Creates ProductDiscountsBL object using the ProductDiscountsDTO
        /// </summary>
        /// <param name="productDiscountsDTO">ProductDiscountsDTO object</param>
        public ProductDiscountsBL(ExecutionContext executionContext, ProductDiscountsDTO productDiscountsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productDiscountsDTO);
            this.productDiscountsDTO = productDiscountsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductDiscounts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productDiscountsDTO.IsChanged == false
                && productDiscountsDTO.ProductDiscountId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ProductDiscountsDataHandler productDiscountsDataHandler = new ProductDiscountsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (productDiscountsDTO.IsActive == true)
            {
                if (productDiscountsDataHandler.IsDiscountActive(productDiscountsDTO.DiscountId) == false)
                {
                    throw new InvalidDiscountException("Only active discounts can be used.");
                }
            }
            if (productDiscountsDTO.ProductDiscountId < 0)
            {
                productDiscountsDTO = productDiscountsDataHandler.Insert(productDiscountsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productDiscountsDTO.AcceptChanges();
            }
            else
            {
                if (productDiscountsDTO.IsChanged)
                {
                    productDiscountsDTO = productDiscountsDataHandler.Update(productDiscountsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productDiscountsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
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

        

        private int GetBuisnessHourStartTime()
        {
            log.LogMethodEntry();
            if (businessHourStartTime == null)
            {
                businessHourStartTime = 6;
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParam = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                searchParafaitDefaultsParam.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "BUSINESS_DAY_START_TIME"));
                List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParam);
                if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(parafaitDefaultsDTOList[0].DefaultValue) == false)
                    {
                        int value;
                        if (int.TryParse(parafaitDefaultsDTOList[0].DefaultValue, out value))
                        {
                            businessHourStartTime = value;
                        }
                    }
                }
            }
            log.LogMethodExit(businessHourStartTime);
            return (int)businessHourStartTime;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductDiscountsDTO ProductDiscountsDTO { get { return productDiscountsDTO; } }

    }

    /// <summary>
    /// Manages the list of ProductDiscounts
    /// </summary>
    public class ProductDiscountsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductDiscountsDTO> productsDiscountedList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parametrized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductDiscountsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized Constructor 
        /// </summary>
        /// <param name="productsDiscountedList"></param>
        /// <param name="executionContext"></param>
        public ProductDiscountsListBL(ExecutionContext executionContext, List<ProductDiscountsDTO> productsDiscountedList)
            : this(executionContext)
        {
            log.LogMethodEntry(productsDiscountedList, executionContext);
            this.productsDiscountedList = productsDiscountedList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductDiscounts list
        /// </summary>
        public List<ProductDiscountsDTO> GetProductDiscountsDTOList(List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductDiscountsDataHandler productDiscountsDataHandler = new ProductDiscountsDataHandler(sqlTransaction);
            List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsDataHandler.GetProductDiscountsDTOList(searchParameters);
            log.LogMethodExit(productDiscountsDTOList);
            return productDiscountsDTOList;
        }


        /// <summary>
        /// Save or Update Product Discounts
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (productsDiscountedList != null)
                {
                    foreach (ProductDiscountsDTO productsDiscountsDTO in productsDiscountedList)
                    {
                        ProductDiscountsBL productDiscountsBL = new ProductDiscountsBL(executionContext, productsDiscountsDTO);
                        productDiscountsBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Represents invalid discount error that occur during application execution. 
    /// </summary>
    public class InvalidDiscountException : Exception
    {
        /// <summary>
        /// Default constructor of InvalidDiscountException.
        /// </summary>
        public InvalidDiscountException()
        {
        }

        /// <summary>
        /// Initializes a new instance of InvalidDiscountException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidDiscountException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of InvalidDiscountException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public InvalidDiscountException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
