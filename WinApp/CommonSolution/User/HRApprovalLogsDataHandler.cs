/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - HRApproval logs data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021  Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// DataHandler class for HRApprovalLogsDataHandler
    /// </summary>
    public class HRApprovalLogsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = "SELECT * FROM HRApprovalLogs as hra";
        private static readonly Dictionary<HRApprovalLogsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<HRApprovalLogsDTO.SearchByParameters, string>()
        {
            {HRApprovalLogsDTO.SearchByParameters.ACTION, "hra.Action" },
            {HRApprovalLogsDTO.SearchByParameters.APPROVAL_LOG_ID, "hra.ApprovalLogId" },
            {HRApprovalLogsDTO.SearchByParameters.ENTITY, "hra.Entity" },
            {HRApprovalLogsDTO.SearchByParameters.ENTITY_GUID, "hra.EntityGuid" },
            {HRApprovalLogsDTO.SearchByParameters.MASTER_ENTITY_ID, "hra.MasterEntityId" },
            {HRApprovalLogsDTO.SearchByParameters.POS_MACHINE_ID, "hra.POSMachineId" },
            {HRApprovalLogsDTO.SearchByParameters.SITE_ID, "hra.site_id" }
        };

        /// <summary>
        /// Default constructor of HRApprovalLogsDataHandler class
        /// </summary>
        public HRApprovalLogsDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of HRApprovalLogsDataHandler class passing SQLTransaction
        /// </summary>
        public HRApprovalLogsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSqlParameters(HRApprovalLogsDTO hRApprovalLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(hRApprovalLogsDTO, loginId, siteId);
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(dataAccessHandler.GetSQLParameter("@ApprovalLogId", hRApprovalLogsDTO.ApprovalLogId));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@Entity", hRApprovalLogsDTO.Entity));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@EntityGuid", hRApprovalLogsDTO.EntityGuid));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@Action", hRApprovalLogsDTO.Action));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@ApproverId", hRApprovalLogsDTO.ApproverId));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@ApprovalLevel", hRApprovalLogsDTO.ApprovalLevel));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@Remarks", hRApprovalLogsDTO.Remarks));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", hRApprovalLogsDTO.CreatedBy));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", hRApprovalLogsDTO.LastUpdatedBy));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", hRApprovalLogsDTO.POSMachineId));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@site_id", hRApprovalLogsDTO.SiteId));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", hRApprovalLogsDTO.SyncStatus));
            parameterList.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", hRApprovalLogsDTO.MasterEntityId));
            log.LogMethodExit();
            return parameterList;
        }

        ///<summary>
        /// method to insert HRApprovalLogs details into database
        ///</summary>
        public HRApprovalLogsDTO InsertHRApprovalLogs(HRApprovalLogsDTO hrApprovalLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(hrApprovalLogsDTO, loginId, siteId);
            string insertQuery = @"INSERT INTO HRApprovalLogs
                                        (
                                            Entity,
                                            EntityGuid,
                                            Action,
                                            ApproverId,
                                            ApprovalTime,
                                            ApprovalLevel,
                                            Remarks,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            POSMachineId,
                                            site_id,
                                            Guid,
                                            SynchStatus,
                                            MasterEntityId
                                        )
                                        values
                                        (
                                            @Entity,
                                            @EntityGuid,
                                            @Action,
                                            @ApproverId,
                                            GetDate(),
                                            @ApprovalLevel,
                                            @Remarks,
                                            @CreatedBy,
                                            GetDate(),
                                            @LastUpdatedBy,
                                            GetDate(),
                                            @POSMachineId,
                                            @site_id,
                                            NEWID(),
                                            @SynchStatus,
                                            @MasterEntityId
                                        )SELECT * FROM HRApprovalLogs WHERE ApprovalLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSqlParameters(hrApprovalLogsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshHRApprovalLogsDTO(hrApprovalLogsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting HRApprovalLogsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(hrApprovalLogsDTO);
            return hrApprovalLogsDTO;
        }

        public List<HRApprovalLogsDTO> GetHRApprovalLogsDTOs(List<KeyValuePair<HRApprovalLogsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectHRApprovalLogsQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<HRApprovalLogsDTO.SearchByParameters, string> parameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(parameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (parameter.Key == HRApprovalLogsDTO.SearchByParameters.APPROVAL_LOG_ID ||
                            parameter.Key == HRApprovalLogsDTO.SearchByParameters.POS_MACHINE_ID ||
                            parameter.Key == HRApprovalLogsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[parameter.Key] + "=" + dataAccessHandler.GetParameterName(parameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(parameter.Key), Convert.ToInt32(parameter.Value)));
                        }
                        else if (parameter.Key == HRApprovalLogsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[parameter.Key] + "=" + dataAccessHandler.GetParameterName(parameter.Key) + " or " + dataAccessHandler.GetParameterName(parameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(parameter.Key), Convert.ToInt32(parameter.Value)));
                        }
                        else if (parameter.Key == HRApprovalLogsDTO.SearchByParameters.ACTION)
                        {

                        }
                        else if (parameter.Key == HRApprovalLogsDTO.SearchByParameters.ENTITY ||
                            parameter.Key == HRApprovalLogsDTO.SearchByParameters.ENTITY_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[parameter.Key] + "=" + dataAccessHandler.GetParameterName(parameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(parameter.Key), parameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        throw new Exception("The query parameter does not exist " + parameter.Key);
                    }
                }
            }
            DataTable hrApprovalLogsData = dataAccessHandler.executeSelectQuery(selectHRApprovalLogsQuery, null);
            if (hrApprovalLogsData.Rows.Count > 0)
            {
                List<HRApprovalLogsDTO> hrApprovalLogsDTOList = new List<HRApprovalLogsDTO>();
                foreach (DataRow hrApprovalLogsDataRow in hrApprovalLogsData.Rows)
                {
                    HRApprovalLogsDTO hrApprovalLogsDataObject = GetHRApprovalLogsDTO(hrApprovalLogsDataRow);
                    hrApprovalLogsDTOList.Add(hrApprovalLogsDataObject);
                }
                log.LogMethodExit(hrApprovalLogsDTOList);
                return hrApprovalLogsDTOList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Converts the Data row object to HRApprovalLogsDTO class type
        /// </summary>
        /// <param name="hrApprovalLogsDataRow">hrApprovalLogs DataRow</param>
        /// <returns>Returns HRApprovalLogsDTO</returns>
        private HRApprovalLogsDTO GetHRApprovalLogsDTO(DataRow hrApprovalLogsDataRow)
        {
            log.LogMethodEntry(hrApprovalLogsDataRow);
            HRApprovalLogsDTO hrApprovalLogsDataObject = new HRApprovalLogsDTO(Convert.ToInt32(hrApprovalLogsDataRow["ApprovalLogId"]),
                                            hrApprovalLogsDataRow["Entity"].ToString(),
                                            hrApprovalLogsDataRow["EntityGuid"].ToString(),
                                            hrApprovalLogsDataRow["Action"].ToString(),
                                            hrApprovalLogsDataRow["ApproverId"].ToString(),
                                            hrApprovalLogsDataRow["ApprovalTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(hrApprovalLogsDataRow["CreationDate"]),
                                            hrApprovalLogsDataRow["ApprovalLevel"].ToString(),
                                            hrApprovalLogsDataRow["Remarks"].ToString(),
                                            hrApprovalLogsDataRow["CreatedBy"].ToString(),
                                            hrApprovalLogsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(hrApprovalLogsDataRow["CreationDate"]),
                                            hrApprovalLogsDataRow["LastUpdatedBy"].ToString(),
                                            hrApprovalLogsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(hrApprovalLogsDataRow["LastUpdateDate"]),
                                            Convert.ToInt32(hrApprovalLogsDataRow["POSMachineId"]),
                                            hrApprovalLogsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(hrApprovalLogsDataRow["site_id"]),
                                            hrApprovalLogsDataRow["Guid"].ToString(),
                                            hrApprovalLogsDataRow["SyncStatus"] == DBNull.Value ? false : Convert.ToBoolean(hrApprovalLogsDataRow["SynchStatus"]),
                                            Convert.ToInt32(hrApprovalLogsDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(hrApprovalLogsDataObject);
            return hrApprovalLogsDataObject;
        }

        private void RefreshHRApprovalLogsDTO(HRApprovalLogsDTO hrApprovalLogsDTO, DataTable dt)
        {
            log.LogMethodEntry(hrApprovalLogsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                hrApprovalLogsDTO.ApprovalLogId = Convert.ToInt32(dt.Rows[0]["ApprovalLogId"]);
                hrApprovalLogsDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                hrApprovalLogsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                hrApprovalLogsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                hrApprovalLogsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                hrApprovalLogsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                hrApprovalLogsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        public HRApprovalLogsDTO Update(HRApprovalLogsDTO hrApprovalLogsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(hrApprovalLogsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[HRApprovalLogs]
                                SET 
                                Action = @Action
                               ,ApproverId = @ApproverId
                               ,ApprovalTime = @ApprovalTime
                               ,ApprovalLevel = @ApprovalLevel
                               ,Remarks = @Remarks
                               ,LastUpdatedBy = @LastUpdatedBy
                               ,LastUpdateDate = @LastUpdateDate
                               ,POSMachineId = @POSMachineId
                               ,site_id = @site_id
                               ,MasterEntityId = @MasterEntityId
                               Where ApprovalLogId = @ApprovalLogId
                               SELECT * FROM HRApprovalLogs WHERE ApprovalLogId = @ApprovalLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSqlParameters(hrApprovalLogsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshHRApprovalLogsDTO(hrApprovalLogsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting HRApprovalLogsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(hrApprovalLogsDTO);
            return hrApprovalLogsDTO;
        }

    }
}
