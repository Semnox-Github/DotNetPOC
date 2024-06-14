/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - IRedemptionUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          27-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Printer;
using System.Drawing;
using System;

namespace Semnox.Parafait.Redemption
{
    public interface IRedemptionUseCases
    {
        Task<List<RedemptionDTO>> GetRedemptionOrders(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true,
            int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        Task<RedemptionDTO> SaveRedemptionOrders(List<RedemptionDTO> redemptionDTOList);
        Task<RedemptionDTO> AddCard(int orderId ,List<RedemptionCardsDTO> redemptionCardDTOList);
        Task<RedemptionDTO> UpdateCard(int orderId, List<RedemptionCardsDTO> redemptionCardDTOList);
        Task<RedemptionDTO> AddGift(int orderId ,List<RedemptionGiftsDTO> redemptionGiftsDTOList);
        Task<RedemptionDTO> UpdateGift(int orderId ,List<RedemptionGiftsDTO> redemptionGiftsDTOList);
        Task<RedemptionDTO> RemoveCard(int orderId ,List<RedemptionCardsDTO> redemptionCardsDTOList);
        Task<RedemptionDTO> RemoveGift(int orderId,List<RedemptionGiftsDTO> redemptionGiftsDTOList);
        Task<List<ApplicationRemarksDTO>> GetApplicationRemarks(List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<ApplicationRemarksDTO> SaveApplicationRemarks(List<ApplicationRemarksDTO> applicationRemarksDTOList);
        Task<RedemptionDTO> AddTurnInCards(int orderId, List<RedemptionCardsDTO> redemptionCardDTOList);
        Task<RedemptionDTO> RemoveTurnInCards(int orderId, List<RedemptionCardsDTO> redemptionCardDTOList);
        Task<RedemptionDTO> AddTurnInGifts( int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList);
        Task<RedemptionDTO> RemoveTurnInGifts(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList);
        Task<RedemptionDTO> AddTicket(int orderId, List<TicketReceiptDTO> ticketReceiptDTOList);
        Task<RedemptionDTO> RemoveTicket(int orderId, List<TicketReceiptDTO> ticketReceiptDTOList);
        Task<RedemptionDTO> ConsolidateTicketReceipts(int orderId ,RedemptionActivityDTO redemptionActivityDTO);
        Task<RedemptionDTO> AddManualTickets(int orderId, RedemptionActivityDTO redemptionActivityDTO);
        Task<RedemptionDTO> LoadTicketsToCard(int orderId ,RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO);
        Task<ReceiptClass> GetRedemptionOrderPrint(int orderId);
        Task<RedemptionDTO> UpdateRedemptionStatus(int orderId , RedemptionActivityDTO redemptionActivityDTO);
        Task<RedemptionDTO> ReverseRedemption(int orderId , RedemptionActivityDTO redemptionActivityDTO);
        Task<RedemptionDTO> RemoveManualTickets (int orderId, RedemptionActivityDTO redemptionActivityDTO);
        Task<RedemptionPriceContainerDTOCollection> GetRedemptionPriceContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<clsTicket> ReprintManualTicketReceipt(int ticketId, RedemptionActivityDTO redemptionActivityDTO);
        Task<clsTicket> PrintManualTicketReceipt(int ticketId);
        Task<clsTicket> GetRealTicketReceiptPrint(int sourceRedemptionId, int tickets, DateTime? issueDate);
        Task<ReceiptClass> GetSuspendedReceiptPrint(int orderId);
        Task<RedemptionDTO> SuspendOrders(List<RedemptionDTO> redemptionDTOList);
        Task<RedemptionDTO> AddCurrency(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList);
        Task<RedemptionDTO> RemoveCurrency(int orderId, List<RedemptionCardsDTO> redemptionCardsDTOList);
    }
}
