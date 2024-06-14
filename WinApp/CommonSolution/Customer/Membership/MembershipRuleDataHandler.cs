/********************************************************************************************
 * Project Name - Membership
 * Description  - Get and Insert or update methods for MembershipRule details.
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019   Girish Kundar          Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
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
    ///  MembershipRule Data Handler - Handles insert, update and select of  MembershipRule objects
    /// </summary>
    public class MembershipRuleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private Utilities utilities;
        private const string SELECT_QUERY = @"SELECT * from MembershipRule AS mrl";

        private static readonly Dictionary<MembershipRuleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MembershipRuleDTO.SearchByParameters, string>
            {
                {MembershipRuleDTO.SearchByParameters.MEMBERSHIP_RULE_ID, "mrl.MembershipRuleId"},
                {MembershipRuleDTO.SearchByParameters.ISACTIVE, "mrl.IsActive"},
                {MembershipRuleDTO.SearchByParameters.MASTER_ENTITY_ID,"mrl.MasterEntityId"},
                {MembershipRuleDTO.SearchByParameters.SITE_ID, "mrl.site_Id"}
            };

        /// <summary>
        /// Default constructor of MembershipRuleDataHandler class
        /// </summary>
        public MembershipRuleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MembershipRule Record.
        /// </summary>
        /// <param name="membershipRuleDTO">MembershipRuleDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MembershipRuleDTO membershipRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipRuleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@membershipRuleID", membershipRuleDTO.MembershipRuleID, true));
            parameters.Add(new SqlParameter("@ruleName", string.IsNullOrEmpty(membershipRuleDTO.RuleName) ? string.Empty : (object)membershipRuleDTO.RuleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(membershipRuleDTO.Description) ? DBNull.Value : (object)membershipRuleDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@qualifyingPoints", (membershipRuleDTO.QualifyingPoints < 0) ? 0 : membershipRuleDTO.QualifyingPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@qualificationWindow", (membershipRuleDTO.QualificationWindow < 0) ? 0 : membershipRuleDTO.QualificationWindow));
            parameters.Add(dataAccessHandler.GetSQLParameter("@retentionPoints", (membershipRuleDTO.RetentionPoints < 0) ? 0 : membershipRuleDTO.RetentionPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@retentionWindow", (membershipRuleDTO.RetentionWindow < 0) ? 0 : membershipRuleDTO.RetentionWindow));
            parameters.Add(dataAccessHandler.GetSQLParameter("@unitOfQualificationWindow", string.IsNullOrEmpty(membershipRuleDTO.UnitOfQualificationWindow) ? DBNull.Value : (object)membershipRuleDTO.UnitOfQualificationWindow));
            parameters.Add(dataAccessHandler.GetSQLParameter("@unitOfRetentionWindow", string.IsNullOrEmpty(membershipRuleDTO.UnitOfRetentionWindow) ? DBNull.Value : (object)membershipRuleDTO.UnitOfRetentionWindow));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", membershipRuleDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", membershipRuleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the membershipRule record to the database
        /// </summary>
        /// <param name="membershipRuleDTO">MembershipRuleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns MembershipRuleDTO</returns>
        public MembershipRuleDTO InsertMembershipRule(MembershipRuleDTO membershipRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipRuleDTO, loginId, siteId);
            string InsertmembershipRuleSetupQuery = @"insert into MembershipRule 
                                                        (
                                                          RuleName,
                                                          Description,
                                                          QualifyingPoints,
                                                          QualificationWindow,
                                                          UnitOfQualificationWindow,
                                                          RetentionPoints,
                                                          RetentionWindow,
                                                          UnitOfRetentionWindow,
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
                                                          @ruleName,
                                                          @description,
                                                          @qualifyingPoints,
                                                          @qualificationWindow,
                                                          @unitOfQualificationWindow,
                                                          @retentionPoints,
                                                          @retentionWindow,
                                                          @unitOfRetentionWindow,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteid,
                                                          @masterEntityId
                                                        ) SELECT * FROM MembershipRule WHERE MembershipRuleID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertmembershipRuleSetupQuery, GetSQLParameters(membershipRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipRuleDTO(membershipRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting membershipRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(membershipRuleDTO);
            return membershipRuleDTO;
        }

        /// <summary>
        /// Updates the MembershipRule record
        /// </summary>
        /// <param name="membershipRuleDTO">MembershipRuleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the MembershipRuleDTO</returns>
        public MembershipRuleDTO UpdateMembershipRule(MembershipRuleDTO membershipRuleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(membershipRuleDTO, loginId, siteId);
            string updateMembershipRuleQuery = @"update MembershipRule 
                                                          set RuleName = @ruleName,
                                                          Description = @description,
                                                          QualifyingPoints = @qualifyingPoints,
                                                          QualificationWindow = @qualificationWindow,
                                                          UnitOfQualificationWindow = @unitOfQualificationWindow,
                                                          RetentionPoints =@retentionPoints,
                                                          RetentionWindow =@retentionWindow,
                                                          UnitOfRetentionWindow = @unitOfRetentionWindow,
                                                          IsActive = @isActive,
                                                          LastUpdatedBy = @lastUpdatedBy, 
                                                          LastupdatedDate = Getdate(),
                                                          --site_Id = @siteId,
                                                          MasterEntityId =  @masterEntityId
                                                          where MembershipRuleID = @membershipRuleID
                                SELECT * FROM MembershipRule WHERE MembershipRuleID = @membershipRuleID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMembershipRuleQuery, GetSQLParameters(membershipRuleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMembershipRuleDTO(membershipRuleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating membershipRuleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(membershipRuleDTO);
            return membershipRuleDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="membershipRuleDTO">MembershipRuleDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshMembershipRuleDTO(MembershipRuleDTO membershipRuleDTO, DataTable dt)
        {
            log.LogMethodEntry(membershipRuleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                membershipRuleDTO.MembershipRuleID = Convert.ToInt32(dt.Rows[0]["MembershipRuleID"]);
                membershipRuleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                membershipRuleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                membershipRuleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                membershipRuleDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                membershipRuleDTO.SiteId = dataRow["site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_Id"]);
                membershipRuleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to MembershipRuleDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns MembershipRuleDTO</returns>
        private MembershipRuleDTO GetMembershipRuleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MembershipRuleDTO membershipRuleDTO = new MembershipRuleDTO(Convert.ToInt32(dataRow["MembershipRuleId"]),
                                            dataRow["RuleName"].ToString(),
                                            dataRow["Description"].ToString(),
                                            dataRow["QualifyingPoints"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["QualifyingPoints"]),
                                            dataRow["QualificationWindow"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["QualificationWindow"]),
                                            dataRow["UnitOfQualificationWindow"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfQualificationWindow"]),
                                            dataRow["RetentionPoints"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RetentionPoints"]),
                                            dataRow["RetentionWindow"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RetentionWindow"]),
                                            dataRow["UnitOfRetentionWindow"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["UnitOfRetentionWindow"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_Id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.Debug(membershipRuleDTO);
            return membershipRuleDTO;
        }
        /// <summary>
        /// Gets the membershipRule detail which matches with the passed membershipRule id 
        /// </summary>
        /// <param name="membershipRuleId">integer type parameter</param>
        /// <returns>Returns MembershipRulesDTO</returns>
        public MembershipRuleDTO GetMembershipRule(int membershipRuleId)
        {
            log.LogMethodEntry();
            MembershipRuleDTO membershipRuleDataObject = null;
            string selectMembershipRulesQuery = SELECT_QUERY + "    WHERE mrl.MembershipRuleId = @membershipRuleId ";
            SqlParameter[] selectMembershipRulesParameters = new SqlParameter[1];
            selectMembershipRulesParameters[0] = new SqlParameter("@membershipRuleId", membershipRuleId);
            DataTable membershipRuleDataTable = dataAccessHandler.executeSelectQuery(selectMembershipRulesQuery, selectMembershipRulesParameters, sqlTransaction);
            if (membershipRuleDataTable.Rows.Count > 0)
            {
                DataRow membershipRulesRow = membershipRuleDataTable.Rows[0];
                membershipRuleDataObject = GetMembershipRuleDTO(membershipRulesRow);
            }
            log.LogVariableState("MembershipRuleDataObject", membershipRuleDataObject);
            log.LogMethodExit(membershipRuleDataObject);
            return membershipRuleDataObject;
        }


        /// <summary>
        /// Gets the MembershipRuleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of membershipRuleDTO matching the search criteria</returns>
        public List<MembershipRuleDTO> GetAllMembershipRuleList(List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int count = 0;
            string selectProductQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MembershipRuleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(MembershipRuleDTO.SearchByParameters.MEMBERSHIP_RULE_ID)
                                || searchParameter.Key.Equals(MembershipRuleDTO.SearchByParameters.MASTER_ENTITY_ID)
                                || searchParameter.Key.Equals(MembershipRuleDTO.SearchByParameters.ISACTIVE))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }

                            else if (searchParameter.Key == MembershipRuleDTO.SearchByParameters.SITE_ID)
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
                    selectProductQuery = selectProductQuery + query;
            }

            DataTable membershipRuleData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (membershipRuleData.Rows.Count > 0)
            {
                List<MembershipRuleDTO> membershipRuleList = new List<MembershipRuleDTO>();
                foreach (DataRow membershipRuleDataRow in membershipRuleData.Rows)
                {
                    MembershipRuleDTO membershipRuleDataObject = GetMembershipRuleDTO(membershipRuleDataRow);
                    membershipRuleList.Add(membershipRuleDataObject);
                }
                log.LogMethodExit(membershipRuleList);
                return membershipRuleList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
    }
}
