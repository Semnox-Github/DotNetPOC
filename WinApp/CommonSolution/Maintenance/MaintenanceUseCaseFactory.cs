/********************************************************************************************
* Project Name - Maintenance
* Description  - MaintenanceUseCaseFactory
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   21-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
   public class MaintenanceUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// AssetGroupAssetUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IAssetGroupAssetUseCases GetAssetGroupAssets(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAssetGroupAssetUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAssetGroupAssetUseCases(executionContext);
            }
            else
            {
                result = new LocalAssetGroupAssetUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// AssetGroupAssetUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IAssetGroupUseCases GetAssetGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAssetGroupUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAssetGroupUseCases(executionContext);
            }
            else
            {
                result = new LocalAssetGroupUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// AssetTypeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IAssetTypeUseCases GetAssetTypes(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAssetTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAssetTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalAssetTypeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// JobTaskUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IJobTaskUseCases GetJobTasks(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IJobTaskUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteJobTaskUseCases(executionContext);
            }
            else
            {
                result = new LocalJobTaskUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// JobTaskGroupUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IJobTaskGroupUseCases GetJobTaskGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IJobTaskGroupUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteJobTaskGroupUseCases(executionContext);
            }
            else
            {
                result = new LocalJobTaskGroupUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// MaintenanceCommentsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IMaintenanceCommentsUseCases GetMaintenanceCommentsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMaintenanceCommentsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMaintenanceCommentsUseCases(executionContext);
            }
            else
            {
                result = new LocalMaintenanceCommentsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// MaintenanceImagesUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IMaintenanceImagesUseCases GetMaintenanceImagesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMaintenanceImagesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMaintenanceImagesUseCases(executionContext);
            }
            else
            {
                result = new LocalMaintenanceImagesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        ///// <summary>
        ///// AssetUseCases
        ///// </summary>
        ///// <param name="executionContext"></param>
        ///// <returns></returns>
        //public static IAssetUseCases GetAssets(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IAssetUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteAssetUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalAssetUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}
        ///// <summary>
        ///// AssetTechnicianMappingUseCases
        ///// </summary>
        ///// <param name="executionContext"></param>
        ///// <returns></returns>
        //public static IAssetTechnicianMappingUseCases GetAssetTechnicianMappings(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IAssetTechnicianMappingUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteAssetTechnicianMappingUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalAssetTechnicianMappingUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}
    }
}
