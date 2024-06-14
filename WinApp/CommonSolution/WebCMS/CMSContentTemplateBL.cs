using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// Business logic for CMSContentTemplateBL class.
    /// </summary>
    public class CMSContentTemplateBL
    {
        private CMSContentTemplateDTO cMSContentTemplateDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of CMSContentTemplateBL class
        /// </summary>
        private CMSContentTemplateBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the  Content TemplateId as the parameter
        /// Would fetch the CMSContentTemplate object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public CMSContentTemplateBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext ,id, sqlTransaction);
            CMSContentTemplateDataHandler cMSContentTemplateDataHandler = new CMSContentTemplateDataHandler(sqlTransaction);
            cMSContentTemplateDTO = cMSContentTemplateDataHandler.GetCMSContentTemplateDTO(id);
            if (cMSContentTemplateDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CMSContent Template", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CMSContentTemplateBL object using the CMSContentTemplateDTO
        /// </summary>
        /// <param name="cMSContentTemplateDTO">CMSContentTemplateDTO object</param>
        public CMSContentTemplateBL(ExecutionContext executionContext, CMSContentTemplateDTO cMSContentTemplateDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(cMSContentTemplateDTO);
            this.cMSContentTemplateDTO = cMSContentTemplateDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CMSContentTemplat
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (cMSContentTemplateDTO.IsChanged == false
                 && cMSContentTemplateDTO.ContentTemplateId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CMSContentTemplateDataHandler cMSContentTemplateDataHandler = new CMSContentTemplateDataHandler(sqlTransaction);
            if (cMSContentTemplateDTO.ContentTemplateId < 0)
            {
                cMSContentTemplateDTO = cMSContentTemplateDataHandler.InsertCMSContentTemplate(cMSContentTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                cMSContentTemplateDTO.AcceptChanges();
            }
            else
            {
                if (cMSContentTemplateDTO.IsChanged)
                {
                    cMSContentTemplateDTO = cMSContentTemplateDataHandler.UpdateCMSContentTemplate(cMSContentTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    cMSContentTemplateDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the CMSContentTemplateDTO
        /// </summary>
        public CMSContentTemplateDTO getCMSContentTemplateDTO
        {
            get { return cMSContentTemplateDTO; }
        }

    }
    /// <summary>
    /// Manages the list of CMSContentTemplate
    /// </summary>
    public class CMSContentTemplateListBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the CMSContentTemplate list
        /// </summary>
        public List<CMSContentTemplateDTO> GetContentTemplateDTOList(List<KeyValuePair<CMSContentTemplateDTO.SearchByCMSContentTemplateParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CMSContentTemplateDataHandler cMSContentTemplateDataHandler = new CMSContentTemplateDataHandler(sqlTransaction);
            List<CMSContentTemplateDTO> cmsContentTemplateDTOList = cMSContentTemplateDataHandler.GetCMSContentTemplateDTOList(searchParameters);
            log.LogMethodExit(cmsContentTemplateDTOList);
            return cmsContentTemplateDTOList;
        }
    }


   
}

