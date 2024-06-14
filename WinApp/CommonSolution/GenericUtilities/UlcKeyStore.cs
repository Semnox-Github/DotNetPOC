/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - Represents collection of valid ULC keys
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      14-June-2019   Lakshminarayana      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents collection of valid ULC keys
    /// </summary>
    public class UlcKeyStore
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<UlcKey> defaultUltralightCKeys;
        private List<UlcKey> customerUltralightCKeys;
        private UlcKey latestCustomerUlcKey;
        private List<UlcKey> validUltralightCKeys;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="defaultKey"></param>
        public UlcKeyStore(byte[] defaultKey)
        {
            log.LogMethodEntry("defaultKey");
            if (UlcKey.IsValidKeyBytes(defaultKey))
            {
                defaultUltralightCKeys = new List<UlcKey>() { new UlcKey(defaultKey) };
                latestCustomerUlcKey = defaultUltralightCKeys.FirstOrDefault();
                customerUltralightCKeys = defaultUltralightCKeys;
                validUltralightCKeys = defaultUltralightCKeys;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="defaultKeys"></param>
        public UlcKeyStore(List<byte[]> defaultKeys)
        {
            log.LogMethodEntry("defaultKeys");
            defaultUltralightCKeys = new List<UlcKey>();
            foreach (byte[] defaultKey in defaultKeys)
            {
                if (UlcKey.IsValidKeyBytes(defaultKey))
                {
                    defaultUltralightCKeys.Add(new UlcKey(defaultKey));
                }
            }
            latestCustomerUlcKey = defaultUltralightCKeys.LastOrDefault();
            validUltralightCKeys = defaultUltralightCKeys;
            customerUltralightCKeys = defaultUltralightCKeys;
            log.LogMethodExit();
        }

        public UlcKeyStore(IEnumerable<MifareKeyContainerDTO> mifareKeyContainerDTOList)
        {
            log.LogMethodEntry("mifareKeyContainerDTOList");
            defaultUltralightCKeys = new List<UlcKey>();
	        foreach (MifareKeyContainerDTO mifareKeyContainerDTO in mifareKeyContainerDTOList)
	        {
                ByteArray byteArray = new ByteArray(mifareKeyContainerDTO.KeyString);
		        if (mifareKeyContainerDTO.Type == MifareKeyContainerDTO.MifareKeyType.ULTRA_LIGHT_C.ToString() && UlcKey.IsValidKeyBytes(byteArray.Value))
		        {
			        UlcKey ulcKey = new UlcKey(byteArray.Value);
			        defaultUltralightCKeys.Add(ulcKey);
                    if(mifareKeyContainerDTO.IsCurrent)
		            {
			            latestCustomerUlcKey = ulcKey;
		            }
		        }
	        }
	        validUltralightCKeys = defaultUltralightCKeys;
	        customerUltralightCKeys = defaultUltralightCKeys;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public UlcKeyStore()
        {
            log.LogMethodEntry();
            LoadUltralightCKeys();
            log.LogMethodExit();
        }

        private void LoadUltralightCKeys()
        {
            log.LogMethodEntry();
            defaultUltralightCKeys = GetUltraLightCKeysFromSystemOption("DefaultMifareUltraLightCKeys");
            customerUltralightCKeys = GetUltraLightCKeysFromSystemOption("CustomerMifareUltraLightCKey");
            latestCustomerUlcKey = customerUltralightCKeys.LastOrDefault();
            validUltralightCKeys = new List<UlcKey>();
            if(latestCustomerUlcKey != null) validUltralightCKeys.Add(latestCustomerUlcKey);
            validUltralightCKeys.AddRange(defaultUltralightCKeys);
            if (customerUltralightCKeys.Count > 1)
            {
                validUltralightCKeys.AddRange(customerUltralightCKeys.Take(customerUltralightCKeys.Count - 1));
            }

            log.LogMethodExit();
        }

        private static List<UlcKey> GetUltraLightCKeysFromSystemOption(string optionName)
        {
            log.LogMethodEntry(optionName);
            List<UlcKey> result = new List<UlcKey>();

            try
            {
                string pipeSeparatedUltralightCKeys = Encryption.GetParafaitKeys(optionName);
                string[] ultralightCKeyStrings =
                    pipeSeparatedUltralightCKeys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ultralightCKeyString in ultralightCKeyStrings)
                {
                    if (UlcKey.IsValidKeyString(ultralightCKeyString) == false)
                    {
                        log.Error("ultralightCKeyString is not a valid ultralight c key");
                        continue;
                    }
                    UlcKey ulcKey = new UlcKey(ultralightCKeyString);
                    result.Add(ulcKey);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieving default mifare ultralight c keys.", ex);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get method of validUltralightCKeys field
        /// </summary>
        public List<UlcKey> ValidUltralightCKeys
        {
            get { return validUltralightCKeys; }
        }

        /// <summary>
        /// Get method of latestCustomerUlcKey field
        /// </summary>
        public UlcKey LatestCustomerUlcKey
        {
            get { return latestCustomerUlcKey; }
        }

        /// <summary>
        /// Get method of customerUltralightCKeys field
        /// </summary>
        public List<UlcKey> CustomerUltralightCKeys
        {
            get { return customerUltralightCKeys; }
        }

        /// <summary>
        /// Get method of defaultUltralightCKeys field
        /// </summary>
        public List<UlcKey> DefaultUltralightCKeys
        {
            get { return defaultUltralightCKeys; }
        }
    }
}
