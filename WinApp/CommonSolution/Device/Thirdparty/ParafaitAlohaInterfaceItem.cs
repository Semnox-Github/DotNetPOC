/********************************************************************************************
 * Class Name - ParafaitAlohaInterfaceItem                                                                          
 * Description - DTO object to handle the interfacing between the Parafait system and Aloha
 * The basic data of Aloha item is passed. This is used to add the items to the Aloha check
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015            Kiran          Created 
 *2.70.2        08-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// DTO class holding the Parafait Aloha interface item
    /// The item is then added to the Aloha check
    /// </summary>
    public class ParafaitAlohaInterfaceItem
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int itemId;
        private int quantity;
        private double price;
        private int modCode;
        private List<ParafaitAlohaInterfaceItem> modifierList;
        /// <summary>
        /// MODCODES
        /// </summary>
        public enum MODCODES
        {         
            // Mod codes
            /// <summary>
            /// Add
            /// </summary>
            ADD = 19,
            /// <summary>
            /// EXTRA
            /// </summary>
            EXTRA = 3,
            /// <summary>
            /// Heavy
            /// </summary>
            HEAVY = 15,
            /// <summary>
            /// Quarter
            /// </summary>
            QUARTER = 18,
            /// <summary>
            /// Only
            /// </summary>
            ONLY = 16,
            /// <summary>
            /// No
            /// </summary>
            NO = 2,
            /// <summary>
            /// Light
            /// </summary>
            LIGHT = 14,
            /// <summary>
            /// Side
            /// </summary>
            SIDE = 4,
            /// <summary>
            /// Half
            /// </summary>
            HALF = 17
        }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public ParafaitAlohaInterfaceItem()
        {
            log.LogMethodEntry();
            itemId = 0;
            quantity = 0;
            price = 0;
            modifierList = null;
            modCode = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the basic parameter
        /// </summary>
        public ParafaitAlohaInterfaceItem(int itemId, int quantity, double price)
        {
            log.LogMethodEntry(itemId, quantity, price);
            this.itemId = itemId;
            this.quantity = quantity;
            this.price = price;
            this.modifierList = null;
            this.modCode = (int)MODCODES.ADD;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the basic parameter
        /// </summary>
        public ParafaitAlohaInterfaceItem(int itemId, int quantity, double price, int modCode)
            : this(itemId, quantity, price)
        {
            log.LogMethodEntry(itemId, quantity, price, modCode);
            this.modCode = modCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the modifier list (which is again a list of ParafaitAlohaInterfaceItem)
        /// </summary>
        public ParafaitAlohaInterfaceItem(int itemId, int quantity, double price, List<ParafaitAlohaInterfaceItem> modifierList)
            : this(itemId, quantity, price)
        {
            log.LogMethodEntry(itemId, quantity, price, modifierList);
            this.modifierList = modifierList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the modifier list (which is again a list of ParafaitAlohaInterfaceItem)
        /// </summary>
        public ParafaitAlohaInterfaceItem(int itemId, int quantity, double price, int modCode, List<ParafaitAlohaInterfaceItem> modifierList)
            : this(itemId, quantity, price, modCode)
        {
            log.LogMethodEntry(itemId, quantity, price, modCode, modifierList);
            this.modifierList = modifierList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets/Sets the Aloha Item id
        /// </summary>
        public int ItemId { get { return itemId; } set { itemId = value; } }
        /// <summary>
        /// Gets/Sets the quantity
        /// </summary>
        public int Quantity { get { return quantity; } set { quantity = value; } }
        /// <summary>
        /// Gets/Sets the price
        /// </summary>
        public double Price { get { return price; } set { price = value; } }
        /// <summary>
        /// Gets/Sets the price
        /// </summary>
        public int ModCode { get { return modCode; } set { modCode = value; } }
        /// <summary>
        /// Gets/Sets the modifier list - which contains the modifiers to the base item
        /// </summary>
        public List<ParafaitAlohaInterfaceItem> ModifierList { get { return modifierList; } set { modifierList = value; } }
    }
}
