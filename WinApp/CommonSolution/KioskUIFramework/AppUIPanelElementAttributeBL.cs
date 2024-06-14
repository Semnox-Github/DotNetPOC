/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppUIPanelElementAttribute
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      21-May-2019   Girish Kundar           Created 
 *2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
*2.90        24-Aug-2020   Girish Kundar            Modified : Issue Fix Child entity delete
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppUIPanelElementAttribute class.
    /// </summary>
    public class AppUIPanelElementAttributeBL
    {
        private AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppUIPanelElementAttributeBL class
        /// </summary>
        private AppUIPanelElementAttributeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AppUIPanelElementAttributeBL object using the AppUIPanelElementAttributeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="appUIPanelElementAttributeDTO">AppUIPanelElementAttributeDTO object</param>
        public AppUIPanelElementAttributeBL(ExecutionContext executionContext, AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIPanelElementAttributeDTO);
            this.appUIPanelElementAttributeDTO = appUIPanelElementAttributeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AppUIPanelElementAttribute id as the parameter
        /// Would fetch the AppUIPanelElementAttribute object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        /// <param name="id"> AppUIPanelElementAttribute Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AppUIPanelElementAttributeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppUIPanelElementAttributeDataHandler appUIPanelElementAttributeDataHandler = new AppUIPanelElementAttributeDataHandler(sqlTransaction);
            appUIPanelElementAttributeDTO = appUIPanelElementAttributeDataHandler.GetAppUIPanelElementAttributeDTO(id);
            if (appUIPanelElementAttributeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " AppUIPanelElementAttribute", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is called from the Parent class  AppUIPanelElementBL Save() method.
        /// Saves the AppUIPanelElementAttributeDTO. This method id called from the Parent class Save() method .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIPanelElementAttributeDTO.IsChanged == false &&
                appUIPanelElementAttributeDTO.UIPanelElementAttributeId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            // Faizan : Uncomment this code if Validation is required
            //List<ValidationError> validationErrors = Validate();
            //if (validationErrors.Any())
            //{
            //    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
            //    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
            //    throw new ValidationException(message, validationErrors);
            //}
            AppUIPanelElementAttributeDataHandler appUIPanelElementAttributeDataHandler = new AppUIPanelElementAttributeDataHandler(sqlTransaction);
            if (appUIPanelElementAttributeDTO.UIPanelElementAttributeId < 0)
            {
                log.LogVariableState("AppUIPanelElementAttributeDTO", appUIPanelElementAttributeDTO);
                appUIPanelElementAttributeDTO = appUIPanelElementAttributeDataHandler.Insert(appUIPanelElementAttributeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIPanelElementAttributeDTO.AcceptChanges();
            }
            else if (appUIPanelElementAttributeDTO.IsChanged)
            {
                log.LogVariableState("AppUIPanelElementAttributeDTO", appUIPanelElementAttributeDTO);
                appUIPanelElementAttributeDTO = appUIPanelElementAttributeDataHandler.Update(appUIPanelElementAttributeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIPanelElementAttributeDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the  appUIPanelElementAttributeDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete(AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIPanelElementAttributeDTO, sqlTransaction);
            try
            {
                AppUIPanelElementAttributeDataHandler appUIPanelElementAttributeDataHandler = new AppUIPanelElementAttributeDataHandler(sqlTransaction);
                if(appUIPanelElementAttributeDTO.ActiveFlag)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                if (appUIPanelElementAttributeDTO.UIPanelElementAttributeId >= 0)
                    appUIPanelElementAttributeDataHandler.Delete(appUIPanelElementAttributeDTO);
                appUIPanelElementAttributeDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Validates the AppUIPanelElementAttributeDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.appUIPanelElementAttributeDTO.DisplayText))
            {
                validationErrorList.Add(new ValidationError("AppUIPanelElementAttribute", "DisplayText", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Text"))));
            }
            if (!string.IsNullOrWhiteSpace(this.appUIPanelElementAttributeDTO.DisplayText) && this.appUIPanelElementAttributeDTO.DisplayText.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppUIPanelElementAttribute", "DisplayText", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Display Text "), 50)));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppUIPanelElementAttributeDTO AppUIPanelElementAttributeDTO
        {
            get
            {
                return appUIPanelElementAttributeDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AppUIPanelElementAttribute
    /// </summary>
    public class AppUIPanelElementAttributeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>(); // must
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public AppUIPanelElementAttributeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="appUIPanelElementAttributeDTOList">appUIPanelElementAttributeDTOList passed as a parameter</param>
        public AppUIPanelElementAttributeListBL(ExecutionContext executionContext,
                                                List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIPanelElementAttributeDTOList);
            this.appUIPanelElementAttributeDTOList = appUIPanelElementAttributeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AppUIPanelElementAttribute List based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the AppUIPanelElementAttributeDTO List</returns>
        public List<AppUIPanelElementAttributeDTO> GetAppUIPanelElementAttributeDTOList(List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppUIPanelElementAttributeDataHandler appUIPanelElementAttributeDataHandler = new AppUIPanelElementAttributeDataHandler(sqlTransaction);
            List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList = appUIPanelElementAttributeDataHandler.GetAppUIPanelElementAttributes(searchParameters);
            log.LogMethodExit(appUIPanelElementAttributeDTOList);
            return appUIPanelElementAttributeDTOList;
        }

        /// <summary>
        /// Saves the AppUIPanelElementAttributeDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIPanelElementAttributeDTOList == null ||
               appUIPanelElementAttributeDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < appUIPanelElementAttributeDTOList.Count; i++)
            {
                var appUIPanelElementAttributeDTO = appUIPanelElementAttributeDTOList[i];
                if (appUIPanelElementAttributeDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AppUIPanelElementAttributeBL appUIPanelElementAttributeBL = new AppUIPanelElementAttributeBL(executionContext, appUIPanelElementAttributeDTO);
                    appUIPanelElementAttributeBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AppUIPanelElementAttributeDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppUIPanelElementAttributeDTO", appUIPanelElementAttributeDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the AppUIPanelElementAttributeDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete()
        {
            log.LogMethodEntry();
            if (appUIPanelElementAttributeDTOList != null && appUIPanelElementAttributeDTOList.Any())
            {
                foreach (AppUIPanelElementAttributeDTO appUIPanelElementAttributeDTO in appUIPanelElementAttributeDTOList)
                {
                    if (appUIPanelElementAttributeDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppUIPanelElementAttributeBL appUIPanelElementAttributeBL = new AppUIPanelElementAttributeBL(executionContext, appUIPanelElementAttributeDTO);
                                appUIPanelElementAttributeBL.Delete(appUIPanelElementAttributeDTO, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
