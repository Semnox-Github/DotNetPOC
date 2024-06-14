/********************************************************************************************
 * Project Name - DSLookup BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Mar-2017      Lakshminarayana     Created 
 *2.40        28-Sep-2018      Jagan Mohan         Added new constructor DSLookupBL, DSLookupListBL and 
 *                                                 methods SaveUpdateDsLookupList
 *2.70.2        30-Jul-2019      Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
*2.90        07-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
 *                                                 List class changes as per 3 tier standards.
*2.110.0     2-Dec-2020       Prajwal S          Modified : Constructor with Id parameter
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Business logic for DSLookup class.
    /// </summary>
    public class DSLookupBL
    {
        private DSLookupDTO dSLookupDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of DSLookupBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private DSLookupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.dSLookupDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with the dSLookup id as the parameter
        /// Would fetch the dSLookup object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="id">id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public DSLookupBL(ExecutionContext executionContext, int id, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqltransaction);
            DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler(sqltransaction);
            List<DSignageLookupValuesDTO> dSignageLookupValuesDTOList;
            this.dSLookupDTO = dSLookupDataHandler.GetDSLookupDTO(id);
            if (dSLookupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DSLookup", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (dSLookupDTO != null)
            {
                DSignageLookupValuesListBL dSignageLookupValuesListBL = new DSignageLookupValuesListBL(executionContext);
                List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>> lookupValuesSearchParameters;
                lookupValuesSearchParameters = new List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>>();
                lookupValuesSearchParameters.Add(new KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>(DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_ID, dSLookupDTO.DSLookupID.ToString()));
                lookupValuesSearchParameters.Add(new KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>(DSignageLookupValuesDTO.SearchByParameters.IS_ACTIVE, "1"));
                dSignageLookupValuesDTOList = dSignageLookupValuesListBL.GetDSignageLookupValuesDTOList(lookupValuesSearchParameters, sqltransaction);
                if (dSignageLookupValuesDTOList != null)
                {
                    this.dSLookupDTO.DSignageLookupValuesDTOList = new SortableBindingList<DSignageLookupValuesDTO>(dSignageLookupValuesDTOList);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with the dSLookup id as the parameter
        /// Would fetch the dSLookup object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="id">id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public DSLookupBL(ExecutionContext executionContext, string contentGuid, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, contentGuid, sqltransaction);
            DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler(sqltransaction);
            List<DSignageLookupValuesDTO> dSignageLookupValuesDTOList;
            this.dSLookupDTO = dSLookupDataHandler.GetDSLookupDTOByGuid(contentGuid);
            if (dSLookupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DSLookup", contentGuid);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (dSLookupDTO != null)
            {
                DSignageLookupValuesListBL dSignageLookupValuesListBL = new DSignageLookupValuesListBL(executionContext);
                List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>> lookupValuesSearchParameters;
                lookupValuesSearchParameters = new List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>>();
                lookupValuesSearchParameters.Add(new KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>(DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_ID, dSLookupDTO.DSLookupID.ToString()));
                lookupValuesSearchParameters.Add(new KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>(DSignageLookupValuesDTO.SearchByParameters.IS_ACTIVE, "1"));
                dSignageLookupValuesDTOList = dSignageLookupValuesListBL.GetDSignageLookupValuesDTOList(lookupValuesSearchParameters);
                if (dSignageLookupValuesDTOList != null)
                {
                    this.dSLookupDTO.DSignageLookupValuesDTOList = new SortableBindingList<DSignageLookupValuesDTO>(dSignageLookupValuesDTOList);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates DSLookupBL object using the DSLookupDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="dSLookupDTO">dSLookupDTO</param>
        public DSLookupBL(ExecutionContext executionContext, DSLookupDTO dSLookupDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dSLookupDTO);
            this.dSLookupDTO = dSLookupDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the DSLookup
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqltransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler(sqlTransaction);
            if (dSLookupDTO.IsChangedRecursive == false && dSLookupDTO.DSLookupID > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            if (dSLookupDTO.DSLookupID < 0)
            {
                dSLookupDTO = dSLookupDataHandler.InsertDSLookup(dSLookupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dSLookupDTO.AcceptChanges();
            }
            else
            {
                if (dSLookupDTO.IsChanged)
                {
                    dSLookupDTO = dSLookupDataHandler.UpdateDSLookup(dSLookupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    dSLookupDTO.AcceptChanges();
                }
            }
            SaveDSignageLookupValues(sqlTransaction);
            dSLookupDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : DSignageLookupValuesDTOList 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveDSignageLookupValues(SqlTransaction sqlTransaction)
        {
            if (dSLookupDTO.DSignageLookupValuesDTOList != null &&
                dSLookupDTO.DSignageLookupValuesDTOList.Any())
            {
                List<DSignageLookupValuesDTO> updatedDSignageLookupValuesList = new List<DSignageLookupValuesDTO>();
                foreach (var dSignageLookupValuesDTO in dSLookupDTO.DSignageLookupValuesDTOList)
                {
                    if (dSignageLookupValuesDTO.DSLookupID != dSLookupDTO.DSLookupID)
                    {
                        dSignageLookupValuesDTO.DSLookupID = dSLookupDTO.DSLookupID;
                    }
                    if (dSignageLookupValuesDTO.IsChanged)
                    {
                        updatedDSignageLookupValuesList.Add(dSignageLookupValuesDTO);
                    }
                }
                if (updatedDSignageLookupValuesList.Any())
                {
                    DSignageLookupValuesListBL dSignageLookupValuesListBL = new DSignageLookupValuesListBL(executionContext, updatedDSignageLookupValuesList);
                    dSignageLookupValuesListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the DSLookupDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Checks whether the query is valid. 
        /// </summary>
        /// <param name="query">query</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public bool CheckQuery(string query, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(query, sqlTransaction);
            DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler(sqlTransaction);
            bool retVal = dSLookupDataHandler.CheckQuery(query);
            log.LogMethodExit(retVal);
            return retVal;
        }

        /// <summary>
        /// Returns the result of the dynamic query
        /// </summary>
        /// <returns></returns>
        public List<List<string>> GetDynamicContentData()
        {
            log.LogMethodEntry();
            List<List<string>> returnValue = null;
            string lookupValue = string.Empty;
            if (dSLookupDTO != null)
            {
                if (string.Equals(dSLookupDTO.DynamicFlag, "Y"))
                {
                    if (!string.IsNullOrEmpty(dSLookupDTO.Query))
                    {
                        DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler();
                        returnValue = dSLookupDataHandler.GetDynamicContentData(dSLookupDTO.Query);
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DSLookupDTO DSLookupDTO
        {
            get
            {
                return dSLookupDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of DSLookup
    /// </summary>
    public class DSLookupListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<DSLookupDTO> dsLookupDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of DSLookupListBL class
        /// </summary>
        /// <param name="executioncontext">executioncontext</param>
        public DSLookupListBL(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor of DSLookupListBL class
        /// </summary>
        /// <param name="dSLookupDTOs">dSLookupDTOs</param>
        /// <param name="executioncontext">executioncontext</param>
        public DSLookupListBL(ExecutionContext executioncontext, List<DSLookupDTO> dSLookupDTOs) : this(executioncontext)
        {
            log.LogMethodEntry(dSLookupDTOs, executioncontext);
            this.dsLookupDTOList = dSLookupDTOs;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DSLookup list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>DSLookup list</returns>
        //public List<DSLookupDTO> GetDSLookupDTOList(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(searchParameters, sqlTransaction);
        //    List<DSLookupDTO> dSLookupDTOList;
        //    List<DSignageLookupValuesDTO> dSignageLookupValuesDTOList;
        //    DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler(sqlTransaction);

        //    dSLookupDTOList = dSLookupDataHandler.GetDSLookupDTOList(searchParameters);
        //    if (dSLookupDTOList != null && dSLookupDTOList.Count > 0)
        //    {
        //        bool getOnlyActiveRecords = false;
        //        foreach (KeyValuePair<DSLookupDTO.SearchByParameters, string> keyValuePair in searchParameters)
        //        {
        //            if (keyValuePair.Key == DSLookupDTO.SearchByParameters.IS_ACTIVE && string.Equals(keyValuePair.Value, "1"))
        //            {
        //                getOnlyActiveRecords = true;
        //            }
        //        }
        //        DSignageLookupValuesListBL dSignageLookupValuesListBL = new DSignageLookupValuesListBL(executionContext);
        //        List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>> lookupValuesSearchParameters;
        //        foreach (DSLookupDTO dSLookupDTO in dSLookupDTOList)
        //        {
        //            lookupValuesSearchParameters = new List<KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>>();
        //            lookupValuesSearchParameters.Add(new KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>(DSignageLookupValuesDTO.SearchByParameters.DSLOOKUP_ID, dSLookupDTO.DSLookupID.ToString()));
        //            if (getOnlyActiveRecords)
        //            {
        //                lookupValuesSearchParameters.Add(new KeyValuePair<DSignageLookupValuesDTO.SearchByParameters, string>(DSignageLookupValuesDTO.SearchByParameters.IS_ACTIVE, "1"));
        //            }
        //            dSignageLookupValuesDTOList = dSignageLookupValuesListBL.GetDSignageLookupValuesDTOList(lookupValuesSearchParameters);
        //            if (dSignageLookupValuesDTOList != null)
        //            {
        //                dSLookupDTO.DSignageLookupValuesDTOList = new SortableBindingList<DSignageLookupValuesDTO>(dSignageLookupValuesDTOList);
        //            }
        //        }
        //    }
        //    log.LogMethodEntry(dSLookupDTOList);
        //    return dSLookupDTOList;
        //}
        public List<DSLookupDTO> GetDSLookupDTOList(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters,
                                                                                bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            DSLookupDataHandler dSLookupDataHandler = new DSLookupDataHandler(sqlTransaction);
            this.dsLookupDTOList = dSLookupDataHandler.GetDSLookupDTOList(searchParameters);
            if (dsLookupDTOList != null && dsLookupDTOList.Any() && loadChildRecords)
            {
                Build(dsLookupDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(dsLookupDTOList);
            return dsLookupDTOList;
        }
        private void Build(List<DSLookupDTO> dsLookupDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, DSLookupDTO> dsLookupDTODictionary = new Dictionary<int, DSLookupDTO>();
            List<int> dsLookupIdList = new List<int>();
            for (int i = 0; i < dsLookupDTOList.Count; i++)
            {
                if (dsLookupDTODictionary.ContainsKey(dsLookupDTOList[i].DSLookupID))
                {
                    continue;
                }
                dsLookupDTODictionary.Add(dsLookupDTOList[i].DSLookupID, dsLookupDTOList[i]);
                dsLookupIdList.Add(dsLookupDTOList[i].DSLookupID);
            }
            DSignageLookupValuesListBL dSignageLookupValuesListBL = new DSignageLookupValuesListBL(executionContext);
            List<DSignageLookupValuesDTO> dSignageLookupValuesDTOList = dSignageLookupValuesListBL.GetDSignageLookupValuesDTOList(dsLookupIdList, activeChildRecords, sqlTransaction);

            if (dSignageLookupValuesDTOList != null && dSignageLookupValuesDTOList.Any())
            {
                for (int i = 0; i < dSignageLookupValuesDTOList.Count; i++)
                {
                    if (dsLookupDTODictionary.ContainsKey(dSignageLookupValuesDTOList[i].DSLookupID) == false)
                    {
                        continue;
                    }
                    DSLookupDTO dsLookupDTO = dsLookupDTODictionary[dSignageLookupValuesDTOList[i].DSLookupID];
                    if (dsLookupDTO.DSignageLookupValuesDTOList == null)
                    {
                        dsLookupDTO.DSignageLookupValuesDTOList = new SortableBindingList<DSignageLookupValuesDTO>();
                    }
                    dsLookupDTO.DSignageLookupValuesDTOList.Add(dSignageLookupValuesDTOList[i]);
                }
            }
        }
      

        /// <summary>
        /// Save and Update the dslookup details
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            bool queryValid = true;
            if (dsLookupDTOList == null ||
                dsLookupDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < dsLookupDTOList.Count; i++)
            {
                var dsLookupDTO = dsLookupDTOList[i];
                if (dsLookupDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    DSLookupBL dsLookupBL = new DSLookupBL(executionContext, dsLookupDTO);
                    if (string.Equals(dsLookupDTO.DynamicFlag, "Y"))
                    {
                        if (!string.IsNullOrEmpty(dsLookupDTO.Query))
                        {
                            queryValid = dsLookupBL.CheckQuery(dsLookupDTO.Query);
                        }
                        if (queryValid == false)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1776));
                        }
                    }
                    dsLookupBL.Save();
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
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving dsLookupDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("dsLookupDTO", dsLookupDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
