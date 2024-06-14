/********************************************************************************************
 * Class Name - Aloha Item                                                                          
 * Description - DTO object for the Parafait Aloha item, to pass the item in the Aloha check
 * to the Parafait program
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015            Kiran          Created 
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// Aloha Item DTO class - To pass the Aloha item details across 
    /// </summary>
    [Serializable]
    public class AlohaItemDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int itemId;
        private string displayName;
        private string itemPrice;
        private string taxId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AlohaItemDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor will all the fields
        /// </summary>
        public AlohaItemDTO(int itemId, string displayName, string itemPrice, string taxId)
        {
            log.LogMethodEntry(itemId, displayName, itemPrice, taxId);
            this.itemId = itemId;
            this.displayName = displayName;
            this.itemPrice = itemPrice;
            this.taxId = taxId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Aloha item id
        /// </summary>
        public int ItemId { get { return itemId; } }

        /// <summary>
        /// Gets the Aloha item name
        /// </summary>
        public string DisplayName { get { return displayName; } }

        /// <summary>
        /// Gets the Aloha item price
        /// </summary>
        public string ItemPrice { get { return itemPrice; } }

        /// <summary>
        /// Gets the Aloha tax id
        /// </summary>
        public string TaxId { get { return taxId; } }
    }
}
