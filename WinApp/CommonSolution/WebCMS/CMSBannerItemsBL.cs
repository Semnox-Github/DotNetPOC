/********************************************************************************************
* Project Name - CMSBannerItems   BL Class  
* Description  - Bussiness logic of the CMSBanner Items BL class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith         Created 
*2.70        09-Jul-2019   Girish Kundar    Modified : Save() method : Insert/update methods returns DTO instead of Id.
*                                                        LogMethodEntry() and LogMethodExit(). 
*2.80        08-May-2020   Indrajeet Kumar  Modified : Create a constructor CMSBannerItemsLists() & modified 
*                                           Existing constructor As per 3 tier Standard, Save()method.
********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSBannerItemsBL
    {        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private CMSBannerItemsDTO cmsBannerItemsDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Private Parameterized Constructor
        /// </summary>
        private CMSBannerItemsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor - with parameter executionContext & cmsBannerItemsDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cmsBannerItemsDTO"></param>
        public CMSBannerItemsBL(ExecutionContext executionContext, CMSBannerItemsDTO cmsBannerItemsDTO) 
            :this (executionContext)
        {
            log.LogMethodEntry();
            this.cmsBannerItemsDTO = cmsBannerItemsDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSBannerItems"'  Request  ====>  ""CMSBannerItems"" DataHandler
        public CMSBannerItemsBL(ExecutionContext executionContext, int bannerItemId, SqlTransaction sqlTrasaction = null) : this(executionContext)
        {
            log.LogMethodEntry(bannerItemId, sqlTrasaction);
            CMSBannerItemsDataHandler cmsBannerItemsDataHandler = new CMSBannerItemsDataHandler(sqlTrasaction);
            cmsBannerItemsDTO = cmsBannerItemsDataHandler.GetCmsbannerItem(bannerItemId);
            if (cmsBannerItemsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSBanner Item", bannerItemId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }        

        /// <summary>
        /// Used For Save -It may by Insert Or Update
        /// </summary>
        /// <param name="sqlTrasaction">sqlTrasaction</param>
        /// <returns></returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsBannerItemsDTO.IsChanged == false
                && cmsBannerItemsDTO.BannerItemId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSBannerItemsDataHandler cmsBannerDataHandler = new CMSBannerItemsDataHandler(sqlTransaction);

            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsBannerItemsDTO.BannerItemId < 0)
            {
                cmsBannerItemsDTO = cmsBannerDataHandler.InsertBannerItems(cmsBannerItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsBannerItemsDTO.AcceptChanges();
            }
            else if (cmsBannerItemsDTO.IsChanged)
            {
                cmsBannerItemsDTO = cmsBannerDataHandler.UpdateBannerItems(cmsBannerItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsBannerItemsDTO.AcceptChanges();
            }
            SaveObjectTransalation(sqlTransaction);
            log.LogMethodExit();            
        }

        private void SaveObjectTransalation(SqlTransaction sqlTransaction)
        {
            if (cmsBannerItemsDTO.ObjectTranslationsDTOList != null &&
                cmsBannerItemsDTO.ObjectTranslationsDTOList.Any())
            {
                List<ObjectTranslationsDTO> updatedObjectTranslationsDTOList = new List<ObjectTranslationsDTO>();
                foreach (var objectTranslationsDTO in cmsBannerItemsDTO.ObjectTranslationsDTOList)
                {
                    if (objectTranslationsDTO.Id != objectTranslationsDTO.Id)
                    {
                        objectTranslationsDTO.Id = objectTranslationsDTO.Id;
                    }
                    if (objectTranslationsDTO.IsChanged)
                    {
                        objectTranslationsDTO.ElementGuid = cmsBannerItemsDTO.Guid;
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
        /// Gets the CMSContentDTO
        /// </summary>
        public CMSBannerItemsDTO GetCMSBannerItemsDTO
        {
            get { return cmsBannerItemsDTO; }
        }

        /// <summary>
        /// Delete the record from the database based on  bannerIemId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTrasaction = null)
        {
            log.LogMethodEntry(sqlTrasaction);
            try
            {
                CMSBannerItemsDataHandler cmsBannerDataHandler = new CMSBannerItemsDataHandler(sqlTrasaction);
                int id = cmsBannerDataHandler.bannerItemDelete(cmsBannerItemsDTO.BannerItemId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception ex)
            {
                log.Error("Error  at Delete(bannerIemId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }
    }
    public class CMSBannerItemsBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CMSBannerItemsDTO> cMSBannerItemsDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CMSBannerItemsBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSBannerItemsBLList(ExecutionContext executionContext, List<CMSBannerItemsDTO> cMSBannerItemsDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, cMSBannerItemsDTOList);
            this.cMSBannerItemsDTOList = cMSBannerItemsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSBannerItemsDTO> GetAllCmsBannerItems(List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>> searchParameters,
                                    SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParameters);
                CMSBannerItemsDataHandler cmsBannerItemsDataHandler = new CMSBannerItemsDataHandler(sqlTransaction);
                List<CMSBannerItemsDTO> cmsBannerItemsDTOList = cmsBannerItemsDataHandler.GetBannerItemsList(searchParameters);
                if (cmsBannerItemsDTOList != null && cmsBannerItemsDTOList.Any())
                {
                    foreach (CMSBannerItemsDTO cMSBannerItemsDTO in cmsBannerItemsDTOList)
                    {
                        cMSBannerItemsDTO.ObjectTranslationsDTOList = LoadObjectTranslations(cMSBannerItemsDTO.Guid);
                    }
                }
                log.LogMethodExit(cmsBannerItemsDTOList);
                return cmsBannerItemsDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at GetAllCmsBannerItems(searchparameters) method ", ex);
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
        /// Returns Search based on bannerItemId And returns  CMSBannerItemsDTO   
        /// </summary>
        public CMSBannerItemsDTO GetBannerItems(int bannerItemId, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(bannerItemId);
                CMSBannerItemsDataHandler cmsBannerItemsDataHandler = new CMSBannerItemsDataHandler(sqlTransaction);
                CMSBannerItemsDTO cmsBannerItemsDTO = cmsBannerItemsDataHandler.GetCmsbannerItem(bannerItemId);
                log.LogMethodExit(cmsBannerItemsDTO);
                return cmsBannerItemsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error  at GetBannerItems(bannerItemId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }


        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSBannerItemsDTOList == null ||
                cMSBannerItemsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < cMSBannerItemsDTOList.Count; i++)
            {
                var cMSBannerItemsDTO = cMSBannerItemsDTOList[i];                
                try
                {
                    CMSBannerItemsBL cMSBannerItems = new CMSBannerItemsBL(executionContext, cMSBannerItemsDTO);
                    cMSBannerItems.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSBannerItemsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSBannerItemsDTO", cMSBannerItemsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}