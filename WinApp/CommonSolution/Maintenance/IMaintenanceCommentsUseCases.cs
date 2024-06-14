/********************************************************************************************
* Project Name - Maintenance
* Description  - IMaintenanceCommentsUseCases class
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
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    public interface IMaintenanceCommentsUseCases
    {
        /// <summary>
        /// Gets the list of Maintenance Comments
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="commentId">commentId</param>
        /// <param name="commentType">commentType</param>
        /// <returns>Returns List of MaintenanceCommentsDTO</returns>
        Task<List<MaintenanceCommentsDTO>> GetMaintenanceCommentsDTOList(int jobId, string isActive = null, int commentId = -1, int commentType = -1);

        /// <summary>
        /// Saves the list of Maintenance Comments
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="maintenanceCommentsDTOList">maintenanceCommentsDTOList</param>
        /// <returns>Returns List of MaintenanceCommentsDTO</returns>
        Task<List<MaintenanceCommentsDTO>> SaveMaintenanceComments(int jobId, List<MaintenanceCommentsDTO>  maintenanceCommentsDTOList);
    }
}
