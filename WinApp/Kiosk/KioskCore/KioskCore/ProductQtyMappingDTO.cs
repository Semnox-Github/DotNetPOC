/********************************************************************************************
 * Project Name -  Semnox.Parafait.KioskCore
 * Description  - Custom DTO for Playground Entry
 * 
 **************
 **Version Log
 **************
 *Version       Date           Modified By            Remarks          
 ********************************************************************************************* 
 *2.150.0.0     21-Oct-2021    Sathyavathi            Created for Check-In feature Phase-2
 *2.150.1     22-Feb-2023      Guru S A               Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Linq;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskCore
{
    public partial class ProductQtyMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductsContainerDTO productsContainerDTO;
        private int quantity;
        private ProfileType productProfileType;

        public enum ProfileType
        {
            NOT_DEFINED = 0,
            CHILD = 1,
            ADULT = 2
        };

        public int Quantity { get { return quantity; } }
        public ProductsContainerDTO ProductsContainerDTO { get { return productsContainerDTO; } }
        public ProfileType ProductProfileType { get { return productProfileType; } }

        public ProductQtyMappingDTO(ProductsContainerDTO product, int qty)
        {
            log.LogMethodEntry(product, qty);
            try
            {
                this.productsContainerDTO = product;
                this.quantity = qty;
                SetProductAgeProfileType();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ProductQtyMappingDTO() : " + ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void SetProductAgeProfileType()
        {
            log.LogMethodEntry();
            try
            {
                decimal thresholdAgeOfChild = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "THRESHOLD_AGE_CHECK_IN_CHILD_SCREEN", -1);
                decimal thresholdAgeOfAdult = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "THRESHOLD_AGE_CHECK_IN_ADULT_SCREEN", -1);
                bool isIgnoreBirthYearSet = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR", false);
                string errMsg = string.Empty;

                if (productsContainerDTO.CustomerProfilingGroupId > -1 && isIgnoreBirthYearSet == true)
                {
                    errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4466)
                    + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5713); //Customer profiling and IGNORE_CUSTOMER_BIRTH_YEAR are both set
                    throw new Exception(errMsg);
                }

                if (productsContainerDTO.CustomerProfilingGroupId == -1 || isIgnoreBirthYearSet == true)
                {
                    productProfileType = ProfileType.NOT_DEFINED;
                    return;
                }

                if (productsContainerDTO.CustomerProfilingGroupId != -1
                    && thresholdAgeOfChild > 0
                    && productsContainerDTO.AgeUpperLimit <= thresholdAgeOfChild)
                {
                    productProfileType = ProfileType.CHILD;
                    return;
                }

                if (productsContainerDTO.CustomerProfilingGroupId != -1
                    && thresholdAgeOfAdult > 0
                    && productsContainerDTO.AgeUpperLimit >= thresholdAgeOfChild)
                {
                    productProfileType = ProfileType.ADULT;
                    return;
                }

                log.LogMethodExit();
                errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4466)
                    + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4471);
                throw new Exception(errMsg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in SetProductAgeProfileType(): " + ex);
                throw new Exception(ex.Message);
            }

        }
    }
}
