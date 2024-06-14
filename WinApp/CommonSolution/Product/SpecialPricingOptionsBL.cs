/********************************************************************************************
 * Project Name - Product
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By                      Remarks          
 *********************************************************************************************
 *2.60        14-Feb-2019      Indrajeet Kumar                  Created 
 *2.60        22-Mar-2019      Nagesh Badiger                   Added removed default constructor and added log method entry and method exit
 *2.70        29-Jun-2019      Akshay Gulaganji                 Added DeleteSpecialPricingOptions() and DeleteSpecialPricingOptionsList() method
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
    public class SpecialPricingOptionsBL
    {
        SpecialPricingOptionsDTO specialPricingOptionsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public SpecialPricingOptionsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.specialPricingOptionsDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="specialPricingOptionsDTO"></param>
        /// <param name="executionContext"></param>
        public SpecialPricingOptionsBL(ExecutionContext executionContext, SpecialPricingOptionsDTO specialPricingOptionsDTO)
        {
            log.LogMethodEntry(specialPricingOptionsDTO, executionContext);
            this.executionContext = executionContext;
            this.specialPricingOptionsDTO = specialPricingOptionsDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Save Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            SpecialPricingOptionsDataHandler specialPricingOptionsDataHandler = new SpecialPricingOptionsDataHandler(sqlTransaction);
            if (specialPricingOptionsDTO.PricingId < 0)
            {
                int Pricing_Id = specialPricingOptionsDataHandler.InsertSpecialPricingOptions(specialPricingOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                specialPricingOptionsDTO.PricingId = Pricing_Id;
            }
            else
            {
                if (specialPricingOptionsDTO.PricingId > 0 && specialPricingOptionsDTO.IsChanged == true)
                {
                    specialPricingOptionsDataHandler.UpdateSpecialPricingOptions(specialPricingOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    specialPricingOptionsDTO.AcceptChanges();
                }
            }
            if (specialPricingOptionsDTO.ProductSpecialPricesList != null && specialPricingOptionsDTO.ProductSpecialPricesList.Count > 0)
            {
                foreach (ProductSpecialPricesDTO productSpecialPricesDTO in specialPricingOptionsDTO.ProductSpecialPricesList)
                {
                    if (productSpecialPricesDTO != null)
                    {
                        if (productSpecialPricesDTO.SpecialPricingId != specialPricingOptionsDTO.PricingId)
                        {
                            productSpecialPricesDTO.SpecialPricingId = specialPricingOptionsDTO.PricingId;
                        }
                        ProductSpecialPricesBL productSpecialPricesBLObj = new ProductSpecialPricesBL(productSpecialPricesDTO, executionContext);
                        productSpecialPricesBLObj.Save();
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the SpecialPricing details based on specialPricingId
        /// </summary>
        /// <param name="specialPricingId">specialPricingId</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>        
        public void DeleteSpecialPricingOptions(int specialPricingId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(specialPricingId);
            try
            {
                SpecialPricingOptionsDataHandler specialPricingOptionsDataHandler = new SpecialPricingOptionsDataHandler(sqlTransaction);
                specialPricingOptionsDataHandler.DeleteSpecialPricing(specialPricingId);
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
    }
    public class SpecialPricingOptionsBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SpecialPricingOptionsDTO> specialPricingOptionsList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public SpecialPricingOptionsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.specialPricingOptionsList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="specialPricingOptionsList"></param>
        /// <param name="executionContext"></param>
        public SpecialPricingOptionsBLList(ExecutionContext executionContext, List<SpecialPricingOptionsDTO> specialPricingOptionsList)
        {
            log.LogMethodEntry(specialPricingOptionsList, executionContext);
            this.executionContext = executionContext;
            this.specialPricingOptionsList = specialPricingOptionsList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get Special pricing options details
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="loadActiveChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<SpecialPricingOptionsDTO> GetSpecialPricingOptionsList(List<KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords);
            SpecialPricingOptionsDataHandler specialPricingOptionsDataHandler = new SpecialPricingOptionsDataHandler(sqlTransaction);
            log.LogMethodExit();
            List<SpecialPricingOptionsDTO> specialPricingOptionsList = specialPricingOptionsDataHandler.GetAllSpecialPricingOptionsList(searchParameters);
            if (specialPricingOptionsList != null && specialPricingOptionsList.Count != 0 && loadChildRecords)
            {
                ProductSpecialPricesBLList productSpecialPricesBLList = new ProductSpecialPricesBLList(executionContext);

                foreach (SpecialPricingOptionsDTO specialPricingOptionsDTO in specialPricingOptionsList)
                {
                    List<ProductSpecialPricesDTO> productSpecialPricesList = productSpecialPricesBLList.GetProductSpecialPricesInfo(specialPricingOptionsDTO.PricingId, specialPricingOptionsDTO.Percentage, executionContext.GetSiteId());
                    if (productSpecialPricesList != null)
                    {
                        specialPricingOptionsDTO.ProductSpecialPricesList = productSpecialPricesList;
                        specialPricingOptionsDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodEntry(specialPricingOptionsList);
            return specialPricingOptionsList;
        }

        /// <summary>
        /// Save and Update the Special Pricing Options
        /// </summary>
        public void SaveUpdateSpecialPricingOptionsList()
        {
            try
            {
                log.LogMethodEntry();
                if (specialPricingOptionsList != null)
                {
                    foreach (SpecialPricingOptionsDTO specialPricingOptionsDTO in specialPricingOptionsList)
                    {
                        SpecialPricingOptionsBL specialPricingOptionsBLObj = new SpecialPricingOptionsBL(executionContext, specialPricingOptionsDTO);
                        specialPricingOptionsBLObj.Save();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Hard Deletions for SpecialPricingOptionsList
        /// </summary>
        public void DeleteSpecialPricingOptionsList()
        {
            log.LogMethodEntry();
            if (specialPricingOptionsList != null && specialPricingOptionsList.Count > 0)
            {
                foreach (SpecialPricingOptionsDTO specialPricingOptionsDTO in specialPricingOptionsList)
                {
                    if (specialPricingOptionsDTO.IsChanged && specialPricingOptionsDTO.ActiveFlag)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                SpecialPricingOptionsBL specialPricingOptionsBL = new SpecialPricingOptionsBL(executionContext);
                                specialPricingOptionsBL.DeleteSpecialPricingOptions(specialPricingOptionsDTO.PricingId, parafaitDBTrx.SQLTrx);
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
    }
}
