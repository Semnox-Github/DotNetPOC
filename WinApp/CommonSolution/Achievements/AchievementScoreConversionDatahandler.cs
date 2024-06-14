
/********************************************************************************************
 * Project Name - Achievements
 * Description  - Bussiness logic of the   AchievementScoreConversion DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70        04-JUl-2019   Deeksha                 Modified :Added GetSqlParameter()  ,
 *                                                  SQL injection issue fix,
 *                                                  changed log.debug to log.logMethodEntry
 *                                                  and log.logMethodExit
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 *1.00        4-may-2017   Rakshith           Created 
 *2.80        27-Aug-2019   Vikas Dwivedi     Added SqlTransaction,
 *                                            Added SITEID for DBSearchParameter,
 *                                            Added SqlTransaction in Constructor as a parameter,
 *                                            Added SITEID in GetAchievementScoreConversionList(),
 *                                            Modified executeUpdateQuery, executeInsertQuery, 
 *                                            executeSelectQuery passed SqlTransaction,
 *                                            Added WHO columns
 *2.80        19-Nov-2019   Vikas Dwivedi     Added Logger Method                                      
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreConversionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AchievementScoreConversion AS amsc ";

        /// <summary>
        /// Dictionary for searching Parameters for the AchievementScoreConversion object.
        /// </summary>
        private static readonly Dictionary<AchievementScoreConversionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AchievementScoreConversionDTO.SearchByParameters, string>
            {
                {AchievementScoreConversionDTO.SearchByParameters.ID, "amsc.Id"},
                {AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID, "amsc.AchievementClassLevelId"},
                {AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST, "amsc.AchievementClassLevelId"},
                {AchievementScoreConversionDTO.SearchByParameters.IS_ACTIVE, "amsc.IsActive"},
                { AchievementScoreConversionDTO.SearchByParameters.MASTER_ENTITY_ID, "amsc.MasterentityId"}
         };
        /// <summary>
        /// Parameterized Constructor for AchievementScoreConversionDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public AchievementScoreConversionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AchievementScoreConversion Record.
        /// </summary>
        /// <param name="AchievementScoreConversion">AchievementScoreConversionDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(AchievementScoreConversionDTO achievementScoreConversionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementScoreConversionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", achievementScoreConversionDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", achievementScoreConversionDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToDate", achievementScoreConversionDTO.ToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", achievementScoreConversionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassLevelId", achievementScoreConversionDTO.AchievementClassLevelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Ratio", achievementScoreConversionDTO.Ratio));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConversionEntitlement", achievementScoreConversionDTO.ConversionEntitlement));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", achievementScoreConversionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to AchievementScoreConversionDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the AchievementScoreConversionDTO</returns>
        public AchievementScoreConversionDTO GetAchievementScoreConversionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AchievementScoreConversionDTO achievementScoreConversionDTO = new AchievementScoreConversionDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["FromDate"]),
                                                         dataRow["ToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ToDate"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["AchievementClassLevelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementClassLevelId"]),
                                                         dataRow["Ratio"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["Ratio"]),
                                                         dataRow["ConversionEntitlement"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ConversionEntitlement"]),
                                                         dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                         dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdateduser"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                        );
            log.LogMethodExit(achievementScoreConversionDTO);
            return achievementScoreConversionDTO;
        }

        /// <summary>
        /// Inserts the AchievementScoreConversionDTO record to the database
        /// </summary>
        /// <param name="AchievementScoreConversionDTO">AchievementScoreConversionDTO type object</param>
        /// <returns>Returns inserted record id</returns>
        public AchievementScoreConversionDTO InsertAchievementScoreConversion(AchievementScoreConversionDTO achievementScoreConversionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementScoreConversionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[AchievementScoreConversion]
                                                         (
                                                            FromDate
                                                            ,ToDate
                                                            ,IsActive
                                                            ,AchievementClassLevelId
                                                            ,Ratio
                                                            ,ConversionEntitlement
                                                            ,LastUpdatedDate
                                                            ,LastUpdatedUser
                                                            ,Guid
                                                            ,MasterEntityId
                                                            ,site_id
                                                            ,CreatedBy
                                                            ,CreationDate
                                                         )
                                                       values
                                                         (
                                                             @FromDate
                                                            ,@ToDate
                                                            ,@isActive
                                                            ,@AchievementClassLevelId
                                                            ,@Ratio
                                                            ,@ConversionEntitlement
                                                            ,GetDate()
                                                            ,@LastUpdatedUser
                                                            ,NewId()
                                                            ,@MasterEntityId
                                                            ,@site_id 
                                                            ,@CreatedBy
                                                            ,GETDATE())
                                                        SELECT* FROM AchievementScoreConversion WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementScoreConversionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementScoreConversionDTO(achievementScoreConversionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting AchievementScoreConversionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementScoreConversionDTO);
            return achievementScoreConversionDTO;
        }

        /// <summary>
        ///  Updates the record to the AchievementScoreConversion Table.
        /// </summary>
        /// <param name="AchievementScoreConversionDTO">AchievementScoreConversionDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the AchievementScoreConversionDTO</returns>
        public AchievementScoreConversionDTO UpdateAchievementScoreConversion(AchievementScoreConversionDTO achievementScoreConversionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(achievementScoreConversionDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[AchievementScoreConversion]
                           SET 
                                                             FromDate = @FromDate
                                                            ,ToDate = @ToDate
                                                            ,IsActive = @isActive
                                                            ,AchievementClassLevelId = @AchievementClassLevelId
                                                            ,Ratio = @Ratio
                                                            ,ConversionEntitlement = @ConversionEntitlement
                                                            ,LastUpdatedDate = GetDate()
                                                            ,LastUpdatedUser = @LastUpdatedUser                                                         
                                                            ,MasterEntityId = @MasterEntityId
                                                            --,site_id = @site_id
                                                      WHERE Id =@Id 
                                    SELECT * FROM AchievementScoreConversion WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(achievementScoreConversionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAchievementScoreConversionDTO(achievementScoreConversionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating AchievementScoreConversionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(achievementScoreConversionDTO);
            return achievementScoreConversionDTO;
        }

        /// <summary>
        /// Delete the record from the AchievementScoreConversion database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        internal int Delete(int Id)
        {
            log.LogMethodEntry(Id);
            string query = @"DELETE  
                             FROM AchievementScoreConversion
                             WHERE AchievementScoreConversion.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", Id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Gets the AchievementScoreConversionDTO data of passed id
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Returns AchievementScoreConversionDTO object</returns>
        public AchievementScoreConversionDTO GetAchievementScoreConversionDTO(int id)
        {
            log.LogMethodEntry(id);
            AchievementScoreConversionDTO result = null;
            string query = SELECT_QUERY + @" WHERE amsc.Id= @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAchievementScoreConversionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="achievementScoreConversionDTO">AchievementScoreConversionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshAchievementScoreConversionDTO(AchievementScoreConversionDTO achievementScoreConversionDTO, DataTable dt)
        {
            log.LogMethodEntry(achievementScoreConversionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                achievementScoreConversionDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                achievementScoreConversionDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                achievementScoreConversionDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                achievementScoreConversionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                achievementScoreConversionDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                achievementScoreConversionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                achievementScoreConversionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the AchievementScoreConversionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AchievementScoreConversionDTO matching the search criteria</returns>
        public List<AchievementScoreConversionDTO> GetAchievementScoreConversionList(List<KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectAchievementScoreConversionQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AchievementScoreConversionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID) ||
                                      searchParameter.Key.Equals(AchievementScoreConversionDTO.SearchByParameters.ID) 
                                      /*|| searchParameter.Key.Equals(AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID)*/)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(AchievementScoreConversionDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(AchievementScoreConversionDTO.SearchByParameters.SITEID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  '" + searchParameter.Value + "')");
                        }
                        else if (searchParameter.Key == AchievementScoreConversionDTO.SearchByParameters.ACHIEVEMENT_CLASS_LEVEL_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                    selectAchievementScoreConversionQuery = selectAchievementScoreConversionQuery + query;
                selectAchievementScoreConversionQuery = selectAchievementScoreConversionQuery + " Order by Id";
            }
            List<AchievementScoreConversionDTO> AchievementScoreConversionList = new List<AchievementScoreConversionDTO>();
            DataTable AchievementScoreConversionData = dataAccessHandler.executeSelectQuery(selectAchievementScoreConversionQuery, parameters.ToArray(), sqlTransaction);
            if (AchievementScoreConversionData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in AchievementScoreConversionData.Rows)
                {
                    AchievementScoreConversionDTO achievementProjectDataObject = GetAchievementScoreConversionDTO(dataRow);
                    AchievementScoreConversionList.Add(achievementProjectDataObject);
                }
                log.LogMethodExit(AchievementScoreConversionList);
            }
            return AchievementScoreConversionList;
        }

        ///// <summary>
        ///// Get AchievementScoreConversion based on the list of AchievementClassLevel Id.
        ///// </summary>
        ///// <param name="achievementClassLevelIdList">achievementClassLevelIdList holds the AchievementClassLevel Id list</param>
        ///// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        ///// <returns>Returns the List of AchievementScoreConversionDTO</returns>
        //public List<AchievementScoreConversionDTO> GetAchievementScoreConversionList(List<int> achievementClassLevelIdList, bool activeChildRecords)
        //{
        //    log.LogMethodEntry(achievementClassLevelIdList, activeChildRecords);
        //    string query = SELECT_QUERY + " INNER JOIN @AchievementClassLevelIdList List ON amsc.AchievementClassLevelId = List.Id ";
        //    if (activeChildRecords)
        //    {
        //        query += " AND ActiveFlag = 1 ";
        //    }
        //    DataTable dataTable = dataAccessHandler.BatchSelect(query, "@AchievementClassLevelIdList", achievementClassLevelIdList, null, sqlTransaction);
        //    List<AchievementScoreConversionDTO> achievementScoreConversionsList = GetAchievementScoreConversionDTOList(dataTable);
        //    log.LogMethodExit(achievementScoreConversionsList);
        //    return achievementScoreConversionsList;
        //}

        ///// <summary>
        ///// Returns the List AchievementScoreConversionDTO from the DataTable object.
        ///// </summary>
        ///// <param name="dataTable">dataTable object of DataTable is passed as parameter.</param>
        ///// <returns>Returns the List of AchievementScoreConversionDTO </returns>
        //private List<AchievementScoreConversionDTO> GetAchievementScoreConversionDTOList(DataTable dataTable)
        //{
        //    log.LogMethodEntry(dataTable);
        //    List<AchievementScoreConversionDTO> achievemetScoreConversionsList = new List<AchievementScoreConversionDTO>();
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            AchievementScoreConversionDTO achievementScoreConversionDTO = GetAchievementScoreConversionDTO(dataRow);
        //            achievemetScoreConversionsList.Add(achievementScoreConversionDTO);
        //        }
        //    }
        //    log.LogMethodExit(achievemetScoreConversionsList);
        //    return achievemetScoreConversionsList;
        //}

        /// <summary>
        /// get the record LoyaltyAttributesDTO list  
        /// </summary>
        /// <returns>return the list of LoyaltyAttributesDTO </returns>
        public List<LoyaltyAttributesDTO> GetLoyaltyAttributes()
        {
            log.LogMethodEntry();
            try
            {
                string loyaltyAttributesDTOQuery = @"select * from LoyaltyAttributes
                                                    where CreditPlusType is not null";

                //SqlParameter[] loyaltyAttributesDTOParameters = new SqlParameter[1];
                //loyaltyAttributesDTOParameters[0] = new SqlParameter("@id", id);

                DataTable dtLoyaltyAttributes = dataAccessHandler.executeSelectQuery(loyaltyAttributesDTOQuery, null, sqlTransaction);

                List<LoyaltyAttributesDTO> LoyaltyAttributesDTOList = new List<LoyaltyAttributesDTO>();
                if (dtLoyaltyAttributes.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dtLoyaltyAttributes.Rows)
                    {
                        LoyaltyAttributesDTO loyaltyAttributesDTO = GetAloyaltyAttributesDTO(dataRow);
                        LoyaltyAttributesDTOList.Add(loyaltyAttributesDTO);
                    }


                }
                log.LogMethodExit(LoyaltyAttributesDTOList);
                return LoyaltyAttributesDTOList;
            }
            catch (Exception ex)
            {
                string message = "Exception at GetLoyaltyAttributes()";
                log.LogMethodExit(null, "Throwing exception -" + ex.Message + ":" + message);
                throw new Exception(message);
            }
        }
        public LoyaltyAttributesDTO GetAloyaltyAttributesDTO(DataRow loyaltyAttributesDataRow)
        {
            log.LogMethodEntry(loyaltyAttributesDataRow);
            LoyaltyAttributesDTO loyaltyAttributesDTO = new LoyaltyAttributesDTO(
            string.IsNullOrEmpty(loyaltyAttributesDataRow["LoyaltyAttributeId"].ToString()) ? -1 : Convert.ToInt32(loyaltyAttributesDataRow["LoyaltyAttributeId"]),
            loyaltyAttributesDataRow["Attribute"].ToString(),
            loyaltyAttributesDataRow["PurchaseApplicable"].ToString(),
            loyaltyAttributesDataRow["ConsumptionApplicable"].ToString(),
            loyaltyAttributesDataRow["DBColumnName"].ToString(),
            loyaltyAttributesDataRow["CreditPlusType"].ToString(),
            loyaltyAttributesDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(loyaltyAttributesDataRow["LastupdatedDate"]),
            loyaltyAttributesDataRow["LastUpdatedBy"].ToString(),
            loyaltyAttributesDataRow["Guid"].ToString(),
            loyaltyAttributesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(loyaltyAttributesDataRow["SynchStatus"]),
            loyaltyAttributesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(loyaltyAttributesDataRow["MasterEntityId"]),
            loyaltyAttributesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(loyaltyAttributesDataRow["site_id"]),
            loyaltyAttributesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(loyaltyAttributesDataRow["CreationDate"]),
            loyaltyAttributesDataRow["CreatedBy"].ToString());
            log.LogMethodExit(loyaltyAttributesDTO);
            return loyaltyAttributesDTO;
        }
    }
}
