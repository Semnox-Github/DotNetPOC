/********************************************************************************************
 * Project Name - Utilities
 * Description  - Sub Types of devices
 * 
 **************
 **Version Log
 **************
 *Version        Date            Modified By         Remarks          
 *********************************************************************************************
 *2.110.0        1-Jul-2019      Lakshminarayana     Created 
 ********************************************************************************************/

namespace Semnox.Core.Utilities
{
    public enum DeviceSubType
    {
        /// <summary>
        /// ACR122U mifare card reader
        /// </summary>
        ACR122U,
        /// <summary>
        /// ACR1252U mifare card reader
        /// </summary>
        ACR1252U,
        /// <summary>
        /// KeyboardWedge card and barcode reader
        /// </summary>
        KeyboardWedge,
        /// <summary>
        /// ACR1222L mifare card reader
        /// </summary>
        ACR1222L,
        /// <summary>
        /// MIBlack mifare card reader
        /// </summary>
        MIBlack,
        /// <summary>
        /// Wacom waiver device
        /// </summary>
        Wacom
    }
}
