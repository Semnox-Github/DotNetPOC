/********************************************************************************************
* Project Name - Maintenance
* Description  - RemoteMaintenanceCommentsUseCases class
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.150.3     07-Mar-2023   Abhishek                Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Maintenance
{
    public class RemoteMaintenanceImagesUseCases : RemoteUseCases, IMaintenanceImagesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MAINTENANCE_IMAGES_URL = "api/Maintenance/{jobId}/Images";

        /// <summary>
        /// Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RemoteMaintenanceImagesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetMaintenanceImagesDTOList
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="imageId">imageId</param>
        /// <param name="imageType">imageType</param>
        /// <returns>Returns List of MaintenanceImagesDTO</returns>
        public async Task<List<MaintenanceImagesDTO>> GetMaintenanceImagesDTOList(int jobId, string isActive = null, int imageId = -1, 
                                                                                  int imageType = -1)
        {
            log.LogMethodEntry(jobId, isActive, imageId, imageType);
            List<MaintenanceImagesDTO> result = await Get<List<MaintenanceImagesDTO>>(MAINTENANCE_IMAGES_URL.Replace("{jobId}", jobId.ToString()),
                                                                                                             new WebApiGetRequestParameterCollection("isActive",
                                                                                                                                                     isActive,
                                                                                                                                                     "imageId",
                                                                                                                                                     imageId,
                                                                                                                                                     "imageType",
                                                                                                                                                     imageType));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// SaveMaintenanceImages
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="maintenanceImagesDTOList">maintenanceImagesDTOList</param>
        /// <returns>Returns List of MaintenanceImagesDTO</returns>
        public async Task<List<MaintenanceImagesDTO>> SaveMaintenanceImages(int jobId, List<MaintenanceImagesDTO> maintenanceImagesDTOList)
        {
            log.LogMethodEntry(jobId, maintenanceImagesDTOList);
            List<MaintenanceImagesDTO> result = await Post<List<MaintenanceImagesDTO>>(MAINTENANCE_IMAGES_URL.Replace("{jobId}",
                                                                                           jobId.ToString()), maintenanceImagesDTOList);
            log.LogMethodExit(result);
            return result;
        }
    }
}
