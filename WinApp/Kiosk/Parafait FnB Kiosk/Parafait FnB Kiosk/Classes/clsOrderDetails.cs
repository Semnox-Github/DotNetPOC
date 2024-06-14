/********************************************************************************************
* Project Name - Parafait_Kiosk -clsOrderDetails.cs
* Description  - clsOrderDetails 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_FnB_Kiosk
{
    public class clsOrderDetails
    {
        int _NumberOfGuests = -1;
        DateTime _AgeGateDate = DateTime.MinValue;
        string _TableNumber;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal decimal SubtotalAmount;
        internal decimal TotalAmount;
        internal decimal TaxAmount;
        internal decimal PlayPoints = 0;

        public int NumberOfGuests 
        { 
            get { return _NumberOfGuests; }
            set { _NumberOfGuests = value; }
        }

        public DateTime AgeGateDate
        {
            get { return _AgeGateDate; }
            set { _AgeGateDate = value; }
        }

        public string TableNumber
        {
            get { return _TableNumber; }
            set { _TableNumber = value; }
        }

        internal TransactionPaymentsDTO transactionPaymentsDTO = null;

        internal List<ScreenModel.UIPanelElement> ElementList = new List<ScreenModel.UIPanelElement>();

        public clsOrderDetails(int inNumberOfGuests)
        {
            log.LogMethodEntry(inNumberOfGuests);
            _NumberOfGuests = inNumberOfGuests;
            _TableNumber = "";
            transactionPaymentsDTO = new TransactionPaymentsDTO();
            log.LogMethodExit();
        }

        public bool GuestCountSelected
        {
            get { return _NumberOfGuests <= 0 ? false : true; }
        }

        internal void AddItem(ScreenModel.UIPanelElement Element)
        {
            log.LogMethodEntry(Element);
            ScreenModel.UIPanelElement findElement = ElementList.Find(x => x == Element);
            if (findElement == null)
                ElementList.Add(Element);
            log.LogMethodExit();
        }

        internal int getOrderedLimitedQty()
        {
            log.LogMethodEntry();
            int totQty = 0;
            foreach(ScreenModel.UIPanelElement element in ElementList)
            {
                List<ScreenModel.ElementParameter> qtLimitParamList =
                    element.Parameters.FindAll(
                                            x => x.UserSelectedValue != null
                                            && x.UserSelectedValue != DBNull.Value
                                            && x.OrderedQuantity > 0
                                            && x.QuantityLimit == true);
                foreach(ScreenModel.ElementParameter parameter in qtLimitParamList)
                {
                    totQty += parameter.OrderedQuantity;
                }
            }

            log.LogMethodExit(totQty);
            return totQty;
        }

        public bool AllCardSale()
        {
            log.LogMethodEntry();
            bool allCardSale = true;
            foreach (ScreenModel.UIPanelElement element in this.ElementList)
            {
                foreach (ScreenModel.ElementParameter x in element.Parameters)
                {
                    if (x.UserSelectedValue != null
                        && x.UserSelectedValue != DBNull.Value
                        && x.OrderedQuantity > 0
                        && x.ParameterType != ScreenModel.ParameterType.CardSale)
                    {
                        allCardSale = false;
                        break;
                    }
                }
            }

            log.LogMethodExit(allCardSale);
            return allCardSale;
        }
    }
}
