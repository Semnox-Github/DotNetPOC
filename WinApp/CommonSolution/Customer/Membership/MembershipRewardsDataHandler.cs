/********************************************************************************************
 * Project Name - Membership
 * Description  - Get and Insert or update methods for MembershipRewards details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019   Girish Kundar          Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue 
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query      
 *2.130.3      16-Dec-2021    Abhishek              WMS fix : Added parameter loadActiveChildRecords to load active membership rewards 
 *2.140.1      20-Dec-2021    Abhishek              WMS fix : Added parameter loadActiveChildRecords to load active membership rewards 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    ///  MembershipRewards Data Handler - Handles insert, update and select of  MembershipRewards objects
    /// </summary>
    public class MembershipRewardsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private Utilities utilities;
        private const string SELECT_QUERY = @"SELECT * from MembershipRewards AS mr";
        private static readonly Dictionary<MembershipRewardsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MembershipRewardsDTO.SearchByParameters, string>
            {
                {MembershipRewardsDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID, "mr.MembershipRewardsId"},
                {MembershipRewardsDTO.SearchByParameters.MEMBERSHIP_ID, "mr.MembershipId"},
                {MembershipRewardsDTO.SearchByParameters.ISACTIVE, "mr.IsActive"},
                {MembershipRewardsDTO.SearchByParameters.MASTER_ENTITY_ID,"mr.MasterEntityId"},
                {MembershipRewardsDTO.SearchByParameters.SITE_ID, "mr.Site_id"}
            };


        /// <summary>
        /// Default constructor of MembershipRewardsDataHandler class
        /// </summary>
        public MembershipRewardsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MembershipRewards Record.
        /// </summary>
        /// <param name="membershipRewardsDTO">MembershipRewardsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MembershipRewardsDTO membershipRewardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipRewardsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@membershipRewardsId", membershipRewardsDTO.MembershipRewardsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@membershipID", membershipRewardsDTO.MembershipID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rewardProductID", membershipRewardsDTO.RewardProductID, true));
            parameters.Add(new SqlParameter("@rewardName", string.IsNullOrEmpty(membershipRewardsDTO.RewardName) ? string.Empty : (object)membershipRewardsDTO.RewardName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(membershipRewardsDTO.Description) ? DBNull.Value : (object)membershipRewardsDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rewardAttribute", string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute) ? DBNull.Value : (object)membershipRewardsDTO.RewardAttribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rewardFunction", string.IsNullOrEmpty(membershipRewardsDTO.RewardFunction) ? DBNull.Value : (object)membershipRewardsDTO.RewardFunction));
            parameters.Add(dataAccessHandler.GetSQLParameter("@unitOfRewardFunctionPeriod", string.IsNullOrEmpty(membershipRewardsDTO.UnitOfRewardFunctionPeriod) ? DBNull.Value : (object)membershipRewardsDTO.UnitOfRewardFunctionPeriod));
            parameters.Add(dataAccessHandler.GetSQLParameter("@unitOfRewardFrequency", string.IsNullOrEmpty(membershipRewardsDTO.UnitOfRewardFrequency) ? DBNull.Value : (object)membershipRewardsDTO.UnitOfRewardFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rewardAttributePercent", membershipRewardsDTO.RewardAttributePercent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rewardFunctionPeriod", membershipRewardsDTO.RewardFunctionPeriod == -1 ? DBNull.Value : (object)membershipRewardsDTO.RewardFunctionPeriod));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rewardFrequency", membershipRewardsDTO.RewardFrequency < 0 ? DBNull.Value : (object)membershipRewardsDTO.RewardFrequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", membershipRewardsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expireWithMembership", membershipRewardsDTO.ExpireWithMembership));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", membershipRewardsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }




        /// <summary>
        /// Inserts the MembershipRewards record to the database
        /// </summary>
        /// <param name="membershipRewardsDTO">MembershipRewardsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns MembershipRewardsDTO</returns>
        public MembershipRewardsDTO InsertMembershipRewards(MembershipRewardsDTO membershipRewardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipRewardsDTO, loginId, siteId);
            string InsertMembershipRewardsQuery = @"insert into MembershipRewards
                                                        (
                                                          RewardName,
                                                          Description,
                                                          MembershipID,
                                                          RewardProductID,
                                                          RewardAttribute,
                                                          RewardAttributePercent,
                                                          RewardFunction,
                                                          RewardFunctionPeriod,
                                                          UnitOfRewardFunctionPeriod,
                                                          RewardFrequency,
                                                          UnitOfRewardFrequency,
                                                          ExpireWithMembership,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastUpdatedDate,
                                                          Guid,
                                                          Site_id,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @rewardName,
                                                          @description,
                                                          @membershipID,
                                                          @rewardProductID,
                                                          @rewardAttribute,
                                                          @rewardAttributePercent,
                                                          @rewardFunction,
                                                          @rewardFunctionPeriod,
                                                          @unitOfRewardFunctionPeriod,
                                                          @rewardFrequency,
                                                          @unitOfRewardFrequency,
                                                          @expireWithMembership,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId
                                                        ) SELECT * FROM MembershipRewards WHERE MembershipRewardsId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertMembershipRewardsQuery, GetSQLParameters(membershipRewardsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipRewardsDTO(membershipRewardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting membershipRewardsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(membershipRewardsDTO);
            return membershipRewardsDTO;
        }

        /// <summary>
        /// Updates the MembershipRewards record
        /// </summary>
        /// <param name="membershipRewardsDTO">MembershipRewardsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns MembershipRewardsDTO</returns>
        public MembershipRewardsDTO UpdateMembershipRewards(MembershipRewardsDTO membershipRewardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipRewardsDTO, loginId, siteId);
            string updateMembershipRewardsQuery = @"update MembershipRewards 
                                                          set RewardName = @rewardName,
                                                          Description = @description,
                                                          MembershipID = @membershipID,
                                                          RewardProductID = @rewardProductID,
                                                          RewardAttribute = @rewardAttribute,
                                                          RewardAttributePercent = @rewardAttributePercent,
                                                          RewardFunction = @rewardFunction,
                                                          RewardFunctionPeriod = @rewardFunctionPeriod,
                                                          UnitOfRewardFunctionPeriod = @unitOfRewardFunctionPeriod,
                                                          RewardFrequency = @rewardFrequency,
                                                          UnitOfRewardFrequency = @unitOfRewardFrequency,
                                                          ExpireWithMembership = @expireWithMembership,
                                                          IsActive=@isActive,
                                                          LastUpdatedBy = @lastUpdatedBy, 
                                                          LastUpdatedDate = Getdate(),
                                                          --Site_id = @siteId,
                                                          MasterEntityId =  @masterEntityId
                                                          where MembershipRewardsId = @membershipRewardsId 
                   SELECT * FROM MembershipRewards WHERE MembershipRewardsId = @membershipRewardsId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMembershipRewardsQuery, GetSQLParameters(membershipRewardsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipRewardsDTO(membershipRewardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating membershipRewardsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(membershipRewardsDTO);
            return membershipRewardsDTO;

        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="membershipRewardsDTO">MembershipRewardsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshMembershipRewardsDTO(MembershipRewardsDTO membershipRewardsDTO, DataTable dt)
        {
            log.LogMethodEntry(membershipRewardsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                membershipRewardsDTO.MembershipRewardsId = Convert.ToInt32(dt.Rows[0]["MembershipRewardsId"]);
                membershipRewardsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                membershipRewardsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                membershipRewardsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                membershipRewardsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                membershipRewardsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                membershipRewardsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MembershipRewardsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns MembershipRewardsDTO</returns>
        private MembershipRewardsDTO GetMembershipRewardsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MembershipRewardsDTO membershipRewardsDTO = new MembershipRewardsDTO(Convert.ToInt32(dataRow["MembershipRewardsId"]),
                                            dataRow["RewardName"].ToString(),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                            dataRow["MembershipID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipID"]),
                                            dataRow["RewardProductID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RewardProductID"]),
                                            dataRow["RewardAttribute"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RewardAttribute"]),
                                            dataRow["RewardAttributePercent"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["RewardAttributePercent"]),
                                            dataRow["RewardFunction"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RewardFunction"]),
                                            dataRow["RewardFunctionPeriod"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["RewardFunctionPeriod"]),
                                            dataRow["UnitOfRewardFunctionPeriod"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfRewardFunctionPeriod"]),
                                            dataRow["RewardFrequency"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RewardFrequency"]),
                                            dataRow["UnitOfRewardFrequency"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfRewardFrequency"]),
                                            dataRow["ExpireWithMembership"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ExpireWithMembership"]),
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
            log.Debug(membershipRewardsDTO);
            return membershipRewardsDTO;
        }

        /// <summary>
        /// Gets the MembershipRewardsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of membershipRewardsDTO matching the search criteria</returns>
        public List<MembershipRewardsDTO> GetAllMembershipRewardsList(List<KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MembershipRewardsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(MembershipRewardsDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID) ||
                                searchParameter.Key.Equals(MembershipRewardsDTO.SearchByParameters.MEMBERSHIP_ID) ||
                                (searchParameter.Key == MembershipRewardsDTO.SearchByParameters.ISACTIVE) ||
                                searchParameter.Key.Equals(MembershipRewardsDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == MembershipRewardsDTO.SearchByParameters.SITE_ID)
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

            DataTable membershipRewardsData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (membershipRewardsData.Rows.Count > 0)
            {
                List<MembershipRewardsDTO> membershipRewardsList = new List<MembershipRewardsDTO>();
                foreach (DataRow membershipRewardsDataRow in membershipRewardsData.Rows)
                {
                    MembershipRewardsDTO membershipRewardsDataObject = GetMembershipRewardsDTO(membershipRewardsDataRow);
                    membershipRewardsList.Add(membershipRewardsDataObject);
                }
                log.LogMethodExit(membershipRewardsList);
                return membershipRewardsList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// Gets the MembershipRewards data of passed membershipId
        /// </summary>
        /// <param name="membershipId">integer type parameter</param>
        /// <returns>Returns List<MembershipRewardsDTO></MembershipRewardsDTO></returns>
        public List<MembershipRewardsDTO> GetMembershipRewardsByMembership(int membershipId, int siteId, bool loadActiveChildRecords = false)
        {
            log.LogMethodEntry(membershipId, siteId);
            List<KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>> membershipRewardsSearchParams = new List<KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>>();
            if (loadActiveChildRecords == true)
            {
                membershipRewardsSearchParams.Add(new KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>(MembershipRewardsDTO.SearchByParameters.ISACTIVE, "1"));
            }
            membershipRewardsSearchParams.Add(new KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>(MembershipRewardsDTO.SearchByParameters.MEMBERSHIP_ID, membershipId.ToString()));
            membershipRewardsSearchParams.Add(new KeyValuePair<MembershipRewardsDTO.SearchByParameters, string>(MembershipRewardsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<MembershipRewardsDTO> membershipRewardsDTOList = GetAllMembershipRewardsList(membershipRewardsSearchParams);
            log.LogMethodExit(membershipRewardsDTOList);
            return membershipRewardsDTOList;
        }

        /// <summary>
        /// Gets the MembershipRewards data of passed membershipRewards Id
        /// </summary>
        /// <param name="membershipRewardsId">integer type parameter</param>
        /// <returns>Returns MembershipRewardsDTO</returns>
        public MembershipRewardsDTO GetMembershipRewards(int membershipRewardsId)
        {
            log.LogMethodEntry();
            string selectMembershipRewardsQuery = SELECT_QUERY + "  WHERE mr.MembershipRewardsID = @membershipRewardsId";
            MembershipRewardsDTO membershipRewardsDataObject = null;
            SqlParameter[] selectMembershipRewardsParameters = new SqlParameter[1];
            selectMembershipRewardsParameters[0] = new SqlParameter("@membershipRewardsId", membershipRewardsId);
            DataTable membershipRewards = dataAccessHandler.executeSelectQuery(selectMembershipRewardsQuery, selectMembershipRewardsParameters, sqlTransaction);
            if (membershipRewards.Rows.Count > 0)
            {
                DataRow MembershipRewardsRow = membershipRewards.Rows[0];
                membershipRewardsDataObject = GetMembershipRewardsDTO(MembershipRewardsRow);
            }
            log.LogMethodExit(membershipRewardsDataObject);
            return membershipRewardsDataObject;
        }
    }
}
