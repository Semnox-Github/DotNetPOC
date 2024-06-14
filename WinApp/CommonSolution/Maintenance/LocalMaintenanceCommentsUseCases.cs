/********************************************************************************************
* Project Name - Maintenance
* Description  - LocalMaintenanceCommentsUseCases class
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
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    public class LocalMaintenanceCommentsUseCases : IMaintenanceCommentsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LocalMaintenanceCommentsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
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
            return await Task<List<MaintenanceCommentsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(jobId, isActive, commentId, commentType);
                List<KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>> maintenanceCommentsSearchParameters = new List<KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>>();
                maintenanceCommentsSearchParameters.Add(new KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>(MaintenanceCommentsDTO.SearchByCommentsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        maintenanceCommentsSearchParameters.Add(new KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>(MaintenanceCommentsDTO.SearchByCommentsParameters.IS_ACTIVE, isActive));
                    }
                }
                if (commentId > -1)
                {
                    maintenanceCommentsSearchParameters.Add(new KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>(MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT_ID, commentId.ToString()));
                }
                if (jobId > -1)
                {
                    maintenanceCommentsSearchParameters.Add(new KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>(MaintenanceCommentsDTO.SearchByCommentsParameters.MAINT_CHECK_LIST_DETAIL_ID, jobId.ToString()));
                }
                if (commentType > -1)
                {
                    maintenanceCommentsSearchParameters.Add(new KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>(MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT_TYPE, commentType.ToString()));
                }
                MaintenanceCommentsListBL maintenanceCommentsListBL = new MaintenanceCommentsListBL(executionContext);
                List<MaintenanceCommentsDTO> maintenanceCommentsDTOList = maintenanceCommentsListBL.GetAllMaintenanceComments(maintenanceCommentsSearchParameters);
                log.LogMethodExit(maintenanceCommentsDTOList);
                return maintenanceCommentsDTOList;
            });
        }

        /// <summary>
        /// SaveMaintenanceComments
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="maintenanceCommentsDTOList">maintenanceCommentsDTOList</param>
        /// <returns>Returns List of MaintenanceCommentsDTO</returns>
        public async Task<List<MaintenanceCommentsDTO>> SaveMaintenanceComments(int jobId, List<MaintenanceCommentsDTO> maintenanceCommentsDTOList)
        {
            return await Task<List<MaintenanceCommentsDTO>>.Factory.StartNew(() =>
            {
                List<MaintenanceCommentsDTO> result = new List<MaintenanceCommentsDTO>();
                log.LogMethodEntry(maintenanceCommentsDTOList);
                if (jobId > -1)
                {
                    UserJobItemsDatahandler userJobItemsDataHandler = new UserJobItemsDatahandler(null);
                    UserJobItemsDTO userJobItemsDTO = userJobItemsDataHandler.GetUserJobItemsDTO(jobId);
                    if (userJobItemsDTO == null)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2196, "UserJobItems", jobId);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new ValidationException(message);
                    }
                }
                else
                {
                    string errorMessage = "UserJobItemsDTO with Job Id "+ jobId + "is not valid.";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                if (maintenanceCommentsDTOList == null || maintenanceCommentsDTOList.Any() == false)
                {
                    string errorMessage = "maintenanceCommentsDTOList is empty or null";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    MaintenanceCommentsListBL maintenanceCommentsListBL = new MaintenanceCommentsListBL(executionContext, maintenanceCommentsDTOList);
                    result = maintenanceCommentsListBL.Save(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
