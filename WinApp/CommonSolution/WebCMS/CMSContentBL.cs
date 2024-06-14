/********************************************************************************************
* Project Name - CMSContent BL Class
* Description  - Bussiness logic of the CMSContent BL Class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith         Created 
*2.70        09-Jul-2019   Girish Kundar    Modified : Save() method : Insert/update methods returns DTO instead of Id.
*                                                        LogMethodEntry() and LogMethodExit(). 
*2.80        08-May-2020   Indrajeet Kumar  Modified : Added Constructor - CMSContentList()                                
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSContentBL
    {
        private CMSContentDTO cmsContentDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private CMSContentBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;            
            log.LogMethodExit();
        }
        public CMSContentBL(ExecutionContext executionContext, CMSContentDTO cmsContentDTO) 
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, cmsContentDTO);            
            this.cmsContentDTO = cmsContentDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'cmsContent"'  Request  ====>  ""cmsContent"" DataHandler
        /// <summary>
        /// Constructor which fetches  the  CMSContent based on id
        /// </summary>
        /// <param name="contentId">contentId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CMSContentBL(ExecutionContext executionContext, int contentId,SqlTransaction sqlTransaction = null):
            this(executionContext)
        {
            log.LogMethodEntry(contentId, contentId, sqlTransaction);
            CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);
            cmsContentDTO = cmsContentDataHandler.GetCmsContents(contentId);
            if (cmsContentDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSContent", contentId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update CMSContentDTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>id of CMSContent </returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsContentDTO.IsChanged == false 
                && cmsContentDTO.ContentId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);

            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsContentDTO.ContentId < 0)
            {
                cmsContentDTO = cmsContentDataHandler.InsertCmsContent(cmsContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsContentDTO.AcceptChanges();
            }
            else if (cmsContentDTO.IsChanged)
            {
                cmsContentDTO = cmsContentDataHandler.UpdatecmsContent(cmsContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsContentDTO.AcceptChanges();
            }
            SaveObjectTransalation(sqlTransaction);
            log.LogMethodExit();                  
        }

        private void SaveObjectTransalation(SqlTransaction sqlTransaction)
        {
            if (cmsContentDTO.ObjectTranslationsDTOList != null &&
                cmsContentDTO.ObjectTranslationsDTOList.Any())
            {
                List<ObjectTranslationsDTO> updatedObjectTranslationsDTOList = new List<ObjectTranslationsDTO>();
                foreach (var objectTranslationsDTO in cmsContentDTO.ObjectTranslationsDTOList)
                {
                    if (objectTranslationsDTO.Id != objectTranslationsDTO.Id)
                    {
                        objectTranslationsDTO.Id = objectTranslationsDTO.Id;
                    }
                    if (objectTranslationsDTO.IsChanged)
                    {
                        objectTranslationsDTO.ElementGuid = cmsContentDTO.Guid;
                        updatedObjectTranslationsDTOList.Add(objectTranslationsDTO);
                    }
                }
                if (updatedObjectTranslationsDTOList.Any())
                {
                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(executionContext, updatedObjectTranslationsDTOList);
                    objectTranslationsList.Save();
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
        /// Delete the record from the database based on  contentId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);
                int id = cmsContentDataHandler.contentDelete(cmsContentDTO.ContentId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception ex)
            {
                log.Error("Error  at Delete(contentId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the CMSContentDTO
        /// </summary>
        public CMSContentDTO GetCMSContentDTO
        { 
            get  { return cmsContentDTO; } 
        }        
    }

    public class CMSContentBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private List<CMSContentDTO> cMSContentDTOList = new List<CMSContentDTO>();
        private ExecutionContext executionContext;
        public CMSContentBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSContentBLList(ExecutionContext executionContext, List<CMSContentDTO> cMSContentDTOList )
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, cMSContentDTOList);
            this.cMSContentDTOList = cMSContentDTOList;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSContentDTO> GetAllCmsContent(List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);
                List<CMSContentDTO> cmsContentDTOList =  cmsContentDataHandler.GetContentsList(searchParameters);

                if (cmsContentDTOList != null && cmsContentDTOList.Any())
                {
                    foreach (CMSContentDTO cmsContentDTO in cmsContentDTOList)
                    {
                        cmsContentDTO.ObjectTranslationsDTOList = LoadObjectTranslations(cmsContentDTO.Guid);
                    }
                }
                log.LogMethodExit(cmsContentDTOList);
                return cmsContentDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at GetAllCmsContent(searchparameters) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }           
        }

        private List<ObjectTranslationsDTO> LoadObjectTranslations(string guid, int languageId = -1)
        {
            List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
            List<ObjectTranslationsDTO> objectTranslationsDTOSortableList = new List<ObjectTranslationsDTO>();
            if (!string.IsNullOrEmpty(guid))
            {
                ObjectTranslationsList objectTransaltionListBL = new ObjectTranslationsList(executionContext);
                searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, guid.ToString()));
                if (languageId > -1)
                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, languageId.ToString()));
                List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTransaltionListBL.GetAllObjectTranslations(searchParameters);
                if (objectTranslationsDTOList != null)
                {
                    objectTranslationsDTOSortableList = new List<ObjectTranslationsDTO>(objectTranslationsDTOList);
                }
            }
            return objectTranslationsDTOSortableList;
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the customerFeedbackSurveyPOSMappingDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSContentDTOList == null ||
                cMSContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < cMSContentDTOList.Count; i++)
            {
                var cMSContentDTO = cMSContentDTOList[i];                
                try
                {
                    CMSContentBL cMSContent = new CMSContentBL(executionContext, cMSContentDTO);
                    cMSContent.Save();                    
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSContentDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSContentDTO", cMSContentDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
