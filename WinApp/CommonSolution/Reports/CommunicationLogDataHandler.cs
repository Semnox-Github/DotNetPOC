/********************************************************************************************
 * Project Name - Reports
 * Description  - Data Handler of CommunicationLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      13-June-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// CommunicationLogDataHandler Data Handler - Handles insert, update and select of  CommunicationLog objects
    /// </summary>
    public class CommunicationLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CommunicationLog AS commLog";
        /// <summary>
        /// Dictionary for searching Parameters for the CommunicationLog object.
        /// </summary>
        private static readonly Dictionary<CommunicationLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CommunicationLogDTO.SearchByParameters, string>
        {
            { CommunicationLogDTO.SearchByParameters.MACHINE_ID,"commLog.MachineId"},
            { CommunicationLogDTO.SearchByParameters.MACHINE_ADDRESS,"commLog.MachineAddress"},
            { CommunicationLogDTO.SearchByParameters.MASTER_ADDRESS,"commLog.MasterAddress"},
            { CommunicationLogDTO.SearchByParameters.SITE_ID,"commLog.site_id"},
            { CommunicationLogDTO.SearchByParameters.MASTER_ENTITY_ID,"commLog.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for CommunicationLogDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public CommunicationLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CommunicationLog Record.
        /// </summary>
        /// <param name="communicationLogDTO">communicationLogDTO object as parameter</param>
        /// <param name="loginId">login id of user</param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CommunicationLogDTO communicationLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(communicationLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastSentData", communicationLogDTO.LastSentData));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineAddress", communicationLogDTO.MachineAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", communicationLogDTO.MachineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterAddress", communicationLogDTO.MasterAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceivedData", communicationLogDTO.ReceivedData));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", communicationLogDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", communicationLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to CommunicationLogDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the CommunicationLogDTO</returns>
        private CommunicationLogDTO GetCommunicationLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CommunicationLogDTO communicationLogDTO = new CommunicationLogDTO(dataRow["MasterAddress"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MasterAddress"]),
                                                         dataRow["MachineAddress"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MachineAddress"]),
                                                         dataRow["ReceivedData"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ReceivedData"]),
                                                         dataRow["LastSentData"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastSentData"]),
                                                         dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                                         dataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MachineId"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(communicationLogDTO);
            return communicationLogDTO;
        }

        /// <summary>
        /// Inserts the record to the communicationLogDTO Table.
        /// </summary>
        /// <param name="communicationLogDTO">communicationLogDTO object as parameter</param>
        /// <param name="loginId">login id of user</param>
        /// <param name="siteId">site id of user</param>
        /// <returns> Returns theCommunicationLogDTO</returns>
        public CommunicationLogDTO Insert(CommunicationLogDTO communicationLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(communicationLogDTO, loginId, siteId);
            int rowInserted = 0;
            string query = @"INSERT INTO [dbo].[CommunicationLog]
                           ([MasterAddress]
                           ,[MachineAddress]
                           ,[ReceivedData]
                           ,[LastSentData]
                           ,[Timestamp]
                           ,[Guid]
                           ,[site_id]
                           ,[MachineId]
                           ,[MasterEntityId]
                           ,[CreatedBy]
                           ,[CreationDate]
                           ,[LastUpdatedBy]
                           ,[LastUpdateDate])
                     VALUES
                           (@MasterAddress
                           ,@MachineAddress
                           ,@ReceivedData
                           ,@LastSentData
                           ,@Timestamp
                           ,NEWID()
                           ,@site_id
                           ,@MachineId
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE())";

            try
            {
                rowInserted = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(communicationLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                // RefreshCommunicationLogDTO(communicationLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CommunicationLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(communicationLogDTO);
            return communicationLogDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="communicationLogDTO">CommunicationLogDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        private void RefreshCommunicationLogDTO(CommunicationLogDTO communicationLogDTO, DataTable dt)
        {
            log.LogMethodEntry(communicationLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                communicationLogDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                communicationLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                communicationLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                communicationLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                communicationLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                communicationLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                communicationLogDTO.Timestamp = dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of CommunicationLogDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the list of CommunicationLogDTO</returns>
        public List<CommunicationLogDTO> GetCommunicationLogDTOList(List<KeyValuePair<CommunicationLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CommunicationLogDTO> communicationLogDTOList = new List<CommunicationLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CommunicationLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CommunicationLogDTO.SearchByParameters.MACHINE_ID
                            || searchParameter.Key == CommunicationLogDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CommunicationLogDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CommunicationLogDTO.SearchByParameters.MACHINE_ADDRESS
                            || searchParameter.Key == CommunicationLogDTO.SearchByParameters.MASTER_ADDRESS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CommunicationLogDTO communicationLogDTO = GetCommunicationLogDTO(dataRow);
                    communicationLogDTOList.Add(communicationLogDTO);
                }
            }
            log.LogMethodExit(communicationLogDTOList);
            return communicationLogDTOList;
        }

    }
}
