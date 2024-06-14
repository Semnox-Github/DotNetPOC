/********************************************************************************************
 * Project Name - ProductCreditPlus BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By             Remarks          
 *********************************************************************************************
 *2.70        31-Feb-2019      Indrajeet Kumar         Created 
 *            24-Mar-2019      Nagesh Badiger          Added log method entry and method exit
 *            29-June-2019     Indrajeet Kumar         Added DeleteProductCreditPlusList() & DeleteProductCreditPlus() method for Implementation of Hard Deletion
 *            25-Sept-2019     Jagan Mohana            Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.80..      25-Mar-2020      Girish Kundar           Modified: 3 tier standard             
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ProductCreditPlusBL
    {
        ProductCreditPlusDTO productCreditPlusDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ProductCreditPlusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productCreditPlusDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="productCreditPlusDTO"></param>
        /// <param name="executionContext"></param>
        public ProductCreditPlusBL(ExecutionContext executionContext, ProductCreditPlusDTO productCreditPlusDTO)
        {
            log.LogMethodEntry(productCreditPlusDTO, executionContext);
            this.executionContext = executionContext;
            this.productCreditPlusDTO = productCreditPlusDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ProductCreditPlusDataHandler productCreditPlusDataHandler = new ProductCreditPlusDataHandler(sqlTransaction);
            if (productCreditPlusDTO.ProductCreditPlusId < 0)
            {
                productCreditPlusDTO = productCreditPlusDataHandler.InsertProductCreditPlus(productCreditPlusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                /// Below code is to save the Audit log details into DBAuditLog            
                if (!string.IsNullOrEmpty(productCreditPlusDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("ProductCreditPlus", productCreditPlusDTO.Guid, sqlTransaction);
                }
                productCreditPlusDTO.AcceptChanges();
            }
            else
            {
                if (productCreditPlusDTO.ProductCreditPlusId > 0 && productCreditPlusDTO.IsChanged == true)
                {
                    productCreditPlusDTO = productCreditPlusDataHandler.UpdateProductCreditPlus(productCreditPlusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    /// Below code is to save the Audit log details into DBAuditLog            
                    if (!string.IsNullOrEmpty(productCreditPlusDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("ProductCreditPlus", productCreditPlusDTO.Guid, sqlTransaction);
                    }
                    productCreditPlusDTO.AcceptChanges();
                }
            }
            if (productCreditPlusDTO.CreditPlusConsumptionRulesList != null && productCreditPlusDTO.CreditPlusConsumptionRulesList.Count > 0)
            {
                foreach (CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO in productCreditPlusDTO.CreditPlusConsumptionRulesList)
                {
                    if (creditPlusConsumptionRulesDTO != null)
                    {
                        if (creditPlusConsumptionRulesDTO.ProductCreditPlusId != productCreditPlusDTO.ProductCreditPlusId)
                        {
                            creditPlusConsumptionRulesDTO.ProductCreditPlusId = productCreditPlusDTO.ProductCreditPlusId;
                        }
                        CreditPlusConsumptionRulesBL creditPlusConsumptionRulesBLObj = new CreditPlusConsumptionRulesBL(creditPlusConsumptionRulesDTO, executionContext);
                        creditPlusConsumptionRulesBLObj.Save(sqlTransaction);
                    }
                }
            }
           
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the ProductCreditPlus based on productCreditPlusId
        /// </summary>   
        /// <param name="productCreditPlusId">productCreditPlusId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeleteProductCreditPlus(int productCreditPlusId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productCreditPlusId, sqlTransaction);
            try
            {
                ProductCreditPlusDataHandler productCreditPlusDataHandler = new ProductCreditPlusDataHandler(sqlTransaction);
                productCreditPlusDataHandler.DeleteProductCreditPlus(productCreditPlusId);
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

        public ProductCreditPlusDTO ProductCreditPlusDTO
        {
            get { return productCreditPlusDTO; }
        }
    }
    public class ProductCreditPlusBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductCreditPlusDTO> productCreditPlusList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductCreditPlusBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productCreditPlusList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="productCreditPlusList"></param>
        /// <param name="executionContext"></param>
        public ProductCreditPlusBLList(ExecutionContext executionContext, List<ProductCreditPlusDTO> productCreditPlusList)
        {
            log.LogMethodEntry(productCreditPlusList, executionContext);
            this.executionContext = executionContext;
            this.productCreditPlusList = productCreditPlusList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetProductCreditPlusList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductCreditPlusDTO> GetProductCreditPlusList(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ProductCreditPlusDataHandler productCreditPlusDataHandler = new ProductCreditPlusDataHandler(sqlTransaction);
            log.LogMethodExit();
            return productCreditPlusDataHandler.GetAllProductCreditPlusList(searchParameters);
        }

        /// <summary>
        /// GetAllProductCreditPlusListDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductCreditPlusDTO> GetAllProductCreditPlusListDTOList(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<ProductCreditPlusDTO> productCreditPlusList;
            List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesList;
            ProductCreditPlusDataHandler productCreditPlusDataHandler = new ProductCreditPlusDataHandler(sqlTransaction);

            productCreditPlusList = productCreditPlusDataHandler.GetAllProductCreditPlusList(searchParameters);
            if (productCreditPlusList != null && productCreditPlusList.Count > 0 && loadChildRecords)
            {
                CreditPlusConsumptionRulesBLList creditPlusConsumptionRulesBLList = new CreditPlusConsumptionRulesBLList(executionContext);

                List<KeyValuePair<CreditPlusConsumptionRulesDTO.SearchByParameters, string>> searchByCreditPlusConsumptionRulesParameters;
                foreach (ProductCreditPlusDTO productCreditPlusDTO in productCreditPlusList)
                {
                    searchByCreditPlusConsumptionRulesParameters = new List<KeyValuePair<CreditPlusConsumptionRulesDTO.SearchByParameters, string>>();
                    searchByCreditPlusConsumptionRulesParameters.Add(new KeyValuePair<CreditPlusConsumptionRulesDTO.SearchByParameters, string>(CreditPlusConsumptionRulesDTO.SearchByParameters.PRODUCTCREDITPLUS_ID, productCreditPlusDTO.ProductCreditPlusId.ToString()));
                    creditPlusConsumptionRulesList = creditPlusConsumptionRulesBLList.GetAllCreditPlusConsumptionRulesList(searchByCreditPlusConsumptionRulesParameters);
                    if (creditPlusConsumptionRulesList != null)
                    {
                        productCreditPlusDTO.CreditPlusConsumptionRulesList = new List<CreditPlusConsumptionRulesDTO>(creditPlusConsumptionRulesList);
                        productCreditPlusDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodEntry(productCreditPlusList);
            return productCreditPlusList;
        }

        /// <summary>
        /// Save Update Product Credit Plus List
        /// </summary>
        public void SaveUpdateProductCreditPlusList()
        {
            try
            {
                log.LogMethodEntry();
                if (productCreditPlusList != null)
                {
                    foreach (ProductCreditPlusDTO productCreditPlusDTO in productCreditPlusList)
                    {
                            ProductCreditPlusBL productCreditPlusBLObj = new ProductCreditPlusBL(executionContext, productCreditPlusDTO);
                            productCreditPlusBLObj.Save();
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
        /// Delete the ProductCreditPlus record and child record of creditPlusConsumptionRules
        /// </summary>
        public void DeleteProductCreditPlusList()
        {
            log.LogMethodEntry();
            if (productCreditPlusList != null && productCreditPlusList.Count > 0)
            {
                foreach (ProductCreditPlusDTO productCreditPlusDTO in productCreditPlusList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (productCreditPlusDTO.CreditPlusConsumptionRulesList != null && productCreditPlusDTO.CreditPlusConsumptionRulesList.Count != 0)
                            {
                                foreach (CreditPlusConsumptionRulesDTO creditPlusConsumptionRulesDTO in productCreditPlusDTO.CreditPlusConsumptionRulesList)
                                {
                                    if (creditPlusConsumptionRulesDTO.IsChanged && creditPlusConsumptionRulesDTO.IsActive == false)
                                    {
                                        CreditPlusConsumptionRulesBL creditPlusConsumptionRulesBL = new CreditPlusConsumptionRulesBL(executionContext);
                                        creditPlusConsumptionRulesBL.DeleteCreditPlusConsumptionRule(creditPlusConsumptionRulesDTO.PKId, parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            if (productCreditPlusDTO.IsChanged && productCreditPlusDTO.IsActive == false)
                            {
                                ProductCreditPlusBL productCreditPlusBL = new ProductCreditPlusBL(executionContext);
                                productCreditPlusBL.DeleteProductCreditPlus(productCreditPlusDTO.ProductCreditPlusId, parafaitDBTrx.SQLTrx);
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
        /// Gets the ProductCreditPlusDTO List for product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductCreditPlusDTO</returns>
        public List<ProductCreditPlusDTO> GetProductCreditPlusDTOListForProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            ProductCreditPlusDataHandler productCreditPlusDataHandler = new ProductCreditPlusDataHandler(sqlTransaction);
            List<ProductCreditPlusDTO> productCreditPlusDTOList = productCreditPlusDataHandler.GetProductCreditPlusDTOList(productIdList, activeRecords);
            Build(productCreditPlusDTOList, activeRecords, sqlTransaction);
            log.LogMethodExit(productCreditPlusDTOList);
            return productCreditPlusDTOList;
        }

        private void Build(List<ProductCreditPlusDTO> productCreditPlusDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productCreditPlusDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProductCreditPlusDTO> productCreditPlusDTOIdMap = new Dictionary<int, ProductCreditPlusDTO>();
            List<int> productCreditPlusIdList = new List<int>();
            Dictionary<int, ProductCreditPlusDTO> scheduleIdProductCreditPlusDTODictionary = new Dictionary<int, ProductCreditPlusDTO>();
            List<int> scheduleIdList = new List<int>();
            for (int i = 0; i < productCreditPlusDTOList.Count; i++)
            {
                if (productCreditPlusDTOIdMap.ContainsKey(productCreditPlusDTOList[i].ProductCreditPlusId))
                {
                    continue;
                }
                productCreditPlusDTOIdMap.Add(productCreditPlusDTOList[i].ProductCreditPlusId, productCreditPlusDTOList[i]);
                productCreditPlusIdList.Add(productCreditPlusDTOList[i].ProductCreditPlusId);
            }
            CreditPlusConsumptionRulesBLList creditPlusConsumptionRulesListBL = new CreditPlusConsumptionRulesBLList(executionContext);
            List<CreditPlusConsumptionRulesDTO> creditPlusConsumptionRulesDTOList = creditPlusConsumptionRulesListBL.GetCreditPlusConsumptionRulesDTOList(productCreditPlusIdList, activeChildRecords, sqlTransaction);
            if (creditPlusConsumptionRulesDTOList != null && creditPlusConsumptionRulesDTOList.Any())
            {
                for (int i = 0; i < creditPlusConsumptionRulesDTOList.Count; i++)
                {
                    if (productCreditPlusDTOIdMap.ContainsKey(creditPlusConsumptionRulesDTOList[i].ProductCreditPlusId) == false)
                    {
                        continue;
                    }
                    ProductCreditPlusDTO productCreditPlusDTO = productCreditPlusDTOIdMap[creditPlusConsumptionRulesDTOList[i].ProductCreditPlusId];
                    if (productCreditPlusDTO.CreditPlusConsumptionRulesList == null)
                    {
                        productCreditPlusDTO.CreditPlusConsumptionRulesList = new List<CreditPlusConsumptionRulesDTO>();
                    }
                    productCreditPlusDTO.CreditPlusConsumptionRulesList.Add(creditPlusConsumptionRulesDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
    }
}
