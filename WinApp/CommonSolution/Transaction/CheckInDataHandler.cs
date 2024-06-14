/********************************************************************************************
 * Project Name - Transaction
 * Description  - CheckIn Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        30-May-2019   Girish Kundar           Created
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CheckInDataHandler Data Handler - Handles insert, update and select of  CheckIns objects
    /// </summary>
    public class CheckInDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CheckIns AS checkIns";
        /// <summary>
        /// Dictionary for searching Parameters for the CheckIns object.
        /// </summary>
        private static readonly Dictionary<CheckInDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CheckInDTO.SearchByParameters, string>
        {
            { CheckInDTO.SearchByParameters.CHECK_IN_ID,"checkIns.CheckInId"},
            { CheckInDTO.SearchByParameters.CHECK_IN_FACILITY_ID,"checkIns.CheckInFacilityId"},
            { CheckInDTO.SearchByParameters.CHECK_IN_TRX_ID,"checkIns.CheckInTrxId"},
            { CheckInDTO.SearchByParameters.CUSTOMER_ID,"checkIns.Customer_Id"},
            { CheckInDTO.SearchByParameters.CARD_ID,"checkIns.CardId"},
            { CheckInDTO.SearchByParameters.TABLE_ID,"checkIns.TableId"},
            { CheckInDTO.SearchByParameters.TRX_LINE_ID,"checkIns.TrxLineId"},
            { CheckInDTO.SearchByParameters.SITE_ID,"checkIns.site_id"},
            { CheckInDTO.SearchByParameters.IS_ACTIVE,"checkIns.IsActive"},
            { CheckInDTO.SearchByParameters.MASTER_ENTITY_ID,"checkIns.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for CheckInDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public CheckInDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CheckIns Record.
        /// </summary>
        /// <param name="checkInDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(CheckInDTO checkInDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInId", checkInDTO.CheckInId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInTrxId", checkInDTO.CheckInTrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInFacilityId", checkInDTO.CheckInFacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", checkInDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", checkInDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableId", checkInDTO.TableId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxLineId", checkInDTO.TrxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AllowedTimeInMinutes", checkInDTO.AllowedTimeInMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CheckInTime", checkInDTO.CheckInTime));
            SqlParameter parameter = new SqlParameter("@FingerPrint", SqlDbType.Image);
            if (checkInDTO.FingerPrint == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = checkInDTO.FingerPrint;
            }
            parameters.Add(parameter);
            //parameters.Add(dataAccessHandler.GetSQLParameter("@FingerPrint", checkInDTO.FingerPrint));
            SqlParameter FPparameter = new SqlParameter("@FPTemplate", SqlDbType.VarBinary);
            if (checkInDTO.FPTemplate == null)
            {
                FPparameter.Value = DBNull.Value;
            }
            else
            {
                FPparameter.Value = checkInDTO.FPTemplate;
            }
            parameters.Add(FPparameter);
            //parameters.Add(dataAccessHandler.GetSQLParameter("@FPTemplate", checkInDTO.FPTemplate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PhotoFileName", checkInDTO.PhotoFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", checkInDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", checkInDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private CheckInDTO GetCheckInDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CheckInDTO checkInDTO = new CheckInDTO(dataRow["CheckInId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInId"]),
                                       dataRow["Customer_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Customer_Id"]),
                                       dataRow["CheckInTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CheckInTime"]),
                                       dataRow["PhotoFileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PhotoFileName"].ToString()),
                                       dataRow["Finger_Print"] == DBNull.Value ? null : dataRow["Finger_Print"] as byte[],
                                       dataRow["FP_Template"] == DBNull.Value ? null : dataRow["FP_Template"] as byte[],
                                       dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                       dataRow["CheckInTrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInTrxId"]),
                                       dataRow["AllowedTimeInMinutes"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AllowedTimeInMinutes"].ToString()),
                                       dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]),
                                       dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]),
                                       dataRow["CheckInFacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CheckInFacilityId"]),
                                       dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                       dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                       dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                       dataRow["TableId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TableId"]),
                                       dataRow["TrxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxLineId"]),
                                       null,
                                       dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                       dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                       dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                       dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                      );
            log.LogMethodExit(checkInDTO);
            return checkInDTO;
        }

        /// <summary>
        /// Gets the CheckIn data of passed checkInId 
        /// </summary>
        /// <param name="checkInId">integer type parameter</param>
        /// <returns>Returns CheckInDTO</returns>
        public CheckInDTO GetCheckIn(int checkInId)
        {
            log.LogMethodEntry(checkInId);
            CheckInDTO result = null;
            string query = SELECT_QUERY + @" WHERE checkIns.CheckInId = @CheckInId";
            SqlParameter parameter = new SqlParameter("@CheckInId", checkInId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCheckInDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Inserts the record to CheckIns Table
        /// </summary>
        /// <param name="checkInDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CheckInDTO Insert(CheckInDTO checkInDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CheckIns]
                           (Customer_Id
                           ,CheckInTime
                           ,PhotoFileName
                           ,Finger_Print
                           ,FP_Template
                           ,CardId
                           ,CheckInTrxId
                           ,AllowedTimeInMinutes
                           ,last_update_date
                           ,last_updated_user
                           ,CheckInFacilityId
                           ,Guid
                           ,site_id
                           ,TableId
                           ,TrxLineId
                           ,MasterEntityId
                           ,CreatedBy
                           ,CreationDate
                           ,IsActive
                           )
                     VALUES
                           (@CustomerId
                           ,@CheckInTime
                           ,@PhotoFileName
                           ,@FingerPrint
                           ,@FPTemplate
                           ,@CardId
                           ,@CheckInTrxId
                           ,@AllowedTimeInMinutes
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,@CheckInFacilityId
                           ,NEWID()
                           ,@site_id
                           ,@TableId
                           ,@TrxLineId
                           ,@MasterEntityId
                           ,@CreatedBy
                           ,GETDATE()
                           ,@IsActive
                             )
                                SELECT * FROM CheckIns WHERE CheckInId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCheckInDTO(checkInDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting CheckInDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkInDTO);
            return checkInDTO;
        }

        /// <summary>
        /// Update the record to CheckIns Table
        /// </summary>
        /// <param name="checkInDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CheckInDTO Update(CheckInDTO checkInDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CheckIns]
                            SET 
                            Customer_Id = @CustomerId
                           ,CheckInTime = @CheckInTime
                           ,PhotoFileName = @PhotoFileName
                           ,Finger_Print = @FingerPrint
                           ,FP_Template = @FPTemplate
                           ,CardId = @CardId
                           ,CheckInTrxId = @CheckInTrxId
                           ,AllowedTimeInMinutes = @AllowedTimeInMinutes
                           ,last_update_date = GETDATE()
                           ,last_updated_user = @LastUpdatedBy
                           ,CheckInFacilityId = @CheckInFacilityId
                           -- ,site_id = @site_id
                           ,TableId = @TableId
                           ,TrxLineId = @TrxLineId
                           ,IsActive = @IsActive
                           ,MasterEntityId = @MasterEntityId
                            Where CheckInId = @CheckInId
                            SELECT * FROM CheckIns WHERE CheckInId = @CheckInId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(checkInDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCheckInDTO(checkInDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting CheckInDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(checkInDTO);
            return checkInDTO;
        }

        /// <summary>
        /// Get total checked in for a facility
        /// </summary>
        /// <param name="pCheckInFacilityId">Facility Id</param>
        /// <returns>Total Checked In Count</returns>
        public int GetTotalCheckedInForFacility(int pCheckInFacilityId)
        {
            log.LogMethodEntry(pCheckInFacilityId);

            string totalCheckedInQuery = @"select count(*) from CheckIns h, checkInDetails d
                                           where h.checkInId = d.CheckInId
                                             and CheckInFacilityId = @CheckInFacilityId
                                             and (CheckOutTime is null or CheckOutTime > getdate())";
            SqlParameter[] checkedInParams = new SqlParameter[1];
            checkedInParams[0] = new SqlParameter("@CheckInFacilityId", pCheckInFacilityId);
            object oCheckedIn = dataAccessHandler.executeScalar(totalCheckedInQuery, checkedInParams, sqlTransaction);
            if (oCheckedIn != null)
            {
                int checkedIn = Convert.ToInt32(oCheckedIn);
                return checkedIn;
            }
            else
                return 0;
        }

        /// <summary>
        /// Get total checked in for a facility
        /// </summary>
        /// <param name="pCheckInFacilityId">Facility Id</param>
        /// <returns>Total Checked In Count</returns>
        public int GetTotalCheckedInForTable(int pCheckInTableId)
        {
            log.LogMethodEntry(pCheckInTableId);

            string totalCheckedInQuery = @"select count(*) from CheckIns h, checkInDetails d
                                           where h.checkInId = d.CheckInId
                                             and tableId = @CheckInTableId
                                             and (CheckOutTime is null or CheckOutTime > getdate())";
            SqlParameter[] checkedInParams = new SqlParameter[1];
            checkedInParams[0] = new SqlParameter("@CheckInTableId", pCheckInTableId);
            object oCheckedIn = dataAccessHandler.executeScalar(totalCheckedInQuery, checkedInParams, sqlTransaction);
            if (oCheckedIn != null)
            {
                int checkedIn = Convert.ToInt32(oCheckedIn);
                return checkedIn;
            }
            else
                return 0;
        }

        /// <summary>
        /// Refresh CheckInDTO with DB values
        /// </summary>
        /// <param name="checkInDTO">Check In DTO</param>
        /// <param name="dt">Data table</param>
        /// <param name="loginId">Login ID</param>
        /// <param name="siteId">site </param>
        private void RefreshCheckInDTO(CheckInDTO checkInDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(checkInDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                checkInDTO.CheckInId = Convert.ToInt32(dt.Rows[0]["CheckInId"]);
                checkInDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["last_update_date"]);
                checkInDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                checkInDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                checkInDTO.LastUpdatedBy = Convert.ToString(dt.Rows[0]["last_updated_user"]);
                checkInDTO.CreatedBy = Convert.ToString(dt.Rows[0]["createdBy"]);
                checkInDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of CheckInDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<CheckInDTO> GetAllCheckInDTOList(List<KeyValuePair<CheckInDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<CheckInDTO> checkInDTOList = new List<CheckInDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CheckInDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ( searchParameter.Key == CheckInDTO.SearchByParameters.CHECK_IN_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.CHECK_IN_FACILITY_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.CHECK_IN_TRX_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.CUSTOMER_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.CARD_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.TABLE_ID
                            || searchParameter.Key == CheckInDTO.SearchByParameters.TRX_LINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CheckInDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CheckInDTO.SearchByParameters.IS_ACTIVE)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        //  Reference For the search by Date Parameter.
                        else if (searchParameter.Key == CheckInDTO.SearchByParameters.CHECK_IN_TIME)

                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE())>=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                    CheckInDTO checkInDTO = GetCheckInDTO(dataRow);
                    checkInDTOList.Add(checkInDTO);
                }
            }
            log.LogMethodExit(checkInDTOList);
            return checkInDTOList;
        }
    }
}
