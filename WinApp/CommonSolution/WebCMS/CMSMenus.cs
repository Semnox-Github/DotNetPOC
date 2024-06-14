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
 *2.70       09-Jul-2019    Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.WebCMS
{
    public class CMSMenus
    {
      
        private CMSMenusDTO cmsMenusDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSMenus()
        {
            log.LogMethodEntry();
            cmsMenusDTO = new CMSMenusDTO();
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander besed id
        //And return Correspond Object
        //EX: "'CMSMenus"'  Request  ====>  ""CMSMenus"" DataHandler
        public CMSMenus(int menuId,SqlTransaction sqlTransaction=null): this()
        {
            log.LogMethodEntry(menuId);
            CMSMenusDataHandler  cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
           cmsMenusDTO = cmsMenusDataHandler.GetcmsMenu(menuId);
            log.LogMethodExit();
        }

        //Constructor Initializes with Corresponding Object
        public CMSMenus(CMSMenusDTO cmsMenusDTO): this()
        {
            log.LogMethodEntry(cmsMenusDTO); ;
            this.cmsMenusDTO = cmsMenusDTO;
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
            CMSMenusDataHandler cmsBannerDataHandler = new CMSMenusDataHandler(sqlTransaction);
            try
            {
                if (cmsMenusDTO.MenuId < 0)
                {
                    cmsMenusDTO = cmsBannerDataHandler.InsertMenus(cmsMenusDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsMenusDTO.AcceptChanges();
                    return cmsMenusDTO.MenuId;
                }
                else
                {
                    if (cmsMenusDTO.IsChanged)
                    {
                        cmsMenusDTO = cmsBannerDataHandler.Updatemenus(cmsMenusDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsMenusDTO.AcceptChanges();
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
        /// Gets the CMSMenusDTO
        /// </summary>
        public CMSMenusDTO GetCmsMenusDTO
        { 
            get  { return cmsMenusDTO; }
        }


        /// <summary>
        /// Delete the CMSMenusDTO based on Id
        /// </summary>
        public int Delete(int menuId,SqlTransaction sqlTransaction = null)
        {
            try
            {
                CMSMenusDataHandler cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
                int id =  cmsMenusDataHandler.menuDelete(menuId);
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
    public class CMSMenusList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSMenusDTO> GetAllCmsMenus(List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> searchParameters,SqlTransaction sqlTransaction = null  )
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            try
            {
                CMSMenusDataHandler cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
                List<CMSMenusDTO> cMSMenusDTOList = cmsMenusDataHandler.GetMenusList(searchParameters);
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

        /// <summary>
        /// Returns Search based on menuId And returns CMSMenusDTO   
        /// </summary>
        public CMSMenusDTO GetMenu(int menuId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(menuId, sqlTransaction);
            try
            {
                CMSMenusDataHandler cmsMenusDataHandler = new CMSMenusDataHandler(sqlTransaction);
                CMSMenusDTO cMSMenusDTO =  cmsMenusDataHandler.GetcmsMenu(menuId);
                log.LogMethodExit(cMSMenusDTO);
                return cMSMenusDTO;
            }
            catch (Exception expn)
            {
                log.Error("Error  at  GetMenu(int menuId,SqlTransaction sqlTransaction = null) method ", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }
    }

}
