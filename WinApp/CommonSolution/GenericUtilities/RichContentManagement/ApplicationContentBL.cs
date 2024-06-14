/********************************************************************************************
 * Project Name - ApplicationContentBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Jeevan           Created 
 *2.4.0       25-Nov-2018      Raghuveera       Children loading and saving is modified
 *2.70.2        25-Jul-2019      Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90       21-May-2020       Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 *2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 ********************************************************************************************/
using Semnox.Parafait.Languages;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;
using System;
using System.Text;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// ApplicationContentBL class
    /// </summary>
    public class ApplicationContentBL
    {
        private ApplicationContentDTO applicationContentDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Parameterized constructor of applicationContentBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ApplicationContentBL(ExecutionContext executionContext)
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
        public ApplicationContentBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ApplicationContentDataHandler applicationContentDataHandler = new ApplicationContentDataHandler(sqlTransaction);
            applicationContentDTO = applicationContentDataHandler.GetApplicationContent(id);
            if (applicationContentDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "applicationContent", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (applicationContentDTO.ContentId > -1)
            {
                Build(sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            RichContentBL richContentBL = new RichContentBL(executionContext, applicationContentDTO.ContentId, sqlTransaction);
            applicationContentDTO.RichContentDTO = richContentBL.RichContentDTO;
            applicationContentDTO.AcceptChanges();
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates applicationContentBL object using the applicationContentDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="applicationContentDTO">applicationContentDTO object</param>
        public ApplicationContentBL(ExecutionContext executionContext, ApplicationContentDTO applicationContentDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationContentDTO);
            this.applicationContentDTO = applicationContentDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the applicationContent
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            ApplicationContentDataHandler applicationContentDataHandler = new ApplicationContentDataHandler(sqlTransaction);
            if (applicationContentDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (applicationContentDTO.IsActive == true)
            {
                if (applicationContentDTO.AppContentId < 0)
                {
                    applicationContentDTO = applicationContentDataHandler.InsertApplicationContent(applicationContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    applicationContentDTO.AcceptChanges();
                }
                else
                {
                    if (applicationContentDTO.IsChanged)
                    {
                        applicationContentDTO = applicationContentDataHandler.UpdateApplicationContent(applicationContentDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        applicationContentDTO.AcceptChanges();
                    }
                }
                SaveApplicationContentChild(sqlTransaction);
            }
            else  // Hard Delete only for the existing Files . For new file use Soft Delete method
            {
                if ((applicationContentDTO.ApplicationContentTranslatedDTOList != null && applicationContentDTO.ApplicationContentTranslatedDTOList.Any(x => x.IsActive == true)))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("applicationContentDTO", applicationContentDTO);
                SaveApplicationContentChild(sqlTransaction);
                if (applicationContentDTO.AppContentId >= 0)
                {
                    applicationContentDataHandler.Delete(applicationContentDTO);
                }
                applicationContentDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveApplicationContentChild(SqlTransaction sqlTransaction)
        {

            // for Child Records : :ApplicationContentTranslatedDTO
            if (applicationContentDTO.ApplicationContentTranslatedDTOList != null &&
                applicationContentDTO.ApplicationContentTranslatedDTOList.Any())
            {
                List<ApplicationContentTranslatedDTO> updatedApplicationContentTranslatedDTOList = new List<ApplicationContentTranslatedDTO>();
                foreach (ApplicationContentTranslatedDTO applicationContentTranslatedDTO in applicationContentDTO.ApplicationContentTranslatedDTOList)
                {
                    if (applicationContentTranslatedDTO.AppContentId != applicationContentDTO.AppContentId)
                    {
                        applicationContentTranslatedDTO.AppContentId = applicationContentDTO.AppContentId;
                    }
                    if (applicationContentTranslatedDTO.IsChanged)
                    {
                        updatedApplicationContentTranslatedDTOList.Add(applicationContentTranslatedDTO);
                    }
                }
                if (updatedApplicationContentTranslatedDTOList.Any())
                {
                    ApplicationContentTranslatedListBL applicationContentTranslatedListBL = new ApplicationContentTranslatedListBL(executionContext, updatedApplicationContentTranslatedDTOList);
                    applicationContentTranslatedListBL.Save(sqlTransaction);
                }
            }
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ApplicationContentDTO ApplicationContentDTO
        {
            get
            {
                return applicationContentDTO;
            }
        }

        /// <summary>
        /// Validates the customer applicationContentDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            //if (applicationContentDTO.IsActive)
            //{
            //    if (applicationContentDTO.AppContentId == -1)
            //    {
            //        validationErrorList.Add(new ValidationError("applicationContent", "AppContentId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Name"))));
            //    }
            //}
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of ApplicationContentList
    /// </summary>
    public class ApplicationContentListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ApplicationContentDTO> applicationContentDTOList = new List<ApplicationContentDTO>();

        public ApplicationContentListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ApplicationContentListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="applicationContentDTOList"></param>
        public ApplicationContentListBL(ExecutionContext executionContext, List<ApplicationContentDTO> applicationContentDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationContentDTOList);
            this.applicationContentDTOList = applicationContentDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ApplicationContent list
        ///// </summary>
        ///// <param name="searchParameters">searchParameters</param>
        ///// <param name="sqlTransaction">sqlTransaction</param>
        ///// <param name="loadChildren">loadChildren</param>
        ///// <returns></returns>
        //public List<ApplicationContentDTO> GetApplicationContentDTOList(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool loadChildren = false)
        //{
        //    log.LogMethodEntry(searchParameters, sqlTransaction, loadChildren);
        //    ApplicationContentDataHandler applicationContentDataHandler = new ApplicationContentDataHandler(sqlTransaction);
        //    List<ApplicationContentDTO> returnValue = applicationContentDataHandler.GetApplicationContentDTOList(searchParameters, sqlTransaction);
        //    log.LogMethodExit(returnValue);
        //    return returnValue;
        //}
        public List<ApplicationContentDTO> GetApplicationContentDTOList(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApplicationContentDataHandler applicationContentDataHandler = new ApplicationContentDataHandler(sqlTransaction);
            this.applicationContentDTOList = applicationContentDataHandler.GetApplicationContentDTOList(searchParameters);
            if (applicationContentDTOList != null && applicationContentDTOList.Any())
            {
                foreach (ApplicationContentDTO applicationContentDTO in applicationContentDTOList)
                {
                    if (applicationContentDTO.ContentId > -1)
                    {
                        RichContentBL richContentBL = new RichContentBL(executionContext, applicationContentDTO.ContentId, sqlTransaction);
                        applicationContentDTO.RichContentDTO = richContentBL.RichContentDTO;
                    }
                }
                if (loadChildRecords)
                {
                    Build(applicationContentDTOList, loadActiveRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(applicationContentDTOList);
            return applicationContentDTOList;
        }


        private void Build(List<ApplicationContentDTO> applicationContentDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicationContentDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ApplicationContentDTO> appContentIdDictionary = new Dictionary<int, ApplicationContentDTO>();
            StringBuilder sb = new StringBuilder("");
            string appContentIdList;
            for (int i = 0; i < applicationContentDTOList.Count; i++)
            {
                if (applicationContentDTOList[i].AppContentId == -1 ||
                    appContentIdDictionary.ContainsKey(applicationContentDTOList[i].AppContentId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(applicationContentDTOList[i].AppContentId.ToString());
                appContentIdDictionary.Add(applicationContentDTOList[i].AppContentId, applicationContentDTOList[i]);
            }
            appContentIdList = sb.ToString();
            ApplicationContentTranslatedListBL applicationContentTranslatedListBL = new ApplicationContentTranslatedListBL(executionContext);
            List<KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>>();
            searchByParams.Add(new KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>(ApplicationContentTranslatedDTO.SearchByParameters.APP_CONTENT_ID_LIST, appContentIdList.ToString()));
            //searchByParams.Add(new KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>(ApplicationContentTranslatedDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));

            if (activeChildRecords)
            {
                searchByParams.Add(new KeyValuePair<ApplicationContentTranslatedDTO.SearchByParameters, string>(ApplicationContentTranslatedDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<ApplicationContentTranslatedDTO> applicationContentTranslatedList = applicationContentTranslatedListBL.GetApplicationContentTranslatedDTOList(searchByParams, sqlTransaction);
            if (applicationContentTranslatedList != null && applicationContentTranslatedList.Any())
            {
                log.LogVariableState("applicationContentTranslatedList", applicationContentTranslatedList);
                foreach (var applicationContentTranslatedDTO in applicationContentTranslatedList)
                {
                    if (appContentIdDictionary.ContainsKey(applicationContentTranslatedDTO.AppContentId))
                    {
                        if (appContentIdDictionary[applicationContentTranslatedDTO.AppContentId].ApplicationContentTranslatedDTOList == null)
                        {
                            appContentIdDictionary[applicationContentTranslatedDTO.AppContentId].ApplicationContentTranslatedDTOList = new List<ApplicationContentTranslatedDTO>();
                        }
                        appContentIdDictionary[applicationContentTranslatedDTO.AppContentId].ApplicationContentTranslatedDTOList.Add(applicationContentTranslatedDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the  list of applicationContentDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (applicationContentDTOList == null ||
                applicationContentDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < applicationContentDTOList.Count; i++)
            {
                var applicationContentDTO = applicationContentDTOList[i];
                if (applicationContentDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    ApplicationContentBL applicationContentBL = new ApplicationContentBL(executionContext, applicationContentDTO);
                    applicationContentBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving applicationContentDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("applicationContentDTOList", applicationContentDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }


        public DateTime? GetApplicationContentLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            ApplicationContentDataHandler applicationContentDataHandler = new ApplicationContentDataHandler(sqlTransaction);
            DateTime? result = applicationContentDataHandler.GetApplicationContentLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }

}
