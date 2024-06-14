/********************************************************************************************
 * Project Name - DataAccessRule BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
  *2.70       09-Apr-2019     Mushahid Faizan  Added SQL Transaction in SaveUpdateDataAccessRuleList() method &
 *                                            Added LogMethodEntry/Exit,removed unused namespaces.
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.70.2        26-Mar-2020      Girish Kundar       Modified : Added build method to build exclusion list 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Data Access Rule 
    /// </summary>
    public class DataAccessRule
    {
        private DataAccessRuleDTO dataAccessRuleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private DataAccessRule(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="dataAccessRuleId"> id of the data access rule</param>
        public DataAccessRule(ExecutionContext executionContext,int dataAccessRuleId, SqlTransaction sqlTransaction = null, bool loadExclusionList = false, bool loadActiveRecords = false)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext,dataAccessRuleId, sqlTransaction);
            //this.executionContext = ExecutionContext.GetExecutionContext();
            DataAccessRuleDataHandler dataAccessRuleDataHandler = new DataAccessRuleDataHandler(sqlTransaction);
            dataAccessRuleDTO = dataAccessRuleDataHandler.GetDataAccessRule(dataAccessRuleId);
            if (loadExclusionList)
            {
                Build(dataAccessRuleDTO, loadActiveRecords ,sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(DataAccessRuleDTO dataAccessRuleDTO, bool loadActiveRecords , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(dataAccessRuleDTO, loadActiveRecords , sqlTransaction);
            EntityExclusionDetailList entityExclusionDetailList = new EntityExclusionDetailList();
            List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> searchEntityExclusionDetailParameters = new List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>>();
            searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            if (loadActiveRecords)
            {
                searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG, "1"));
            }
            List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = entityExclusionDetailList.GetAllEntityExclusionDetail(searchEntityExclusionDetailParameters, sqlTransaction);
            if (dataAccessRuleDTO.DataAccessDetailDTOList != null && dataAccessRuleDTO.DataAccessDetailDTOList.Any())
            {
                foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                {
                    if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Any())
                    {
                        dataAccessDetailDTO.EntityExclusionDetailDTOList = entityExclusionDetailDTOList.FindAll(m => m.RuleDetailId == dataAccessDetailDTO.RuleDetailId).ToList();
                    }
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="dataAccessRuleDTO">Parameter of the type DataAccessRuleDTO</param>
        /// <param name="executionContext">Parameter of the type ExecutionContext</param>
        public DataAccessRule(ExecutionContext executionContext, DataAccessRuleDTO dataAccessRuleDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, dataAccessRuleDTO);
            this.dataAccessRuleDTO = dataAccessRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the data access rules
        /// data access rules will be inserted if DataAccessRuleId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            DataAccessRuleDataHandler dataAccessRuleDataHandler = new DataAccessRuleDataHandler(sqlTransaction);
            if (dataAccessRuleDTO.DataAccessRuleId < 0)
            {
                dataAccessRuleDTO = dataAccessRuleDataHandler.InsertDataAccessRule(dataAccessRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dataAccessRuleDTO.AcceptChanges();

            }
            else
            {
                if (dataAccessRuleDTO.IsChanged)
                {
                    dataAccessRuleDTO = dataAccessRuleDataHandler.UpdateDataAccessRule(dataAccessRuleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dataAccessRuleDTO.AcceptChanges();
                }
            }
            if (dataAccessRuleDTO.DataAccessDetailDTOList != null && dataAccessRuleDTO.DataAccessDetailDTOList.Count > 0)
            {
                foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                {
                    dataAccessDetailDTO.DataAccessRuleId = dataAccessRuleDTO.DataAccessRuleId;
                    DataAccessDetail dataAccessDetail = new DataAccessDetail(executionContext, dataAccessDetailDTO);
                    dataAccessDetail.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// this returns true or false based on the data access object selected
        /// </summary>
        /// <param name="uiName"> Ui name how we used in the lookups</param>
        /// <param name="guid"> guid of the record to check exclusion</param>
        /// <returns>boolean result</returns>
        public bool IsEditable(string uiName, string guid = "")
        {
            log.LogMethodEntry(uiName, guid);
            bool IsEditable = false;
            if (dataAccessRuleDTO != null && dataAccessRuleDTO.IsActive)
            {
                log.LogVariableState("dataAccessRuleDTO.DataAccessRuleId", dataAccessRuleDTO.DataAccessRuleId);
                log.LogVariableState("dataAccessRuleDTO.DataAccessDetailDTOList", dataAccessRuleDTO.DataAccessDetailDTOList);
                if (dataAccessRuleDTO.DataAccessDetailDTOList != null)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_ACCESS_ENTITY"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, uiName));
                    List<LookupValuesDTO> lookupValuesDataAccessEntityDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                    lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_ACCESS_LIMIT"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Read/Write"));
                    List<LookupValuesDTO> lookupValuesDataAccessLimitDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

                    log.LogVariableState("lookupValuesDataAccessEntityDTOList", lookupValuesDataAccessEntityDTOList);
                    log.LogVariableState("lookupValuesDataAccessLimitDTOList", lookupValuesDataAccessLimitDTOList);
                    log.LogVariableState("dataAccessRuleDTO.DataAccessDetailDTOList", dataAccessRuleDTO.DataAccessDetailDTOList);
                    if (lookupValuesDataAccessEntityDTOList != null && lookupValuesDataAccessEntityDTOList.Count > 0 && lookupValuesDataAccessLimitDTOList != null && lookupValuesDataAccessLimitDTOList.Count > 0)
                    {
                        foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                        {
                            if (dataAccessDetailDTO.EntityId == lookupValuesDataAccessEntityDTOList[0].LookupValueId && dataAccessDetailDTO.IsActive)
                            {
                                if (lookupValuesDataAccessLimitDTOList[0].LookupValueId == dataAccessDetailDTO.AccessLimitId)
                                {
                                    IsEditable = true;
                                    if (!string.IsNullOrEmpty(guid) && dataAccessDetailDTO.EntityExclusionDetailDTOList != null)
                                    {
                                        foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in dataAccessDetailDTO.EntityExclusionDetailDTOList)
                                        {
                                            if (entityExclusionDetailDTO.IsActive && string.IsNullOrEmpty(entityExclusionDetailDTO.FieldName)
                                                && entityExclusionDetailDTO.TableAttributeGuid.ToLower().Equals(guid.ToLower()))
                                            {
                                                IsEditable = false;
                                                log.LogMethodExit(IsEditable);
                                                return IsEditable;
                                            }
                                        }
                                        log.LogMethodExit(IsEditable);
                                        return IsEditable;
                                    }
                                    else
                                    {
                                        log.LogMethodExit(IsEditable);
                                        return IsEditable;
                                    }
                                }

                            }
                        }
                    }

                }
            }
            else
            {
                log.Debug("Rule is not set");
                IsEditable = true;
            }
            log.LogMethodExit(IsEditable);
            return IsEditable;
        }

        /// <summary>
        /// this returns  List<EntityExclusionDetailDTO> 
        /// </summary>
        /// <param name="uiName"> Ui name how we used in the lookups</param> 
        /// <returns> List<EntityExclusionDetailDTO></returns>
        public List<EntityExclusionDetailDTO> GetUIFieldsToHide(string uiName)
        {
            log.LogMethodEntry(uiName);
            List<EntityExclusionDetailDTO> listOfUIFieldsToHide = new List<EntityExclusionDetailDTO>();
            if (dataAccessRuleDTO != null && dataAccessRuleDTO.IsActive)
            {
                log.LogVariableState("dataAccessRuleDTO.DataAccessRuleId", dataAccessRuleDTO.DataAccessRuleId);
                if (dataAccessRuleDTO.DataAccessDetailDTOList != null)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_ACCESS_ENTITY"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, uiName));
                    List<LookupValuesDTO> lookupValuesDataAccessEntityDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);


                    if (lookupValuesDataAccessEntityDTOList != null && lookupValuesDataAccessEntityDTOList.Count > 0)
                    {
                        foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                        {
                            if (dataAccessDetailDTO.EntityId == lookupValuesDataAccessEntityDTOList[0].LookupValueId)
                            {

                                if (dataAccessDetailDTO.EntityExclusionDetailDTOList != null)
                                {
                                    foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in dataAccessDetailDTO.EntityExclusionDetailDTOList)
                                    {
                                        if (!string.IsNullOrEmpty(entityExclusionDetailDTO.FieldName) && entityExclusionDetailDTO.IsActive)
                                        {
                                            listOfUIFieldsToHide.Add(entityExclusionDetailDTO);
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
            }
            log.LogMethodExit(listOfUIFieldsToHide);
            return listOfUIFieldsToHide;
        }
        /// <summary>
        /// get the data access rules DTO
        /// </summary>
        public DataAccessRuleDTO DataAccessRuleDTO { get { return dataAccessRuleDTO; } }
    }
    /// <summary>
    /// Manages the list of data access rules
    /// </summary>
    public class DataAccessRuleList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<DataAccessRuleDTO> dataAccessRuleDTOList;

        /// <summary>
        /// Constructor having Execution Context
        /// </summary>
        /// <param name="executionContext"></param>
        public DataAccessRuleList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="dataAccessRuleDTOList"></param>
        public DataAccessRuleList(ExecutionContext executionContext, List<DataAccessRuleDTO> dataAccessRuleDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, dataAccessRuleDTOList);
            this.dataAccessRuleDTOList = dataAccessRuleDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// Returns the data access rule list
        /// </summary>
        public DataAccessRuleDTO GetDataAccessRule(int dataAccessRuleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(dataAccessRuleId, sqlTransaction);
            DataAccessRuleDataHandler dataAccessRuleDataHandler = new DataAccessRuleDataHandler(sqlTransaction);
            DataAccessRuleDTO dataAccessRuleDTO = dataAccessRuleDataHandler.GetDataAccessRule(dataAccessRuleId);
            log.LogMethodExit(dataAccessRuleDTO);
            return dataAccessRuleDTO;
        }

        /// <summary>
        /// Returns the data access rules and access rule details and exclusion details list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecord"></param>
        /// <param name="sqlTrxn"></param>
        /// <returns></returns>
        public List<DataAccessRuleDTO> GetAllDataAccessRule(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecord, sqlTransaction);
            DataAccessRuleDataHandler dataAccessRuleDataHandler = new DataAccessRuleDataHandler();
            log.LogMethodExit(null, " returning the result of DataAccessRuleDataHandler.GetDataAccessRuleList() call.");
            var dataAccessRuleDTOs = dataAccessRuleDataHandler.GetDataAccessRuleList(searchParameters);
            if (loadChildRecord && dataAccessRuleDTOs != null && dataAccessRuleDTOs.Count != 0)
            {
                DataAccessDetailList dataAccessDetailList = new DataAccessDetailList();
                List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>> searchDataAccessRuleParameters = new List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>>();
                searchDataAccessRuleParameters.Add(new KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>(DataAccessDetailDTO.SearchByDataAccessDetailParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (loadActiveChildRecords)
                {
                    searchDataAccessRuleParameters.Add(new KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>(DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACTIVE_FLAG, "1"));
                }
                List<DataAccessDetailDTO> dataAccessDetailDTOList = dataAccessDetailList.GetAllDataAccessDetail(searchDataAccessRuleParameters);

                EntityExclusionDetailList entityExclusionDetailList = new EntityExclusionDetailList();
                List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> searchEntityExclusionDetailParameters = new List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>>();
                searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (loadActiveChildRecords)
                {
                    searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG, "1"));
                }
                List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = entityExclusionDetailList.GetAllEntityExclusionDetail(searchEntityExclusionDetailParameters);
                foreach (DataAccessRuleDTO dataAccessRuleDTO in dataAccessRuleDTOs)
                {
                    if (dataAccessDetailDTOList != null && dataAccessDetailDTOList.Any())
                    {
                        dataAccessRuleDTO.DataAccessDetailDTOList = dataAccessDetailDTOList.FindAll(m => m.DataAccessRuleId == dataAccessRuleDTO.DataAccessRuleId).ToList();
                    }
                    if (dataAccessRuleDTO.DataAccessDetailDTOList != null && dataAccessRuleDTO.DataAccessDetailDTOList.Any())
                    {
                        foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                        {
                            if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Any())
                            {
                                dataAccessDetailDTO.EntityExclusionDetailDTOList = entityExclusionDetailDTOList.FindAll(m => m.RuleDetailId == dataAccessDetailDTO.RuleDetailId).ToList();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(dataAccessRuleDTOs);
            return dataAccessRuleDTOs;
        }

        /// <summary>
        /// Returns the data access rules and access rule details and exclusion details list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecord"></param>
        /// <param name="sqlTrxn"></param>
        /// <returns></returns>
        public List<DataAccessRuleDTO> GetAllDataAccessRules(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, bool loadChildRecord = false, bool loadActiveChildRecords = false, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecord, sqlTransaction);
            DataAccessRuleDataHandler dataAccessRuleDataHandler = new DataAccessRuleDataHandler(sqlTransaction);
            log.LogMethodExit(null, " returning the result of DataAccessRuleDataHandler.GetDataAccessRuleList() call.");
            var dataAccessRuleDTOs = dataAccessRuleDataHandler.GetDataAccessRuleLists(searchParameters, currentPage, pageSize, sqlTransaction );
            if (loadChildRecord && dataAccessRuleDTOs != null && dataAccessRuleDTOs.Count != 0)
            {
                DataAccessDetailList dataAccessDetailList = new DataAccessDetailList();
                List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>> searchDataAccessRuleParameters = new List<KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>>();
                searchDataAccessRuleParameters.Add(new KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>(DataAccessDetailDTO.SearchByDataAccessDetailParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (loadActiveChildRecords)
                {
                    searchDataAccessRuleParameters.Add(new KeyValuePair<DataAccessDetailDTO.SearchByDataAccessDetailParameters, string>(DataAccessDetailDTO.SearchByDataAccessDetailParameters.ACTIVE_FLAG, "1"));
                }
                List<DataAccessDetailDTO> dataAccessDetailDTOList = dataAccessDetailList.GetAllDataAccessDetail(searchDataAccessRuleParameters);

                EntityExclusionDetailList entityExclusionDetailList = new EntityExclusionDetailList();
                List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>> searchEntityExclusionDetailParameters = new List<KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>>();
                searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (loadActiveChildRecords)
                {
                    searchEntityExclusionDetailParameters.Add(new KeyValuePair<EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters, string>(EntityExclusionDetailDTO.SearchByEntityExclusionDetailParameters.ACTIVE_FLAG, "1"));
                }
                List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = entityExclusionDetailList.GetAllEntityExclusionDetail(searchEntityExclusionDetailParameters);
                foreach (DataAccessRuleDTO dataAccessRuleDTO in dataAccessRuleDTOs)
                {
                    if (dataAccessDetailDTOList != null && dataAccessDetailDTOList.Any())
                    {
                        dataAccessRuleDTO.DataAccessDetailDTOList = dataAccessDetailDTOList.FindAll(m => m.DataAccessRuleId == dataAccessRuleDTO.DataAccessRuleId).ToList();
                    }
                    if (dataAccessRuleDTO.DataAccessDetailDTOList != null && dataAccessRuleDTO.DataAccessDetailDTOList.Any())
                    {
                        foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                        {
                            if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Any())
                            {
                                dataAccessDetailDTO.EntityExclusionDetailDTOList = entityExclusionDetailDTOList.FindAll(m => m.RuleDetailId == dataAccessDetailDTO.RuleDetailId).ToList();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(dataAccessRuleDTOs);
            return dataAccessRuleDTOs;
        }
        /// <summary>
        /// This method should be used to Save and Update the Data Access Rules details for Web Management Studio.
        /// </summary>
        public List<DataAccessRuleDTO> SaveUpdateDataAccessRuleList()
        {
            log.LogMethodEntry();
            List<DataAccessRuleDTO> savedDataAccesRuleDTOList = new List<DataAccessRuleDTO>();
            if (dataAccessRuleDTOList != null && dataAccessRuleDTOList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (DataAccessRuleDTO dataAccessRuleDTO in dataAccessRuleDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DataAccessRule dataAccessRule = new DataAccessRule(executionContext, dataAccessRuleDTO);
                            dataAccessRule.Save(parafaitDBTrx.SQLTrx);
                            savedDataAccesRuleDTOList.Add(dataAccessRule.DataAccessRuleDTO);
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
                log.LogMethodExit();
            }
            return savedDataAccesRuleDTOList;
        }

        public int GetDataAccessRuleCount(List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DataAccessRuleDataHandler dataAccessRuleDataHandler = new DataAccessRuleDataHandler(sqlTransaction);
            int dataAccessRuleCount = dataAccessRuleDataHandler.GetDataAccessRuleCount(searchParameters);
            log.LogMethodExit(dataAccessRuleCount);
            return dataAccessRuleCount;
        }

    }
}
