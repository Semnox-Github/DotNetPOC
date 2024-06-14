/********************************************************************************************
* Project Name - CMSPeges BL   
* Description  - Business logic of the CMSPeges BL class
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        06-Apr-2016    Rakshith          Created 
*2.70        09-Jul-2019    Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
*                                                          LogMethodEntry() and LogMethodExit(). 
*2.80        08-May-2020    Indrajeet Kumar   Modified : GetAllPages() Method - To Load CMSBanner & CMSContent.
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Linq;

namespace Semnox.Parafait.WebCMS
{
    public class CMSPagesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private CMSPagesDTO cmsPagesDTO;
        /// <summary>
        /// Default constructor
        /// </summary>
        private CMSPagesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;            
            log.LogMethodExit();
        }

        public CMSPagesBL(ExecutionContext executionContext, CMSPagesDTO cMSPagesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cMSPagesDTO);
            this.cmsPagesDTO = cMSPagesDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSPeges"'  Request  ====>  ""CMSPeges"" DataHandler
        public CMSPagesBL(ExecutionContext executionContext, int pageId, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(pageId, sqlTransaction);
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
            cmsPagesDTO = cmsPagesDataHandler.GetcmsPages(pageId);

            if (cmsPagesDTO.PageId != -1)
            {
                List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>>();
                searchParameters.Add(new KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>(CMSContentDTO.SearchByRequestParameters.PAGE_ID, pageId.ToString()));
                cmsPagesDTO.CMSContentDTOList = new CMSContentBLList(executionContext).GetAllCmsContent(searchParameters);
                if (cmsPagesDTO.CMSContentDTOList != null && cmsPagesDTO.CMSContentDTOList.Count > 0)
                {
                    foreach (var cmsContent in cmsPagesDTO.CMSContentDTOList)
                    {
                        if (cmsContent.ContentTemplateId != -1)
                        {
                            CMSContentTemplateBL obj = new CMSContentTemplateBL(executionContext,cmsContent.ContentTemplateId);
                            cmsContent.CMSContentTemplateDTO = obj.getCMSContentTemplateDTO;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        //Used For Save 
        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsPagesDTO.IsChangedRecursive == false && cmsPagesDTO.PageId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);

            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsPagesDTO.PageId < 0)
            {
                cmsPagesDTO = cmsPagesDataHandler.InsertPages(cmsPagesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsPagesDTO.AcceptChanges();
            }
            else if (cmsPagesDTO.IsChanged)
            {
                cmsPagesDTO = cmsPagesDataHandler.UpdatePages(cmsPagesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsPagesDTO.AcceptChanges();
            }
            SavePageChild(sqlTransaction);
            log.LogMethodExit();
        }

        private void SavePageChild(SqlTransaction sqlTransaction)
        {
            if (cmsPagesDTO.CMSContentDTOList != null && cmsPagesDTO.CMSContentDTOList.Any())
            {
                List<CMSContentDTO> updatedCMSContentDTOList = new List<CMSContentDTO>();
                foreach (var cMSContentDTO in cmsPagesDTO.CMSContentDTOList)
                {
                    if (cMSContentDTO.PageId != cmsPagesDTO.PageId)
                    {
                        cMSContentDTO.PageId = cmsPagesDTO.PageId;
                    }
                    if (cMSContentDTO.IsChanged)
                    {
                        updatedCMSContentDTOList.Add(cMSContentDTO);
                    }
                }
                if (updatedCMSContentDTOList.Any())
                {
                    CMSContentBLList cMSContentBLList = new CMSContentBLList(executionContext, updatedCMSContentDTOList);
                    cMSContentBLList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the details
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Delete the record from the database based on  pageId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
                int id = cmsPagesDataHandler.cmsPageDelete(cmsPagesDTO.PageId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception expn)
            {
                log.Error("Error  at Delete() method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the CMSPagesDTO
        /// </summary>
        public CMSPagesDTO GetCmsPagesDTO
        {
            get { return cmsPagesDTO; }
        }
    }

    /// <summary>
    /// Manages the list of CMSPages List
    /// </summary>
    public class CMSPagesBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CMSPagesDTO> cMSPagesDTOList = new List<CMSPagesDTO>();
        private List<CMSBannersDTO> cMSBannersDTOList = new List<CMSBannersDTO>();
        private List<CMSContentDTO> cMSContentDTOList = new List<CMSContentDTO>();
        private ExecutionContext executionContext;

        public CMSPagesBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.cMSPagesDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public CMSPagesBLList(ExecutionContext executionContext, List<CMSPagesDTO> cMSPagesDTOList)
        {
            log.LogMethodEntry();
            this.cMSPagesDTOList = cMSPagesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSPagesDTO> GetAllPages(List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, activeChildRecords, sqlTransaction);
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
            List<CMSPagesDTO> cMSPagesDTOList = cmsPagesDataHandler.GetPagesList(searchParameters);
            if (cMSPagesDTOList != null && cMSPagesDTOList.Any())
            {
                foreach (CMSPagesDTO cMSPagesDTO in cMSPagesDTOList)
                {
                    List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> searchParam = new List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>>();
                    searchParam.Add(new KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>(CMSContentDTO.SearchByRequestParameters.PAGE_ID, cMSPagesDTO.PageId.ToString()));
                    if (activeChildRecords)
                    {
                        searchParam.Add(new KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>(CMSContentDTO.SearchByRequestParameters.ACTIVE, "1"));
                    }

                    CMSContentBLList cMSContentBLList = new CMSContentBLList(executionContext);
                    cMSContentDTOList = cMSContentBLList.GetAllCmsContent(searchParam);
                    cMSPagesDTO.CMSContentDTOList = cMSContentDTOList;

                    if (cMSPagesDTO.BannerId != -1)
                    {
                        List<KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>> searchParameter = new List<KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>>();
                        searchParameter.Add(new KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>(CMSBannersDTO.SearchByRequestParameters.BANNER_ID, cMSPagesDTO.BannerId.ToString()));
                        if (activeChildRecords)
                        {
                            searchParameter.Add(new KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>(CMSBannersDTO.SearchByRequestParameters.ACTIVE, "1"));
                        }
                        CMSBannerBLList cMSBannerList = new CMSBannerBLList(executionContext);
                        cMSBannersDTOList = cMSBannerList.GetAllCmsBanners(searchParameter);

                        foreach (CMSBannersDTO cMSBannersDTO in cMSBannersDTOList)
                        {
                            cMSPagesDTO.CMSBannersDTO = cMSBannersDTO;
                        }
                    }
                }
            }
            log.LogMethodExit(cMSPagesDTOList);
            return cMSPagesDTOList;
        }

        /// <summary>
        /// Returns the CMSPagesDTOTree Object using PageID
        /// </summary>
        public CMSPagesDTOTree GetPagesById(int pageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pageId, sqlTransaction);
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
            CMSPageParams cMSPageParams = new CMSPageParams();
            cMSPageParams.PageId = pageId;
            cMSPageParams.ShowContents = true;
            CMSPagesDTOTree cMSPagesDTOTree = cmsPagesDataHandler.GetCMSPages(cMSPageParams);
            log.LogMethodExit(cMSPagesDTOTree);
            return cMSPagesDTOTree;
        }

        /// <summary>
        /// Returns the CMSPagesDTOTree Object using PageName
        /// </summary>
        public CMSPagesDTOTree GetPagesByName(CMSPageParams cMSPageParams, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cMSPageParams, sqlTransaction);
            try
            {
                CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
                CMSPagesDTOTree cMSPagesDTOTree = new CMSPagesDTOTree();

                string pagename = cMSPageParams.PageName;
                cMSPageParams.PageName = "master";
                cMSPagesDTOTree = cmsPagesDataHandler.GetCMSPages(cMSPageParams);
                cMSPagesDTOTree.Title = string.Empty;

                cMSPageParams.PageName = pagename;
                CMSPagesDTOTree cMSPagesDTOTreeChild = cmsPagesDataHandler.GetCMSPages(cMSPageParams);

                foreach (CMSContentDTO contentDTO in cMSPagesDTOTreeChild.CMSContentDTOList)
                {
                    if (!String.IsNullOrEmpty(contentDTO.ContentKey))
                        cMSPagesDTOTree.CMSContentDTOList.RemoveAll(e => e.ContentKey == contentDTO.ContentKey);
                }

                foreach (CMSContentDTO contentDTO in cMSPagesDTOTreeChild.CMSContentDTOList)
                {
                    cMSPagesDTOTree.CMSContentDTOList.Add(contentDTO);
                }

                if (cMSPageParams.LanguageId != -1 && cMSPagesDTOTree.CMSContentDTOList != null)
                {
                    List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, cMSPageParams.LanguageId.ToString()));
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, "CMSCONTENTS".ToUpper()));
                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(ExecutionContext.GetExecutionContext());
                    List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(searchParameters);

                    if (objectTranslationsDTOList != null && objectTranslationsDTOList.Count > 0)
                    {
                        foreach (CMSContentDTO contentDTO in cMSPagesDTOTree.CMSContentDTOList)
                        {
                            int contentIndex = objectTranslationsDTOList.FindIndex(e => e.ElementGuid == contentDTO.Guid);
                            if (contentIndex != -1)
                            {
                                contentDTO.Source = objectTranslationsDTOList[contentIndex].Translation;
                            }
                        }
                    }

                }

                if (cMSPagesDTOTreeChild != null)
                {
                    cMSPagesDTOTree.GroupId = cMSPagesDTOTreeChild.GroupId;
                    cMSPagesDTOTree.PageName = cMSPagesDTOTreeChild.PageName;

                    if (!String.IsNullOrWhiteSpace(cMSPagesDTOTreeChild.MetaTitle))
                        cMSPagesDTOTree.MetaTitle = cMSPagesDTOTreeChild.MetaTitle;

                    if (!String.IsNullOrWhiteSpace(cMSPagesDTOTreeChild.MetaDesc))
                        cMSPagesDTOTree.MetaDesc = cMSPagesDTOTreeChild.MetaDesc;

                    if (!String.IsNullOrWhiteSpace(cMSPagesDTOTreeChild.MetaKeywords))
                        cMSPagesDTOTree.MetaKeywords = cMSPagesDTOTreeChild.MetaKeywords;

                    if (!String.IsNullOrWhiteSpace(cMSPagesDTOTreeChild.Title))
                        cMSPagesDTOTree.Title = cMSPagesDTOTreeChild.Title;
                }
                return cMSPagesDTOTree;
            }
            catch (Exception expn)
            {
                log.Error("Error  at GetPagesByName(CMSPageParams cMSPageParams, SqlTransaction sqlTransaction = null) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the  list of .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSPagesDTOList == null ||
                cMSPagesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < cMSPagesDTOList.Count; i++)
            {
                var cMSPagesDTO = cMSPagesDTOList[i];                
                try
                {
                    CMSPagesBL cMSPages = new CMSPagesBL(executionContext, cMSPagesDTO);
                    cMSPages.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving customerFeedbackSurveyDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("customerFeedbackSurveyDTO", cMSPagesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
