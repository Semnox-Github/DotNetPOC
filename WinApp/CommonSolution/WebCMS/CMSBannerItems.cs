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
 *2.70       09-Jul-2019    Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSBannerItems
    {
        private CMSBannerItemsDTO cmsBannerItemsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSBannerItems()
        {
            log.LogMethodEntry();
            cmsBannerItemsDTO = new CMSBannerItemsDTO();
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSBannerItems"'  Request  ====>  ""CMSBannerItems"" DataHandler
        public CMSBannerItems(int bannerItemId, SqlTransaction sqlTrasaction = null) : this()
        {
            log.LogMethodEntry(bannerItemId, sqlTrasaction);
            CMSBannerItemsDataHandler cmsBannerItemsDataHandler = new CMSBannerItemsDataHandler(sqlTrasaction);
            cmsBannerItemsDTO = cmsBannerItemsDataHandler.GetCmsbannerItem(bannerItemId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor Initializes with Corresponding Object
        /// </summary>
        /// <param name="cmsBannerItemsDTO">cmsBannerItemsDTO</param>
        public CMSBannerItems(CMSBannerItemsDTO cmsBannerItemsDTO) : this()
        {
            log.LogMethodEntry(cmsBannerItemsDTO);
            this.cmsBannerItemsDTO = cmsBannerItemsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        /// <param name="sqlTrasaction">sqlTrasaction</param>
        /// <returns>  id for cmsBannerItemsDTO </returns>
        public int Save(SqlTransaction sqlTrasaction = null)
        {
            log.LogMethodEntry(sqlTrasaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CMSBannerItemsDataHandler cmsBannerDataHandler = new CMSBannerItemsDataHandler(sqlTrasaction);

            try
            {
                if (cmsBannerItemsDTO.BannerItemId < 0)
                {
                    cmsBannerItemsDTO = cmsBannerDataHandler.InsertBannerItems(cmsBannerItemsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsBannerItemsDTO.AcceptChanges();
                    return cmsBannerItemsDTO.BannerItemId;
                }
                else
                {
                    if (cmsBannerItemsDTO.IsChanged)
                    {
                        cmsBannerItemsDTO = cmsBannerDataHandler.UpdateBannerItems(cmsBannerItemsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsBannerItemsDTO.AcceptChanges();
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
        /// Gets the CMSContentDTO
        /// </summary>
        public CMSBannerItemsDTO GetcmsBannerItemsDTO
        {
            get { return cmsBannerItemsDTO; }
        }

        /// <summary>
        /// Delete the record from the database based on  bannerIemId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int bannerIemId, SqlTransaction sqlTrasaction = null)
        {
            log.LogMethodEntry(bannerIemId, sqlTrasaction);
            try
            {
                CMSBannerItemsDataHandler cmsBannerDataHandler = new CMSBannerItemsDataHandler(sqlTrasaction);
                int id = cmsBannerDataHandler.bannerItemDelete(bannerIemId);
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
    public class CMSBannerItemsLists
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSBannerItemsDTO> GetAllCmsBannerItems(List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParameters);
                CMSBannerItemsDataHandler cmsBannerItemsDataHandler = new CMSBannerItemsDataHandler(sqlTransaction);
                List<CMSBannerItemsDTO> cmsBannerItemsDTOList = cmsBannerItemsDataHandler.GetBannerItemsList(searchParameters);
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

        public List<CMSBannerItemsDTO> GetCmsBannerItemsByBannerId(int bannerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(bannerId, sqlTransaction);
            try
            {
                List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>>();
                searchParameters.Add(new KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>(CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ID, bannerId.ToString()));
                CMSBannerItemsDataHandler cmsBannerItemsDataHandler = new CMSBannerItemsDataHandler(sqlTransaction);
                List<CMSBannerItemsDTO> cmsBannerItemsDTOList = cmsBannerItemsDataHandler.GetBannerItemsList(searchParameters);
                log.LogMethodExit(cmsBannerItemsDTOList);
                return cmsBannerItemsDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at GetCmsBannerItemsByBannerId(bannerId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
    }
}


