
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
 *2.70       09-Jul-2019    Girish Kundar     Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSContent
    {
        private CMSContentDTO cmsContentDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSContent()
        {
            log.LogMethodEntry();
            cmsContentDTO = new CMSContentDTO();
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
        public CMSContent(int contentId,SqlTransaction sqlTransaction = null):
            this()
        {
            log.LogMethodEntry(contentId, sqlTransaction);
            CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);
            cmsContentDTO = cmsContentDataHandler.GetCmsContents(contentId);
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor Initializes with CMSContentDTO Object
        /// </summary>
        /// <param name="cmsContentDTO">cmsContentDTO</param>
        public CMSContent(CMSContentDTO cmsContentDTO): this()
        {
            log.LogMethodEntry(cmsContentDTO);
            this.cmsContentDTO = cmsContentDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update CMSContentDTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>id of CMSContent </returns>
        public int Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);

            try
            {
                if (cmsContentDTO.ContentId < 0)
                {
                    cmsContentDTO = cmsContentDataHandler.InsertCmsContent(cmsContentDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cmsContentDTO.AcceptChanges();
                    return cmsContentDTO.ContentId;
                }
                else
                {
                    if (cmsContentDTO.IsChanged)
                    {
                        cmsContentDTO = cmsContentDataHandler.UpdatecmsContent(cmsContentDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                        cmsContentDTO.AcceptChanges();
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
        /// Delete the record from the database based on  contentId
        /// </summary>
        /// <returns>return the int </returns>
        public int Delete(int contentId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(contentId, sqlTransaction);
            try
            {
                CMSContentDataHandler cmsContentDataHandler = new CMSContentDataHandler(sqlTransaction);
                int id = cmsContentDataHandler.contentDelete(contentId);
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
        public CMSContentDTO GetcmsContent 
        { 
            get  { return cmsContentDTO; } 
        }
        
    }

    public class CMSContentList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
       
    }

}
