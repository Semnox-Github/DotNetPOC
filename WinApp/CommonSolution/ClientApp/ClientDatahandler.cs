/********************************************************************************************
 * Project Name - Client Data Handler Programs  
 * linkName  - Data handler of the ClientDatahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        04-May-2016   Rakshith          Created 
 *2.70.2      03-Jan-2010   Nitin             Modified for guest app
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
    ////ClientDatahandler Class 
    ////</summary>
    public class ClientDatahandler
    {
        DataAccessHandler dataAccessHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of CMSSocalLinks Data Handler class
        /// </summary>
        public ClientDatahandler()
        {
            dataAccessHandler = new DataAccessHandler();
        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<ClientDTO.SearchParameters, string> DBSearchParameters = new Dictionary<ClientDTO.SearchParameters, string>
        {
                {ClientDTO.SearchParameters.ACTIVE, "Active"},
                {ClientDTO.SearchParameters.SECURITY_TOKEN, "SecurityToken"},
                {ClientDTO.SearchParameters.GUID, "Guid"}
        };

        /// <summary>
        /// Inserts the ClientDTO  Items record to the database
        /// </summary>
        /// <param name="ClientDTO">ClientDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertClient(ClientDTO clientDTO, string userId )
        {
            log.Debug("Starts-InsertClient(ClientDTO clientDTO, string userId ) Method.");
            try
            {
                string insertClientQuery = @"insert into Clients
                                                            (  
                                                               CompanyName,
                                                               SecurityToken,
                                                               GatewayUrl,
                                                               Active,
                                                               LicenseCount,
                                                               CreationDate,
                                                               CreatedBy,
                                                               LastUpdatedDate,
                                                               LastUpdatedBy, 
                                                               Guid,
                                                               CompanyEmail
                                                        ) 
                                                values 
                                                        (
                                                              @companyName,
                                                              @securityToken,
                                                              @gatewayUrl,
                                                              @active,
                                                              @licenseCount,
                                                              Getdate(),
                                                              @createdBy,
                                                              Getdate(), 
                                                              @lastUpdatedBy,
                                                              NEWID(),
                                                              @companyEmail 
                                                             )SELECT CAST(scope_identity() AS int)";

                List<SqlParameter> insertClientParameter = new List<SqlParameter>();

                insertClientParameter.Add(new SqlParameter("@companyName", clientDTO.CompanyName));

                GenerateToken generateToken = new GenerateToken();
                insertClientParameter.Add(new SqlParameter("@securityToken", generateToken.GetToken()));
                
                if (string.IsNullOrEmpty(clientDTO.GatewayUrl))
                {
                    insertClientParameter.Add(new SqlParameter("@gatewayUrl", DBNull.Value));
                }
                else
                {
                    insertClientParameter.Add(new SqlParameter("@gatewayUrl", clientDTO.GatewayUrl));
                }
                insertClientParameter.Add(new SqlParameter("@active", clientDTO.Active));
                insertClientParameter.Add(new SqlParameter("@licenseCount", clientDTO.LicenseCount));
                insertClientParameter.Add(new SqlParameter("@createdBy", userId));
                insertClientParameter.Add(new SqlParameter("@lastUpdatedBy", userId));
                if (string.IsNullOrEmpty(clientDTO.CompanyEmail))
                {
                    insertClientParameter.Add(new SqlParameter("@companyEmail", DBNull.Value));
                }
                else
                {
                    insertClientParameter.Add(new SqlParameter("@companyEmail", clientDTO.CompanyEmail));
                }
                int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertClientQuery, insertClientParameter.ToArray());
                log.Debug("Ends-InsertClient(ClientDTO clientDTO, string userId, int siteId) Method.");
                return idOfRowInserted;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Updates the ClientDTO  Items record to the database
        /// </summary>
        /// <param name="ClientDTO">ClientDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <returns>Returns inserted record id</returns>
        /// 
        public int UpdateClient(ClientDTO clientDTO, string userId)
        {
            log.Debug("Starts-UpdateClient(ClientDTO clientDTO, string userId  Method.");
            try
            {
                string updateClientQuery = @"update Clients 
                                                          set 
                                                               CompanyName= @companyName,
                                                               SecurityToken=@securityToken,
                                                               GatewayUrl=@gatewayUrl,
                                                               Active=@active,
                                                               LicenseCount=@licenseCount,
                                                               LastUpdatedDate=GetDate(),
                                                               LastUpdatedBy=@lastUpdatedBy,
                                                               CompanyEmail=@companyEmail
                                                               where  ClientId=@clientId";

                List<SqlParameter> updateClientParameter = new List<SqlParameter>();

                updateClientParameter.Add(new SqlParameter("@clientId", clientDTO.ClientId));

                if (string.IsNullOrEmpty(clientDTO.CompanyName))
                {
                    updateClientParameter.Add(new SqlParameter("@companyName", DBNull.Value));
                }
                else
                {
                    updateClientParameter.Add(new SqlParameter("@companyName", clientDTO.CompanyName));
                }
                if (string.IsNullOrEmpty(clientDTO.SecurityToken))
                {
                    updateClientParameter.Add(new SqlParameter("@securityToken", DBNull.Value));
                }
                else
                {
                    updateClientParameter.Add(new SqlParameter("@securityToken", clientDTO.SecurityToken));
                }
                if (string.IsNullOrEmpty(clientDTO.GatewayUrl))
                {
                    updateClientParameter.Add(new SqlParameter("@gatewayUrl", DBNull.Value));
                }
                else
                {
                    updateClientParameter.Add(new SqlParameter("@gatewayUrl", clientDTO.GatewayUrl));
                }
                updateClientParameter.Add(new SqlParameter("@licenseCount", clientDTO.LicenseCount));
                updateClientParameter.Add(new SqlParameter("@active", clientDTO.Active));
                updateClientParameter.Add(new SqlParameter("@lastUpdatedBy", userId));
                if (string.IsNullOrEmpty(clientDTO.CompanyEmail))
                {
                    updateClientParameter.Add(new SqlParameter("@companyEmail", DBNull.Value));
                }
                else
                {
                    updateClientParameter.Add(new SqlParameter("@companyEmail", clientDTO.CompanyEmail));
                }
                int idOfRowInserted = dataAccessHandler.executeUpdateQuery(updateClientQuery, updateClientParameter.ToArray());
                log.Debug("Ends-UpdateClient(ClientDTO clientDTO, string userId, int siteId) Method.");
                return idOfRowInserted;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to clientDTO object
        /// </summary>
        /// <param name="ClientDTORow">ClientDTORow</param>
        /// <returns>return the clientDTO object</returns>
        public ClientDTO GetClientDTO(DataRow ClientDTORow)
        {

            try
            {
                ClientDTO clientDTO = new ClientDTO(
                                                    ClientDTORow["ClientId"] == DBNull.Value ? -1 : Convert.ToInt32(ClientDTORow["ClientId"]),
                                                    ClientDTORow["CompanyName"].ToString(),
                                                    ClientDTORow["SecurityToken"].ToString(),
                                                    ClientDTORow["GatewayUrl"].ToString(),
                                                    ClientDTORow["Active"] == DBNull.Value ? false : Convert.ToBoolean(ClientDTORow["Active"]),
                                                    ClientDTORow["LicenseCount"] == DBNull.Value ? -1 : Convert.ToInt32(ClientDTORow["LicenseCount"]),
                                                    ClientDTORow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ClientDTORow["CreationDate"]),
                                                    ClientDTORow["CreatedBy"].ToString(),
                                                    ClientDTORow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ClientDTORow["LastupdatedDate"]),
                                                    ClientDTORow["LastUpdatedBy"].ToString(),
                                                    ClientDTORow["Guid"].ToString(),
                                                    ClientDTORow["CompanyEmail"].ToString() 
                                                 );

                return clientDTO;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());

            }
        }

        /// <summary>
        /// return the record from the database based on  clientId
        /// </summary>
        /// <param name="clientId">clientId</param>
        /// <returns>return the ClientDTO object</returns>
        /// or null
        public ClientDTO GetClientDTOById(int clientId)
        {
            try
            {
                string clientQuery = @"select * from Clients where ClientId=@clientId";

                SqlParameter[] clientParameters = new SqlParameter[1];
                clientParameters[0] = new SqlParameter("@clientId", clientId);
                DataTable clientDataTable = dataAccessHandler.executeSelectQuery(clientQuery, clientParameters);
                if (clientDataTable.Rows.Count > 0)
                {
                    DataRow clientRow = clientDataTable.Rows[0];
                    ClientDTO clientDTO = GetClientDTO(clientRow);

                    return clientDTO;
                }
                else
                {
                    ClientDTO clientDTO = new ClientDTO();
                    return clientDTO;
                }
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// return the record from the database based on appId
        /// </summary>
        /// <param name="appId">clientId</param>
        /// <returns>return the ClientDTO object</returns>
        /// or null
        public ClientDTO GetClientDTOByAppId(string appId, string releaseNumber) 
        {
            try
            {
                //ClientId, CompanyName, SecurityToken, GatewayUrl, Active, LicenseCount, CreationDate, CreatedBy, LastupdatedDate, LastUpdatedBy, Guid, CompanyEmail

                string clientQuery = @"select c.ClientId, c.CompanyName, isnull(g.TokenizationKey,c.SecurityToken) SecurityToken, isnull(g.APIKey,'') APIKey, ISNULL(g.gatewayurl, c.GatewayURL) GatewayURL,
                                     c.Active, c.CreationDate, c.CreatedBy, c.lastupdateddate, c.LastUpdatedBy, c.Guid, c.companyemail, c.licenseCount
                                    from ClientGuestAppIdMapping g, Clients c 
                                    where g.[ClientId ] = c.ClientId
                                    and g.APPID = @appId";
                if (!String.IsNullOrWhiteSpace(releaseNumber))
                    clientQuery += " and (g.ReleaseNumber = @releaseNumber) ";
                else
                    clientQuery += " and (g.ReleaseNumber is null)";

                SqlParameter[] clientParameters = new SqlParameter[2];
                clientParameters[0] = new SqlParameter("@appId", appId);
                clientParameters[1] = new SqlParameter("@releaseNumber", !String.IsNullOrWhiteSpace(releaseNumber) ? releaseNumber : "");

                DataTable clientDataTable = dataAccessHandler.executeSelectQuery(clientQuery, clientParameters);
                if (clientDataTable.Rows.Count > 0)
                {
                    DataRow clientRow = clientDataTable.Rows[0];
                    ClientDTO clientDTO = GetClientDTO(clientRow);

                    return clientDTO;
                }
                else
                {
                    ClientDTO clientDTO = new ClientDTO();
                    return clientDTO;
                }
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Gets the GetAllClientApp list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic ClientDTO   matching the search criteria</returns>
        public List<ClientDTO> GetAllClients(List<KeyValuePair<ClientDTO.SearchParameters, string>> searchParameters)
        {

            log.Debug("Starts-GetClientDTOList(List<KeyValuePair<ClientDTO.SearchParameters, string>> searchParameters) Method.");
            int count = 0;
            try
            {
                string clientSelectQuery = @"SELECT *
                                                 FROM Clients ";

                
                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach(KeyValuePair<ClientDTO.SearchParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joinOperartor = (count == 0) ? " " : " and ";

                            if (searchParameter.Key.Equals(ClientDTO.SearchParameters.ACTIVE))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                            } 
                            else if (searchParameter.Key.Equals(ClientDTO.SearchParameters.SECURITY_TOKEN) || 
                                              searchParameter.Key.Equals(ClientDTO.SearchParameters.GUID))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  '" + searchParameter.Value + "' ");
                            }
                          
                            else
                            {
                                query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                            count++;
                        }
                        else
                        {
                            log.Debug("Ends-GetClientDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }
                    clientSelectQuery = clientSelectQuery + query + "  order by CompanyName";
                }
                DataTable clientDataTable = dataAccessHandler.executeSelectQuery(clientSelectQuery, null);
                List<ClientDTO> clientDTOList = new List<ClientDTO>();
                if (clientDataTable.Rows.Count > 0)
                {

                    foreach (DataRow clientDataRow in clientDataTable.Rows)
                    {
                        ClientDTO clientDTO = GetClientDTO(clientDataRow);
                        clientDTOList.Add(clientDTO);
                    }
                    log.Debug("Ends-GetClientDTOList((searchParameters) Method by returning clientDTOList.");
                }
                return clientDTOList;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Delete the ClientDTO based on Id
        /// </summary>
        /// <param name="clientId">clientId</param>
        /// <returns>Returns int delete status</returns>
        public int DeleteClient(int clientId)
        {
            try
            {
                string clientQuery = @"delete  
                                        from Clients
                                        where ClientId = @clientId";

                SqlParameter[] clientParameters = new SqlParameter[1];
                clientParameters[0] = new SqlParameter("@clientId", clientId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(clientQuery, clientParameters);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }



    }

}
