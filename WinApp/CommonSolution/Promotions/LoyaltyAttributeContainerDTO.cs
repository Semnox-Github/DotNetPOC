/********************************************************************************************
 * Project Name - Achievements
 * Description  - LoyaltyAttributeContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
   public class LoyaltyAttributeContainerDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int loyaltyAttributeId;
        private string attribute;
        private string purchaseApplicable;
        private string consumptionApplicable;
        private string dBColumnName;

        public LoyaltyAttributeContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        /// 
        public LoyaltyAttributeContainerDTO(int loyaltyAttributeIdPassed, string attributePassed, string purchaseApplicablePassed, string consumptionApplicablePassed, string dBColumnNamePassed) : this()
        {
            log.LogMethodEntry(loyaltyAttributeIdPassed,attributePassed,purchaseApplicablePassed,consumptionApplicablePassed,dBColumnNamePassed);
            this.loyaltyAttributeId = loyaltyAttributeIdPassed;
            this.attribute = attributePassed;
            this.purchaseApplicable = purchaseApplicablePassed;
            this.consumptionApplicable = consumptionApplicablePassed;
            this.dBColumnName = dBColumnNamePassed;

            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("LoyaltyAttributeId")]
        [ReadOnly(true)]
        public int LoyaltyAttributeId { get { return loyaltyAttributeId; } set { loyaltyAttributeId = value; } }
        /// <summary>
        /// Get/Set method of the Attribute field
        /// </summary>
        [DisplayName("Attribute")]
        [ReadOnly(true)]
        public string Attribute { get { return attribute; } set { attribute = value; } }
        /// <summary>
        /// Get/Set method of PurchaseApplicable field
        /// </summary>
        [DisplayName("PurchaseApplicable")]
        [ReadOnly(true)]
        public string PurchaseApplicable { get { return purchaseApplicable; } set { purchaseApplicable = value; } }
        /// <summary>
        /// Get/Set method of ConsumptionApplicable field
        /// </summary>
        [DisplayName("ConsumptionApplicable")]
        [ReadOnly(true)]
        public string ConsumptionApplicable { get { return consumptionApplicable; } set { consumptionApplicable = value; } }
        /// <summary>
        /// Get/Set method of DBColumnName field
        /// </summary>
        [DisplayName("DBColumnName")]
        [ReadOnly(true)]
        public string DBColumnName { get { return dBColumnName; } set { dBColumnName = value; } }

    }
}
