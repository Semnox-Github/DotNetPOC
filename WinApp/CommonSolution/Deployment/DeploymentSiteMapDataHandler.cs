/********************************************************************************************
 * Project Name - Deployment Site Map Data Handler
 * Description  - Data handler of the deployment site map data handler class
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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace  Semnox.Parafait.Deployment
{
    /// <summary>
    /// Deployment site map  Data Handler - Handles insert, update and select of deployment site map data objects
    /// </summary>
    public class DeploymentSiteMapDataHandler
    {
        Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of DeploymentSiteMapDataHandler class
        /// </summary>
        public DeploymentSiteMapDataHandler()
        {
            log.Debug("Starts-DeploymentSiteMapDataHandler() default constructor.");
            dataAccessHandler = new  DataAccessHandler();
            log.Debug("Ends-DeploymentSiteMapDataHandler() default constructor.");
        }
        /// <summary>
        /// Converts the Data row object to DeploymentSiteMapDTO class type
        /// </summary>
        /// <param name="deploymentSiteMapDataRow">DeploymentSiteMapDTO DataRow</param>
        /// <returns>Returns DeploymentSiteMapDTO</returns>
        private DeploymentSiteMapDTO GetDeploymentSiteMapDTO(DataRow deploymentSiteMapDataRow)
        {
            log.Debug("Starts-GetDeploymentSiteMapDTO(deploymentSiteMapDataRow) Method.");
            DeploymentSiteMapDTO deploymentSiteMapDataObject = new DeploymentSiteMapDTO(deploymentSiteMapDataRow["PatchDeploymentPlanId"] == DBNull.Value ? -1 : Convert.ToInt32(deploymentSiteMapDataRow["PatchDeploymentPlanId"]),
                                            deploymentSiteMapDataRow["Siteid"] == DBNull.Value ? -1 : Convert.ToInt32(deploymentSiteMapDataRow["Siteid"]),
                                            deploymentSiteMapDataRow["SiteName"].ToString(),
                                            deploymentSiteMapDataRow["IsActive"] == DBNull.Value ? false : true);
            log.Debug("Ends-GetDeploymentSiteMapDTO(deploymentSiteMapDataRow) Method.");
            return deploymentSiteMapDataObject;
        }

        /// <summary>
        /// Gets the deployment site map list which is not related to the passed site id
        /// </summary>
        /// <param name="siteId">integer type parameter</param>
        /// <returns>Returns list of DeploymentSiteMapDTO</returns>
        public List<DeploymentSiteMapDTO> GetDeploymentSiteMapList(int siteId)
        {
            log.Debug("Starts-GetDeploymentSiteMap(siteId) Method.");
            string selectDeploymentSiteMapQuery = @"Select PatchDeploymentPlanId,s.site_id as SiteId,s.site_name as SiteName,dp.IsActive 
                                                    From  site s left join Patch_Application_Deployment_Plan dp on dp.SiteId = s.site_id
                                                    where s.site_id <> @siteId ";
            SqlParameter[] selectDeploymentSiteMapParameters = new SqlParameter[1];
            if (siteId==-1)
            {
                selectDeploymentSiteMapParameters[0] = new SqlParameter("@siteId", DBNull.Value);
            }
            else
            {
                selectDeploymentSiteMapParameters[0] = new SqlParameter("@siteId", siteId);
            }

            DataTable deploymentSiteMapData = dataAccessHandler.executeSelectQuery(selectDeploymentSiteMapQuery, selectDeploymentSiteMapParameters);
            if (deploymentSiteMapData.Rows.Count > 0)
            {
                List<DeploymentSiteMapDTO> deploymentSiteMapList = new List<DeploymentSiteMapDTO>();
                foreach (DataRow deploymentSiteMapDataRow in deploymentSiteMapData.Rows)
                {
                    DeploymentSiteMapDTO deploymentSiteMapDataObject = GetDeploymentSiteMapDTO(deploymentSiteMapDataRow);
                    deploymentSiteMapList.Add(deploymentSiteMapDataObject);
                }
                log.Debug("Ends-GetDeploymentSiteMapList(siteId) Method by returning deploymentSiteMapList.");
                return deploymentSiteMapList;
            }
            else
            {
                log.Debug("Ends-GetDeploymentSiteMapList(siteId) Method by returning null.");
                return null;
            }
        }        
    }
}
