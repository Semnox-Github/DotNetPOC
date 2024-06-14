/********************************************************************************************
 * Project Name - ClientAppDataHandler
 * Description  - Business Login of Client App entity created for Dashboards
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
    public class ClientAppDataHandler
    {
        DataAccessHandler dataAccessHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ClientApp as ca";

        /// <summary>
        /// Default constructor of CMSSocalLinks Data Handler class
        /// </summary>
        public ClientAppDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<ClientAppDTO.SearchParameters, string> DBSearchParameters = new Dictionary<ClientAppDTO.SearchParameters, string>
        {
                {ClientAppDTO.SearchParameters.ACTIVE, "ca.IsActive"},
                {ClientAppDTO.SearchParameters.APP_ID, "ca.AppId"},
                {ClientAppDTO.SearchParameters.GUID, "ca.Guid"}
        };

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerRelationship Record.
        /// </summary>
        /// <param name="clientAppDTO">ClientAppDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ClientAppDTO clientAppDTO, string loginId)
        {
            log.LogMethodEntry(clientAppDTO, loginId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ClientAppId", clientAppDTO.ClientAppId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppId", clientAppDTO.AppId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AppName", clientAppDTO.AppName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", clientAppDTO.MasterEntityId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", clientAppDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", clientAppDTO.SiteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", clientAppDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ClientAppDTO InsertClientAppVersionMapping(ClientAppDTO clientAppDTO, string userId)
        {
            log.LogMethodEntry(clientAppDTO, userId);

            string insertClientQuery = @"insert into ClientApp
                                                (  
                                                    ClientAppId,
                                                    AppId,
                                                    AppName,
                                                    IsActive,
                                                    CreatedBy,
                                                    CreationDate,
                                                    LastUpdatedBy, 
                                                    LastupdatedDate,
                                                    GUID,
                                                    site_id,
                                                    SynchStatus,
                                                    MasterEntityId
                                                ) 
                                                values 
                                                (
                                                    @ClientAppId
                                                    @AppId,
                                                    @AppName,
                                                    @IsActive,
                                                    @CreatedBy,
                                                    Getdate(),
                                                    @LastUpdatedBy, 
                                                    Getdate(),
                                                    NEWID(),
                                                    @site_id,
                                                    @SynchStatus,
                                                    @MasterEntityId
                                                    )SELECT FROM ClientGuestAppIdMapping WHERE Id=scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertClientQuery, GetSQLParameters(clientAppDTO, userId).ToArray(), sqlTransaction);
                RefreshClientAppDTO(clientAppDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ClientAppDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(clientAppDTO);
            return clientAppDTO;


        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="clientAppDTO">ClientAppDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshClientAppDTO(ClientAppDTO clientAppDTO, DataTable dt)
        {
            log.LogMethodEntry(clientAppDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                clientAppDTO.ClientAppId = Convert.ToInt32(dt.Rows[0]["Id"]);
                clientAppDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                clientAppDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                clientAppDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                clientAppDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                clientAppDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                clientAppDTO.MasterEntityId = dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]);

            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the ClientAppDTO  Items record to the database
        /// </summary>
        /// <param name="clientAppDTO">ClientAppDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        /// 
        public ClientAppDTO UpdateClientAppVersionMapping(ClientAppDTO clientAppDTO, string userId)
        {
            log.LogMethodEntry(clientAppDTO, userId);
            string updateClientQuery = @"update ClientApp 
                                        set 
                                        AppId=@AppId,
                                        AppName=@AppName,
                                        IsActive=@IsActive,
                                        LastUpdatedBy=@LastUpdatedBy,
                                        LastupdatedDate=GetDate()
                                        where  ClientAppId=@ClientAppId
                                        SELECT * FROM ClientApp WHERE ClientAppId  = @ClientAppId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateClientQuery, GetSQLParameters(clientAppDTO, userId).ToArray(), sqlTransaction);
                RefreshClientAppDTO(clientAppDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ClientAppDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(clientAppDTO);
            return clientAppDTO;
        }

        /// <summary>
        /// return the record from the database
        /// Convert the datarow to ClientAppDTO object
        /// </summary>
        /// <param name="clientAppDTORow">ClientAppDTORow</param>
        /// <returns>return the ClientAppDTO object</returns>
        public ClientAppDTO GetClientAppDTO(DataRow clientAppDTORow)
        {
            log.LogMethodEntry(clientAppDTORow);

            ClientAppDTO ClientAppDTO = new ClientAppDTO(
                                                clientAppDTORow["ClientAppId"] == DBNull.Value ? -1 : Convert.ToInt32(clientAppDTORow["ClientAppId"]),
                                                clientAppDTORow["AppId"] == DBNull.Value ? null : clientAppDTORow["AppId"].ToString(),
                                                clientAppDTORow["AppName"] == DBNull.Value ? null : clientAppDTORow["AppName"].ToString(),
                                                clientAppDTORow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(clientAppDTORow["IsActive"]),
                                                clientAppDTORow["CreatedBy"] == DBNull.Value ? null : clientAppDTORow["CreatedBy"].ToString(),
                                                clientAppDTORow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(clientAppDTORow["CreationDate"]),
                                                clientAppDTORow["LastUpdatedBy"] == DBNull.Value ? null : clientAppDTORow["LastUpdatedBy"].ToString(),
                                                clientAppDTORow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(clientAppDTORow["LastupdatedDate"]),
                                                clientAppDTORow["Guid"].ToString(),
                                                clientAppDTORow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(clientAppDTORow["SynchStatus"]),
                                                clientAppDTORow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(clientAppDTORow["site_id"]),
                                                clientAppDTORow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(clientAppDTORow["MasterEntityId"])
                                                 );
            log.LogMethodExit();
            return ClientAppDTO;



        }

        /// <summary>
        /// return the record from the database based on  ClientAppId
        /// </summary>
        /// <param name="clientAppId">clientId</param>
        /// <returns>return the ClientAppDTO object</returns>
        /// or null
        public ClientAppDTO GetClientAppDTOById(int clientAppId)
        {
            log.LogMethodEntry(clientAppId);
            string query = @"select * from ClientApp where ClientAppId=@Id";
            ClientAppDTO returnValue = null;
            SqlParameter parameter = new SqlParameter("@Id", clientAppId);

            DataTable datatable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (datatable.Rows.Count > 0)
            {
                returnValue = GetClientAppDTO(datatable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the GetAllClientApp list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic ClientAppDTO   matching the search criteria</returns>
        public List<ClientAppDTO> GetAllClientAppDTO(List<KeyValuePair<ClientAppDTO.SearchParameters, string>> searchParameters)
        {

            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;

            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ClientAppDTO.SearchParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(ClientAppDTO.SearchParameters.APP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            List<ClientAppDTO> clientAppDTOList = new List<ClientAppDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ClientAppDTO clientAppDTO = GetClientAppDTO(dataRow);
                    clientAppDTOList.Add(clientAppDTO);
                }
            }
            log.LogMethodExit(clientAppDTOList);
            return clientAppDTOList;

        }

        /// <summary>
        /// Delete the ClientAppDTO based on clientAppId
        /// </summary>
        /// <param name="clientAppId">clientAppId</param>
        /// <returns>Returns int delete status</returns>
        public int DeleteClientAppDTO(int clientAppId)
        {
            log.LogMethodEntry(clientAppId);
            string clientQuery = @"delete  from ClientApp where ClientAppId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", clientAppId);
            int id = dataAccessHandler.executeUpdateQuery(clientQuery, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;

        }
    }
}
