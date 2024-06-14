using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    class AlohaLoyaltyCreateAssignmentRequestDTO
    {
        string compId;
        string cardNum;
        int checkIdentifier;
        int storeIdentifier;
        string termIdentifier;
        string dateOfBuss;
        int mgrOverrideMagCardOnly = 0;
        int mgrOverrideMaxChecks = 0;
        int mgrOverrideSeries = 1;
        bool isSim = false;
        bool compAssignment = true;
        bool autoAgn = false;
        string trxId = Guid.NewGuid().ToString();
        string magStrp = null;
        AlohaCheckItems checkItms;

        public AlohaLoyaltyCreateAssignmentRequestDTO()
        {
            checkItms = new AlohaCheckItems();
        }

        public AlohaLoyaltyCreateAssignmentRequestDTO(string compId, string cardNumber, int checkId, int storeId, string termId, string dateOfBusiness) : this()
            {
            this.compId = compId;
            this.cardNum = cardNumber;
            this.checkIdentifier = checkId;
            this.storeIdentifier = storeId;
            this.termIdentifier = termId;
            this.dateOfBuss = dateOfBusiness;
        }

        public string companyId { get { return compId; } set { compId = value; } }
        public string cardNumber { get { return cardNum; } set { cardNum = value; } }
        public int checkID { get { return checkIdentifier; } set { checkIdentifier = value; } }
        public int storeId { get { return storeIdentifier; } set { storeIdentifier = value; } }
        public string termId { get { return termIdentifier; } set { termIdentifier = value; } }
        public string dateOfBusiness { get { return dateOfBuss; } set { dateOfBuss = value; } }
        public int managerOverrideMagCardOnly { get { return mgrOverrideMagCardOnly; } set { mgrOverrideMagCardOnly = value; } }
        public int managerOverrideMaxChecks { get { return mgrOverrideMaxChecks; } set { mgrOverrideMaxChecks = value; } }
        public int managerOverrideSeries { get { return mgrOverrideSeries; } set { mgrOverrideSeries = value; } }
        public bool isSimulated { get { return isSim; } set { isSim = value; } }
        public bool completeAssignment { get { return compAssignment; } set { compAssignment = value; } }
        public bool autoAssign { get { return autoAgn; } set { autoAgn = value; } }
        public string transactionId { get { return trxId; } set { trxId = value; } }
        public AlohaCheckItems checkDetail { get { return checkItms; } set { checkItms = value; } }
        public string magStripe { get { return magStrp; } set { magStrp = value; } }
    }

    public class AlohaCheckItems
    {
        public class AlohaTime
        {
            int hr;
            int min;
            int sec;
            public AlohaTime()
            {
                hr = 0;
                min = 0;
                sec = 0;
            }
            public int hour { get { return hr; } set { hr = value; } }
            public int minute { get { return min; } set { min = value; } }
            public int second { get { return sec; } set { sec = value; } }
        };

        public class CheckItems
        {
            int checkItmId;
            bool compen;
            int discountPrc;
            int itemId;
            int orderModeId;
            AlohaTime orderTm;
            int parentCheckItemId;
            float priceOfItem;
            bool promotion;
            int splitFact;
            bool splitPrim;

            public CheckItems()
            {
                checkItmId = 1;
                compen = false;
                discountPrc = 0;
                itemId = 0;
                orderModeId = 0;
                orderTime = new AlohaTime();
                parentCheckItemId = 0;
                priceOfItem = 0;
                promotion = false;
                splitFact = 0;
                splitPrim = true;
            }
            public int checkItemID { get { return checkItmId; } set { checkItmId = value; } }
            public bool comp { get { return compen; } set { compen = value; } }
            public int discountPrice { get { return discountPrc; } set { discountPrc = value; } }
            public int itemID { get { return itemId; } set { itemId = value; } }
            public int orderModeID { get { return orderModeId; } set { orderModeId = value; } }
            public AlohaTime orderTime { get { return orderTm; } set { orderTm = value; } }
            public int parentCheckItemID { get { return parentCheckItemId; } set { parentCheckItemId = value; } }
            public float price { get { return priceOfItem; } set { priceOfItem = value; } }
            public bool promo { get { return promotion; } set { promotion = value; } }
            public int splitFactor { get { return splitFact; } set { splitFact = value; } }
            public bool splitPrimary { get { return splitPrim; } set { splitPrim = value; } }
        }

        float totalAmount;
        int dayPart;
        int employee;
        string empName;
        AlohaTime openTm;
        int queId;
        int revCenterId;
        int tableDef;
        int tableid;
        int currencyDesign;
        List<CheckItems> checkItms;
        public AlohaCheckItems()
        {
            totalAmount = 0;
            dayPart = 0;
            employee = 0;
            empName = "";
            openTm = new AlohaTime();
            queId = 0;
            revCenterId = 0;
            tableDef = 0;
            tableid = 0;
            currencyDesign = 840;
            checkItms = new List<CheckItems>();
        }

        public float total { get { return totalAmount; } set { totalAmount = value; } }
        public int dayPartID { get { return dayPart; } set { dayPart = value; } }
        public int employeeID { get { return employee; } set { employee = value; } }
        public string employeeNickname { get { return empName; } set { empName = value; } }
        public AlohaTime openTime { get { return openTm; } set { openTm = value; } }

        public int queueID { get { return queId; } set { queId = value; } }
        public int revenueCenterID { get { return revCenterId; } set { revCenterId = value; } }
        public int tableDefID { get { return tableDef; } set { tableDef = value; } }
        public int tableID { get { return tableid; } set { tableid = value; } }
        public int currencyDesignator { get { return currencyDesign; } set { currencyDesign = value; } }
        public List<CheckItems> checkItems { get { return checkItms; } set { checkItms = value; } }
    }
}
