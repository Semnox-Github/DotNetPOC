/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppScreenUIPanel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        21-May-2019   Girish Kundar           Created 
 *2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppScreenUIPanel class.
    /// </summary>
    public class AppScreenUIPanelBL
    {
        private AppScreenUIPanelDTO appScreenUIPanelDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppScreenUIPanelBL class
        /// </summary>
        private AppScreenUIPanelBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates AppScreenUIPanelBL object using the appScreenUIPanelDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="appScreenUIPanelDTO">AppUIPanelDTO object</param>
        public AppScreenUIPanelBL(ExecutionContext executionContext, AppScreenUIPanelDTO appScreenUIPanelDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appScreenUIPanelDTO);
            this.appScreenUIPanelDTO = appScreenUIPanelDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the AppScreenUIPanel id as the parameter
        /// Would fetch the AppScreenUIPanel object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public AppScreenUIPanelBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
           bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppScreenUIPanelDataHandler appScreenUIPanelDataHandler = new AppScreenUIPanelDataHandler(sqlTransaction);
            appScreenUIPanelDTO = appScreenUIPanelDataHandler.GetAppScreenUIPanel(id);
            if (appScreenUIPanelDTO == null)
            {
                //1567 "Unable to find a AppUIPanel with id " + id
                // Unable to find a &0 with id &1
                // script available
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AppScreenUIPanel", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appScreenUIPanelDTO.ScreenUIPanelId > -1 && 
                          appScreenUIPanelDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AppScreenUIPanelDataHandler appScreenUIPanelDataHandler = new AppScreenUIPanelDataHandler(sqlTransaction);
            //if (appScreenUIPanelDTO.ActiveFlag)
            //{
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, 14773);
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (appScreenUIPanelDTO.ScreenUIPanelId < 0)
            {
                log.LogVariableState("AppScreenUIPanelDTO", appScreenUIPanelDTO);
                appScreenUIPanelDTO = appScreenUIPanelDataHandler.Insert(appScreenUIPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appScreenUIPanelDTO.AcceptChanges();
            }
            else if (appScreenUIPanelDTO.IsChanged)
            {
                log.LogVariableState("AppScreenUIPanelDTO", appScreenUIPanelDTO);
                appScreenUIPanelDTO = appScreenUIPanelDataHandler.Update(appScreenUIPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appScreenUIPanelDTO.AcceptChanges();
            }
            SaveAppScreenUIPanels(sqlTransaction);

            log.LogMethodExit();
        }
        // Use this if Child record AppUIPanelElementAttributeDTO also to be saved along with the Parent AppScreen
        private void SaveAppScreenUIPanels(SqlTransaction sqlTransaction)
        {
            if (appScreenUIPanelDTO.AppUIPanelElementAttributeDTOList != null &&
                appScreenUIPanelDTO.AppUIPanelElementAttributeDTOList.Any())
            {
                List<AppUIPanelElementAttributeDTO> updatedAppUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
                foreach (var appUIPanelElementAttributeDTO in appScreenUIPanelDTO.AppUIPanelElementAttributeDTOList)
                {
                    if (appUIPanelElementAttributeDTO.ScreenUIPanelId != appScreenUIPanelDTO.ScreenUIPanelId)
                    {
                        appUIPanelElementAttributeDTO.ScreenUIPanelId = appScreenUIPanelDTO.ScreenUIPanelId;
                    }
                    if (appUIPanelElementAttributeDTO.IsChanged)
                    {
                        updatedAppUIPanelElementAttributeDTOList.Add(appUIPanelElementAttributeDTO);
                    }
                }
                if (updatedAppUIPanelElementAttributeDTOList.Any())
                {
                    AppUIPanelElementAttributeListBL appUIPanelElementAttributeListBL = new AppUIPanelElementAttributeListBL(executionContext, updatedAppUIPanelElementAttributeDTOList);
                    appUIPanelElementAttributeListBL.Save(sqlTransaction);
                }
            }
        }
        /// <summary>
        /// Delete the Ads appScreenUIPanelDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete(AppScreenUIPanelDTO appScreenUIPanelDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appScreenUIPanelDTO, sqlTransaction);
            try
            {
                AppScreenUIPanelDataHandler appScreenUIPanelDataHandler = new AppScreenUIPanelDataHandler(sqlTransaction);
                if ((appScreenUIPanelDTO.AppUIPanelElementAttributeDTOList != null && appScreenUIPanelDTO.AppUIPanelElementAttributeDTOList.Any(x => x.ActiveFlag == true)))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("appScreenUIPanelDTO", appScreenUIPanelDTO);
                // Call Delete method for the Child DTO.
                AppUIPanelElementAttributeListBL appUIPanelElementAttributeListBL = new AppUIPanelElementAttributeListBL(executionContext, appScreenUIPanelDTO.AppUIPanelElementAttributeDTOList);
                appUIPanelElementAttributeListBL.Delete();
                if (appScreenUIPanelDTO.ScreenUIPanelId >= 0)
                {
                    appScreenUIPanelDataHandler.Delete(appScreenUIPanelDTO);
                }
                appScreenUIPanelDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the AppUIPanelsDTO  ,AppUIPanelElementsDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (appScreenUIPanelDTO.UIPanelIndex < 0)
            {   // 1133 - The &1 must be a non-negative integer.
                validationErrorList.Add(new ValidationError("AppScreenUIPanel", "UIPanelIndex", MessageContainerList.GetMessage(executionContext, 1133, MessageContainerList.GetMessage(executionContext, "UIPanel Index Value"))));
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppScreenUIPanelDTO AppScreenUIPanelDTO
        {
            get
            {
                return appScreenUIPanelDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of AppScreenUIPanel
    /// </summary>
    public class AppScreenUIPanelListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AppScreenUIPanelDTO> appScreenUIPanelDTOList = new List<AppScreenUIPanelDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AppScreenUIPanelListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="appScreenUIPanelDTOList"></param>
        public AppScreenUIPanelListBL(ExecutionContext executionContext,
                                               List<AppScreenUIPanelDTO> appScreenUIPanelDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appScreenUIPanelDTOList);
            this.appScreenUIPanelDTOList = appScreenUIPanelDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the AppUIPanelDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>appUIPanelsDTOList</returns>
        public List<AppScreenUIPanelDTO> GetAppScreenUIPanelDTOList(List<KeyValuePair<AppScreenUIPanelDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppScreenUIPanelDataHandler appScreenUIPanelDataHandler = new AppScreenUIPanelDataHandler(sqlTransaction);
            List<AppScreenUIPanelDTO> appScreenUIPanelDTOList = appScreenUIPanelDataHandler.GetAllAppScreenUIPanel(searchParameters);
            if (appScreenUIPanelDTOList != null && appScreenUIPanelDTOList.Any() && loadChildRecords)
            {
                Build(appScreenUIPanelDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(appScreenUIPanelDTOList);
            return appScreenUIPanelDTOList;
        }
        private void Build(List<AppScreenUIPanelDTO> appScreenUIPanelList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appScreenUIPanelList, activeChildRecords, sqlTransaction);
            Dictionary<int, AppScreenUIPanelDTO> appScreenUIPanelIdDictionary = new Dictionary<int, AppScreenUIPanelDTO>();
            StringBuilder sb = new StringBuilder("");
            // List<int> appScreenIdSet = new List<int>();
            string appScreenUIPanelIdSet = string.Empty;
            for (int i = 0; i < appScreenUIPanelList.Count; i++)
            {
                if (appScreenUIPanelList[i].ScreenUIPanelId == -1 ||
                    appScreenUIPanelIdDictionary.ContainsKey(appScreenUIPanelList[i].ScreenUIPanelId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(appScreenUIPanelList[i].ScreenUIPanelId.ToString());
                appScreenUIPanelIdDictionary.Add(appScreenUIPanelList[i].ScreenUIPanelId, appScreenUIPanelList[i]);

                //appScreenIdSet.Add(appScreenUIPanelList[i].ScreenId);
                //appScreenUIPanelIdDictionary.Add(appScreenUIPanelList[i].ScreenUIPanelId, appScreenUIPanelList[i]);
            }
            appScreenUIPanelIdSet = sb.ToString();
            AppUIPanelElementAttributeListBL appUIPanelElementAttributeListBL = new AppUIPanelElementAttributeListBL(executionContext);
            List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.SCREEN_UI_PANEL_ID_LIST, appScreenUIPanelIdSet.ToString()));
            searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<AppUIPanelElementAttributeDTO> appScreenUIPanelDTOList = appUIPanelElementAttributeListBL.GetAppUIPanelElementAttributeDTOList(searchParameters);
            if (appScreenUIPanelDTOList.Any() && appScreenUIPanelDTOList != null)
            {
                log.LogVariableState("appScreenUIPanelDTOList", appScreenUIPanelDTOList);
                foreach (var appScreenUIPanelDTO in appScreenUIPanelDTOList)
                {
                    if (appScreenUIPanelIdDictionary.ContainsKey(appScreenUIPanelDTO.ScreenUIPanelId))
                    {
                        if (appScreenUIPanelIdDictionary[appScreenUIPanelDTO.ScreenUIPanelId].AppUIPanelElementAttributeDTOList == null)
                        {
                            appScreenUIPanelIdDictionary[appScreenUIPanelDTO.ScreenUIPanelId].AppUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
                        }
                        appScreenUIPanelIdDictionary[appScreenUIPanelDTO.ScreenUIPanelId].AppUIPanelElementAttributeDTOList.Add(appScreenUIPanelDTO);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// SAving the appScreenUIPanel List obkects 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appScreenUIPanelDTOList == null ||
                appScreenUIPanelDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < appScreenUIPanelDTOList.Count; i++)
            {
                var appScreenUIPanelDTO = appScreenUIPanelDTOList[i];
                if (appScreenUIPanelDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AppScreenUIPanelBL appScreenUIPanelBL = new AppScreenUIPanelBL(executionContext, appScreenUIPanelDTO);
                    appScreenUIPanelBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving AppScreenUIPanelDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppScreenUIPanelDTO", appScreenUIPanelDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the appScreenUIPanelDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete()
        {
            log.LogMethodEntry();
            if (appScreenUIPanelDTOList != null && appScreenUIPanelDTOList.Any())
            {
                foreach (AppScreenUIPanelDTO appScreenUIPanelDTO in appScreenUIPanelDTOList)
                {
                    if (appScreenUIPanelDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppScreenUIPanelBL appScreenUIPanelBL = new AppScreenUIPanelBL(executionContext, appScreenUIPanelDTO);
                                appScreenUIPanelBL.Delete(appScreenUIPanelDTO, parafaitDBTrx.SQLTrx);
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
