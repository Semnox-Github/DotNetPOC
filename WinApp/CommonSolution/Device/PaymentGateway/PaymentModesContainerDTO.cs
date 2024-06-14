/********************************************************************************************
 * Project Name - Device
 * Description  - Data structure of PaymentModesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     17-Aug-2021    Fiona               Created
 *2.150.1     22-Feb-2023    Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int paymentModeId;
        private string paymentMode;
        private bool isCreditCard;
        private decimal? creditCardSurchargePercentage;
        private bool isCash;
        private bool isDebitCard;
        private int gateway;
        private bool managerApprovalRequired;
        private bool isRoundOff;
        private bool pOSAvailable;
        private int displayOrder;
        private string imageFileName;
        private char isQRCode;
        private bool paymentReferenceMandatory;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private string guid;
        private List<CompatiablePaymentModesContainerDTO> compatiablePaymentModesContainerDTOList;
        private List<PaymentModeChannelsContainerDTO> paymentModeChannelsContainerDTOList;
        private List<PaymentModeDisplayGroupsContainerDTO> paymentModeDisplayGroupsContainerDTOList;
        //private List<DiscountCouponsContainerDTO> discountCouponsContainerDTOList;

        /// <summary>
        /// Default Contructor
        /// </summary>
        public PaymentModesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        /// <param name="paymentModeId"></param>
        /// <param name="paymentMode"></param>
        /// <param name="isCreditCard"></param>
        /// <param name="creditCardSurchargePercentage"></param>
        /// <param name="isCash"></param>
        /// <param name="isDebitCard"></param>
        /// <param name="gateway"></param>
        /// <param name="managerApprovalRequired"></param>
        /// <param name="isRoundOff"></param>
        /// <param name="pOSAvailable"></param>
        /// <param name="displayOrder"></param>
        /// <param name="imageFileName"></param>
        /// <param name="isQRCode"></param>
        /// <param name="paymentReferenceMandatory"></param>
        /// <param name="attribute1"></param>
        /// <param name="attribute2"></param>
        /// <param name="attribute3"></param>
        /// <param name="attribute4"></param>
        /// <param name="attribute5"></param>
        /// <param name="guid"></param>
        public PaymentModesContainerDTO(int paymentModeId, string paymentMode, bool isCreditCard, decimal? creditCardSurchargePercentage, bool isCash, bool isDebitCard, int gateway, bool managerApprovalRequired, bool isRoundOff, bool pOSAvailable, int displayOrder, string imageFileName,
            char isQRCode, bool paymentReferenceMandatory, string attribute1, string attribute2, string attribute3, string attribute4, string attribute5, string guid) 
        {
            log.LogMethodEntry(paymentModeId,  paymentMode,  isCreditCard,  creditCardSurchargePercentage,  isCash,  isDebitCard,  gateway,  managerApprovalRequired,  isRoundOff,  pOSAvailable,  displayOrder,  imageFileName,
             isQRCode,  paymentReferenceMandatory,  attribute1,  attribute2,  attribute3,  attribute4,  attribute5,  guid);
            this.paymentModeId = paymentModeId;
            this.paymentMode = paymentMode;
            this.isCreditCard = isCreditCard;
            this.creditCardSurchargePercentage = creditCardSurchargePercentage;
            this.isCash = isCash;
            this.isDebitCard = isDebitCard;
            this.gateway = gateway;
            this.managerApprovalRequired = managerApprovalRequired;
            this.isRoundOff = isRoundOff;
            this.pOSAvailable = pOSAvailable;
            this.displayOrder = displayOrder;
            this.imageFileName = imageFileName;
            this.isQRCode = isQRCode;
            this.paymentReferenceMandatory = paymentReferenceMandatory;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            this.guid = guid;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Get/Set method of the PaymentModeId field
        /// </summary>
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value;  } }

        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        
        public string PaymentMode { get { return paymentMode; } set { paymentMode = value;  } }

        /// <summary>
        /// Get/Set method of the IsCreditCard field
        /// </summary>
        
        public bool IsCreditCard { get { return isCreditCard; } set { isCreditCard = value;  } }

        /// <summary>
        /// Get/Set method of the CreditCardSurchargePercentage field
        /// </summary>
        
        public decimal? CreditCardSurchargePercentage { get { return creditCardSurchargePercentage; } set { creditCardSurchargePercentage = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the IsCash field
        /// </summary>
        
        public bool IsCash { get { return isCash; } set { isCash = value;  } }

        /// <summary>
        /// Get/Set method of the IsDebitCard field
        /// </summary>
        
        public bool IsDebitCard { get { return isDebitCard; } set { isDebitCard = value;  } }

        /// <summary>
        /// Get/Set method of the Gateway field
        /// </summary>
        
        public int Gateway { get { return gateway; } set { gateway = value;  } }
        
        /// <summary>
        /// Get/Set method of the ManagerApprovalRequired field
        /// </summary>
        
        public bool ManagerApprovalRequired { get { return managerApprovalRequired; } set { managerApprovalRequired = value;  } }

        /// <summary>
        /// Get/Set method of the IsRoundOff field
        /// </summary>
        
        public bool IsRoundOff { get { return isRoundOff; } set { isRoundOff = value;  } }

        /// <summary>
        /// Get/Set method of the POSAvailable field
        /// </summary>
        
        public bool POSAvailable { get { return pOSAvailable; } set { pOSAvailable = value;  } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value;  } }

        /// <summary>
        /// Get/Set method of the ImageFileName field
        /// </summary>
        
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value;  } }

        

        /// <summary>
        /// Get/Set method of the IsQRCode field
        /// </summary>
       
        public char IsQRCode { get { return isQRCode; } set { isQRCode = value;  } }//Ends:ChinaICBC changes

        /// <summary>
        /// Get/Set method of the PaymentReferenceMandatory field
        /// </summary>
        
        public bool PaymentReferenceMandatory { get { return paymentReferenceMandatory; } set { paymentReferenceMandatory = value;  } }
        /// <summary>
        ///  Get/Set method of the CompatiablePaymentModesContainerDTOList field
        /// </summary>
        public List<CompatiablePaymentModesContainerDTO> CompatiablePaymentModesContainerDTOList
        {
            get { return compatiablePaymentModesContainerDTOList; }
            set { compatiablePaymentModesContainerDTOList = value; }
        }
        /// <summary>
        /// Get/Set method of the PaymentModeChannelsContainerDTO field
        /// </summary>
        public List<PaymentModeChannelsContainerDTO> PaymentModeChannelsContainerDTOList
        {
            get { return paymentModeChannelsContainerDTOList; }
            set { paymentModeChannelsContainerDTOList = value; }
        }
        ///// <summary>
        ///// Get/Set method of the DiscountCouponsContainerDTO field
        ///// </summary>
        //public List<DiscountCouponsContainerDTO> DiscountCouponsContainerDTOList
        //{
        //    get { return discountCouponsContainerDTOList; }
        //    set { discountCouponsContainerDTOList = value; }
        //}

        /// <summary>
        /// Get/Set method of the PaymentModeDisplayGroupsContainerDTOList field
        /// </summary>
        public List<PaymentModeDisplayGroupsContainerDTO> PaymentModeDisplayGroupsContainerDTOList
        {
            get { return paymentModeDisplayGroupsContainerDTOList; }
            set { paymentModeDisplayGroupsContainerDTOList = value; }
        }

    }
}
