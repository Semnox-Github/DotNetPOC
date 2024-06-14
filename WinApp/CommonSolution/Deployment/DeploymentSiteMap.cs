/********************************************************************************************
 * Project Name - Deployment site map
 * Description  - Bussiness logic of patch application deployment site map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Feb-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace  Semnox.Parafait.Deployment
{   
    /// <summary>
    /// Manages the list of deployment site map
    /// </summary>
    public class DeploymentSiteMapList
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the deployment site map list
        /// </summary>
        public List<DeploymentSiteMapDTO> GetAllDeploymentSiteMap()
        {
            log.Debug("Starts-GetAllDeploymentSiteMap(searchParameters) method.");
             ExecutionContext deploymentPlanUserContext =  ExecutionContext.GetExecutionContext();
            DeploymentSiteMapDataHandler deploymentSiteMapDataHandler = new DeploymentSiteMapDataHandler();
            log.Debug("Ends-GetAllDeploymentSiteMap(searchParameters) method by returning the result of deploymentSiteMapDataHandler.GetDeploymentSiteMapList() call.");
            return deploymentSiteMapDataHandler.GetDeploymentSiteMapList(deploymentPlanUserContext.GetSiteId());
        }
    }
}
