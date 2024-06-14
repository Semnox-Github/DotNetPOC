/********************************************************************************************
 * Project Name - ParafaitDefaults BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Mar-2017      Lakshminarayana     Created
 *2.60        30-Apr-2019      Mushahid Faizan     Added SaveUpdateParafaitDefaultList(),Save() method and ParafaitDefaultsListBL constructor.
 *2.70        4-Jul-2019       Girish Kundar       Modified : Passing sqlTransaction object to DataHandler
 *                                                            Cleanup : Adding private modifiers ,logs and Spell check
 *2.120       09-Mar-2020      Girish Kundar       Modified : Added event log update when parafait default value changes
 *2.140       11-Mar-2021      Abhishek            WMS Fix :  Modified SaveUpdateParafaitDefaultList() to save defaults
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Business logic for ParafaitDefaults class.
    /// </summary>
    public class ParafaitDefaultsBL
    {
        private ParafaitDefaultsDTO parafaitDefaultsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ParafaitDefaultsBL class
        /// </summary>
        /// <param name="executionContext"executionContext></param>
        public ParafaitDefaultsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            parafaitDefaultsDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the default_value_id as the parameter
        /// Would fetch the ParafaitDefaults object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution executionContext</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ParafaitDefaultsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            parafaitDefaultsDTO = parafaitDefaultsDataHandler.GetParafaitDefaultsDTO(id, sqlTransaction);
            if(parafaitDefaultsDTO == null)
            {
                //string message = MessageContainer.GetMessage(executionContext, 2196, "ParafaitDefaults", id);
                //log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException("Entity not found with id =" + id);
            }
            Build(sqlTransaction);
            log.LogMethodExit();
        }

        private void Build(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext);
            List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.SITE_ID,executionContext.GetSiteId().ToString() ));
            searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.OPTION_ID, parafaitDefaultsDTO.DefaultValueId.ToString()));
            searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE, "Y"));
            parafaitDefaultsDTO.ParafaitOptionValuesDTOList = parafaitOptionValuesListBL.GetParafaitOptionValuesDTOList(searchParameters,sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the default_value as the parameter
        /// Would fetch the ParafaitDefaults object from the database based on the defaultValueName passed. 
        /// </summary>
        /// <param name="defaultValueName">defaultValueName</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ParafaitDefaultsBL(ExecutionContext executionContext, string defaultValueName, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(defaultValueName, sqlTransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, defaultValueName));
            searchParameter.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchParameter.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsDataHandler.GetParafaitDefaultsDTOList(searchParameter);
            if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count > 0)
            {
                parafaitDefaultsDTO = parafaitDefaultsDTOList[0];
                Build(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ParafaitDefaultsBL object using the ParafaitDefaultsDTO
        /// </summary>
        /// <param name="executionContext">execution executionContext</param>
        /// <param name="parafaitDefaultsDTO">ParafaitDefaultsDTO object</param>
        public ParafaitDefaultsBL(ExecutionContext executionContext, ParafaitDefaultsDTO parafaitDefaultsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parafaitDefaultsDTO);
            this.parafaitDefaultsDTO = parafaitDefaultsDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Parafait Defaults
        /// Checks if the  DefaultValueId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            if (parafaitDefaultsDTO.DefaultValueId < 0)
            {
                int id = parafaitDefaultsDataHandler.InsertDefaultValues(parafaitDefaultsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parafaitDefaultsDTO.DefaultValueId = id;
                parafaitDefaultsDTO.AcceptChanges();
            }
            else
            {
                if (parafaitDefaultsDTO.IsChanged)
                {
                    UpdateEventLog(sqlTransaction);
                    parafaitDefaultsDataHandler.UpdateDefaultValues(parafaitDefaultsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    parafaitDefaultsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void UpdateEventLog(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            ParafaitDefaultsDTO existingParafaitDefaultsDTO = parafaitDefaultsDataHandler.GetParafaitDefaultsDTO(parafaitDefaultsDTO.DefaultValueId, sqlTransaction);
            if (existingParafaitDefaultsDTO == null)
            {
                throw new EntityNotFoundException("Entity not found with id =" + parafaitDefaultsDTO.DefaultValueId);
            }
            EventLog eventLog = new EventLog(new Utilities());
            eventLog.logEvent("SiteLevelConfig", 'D', parafaitDefaultsDTO.DefaultValueName + ": " + parafaitDefaultsDTO.DefaultValue + " [" + existingParafaitDefaultsDTO.DefaultValue + "]",
                                                      parafaitDefaultsDTO.DefaultValueName.ToString() + " changed to " + parafaitDefaultsDTO.DefaultValue + " from " + existingParafaitDefaultsDTO.DefaultValue.ToString(),
                                                      "CONFIGURATION",3,"","", sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ParafaitDefaultsDTO ParafaitDefaultsDTO
        {
            get
            {
                return parafaitDefaultsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Parafait defaults
    /// </summary>
    public class ParafaitDefaultsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ParafaitDefaultsDTO> parafaitDefaultDTOList;


        /// <summary>
        /// </summary>
        /// <param name="executionContext">application executionContext</param>
        public ParafaitDefaultsListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">application executionContext</param>
        public ParafaitDefaultsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parafaitOptionValuesDTOList"></param>
        public ParafaitDefaultsListBL(ExecutionContext executionContext, List<ParafaitDefaultsDTO> parafaitDefaultDTOList)
        {
            log.LogMethodEntry(executionContext, parafaitDefaultDTOList);
            this.executionContext = executionContext;
            this.parafaitDefaultDTOList = parafaitDefaultDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParafaitDefaults list
        /// </summary>
        public List<ParafaitDefaultsDTO> GetParafaitDefaultsDTOList(List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsDataHandler.GetParafaitDefaultsDTOList(searchParameters);
            log.LogMethodExit(parafaitDefaultsDTOList);
            return parafaitDefaultsDTOList;
        }

        /// <summary>
        /// Returns the ParafaitDefaults list
        /// </summary>
        public List<ParafaitDefaultsDTO> GetParafaitDefaults(List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters,bool loadChildRecords = false ,bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsDataHandler.GetParafaitDefaultsDTOList(searchParameters);
            if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Any() && loadChildRecords)
            {
                Build(parafaitDefaultsDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(parafaitDefaultsDTOList);
            return parafaitDefaultsDTOList;
        }

        private void Build(List<ParafaitDefaultsDTO> parafaitDefaultsDTOList,bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            Dictionary<int, ParafaitDefaultsDTO> parafaitDefaultsDTODictionary = new Dictionary<int, ParafaitDefaultsDTO>();
            List<int> defaultIdList = new List<int>();
            for (int i = 0; i < parafaitDefaultsDTOList.Count; i++)
            {
                if (parafaitDefaultsDTODictionary.ContainsKey(parafaitDefaultsDTOList[i].DefaultValueId))
                {
                    continue;
                }
                parafaitDefaultsDTODictionary.Add(parafaitDefaultsDTOList[i].DefaultValueId, parafaitDefaultsDTOList[i]);
                defaultIdList.Add(parafaitDefaultsDTOList[i].DefaultValueId);
            }
            ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext);
            List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList = parafaitOptionValuesListBL.GetParafaitOptionValuesDTOList(defaultIdList, activeChildRecords, sqlTransaction);

            if (parafaitOptionValuesDTOList != null && parafaitOptionValuesDTOList.Any())
            {
                for (int i = 0; i < parafaitOptionValuesDTOList.Count; i++)
                {
                    if (parafaitDefaultsDTODictionary.ContainsKey(parafaitOptionValuesDTOList[i].OptionId) == false)
                    {
                        continue;
                    }
                    ParafaitDefaultsDTO parafaitDefaultsDTO = parafaitDefaultsDTODictionary[parafaitOptionValuesDTOList[i].OptionId];
                    if (parafaitDefaultsDTO.ParafaitOptionValuesDTOList == null)
                    {
                        parafaitDefaultsDTO.ParafaitOptionValuesDTOList = new List<ParafaitOptionValuesDTO>();
                    }
                    parafaitDefaultsDTO.ParafaitOptionValuesDTOList.Add(parafaitOptionValuesDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Parafait defaults dictionary with default value name as key and default value as value.
        /// user and pos machine level overrides are considered while computing the value.
        /// when the value is not specified empty string is used as default value.
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, string> GetOverridenParafaitDefaultDictionary(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            ConcurrentDictionary<string, string> parafaitDefaultsDictionary = parafaitDefaultsDataHandler.GetOverridenParafaitDefaultDictonary(executionContext.GetUserPKId(), executionContext.GetMachineId(), executionContext.GetSiteId());
            GetOverriddenTransactiondefaults(parafaitDefaultsDictionary, executionContext.GetUserPKId(), executionContext.GetMachineId(), executionContext.GetSiteId());
            log.LogMethodExit(parafaitDefaultsDictionary);
            return parafaitDefaultsDictionary;
        }

        /// <summary>
        /// Returns the parafait defaults dictionary with default value name as key and default value as value.
        /// user and pos machine level overrides are considered while computing the value.
        /// when the value is not specified empty string is used as default value.
        /// </summary>
        /// <returns></returns>
        internal ConcurrentDictionary<string, string> GetSiteLevelParafaitDefaultDictionary(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            ConcurrentDictionary<string, string> parafaitDefaultsDictionary = parafaitDefaultsDataHandler.GetSiteLevelParafaitDefaultDictonary(executionContext.GetSiteId());
            GetSiteTransactiondefaults(parafaitDefaultsDictionary, executionContext.GetSiteId());
            log.LogMethodExit(parafaitDefaultsDictionary);
            return parafaitDefaultsDictionary;
        }

        /// <summary>
        /// Returns dictionary object after populating transaction specific environment properties
        /// </summary>
        /// <param name="parafaitDefaultsDictionary">ConcurrentDictionary object</param>
        /// <param name="machineId">Machine id executionContext</param>
        /// <param name="siteId">Site id executionContext</param>
        /// <param name="userId">User PK id executionContext</param>
        internal void GetOverriddenTransactiondefaults(ConcurrentDictionary<string, string> parafaitDefaultsDictionary, int userId, int machineId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parafaitDefaultsDictionary, userId, machineId, siteId, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            parafaitDefaultsDataHandler.GetOverridenTransactionDefaultDictonary(parafaitDefaultsDictionary, userId, machineId, siteId);
            log.LogMethodExit(parafaitDefaultsDictionary);
        }

        /// <summary>
        /// Returns dictionary object after populating transaction specific environment properties
        /// </summary>
        /// <param name="parafaitDefaultsDictionary">ConcurrentDictionary object</param>
        /// <param name="siteId">Site id executionContext</param>
        internal void GetSiteTransactiondefaults(ConcurrentDictionary<string, string> parafaitDefaultsDictionary, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parafaitDefaultsDictionary, siteId, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            parafaitDefaultsDataHandler.GetSiteTransactionDefaultDictonary(parafaitDefaultsDictionary, siteId);
            log.LogMethodExit(parafaitDefaultsDictionary);
        }

        /// <summary>
        /// This method should be used to Save and Update the Parafait Default List details for Web Management Studio. 
        /// </summary>
        public void SaveUpdateParafaitDefaultList()
        {
            log.LogMethodEntry();
            if (parafaitDefaultDTOList != null)
            {
                foreach (ParafaitDefaultsDTO parafaitDefaultDTO in parafaitDefaultDTOList)
                {
                    try
                    {
                        ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, parafaitDefaultDTO);
                        parafaitDefaultsBL.Save();
                    }
                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw new Exception(ex.Message, ex);
                    }
                }
                log.LogMethodExit();
            }
        }

        public DateTime? GetParafaitDefaultModuleLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            DateTime? result = parafaitDefaultsDataHandler.GetParafaitDefaultModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
        public int GetEncryptedPasswordDataTypeId(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            ParafaitDefaultsDataHandler parafaitDefaultsDataHandler = new ParafaitDefaultsDataHandler(sqlTransaction);
            int  dataTypeId = parafaitDefaultsDataHandler.GetEncryptedPasswordDataTypeId(siteId);
            log.LogMethodExit(dataTypeId);
            return dataTypeId;
        }
    }
}
