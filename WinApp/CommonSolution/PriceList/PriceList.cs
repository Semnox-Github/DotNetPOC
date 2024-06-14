/********************************************************************************************
 * Project Name - PriceList
 * Description  - Bussiness logic of the  Price List class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        18-may-2016   Amaresh          Created 
 *2.50        05-Feb-2019   Indrajeet Kumar  Added --> GetAllPriceListProducts, SaveUpdatePriceList, Save Method,
 *                                           Created Constructor.
 *2.70        29-Jun-2019   Akshay Gulaganji Added SqlTransaction, DeletePriceList() and DeletePriceListList() methods
 *2.70.2        17-Jul-2019   Deeksha          Modified :Added log() methods.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.PriceList
{
    /// <summary>
    /// Price List
    /// </summary>

    public class PriceList
    {
        private PriceListDTO priceList;
        private readonly SqlTransaction sqlTransaction;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ConcurrentPrograms class
        /// </summary>
        public PriceList()
        {
            log.LogMethodEntry();
            this.priceList = null;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        public PriceList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.priceList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the priceList Id  as the parameter
        /// Would fetch the PriceListDTO object from the database based on the PriceList id passed. 
        /// </summary>
        /// <param name="priceListId">PriceList id </param>
        public PriceList(int priceListId, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(priceListId, sqlTransaction);
            PriceListDataHandler priceListDataHandler = new PriceListDataHandler(sqlTransaction);
            priceList = priceListDataHandler.GetPriceList(priceListId);
            SetFromSiteTimeOffset();
            log.LogMethodExit(priceList);
        }

        /// <summary>
        /// Creates priceList object using the PriceListDTO
        /// </summary>
        /// <param name="priceList">PriceListDTO object</param>
        public PriceList(PriceListDTO priceList)
            : this()
        {
            log.LogMethodEntry(priceList);
            this.priceList = priceList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="priceList"></param>
        /// <param name="executionContext"></param>
        public PriceList(ExecutionContext executionContext, PriceListDTO priceList)
        {
            log.LogMethodEntry(priceList, executionContext);
            this.executionContext = executionContext;
            this.priceList = priceList;
            log.LogMethodExit();
        }
        private List<ValidationError> Validation()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters = new List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>();
            searchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            PriceListList priceListList = new PriceListList(executionContext);
            List<PriceListDTO> priceListDTOList = priceListList.GetAllPriceList(searchParameters, sqlTransaction);
            if (priceListDTOList != null && priceListDTOList.Any())
            {
                var ispriceListNameExist = priceListDTOList.Find(m => m.PriceListName == priceList.PriceListName && m.PriceListId != priceList.PriceListId);
                if (ispriceListNameExist != null)
                {
                    validationErrorList.Add(new ValidationError("PriceList", "PriceListName", MessageContainerList.GetMessage(executionContext, 5122)));
                }
                log.LogMethodExit();
            }
            return validationErrorList;
        }
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PriceListDataHandler priceListDataHandler = new PriceListDataHandler(sqlTransaction);
            SetToSiteTimeOffset();
            List<ValidationError> validationErrorList = Validation();
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (priceList.PriceListId < 0)
            {
                priceList = priceListDataHandler.InsertPriceList(priceList, executionContext.GetUserId(), executionContext.GetSiteId());
                priceList.AcceptChanges();
            }
            else
            {
                if (priceList.PriceListId > 0 && priceList.IsChanged)
                {
                    priceList = priceListDataHandler.UpdatePriceList(priceList, executionContext.GetUserId(), executionContext.GetSiteId());
                    priceList.AcceptChanges();
                }
            }
            if (priceList.PriceListProductsList != null && priceList.PriceListProductsList.Count > 0)
            {
                foreach (PriceListProductsDTO priceListProductsDTO in priceList.PriceListProductsList)
                {
                    if (priceListProductsDTO != null)
                    {
                        if (priceListProductsDTO.PriceListId != priceList.PriceListId)
                        {
                            priceListProductsDTO.PriceListId = priceList.PriceListId;
                        }
                        PriceListProductsBL priceListProductsBLObj = new PriceListProductsBL(executionContext, priceListProductsDTO);
                        priceListProductsBLObj.Save();
                    }
                }
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit();
        }


        /// <summary>
        /// Deletes the PriceList based on PriceListId
        /// </summary>   
        /// <param name="priceListId">priceListId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeletePriceList(int priceListId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(priceListId, sqlTransaction);
            try
            {
                PriceListDataHandler priceListDataHandler = new PriceListDataHandler(sqlTransaction);
                priceListDataHandler.DeletePriceList(priceListId);
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

        private void SetFromSiteTimeOffset()
        {
            log.LogMethodEntry(priceList);
            if (SiteContainerList.IsCorporate() == false)
            {
                log.LogMethodExit(priceList, "local site");
                return;
            }
            if (priceList == null)
            {
                log.LogMethodExit(priceList, "priceList is empty");
                return;
            }
            if (priceList.PriceListProductsList == null || priceList.PriceListProductsList.Any() == false)
            {
                log.LogMethodExit(priceList.PriceListProductsList, "PriceListProductsList is empty");
                return;
            }
            for (int i = 0; i < priceList.PriceListProductsList.Count; i++)
            {
                if (priceList.PriceListProductsList[i].EffectiveDate.HasValue == false)
                {
                    continue;
                }
                priceList.PriceListProductsList[i].EffectiveDate = SiteContainerList.FromSiteDateTime(priceList.PriceListProductsList[i].Site_id, priceList.PriceListProductsList[i].EffectiveDate.Value);
            }
            priceList.AcceptChanges();
            log.LogMethodExit(priceList);
        }
        private void SetToSiteTimeOffset()
        {
            log.LogMethodEntry(priceList);
            if (SiteContainerList.IsCorporate())
            {
                if (priceList != null && (priceList.PriceListId == -1 || priceList.IsChangedRecursive))
                {
                    int siteId = executionContext.GetSiteId();
                    log.Info(siteId);
                    if (priceList.PriceListProductsList != null && priceList.PriceListProductsList.Any())
                    {
                        for (int i = 0; i < priceList.PriceListProductsList.Count; i++)
                        {
                            if (priceList.PriceListProductsList[i].EffectiveDate.HasValue == false)
                            {
                                continue;
                            }
                            priceList.PriceListProductsList[i].EffectiveDate = SiteContainerList.ToSiteDateTime(siteId, (DateTime)priceList.PriceListProductsList[i].EffectiveDate);
                        }
                    }
                }
            }
            log.LogMethodExit(priceList);
        }
    }
    /// <summary>
    /// Manages the list of Price List
    /// </summary>
    public class PriceListList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PriceListDTO> priceListDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PriceListList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public PriceListList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.priceListDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public PriceListList(ExecutionContext executionContext, List<PriceListDTO> priceList)
        {
            log.LogMethodEntry(priceList, executionContext);
            this.executionContext = executionContext;
            this.priceListDTOList = priceList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PriceList List 
        /// </summary>
        public List<PriceListDTO> GetAllPriceList(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PriceListDataHandler priceListDataHandler = new PriceListDataHandler(sqlTransaction);
            log.LogMethodExit();
            return priceListDataHandler.GetPriceListList(searchParameters);
        }

        /// <summary>
        /// Return PriceList and  PriceListProducts
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<PriceListDTO> GetAllPriceListProducts(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters, bool loadActiveRecordsOnly = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadActiveRecordsOnly, sqlTransaction);

            List<PriceListDTO> priceListList;
            List<PriceListProductsDTO> priceListProductsList;
            PriceListDataHandler priceListDataHandler = new PriceListDataHandler(sqlTransaction);

            priceListList = priceListDataHandler.GetPriceListList(searchParameters);
            if (priceListList != null && priceListList.Count > 0)
            {
                PriceListProductsBLList PriceListProductsBLList = new PriceListProductsBLList(executionContext);
                foreach (PriceListDTO priceListDTO in priceListList)
                {
                    List<KeyValuePair<PriceListProductsDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<PriceListProductsDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<PriceListProductsDTO.SearchByParameters, string>(PriceListProductsDTO.SearchByParameters.PRICELIST_ID, priceListDTO.PriceListId.ToString()));
                    if (loadActiveRecordsOnly)
                    {
                        searchByParameters.Add(new KeyValuePair<PriceListProductsDTO.SearchByParameters, string>(PriceListProductsDTO.SearchByParameters.ISACTIVE, "1"));
                    }
                    priceListProductsList = PriceListProductsBLList.GetPriceListProductsList(searchByParameters);
                    if (priceListProductsList != null)
                    {
                        priceListDTO.PriceListProductsList = new List<PriceListProductsDTO>(priceListProductsList);
                        priceListDTO.AcceptChanges();
                    }
                }
            }
            priceListList = SetFromSiteTimeOffset(priceListList);
            log.LogMethodEntry(priceListList);
            return priceListList;
        }

        /// <summary>
        /// Save and Update for Price List.
        /// </summary>
        public void SaveUpdatePriceList()
        {
            try
            {
                log.LogMethodEntry();
                if (priceListDTOList != null)
                {
                    foreach (PriceListDTO priceListDTO in priceListDTOList)
                    {
                        PriceList priceListObj = new PriceList(executionContext, priceListDTO);
                        priceListObj.Save();
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
        /// Delete PriceList List
        /// </summary>
        public void DeletePriceListList()
        {
            log.LogMethodEntry();
            if (priceListDTOList != null && priceListDTOList.Count > 0)
            {
                foreach (PriceListDTO priceListDTO in priceListDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (priceListDTO.PriceListProductsList != null && priceListDTO.PriceListProductsList.Count != 0)
                            {
                                foreach (PriceListProductsDTO priceListProductsDTO in priceListDTO.PriceListProductsList)
                                {
                                    if (priceListProductsDTO.IsChanged)
                                    {
                                        PriceListProductsBL priceListProductsBL = new PriceListProductsBL(executionContext);
                                        priceListProductsBL.DeletePriceListProducts(priceListProductsDTO.PriceListProductId, parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            if (priceListDTO.IsChanged)
                            {
                                PriceList priceListBL = new PriceList(executionContext);
                                priceListBL.DeletePriceList(priceListDTO.PriceListId, parafaitDBTrx.SQLTrx);
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
        /// Returns the max last updated date of price list module
        /// </summary>
        /// <param name="siteId">site identifier</param>
        /// <returns></returns>
        public DateTime? GetPriceListModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            PriceListDataHandler priceListDataHandler = new PriceListDataHandler();
            DateTime? result = priceListDataHandler.GetPriceListModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        private List<PriceListDTO> SetFromSiteTimeOffset(List<PriceListDTO> priceListList)
        {
            log.LogMethodEntry(priceListList);
            if (SiteContainerList.IsCorporate() == false)
            {
                log.LogMethodExit(priceListList, "local site");
                return priceListList;
            }
            if (priceListList == null || priceListList.Any() == false)
            {
                log.LogMethodExit(priceListList, "priceListList is empty");
                return priceListList;
            }
            for (int i = 0; i < priceListList.Count; i++)
            {
                if (priceListList[i].PriceListProductsList == null || priceListList[i].PriceListProductsList.Any() == false)
                {
                    continue;
                }
                for (int j = 0; j < priceListList[i].PriceListProductsList.Count; j++)
                {
                    if (priceListList[i].PriceListProductsList[j].EffectiveDate.HasValue == false)
                    {
                        continue;
                    }
                    priceListList[i].PriceListProductsList[j].EffectiveDate = SiteContainerList.FromSiteDateTime(priceListList[i].PriceListProductsList[j].Site_id, priceListList[i].PriceListProductsList[j].EffectiveDate.Value);
                }
                priceListList[i].AcceptChanges();
            }
            log.LogMethodExit(priceListList);
            return priceListList;
        }
    }
}
