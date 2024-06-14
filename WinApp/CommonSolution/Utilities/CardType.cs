/********************************************************************************************
 * Project Name - Device
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-Jul-2019      Lakshminarayana     Created 
 ********************************************************************************************/

namespace Semnox.Core.Utilities
{
    public enum CardType
    {
        /// <summary>
        /// Represents a mifare card
        /// </summary>
        MIFARE,
        /// <summary>
        /// Represents a mifare ultralight card
        /// </summary>
        MIFARE_ULTRA_LIGHT_C,
        /// <summary>
        /// Represents a unknown card type (LF, HF cards)
        /// </summary>
        UNKNOWN
    }
}
