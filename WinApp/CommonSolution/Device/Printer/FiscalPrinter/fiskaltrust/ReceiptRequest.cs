/********************************************************************************************
 * Project Name - Device
 * Description  - Fiskaltrust data structure
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 *2.90.0           14-Jul-2020      Gururaja Kanjan    Created for fiskaltrust integration.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
   
    public class ReceiptRequest
    {
        public string ftCashBoxID { get; set; }
        public string ftQueueID { get; set; }
        public string ftPosSystemId { get; set; }
        public string cbTerminalID { get; set; }
        public string cbReceiptReference { get; set; }
        public DateTime cbReceiptMoment { get; set; }
        public ChargeItem[] cbChargeItems { get; set; }
        public PayItem[] cbPayItems { get; set; }
        public long ftReceiptCase { get; set; }
        public string ftReceiptCaseData { get; set; }
        public decimal? cbReceiptAmount { get; set; }
        public string cbUser { get; set; }
        public string cbArea { get; set; }
        public string cbCustomer { get; set; }
        public string cbSettlement { get; set; }
        public string cbPreviousReceiptReference { get; set; }
    }



    public partial class ReceiptResponse
    {
        public string ftCashBoxID { get; set; }
        public string ftQueueID { get; set; }
        public string ftQueueItemID { get; set; }
        public long ftQueueRow { get; set; }
        public string cbTerminalID { get; set; }
        public string cbReceiptReference { get; set; }
        public string ftCashBoxIdentification { get; set; }
        public string ftReceiptIdentification { get; set; }
        public DateTime ftReceiptMoment { get; set; }
        public string[] ftReceiptHeader { get; set; }
        public ChargeItem[] ftChargeItems { get; set; }
        public string[] ftChargeLines { get; set; }
        public PayItem[] ftPayItems { get; set; }
        public string[] ftPayLines { get; set; }
        public SignaturItem[] ftSignatures { get; set; }
        public string[] ftReceiptFooter { get; set; }
        public long ftState { get; set; }
        public string ftStateData { get; set; }
    }

    public class ChargeItem
    {
        public long Position { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal VATRate { get; set; }
        public long ftChargeItemCase { get; set; }
        public string ftChargeItemCaseData { get; set; }
        public decimal? VATAmount { get; set; }
        public string AccountNumber { get; set; }
        public string CostCenter { get; set; }
        public string ProductGroup { get; set; }
        public string ProductNumber { get; set; }
        public string ProductBarcode { get; set; }
        public string Unit { get; set; }
        public decimal? UnitQuantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? Moment { get; set; }
    }

    public class PayItem
    {
        public long Position { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long ftPayItemCase { get; set; }
        public string ftPayItemCaseData { get; set; }
        public string AccountNumber { get; set; }
        public string CostCenter { get; set; }
        public string MoneyGroup { get; set; }
        public string MoneyNumber { get; set; }
        public DateTime? Moment { get; set; }
    }

    public class SignaturItem
    {
        public long ftSignatureFormat { get; set; }
        public long ftSignatureType { get; set; }
        public string Caption { get; set; }
        public string Data { get; set; }

    }


}
