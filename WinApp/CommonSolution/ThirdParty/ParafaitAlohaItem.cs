/********************************************************************************************
 * Class Name - Aloha check entry line                                                                          
 * Description - DTO object for the Parafait check entry line, to pass the item in the Aloha check
 * to the Parafait program
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015            Kiran          Created 
 *2.70.2        16-Jul-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// DTO class holding the Aloha item, used to pass the items in the Aloha check 
    /// to the Parafait programs 
    /// </summary>
    public class ParafaitAlohaItem
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private String itemId;
        private String checkId;
        private String displayName;
        private String quantity;
        private String units;
        private String entryId;
        private String tokens;
        private String changed;
        private String stored;

        private String mode;
        private String type;
        private String selected;
        private String period;
        private String modCode;
        private String revisionId;
        private String termId;
        private String origin;
        private String itemOrderTime;
        private String itemPrice;
        private int time;

        /// <summary>
        /// Constructor to initialize the DTO object
        /// </summary>
        public ParafaitAlohaItem(String entryIdentifier, String checkIdentifier, String itemIdentifier, String itemDisplayName, String itemQuantity, String itemUnits, String itemTokens, String itemChanged, String itemStored,
            String itemMode, String itemType, String itemSelected, String itemPeriod, String itemModCode, String itemRevisionId, String itemTermId,
            String itemOrigin, String itemTime, String itemPrice, int time)
        {
            log.LogMethodEntry();
            entryId = entryIdentifier;
            checkId = checkIdentifier;
            itemId = itemIdentifier;
            displayName = itemDisplayName;
            quantity = itemQuantity;
            units = itemUnits;
            tokens = itemTokens;
            changed = itemChanged;
            stored = itemStored;
            mode = itemMode;
            type = itemType;
            selected = itemSelected;
            period = itemPeriod;
            modCode = itemModCode;
            revisionId = itemRevisionId;
            termId = itemTermId;
            origin = itemOrigin;
            itemOrderTime = itemTime;
            this.itemPrice = itemPrice;
            this.time = time;
            log.LogMethodExit();
        }

        /// <summary>
        /// The line id in the check
        /// </summary>
        public int EntryId { get { return Convert.ToInt32(entryId); } }
        /// <summary>
        /// Aloha Check id
        /// </summary>
        public int CheckId { get { return Convert.ToInt32(checkId); } }
        /// <summary>
        /// Aloha Item id 
        /// </summary>
        public string ItemId { get { return itemId; } }
        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get { return displayName; } }
        /// <summary>
        /// Quantity
        /// </summary>
        public String Quantity { get { return quantity; } }
        /// <summary>
        /// Units
        /// </summary>
        public String Units { get { return units; } }
        /// <summary>
        /// Changed
        /// </summary>
        public String Changed { get { return changed; } }
        /// <summary>
        /// Stored
        /// </summary>
        public String Stored { get { return stored; } }
        /// <summary>
        /// Mode
        /// </summary>
        public String Mode { get { return mode; } }
        /// <summary>
        /// Type
        /// </summary>
        public String Type { get { return type; } }
        /// <summary>
        /// Selected
        /// </summary>
        public String Selected { get { return selected; } }
        /// <summary>
        /// Period
        /// </summary>
        public String Period { get { return period; } }
        /// <summary>
        /// Modifier code
        /// </summary>
        public String ModCode { get { return modCode; } }
        /// <summary>
        /// Revision Id
        /// </summary>
        public String RevisionId { get { return revisionId; } }
        /// <summary>
        /// Terminal from where the order line was put
        /// </summary>
        public String TermId { get { return termId; } }
        /// <summary>
        /// Origin
        /// </summary>
        public String Origin { get { return origin; } }
        /// <summary>
        /// Item order time
        /// </summary>
        public String ItemOrderTime { get { return itemOrderTime; } }
        /// <summary>
        /// Item price on the check
        /// </summary>
        public double Price
        {
            get
            {
                try
                {
                    return Convert.ToDouble(itemPrice);
                }
                catch (FormatException)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// The tokens associated with the product - Determines the number of credits to be loaded onto the card
        /// </summary>
        public int Tokens
        {
            get
            {
                try
                {
                    return Convert.ToInt32(tokens);
                }
                catch (FormatException)
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// The time in number of seconds - used for time card creation
        /// </summary>
        public int Time { get { return time; } }
    }
}
