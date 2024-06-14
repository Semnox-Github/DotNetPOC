/********************************************************************************************
 * Project Name -AlohaTenderIdMaps DataHandler
 * Description  -Data object of AlohaTenderIdMapsDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-May-2017   Amaresh          Created 
 *2.70.2        24-Jul-2019   Deeksha          Modifications as per 3 tier standard.
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
    /// AlohaTenderIdMapsDataHandler - Handles insert, update and select of AlohaTenderIdMaps objects
    /// </summary>
    public class AlohaTenderIdMapsDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM AlohaTenderIdMaps AS a ";

        /// <summary>
        /// Dictionary for searching Parameters for the AlohaTenderIdMaps object.
        /// </summary>
        private static readonly Dictionary<AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters, string> DBSearchParameters = new Dictionary<AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters, string>
            {
                {AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.ALOHA_MAP_ID, "a.AlohaMapId"},
                {AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.NAME, "a.Name"},
                {AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.IS_ACTIVE, "a.IsActive"},
                {AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.SITE_ID, "a.site_id"},
                {AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.MASTER_ENTITY_ID, "a.MasterEntityId"}
            };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AlohaTenderIdMapsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AlohaTenderIdMapsDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AlohaTenderIdMaps Record.
        /// </summary>
        /// <param name="alohaTenderIdMapsDTO">AlohaTenderIdMapsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AlohaTenderIdMapsDTO alohaTenderIdMapsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaTenderIdMapsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AlohaMapId", alohaTenderIdMapsDTO.AlohaMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", alohaTenderIdMapsDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", alohaTenderIdMapsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", alohaTenderIdMapsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the AlohaTenderIdCardMapping record to the database
        /// </summary>
        /// <param name="alohaTenderIdCardMappingDTO">AlohaTenderIdCardMappingDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public AlohaTenderIdMapsDTO InsertAlohaTenderIdMaps(AlohaTenderIdMapsDTO alohaTenderIdMapsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaTenderIdMapsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AlohaTenderIdMaps]
                                                        (                                                         
                                                         Name,
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
                                                         @Name,
                                                         @IsActive,
                                                         @CreatedBy,
                                                         GETDATE(),
                                                         GETDATE(),
                                                         @LastUpdatedUser,
                                                         NEWID(),
                                                         @site_id,
                                                         @MasterEntityId
                                                        ) SELECT * FROM AlohaTenderIdMaps WHERE AlohaMapId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(alohaTenderIdMapsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAlohaTenderIdMapsDTO(alohaTenderIdMapsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting alohaTenderIdMapsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(alohaTenderIdMapsDTO);
            return alohaTenderIdMapsDTO;
        }



        /// <summary>
        /// Updates the AlohaTenderIdCardMapping record
        /// </summary>
        /// <param name="alohaTenderIdMapsDTO">AlohaTenderIdMapsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AlohaTenderIdMapsDTO UpdateAlohaTenderIdMaps(AlohaTenderIdMapsDTO alohaTenderIdMapsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaTenderIdMapsDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AlohaTenderIdMaps]
                                    SET 
                                             Name = @Name,
                                             IsActive = @IsActive,
                                             CreatedBy = @CreatedBy,                                      
                                             LastUpdatedDate = GETDATE(),
                                             LastUpdatedUser = @LastUpdatedUser
                                             
                                       WHERE AlohaMapId =@AlohaMapId 
                                    SELECT * FROM AlohaTenderIdMaps WHERE AlohaMapId = @AlohaMapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(alohaTenderIdMapsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAlohaTenderIdMapsDTO(alohaTenderIdMapsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating alohaTenderIdMapsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(alohaTenderIdMapsDTO);
            return alohaTenderIdMapsDTO;
        }

        /// <summary>
        /// Delete the record from the AlohaTenderIdMaps database based on AlohaMapId
        /// </summary>
        /// <param name="alohaMapId">alohaMapId</param>
        /// <returns>return the int </returns>
        internal int Delete(int alohaMapId)
        {
            log.LogMethodEntry(alohaMapId);
            string query = @"DELETE  
                             FROM AlohaTenderIdMaps
                             WHERE AlohaTenderIdMaps.AlohaMapId = @AlohaMapId";
            SqlParameter parameter = new SqlParameter("@AlohaMapId", alohaMapId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="alohaTenderIdMapsDTO">AlohaTenderIdMapsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAlohaTenderIdMapsDTO(AlohaTenderIdMapsDTO alohaTenderIdMapsDTO, DataTable dt)
        {
            log.LogMethodEntry(alohaTenderIdMapsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                alohaTenderIdMapsDTO.AlohaMapId = Convert.ToInt32(dt.Rows[0]["AlohaMapId"]);
                alohaTenderIdMapsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                alohaTenderIdMapsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                alohaTenderIdMapsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                alohaTenderIdMapsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                alohaTenderIdMapsDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                alohaTenderIdMapsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AlohaTenderIdMapsDTO class type
        /// </summary>
        /// <param name="alohaTenderIdMapsDataRow">AlohaTenderIdMaps DataRow</param>
        /// <returns>Returns AlohaTenderIdMaps</returns>
        private AlohaTenderIdMapsDTO GetAlohaTenderIdMapsDTO(DataRow alohaTenderIdMapsDataRow)
        {
            log.LogMethodEntry(alohaTenderIdMapsDataRow);
            AlohaTenderIdMapsDTO alohaTenderIdMapsDataObject = new AlohaTenderIdMapsDTO(
                                            Convert.ToInt32(alohaTenderIdMapsDataRow["AlohaMapId"]),
                                            alohaTenderIdMapsDataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdMapsDataRow["Name"]),
                                            alohaTenderIdMapsDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(alohaTenderIdMapsDataRow["IsActive"]),
                                            alohaTenderIdMapsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdMapsDataRow["CreatedBy"]),
                                            alohaTenderIdMapsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(alohaTenderIdMapsDataRow["CreationDate"]),
                                            alohaTenderIdMapsDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(alohaTenderIdMapsDataRow["LastUpdatedDate"]),
                                            alohaTenderIdMapsDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdMapsDataRow["LastUpdatedUser"]),
                                            alohaTenderIdMapsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdMapsDataRow["Guid"]),
                                            alohaTenderIdMapsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdMapsDataRow["site_id"]),
                                            alohaTenderIdMapsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(alohaTenderIdMapsDataRow["SynchStatus"]),
                                            alohaTenderIdMapsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdMapsDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(alohaTenderIdMapsDataObject);
            return alohaTenderIdMapsDataObject;
        }

        /// <summary>
        /// Gets the AlohaTenderIdMaps data of passed id 
        /// </summary>
        /// <param name="alohaMapId">id of AlohaTenderIdMapsID is passed as parameter</param>
        /// <returns>Returns AlohaTenderIdMaps</returns>
        public AlohaTenderIdMapsDTO GetAlohaTenderIdMaps(int alohaMapId)
        {
            log.LogMethodEntry(alohaMapId);
            AlohaTenderIdMapsDTO result = null;
            string query = SELECT_QUERY + @" WHERE a.AlohaMapId= @AlohaMapId";
            SqlParameter parameter = new SqlParameter("@AlohaMapId", alohaMapId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAlohaTenderIdMapsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the AlohaTenderIdMapsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AlohaTenderIdMapsDTO matching the search criteria</returns>
        public List<AlohaTenderIdMapsDTO> GetAlohaTenderIdMapsList(List<KeyValuePair<AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AlohaTenderIdMapsDTO> alohaTenderIdMapsList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectAlohaTenderIdMapsQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        
                        if (searchParameter.Key == AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.ALOHA_MAP_ID ||
                            searchParameter.Key == AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

                        }
                        else if (searchParameter.Key == AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == AlohaTenderIdMapsDTO.SearchByAlohaTenderIdMapsParameters.SITE_ID)
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
                    selectAlohaTenderIdMapsQuery = selectAlohaTenderIdMapsQuery + query;
            }
            DataTable alohaTenderIdMapsData = dataAccessHandler.executeSelectQuery(selectAlohaTenderIdMapsQuery, parameters.ToArray(), sqlTransaction);

            if (alohaTenderIdMapsData.Rows.Count > 0)
            {
                alohaTenderIdMapsList = new List<AlohaTenderIdMapsDTO>();
                foreach (DataRow alohaTenderIdMapsDataRow in alohaTenderIdMapsData.Rows)
                {
                    AlohaTenderIdMapsDTO alohaTenderIdMapsDataObject = GetAlohaTenderIdMapsDTO(alohaTenderIdMapsDataRow);
                    alohaTenderIdMapsList.Add(alohaTenderIdMapsDataObject);
                }             
            }
            log.LogMethodExit(alohaTenderIdMapsList);
            return alohaTenderIdMapsList;
        }
    }
}
