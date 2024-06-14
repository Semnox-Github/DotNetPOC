/********************************************************************************************
 * Project Name - ApplicationContentTransalatedBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019      Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90.0        02-Jun-2020      Girish Kundar    Modified : REST API phase -2 changes / 3 tier standard
 ********************************************************************************************/
using Semnox.Parafait.Languages;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;
using System;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentTranslatedBL
    {
        private ApplicationContentTranslatedDTO applicationContentTranslatedDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of applicationContentBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ApplicationContentTranslatedBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the applicationContent id as the parameter
        /// Would fetch the applicationContent object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ApplicationContentTranslatedBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ApplicationContentTranslatedDataHandler applicationContentTranslatedDataHandler = new ApplicationContentTranslatedDataHandler(sqlTransaction);
            applicationContentTranslatedDTO = applicationContentTranslatedDataHandler.GetApplicationContentTranslated(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates applicationContentBL object using the applicationContentDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="applicationContentDTO">applicationContentDTO object</param>
        public ApplicationContentTranslatedBL(ExecutionContext executionContext, ApplicationContentTranslatedDTO applicationContentTranslatedDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationContentTranslatedDTO);
            this.applicationContentTranslatedDTO = applicationContentTranslatedDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the applicationContent
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (applicationContentTranslatedDTO.IsChanged == false &&
            applicationContentTranslatedDTO.Id > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            ApplicationContentTranslatedDataHandler applicationContentTranslatedDataHandler = new ApplicationContentTranslatedDataHandler(sqlTransaction);
            if (applicationContentTranslatedDTO.IsActive == true)
            {
                if (applicationContentTranslatedDTO.Id < 0)
                {
                    applicationContentTranslatedDTO = applicationContentTranslatedDataHandler.InsertApplicationContent(applicationContentTranslatedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    applicationContentTranslatedDTO.AcceptChanges();
                }
                else
                {
                    if (applicationContentTranslatedDTO.IsChanged)
                    {
                        applicationContentTranslatedDTO = applicationContentTranslatedDataHandler.UpdateApplicationContent(applicationContentTranslatedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        applicationContentTranslatedDTO.AcceptChanges();
                    }
                }
            }
            else  // Hard Delete only for the existing files. For new Files only the Soft Delete to be used. 
            {
                if (applicationContentTranslatedDTO.Id >= 0)
                    applicationContentTranslatedDataHandler.Delete(applicationContentTranslatedDTO);
                applicationContentTranslatedDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ApplicationContentTranslatedDTO ApplicationContentTranslatedDTO
        {
            get
            {
                return applicationContentTranslatedDTO;
            }
        }

        /// <summary>
        /// Validates the ApplicationContentTranslatedDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }
    /// <summary>
    /// Manages the list of ApplicationContentTranslatedListBL
    /// </summary>
    public class ApplicationContentTranslatedListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ApplicationContentTranslatedDTO> applicationContentTranslatedDTOList = new List<ApplicationContentTranslatedDTO>();

        public ApplicationContentTranslatedListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public ApplicationContentTranslatedListBL(ExecutionContext executionContext, List<ApplicationContentTranslatedDTO> applicationContentTranslatedDTOList) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationContentTranslatedDTOList);
            this.applicationContentTranslatedDTOList = applicationContentTranslatedDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Application Content Translated list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ApplicationContentTranslatedDTO> GetApplicationContentTranslatedDTOList(List<KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>> 
                                               searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApplicationContentTranslatedDataHandler applicationContentTranslatedDataHandler = new ApplicationContentTranslatedDataHandler(sqlTransaction);
            List <ApplicationContentTranslatedDTO> returnValue = applicationContentTranslatedDataHandler.GetApplicationContentTranslatedDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Saves the applicationContentTranslated DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (applicationContentTranslatedDTOList == null ||
                applicationContentTranslatedDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < applicationContentTranslatedDTOList.Count; i++)
            {
                var applicationContentTranslatedDTO = applicationContentTranslatedDTOList[i];
                if (applicationContentTranslatedDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ApplicationContentTranslatedBL applicationContentTranslatedBL = new ApplicationContentTranslatedBL(executionContext, applicationContentTranslatedDTO);
                    applicationContentTranslatedBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving applicationContentTranslatedDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("applicationContentTranslatedDTO", applicationContentTranslatedDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }

}
