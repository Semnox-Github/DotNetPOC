
/********************************************************************************************
 * Project Name - CMSMenuItem  BL Class  
 * Description  - Business logic of the CMSMenuItem  Requests class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016    Rakshith          Created 
 *2.70        09-Jul-2019    Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 *2.80        20-May-2020   Indrajeet Kumar   Modified : Save() & Constructor As per 3 tier Standard.                                                        
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSMenuItemBL
    {        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CMSMenuItemsDTO cmsMenuItemsDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Private Parameterized constructor
        /// </summary>
        private CMSMenuItemBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with parameter executionContext & cMSMenuItemsDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cMSMenuItemsDTO"></param>
        public CMSMenuItemBL(ExecutionContext executionContext, CMSMenuItemsDTO cMSMenuItemsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cMSMenuItemsDTO);
            this.executionContext = executionContext;
            this.cmsMenuItemsDTO = cMSMenuItemsDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSMenuItem"'  Request  ====>  ""CMSMenuItem"" DataHandler
        public CMSMenuItemBL(ExecutionContext executionContext, int menuItemId, SqlTransaction sqlTransaction = null): this(executionContext)
        {
            log.LogMethodEntry(executionContext, menuItemId, sqlTransaction);
            CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
            cmsMenuItemsDTO = cmsMenuItemDataHandler.GetCmsMenuItem(menuItemId);
            if (cmsMenuItemsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "cmsMenuItems", menuItemId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save - It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsMenuItemsDTO.IsChanged == false 
                && cmsMenuItemsDTO.ItemId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);            
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsMenuItemsDTO.ItemId < 0)
            {
                cmsMenuItemsDTO = cmsMenuItemDataHandler.InsertMenuItems(cmsMenuItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsMenuItemsDTO.AcceptChanges();
            }
            else if (cmsMenuItemsDTO.IsChanged)
            {
                cmsMenuItemDataHandler.UpdateMenuItems(cmsMenuItemsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsMenuItemsDTO.AcceptChanges();
            }
            SaveObjectTransalation(sqlTransaction);
            log.LogMethodExit();          
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

        private void SaveObjectTransalation(SqlTransaction sqlTransaction)
        {
            if (cmsMenuItemsDTO.ObjectTranslationsDTOList != null &&
                cmsMenuItemsDTO.ObjectTranslationsDTOList.Any())
            {
                List<ObjectTranslationsDTO> updatedObjectTranslationsDTOList = new List<ObjectTranslationsDTO>();
                foreach (var objectTranslationsDTO in cmsMenuItemsDTO.ObjectTranslationsDTOList)
                {
                    if (objectTranslationsDTO.Id != objectTranslationsDTO.Id)
                    {
                        objectTranslationsDTO.Id = objectTranslationsDTO.Id;
                    }
                    if (objectTranslationsDTO.IsChanged)
                    {
                        objectTranslationsDTO.ElementGuid = cmsMenuItemsDTO.Guid;
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
        /// Gets the CMSMenuItemsDTO
        /// </summary>
        public CMSMenuItemsDTO GetCMSMenuItemDTO
        { 
            get { return cmsMenuItemsDTO; } 
        }

        /// <summary>
        /// Delete the record from the database based on  menuId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                int id = cmsMenuItemDataHandler.menuItemDelete(cmsMenuItemsDTO.ItemId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception expn)
            {
                log.Error("Error  at Delete method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }          
        }
    }

    public class CMSMenuItemBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CMSMenuItemsDTO> cMSMenuItemsDTOList;

        public CMSMenuItemBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSMenuItemBLList(ExecutionContext executionContext, List<CMSMenuItemsDTO> cMSMenuItemsDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext);            
            this.cMSMenuItemsDTOList = cMSMenuItemsDTOList;           
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CMS Menu list based on the search parameters
        /// </summary>
        public List<CMSMenuItemsDTO> GetAllCMSMenuItems(List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                CMSMenuItemDataHandler CMSMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                List<CMSMenuItemsDTO> cMSMenuItemsDTOList  =  CMSMenuItemDataHandler.GetMenuItemsList(searchParameters);
                if (cMSMenuItemsDTOList != null && cMSMenuItemsDTOList.Any())
                {
                    foreach (CMSMenuItemsDTO cMSMenuItemsDTO in cMSMenuItemsDTOList)
                    {
                        cMSMenuItemsDTO.ObjectTranslationsDTOList = LoadObjectTranslations(cMSMenuItemsDTO.Guid);
                    }
                }
                log.LogMethodExit(cMSMenuItemsDTOList);
                return cMSMenuItemsDTOList;
            }
            catch (Exception expn)
            {
                log.Error("Error  at GetAllCMSMenuItems(searchParameters) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
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
                if(languageId > -1)
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
        /// Returns the CMS Menu list based on the search paramaters
        /// </summary>  
        public List<CMSMenuItemsTree> GetMenuItemsListTree(int menuId,bool showActive,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(menuId,  showActive, sqlTransaction);
            try
            {
                CMSMenuItemDataHandler CMSMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                List<CMSMenuItemsTree> cMSMenuItemsTrees =  CMSMenuItemDataHandler.GetMenuItemsTree(menuId,showActive);
                log.LogMethodExit(cMSMenuItemsTrees);
                return cMSMenuItemsTrees;
            }
            catch (Exception expn)
            {
                log.Error("Error  at GetMenuItemsListTree(int menuId,bool showActive) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }
        public List<CMSMenuItemsDTO.TargetType> GetTargetType(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry( sqlTransaction);
            try
            {
                CMSMenuItemDataHandler CMSMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                log.Debug("Ends-GetSelectTargetType() method by returning the result of CMSMenuItemDataHandler.GetSelectTargetType() call");
                List<CMSMenuItemsDTO.TargetType> targetTypes =  CMSMenuItemDataHandler.GetTargetType();
                log.LogMethodExit(targetTypes);
                return targetTypes;
            }
            catch (Exception expn)
            {
                log.Error("Error  at  GetTargetType(SqlTransaction) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        public List<CMSMenuItemsTree> GetMenuListTreeType(string headerType,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(headerType,sqlTransaction);
            try
            {
                CMSMenuItemDataHandler CMSMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                List<CMSMenuItemsTree> cMSMenuItemsTrees =  CMSMenuItemDataHandler.GetMenuListTreeType(headerType);
                log.LogMethodExit(cMSMenuItemsTrees);
                return cMSMenuItemsTrees;
            }
            catch (Exception expn)
            {
                log.Error("Error  at   GetMenuListTreeType(string headerType,SqlTransaction sqlTransaction) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }
       
        internal void SaveMenuItem(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSMenuItemsDTOList == null ||
                cMSMenuItemsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < cMSMenuItemsDTOList.Count; i++)
            {
                var cMSMenuItemsDTO = cMSMenuItemsDTOList[i];                
                try
                {
                    CMSMenuItemBL cMSMenuItem = new CMSMenuItemBL(executionContext, cMSMenuItemsDTO);
                    cMSMenuItem.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSMenuItemsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSMenuItemsDTO", cMSMenuItemsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
