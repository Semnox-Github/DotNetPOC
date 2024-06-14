/********************************************************************************************
 * Project Name - LookupValues
 * Description  - Bussiness logic of lookup values
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        27-Jan-2016   Raghuveera          Created
 *2.70        09-Apr-2019   Mushahid Faizan     Added DeleteLookupValues() method
              05-Aug-2019   Mushahid Faizan     Added SqlTransaction and Delete in Save() method and 
 *2.120.0     09-Oct-2020   Guru S A            Membership engine sql session issue
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Lookup values allows to access the lookup values based on the business logic.
    /// </summary>
    public class LookupValues
    {
        private LookupValuesDTO lookupValuesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;
        private LookupValuesDataHandler lookupValuesDataHandler;


        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public LookupValues(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.lookupValuesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the lookupvalueid parameter
        /// </summary>
        /// <param name="lookupValuesDTO">lookup values DTO Object</param>
        public LookupValues(ExecutionContext executionContext, int lookupValueId, SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(lookupValueId);
            LookupValuesDataHandler lookupValueIdDataHandler = new LookupValuesDataHandler(sqlTransaction);
            lookupValuesDTO = lookupValueIdDataHandler.GetLookupValues(lookupValueId);
            if (lookupValuesDTO == null)
            {
                string message = " Record Not found with id" + lookupValueId;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(lookupValuesDTO);
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext Object</param>
        /// <param name="lookupValuesDTO">lookup values DTO Object</param>
        public LookupValues(ExecutionContext executionContext, LookupValuesDTO lookupValuesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, lookupValuesDTO);
            this.lookupValuesDTO = lookupValuesDTO;
            log.LogMethodExit();
        }

        // <summary>
        /// Validate DTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }
        /// <summary>
        /// Saves the LookupValues
        /// Checks if the Lookup value id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (lookupValuesDTO.IsChanged == false
               && lookupValuesDTO.LookupValueId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            LookupValuesDataHandler lookupValueIdDataHandler = new LookupValuesDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = "Validation Error";
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (lookupValuesDTO.IsActive)
            {
                if (lookupValuesDTO.LookupValueId <= 0)
                {
                    lookupValuesDTO = lookupValueIdDataHandler.InsertLookupValue(lookupValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    lookupValuesDTO.AcceptChanges();
                }
                else
                {
                    if (lookupValuesDTO.IsChanged)
                    {
                        lookupValuesDTO = lookupValueIdDataHandler.UpdateLookupValues(lookupValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        lookupValuesDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if(lookupValuesDTO.LookupValueId >= 0)
                {
                    lookupValueIdDataHandler.DeleteLookupValues(lookupValuesDTO.LookupValueId);
                }
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Delete the LookupValues based on lookupId
        ///// </summary>
        ///// <param name="lookupValueId"></param>        
        //internal void DeleteLookupValues(int lookupValueId,SqlTransaction sqlTransaction)
        //{
        //    log.LogMethodEntry(lookupValueId);
        //    try
        //    {
        //        if (lookupValuesDTO.IsChanged == true)
        //        {
        //            this.lookupValuesDataHandler = new LookupValuesDataHandler(sqlTransaction);
        //            lookupValuesDataHandler.DeleteLookupValues(lookupValueId);
        //        }
        //        log.LogMethodExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}

        public LookupValuesDTO LookupValuesDTO
        {
            get
            {
                return lookupValuesDTO;
            }
        }
    }
    /// <summary>
    /// Manages the list of LookupValues
    /// </summary>
    public class LookupValuesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<LookupValuesDTO> lookupValuesDTOsList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LookupValuesList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having execution Context
        /// </summary>
        /// <param name="executioncontext"></param>
        public LookupValuesList(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executioncontext);
            this.executionContext = executioncontext;
            this.lookupValuesDTOsList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="lookupValuesDTOsList"></param>
        public LookupValuesList(ExecutionContext executionContext, List<LookupValuesDTO> lookupValuesDTOsList)
            : this(executionContext)
        {
            log.LogMethodEntry(lookupValuesDTOsList, executionContext);
            this.lookupValuesDTOsList = lookupValuesDTOsList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the lookup values list
        /// </summary>
        public List<LookupValuesDTO> GetAllLookupValues(List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                LookupValuesDataHandler lookupValuesDataHandler = new LookupValuesDataHandler(sqlTransaction);
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesDataHandler.GetLookupValuesList(searchParameters);
                log.LogMethodExit(lookupValuesDTOList);
                return lookupValuesDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Searching  LookupValuesDTO", ex);
                log.LogMethodExit(null, "Throwing exception in GetAllLookupValues(SearchByLookupValuesParameters) - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the lookup values dictionary
        /// </summary>
        /// <param name="lookupName">Lookup Name</param>
        /// <returns></returns>
        public Dictionary<int, LookupValuesDTO> GetLookupValuesMap(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            Dictionary<int, LookupValuesDTO> lookupValuesDTOMap = null;
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            try
            {
                if (string.IsNullOrEmpty(lookupName) == false)
                {
                    searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
                    if (executionContext != null)
                        searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                }
                List<LookupValuesDTO> lookupValuesDTOList = GetAllLookupValues(searchParameters);
                if (lookupValuesDTOList != null)
                {
                    lookupValuesDTOMap = new Dictionary<int, LookupValuesDTO>();
                    foreach (var item in lookupValuesDTOList)
                    {
                        lookupValuesDTOMap.Add(item.LookupValueId, item);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred At GetLookupValuesMap(string lookupName)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(lookupValuesDTOMap);
            return lookupValuesDTOMap;
        }

        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns> Returns List KeyValuePair LookupValuesDTO.SearchByMachineParameters, string by converting LookupParams</returns>
        public List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> BuildLookupsSearchParametersList(LookupParams lookupSearchParams)
        {
            log.LogMethodEntry(lookupSearchParams);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            if (lookupSearchParams == null)
            {
                lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, ""));
            }
            else
            {
                if (lookupSearchParams.LookupId != -1)
                    lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID, Convert.ToString(lookupSearchParams.LookupId)));
                if (lookupSearchParams.LookupValueId != -1)
                    lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, Convert.ToString(lookupSearchParams.LookupValueId)));
                if (lookupSearchParams.LookupName != null)
                    lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupSearchParams.LookupName));
                if (lookupSearchParams.LookupValue != null)
                    lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, lookupSearchParams.LookupValue));
                if (lookupSearchParams.Description != null)
                    lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.DESCRIPTION, lookupSearchParams.Description));
                if (lookupSearchParams.SiteId != -1)
                    lSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, lookupSearchParams.SiteId.ToString()));

            }
            log.LogMethodExit(lSearchParams);
            return lSearchParams;
        }

        /// <summary>
        ///GetAllInventoryLookupValues 
        /// </summary>
        /// <returns></returns>

        public List<LookupValuesDTO> GetAllInventoryLookupValues(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                LookupValuesDataHandler lookupValuesDataHandler = new LookupValuesDataHandler(sqlTransaction);
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesDataHandler.GetInventoryLookupValues();
                log.LogMethodExit(lookupValuesDTOList);
                return lookupValuesDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred At GetAllInventoryLookupValues()", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the lookup values list
        /// </summary>
        public List<LookupValuesDTO> GetInventoryLookupValuesByValueName(string lookupName, int siteId, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(lookupName, siteId);
                LookupValuesDataHandler lookupValuesDataHandler = new LookupValuesDataHandler(sqlTransaction);
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesDataHandler.GetInventoryLookupValuesByValueName(lookupName, siteId);
                log.LogMethodExit(lookupValuesDTOList);
                return lookupValuesDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred At GetInventoryLookupValuesByValueName(string lookupName, int siteId)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the server datetime
        /// </summary>
        public DateTime GetServerDateTime()
        {
            log.LogMethodEntry();
            DateTime result = ServerDateTime.Now;
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Saves the lookupValuesDTOsList
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (lookupValuesDTOsList == null ||
                lookupValuesDTOsList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < lookupValuesDTOsList.Count; i++)
            {
                var lookupValuesDTO = lookupValuesDTOsList[i];
                if (lookupValuesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    LookupValues lookupValues = new LookupValues(executionContext, lookupValuesDTO);
                    lookupValues.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving lookupValuesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("lookupValuesDTO", lookupValuesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the server datetime
        /// </summary>
        public string GetServerTimeZone()
        {
            log.LogMethodEntry();
            string result = ServerDateTime.TimeZone;
            log.LogMethodExit(result);
            return result;
        }
    }
}
