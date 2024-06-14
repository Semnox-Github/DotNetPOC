/********************************************************************************************
 * Project Name - Transaction Services - AttractionBookingViewDTO
 * Description  - View DTO of Attraction Booking. 
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
  *2.110      20-Jan-2021   Nitin Pai                Created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction.TransactionFunctions
{
    public class AttractionBookingViewDTO
    {
        private AttractionBookingDTO attractionBookingDTO;
        private String customerName;
        private int productId;
        private string facilityMapName;
        private int cardId;
        private string cardNumber;
        private string trxNo;
        private string remarks;

        public AttractionBookingDTO AttractionBookingDTO { get { return attractionBookingDTO; } set { attractionBookingDTO = value; } }
        public String CustomerName { get { return customerName; } set { customerName = value; } }
        public int ProductId { get { return productId; } set { productId = value; } }
        public string FacilityMapName { get { return facilityMapName; } set { facilityMapName = value; } }
        public int CardId { get { return cardId; } set { cardId = value; } }
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; } }
        public string TrxNo { get { return trxNo; } set { trxNo = value; } }
        public string Remarks { get { return remarks; } set { remarks = value; } }

    }
}
