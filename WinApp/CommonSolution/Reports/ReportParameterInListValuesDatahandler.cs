/********************************************************************************************
 * Project Name - ReportParameterInListValues Datahandler Programs 
 * Description  - Data object of the ReportParameterInListValuesDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-April-2017   Rakshith           Created
 *2.70.2        10-Jul-2019     Dakshakh raj       Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas         Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportParameterInListValues Datahandler - Handles insert, update and select of ReportParameterInListValues Data objects
    /// </summary>
    public class ReportParameterInListValuesDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ReportParameterInListValues AS rpin ";

        /// <summary>
        /// Dictionary for searching Parameters for the ReportParameterInListValues object.
        /// </summary>
        private static readonly Dictionary<ReportParameterInListValuesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReportParameterInListValuesDTO.SearchByParameters, string>
            {
                {ReportParameterInListValuesDTO.SearchByParameters.REPORT_PARMAMETER_VALUE_ID, "rpin.ReportParameterValueId"},
                {ReportParameterInListValuesDTO.SearchByParameters.IN_LIST_VALUE, "rpin.InListValue"},
                {ReportParameterInListValuesDTO.SearchByParameters.ID, "rpin.Id"},
                {ReportParameterInListValuesDTO.SearchByParameters.SITEID, "rpin.Site_id"},
                {ReportParameterInListValuesDTO.SearchByParameters.MASTERENTITYID, "rpin.Masterentityid"}
            };

        /// <summary>
        ///  Default constructor of ReportParameterInListValuesDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportParameterInListValuesDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// Inserts the ReportParameterInListValues record to the database
        /// </summary>
        /// <param name="reportParameterInListValuesDTO">ReportParameterInListValuesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportParameterInListValuesDTO InsertReportParameterInlistValues(ReportParameterInListValuesDTO reportParameterInListValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParameterInListValuesDTO, loginId, siteId);
            string insertReportParameterInlistValuesQuery = @"INSERT INTO[dbo].[ReportParameterInListValues] 
                                                        (                                                         
                                                            ReportParameterValueId
                                                            ,InListValue
                                                            ,Guid
                                                            ,site_id
                                                            ,MasterEntityId
                                                            ,CreatedBy
                                                            ,CreationDate
                                                            ,LastUpdatedBy
                                                            ,LastupdateDate   
                                                        ) 
                                                        values 
                                                        (  
                                                            @reportParameterValueId
                                                            ,@inListValue
                                                            ,NewId()
                                                            ,@siteId
                                                            ,@masterEntityId   
                                                            ,@createdBy
                                                            ,getDate()
                                                            ,@lastUpdatedBy
                                                            ,getDate())
                                                 SELECT * FROM ReportParameterInListValues WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportParameterInlistValuesQuery, GetSQLParameters(reportParameterInListValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportParameterInListValuesDTO(reportParameterInListValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportParameterInListValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportParameterInListValuesDTO);
            return reportParameterInListValuesDTO;
        }

        /// <summary>
        /// Updates the user record
        /// </summary>reportParameterInListValuesDTO
        /// <param name="reportParameterInListValuesDTO">ReportParameterInListValuesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportParameterInListValuesDTO UpdateReportParameterInlistValues(ReportParameterInListValuesDTO reportParameterInListValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParameterInListValuesDTO, loginId, siteId);

            string updateReportParameterInlistValuesQuery = @"UPDATE [dbo].[ReportParameterInListValues] 
                                            set ReportParameterValueId = @reportParameterValueId,  
                                            InListValue = @inListValue,  
                                            -- site_id = @siteId,  
                                            MasterEntityId = masterEntityId,
                                            LastUpdatedBy = @lastUpdatedBy,
                                            LastUpdateDate = GETDATE()
                                            where Id = @id
                                            SELECT* FROM ReportParameterInListValues WHERE Id = @id";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReportParameterInlistValuesQuery, GetSQLParameters(reportParameterInListValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportParameterInListValuesDTO(reportParameterInListValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportParameterInListValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportParameterInListValuesDTO);
            return reportParameterInListValuesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportParameterInListValuesDTO">reportParameterInListValuesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportParameterInListValuesDTO(ReportParameterInListValuesDTO reportParameterInListValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(reportParameterInListValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportParameterInListValuesDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                reportParameterInListValuesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reportParameterInListValuesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportParameterInListValuesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reportParameterInListValuesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportParameterInListValuesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Builds the SQL Parameter list used for inserting and updating Report parameters Record.
        /// </summary>
        /// <param name="reportParameterInListValuesDTO">reportParameterInListValuesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportParameterInListValuesDTO reportParameterInListValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParameterInListValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", reportParameterInListValuesDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportParameterValueId", reportParameterInListValuesDTO.ReportParameterValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@inListValue", string.IsNullOrEmpty(reportParameterInListValuesDTO.InListValue) ? DBNull.Value : (object)reportParameterInListValuesDTO.InListValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportParameterInListValuesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to ReportParameterInListValuesDTO class type
        /// </summary>
        /// <param name="reportParameterInListDataRow">reportParameterInListDataRow DataRow</param>
        /// <returns>Returns ReportParameterInListValuesDTO</returns>
        private ReportParameterInListValuesDTO GetReportParameterInListValuesDTO(DataRow reportParameterInListDataRow)
        {
            log.LogMethodEntry(reportParameterInListDataRow);
            ReportParameterInListValuesDTO reportsDataObject = new ReportParameterInListValuesDTO(
                                        reportParameterInListDataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterInListDataRow["Id"]),
                                        reportParameterInListDataRow["ReportParameterValueId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterInListDataRow["ReportParameterValueId"]),
                                        reportParameterInListDataRow["InListValue"].ToString(),
                                        reportParameterInListDataRow["Guid"].ToString(),
                                        reportParameterInListDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterInListDataRow["site_id"]),
                                        reportParameterInListDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportParameterInListDataRow["SynchStatus"]),
                                        reportParameterInListDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterInListDataRow["MasterEntityId"]),
                                        reportParameterInListDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportParameterInListDataRow["CreatedBy"]),
                                        reportParameterInListDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportParameterInListDataRow["CreationDate"]),
                                        reportParameterInListDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportParameterInListDataRow["LastUpdatedBy"]),
                                        reportParameterInListDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportParameterInListDataRow["LastUpdateDate"])
                                                    );
            log.LogMethodExit(reportParameterInListDataRow);
            return reportsDataObject;
        }

        /// <summary>
        /// Gets the ReportParameterInListValuesDTO of passed userId
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Returns ReportParameterInListValuesDTO</returns>
        public ReportParameterInListValuesDTO GetReportParameterInListValuesDTO(int id)
        {
            log.LogMethodEntry(id);
            ReportParameterInListValuesDTO result = null;
            string selectReportQuery = SELECT_QUERY + @" WHERE rpin.Id = @id";
            SqlParameter parameter = new SqlParameter("@Id", id);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectReportQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetReportParameterInListValuesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReportParameterInListValuesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportParameterInListValuesDTO matching the search criteria</returns>
        public List<ReportParameterInListValuesDTO> GetReportParameterInListValuesList(List<KeyValuePair<ReportParameterInListValuesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ReportParameterInListValuesDTO> reportParameterInListValuesDTOList = new List<ReportParameterInListValuesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportParameterInListValuesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportParameterInListValuesDTO.SearchByParameters.ID
                            || searchParameter.Key == ReportParameterInListValuesDTO.SearchByParameters.MASTERENTITYID
                            || searchParameter.Key == ReportParameterInListValuesDTO.SearchByParameters.REPORT_PARMAMETER_VALUE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportParameterInListValuesDTO.SearchByParameters.IN_LIST_VALUE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportParameterInListValuesDTO.SearchByParameters.SITEID)
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
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query + " Order by Id "; 
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportParameterInListValuesDTO reportParameterInListValuesDTO = GetReportParameterInListValuesDTO(dataRow);
                    reportParameterInListValuesDTOList.Add(reportParameterInListValuesDTO);
                }
            }
            log.LogMethodExit(reportParameterInListValuesDTOList);
            return reportParameterInListValuesDTOList;
        }

        /// <summary>
        /// DeleteReportParameterInListValues method
        /// </summary>
        /// <param name="valueId">int valueId</param>
        /// <returns>Returns ReportParameterInListValuesDTO</returns>
        public int DeleteReportParameterInListValues(int valueId)
        {
            log.LogMethodEntry(valueId);
            string selectReportQuery = @"delete from ReportParameterInListValues 
                                         where ReportParameterValueId = @valueId";
            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@valueId", valueId);
            int rowsAffected = dataAccessHandler.executeUpdateQuery(selectReportQuery, selectReportParameters,sqlTransaction);

            log.LogMethodExit(rowsAffected);
            return rowsAffected;
        }

        /// <summary>
        /// Gets the ReportParameterInListValuesDTO of passed userId
        /// </summary>
        /// <param name="reportScheduleReportId">int reportScheduleReportId</param>
        /// /// <param name="parameterId">int parameterId</param>
        /// <returns>Returns dtData</returns>
        public DataTable getScheduleParameterInListValue(int reportScheduleReportId, int parameterId)
        {
            log.LogMethodEntry(reportScheduleReportId, parameterId);
            string selectReportQuery = @"select InListValue 
                                                    from ReportParameterValues rp, ReportParameterInListValues rpi 
                                                    where rp.ReportScheduleReportId = @reportScheduleReportId 
                                                    and rp.ParameterId = @parameterId
                                                    and rpi.ReportParameterValueId = rp.ReportParameterValueId";
            SqlParameter[] selectReportParameters = new SqlParameter[2];
            selectReportParameters[0] = new SqlParameter("@reportScheduleReportId", reportScheduleReportId);
            selectReportParameters[1] = new SqlParameter("@parameterId", parameterId);
            DataTable dtData = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters,sqlTransaction);
            if (dtData.Rows.Count == 0)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                log.LogMethodExit(dtData); 
                return dtData;
            }
        }

        /// <summary>
        /// Gets the ReportParameterInListValuesDTO of passed userId
        /// </summary>
        /// <param name="reportId">int reportScheduleReportId</param>
        /// <param name="parameterId">int parameterId</param>
        /// <param name="scheduleid">int scheduleid</param>
        /// <returns>Returns dtData</returns>
        public DataTable GetScheduleParameterInListValueByReportId(int reportId, int parameterId, int scheduleid)
        {
            log.LogMethodEntry(reportId, parameterId, scheduleid);
            string selectReportQuery = @"select InListValue ,ReportId,rpv.ReportParameterValueId
                                        from  ReportParameters rp,ReportParameterValues rpv ,ReportParameterInListValues rplv , report_schedule_reports rsr
                                        where 
                                        rp.ParameterId=rpv.ParameterId
                                         and rsr.report_id=rp.ReportId
                                        and rsr.report_schedule_report_id=rpv.ReportScheduleReportId
                                         and rpv.ReportParameterValueId=rplv.ReportParameterValueId
                                        and rpv.ParameterId=@parameterId
                                        and rp.ReportId=@reportId
                                        and rsr.schedule_id=@scheduleid
                                        ";
            SqlParameter[] selectReportParameters = new SqlParameter[3];
            selectReportParameters[0] = new SqlParameter("@reportId", reportId);
            selectReportParameters[1] = new SqlParameter("@parameterId", parameterId);
            selectReportParameters[2] = new SqlParameter("@scheduleid", scheduleid);
            DataTable dtData = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportParameters,sqlTransaction);
            if (dtData.Rows.Count == 0)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                log.LogMethodExit(dtData);
                return dtData;
            }
        }
    }
}

