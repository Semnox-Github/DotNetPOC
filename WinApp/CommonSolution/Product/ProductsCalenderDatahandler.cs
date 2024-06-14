/********************************************************************************************
 * Project Name - Product Calender Datahandler
 * Description  - Data handler of the Product Calender Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   rakshith                    Created 
 *2.70        29-Jan-2019   Jagan Mohana                Created a method GetAllProductCalenderList() List as a Parameter
 *                                                      InsertProductCalender() and UpdateProductCalender()
 *            26-Apr-2019   Akshay G                    modified showHide dataType(from char to bool), InsertProductCalender(), UpdateProductCalender() and added GetSQLParameters()
 *            21-Jun-2019   Nagesh Badiger              Added DeleteProductsCalender() method.
 *2.70.2        10-Dec-2019   Jinto Thomas                Removed siteid from update query            
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using System.Text;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Class ProductCalenderDatahandler
    /// </summary>
    public class ProductsCalenderDatahandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<ProductsCalenderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductsCalenderDTO.SearchByParameters, string>
        {
            {ProductsCalenderDTO.SearchByParameters.PRODUCT_ID, "Product_Id"},
            {ProductsCalenderDTO.SearchByParameters.PRODUCT_CALENDER_ID, "ProductCalendarId"},
            {ProductsCalenderDTO.SearchByParameters.SITE_ID,"site_id"},
            {ProductsCalenderDTO.SearchByParameters.IS_ACTIVE,"IsActive"},
            {ProductsCalenderDTO.SearchByParameters.MASTERENTITY_ID,"MasterEntityId"},
            {ProductsCalenderDTO.SearchByParameters.PRODUCT_ID_LIST,"Product_Id"},
            {ProductsCalenderDTO.SearchByParameters.SHOWHIDE,"ShowHide"}
        };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ProductCalenderDatahandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public ProductsCalenderDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating product special pricing Record.
        /// </summary>
        /// <param name="specialPricingOptionsDTO">LookupsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductsCalenderDTO productsCalenderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsCalenderDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductCalendarId", productsCalenderDTO.ProductCalendarId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Product_Id", productsCalenderDTO.Product_Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Day", productsCalenderDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Date", (productsCalenderDTO.Date)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromTime", productsCalenderDTO.FromTime == Double.NaN ? 0 : productsCalenderDTO.FromTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToTime", productsCalenderDTO.ToTime == Double.NaN ? 0 : productsCalenderDTO.ToTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ShowHide", productsCalenderDTO.ShowHide == true ? 'Y' : 'N'));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the Product Calender record to the database
        /// </summary>
        /// <param name="productsCalender"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns inserted record id</returns>
        public int InsertProductCalender(ProductsCalenderDTO productsCalenderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsCalenderDTO, userId, siteId);
            int idOfRowInserted = 0;
            string insertProductCalenderQuery = @"INSERT INTO ProductCalendar 
                                                                       (
                                                                          Product_Id,
                                                                          Day,
                                                                          Date,
                                                                          FromTime,
                                                                          ToTime,
                                                                          ShowHide,
                                                                          Guid,
                                                                          site_id,
                                                                          CreatedBy,
                                                                          CreationDate,
                                                                          LastUpdatedBy,
                                                                          LastUpdateDate
                                                                        )
                                                                   values
                                                                        (
                                                                           @Product_Id,
                                                                           @Day,
                                                                           @Date,
                                                                           @FromTime,
                                                                           @ToTime,
                                                                           @ShowHide,
                                                                           NEWID(),
                                                                           @site_id,                                                            
                                                                           @createdBy,
                                                                           GetDate(),                                                          
                                                                           @lastUpdatedBy,
                                                                           GetDate()
                                                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertProductCalenderQuery, GetSQLParameters(productsCalenderDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Update the Product Calender record to the database
        /// </summary>
        /// <param name="productsCalender"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns # of rows updated</returns>
        public int UpdateProductCalender(ProductsCalenderDTO productsCalenderDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsCalenderDTO, userId, siteId);
            int rowsUpdated;
            string UpdateProductCalenderQuery = @"update ProductCalendar
						                                    set							                                   
							                                    Day = @Day,
							                                    Date = @Date,
							                                    FromTime = @FromTime,
							                                    ToTime = @ToTime,
							                                    ShowHide = @ShowHide,
							                                    -- site_id = @site_id,
                                                                LastUpdateDate = GetDate(),
                                                                LastUpdatedBy = @lastUpdatedBy 
							                                Where 
                                                                Product_Id = @Product_Id 
                                                            And 
                                                                ProductCalendarId = @ProductCalendarId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(UpdateProductCalenderQuery, GetSQLParameters(productsCalenderDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// GetProductCalenderDTO(DataRow productCalenderRow)
        /// </summary>
        /// <param name="productCalenderRow">productCalenderRow</param>
        /// <returns> returns ProductCalenderDTO object</returns>
        public ProductsCalenderDTO GetProductCalenderDTO(DataRow productCalenderRow)
        {
            log.LogMethodEntry(productCalenderRow);
            try
            {
                ProductsCalenderDTO agentsDTO = new ProductsCalenderDTO(
                                                        productCalenderRow["ProductCalendarId"] == DBNull.Value ? -1 : Convert.ToInt32(productCalenderRow["ProductCalendarId"]),
                                                        productCalenderRow["Product_Id"] == DBNull.Value ? -1 : Convert.ToInt32(productCalenderRow["Product_Id"]),
                                                        productCalenderRow["Day"].ToString(),
                                                        productCalenderRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productCalenderRow["Date"]),
                                                        productCalenderRow["FromTime"] == DBNull.Value ? (double?)null : Convert.ToDouble(productCalenderRow["FromTime"]),
                                                        productCalenderRow["ToTime"] == DBNull.Value ? (double?)null : Convert.ToDouble(productCalenderRow["ToTime"]),
                                                        productCalenderRow["ShowHide"] == DBNull.Value ? true : Convert.ToChar(productCalenderRow["ShowHide"]) == 'Y' ? true : false,
                                                        productCalenderRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productCalenderRow["site_id"]),
                                                        productCalenderRow["Guid"].ToString(),
                                                        productCalenderRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productCalenderRow["SynchStatus"]),
                                                        productCalenderRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productCalenderRow["MasterEntityId"])

                                                        );
                log.LogMethodExit(agentsDTO);
                return agentsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        ///  GetAllProductCalenderList() method
        /// </summary>
        /// <returns>returns List of ProductCalenderDTO </returns>
        public List<ProductsCalenderDTO> GetAllProductCalenderList(int productId)
        {
            string productCalenderQuery = @"SELECT p.ProductCalendarId,p.Product_Id, d.Display as Day ,p.Date,p.FromTime,p.ToTime,p.ShowHide,p.site_id
                                               FROM ProductCalendar p 
                                               JOIN DayLookup d
                                               ON p.Day=d.Day
                                            where Product_Id=@product_Id ";
            SqlParameter[] queryParams = new SqlParameter[1];
            queryParams[0] = new SqlParameter("@product_Id", productId);

            DataTable dtProductCalender = dataAccessHandler.executeSelectQuery(productCalenderQuery, queryParams, sqlTransaction);
            if (dtProductCalender.Rows.Count > 0)
            {
                List<ProductsCalenderDTO> productCalenderDTOList = new List<ProductsCalenderDTO>();
                foreach (DataRow productCalenderDTORow in dtProductCalender.Rows)
                {
                    ProductsCalenderDTO productCalenderDTO = GetProductCalenderDTO(productCalenderDTORow);
                    productCalenderDTOList.Add(productCalenderDTO);
                }
                log.Debug("Ends-GetAllProductCalenderList(productId) Method by returning productCalenderDTOList.");
                return productCalenderDTOList;
            }
            else
            {
                log.Debug("Ends-GetAllProductCalenderList(productId) Method by returning null.");
                List<ProductsCalenderDTO> productCalenderDTOList = new List<ProductsCalenderDTO>();
                return productCalenderDTOList;
            }

        }


        internal List<ProductsCalenderDTO> GetProductsCalenderDTOList(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<ProductsCalenderDTO> productIdListDetailsDTOList = new List<ProductsCalenderDTO>();
            string query = @"SELECT *
                            FROM ProductCalendar, @productIdList List
                            WHERE Product_Id = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductCalenderDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }

        /// <summary>
        /// Gets the ProductCalender list 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ProductsCalenderDTO> GetAllProductCalenderList(List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;

            string selectProductCalenderQuery = @"select * from  ProductCalendar";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductsCalenderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductsCalenderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor+ " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1) ");
                        }
                        else if (searchParameter.Key.Equals(ProductsCalenderDTO.SearchByParameters.PRODUCT_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key.Equals(ProductsCalenderDTO.SearchByParameters.PRODUCT_CALENDER_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + Int32.Parse(searchParameter.Value) + "' ");
                        }
                        else if (searchParameter.Key == ProductsCalenderDTO.SearchByParameters.PRODUCT_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " in (" + searchParameter.Value + " )");
                        }
                        else if (searchParameter.Key.Equals(ProductsCalenderDTO.SearchByParameters.SHOWHIDE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? 'Y' : 'N') + "'");
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + "'" + searchParameter.Value + "'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit();
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectProductCalenderQuery = selectProductCalenderQuery + query;
                selectProductCalenderQuery = selectProductCalenderQuery + "Order by ProductCalendarId";
            }
            DataTable productCalenderDataTable = dataAccessHandler.executeSelectQuery(selectProductCalenderQuery, null, sqlTransaction);
            List<ProductsCalenderDTO> productCalenderList = new List<ProductsCalenderDTO>();
            if (productCalenderDataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in productCalenderDataTable.Rows)
                {
                    ProductsCalenderDTO productCalenderObject = GetProductCalenderDTO(dataRow);
                    productCalenderList.Add(productCalenderObject);
                }
            }
            log.LogMethodExit(productCalenderList);
            return productCalenderList;
        }

        /// <summary>
        /// Based on the productCalendarId, appropriate ProductCalendar record will be deleted
        /// </summary>
        /// <param name="productCalendarId">productCalendarId</param>
        /// <returns>return the int </returns>
        public int DeleteProductsCalender(int productCalendarId)
        {
            log.LogMethodEntry(productCalendarId);
            try
            {
                string deleteQuery = @"delete from ProductCalendar where ProductCalendarId = @productCalendarId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@productCalendarId", productCalendarId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
    }
}