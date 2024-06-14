/********************************************************************************************
* Project Name - CMSModule  Program
* Description  - Data object of the CMSModule 
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        11-Oct-2016   Rakshith           Created 
*2.70        09-Jul-2019   Girish Kundar      Modified : Save() method : Insert/update methods returns DTO instead of Id.
*                                                        LogMethodEntry() and LogMethodExit(). 
*2.80        08-May-2020   Indrajeet Kumar    Modified : As per As per 3 Tier Standard & GetAllCMSModules() Method, Created Method Build(), 
*                                                        Added Parameterized CMSModule(executionContext,cMSModulesDTO), Modified Save().
*2.130.2     17-Feb-2022   Nitin Pai         CMS Changes for SmartFun
********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModulesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private CMSModulesDTO cmsModulesDTO;

        /// <summary>
        /// Private Parameterized Constructor.
        /// </summary>
        /// <param name="executionContext"></param>
        private CMSModulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Public parameterized constructor with executionContext and DTO as parameters 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cMSModulesDTO"></param>
        public CMSModulesBL(ExecutionContext executionContext, CMSModulesDTO cMSModulesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cMSModulesDTO);
            this.cmsModulesDTO = cMSModulesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext, Id, if It has child list then constuctor should have loadChildRecords, 
        /// activeChildRecords, sqlTransaction and calls the Build() method
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="moduleId"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public CMSModulesBL(ExecutionContext executionContext, int moduleId, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, moduleId, loadChildRecords, activeChildRecords, sqlTransaction);
            CMSModulesDatahandler cMSModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);
            cmsModulesDTO = cMSModulesDatahandler.GetModule(moduleId);
            if (cmsModulesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSModules", moduleId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(loadChildRecords, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Build the Child
        /// </summary>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            // Build the Child Module Menu.
            List<KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>> cMSModuleMenuSearchParameter = new List<KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>>();
            cMSModuleMenuSearchParameter.Add(new KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>(CMSModuleMenuDTO.SearchByRequestParameters.MODULE_ID, cmsModulesDTO.ModuleId.ToString()));
            if (activeChildRecords)
            {
                cMSModuleMenuSearchParameter.Add(new KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>(CMSModuleMenuDTO.SearchByRequestParameters.IS_ACTIVE, "1"));
            }
            CMSModuleMenuBLList cMSModuleMenuBLList = new CMSModuleMenuBLList(executionContext);
            List<CMSModuleMenuDTO> cMSModuleMenuDTOList = cMSModuleMenuBLList.GetAllCMSModuleMenu(cMSModuleMenuSearchParameter, loadChildRecords, activeChildRecords, sqlTransaction);

            // Build the Child Module Page.
            List<KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>> cMSModulePageSearchParameter = new List<KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>>();
            cMSModulePageSearchParameter.Add(new KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>(CMSModulePageDTO.SearchByRequestParameters.MODULE_ID, cmsModulesDTO.ModuleId.ToString()));
            if (activeChildRecords)
            {
                cMSModulePageSearchParameter.Add(new KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>(CMSModulePageDTO.SearchByRequestParameters.IS_ACTIVE, "1"));
            }
            CMSModulePageBLList cMSModulePageBLList = new CMSModulePageBLList(executionContext);
            List<CMSModulePageDTO> cMSModulePageDTOList = cMSModulePageBLList.GetAllCMSModulePage(cMSModulePageSearchParameter, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Used for Save CMSModulesDTO - It may by Insert Or Update.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsModulesDTO.IsChangedRecursive == false
                && cmsModulesDTO.ModuleId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSModulesDatahandler cmsModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);

            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsModulesDTO.ModuleId < 0)
            {
                cmsModulesDTO = cmsModulesDatahandler.InsertModule(cmsModulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsModulesDTO.AcceptChanges();
            }
            else if (cmsModulesDTO.IsChanged)
            {
                cmsModulesDTO = cmsModulesDatahandler.UpdateModule(cmsModulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsModulesDTO.AcceptChanges();
            }

            SaveModuleChild(sqlTransaction);
            log.LogMethodExit();
        }

        private void SaveModuleChild(SqlTransaction sqlTransaction)
        {
            if (cmsModulesDTO.CMSModuleMenuDTOList != null && cmsModulesDTO.CMSModuleMenuDTOList.Any())
            {
                List<CMSModuleMenuDTO> updatedCMSModuleMenuDTOList = new List<CMSModuleMenuDTO>();
                foreach (var cMSModuleMenuDTO in cmsModulesDTO.CMSModuleMenuDTOList)
                {
                    if (cMSModuleMenuDTO.ModuleId != cmsModulesDTO.ModuleId)
                    {
                        cMSModuleMenuDTO.ModuleId = cmsModulesDTO.ModuleId;
                    }
                    if (cMSModuleMenuDTO.IsChanged)
                    {
                        updatedCMSModuleMenuDTOList.Add(cMSModuleMenuDTO);
                    }
                }
                if (updatedCMSModuleMenuDTOList.Any())
                {
                    CMSModuleMenuBLList cMSModuleMenuBLList = new CMSModuleMenuBLList(executionContext, updatedCMSModuleMenuDTOList);
                    cMSModuleMenuBLList.Save(sqlTransaction);
                }
            }

            if (cmsModulesDTO.CMSModulePageDTOList != null && cmsModulesDTO.CMSModulePageDTOList.Any())
            {
                List<CMSModulePageDTO> updatedCMSModulePageDTOList = new List<CMSModulePageDTO>();
                foreach (var cMSModulePageDTO in cmsModulesDTO.CMSModulePageDTOList)
                {
                    if (cMSModulePageDTO.ModuleId != cmsModulesDTO.ModuleId)
                    {
                        cMSModulePageDTO.ModuleId = cmsModulesDTO.ModuleId;
                    }
                    if (cMSModulePageDTO.IsChanged)
                    {
                        updatedCMSModulePageDTOList.Add(cMSModulePageDTO);
                    }
                }
                if (updatedCMSModulePageDTOList.Any())
                {
                    CMSModulePageBLList cMSModulePageBLList = new CMSModulePageBLList(executionContext, updatedCMSModulePageDTOList);
                    cMSModulePageBLList.Save(sqlTransaction);
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
        /// Gets the CMSModulesDTO
        /// </summary>
        public CMSModulesDTO GetCMSModulesDTO
        {
            get { return cmsModulesDTO; }
        }

        /// <summary>
        /// Delete the record from the database based on  moduleId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            CMSModulesDatahandler cmsModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);
            int id = cmsModulesDatahandler.cmsModuleDelete(cmsModulesDTO.ModuleId);
            log.LogMethodExit(id);
            return id;
        }
    }

    /// <summary>
    /// List of CMSModule
    /// </summary>
    public class CMSModulesBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CMSModulesDTO> cMSModulesDTOList = new List<CMSModulesDTO>();
        private ExecutionContext executionContext;

        public CMSModulesBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSModulesBLList(ExecutionContext executionContext,
            List<CMSModulesDTO> cMSModulesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.cMSModulesDTOList = cMSModulesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GetAllCMGroups based on the search parameters
        /// </summary>
        public List<CMSModulesDTO> GetAllCMSModules(List<KeyValuePair<CMSModulesDTO.SearchByRequestParameters, string>> searchParameters, bool loadChildRecords = false,
                                                    bool activeChildRecords = false, SqlTransaction sqlTransaction = null, bool replacePlaceHolders = false)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            try
            {
                CMSModulesDatahandler cMSModulesDatahandler = new CMSModulesDatahandler(sqlTransaction);
                cMSModulesDTOList = cMSModulesDatahandler.GetCmsModulesList(searchParameters);
                if (cMSModulesDTOList != null && cMSModulesDTOList.Any() && loadChildRecords)
                {
                    Build(cMSModulesDTOList, loadChildRecords, activeChildRecords, sqlTransaction);
                }

                // this parts need to be converted to a parameterized section when the CMS UI is built
                if (cMSModulesDTOList != null && cMSModulesDTOList.Any() && replacePlaceHolders)
                {
                    String cmsImageURL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CMS_SMARTFUN_IMAGE_URL");
                    foreach (CMSModulesDTO modulesDTO in cMSModulesDTOList)
                    {
                        if (modulesDTO.CMSModuleMenuDTOList != null && modulesDTO.CMSModuleMenuDTOList.Any())
                        {
                            foreach (CMSModuleMenuDTO moduleMenuDTO in modulesDTO.CMSModuleMenuDTOList)
                            {
                                if (moduleMenuDTO.CMSMenusDTOList != null && moduleMenuDTO.CMSMenusDTOList.Any())
                                {
                                    foreach (CMSMenusDTO menuDTO in moduleMenuDTO.CMSMenusDTOList)
                                    {
                                        if (menuDTO.CMSMenuItemsDTOList != null && menuDTO.CMSMenuItemsDTOList.Any())
                                        {
                                            foreach (CMSMenuItemsDTO menuItemDTO in menuDTO.CMSMenuItemsDTOList)
                                            {
                                                if (menuItemDTO.ItemUrl.Contains("@imageURL"))
                                                {
                                                    menuItemDTO.ItemUrl = menuItemDTO.ItemUrl.Replace("@imageURL", cmsImageURL);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (modulesDTO.CMSModulePageDTOList != null && modulesDTO.CMSModulePageDTOList.Any())
                        {
                            foreach (CMSModulePageDTO modulePageDTO in modulesDTO.CMSModulePageDTOList)
                            {
                                if (modulePageDTO.CMSPagesDTOList != null && modulePageDTO.CMSPagesDTOList.Any())
                                {
                                    foreach (CMSPagesDTO pageDTO in modulePageDTO.CMSPagesDTOList)
                                    {
                                        if (pageDTO.CMSContentDTOList != null && pageDTO.CMSContentDTOList.Any())
                                        {
                                            foreach (CMSContentDTO contentDTO in pageDTO.CMSContentDTOList)
                                            {
                                                if (contentDTO.ContentURL.Contains("@imageURL"))
                                                {
                                                    contentDTO.ContentURL = contentDTO.ContentURL.Replace("@imageURL", cmsImageURL);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                log.LogMethodExit(cMSModulesDTOList);
                return cMSModulesDTOList;
            }
            catch (Exception expn)
            {
                log.Error("Error  at  GetAllCMSModules(searchParameters) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        private void Build(List<CMSModulesDTO> cMSModulesDTOList, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cMSModulesDTOList, activeChildRecords, sqlTransaction);
            foreach(CMSModulesDTO cmsModuleDTO in cMSModulesDTOList)
            {
                var moduleId = cmsModuleDTO.ModuleId;
                if (moduleId > 0)
                {
                    //Build the Menu and Related Page
                    List<KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>> searchParameter = new List<KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>>();
                    searchParameter.Add(new KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>(CMSModuleMenuDTO.SearchByRequestParameters.MODULE_ID, moduleId.ToString()));
                    if (activeChildRecords)
                    {
                        searchParameter.Add(new KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>(CMSModuleMenuDTO.SearchByRequestParameters.IS_ACTIVE, "1"));
                    }
                    CMSModuleMenuBLList cMSModuleMenuBLList = new CMSModuleMenuBLList(executionContext);
                    List<CMSModuleMenuDTO> cMSModuleMenuDTOList = cMSModuleMenuBLList.GetAllCMSModuleMenu(searchParameter, loadChildRecords, activeChildRecords, sqlTransaction);

                    cmsModuleDTO.CMSModuleMenuDTOList = cMSModuleMenuDTOList;

                    //Build the Page and Related Page
                    List<KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>> searchParam = new List<KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>>();
                    searchParam.Add(new KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>(CMSModulePageDTO.SearchByRequestParameters.MODULE_ID, moduleId.ToString()));
                    if (activeChildRecords)
                    {
                        searchParam.Add(new KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>(CMSModulePageDTO.SearchByRequestParameters.IS_ACTIVE, "1"));
                    }
                    CMSModulePageBLList cMSModulePageBLList = new CMSModulePageBLList(executionContext);
                    List<CMSModulePageDTO> cMSModulePageDTOList = cMSModulePageBLList.GetAllCMSModulePage(searchParam, loadChildRecords, activeChildRecords, sqlTransaction);
                    cmsModuleDTO.CMSModulePageDTOList = cMSModulePageDTOList;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of CMSModulesDTOList.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSModulesDTOList == null ||
                cMSModulesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < cMSModulesDTOList.Count; i++)
            {
                var cMSModulesDTO = cMSModulesDTOList[i];
                try
                {
                    CMSModulesBL cMSModulesBL = new CMSModulesBL(executionContext, cMSModulesDTO);
                    cMSModulesBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSModulesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSModulesDTO", cMSModulesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
