/********************************************************************************************
* Project Name - WebCMS CMSModuleMenuBL
* Description  - Mapper between Module Menu 
* 
**************
**Version Log
**************
*Version     Date           Modified By        Remarks          
*********************************************************************************************
*2.80        06-May-2020    Indrajeet K        Created  
********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModuleMenuBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CMSModuleMenuDTO cmsModuleMenuDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Private Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private CMSModuleMenuBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Public Parameterized Constructor with executionContext and DTO as parameters 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cmsModuleMenuDTO"></param>
        public CMSModuleMenuBL(ExecutionContext executionContext, CMSModuleMenuDTO cmsModuleMenuDTO) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cmsModuleMenuDTO);            
            this.cmsModuleMenuDTO = cmsModuleMenuDTO;
            log.LogMethodExit();
        }

        public CMSModuleMenuBL(ExecutionContext executionContext, int id,
                        bool builChildRecord = false, bool loadActiveRecords = false ,SqlTransaction sqlTransaction = null)
        : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CMSModuleMenuDataHandler cMSModuleMenuDataHandler = new CMSModuleMenuDataHandler(sqlTransaction);
            cmsModuleMenuDTO = cMSModuleMenuDataHandler.GetCMSModuleMenuDTO(id);
            if (cmsModuleMenuDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "cmsModuleMenu", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if(loadActiveRecords)
            {
                Build(loadActiveRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords ,SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> cMSMenusSearchParameter = new List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>>();
            cMSMenusSearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.MENU_ID, cmsModuleMenuDTO.MenuId.ToString()));
            if (activeChildRecords)
            {
                cMSMenusSearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.ACTIVE, "1"));
            }
            CMSMenusBLList cMSMenusBLList = new CMSMenusBLList(executionContext);
            cmsModuleMenuDTO.CMSMenusDTOList = cMSMenusBLList.GetAllCmsMenus(cMSMenusSearchParameter, true,activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }



        /// <summary>
        /// Used to save - It may by Insert Or Update based on condition.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsModuleMenuDTO.IsChangedRecursive == false
                && cmsModuleMenuDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSModuleMenuDataHandler cMSModuleMenuDataHandler = new CMSModuleMenuDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            if (cmsModuleMenuDTO.Id < 0)
            {
                cmsModuleMenuDTO = cMSModuleMenuDataHandler.InsertCMSModuleMenu(cmsModuleMenuDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsModuleMenuDTO.AcceptChanges();
            }
            else if (cmsModuleMenuDTO.IsChanged)
            {
                cmsModuleMenuDTO = cMSModuleMenuDataHandler.UpdateCMSModuleMenu(cmsModuleMenuDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsModuleMenuDTO.AcceptChanges();
            }
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
    }

    public class CMSModuleMenuBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CMSModuleMenuDTO> cMSModuleMenuDTOList = new List<CMSModuleMenuDTO>();
        private List<CMSMenusDTO> cMSMenusDTOList = new List<CMSMenusDTO>();
        private ExecutionContext executionContext;
        public CMSModuleMenuBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSModuleMenuBLList(ExecutionContext executionContext,
              List<CMSModuleMenuDTO> cMSModuleMenuDTOList)
              :this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.cMSModuleMenuDTOList = cMSModuleMenuDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public List<CMSModuleMenuDTO> GetAllCMSModuleMenu(List<KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                CMSModuleMenuDataHandler cmsModuleMenuDataHandler = new CMSModuleMenuDataHandler(sqlTransaction);
                cMSModuleMenuDTOList = cmsModuleMenuDataHandler.GetCMSModuleMenuDTOList(searchParameters);
                if (cMSModuleMenuDTOList != null && cMSModuleMenuDTOList.Any() && loadChildRecords)
                {
                    Build(cMSModuleMenuDTOList, loadChildRecords, activeChildRecords, sqlTransaction);
                }                
                log.LogMethodExit(cMSModuleMenuDTOList);
                return cMSModuleMenuDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at  GetAllCMSModuleMenu(searchParameters) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        private void Build(List<CMSModuleMenuDTO> cMSModuleMenuDTOList, bool loadChildRecords = false,
            bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cMSModuleMenuDTOList,loadChildRecords, activeChildRecords);            
            // CMSMenuDTO do not have moduleId as member 
            if (cMSModuleMenuDTOList != null && cMSModuleMenuDTOList.Any())
            {
                foreach(CMSModuleMenuDTO cMSModuleMenuDTO in cMSModuleMenuDTOList)
                {
                    List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> cMSMenusSearchParameter = new List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>>();
                    cMSMenusSearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.MENU_ID, cMSModuleMenuDTO.MenuId.ToString()));
                    if (activeChildRecords)
                    {
                        cMSMenusSearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.ACTIVE, "1"));
                    }
                    CMSMenusBLList cMSMenusBLList = new CMSMenusBLList(executionContext);
                    cMSModuleMenuDTO.CMSMenusDTOList = cMSMenusBLList.GetAllCmsMenus(cMSMenusSearchParameter, loadChildRecords,activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit();
            //
            //if (cMSModuleMenuDTOList != null && cMSModuleMenuDTOList.Any())
            //{
            //    Dictionary<int, CMSModuleMenuDTO> cMSModuleMenuDictionary = new Dictionary<int, CMSModuleMenuDTO>();
            //    StringBuilder sb = new StringBuilder("");
            //    string cMSModuleMenuList;
            //    for (int i = 0; i < cMSModuleMenuDTOList.Count; i++)
            //    {
            //        if (cMSModuleMenuDTOList[i].Id == -1 ||
            //            cMSModuleMenuDictionary.ContainsKey(cMSModuleMenuDTOList[i].Id))
            //        {
            //            continue;
            //        }
            //        if (i != 0)
            //        {
            //            sb.Append(",");
            //        }
            //        sb.Append(cMSModuleMenuDTOList[i].MenuId.ToString());
            //        cMSModuleMenuDictionary.Add(cMSModuleMenuDTOList[i].MenuId, cMSModuleMenuDTOList[i]);
            //    }

            //    cMSModuleMenuList = sb.ToString();

            //    List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> cMSMenusSearchParameter = new List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>>();
            //    cMSMenusSearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.MENU_ID_LIST, cMSModuleMenuList.ToString()));
            //    if (activeChildRecords)
            //    {
            //        cMSMenusSearchParameter.Add(new KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>(CMSMenusDTO.SearchByRequestParameters.ACTIVE, "1"));
            //    }

            //    CMSMenusBLList cMSMenusBLList = new CMSMenusBLList(executionContext);
            //    cMSMenusDTOList = cMSMenusBLList.GetAllCmsMenus(cMSMenusSearchParameter, activeChildRecords, sqlTransaction);

            //    if (cMSMenusDTOList != null && cMSMenusDTOList.Any())
            //    {
            //        foreach (var cMSMenusDTO in cMSMenusDTOList)
            //        {
            //            if (cMSModuleMenuDictionary.ContainsKey(cMSMenusDTO.MenuId))
            //            {
            //                if (cMSModuleMenuDictionary[cMSMenusDTO.MenuId].CMSMenusDTOList == null)
            //                {
            //                    cMSModuleMenuDictionary[cMSMenusDTO.MenuId].CMSMenusDTOList = new List<CMSMenusDTO>();
            //                }
            //                cMSModuleMenuDictionary[cMSMenusDTO.MenuId].CMSMenusDTOList.Add(cMSMenusDTO);
            //            }
            //        }
            //    }

            //}
        }

        /// <summary>
        /// Saves the  list of cMSModuleMenuDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSModuleMenuDTOList == null ||
                cMSModuleMenuDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < cMSModuleMenuDTOList.Count; i++)
            {
                var cMSModuleMenuDTO = cMSModuleMenuDTOList[i];                
                try
                {
                    CMSModuleMenuBL cMSModuleMenuBL = new CMSModuleMenuBL(executionContext, cMSModuleMenuDTO);
                    cMSModuleMenuBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSModuleMenuDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSModuleMenuDTO", cMSModuleMenuDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
