/********************************************************************************************
* Project Name - POS 
* Description  - LocalRedemptionUseCases class to get the data  from local DB 
* 
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
0.0          26-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Redemption.V2;
using Semnox.Parafait.User;
using System.Drawing;
using Semnox.Parafait.POS;

namespace Semnox.Parafait.Redemption
{
    public class LocalRedemptionUseCases : LocalUseCases, IRedemptionUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalRedemptionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<RedemptionDTO>> GetRedemptionOrders(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true,
            int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<RedemptionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        RedemptionUseCaseListBL redemptionUseCaseListBL = new RedemptionUseCaseListBL(executionContext);
                        int siteId = GetSiteId();
                        List<RedemptionDTO> redemptionDTOList = redemptionUseCaseListBL.GetRedemptionDTOList(parameters, parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        log.LogMethodExit(redemptionDTOList);
                        return redemptionDTOList;
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
            });
        }

        public async Task<RedemptionDTO> SaveRedemptionOrders(List<RedemptionDTO> redemptionDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                V2.RedemptionBL redemptionBL = null;
                log.LogMethodEntry(redemptionDTOList);
                if (redemptionDTOList == null)
                {
                    throw new ValidationException("redemptionDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RedemptionDTO redemptionDTO in redemptionDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                            redemptionBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                    }
                }
                log.LogMethodExit(redemptionBL.RedemptionDTO);
                return redemptionBL.RedemptionDTO;
            });
        }

        public async Task<RedemptionDTO> SuspendOrders(List<RedemptionDTO> redemptionDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                RedemptionDTO result = null;
                V2.RedemptionBL redemptionBL = null;
                log.LogMethodEntry(redemptionDTOList);
                if (redemptionDTOList == null)
                {
                    throw new ValidationException("redemptionDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RedemptionDTO redemptionDTO in redemptionDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                            redemptionBL.Suspend(parafaitDBTrx.SQLTrx);
                            result = redemptionBL.RedemptionDTO;
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
                        }
                    }

                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<RedemptionDTO> AddCard(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionCardsDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            redemptionDTO = new RedemptionDTO();
                            redemptionDTO.RedeemedDate = ServerDateTime.Now;
                            redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                            redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                            orderId = redemptionBL.RedemptionDTO.RedemptionId;
                        }
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        if (redemptionDTO.RedemptionCardsListDTO == null)
                        {
                            redemptionDTO.RedemptionCardsListDTO = new List<RedemptionCardsDTO>();
                        }
                        redemptionDTO.RedemptionCardsListDTO.AddRange(redemptionCardsDTOList);
                        redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw ;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ;
                    }

                }
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }
        public async Task<RedemptionDTO> UpdateCard(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionCardsDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        if (redemptionDTO.RedemptionCardsListDTO == null)
                        {
                            redemptionDTO.RedemptionCardsListDTO = new List<RedemptionCardsDTO>();
                        }
                        foreach (RedemptionCardsDTO redemptionCardsDTO in redemptionCardsDTOList)
                        {
                            RedemptionCardsDTO result = redemptionDTO.RedemptionCardsListDTO.Where(x => x.RedemptionCardsId == redemptionCardsDTO.RedemptionCardsId).FirstOrDefault();
                            if (result != null)
                            {
                                int index = redemptionDTO.RedemptionCardsListDTO.FindIndex(x => x.RedemptionCardsId == redemptionCardsDTO.RedemptionCardsId);
                                redemptionDTO.RedemptionCardsListDTO[index] = result;
                            }
                        }
                        redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }


        public async Task<RedemptionDTO> AddGift(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionGiftsDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            redemptionDTO = new RedemptionDTO();
                            redemptionDTO.RedeemedDate = ServerDateTime.Now;
                            redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                            redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                            orderId = redemptionBL.RedemptionDTO.RedemptionId;
                        }
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                        }
                        redemptionDTO.RedemptionGiftsListDTO.AddRange(redemptionGiftsDTOList);
                        redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }
        public async Task<RedemptionDTO> UpdateGift(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionGiftsDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                        }
                        List<RedemptionGiftsDTO> backupList = new List<RedemptionGiftsDTO>();
                        foreach (RedemptionGiftsDTO redemptionGiftsDTO in redemptionDTO.RedemptionGiftsListDTO)
                        {
                            backupList.Add(redemptionGiftsDTO);
                        }
                        foreach (RedemptionGiftsDTO redemptionGiftsDTO in backupList)
                        {
                            RedemptionGiftsDTO result = redemptionGiftsDTOList.Where(x => x.RedemptionGiftsId == redemptionGiftsDTO.RedemptionGiftsId).FirstOrDefault();
                            if (result != null)
                            {
                                int index = redemptionDTO.RedemptionGiftsListDTO.FindIndex(x => x.RedemptionGiftsId == redemptionGiftsDTO.RedemptionGiftsId);
                                redemptionDTO.RedemptionGiftsListDTO[index] = result;
                            }
                        }
                        redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> AddTurnInCards(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(orderId, redemptionCardsDTOList);

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            redemptionDTO = new RedemptionDTO();
                            redemptionDTO.RedeemedDate = ServerDateTime.Now;
                            redemptionDTO.Remarks = "TURNINREDEMPTION";
                            redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                            redemptionBL.SaveTurnIns(parafaitDBTrx.SQLTrx);
                            orderId = redemptionBL.RedemptionDTO.RedemptionId;
                        }
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                        }
                        redemptionDTO.RedemptionCardsListDTO.AddRange(redemptionCardsDTOList);
                        redemptionBL.SaveTurnIns(parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> AddTurnInGifts(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(orderId, redemptionGiftsDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            redemptionDTO = new RedemptionDTO();
                            redemptionDTO.RedeemedDate = ServerDateTime.Now;
                            redemptionDTO.Remarks = "TURNINREDEMPTION";
                            redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                            redemptionBL.SaveTurnIns(parafaitDBTrx.SQLTrx);
                            orderId = redemptionBL.RedemptionDTO.RedemptionId;
                        }
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            redemptionDTO.RedemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
                        }
                        redemptionDTO.RedemptionGiftsListDTO.AddRange(redemptionGiftsDTOList);
                        redemptionBL.SaveTurnIns(parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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

                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> RemoveCard(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionCardsDTOList);

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        else
                        {
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                        }
                        if (redemptionDTO.RedemptionCardsListDTO == null)
                        {
                            throw new Exception("redemptionCards details not found");
                        }
                        redemptionBL.DeleteRedemptionCards(redemptionCardsDTOList, parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx, true);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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

                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> RemoveGift(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionGiftsDTOList);

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        else
                        {
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                        }
                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            throw new Exception("redemptionCards details not found");
                        }
                        redemptionBL.DeleteRedemptionGifts(redemptionGiftsDTOList, parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> RemoveTurnInCards(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionCardsDTOList);

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        else
                        {
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                        }
                        if (redemptionDTO.RedemptionCardsListDTO == null)
                        {
                            throw new Exception("redemptionCards details not found");
                        }
                        redemptionBL.DeleteRedemptionCards(redemptionCardsDTOList, parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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

                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> RemoveTurnInGifts(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionGiftsDTOList);
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        else
                        {
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                        }
                        if (redemptionDTO.RedemptionGiftsListDTO == null)
                        {
                            throw new Exception("redemptionCards details not found");
                        }
                        redemptionBL.DeleteRedemptionGifts(redemptionGiftsDTOList, parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> AddTicket(int orderId, List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(ticketReceiptDTOList);
                    if (ticketReceiptDTOList != null && ticketReceiptDTOList.Any())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (orderId == -1)    // Create new redemption Order with status OPEN
                            {
                                redemptionDTO = new RedemptionDTO();
                                redemptionDTO.RedeemedDate = ServerDateTime.Now;
                                redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                                redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                                orderId = redemptionBL.RedemptionDTO.RedemptionId;
                            }
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                            if (redemptionDTO.TicketReceiptListDTO == null)
                            {
                                redemptionDTO.TicketReceiptListDTO = new List<TicketReceiptDTO>();
                            }
                            redemptionDTO.TicketReceiptListDTO = ticketReceiptDTOList;
                            redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> RemoveTicket(int orderId, List<TicketReceiptDTO> ticketReceiptDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(ticketReceiptDTOList);
                    if (ticketReceiptDTOList != null && ticketReceiptDTOList.Any())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (orderId == -1)    // Create new redemption Order with status OPEN
                            {
                                throw new Exception("Order Id is not found");
                            }
                            else
                            {
                                redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                                redemptionDTO = redemptionBL.RedemptionDTO;
                            }
                            if (redemptionDTO.TicketReceiptListDTO == null)
                            {
                                throw new Exception("redemptionCards details not found");
                            }
                            redemptionBL.DeletTicketReceipt(ticketReceiptDTOList, parafaitDBTrx.SQLTrx);
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
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
                    log.LogMethodExit(redemptionDTO);
                    return redemptionDTO;
                }
            });
        }

        public async Task<List<ApplicationRemarksDTO>> GetApplicationRemarks(List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {

            return await Task<List<ApplicationRemarksDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        ApplicationRemarksList applicationRemarksList = new ApplicationRemarksList(executionContext);
                        List<ApplicationRemarksDTO> applicationRemarksDTOList = applicationRemarksList.GetAllApplicationRemarks(searchParameters);
                        log.LogMethodExit(applicationRemarksDTOList);                        
                        parafaitDBTrx.EndTransaction();
                        return applicationRemarksDTOList;
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
            });
        }

        public async Task<ReceiptClass> GetRedemptionOrderPrint(int orderId)
        {
            return await Task<ReceiptClass>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(orderId);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    { 
                        using (Utilities Utilities = GetUtility())
                        {
                            parafaitDBTrx.BeginTransaction();
                            V2.RedemptionBL redemptionUseCaseListBL = new V2.RedemptionBL(executionContext, orderId);
                            ReceiptClass receiptImage = redemptionUseCaseListBL.PrintRedemption(Utilities,-1,null,null,false, parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                            log.LogMethodExit(receiptImage);
                            return receiptImage;
                        }
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
            });
        }
        public async Task<clsTicket> GetRealTicketReceiptPrint(int sourceRedemptionId, int tickets, DateTime? issueDate)
        {
            return await Task<clsTicket>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        log.LogMethodEntry(sourceRedemptionId, tickets, issueDate);
                        clsTicket receiptImage;
                        using (Utilities Utilities = GetUtility())
                        {
                            V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext, sourceRedemptionId);
                            receiptImage = redemptionUseCaseBL.PrintRealTicketReceipt(sourceRedemptionId, tickets, Utilities, parafaitDBTrx.SQLTrx, issueDate);
                        }
                        log.LogMethodExit(receiptImage);
                        parafaitDBTrx.EndTransaction();
                        return receiptImage;
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
            });
        }
        public async Task<clsTicket> PrintManualTicketReceipt(int ticketId)
        {
            return await Task<clsTicket>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        using (Utilities Utilities = GetUtility())
                        {

                            log.LogMethodEntry(ticketId);
                            parafaitDBTrx.BeginTransaction();
                            clsTicket receiptImage;
                            TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketId);
                            if (ticketReceipt != null && ticketReceipt.TicketReceiptDTO != null && ticketReceipt.TicketReceiptDTO.SOurceRedemptionId > -1)
                            {
                                V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext, ticketReceipt.TicketReceiptDTO.SOurceRedemptionId);
                                receiptImage = redemptionUseCaseBL.PrintManualTicketReceipt(ticketReceipt.TicketReceiptDTO,  Utilities, parafaitDBTrx.SQLTrx);
                            }
                            else
                            {
                                V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext);
                                receiptImage = redemptionUseCaseBL.PrintManualTicketReceipt(ticketReceipt.TicketReceiptDTO, Utilities, parafaitDBTrx.SQLTrx);
                            }
                            log.LogMethodExit(receiptImage);
                            parafaitDBTrx.EndTransaction();
                            return receiptImage;
                        }
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
            });
        }
        public async Task<clsTicket> PrintTotalManualTickets(int sourceRedemptionId)
        {
            return await Task<clsTicket>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        log.LogMethodEntry(sourceRedemptionId);
                        clsTicket receiptImage;
                        parafaitDBTrx.BeginTransaction();
                        using (Utilities Utilities = GetUtility())
                        {
                            V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext);
                            receiptImage = redemptionUseCaseBL.PrintTotalManualTickets(sourceRedemptionId, Utilities, parafaitDBTrx.SQLTrx);
                        }
                        log.LogMethodExit(receiptImage);
                        parafaitDBTrx.EndTransaction();
                        return receiptImage;
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
            });
        }
        public async Task<clsTicket> ReprintManualTicketReceipt(int ticketId, RedemptionActivityDTO redemptionActivityDTO)
        {
            return await Task<clsTicket>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        log.LogMethodEntry(ticketId);
                        parafaitDBTrx.BeginTransaction();                        
                        clsTicket receiptImage;
                        using (Utilities Utilities = GetUtility())
                        {
                            TicketReceipt ticketReceipt = new TicketReceipt(executionContext, ticketId);
                            if (ticketReceipt != null && ticketReceipt.TicketReceiptDTO != null &&  ticketReceipt.TicketReceiptDTO.SOurceRedemptionId>-1)
                            {
                                V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext, ticketReceipt.TicketReceiptDTO.SOurceRedemptionId);
                                receiptImage = redemptionUseCaseBL.ReprintManualTicketReceipt(ticketId, redemptionActivityDTO, Utilities, parafaitDBTrx.SQLTrx);
                            }
                            else
                            {
                                V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext);
                                receiptImage = redemptionUseCaseBL.ReprintManualTicketReceipt(ticketId, redemptionActivityDTO, Utilities, parafaitDBTrx.SQLTrx);
                            }
                        }
                        log.LogMethodExit(receiptImage);
                        parafaitDBTrx.EndTransaction();
                        return receiptImage;
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
            });
        }

        public async Task<ReceiptClass> GetSuspendedReceiptPrint(int orderId)
        {
            return await Task<ReceiptClass>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        log.LogMethodEntry(orderId);
                        ReceiptClass receiptImage;
                        using (Utilities Utilities = GetUtility())
                        {
                            V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext, orderId);
                            receiptImage = redemptionUseCaseBL.PrintSuspended(Utilities, parafaitDBTrx.SQLTrx);
                        }
                        log.LogMethodExit(receiptImage);
                        parafaitDBTrx.EndTransaction();
                        return receiptImage;
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
            });
        }
        private Utilities GetUtility()
        {
            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            if (string.IsNullOrWhiteSpace(executionContext.POSMachineName))
            {
                POSMachines pOSMachineContainerDTO = new POSMachines(executionContext, executionContext.GetMachineId());
                if (pOSMachineContainerDTO.POSMachineDTO != null)
                {
                    executionContext.POSMachineName = pOSMachineContainerDTO.POSMachineDTO.POSName;
                    Utilities.ParafaitEnv.SetPOSMachine("", executionContext.POSMachineName);
                }
            }
            else
            {
                Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            Utilities.ParafaitEnv.Initialize();
            return Utilities;
        }

        public async Task<ApplicationRemarksDTO> SaveApplicationRemarks(List<ApplicationRemarksDTO> applicationRemarksDTOList)
        {
            return await Task<ApplicationRemarksDTO>.Factory.StartNew(() =>
            {
                ApplicationRemarksDTO result = null;

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(applicationRemarksDTOList);
                    foreach (ApplicationRemarksDTO applicationRemarksDTO in applicationRemarksDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ApplicationRemarks applicationRemarks = new ApplicationRemarks(executionContext, applicationRemarksDTO);
                            applicationRemarks.Save(parafaitDBTrx.SQLTrx);
                            result = applicationRemarks.ApplicationRemarksDTO;
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
                    log.LogMethodExit(result);
                    return result;
                }


            });
        }
        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }

        public async Task<RedemptionDTO> ConsolidateTicketReceipts(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionActivityDTO);
                    if (redemptionActivityDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            TicketReceiptDTO ticketReceiptDTO = redemptionUseCaseBL.ConsolidateTickets(redemptionActivityDTO, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionUseCaseBL.RedemptionDTO;
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
                    List<RedemptionDTO> redemptionDTOList = new List<RedemptionDTO>();
                    redemptionDTOList.Add(redemptionDTO);
                    RedemptionUseCaseListBL redemptionUseCaseListBL = new RedemptionUseCaseListBL(executionContext, redemptionDTOList);
                    redemptionUseCaseListBL.SetToSiteTimeOffset(redemptionDTOList);
                    redemptionDTO = redemptionDTOList.FirstOrDefault();
                    redemptionDTO.AcceptChanges();
                    log.LogMethodExit(redemptionDTO);
                    return redemptionDTO;
                }
            });
        }
        public async Task<RedemptionDTO> UpdateRedemptionStatus(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionActivityDTO);
                    if (redemptionActivityDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            V2.RedemptionBL redemptionUseCaseListBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionUseCaseListBL.UpdateRedemptionStatus(redemptionActivityDTO, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionUseCaseListBL.RedemptionDTO;
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

                log.LogMethodExit(redemptionDTO);
                //log.Error("local use case"+ redemptionDTO.OrderDeliveredDate);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> LoadTicketsToCard(int orderId, RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionLoadToCardRequestDTO);
                    if (redemptionLoadToCardRequestDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            using (Utilities Utilities = GetUtility())
                            {
                                V2.RedemptionBL RedemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                                RedemptionBL.LoadTicketsToCard(redemptionLoadToCardRequestDTO, Utilities, parafaitDBTrx.SQLTrx);
                                RedemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                                redemptionDTO = RedemptionBL.RedemptionDTO;
                            }
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
                    List<RedemptionDTO> redemptionDTOList = new List<RedemptionDTO>();
                    redemptionDTOList.Add(redemptionDTO);
                    RedemptionUseCaseListBL redemptionUseCaseListBL = new RedemptionUseCaseListBL(executionContext, redemptionDTOList);
                    redemptionUseCaseListBL.SetToSiteTimeOffset(redemptionDTOList);
                    redemptionDTO = redemptionDTOList.FirstOrDefault();
                    redemptionDTO.AcceptChanges();
                    log.LogMethodExit(redemptionDTO);
                    return redemptionDTO;
                }
            });

        }
        public async Task<RedemptionDTO> AddManualTickets(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(orderId,redemptionActivityDTO);
                    if (redemptionActivityDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (orderId == -1)    // Create new redemption Order with status OPEN
                            {
                                redemptionDTO = new RedemptionDTO();
                                redemptionDTO.RedeemedDate = ServerDateTime.Now;
                                redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                                redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                                orderId = redemptionBL.RedemptionDTO.RedemptionId;
                            }
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                            redemptionBL.AddManualTickets(redemptionActivityDTO, parafaitDBTrx.SQLTrx);
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
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
                    log.LogMethodExit(redemptionDTO);
                    return redemptionDTO;
                }
            });
        }
        public async Task<RedemptionDTO> RemoveManualTickets(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionActivityDTO);
                    if (redemptionActivityDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            V2.RedemptionBL redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionBL.RemoveManualTickets(redemptionActivityDTO, parafaitDBTrx.SQLTrx);
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
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
                    log.LogMethodExit(redemptionDTO);
                    return redemptionDTO;
                }

            });
        }

        public async Task<RedemptionDTO> AddCurrency(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {

            log.LogMethodEntry("redemptionCardsDTOList");
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionCardsDTOList);
                    if (redemptionCardsDTOList != null && redemptionCardsDTOList.Any())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (orderId == -1)    // Create new redemption Order with status OPEN
                            {
                                redemptionDTO = new RedemptionDTO();
                                redemptionDTO.RedeemedDate = ServerDateTime.Now;
                                redemptionBL = new V2.RedemptionBL(executionContext, redemptionDTO);
                                redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                                orderId = redemptionBL.RedemptionDTO.RedemptionId;
                            }
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                            if (redemptionDTO.RedemptionCardsListDTO == null)
                            {
                                redemptionDTO.RedemptionCardsListDTO = new List<RedemptionCardsDTO>();
                            }
                            redemptionDTO.RedemptionCardsListDTO.AddRange(redemptionBL.AddRedemptionCurrency(redemptionCardsDTOList, parafaitDBTrx.SQLTrx));
                            redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx);
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
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
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        public async Task<RedemptionDTO> RemoveCurrency(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                V2.RedemptionBL redemptionBL = null;
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(redemptionCardsDTOList);

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (orderId == -1)    // Create new redemption Order with status OPEN
                        {
                            throw new Exception("Order Id is not found");
                        }
                        else
                        {
                            redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                            redemptionDTO = redemptionBL.RedemptionDTO;
                        }
                        if (redemptionDTO.RedemptionCardsListDTO == null)
                        {
                            throw new Exception("redemptionCards details not found");
                        }
                        redemptionBL.DeleteRedemptionCards(redemptionCardsDTOList, parafaitDBTrx.SQLTrx);
                        redemptionBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                        redemptionBL.SaveRedemptionOrder(parafaitDBTrx.SQLTrx, true);
                        redemptionDTO = redemptionBL.RedemptionDTO;
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

                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });
        }

        /// <summary>
        /// returns the redemption price view container data structure
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">whether to rebuild the cache</param>
        /// <returns></returns>
        public async Task<RedemptionPriceContainerDTOCollection> GetRedemptionPriceContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<RedemptionPriceContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    RedemptionPriceContainerList.Rebuild(siteId);
                }
                RedemptionPriceContainerDTOCollection result = RedemptionPriceContainerList.GetRedemptionPriceContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });

        }
        public async Task<RedemptionDTO> ReverseRedemption(int orderId, RedemptionActivityDTO redemptionActivityDTO)
        {
            return await Task<RedemptionDTO>.Factory.StartNew(() =>
            {
                RedemptionDTO redemptionDTO = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    log.LogMethodEntry(orderId);
                    if (orderId > -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            using (Utilities utilities = GetUtility())
                            {
                                V2.RedemptionBL redemptionUseCaseBL = new V2.RedemptionBL(executionContext, orderId, parafaitDBTrx.SQLTrx);
                                redemptionDTO = redemptionUseCaseBL.ReverseRedemption(redemptionActivityDTO, utilities, parafaitDBTrx.SQLTrx);
                            }
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

                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            });

        }

    }
}
