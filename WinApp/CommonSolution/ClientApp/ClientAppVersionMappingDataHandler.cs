/********************************************************************************************
 * Project Name - ImagesController
 * Description  - API to return images
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ClientApp
{
    ////<summary>
    ////ClientAppVersionMappingDatahandler Class 
    ////</summary>
    public class ClientAppVersionMappingDataHandler
    {
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public ClientAppVersionMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        private static readonly Dictionary<ClientAppVersionMappingDTO.SearchParameters, string> DBSearchParameters = new Dictionary<ClientAppVersionMappingDTO.SearchParameters, string>
        {
                {ClientAppVersionMappingDTO.SearchParameters.APP_ID, "cm.AppId"},
                {ClientAppVersionMappingDTO.SearchParameters.SECURITY_TOKEN, "cm.SecurityCode"},
                {ClientAppVersionMappingDTO.SearchParameters.RELEASE_NUMBER, "cm.ReleaseNumber"},
                {ClientAppVersionMappingDTO.SearchParameters.SECURITY_TOKEN_PARTIAL, "RIGHT(cm.SecurityCode, 2)"},
        };


        private const string SELECT_QUERY = @"select ISNULL(cm.securitycode, c.SecurityToken) As SecurityToken, ISNULL(cm.GatewayURL, c.GatewayURL) As SecurityToken, cm.* from ClientGuestAppIdMapping cm, clients c
                                            where c.ClientId = cm.[ClientId ] ";


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerRelationship Record.
        /// </summary>
        /// <param name="ClientAppVersionMappingDTO">ClientAppVersionMappingDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ClientAppVersionMappingDTO ClientAppVersionMappingDTO, string loginId)
        {
            log.LogMethodEntry(ClientAppVersionMappingDTO, loginId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", ClientAppVersionMappingDTO.ClientAppVersionMappingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@appId", ClientAppVersionMappingDTO.AppId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@clientId", ClientAppVersionMappingDTO.ClientId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@securityToken", ClientAppVersionMappingDTO.TokenizationKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@releaseNumber", ClientAppVersionMappingDTO.ReleaseNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@securityCode", ClientAppVersionMappingDTO.SecurityCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", ClientAppVersionMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@apiKey", ClientAppVersionMappingDTO.ApiKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gatewayUrl", ClientAppVersionMappingDTO.GateWayURL));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ClientAppVersionMappingDTO  Items record to the database
        /// </summary>
        /// <param name="clientAppVersionMappingDTO">ClientAppVersionMappingDTO type object</param>
        /// <param name="clientAppVersionMappingId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        public ClientAppVersionMappingDTO InsertClientAppVersionMapping(ClientAppVersionMappingDTO clientAppVersionMappingDTO, string userId)
        {
            log.LogMethodEntry(clientAppVersionMappingDTO, userId);

            string insertClientQuery = @"insert into ClientGuestAppIdMapping
                                                            (  
                                                               APPID,
                                                               ClientId,
                                                               IsActive,
                                                               CreatedBy,
                                                               CreationDate,
                                                               LastUpdatedBy, 
                                                               LastUpdatedDate,
                                                               ReleaseNumber,
                                                               GatewayURL,
                                                               TokenizationKey,
                                                               APIKey,
                                                               SecurityCode
                                                        ) 
                                                values 
                                                        (
                                                             @appId
                                                               @clientId,
                                                               @IsActive,
                                                               @createdBy,
                                                               Getdate(),
                                                               @lastUpdatedBy, 
                                                               Getdate(),
                                                               @releaseNumber,
                                                               @gatewayUrl,
                                                               @securityToken,
                                                               @apiKey,
                                                               @securityCode
                                                             )SELECT FROM ClientGuestAppIdMapping WHERE Id=scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertClientQuery, GetSQLParameters(clientAppVersionMappingDTO, userId).ToArray(), sqlTransaction);
                RefreshClientAppVersionMappingDTO(clientAppVersionMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting clientAppVersionMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(clientAppVersionMappingDTO);
            return clientAppVersionMappingDTO;


        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ClientAppVersionMappingDTO">ClientAppVersionMappingDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshClientAppVersionMappingDTO(ClientAppVersionMappingDTO ClientAppVersionMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(ClientAppVersionMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ClientAppVersionMappingDTO.ClientAppVersionMappingId = Convert.ToInt32(dt.Rows[0]["Id"]);
                ClientAppVersionMappingDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                ClientAppVersionMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ClientAppVersionMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ClientAppVersionMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the ClientAppVersionMappingDTO  Items record to the database
        /// </summary>
        /// <param name="ClientAppVersionMappingDTO">ClientAppVersionMappingDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        /// 
        public ClientAppVersionMappingDTO UpdateClientAppVersionMapping(ClientAppVersionMappingDTO clientAppVersionMappingDTO, string userId)
        {
            log.LogMethodEntry(clientAppVersionMappingDTO, userId);
            string updateClientQuery = @"update ClientGuestAppIdMapping 
                                                          set 
                                                               APPID=@appId,
                                                               ClientId=@clientId,
                                                               IsActive=@IsActive,                                                               
                                                               LastUpdatedBy=@lastUpdatedBy, 
                                                               LastupdatedDate=GetDate(),
                                                               ReleaseNumber=@releaseNumber,
                                                               GatewayURL=@gatewayUrl,
                                                               TokenizationKey=@securityToken,
                                                               APIKey=@apiKey
                                                               SecurityCode=@securityCode
                                                               where  Id=@clientAppVersionMappingId
                                                              SELECT * FROM ClientGuestAppIdMapping WHERE Id = @clientAppVersionMappingId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateClientQuery, GetSQLParameters(clientAppVersionMappingDTO, userId).ToArray(), sqlTransaction);
                RefreshClientAppVersionMappingDTO(clientAppVersionMappingDTO, dt);
            }


            catch (Exception ex)
            {
                log.Error("Error occurred while Updating clientAppVersionMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(clientAppVersionMappingDTO);
            return clientAppVersionMappingDTO;
        }

        /// <summary>
        /// return the record from the database
        /// Convert the datarow to ClientAppVersionMappingDTO object
        /// </summary>
        /// <param name="ClientAppVersionMappingDTORow">ClientAppVersionMappingDTORow</param>
        /// <returns>return the ClientAppVersionMappingDTO object</returns>
        public ClientAppVersionMappingDTO GetClientAppVersionMappingDTO(DataRow ClientAppVersionMappingDTORow)
        {
            log.LogMethodEntry(ClientAppVersionMappingDTORow);

            ClientAppVersionMappingDTO ClientAppVersionMappingDTO = new ClientAppVersionMappingDTO(
                                                ClientAppVersionMappingDTORow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppVersionMappingDTORow["Id"]),
                                                ClientAppVersionMappingDTORow["APPID"].ToString(),
                                                ClientAppVersionMappingDTORow["ClientId "] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppVersionMappingDTORow["ClientId "]),
                                                ClientAppVersionMappingDTORow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(ClientAppVersionMappingDTORow["IsActive"]),
                                                ClientAppVersionMappingDTORow["CreatedBy"].ToString(),
                                                ClientAppVersionMappingDTORow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ClientAppVersionMappingDTORow["CreationDate"]),
                                                ClientAppVersionMappingDTORow["LastUpdatedBy"].ToString(),
                                                ClientAppVersionMappingDTORow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ClientAppVersionMappingDTORow["LastupdatedDate"]),
                                                ClientAppVersionMappingDTORow["ReleaseNumber"].ToString(),
                                                ClientAppVersionMappingDTORow["GatewayUrl"].ToString(),
                                                ClientAppVersionMappingDTORow["Tokenizationkey"].ToString(),
                                                ClientAppVersionMappingDTORow["APIKey"].ToString(),
                                                ClientAppVersionMappingDTORow["SecurityCode"].ToString(),
                                                ClientAppVersionMappingDTORow["Deprecated"].ToString(),
                                                ClientAppVersionMappingDTORow.Table.Columns.Contains("Remarks") ? ClientAppVersionMappingDTORow["Remarks"].ToString() : ""
                                                );
            log.LogMethodExit();
            return ClientAppVersionMappingDTO;



        }

        /// <summary>
        /// return the record from the database based on  clientAppVersionMappingId
        /// </summary>
        /// <param name="clientAppVersionMappingId">clientId</param>
        /// <returns>return the ClientAppVersionMappingDTO object</returns>
        /// or null
        public ClientAppVersionMappingDTO GetClientAppVersionMappingDTOById(int clientAppVersionMappingId)
        {
            log.LogMethodEntry(clientAppVersionMappingId);

            string query = @"select * from ClientGuestAppIdMapping where Id=@Id";
            ClientAppVersionMappingDTO returnValue = null;
            SqlParameter parameter = new SqlParameter("@Id", clientAppVersionMappingId);

            DataTable datatable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (datatable.Rows.Count > 0)
            {

                returnValue = GetClientAppVersionMappingDTO(datatable.Rows[0]);

            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppVersionMappingId"></param>
        /// <returns></returns>
        public string GetSecurityCode(int clientAppVersionMappingId)
        {
            log.LogMethodEntry(clientAppVersionMappingId);
            string query = @"select * from ClientGuestAppIdMapping where Id=@Id";

            string securitycode = null;
            SqlParameter parameter = new SqlParameter("@Id", clientAppVersionMappingId);

            DataTable datatable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (datatable.Rows.Count > 0)
            {
                DataRow ClientAppVersionMappingDTORow = datatable.Rows[0];

                securitycode = ClientAppVersionMappingDTORow["SecurityCode"].ToString();

            }
            log.LogMethodExit(securitycode);
            return securitycode;

        }

        /// <summary>
        /// Gets the GetAllClientApp list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic ClientAppVersionMappingDTO   matching the search criteria</returns>
        public List<ClientAppVersionMappingDTO> GetAllClientAppVersionMapping(List<KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string>> searchParameters)
        {

            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;

            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" and ");
                foreach (KeyValuePair<ClientAppVersionMappingDTO.SearchParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(ClientAppVersionMappingDTO.SearchParameters.APP_ID) ||
                                          searchParameter.Key.Equals(ClientAppVersionMappingDTO.SearchParameters.RELEASE_NUMBER) ||
                                          searchParameter.Key.Equals(ClientAppVersionMappingDTO.SearchParameters.SECURITY_TOKEN))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToString(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(ClientAppVersionMappingDTO.SearchParameters.SECURITY_TOKEN_PARTIAL))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToString(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;//+ "  order by CompanyName";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            List<ClientAppVersionMappingDTO> ClientAppVersionMappingDTOList = new List<ClientAppVersionMappingDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ClientAppVersionMappingDTO ClientAppVersionMappingDTO = GetClientAppVersionMappingDTO(dataRow);
                    ClientAppVersionMappingDTOList.Add(ClientAppVersionMappingDTO);
                }
            }
            log.LogMethodExit(ClientAppVersionMappingDTOList);
            return ClientAppVersionMappingDTOList;

        }
        /// <summary>
        /// Delete the ClientAppVersionMappingDTO based on Id
        /// </summary>
        /// <param name="clientAppVersionMappingId">clientAppVersionMappingId</param>
        /// <returns>Returns int delete status</returns>
        public int DeleteClientAppVersionMapping(int clientAppVersionMappingId)
        {
            log.LogMethodEntry(clientAppVersionMappingId);

            string clientQuery = @"delete  
                                        from ClientGuestAppIdMapping
                                        where Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", clientAppVersionMappingId);
            int id = dataAccessHandler.executeUpdateQuery(clientQuery, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;

        }
    }
}
