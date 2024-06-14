/********************************************************************************************
 * Project Name - ProductSubscription BL
 * Description  -BL class of the ProductSubscription 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Fiona             Created for Subscription changes
 *2.120.0     18-Mar-2021    Guru S A         s For Subscription phase 2 changes
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
    /// ProductSubscriptionBL
    /// </summary>
    public class ProductSubscriptionBL
    {
        private ProductSubscriptionDTO productSubscriptionDTO;
        private readonly ExecutionContext executionContext;       
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor of ProductSubscriptionBL class
        /// </summary>
        private ProductSubscriptionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            productSubscriptionDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductSubscriptionBL object using the ProductSubscriptionDTO
        /// </summary>
        /// <param name="productSubscriptionDTO">ProductSubscriptionDTO object</param>
        public ProductSubscriptionBL(ExecutionContext executionContext, ProductSubscriptionDTO productSubscriptionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(productSubscriptionDTO);
            this.productSubscriptionDTO = productSubscriptionDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the ProductSubscription id as the parameter
        /// Would fetch the ProductSubscription object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public ProductSubscriptionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ProductSubscriptionDataHandler productSubscriptionDataHandler = new ProductSubscriptionDataHandler(sqlTransaction);
            productSubscriptionDTO = productSubscriptionDataHandler.GetProductSubscriptionDTO(id);
            if (productSubscriptionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductSubscription", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }

            log.LogMethodExit(productSubscriptionDTO);
        }
        /// <summary>
        /// Saves the ProductSubscription
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductSubscriptionDataHandler productSubscriptionDataHandler = new ProductSubscriptionDataHandler(sqlTransaction);
            if (productSubscriptionDTO.IsChanged == false && productSubscriptionDTO.ProductSubscriptionId > -1)
            {
                log.LogMethodExit(null, "ProductSubscriptionDTO is not changed.");
                return;
            }
            List<ValidationError> validationErrorList = ValidateProductSubscription(sqlTransaction);
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
            productSubscriptionDataHandler.Save(productSubscriptionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
           
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ProductSubscriptionDTO. 
        /// </summary>
        public List<ValidationError> ValidateProductSubscription(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (productSubscriptionDTO == null)
            {
                throw new ArgumentNullException(MessageContainerList.GetMessage(executionContext, "Product Subscription DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if(string.IsNullOrWhiteSpace(productSubscriptionDTO.ProductSubscriptionName))
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Subscription Name", MessageContainerList.GetMessage(executionContext, "Subscription Name") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            if (string.IsNullOrWhiteSpace(productSubscriptionDTO.ProductSubscriptionDescription))
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Subscription Description", MessageContainerList.GetMessage(executionContext, "Subscription Description") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            if (productSubscriptionDTO.SubscriptionCycle <= 0)
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Subscription Cycle", MessageContainerList.GetMessage(executionContext, "Subscription Cycle") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            if (string.IsNullOrWhiteSpace(productSubscriptionDTO.UnitOfSubscriptionCycle))
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Unit of Subscription Cycle", MessageContainerList.GetMessage(executionContext, "Unit of Subscription Cycle") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            else
            {  
                if (UnitOfSubscriptionCycle.ValidUnitOfSubscriptionCycle(productSubscriptionDTO.UnitOfSubscriptionCycle) == false)
                {
                    validationErrorList.Add(new ValidationError("SubscriptionDTO", "Unit of Subscription Cycle", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Unit of Subscription Cycle"))));//Please enter valid value for &1
                } 
            }
            if (productSubscriptionDTO.SubscriptionCycleValidity <= 0)
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Subscription Cycle Validity", MessageContainerList.GetMessage(executionContext, "Subscription Cycle Validity") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            if (string.IsNullOrWhiteSpace(productSubscriptionDTO.PaymentCollectionMode))
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Payment Collection Mode", MessageContainerList.GetMessage(executionContext, "Payment Collection Mode") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            else
            {
                if (SubscriptionPaymentCollectionMode.ValidPaymentCollectionMode(productSubscriptionDTO.PaymentCollectionMode) == false)
                {
                    validationErrorList.Add(new ValidationError("SubscriptionDTO", "Payment Collection Mode", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Payment Collection Mode"))));//Please enter valid value for &1
                }
            }

            if (string.IsNullOrWhiteSpace(productSubscriptionDTO.CancellationOption))
            {
                validationErrorList.Add(new ValidationError("SubscriptionDTO", "Cancellation Option", MessageContainerList.GetMessage(executionContext, "Cancellation Option") + " " + MessageContainerList.GetMessage(executionContext, "is null")));
            }
            else
            {
                if (SubscriptionCancellationOption.ValidCancellationOption(productSubscriptionDTO.CancellationOption) == false)
                {
                    validationErrorList.Add(new ValidationError("SubscriptionDTO", "Cancellation Option", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Cancellation Option"))));//Please enter valid value for &1
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        public ProductSubscriptionDTO ProductSubscriptionDTO { get { return productSubscriptionDTO; } }
    }
    /// <summary>
    /// Manages the list of ProductSubscription
    /// </summary>
    public class ProductSubscriptionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductSubscriptionDTO> productSubscriptionDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductSubscriptionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productSubscriptionDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productSubscriptionDTOList"></param>
        public ProductSubscriptionListBL(ExecutionContext executionContext, List<ProductSubscriptionDTO> productSubscriptionDTOList)
        {
            log.LogMethodEntry(executionContext, productSubscriptionDTOList);
            this.executionContext = executionContext;
            this.productSubscriptionDTOList = productSubscriptionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductSubscription list
        /// </summary>
        public List<ProductSubscriptionDTO> GetProductSubscriptionDTOList(List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//modified
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductSubscriptionDataHandler productSubscriptionDataHandler = new ProductSubscriptionDataHandler(sqlTransaction);
            List<ProductSubscriptionDTO> productSubscriptionDTOList= productSubscriptionDataHandler.GetProductSubscriptionDTOList(searchParameters);
            log.LogMethodExit(productSubscriptionDTOList);
            return productSubscriptionDTOList;
        }

        /// <summary>
        /// Gets GetProductSubscriptionDTOList for productsIdList
        /// </summary>
        /// <param name="productsIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<ProductSubscriptionDTO> GetProductSubscriptionDTOList(List<int> productsIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productsIdList,  sqlTransaction);
            ProductSubscriptionDataHandler productSubscriptionDataHandler = new ProductSubscriptionDataHandler(sqlTransaction);
            List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionDataHandler.GetProductSubscriptionDTOList(productsIdList);
            
            log.LogMethodExit(productSubscriptionDTOList);
            return productSubscriptionDTOList;
        }
        /// <summary>
        /// Validates and saves the productSubscriptionDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ProductSubscriptionDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (productSubscriptionDTOList == null ||
                productSubscriptionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception(MessageContainerList.GetMessage(executionContext,"Cant save empty list"));
            }
            Validate(sqlTransaction);
            
            ProductSubscriptionDataHandler productSubscriptionDataHandler = new ProductSubscriptionDataHandler(sqlTransaction);
            productSubscriptionDataHandler.Save(productSubscriptionDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit(productSubscriptionDTOList);
            return productSubscriptionDTOList;
        }

        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
             if (productSubscriptionDTOList != null && productSubscriptionDTOList.Any())
            {
                for (int i = 0; i < productSubscriptionDTOList.Count; i++)
                {
                    ProductSubscriptionDTO productSubscriptionDTO = productSubscriptionDTOList[i];
                    ProductSubscriptionBL productSubscriptionBL = new ProductSubscriptionBL(executionContext, productSubscriptionDTO);
                    List<ValidationError> validationErrorListLocal = productSubscriptionBL.ValidateProductSubscription(sqlTransaction);
                    if (validationErrorListLocal != null && validationErrorListLocal.Any())
                    {
                        validationErrorList.AddRange(validationErrorListLocal);
                    }
                }
            }
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
        }
    }
}
