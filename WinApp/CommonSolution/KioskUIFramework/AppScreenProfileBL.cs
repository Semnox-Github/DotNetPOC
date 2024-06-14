
/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Business logic file for  AppScreenProfile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        21-May-2019   Girish Kundar           Created 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// Business logic for AppScreenProfile class
    /// </summary>
    public class AppScreenProfileBL
    {
        private AppScreenProfileDTO appScreenProfileDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AppScreenProfileBL class
        /// </summary>
        private AppScreenProfileBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates AppScreenProfileBL object using the appScreenProfileDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="appScreenProfileDTO">appScreenProfileDTO object</param>
        public AppScreenProfileBL(ExecutionContext executionContext, AppScreenProfileDTO appScreenProfileDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appScreenProfileDTO);
            this.appScreenProfileDTO = appScreenProfileDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the AppScreenProfile id as the parameter
        /// Would fetch the AppScreenProfile object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public AppScreenProfileBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AppScreenProfileDataHandler appScreenProfileDataHandler = new AppScreenProfileDataHandler(sqlTransaction);
            appScreenProfileDTO = appScreenProfileDataHandler.GetAppScreenProfile(id);
            if (appScreenProfileDTO == null)
            {
                //1567 "Unable to find a AppUIPanel with id " + id
                // Unable to find a &0 with id &1
                // script available
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AppScreenProfile", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appScreenProfileDTO.AppScreenProfileId > -1 && 
                 appScreenProfileDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            AppScreenProfileDataHandler appScreenProfileDataHandler = new AppScreenProfileDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, 14773);
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (appScreenProfileDTO.AppScreenProfileId < 0)
            {
                log.LogVariableState("AppScreenProfileDTO", appScreenProfileDTO);
                appScreenProfileDTO = appScreenProfileDataHandler.Insert(appScreenProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appScreenProfileDTO.AcceptChanges();
            }
            else if (appScreenProfileDTO.IsChanged)
            {
                appScreenProfileDTO = appScreenProfileDataHandler.Update(appScreenProfileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                appScreenProfileDTO.AcceptChanges();
            }
            //if (appScreenProfileDTO.AppScreenProfileId >= 0)
            // {
            //   appScreenProfileDataHandler.Delete(appScreenProfileDTO);
            // }


            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the AppScreenProfileDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(appScreenProfileDTO.AppScreenProfileName))
            {
                validationErrorList.Add(new ValidationError("AppScreenProfile", "AppScreenProfileName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "AppScreen Profile Name"))));
            }

            if (!string.IsNullOrWhiteSpace(appScreenProfileDTO.AppScreenProfileName) && appScreenProfileDTO.AppScreenProfileName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("AppScreenProfile", "AppScreenProfileName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "AppScreen Profile Name"), 50)));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AppScreenProfileDTO AppScreenProfileDTO
        {
            get
            {
                return appScreenProfileDTO;
            }
        }
    
    }

    /// <summary>
    /// Manages the list of AppScreenProfile
    /// </summary>
    public class AppScreenProfileListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<AppScreenProfileDTO> appScreenProfileDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AppScreenProfileListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="appScreenProfileDTOList"></param>
        public AppScreenProfileListBL(ExecutionContext executionContext,
                                               List<AppScreenProfileDTO> appScreenProfileDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, appScreenProfileDTOList);
            this.appScreenProfileDTOList = appScreenProfileDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the AppScreenProfileDTO List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<AppScreenProfileDTO> GetAllAppScreenProfileDTOList(List<KeyValuePair<AppScreenProfileDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AppScreenProfileDataHandler appScreenProfileDataHandler = new AppScreenProfileDataHandler(sqlTransaction);
            List<AppScreenProfileDTO> appScreenProfileDTOList = appScreenProfileDataHandler.GetAllappScreenProfile(searchParameters);
            log.LogMethodExit(appScreenProfileDTOList);
            return appScreenProfileDTOList;
        }

        /// <summary>
        /// Saves the AppScreenProfileDTOList
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (appScreenProfileDTOList == null ||
               appScreenProfileDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < appScreenProfileDTOList.Count; i++)
            {
                var appScreenProfileDTO = appScreenProfileDTOList[i];
                if (appScreenProfileDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    AppScreenProfileBL appScreenProfileBL = new AppScreenProfileBL(executionContext, appScreenProfileDTO);
                    appScreenProfileBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving AppScreenProfileDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("AppScreenProfileDTO", appScreenProfileDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
