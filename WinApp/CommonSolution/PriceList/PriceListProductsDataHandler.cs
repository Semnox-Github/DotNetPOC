/********************************************************************************************
 * Project Name - PriceListProducts DataHandler 
 * Description  - DataHandler for PriceListProducts 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By            Remarks          
 ********************************************************************************************* 
 *2.60        06-Feb-2019    Indrajeet Kumar        Created 
              26-Mar-2019    Akshay Gulaganji       modified InsertProductCreditPlus(), UpdateProductCreditPlus() 
 *                                                  and added GetSQLParameters(), log.MethodEntry(), log.MethodExit() and exceptions
 *2.70        29-Jun-2019    Akshay Gulaganji       Added SqlTransactions and DeletePriceListProducts() method
 *2.70.2        30-Jul-2019    Deeksha                Modifications as per three tier standard.
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.PriceList
{
    public class PriceListProductsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PriceListProducts AS p ";

        private static readonly Dictionary<PriceListProductsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PriceListProductsDTO.SearchByParameters, string>
        {
            {PriceListProductsDTO.SearchByParameters.PRICELISTPRODUCT_ID, "p.PriceListProductId"},
            {PriceListProductsDTO.SearchByParameters.PRICELIST_ID, "p.PriceListId"},
            {PriceListProductsDTO.SearchByParameters.PRODUCT_ID, "p.ProductId"},
            {PriceListProductsDTO.SearchByParameters.SITE_ID,"p.site_id"},
            {PriceListProductsDTO.SearchByParameters.MASTERENTITY_ID,"p.MasterEntityId"},
            {PriceListProductsDTO.SearchByParameters.ISACTIVE, "p.IsActive"}
        };

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public PriceListProductsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// GetPriceListProductsDTO(priceListProductsDataRow) - to convert the dataRow to DTO Object
        /// </summary>
        /// <param name="priceListProductsDataRow"></param>
        /// <returns></returns>
        private PriceListProductsDTO GetPriceListProductsDTO(DataRow priceListProductsDataRow, SqlTransaction sqlTransaction = null) //added sqltansaction parameter
        {
            log.LogMethodEntry(priceListProductsDataRow);
            try
            {
                PriceListProductsDTO priceListProductsDTOObject = new PriceListProductsDTO(
                    Convert.ToInt32(priceListProductsDataRow["PriceListProductId"]),
                    priceListProductsDataRow["PriceListId"] == DBNull.Value ? -1 : Convert.ToInt32(priceListProductsDataRow["PriceListId"]),
                    priceListProductsDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(priceListProductsDataRow["ProductId"]),
                    priceListProductsDataRow["Price"] == DBNull.Value ? -1 : Convert.ToDecimal(priceListProductsDataRow["Price"]),
                    priceListProductsDataRow["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(priceListProductsDataRow["EffectiveDate"]),
                    priceListProductsDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(priceListProductsDataRow["LastUpdatedDate"]),
                    priceListProductsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(priceListProductsDataRow["LastUpdatedBy"]),
                    priceListProductsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(priceListProductsDataRow["Guid"]),
                    priceListProductsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(priceListProductsDataRow["SynchStatus"])),
                    priceListProductsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(priceListProductsDataRow["site_id"]),
                    priceListProductsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(priceListProductsDataRow["MasterEntityId"]),
                    priceListProductsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(priceListProductsDataRow["CreatedBy"]),
                    priceListProductsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(priceListProductsDataRow["CreationDate"]),
                    priceListProductsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(priceListProductsDataRow["IsActive"])
                    );
                log.LogMethodExit(priceListProductsDTOObject);
                return priceListProductsDTOObject;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PriceListProducts Record.
        /// </summary>
        /// <param name="PriceListProductsDTO">PriceListProductsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PriceListProductsDTO priceListProductsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(priceListProductsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PriceListProductId", priceListProductsDTO.PriceListProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PriceListId", priceListProductsDTO.PriceListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", priceListProductsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Price", priceListProductsDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", priceListProductsDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", priceListProductsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", priceListProductsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts PriceListProducts
        /// </summary>
        /// <param name="priceListProductsDTO">priceListProductsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>priceListProductsDTO</returns>
        public PriceListProductsDTO InsertPriceListProducts(PriceListProductsDTO priceListProductsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(priceListProductsDTO, loginId, siteId);
            string insertPriceListProductsQuery = @"Insert into PriceListProducts
                                                            (
                                                                PriceListId,
                                                                ProductId,
                                                                Price,
                                                                EffectiveDate,
                                                                LastUpdatedDate,
                                                                LastUpdatedBy,
                                                                Guid,
                                                                site_id,
                                                                CreatedBy,
                                                                CreationDate,
                                                                IsActive
                                                            )
                                                           values
                                                            (
                                                                @PriceListId,
                                                                @ProductId,
                                                                @Price,
                                                                @EffectiveDate,
                                                                GetDate(),
                                                                @LastUpdatedBy,
                                                                NEWID(),
                                                                @site_id,
                                                                @createdBy,
                                                                GetDate(),
                                                                @IsActive
                                                            )SELECT * FROM PriceListProducts WHERE PriceListProductId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertPriceListProductsQuery, GetSQLParameters(priceListProductsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPriceListProductsDTO(priceListProductsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting priceListProductsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(priceListProductsDTO);
            return priceListProductsDTO;
        }

        /// <summary>
        /// Updates PriceListProducts
        /// </summary>
        /// <param name="priceListProductsDTO">priceListProductsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>priceListProductsDTO</returns>
        public PriceListProductsDTO UpdatePriceListProducts(PriceListProductsDTO priceListProductsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(priceListProductsDTO, loginId, siteId);
            string updatePriceListProductsQuery = @"Update PriceListProducts
                                                        set
                                                                PriceListId     = @PriceListId,
                                                                ProductId       = @ProductId,
                                                                Price           = @Price,
                                                                EffectiveDate   = @EffectiveDate,
                                                                MasterEntityId  = @MasterEntityId,
                                                                LastUpdatedDate = GetDate(),
                                                                LastUpdatedBy   = @LastUpdatedBy,
                                                                -- site_id         = @site_id,
                                                                IsActive        = @IsActive
                                                            where 
                                                                PriceListProductId = @PriceListProductId
                                                            and 
                                                                PriceListId     = @PriceListId
                                SELECT * FROM PriceListProducts WHERE PriceListProductId = @PriceListProductId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updatePriceListProductsQuery, GetSQLParameters(priceListProductsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPriceListProductsDTO(priceListProductsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating priceListProductsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(priceListProductsDTO);
            return priceListProductsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="priceListProductsDTO">PriceListProductsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPriceListProductsDTO(PriceListProductsDTO priceListProductsDTO, DataTable dt)
        {
            log.LogMethodEntry(priceListProductsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                priceListProductsDTO.PriceListProductId = Convert.ToInt32(dt.Rows[0]["PriceListProductId"]);
                priceListProductsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                priceListProductsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                priceListProductsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                priceListProductsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                priceListProductsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                priceListProductsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets PriceListsProducts List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<PriceListProductsDTO> GetAllPriceListProductsList(List<KeyValuePair<PriceListProductsDTO.SearchByParameters, string>> searchParameters,SqlTransaction sqlTransaction = null) //added sqltransaction parameter
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectProductCreditPlusQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PriceListProductsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PriceListProductsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key.Equals(PriceListProductsDTO.SearchByParameters.PRICELIST_ID) ||
                                 searchParameter.Key.Equals(PriceListProductsDTO.SearchByParameters.PRODUCT_ID)||
                                 searchParameter.Key.Equals(PriceListProductsDTO.SearchByParameters.MASTERENTITY_ID) ||
                                 searchParameter.Key.Equals(PriceListProductsDTO.SearchByParameters.PRICELISTPRODUCT_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PriceListProductsDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                count++;
            }
            if (searchParameters.Count > 0)
                    selectProductCreditPlusQuery = selectProductCreditPlusQuery + query;
                selectProductCreditPlusQuery = selectProductCreditPlusQuery + " Order by PriceListProductId";
            }
            DataTable priceListProductsDataTable = dataAccessHandler.executeSelectQuery(selectProductCreditPlusQuery, parameters.ToArray(), sqlTransaction);
            List<PriceListProductsDTO> priceListProductsList = new List<PriceListProductsDTO>();
            if (priceListProductsDataTable.Rows.Count > 0)
            {              
                foreach (DataRow dataRow in priceListProductsDataTable.Rows)
                {
                    PriceListProductsDTO priceListProductsDTO = GetPriceListProductsDTO(dataRow, sqlTransaction);
                    priceListProductsList.Add(priceListProductsDTO);
                }
            }
            log.LogMethodEntry(priceListProductsList);
            return priceListProductsList;
        }

        internal PriceListProductsDTO GetPriceListProductsId(int priceListProductId) //added 
        {
            log.LogMethodEntry(priceListProductId);
            PriceListProductsDTO priceListProductsDTO = null;
            string query = SELECT_QUERY + @" WHERE p.PriceListProductId = @PriceListProductId";
            SqlParameter parameter = new SqlParameter("@PriceListProductId", priceListProductId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                priceListProductsDTO = GetPriceListProductsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(priceListProductsDTO);
            return priceListProductsDTO;
        }


        /// <summary>
        /// Based on the priceListProductId, appropriate PriceListProducts record will be deleted
        /// </summary>
        /// <param name="priceListProductId">priceListProductId</param>
        /// <returns>return the int</returns>
        public int DeletePriceListProducts(int priceListProductId)
        {
            log.LogMethodEntry(priceListProductId);
            try
            {
                string deleteQuery = @"delete from PriceListProducts where PriceListProductId = @priceListProductId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@priceListProductId", priceListProductId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }
    }
}
