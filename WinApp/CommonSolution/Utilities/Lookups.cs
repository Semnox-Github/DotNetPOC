/********************************************************************************************
 * Project Name - Lookups
 * Description  - Business logic of lookups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        13-Mar-2016   Jagan Mohana        Created 
 *2.60        08-Apr-2019   Akshay Gulaganji    modified GetAllLookups()
              09-Apr-2019   Mushahid Faizan     modified SaveUpdateLookupsList() try-catch block.
                                                Added DeleteLookupsList() & DeleteLookups() method.
 *2.70        03-Jul-2019   Girish Kundar       Modified: Changed the Save() method. Insert/Update Returns DTO instead of id 
 *2.90        11-Aug-2020   Girish Kundar       Modified : Added GetLookupModuleLastUpdateTime method to get last update date / delete exception message
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Core.Utilities
{
    public class Lookups
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LookupsDTO lookupsDTO;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;
        private LookupsDataHandler lookupsDataHandler;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private Lookups(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="lookupsDTO">LookupsDTO</param>
        public Lookups(ExecutionContext executionContext, LookupsDTO lookupsDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, lookupsDTO);
            this.lookupsDTO = lookupsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Lookups id as the parameter
        /// Would fetch the Lookups object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of Lookups passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public Lookups(ExecutionContext executionContext, int id, bool activeRecords = true, bool buildChildRecords = false, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, buildChildRecords, sqlTransaction);
            LookupsDataHandler lookupsDataHandler = new LookupsDataHandler(sqlTransaction);
            lookupsDTO = lookupsDataHandler.GetLookupsDTO(id);
            if (lookupsDTO == null)
            {
                string message = " Record Not found with id" + id;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (buildChildRecords)
            {
                Build(activeRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeRecords, sqlTransaction);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchByLookUpValueParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchByLookUpValueParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID, lookupsDTO.LookupId.ToString()));
            if (activeRecords)
            {
                searchByLookUpValueParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.IS_ACTIVE, "1"));
            }
            lookupsDTO.LookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchByLookUpValueParams, sqlTransaction);
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
        /// Saves the Lookups
        /// Checks if the lookup id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LookupsDataHandler lookupsDataHandler = new LookupsDataHandler(sqlTransaction);
            if (lookupsDTO.IsChangedRecursive == false
                && lookupsDTO.LookupId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = "Validation Error";
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
          
                if (lookupsDTO.LookupId < 0)
                {
                    lookupsDTO = lookupsDataHandler.InsertLookups(lookupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    lookupsDTO.AcceptChanges();
                }
                else
                {
                    if (lookupsDTO.IsChanged)
                    {
                        lookupsDTO = lookupsDataHandler.UpdateLookups(lookupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        lookupsDTO.AcceptChanges();
                    }
                }
            SaveLookUpValues(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveLookUpValues(SqlTransaction sqlTransaction)
        {

            // for Child Records : :LookupValuesDTOList
            if (lookupsDTO.LookupValuesDTOList != null && lookupsDTO.LookupValuesDTOList.Any())
            {
                List<LookupValuesDTO> updatedLookupValuesDTOList = new List<LookupValuesDTO>();
                foreach (LookupValuesDTO lookupValuesDTOList in lookupsDTO.LookupValuesDTOList)
                {
                    if (lookupValuesDTOList.LookupId != lookupsDTO.LookupId)
                    {
                        lookupValuesDTOList.LookupId = lookupsDTO.LookupId;
                    }
                    if (lookupValuesDTOList.IsChanged)
                    {
                        updatedLookupValuesDTOList.Add(lookupValuesDTOList);
                    }
                }
                if (updatedLookupValuesDTOList.Any())
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext, updatedLookupValuesDTOList);
                    lookupValuesList.Save(sqlTransaction);
                }
            }
        }

        

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LookupsDTO LookupsDTO
        {
            get
            {
                return lookupsDTO;
            }
        }
        /// <summary>
        /// Delete the Lookup and LookupValues based on lookupId
        /// </summary>
        /// <param name="lookupId"></param>        
        public void DeleteLookups(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                LookupsDataHandler lookupsDataHandler = new LookupsDataHandler(sqlTransaction);
                if ((lookupsDTO.LookupValuesDTOList != null && lookupsDTO.LookupValuesDTOList.Any((x => x.IsActive == true))))
                {
                    string message = "Cannot Inactivate records for which matching detail data exists.";// MessageUtils(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("lookupsDTO", lookupsDTO);
                // Call Delete for the Child DTO.
                SaveLookUpValues(sqlTransaction);

                if (lookupsDTO.LookupId >= 0 && lookupsDTO.IsActive == false)
                {
                    lookupsDataHandler.DeleteLookups(lookupsDTO.LookupId);
                }
                lookupsDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
    /// <summary>
    /// Manages the list of Lookups List
    /// </summary>
    public class LookupsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<LookupsDTO> lookupsDTOList = new List<LookupsDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LookupsList()
        {
            log.LogMethodEntry();
            this.lookupsDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LookupsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.lookupsDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="lookupsDTOList"></param>
        public LookupsList(ExecutionContext executionContext, List<LookupsDTO> lookupsDTOList)
        {
            log.LogMethodEntry(executionContext, lookupsDTOList);
            this.lookupsDTOList = lookupsDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the lookups and lookupvalues list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChild">loadChild</param>
        /// <returns>lookupsDTOList</returns>
        public List<LookupsDTO> GetAllLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters, bool loadChild = false,
                                              bool loadActiveChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChild, loadActiveChildRecords, sqlTransaction);
            LookupsDataHandler lookupsDataHandler = new LookupsDataHandler(sqlTransaction);
            lookupsDTOList = lookupsDataHandler.GetLookups(searchParameters);
            if (loadChild && lookupsDTOList != null && lookupsDTOList.Any())
            {
                Build(lookupsDTOList, loadActiveChildRecords, sqlTransaction);
            }
            log.LogMethodExit(lookupsDTOList);
            return lookupsDTOList;
        }

        private void Build(List<LookupsDTO> lookupsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lookupsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, LookupsDTO> lookupIdIdLookUpDTODictionary = new Dictionary<int, LookupsDTO>();
            string lookupIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < lookupsDTOList.Count; i++)
            {
                if (lookupsDTOList[i].LookupId == -1 ||
                    lookupIdIdLookUpDTODictionary.ContainsKey(lookupsDTOList[i].LookupId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(lookupsDTOList[i].LookupId);
                lookupIdIdLookUpDTODictionary.Add(lookupsDTOList[i].LookupId, lookupsDTOList[i]);
            }
            lookupIdSet = sb.ToString();

            // loads child records - Look up values
            LookupValuesList lookupValuesListBL = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchByPosPrinterParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchByPosPrinterParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_ID_LIST, lookupIdSet.ToString()));
            //searchByPosPrinterParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchByPosPrinterParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.IS_ACTIVE, "1"));
            }
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesListBL.GetAllLookupValues(searchByPosPrinterParams, null);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                log.LogVariableState("lookupValuesDTOList", lookupValuesDTOList);
                foreach (var lookupValuesDTO in lookupValuesDTOList)
                {
                    if (lookupIdIdLookUpDTODictionary.ContainsKey(lookupValuesDTO.LookupId))
                    {
                        if (lookupIdIdLookUpDTODictionary[lookupValuesDTO.LookupId].LookupValuesDTOList == null)
                        {
                            lookupIdIdLookUpDTODictionary[lookupValuesDTO.LookupId].LookupValuesDTOList = new List<LookupValuesDTO>();
                        }
                        lookupIdIdLookUpDTODictionary[lookupValuesDTO.LookupId].LookupValuesDTOList.Add(lookupValuesDTO);
                    }
                }
            }
        }

        /// <summary>
        ///This method should be used to Save and Update the Lookups details for Web Management Studio.
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (lookupsDTOList != null && lookupsDTOList.Any())
            {
                Utilities utilities = new Utilities(); 
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LookupsDTO lookupsDto in lookupsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Lookups lookups = new Lookups(executionContext, lookupsDto);
                            lookups.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(utilities.MessageUtils.getMessage(1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// Delete the Lookups List 
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (lookupsDTOList != null && lookupsDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (LookupsDTO lookupsDto in lookupsDTOList)
                    {
                        if (lookupsDto.IsChangedRecursive)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                Lookups lookups = new Lookups(executionContext, lookupsDto);
                                lookups.DeleteLookups(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw new Exception(ex.Message, ex);
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
        }
        internal DateTime? GetLookupModuleLastUpdateTime(int siteId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            LookupsDataHandler lookupsDataHandler = new LookupsDataHandler(sqlTransaction);
            DateTime? result = lookupsDataHandler.GetLookupModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
