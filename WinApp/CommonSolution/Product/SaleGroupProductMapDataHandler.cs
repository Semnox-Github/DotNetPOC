/********************************************************************************************
 * Project Name - Sales Offer Group Data Handler
 * Description  - The sales offer group data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Apr-2017   Raghuveera     Created 
 *2.70.2        10-Dec-2019   Jinto Thomas   Removed siteid from update query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class SaleGroupProductMapDataHandler
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string> DBSearchParameters = new Dictionary<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>
            {
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SALE_GROUP_ID, "SaleGroupId"},
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.PRODUCT_ID, "ProductId"},
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SQUENCE_ID, "SequenceId"},
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.IS_ACTIVE, "IsActive"},
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.TYPE_MAP_ID, "TypeMapId"},
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.MASTER_ENTITY_ID,"MasterEntityId"},                
                {SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SITE_ID, "site_id"}
            };
         DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of SaleGroupProductMapDataHandler class
        /// </summary>
        public SaleGroupProductMapDataHandler()
        {
            log.Debug("Starts-SaleGroupProductMapDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-SaleGroupProductMapDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the sales offer group record to the database
        /// </summary>
        /// <param name="saleGroupProductMap">SaleGroupProductMapDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTrxn">Sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSaleGroupProductMap(SaleGroupProductMapDTO saleGroupProductMap, string userId, int siteId, SqlTransaction sqlTrxn)
        {
            log.Debug("Starts-InsertSaleGroupProductMap(saleGroupProductMap, userId, siteId,sqlTrxn) method.");
            string insertSaleGroupProductMapQuery = @"INSERT INTO SaleGroupProductMap 
                                                        ( 
                                                        SaleGroupId,
                                                        ProductId,
                                                        SequenceId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedDate,
                                                        LastUpdatedUser,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (  
                                                        @saleGroupId,
                                                        @productId,
                                                        @sequenceId,
                                                        @isActive,
                                                        @createdBy,
                                                        Getdate(),
                                                        Getdate(),                                                         
                                                        @lastUpdatedBy,
                                                        NEWID(),
                                                        @siteid,
                                                        @masterEntityId
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateSaleGroupProductMapParameters = new List<SqlParameter>();
            if (saleGroupProductMap.SaleGroupId == -1)
            {
                throw new Exception("Sales group should not be null.");
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@saleGroupId", saleGroupProductMap.SaleGroupId));
            }
            if (saleGroupProductMap.ProductId == -1)
            {
                throw new Exception("Product should not be null.");
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@productId", saleGroupProductMap.ProductId));
            }
            if (saleGroupProductMap.SequenceId == -1)
            {
                throw new Exception("Squence should not be null.");
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@sequenceId", saleGroupProductMap.SequenceId));
            }

            updateSaleGroupProductMapParameters.Add(new SqlParameter("@isActive", saleGroupProductMap.IsActive));
            if (saleGroupProductMap.MasterEntityId == -1)
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@masterEntityId", saleGroupProductMap.MasterEntityId));
            }
            updateSaleGroupProductMapParameters.Add(new SqlParameter("@createdBy", userId));
            updateSaleGroupProductMapParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@siteId", siteId));            
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertSaleGroupProductMapQuery, updateSaleGroupProductMapParameters.ToArray(), sqlTrxn);
            log.Debug("Ends-InsertSaleGroupProductMap(saleGroupProductMap, userId, siteId,sqlTrxn) method.");
            return idOfRowInserted;
        }


        /// <summary>
        /// Updates the sales offer group record
        /// </summary>
        /// <param name="saleGroupProductMap">SaleGroupProductMapDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTrxn">Sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateSaleGroupProductMap(SaleGroupProductMapDTO saleGroupProductMap, string userId, int siteId, SqlTransaction sqlTrxn)
        {
            log.Debug("Starts-UpdateSaleGroupProductMap(saleGroupProductMap, userId, siteId,sqlTrxn) method.");
            string updateSaleGroupProductMapQuery = @"UPDATE SaleGroupProductMap 
                                         set SaleGroupId = @saleGroupId,
                                             ProductId = @productId,
                                             SequenceId = @sequenceId ,
                                             IsActive = @isActive,
                                             LastUpdatedUser = @lastUpdatedBy,
                                             LastUpdatedDate = getdate(),
                                             --site_id = @siteid,
                                             MasterEntityId = @masterEntityId                                           
                                       WHERE TypeMapId = @typeMapId";
            List<SqlParameter> updateSaleGroupProductMapParameters = new List<SqlParameter>();
            updateSaleGroupProductMapParameters.Add(new SqlParameter("@typeMapId", saleGroupProductMap.TypeMapId));
            if (saleGroupProductMap.SaleGroupId == -1)
            {
                throw new Exception("Sales group should not be null.");
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@saleGroupId", saleGroupProductMap.SaleGroupId));
            }
            if (saleGroupProductMap.ProductId == -1)
            {
                throw new Exception("Product should not be null.");
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@productId", saleGroupProductMap.ProductId));
            }
            if (saleGroupProductMap.SequenceId == -1)
            {
                throw new Exception("Squence should not be null.");
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@sequenceId", saleGroupProductMap.SequenceId));
            }

            updateSaleGroupProductMapParameters.Add(new SqlParameter("@isActive", saleGroupProductMap.IsActive));
            if (saleGroupProductMap.MasterEntityId == -1)
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@masterEntityId", DBNull.Value));
            }
            else
            {
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@masterEntityId", saleGroupProductMap.MasterEntityId));
            }
            updateSaleGroupProductMapParameters.Add(new SqlParameter("@lastUpdatedBy", userId));
            if (siteId == -1)
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@siteId", DBNull.Value));
            else
                updateSaleGroupProductMapParameters.Add(new SqlParameter("@siteId", siteId));
            
            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateSaleGroupProductMapQuery, updateSaleGroupProductMapParameters.ToArray(), sqlTrxn);
            log.Debug("Ends-UpdateSaleGroupProductMap(saleGroupProductMap, userId, siteId,sqlTrxn) method.");
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to SaleGroupProductMapDTO class type
        /// </summary>
        /// <param name="saleGroupProductMapDataRow">SaleGroupProductMap DataRow</param>
        /// <returns>Returns SaleGroupProductMap</returns>
        private SaleGroupProductMapDTO GetSaleGroupProductMapDTO(DataRow saleGroupProductMapDataRow)
        {
            log.Debug("Starts-GetSaleGroupProductMapDTO(saleGroupProductMapDataRow) method.");
            SaleGroupProductMapDTO saleGroupProductMapDataObject = new SaleGroupProductMapDTO(Convert.ToInt32(saleGroupProductMapDataRow["TypeMapId"]),
                                            saleGroupProductMapDataRow["SaleGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(saleGroupProductMapDataRow["SaleGroupId"]),
                                            saleGroupProductMapDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(saleGroupProductMapDataRow["ProductId"]),
                                            saleGroupProductMapDataRow["SequenceId"] == DBNull.Value ? -1 : Convert.ToInt32(saleGroupProductMapDataRow["SequenceId"]),
                                            saleGroupProductMapDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(saleGroupProductMapDataRow["IsActive"]),
                                            saleGroupProductMapDataRow["CreatedBy"].ToString(),
                                            saleGroupProductMapDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(saleGroupProductMapDataRow["CreationDate"]),
                                            saleGroupProductMapDataRow["LastUpdatedUser"].ToString(),
                                            saleGroupProductMapDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(saleGroupProductMapDataRow["LastUpdatedDate"]),
                                            saleGroupProductMapDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(saleGroupProductMapDataRow["site_id"]),
                                            saleGroupProductMapDataRow["Guid"].ToString(),
                                            saleGroupProductMapDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(saleGroupProductMapDataRow["SynchStatus"]),
                                            saleGroupProductMapDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(saleGroupProductMapDataRow["MasterEntityId"])
                                            );
            log.Debug("Ends-GetSaleGroupProductMapDTO(saleGroupProductMapDataRow) method.");
            return saleGroupProductMapDataObject;
        }

        /// <summary>
        /// Gets the sales offer group record which matches the locker card id  
        /// </summary>
        /// <param name="saleGroupId">integer type parameter</param>
        /// <returns>Returns SaleGroupProductMapDTO</returns>
        public SaleGroupProductMapDTO GetSaleGroupProductMapDTO(int saleGroupId)
        {
            log.Debug("Starts-GetSaleGroupProductMapOnCardId(saleGroupId) method.");
            string selectSaleGroupProductMapQuery = @"SELECT *
                                                   FROM SaleGroupProductMap
                                                   WHERE SaleGroupId = @saleGroupId";
            SqlParameter[] selectSaleGroupProductMapParameters = new SqlParameter[1];
            selectSaleGroupProductMapParameters[0] = new SqlParameter("@saleGroupId", saleGroupId);
            DataTable saleGroupProductMap = dataAccessHandler.executeSelectQuery(selectSaleGroupProductMapQuery, selectSaleGroupProductMapParameters);
            if (saleGroupProductMap.Rows.Count > 0)
            {
                DataRow saleGroupProductMapRow = saleGroupProductMap.Rows[0];
                SaleGroupProductMapDTO saleGroupProductMapDataObject = GetSaleGroupProductMapDTO(saleGroupProductMapRow);
                log.Debug("Ends-GetSaleGroupProductMapOnCardId(saleGroupId) method by returning saleGroupProductMapDataObject.");
                return saleGroupProductMapDataObject;
            }
            else
            {
                log.Debug("Ends-GetSaleGroupProductMapOnCardId(saleGroupId) method by returning null.");
                return null;
            }
        }

        internal List<SaleGroupProductMapDTO> GetSaleGroupProductMapDTOList(List<int> saleGroupIdList, bool activeRecords, SqlTransaction sqlTransaction) //added
        {
            log.LogMethodEntry(saleGroupIdList);
            List<SaleGroupProductMapDTO> saleGroupProductMapDTOList = new List<SaleGroupProductMapDTO>();
            string query = @"SELECT *
                            FROM SaleGroupProductMap, @saleGroupIdList List
                            WHERE SaleGroupId = List.Id ";
            if (activeRecords)
            {
                query += " AND (isActive is null or isActive = 1) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@saleGroupIdList", saleGroupIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                saleGroupProductMapDTOList = table.Rows.Cast<DataRow>().Select(x => GetSaleGroupProductMapDTO(x)).ToList();
            }
            log.LogMethodExit(saleGroupProductMapDTOList);
            return saleGroupProductMapDTOList;
        }

        /// <summary>
        /// Gets the SaleGroupProductMapDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of SaleGroupProductMapDTO matching the search criteria</returns>
        public List<SaleGroupProductMapDTO> GetSaleGroupProductMapList(List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetSaleGroupProductMapList(searchParameters) method.");
            int count = 0;
            string selectSaleGroupProductMapQuery = @"SELECT *
                                              FROM SaleGroupProductMap";
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SALE_GROUP_ID
                            || searchParameter.Key == SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.PRODUCT_ID
                            || searchParameter.Key == SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SQUENCE_ID
                            || searchParameter.Key == SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetSaleGroupProductMapList(searchParameters) method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSaleGroupProductMapQuery = selectSaleGroupProductMapQuery + query;
            }
            DataTable saleGroupProductMapData = dataAccessHandler.executeSelectQuery(selectSaleGroupProductMapQuery, null);
            if (saleGroupProductMapData.Rows.Count > 0)
            {
                List<SaleGroupProductMapDTO> saleGroupProductMapList = new List<SaleGroupProductMapDTO>();
                foreach (DataRow saleGroupProductMapDataRow in saleGroupProductMapData.Rows)
                {
                    SaleGroupProductMapDTO saleGroupProductMapDataObject = GetSaleGroupProductMapDTO(saleGroupProductMapDataRow);
                    saleGroupProductMapList.Add(saleGroupProductMapDataObject);
                }
                log.Debug("Ends-GetSaleGroupProductMapList(searchParameters) method by returning saleGroupProductMapList.");
                return saleGroupProductMapList;
            }
            else
            {
                log.Debug("Ends-GetSaleGroupProductMapList(searchParameters) method by returning null.");
                return null;
            }
        }
    }
}
