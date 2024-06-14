/********************************************************************************************
 * Project Name - ReportsParameter
 * Description  - Bussiness logic of Report parameter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Apr-2017   Amaresh        Created 
 *2.70.2        12-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportParameters class
    /// </summary>
    public class ReportParameters
    {
        private ReportParametersDTO reportParametersDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportParameters()
        {
            log.LogMethodEntry();
            reportParametersDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the reportParameters DTO parameter
        /// </summary>
        /// <param name="reportParametersDTO">Parameter of the type ReportParametersDTO</param>
        public ReportParameters(ReportParametersDTO reportParametersDTO)
        {
            log.LogMethodEntry(reportParametersDTO);
            this.reportParametersDTO = reportParametersDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Reports  
        /// Reports   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Semnox.Core.Utilities.ExecutionContext executionUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            ReportParametersDataHandler reportParametersDataHandler = new ReportParametersDataHandler(sqlTransaction);

            if (reportParametersDTO.ParameterId < 0)
            {
                reportParametersDTO = reportParametersDataHandler.InsertReportParameter(reportParametersDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportParametersDTO.AcceptChanges();
            }
            else
            {
                if (reportParametersDTO.IsChanged)
                {
                    reportParametersDTO = reportParametersDataHandler.UpdateReportParameter(reportParametersDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportParametersDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ReportsParameter List
    /// </summary>
    public class ReportsParameterList
    {
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Returns the ReportParametersDTO
        /// </summary>
        /// <param name="parameterId">parameterId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public ReportParametersDTO GetReportParameter(int parameterId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterId, sqlTransaction);
            ReportParametersDataHandler  reportParametersDataHandler = new ReportParametersDataHandler(sqlTransaction);
            ReportParametersDTO reportParametersDTO = reportParametersDataHandler.GetReportParameter(parameterId);
            log.LogMethodExit(reportParametersDTO);
            return reportParametersDTO;
        }

        /// <summary>
        /// Returns the List of ReportParametersDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ReportParametersDTO> GetAllReports(List<KeyValuePair<ReportParametersDTO.SearchByReportsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportParametersDataHandler reportParametersDataHandler = new ReportParametersDataHandler(sqlTransaction);
            List<ReportParametersDTO> reportParametersDTOList = reportParametersDataHandler.GetReportsParameterList(searchParameters);
            log.LogMethodExit(reportParametersDTOList);
            return reportParametersDTOList;
        }

        /// <summary>
        /// Returns the ReportParametersDTO List
        /// </summary>
        /// <param name="reportId">reportId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ReportParametersDTO> GetReportParameterListByReport(int reportId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportId, sqlTransaction);
            ReportParametersDataHandler reportParametersDataHandler = new ReportParametersDataHandler(sqlTransaction);
            List<ReportParametersDTO> reportParametersDTOList = reportParametersDataHandler.GetReportParameterListByReport(reportId);
            log.LogMethodExit(reportParametersDTOList);
            return reportParametersDTOList;
        }
    }
}
