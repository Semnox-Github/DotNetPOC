/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppUIElementParameter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        21-May-2019   Girish Kundar           Created 
 *2.80      04-Jun-2020   Mushahid Faizan          Modified : 3 Tier Changes for Rest API
*2.90        24-Aug-2020   Girish Kundar            Modified : Issue Fix Child entity delete
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppUIElementParameter class.
    /// </summary>
    public class AppUIElementParameterBL
    {
        private AppUIElementParameterDTO appUIElementParameterDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppUIElementParameterBL class
        /// </summary>
        /// <param name="executionContext"></param>
        private AppUIElementParameterBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AppUIElementParameterBL object using the appUIElementParameterDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="appUIElementParameterDTO"></param>
        public AppUIElementParameterBL(ExecutionContext executionContext, AppUIElementParameterDTO appUIElementParameterDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIElementParameterDTO);
            this.appUIElementParameterDTO = appUIElementParameterDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the appUIPanelElementAttribute id as the parameter
        /// Would fetch the appUIPanelElementAttribute object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>

        public AppUIElementParameterBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppUIElementParameterDataHandler appUIElementParameterDataHandler = new AppUIElementParameterDataHandler(sqlTransaction);
            appUIElementParameterDTO = appUIElementParameterDataHandler.GetAppUIElementParameter(id);
            if (appUIElementParameterDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AppUIElementParameter", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (appUIElementParameterDTO != null && loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            AppUIElementParameterAttributeListBL appUIElementParameterAttributeListBL = new AppUIElementParameterAttributeListBL(executionContext);
            List<KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>(AppUIElementParameterAttributeDTO.SearchByParameters.PARAMETER_ID, appUIElementParameterDTO.ParameterId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>(AppUIElementParameterAttributeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            appUIElementParameterDTO.AppUIElementParameterAttributeDTOList = appUIElementParameterAttributeListBL.GetAllAppUIElementParameterAttributeDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIElementParameterAttribute
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (appUIElementParameterDTO.ParameterId > -1 && 
                         appUIElementParameterDTO.IsChangedRecursive == false)
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
            AppUIElementParameterDataHandler appUIElementParameterDataHandler = new AppUIElementParameterDataHandler(sqlTransaction);

            if (appUIElementParameterDTO.ParameterId < 0)
            {
                log.LogVariableState("AppUIElementParameterDTO", appUIElementParameterDTO);
                appUIElementParameterDTO = appUIElementParameterDataHandler.Insert(appUIElementParameterDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIElementParameterDTO.AcceptChanges();
            }
            else if (appUIElementParameterDTO.IsChanged)
            {
                log.LogVariableState("AppUIElementParameterDTO", appUIElementParameterDTO);
                appUIElementParameterDTO = appUIElementParameterDataHandler.Update(appUIElementParameterDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIElementParameterDTO.AcceptChanges();
            }
            SaveAppUIElementParameterAttrubutes(sqlTransaction);

        }
        private void SaveAppUIElementParameterAttrubutes(SqlTransaction sqlTransaction)
        {

            // For child records :AppUIPanelElementAttribute
            if (appUIElementParameterDTO.AppUIElementParameterAttributeDTOList != null &&
                appUIElementParameterDTO.AppUIElementParameterAttributeDTOList.Any())
            {
                List<AppUIElementParameterAttributeDTO> updatedAppUIElementParameterAttributeDTOList = new List<AppUIElementParameterAttributeDTO>();
                foreach (var appUIElementParameterAttributeDTO in appUIElementParameterDTO.AppUIElementParameterAttributeDTOList)
                {
                    if (appUIElementParameterAttributeDTO.ParameterId != appUIElementParameterDTO.ParameterId)
                    {
                        appUIElementParameterAttributeDTO.ParameterId = appUIElementParameterDTO.ParameterId;
                    }
                    if (appUIElementParameterAttributeDTO.IsChanged)
                    {
                        updatedAppUIElementParameterAttributeDTOList.Add(appUIElementParameterAttributeDTO);
                    }
                }
                if (updatedAppUIElementParameterAttributeDTOList.Any())
                {
                    log.LogVariableState("UpdatedAppUIElementParameterAttributeDTOList", updatedAppUIElementParameterAttributeDTOList);
                    AppUIElementParameterAttributeListBL appUIElementParameterAttributeListBL = new AppUIElementParameterAttributeListBL(executionContext, updatedAppUIElementParameterAttributeDTOList);
                    appUIElementParameterAttributeListBL.Save(sqlTransaction);
                }
            }
            else
            {
                log.Debug("appUIElementParameterDTO.AppUIElementParameterAttributeDTOList");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the appUIElementParameterDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete(AppUIElementParameterDTO appUIElementParameterDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIElementParameterDTO, sqlTransaction);
            try
            {
                AppUIElementParameterDataHandler appUIElementParameterDataHandler = new AppUIElementParameterDataHandler(sqlTransaction);
                if ((appUIElementParameterDTO.AppUIElementParameterAttributeDTOList != null && appUIElementParameterDTO.AppUIElementParameterAttributeDTOList.Any(x => x.ActiveFlag == true)))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("appUIElementParameterDTO", appUIElementParameterDTO);

                // Call Delete method for the Child DTO.
                AppUIElementParameterAttributeListBL appUIElementParameterAttributeListBL = new AppUIElementParameterAttributeListBL(executionContext, appUIElementParameterDTO.AppUIElementParameterAttributeDTOList);
                appUIElementParameterAttributeListBL.Delete();

                if (appUIElementParameterDTO.ParameterId >= 0 && appUIElementParameterDTO.ActiveFlag == false)
                {
                    appUIElementParameterDataHandler.Delete(appUIElementParameterDTO);
                }
                appUIElementParameterDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validates the AppUIElementParameterDTO , AppUIElementParameterAttributeDTOList- child 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(appUIElementParameterDTO.ParameterName))
            {
                validationErrorList.Add(new ValidationError("AppUIElementParameter", "ParameterName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Parameter Name"))));
            }
            if (!string.IsNullOrWhiteSpace(appUIElementParameterDTO.ParameterName) && appUIElementParameterDTO.ParameterName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppUIElementParameter", "ParameterName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "ParameterName"), 50)));
            }
            if (appUIElementParameterDTO.AppUIElementParameterAttributeDTOList != null)
            {
                foreach (var appUIElementParameterAttributeDTO in appUIElementParameterDTO.AppUIElementParameterAttributeDTOList)
                {
                    if (appUIElementParameterAttributeDTO.IsChanged)
                    {
                        log.LogVariableState("AppUIElementParameterAttributeDTO", appUIElementParameterAttributeDTO);
                        AppUIElementParameterAttributeBL appUIElementParameterAttributeBL = new AppUIElementParameterAttributeBL(executionContext, appUIElementParameterAttributeDTO);
                        validationErrorList.AddRange(appUIElementParameterAttributeBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppUIElementParameterDTO AppUIElementParameterDTO
        {
            get
            {
                return appUIElementParameterDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of AppUIElementParameter
    /// </summary>
    public class AppUIElementParameterListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AppUIElementParameterDTO> appUIElementParameterDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AppUIElementParameterListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="appUIElementParameterDTOList"></param>
        public AppUIElementParameterListBL(ExecutionContext executionContext,
                                               List<AppUIElementParameterDTO> appUIElementParameterDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIElementParameterDTOList);
            this.appUIElementParameterDTOList = appUIElementParameterDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the AppUIElementParameterDTO List bases on search Parameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AppUIElementParameterDTO> GetAppUIElementParameterDTOList(List<KeyValuePair<AppUIElementParameterDTO.SearchByParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppUIElementParameterDataHandler appUIElementParameterDataHandler = new AppUIElementParameterDataHandler(sqlTransaction);
            List<AppUIElementParameterDTO> appUIElementParameterDTOList = appUIElementParameterDataHandler.GetAllAppUIElementParameter(searchParameters);
            if (loadChildRecords && appUIElementParameterDTOList.Any())
            {
                Build(appUIElementParameterDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(appUIElementParameterDTOList);
            return appUIElementParameterDTOList;
        }

        private void Build(List<AppUIElementParameterDTO> appUIElementParameterDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIElementParameterDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AppUIElementParameterDTO> appUIElementParameterIdDictionary = new Dictionary<int, AppUIElementParameterDTO>();
            string appElementIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < appUIElementParameterDTOList.Count; i++)
            {
                if (appUIElementParameterDTOList[i].ParameterId == -1 || appUIElementParameterIdDictionary.ContainsKey(appUIElementParameterDTOList[i].ParameterId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(appUIElementParameterDTOList[i].ParameterId);
                appUIElementParameterIdDictionary.Add(appUIElementParameterDTOList[i].ParameterId, appUIElementParameterDTOList[i]);
            }
            appElementIdSet = sb.ToString();
            AppUIElementParameterAttributeListBL appUIElementParameterAttributeListBL = new AppUIElementParameterAttributeListBL(executionContext);
            List<KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>(AppUIElementParameterAttributeDTO.SearchByParameters.PARAMETER_ID_LIST, appElementIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>(AppUIElementParameterAttributeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            }
            List<AppUIElementParameterAttributeDTO> appUIElementParameterAttributeDTOList = appUIElementParameterAttributeListBL.GetAllAppUIElementParameterAttributeDTOList(searchParameters, sqlTransaction);
            if (appUIElementParameterAttributeDTOList != null && appUIElementParameterAttributeDTOList.Any())
            {
                log.LogVariableState("AppUIElementParameterAttributeDTOList", appUIElementParameterAttributeDTOList);
                foreach (var appUIElementParameterAttributeDTO in appUIElementParameterAttributeDTOList)
                {
                    if (appUIElementParameterIdDictionary.ContainsKey(appUIElementParameterAttributeDTO.ParameterId))
                    {
                        if (appUIElementParameterIdDictionary[appUIElementParameterAttributeDTO.ParameterId].AppUIElementParameterAttributeDTOList == null)
                        {
                            appUIElementParameterIdDictionary[appUIElementParameterAttributeDTO.ParameterId].AppUIElementParameterAttributeDTOList = new List<AppUIElementParameterAttributeDTO>();
                        }
                        appUIElementParameterIdDictionary[appUIElementParameterAttributeDTO.ParameterId].AppUIElementParameterAttributeDTOList.Add(appUIElementParameterAttributeDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIElementParameter List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIElementParameterDTOList == null ||
                appUIElementParameterDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < appUIElementParameterDTOList.Count; i++)
            {
                var appUIElementParameterDTO = appUIElementParameterDTOList[i];
                if (appUIElementParameterDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    AppUIElementParameterBL appUIElementParameterBL = new AppUIElementParameterBL(executionContext, appUIElementParameterDTO);
                    appUIElementParameterBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AppUIElementParameterDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppUIElementParameterDTO", appUIElementParameterDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the appUIElementParameterDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete()
        {
            log.LogMethodEntry();
            if (appUIElementParameterDTOList != null && appUIElementParameterDTOList.Any())
            {
                foreach (AppUIElementParameterDTO appUIElementParameterDTO in appUIElementParameterDTOList)
                {
                    if (appUIElementParameterDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppUIElementParameterBL appUIElementParameterBL = new AppUIElementParameterBL(executionContext, appUIElementParameterDTO);
                                appUIElementParameterBL.Delete(appUIElementParameterDTO, parafaitDBTrx.SQLTrx);
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
