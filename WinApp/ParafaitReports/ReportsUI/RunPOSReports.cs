/********************************************************************************************
*Project Name - Parafait Report                                                                          
*Description  - RunPOSReports
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80       18-Sep-2019             Dakshakh raj                Modified : Added logs                           
*********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// RunPOSReports class
    /// </summary>
    public class RunPOSReports
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
        string reportName = string.Empty;
        int reportId = -1;

        /// <summary>
        /// RunPOSReports constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public RunPOSReports(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            Common.initEnv();
            Common.ParafaitEnv.Username = _utilities.ParafaitEnv.Username;
            Common.ParafaitEnv.LoginID = _utilities.ParafaitEnv.LoginID;
            Common.ParafaitEnv.Role = _utilities.ParafaitEnv.Role;
            Common.ParafaitEnv.User_Id = _utilities.ParafaitEnv.User_Id;
            Common.ParafaitEnv.RoleId = _utilities.ParafaitEnv.RoleId;
            log.LogMethodExit();
        }


        /// <summary>
        /// GenerateBackgroundReport method
        /// </summary>
        /// <param name="reportKey"></param>
        /// <param name="timeStamp"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="lstBackgroundParams"></param>
        /// <param name="outputFormat"></param>
        /// <returns></returns>
        public void GenerateBackgroundReport(string reportKey, string timeStamp, DateTime fromDate, DateTime toDate, List<clsReportParameters.SelectedParameterValue> lstBackgroundParams, string outputFormat = "P")
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, lstBackgroundParams, outputFormat);
            try
            {
                Common.UpdateTelerikConfigs();
                RunBackground.CreateBackgroundReport(reportKey, timeStamp, fromDate, toDate, lstBackgroundParams, outputFormat);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw new Exception(ex.Message);
            }                     
        }
    }
}
