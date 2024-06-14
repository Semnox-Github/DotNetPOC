/********************************************************************************************
 * Project Name - MembershipExclusionRule Data Handler
 * Description  - Data handler of the MembershipExclusionRule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Ankitha C Kothwal     Created
 *2.70.2      10-Dec-2019   Jinto Thomas          Removed siteid from update query
 *2.110.00    26-Nov-2020   Abhishek              Modified : Modified to 3 Tier Standard
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class MembershipExclusionRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CardTypeRule AS ctr ";

        private static readonly Dictionary<MembershipExclusionRuleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MembershipExclusionRuleDTO.SearchByParameters, string>
            {
                {MembershipExclusionRuleDTO.SearchByParameters.ID, "ctr.ID"},
                {MembershipExclusionRuleDTO.SearchByParameters.PRODUCT_ID, "ctr.Product_Id"},
                {MembershipExclusionRuleDTO.SearchByParameters.GAME_ID, "ctr.game_id"},
                {MembershipExclusionRuleDTO.SearchByParameters.GAME_PROFILE_ID, "ctr.game_profile_id"},
                {MembershipExclusionRuleDTO.SearchByParameters.IS_ACTIVE, "ctr.Active"},
                {MembershipExclusionRuleDTO.SearchByParameters.DISALLOWED, "ctr.DisAllowed"},
                {MembershipExclusionRuleDTO.SearchByParameters.SITE_ID, "ctr.site_id"},
                {MembershipExclusionRuleDTO.SearchByParameters.MASTER_ENTITY_ID,"ctr.MasterEntityId"},
                {MembershipExclusionRuleDTO.SearchByParameters.MEMBERSHIP_ID,"ctr.MembershipID"},
                {MembershipExclusionRuleDTO.SearchByParameters.LAST_UPDATED_BY,"ctr.LastUpdatedBy"}
            };

        /// <summary>
        /// Default constructor of MembershipExclusionRuleDataHandler class
        /// </summary>
        public MembershipExclusionRuleDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MembershipExclusionRule Record.
        /// </summary>
        /// <param name="membershipExclusionRuleDTO">MembershipExclusionRuleDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MembershipExclusionRuleDTO membershipExclusionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipExclusionRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", membershipExclusionRuleDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", membershipExclusionRuleDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameId", membershipExclusionRuleDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameProfileId", membershipExclusionRuleDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", membershipExclusionRuleDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@disallowed", membershipExclusionRuleDTO.Disallowed == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", membershipExclusionRuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@membershipId", membershipExclusionRuleDTO.MembershipId, true));//true passed bcz if id is -1, it passes null
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MembershipExclusionRule record to the database
        /// </summary>
        /// <param name="membershipExclusionRuleDTO">MembershipExclusionRuleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted MembershipExclusionRuleDTO</returns>
        public MembershipExclusionRuleDTO Insert(MembershipExclusionRuleDTO membershipExclusionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipExclusionRuleDTO, loginId, siteId);
            string query = @"INSERT INTO CardTypeRule
                                        ( 
                                            [Product_Id],
                                            [game_id],
                                            [game_profile_id],
                                            [Active],
                                            [Disallowed],
                                            [LastUpdatedBy],
                                            [LastUpdatedDate],
                                            [Guid],
                                            [site_id], 
                                            [MasterEntityId],
                                            [MembershipID],
                                            [CreatedBy],
                                            [CreationDate] 
                                        ) 
                                VALUES 
                                        (
                                            @productId ,
                                            @gameId ,
                                            @gameProfileId, 
                                            @isActive,
                                            @disallowed,
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            NEWID(),
                                            @siteId, 
                                            @masterEntityId,
                                            @membershipId,
                                            @createdBy,
                                            GETDATE()
                                         ) SELECT * FROM CardTypeRule WHERE ID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(membershipExclusionRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipExclusionRuleDTO(membershipExclusionRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(membershipExclusionRuleDTO);
            return membershipExclusionRuleDTO;
        }

        /// <summary>
        /// Deletes the MembershipExclusionRule based on id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteQuery = @"delete from CardTypeRule where ID = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", id);
                dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the MembershipExclusionRule record
        /// </summary>
        /// <param name="MembershipExclusionRuleDTO">MembershipExclusionRuleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MembershipExclusionRuleDTO Update(MembershipExclusionRuleDTO membershipExclusionRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipExclusionRuleDTO, loginId, siteId);
            string query = @"UPDATE CardTypeRule
                                SET [Product_Id] = @productId,
                                    [game_id] = @gameId,
                                    [game_profile_id] = @gameProfileId,
                                    [Active] = @isActive,
                                    [Disallowed] = @disallowed, 
                                    [LastUpdatedBy] = @lastUpdatedBy,
                                    [LastUpdatedDate] = getdate(), 
                                    -- [site_id] = @siteId,
                                    [MasterEntityId] = @masterEntityId,
                                    [MembershipID]=@membershipId 
                                WHERE ID = @id
                                SELECT* FROM CardTypeRule WHERE ID = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(membershipExclusionRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipExclusionRuleDTO(membershipExclusionRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(membershipExclusionRuleDTO);
            return membershipExclusionRuleDTO;
        }

        private void RefreshMembershipExclusionRuleDTO(MembershipExclusionRuleDTO membershipExclusionRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(membershipExclusionRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                membershipExclusionRuleDTO.Id = Convert.ToInt32(dt.Rows[0]["ID"]);
                membershipExclusionRuleDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                membershipExclusionRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                membershipExclusionRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                membershipExclusionRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                membershipExclusionRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                membershipExclusionRuleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MembershipExclusionRuleDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns MembershipExclusionRuleDTO</returns>
        private MembershipExclusionRuleDTO GetMembershipExclusionRuleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MembershipExclusionRuleDTO membershipExclusionRuleDTO = new MembershipExclusionRuleDTO(Convert.ToInt32(dataRow["ID"]),
                                            dataRow["Product_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Product_Id"]),
                                            dataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_id"]),
                                            dataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_profile_id"]),
                                            dataRow["Active"] == DBNull.Value ? true : (dataRow["Active"].ToString() == "Y" ? true : false),
                                            dataRow["DisAllowed"] == DBNull.Value ? false : (dataRow["DisAllowed"].ToString() == "Y" ? true : false),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["MembershipID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipID"]),
                                            dataRow["createdBy"] == DBNull.Value ? string.Empty : dataRow["createdBy"].ToString(),
                                            dataRow["creationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["creationDate"])
                                            );
            log.LogMethodExit(membershipExclusionRuleDTO);
            return membershipExclusionRuleDTO;
        }

        /// <summary>
        /// Gets the MembersipExclusionRule data of passed MembershipExclusionRule Id
        /// </summary>
        /// <param name="Id">integer type parameter</param>
        /// <returns>Returns MembershipExclusionRuleDTO</returns>
        public MembershipExclusionRuleDTO GetMembershipExclusionRuleDTO(int id)
        {
            log.LogMethodEntry(id);
            MembershipExclusionRuleDTO membershipExclusionRuleDTO = null;
            string query = SELECT_QUERY + @" WHERE ctr.ID = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                membershipExclusionRuleDTO = GetMembershipExclusionRuleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(membershipExclusionRuleDTO);
            return membershipExclusionRuleDTO;
        }

        /// <summary>
        /// Gets the MembershipExclusionRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MembershipExclusionRuleDTO matching the search criteria</returns>
        public List<MembershipExclusionRuleDTO> GetMembershipExclusionRuleDTOList(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList = null;
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.ID ||
                            searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.GAME_ID ||
                            searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.GAME_PROFILE_ID ||
                            searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.MASTER_ENTITY_ID
                           )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.DISALLOWED ||
                                 searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        //else if (searchParameter.Key == MembershipExclusionRuleDTO.SearchByParameters.IS_ACTIVE)
                        //{    
                        //    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        //    parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        //}
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                membershipExclusionRuleDTOList = new List<MembershipExclusionRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MembershipExclusionRuleDTO membershipExclusionRuleDTO = GetMembershipExclusionRuleDTO(dataRow);
                    membershipExclusionRuleDTOList.Add(membershipExclusionRuleDTO);
                }
            }
            log.LogMethodExit(membershipExclusionRuleDTOList);
            return membershipExclusionRuleDTOList;
        }

        /// <summary>
        /// Populates the MembersipExclusionRule using searchParameter
        /// </summary>
        /// <param name="searchParameters">Search Parameters</param>
        /// <returns>Returns MembershipExclusionRuleDTO List</returns>
        public List<MembershipExclusionRuleDTO> PopulateMembershipExclusionRuleList(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int productId = -1;
            int gameId = -1;
            int gameProfileId = -1;
            int site_Id = -1;
            string lastUpdatedBy = string.Empty;
            List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList = null;
            string selectQuery = @"select @Product_Id as Product_Id, @game_id as game_id, @game_profile_id as game_profile_id, MembershipId as MembershipID, 'Y' as Active, 'N' as DisAllowed, @site_id as site_id, @lastUpdatedBy as LastUpdatedBy, getdate() as LastUpdatedDate
                                                    from Membership m
                                                    where not exists (select 1 from cardTypeRule ct
                                                                      where isnull(ct.MembershipId, -1) = isnull(m.MembershipId, -1)
                                                                            and (product_id = @Product_Id
                                                                            or game_id = @game_id
                                                                            or game_profile_id = @game_profile_id))";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.PRODUCT_ID)).FirstOrDefault().Value != "0")
            {
                productId = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.PRODUCT_ID)).FirstOrDefault().Value);
            }
            else if (searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.GAME_ID)).FirstOrDefault().Value != "0")
            {
                gameId = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.GAME_ID)).FirstOrDefault().Value);
            }
            else if (searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.GAME_PROFILE_ID)).FirstOrDefault().Value != "0")
            {
                gameProfileId = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.GAME_PROFILE_ID)).FirstOrDefault().Value);
            }
            if (searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.LAST_UPDATED_BY)).FirstOrDefault().Value != string.Empty)
            {
                lastUpdatedBy = searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.LAST_UPDATED_BY)).FirstOrDefault().Value.ToString();
            }
            if (searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.SITE_ID)).FirstOrDefault().Value != string.Empty)
            {
                site_Id = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(MembershipExclusionRuleDTO.SearchByParameters.SITE_ID)).FirstOrDefault().Value);
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@Product_Id", productId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_id", gameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_profile_id", gameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", site_Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", lastUpdatedBy));

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                membershipExclusionRuleDTOList = new List<MembershipExclusionRuleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MembershipExclusionRuleDTO membershipExclusionRuleDTO = new MembershipExclusionRuleDTO();
                    membershipExclusionRuleDTO.ProductId = dataRow["Product_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Product_Id"]);
                    membershipExclusionRuleDTO.GameId = dataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_id"]);
                    membershipExclusionRuleDTO.GameProfileId = dataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["game_profile_id"]);
                    membershipExclusionRuleDTO.MembershipId = Convert.ToInt32(dataRow["MembershipID"]);
                    membershipExclusionRuleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                    membershipExclusionRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"].ToString();
                    membershipExclusionRuleDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                    membershipExclusionRuleDTO.IsActive = dataRow["Active"] == DBNull.Value ? true : (dataRow["Active"].ToString() == "Y" ? true : false);
                    membershipExclusionRuleDTO.Disallowed = dataRow["DisAllowed"] == DBNull.Value ? false : (dataRow["DisAllowed"].ToString() == "Y" ? true : false);
                    membershipExclusionRuleDTO.AcceptChanges();
                    membershipExclusionRuleDTOList.Add(membershipExclusionRuleDTO);
                }
            }
            log.LogMethodExit(membershipExclusionRuleDTOList);
            return membershipExclusionRuleDTOList;
        }
    }
}




















