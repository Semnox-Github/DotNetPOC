/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppUIPanel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      21-May-2019    Girish Kundar           Created 
 *2.80      04-Jun-2020   Mushahid Faizan          Modified : 3 Tier Changes for Rest API
*2.90        24-Aug-2020   Girish Kundar            Modified : Issue Fix Child entity delete
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppUIPanel class.
    /// </summary>
    public class AppUIPanelBL
    {
        private AppUIPanelDTO appUIPanelDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppUIPanelBL class
        /// </summary>
        private AppUIPanelBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates AppUIPanelBL object using the AppUIPanelDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="appUIPanelDTO">AppUIPanelDTO object</param>
        public AppUIPanelBL(ExecutionContext executionContext, AppUIPanelDTO appUIPanelDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIPanelDTO);
            this.appUIPanelDTO = appUIPanelDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AppUIPanel id as the parameter
        /// Would fetch the appUIPanel object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id - AppUIPanel</param>
        /// <param name="loadChildRecords">loadChildRecords either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public AppUIPanelBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppUIPanelDataHandler appUIPanelsDataHandler = new AppUIPanelDataHandler(sqlTransaction);
            appUIPanelDTO = appUIPanelsDataHandler.GetAppUIPanel(id);
            if (appUIPanelDTO == null)
            {
                //1567 "Unable to find a AppUIPanel with id " + id
                // Unable to find a &0 with id &1
                // script available
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AppUIPanel", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the child records for AppUIPanel object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AppUIPanelElementListBL appUIPanelElementsListBL = new AppUIPanelElementListBL(executionContext);
            List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>(AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ID, appUIPanelDTO.UIPanelId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>(AppUIPanelElementDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            appUIPanelDTO.AppUIPanelElementsDTOList = appUIPanelElementsListBL.GetAppUIPanelElementDTOList(searchParameters, true, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIPanelDTO.UIPanelId > -1 &&
                    appUIPanelDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AppUIPanelDataHandler appUIPanelsDataHandler = new AppUIPanelDataHandler(sqlTransaction);

            // Faizan : Uncomment this code if Validation is required
            //List<ValidationError> validationErrors = Validate();
            //if (validationErrors.Any())
            //{
            //    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
            //    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
            //    throw new ValidationException(message, validationErrors);
            //}
            if (appUIPanelDTO.UIPanelId < 0)
            {
                log.LogVariableState("AppUIPanelDTO", appUIPanelDTO);
                appUIPanelDTO = appUIPanelsDataHandler.Insert(appUIPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIPanelDTO.AcceptChanges();
            }
            else if (appUIPanelDTO.IsChanged)
            {
                appUIPanelDTO = appUIPanelsDataHandler.Update(appUIPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIPanelDTO.AcceptChanges();
            }
            SaveAppPanelElements(sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the child records : AppUIPanelElementsDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveAppPanelElements(SqlTransaction sqlTransaction)
        {
            if (appUIPanelDTO.AppUIPanelElementsDTOList != null &&
                appUIPanelDTO.AppUIPanelElementsDTOList.Any())
            {
                List<AppUIPanelElementDTO> updatedAppUIPanelElementsDTOList = new List<AppUIPanelElementDTO>();
                foreach (var appUIPanelElementsDTO in appUIPanelDTO.AppUIPanelElementsDTOList)
                {
                    if (appUIPanelElementsDTO.UIPanelId != appUIPanelDTO.UIPanelId)
                    {
                        appUIPanelElementsDTO.UIPanelId = appUIPanelDTO.UIPanelId;
                    }
                    if (appUIPanelElementsDTO.IsChangedRecursive)
                    {
                        updatedAppUIPanelElementsDTOList.Add(appUIPanelElementsDTO);
                    }
                }
                if (updatedAppUIPanelElementsDTOList.Any())
                {
                    AppUIPanelElementListBL appUIPanelElementsListBL = new AppUIPanelElementListBL(executionContext, updatedAppUIPanelElementsDTOList);
                    appUIPanelElementsListBL.Save(sqlTransaction);
                }
            }
        }
        /// <summary>
        /// Delete the  appScreenDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(AppUIPanelDTO appUIPanelDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIPanelDTO, sqlTransaction);
            try
            {
                AppUIPanelDataHandler appUIPanelsDataHandler = new AppUIPanelDataHandler(sqlTransaction);
                if (appUIPanelDTO.AppUIPanelElementsDTOList != null && appUIPanelDTO.AppUIPanelElementsDTOList.Any())
                {
                    AppUIPanelElementListBL appUIPanelElementsListBL = new AppUIPanelElementListBL(executionContext, appUIPanelDTO.AppUIPanelElementsDTOList);
                    appUIPanelElementsListBL.Delete();
                }
                log.LogVariableState("appUIPanelDTO", appUIPanelDTO);
                if (appUIPanelDTO.UIPanelId >= 0 && appUIPanelDTO.ActiveFlag == false)
                {
                    appUIPanelsDataHandler.Delete(appUIPanelDTO);
                }
                appUIPanelDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Validates the AppUIPanelDTO  ,AppUIPanelElementDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(appUIPanelDTO.UIPanelName))
            {
                validationErrorList.Add(new ValidationError("AppUIPanel", "UIPanelName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Panel Name"))));
            }

            if (!string.IsNullOrWhiteSpace(appUIPanelDTO.UIPanelName) && appUIPanelDTO.UIPanelName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppUIPanel", "UIPanelName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Panel Name"), 50)));
            }

            if (appUIPanelDTO.AppUIPanelElementsDTOList != null)
            {
                foreach (var appUIPanelElementsDTO in appUIPanelDTO.AppUIPanelElementsDTOList)
                {
                    if (appUIPanelElementsDTO.IsChangedRecursive)
                    {
                        AppUIPanelElementBL appUIPanelElementsBL = new AppUIPanelElementBL(executionContext, appUIPanelElementsDTO);
                        validationErrorList.AddRange(appUIPanelElementsBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppUIPanelDTO AppUIPanelDTO
        {
            get
            {
                return appUIPanelDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AppUIPanel
    /// </summary>
    public class AppUIPanelListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AppUIPanelDTO> appUIPanelsDTOList = new List<AppUIPanelDTO>();
        /// <summary>
        /// Parameterized constructor of AppUIPanelListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public AppUIPanelListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="appUIPanelDTOList">AppUIPanelDTO List </param>
        public AppUIPanelListBL(ExecutionContext executionContext,
                                               List<AppUIPanelDTO> appUIPanelDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIPanelDTOList);
            this.appUIPanelsDTOList = appUIPanelDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Returns the Get the AppUIPanelDTO list
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords true or false</param>
        /// <param name="activeChildRecords">activeChildRecords true or false </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of AppUIPanelsDTO</returns>
        public List<AppUIPanelDTO> GetAppUIPanelDTOList(List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            //child records needs to  build
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppUIPanelDataHandler appUIPanelsDataHandler = new AppUIPanelDataHandler(sqlTransaction);
            List<AppUIPanelDTO> appUIPanelsDTOList = appUIPanelsDataHandler.GetAppUIPanels(searchParameters);
            if (appUIPanelsDTOList != null && appUIPanelsDTOList.Any() && loadChildRecords)
            {
                Build(appUIPanelsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(appUIPanelsDTOList);
            return appUIPanelsDTOList;
        }
        /// <summary>
        /// Builds the List of AppUIPanel object based on the list of AppUIPanel id.
        /// </summary>
        /// <param name="appUIPanelDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<AppUIPanelDTO> appUIPanelDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIPanelDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AppUIPanelDTO> appUIPanelIdAppUIPanelDictionary = new Dictionary<int, AppUIPanelDTO>();
            string appUIPanelIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < appUIPanelDTOList.Count; i++)
            {
                if (appUIPanelDTOList[i].UIPanelId == -1 ||
                    appUIPanelIdAppUIPanelDictionary.ContainsKey(appUIPanelDTOList[i].UIPanelId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(appUIPanelDTOList[i].UIPanelId);
                appUIPanelIdAppUIPanelDictionary.Add(appUIPanelDTOList[i].UIPanelId, appUIPanelDTOList[i]);
            }
            appUIPanelIdSet = sb.ToString();
            AppUIPanelElementListBL appUIPanelElementListBL = new AppUIPanelElementListBL(executionContext);
            List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>(AppUIPanelElementDTO.SearchByParameters.UI_PANEL_ID_LIST, appUIPanelIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>(AppUIPanelElementDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<AppUIPanelElementDTO> appUIPanelElementsDTOList = appUIPanelElementListBL.GetAppUIPanelElementDTOList(searchParameters, true, activeChildRecords, sqlTransaction);
            if (appUIPanelElementsDTOList != null && appUIPanelElementsDTOList.Any())
            {
                log.LogVariableState("AppUIPanelElementDTOList", appUIPanelElementsDTOList);
                foreach (var appUIPanelElementDTO in appUIPanelElementsDTOList)
                {
                    if (appUIPanelIdAppUIPanelDictionary.ContainsKey(appUIPanelElementDTO.UIPanelId))
                    {
                        if (appUIPanelIdAppUIPanelDictionary[appUIPanelElementDTO.UIPanelId].AppUIPanelElementsDTOList == null)
                        {
                            appUIPanelIdAppUIPanelDictionary[appUIPanelElementDTO.UIPanelId].AppUIPanelElementsDTOList = new List<AppUIPanelElementDTO>();
                        }
                        appUIPanelIdAppUIPanelDictionary[appUIPanelElementDTO.UIPanelId].AppUIPanelElementsDTOList.Add(appUIPanelElementDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of AppUIPanelDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIPanelsDTOList == null ||
                appUIPanelsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < appUIPanelsDTOList.Count; i++)
            {
                var appUIPanelDTO = appUIPanelsDTOList[i];
                if (appUIPanelDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AppUIPanelBL appUIPanelBL = new AppUIPanelBL(executionContext, appUIPanelDTO);
                    appUIPanelBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AppUIPanelDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppUIPanelDTO", appUIPanelDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the appUIPanelsDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (appUIPanelsDTOList != null && appUIPanelsDTOList.Any())
            {
                foreach (AppUIPanelDTO appUIPanelDTO in appUIPanelsDTOList)
                {
                    if (appUIPanelDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppUIPanelBL appUIPanelBL = new AppUIPanelBL(executionContext, appUIPanelDTO);
                                appUIPanelBL.Delete(appUIPanelDTO, parafaitDBTrx.SQLTrx);
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
