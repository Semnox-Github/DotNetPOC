/********************************************************************************************
 * Project Name - Transaction
 * Description  - CheckInDetail Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        16-May-2019   Girish Kundar           Created 
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CheckInDetailDataHandler Data Handler - Handles insert, update and select of  CheckInDetails objects
    /// </summary>
    public class CheckInDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CheckInDetails As checkInDetails ";
        /// <summary>
        /// Dictionary for searching Parameters for the CheckInDetails object.
        /// </summary>
        private static readonly Dictionary<CheckInDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CheckInDetailDTO.SearchByParameters, string>
        {
            { CheckInDetailDTO.SearchByParameters.CHECK_IN_DETAIL_ID,"checkInDetails.CheckInDetailId"},
            { CheckInDetailDTO.SearchByParameters.CHECK_IN_ID,"checkInDetails.CheckInId"},
            { CheckInDetailDTO.SearchByParameters.CHECKIN_ID_LIST,"checkInDetails.CheckInId"},
            { CheckInDetailDTO.SearchByParameters.CARD_ID,"checkInDetails.CardId"},
            { CheckInDetailDTO.SearchByParameters.CHECK_OUT_TRX_ID,"checkInDetails.CheckOutTrxId"},
            { CheckInDetailDTO.SearchByParameters.TRX_LINE_ID,"checkInDetails.TrxLineId"},
            { CheckInDetailDTO.SearchByParameters.VEHICHLE_NUMBER,"checkInDetails.VehicleNumber"},
            { CheckInDetailDTO.SearchByParameters.SITE_ID,"checkInDetails.site_id"},
            { CheckInDetailDTO.SearchByParameters.MASTER_ENTITY_ID,"checkInDetails.MasterEntityId"},
            { CheckInDetailDTO.SearchByParameters.CHECK_IN_TRX_ID,"checkInDetails.CheckInTrxId"},
            { CheckInDetailDTO.SearchByParameters.CHECK_IN_TRX_LINE_ID,"checkInDetails.CheckInTrxLineId"},
            { CheckInDetailDTO.SearchByParameters.IS_ACTIVE,"checkInDetails.IsActive"},
            { CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS,"checkInDetails.Status"},
            { CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS_LIST,"checkInDetails.Status"},
            { CheckInDetailDTO.SearchByParameters.ACTIVE_CHECK_IN_DETAILS," (checkInDetails.CheckOutTime is null OR checkInDetails.CheckOutTime > getdate()) "}
        };

        /// <summary>
        /// Parameterized Constructor for CheckInDetailDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public CheckInDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CheckIns Record.
        /// </summary>
        /// <param name="checkInDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(CheckInDetailDTO checkInDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInDetailId", checkInDetailDTO.CheckInDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInId", checkInDetailDTO.CheckInId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", checkInDetailDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", checkInDetailDTO.TrxLineId == -1 ? (int?)null : checkInDetailDTO.TrxLineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckOutTrxId", checkInDetailDTO.CheckOutTrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Age", checkInDetailDTO.Age == 0 ? (decimal?)null : checkInDetailDTO.Age));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Allergies", checkInDetailDTO.Allergies));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckOutTime", checkInDetailDTO.CheckOutTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DateOfBirth", checkInDetailDTO.DateOfBirth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", checkInDetailDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", checkInDetailDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SpecialNeeds", checkInDetailDTO.SpecialNeeds));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VehicleColor", checkInDetailDTO.VehicleColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VehicleModel", checkInDetailDTO.VehicleModel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VehicleNumber", checkInDetailDTO.VehicleNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", checkInDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInTrxId", checkInDetailDTO.CheckInTrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInTrxLineId", checkInDetailDTO.CheckInTrxLineId == -1 ? (int?)null : checkInDetailDTO.CheckInTrxLineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInTime", checkInDetailDTO.CheckInTime == null ? (DateTime?)null : checkInDetailDTO.CheckInTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", CheckInStatusConverter.ToString(checkInDetailDTO.Status)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", checkInDetailDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to CheckInDetailDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>checkInDetailDTO</returns>
        private CheckInDetailDTO GetCheckInDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CheckInDetailDTO checkInDetailDTO = new CheckInDetailDTO( dataRow["CheckInDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInDetailId"]),
                                                         dataRow["CheckInId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInId"]),
                                                         dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                         dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                                         dataRow["VehicleNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VehicleNumber"]),
                                                         dataRow["VehicleModel"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VehicleModel"]),
                                                         dataRow["VehicleColor"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VehicleColor"]),
                                                         dataRow["DateOfBirth"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["DateOfBirth"]),
                                                         dataRow["Age"] == DBNull.Value ?  0 : Convert.ToDecimal(dataRow["Age"]),
                                                         dataRow["SpecialNeeds"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["SpecialNeeds"]),
                                                         dataRow["Allergies"] == DBNull.Value ?  string.Empty : Convert.ToString(dataRow["Allergies"]),
                                                         dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                         dataRow["CheckOutTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckOutTime"]),
                                                         dataRow["CheckOutTrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckOutTrxId"]),
                                                         dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]),
                                                         dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]),
                                                         dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["CheckInTrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInTrxId"]),
                                                         dataRow["CheckInTrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInTrxLineId"]),
                                                         null, 
                                                         dataRow["CheckInTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CheckInTime"]),
                                                         dataRow["Status"] == DBNull.Value ? CheckInStatus.PENDING : CheckInStatusConverter.FromString(Convert.ToString(dataRow["Status"])),
                                                         dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                        );
            log.LogMethodExit(checkInDetailDTO);
            return checkInDetailDTO;
        }

        /// <summary>
        /// Gets the CheckInDetail data of passed CheckInDetailId 
        /// </summary>
        /// <param name="checkInDetailId">integer type parameter</param>
        /// <returns>Returns CheckInDetailDTO</returns>
        public CheckInDetailDTO GetCheckInDetailDTO(int checkInDetailId)
        {
            log.LogMethodEntry(checkInDetailId);
            CheckInDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE checkInDetails.CheckInDetailId = @CheckInDetailId";
            SqlParameter parameter = new SqlParameter("@CheckInDetailId", checkInDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCheckInDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Updates the record to the CheckInDetail Table.
        /// </summary>
        /// <param name="checkInDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CheckInDetailDTO Update(CheckInDetailDTO checkInDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDetailDTO, loginId, siteId);
                                string query = @"UPDATE [dbo].[CheckInDetails]
                               SET
                                CheckInId = @CheckInId
                               ,Name = @Name
                               ,CardId = @CardId
                               ,VehicleNumber = @VehicleNumber
                               ,VehicleModel = @VehicleModel
                               ,VehicleColor = @VehicleColor
                               ,DateOfBirth = @DateOfBirth
                               ,Age = @Age
                               ,SpecialNeeds = @SpecialNeeds
                               ,Allergies = @Allergies
                               ,Remarks = @Remarks
                               ,CheckOutTime = @CheckOutTime
                               ,CheckOutTrxId = @CheckOutTrxId
                               ,last_update_date = GETDATE()
                               ,last_updated_user = @LastUpdatedBy
                               -- ,site_id = @site_id
                               ,TrxLineId = @TrxLineId
                               ,MasterEntityId = @MasterEntityId
                               ,CheckInTrxId = @CheckInTrxId
                               ,CheckInTrxLineId = @CheckInTrxLineId
                               ,CheckInTime = @CheckInTime
                               ,IsActive = @IsActive
                               ,Status = @Status
                                where CheckInDetailId = @CheckInDetailId
                                SELECT * FROM CheckInDetails WHERE CheckInDetailId = @CheckInDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCheckInDetailDTO(checkInDetailDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating CheckInDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkInDetailDTO);
            return checkInDetailDTO;
        }

        /// <summary>
        ///  Inserts the record to the CheckInDetail Table.
        /// </summary>
        /// <param name="checkInDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CheckInDetailDTO Insert(CheckInDetailDTO checkInDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDetailDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CheckInDetails]
                               (CheckInId
                               ,Name
                               ,CardId
                               ,VehicleNumber
                               ,VehicleModel
                               ,VehicleColor
                               ,DateOfBirth
                               ,Age
                               ,SpecialNeeds
                               ,Allergies
                               ,Remarks
                               ,CheckOutTime
                               ,CheckOutTrxId
                               ,last_update_date
                               ,last_updated_user
                               ,Guid
                               ,site_id
                               ,TrxLineId
                               ,MasterEntityId
                               ,CreatedBy
                               ,CreationDate
                               ,CheckInTrxId
                               ,CheckInTrxLineId
                               ,CheckInTime
                               ,Status
                               ,IsActive)
                         VALUES
                               (@CheckInId
                               ,@Name
                               ,@CardId
                               ,@VehicleNumber
                               ,@VehicleModel
                               ,@VehicleColor
                               ,@DateOfBirth
                               ,@Age
                               ,@SpecialNeeds
                               ,@Allergies
                               ,@Remarks
                               ,@CheckOutTime
                               ,@CheckOutTrxId
                               ,GETDATE()
                               ,@LastUpdatedBy
                               ,NEWID()
                               ,@site_id
                               ,@TrxLineId
                               ,@MasterEntityId
                               ,@CreatedBy
                               ,GETDATE()
                               ,@CheckInTrxId
                               ,@CheckInTrxLineId
                               ,@CheckInTime
                               ,@Status
                               ,@IsActive)
                                    SELECT * FROM CheckInDetails WHERE CheckInDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCheckInDetailDTO(checkInDetailDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting CheckInDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkInDetailDTO);
            return checkInDetailDTO;
        }

        private void RefreshCheckInDetailDTO(CheckInDetailDTO checkInDetailDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDetailDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                checkInDetailDTO.CheckInDetailId = Convert.ToInt32(dt.Rows[0]["CheckInDetailId"]);
                checkInDetailDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["last_update_date"]);
                checkInDetailDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                checkInDetailDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                checkInDetailDTO.LastUpdatedBy = Convert.ToString(dt.Rows[0]["last_updated_user"]);
                checkInDetailDTO.CreatedBy = Convert.ToString(dt.Rows[0]["createdBy"]);
                checkInDetailDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of CheckInDetailDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CheckInDetailDTO> GetAllCheckInDetailDTOList(List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CheckInDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ( searchParameter.Key == CheckInDetailDTO.SearchByParameters.CARD_ID 
                            ||searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECK_IN_DETAIL_ID
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECK_IN_ID
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECK_OUT_TRX_ID
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECK_IN_TRX_ID
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECK_IN_TRX_LINE_ID
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.TRX_LINE_ID
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CheckInDetailDTO.SearchByParameters.ACTIVE_CHECK_IN_DETAILS)
                        {
                            query.Append(joiner + " (checkInDetails.CheckOutTime is null OR checkInDetails.CheckOutTime > getdate()) ");
                        }
                        else if (searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECKIN_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CheckInDetailDTO.SearchByParameters.IS_ACTIVE)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == CheckInDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CheckInDetailDTO.SearchByParameters.NAME
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.VEHICHLE_NUMBER
                            || searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CheckInDetailDTO.SearchByParameters.CHECKIN_STATUS_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
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
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CheckInDetailDTO checkInDetailDTO = GetCheckInDetailDTO(dataRow);
                    checkInDetailDTOList.Add(checkInDetailDTO);
                }
            }
            log.LogMethodExit(checkInDetailDTOList);
            return checkInDetailDTOList;
        }
        /// <summary>
        /// Gets the List of CheckInDetail DTO
        /// </summary>
        /// <param name="checkInIdList"></param>
        /// <returns>checkInDetailDTOList</returns>
        public List<CheckInDetailDTO> GetAllCheckInDetailDTOList(List<int> checkInIdList)
        {
            log.LogMethodEntry(checkInIdList);
            string query = SELECT_QUERY + " INNER JOIN @CheckInIdList List ON checkInDetails.CheckInId = List.Id ";
            DataTable dataTable = dataAccessHandler.BatchSelect(query, "@CheckInIdList", checkInIdList, null, sqlTransaction);
            List<CheckInDetailDTO> checkInDetailDTOList = GetCheckInDetailDTOList(dataTable);
            log.LogMethodExit(checkInDetailDTOList);
            return checkInDetailDTOList;
        }
        private List<CheckInDetailDTO> GetCheckInDetailDTOList(DataTable dataTable)
        {
            log.LogMethodEntry(dataTable);
            List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CheckInDetailDTO checkInDetailDTO = GetCheckInDetailDTO(dataRow);
                    checkInDetailDTOList.Add(checkInDetailDTO);
                }
            }
            log.LogMethodExit(checkInDetailDTOList);
            return checkInDetailDTOList;
        }

    }
}

