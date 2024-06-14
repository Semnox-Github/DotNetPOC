/******************************************************************************************************
 * Project Name - Device
 * Description  - Mashreq Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                 Remarks          
 ******************************************************************************************************
 *2.150.0     26-July-2022    Prasad, Dakshakh Raj       Mashreq Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class MashreqResponseDTO
    {
    }


    [Serializable]

	[XmlRoot(ElementName = "EFTData")]
	public class EFTData
	{
		[XmlElement(ElementName = "CommandType")]
		public string CommandType { get; set; }
		[XmlElement(ElementName = "ErrorCode")]
		public string ErrorCode { get; set; }
		[XmlElement(ElementName = "SaleToEpp")]
		public string SaleToEpp { get; set; }
		[XmlElement(ElementName = "ResponseCode")]
		public string ResponseCode { get; set; }
		[XmlElement(ElementName = "TxnDescription")]
		public string TxnDescription { get; set; }
		[XmlElement(ElementName = "HostActionCode")]
		public string HostActionCode { get; set; }
		[XmlElement(ElementName = "HostActionCodeMsg")]
		public string HostActionCodeMsg { get; set; }
		[XmlElement(ElementName = "Reversed")]
		public string Reversed { get; set; }
		[XmlElement(ElementName = "TransactionDate")]
		public string TransactionDate { get; set; }
		[XmlElement(ElementName = "TransactionTime")]
		public string TransactionTime { get; set; }
		[XmlElement(ElementName = "SequenceNo")]
		public string SequenceNo { get; set; }
		[XmlElement(ElementName = "CardSchemeName")]
		public string CardSchemeName { get; set; }
		[XmlElement(ElementName = "MaskCardNumber")]
		public string MaskCardNumber { get; set; }
		[XmlElement(ElementName = "ExpiryDate")]
		public string ExpiryDate { get; set; }
		[XmlElement(ElementName = "CardHolderName")]
		public string CardHolderName { get; set; }
		[XmlElement(ElementName = "Amount")]
		public string Amount { get; set; }
		[XmlElement(ElementName = "TipAmount")]
		public string TipAmount { get; set; }
		[XmlElement(ElementName = "CupBilledAmt")]
		public string CupBilledAmt { get; set; }
		[XmlElement(ElementName = "CupDiscountAmt")]
		public string CupDiscountAmt { get; set; }
		[XmlElement(ElementName = "CupFinalAmt")]
		public string CupFinalAmt { get; set; }
		[XmlElement(ElementName = "Currency")]
		public string Currency { get; set; }
		[XmlElement(ElementName = "TxnStatus")]
		public string TxnStatus { get; set; }
		[XmlElement(ElementName = "AuthCode")]
		public string AuthCode { get; set; }
		[XmlElement(ElementName = "EntryMode")]
		public string EntryMode { get; set; }
		//[XmlElement(ElementName = "EMVData")]
		//public EMVData EMVData { get; set; }
		[XmlElement(ElementName = "DCCStatus")]
		public string DCCStatus { get; set; }
		//[XmlElement(ElementName = "DccData")]
		//public DccData DccData { get; set; }
		[XmlElement(ElementName = "CHVerify")]
		public string CHVerify { get; set; }
		[XmlElement(ElementName = "TID")]
		public string TID { get; set; }
		[XmlElement(ElementName = "MID")]
		public string MID { get; set; }
		[XmlElement(ElementName = "InvoiceNo")]
		public string InvoiceNo { get; set; }
		[XmlElement(ElementName = "BatchNo")]
		public string BatchNo { get; set; }
		[XmlElement(ElementName = "MREFLabel")]
		public string MREFLabel { get; set; }
		[XmlElement(ElementName = "MREFValue")]
		public string MREFValue { get; set; }
		[XmlElement(ElementName = "ReceiptDataMerchant")]
		public string ReceiptDataMerchant { get; set; }
		[XmlElement(ElementName = "ReceiptDataCustomer")]
		public string ReceiptDataCustomer { get; set; }
		[XmlElement(ElementName = "TransactionNo")]
		public string TransactionNo { get; set; }
		[XmlElement(ElementName = "EDWTID")]
		public string EDWTID { get; set; }
		[XmlElement(ElementName = "EDWMID")]
		public string EDWMID { get; set; }
		[XmlElement(ElementName = "RRN")]
		public string RRN { get; set; }
		[XmlElement(ElementName = "MerchantTranId")]
		public string MerchantTranId { get; set; }
	}


}
