/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Data Transfer object of Mifare key
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     12-Nov-2020   Lakshminarayana     Created
 ********************************************************************************************/
using System;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// This is the Parafait Mifare Key data object class. This acts as data holder for the Parafait Mifare Key container object
    /// </summary>
    public class MifareKeyContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Different type of mifare keys
        /// </summary>
        public enum MifareKeyType
        {
            /// <summary>
            /// Key type classic
            /// </summary>
            CLASSIC,

            /// <summary>
            /// Key type Ultralight C
            /// </summary>
            ULTRA_LIGHT_C,
        }

        string type;
	    string keyString;
	    bool isCurrent;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MifareKeyContainerDTO()
        {
            log.LogMethodEntry();
            type =  string.Empty;
            keyString = string.Empty;
            isCurrent = false;
            log.LogMethodExit();
        }

        public MifareKeyContainerDTO(string type, string keyString, bool isCurrent)
        {
            log.LogMethodEntry(type, "keyString", isCurrent);
            this.type = type;
            this.keyString = keyString;
            this.isCurrent = isCurrent;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public string Type { get { return type; } set { type = value; } }

        /// <summary>
        /// Get/Set method of the KeyString field
        /// </summary>
        public string KeyString { get { return keyString; } set { keyString = value; } }

        /// <summary>
        /// Get/Set method of the IsCurrent field
        /// </summary>
        public bool IsCurrent { get { return isCurrent; } set { isCurrent = value; } }
    }
}
