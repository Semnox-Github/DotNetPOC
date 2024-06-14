/********************************************************************************************
 * Project Name - ApplicationRemarks
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019      Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Business logic for application remarks
    /// </summary>
    public class ApplicationRemarks
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private ApplicationRemarksDTO applicationRemarksDTO;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ApplicationRemarks(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="remarkId"> id of the remark</param>
        /// <param name="sqlTransaction"> sqlTransaction</param>
        public ApplicationRemarks(ExecutionContext executionContext, int remarkId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, remarkId, sqlTransaction);
            ApplicationRemarksDataHandler applicationRemarksDataHandler = new ApplicationRemarksDataHandler(sqlTransaction);
            applicationRemarksDTO = applicationRemarksDataHandler.GetApplicationRemarks(remarkId, sqlTransaction);
            if (applicationRemarksDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ApplicationRemarksDTO", remarkId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="applicationRemarksDTO">Parameter of the type ApplicationRemarksDTO</param>
        public ApplicationRemarks(ExecutionContext executionContext, ApplicationRemarksDTO applicationRemarksDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationRemarksDTO);
            this.applicationRemarksDTO = applicationRemarksDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Application Remarks
        /// Application Remarks will be inserted if RemarkId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction"> sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ApplicationRemarksDataHandler applicationRemarksDataHandler = new ApplicationRemarksDataHandler(sqlTransaction);
            if (applicationRemarksDTO.Id <= 0)
            {
                applicationRemarksDTO = applicationRemarksDataHandler.InsertApplicationRemarks(applicationRemarksDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                applicationRemarksDTO.AcceptChanges();
            }
            else
            {
                if (applicationRemarksDTO.IsChanged)
                {
                    applicationRemarksDTO = applicationRemarksDataHandler.UpdateApplicationRemarks(applicationRemarksDTO, executionContext.GetUserId(), executionContext.GetSiteId(), sqlTransaction);
                    applicationRemarksDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// get the application remarks DTO
        /// </summary>
        public ApplicationRemarksDTO ApplicationRemarksDTO { get { return applicationRemarksDTO; } }
    }
    /// <summary>
    /// Manages the list of application remarks
    /// </summary>
    public class ApplicationRemarksList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ApplicationRemarksDTO> applicationRemarksDTOList = new List<ApplicationRemarksDTO>();

        /// <summary>
        /// Parameterized constructor for ApplicationRemarksList
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ApplicationRemarksList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for ApplicationRemarksList
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="applicationRemarksDTOList">applicationRemarksDTOList passed as a parameter</param>
        public ApplicationRemarksList(ExecutionContext executionContext, List<ApplicationRemarksDTO> applicationRemarksDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationRemarksDTOList);
            this.applicationRemarksDTOList = applicationRemarksDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the application remark list
        /// </summary>
        /// <param name="applicationRemarkId">applicationRemarkId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ApplicationRemarks</returns>
        public ApplicationRemarksDTO GetApplicationRemarks(int applicationRemarkId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicationRemarkId, sqlTransaction);
            ApplicationRemarksDataHandler applicationRemarksDataHandler = new ApplicationRemarksDataHandler(sqlTransaction);
            ApplicationRemarksDTO applicationRemarksDTO =  applicationRemarksDataHandler.GetApplicationRemarks(applicationRemarkId, sqlTransaction);
            log.LogMethodExit(applicationRemarksDTO);
            return applicationRemarksDTO;
        }

        /// <summary>
        ///  Returns the Application Remarks list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ApplicationRemarks</returns>
        public List<ApplicationRemarksDTO> GetAllApplicationRemarks(List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApplicationRemarksDataHandler applicationRemarksDataHandler = new ApplicationRemarksDataHandler(sqlTransaction);
            applicationRemarksDTOList = applicationRemarksDataHandler.GetApplicationRemarksList(searchParameters, sqlTransaction);
            log.LogMethodExit(applicationRemarksDTOList);
            return applicationRemarksDTOList;
        }

    }
}
