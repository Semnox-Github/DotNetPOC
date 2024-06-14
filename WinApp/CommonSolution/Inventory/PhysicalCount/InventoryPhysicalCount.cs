/********************************************************************************************
 * Project Name - InventoryPhysicalCount
 * Description  - Business logic of InventoryPhysicalCount
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        6-Jan-2017   Amaresh              Created 
 *2.80        18-Aug-2019  Deeksha              Added logger methods.
 *2.110.0     04-Jan-2021  Abhishek             Modified : modified for web API changes
 *2.110.0     11-Jan-2021  Mushahid Faizan      Web Inventory UI changes with Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// InventoryPhysicalCount allows to access the InventoryPhysicalCount details based on the business logic.
    /// </summary>
    public class InventoryPhysicalCount
    {
        private InventoryPhysicalCountDTO inventoryPhysicalCountDTO;
        private int selectedPhysicalCountID;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryPhysicalCount(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the physicalCountID parameter
        /// </summary>
        ///  <param name="executionContext">executionContext</param>
        /// <param name="physicalCountId">physicalCountID</param>
        /// <param name="SQLTrx"></param>
        public InventoryPhysicalCount(ExecutionContext executionContext, int physicalCountId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, physicalCountId, sqlTransaction);
            InventoryPhysicalCountDataHandler inventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            this.inventoryPhysicalCountDTO = inventoryPhysicalCountDataHandler.GetInventoryPhysicalCount(physicalCountId, sqlTransaction);
            if (inventoryPhysicalCountDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Inventory Physical Count ", physicalCountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="inventoryPhysicalCountDTO">InventoryPhysicalCountDTO</param>
        public InventoryPhysicalCount(ExecutionContext executionContext, InventoryPhysicalCountDTO inventoryPhysicalCountDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, inventoryPhysicalCountDTO);
            this.inventoryPhysicalCountDTO = inventoryPhysicalCountDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get InventoryPhysicalCountDTO Object
        /// </summary>
        public InventoryPhysicalCountDTO GetInventoryPhysicalCountDTO
        {
            get { return inventoryPhysicalCountDTO; }
        }

        /// <summary>
        /// Saves the InventoryPhysicalCount
        /// Checks if the physicalCountID is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryPhysicalCountDTO == null || inventoryPhysicalCountDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            // Validate(sqlTransaction);
            InventoryPhysicalCountDataHandler InventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            if (inventoryPhysicalCountDTO.PhysicalCountID < 0)
            {
                inventoryPhysicalCountDTO = InventoryPhysicalCountDataHandler.InsertInventoryPhysicalCount(inventoryPhysicalCountDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryPhysicalCountDTO.AcceptChanges();
                InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Physical Count Inserted",
                                                              inventoryPhysicalCountDTO.Guid, false, executionContext.GetSiteId(), "invphysicalcount", -1,
                                                              inventoryPhysicalCountDTO.PhysicalCountID + ":" + inventoryPhysicalCountDTO.Name.ToString(), -1, executionContext.GetUserId(),
                                                              ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
                CreateInventoryHistory(sqlTransaction);
            }
            else
            {
                if (inventoryPhysicalCountDTO.IsChanged)
                {
                    inventoryPhysicalCountDTO = InventoryPhysicalCountDataHandler.UpdateInventoryPhysicalCount(inventoryPhysicalCountDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    inventoryPhysicalCountDTO.AcceptChanges();
                    InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Physical Count Updated",
                                                              inventoryPhysicalCountDTO.Guid, false, executionContext.GetSiteId(), "invphysicalcount", -1,
                                                              inventoryPhysicalCountDTO.PhysicalCountID + ":" + inventoryPhysicalCountDTO.Name.ToString(), -1, executionContext.GetUserId(),
                                                              ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                    InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                    inventoryActivityLogBL.Save(sqlTransaction);
                }
            }
            //CreateInventoryHistory(sqlTransaction); 
            log.LogMethodExit();
        }

        public void UpdatePhysicalCountStatus(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryPhysicalCount inventoryPhysicalCount = new InventoryPhysicalCount(executionContext);
            selectedPhysicalCountID = inventoryPhysicalCountDTO.PhysicalCountID;
            inventoryPhysicalCountDTO = inventoryPhysicalCount.GetInventoryPhysicalCountByID(selectedPhysicalCountID, sqlTransaction);
            int locationID = inventoryPhysicalCountDTO.LocationId;
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            InventoryList inventoryList = new InventoryList();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locationID.ToString()));
            inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.MASS_UPDATE_ALLOWED, "Y"));
            inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams, false, sqlTransaction);
            if (inventoryDTOList != null)
            {
                if (CreateInventoryHistory(inventoryDTOList, sqlTransaction) == false)
                {
                    return;
                }
                ClosePhysicalCount(inventoryPhysicalCountDTO, sqlTransaction);
            }
            else
            {
                ClosePhysicalCount(inventoryPhysicalCountDTO, sqlTransaction);
            }
        }

        private bool CreateInventoryHistory(List<InventoryDTO> inventoryDTOLists, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryDTOLists, sqlTransaction);
            try
            {
                List<InventoryHistoryDTO> inventoryHistoryList = new List<InventoryHistoryDTO>();
                InventoryHistoryList inventoryHistorys = new InventoryHistoryList(executionContext);
                List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParams = new List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>>();
                searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT, "0"));
                searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID, selectedPhysicalCountID.ToString()));
                searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                inventoryHistoryList = inventoryHistorys.GetAllInventoryHistory(searchParams); // Unmodified

                foreach (InventoryDTO inventoryDTO in inventoryDTOLists)
                {
                    if (inventoryHistoryList == null
                        || (inventoryHistoryList != null
                            && inventoryHistoryList.Exists(x => x.ProductId == inventoryDTO.ProductId
                                                                && x.LotId == inventoryDTO.LotId) == false)
                       )
                    {
                        SaveInventoryHistory(inventoryDTO, sqlTransaction);
                    }

                }
                log.LogMethodExit();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Ends-CreateInventoryHistory() method with exception: " + ex.ToString());
                return false;
            }
        }

        private void SaveInventoryHistory(InventoryDTO inventoryDTO, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(inventoryDTO);
            InventoryHistoryDTO inventoryHistoryDTO = new InventoryHistoryDTO(-1,
                                                                              inventoryDTO.ProductId,
                                                                              inventoryDTO.LocationId,
                                                                              selectedPhysicalCountID,
                                                                              inventoryDTO.Quantity,
                                                                              inventoryDTO.Timestamp,
                                                                              inventoryDTO.AllocatedQuantity,
                                                                              inventoryDTO.LotId,
                                                                              inventoryDTO.ReceivePrice,
                                                                              false,
                                                                              inventoryDTO.UOMId);
            InventoryHistory inventoryHistory = new InventoryHistory(executionContext, inventoryHistoryDTO);
            inventoryHistory.Save(SQLTrx);
            log.LogMethodExit();
        }

        /// <summary>
        /// Close Physical Counting Process
        /// </summary>
        /// <param name="inventoryPhysicalCountDTO"></param>
        /// <param name="sqlTransaction"></param>
        private void ClosePhysicalCount(InventoryPhysicalCountDTO inventoryPhysicalCountDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTO, sqlTransaction);
            try
            {
                if (inventoryPhysicalCountDTO.Status.Trim() != "Open")
                {
                    log.Error("Cannot Update Physical Count status to Closed.");
                    throw new ValidationException("Cannot Update Physical Count status to Closed.");

                }
                inventoryPhysicalCountDTO.Status = "Closed";
                inventoryPhysicalCountDTO.EndDate = ServerDateTime.Now;
                inventoryPhysicalCountDTO.ClosedBy = executionContext.GetUserId();
                InventoryPhysicalCount inventoryPhysicalCount = new InventoryPhysicalCount(executionContext, inventoryPhysicalCountDTO);
                inventoryPhysicalCount.Save(sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-ClosePhysicalCount() Method with exception: " + ex.ToString());
            }
        }

        /// <summary>
        /// Creates History record
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void CreateInventoryHistory(SqlTransaction sqlTransaction)
        {
            InventoryList inventoryList = new InventoryList(executionContext);
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.MASS_UPDATE_ALLOWED, "Y"));
            if (inventoryPhysicalCountDTO.LocationId != -1)
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryPhysicalCountDTO.LocationId.ToString()));
            inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams);

            if (inventoryDTOList != null)
            {
                foreach (InventoryDTO inventoryDTO in inventoryDTOList)
                {
                    InventoryHistoryDTO inventoryHistoryDTO = new InventoryHistoryDTO();
                    inventoryHistoryDTO.ProductId = inventoryDTO.ProductId;
                    inventoryHistoryDTO.LocationId = inventoryDTO.LocationId;
                    inventoryHistoryDTO.PhysicalCountId = inventoryPhysicalCountDTO.PhysicalCountID;
                    inventoryHistoryDTO.Quantity = inventoryDTO.Quantity;
                    inventoryHistoryDTO.Timestamp = inventoryDTO.Timestamp;
                    inventoryHistoryDTO.LastupdatedUserid = inventoryDTO.Lastupdated_userid;
                    inventoryHistoryDTO.AllocatedQuantity = inventoryDTO.AllocatedQuantity;
                    inventoryHistoryDTO.SiteId = executionContext.GetSiteId();
                    inventoryHistoryDTO.LotId = inventoryDTO.LotId;
                    inventoryHistoryDTO.UOMId = inventoryDTO.UOMId;
                    inventoryHistoryDTO.ReceivePrice = inventoryDTO.ReceivePrice;
                    inventoryHistoryDTO.InitialCount = true;
                    InventoryHistory inventoryHistory = new InventoryHistory(executionContext, inventoryHistoryDTO);
                    inventoryHistory.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the InventoryWastageSummaryDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(inventoryPhysicalCountDTO.Name))
            {
                log.Debug("Physical count process name cannot be empty.");
                validationErrorList.Add(new ValidationError("PhysicalCount", "Name", MessageContainerList.GetMessage(executionContext, "Physical count process name cannot be empty.", MessageContainerList.GetMessage(executionContext, "Name"))));
            }
            InventoryPhysicalCountList inventoryPhysicalCountList = new InventoryPhysicalCountList(executionContext);
            List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters = new List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.STATUS, "Open"));
            List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList = inventoryPhysicalCountList.GetAllInventoryPhysicalCount(searchParameters);
            if (inventoryPhysicalCountDTOList != null && inventoryPhysicalCountDTOList.Count > 0 && inventoryPhysicalCountDTO.PhysicalCountID < 0)
            {
                if (inventoryPhysicalCountDTO.LocationId < 0)
                {
                    log.Debug("Active Physical count exists.Please close the Physical Count");
                    validationErrorList.Add(new ValidationError("PhysicalCount", "LocationId", MessageContainerList.GetMessage(executionContext, "Active Physical count exists.Please close the Physical Count")));
                }
                else if((inventoryPhysicalCountDTOList.Where(y => y.LocationId == -1).Count() > 0) && (inventoryPhysicalCountDTOList.Where(x => x.LocationId == inventoryPhysicalCountDTO.LocationId).Count() > 0))
                {
                    log.Debug("Physical count already exist for Location");
                    validationErrorList.Add(new ValidationError("PhysicalCount", "LocationId", MessageContainerList.GetMessage(executionContext, "Physical count already exist for Location", MessageContainerList.GetMessage(executionContext, "LocationId"))));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Method with the physicalCountID parameter
        /// </summary>
        /// <param name="physicalCountID">physicalCountID</param>
        /// <param name="SQLTrx"></param>
        public InventoryPhysicalCountDTO GetInventoryPhysicalCountByID(int physicalCountID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(physicalCountID, sqlTransaction);
            InventoryPhysicalCountDataHandler inventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            InventoryPhysicalCountDTO inventoryPhysicalCountDTO = new InventoryPhysicalCountDTO();
            inventoryPhysicalCountDTO = inventoryPhysicalCountDataHandler.GetInventoryPhysicalCount(physicalCountID, sqlTransaction);
            log.LogMethodExit(inventoryPhysicalCountDTO);
            return inventoryPhysicalCountDTO;
        }

        /// <summary>
        /// Method with the SiteID parameter
        /// </summary>
        /// <param name="SiteID">physicalCountID</param>
        /// <param name="SQLTrx"></param>
        public bool AllLocationsPhysicalCountExists(int SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(SiteID, sqlTransaction);
            InventoryPhysicalCountDataHandler inventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            bool returnValue = inventoryPhysicalCountDataHandler.AllLocationsPhysicalCountExists(SiteID, sqlTransaction);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }

    /// <summary>
    /// Manages the list of InventoryPhysicalCount
    /// </summary>
    public class InventoryPhysicalCountList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList = new List<InventoryPhysicalCountDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryPhysicalCountList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryPhysicalCountList(ExecutionContext executionContext, List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.inventoryPhysicalCountDTOList = inventoryPhysicalCountDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the InventoryPhysicalCount list
        /// </summary>
        public List<InventoryPhysicalCountDTO> GetAllInventoryPhysicalCount(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize, sqlTransaction);
            InventoryPhysicalCountDataHandler InventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            inventoryPhysicalCountDTOList = InventoryPhysicalCountDataHandler.GetInventoryPhysicalCountList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(inventoryPhysicalCountDTOList);
            return inventoryPhysicalCountDTOList;
        }

        /// <summary>
        /// Returns the no of Inventory Physical Count matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryPhysicalCountsCount(List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryPhysicalCountDataHandler InventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            int inventoryPhysicalCount = InventoryPhysicalCountDataHandler.GetInventoryPhysicalCountsCount(searchParameters);
            log.LogMethodExit(inventoryPhysicalCount);
            return inventoryPhysicalCount;
        }

        /// <summary>
        /// Saves Physical Count
        /// </summary>
        public List<InventoryPhysicalCountDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<InventoryPhysicalCountDTO> savedInventoryPhysicalCountDTOList = new List<InventoryPhysicalCountDTO>();
            if (inventoryPhysicalCountDTOList == null || inventoryPhysicalCountDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return savedInventoryPhysicalCountDTOList;
            }

            try
            {
                foreach (InventoryPhysicalCountDTO inventoryPhysicalCountDTO in inventoryPhysicalCountDTOList)
                {
                    InventoryPhysicalCount inventoryPhysicalCountBL = new InventoryPhysicalCount(executionContext, inventoryPhysicalCountDTO);
                    inventoryPhysicalCountBL.Save(sqlTransaction);
                    savedInventoryPhysicalCountDTOList.Add(inventoryPhysicalCountBL.GetInventoryPhysicalCountDTO);
                }
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
                log.Error("Error occurred while saving inventoryPhysicalCountDTO.", ex);
                throw;
            }
            log.LogMethodExit();
            return savedInventoryPhysicalCountDTOList;
        }

        /// <summary>
        /// This method is will return Sheet object for iInventoryPhysicalCountDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            InventoryPhysicalCountDataHandler inventoryPhysicalCountDataHandler = new InventoryPhysicalCountDataHandler(sqlTransaction);
            List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>> searchParameters = new List<KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters, string>(InventoryPhysicalCountDTO.SearchByInventoryPhysicalCountParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            inventoryPhysicalCountDTOList = inventoryPhysicalCountDataHandler.GetInventoryPhysicalCountList(searchParameters);

            InventoryPhysicalCountExcelDTODefinition inventoryPhysicalCountExcelDTODefinition = new InventoryPhysicalCountExcelDTODefinition(executionContext, "");
            ///Building headers from InventoryPhysicalCountExcelDTODefinition
            inventoryPhysicalCountExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (inventoryPhysicalCountDTOList != null && inventoryPhysicalCountDTOList.Any())
            {
                foreach (InventoryPhysicalCountDTO inventoryPhysicalCountDTO in inventoryPhysicalCountDTOList)
                {
                    inventoryPhysicalCountExcelDTODefinition.Configure(inventoryPhysicalCountDTO);

                    Row row = new Row();
                    inventoryPhysicalCountExcelDTODefinition.Serialize(row, inventoryPhysicalCountDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            InventoryPhysicalCountExcelDTODefinition inventoryPhysicalCountExcelDTODefinition = new InventoryPhysicalCountExcelDTODefinition(executionContext, "");
            List<InventoryPhysicalCountDTO> rowInventoryPhysicalCountDTOList = new List<InventoryPhysicalCountDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    InventoryPhysicalCountDTO rowInventoryPhysicalCountDTO = (InventoryPhysicalCountDTO)inventoryPhysicalCountExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowInventoryPhysicalCountDTOList.Add(rowInventoryPhysicalCountDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowInventoryPhysicalCountDTOList != null && rowInventoryPhysicalCountDTOList.Any())
                    {
                        InventoryPhysicalCountList inventoryPhysicalCountsListBL = new InventoryPhysicalCountList(executionContext, rowInventoryPhysicalCountDTOList);
                        inventoryPhysicalCountsListBL.Save(sqlTransaction);
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
    }
}
