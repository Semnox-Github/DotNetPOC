/********************************************************************************************
 * Project Name - Device
 * Description  - KoreaFiscalization  Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************

*2.150.0      01-Dec-2021      Vidita Solution        Korean Fiscalization
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Device.Printer.FiscalPrinter.Smartro
{
    public class SmartroPaymentResponseDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string CardNumber { set; get; }
        public string TradeUniqueId { set; get; }
        public string ItemApprovalId { set; get; }
        public string CardCompany { set; get; }

        public SmartroPaymentResponseDTO()
        {
            log.LogMethodEntry();
            CardNumber = string.Empty;
            TradeUniqueId = string.Empty;
            ItemApprovalId = string.Empty;
            CardCompany = string.Empty;
            log.LogMethodExit();
        }
        public SmartroPaymentResponseDTO(string cardNumber, string tradeUniqueId, string itemApprovalId, string cardCompany)
        {
            log.LogMethodEntry();
            CardNumber = cardNumber;
            TradeUniqueId = tradeUniqueId;
            ItemApprovalId = itemApprovalId;
            CardCompany = cardCompany;
            log.LogMethodExit();
        }
    }
}