/********************************************************************************************
 * Project Name - UpsellOffers DTO
 * Description  - Data object of UpsellOffers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Jan-2017   Amaresh          Created 
 ********************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  This is the UpsellOfferProducts data object class. This acts as data holder for the UpsellOfferProducts object
    /// </summary>  
    public class UpsellOfferProductsDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        int productId;
        string productName;
        string offerMessage;
        double price;
        string description;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UpsellOfferProductsDTO()
        {
            log.Debug("Starts-UpsellOfferProductsDTO() default constructor.");
            productId = -1;
            log.Debug("Starts-UpsellOfferProductsDTO() default constructor.");
        }
       
        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="offerMessage"></param>
        /// <param name="price"></param>
        /// <param name="description"></param>
        public UpsellOfferProductsDTO(int productId, string productName, string offerMessage, double price, string description)
        {
            log.Debug("Starts-UpsellOfferProductsDTO() argument constructor.");
            this.productId = productId;
            this.productName = productName;
            this.offerMessage = offerMessage;
            this.price = price;
            this.description = description;
            log.Debug("Starts-UpsellOfferProductsDTO() argument constructor.");
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        [DisplayName("ProductName")]
        public string ProductName { get { return productName; } set { productName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OfferMessage field
        /// </summary>
        [DisplayName("OfferMessage")]
        public string OfferMessage { get { return offerMessage; } set { offerMessage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }
    }
}
