/********************************************************************************************
 * Project Name - ProductCalenderDTO Programs 
 * Description  - Data object of the ProductCalenderDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Rakshith           Created 
 *2.70        29-Jan-2019   Jagan Mohana       Created Class ProductsCalenderList Method and Constructor
 *                                             SaveUpdateProductCalenderList() & Save() Method
 *2.70        27-Jun-2019   Nagesh Badiger     Added DeleteProductsCalender() and DeleteProductCalenderList() methods
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Product
{

    /// <summary>
    ///   ProductCalender class
    /// </summary>
    public class ProductsCalender
    {
        ProductsCalenderDTO productsCalenderDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductsCalender(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productsCalenderDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with executionContext and productsCalenderDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productsCalenderDTO"></param>
        public ProductsCalender(ExecutionContext executionContext, ProductsCalenderDTO productsCalenderDTO)
        {
            log.LogMethodEntry(productsCalenderDTO, executionContext);
            this.productsCalenderDTO = productsCalenderDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public List<ProductsCalenderDTO> GetAllProductCalenderList(int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId);
            ProductsCalenderDatahandler productCalenderDatahandler = new ProductsCalenderDatahandler(sqlTransaction);
            log.LogMethodExit();
            return productCalenderDatahandler.GetAllProductCalenderList(productId);
        }
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.Debug("Starts-Save() method.");
            SetToSiteDateTime();
            ProductsCalenderDatahandler productsCalenderDatahandler = new ProductsCalenderDatahandler(sqlTransaction);
            if (productsCalenderDTO.ProductCalendarId < 0)
            {
                int productCalender_Id = productsCalenderDatahandler.InsertProductCalender(productsCalenderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productsCalenderDTO.ProductCalendarId = productCalender_Id;
            }
            else
            {
                if (productsCalenderDTO.ProductCalendarId > 0 && productsCalenderDTO.IsChanged == true)
                {
                    productsCalenderDatahandler.UpdateProductCalender(productsCalenderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productsCalenderDTO.AcceptChanges();
                }
            }
            SetToSiteFromTime();
            log.Debug("Ends-Save() method.");
        }
        /// <summary>
        /// Delete the ProductCalendar details based on productCalendarId
        /// </summary>
        /// <param name="productCalendarId">productCalendarId</param>        
        public void DeleteProductsCalender(int productCalendarId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productCalendarId, sqlTransaction);
            try
            {
                ProductsCalenderDatahandler productsCalenderDatahandler = new ProductsCalenderDatahandler(sqlTransaction);
                productsCalenderDatahandler.DeleteProductsCalender(productCalendarId);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private void SetToSiteDateTime()
        {
            log.LogMethodEntry(productsCalenderDTO);
            if (productsCalenderDTO != null)
            {
                if (SiteContainerList.IsCorporate())
                {
                    int siteId = executionContext.GetSiteId();
                    log.Info(siteId);
                    if (productsCalenderDTO.Date != DateTime.MinValue && (productsCalenderDTO.ProductCalendarId < 0 || (productsCalenderDTO.ProductCalendarId > -1 && productsCalenderDTO.IsChanged)))
                    {
                        productsCalenderDTO.Date = SiteContainerList.ToSiteDateTime(siteId, productsCalenderDTO.Date);
                    }
                }
            }
            log.LogMethodExit(productsCalenderDTO);
        }

        private void SetToSiteFromTime()
        {
            log.LogMethodEntry(productsCalenderDTO);
            if (SiteContainerList.IsCorporate())
            {
                if (productsCalenderDTO != null)
                {
                    if (productsCalenderDTO.Date != DateTime.MinValue)
                    {
                        productsCalenderDTO.Date = SiteContainerList.FromSiteDateTime(productsCalenderDTO.Site_id, productsCalenderDTO.Date);
                    }
                    productsCalenderDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(productsCalenderDTO);
        }
    }


    /// <summary>
    ///   ProductCalenderList class
    /// </summary>
    public class ProductsCalenderList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductsCalenderDTO> productsCalenderList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductsCalenderList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.productsCalenderList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="productsCalenderList"></param>
        /// <param name="executionContext"></param>
        public ProductsCalenderList(ExecutionContext executionContext, List<ProductsCalenderDTO> productsCalenderList)
        {
            log.LogMethodEntry(productsCalenderList, executionContext);
            this.executionContext = executionContext;
            this.productsCalenderList = productsCalenderList;
            log.LogMethodExit();
        }

        public List<ProductsCalenderDTO> GetAllProductCalenderList(List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProductsCalenderDatahandler productsCalenderDatahandler = new ProductsCalenderDatahandler(sqlTransaction);
            log.LogMethodExit();
            return SetToSiteFromTime(productsCalenderDatahandler.GetAllProductCalenderList(searchParameters));
        }

        /// <summary>
        /// Save Update Product Category List
        /// </summary>
        public void SaveUpdateProductCalenderList()
        {
            try
            {
                log.LogMethodEntry();
                if (productsCalenderList != null && productsCalenderList.Any())
                {
                    foreach (ProductsCalenderDTO productsCalenderDTO in productsCalenderList)
                    {
                        ProductsCalender productCalenderObj = new ProductsCalender(executionContext, productsCalenderDTO);
                        productCalenderObj.Save();
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
        /// Delete DeleteProductCalenderList
        /// </summary>
        public void DeleteProductCalenderList()
        {
            log.LogMethodEntry();
            if (productsCalenderList != null && productsCalenderList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ProductsCalenderDTO productsCalenderDTO in productsCalenderList)
                    {
                        if (productsCalenderDTO.IsChanged && productsCalenderDTO.IsActive == false)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProductsCalender productsCalender = new ProductsCalender(executionContext);
                                productsCalender.DeleteProductsCalender(productsCalenderDTO.ProductCalendarId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                log.LogMethodExit(null, "Throwing validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// Gets the ProductsCalenderDetailsDTO List for product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ProductsCalenderDTO> GetProductsCalenderDTOListForProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            ProductsCalenderDatahandler productsCalenderDataHandler = new ProductsCalenderDatahandler(sqlTransaction);
            List<ProductsCalenderDTO> productsCalenderDTOList = productsCalenderDataHandler.GetProductsCalenderDTOList(productIdList, activeRecords);
            productsCalenderDTOList = SetToSiteFromTime(productsCalenderDTOList);
            log.LogMethodExit(productsCalenderDTOList);
            return productsCalenderDTOList;
        }

        private List<ProductsCalenderDTO> SetToSiteFromTime(List<ProductsCalenderDTO> productsCalenderDTOList)
        {
            log.LogMethodEntry(productsCalenderDTOList);
            if (SiteContainerList.IsCorporate())
            {
                if (productsCalenderDTOList != null && productsCalenderDTOList.Any())
                {
                    for (int i = 0; i < productsCalenderDTOList.Count; i++)
                    {
                        if (productsCalenderDTOList[i].Date != DateTime.MinValue)
                        {
                            productsCalenderDTOList[i].Date = SiteContainerList.FromSiteDateTime(productsCalenderDTOList[i].Site_id, productsCalenderDTOList[i].Date);
                        }
                        productsCalenderDTOList[i].AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(productsCalenderDTOList);
            return productsCalenderDTOList;
        }
    }
}