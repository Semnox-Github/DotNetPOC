/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the add product details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalAddProductDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Get/Set for amount
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Get/Set for ProductReference
        /// </summary>
        public string ProductReference { get; set; }
        /// <summary>
        /// Get/Set for CardNumber
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Get/Set for Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExternalAddProductDTO()
        {
            log.LogMethodEntry();
            Type = String.Empty; ;
            //Amount = String.Empty;
            ProductReference = String.Empty;
            CardNumber = String.Empty;
            Quantity = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public ExternalAddProductDTO(string type, int productId, decimal? amount, string productReference, string cardNumber, int quantity)
        {
            log.LogMethodEntry(type, productId, amount, productReference, cardNumber, quantity);
            this.Type = type;
            this.ProductId = productId;
            this.Amount = amount;
            this.ProductReference = productReference;
            this.CardNumber = cardNumber;
            this.Quantity = quantity;
            log.LogMethodExit();
        }
    }
}
