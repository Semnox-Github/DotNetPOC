/********************************************************************************************
 * Project Name - InventoryReceiveLines
 * Description  - LocalInventoryReceiveLinesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     23-Dec-2020       Prajwal S                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    class LocalInventoryReceiveLinesUseCases : IInventoryReceiveLinesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalInventoryReceiveLinesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<InventoryReceiveLinesDTO>> GetInventoryReceiveLines(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0, bool loadChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<InventoryReceiveLinesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                InventoryReceiptLineList inventoryReceiptLineList = new InventoryReceiptLineList(executionContext);
                List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList = inventoryReceiptLineList.GetAllInventoryReceiveLine(searchParameters, currentPage, pageSize, loadChildRecords, loadActiveChild);

                log.LogMethodExit(inventoryReceiveLinesDTOList);
                return inventoryReceiveLinesDTOList;
            });
        }

        public async Task<int> GetInventoryReceiveLineCounts(List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>
                          searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                
                InventoryReceiptLineList inventoryReceiptLineList = new InventoryReceiptLineList(executionContext);
                int inventoryReceiveLineCount = inventoryReceiptLineList.GetInventoryReceiveLinesCount(searchParameters);

                log.LogMethodExit(inventoryReceiveLineCount);
                return inventoryReceiveLineCount;
            });
        }

        public async Task<InventoryReceiptDTO> UpdateInventoryReceiveLines(int receiptId, List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            return await Task<InventoryReceiptDTO>.Factory.StartNew(() =>
            {
                InventoryReceiptsBL inventoryReceiptsBL = null;
                InventoryReceiptDTO inventoryReceiptDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(inventoryReceiveLinesDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (receiptId == -1)    
                        {
                            throw new Exception("receipt Id is not found");
                        }
                        inventoryReceiptsBL = new InventoryReceiptsBL(executionContext,receiptId, parafaitDBTrx.SQLTrx);
                        inventoryReceiptDTO = inventoryReceiptsBL.InventoryReceiptDTO;
                        if (inventoryReceiptDTO.InventoryReceiveLinesListDTO == null || inventoryReceiptDTO.InventoryReceiveLinesListDTO.Count == 0)
                        {
                            inventoryReceiptDTO.InventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
                        }
                        
                        inventoryReceiptDTO.InventoryReceiveLinesListDTO.AddRange(inventoryReceiveLinesDTOList);
                        inventoryReceiptsBL.Save(parafaitDBTrx.SQLTrx);
                        inventoryReceiptsBL = new InventoryReceiptsBL(executionContext,receiptId, parafaitDBTrx.SQLTrx,true);
                        inventoryReceiptDTO = inventoryReceiptsBL.InventoryReceiptDTO;
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
                        throw ex;
                    }

                }
                log.LogMethodExit(inventoryReceiptDTO);
                return inventoryReceiptDTO;
            });
        }

        public async Task<InventoryReceiptDTO> AddInventoryReceiveLines(int receiptId, List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            return await Task<InventoryReceiptDTO>.Factory.StartNew(() =>
            {
                InventoryReceiptsBL inventoryReceiptsBL = null;
                InventoryReceiptDTO inventoryReceiptDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(inventoryReceiveLinesDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (receiptId == -1)    
                        {
                            inventoryReceiptDTO = new InventoryReceiptDTO();
                            inventoryReceiptDTO.OrderDate = ServerDateTime.Now;
                            inventoryReceiptsBL = new InventoryReceiptsBL(inventoryReceiptDTO, executionContext);
                            inventoryReceiptsBL.Save(parafaitDBTrx.SQLTrx);
                            receiptId = inventoryReceiptsBL.InventoryReceiptDTO.ReceiptId;
                        }
                        inventoryReceiptsBL = new InventoryReceiptsBL(executionContext,receiptId, parafaitDBTrx.SQLTrx);
                        inventoryReceiptDTO = inventoryReceiptsBL.InventoryReceiptDTO;
                        if (inventoryReceiptDTO.InventoryReceiveLinesListDTO == null)
                        {
                            inventoryReceiptDTO.InventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
                        }
                        inventoryReceiptDTO.InventoryReceiveLinesListDTO.AddRange(inventoryReceiveLinesDTOList);
                        inventoryReceiptsBL.Save(parafaitDBTrx.SQLTrx);
                        inventoryReceiptsBL = new InventoryReceiptsBL(executionContext, receiptId, parafaitDBTrx.SQLTrx,true);
                        inventoryReceiptDTO = inventoryReceiptsBL.InventoryReceiptDTO;
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit(inventoryReceiptDTO);
                return inventoryReceiptDTO;
            });
        }

        public async Task<string> SaveInventoryReceiveLines(List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(inventoryReceiveLinesDTOList);
                if (inventoryReceiveLinesDTOList == null)
                {
                    throw new ValidationException("InventoryReceiveLinesDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InventoryReceiveLinesDTO inventoryReceiveLinesDTO in inventoryReceiveLinesDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            InventoryReceiveLinesBL inventoryReceiveLines = new InventoryReceiveLinesBL(inventoryReceiveLinesDTO, executionContext);
                            inventoryReceiveLines.Save(parafaitDBTrx.SQLTrx);
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
                            throw ex;
                        }
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}