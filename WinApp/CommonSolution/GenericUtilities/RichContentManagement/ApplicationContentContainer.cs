/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - ApplicationContentContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentContainer 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<ApplicationContentDTO> applicationContentDTOList;
        private readonly int siteId;
        private readonly Dictionary<int, ApplicationContentContainerDTOCollection> languageIdApplicationContentContainerDTOCollectionDictionary = new Dictionary<int, ApplicationContentContainerDTOCollection>();
        private readonly HashSet<int> languageIdHashSet = new HashSet<int>() { -1 };
        private readonly DateTime? applicationContentModuleLastUpdateTime;
        private readonly Dictionary<int, ApplicationContentDTO> applicationContentIdApplicationContentDTODictionary = new Dictionary<int, ApplicationContentDTO>();
        private readonly Dictionary<int, List<ApplicationContentTranslatedDTO>> languageIdContentTranslatedDTODictionary = new Dictionary<int, List<ApplicationContentTranslatedDTO>>();
        //private readonly ApplicationContentContainerDTOCollection applicationContentContainerDTOCollection;
        private readonly Dictionary<int, ApplicationContentContainerDTO> applicationContentIdapplicationContentContainerDTODictionary = new Dictionary<int, ApplicationContentContainerDTO>();
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        public ApplicationContentContainer(int siteId) : this(siteId, GetApplicationContentDTOList(siteId), GetApplicationContentModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public ApplicationContentContainer(int siteId, List<ApplicationContentDTO> applicationContentDTOList, DateTime? applicationContentModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.applicationContentDTOList = applicationContentDTOList;
            this.applicationContentModuleLastUpdateTime = applicationContentModuleLastUpdateTime;
            foreach (var applicationContentDTO in applicationContentDTOList)
            {
                if (applicationContentIdApplicationContentDTODictionary.ContainsKey(applicationContentDTO.AppContentId))
                {
                    continue;
                }
                applicationContentIdApplicationContentDTODictionary.Add(applicationContentDTO.AppContentId, applicationContentDTO);
            }
            List<ApplicationContentContainerDTO> applicationContentContainerDTOList = new List<ApplicationContentContainerDTO>();
            foreach (ApplicationContentDTO applicationContentDTO in applicationContentDTOList)
            {
                if (applicationContentIdapplicationContentContainerDTODictionary.ContainsKey(applicationContentDTO.AppContentId))
                {
                    continue;
                }
                ApplicationContentContainerDTO applicationContentContainerDTO = new ApplicationContentContainerDTO(applicationContentDTO.AppContentId,
                    applicationContentDTO.Application, applicationContentDTO.Module, applicationContentDTO.Chapter, applicationContentDTO.RichContentDTO.FileName, applicationContentDTO.ContentId);
                applicationContentContainerDTOList.Add(applicationContentContainerDTO);
                applicationContentIdapplicationContentContainerDTODictionary.Add(applicationContentDTO.AppContentId, applicationContentContainerDTO);
                if (applicationContentDTO.ApplicationContentTranslatedDTOList != null && applicationContentDTO.ApplicationContentTranslatedDTOList.Any())
                {
                    foreach (var translatedApplicationContentDTO in applicationContentDTO.ApplicationContentTranslatedDTOList)
                    {
                        if (languageIdContentTranslatedDTODictionary.ContainsKey(translatedApplicationContentDTO.LanguageId))
                        {
                            languageIdContentTranslatedDTODictionary[translatedApplicationContentDTO.LanguageId].Add(translatedApplicationContentDTO);
                        }
                        else
                        {
                            languageIdContentTranslatedDTODictionary.Add(translatedApplicationContentDTO.LanguageId, new List<ApplicationContentTranslatedDTO>(){ translatedApplicationContentDTO });
                        }
                        if (!languageIdHashSet.Contains(translatedApplicationContentDTO.LanguageId))
                        {
                            languageIdHashSet.Add(translatedApplicationContentDTO.LanguageId);
                        }
                    }
                }
            }
            languageIdApplicationContentContainerDTOCollectionDictionary.Add(-1, new ApplicationContentContainerDTOCollection(applicationContentContainerDTOList));
            //applicationContentContainerDTOCollection = new ApplicationContentContainerDTOCollection(applicationContentContainerDTOList);
            foreach (var languageId in languageIdHashSet)
            {
                if (languageId != -1)
                {
                    List<ApplicationContentContainerDTO> langapplicationContentContainerDTOList = new List<ApplicationContentContainerDTO>();
                    if (languageIdContentTranslatedDTODictionary.ContainsKey(languageId))
                    {
                        foreach (var langtranslatedDTO in languageIdContentTranslatedDTODictionary[languageId])
                        {
                            if (applicationContentIdapplicationContentContainerDTODictionary.ContainsKey(langtranslatedDTO.AppContentId))
                            {
                                ApplicationContentContainerDTO langapplicationContentContainerDTO = new ApplicationContentContainerDTO(langtranslatedDTO.AppContentId,
                            applicationContentIdapplicationContentContainerDTODictionary[langtranslatedDTO.AppContentId].Application, applicationContentIdapplicationContentContainerDTODictionary[langtranslatedDTO.AppContentId].Module, langtranslatedDTO.Chapter, "", langtranslatedDTO.ContentId);
                                RichContentListBL richContentListBL = new RichContentListBL();
                                List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RichContentDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                                searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ID, langtranslatedDTO.ContentId.ToString()));
                                List<RichContentDTO> richcontentDTO = richContentListBL.GetRichContentDTOList(searchParameters);
                                if (richcontentDTO != null && richcontentDTO.Any())
                                {
                                    langapplicationContentContainerDTO.FileName = richcontentDTO[0].FileName;
                                }
                                langapplicationContentContainerDTOList.Add(langapplicationContentContainerDTO);
                            }
                        }
                    }
                    foreach (int applicationContentId in applicationContentIdapplicationContentContainerDTODictionary.Keys)
                    {
                        if (!langapplicationContentContainerDTOList.Any(x => x.ApplicationContentId == applicationContentId))
                        {
                            langapplicationContentContainerDTOList.Add(applicationContentIdapplicationContentContainerDTODictionary[applicationContentId]);
                        }
                    }
                    languageIdApplicationContentContainerDTOCollectionDictionary.Add(languageId, new ApplicationContentContainerDTOCollection(langapplicationContentContainerDTOList));
                }
            }
            log.LogMethodExit();
        }

        private static List<ApplicationContentDTO> GetApplicationContentDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ApplicationContentDTO> applicationContentDTOList = null;
            try
            {
                ApplicationContentListBL applicationContentList = new ApplicationContentListBL();
                List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                applicationContentDTOList = applicationContentList.GetApplicationContentDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the ApplicationContent.", ex);
            }

            if (applicationContentDTOList == null)
            {
                applicationContentDTOList = new List<ApplicationContentDTO>();
            }
            log.LogMethodExit(applicationContentDTOList);
            return applicationContentDTOList;
        }
        private static DateTime? GetApplicationContentModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ApplicationContentListBL applicationContentList = new ApplicationContentListBL();
                result = applicationContentList.GetApplicationContentLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the ApplicationContent max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }


        //internal ApplicationContentContainer(int siteId)
        //{
        //    log.LogMethodEntry(siteId);
        //    this.siteId = siteId;
        //    applicationContentDTODictionary = new ConcurrentDictionary<int, ApplicationContentDTO>();
        //    applicationContentDTOList = new List<ApplicationContentDTO>();
        //    ApplicationContentListBL applicationContentList = new ApplicationContentListBL(executionContext);
        //    applicationContentModuleLastUpdateTime = applicationContentList.GetApplicationContentLastUpdateTime(siteId);

        //    List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>>();
        //    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.ACTIVE_FLAG, "1"));
        //    searchParameters.Add(new KeyValuePair<ApplicationContentDTO.SearchByParameters, string>(ApplicationContentDTO.SearchByParameters.SITE_ID, siteId.ToString()));
        //    applicationContentDTOList = applicationContentList.GetApplicationContentDTOList(searchParameters, true, true);

        //    if (applicationContentDTOList != null && applicationContentDTOList.Any())
        //    {
        //        foreach (ApplicationContentDTO applicationContentDTO in applicationContentDTOList)
        //        {
        //            applicationContentDTODictionary[applicationContentDTO.AppContentId] = applicationContentDTO;
        //        }
        //    }
        //    else
        //    {
        //        applicationContentDTOList = new List<ApplicationContentDTO>();
        //        applicationContentDTODictionary = new ConcurrentDictionary<int, ApplicationContentDTO>();
        //    }
        //    log.LogMethodExit();
        //}

        public ApplicationContentContainerDTO GetApplicationContentContainerDTO(int applicationContentId)
        {
            log.LogMethodEntry(applicationContentId);
            if (applicationContentIdapplicationContentContainerDTODictionary.ContainsKey(applicationContentId) == false)
            {
                string errorMessage = "applicationContent with applicationContent Id :" + applicationContentId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ApplicationContentContainerDTO result = applicationContentIdapplicationContentContainerDTODictionary[applicationContentId]; ;
            log.LogMethodExit(result);
            return result;
        }
        public ApplicationContentContainerDTOCollection GetApplicationContentContainerDTOCollection(int languageId)
        {
            log.LogMethodEntry(languageId);
            ApplicationContentContainerDTOCollection result;
            if (languageIdApplicationContentContainerDTOCollectionDictionary.ContainsKey(languageId))
            {
                result = languageIdApplicationContentContainerDTOCollectionDictionary[languageId];
            }
            else if (languageIdApplicationContentContainerDTOCollectionDictionary.ContainsKey(-1))
            {
                result = languageIdApplicationContentContainerDTOCollectionDictionary[-1];
            }
            else
            {
                result = new ApplicationContentContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<ApplicationContentContainerDTO> GetApplicationContentContainerDTOList(int languageId)
        {
            log.LogMethodEntry();
            Dictionary<int, ApplicationContentDTO> applicationContentDTODictionary = new Dictionary<int, ApplicationContentDTO>();
            List<ApplicationContentContainerDTO> applicationContentContainerDTOList = new List<ApplicationContentContainerDTO>();

            foreach (ApplicationContentDTO applicationContentDTO in applicationContentDTOList)
            {
                ApplicationContentContainerDTO applicationContentContainerDTO = new ApplicationContentContainerDTO(
                    applicationContentDTO.AppContentId, applicationContentDTO.Application, applicationContentDTO.Module, "",
                    "", -1);
                if (applicationContentDTO.RichContentDTO != null)
                {
                    applicationContentContainerDTO.FileName = applicationContentDTO.RichContentDTO.FileName;
                }
                if (applicationContentDTO.ApplicationContentTranslatedDTOList != null && applicationContentDTO.ApplicationContentTranslatedDTOList.Any())
                {
                    foreach (ApplicationContentTranslatedDTO applicationContentTranslatedDTO in applicationContentDTO.ApplicationContentTranslatedDTOList)
                    {
                        applicationContentContainerDTO.Chapter = languageId == applicationContentTranslatedDTO.LanguageId ?
                            applicationContentDTO.ApplicationContentTranslatedDTOList.Where(f => f.LanguageId == languageId).Select(n => n.Chapter).SingleOrDefault() :
                            applicationContentDTO.ApplicationContentTranslatedDTOList.Where(f => f.LanguageId == applicationContentTranslatedDTO.LanguageId).Select(n => n.Chapter).SingleOrDefault();

                        applicationContentContainerDTO.ContentId = languageId == applicationContentTranslatedDTO.LanguageId ?
                            applicationContentDTO.ApplicationContentTranslatedDTOList.Where(f => f.LanguageId == languageId).Select(n => n.ContentId).SingleOrDefault() :
                             applicationContentDTO.ApplicationContentTranslatedDTOList.Where(f => f.LanguageId == applicationContentTranslatedDTO.LanguageId).Select(n => n.ContentId).SingleOrDefault(); //SingleOrDefault

                    }
                }

                applicationContentContainerDTOList.Add(applicationContentContainerDTO);
            }
            log.LogMethodExit(applicationContentContainerDTOList);
            return applicationContentContainerDTOList;
        }
        public ApplicationContentContainer Refresh()
        {
            log.LogMethodEntry();
            ApplicationContentListBL applicationContentList = new ApplicationContentListBL();
            DateTime? updateTime = applicationContentList.GetApplicationContentLastUpdateTime(siteId);
            if (applicationContentModuleLastUpdateTime.HasValue
                && applicationContentModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in ApplicationContent since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ApplicationContentContainer result = new ApplicationContentContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
