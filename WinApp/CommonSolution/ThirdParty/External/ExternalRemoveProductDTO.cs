/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the remove product details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalRemoveProductDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for amount
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// Get/Set for ProductReference
        /// </summary>
        public string ProductReference { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalRemoveProductDTO()
        {
            log.LogMethodEntry();
            Type = String.Empty;
            Amount = String.Empty;
            ProductReference = String.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Parameter
        /// </summary>
        public ExternalRemoveProductDTO(string type, string amount, string productReference)
        {
            log.LogMethodEntry(type, amount, productReference);
            this.Type = type;
            this.Amount = amount;
            this.ProductReference = productReference;
            log.LogMethodExit();
        }
    }

}
