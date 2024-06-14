/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - Data object of RedemptionCurrencyContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *0.0         05-Jul-2020   Vikas Dwivedi       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int currencyId;
        private int productId;
        private string currencyName;
        private double valueInTickets;
        private string barCode;
        //private bool isActive;
        private bool showQtyPrompt;
        private bool managerApproval;
        private string shortCutKeys;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionCurrencyContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required table fields.
        /// </summary>
        public RedemptionCurrencyContainerDTO(int currencyId, int productId, string currencyName, double valueInTickets, string barCode, bool showQtyPrompt, bool managerApproval, string shortCutKeys)
            : this()
        {
            log.LogMethodEntry(currencyId, productId, currencyName, valueInTickets, barCode, showQtyPrompt, managerApproval, shortCutKeys);
            this.currencyId = currencyId;
            this.productId = productId;
            this.currencyName = currencyName;
            this.valueInTickets = valueInTickets;
            this.barCode = barCode;
            //this.isActive = isActive;
            this.showQtyPrompt = showQtyPrompt;
            this.managerApproval = managerApproval;
            this.shortCutKeys = shortCutKeys;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        [DisplayName("CurrencyId")]
        [ReadOnly(true)]
        public int CurrencyId { get { return currencyId; } set { currencyId = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the CurrencyName field
        /// </summary>
        [DisplayName("CurrencyName")]
        public string CurrencyName { get { return currencyName; } set { currencyName = value; } }

        /// <summary>
        /// Get/Set method of the ValueInTickets field
        /// </summary>
        [DisplayName("ValueInTickets")]
        public double ValueInTickets { get { return valueInTickets; } set { valueInTickets = value; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("BarCode")]
        public string BarCode { get { return barCode; } set { barCode = value;} }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        //[DisplayName("Is Active")]
        //public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the ShowQtyPrompt field
        /// </summary>
        [DisplayName("Show Quantity Prompt")]
        public bool ShowQtyPrompt { get { return showQtyPrompt; } set { showQtyPrompt = value;} }

        /// <summary>
        /// Get/Set method of the managerApproval field
        /// </summary>
        [DisplayName("Manager Approval")]
        public bool ManagerApproval { get { return managerApproval; } set { managerApproval = value;} }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("ShortCutKeys")]
        public string ShortCutKeys { get { return shortCutKeys; } set { shortCutKeys = value;} }
    }
}
