/********************************************************************************************
 * Project Name - CMSMenus BL Class  
 * Description  - Bussiness logic of the CMSMenus BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        07-Apr-2016    Rakshith          Created 
 *2.70        09-Jul-2019    Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 *2.80        10-May-2020    Indrajeet Kumar   Modified : GetAllCmsMenus() to support - CMSMenuItems && Save()                                             
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    public class CMSMenusBL
    {
        private CMSMenusDTO cmsMenusDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private CMSMenusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSMenusBL(ExecutionContext executionContext, CMSMenusDTO cmsMenusDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cmsMenusDTO);
            this.cmsMenusDTO = cmsMenusDTO;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'CMSMenus"'  Request  ====>  ""CMSMenus"" DataHandler

        public CMSMenusBL(ExecutionContext executionContext, int menuId,
               SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, menuId, sqlTransaction);
            CMSMenusDataHandler cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
            cmsMenusDTO = cmsMenusDataHandler.GetcmsMenu(menuId);
            if (cmsMenusDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSMenus", menuId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsMenusDTO.IsChangedRecursive == false
                && cmsMenusDTO.MenuId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSMenusDataHandler cmsBannerDataHandler = new CMSMenusDataHandler(sqlTransaction);            
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (cmsMenusDTO.MenuId < 0)
            {
                cmsMenusDTO = cmsBannerDataHandler.InsertMenus(cmsMenusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsMenusDTO.AcceptChanges();
            }
            else if (cmsMenusDTO.IsChanged)
            {
                cmsMenusDTO = cmsBannerDataHandler.Updatemenus(cmsMenusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cmsMenusDTO.AcceptChanges();
            }                        
            SaveCMSMenuItemChild(sqlTransaction);
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

        /// <summary>
        /// Saves the child records : CMSMenuItem
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveCMSMenuItemChild(SqlTransaction sqlTransaction)
        {
            if (cmsMenusDTO.CMSMenuItemsDTOList != null &&
                cmsMenusDTO.CMSMenuItemsDTOList.Any())
            {
                List<CMSMenuItemsDTO> updatedCMSMenuItemsDTOList = new List<CMSMenuItemsDTO>();
                foreach (var cMSMenuItemsDTO in cmsMenusDTO.CMSMenuItemsDTOList)
                {
                    if (cMSMenuItemsDTO.MenuId != cmsMenusDTO.MenuId)
                    {
                        cMSMenuItemsDTO.MenuId = cmsMenusDTO.MenuId;
                    }
                    if (cMSMenuItemsDTO.IsChanged)
                    {
                        updatedCMSMenuItemsDTOList.Add(cMSMenuItemsDTO);
                    }
                }
                if (updatedCMSMenuItemsDTOList.Any())
                {
                    CMSMenuItemBLList cMSMenuItemBLList = new CMSMenuItemBLList(executionContext, updatedCMSMenuItemsDTOList);
                    cMSMenuItemBLList.SaveMenuItem(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Gets the CMSMenusDTO
        /// </summary>
        public CMSMenusDTO GetCMSMenusDTO
        {
            get { return cmsMenusDTO; }
        }


        /// <summary>
        /// Delete the CMSMenusDTO based on Id
        /// </summary>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            try
            {
                CMSMenusDataHandler cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
                int id = cmsMenusDataHandler.menuDelete(cmsMenusDTO.MenuId);
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
    }
    public class CMSMenusBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CMSMenusDTO> cMSMenusDTOList = new List<CMSMenusDTO>();
        private List<CMSMenuItemsDTO> cMSMenuItemsDTOList = new List<CMSMenuItemsDTO>();
        private ExecutionContext executionContext;

        public CMSMenusBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public CMSMenusBLList(ExecutionContext executionContext, List<CMSMenusDTO> cMSMenusDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, cMSMenusDTOList);
            this.cMSMenusDTOList = cMSMenusDTOList;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSMenusDTO> GetAllCmsMenus(List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            try
            {
                CMSMenusDataHandler cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
                List<CMSMenusDTO> cMSMenusDTOList = cmsMenusDataHandler.GetMenusList(searchParameters);
                if (cMSMenusDTOList != null && cMSMenusDTOList.Any())
                {
                    if (loadChildRecords)
                    {
                        Build(cMSMenusDTOList, activeChildRecords, sqlTransaction);
                    }
                }
                log.LogMethodExit(cMSMenusDTOList);
                return cMSMenusDTOList;
            }
            catch (Exception expn)
            {
                log.Error("Error  at  GetAllCmsMenus(searchParameters) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        private void Build(List<CMSMenusDTO> cMSMenusDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cMSMenusDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CMSMenusDTO> cmsMenusIdDictionary = new Dictionary<int, CMSMenusDTO>();
            string cmsMenuIdSet = string.Empty;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < cMSMenusDTOList.Count; i++)
            {
                if (cMSMenusDTOList[i].MenuId == -1 ||
                    cmsMenusIdDictionary.ContainsKey(cMSMenusDTOList[i].MenuId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(cMSMenusDTOList[i].MenuId);
                cmsMenusIdDictionary.Add(cMSMenusDTOList[i].MenuId, cMSMenusDTOList[i]);
            }
            cmsMenuIdSet = sb.ToString();
            CMSMenuItemBLList cMSMenuItemList = new CMSMenuItemBLList(executionContext);
            List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>> searchParameter = new List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>>();
            searchParameter.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID_LIST, cmsMenuIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameter.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.ACTIVE, "1"));
            }
            cMSMenuItemsDTOList = cMSMenuItemList.GetAllCMSMenuItems(searchParameter);
            if (cMSMenuItemsDTOList != null && cMSMenuItemsDTOList.Any())
            {
                log.LogVariableState("cMSMenuItemsDTOList", cMSMenuItemsDTOList);
                foreach (var cmsMenuItemsDTO in cMSMenuItemsDTOList)
                {
                    if (cmsMenusIdDictionary.ContainsKey(cmsMenuItemsDTO.MenuId))
                    {
                        if (cmsMenusIdDictionary[cmsMenuItemsDTO.MenuId].CMSMenuItemsDTOList == null)
                        {
                            cmsMenusIdDictionary[cmsMenuItemsDTO.MenuId].CMSMenuItemsDTOList = new List<CMSMenuItemsDTO>();
                        }
                        cmsMenusIdDictionary[cmsMenuItemsDTO.MenuId].CMSMenuItemsDTOList.Add(cmsMenuItemsDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of cMSMenusDTOList.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cMSMenusDTOList == null ||
                cMSMenusDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < cMSMenusDTOList.Count; i++)
            {
                var cMSMenusDTO = cMSMenusDTOList[i];                
                try
                {
                    CMSMenusBL cMSMenus = new CMSMenusBL(executionContext, cMSMenusDTO);
                    cMSMenus.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving cMSMenusDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("cMSMenusDTO", cMSMenusDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
