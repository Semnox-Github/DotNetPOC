/********************************************************************************************
 * Project Name - Customer App User Log                                                                     
 * Description  - DH for Customer App configuration
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.CustomerApp
{
    public class CustomerAppUserLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        public CustomerAppUserLogDataHandler()
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        public CustomerAppUserLogDataHandler( SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        private List<SqlParameter> GetSQLParameters(CustomerAppUserLogDTO logDTO, string userId, int siteId)
        {
            log.LogMethodEntry(logDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", logDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", logDTO.CustomerId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Controller", logDTO.Controller));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Action", logDTO.Action));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VariableState", logDTO.VariableState));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Message", logDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@uuid", logDTO.UUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", logDTO.guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", logDTO.IsActive ? 1 : 0));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", logDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", logDTO.MasterEntityId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        public int InsertUserLog(CustomerAppUserLogDTO userLog, string userId, int siteId)
        {
            log.LogMethodEntry(userLog, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO CustomerAppUserLog 
                                        ( 
                                            CustomerId,
                                            Controller,
                                            Action,
                                            VariableState,
                                            Message,
                                            UUID,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            site_id
                                        ) 
                                VALUES  
                                        (
                                            @CustomerId,
                                            @Controller,
                                            @Action,
                                            @VariableState,
                                            @Message,
                                            @uuid,
                                            1,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id
                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(userLog, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        public int UpdateUserLog(CustomerAppUserLogDTO userLog, string userId, int siteId)
        {
            log.LogMethodEntry(userLog, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE CustomerAppUserLog
                             SET Controller=@Controller,
                                 Action=@Action,
                                 VariableState=@VariableState,
                                 Message=@Message,
                                 UUID=@uuid,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastupdatedDate=GETDATE()
                             WHERE Id = @Id";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(userLog, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        public CustomerAppUserLogDTO GetuserLogDTODTO(int userLogId)
        {
            log.LogMethodEntry(userLogId);
            CustomerAppUserLogDTO returnValue = null;
            string query = @"Select * from CustomerAppLoginOTP WHERE Id = @Id and ExpiryTime >= GETDATE() and IsVerified != 1";
            SqlParameter parameter = new SqlParameter("@Id", userLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetUserLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the OTP DTO from the DataRow
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private CustomerAppUserLogDTO GetUserLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerAppUserLogDTO otpDTO = new CustomerAppUserLogDTO(Convert.ToInt32(dataRow["id"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["Controller"] == DBNull.Value ? "" : dataRow["Controller"].ToString(),
                                            dataRow["Action"] == DBNull.Value ? "" : dataRow["Action"].ToString(),
                                            dataRow["VariableState"] == DBNull.Value ? "" : dataRow["VariableState"].ToString(),
                                            dataRow["Message"] == DBNull.Value ? "" : dataRow["Message"].ToString(),
                                            dataRow["UUID"] == DBNull.Value ? "" : dataRow["UUID"].ToString(),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"])
                                            );
            log.LogMethodExit(otpDTO);
            return otpDTO;
        }
    }
}
