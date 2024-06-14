/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppUIPanelElement
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      21-May-2019    Girish Kundar            Created 
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
    ///  Business logic for AppUIPanelElement class.
    /// </summary>
    public class AppUIPanelElementBL
    {
        private AppUIPanelElementDTO appUIPanelElementDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppUIPanelElementBL class
        /// </summary>
        /// <param name="executionContext"></param>
        private AppUIPanelElementBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AppUIPanelElementBL object using the appUIPanelElementDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="appUIPanelElementDTO">AppUIPanelElementDTO object is passed as parameter</param>
        public AppUIPanelElementBL(ExecutionContext executionContext, AppUIPanelElementDTO appUIPanelElementDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIPanelElementDTO);
            this.appUIPanelElementDTO = appUIPanelElementDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AppUIPanelElement id as the parameter
        /// Would fetch the AppUIPanelElement object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of AppUIPanelElement Object </param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>

        public AppUIPanelElementBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppUIPanelElementDataHandler appUIPanelElementsDataHandler = new AppUIPanelElementDataHandler(sqlTransaction);
            appUIPanelElementDTO = appUIPanelElementsDataHandler.GetAppUIPanelElementDTO(id);
            if (appUIPanelElementDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AppUIPanelElement", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (appUIPanelElementDTO != null && loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the child records for AppUIPanelElement object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AppUIPanelElementAttributeListBL appUIPanelElementAttributeListBL = new AppUIPanelElementAttributeListBL(executionContext);
            List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ID, appUIPanelElementDTO.UIPanelElementId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            appUIPanelElementDTO.AppUIPanelElementAttributeDTOList = appUIPanelElementAttributeListBL.GetAppUIPanelElementAttributeDTOList(searchParameters, sqlTransaction);

            AppUIElementParameterListBL appUIElementParameterListBL = new AppUIElementParameterListBL(executionContext);
            List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>(AppUIElementParameterDTO.SearchByParameters.UI_PANEL_ELEMENT_ID, appUIPanelElementDTO.UIPanelElementId.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>(AppUIElementParameterDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            appUIPanelElementDTO.AppUIElementParameterDTOList = appUIElementParameterListBL.GetAppUIElementParameterDTOList(psearchParameters, true, true, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be called from the Parent class BL method Save().
        /// Saves the AppUIPanelElement
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (appUIPanelElementDTO.UIPanelElementId > -1 && 
                          appUIPanelElementDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
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
            AppUIPanelElementDataHandler appUIPanelElementDataHandler = new AppUIPanelElementDataHandler(sqlTransaction);
           
            if (appUIPanelElementDTO.UIPanelElementId < 0)
            {
                log.LogVariableState("AppUIPanelElementDTO", appUIPanelElementDTO);
                appUIPanelElementDTO = appUIPanelElementDataHandler.Insert(appUIPanelElementDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIPanelElementDTO.AcceptChanges();
            }
            else if (appUIPanelElementDTO.IsChanged)
            {
                log.LogVariableState("AppUIPanelElementDTO", appUIPanelElementDTO);
                appUIPanelElementDTO = appUIPanelElementDataHandler.Update(appUIPanelElementDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIPanelElementDTO.AcceptChanges();
            }
            SaveAppPanelElementAttrubutes(sqlTransaction);
        }
        /// <summary>
        /// Saves the child records : AppUIPanelElementAttributeDTOList and AppUIElementParametersDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveAppPanelElementAttrubutes(SqlTransaction sqlTransaction)
        {

            // For child records :AppUIPanelElementAttribute
            if (appUIPanelElementDTO.AppUIPanelElementAttributeDTOList != null &&
                appUIPanelElementDTO.AppUIPanelElementAttributeDTOList.Any())
            {
                List<AppUIPanelElementAttributeDTO> updatedAppUIPanelElementDTOList = new List<AppUIPanelElementAttributeDTO>();
                foreach (var appUIPanelElementAttributeDTO in appUIPanelElementDTO.AppUIPanelElementAttributeDTOList)
                {
                    if (appUIPanelElementAttributeDTO.UIPanelElementId != appUIPanelElementDTO.UIPanelElementId)
                    {
                        appUIPanelElementAttributeDTO.UIPanelElementId = appUIPanelElementDTO.UIPanelElementId;
                    }
                    if (appUIPanelElementAttributeDTO.IsChanged)
                    {
                        updatedAppUIPanelElementDTOList.Add(appUIPanelElementAttributeDTO);
                    }
                }
                if (updatedAppUIPanelElementDTOList.Any())
                {
                    log.LogVariableState("UpdatedAppUIPanelElementDTOList", updatedAppUIPanelElementDTOList);
                    AppUIPanelElementAttributeListBL appUIPanelElementAttributeBL = new AppUIPanelElementAttributeListBL(executionContext, updatedAppUIPanelElementDTOList);
                    appUIPanelElementAttributeBL.Save(sqlTransaction);
                }
            }

            if (appUIPanelElementDTO.AppUIElementParameterDTOList != null &&
                appUIPanelElementDTO.AppUIElementParameterDTOList.Any())
            {
                List<AppUIElementParameterDTO> updatedAppUIElementparameterDTOList = new List<AppUIElementParameterDTO>();
                foreach (var appUIElementParametersDTO in appUIPanelElementDTO.AppUIElementParameterDTOList)
                {
                    if (appUIElementParametersDTO.UIPanelElementId != appUIPanelElementDTO.UIPanelElementId)
                    {
                        appUIElementParametersDTO.UIPanelElementId = appUIPanelElementDTO.UIPanelElementId;
                    }
                    if (appUIElementParametersDTO.IsChanged)
                    {
                        updatedAppUIElementparameterDTOList.Add(appUIElementParametersDTO);
                    }
                }
                if (updatedAppUIElementparameterDTOList.Any())
                {
                    AppUIElementParameterListBL appUIElementParameterListBL = new AppUIElementParameterListBL(executionContext, updatedAppUIElementparameterDTOList);
                    appUIElementParameterListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug("appUIPanelElementDTO.AppUIPanelElementAttributeDTOList");
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the appUIPanelElementDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete(AppUIPanelElementDTO appUIPanelElementDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIPanelElementDTO, sqlTransaction);
            try
            {
                AppUIPanelElementDataHandler appUIPanelElementDataHandler = new AppUIPanelElementDataHandler(sqlTransaction);
               
                if (appUIPanelElementDTO.AppUIPanelElementAttributeDTOList != null && appUIPanelElementDTO.AppUIPanelElementAttributeDTOList.Any())
                {
                    AppUIPanelElementAttributeListBL appUIPanelElementAttributeBL = new AppUIPanelElementAttributeListBL(executionContext, appUIPanelElementDTO.AppUIPanelElementAttributeDTOList);
                    appUIPanelElementAttributeBL.Delete();
                }
                if( appUIPanelElementDTO.AppUIElementParameterDTOList != null && appUIPanelElementDTO.AppUIElementParameterDTOList.Any())
                {
                    AppUIElementParameterListBL appUIElementParameterListBL = new AppUIElementParameterListBL(executionContext, appUIPanelElementDTO.AppUIElementParameterDTOList);
                    appUIElementParameterListBL.Delete();
                }
                 
                log.LogVariableState("appUIPanelElementDTO", appUIPanelElementDTO);
                if(appUIPanelElementDTO.UIPanelElementId >= 0 && appUIPanelElementDTO.ActiveFlag == false)
                {
                    appUIPanelElementDataHandler.Delete(appUIPanelElementDTO);
                }
                appUIPanelElementDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Validates the AppUIPanelElement and AppUIPanelElementAttributeList , AppUIElementParametersDTOList - children 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(appUIPanelElementDTO.ElementName))
            {
                validationErrorList.Add(new ValidationError("AppUIPanelElementAttribute", "ElementName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Element Name"))));
            }
            if (!string.IsNullOrWhiteSpace(appUIPanelElementDTO.ElementName) && appUIPanelElementDTO.ElementName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("appUIPanelElementDTO", "ElementName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Element Name"), 50)));
            }
            if (appUIPanelElementDTO.AppUIPanelElementAttributeDTOList != null)
            {
                foreach (var appUIPanelElementAttributeDTO in appUIPanelElementDTO.AppUIPanelElementAttributeDTOList)
                {
                    if (appUIPanelElementAttributeDTO.IsChanged)
                    {
                        log.LogVariableState("AppUIPanelElementAttributeDTO", appUIPanelElementAttributeDTO);
                        AppUIPanelElementAttributeBL appUIPanelElementAttributeBL = new AppUIPanelElementAttributeBL(executionContext, appUIPanelElementAttributeDTO);
                        validationErrorList.AddRange(appUIPanelElementAttributeBL.Validate(sqlTransaction));
                    }
                }
            }

            if (appUIPanelElementDTO.AppUIElementParameterDTOList != null)
            {
                foreach (var appUIElementParameterDTO in appUIPanelElementDTO.AppUIElementParameterDTOList)
                {
                    if (appUIElementParameterDTO.IsChanged)
                    {
                        log.LogVariableState("AppUIElementParameterDTO", appUIElementParameterDTO);
                        AppUIElementParameterBL appUIElementParameterBL = new AppUIElementParameterBL(executionContext, appUIElementParameterDTO);
                        validationErrorList.AddRange(appUIElementParameterBL.Validate(sqlTransaction));
                    }
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppUIPanelElementDTO AppUIPanelElementDTO
        {
            get
            {
                return appUIPanelElementDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AppUIPanelElementAttribute
    /// </summary>
    public class AppUIPanelElementListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AppUIPanelElementDTO> appUIPanelElementDTOList = new List<AppUIPanelElementDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        public AppUIPanelElementListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext object as parameter</param>
        /// <param name="appUIPanelElementDTOList">AppUIPanelElementsDTO List object as parameter</param>
        public AppUIPanelElementListBL(ExecutionContext executionContext,
                                               List<AppUIPanelElementDTO> appUIPanelElementDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIPanelElementDTOList);
            this.appUIPanelElementDTOList = appUIPanelElementDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the AppUIPanelElementDTO List based on the search Parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>returns the AppUIPanelElementDTO List</returns>
        public List<AppUIPanelElementDTO> GetAppUIPanelElementDTOList(List<KeyValuePair<AppUIPanelElementDTO.SearchByParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppUIPanelElementDataHandler appUIPanelElementsDataHandler = new AppUIPanelElementDataHandler(sqlTransaction);
            List<AppUIPanelElementDTO> appUIPanelElementDTOList = appUIPanelElementsDataHandler.GetAppUIPanelElements(searchParameters);
            if (loadChildRecords && appUIPanelElementDTOList.Any())
            {
                Build(appUIPanelElementDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(appUIPanelElementDTOList);
            return appUIPanelElementDTOList;
        }
        /// <summary>
        /// Builds the List of AppUIPanelElement object based on the list of AppUIPanelElement id.
        /// </summary>
        /// <param name="appUIPanelElementDTOList">AppUIPanelElementDTO List</param>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(List<AppUIPanelElementDTO> appUIPanelElementDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIPanelElementDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AppUIPanelElementDTO> appUIPanelElementsIdAppUIPanelElementsDictionary = new Dictionary<int, AppUIPanelElementDTO>();
            string appElementIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < appUIPanelElementDTOList.Count; i++)
            {
                if (appUIPanelElementDTOList[i].UIPanelElementId == -1 ||
                    appUIPanelElementsIdAppUIPanelElementsDictionary.ContainsKey(appUIPanelElementDTOList[i].UIPanelElementId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(appUIPanelElementDTOList[i].UIPanelElementId.ToString());
                appUIPanelElementsIdAppUIPanelElementsDictionary.Add(appUIPanelElementDTOList[i].UIPanelElementId, appUIPanelElementDTOList[i]);
            }
            appElementIdSet = sb.ToString();
            AppUIPanelElementAttributeListBL appUIPanelElementAttributeListBL = new AppUIPanelElementAttributeListBL(executionContext);
            List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST, appElementIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIPanelElementAttributeDTO.SearchByParameters, string>(AppUIPanelElementAttributeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }

            List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList = appUIPanelElementAttributeListBL.GetAppUIPanelElementAttributeDTOList(searchParameters, sqlTransaction);
            if (appUIPanelElementAttributeDTOList != null && appUIPanelElementAttributeDTOList.Any())
            {
                log.LogVariableState("AppUIPanelElementAttributeDTOList", appUIPanelElementAttributeDTOList);
                foreach (var appUIPanelElementAttributeDTO in appUIPanelElementAttributeDTOList)
                {
                    if (appUIPanelElementsIdAppUIPanelElementsDictionary.ContainsKey(appUIPanelElementAttributeDTO.UIPanelElementId))
                    {
                        if (appUIPanelElementsIdAppUIPanelElementsDictionary[appUIPanelElementAttributeDTO.UIPanelElementId].AppUIPanelElementAttributeDTOList == null)
                        {
                            appUIPanelElementsIdAppUIPanelElementsDictionary[appUIPanelElementAttributeDTO.UIPanelElementId].AppUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
                        }
                        appUIPanelElementsIdAppUIPanelElementsDictionary[appUIPanelElementAttributeDTO.UIPanelElementId].AppUIPanelElementAttributeDTOList.Add(appUIPanelElementAttributeDTO);
                    }
                }
            }

            AppUIElementParameterListBL appUIElementParameterListBL = new AppUIElementParameterListBL(executionContext);
            List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>> psearchParameters = new List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>>();
            psearchParameters.Add(new KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>(AppUIElementParameterDTO.SearchByParameters.UI_PANEL_ELEMENT_ID_LIST, appElementIdSet.ToString()));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>(AppUIElementParameterDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<AppUIElementParameterDTO> appUIElementParameterDTOList = appUIElementParameterListBL.GetAppUIElementParameterDTOList(psearchParameters, true, true, sqlTransaction);
            if (appUIElementParameterDTOList != null && appUIElementParameterDTOList.Any())
            {
                log.LogVariableState("AppUIElementParameterDTOList", appUIElementParameterDTOList);
                foreach (var appUIElementParameterDTO in appUIElementParameterDTOList)
                {
                    if (appUIPanelElementsIdAppUIPanelElementsDictionary.ContainsKey(appUIElementParameterDTO.UIPanelElementId))
                    {
                        if (appUIPanelElementsIdAppUIPanelElementsDictionary[appUIElementParameterDTO.UIPanelElementId].AppUIElementParameterDTOList == null)
                        {
                            appUIPanelElementsIdAppUIPanelElementsDictionary[appUIElementParameterDTO.UIPanelElementId].AppUIElementParameterDTOList = new List<AppUIElementParameterDTO>();
                        }
                        appUIPanelElementsIdAppUIPanelElementsDictionary[appUIElementParameterDTO.UIPanelElementId].AppUIElementParameterDTOList.Add(appUIElementParameterDTO);
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the AppUIPanelElement List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIPanelElementDTOList == null ||
                appUIPanelElementDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < appUIPanelElementDTOList.Count; i++)
            {
                var appUIPanelElementDTO = appUIPanelElementDTOList[i];
                if (appUIPanelElementDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AppUIPanelElementBL appUIPanelElementBL = new AppUIPanelElementBL(executionContext, appUIPanelElementDTO);
                    appUIPanelElementBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AppUIPanelElementDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppUIPanelElementDTO", appUIPanelElementDTO);
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
            if (appUIPanelElementDTOList != null && appUIPanelElementDTOList.Any())
            {
                foreach (AppUIPanelElementDTO appUIPanelElementDTO in appUIPanelElementDTOList)
                {
                    if (appUIPanelElementDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppUIPanelElementBL appUIPanelElementBL = new AppUIPanelElementBL(executionContext, appUIPanelElementDTO);
                                appUIPanelElementBL.Delete(appUIPanelElementDTO, parafaitDBTrx.SQLTrx);
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
