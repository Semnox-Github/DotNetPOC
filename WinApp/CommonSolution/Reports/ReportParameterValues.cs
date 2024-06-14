/********************************************************************************************
 * Project Name - ReportParameterValues   Programs 
 * Description  - Data object of the ReportParameterValues
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-April-2017   Rakshith           Created 
 *2.70.2        12-Jul-2019     Dakshakh raj       Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// class of ReportParameterValues
    /// </summary>
    public class ReportParameterValues
    {
        private ReportParameterValuesDTO reportParameterValuesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportParameterValues()
        {
            log.LogMethodEntry();
            reportParameterValuesDTO = new ReportParameterValuesDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ReportParameterValues DTO parameter
        /// </summary>
        /// <param name="reportParameterValueId">reportParameterValueId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportParameterValues(int reportParameterValueId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportParameterValueId, sqlTransaction);
            ReportParameterValuesDatahandler reportParameterValuesDatahandler = new ReportParameterValuesDatahandler(sqlTransaction);
            this.reportParameterValuesDTO = reportParameterValuesDatahandler.GetReportParameterValuesDTO(reportParameterValueId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ReportParameterValues DTO parameter
        /// </summary>
        /// <param name="reportParameterValuesDTO">Parameter of the type ReportParameterValuesDTO</param>
        public ReportParameterValues(ReportParameterValuesDTO reportParameterValuesDTO)
        {
            log.LogMethodEntry(reportParameterValuesDTO);
            this.reportParameterValuesDTO = reportParameterValuesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// gets the GetReportParameterValuesDTO
        /// </summary>
        public ReportParameterValuesDTO GetReportParameterValuesDTO
        {

            get { return reportParameterValuesDTO; }
        }

        /// <summary>
        ///  Saves the ReportParameterValues  
        /// Reports will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Semnox.Core.Utilities.ExecutionContext executionUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            ReportParameterValuesDatahandler reportParameterValuesDatahandler = new ReportParameterValuesDatahandler(sqlTransaction);

            if (reportParameterValuesDTO.ReportParameterValueId < 0)
            {
                reportParameterValuesDTO = reportParameterValuesDatahandler.InsertReportParameterValues(reportParameterValuesDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportParameterValuesDTO.AcceptChanges();
            }
            else
            {
                if (reportParameterValuesDTO.IsChanged)
                {
                    reportParameterValuesDTO = reportParameterValuesDatahandler.UpdateReportParameterValues(reportParameterValuesDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportParameterValuesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Reports List
    /// </summary>
    public class ReportParameterValuesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the List of ReportParameterValuesDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ReportParameterValuesDTO> GetReportsParameterValuesList(List<KeyValuePair<ReportParameterValuesDTO.SearchByParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportParameterValuesDatahandler reportParameterValuesDatahandler = new ReportParameterValuesDatahandler(sqlTransaction);
            List<ReportParameterValuesDTO> reportParameterValuesDTOList = reportParameterValuesDatahandler.GetReportsParameterValuesList(searchParameters);
            log.LogMethodExit(reportParameterValuesDTOList);
            return reportParameterValuesDTOList;
        }
    }
}

