
/********************************************************************************************
 * Project Name - PriceList Data Handler
 * Description  - Data handler of the PriceList class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016    Amaresh            Created 
 *2.60        05-Feb-2019    Indrajeet Kumar    Added --> InsertPriceList, UpdatePriceList and modified GetPriceListList Method
 **********************************************************************************************
 *2.60        27-Mar-2019    Akshay Gulaganji   Modified --> InsertPriceList, UpdatePriceList 
 *                                              and added GetSQLParameters(),try & catch block in GetPriceListDTO(), log.MethodEntry(), log.MethodExit()
 *2.70        29-Jun-2019    Akshay Gulaganji   Added SqlTransaction, DeletePriceList() method
 *2.70.2        17-Jul-2019    Deeksha            Modifications as per three tier standard.
 *2.70.2        10-Dec-2019   Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.PriceList
{
    /// <summary>
    /// PriceList DataHandler - select of PriceList objects
    /// </summary>
    /// 
    public class PriceListDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PriceList AS pl ";
        /// <summary>
        /// Dictionary for searching Parameters for the PriceList  object.
        /// </summary>
        private static readonly Dictionary<PriceListDTO.SearchByPriceListParameters, string> DBSearchParameters = new Dictionary<PriceListDTO.SearchByPriceListParameters, string>
        {
              {PriceListDTO.SearchByPriceListParameters.PRICE_LIST_ID, "pl.PriceListId"},
              {PriceListDTO.SearchByPriceListParameters.SITE_ID, "pl.Site_id"},
              {PriceListDTO.SearchByPriceListParameters.IS_ACTIVE, "pl.IsActive"},
              {PriceListDTO.SearchByPriceListParameters.MASTER_ENTITY_ID, "pl.MasterEntityId"}
        };
        
        /// <summary>
        /// Default constructor of PriceListDataHandler class
        /// </summary>
        public PriceListDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryDocumentTypeDataHandler Record.
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PriceListDTO priceListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(priceListDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@PriceListId", priceListDTO.PriceListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PriceListName", priceListDTO.PriceListName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", priceListDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", priceListDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PriceListDTO class type
        /// </summary>
        /// <param name="priceListDataRow">PriceList DataRow</param>
        /// <returns>Returns PriceList</returns>
        private PriceListDTO GetPriceListDTO(DataRow priceListDataRow, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(priceListDataRow);
            PriceListDTO PriceListDataObject = new PriceListDTO(Convert.ToInt32(priceListDataRow["PriceListId"]),
                                                    priceListDataRow["PriceListName"] == DBNull.Value ? string.Empty : Convert.ToString(priceListDataRow["PriceListName"]),
                                                    priceListDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(priceListDataRow["Guid"]),
                                                    priceListDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(priceListDataRow["SynchStatus"]),
                                                    priceListDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(priceListDataRow["site_id"]),
                                                    priceListDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(priceListDataRow["LastUpdatedDate"]),
                                                    priceListDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(priceListDataRow["LastUpdatedBy"]),
                                                    priceListDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(priceListDataRow["MasterEntityId"]),
                                                    priceListDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(priceListDataRow["CreatedBy"]),
                                                    priceListDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(priceListDataRow["CreationDate"]),
                                                    priceListDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(priceListDataRow["IsActive"])
                                                    );
            log.LogMethodExit(PriceListDataObject);
            return PriceListDataObject;
        }


        /// <summary>
        /// Inserts the PriceList record to the database
        /// </summary>
        /// <param name="priceListDTO">PriceList type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public PriceListDTO InsertPriceList(PriceListDTO priceListDTO, string loginId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(priceListDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[PriceList]
                                                        (    
                                                          PriceListName,
                                                          Guid,
                                                          site_id,
                                                          LastUpdatedDate,
                                                          LastUpdatedBy,
                                                          MasterEntityId,
                                                          CreatedBy,
                                                          CreationDate,
                                                          IsActive
                                                        ) 
                                                values 
                                                        (    
                                                          @PriceListName,
                                                          NEWID(),
                                                          @site_id,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          @MasterEntityId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @IsActive)
                                               SELECT * FROM PriceList WHERE PriceListId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(priceListDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPriceListDTO(priceListDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting priceListDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(priceListDTO);
            return priceListDTO;
        }



        /// <summary>
        /// Updates the PriceList record
        /// </summary>
        /// <param name="priceListDTO">PriceListDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public PriceListDTO UpdatePriceList(PriceListDTO priceListDTO, string loginId, int siteId, SqlTransaction sql = null)
        {
            log.LogMethodEntry(priceListDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[PriceList]
                                    SET 
                                             PriceListName = @PriceListName,
                                             --Site_id = @site_id,
                                             MasterEntityId = @MasterEntityId,
                                             LastupdatedDate = GETDATE(),
                                             LastUpdatedBy = @LastUpdatedBy,
                                             IsActive        =   @IsActive
                                       WHERE PriceListId =@PriceListId 
                                    SELECT * FROM PriceList WHERE PriceListId = @PriceListId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(priceListDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPriceListDTO(priceListDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating priceListDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(priceListDTO);
            return priceListDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="priceListDTO">priceListDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPriceListDTO(PriceListDTO priceListDTO, DataTable dt)
        {
            log.LogMethodEntry(priceListDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                priceListDTO.PriceListId = Convert.ToInt32(dt.Rows[0]["PriceListId"]);
                priceListDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                priceListDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                priceListDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                priceListDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                priceListDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                priceListDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PriceList data of passed id 
        /// </summary>
        /// <param name="id">id of PriceList is passed as parameter</param>
        /// <returns>Returns PriceList</returns>
        public PriceListDTO GetPriceList(int id, SqlTransaction sqlTransaction = null)  //sqlTransaction parameter;
        {
            log.LogMethodEntry(id);
            PriceListDTO result = null;
            string query = SELECT_QUERY + @" WHERE pl.PriceListId= @PriceListId";
            SqlParameter parameter = new SqlParameter("@PriceListId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPriceListDTO(dataTable.Rows[0],sqlTransaction);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal DateTime? GetPriceListModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from PriceList WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from PriceListProducts WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Gets the PriceListDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PriceListDTO matching the search criteria</returns>
        public List<PriceListDTO> GetPriceListList(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters,SqlTransaction sqlTrxn=null)
        {
            log.LogMethodEntry(searchParameters, sqlTrxn);
            List<PriceListDTO> priceListDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectPriceListQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PriceListDTO.SearchByPriceListParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        

                        if (searchParameter.Key.Equals(PriceListDTO.SearchByPriceListParameters.PRICE_LIST_ID)||
                            searchParameter.Key.Equals(PriceListDTO.SearchByPriceListParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PriceListDTO.SearchByPriceListParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PriceListDTO.SearchByPriceListParameters.IS_ACTIVE)
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
                    selectPriceListQuery = selectPriceListQuery + query;
                selectPriceListQuery = selectPriceListQuery + " Order by PriceListId";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectPriceListQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                priceListDTOList = new List<PriceListDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PriceListDTO priceListDTO = GetPriceListDTO(dataRow);
                    priceListDTOList.Add(priceListDTO);
                }
            }
            log.LogMethodExit(priceListDTOList);
            return priceListDTOList;
        }

        /// <summary>
        /// Delete the record from the PriceList database based on PriceListId
        /// </summary>
        /// <returns>return the int </returns>
        internal int DeletePriceList(int priceListId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(priceListId,sqlTransaction);
            string query = @"DELETE  
                             FROM PriceList
                             WHERE PriceList.PriceListId = @PriceListId";
            SqlParameter parameter = new SqlParameter("@PriceListId", priceListId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }
    }
}
