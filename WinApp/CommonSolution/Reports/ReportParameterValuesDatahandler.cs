/********************************************************************************************
 * Project Name - ReportParameterValues Datahandler Programs 
 * Description  - Data object of the ReportParameterValuesDatahandler
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-April-2017   Rakshith           Created 
 *2.70.2        12-Jul-2019     Dakshakh raj       Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2        10-Dec-2019     Jinto Thomas       Removed siteid from update query
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
    /// ReportParameterValues Datahandler - Handles insert, update and select of ReportParameterValues Data objects
    /// </summary>
    public class ReportParameterValuesDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ReportParameterValues AS rpv";

        /// <summary>
        /// Dictionary for searching Parameters for the ReportParameterValues object.
        /// </summary>

        private static readonly Dictionary<ReportParameterValuesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReportParameterValuesDTO.SearchByParameters, string>
            {
                {ReportParameterValuesDTO.SearchByParameters.REPORT_PARMAMETER_VALUE_ID, "rpv.ReportParameterValueId"},
                {ReportParameterValuesDTO.SearchByParameters.REPORT_SCHEDULE_REPORT_ID, "rpv.ReportScheduleReportId"},
                {ReportParameterValuesDTO.SearchByParameters.PARAMETER_ID, "rpv.ParameterId"},
                {ReportParameterValuesDTO.SearchByParameters.PARAMETER_VALUE, "rpv.ParameterValue"},
                {ReportParameterValuesDTO.SearchByParameters.SITEID, "rpv.site_id"},
                {ReportParameterValuesDTO.SearchByParameters.MASTERENTITYID, "rpv.Masterentityid"}
            };

        /// <summary>
        /// Default constructor of ReportsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportParameterValuesDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating ReportParameterValues.
        /// 
        /// </summary>
        /// <param name="reportParameterValuesDTO">reportParameterValuesDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportParameterValuesDTO reportParameterValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParameterValuesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportParameterValueId", reportParameterValuesDTO.ReportParameterValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportScheduleReportId", reportParameterValuesDTO.ReportScheduleReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parameterId", reportParameterValuesDTO.ParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parameterValue", string.IsNullOrEmpty(reportParameterValuesDTO.ParameterValue) ? DBNull.Value : (object)reportParameterValuesDTO.ParameterValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportParameterValuesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the reports record to the database
        /// </summary>
        /// <param name="reportParameterValuesDTO">ReportParameterValuesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportParameterValuesDTO InsertReportParameterValues(ReportParameterValuesDTO reportParameterValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParameterValuesDTO, loginId, siteId);
            string insertReportsQuery = @"INSERT INTO [dbo].[ReportParameterValues] 
                                                        (                                                         
                                                        ReportScheduleReportId
                                                        ,ParameterId
                                                        ,ParameterValue
                                                        ,LastUpdatedDate
                                                        ,LastUpdatedUser
                                                        ,Guid
                                                        ,site_id
                                                        ,MasterEntityId
                                                        ,CreatedBy
                                                        ,CreationDate
                                                        ) 
                                                        values 
                                                        (                                                        
                                                        @reportScheduleReportId
                                                        ,@parameterId
                                                        ,@parameterValue
                                                        ,getdate()
                                                        ,@lastUpdatedUser
                                                        ,NewId()
                                                        ,@siteId
                                                        ,@masterEntityId
                                                        ,@createdBy
                                                        ,getDate()
                                                    )SELECT * FROM ReportParameterValues WHERE ReportParameterValueId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportsQuery, GetSQLParameters(reportParameterValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportParameterValuesDTO(reportParameterValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportParameterValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportParameterValuesDTO);
            return reportParameterValuesDTO;
        }

        /// <summary>
        /// Updates the user record
        /// </summary>reportParameterValuesDTO
        /// <param name="reportParameterValuesDTO">ReportParameterValuesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportParameterValuesDTO UpdateReportParameterValues(ReportParameterValuesDTO reportParameterValuesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParameterValuesDTO, loginId, siteId);

            string updateReportParameterQueryO = @"update ReportParameterValues 
                                            set ReportScheduleReportId = @reportScheduleReportId,  
                                                ParameterId = @parameterId, 
                                                ParameterValue = @parameterValue, 
                                                LastUpdatedDate =getdate(),  
                                                LastUpdatedUser = @lastUpdatedUser,  
                                                -- site_id = @siteId,  
                                                MasterEntityId = @masterEntityId  
                                                where ReportParameterValueId = @reportParameterValueId
                                                SELECT* FROM ReportParameterValues WHERE ReportParameterValueId = @reportParameterValueId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReportParameterQueryO, GetSQLParameters(reportParameterValuesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportParameterValuesDTO(reportParameterValuesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportParameterValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportParameterValuesDTO);
            return reportParameterValuesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportParameterValuesDTO">reportParameterValuesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportParameterValuesDTO(ReportParameterValuesDTO reportParameterValuesDTO, DataTable dt)
        {
            log.LogMethodEntry(reportParameterValuesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportParameterValuesDTO.ReportParameterValueId = Convert.ToInt32(dt.Rows[0]["ReportParameterValueId"]);
                reportParameterValuesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                reportParameterValuesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportParameterValuesDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                reportParameterValuesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportParameterValuesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ReportParameterValuesDTO class type
        /// </summary>
        /// <param name="reportParameterDataRow">ReportParameterValuesDTO DataRow</param>
        /// <returns>Returns ReportParameterValuesDTO</returns>
        private ReportParameterValuesDTO GetReportParameterValuesDTO(DataRow reportParameterDataRow)
        {
            log.LogMethodEntry(reportParameterDataRow);
            ReportParameterValuesDTO reportsDataObject = new ReportParameterValuesDTO(
                                        reportParameterDataRow["ReportParameterValueId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["ReportParameterValueId"]),
                                        reportParameterDataRow["ReportScheduleReportId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["ReportScheduleReportId"]),
                                        reportParameterDataRow["ParameterId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["ParameterId"]),
                                        reportParameterDataRow["ParameterValue"].ToString(),
                                        reportParameterDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportParameterDataRow["LastUpdatedDate"]),
                                        reportParameterDataRow["LastUpdatedUser"].ToString(),
                                        reportParameterDataRow["Guid"].ToString(),
                                        reportParameterDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["site_id"]),
                                        reportParameterDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportParameterDataRow["SynchStatus"]),
                                        reportParameterDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["MasterEntityId"]),
                                        reportParameterDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportParameterDataRow["CreatedBy"]),
                                        reportParameterDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportParameterDataRow["CreationDate"])

                                        );
            log.LogMethodExit(reportsDataObject);
            return reportsDataObject;
        }

        /// <summary>
        /// Gets the ReportParameterValuesDTO of passed userId
        /// </summary>
        /// <param name="reportParameterValueId">integer type parameter</param>
        /// <returns>Returns ReportParameterValuesDTO</returns>
        public ReportParameterValuesDTO GetReportParameterValuesDTO(int reportParameterValueId)
        {
            log.LogMethodEntry(reportParameterValueId);
            ReportParameterValuesDTO result = null;
            string selectReportQuery = SELECT_QUERY + @" WHERE ReportParameterValueId = @reportParameterValueId";
             SqlParameter parameter = new SqlParameter("@reportParameterValueId", reportParameterValueId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectReportQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetReportParameterValuesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReportParameterValuesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportParameterValuesDTO matching the search criteria</returns>
        public List<ReportParameterValuesDTO> GetReportsParameterValuesList(List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ReportParameterValuesDTO> reportParameterValuesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportParameterValuesDTO.SearchByParameters.PARAMETER_ID
                            || searchParameter.Key == ReportParameterValuesDTO.SearchByParameters.MASTERENTITYID
                            || searchParameter.Key == ReportParameterValuesDTO.SearchByParameters.REPORT_PARMAMETER_VALUE_ID
                            || searchParameter.Key == ReportParameterValuesDTO.SearchByParameters.REPORT_SCHEDULE_REPORT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportParameterValuesDTO.SearchByParameters.PARAMETER_VALUE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportParameterValuesDTO.SearchByParameters.SITEID)
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
                selectQuery = selectQuery + query + " Order by ReportParameterValueId "; 
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                reportParameterValuesDTOList = new List<ReportParameterValuesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportParameterValuesDTO reportParameterValuesDTO  = GetReportParameterValuesDTO(dataRow);
                    reportParameterValuesDTOList.Add(reportParameterValuesDTO);
                }
            }
            log.LogMethodExit(reportParameterValuesDTOList);
            return reportParameterValuesDTOList;
         }

    }
}

