/********************************************************************************************
 * Project Name - Communication
 * Description  - Data Handler object -MessagingClientDataHandler
 *
 **************
 ** Version Log
  **************
  * Version     Date             Modified By             Remarks
 *********************************************************************************************
 *2.80          24-Jun-2020     Jinto Thomas             Created
 *2.100.0       22-Aug-2020     Vikas Dwivedi            Modified as per 3-Tier Standard CheckList
 *2.100.0       22-Aug-2020     Girish Kundar            Modified :Issue FIx  SMTP port number update as NULL when it is -1
 *2.100.0       15-Sep-2020     Nitin Pai                Encrypting and Decrypting the password
 *2.120.6       20-Feb-2022     Nitin Pai                SendGrid Email Change - From Email and Domain Name
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// MessagingClientDataHandler object for Insert, Update and Search for MessagingClient Object
    /// </summary>
    public class MessagingClientDataHandler
    {
        private static readonly logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagingClient AS mc ";

        /// <summary>
        /// Dictionary for searching Parameters for the MessagingClient object.
        /// </summary>

        private static readonly Dictionary<MessagingClientDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MessagingClientDTO.SearchByParameters, string>
        {
            {MessagingClientDTO.SearchByParameters.CLIENT_ID,"mc.ClientId"},
            {MessagingClientDTO.SearchByParameters.CLIENT_ID_LIST,"mc.ClientId"},
            {MessagingClientDTO.SearchByParameters.CLIENT_NAME,"mc.ClientName"},
            {MessagingClientDTO.SearchByParameters.MESSAGING_CHANNEL_CODE,"mc.MessagingChannelCode"},
            {MessagingClientDTO.SearchByParameters.MESSAGING_SUB_CHANNEL_TYPE,"mc.MessagingSubChannelType"},
            {MessagingClientDTO.SearchByParameters.SITE_ID,"mc.site_id"},
            {MessagingClientDTO.SearchByParameters.IS_ACTIVE,"mc.IsActive"},
            {MessagingClientDTO.SearchByParameters.MASTER_ENTITY_ID,"mc.MasterEntityId"},
        };

        /// <summary>
        /// Parameterized Constructor for MessagingClientDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public MessagingClientDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MessagingClient Record.
        /// </summary>
        /// <param name="messagingClientDTO">MessagingClientDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>SQL parameters</returns>

        private List<SqlParameter> GetSQLParameters(MessagingClientDTO messagingClientDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingClientDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ClientId", messagingClientDTO.ClientId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ClientName", messagingClientDTO.ClientName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessagingChannelCode", messagingClientDTO.MessagingChannelCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessagingSubChannelType", messagingClientDTO.MessagingSubChannelType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sender", messagingClientDTO.Sender));
            parameters.Add(dataAccessHandler.GetSQLParameter("@HostUrl", messagingClientDTO.HostUrl));
            if (messagingClientDTO.SmtpPort == -1)
            {
                parameters.Add(new SqlParameter("@SmtpPort", DBNull.Value));
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@SmtpPort", messagingClientDTO.SmtpPort));
            }
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SmtpPort", messagingClientDTO.SmtpPort));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserName", messagingClientDTO.UserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Password", Encryption.Encrypt(messagingClientDTO.Password)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EnableSsl", messagingClientDTO.EnableSsl));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", messagingClientDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", messagingClientDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", messagingClientDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromEmail", messagingClientDTO.FromEmail));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Domain", messagingClientDTO.Domain));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MessagingClientDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>MessagingClientDTO</returns>
        private MessagingClientDTO GetMessagingClientDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MessagingClientDTO messagingClientDTO = new MessagingClientDTO(dataRow["ClientId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ClientId"]),
                                                         dataRow["ClientName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ClientName"]),
                                                         dataRow["MessagingChannelCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MessagingChannelCode"]),// To be checked for default message Type
                                                         dataRow["Sender"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Sender"]),
                                                         dataRow["HostUrl"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["HostUrl"]),
                                                         dataRow["SmtpPort"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SmtpPort"]),
                                                         dataRow["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UserName"]),
                                                         dataRow["Password"] == DBNull.Value ? string.Empty : Encryption.Decrypt(Convert.ToString(dataRow["Password"])),
                                                         dataRow["EnableSsl"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["EnableSsl"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["MessagingSubChannelType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MessagingSubChannelType"]),
                                                         dataRow["FromEmail"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FromEmail"]),
                                                         dataRow["Domain"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Domain"])
                                                        );
            log.LogMethodExit(messagingClientDTO);
            return messagingClientDTO;
        }

        /// <summary>
        /// Gets the MessagingClientDTO data of passed ClientId 
        /// </summary>
        /// <param name="clientId">ClientId -MessagingClientId </param>
        /// <returns>Returns MessagingClientDTO</returns>
        public MessagingClientDTO GetMessagingClient(int clientId)
        {
            log.LogMethodEntry(clientId);
            MessagingClientDTO result = null;
            string query = SELECT_QUERY + @" WHERE mc.ClientId = @ClientId";
            SqlParameter parameter = new SqlParameter("@ClientId", clientId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMessagingClientDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the MessagingClient Table.
        /// </summary>
        /// <param name="messagingClientDTO">MessagingClientDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>MessagingClientDTO</returns>
        public MessagingClientDTO Insert(MessagingClientDTO messagingClientDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingClientDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[MessagingClient]
                           ([ClientName],
                            [MessagingChannelCode],
                            [MessagingSubChannelType],
                            [Sender],
                            [HostUrl],
                            [SmtpPort],
                            [UserName],
                            [Password],
                            [EnableSsl],
                            [SynchStatus],
                            [Guid],
                            [site_id],
                            [IsActive],
                            [MasterEntityId],
                            [CreatedBy],
                            [CreationDate],
                            [LastUpdatedBy],
                            [LastUpdateDate],
                            [FromEmail],
                            [Domain])
                     VALUES
                           (@ClientName,
                            @MessagingChannelCode,
                            @MessagingSubChannelType,
                            @Sender,
                            @HostUrl,
                            @SmtpPort,
                            @UserName,
                            @Password,
                            @EnableSsl,
                            @SynchStatus,
                            NEWID(),
                            @SiteId,
                            @IsActive,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @FromEmail,
                            @Domain)
                            SELECT * FROM MessagingClient WHERE ClientId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingClientDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingClientDTO(messagingClientDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MessagingClientDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingClientDTO);
            return messagingClientDTO;
        }

        /// <summary>
        ///  Updates the record to the MessagingClient Table.
        /// </summary>
        /// <param name="messagingClientDTO">MessagingClientDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>MessagingClientDTO</returns>
        public MessagingClientDTO Update(MessagingClientDTO messagingClientDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingClientDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[MessagingClient]
                           SET
                            [ClientName]           = @ClientName,
                            [MessagingChannelCode]  = @MessagingChannelCode,
                            [MessagingSubChannelType]  = @MessagingSubChannelType,
                            [Sender]               = @Sender,
                            [HostUrl]              = @HostUrl,
                            [SmtpPort]             = @SmtpPort,
                            [UserName]             = @UserName,
                            [Password]             = @Password,
                            [EnableSsl]            = @EnableSsl,
                            [MasterEntityId]       = @MasterEntityId,
                            [LastUpdatedBy]        = @LastUpdatedBy,
                            [IsActive]             = @IsActive,
                            [LastUpdateDate]       = GETDATE(),
                            [FromEmail]            = @FromEmail,
                            [Domain]               = @Domain
                            WHERE ClientId = @ClientId;
                            SELECT * FROM MessagingClient WHERE ClientId = @ClientId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingClientDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingClientDTO(messagingClientDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating MessagingClientDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingClientDTO);
            return messagingClientDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="messagingClientDTO">MessagingClientDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        private void RefreshMessagingClientDTO(MessagingClientDTO messagingClientDTO, DataTable dt)
        {
            log.LogMethodEntry(messagingClientDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                messagingClientDTO.ClientId = Convert.ToInt32(dt.Rows[0]["ClientId"]);
                messagingClientDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                messagingClientDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                messagingClientDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                messagingClientDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                messagingClientDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                messagingClientDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MessagingClientDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">Search Parameters</param>
        /// <returns>Returns the List of MessagingClientDTO</returns>
        public List<MessagingClientDTO> GetMessagingClientDTOList(List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MessagingClientDTO> messagingClientDTOList = new List<MessagingClientDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MessagingClientDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MessagingClientDTO.SearchByParameters.CLIENT_ID
                            || searchParameter.Key == MessagingClientDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingClientDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingClientDTO.SearchByParameters.MESSAGING_CHANNEL_CODE
                            || searchParameter.Key == MessagingClientDTO.SearchByParameters.CLIENT_NAME
                            || searchParameter.Key == MessagingClientDTO.SearchByParameters.MESSAGING_SUB_CHANNEL_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingClientDTO.SearchByParameters.IS_ACTIVE) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(MessagingClientDTO.SearchByParameters.CLIENT_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, (string.IsNullOrWhiteSpace(searchParameter.Value) ? "-1" : searchParameter.Value)) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, (string.IsNullOrWhiteSpace(searchParameter.Value) ? "-1" : searchParameter.Value)));
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
                    MessagingClientDTO messagingClientDTO = GetMessagingClientDTO(dataRow);
                    messagingClientDTOList.Add(messagingClientDTO);
                }
            }
            log.LogMethodExit(messagingClientDTOList);
            return messagingClientDTOList;
        }
    }
}
