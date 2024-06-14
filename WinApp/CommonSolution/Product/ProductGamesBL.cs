/********************************************************************************************
 * Project Name - Product Game BL
 * Description  - Bussiness logic of Product Game
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        29-Jan-2019   Akshay Gulaganji    Created 
 *2.70        27-Mar-2019   Nagesh Badiger      Modified:Added IS_ACTIVE parameter in GetProductGamesDTOList
 *2.70        29-June-2019  Indrajeet Kumar     Created DeleteProductGamesList() & DeleteProductGames() method 
 *                                              Added SqlTrasaction & Modified Default Constructor to Parameterized Constructor added executionContext as parameter.
 *            25-Sept-2019  Jagan Mohana        Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 **********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for Product Game class.
    /// </summary>
    public class ProductGamesBL
    {
        private ProductGamesDTO productGamesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        ///<summary>
        /// Default constructor
        ///</summary>
        public ProductGamesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productGamesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the parameters
        /// </summary>
        /// <param name="productGamesDTO">productGamesDTO</param>
        /// <param name="executionContext">executionContext</param>
        public ProductGamesBL(ExecutionContext executionContext, ProductGamesDTO productGamesDTO) :this(executionContext)
        {
            log.LogMethodEntry(productGamesDTO, executionContext);
            this.productGamesDTO = productGamesDTO;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Constructor with the ProductGame id as the parameter
        /// Would fetch the ProductGamesExtended object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public ProductGamesBL(ExecutionContext executionContext, int productGameId, SqlTransaction sqlTransaction= null):this(executionContext)
        {
            log.LogMethodEntry(executionContext,productGameId);
            ProductGamesDataHandler productGamesDataHandler = new ProductGamesDataHandler(sqlTransaction);
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList;
            productGamesDTO = productGamesDataHandler.GetProductGamesDTO(productGameId);
            if (productGamesDTO != null)
            {
                ProductGamesExtendedListBL productGamesExtendedListBL= new ProductGamesExtendedListBL(executionContext);
                List<KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>> productGamesExtendedSearchParameters;
                productGamesExtendedSearchParameters = new List<KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>>();
                productGamesExtendedSearchParameters.Add(new KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>(ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.PRODUCTGAMEID, productGamesDTO.Product_game_id.ToString()));
                productGamesExtendedSearchParameters.Add(new KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>(ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.SITE_ID,executionContext.GetSiteId().ToString()));
                productGamesExtendedDTOList = productGamesExtendedListBL.GetProductGamesExtendedList(productGamesExtendedSearchParameters);
                if (productGamesExtendedDTOList != null)
                {
                    productGamesDTO.ProductGamesExtendedDTOList = new SortableBindingList<ProductGamesExtendedDTO>(productGamesExtendedDTOList);
                    productGamesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProductGames
        /// Checks if the  Product_game_id is not less than 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// and also inserts or updates the child records
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductGamesDataHandler productGamesDataHandler = new ProductGamesDataHandler(sqlTransaction);
            if (productGamesDTO.Product_game_id < 0)
            {
                productGamesDTO = productGamesDataHandler.InsertProductGames(productGamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(productGamesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("ProductGames", productGamesDTO.Guid, sqlTransaction);
                }
                productGamesDTO.AcceptChanges();
                log.Debug(string.Format("ProductGameId {0} has been stored from SaveUpdateProductGamesList() ", productGamesDTO.Product_game_id));
            }
            else
            {
                if (productGamesDTO.Product_game_id > 0 && productGamesDTO.IsChanged)
                {
                    productGamesDTO = productGamesDataHandler.UpdateProductGames(productGamesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(productGamesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("ProductGames", productGamesDTO.Guid, sqlTransaction);
                    }
                    productGamesDTO.AcceptChanges();
                    log.Debug(string.Format("ProductGameId {0} Rows has been updated from SaveUpdateProductGamesList() ", productGamesDTO.Product_game_id));
                }
            }
            if (productGamesDTO.ProductGamesExtendedDTOList != null && productGamesDTO.ProductGamesExtendedDTOList.Count > 0)
            {
                foreach (ProductGamesExtendedDTO productGamesExtendedDto in productGamesDTO.ProductGamesExtendedDTOList)
                {
                    if (productGamesExtendedDto.IsChanged || productGamesExtendedDto.Id < 0)
                    {
                        productGamesExtendedDto.ProductGameId = productGamesDTO.Product_game_id;
                        ProductGamesExtendedBL productGamesExtendedBL = new ProductGamesExtendedBL(executionContext, productGamesExtendedDto);
                        productGamesExtendedBL.Save();
                    }
                }
            }
            /// Below code is to save the Audit log details into DBAuditLog
            //productGamesDTO = productGamesDataHandler.GetProductGamesDTO(productGamesDTO.Product_game_id);
            if (!string.IsNullOrEmpty(productGamesDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("ProductGames", productGamesDTO.Guid, sqlTransaction);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the productGames based on productGameId
        /// </summary>   
        /// <param name="productGameId">productGameId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeleteProductGames(int productGameId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productGameId, sqlTransaction);
            try
            {
                ProductGamesDataHandler productGamesDataHandler = new ProductGamesDataHandler(sqlTransaction);
                productGamesDataHandler.DeleteProductGames(productGameId);
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
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether the query is valid. 
        /// </summary>
        /// <param name="query">query</param>
        public bool CheckQuery(string query, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(query);
            ProductGamesDataHandler productGamesDataHandler = new ProductGamesDataHandler(sqlTransaction);
            bool retVal = productGamesDataHandler.CheckQuery(query);
            log.LogMethodExit(retVal);
            return retVal;
        }
        
        /// <summary>
        /// Gets the Product Games DTO
        /// </summary>
        public ProductGamesDTO ProductGamesDTO
        {
            get
            {
                return productGamesDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of ProductGames
    /// </summary>
    public class ProductGamesListBL
    {   
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ProductGamesDTO> productGamesDTOList;

        /// <summary>
        /// Default constructor of ProductGamesListBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ProductGamesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productGamesDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor of ProductGamesListBL class with productGamesDTOList and executionContext
        /// </summary>
        /// <param name="productGamesDTOList">productGamesDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public ProductGamesListBL(ExecutionContext executionContext, List<ProductGamesDTO> productGamesDTOList)
        {
            log.LogMethodEntry(productGamesDTOList, executionContext);
            this.executionContext = executionContext;
            this.productGamesDTOList = productGamesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductGamesDTO List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public List<ProductGamesDTO> GetProductGamesDTOList(List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductGamesDTO> productGamesDTOList;
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList;
            ProductGamesDataHandler productGamesDataHandler = new ProductGamesDataHandler(sqlTransaction);

            productGamesDTOList = productGamesDataHandler.GetProductGamesDTOList(searchParameters);
            if (productGamesDTOList != null && productGamesDTOList.Count > 0)
            {
                ProductGamesExtendedListBL productGamesExtendedListBL = new ProductGamesExtendedListBL(executionContext);                 
                foreach (ProductGamesDTO productGamesDTO in productGamesDTOList)
                {
                    List<KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>> productGamesExtendedSearchParameters = new List<KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>>();
                    productGamesExtendedSearchParameters.Add(new KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>(ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.PRODUCTGAMEID, productGamesDTO.Product_game_id.ToString()));
                    productGamesExtendedSearchParameters.Add(new KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>(ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    if (searchParameters.Where(m => m.Key.Equals(ProductGamesDTO.SearchByProductGamesParameters.ISACTIVE)).FirstOrDefault().Value == "1")
                    {
                        productGamesExtendedSearchParameters.Add(new KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>(ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.ISACTIVE, "1"));
                    }                    
                    productGamesExtendedDTOList = productGamesExtendedListBL.GetProductGamesExtendedList(productGamesExtendedSearchParameters);
                    if (productGamesExtendedDTOList != null)
                    {
                        productGamesDTO.ProductGamesExtendedDTOList= new SortableBindingList<ProductGamesExtendedDTO>(productGamesExtendedDTOList);
                        productGamesDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodEntry(productGamesDTOList);
            return productGamesDTOList;
        }

        /// <summary>
        /// Save and Updated the productGames details
        /// </summary>
        public void SaveUpdateProductGamesList()
        {
            try
            {
                log.LogMethodEntry();
                if (productGamesDTOList != null)
                {
                    foreach (ProductGamesDTO productGamesDTO in productGamesDTOList)
                    {
                        ProductGamesBL productGamesBL = new ProductGamesBL(executionContext, productGamesDTO);
                        productGamesBL.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the ProductGames record and child record of ProductGameExtended
        /// </summary>
        public void DeleteProductGamesList()
        {
            log.LogMethodEntry();
            if (productGamesDTOList != null && productGamesDTOList.Count > 0)
            {
                foreach (ProductGamesDTO productGamesDTO in productGamesDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (productGamesDTO.ProductGamesExtendedDTOList != null && productGamesDTO.ProductGamesExtendedDTOList.Count != 0)
                            {
                                foreach (ProductGamesExtendedDTO productGamesExtendedDTO in productGamesDTO.ProductGamesExtendedDTOList)
                                {
                                    if (productGamesExtendedDTO.IsChanged && productGamesExtendedDTO.ISActive == false)
                                    {
                                        ProductGamesExtendedBL productGamesExtendedBL = new ProductGamesExtendedBL(executionContext);
                                        productGamesExtendedBL.DeleteProductGameExtended(productGamesExtendedDTO.Id, parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            if (productGamesDTO.IsChanged && productGamesDTO.ISActive == false)
                            {
                                ProductGamesBL productGamesBL = new ProductGamesBL(executionContext);
                                productGamesBL.DeleteProductGames(productGamesDTO.Product_game_id, parafaitDBTrx.SQLTrx);
                            }
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
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the ProductGamesDTO List for product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductGamesDTO</returns>
        public List<ProductGamesDTO> GetProductGamesDTOListForProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            ProductGamesDataHandler productGamesDataHandler = new ProductGamesDataHandler(sqlTransaction);
            List<ProductGamesDTO> productGamesDTOList = productGamesDataHandler.GetProductGamesDTOList(productIdList, activeRecords);
            Build(productGamesDTOList, activeRecords, sqlTransaction);
            log.LogMethodExit(productGamesDTOList);
            return productGamesDTOList;
        }
        private void Build(List<ProductGamesDTO> productGamesDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productGamesDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductGamesDTO> productGamesDTOIdMap = new Dictionary<int, ProductGamesDTO>();
            List<int> productGamesIdList = new List<int>();
            Dictionary<int, ProductGamesDTO> scheduleIdProductGamesDTODictionary = new Dictionary<int, ProductGamesDTO>();
            List<int> scheduleIdList = new List<int>();
            for (int i = 0; i < productGamesDTOList.Count; i++)
            {
                if (productGamesDTOIdMap.ContainsKey(productGamesDTOList[i].Product_game_id))
                {
                    continue;
                }
                productGamesDTOIdMap.Add(productGamesDTOList[i].Product_game_id, productGamesDTOList[i]);
                productGamesIdList.Add(productGamesDTOList[i].Product_game_id);
            }
            ProductGamesExtendedListBL productGamesExtendedListBL = new ProductGamesExtendedListBL(executionContext);
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList = productGamesExtendedListBL.GetProductGamesExtendedDTOList(productGamesIdList, activeChildRecords, sqlTransaction);
            if (productGamesExtendedDTOList != null && productGamesExtendedDTOList.Any())
            {
                for (int i = 0; i < productGamesExtendedDTOList.Count; i++)
                {
                    if (productGamesDTOIdMap.ContainsKey(productGamesExtendedDTOList[i].ProductGameId) == false)
                    {
                        continue;
                    }
                    ProductGamesDTO productGamesDTO = productGamesDTOIdMap[productGamesExtendedDTOList[i].ProductGameId];
                    if (productGamesDTO.ProductGamesExtendedDTOList == null)
                    {
                        productGamesDTO.ProductGamesExtendedDTOList = new SortableBindingList<ProductGamesExtendedDTO>();
                    }
                    productGamesDTO.ProductGamesExtendedDTOList.Add(productGamesExtendedDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
    }
}
