/********************************************************************************************
 * Project Name - ReportParameterInListValues   Programs 
 * Description  - Data object of the ReportParameterInListValues
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By          Remarks          
 *********************************************************************************************
 *1.00        18-April-2017   Rakshith             Created
 *2.70.2        02-Jul-2019     Dakshakh raj         Modified : Save() method Insert/Update method returns DTO.
 *                                                             Added execution Context object to the constructors.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// class of ReportParameterInListValues
    /// </summary>
    public class ReportParameterInListValues
    {
        private ReportParameterInListValuesDTO reportParameterInListValuesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportParameterInListValues()
        {
            log.LogMethodEntry();
            reportParameterInListValuesDTO = new ReportParameterInListValuesDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ReportParameterInListValues DTO parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportParameterInListValues(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            ReportParameterInListValuesDatahandler reportParameterInListValuesDatahandler = new ReportParameterInListValuesDatahandler(sqlTransaction);
            this.reportParameterInListValuesDTO = reportParameterInListValuesDatahandler.GetReportParameterInListValuesDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ReportParameterInListValues DTO parameter
        /// </summary>
        /// <param name="reportParameterInListValuesDTO">Parameter of the type ReportParameterInListValuesDTO</param>
        public ReportParameterInListValues(ReportParameterInListValuesDTO reportParameterInListValuesDTO)
        {
            log.LogMethodEntry(reportParameterInListValuesDTO);
            this.reportParameterInListValuesDTO = reportParameterInListValuesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// gets the GetReportParameterInListValuesDTO
        /// </summary>
        public ReportParameterInListValuesDTO GetReportParameterInListValuesDTO
        {
            get { return reportParameterInListValuesDTO; }
        }

        /// <summary>
        ///  Saves the Reports  
        /// Reports   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Semnox.Core.Utilities.ExecutionContext executionUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            ReportParameterInListValuesDatahandler reportParameterInListValuesDatahandler = new ReportParameterInListValuesDatahandler(sqlTransaction);

            if (reportParameterInListValuesDTO.Id < 0)
            {
                reportParameterInListValuesDTO = reportParameterInListValuesDatahandler.InsertReportParameterInlistValues(reportParameterInListValuesDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportParameterInListValuesDTO.AcceptChanges();
            }
            else
            {
                if (reportParameterInListValuesDTO.IsChanged)
                {
                    reportParameterInListValuesDTO = reportParameterInListValuesDatahandler.UpdateReportParameterInlistValues(reportParameterInListValuesDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportParameterInListValuesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Reports List
    /// </summary>
    public class ReportParameterInListValuesList
    {
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the List of ReportParameterInListValuesDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ReportParameterInListValuesDTO> GetReportsParameterValuesList(List<KeyValuePair<ReportParameterInListValuesDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportParameterInListValuesDatahandler reportParameterInListValuesDatahandler = new ReportParameterInListValuesDatahandler(sqlTransaction);
            List<ReportParameterInListValuesDTO> reportParameterInListValuesDTOs = reportParameterInListValuesDatahandler.GetReportParameterInListValuesList(searchParameters);
            log.LogMethodExit(reportParameterInListValuesDTOs);
            return reportParameterInListValuesDTOs;
        }

        /// <summary>
        /// Returns number of rows affected
        /// </summary>
        /// <param name="valueId">valueId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public int DeleteReportParameterInListValues(int valueId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(valueId, sqlTransaction);
            ReportParameterInListValuesDatahandler reportParameterInListValuesDatahandler = new ReportParameterInListValuesDatahandler(sqlTransaction);
            int values = reportParameterInListValuesDatahandler.DeleteReportParameterInListValues(valueId);
            log.LogMethodExit(values);
            return values;
        }
    }
}



