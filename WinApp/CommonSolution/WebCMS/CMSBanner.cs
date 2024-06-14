/********************************************************************************************
 * Project Name - CMSBanner  BL Class  
 * Description  - Bussiness logic of the CMSBanner  BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Rakshith          Created 
 *2.70        09-Jul-2019   Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                       LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSBanner
    {
       
        private CMSBannersDTO cmsBannerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSBanner()
        {
            log.LogMethodEntry();
            cmsBannerDTO = new CMSBannersDTO();
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'cmsBanner"'  Request  ====>  ""cmsBanner"" DataHandler
        public CMSBanner(int bannerId, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(bannerId, sqlTransaction);
           CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
           cmsBannerDTO = cmsBannerDataHandler.GetcmsBanner(bannerId);
           log.LogMethodExit();
        }

        //Constructor Initializes with Corresponding Object
        public CMSBanner(CMSBannersDTO cmsBannerDTO): this()
        {
            log.LogMethodEntry(cmsBannerDTO);
            this.cmsBannerDTO = cmsBannerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public int Save(SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(sqlTransaction);
            CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);

            try
            {
                if (cmsBannerDTO.BannerId < 0)
                {
                    cmsBannerDTO = cmsBannerDataHandler.InsertBanner(cmsBannerDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsBannerDTO.AcceptChanges();
                    return cmsBannerDTO.BannerId;
                }
                else
                {
                    if (cmsBannerDTO.IsChanged)
                    {
                        cmsBannerDTO = cmsBannerDataHandler.UpdateBanner(cmsBannerDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsBannerDTO.AcceptChanges();
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
        public CMSBannersDTO GetcmsBanner 
        { 
            get  { return cmsBannerDTO; }  
        }


        /// <summary>
        /// Delete the record from the database based on  bannerId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int bannerId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(bannerId,sqlTransaction);
            try
            {
                CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
                int id = cmsBannerDataHandler.bannerDelete(bannerId);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception ex)
            {
                log.Error("Error  at Delete(bannerId) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            

        }

        
    }
    public class CMSBannerList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<CMSBannersDTO> GetAllCmsBanners(List<KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                CMSBannersDataHandler cmsBannerDataHandler = new CMSBannersDataHandler(sqlTransaction);
                List<CMSBannersDTO> cmsBannersDTOList =   cmsBannerDataHandler.GetBannersList(searchParameters);
                log.LogMethodExit(cmsBannersDTOList);
                return cmsBannersDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error  at GetAllCmsBanners(searchparameters) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
           
        }
        
    }

}
