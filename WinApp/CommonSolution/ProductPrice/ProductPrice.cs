/********************************************************************************************
 * Project Name - ProductPrice
 * Description  - Represents price of the product.
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     21-Mar-2021      Lakshminarayana     Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System.Collections.Generic;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents price of the product.
    /// </summary>
    public class ProductPrice : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string priceType;
        private readonly decimal price;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductPrice(string priceType, decimal price)
        {
            log.LogMethodEntry(priceType, price);
            this.priceType = priceType;
            this.price = price;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the individual components of the product price
        /// </summary>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return priceType;
            yield return price;
        }

        /// <summary>
        /// Get method of price type
        /// </summary>
        public string PriceType
        {
            get
            {
                return priceType;
            }
        }

        /// <summary>
        /// Get method of price
        /// </summary>
        public decimal Price
        {
            get
            {
                return price;
            }
        }
    }
}
