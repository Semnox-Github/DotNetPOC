/********************************************************************************************
 * Project Name - ClientAppUser  Data Handler Programs  
 * linkName  - Data handler of the ClientAppUser Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        04-May-2016   Rakshith        Created 
 *2.70        19-Jun-2019    Nitin            Modified for guest app
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using System.Net;
using Semnox.Core.Utilities;



namespace Semnox.Parafait.ClientApp
{
    ////<summary>
    ////ClientAppUserDatahandler Class 
    ////</summary>
    public class ClientAppUserDatahandler
    {
        DataAccessHandler dataAccessHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of CMSSocalLinks Data Handler class
        /// </summary>
        public ClientAppUserDatahandler(SqlTransaction sqlTransaction = null)
        {
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
    }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<ClientAppUserDTO.SearchParameters, string> DBSearchParameters = new Dictionary<ClientAppUserDTO.SearchParameters, string>
        {
                {ClientAppUserDTO.SearchParameters.IS_ACTIVE, "cau.IsActive"},
                {ClientAppUserDTO.SearchParameters.CLIENT_APP_ID, "cau.ClientAppId"},
                {ClientAppUserDTO.SearchParameters.CLIENT_APP_USER_ID, "cau.ClientAppUserId"},
                {ClientAppUserDTO.SearchParameters.IS_SIGNED_IN, "cau.UserSignedIn"},
                {ClientAppUserDTO.SearchParameters.DEVICE_GUID, "cau.DeviceGuid"},
                {ClientAppUserDTO.SearchParameters.USER_ID, "cau.UserId"},
                {ClientAppUserDTO.SearchParameters.CUSTOMER_ID, "cau.CustomerId"},
        };

        private const string SELECT_QUERY = @"select cau.* from ClientAppUser cau ";
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Address Record.
        /// </summary>
        /// <param name="clientAppUserDTO">ClientAppUserDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ClientAppUserDTO clientAppUserDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(clientAppUserDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ClientAppUserId", clientAppUserDTO.ClientAppUserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ClientAppId", clientAppUserDTO.ClientAppId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserId", clientAppUserDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", clientAppUserDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeviceType", clientAppUserDTO.DeviceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeviceGuid", clientAppUserDTO.DeviceGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserSignedIn", clientAppUserDTO.UserSignedIn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SignInExpiry", clientAppUserDTO.SignInExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", clientAppUserDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", clientAppUserDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", clientAppUserDTO.SiteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", clientAppUserDTO.MasterEntityId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ClientDTO  Items record to the database
        /// </summary>
        /// <param name="clientAppUserDTO">ClientDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        public ClientAppUserDTO InsertClientAppUser(ClientAppUserDTO clientAppUserDTO, string userId,int siteId)
        {
            log.LogMethodEntry();
            //log.Debug("Starts-InsertClientUser(ClientAppUserDTO clientAppUserDTO, string userId ) Method.");
           
                string insertClientQuery = @"insert into ClientAppUser
                                                            (  
                                                                ClientAppId,
                                                                UserId,
                                                                CustomerId,
                                                                DeviceGuid,
                                                                DeviceType,
                                                                UserSignedIn,
                                                                SignInExpiry,
                                                                IsActive,
                                                                CreatedBy,
                                                                CreationDate,
                                                                LastUpdatedBy,
                                                                LastupdatedDate,
                                                                Guid,
                                                                site_id
                                                            ) 
                                                values 
                                                        (
                                                                @ClientAppId,
                                                                @UserId,
                                                                @CustomerId,
                                                                @DeviceGuid,                                                               
                                                                @DeviceType,
                                                                @UserSignedIn,
                                                                @SignInExpiry,
                                                                @IsActive,
                                                                @CreatedBy,
                                                                Getdate(), 
                                                                @lastUpdatedBy ,
                                                                Getdate(), 
                                                                NewId(),
                                                                @site_id
                                                                
                                                             )SELECT * FROM ClientAppUser WHERE ClientAppUserId=scope_identity()";
                try
                {
                    DataTable dt = dataAccessHandler.executeSelectQuery(insertClientQuery, GetSQLParameters(clientAppUserDTO, userId, siteId).ToArray(), sqlTransaction);
                    RefreshClientAppUserDTO(clientAppUserDTO, dt);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while inserting ClientAppUserDTO", ex);
                    log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                    throw;
                }
                log.LogMethodExit(clientAppUserDTO);
                return clientAppUserDTO;
            

        }

        private void RefreshClientAppUserDTO(ClientAppUserDTO clientAppUserDTO, DataTable dt)
        {
            log.LogMethodEntry(clientAppUserDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                clientAppUserDTO.ClientAppUserId = Convert.ToInt32(dt.Rows[0]["ClientAppUserId"]);
                clientAppUserDTO.LastupdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                clientAppUserDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                clientAppUserDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                clientAppUserDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                clientAppUserDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                clientAppUserDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);

            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Inserts the ClientAppUserDTO  Items record to the database
        /// </summary>
        /// <param name="ClientAppUserDTO">ClientAppUserDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        public ClientAppUserDTO UpdateClientAppUser(ClientAppUserDTO clientAppUserDTO, string userId,int siteId)
        {
            log.LogMethodEntry(clientAppUserDTO, userId,siteId);
            //log.Debug("Starts-UpdateClientUser(ClientAppUserDTO clientAppUserDTO, string userId, int siteId) Method.");
           
                string updateClientUserQuery = @"update ClientAppUser 
                                                          set 
                                                            --ClientAppId=@ClientAppId,
                                                            UserId=@UserId,
                                                            CustomerId=@CustomerId,
                                                            --DeviceGuid=@DeviceGuid,
                                                            --DeviceType=@DeviceType,
                                                            UserSignedIn=@UserSignedIn,
                                                            SignInExpiry=@SignInExpiry,
                                                            IsActive=@IsActive,
                                                            LastUpdatedBy=@LastUpdatedBy,
                                                            LastupdatedDate=GetDate()
                                                            WHERE ClientAppUserId=@ClientAppUserId
                                                            SELECT * FROM ClientAppUser WHERE ClientAppUserId  = @ClientAppUserId";
                try
                {
                    DataTable dt = dataAccessHandler.executeSelectQuery(updateClientUserQuery, GetSQLParameters(clientAppUserDTO, userId,siteId).ToArray(), sqlTransaction);
                    RefreshClientAppUserDTO(clientAppUserDTO, dt);
                }


                catch (Exception ex)
                {
                    log.Error("Error occurred while Updating ClientAppUserDTO", ex);
                    log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                    throw;
                }
                log.LogMethodExit(clientAppUserDTO);
                return clientAppUserDTO;

            
        }

        /// <summary>
        /// return the record from the database
        /// Convert the datarow to ClientAppUserDTO object
        /// </summary>
        /// <returns>return the ClientAppUserDTO object</returns>
        public ClientAppUserDTO GetClientAppUserDTO(DataRow ClientAppUserRow)
        {
            log.LogMethodEntry(ClientAppUserRow);

            ClientAppUserDTO clientAppUserDTO = new ClientAppUserDTO(
                                                        ClientAppUserRow["ClientAppUserId"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppUserRow["ClientAppUserId"]),
                                                        ClientAppUserRow["ClientAppId"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppUserRow["ClientAppId"]),
                                                        ClientAppUserRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppUserRow["UserId"]),
                                                        ClientAppUserRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppUserRow["CustomerId"]),
                                                        ClientAppUserRow["DeviceGuid"].ToString(),
                                                        ClientAppUserRow["DeviceType"].ToString(),
                                                        ClientAppUserRow["UserSignedIn"] == DBNull.Value ? false : Convert.ToBoolean(ClientAppUserRow["UserSignedIn"]),
                                                        ClientAppUserRow["SignInExpiry"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(ClientAppUserRow["SignInExpiry"]),
                                                        ClientAppUserRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(ClientAppUserRow["IsActive"]),
                                                        ClientAppUserRow["creationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ClientAppUserRow["creationDate"]),
                                                        ClientAppUserRow["CreatedBy"].ToString(),
                                                        ClientAppUserRow["LastUpdatedBy"].ToString(),
                                                        ClientAppUserRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ClientAppUserRow["LastupdatedDate"]),
                                                        ClientAppUserRow["Guid"].ToString(),
                                                        ClientAppUserRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppUserRow["site_id"]),
                                                        ClientAppUserRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(ClientAppUserRow["MasterEntityId"]),
                                                        ClientAppUserRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(ClientAppUserRow["SynchStatus"]));
            log.LogMethodExit(clientAppUserDTO);
            return clientAppUserDTO;
            
        }


        /// <summary>
        /// return the record from the database based on   UserId
        /// </summary>
        /// <returns>return the ClientAppUserDTO object</returns>
        /// or null
        public ClientAppUserDTO GetClientAppUserDTOById(int clientAppUserId)
        {
            log.LogMethodEntry(clientAppUserId);

            string query = @"select * from ClientAppUser where ClientAppUserId=@Id";
            ClientAppUserDTO returnValue = null;
            SqlParameter parameter = new SqlParameter("@Id", clientAppUserId);

            DataTable datatable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (datatable.Rows.Count > 0)
            {

                returnValue = GetClientAppUserDTO(datatable.Rows[0]);

            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the GetAllClientUser list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic ClientAppUserDTO   matching the search criteria</returns>
        public List<ClientAppUserDTO> GetAllClientAppUsers(List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters)
        {
            //log.Debug("Starts-GetClientUserDTOList(List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters) Method.");
            log.LogMethodEntry(searchParameters);
            int count = 0;

            string clientUserSelectQuery = SELECT_QUERY;//"Select * from ClientAppUser as cau";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder();//(" where ");
                foreach (KeyValuePair<ClientAppUserDTO.SearchParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0) ? " where " : " and ";

                        if (searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.IS_ACTIVE) || searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.IS_SIGNED_IN))
                        {
                            query.Append(joinOperator + " isnull( " + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.CLIENT_APP_ID) ||
                                 searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.CLIENT_APP_USER_ID) ||
                                 searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.USER_ID) ||
                                 searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.CUSTOMER_ID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                        }
                        else if (searchParameter.Key.Equals(ClientAppUserDTO.SearchParameters.DEVICE_GUID))
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToString(searchParameter.Value)));

                        }
                        else
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.Error(searchParameter.Key + " search parameter not found");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                clientUserSelectQuery = clientUserSelectQuery + query;
            }
            DataTable clientUserDataTable = dataAccessHandler.executeSelectQuery(clientUserSelectQuery, parameters.ToArray(), sqlTransaction);
            List<ClientAppUserDTO> clientAppUserDTOList = new List<ClientAppUserDTO>();
            if (clientUserDataTable.Rows.Count > 0)
            {
                foreach (DataRow clientUserDataRow in clientUserDataTable.Rows)
                {
                    ClientAppUserDTO clientAppUserDTO = GetClientAppUserDTO(clientUserDataRow);
                    clientAppUserDTOList.Add(clientAppUserDTO);
                }
            }
            log.LogMethodExit(clientAppUserDTOList);
            return clientAppUserDTOList;
        }

        /// <summary>
        /// Delete the ClientAppUserDTO based on Id
        /// </summary> 
        /// <param name="userId">userId</param>
        /// <return>return  int status</return> 
        public int DeleteClientAppUser(int clientAppUserId)
        {
            log.LogMethodEntry(clientAppUserId);
            string clientUserQuery = @"delete  
                                        from ClientAppUser
                                    where ClientAppUserId = @ClientAppUserId";

            SqlParameter parameter = new SqlParameter("@ClientAppUserId", clientAppUserId);
            int deleteId = dataAccessHandler.executeUpdateQuery(clientUserQuery, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(deleteId);
            return deleteId;
        }
    }
}
