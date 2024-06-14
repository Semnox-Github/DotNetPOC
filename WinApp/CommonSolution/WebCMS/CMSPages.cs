
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
 *2.70        09-Jul-2019    Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                          LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSPages
    {

        private CMSPagesDTO cmsPagesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSPages()
        {
            log.LogMethodEntry();
            cmsPagesDTO = new CMSPagesDTO();
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSPeges"'  Request  ====>  ""CMSPeges"" DataHandler
        public CMSPages(int pageId, SqlTransaction sqlTransaction = null) : this()
        {
            log.LogMethodEntry(pageId, sqlTransaction);
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
            cmsPagesDTO = cmsPagesDataHandler.GetcmsPages(pageId);

            if (cmsPagesDTO.PageId != -1)
            {
                List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>>();
                searchParameters.Add(new KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>(CMSContentDTO.SearchByRequestParameters.PAGE_ID, pageId.ToString()));
                cmsPagesDTO.PageContents = new CMSContentList().GetAllCmsContent(searchParameters);
                if (cmsPagesDTO.PageContents != null && cmsPagesDTO.PageContents.Count > 0)
                {
                    foreach (var cmsContent in cmsPagesDTO.PageContents)
                    {
                        if (cmsContent.ContentTemplateId != -1)
                        {
                            CMSContentTemplateBL obj = new CMSContentTemplateBL(cmsContent.ContentTemplateId);
                            cmsContent.CMSContentTemplateDTO = obj.getCMSContentTemplateDTO;
                        }
                    }
                }

            }

            log.LogMethodExit();
        }

        //Constructor Initializes with Corresponding Object
        public CMSPages(CMSPagesDTO cmsPagesDTO) : this()
        {
            log.LogMethodEntry(cmsPagesDTO);
            this.cmsPagesDTO = cmsPagesDTO;
            log.LogMethodExit();
        }

        //Used For Save 
        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);

            try
            {
                if (cmsPagesDTO.PageId < 0)
                {
                    cmsPagesDTO = cmsPagesDataHandler.InsertPages(cmsPagesDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsPagesDTO.AcceptChanges();
                    return cmsPagesDTO.PageId;

                }
                else
                {
                    if (cmsPagesDTO.IsChanged)
                    {
                        cmsPagesDTO = cmsPagesDataHandler.UpdatePages(cmsPagesDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsPagesDTO.AcceptChanges();
                        return cmsPagesDTO.PageId;
                    }
                    log.LogMethodExit(0);
                    return 0;
                }
            }
            catch (Exception expn)
            {
                log.Error("Error  at Save() method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the record from the database based on  pageId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int pageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pageId, sqlTransaction);
            try
            {
                CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
                int id = cmsPagesDataHandler.cmsPageDelete(pageId);
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
    public class CMSPagesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSPagesDTO> GetAllPages(List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CMSPagesDataHandler cmsPagesDataHandler = new CMSPagesDataHandler(sqlTransaction);
            List<CMSPagesDTO> cMSPagesDTOList = cmsPagesDataHandler.GetPagesList(searchParameters);
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

                foreach (CMSContentDTO contentDTO in cMSPagesDTOTreeChild.PageContents)
                {
                    if (!String.IsNullOrEmpty(contentDTO.ContentKey))
                        cMSPagesDTOTree.PageContents.RemoveAll(e => e.ContentKey == contentDTO.ContentKey);
                }

                foreach (CMSContentDTO contentDTO in cMSPagesDTOTreeChild.PageContents)
                {
                    cMSPagesDTOTree.PageContents.Add(contentDTO);
                }

                if (cMSPageParams.LanguageId != -1 && cMSPagesDTOTree.PageContents != null)
                {
                    List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, cMSPageParams.LanguageId.ToString()));
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT, "CMSCONTENTS".ToUpper()));
                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(ExecutionContext.GetExecutionContext());
                    List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(searchParameters);

                    if (objectTranslationsDTOList != null && objectTranslationsDTOList.Count > 0)
                    {
                        foreach (CMSContentDTO contentDTO in cMSPagesDTOTree.PageContents)
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

    }
}
