/********************************************************************************************
 * Project Name - Product Display Filter DTO
 * Description  - Data object of product Display Filter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Jeevan              Created 
 ********************************************************************************************
 *1.00       26-Apr-2017   Suneetha             Modified
 ********************************************************************************************
 *1.00       25-May-2017   Rakshith             Modified
 *********************************************************************************************
 *2.60       04-Apr-2019    Akshay Gulaganji    modified isActive DataType(from string to bool)
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the oduct Display Filter  object class. This acts as data holder for the product display list filter 
    /// </summary>
    public class ProductsFilterParams
    {
         private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      private string machineName;
      private int siteId;
      private DateTime dateOfPurchase;
      private int languageId;
      private bool requiresCardProduct;
      private bool newCard;
      private string cardNumber;
      private string loginId;
      private string languageCode;
      private string productTypeExclude;
      private List<string> productDisplayGroupsList;
      private int productId;
       private bool showProductContents;
        //added for filter staff card products
       private int displayGroupId;
       private int posMachineId;
       private int posTypeId;
       private string productType;
       private int productTypeId;
       private bool isActive;
       private string deviceType;
       private bool validateCard;
       private bool fetchUpsellProduct;
       private string hsnsacCode;
       private string externalSystemReference;


        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsFilterParams()
        {
            log.LogMethodEntry();
             machineName = "";
             siteId = -1;
             dateOfPurchase = DateTime.Today;
             languageId = -1;
             requiresCardProduct = false;
             newCard = false;
             cardNumber = "";
             loginId = "";
             languageCode = "";
             productTypeExclude = "";
             productDisplayGroupsList = new List<string>();
             productTypeId = -1;
             productId = -1;
             showProductContents = false;
             productType = "";
             deviceType = "";
             validateCard = false;
             fetchUpsellProduct = false;
             hsnsacCode = "";
             isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with data fields
        /// </summary>
        public ProductsFilterParams(int displayGrpId, int posTypeId, int posMachineId)

        {
            log.LogMethodEntry(displayGrpId, posTypeId, posMachineId);
            this.displayGroupId = displayGrpId;
            this.posTypeId = posTypeId;
            this.posMachineId = posMachineId;
            this.productTypeId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with data fields
        /// </summary>
        public ProductsFilterParams(string machineName, int siteId, DateTime dateOfPurchase, string languageCode)

        {
            log.LogMethodEntry(machineName,  siteId,  dateOfPurchase, languageCode);
            this.siteId = siteId;
            this.dateOfPurchase = dateOfPurchase;
            this.machineName = machineName;
            this.languageCode = languageCode;
            this.languageId = -1;
            this.requiresCardProduct = false;
            this.newCard = false;
            this.cardNumber = "";
            this.loginId = "";
            this.productTypeId = -1;

            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with data fields
        /// </summary>
        public ProductsFilterParams(string machineName, int siteId, DateTime dateOfPurchase, int languageId)
        {
            log.LogMethodEntry(machineName, siteId, dateOfPurchase, languageId);
            this.siteId = siteId;
            this.dateOfPurchase = dateOfPurchase;
            this.machineName = machineName;
            this.languageId = languageId;
            this.languageCode = "";
            this.requiresCardProduct = false;
            this.newCard = false;
            this.cardNumber = "";
            this.loginId = "";
            this.productTypeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        /// used
        public ProductsFilterParams(string machineName, int siteId, bool requiresCardProduct, bool newCard,
                                       DateTime dateOfPurchase, int languageId)
        {
            log.LogMethodEntry(machineName, siteId, requiresCardProduct, newCard, dateOfPurchase,  languageId);
            this.siteId = siteId;
            this.dateOfPurchase = dateOfPurchase;
            this.languageId = languageId;
            this.requiresCardProduct = requiresCardProduct;
            this.newCard = newCard;
            this.languageCode = "";
            this.machineName = machineName;
            this.languageId = -1;
            this.cardNumber = "";
            this.loginId = "";
            this.productTypeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        /// used
        public ProductsFilterParams(string machineName, int siteId, bool requiresCardProduct, bool newCard,
                                       DateTime dateOfPurchase, string languageCode)
        {
            log.LogMethodEntry(machineName, siteId,  requiresCardProduct,  newCard,  dateOfPurchase,  languageCode);
            this.siteId = siteId;
            this.dateOfPurchase = dateOfPurchase;
            this.LanguageCode = languageCode;
            this.requiresCardProduct = requiresCardProduct;
            this.newCard = newCard;
            this.machineName = machineName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductsFilterParams(string machineName, int siteId, DateTime dateOfPurchase, int languageId,
                                        bool requiresCardProduct, bool newCard, string cardNumber, string loginId,
                                        string languageCode)
        {
            log.LogMethodEntry(machineName,  siteId,  dateOfPurchase,  languageId,
                                         requiresCardProduct,  newCard,  cardNumber,  loginId,
                                         languageCode);
            this.machineName = machineName;
            this.siteId = siteId;
            this.dateOfPurchase = dateOfPurchase;
            this.languageId = languageId;
            this.requiresCardProduct = requiresCardProduct;
            this.newCard = newCard;
            this.cardNumber = cardNumber;
            this.loginId = loginId;
            this.languageCode = languageCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MachineName field
        /// </summary>
        [DisplayName("MachineName")]
        [DefaultValue("")]
        public string MachineName { get { return machineName; } set { machineName = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the DateOfPurchase field
        /// </summary>
        [DisplayName("DateOfPurchase")]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime DateOfPurchase
        {
            get
            {
                if (dateOfPurchase.Year == 1)
                {
                    dateOfPurchase = DateTime.Now;
                }
                return dateOfPurchase;

            }
            set { dateOfPurchase = value; }
        }



        /// <summary>
        /// Get/Set method of the LanguageId field
        /// </summary>
        [DisplayName("LanguageId")]
        [DefaultValue(-1)]
        public int LanguageId { get { return languageId; } set { languageId = value; } }

        /// <summary>
        /// Get/Set method of the RequiresCardProduct field
        /// </summary>
        [DisplayName("RequiresCardProduct")]
        [DefaultValue(false)]
        public bool RequiresCardProduct { get { return requiresCardProduct; } set { requiresCardProduct = value; } }

        /// <summary>
        /// Get/Set method of the NewCard field
        /// </summary>
        [DisplayName("NewCard")]
        [DefaultValue(false)]
        public bool NewCard { get { return newCard; } set { newCard = value; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("CardNumber")]
        [DefaultValue("")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; } }

        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        [DisplayName("LoginId")]
        [DefaultValue("")]
        public string LoginId { get { return loginId; } set { loginId = value; } }


        /// <summary>
        /// Get/Set method of the LanguageCode field
        /// </summary>
        [DisplayName("LanguageCode")]
        [DefaultValue("")]
        public string LanguageCode { get { return languageCode; } set { languageCode = value; } }


        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        [DisplayName("ProductTypeExclude")]
        [DefaultValue("")]
        public string ProductTypeExclude { get { return productTypeExclude; } set { productTypeExclude = value; } }


        /// <summary>
        /// Get/Set method of the ProductDisplayGroups field
        /// </summary>
        [DisplayName("ProductDisplayGroups")]
        public List<string> ProductDisplayGroups { get { return productDisplayGroupsList; } set { productDisplayGroupsList = value; } }


        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        [DefaultValue(-1)]
        public int ProductId { get { return productId; } set { productId = value; } }


        /// <summary>
        /// Get/Set method of the ShowProductContents field
        /// </summary>
        [DisplayName("ShowProductContents")]
        [DefaultValue(false)]
        public bool ShowProductContents { get { return showProductContents; } set { showProductContents = value; } }


        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POSMachineId")]
        public int POSMachineId { get { return posMachineId; } set { posMachineId = value; } }
        /// <summary>
        /// Get/Set method of the POSTypeId field
        /// </summary>
        [DisplayName("POSTypeId")]
        public int POSTypeId { get { return posTypeId; } set { posTypeId = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        [DisplayName("DisplayGroupId")]
        public int DisplayGroupId { get { return displayGroupId; } set { displayGroupId = value; } }

        /// <summary>
        /// Get/Set method of the ProductType field
        /// </summary>
        [DisplayName("ProductType")]
        [DefaultValue("")]
        public string ProductType { get { return productType; } set { productType = value; } }

        /// <summary>
        /// Get/Set method of the ProductTypeId field
        /// </summary>
        [DisplayName("ProductTypeId")]
        public int ProductTypeId { get { return productTypeId; } set { productTypeId = value; } }

        /// <summary>
        /// Get/Set method of the DeviceType field
        /// </summary>
        [DisplayName("DeviceType")]
        public string DeviceType { get { return deviceType; } set { deviceType = value; } }


        /// <summary>
        /// Get/Set method of the ValidateCard field
        /// </summary>
        [DisplayName("ValidateCard")]
        public bool ValidateCard { get { return validateCard; } set { validateCard = value; } }

        /// <summary>
        /// Get/Set method of the FetchUpsellProduct field
        /// </summary>
        [DisplayName("FetchUpsellProduct")]
        public bool FetchUpsellProduct { get { return fetchUpsellProduct; } set { fetchUpsellProduct = value; } }

        // <summary>
        /// Get/Set method of the HsnSacCode field
        /// </summary>
        [DisplayName("HsnSacCode")]
        public string HsnSacCode { get { return hsnsacCode; } set { hsnsacCode = value; } }

        // <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        [DefaultValue(true)]
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }
    }

}
