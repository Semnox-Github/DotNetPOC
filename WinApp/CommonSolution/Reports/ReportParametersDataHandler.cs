/************************************************************************************************************
 * Project Name - Reports Data Handler
 * Description  - Data handler of the reports class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ************************************************************************************************************
 *1.00        14-Apr-2017   Amaresh          Created 
 *2.70.2        10-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query
 ************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportsParameter Data Handler - Handles insert, update and select of ReportsParameter Data objects
    /// </summary>
    public class ReportParametersDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ReportParameters AS rp";

        /// <summary>
        /// Dictionary for searching Parameters for the ReportParameters object.
        /// </summary>
        private static readonly Dictionary<ReportParametersDTO.SearchByReportsParameters, string> DBSearchParameters = new Dictionary<ReportParametersDTO.SearchByReportsParameters, string>
            {
                {ReportParametersDTO.SearchByReportsParameters.REPORT_ID, "rp.ReportId"},
                {ReportParametersDTO.SearchByReportsParameters.PARAMETER_ID, "rp.ParameterId"},
                {ReportParametersDTO.SearchByReportsParameters.ACTIVE_FLAG, "rp.ActiveFlag"},
                {ReportParametersDTO.SearchByReportsParameters.PARAMETER_NAME, "rp.ParameterName"},
                {ReportParametersDTO.SearchByReportsParameters.SITE_ID, "rp.site_id"},
                {ReportParametersDTO.SearchByReportsParameters.MASTERENTITYID, "rp.Masterentityid"}
            };

        /// <summary>
        /// Default constructor of ReportParametersDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportParametersDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating ReportParameters Reecord.
        /// 
        /// </summary>
        /// <param name="reportParametersDTO">reportParametersDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportParametersDTO reportParametersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParametersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@parameterId", reportParametersDTO.ParameterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportId", reportParametersDTO.ReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parameterName", reportParametersDTO.ParameterName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sqlParameter", reportParametersDTO.SqlParameter));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(reportParametersDTO.Description) ? DBNull.Value : (object)reportParametersDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataType", reportParametersDTO.DataType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataSourceType", reportParametersDTO.DataSourceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@dataSource", string.IsNullOrEmpty(reportParametersDTO.DataSource) ? DBNull.Value : (object)reportParametersDTO.DataSource));
            parameters.Add(dataAccessHandler.GetSQLParameter("@operator", string.IsNullOrEmpty(reportParametersDTO.Operator) ? DBNull.Value : (object)reportParametersDTO.Operator));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", reportParametersDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", reportParametersDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mandatory", reportParametersDTO.Mandatory));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportParametersDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the reports record to the database
        /// </summary>
        /// <param name="reportParametersDTO">ReportParametersDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportParametersDTO InsertReportParameter(ReportParametersDTO reportParametersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParametersDTO, loginId, siteId);
            string insertReportParameterQuery = @"INSERT INTO [dbo].[ReportParameters] 
                                                        (    
                                                        ReportId,
                                                        ParameterName,
                                                        SQLParameter,
                                                        Description,
                                                        DataType,
                                                        DataSourceType,
                                                        DataSource,
                                                        Operator,
                                                        ActiveFlag,
                                                        DisplayOrder,
                                                        Mandatory,
                                                        LastUpdatedDate,
                                                        LastUpdatedUser,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @reportId,
                                                        @parameterName,
                                                        @sqlParameter,
                                                        @description,                                              
                                                        @dataType,
                                                        @dataSourceType,
                                                        @dataSource,
                                                        @operator,
                                                        @activeFlag,
                                                        @displayOrder,
                                                        @mandatory,
                                                        Getdate(),
                                                        @lastUpdatedUser,
                                                        NewId() ,
                                                        @siteId,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate()
                                                    )SELECT * FROM ReportParameters WHERE parameterId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportParameterQuery, GetSQLParameters(reportParametersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportParametersDTO(reportParametersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportParametersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportParametersDTO);
            return reportParametersDTO;
        }

        /// <summary>
        /// Updates the ReportParameter record
        /// </summary>
        /// <param name="reportParametersDTO">ReportParametersDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportParametersDTO UpdateReportParameter(ReportParametersDTO reportParametersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportParametersDTO, loginId, siteId);

            string UpdateReportParameterQuery = @"update ReportParameters 
                                            set ReportId = @reportId,
                                            ParameterName = @parameterName,
                                            SQLParameter = @sqlParameter,
                                            Description = @description,
                                            DataType = @dataType,
                                            DataSourceType = @dataSourceType,
                                            DataSource = @dataSource,
                                            Operator = @operator,
                                            ActiveFlag = @activeFlag,
                                            DisplayOrder = @displayOrder,
                                            Mandatory = @mandatory,
                                            LastUpdatedDate = Getdate(),
                                            LastUpdatedUser = @lastUpdatedUser,
                                            -- site_id = @siteId,
                                            MasterEntityId = @masterEntityId
                                        where ParameterId = @parameterId
                                        SELECT* FROM ReportParameters WHERE ParameterId = @parameterId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(UpdateReportParameterQuery, GetSQLParameters(reportParametersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportParametersDTO(reportParametersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportParametersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportParametersDTO);
            return reportParametersDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportParametersDTO">reportParametersDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportParametersDTO(ReportParametersDTO reportParametersDTO, DataTable dt)
        {
            log.LogMethodEntry(reportParametersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportParametersDTO.ParameterId = Convert.ToInt32(dt.Rows[0]["ParameterId"]);
                reportParametersDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                reportParametersDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportParametersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reportParametersDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                reportParametersDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportParametersDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ReportParametersDTO class type
        /// </summary>
        /// <param name="reportParameterDataRow">ReportParametersDTO DataRow</param>
        /// <returns>Returns ReportParametersDTO</returns>
        private ReportParametersDTO GetReportParametersDTO(DataRow reportParameterDataRow)
        {
            log.LogMethodEntry(reportParameterDataRow);
            ReportParametersDTO reportsDataObject = new ReportParametersDTO(Convert.ToInt32(reportParameterDataRow["ParameterId"]),
                                                    reportParameterDataRow["ReportId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["ReportId"]),
                                                    reportParameterDataRow["ParameterName"].ToString(),
                                                    reportParameterDataRow["SQLParameter"].ToString(),
                                                    reportParameterDataRow["Description"].ToString(),
                                                    reportParameterDataRow["DataType"].ToString(),
                                                    reportParameterDataRow["DataSourceType"].ToString(),
                                                    reportParameterDataRow["DataSource"] == DBNull.Value ? "" : reportParameterDataRow["DataSource"].ToString(),
                                                    reportParameterDataRow["Operator"].ToString(),
                                                    reportParameterDataRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["DisplayOrder"]),
                                                    reportParameterDataRow["Mandatory"] == DBNull.Value ? false : Convert.ToBoolean(reportParameterDataRow["Mandatory"]),
                                                    reportParameterDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportParameterDataRow["LastUpdatedDate"]),
                                                    reportParameterDataRow["LastUpdatedUser"].ToString(),
                                                    reportParameterDataRow["Guid"].ToString(),
                                                    reportParameterDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["site_id"]),
                                                    reportParameterDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportParameterDataRow["SynchStatus"]),
                                                    reportParameterDataRow["ActiveFlag"] == DBNull.Value ? false : Convert.ToBoolean(reportParameterDataRow["ActiveFlag"]),
                                                    reportParameterDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportParameterDataRow["MasterEntityId"]),
                                                    reportParameterDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportParameterDataRow["CreatedBy"]),
                                                    reportParameterDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportParameterDataRow["CreationDate"])
                                                    );
            log.LogMethodExit(reportsDataObject);
            return reportsDataObject;
        }

        /// <summary>
        /// Gets the user data of passed userId
        /// </summary>
        /// <param name="parameterId">integer type parameter</param>
        /// <returns>Returns ReportParametersDTO</returns>
        public ReportParametersDTO GetReportParameter(int parameterId)
        {
            log.LogMethodEntry(parameterId);
            ReportParametersDTO result = null;
            string selectReportParameterQuery = SELECT_QUERY + @" WHERE rp.ParameterId = @parameterId";

            SqlParameter parameter = new SqlParameter("@parameterId", parameterId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectReportParameterQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReportParametersDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets parameters for a given reportId
        /// </summary>
        /// <param name="reportId">integer type parameter</param>
        /// <returns>Returns ReportParametersDTO</returns>
        public List<ReportParametersDTO> GetReportParameterListByReport(int reportId)
        {
            log.LogMethodEntry(reportId);
            string selectReportParameterQuery = @"select *
                                         from ReportParameters
                                        where reportId = @reportId and ActiveFlag=1";

            SqlParameter[] selectReportParameters = new SqlParameter[1];
            selectReportParameters[0] = new SqlParameter("@reportId", reportId);

            DataTable reportParameterData = dataAccessHandler.executeSelectQuery(selectReportParameterQuery, selectReportParameters,sqlTransaction);
            List<ReportParametersDTO> reportsList = new List<ReportParametersDTO>();

            if (reportParameterData.Rows.Count > 0)
            {
                foreach (DataRow reportsDataRow in reportParameterData.Rows)
                {
                    ReportParametersDTO reportsDataObject = GetReportParametersDTO(reportsDataRow);
                    reportsList.Add(reportsDataObject);
                }
                log.LogMethodExit(reportsList);
                return reportsList;
            }
            else
            {
                log.LogMethodExit(reportsList);
                return reportsList;
            }
        }

        /// <summary>
        /// Gets the ReportParametersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportParametersDTO matching the search criteria</returns>
        public List<ReportParametersDTO> GetReportsParameterList(List<KeyValuePair<ReportParametersDTO.SearchByReportsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ReportParametersDTO> reportParametersList = new List<ReportParametersDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportParametersDTO.SearchByReportsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportParametersDTO.SearchByReportsParameters.PARAMETER_ID
                            || searchParameter.Key == ReportParametersDTO.SearchByReportsParameters.REPORT_ID
                            || searchParameter.Key == ReportParametersDTO.SearchByReportsParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportParametersDTO.SearchByReportsParameters.PARAMETER_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportParametersDTO.SearchByReportsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportParametersDTO.SearchByReportsParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                selectQuery = selectQuery + query + " Order by ParameterId ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportParametersDTO reportParametersDTO = GetReportParametersDTO(dataRow);
                    reportParametersList.Add(reportParametersDTO);
                }
            }
            log.LogMethodExit(reportParametersList);
            return reportParametersList;
        }
    }
}
