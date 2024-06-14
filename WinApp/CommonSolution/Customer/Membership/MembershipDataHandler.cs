/********************************************************************************************
 * Project Name - Membership
 * Description  - Get and Insert or update methods for Membership details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019   Girish Kundar          Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query    
 *2.130.3      16-Dec-2021     Abhishek              WMS fix : Added parameters loadActiveChildRecords 
 *2.140.1      20-Dec-2021   Abhishek              WMS fix : Added parameter loadActiveChildRecords 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Customer.Membership
{
    /// <summaryUtilities 
    /// Membership Data Handler - Handles insert, update and select of  Membership objects
    /// </summary>
    public class MembershipDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private Utilities utilities;
        private const string SELECT_QUERY = @"SELECT * from Membership AS m";
        private static readonly Dictionary<MembershipDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MembershipDTO.SearchByParameters, string>
            {
                {MembershipDTO.SearchByParameters.MEMBERSHIP_ID, "m.MembershipID"},
                {MembershipDTO.SearchByParameters.IS_ACTIVE, "m.IsActive"},
                {MembershipDTO.SearchByParameters.MASTER_ENTITY_ID,"m.MasterEntityId"},
                {MembershipDTO.SearchByParameters.SITE_ID, "m.Site_id"}
            };


        /// <summary>
        /// Default constructor of MembershipDataHandler class
        /// </summary>
        public MembershipDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Membership Record.
        /// </summary>
        /// <param name="membershipDTO">MembershipDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MembershipDTO membershipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipDTO, loginId, siteId); 
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@membershipID", membershipDTO.MembershipID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@baseMembershipID", membershipDTO.BaseMembershipID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@membershipRuleID", membershipDTO.MembershipRuleID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@priceListId", membershipDTO.PriceListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vip", membershipDTO.VIP));
            parameters.Add(dataAccessHandler.GetSQLParameter("@autoApply", membershipDTO.AutoApply));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemptionDiscount", membershipDTO.RedemptionDiscount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", membershipDTO.IsActive));
            parameters.Add(new SqlParameter("@membershipName", string.IsNullOrEmpty(membershipDTO.MembershipName) ? string.Empty : (object)membershipDTO.MembershipName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(membershipDTO.Description) ? DBNull.Value : (object)membershipDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", membershipDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the membership record to the database
        /// </summary>
        /// <param name="membershipDTO">MembershipDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns MembershipDTO</returns>
        public MembershipDTO InsertMembership(MembershipDTO membershipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipDTO, loginId, siteId);
            string InsertMembershipQuery = @"insert into Membership
                                                        (
                                                          MembershipName,
                                                          Description,
                                                          VIP,
                                                          AutoApply,
                                                          BaseMembershipID,
                                                          MembershipRuleID,
                                                          RedemptionDiscount,
                                                          PriceListId,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          Guid,
                                                          Site_id,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @membershipName,
                                                          @description,
                                                          @vip,
                                                          @autoApply,
                                                          @baseMembershipID,
                                                          @membershipRuleID,
                                                          @redemptionDiscount,
                                                          @priceListId,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId
                                                        ) SELECT * FROM Membership WHERE MembershipID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertMembershipQuery, GetSQLParameters(membershipDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipDTO(membershipDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting membershipDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

            try
            {
                string checkForCircularDependency = @"  with n(MembershipID, BaseMembershipID, TierLevel) as
                                                          (select MembershipID, BaseMembershipID, 0 TierLevel
                                                             from membership
                                                           union all 
                                                           select m.MembershipID, m.BaseMembershipID, TierLevel + 1
                                                             from membership m, n
                                                            where m.MembershipID = n.BaseMembershipID
                                                           )
                                                           select MembershipID, BaseMembershipID from n";
                DataTable circularDependencyData = dataAccessHandler.executeSelectQuery(checkForCircularDependency, null, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                log.Error("Circular Dependency error", ex);
                throw new Exception("Unable to save the record, Circular Dependency error");
            }
            log.LogMethodExit(membershipDTO);
            return membershipDTO;
        }

        /// <summary>
        /// Updates the Membership record
        /// </summary>
        /// <param name="membershipDTO">MembershipDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns MembershipDTO</returns>
        public MembershipDTO UpdateMembership(MembershipDTO membershipDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipDTO, loginId, siteId);
            string updateMembershipQuery = @"update Membership 
                                                          set MembershipName = @membershipName,
                                                          Description = @description,
                                                          VIP = @vip,
                                                          AutoApply = @autoApply,
                                                          BaseMembershipID = @baseMembershipID,
                                                          MembershipRuleID =@membershipRuleID,
                                                          RedemptionDiscount =@redemptionDiscount,
                                                          PriceListId = @priceListId,
                                                          IsActive = @isActive,
                                                          LastUpdatedBy = @lastUpdatedBy, 
                                                          LastupdatedDate = Getdate(),
                                                         -- Site_id = @siteId,
                                                          MasterEntityId =  @masterEntityId
                                                          where MembershipID = @membershipID
                                            SELECT * FROM Membership WHERE MembershipID = @membershipID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMembershipQuery, GetSQLParameters(membershipDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipDTO(membershipDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating membershipDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            try
            {
                string checkForCircularDependency = @"  with n(MembershipID, BaseMembershipID, TierLevel) as
                                                          (select MembershipID, BaseMembershipID, 0 TierLevel
                                                             from membership
                                                           union all 
                                                           select m.MembershipID, m.BaseMembershipID, TierLevel + 1
                                                             from membership m, n
                                                            where m.MembershipID = n.BaseMembershipID
                                                           )
                                                           select MembershipID, BaseMembershipID from n";
                DataTable circularDependencyData = dataAccessHandler.executeSelectQuery(checkForCircularDependency, null, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                log.Error("Circular Dependency error", ex);
                throw new Exception("Unable to save the record, Circular Dependency error");
            }
            log.LogMethodExit(membershipDTO);
            return membershipDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="membershipDTO">MembershipDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshMembershipDTO(MembershipDTO membershipDTO, DataTable dt)
        {
            log.LogMethodEntry(membershipDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                membershipDTO.MembershipID = Convert.ToInt32(dt.Rows[0]["MembershipID"]);
                membershipDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                membershipDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                membershipDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                membershipDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                membershipDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                membershipDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to MembershipDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns MembershipDTO</returns>
        private MembershipDTO GetMembershipDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MembershipDTO membershipDTO = new MembershipDTO(Convert.ToInt32(dataRow["MembershipID"]),
                                            dataRow["MembershipName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MembershipName"]),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                            dataRow["VIP"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["VIP"]),
                                            dataRow["AutoApply"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["AutoApply"]),
                                            dataRow["BaseMembershipID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BaseMembershipID"]),
                                            dataRow["MembershipRuleID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipRuleID"]),
                                            dataRow["RedemptionDiscount"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["RedemptionDiscount"]),
                                            dataRow["PriceListId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PriceListId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"])
                                            );
            log.Debug(membershipDTO);
            return membershipDTO;
        }

        /// <summary>
        /// Gets the Membership data of passed membership Id
        /// </summary>
        /// <param name="membershipId">integer type parameter</param>
        /// <returns>Returns MembershipDTO</returns>
        public MembershipDTO GetMembership(int membershipId, int siteId)
        {
            log.LogMethodEntry(membershipId, siteId);
            string selectMembershipQuery = SELECT_QUERY + "    WHERE m.MembershipID = @membershipId";
            MembershipDTO membershipDataObject = null;
            SqlParameter[] selectMembershipParameters = new SqlParameter[1];
            selectMembershipParameters[0] = new SqlParameter("@membershipId", membershipId);
            DataTable membership = dataAccessHandler.executeSelectQuery(selectMembershipQuery, selectMembershipParameters, sqlTransaction);
            if (membership.Rows.Count > 0)
            {
                DataRow MembershipRow = membership.Rows[0];
                membershipDataObject = GetMembershipDTO(MembershipRow);
                MembershipRewardsDataHandler membershipRewardsDataHandler = new MembershipRewardsDataHandler(sqlTransaction);
                membershipDataObject.MembershipRewardsDTOList = membershipRewardsDataHandler.GetMembershipRewardsByMembership(membershipDataObject.MembershipID, siteId);
            }
            log.LogMethodExit(membershipDataObject);
            return membershipDataObject;

        }

        /// <summary>
        /// Gets the MembershipDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadAll"> to load complex DTO</param>
        /// <returns>Returns the list of membershipDTO matching the search criteria</returns>
        public List<MembershipDTO> GetAllMembershipList(List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters, bool loadAll, int siteId, bool loadActiveChildRecords = false)
        {
            log.LogMethodEntry(searchParameters, loadAll, siteId);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MembershipDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(MembershipDTO.SearchByParameters.MEMBERSHIP_ID) ||
                                searchParameter.Key.Equals(MembershipDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == MembershipDTO.SearchByParameters.IS_ACTIVE)
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ", 1) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == MembershipDTO.SearchByParameters.SITE_ID )
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
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable membershipData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (membershipData.Rows.Count > 0)
            {
                List<MembershipDTO> membershipList = new List<MembershipDTO>();
                foreach (DataRow membershipDataRow in membershipData.Rows)
                {
                    MembershipDTO membershipDataObject = GetMembershipDTO(membershipDataRow);
                    if (loadAll)
                    {
                        MembershipRuleDataHandler membershipRuleDataHandler = new MembershipRuleDataHandler();
                        membershipDataObject.MembershipRuleDTORecord = membershipRuleDataHandler.GetMembershipRule(membershipDataObject.MembershipRuleID);
                        MembershipRewardsDataHandler membershipRewardsDataHandler = new MembershipRewardsDataHandler(sqlTransaction);
                        membershipDataObject.MembershipRewardsDTOList = membershipRewardsDataHandler.GetMembershipRewardsByMembership(membershipDataObject.MembershipID, siteId, loadActiveChildRecords);
                    }
                    membershipList.Add(membershipDataObject);
                }
                log.LogMethodExit(membershipList);
                return membershipList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        internal DateTime? GetMemershipModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from games WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1) and machine_id is null
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
