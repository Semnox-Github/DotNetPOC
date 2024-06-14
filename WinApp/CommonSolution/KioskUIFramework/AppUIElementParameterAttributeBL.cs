/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for   AppUIElementParameterAttribute
 *
 **************
 ** Version Log
  **************
  * Version     Date        Modified By             Remarks
 *********************************************************************************************
 *2.70        21-May-2019   Girish Kundar           Created
 *2.80      04-Jun-2020   Mushahid Faizan          Modified : 3 Tier Changes for Rest API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppUIElementParameterAttribute class.
    /// </summary>
    public class AppUIElementParameterAttributeBL
    {
        private AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppUIElementParameterAttributeBL class
        /// </summary>
        private AppUIElementParameterAttributeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AppUIElementParameterAttributeBL object using the appUIElementParameterAttributeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="appUIElementParameterAttributeDTO">AppUIElementParameterAttributeDTO object</param>
        public AppUIElementParameterAttributeBL(ExecutionContext executionContext, AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIElementParameterAttributeDTO);
            this.appUIElementParameterAttributeDTO = appUIElementParameterAttributeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the appUIPanelElementParameterAttribute id as the parameter
        /// Would fetch the appUIPanelElementParameterAttribute object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public AppUIElementParameterAttributeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppUIElementParameterAttributeDataHandler appUIElementParameterAttributeDataHandler = new AppUIElementParameterAttributeDataHandler(sqlTransaction);
            appUIElementParameterAttributeDTO = appUIElementParameterAttributeDataHandler.GetAppUIElementParameterAttribute(id);
            if (appUIElementParameterAttributeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " AppUIElementParameterAttributeDTO", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIElementParameterAttributeDTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIElementParameterAttributeDTO.IsChanged == false &&
                appUIElementParameterAttributeDTO.UIParameterAttributeId > -1)
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
            AppUIElementParameterAttributeDataHandler appUIElementParameterAttributeDataHandler = new AppUIElementParameterAttributeDataHandler(sqlTransaction);
            if (appUIElementParameterAttributeDTO.UIParameterAttributeId < 0)
            {
                log.LogVariableState("AppUIElementParameterAttributeDTO", appUIElementParameterAttributeDTO);
                appUIElementParameterAttributeDTO = appUIElementParameterAttributeDataHandler.Insert(appUIElementParameterAttributeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIElementParameterAttributeDTO.AcceptChanges();
            }
            else if (appUIElementParameterAttributeDTO.IsChanged)
            {
                log.LogVariableState("AppUIElementParameterAttributeDTO", appUIElementParameterAttributeDTO);
                appUIElementParameterAttributeDTO = appUIElementParameterAttributeDataHandler.Update(appUIElementParameterAttributeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appUIElementParameterAttributeDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the  appUIElementParameterAttributeDTO from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete(AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(appUIElementParameterAttributeDTO, sqlTransaction);
            try
            {
                AppUIElementParameterAttributeDataHandler appUIElementParameterAttributeDataHandler = new AppUIElementParameterAttributeDataHandler(sqlTransaction);
                if (appUIElementParameterAttributeDTO.UIParameterAttributeId >= 0)
                    appUIElementParameterAttributeDataHandler.Delete(appUIElementParameterAttributeDTO);
                appUIElementParameterAttributeDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Validates the AppUIElementParameterAttributeDTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(appUIElementParameterAttributeDTO.DisplayText1))
            {
                validationErrorList.Add(new ValidationError("AppUIElementParameterAttributeDTO", "DisplayText1", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Text1 "))));
            }
            if (string.IsNullOrWhiteSpace(appUIElementParameterAttributeDTO.DisplayText2))
            {
                validationErrorList.Add(new ValidationError("AppUIElementParameterAttributeDTO", "DisplayText2", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Display Text1 "))));
            }
            if (!string.IsNullOrWhiteSpace(appUIElementParameterAttributeDTO.DisplayText1) && appUIElementParameterAttributeDTO.DisplayText1.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppUIElementParameterAttributeDTO", "DisplayText1", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Display Text 1 "), 50)));
            }
            if (!string.IsNullOrWhiteSpace(appUIElementParameterAttributeDTO.DisplayText2) && appUIElementParameterAttributeDTO.DisplayText2.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppUIElementParameterAttributeDTO", "DisplayText1", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Display Text 2 "), 50)));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the AppUIElementParameterAttributeDTO 
        /// </summary>
        public AppUIElementParameterAttributeDTO AppUIElementParameterAttributeDTO
        {
            get
            {
                return appUIElementParameterAttributeDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of AppUIElementParameterAttribute DTO
    /// </summary>
    public class AppUIElementParameterAttributeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<AppUIElementParameterAttributeDTO> appUIElementParameterAttributeDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AppUIElementParameterAttributeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="appUIElementParameterAttributeDTOList"></param>
        public AppUIElementParameterAttributeListBL(ExecutionContext executionContext,
                                                List<AppUIElementParameterAttributeDTO> appUIElementParameterAttributeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appUIElementParameterAttributeDTOList);
            this.appUIElementParameterAttributeDTOList = appUIElementParameterAttributeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AppUappUIElementParameterAttribute List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>appUIElementParameterAttributeDTOList</returns>
        public List<AppUIElementParameterAttributeDTO> GetAllAppUIElementParameterAttributeDTOList(List<KeyValuePair<AppUIElementParameterAttributeDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppUIElementParameterAttributeDataHandler appUIElementParameterAttributeDataHandler = new AppUIElementParameterAttributeDataHandler(sqlTransaction);
            List<AppUIElementParameterAttributeDTO> appUIElementParameterAttributeDTOList = appUIElementParameterAttributeDataHandler.GetAllAppUIElementParameterAttribute(searchParameters);
            log.LogMethodExit(appUIElementParameterAttributeDTOList);
            return appUIElementParameterAttributeDTOList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appUIElementParameterAttributeDTOList == null ||
               appUIElementParameterAttributeDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < appUIElementParameterAttributeDTOList.Count; i++)
            {
                var appUIElementParameterAttributeDTO = appUIElementParameterAttributeDTOList[i];
                if (appUIElementParameterAttributeDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AppUIElementParameterAttributeBL appUIElementParameterAttributeBL = new AppUIElementParameterAttributeBL(executionContext, appUIElementParameterAttributeDTO);
                    appUIElementParameterAttributeBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving AppUIElementParameterAttributeDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppUIElementParameterAttributeDTO", appUIElementParameterAttributeDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the appUIElementParameterAttributeDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        internal void Delete()
        {
            log.LogMethodEntry();
            if (appUIElementParameterAttributeDTOList != null && appUIElementParameterAttributeDTOList.Any())
            {
                foreach (AppUIElementParameterAttributeDTO appUIElementParameterAttributeDTO in appUIElementParameterAttributeDTOList)
                {
                    if (appUIElementParameterAttributeDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                AppUIElementParameterAttributeBL appUIElementParameterAttributeBL = new AppUIElementParameterAttributeBL(executionContext, appUIElementParameterAttributeDTO);
                                appUIElementParameterAttributeBL.Delete(appUIElementParameterAttributeDTO, parafaitDBTrx.SQLTrx);
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
