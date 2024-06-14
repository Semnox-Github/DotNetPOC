
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
 *2.70        09-Jul-2019    Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.WebCMS
{
    public class CMSMenuItem
    {
        private  CMSMenuItemsDTO cmsMenuItemsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSMenuItem()
        {
            log.LogMethodEntry();
            cmsMenuItemsDTO = new CMSMenuItemsDTO();
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSMenuItem"'  Request  ====>  ""CMSMenuItem"" DataHandler
        public CMSMenuItem(int menuItemId, SqlTransaction sqlTransaction = null): this()
        {
            log.LogMethodEntry(menuItemId, sqlTransaction);
            CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
            cmsMenuItemsDTO = cmsMenuItemDataHandler.GetCmsMenuItem(menuItemId);
            log.LogMethodExit();
        }

        //Constructor Initializes with Corresponding Object
        public CMSMenuItem(CMSMenuItemsDTO cmsMenuItemsDTO) : this()
        {
            log.LogMethodEntry(cmsMenuItemsDTO);
            this.cmsMenuItemsDTO = cmsMenuItemsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);

            try
            {
                if (cmsMenuItemsDTO.ItemId < 0)
                {
                    cmsMenuItemsDTO = cmsMenuItemDataHandler.InsertMenuItems(cmsMenuItemsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsMenuItemsDTO.AcceptChanges();
                    return cmsMenuItemsDTO.ItemId;
                }
                else
                {
                    if (cmsMenuItemsDTO.IsChanged)
                    {
                        cmsMenuItemDataHandler.UpdateMenuItems(cmsMenuItemsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsMenuItemsDTO.AcceptChanges();
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
        /// Gets the CMSMenuItemsDTO
        /// </summary>
        public CMSMenuItemsDTO GetcmsMenuItemDTO
        { 
            get { return cmsMenuItemsDTO; } 
        }

        /// <summary>
        /// Delete the record from the database based on  menuId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int menuItemId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(menuItemId, sqlTransaction);
            try
            {
                CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                int id = cmsMenuItemDataHandler.menuItemDelete(menuItemId);
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

    public class CMSMenuItemList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
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

        /// <summary>
        /// Returns Search based on menuItemId And returns  CMSMenuItemsDTO   
        /// </summary>
        public CMSMenuItemsDTO GetMenuItem(int menuItemId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(menuItemId, sqlTransaction);
            try
            {
                CMSMenuItemDataHandler CMSMenuItemDataHandler = new CMSMenuItemDataHandler(sqlTransaction);
                CMSMenuItemsDTO cMSMenuItemsDTO = CMSMenuItemDataHandler.GetCmsMenuItem(menuItemId);
                log.LogMethodExit(cMSMenuItemsDTO);
                return cMSMenuItemsDTO;
            }
            catch (Exception expn)
            {
                log.Error("Error  at GetMenuItem(int menuItemId,SqlTransaction sqlTransaction = null) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
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
    }
}
