
/********************************************************************************************
 * Project Name - User Role Price List  Data Handler
 * Description  - Data handler of the UserRole Price List class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016    Amaresh          Created 
 *2.70.2        17-Jul-2019    Deeksha          Modifications as per three tier standard.
 *            07-Aug-2019    Mushahid Faizan  Added isActive
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.PriceList
{
    /// <summary>
    /// UserRole PriceList DataHandler - Handles insert, update and select of userRolePriceList objects
    /// </summary>

    public class UserRolePriceListDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM UserRolePriceList AS upl ";
        /// <summary>
        /// Dictionary for searching Parameters for the Inventory Document Type  object.
        /// </summary>
        private static readonly Dictionary<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string> DBSearchParameters = new Dictionary<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>
        {
              {UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_PRICE_LISTID , "upl.RolePriceListId"},
              {UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_ID, "upl.Role_id"} ,
              {UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_ID_LIST, "upl.Role_id"} ,
              {UserRolePriceListDTO.SearchByUserRolePriceListParameters.SITE_ID, "upl.Site_id"} ,
              {UserRolePriceListDTO.SearchByUserRolePriceListParameters.ISACTIVE, "upl.IsActive"} ,
              {UserRolePriceListDTO.SearchByUserRolePriceListParameters.MASTER_ENTITY_ID, "upl.MasterEntityId"}
        };




        /// <summary>
        /// Default constructor of UserRolePriceListDataHandler class
        /// </summary>
        public UserRolePriceListDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }



        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating UserRolePriceListDataHandler Record.
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(UserRolePriceListDTO userRolePriceListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userRolePriceListDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@role_id", userRolePriceListDTO.Roleid, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rolePriceListId", userRolePriceListDTO.RolePriceListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@priceListId", userRolePriceListDTO.PriceListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", userRolePriceListDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", userRolePriceListDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the UserRolePriceList Programs record to the database
        /// </summary>
        /// <param name="userRolePriceList">UserRoleUserRolePriceListDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public UserRolePriceListDTO InsertuserRolePriceList(UserRolePriceListDTO userRolePriceList, string loginId, int siteId)
        {
            log.LogMethodEntry(userRolePriceList, loginId, siteId);
            string query = @"INSERT INTO[dbo].[UserRolePriceList]
                                                        (                                                 
                                                         Role_id,
                                                         PriceListId,
                                                         Guid,
                                                         Site_id,
                                                         CreationDate,
                                                         CreatedBy,
                                                         LastUpdatedDate,
                                                         LastUpdatedUser,
                                                         MasterEntityId,
                                                         IsActive
                                                        ) 
                                                values 
                                                        (
                                                          @role_id,
                                                          @priceListId,
                                                          Newid(),
                                                          @siteId,
                                                          Getdate(),
                                                          @createdBy,
                                                          Getdate(),
                                                          @createdBy,
                                                          @masterEntityId,
                                                          @isActive                                                          
                                                         ) 
                                               SELECT * FROM UserRolePriceList WHERE RolePriceListId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userRolePriceList, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRolePriceDTO(userRolePriceList, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting userRolePriceList", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(userRolePriceList);
            return userRolePriceList;
        }



        /// <summary>
        /// Updates the UserRole PriceList record
        /// </summary>
        /// <param name="userRolePriceList">UserRoleUserRolePriceListDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public UserRolePriceListDTO UpdateUserRolePriceList(UserRolePriceListDTO userRolePriceList, string loginId, int siteId)
        {
            log.LogMethodEntry(userRolePriceList, loginId, siteId);
            string query = @"UPDATE  [dbo].[UserRolePriceList]
                                    SET                  Role_id =@role_id, 
                                                         PriceListId =@priceListId,
                                                         -- Site_id =@siteId,                                 
                                                         LastUpdatedDate =Getdate(),
                                                         LastUpdatedUser =@lastUpdatedUser,
                                                         IsActive=@isActive                                                      
                                                         where RolePriceListId = @rolePriceListId
                                                SELECT * FROM UserRolePriceList WHERE RolePriceListId = @rolePriceListId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(userRolePriceList, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserRolePriceDTO(userRolePriceList, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating userRolePriceList", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userRolePriceList);
            return userRolePriceList;
        }


        /// <summary>
        /// Delete the record from the UserRolePriceList database based on RolePriceListId
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int rolePriceListId)
        {
            log.LogMethodEntry(rolePriceListId);
            string query = @"DELETE  
                             FROM UserRolePriceList
                             WHERE UserRolePriceList.RolePriceListId = @rolePriceListId";
            SqlParameter parameter = new SqlParameter("@rolePriceListId", rolePriceListId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Delete the record from the database based on  userRolePriceListId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteUserRolePriceList(int RolePriceListId)
        {
            log.Debug(" Starts-DeleteUserRolePriceList(int RolePriceListId) Method.");
            try
            {
                string UserRolePriceListQuery = @"delete  
                                          from UserRolePriceList
                                          where RolePriceListId = @RolePriceListId";

                SqlParameter[] UserRolePriceListParameters = new SqlParameter[1];
                UserRolePriceListParameters[0] = new SqlParameter("@RolePriceListId", RolePriceListId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(UserRolePriceListQuery, UserRolePriceListParameters);
                log.Debug(" Ends-DeleteUserRolePriceList(int RolePriceListId) Method.");
                return deleteStatus;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="inventoryDocumentTypeDTO">InventoryDocumentTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserRolePriceDTO(UserRolePriceListDTO userRolePriceListDTO, DataTable dt)
        {
            log.LogMethodEntry(userRolePriceListDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userRolePriceListDTO.RolePriceListId = Convert.ToInt32(dt.Rows[0]["RolePriceListId"]);
                userRolePriceListDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userRolePriceListDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userRolePriceListDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                userRolePriceListDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userRolePriceListDTO.LastUpdatedBy = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                userRolePriceListDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to UserRoleUserRolePriceListDTO class type
        /// </summary>
        /// <param name="userRolePriceListDataRow">UserRole DataRow</param>
        /// <returns>Returns UserRolePriceListDTO</returns>
        private UserRolePriceListDTO GetUserRoleUserRolePriceListDTO(DataRow userRolePriceListDataRow)
        {
            log.LogMethodEntry(userRolePriceListDataRow);
            UserRolePriceListDTO UserRolePriceListDataObject = new UserRolePriceListDTO(Convert.ToInt32(userRolePriceListDataRow["RolePriceListId"]),
                                                     Convert.ToInt32(userRolePriceListDataRow["Role_id"]),
                                                     Convert.ToInt32(userRolePriceListDataRow["PriceListId"]),
                                                     userRolePriceListDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(userRolePriceListDataRow["Guid"]),
                                                     userRolePriceListDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userRolePriceListDataRow["Site_id"]),
                                                     userRolePriceListDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userRolePriceListDataRow["LastUpdatedDate"]),
                                                     userRolePriceListDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(userRolePriceListDataRow["LastUpdatedUser"]),
                                                     userRolePriceListDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(userRolePriceListDataRow["CreatedBy"]),
                                                     userRolePriceListDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userRolePriceListDataRow["CreationDate"]),
                                                     userRolePriceListDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userRolePriceListDataRow["SynchStatus"]),
                                                     userRolePriceListDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userRolePriceListDataRow["MasterEntityId"]),
                                                     userRolePriceListDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(userRolePriceListDataRow["IsActive"])

                                                   );
            log.LogMethodExit(UserRolePriceListDataObject);
            return UserRolePriceListDataObject;
        }


        /// <summary>
        /// Gets the UserRolePriceList data of passed id 
        /// </summary>
        /// <param name="id">id of UserRolePriceList is passed as parameter</param>
        /// <returns>Returns UserRolePriceList</returns>
        public UserRolePriceListDTO GetUserRolePriceList(int id)
        {
            log.LogMethodEntry(id);
            UserRolePriceListDTO result = null;
            string query = SELECT_QUERY + @" WHERE upl.RolePriceListId= @rolePriceListId";
            SqlParameter parameter = new SqlParameter("@rolePriceListId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUserRoleUserRolePriceListDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the UserRolePriceListDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserRolePriceListDTO matching the search criteria</returns>
        public List<UserRolePriceListDTO> GetUserRolePriceList(List<KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<UserRolePriceListDTO> userRolePriceListDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectPriceListQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and "; 
                        if (searchParameter.Key.Equals(UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_PRICE_LISTID)
                           || searchParameter.Key.Equals(UserRolePriceListDTO.SearchByUserRolePriceListParameters.MASTER_ENTITY_ID)
                           || searchParameter.Key.Equals(UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_ID))
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UserRolePriceListDTO.SearchByUserRolePriceListParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UserRolePriceListDTO.SearchByUserRolePriceListParameters.ISACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                    selectPriceListQuery = selectPriceListQuery + query;
                selectPriceListQuery = selectPriceListQuery + " Order by RolePriceListId";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectPriceListQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                userRolePriceListDTOList = new List<UserRolePriceListDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserRolePriceListDTO userRolePriceListDTO = GetUserRoleUserRolePriceListDTO(dataRow);
                    userRolePriceListDTOList.Add(userRolePriceListDTO);
                }
            }
            log.LogMethodExit(userRolePriceListDTOList);
            return userRolePriceListDTOList;
        }
    }
}
