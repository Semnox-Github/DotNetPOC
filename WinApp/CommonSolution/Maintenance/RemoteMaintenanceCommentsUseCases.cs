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
    public class RemoteMaintenanceCommentsUseCases : RemoteUseCases, IMaintenanceCommentsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MAINTENANCE_COMMENTS_URL = "api/Maintenance/{jobId}/Comments";

        /// <summary>
        /// Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RemoteMaintenanceCommentsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetMaintenanceCommentsDTOList
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="commentId">commentId</param>
        /// <param name="commentType">commentType</param>
        /// <returns>Returns List of MaintenanceCommentsDTO</returns>
        public async Task<List<MaintenanceCommentsDTO>> GetMaintenanceCommentsDTOList(int jobId, string isActive = null, int commentId = -1,
                                                                                      int commentType = -1)
        {
            log.LogMethodEntry(jobId, isActive, commentId, commentType);
            List<MaintenanceCommentsDTO> result = await Get<List<MaintenanceCommentsDTO>>(MAINTENANCE_COMMENTS_URL.Replace("{jobId}", jobId.ToString()),
                                                                                                             new WebApiGetRequestParameterCollection("isActive",
                                                                                                                                                     isActive,
                                                                                                                                                     "commentId",
                                                                                                                                                     commentId,
                                                                                                                                                     "commentType",
                                                                                                                                                     commentType));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// SaveMaintenanceComments
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="maintenanceCommentsDTOList">maintenanceCommentsDTOList</param>
        /// <returns>Returns List of MaintenanceCommentsDTO</returns>
        public async Task<List<MaintenanceCommentsDTO>> SaveMaintenanceComments(int jobId, List<MaintenanceCommentsDTO> maintenanceCommentsDTOList)
        {
            log.LogMethodEntry(jobId, maintenanceCommentsDTOList);
            List<MaintenanceCommentsDTO> result = await Post<List<MaintenanceCommentsDTO>>(MAINTENANCE_COMMENTS_URL.Replace("{jobId}",
                                                                                           jobId.ToString()), maintenanceCommentsDTOList);
            log.LogMethodExit(result);
            return result;
        }
    }
}
