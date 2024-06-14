/********************************************************************************************
 * Project Name -POSPRintDisplayGroup DataHandler
 * Description  -Data object of POSPRintDisplayGroup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Nov-2016   Amaresh          Created 
 *2.70.2      06-Dec-2019   Jinto Thomas     Removed siteid from update query 
 *2.90        06-jul-2020   Girish Kundar    Modifed: 3 tier changes 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DisplayGroup
{
    /// <summary>
    /// POSPrintDisplayGroupDataHandler - Handles insert, update and select of POSPRintDisplayGroup objects
    /// </summary>
    public class POSPrintDisplayGroupDataHandler
    {
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string> DBSearchParameters = new Dictionary<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string>
            {
                {POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.ID, "Id"},
                {POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.POS_PRINTER_ID, "POSPrinterId"},
                {POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.DISPLAYGROUP_ID, "DisplayGroupId"},
                {POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.SITE_ID, "site_id"},
            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of POSPrintDisplayGroupDataHandler class
        /// </summary>
        public POSPrintDisplayGroupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrintDisplayGroupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", pOSPrintDisplayGroupDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pOSPrinterId", pOSPrintDisplayGroupDTO.POSPrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayGroupId", pOSPrintDisplayGroupDTO.DisplayGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", pOSPrintDisplayGroupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the POSPrintDisplayGroup record to the database
        /// </summary>
        /// <param name="pOSPrintDisplayGroupDTO">POSPrintDisplayGroupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public POSPrintDisplayGroupDTO InsertPOSPrintDisplayGroup(POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrintDisplayGroupDTO, loginId, siteId);
            string insertPOSPrintDisplayGroupyQuery = @"insert into POSPrintDisplayGroup 
                                                        (
                                                        POSPrinterId,
                                                        DisplayGroupId,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdateDate,
                                                         LastUpdatedBy
                                                        ) 
                                                values 
                                                       ( 
                                                        @pOSPrinterId,
                                                        @displayGroupId,
                                                        NEWID(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @CreatedBy,
                                                        GetDate(),
                                                        GetDate(),
                                                        @LastUpdatedBy
                                                        ) SELECT * FROM POSPrintDisplayGroup WHERE Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertPOSPrintDisplayGroupyQuery, GetSQLParameters(pOSPrintDisplayGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrintDisplayGroupDTO(pOSPrintDisplayGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting pOSPrintDisplayGroupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrintDisplayGroupDTO);
            return pOSPrintDisplayGroupDTO;
        }

        /// <summary>
        /// Updates the POSPrintDisplayGroup record
        /// </summary>
        /// <param name="pOSPrintDisplayGroupDTO">POSPrintDisplayGroupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public POSPrintDisplayGroupDTO UpdatePOSPrintDisplayGroup(POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrintDisplayGroupDTO, loginId, siteId);
            string updatePOSPrintDisplayGroupQuery = @"update POSPrintDisplayGroup 
                                                  set  POSPrinterId = @posPrinterId,
                                                  DisplayGroupId = @displayGroupId,
                                                  --site_id=@siteId,
                                                  MasterEntityId = @masterEntityId,
                                                  LastUpdateDate = GetDate(),
                                                  LastUpdatedBy = @LastUpdatedBy
                                                  WHERE Id = @id
                           SELECT * FROM POSPrintDisplayGroup WHERE Id = @id";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updatePOSPrintDisplayGroupQuery, GetSQLParameters(pOSPrintDisplayGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrintDisplayGroupDTO(pOSPrintDisplayGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating pOSPrintDisplayGroupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrintDisplayGroupDTO);
            return pOSPrintDisplayGroupDTO;
        }
        private void RefreshPOSPrintDisplayGroupDTO(POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSPrintDisplayGroupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow posPrintDisplayDataRow = dt.Rows[0];
                pOSPrintDisplayGroupDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                pOSPrintDisplayGroupDTO.LastUpdatedDate = posPrintDisplayDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posPrintDisplayDataRow["LastUpdateDate"]);
                pOSPrintDisplayGroupDTO.CreationDate = posPrintDisplayDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posPrintDisplayDataRow["CreationDate"]);
                pOSPrintDisplayGroupDTO.Guid = posPrintDisplayDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(posPrintDisplayDataRow["Guid"]);
                pOSPrintDisplayGroupDTO.LastUpdatedBy = posPrintDisplayDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(posPrintDisplayDataRow["LastUpdatedBy"]);
                pOSPrintDisplayGroupDTO.CreatedBy = posPrintDisplayDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(posPrintDisplayDataRow["CreatedBy"]);
                pOSPrintDisplayGroupDTO.SiteId = posPrintDisplayDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(posPrintDisplayDataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to POSPrintDisplayGroupDTO class type
        /// </summary>
        /// <param name="posPrintDisplayDataRow">POSPrintDisplayGroup posPrintDisplayDataRow</param>
        /// <returns>Returns POSPrintDisplayGroup</returns>
        private POSPrintDisplayGroupDTO GetPOSPrintDisplayGroupDTO(DataRow posPrintDisplayDataRow)
        {
            log.LogMethodEntry(posPrintDisplayDataRow);
            POSPrintDisplayGroupDTO posPrintDisplayGroupDataObject = new POSPrintDisplayGroupDTO(
                                            posPrintDisplayDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(posPrintDisplayDataRow["Id"]),
                                            posPrintDisplayDataRow["POSPrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(posPrintDisplayDataRow["POSPrinterId"]),
                                            posPrintDisplayDataRow["DisplayGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(posPrintDisplayDataRow["DisplayGroupId"]),
                                            posPrintDisplayDataRow["Guid"].ToString(),
                                            posPrintDisplayDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(posPrintDisplayDataRow["site_id"]),
                                            posPrintDisplayDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(posPrintDisplayDataRow["SynchStatus"]),
                                            posPrintDisplayDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(posPrintDisplayDataRow["MasterEntityId"]),
                                            posPrintDisplayDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posPrintDisplayDataRow["CreationDate"]),
                                            posPrintDisplayDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(posPrintDisplayDataRow["CreatedBy"]),
                                            posPrintDisplayDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(posPrintDisplayDataRow["LastUpdateDate"]),
                                            posPrintDisplayDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(posPrintDisplayDataRow["LastUpdatedBy"])
                                            );
            log.LogMethodExit(posPrintDisplayGroupDataObject);
            return posPrintDisplayGroupDataObject;
        }


        /// <summary>
        /// Gets the POSPrintDisplayGroup data of passed Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns POSPrintDisplayGroupDTO</returns>
        public POSPrintDisplayGroupDTO GetPOSPrintDisplayGroup(int id)
        {
            log.LogMethodEntry(id);
            POSPrintDisplayGroupDTO pOSPrintDisplayGroupDataObject = null;
            string selectPOSPrintDisplayGroupQuery = @"SELECT *
                                                    FROM POSPrintDisplayGroup
                                                    WHERE Id = @id";
            SqlParameter[] selectPOSPrintDisplayGrouParameters = new SqlParameter[1];
            selectPOSPrintDisplayGrouParameters[0] = new SqlParameter("@id", id);
            DataTable pOSPrintDisplayGroup = dataAccessHandler.executeSelectQuery(selectPOSPrintDisplayGroupQuery, selectPOSPrintDisplayGrouParameters);

            if (pOSPrintDisplayGroup.Rows.Count > 0)
            {
                DataRow pOSPrintDisplayGroupRow = pOSPrintDisplayGroup.Rows[0];
                pOSPrintDisplayGroupDataObject = GetPOSPrintDisplayGroupDTO(pOSPrintDisplayGroupRow);
            }
            log.LogMethodExit(pOSPrintDisplayGroupDataObject);
            return pOSPrintDisplayGroupDataObject;
        }

        /// <summary>
        /// Gets the POSPrintDisplayGroupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of POSPrintDisplayGroupDTO matching the search criteria</returns>
        public List<POSPrintDisplayGroupDTO> GetPOSPrintDisplayGroupList(List<KeyValuePair<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectPosPrintDisplayGroupQuery = @"select * from POSPrintDisplayGroup ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? " " : " and ";

                        if (searchParameter.Key == POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.ID ||
                            searchParameter.Key == POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.POS_PRINTER_ID ||
                            searchParameter.Key == POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.DISPLAYGROUP_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectPosPrintDisplayGroupQuery = selectPosPrintDisplayGroupQuery + query;
            }
            DataTable PosPrintDisplayGroupData = dataAccessHandler.executeSelectQuery(selectPosPrintDisplayGroupQuery, parameters.ToArray(),sqlTransaction);

            if (PosPrintDisplayGroupData.Rows.Count > 0)
            {
                List<POSPrintDisplayGroupDTO> PosPrintDisplayGroupList = new List<POSPrintDisplayGroupDTO>();
                foreach (DataRow PosPrintDisplayGroupDataRow in PosPrintDisplayGroupData.Rows)
                {
                    POSPrintDisplayGroupDTO PosPrintDisplayGroupDataObject = GetPOSPrintDisplayGroupDTO(PosPrintDisplayGroupDataRow);
                    PosPrintDisplayGroupList.Add(PosPrintDisplayGroupDataObject);
                }
                log.LogMethodExit(PosPrintDisplayGroupList);
                return PosPrintDisplayGroupList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
    }
}
