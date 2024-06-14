/********************************************************************************************
 * Project Name - CMSSocialLinks BL  
 * Description  - Bussiness logic of the CMSSocial Links BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016    Rakshith         Created 
 *2.70       09-Jul-2019    Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.WebCMS
{
    public class CMSSocialLinks
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CMSSocialLinksDTO cmsSocialLinksDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        private CMSSocialLinks(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'CMSSocialLinks"'  Request  ====>  ""CMSSocialLinks"" DataHandler
        public CMSSocialLinks(ExecutionContext executionContext ,int socialLinkId,
            SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(socialLinkId, sqlTransaction);
            CMSSocalLinksDataHandler cmsSocalLinksDataHandler = new CMSSocalLinksDataHandler(sqlTransaction);
            cmsSocialLinksDTO = cmsSocalLinksDataHandler.GetcmsSocialLinks(socialLinkId);
            if (cmsSocialLinksDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "cmsSocialLinks", socialLinkId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        //Constructor Initializes with Corresponding Object
        public CMSSocialLinks(ExecutionContext executionContext, CMSSocialLinksDTO cmsSocialLinksDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(cmsSocialLinksDTO);
            this.cmsSocialLinksDTO = cmsSocialLinksDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (cmsSocialLinksDTO.IsChanged == false
               && cmsSocialLinksDTO.SocialLinkId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSSocalLinksDataHandler cmsSocalLinksDataHandler = new CMSSocalLinksDataHandler(sqlTransaction);
            try
            {
                if (cmsSocialLinksDTO.SocialLinkId < 0)
                {
                    cmsSocialLinksDTO = cmsSocalLinksDataHandler.InsertSocialLinks(cmsSocialLinksDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cmsSocialLinksDTO.AcceptChanges();
                }
                else
                {
                    if (cmsSocialLinksDTO.IsChanged)
                    {
                        cmsSocialLinksDTO = cmsSocalLinksDataHandler.UpdateSocialLinks(cmsSocialLinksDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        cmsSocialLinksDTO.AcceptChanges();
                    }
                    log.LogMethodExit(0);
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
        /// Delete the CMSSocialLinksDTO based on Id
        /// </summary>
        /// <param name="socialLinkId">socialLinkId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> id </returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CMSSocalLinksDataHandler cmsSocalLinksDataHandler = new CMSSocalLinksDataHandler(sqlTransaction);
                int id = cmsSocalLinksDataHandler.cmsSocialLinkDelete(cmsSocialLinksDTO.SocialLinkId);
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

        /// <summary>
        /// Gets the CMSSocialLinksDTO
        /// </summary>
        public CMSSocialLinksDTO GetCMSSocialLinksDTO
        {
            get { return cmsSocialLinksDTO; }
        }
    }

    public class CMSSocialLinksList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns Search based on Parameters And returns  List<CMSSocialLinksDTO>   
        /// </summary>
        public List<CMSSocialLinksDTO> GetAllCmsSocialLinks(List<KeyValuePair<CMSSocialLinksDTO.SearchByRequestParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(searchParameters, sqlTransaction);
            CMSSocalLinksDataHandler cmsSocalLinksDataHandler = new CMSSocalLinksDataHandler(sqlTransaction);
            List<CMSSocialLinksDTO> cMSSocialLinksDTOList = cmsSocalLinksDataHandler.GetSocialLinksList(searchParameters);
            log.LogMethodExit(cMSSocialLinksDTOList);
            return cMSSocialLinksDTOList;

        }

        /// <summary>
        /// Returns Search based on socialLinkId And returns  CMSSocialLinksDTO   
        /// </summary>
        public CMSSocialLinksDTO GetSocialLink(int socialLinkId, SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(sqlTransaction);
            CMSSocalLinksDataHandler cmsSocalLinksDataHandler = new CMSSocalLinksDataHandler(sqlTransaction);
            CMSSocialLinksDTO cMSSocialLinksDTO = cmsSocalLinksDataHandler.GetcmsSocialLinks(socialLinkId);
            log.LogMethodExit(cMSSocialLinksDTO);
            return cMSSocialLinksDTO;
        }
    }
}
