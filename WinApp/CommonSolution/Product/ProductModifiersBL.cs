/********************************************************************************************
 * Project Name - ProductModifiers BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created
 *2.50        10-Jan-2019      Jagan Mohana        Craeted new constructor ProductModifiersList and 
 *                                                 added new method SaveUpdateProductModifiersList
 *2.70        07-Jul-2019      Mehraj              Added  DeleteProductModifiersList() and 
                                                   DeleteProductModifiers methods
                               Indrajeet K         Modified DeleteProductModifiersList() Method according to new approach
 *            15-July-2019     Jagan Mohana        Validation method has been implmeneted in save method
 *2.110.00    01-Dec-2020      Abhishek            Modified : Modified to 3 Tier Standard            
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductModifiersBL
    {
        private ProductModifiersDTO productModifiersDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ProductModifiers class
        /// </summary>
        private ProductModifiersBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the ProductModifiers DTO based on the productModifiers id passed 
        /// </summary>
        /// <param name="productModifierId">productModifierId id</param>
        public ProductModifiersBL(ExecutionContext executionContext, int productModifierId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productModifierId, sqlTransaction);
            ProductModifiersDataHandler productModifiersDataHandler = new ProductModifiersDataHandler(sqlTransaction);
            productModifiersDTO = productModifiersDataHandler.GetProductModifier(productModifierId);
            if (productModifiersDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Product Modifiers ", productModifierId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates productModifiers object using the ProductModifiersDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productModifiers">ProductModifiersDTO object</param>
        public ProductModifiersBL(ExecutionContext executionContext, ProductModifiersDTO productModifiersDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productModifiersDTO);
            this.productModifiersDTO = productModifiersDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the productModifiers record
        /// Checks if the ProductModifiersId is not less 0
        ///     If it is less than 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductModifiersDataHandler productModifiersDataHandler = new ProductModifiersDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (productModifiersDTO.ProductModifierId < 0)
            {
                productModifiersDTO = productModifiersDataHandler.Insert(productModifiersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productModifiersDTO.AcceptChanges();
            }
            else
            {
                if (productModifiersDTO.IsChanged)
                {
                    productModifiersDTO = productModifiersDataHandler.Update(productModifiersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productModifiersDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the product modifier dto
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (productModifiersDTO.ProductModifierId < 0)
            {
                int productId = productModifiersDTO.ProductId;
                List<int> modifierSetIdList = new List<int>();
                ProductModifiersDataHandler productModifiersDataHandler = new ProductModifiersDataHandler(sqlTransaction);
                List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, Convert.ToString(productId)));
                List<ProductModifiersDTO> productModifiersDTOList = productModifiersDataHandler.GetProductModifiers(searchParameters);

                if (productModifiersDTOList != null && productModifiersDTOList.Count != 0)
                {
                    foreach (ProductModifiersDTO productModifiers in productModifiersDTOList)
                    {
                        modifierSetIdList.Add(productModifiers.ModifierSetId);
                    }
                    modifierSetIdList.Add(productModifiersDTO.ModifierSetId);
                }
                if (modifierSetIdList.Count > 0)
                {
                    OrderTypeListBL orderTypeListBL = new OrderTypeListBL(executionContext);
                    HashSet<int> orderTypeIdSet = orderTypeListBL.GetOrderTypeIdSetOfProducts(new List<int>() { productId });
                    HashSet<int> modifiersetOrderTypeIdSet = orderTypeListBL.GetOrderTypeIdSetOfModifierSets(modifierSetIdList);
                    if (modifiersetOrderTypeIdSet != null)
                    {
                        orderTypeIdSet.UnionWith(modifiersetOrderTypeIdSet);
                        bool orderTypeGroupFound = false;
                        List<OrderTypeGroupBL> orderTypeGroupBLList = null;
                        if (orderTypeIdSet.SetEquals(new HashSet<int>() { -1 }))
                        {
                            orderTypeGroupFound = true;
                        }
                        else
                        {
                            if (orderTypeGroupBLList == null)
                            {
                                OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(executionContext);
                                List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                                searchParams.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                                searchParams.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, 1.ToString()));
                                List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParams);
                                orderTypeGroupBLList = new List<OrderTypeGroupBL>();
                                if (orderTypeGroupDTOList != null)
                                {
                                    foreach (var orderTypeGroupDTO in orderTypeGroupDTOList)
                                    {
                                        orderTypeGroupBLList.Add(new OrderTypeGroupBL(executionContext, orderTypeGroupDTO));
                                    }
                                }
                            }
                            foreach (var orderTypeGroupBL in orderTypeGroupBLList)
                            {
                                if (orderTypeGroupBL.Match(orderTypeIdSet))
                                {
                                    orderTypeGroupFound = true;
                                    break;
                                }
                            }
                        }
                        if (orderTypeGroupFound == false)
                        {
                            validationErrorList.Add(new ValidationError("ProductModifier", "ModifierSetId", MessageContainerList.GetMessage(executionContext, 1406)));
                        }
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductModifiersDTO GetProductModifiersDTO { get { return productModifiersDTO; } }


        /// <summary>
        /// Deletes Productmodifiers based on productModifierId
        /// </summary>
        /// <param name="productModifierId"></param>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                ProductModifiersDataHandler productModifiersDataHandler = new ProductModifiersDataHandler(sqlTransaction);
                productModifiersDataHandler.Delete(productModifiersDTO.ProductModifierId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Manages the list of productModifiers
    /// </summary>
    public class ProductModifiersList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductModifiersDTO> productModifiersList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductModifiersList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized Constructor with productModifiersList and executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productModifiersList"></param>
        public ProductModifiersList(ExecutionContext executionContext, List<ProductModifiersDTO> productModifiersList)
            : this(executionContext)
        {
            log.LogMethodEntry(productModifiersList, executionContext);
            this.productModifiersList = productModifiersList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the productModifiers list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ProductModifiersDTO> GetAllProductModifiersList(List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool loadChildRecords = false)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductModifiersDataHandler productModifiersDataHandler = new ProductModifiersDataHandler(sqlTransaction);
            List<ProductModifiersDTO> productModifiersDTOList = productModifiersDataHandler.GetProductModifiers(searchParameters);
            if (productModifiersDTOList != null && productModifiersDTOList.Any() && loadChildRecords)
            {
                Dictionary<int, ProductModifiersDTO> modifierSetToProductsIdMap = new Dictionary<int, ProductModifiersDTO>();
                String modifiersSetIdListString = "";
                for (int i = 0; i < productModifiersDTOList.Count; i++)
                {
                    if (modifierSetToProductsIdMap.ContainsKey(productModifiersDTOList[i].ModifierSetId))
                    {
                        continue;
                    }
                    modifierSetToProductsIdMap.Add(productModifiersDTOList[i].ModifierSetId, productModifiersDTOList[i]);
                    modifiersSetIdListString += productModifiersDTOList[i].ModifierSetId.ToString() + ",";
                }

                // remove the last ,
                if (modifiersSetIdListString.Length > 0)
                    modifiersSetIdListString = modifiersSetIdListString.Remove(modifiersSetIdListString.Length - 1);

                List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> productModifiersListSearchParameters = new List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>();
                productModifiersListSearchParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.ISACTIVE, "1"));
                productModifiersListSearchParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID_LIST, modifiersSetIdListString));

                ModifierSetDTOList modifierSetBL = new ModifierSetDTOList(executionContext);
                List<ModifierSetDTO> modifierSetDTOList = modifierSetBL.GetAllModifierSetDTOList(productModifiersListSearchParameters, loadChildRecords);
                if (modifierSetDTOList != null && modifierSetDTOList.Any())
                {
                    foreach (ModifierSetDTO modifierSetDTO in modifierSetDTOList)
                    {
                        if (modifierSetToProductsIdMap.ContainsKey(modifierSetDTO.ModifierSetId) == false)
                        {
                            continue;
                        }
                        ProductModifiersDTO productModifierDTO = modifierSetToProductsIdMap[modifierSetDTO.ModifierSetId];
                        if (productModifierDTO != null)
                        {
                            productModifierDTO.ModifierSetDTO = modifierSetDTO;
                        }
                    }

                }

            }
            log.LogMethodExit(productModifiersDTOList);
            return productModifiersDTOList;
        }

        /// <summary>
        /// save or update product modifers details 
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                foreach (ProductModifiersDTO productModifiersDTO in productModifiersList)
                {
                    ProductModifiersBL productModifiersBL = new ProductModifiersBL(executionContext, productModifiersDTO);
                    productModifiersBL.Save(sqlTransaction);
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                throw valEx;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (productModifiersList != null && productModifiersList.Count > 0)
            {
                foreach (ProductModifiersDTO productModifiersDTO in productModifiersList)
                {
                    if (productModifiersDTO.IsChanged && productModifiersDTO.IsActive == false)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProductModifiersBL productModifiers = new ProductModifiersBL(executionContext, productModifiersDTO);
                                productModifiers.Delete(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                parafaitDBTrx.RollBack();
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

        /// <summary>
        /// Gets the ProductModifiersDetailsDTO List for product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ProductModifiersDTO> GetProductModifiersDTOListForProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            ProductModifiersDataHandler productModifiersDataHandler = new ProductModifiersDataHandler(sqlTransaction);
            List<ProductModifiersDTO> productModifiersDTOList = productModifiersDataHandler.GetProductModifiersDTOList(productIdList, activeRecords);
            Build(productModifiersDTOList, activeRecords, sqlTransaction);
            log.LogMethodExit(productModifiersDTOList);
            return productModifiersDTOList;
        }

        // <summary>
        /// Builds the List of ProductModifiers object based on the list of ProductModifiers id.
        /// </summary>
        /// <param name="productModifiersDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<ProductModifiersDTO> productModifiersDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productModifiersDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, List<ProductModifiersDTO>> modifierSetIdProductModifiersDTOListDictionary = new Dictionary<int, List<ProductModifiersDTO>>();
            for (int i = 0; i < productModifiersDTOList.Count; i++)
            {
                if(productModifiersDTOList[i].ModifierSetId == -1)
                {
                    continue;
                }
                if(modifierSetIdProductModifiersDTOListDictionary.ContainsKey(productModifiersDTOList[i].ModifierSetId) == false)
                {
                    modifierSetIdProductModifiersDTOListDictionary.Add(productModifiersDTOList[i].ModifierSetId, new List<ProductModifiersDTO>());
                }
                modifierSetIdProductModifiersDTOListDictionary[productModifiersDTOList[i].ModifierSetId].Add(productModifiersDTOList[i]);
            }
            ModifierSetDTOList modifierSetDTOList = new ModifierSetDTOList(executionContext);
            List<ModifierSetDTO> modifierSetDTOLists = modifierSetDTOList.GetModifierSetDTOListForModifierSet(modifierSetIdProductModifiersDTOListDictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
            if (modifierSetDTOLists != null && modifierSetDTOLists.Any())
            {
                foreach (var modifierSetDTO in modifierSetDTOLists)
                {
                    if (modifierSetIdProductModifiersDTOListDictionary.ContainsKey(modifierSetDTO.ModifierSetId) == false)
                    {
                        continue;
                    }
                    foreach (var productModifiersDTO in modifierSetIdProductModifiersDTOListDictionary[modifierSetDTO.ModifierSetId])
                    {
                        productModifiersDTO.ModifierSetDTO = new ModifierSetDTO(modifierSetDTO);
                    }
                }
            }
        }
    }
}
