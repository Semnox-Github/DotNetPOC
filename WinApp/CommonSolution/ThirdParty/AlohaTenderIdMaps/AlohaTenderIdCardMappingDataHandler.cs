/********************************************************************************************
 * Project Name - AlohaTenderIDCardMapping DataHandler
 * Description  - Data object of AlohaTenderIDCardMappingDH
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
    /// AlohaTenderIDCardMappingDH - Handles insert, update and select of AlohaTenderIDCardMapping objects
    /// </summary>
    public class AlohaTenderIdCardMappingDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM AlohaTenderIDCardMapping AS cp ";

        /// <summary>
        ///  Dictionary for searching Parameters for the AlohaTenderIdCardMapping object.
        /// </summary>
        private static readonly Dictionary<AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string> DBSearchParameters = new Dictionary<AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>
            {
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.ID, "cp.Id"},
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.ALOHA_MAP_ID, "cp.AlohaMapId"},
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.CARD_TYPE_ID, "cp.CardTypeId"},
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.TENDER_ID, "cp.TenderId"},
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.IS_ACTIVE, "cp.IsActive"},
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.SITE_ID, "cp.site_id"},
                {AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.MASTER_ENTITY_ID, "cp.MasterEntityId"}
            };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of AlohaTenderIDCardMappingDH class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AlohaTenderIdCardMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AlohaTenderIdCardMapping Record.
        /// </summary>
        /// <param name="alohaTenderIdCardMappingDTO">AlohaTenderIdCardMappingDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(AlohaTenderIdCardMappingDTO alohaTenderIdCardMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaTenderIdCardMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", alohaTenderIdCardMappingDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AlohaMapId", alohaTenderIdCardMappingDTO.AlohaMapId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", alohaTenderIdCardMappingDTO.CardTypeId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TenderId", alohaTenderIdCardMappingDTO.TenderId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", alohaTenderIdCardMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", alohaTenderIdCardMappingDTO.MasterEntityId, true));
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
        public AlohaTenderIdCardMappingDTO InsertAlohaTenderIdCardMapping(AlohaTenderIdCardMappingDTO alohaTenderIdCardMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaTenderIdCardMappingDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AlohaTenderIDCardMapping]
                                                        (                                                         
                                                         AlohaMapId,
                                                         CardTypeId,
                                                         TenderId,
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
                                                         @CardTypeId,
                                                         @TenderId,
                                                         @IsActive,
                                                         @CreatedBy,
                                                         GETDATE(),
                                                         GETDATE(),
                                                         @LastUpdatedUser,
                                                         NEWID(),
                                                         @site_id,
                                                         @MasterEntityId
                                                        ) SELECT * FROM AlohaTenderIDCardMapping WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(alohaTenderIdCardMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAlohaTenderIdCardMapping(alohaTenderIdCardMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting alohaTenderIdCardMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(alohaTenderIdCardMappingDTO);
            return alohaTenderIdCardMappingDTO;
        }

        /// <summary>
        /// Updates the AlohaTenderIdCardMapping record
        /// </summary>
        /// <param name="AlohaTenderIdCardMappingDTO">AlohaTenderIdCardMappingDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AlohaTenderIdCardMappingDTO UpdateAlohaTenderIdCardMapping(AlohaTenderIdCardMappingDTO alohaTenderIdCardMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(alohaTenderIdCardMappingDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AlohaTenderIDCardMapping]
                                    SET 
                                             AlohaMapId = @AlohaMapId,
                                             CardTypeId = @CardTypeId,
                                             TenderId = @TenderId,
                                             IsActive = @IsActive,
                                             -- site_id = @site_id,
                                             MasterEntityId = @MasterEntityId,
                                             LastUpdatedDate = GETDATE(),
                                             LastUpdatedUser = @LastUpdatedUser
                                             
                                       WHERE Id =@Id 
                                    SELECT * FROM AlohaTenderIDCardMapping WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(alohaTenderIdCardMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAlohaTenderIdCardMapping(alohaTenderIdCardMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating alohaTenderIdCardMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(alohaTenderIdCardMappingDTO);
            return alohaTenderIdCardMappingDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="alohaTenderIdCardMappingDTO">AlohaTenderIdCardMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAlohaTenderIdCardMapping(AlohaTenderIdCardMappingDTO alohaTenderIdCardMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(alohaTenderIdCardMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                alohaTenderIdCardMappingDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                alohaTenderIdCardMappingDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                alohaTenderIdCardMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                alohaTenderIdCardMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                alohaTenderIdCardMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                alohaTenderIdCardMappingDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                alohaTenderIdCardMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AlohaTenderIDCardMappingDTO class type
        /// </summary>
        /// <param name="alohaTenderIdCardMappingDataRow">AlohaTenderIdCardMapping DataRow</param>
        /// <returns>Returns AlohaTenderIdCardMapping</returns>
        private AlohaTenderIdCardMappingDTO GetAlohaTenderIDCardMappingDTO(DataRow alohaTenderIdCardMappingDataRow)
        {
            log.LogMethodEntry(alohaTenderIdCardMappingDataRow);
            AlohaTenderIdCardMappingDTO alohaTenderIdCardMappingDataObject = new AlohaTenderIdCardMappingDTO(
                                            Convert.ToInt32(alohaTenderIdCardMappingDataRow["Id"]),
                                            alohaTenderIdCardMappingDataRow["AlohaMapId"]== DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdCardMappingDataRow["AlohaMapId"]),
                                            alohaTenderIdCardMappingDataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdCardMappingDataRow["CardTypeId"]),
                                            alohaTenderIdCardMappingDataRow["TenderId"] == DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdCardMappingDataRow["TenderId"]),
                                            alohaTenderIdCardMappingDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(alohaTenderIdCardMappingDataRow["IsActive"]),
                                            alohaTenderIdCardMappingDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdCardMappingDataRow["CreatedBy"]),
                                            alohaTenderIdCardMappingDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(alohaTenderIdCardMappingDataRow["CreationDate"]),
                                            alohaTenderIdCardMappingDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(alohaTenderIdCardMappingDataRow["LastUpdatedDate"]),
                                            alohaTenderIdCardMappingDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdCardMappingDataRow["LastUpdatedUser"]),
                                            alohaTenderIdCardMappingDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(alohaTenderIdCardMappingDataRow["Guid"]),
                                            alohaTenderIdCardMappingDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdCardMappingDataRow["site_id"]),
                                            alohaTenderIdCardMappingDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(alohaTenderIdCardMappingDataRow["SynchStatus"]),
                                            alohaTenderIdCardMappingDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(alohaTenderIdCardMappingDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(alohaTenderIdCardMappingDataObject);
            return alohaTenderIdCardMappingDataObject;
        }

        /// <summary>
        /// Gets the AlohaTenderIdCardMapping data of passed id 
        /// </summary>
        /// <param name="id">id of AlohaTenderIdCardMapping is passed as parameter</param>
        /// <returns>Returns AlohaTenderIdCardMapping</returns>
        public AlohaTenderIdCardMappingDTO GetAlohaTenderIDCardMapping(int id)
        {
            log.LogMethodEntry(id);
            AlohaTenderIdCardMappingDTO result = null;
            string query = SELECT_QUERY + @" WHERE cp.Id= @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAlohaTenderIDCardMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the AlohaTenderIDCardMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AlohaTenderIDCardMappingDTO matching the search criteria</returns>
        public List<AlohaTenderIdCardMappingDTO> GetAlohaTenderIdCardMappingList(List<KeyValuePair<AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AlohaTenderIdCardMappingDTO> alohaTenderIdCardMappingList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectAlohaTenderIdCardMappingQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.ID ||
                            searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.ALOHA_MAP_ID ||
                            searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.CARD_TYPE_ID ||
                            searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.TENDER_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));

                        }
                        else if (searchParameter.Key == AlohaTenderIdCardMappingDTO.SearchByAlohaTenderIdCardMappingParameters.SITE_ID)
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
                    selectAlohaTenderIdCardMappingQuery = selectAlohaTenderIdCardMappingQuery + query;
            }
            DataTable alohaTenderIdCardMappingData = dataAccessHandler.executeSelectQuery(selectAlohaTenderIdCardMappingQuery, parameters.ToArray(), sqlTransaction);

            if (alohaTenderIdCardMappingData.Rows.Count > 0)
            {
                alohaTenderIdCardMappingList = new List<AlohaTenderIdCardMappingDTO>();
                foreach (DataRow alohaTenderIdCardMappingDataRow in alohaTenderIdCardMappingData.Rows)
                {
                    AlohaTenderIdCardMappingDTO alohaTenderIdCardMappingDataObject = GetAlohaTenderIDCardMappingDTO(alohaTenderIdCardMappingDataRow);
                    alohaTenderIdCardMappingList.Add(alohaTenderIdCardMappingDataObject);
                }
            }
            log.LogMethodExit(alohaTenderIdCardMappingList);
            return alohaTenderIdCardMappingList;
        } 
    }
}
