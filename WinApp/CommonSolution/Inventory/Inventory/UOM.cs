/********************************************************************************************
 * Project Name - UOM BL
 * Description  - BL of the UOM class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Dec-2016   Soumya          Created
 *2.60.3      14-Jun-2019   Nagesh Badiger  Added parametrized constructor and SaveUpdateUOMList() in UOMList
 *2.70.2        20-Jul-2019   Deeksha         Modifications as per three tier standard
 *            22-Oct-2019   Rakesh          Added to get the UMODTO property
 *2.100.0     26-Jul-2020   Deeksha         Modified for Recipe Management enhancement 
 *2.110.0     07-Oct-2020   Mushahid Faizan Modified as per 3 tier standards, Added methods for Pagination and Excel Sheet functionalities,
 *                                          Renamed SaveUpdateUOMList method to Save.
 *2.120.0     11-May-2021   Mushahid Faizan Modified for Web Inventory and Renamed BuildUOMDTOList() to BuildRelatedUOMDTOList()
 *2.150.0     13-Dec-2022   Abhishek        Modified:Validate() as a part of Web Inventory Redesign.
 *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// BL class for UOM
    /// </summary>
    public class UOM
    {
        private UOMDTO uomDTO;
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private UOM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.uomDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="uomDTO">Parameter of the type UOMDTO</param>
        public UOM(ExecutionContext executionContext, UOMDTO uomDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, uomDTO);
            this.executionContext = executionContext;
            this.uomDTO = uomDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the UOM id as the parameter
        /// Would fetch the UOM object based on the ID passed. 
        /// </summary>
        /// <param name="locationId">Location id</param>
        /// <param name="ExecutionContext">ExecutionContext</param>
        public UOM(ExecutionContext executionContext, int uomId,
                                         bool loadChildRecords = false, bool activeChildRecords = false,
                                         SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, uomId, loadChildRecords, activeChildRecords, sqlTransaction);
            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            this.uomDTO = uomDataHandler.GetUOM(uomId);
            if (loadChildRecords == false ||
                uomDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            UOMConversionFactorListBL uomConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
            uomDTO.UOMConversionFactorDTOList = uomConversionFactorListBL.GetUOMDTOListOfUoms(new List<int> { uomId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get the Conversion Factor.
        /// </summary>
        /// <param name="baseInventoryUOM"></param>
        /// <returns></returns>
        public double GetConversionFactor(int baseInventoryUOM)
        {
            log.LogMethodEntry(baseInventoryUOM);
            double conversionFactor = 1;
            try
            {
                UOMList uOMList = new UOMList(executionContext);
                UOMConversionFactorListBL uomConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
                List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<UOMConversionFactorDTO> uomConversionFactorDTOList = uomConversionFactorListBL.GetUOMConversionFactorDTOList(searchParameters);

                if (uomConversionFactorDTOList != null && uomConversionFactorDTOList.Any())
                {
                    List<UOMConversionFactorDTO> conversionDTO = uomConversionFactorDTOList.FindAll(x => x.BaseUOMId == baseInventoryUOM & x.UOMId == uomDTO.UOMId).ToList();
                    if (conversionDTO != null && conversionDTO.Any())
                    {
                        conversionFactor = 1 / conversionDTO[0].ConversionFactor;
                    }
                    else
                    {
                        conversionDTO = uomConversionFactorDTOList.FindAll(x => x.BaseUOMId == uomDTO.UOMId & x.UOMId == baseInventoryUOM).ToList();
                        if (conversionDTO != null && conversionDTO.Any())
                        {
                            conversionFactor = 1 * conversionDTO[0].ConversionFactor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(conversionFactor);
            return conversionFactor;
        }

        /// <summary>
        /// Saves the UOM
        /// UOM will be inserted if UOMId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            if (uomDTO == null || uomDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            Validate(sqlTransaction);
            if (uomDTO.UOMId < 0)
            {
                uomDTO = uomDataHandler.InsertUOM(uomDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                uomDTO.AcceptChanges();
            }
            else
            {
                if (uomDTO.IsChanged)
                {
                    uomDTO = uomDataHandler.UpdateUOM(uomDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    uomDTO.AcceptChanges();
                }
            }
            if (uomDTO.UOMConversionFactorDTOList != null &&
                uomDTO.UOMConversionFactorDTOList.Count != 0)
            {
                foreach (UOMConversionFactorDTO uomConversionFactorDTO in uomDTO.UOMConversionFactorDTOList)
                {
                    if (uomConversionFactorDTO.IsChanged)
                    {
                        uomConversionFactorDTO.BaseUOMId = uomDTO.UOMId;
                        UOMConversionFactorBL uomConversionFactorBL = new UOMConversionFactorBL(executionContext, uomConversionFactorDTO);
                        uomConversionFactorBL.Save(sqlTransaction);
                    }
                }
            }
            if (!string.IsNullOrEmpty(uomDTO.Guid))
            {
                InventoryActivityLogDTO InventoryActivityLogDTO = new InventoryActivityLogDTO(serverTimeObject.GetServerDateTime(), "UOM Inserted",
                                                         uomDTO.Guid, false, executionContext.GetSiteId(), "UOM", -1, uomDTO.UOMId + ":" + uomDTO.UOM.ToString(), -1, executionContext.GetUserId(),
                                                         serverTimeObject.GetServerDateTime(), executionContext.GetUserId(), serverTimeObject.GetServerDateTime());


                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, InventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the uomDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (string.IsNullOrWhiteSpace(uomDTO.UOM))
            {
                log.Error("Enter UOM ");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2607, MessageContainerList.GetMessage(executionContext, "UOM"));
                throw new ValidationException(errorMessage);
            }
            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<UOMDTO> uomDTOList = uomDataHandler.GetUOMList(searchParameters);
            if (uomDTOList != null && uomDTOList.Any())
            {
                if (uomDTOList.Exists(x => x.UOM.ToLower() == uomDTO.UOM.ToLower()) && uomDTO.UOMId == -1)
                {
                    log.Error("Duplicate entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "UOM"));
                    throw new ValidationException(errorMessage);
                }
                if (uomDTOList.Exists(x => x.UOM.ToLower() == uomDTO.UOM.ToLower() && x.UOMId != uomDTO.UOMId))
                {
                    log.Error("Duplicate update entries detail");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "UOM"));
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        public UOMDTO getUOMDTO { get { return uomDTO; } }
    }

    public class UOMList
    {
        /// <summary>
        /// UOMList class
        /// </summary>
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<UOMDTO> uomDTOList = new List<UOMDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public UOMList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="uOMDTOList">uOMDTOList</param>
        public UOMList(ExecutionContext executionContext, List<UOMDTO> uomDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, uomDTOList);
            this.uomDTOList = uomDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of Categories matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetUOMCount(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            int uomCount = uomDataHandler.GetUOMCount(searchParameters);
            log.LogMethodExit(uomCount);
            return uomCount;
        }

        /// <summary>
        /// Returns the UOMDTO list
        /// </summary>
        public List<UOMDTO> GetAllUOMDTOList(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters,
            bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null, int currentPage = 0, int pageSize = 0, bool uomConversionFactor = false)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            UOMDataHandler uOMDataHandler = new UOMDataHandler(sqlTransaction);
            List<UOMDTO> UOMDTOList = uOMDataHandler.GetUOMList(searchParameters, currentPage, pageSize);
            if (loadChildRecords == false ||
                UOMDTOList == null ||
                UOMDTOList.Any() == false)
            {
                log.LogMethodExit(UOMDTOList, "Child records are not loaded.");
                return UOMDTOList;
            }
            if (loadChildRecords && uomConversionFactor)
            {
                BuildUOMConversionDTOList(UOMDTOList, activeChildRecords, sqlTransaction);
            }
            else
            {
                BuildRelatedUOMDTOList(UOMDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(UOMDTOList);
            return UOMDTOList;
        }

        private void BuildUOMConversionDTOList(List<UOMDTO> UOMDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(UOMDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, UOMDTO> UOMDTOIdMap = new Dictionary<int, UOMDTO>();
            List<int> uomIdList = new List<int>();
            for (int i = 0; i < UOMDTOList.Count; i++)
            {
                if (UOMDTOIdMap.ContainsKey(UOMDTOList[i].UOMId))
                {
                    continue;
                }
                UOMDTOIdMap.Add(UOMDTOList[i].UOMId, UOMDTOList[i]);
                uomIdList.Add(UOMDTOList[i].UOMId);
            }
            UOMConversionFactorListBL uOMConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
            List<UOMConversionFactorDTO> uOMConversionFactorDTOList = uOMConversionFactorListBL.GetUOMConversionFactorDTOList(uomIdList, activeChildRecords, sqlTransaction);
            if (uOMConversionFactorDTOList != null && uOMConversionFactorDTOList.Any())
            {
                for (int i = 0; i < uOMConversionFactorDTOList.Count; i++)
                {
                    if ((UOMDTOIdMap.ContainsKey(uOMConversionFactorDTOList[i].BaseUOMId) | UOMDTOIdMap.ContainsKey(uOMConversionFactorDTOList[i].UOMId)) == false)
                    {
                        continue;
                    }
                    UOMDTO uOMDTO = UOMDTOIdMap[uOMConversionFactorDTOList[i].BaseUOMId];
                    if (uOMDTO.UOMConversionFactorDTOList == null || uOMDTO.UOMConversionFactorDTOList.Count == 0)
                    {
                        uOMDTO.UOMConversionFactorDTOList = new List<UOMConversionFactorDTO>();
                    }
                    uOMDTO.UOMConversionFactorDTOList.Add(uOMConversionFactorDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
        private void BuildRelatedUOMDTOList(List<UOMDTO> UOMDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(UOMDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, UOMDTO> UOMDTOIdMap = new Dictionary<int, UOMDTO>();
            List<int> uomIdList = new List<int>();
            for (int i = 0; i < UOMDTOList.Count; i++)
            {
                if (UOMDTOIdMap.ContainsKey(UOMDTOList[i].UOMId))
                {
                    continue;
                }
                UOMDTOIdMap.Add(UOMDTOList[i].UOMId, UOMDTOList[i]);
                uomIdList.Add(UOMDTOList[i].UOMId);
            }
            UOMDTO UOMDTO = UOMDTOList[0];
            UOMConversionFactorListBL uOMConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
            List<UOMConversionFactorDTO> uOMConversionFactorDTOList = uOMConversionFactorListBL.GetUOMDTOListOfUoms(uomIdList, activeChildRecords, sqlTransaction);
            if (uOMConversionFactorDTOList != null && uOMConversionFactorDTOList.Any())
            {
                for (int i = 0; i < uOMConversionFactorDTOList.Count; i++)
                {
                    if ((UOMDTOIdMap.ContainsKey(uOMConversionFactorDTOList[i].BaseUOMId) | UOMDTOIdMap.ContainsKey(uOMConversionFactorDTOList[i].UOMId)) == false)
                    {
                        continue;
                    }
                    if (UOMDTO.UOMConversionFactorDTOList == null)
                    {
                        UOMDTO.UOMConversionFactorDTOList = new List<UOMConversionFactorDTO>();
                    }
                    UOMDTO.UOMConversionFactorDTOList.Add(uOMConversionFactorDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the UOM list
        /// </summary>
        public List<UOMDTO> GetAllUOMs(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            uomDTOList = uomDataHandler.GetUOMList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(uomDTOList);
            return uomDTOList;
        }
        /// <summary>
        /// Returns the columns name list of uom table.
        /// </summary>
        /// <returns></returns>
        public DataTable GetUOMColumnsName()
        {
            log.LogMethodEntry();
            UOMDataHandler uomDataHandler = new UOMDataHandler();
            log.LogMethodExit();
            return uomDataHandler.GetUOMColumns();
        }

        /// <summary>
        /// This method is will return Sheet object for UOM.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString()));
            uomDTOList = uomDataHandler.GetUOMList(searchParameters);

            UOMExcelDTODefinition uOMExcelDTODefinition = new UOMExcelDTODefinition(executionContext, "");
            ///Building headers from UOMExcelDTODefinition
            uOMExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (uomDTOList != null && uomDTOList.Any())
            {
                foreach (UOMDTO uomDTO in uomDTOList)
                {
                    uOMExcelDTODefinition.Configure(uomDTO);

                    Row row = new Row();
                    uOMExcelDTODefinition.Serialize(row, uomDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            UOMExcelDTODefinition uOMExcelDTODefinition = new UOMExcelDTODefinition(executionContext, "");
            List<UOMDTO> rowUOMDTOList = new List<UOMDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    UOMDTO rowUOMDTO = (UOMDTO)uOMExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowUOMDTOList.Add(rowUOMDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowUOMDTOList != null && rowUOMDTOList.Any())
                    {
                        UOMList uOMListBL = new UOMList(executionContext, rowUOMDTOList);
                        uOMListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;

        }

        /// <summary>
        /// Saves UOM List
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (uomDTOList == null ||
               uomDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < uomDTOList.Count; i++)
            {
                var uomDTO = uomDTOList[i];
                if (uomDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    UOM uomBL = new UOM(executionContext, uomDTO);
                    uomBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving uomDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("uomDTO", uomDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the UOMConversionFactorDTO List for UOMIdList
        /// </summary>
        /// <param name="UOMIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of UOMConversionFactorDTO</returns>
        public List<UOMDTO> GetUOMDTOListOfConversionFactor(List<int> UOMIdList, bool activeRecords = true,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(UOMIdList, activeRecords, sqlTransaction);
            UOMDataHandler uomDataHandler = new UOMDataHandler(sqlTransaction);
            List<UOMDTO> uomConversionFactorDTOList = uomDataHandler.GetUOMDTOListOfConversionFactor(UOMIdList, activeRecords);
            log.LogMethodExit(uomConversionFactorDTOList);
            return uomConversionFactorDTOList;
        }

        /// <summary>
        /// Returns related UOM's list 
        /// </summary>
        /// <param name="uomId"></param>
        /// <returns></returns>
        public List<UOMDTO> GetRelatedUOMList(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomSearchParameter, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(uomSearchParameter, sqlTransaction);
            List<UOMDTO> uomListOnDisplay;
            UOMList uOMList = new UOMList(executionContext);
            uomListOnDisplay = uOMList.GetAllUOMDTOList(uomSearchParameter, true, true);
            List<UOMDTO> relatedUOMList = new List<UOMDTO>();
            if (uomListOnDisplay[0].UOMConversionFactorDTOList != null && uomListOnDisplay[0].UOMConversionFactorDTOList.Any())
            {
                List<int> childList = uomListOnDisplay[0].UOMConversionFactorDTOList.Select(x => x.UOMId).ToList();
                childList.AddRange(uomListOnDisplay[0].UOMConversionFactorDTOList.Select(x => x.BaseUOMId).ToList());
                childList = childList.Distinct().ToList();
                List<UOMDTO> childDTOList = GetUOMDTOListOfConversionFactor(childList);
                foreach (UOMDTO childUomDTO in childDTOList)
                {
                    relatedUOMList.Add(childUomDTO);
                }
            }
            else
            {
                relatedUOMList.Add(uomListOnDisplay[0]);
            }
            log.LogMethodExit(relatedUOMList);
            return relatedUOMList;
        }


        public DateTime? GetUOMModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            UOMDataHandler uomDataHandler = new UOMDataHandler();
            DateTime? result = uomDataHandler.GetUOMModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
