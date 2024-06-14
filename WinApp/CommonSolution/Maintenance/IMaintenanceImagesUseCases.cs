/********************************************************************************************
* Project Name - Maintenance
* Description  - IMaintenanceImagesUseCases class
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

namespace Semnox.Parafait.Maintenance
{
    public interface IMaintenanceImagesUseCases
    {
        /// <summary>
        /// Gets the list of Maintenance Images
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="imageId">imageId</param>
        /// <param name="imageType">imageType</param>
        /// <returns>Returns List of MaintenanceImagesDTO</returns>
        Task<List<MaintenanceImagesDTO>> GetMaintenanceImagesDTOList(int jobId, string isActive = null, int imageId = -1, int imageType = -1);

        /// <summary>
        /// Saves the list of Maintenance Images
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="maintenanceImagesDTOList">maintenanceImagesDTOList</param>
        /// <returns>Returns List of MaintenanceImagesDTO</returns>
        Task<List<MaintenanceImagesDTO>> SaveMaintenanceImages(int jobId, List<MaintenanceImagesDTO>  maintenanceImagesDTOList);
    }
}
