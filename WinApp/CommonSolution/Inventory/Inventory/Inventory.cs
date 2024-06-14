
/********************************************************************************************
 * Project Name - Inventory 
 * Description  - Bussiness logic of Inventory 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       12-Aug-2016    Amaresh          Created 
 *******************************************************************************************
 *1.00       28-Jun-2019    Archana          Modified:Added a method to get inventory stock details  
 *2.70.2     12-Jul-2019    Deeksha          Modifications as per three tier standard
 *2.70.2     22-Dec-2019    Girish Kundar    Modified : GetAllInventory() method added product search parameter
 *2.100.0    13-Sep-2020    Deeksha          Modified : Recipe Management enhancement changes.
 *2.130.0    19-Aug-2021    Deeksha          Modified : Issue Fix: Inventory stock not displaying decimal values.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory will creates and modifies the inventory
    /// </summary>
    public class Inventory
    {
        private InventoryDTO inventoryDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionUserContext;// = ExecutionContext.GetExecutionContext();
        //Utilities _utilities;

        /// <summary>
        /// Constructor with executionUserContext as a parameter
        /// </summary>
        /// <param name="executionUserContext">ExecutionContext</param>
        public Inventory(ExecutionContext executionUserContext)
        {
            log.LogMethodEntry(executionUserContext);
            inventoryDTO = null;
            this.executionUserContext = executionUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Inventory DTO parameter
        /// </summary>
        /// <param name="inventoryDTO">Parameter of the type InventoryDTO</param>
        /// <param name="executionUserContext">Excecution context</param>
        public Inventory(InventoryDTO inventoryDTO, ExecutionContext executionUserContext)
        {
            log.LogMethodEntry(inventoryDTO, executionUserContext);
            this.inventoryDTO = inventoryDTO;
            this.executionUserContext = executionUserContext;
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Constructor with the Inventory DTO parameter
        ///// </summary>
        ///// <param name="utilities">Parameter of the type utilities</param>
        ///// <param name="inventoryDTO">Parameter of the type InventoryDTO</param>
        //public Inventory(Utilities utilities, InventoryDTO inventoryDTO)
        //{
        //    log.Debug("Starts-Inventory(InventoryDTO) parameterized constructor.");
        //    _utilities = utilities;
        //    this.inventoryDTO = inventoryDTO;
        //    log.Debug("Ends-Inventory(InventoryDTO) parameterized constructor.");
        //}

        /// <summary>
        /// Validaates the inventory DTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrors = new List<ValidationError>();
            if (inventoryDTO.ProductId < 0)
            {
                ValidationError validationError = new ValidationError("Inventory", "ProductId", MessageContainerList.GetMessage(executionUserContext, 1144, MessageContainerList.GetMessage(executionUserContext, "Product")));
                validationErrors.Add(validationError);
            }
            log.LogMethodExit(validationErrors);
            return validationErrors;
        }

        /// <summary>
        /// Saves the Inventory
        /// Inventory  will be inserted if ProductId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public int Save(SqlTransaction  sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            int rowInserted = -1;
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            InventoryDTO inventoryDTOExist = inventoryDataHandler.GetInventory(inventoryDTO.ProductId, inventoryDTO.LocationId, inventoryDTO.LotId);
            
            if (inventoryDTOExist == null)
            {
                inventoryDTOExist = inventoryDataHandler.InsertInventory(inventoryDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                inventoryDTO.AcceptChanges();
                rowInserted = inventoryDTOExist.InventoryId;
            }
            else
            {
                if (inventoryDTO.IsChanged)
                {
                    inventoryDTOExist = inventoryDataHandler.UpdateInventory(inventoryDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    inventoryDTO.AcceptChanges();
                    rowInserted = inventoryDTOExist.InventoryId;
                }
            }
            log.LogMethodExit();
            return rowInserted;
        }

        public InventoryDTO GetInventoryDTO
        {
            get
            {
                return inventoryDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Inventory  List
    /// </summary>
    public class InventoryList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<InventoryDTO> inventoryDTOList;
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InventoryList():this(ExecutionContext.GetExecutionContext())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="inventoryDTOList">inventoryDTOList</param>
        public InventoryList(ExecutionContext executionContext, List<InventoryDTO> inventoryDTOList)
        {
            log.LogMethodEntry(executionContext, inventoryDTOList);
            this.executionContext = executionContext;
            this.inventoryDTOList = inventoryDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Inventory
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDTO</returns>
        public InventoryDTO GetInventory(int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            InventoryDTO inventoryDTO = new InventoryDTO();
            inventoryDTO = inventoryDataHandler.GetInventory(productId);
            log.LogMethodExit(inventoryDTO);
            return inventoryDTO;
        }

        /// <summary>
        /// Returns the InventoryDTO
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="locationId">locationId</param>
        /// <param name="lotId">lotId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDTO</returns>
        public InventoryDTO GetInventory(int productId, int locationId, int lotId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, locationId, lotId, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            InventoryDTO inventoryDTO = new InventoryDTO();
            inventoryDTO = inventoryDataHandler.GetInventory(productId, locationId, lotId);
            log.LogMethodExit(inventoryDTO);
            return inventoryDTO;
        }

        /// <summary>
        /// Get the Inventory details locations
        /// </summary>
        /// <param name="invProductCode"></param>
        /// <returns>inventoryLocations</returns>
        public Dictionary<string, string> GetInventoryLocations(string invProductCode, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(invProductCode, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            Dictionary<string, string> inventoryLocations = inventoryDataHandler.GetInventoryLocations(invProductCode);
            log.LogMethodExit(inventoryLocations);
            return inventoryLocations;
        }

        /// <summary>
        /// Returns the Inventory List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadInvLotInfo">loadInvLotInfo</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List InventoryDTO</returns>
        public List<InventoryDTO> GetAllInventory(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParameters, bool loadInvLotInfo = false, SqlTransaction sqlTransaction = null, bool multiFieldDescriptionSearch = false)
        {
            log.LogMethodEntry(searchParameters, loadInvLotInfo, sqlTransaction, multiFieldDescriptionSearch);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            List<InventoryDTO> listInventoryDTO = new List<InventoryDTO>();
            if (multiFieldDescriptionSearch)
            {
                listInventoryDTO = inventoryDataHandler.GetInventoryListDTO(searchParameters, multiFieldDescriptionSearch);
            }
            else
            {
                listInventoryDTO = inventoryDataHandler.GetInventoryListDTO(searchParameters);
            }
            string lotIdList = string.Empty;
            if (listInventoryDTO != null && listInventoryDTO.Count > 0 && loadInvLotInfo == true)
            {
                foreach (InventoryDTO invItem in listInventoryDTO)
                {
                    if (invItem.LotId != -1)
                    {
                        lotIdList = lotIdList + invItem.LotId + ",";
                    }
                }

                if (string.IsNullOrEmpty(lotIdList) == false)
                {
                    lotIdList = lotIdList.Substring(0, lotIdList.Length - 1);
                    List<InventoryLotDTO> inventoryLotDTOList = GetInventoryLotInfo(lotIdList);
                    if (inventoryLotDTOList.Count > 0)
                    {
                        foreach (InventoryDTO invItem in listInventoryDTO)
                        {
                            invItem.InventoryLotDTO = inventoryLotDTOList.Find(x => x.LotId == invItem.LotId);
                        }
                    }
                }
            }
            log.LogMethodExit(listInventoryDTO);
            return listInventoryDTO;
        }

        private List<InventoryLotDTO> GetInventoryLotInfo(string lotIdList)
        {
            log.LogMethodEntry(lotIdList);
            InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
            List<InventoryLotDTO> inventoryLotDTOList = new List<InventoryLotDTO>();
            List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> searchParams = new List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>
            {
                new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID_LIST, lotIdList),
                new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.SITE_ID,executionContext.GetSiteId().ToString())
            };
            inventoryLotDTOList = inventoryLotList.GetAllInventoryLot(searchParams);
            log.LogMethodExit(inventoryLotDTOList);
            return inventoryLotDTOList;
        }

        /// <summary>
        /// Returns the Inventory List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDTOList</returns>
        public List<InventoryDTO> GetAllInventory(List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler();
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList = inventoryDataHandler.GetInventoryList(searchParameters);
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;
        }

        /// <summary>
        /// Returns the Inventory
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="locationid">locationid</param>
        /// <param name="lottable">lottable</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDTO</returns>
        public InventoryDTO GetInventoryOnLots(int productId, int locationid, string lottable, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, locationid, lottable, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            InventoryDTO inventoryDTO = new InventoryDTO();
            inventoryDTO = inventoryDataHandler.GetInventoryOnLotControlled(productId, locationid, lottable);
            log.LogMethodExit(inventoryDTO);
            return inventoryDTO;
        }


        /// <summary>
        /// Gets the ProductDTO list matching the search key
        /// </summary>
        /// <param name="filterCondition">Filter condition and For product table columns use p.,UOM table columns u.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the list of ProductDTO matching the search criteria</returns>
        public List<InventoryDTO> GetInventoryList(string filterCondition, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterCondition, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList = inventoryDataHandler.GetInventoryList(filterCondition);
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;
        }

        /// <summary>
        /// GetUntouchedInventory
        /// </summary>
        /// <param name="StartDate">StartDate</param>
        /// <param name="LocationID">LocationID</param>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryDTOList</returns>
        public List<InventoryDTO> GetUntouchedInventory(DateTime StartDate, int LocationID, int SiteID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(StartDate, LocationID, SiteID, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList = inventoryDataHandler.GetUntouchedInventory(StartDate, LocationID, SiteID);
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;
        }

        /// <summary>
        /// GetInventoryDataForExcel
        /// </summary>
        /// <param name="locationID">locationID</param>
        /// <param name="siteID">siteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>dt</returns>
        public DataTable GetInventoryDataForExcel(int locationID, int siteID, SqlTransaction sqlTransaction = null, int physicalCountId = -1)
        {
            log.LogMethodEntry(locationID, siteID, sqlTransaction, physicalCountId);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            DataTable dt = inventoryDataHandler.GetInventoryDataForExcel(locationID, siteID, physicalCountId);
            log.LogMethodExit(dt);
            return dt;
        }

        /// <summary>
        /// GetInventoryList which retorns list of inventory DTO
        /// </summary>
        /// <param name="lastTimeStamp">timestamp of last updated</param>
        /// <param name="maxRowsToFetch">max number of rows can be fetched</param>
        /// <param name="lastInventoryId">last updated inventory id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>list of inventory DTOs</returns>
        public List<InventoryDTO> GetInventoryList(string lastTimeStamp, int maxRowsToFetch, int lastInventoryId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lastTimeStamp, maxRowsToFetch, lastInventoryId, sqlTransaction);
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            List<InventoryDTO> inventoryDTOs = new List<InventoryDTO>();
            inventoryDTOs = inventoryDataHandler.GetInventoryList(lastTimeStamp, maxRowsToFetch, lastInventoryId);
            log.LogMethodExit(inventoryDTOs);
            return inventoryDTOs;
        }

        /// <summary>
        /// Validates and saves the inventoryDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryDTOList == null ||
                inventoryDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }
            List<InventoryDTO> updatedInventoryDTOList = new List<InventoryDTO>(inventoryDTOList.Count);
            for (int i = 0; i < inventoryDTOList.Count; i++)
            {
                if (inventoryDTOList[i].IsChanged == false)
                {
                    continue;
                }
                Inventory inventory = new Inventory(inventoryDTOList[i], executionContext);
                List<ValidationError> validationErrors = inventory.Validate();
                if (validationErrors.Any())
                {
                    log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException("Validation failed for Inventory.", validationErrors, i);
                }
                updatedInventoryDTOList.Add(inventoryDTOList[i]);
            }
            if (updatedInventoryDTOList.Any() == false)
            {
                log.LogMethodExit(null, "Nothing changed.");
                return;
            }
            InventoryDataHandler inventoryDataHandler = new InventoryDataHandler(sqlTransaction);
            inventoryDataHandler.Save(updatedInventoryDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        public decimal GetProductStockQuantity(int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, sqlTransaction);
            decimal stockCount = 0;
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>
            {
                new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString())
            };
            List<InventoryDTO> listInventoryDTO = new List<InventoryDTO>();
            listInventoryDTO = GetAllInventory(searchParams, false, sqlTransaction);
            if (listInventoryDTO != null && listInventoryDTO.Any())
            {
                stockCount = Convert.ToDecimal(listInventoryDTO.Sum(inv => inv.Quantity));
            }
            log.LogMethodExit(stockCount);
            return stockCount;
        }

        public List<KeyValuePair<int ,decimal>>  GetProductStockQuantity(List<int> productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, sqlTransaction);
            List<KeyValuePair<int, decimal>> productIdStock = new List<KeyValuePair<int, decimal>>();
            List<InventoryDTO> stockDetailsDTO = new List<InventoryDTO>();
            string productIdList = string.Join(",", productId);
            if(productIdList == string.Empty)
            {
                return productIdStock;
            }
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>
            {
                new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST, productIdList)
            };
            List<InventoryDTO> listInventoryDTO = new List<InventoryDTO>();
            listInventoryDTO = GetAllInventory(searchParams, false, sqlTransaction);
            if (listInventoryDTO != null && listInventoryDTO.Count > 0)
            {
                for (int i = 0; i < productId.Count; i++)
                {
                    decimal stock = Convert.ToDecimal(listInventoryDTO.FindAll(x => x.ProductId == productId[i]).Sum(x => x.Quantity));
                    productIdStock.Add(new KeyValuePair<int, decimal>(productId[i], stock));
                }
            }
            log.LogMethodExit(productIdStock);
            return productIdStock;
        }

        public List<InventoryDTO> GetProductStockDetails(int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productId, sqlTransaction);
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>
            {
                new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString()),
                new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString())
            };
            List<InventoryDTO> inventoryDTOList = GetAllInventory(searchParams, true, sqlTransaction);
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;

        }
    }
}
