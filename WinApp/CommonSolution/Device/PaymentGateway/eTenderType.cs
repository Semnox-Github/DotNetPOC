using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum eTenderType : ulong
    {

        /// <summary>
        /// 
        /// </summary>
        GENERIC_EFT = 0xFFFFFFFFFFFFFFFF,
        /// <summary>
        /// 
        /// </summary>
        DEBIT = 1,
        /// <summary>
        /// 
        /// </summary>
        CREDIT = 2,
        /// <summary>
        /// 
        /// </summary>
        EBT_FS = 4,
        /// <summary>
        /// 
        /// </summary>
        EBT_CA = 8,
        /// <summary>
        /// 
        /// </summary>
        PRIVATE_DEBIT = 16,
        /// <summary>
        /// 
        /// </summary>
        PRIVATE_CREDIT = 32,
        /// <summary>
        /// 
        /// </summary>
        USER_DEFINED1 = 64,
        /// <summary>
        /// 
        /// </summary>
        USER_DEFINED2 = 128,
        /// <summary>
        /// 
        /// </summary>
        CHECK = 256,
        /// <summary>
        /// 
        /// </summary>
        PIN_CHANGE = 512,
        /// <summary>
        /// 
        /// </summary>
        EBT_FS_BALANCE = 1024,
        /// <summary>
        /// 
        /// </summary>
        EBT_CA_BALANCE = 2048,
        /// <summary>
        /// 
        /// </summary>
        GIFT_CARD = 4096,
        /// <summary>
        /// 
        /// </summary>
        PHONE_CARD = 8192,
        /// <summary>
        /// 
        /// </summary>
        FLEET = 16384,
        /// <summary>
        /// 
        /// </summary>
        PREPAID_WIRELESS = 32768,
        /// <summary>
        /// 
        /// </summary>
        ACH = 65536,
        /// <summary>
        /// 
        /// </summary>
        EBT_GENERIC = 131072,
        /// <summary>
        /// 
        /// </summary>
        BIOMETRIC_SELF_CHECKOUT = 262144,
        /// <summary>
        /// 
        /// </summary>
        CONNECTPAY = 524288,
        /// <summary>
        /// 
        /// </summary>
        EWIC = 1048576,
    }
}
