/********************************************************************************************
 * Project Name - AlohaPOSTenderIdMapping DataHandler
 * Description  - Data object of alohaPOSTenderIDMappingDH
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-May-2017   Amaresh          Created 
 *2.70.2        24-Jul-2019   Deeksha          Modifications as per 3 tier standard.
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// alohaPOSTenderIDMappingDH - Handles insert, update and select of alohaPOSTenderIDMapping objects
    /// </summary>
    public class AlohaPOSTenderIdMappingDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM AlohaPOSTenderIDMapping AS ap ";

        /// <summary>
        ///  Dictionary for searching Parameters for the AlohaPOSTenderIdMapping object.
        /// </summary>
        private static readonly Dictionary<AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string> DBSearchParameters = new Dictionary<AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>
            {
                {AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.ALOHA_POS_MAP_ID, "ap.AlohaPOSMapId"},
                {AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.ALOHA_MAP_ID, "ap.AlohaMapId"},
                {AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.POS_MACHINE_ID, "ap.POSMachineId"},
                {AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.IS_ACTIVE, "ap.IsActive"},
                {AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.SITE_ID, "ap.site_id"},
                {AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.MASTER_ENTITY_ID, "ap.MasterEntityId"}
            };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AlohaPOSTenderIDMappingDH class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AlohaPOSTenderIdMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AlohaPOSTenderIdMapping Record.
        /// </summary>
        /// <param name="alohaPOSTenderIdMappingDTO">AlohaPOSTenderIdMappingDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaPOSTenderIdMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AlohaPOSMapId", alohaPOSTenderIdMappingDTO.AlohaPOSMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AlohaMapId", alohaPOSTenderIdMappingDTO.AlohaMapId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", alohaPOSTenderIdMappingDTO.POSMachineId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", alohaPOSTenderIdMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", alohaPOSTenderIdMappingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the AlohaPOSTenderIdMapping record to the database
        /// </summary>
        /// <param name="alohaPOSTenderIdMappingDTO">AlohaPOSTenderIdMappingDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public AlohaPOSTenderIdMappingDTO InsertAlohaPOSTenderIdMapping(AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaPOSTenderIdMappingDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AlohaPOSTenderIDMapping]
                                                        (    
                                                          AlohaMapId,
                                                          POSMachineId,
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
                                                          @AlohaMapId,
                                                          @POSMachineId,
                                                          @IsActive,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          NewId(),
                                                          @site_id,                                                       
                                                          @MasterEntityId)
                                                       SELECT * FROM AlohaPOSTenderIDMapping WHERE AlohaPOSMapId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(alohaPOSTenderIdMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAlohaPOSTenderIdMappingDTO(alohaPOSTenderIdMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting alohaPOSTenderIdMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(alohaPOSTenderIdMappingDTO);
            return alohaPOSTenderIdMappingDTO;
        }

        /// <summary>
        /// Updates the AlohaPOSTenderIdMapping record
        /// </summary>
        /// <param name="alohaPOSTenderIdMappingDTO">AlohaPOSTenderIdMappingDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AlohaPOSTenderIdMappingDTO UpdateAlohaPOSTenderIdMapping(AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaPOSTenderIdMappingDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AlohaPOSTenderIDMapping]
                                    SET 
                                             AlohaMapId = @AlohaMapId,
                                             POSMachineId = @POSMachineId,
                                             IsActive = @IsActive,
                                             -- site_id = @site_id,
                                             MasterEntityId = @MasterEntityId,
                                             LastUpdatedDate = GETDATE(),
                                             LastUpdatedUser = @LastUpdatedUser
                                             
                                       WHERE AlohaPOSMapId = @AlohaPOSMapId 
                                    SELECT * FROM AlohaPOSTenderIDMapping WHERE AlohaPOSMapId = @AlohaPOSMapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(alohaPOSTenderIdMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAlohaPOSTenderIdMappingDTO(alohaPOSTenderIdMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating alohaPOSTenderIdMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(alohaPOSTenderIdMappingDTO);
            return alohaPOSTenderIdMappingDTO;
        }


        /// <summary>
        /// Delete the record from the AlohaPOSTenderIdMapping database based on AlohaPOSMapId
        /// </summary>
        /// <param name="alohaPOSMapId">alohaPOSMapId</param>
        /// <returns>return the int </returns>
        internal int Delete(int alohaPOSMapId)
        {
            log.LogMethodEntry(alohaPOSMapId);
            string query = @"DELETE  
                             FROM AlohaPOSTenderIDMapping
                             WHERE AlohaPOSTenderIDMapping.AlohaPOSMapId = @AlohaPOSMapId";
            SqlParameter parameter = new SqlParameter("@AlohaPOSMapId", alohaPOSMapId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="alohaPOSTenderIdMappingDTO">AlohaPOSTenderIdMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAlohaPOSTenderIdMappingDTO(AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(alohaPOSTenderIdMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                alohaPOSTenderIdMappingDTO.AlohaPOSMapId = Convert.ToInt32(dt.Rows[0]["AlohaPOSMapId"]);
                alohaPOSTenderIdMappingDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                alohaPOSTenderIdMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                alohaPOSTenderIdMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                alohaPOSTenderIdMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                alohaPOSTenderIdMappingDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                alohaPOSTenderIdMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AlohaPOSTenderIdMappingDTO class type
        /// </summary>
        /// <param name="alohaPOSTenderIdMappingDataRow">alohaPOSTenderIDMapping DataRow</param>
        /// <returns>Returns alohaPOSTenderIDMapping</returns>
        private AlohaPOSTenderIdMappingDTO GetAlohaPOSTenderIdMappingDTO(DataRow alohaPOSTenderIdMappingDataRow)
        {
            log.LogMethodEntry(alohaPOSTenderIdMappingDataRow);
            AlohaPOSTenderIdMappingDTO alohaPOSTenderIdMappingDataObject = new AlohaPOSTenderIdMappingDTO(
                                            Convert.ToInt32(alohaPOSTenderIdMappingDataRow["AlohaPOSMapId"]),
                                            alohaPOSTenderIdMappingDataRow["AlohaMapId"]== DBNull.Value ? -1 : Convert.ToInt32(alohaPOSTenderIdMappingDataRow["AlohaMapId"]),
                                            alohaPOSTenderIdMappingDataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(alohaPOSTenderIdMappingDataRow["POSMachineId"]),
                                            alohaPOSTenderIdMappingDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(alohaPOSTenderIdMappingDataRow["IsActive"]),
                                            alohaPOSTenderIdMappingDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(alohaPOSTenderIdMappingDataRow["CreatedBy"]),
                                            alohaPOSTenderIdMappingDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(alohaPOSTenderIdMappingDataRow["CreationDate"]),
                                            alohaPOSTenderIdMappingDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(alohaPOSTenderIdMappingDataRow["LastUpdatedDate"]),
                                            alohaPOSTenderIdMappingDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(alohaPOSTenderIdMappingDataRow["LastUpdatedUser"]),
                                            alohaPOSTenderIdMappingDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(alohaPOSTenderIdMappingDataRow["Guid"]),
                                            alohaPOSTenderIdMappingDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(alohaPOSTenderIdMappingDataRow["site_id"]),
                                            alohaPOSTenderIdMappingDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(alohaPOSTenderIdMappingDataRow["SynchStatus"]),
                                            alohaPOSTenderIdMappingDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(alohaPOSTenderIdMappingDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(alohaPOSTenderIdMappingDataObject);
            return alohaPOSTenderIdMappingDataObject;
        }

        /// <summary>
        /// Gets the AlohaTenderIdCardMapping data of passed Id
        /// </summary>
        /// <param name="alohaPOSMapId">Int type parameter</param>
        /// <returns>Returns AlohaPOSTenderIdMappingDTO</returns>
        public AlohaPOSTenderIdMappingDTO GetAlohaPOSTenderIdMapping(int alohaPOSMapId)
        {
            log.LogMethodEntry(alohaPOSMapId);
            AlohaPOSTenderIdMappingDTO result = null;
            string query = SELECT_QUERY + @" WHERE ap.AlohaPOSMapId= @AlohaPOSMapId";
            SqlParameter parameter = new SqlParameter("@AlohaPOSMapId", alohaPOSMapId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAlohaPOSTenderIdMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the AlohaPOSTenderIdMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AlohaPOSTenderIdMappingDTO matching the search criteria</returns>
        public List<AlohaPOSTenderIdMappingDTO> GetAlohaPOSTenderIdMappingList(List<KeyValuePair<AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AlohaPOSTenderIdMappingDTO> alohaPOSTenderIDMappingList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectalohaPOSTenderIDMappingQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        if (searchParameter.Key == AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.ALOHA_POS_MAP_ID ||
                            searchParameter.Key == AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.ALOHA_MAP_ID ||
                            searchParameter.Key == AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.POS_MACHINE_ID ||
                            searchParameter.Key == AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.MASTER_ENTITY_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

                        }
                        else if (searchParameter.Key == AlohaPOSTenderIdMappingDTO.SearchByAlohaPOSTenderIdMappingParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    selectalohaPOSTenderIDMappingQuery = selectalohaPOSTenderIDMappingQuery + query;
            }
            DataTable alohaPOSTenderIDMappingData = dataAccessHandler.executeSelectQuery(selectalohaPOSTenderIDMappingQuery, parameters.ToArray(), sqlTransaction);
            if (alohaPOSTenderIDMappingData.Rows.Count > 0)
            {
                alohaPOSTenderIDMappingList = new List<AlohaPOSTenderIdMappingDTO>();
                foreach (DataRow alohaPOSTenderIDMappingDataRow in alohaPOSTenderIDMappingData.Rows)
                {
                    AlohaPOSTenderIdMappingDTO alohaPOSTenderIDMappingDataObject = GetAlohaPOSTenderIdMappingDTO(alohaPOSTenderIDMappingDataRow);
                    alohaPOSTenderIDMappingList.Add(alohaPOSTenderIDMappingDataObject);
                }
            }
            log.LogMethodExit(alohaPOSTenderIDMappingList);
            return alohaPOSTenderIDMappingList;
        }
    }
}
