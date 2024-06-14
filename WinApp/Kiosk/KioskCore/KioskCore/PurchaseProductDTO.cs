/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - Custom DTO for Playground Entry
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.0.0    21-Oct-2021   Sathyavathi             Created for Check-In feature Phase-2
 *2.150.1      22-Feb-2023   Guru S A                Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Linq;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.KioskCore
{
    public partial class PurchaseProductDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string parentCardNumber;
        private int parentCustomerId;
        private List<ProductQtyMappingDTO> productQtyMappingDTOs;
        internal const int AGE_LOWER_LIMIT = 0;  //as per productContainerDTO definition
        internal const int AGE_UPPER_LIMIT = 999;  //as per productContainerDTO definition

        public string ParentCardNumber { get { return parentCardNumber; } }
        public int ParentCustomerId { get { return parentCustomerId; }}
        public List<ProductQtyMappingDTO> ProductQtyMappingDTOs { get { return productQtyMappingDTOs; } }

        public PurchaseProductDTO(List<ProductsContainerDTO> childProducts, Dictionary<int, int> productQtyKeyValuePairs, string cardNumber, int customerId)
        {
            log.LogMethodEntry(childProducts, productQtyKeyValuePairs, cardNumber, customerId);
            try
            {
                this.parentCardNumber = cardNumber;
                this.parentCustomerId = customerId;
                BuildProductQtyMappingDTOs(childProducts, productQtyKeyValuePairs);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in PurchaseProductDTO(): " + ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void BuildProductQtyMappingDTOs(List<ProductsContainerDTO> childProducts, Dictionary<int, int> productQtyKeyValuePairs)
        {
            log.LogMethodEntry();
            try
            {
                productQtyMappingDTOs = new List<ProductQtyMappingDTO>();
                foreach (ProductsContainerDTO productContainerDTO in childProducts)
                {
                    if (productQtyKeyValuePairs.ContainsKey(productContainerDTO.ProductId))
                    {
                        ProductQtyMappingDTO qtyMappingDTO = new ProductQtyMappingDTO(productContainerDTO, productQtyKeyValuePairs[productContainerDTO.ProductId]);
                        if (qtyMappingDTO.Quantity > 0)  //check is added so that we don't callup child/adult screens when it is never opted.
                        {                                //Also checkin summary should showup only the products purchased.
                            productQtyMappingDTOs.Add(qtyMappingDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in BuildProductQtyMappingDTOs of PurchaseProductDTO(): " + ex);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
