/********************************************************************************************
 * Project Name - Auto Patch Log
 * Description  - Bussiness logic of auto patch log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Feb-2016   Raghuveera          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    ///  Auto patch log is creating
    /// </summary>
    public class AutoPatchLog
    {
        private AutoPatchLogDTO autoPatchLogDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public AutoPatchLog()
        {
            log.Debug("Starts-AutoPatchLog() default constructor");
            autoPatchLogDTO = null;
            log.Debug("Ends-AutoPatchLog() default constructor");
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="autoPatchLogDTO">Parameter of the type AutoPatchLogDTO</param>
        public AutoPatchLog(AutoPatchLogDTO autoPatchLogDTO)
        {
            log.Debug("Starts-AutoPatchLog(autoPatchLogDTO) parameterized constructor.");
            this.autoPatchLogDTO = autoPatchLogDTO;
            log.Debug("Ends-AutoPatchLog(autoPatchLogDTO) parameterized constructor.");
        }
        /// <summary>
        /// Saves the auto patch log
        /// deployment plan will be inserted if LogId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            AutoPatchLogDataHandler patchApplicationDeploymentPlanDataHandler = new AutoPatchLogDataHandler();
            if (autoPatchLogDTO.LogId <= 0)
            {
                int patchApplicationDeploymentPlanId = patchApplicationDeploymentPlanDataHandler.InsertAutoPatchLog(autoPatchLogDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                autoPatchLogDTO.LogId = patchApplicationDeploymentPlanId;
            }
            log.Debug("Ends-Save() method.");
        }
    }
}
