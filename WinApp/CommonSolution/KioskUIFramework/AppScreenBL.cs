/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppScreens
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        21-May-2019   Girish Kundar           Created 
 *2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppScreens class.
    /// </summary>
    public class AppScreenBL
    {
        private AppScreenDTO appScreenDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppScreenBL class
        /// </summary>
        private AppScreenBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates AppScreenBL object using the appScreenDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="appScreenDTO">AppScreenDTO object</param>
        public AppScreenBL(ExecutionContext executionContext, AppScreenDTO appScreenDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appScreenDTO);
            this.appScreenDTO = appScreenDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AppScreens id as the parameter
        /// Would fetch the AppScreenDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public AppScreenBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppScreenDataHandler appScreenDataHandler = new AppScreenDataHandler(sqlTransaction);
            appScreenDTO = appScreenDataHandler.GetAppScreen(id);
            if (appScreenDTO == null)
            {
                //1567 "Unable to find a AppScreens with id " + id
                // Unable to find a &0 with id &1
                // script available
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AppScreens", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        // gets the AppUIPanel Elements bases on the AppScreenProfile Id. 
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AppScreenUIPanelListBL appUIPanelListBL = new AppScreenUIPanelListBL(executionContext);
            List<KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>(AppScreenUIPanelDTO.SearchByParameters.SCREEN_ID, appScreenDTO.ScreenId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>(AppScreenUIPanelDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            appScreenDTO.AppScreenUIPanelDTOList = appUIPanelListBL.GetAppScreenUIPanelDTOList(searchParameters, true, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppScreen
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appScreenDTO.ScreenId > -1 && 
                          appScreenDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AppScreenDataHandler appScreenDataHandler = new AppScreenDataHandler(sqlTransaction);

            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, 14773);
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (appScreenDTO.ScreenId < 0)
            {
                log.LogVariableState("AppScreenDTO", appScreenDTO);
                appScreenDTO = appScreenDataHandler.Insert(appScreenDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appScreenDTO.AcceptChanges();
            }
            else if (appScreenDTO.IsChanged)
            {
                log.LogVariableState("AppScreenDTO", appScreenDTO);
                appScreenDTO = appScreenDataHandler.Update(appScreenDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appScreenDTO.AcceptChanges();
            }
            SaveAppScreenUIPanels(sqlTransaction);

            log.LogMethodExit();
        }
        // Use this if Child record AppScreenUIPanel also to be saved along with the Parent AppScreen
        private void SaveAppScreenUIPanels(SqlTransaction sqlTransaction)
        {
            if (appScreenDTO.AppScreenUIPanelDTOList != null &&
                appScreenDTO.AppScreenUIPanelDTOList.Any())
            {
                List<AppScreenUIPanelDTO> updatedAppScreenUIPanelDTOList = new List<AppScreenUIPanelDTO>();
                foreach (var appScreenUIPanelDTO in appScreenDTO.AppScreenUIPanelDTOList)
                {
                    if (appScreenUIPanelDTO.ScreenId != appScreenDTO.ScreenId)
                    {
                        appScreenUIPanelDTO.ScreenId = appScreenDTO.ScreenId;
                    }
                    if (appScreenUIPanelDTO.IsChangedRecursive)
                    {
                        updatedAppScreenUIPanelDTOList.Add(appScreenUIPanelDTO);
                    }
                }
                if (updatedAppScreenUIPanelDTOList.Any())
                {
                    AppScreenUIPanelListBL appScreenUIPanelListBL = new AppScreenUIPanelListBL(executionContext, updatedAppScreenUIPanelDTOList);
                    appScreenUIPanelListBL.Save(sqlTransaction);
                }
            }
         
        }

        /// <summary>
        /// Validates the AppScreenDTO  ,AppUIPanelDTO - child only if saving is needed. 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(appScreenDTO.ScreenName))
            {
                validationErrorList.Add(new ValidationError("AppScreen", "ScreenName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Screen Name"))));
            }

            if (!string.IsNullOrWhiteSpace(appScreenDTO.ScreenName) && appScreenDTO.ScreenName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppScreen", "ScreenName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Screen Name"), 50)));
            }
            //Use Only if validation before saving of child record  is needed.
            if (appScreenDTO.AppScreenUIPanelDTOList != null)
            {
                foreach (var AppScreenUIPanelDTO in appScreenDTO.AppScreenUIPanelDTOList)
                {
                    if (AppScreenUIPanelDTO.IsChangedRecursive)
                    {
                        AppScreenUIPanelBL appScreenUIPanelBL = new AppScreenUIPanelBL(executionContext, AppScreenUIPanelDTO);
                        validationErrorList.AddRange(appScreenUIPanelBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Delete the  appScreenDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(AppScreenDTO appScreenDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appScreenDTO, sqlTransaction);
            try
            {
                AppScreenDataHandler appScreenDataHandler = new AppScreenDataHandler(sqlTransaction);
                if ((appScreenDTO.AppScreenUIPanelDTOList != null && appScreenDTO.AppScreenUIPanelDTOList.Any(x => x.ActiveFlag == true)))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("appScreenDTO", appScreenDTO);
                // Call Delete for the Child DTO.
                AppScreenUIPanelListBL appScreenUIPanelListBL = new AppScreenUIPanelListBL(executionContext, appScreenDTO.AppScreenUIPanelDTOList);
                appScreenUIPanelListBL.Delete();
                if (appScreenDTO.ScreenId >= 0)
                {
                    appScreenDataHandler.Delete(appScreenDTO);
                }
                appScreenDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppScreenDTO AppScreenDTO
        {
            get
            {
                return appScreenDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of AppScreen
    /// </summary>
    public class AppScreenListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AppScreenDTO> appScreenDTOList = new List<AppScreenDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AppScreenListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="appScreenDTOList"></param>
        public AppScreenListBL(ExecutionContext executionContext,
                                               List<AppScreenDTO> appScreenDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appScreenDTOList);
            this.appScreenDTOList = appScreenDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the AppScreenDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>appScreenDTOList</returns>
        public List<AppScreenDTO> GetAppScreenDTOList(List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            //child records needs to  build
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppScreenDataHandler appScreenDataHandler = new AppScreenDataHandler(sqlTransaction);
            List<AppScreenDTO> appScreenDTOList = appScreenDataHandler.GetAllAppScreen(searchParameters);
            if (appScreenDTOList != null && appScreenDTOList.Any() && loadChildRecords)
            {
                Build(appScreenDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(appScreenDTOList);
            return appScreenDTOList;
        }


        private void Build(List<AppScreenDTO> appScreenDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appScreenDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AppScreenDTO> appScreenIdDictionary = new Dictionary<int, AppScreenDTO>();
            StringBuilder sb = new StringBuilder("");
            string appScreenIdSet;
            for (int i = 0; i < appScreenDTOList.Count; i++)
            {
                if (appScreenDTOList[i].ScreenId == -1 ||
                    appScreenIdDictionary.ContainsKey(appScreenDTOList[i].ScreenId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(appScreenDTOList[i].ScreenId.ToString());
                appScreenIdDictionary.Add(appScreenDTOList[i].ScreenId, appScreenDTOList[i]);
            }
            appScreenIdSet = sb.ToString();
            AppScreenUIPanelListBL appScreenUIPanelListBL = new AppScreenUIPanelListBL(executionContext);
            List<KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>(AppScreenUIPanelDTO.SearchByParameters.SCREEN_ID_LIST, appScreenIdSet.ToString()));
            searchParameters.Add(new KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>(AppScreenUIPanelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>(AppScreenUIPanelDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<AppScreenUIPanelDTO> appScreenUIPanelDTOList = appScreenUIPanelListBL.GetAppScreenUIPanelDTOList(searchParameters, true, activeChildRecords, sqlTransaction);
            if (appScreenUIPanelDTOList.Any() && appScreenUIPanelDTOList != null)
            {
                log.LogVariableState("appScreenUIPanelDTOList", appScreenUIPanelDTOList);
                foreach (var appScreenUIPanelDTO in appScreenUIPanelDTOList)
                {
                    if (appScreenIdDictionary.ContainsKey(appScreenUIPanelDTO.ScreenId))
                    {
                        if (appScreenIdDictionary[appScreenUIPanelDTO.ScreenId].AppScreenUIPanelDTOList == null)
                        {
                            appScreenIdDictionary[appScreenUIPanelDTO.ScreenId].AppScreenUIPanelDTOList = new List<AppScreenUIPanelDTO>();
                        }
                        appScreenIdDictionary[appScreenUIPanelDTO.ScreenId].AppScreenUIPanelDTOList.Add(appScreenUIPanelDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the appScreenDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appScreenDTOList == null ||
                appScreenDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < appScreenDTOList.Count; i++)
            {
                var appScreenDTO = appScreenDTOList[i];
                if (appScreenDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AppScreenBL appScreenBL = new AppScreenBL(executionContext, appScreenDTO);
                    appScreenBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving AppScreenDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppScreenDTO", appScreenDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Delete the AppScreenDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (appScreenDTOList != null && appScreenDTOList.Any())
            {
                foreach (AppScreenDTO appScreenDTO in appScreenDTOList)
                {
                    if (appScreenDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppScreenBL appScreenBL = new AppScreenBL(executionContext, appScreenDTO);
                                appScreenBL.Delete(appScreenDTO, parafaitDBTrx.SQLTrx);
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
