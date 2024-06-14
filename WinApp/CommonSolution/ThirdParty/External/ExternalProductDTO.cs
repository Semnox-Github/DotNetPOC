/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the External Product details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Ashish Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalProductDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Get/Set for ProductGroups
        /// </summary>
        public List<int> ProductGroups { get; set; }
        /// <summary>
        /// Get/Set for Price
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Get/Set for TaxId
        /// </summary>

        public int TaxId { get; set; }
        /// <summary>
        /// Get/Set for FaceValue
        /// </summary>
        public int FaceValue { get; set; }
        /// <summary>
        /// Get/Set for AvailableUnits
        /// </summary>
        public int AvailableUnits { get; set; }
        /// <summary>
        /// Get/Set for Minimumquantity
        /// </summary>
        public int MinimumQuantity { get; set; }
        /// <summary>
        /// Get/Set for TaxPercentage
        /// </summary>
        public int TaxPercentage { get; set; }
        /// <summary>
        /// Get/Set for IsRecommended
        /// </summary>
        public bool IsRecommended { get; set; }
        /// <summary>
        /// Get/Set for OnlyForVIP
        /// </summary>
        public bool OnlyForVIP { get; set; }
        /// <summary>
        /// Get/Set for AllowPriceOverride
        /// </summary>
        public bool AllowPriceOverride { get; set; }
        /// <summary>
        /// Get/Set for RegisteredCustomerOnly
        /// </summary>
        public bool RegisteredCustomerOnly { get; set; }
        /// <summary>
        /// Get/Set for ManagerApprovalRequired
        /// </summary>
        public bool ManagerApprovalRequired { get; set; }
        /// <summary>
        /// Get/Set for DisplayInPOS
        /// </summary>
        public bool DisplayInPOS { get; set; }
        /// <summary>
        /// Get/Set for MaxQtyPerDay
        /// </summary>
        public string MaxQtyPerDay { get; set; }
        /// <summary>
        /// Get/Set for ExternalSystemReference
        /// </summary>
        public string ExternalSystemReference { get; set; }
        /// <summary>
        /// Get/Set for ShortName
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Get/Set for ProductType
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// Get/Set for CategoryName
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// Get/Set for ProductName
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Get/Set for Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Get/Set for WebDescription
        /// </summary>
        public string WebDescription { get; set; }
        /// <summary>
        /// Get/Set for TaxInclusivePrice
        /// </summary>
        public string TaxInclusivePrice { get; set; }
        /// <summary>
        /// Get/Set for ExpiryDate
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExternalProductDTO()
        {
            log.LogMethodEntry();
            SiteId = -1;
            ProductName = string.Empty;
            Price = -1;
            TaxId = -1;
            FaceValue = -1;
            TaxInclusivePrice = string.Empty;
            ExpiryDate = string.Empty;
            AvailableUnits = -1;
            RegisteredCustomerOnly = true;
            MinimumQuantity = -1;
            DisplayInPOS = true;
            ProductType = string.Empty;
            CategoryName = string.Empty;
            TaxPercentage = -1;
            MaxQtyPerDay = string.Empty;
            ExternalSystemReference = string.Empty;
            ShortName = string.Empty;
            IsRecommended = true;
            ProductGroups = new List<int>();
            log.LogMethodExit();

        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ExternalProductDTO(int productId, List<int> productGroups, string productName, double price, int taxId, int FaceValue, string taxInclusivePrice,
                                  string expiryDate, int availableUnits, string description, string webDescription, bool onlyForVIP,
                                  bool allowPriceOverride, bool registeredCustomerOnly, bool managerApprovalRequired, int minimumQuantity,
                                  bool displayInPOS, string productType, string categoryName, int taxPercentage, string maxQtyPerDay,
                                  string externalSystemReference, string shortName, bool isRecommended, int siteId)
        {
            log.LogMethodEntry(productId, productGroups, productName, price, taxId, FaceValue, taxInclusivePrice, expiryDate, availableUnits, description,
                               webDescription, onlyForVIP, allowPriceOverride, registeredCustomerOnly, managerApprovalRequired, minimumQuantity,
                               displayInPOS, productType, categoryName, taxPercentage, maxQtyPerDay, externalSystemReference,
                               shortName, isRecommended, siteId);

            this.ProductId = productId;
            this.ProductGroups = productGroups;
            this.IsRecommended = isRecommended;
            this.ShortName = shortName;
            this.ExternalSystemReference = externalSystemReference;
            this.MaxQtyPerDay = maxQtyPerDay;
            this.TaxPercentage = taxPercentage;
            this.CategoryName = categoryName;
            this.ProductType = productType;
            this.DisplayInPOS = displayInPOS;
            this.MinimumQuantity = minimumQuantity;
            this.ManagerApprovalRequired = managerApprovalRequired;
            this.RegisteredCustomerOnly = registeredCustomerOnly;
            this.OnlyForVIP = onlyForVIP;
            this.AllowPriceOverride = allowPriceOverride;
            this.Description = description;
            this.WebDescription = webDescription;
            this.AvailableUnits = availableUnits;
            this.ExpiryDate = expiryDate;
            this.TaxInclusivePrice = taxInclusivePrice;
            this.FaceValue = FaceValue;
            this.TaxId = taxId;
            this.Price = price;
            this.ProductName = productName;
            this.SiteId = siteId;
            log.LogMethodExit();
        }
    }
}
