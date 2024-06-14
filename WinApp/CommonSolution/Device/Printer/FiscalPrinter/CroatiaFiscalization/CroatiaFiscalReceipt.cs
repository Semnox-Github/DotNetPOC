/********************************************************************************************
 * Project Name - Device
 * Description  - Croatia Fiscalization data structure
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By       Remarks          
 *********************************************************************************************
 *2.140.0      04-Oct-2021      Laster Menezes    Created for Croatia Fiscalization
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Device.Printer.FiscalPrinter
{
    public class CroatiaFiscalReceipt
    {
        public List<EligibleFiscalPayments> eligibleFiscalPayments;
        public List<FiscalTaxLines> fiscalTaxLines;
    }

    public class EligibleFiscalPayments
    {
        public int siteId { get; set; }
        public decimal paymentAmount { get; set; }
        public int paymentId { get; set; }
        public string paymentMode { get; set; }
        public string isCash { get; set; }
        public string isDebitCard { get; set; }
        public string isCreditCard { get; set; }
        public string fiscalReference { get; set; }
        public string posMachine { get; set; }
        public string posFriendlyName { get; set; }
    }

    public class FiscalTaxLines
    {
        public decimal taxPercentage { get; set; }
        public decimal taxAmount { get; set; }
        public decimal amountWithoutTax { get; set; }
    }
}
