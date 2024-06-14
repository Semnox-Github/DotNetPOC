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
    public class LocalMaintenanceImagesUseCases : IMaintenanceImagesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LocalMaintenanceImagesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetMaintenanceImagesDTOList
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="imageId">commentId</param>
        /// <param name="imageType">commentType</param>
        /// <returns>Returns List of MaintenanceImagesDTO</returns>
        public async Task<List<MaintenanceImagesDTO>> GetMaintenanceImagesDTOList(int jobId, string isActive = null, int imageId = -1,
                                                                                      int imageType = -1)
        {
            return await Task<List<MaintenanceImagesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(jobId, isActive, imageId, imageType);
                List<KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>> maintenanceImagesSearchParameters = new List<KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>>();
                maintenanceImagesSearchParameters.Add(new KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>(MaintenanceImagesDTO.SearchByImagesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        maintenanceImagesSearchParameters.Add(new KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>(MaintenanceImagesDTO.SearchByImagesParameters.IS_ACTIVE, isActive));
                    }
                }
                if (imageId > -1)
                {
                    maintenanceImagesSearchParameters.Add(new KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>(MaintenanceImagesDTO.SearchByImagesParameters.IMAGE_ID, imageId.ToString()));
                }
                if (jobId > -1)
                {
                    maintenanceImagesSearchParameters.Add(new KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>(MaintenanceImagesDTO.SearchByImagesParameters.MAINT_CHECK_LIST_DETAIL_ID, jobId.ToString()));
                }
                if (imageType > -1)
                {
                    maintenanceImagesSearchParameters.Add(new KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>(MaintenanceImagesDTO.SearchByImagesParameters.IMAGE_TYPE, imageType.ToString()));
                }
                MaintenanceImagesListBL maintenanceImagesListBL = new MaintenanceImagesListBL(executionContext);
                List<MaintenanceImagesDTO> maintenanceImagesDTOList = maintenanceImagesListBL.GetAllMaintenanceImages(maintenanceImagesSearchParameters);
                log.LogMethodExit(maintenanceImagesDTOList);
                return maintenanceImagesDTOList;
            });
        }

        /// <summary>
        /// SaveMaintenanceImages
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="maintenanceImagesDTOList">maintenanceImagesDTOList</param>
        /// <returns>Returns List of MaintenanceImagesDTO</returns>
        public async Task<List<MaintenanceImagesDTO>> SaveMaintenanceImages(int jobId, List<MaintenanceImagesDTO> maintenanceImagesDTOList)
        {
            return await Task<List<MaintenanceImagesDTO>>.Factory.StartNew(() =>
            {
                List<MaintenanceImagesDTO> result = new List<MaintenanceImagesDTO>();
                log.LogMethodEntry(maintenanceImagesDTOList);
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
                if (maintenanceImagesDTOList == null || maintenanceImagesDTOList.Any() == false)
                {
                    string errorMessage = "maintenanceImagesDTOList is empty or null";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    MaintenanceImagesListBL maintenanceImagesListBL = new MaintenanceImagesListBL(executionContext, maintenanceImagesDTOList);
                    result = maintenanceImagesListBL.Save(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
