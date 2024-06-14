/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Container class, builds and caches the mifare keys used for reading and writing mifare tags
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Class holds the mifare keys.
    /// </summary>
    internal class MifareKeyContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MifareKeyContainerDTOCollection mifareKeyContainerDTOCollection;
        private readonly int siteId;
        private readonly DateTime? mifareKeyModuleLastUpdateTime;

        internal MifareKeyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            List<MifareKeyContainerDTO> mifareKeyContainerDTOList = new List<MifareKeyContainerDTO>();
            try
            {
                MifareKeyContainerDataHandler mifareKeyContainerDataHandler = new MifareKeyContainerDataHandler();
                mifareKeyModuleLastUpdateTime = mifareKeyContainerDataHandler.GetMifareKeyModuleLastUpdateTime(siteId);
                List<MifareKeyContainerDTO> standardMifareKeyContainerDTOList = GetStandardMifareKeyContainerDTOList();
                MifareKeyContainerDTO customerMifareKeyContainerDTO = GetCustomerMifareKeyContainerDTO();
                if (ParafaitDefaultContainerList.GetParafaitDefault(siteId, "MIFARE_CARD") == "Y")
                    standardMifareKeyContainerDTOList[0].IsCurrent = true;
                else
                    customerMifareKeyContainerDTO.IsCurrent = true;

                List<MifareKeyContainerDTO> defaultMifareUlcKeys = GetDefaultMifareUlcKeys();
                List<MifareKeyContainerDTO> customerMifareUlcKeys = GetCustomerMifareUlcKeys();
                MifareKeyContainerDTO currentMifareUltraLightKey;
                if (customerMifareUlcKeys.Any())
                {
                    currentMifareUltraLightKey = customerMifareUlcKeys.LastOrDefault();
                }
                else
                {
                    currentMifareUltraLightKey = defaultMifareUlcKeys.LastOrDefault();
                }
                if (currentMifareUltraLightKey != null)
                {
                    currentMifareUltraLightKey.IsCurrent = true;
                }

                mifareKeyContainerDTOList.AddRange(standardMifareKeyContainerDTOList);
                mifareKeyContainerDTOList.Add(customerMifareKeyContainerDTO);
                mifareKeyContainerDTOList.AddRange(defaultMifareUlcKeys);
                mifareKeyContainerDTOList.AddRange(customerMifareUlcKeys);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the parafait default container.", ex);
                mifareKeyModuleLastUpdateTime = null;
                mifareKeyContainerDTOList.Clear();
            }

            mifareKeyContainerDTOCollection = new MifareKeyContainerDTOCollection(mifareKeyContainerDTOList);
            log.LogMethodExit();
        }

        private MifareKeyContainerDTO GetCustomerMifareKeyContainerDTO()
        {
            string customerKeyString = SiteContainerList.GetCustomerKey(siteId);
            int customerKey;
            if (int.TryParse(customerKeyString, out customerKey) == false)
            {
                log.Error("Invalid customer key: " + customerKeyString);
                customerKey = -1;
            }
            byte[] customerAuthKey = new byte[6];
            string customerAuthenticationKeyString = GetCustomerAuthenticationKeyString();
            for (int i = 0; i < 5; i++)
                customerAuthKey[i] = Convert.ToByte(customerAuthenticationKeyString[i]);
            customerAuthKey[5] = Convert.ToByte(customerKey);
            MifareKeyContainerDTO customerMifareKeyContainerDTO = new MifareKeyContainerDTO(MifareKeyContainerDTO.MifareKeyType.CLASSIC.ToString(),
                                                                                                            (new ByteArray(customerAuthKey)).ToString(),
                                                                                                            false);
            return customerMifareKeyContainerDTO;
        }

        private List<MifareKeyContainerDTO> GetStandardMifareKeyContainerDTOList()
        {
            List<MifareKeyContainerDTO> standardMifareKeyContainerDTOList = new List<MifareKeyContainerDTO>();
            string standardAuthenticationKeysString = GetStandardAuthenticationKeysString();
            if (string.IsNullOrWhiteSpace(standardAuthenticationKeysString) == false)
            {
                string[] standardAuthenticationKeysStringArray = standardAuthenticationKeysString.Split('|');
                foreach (var standardAuthenticationKey in standardAuthenticationKeysStringArray)
                {
                    try
                    {
                        string key = Encryption.Decrypt(standardAuthenticationKey);
                        if (key.Length > 17)
                        {
                            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                            DateTime expiryDate = DateTime.ParseExact(key.Substring(17), "dd-MMM-yyyy", provider);
                            if (expiryDate < DateTime.Now.Date)
                                continue;
                        }

                        if (key.Length > 0)
                        {
                            byte[] bytes = new byte[6];
                            string[] sa = key.Substring(0, 17).Split('-');
                            int i = 0;
                            foreach (string s in sa)
                            {
                                bytes[i++] = Convert.ToByte(s, 16);
                            }
                            ByteArray mifareKey = new ByteArray(bytes);
                            MifareKeyContainerDTO mifareKeyContainerDTO = new MifareKeyContainerDTO(MifareKeyContainerDTO.MifareKeyType.CLASSIC.ToString(),
                                                                                                                            mifareKey.ToString(), false);
                            standardMifareKeyContainerDTOList.Add(mifareKeyContainerDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred  while executing getAuthKeyList", ex);
                    }
                }
            }

            return standardMifareKeyContainerDTOList;
        }

        private string GetCustomerAuthenticationKeyString()
        {
            string customerAuthenticationKeyString = SystemOptionContainerList.GetSystemOption(siteId, "Parafait Keys", "NonMifareAuthorization");
            return customerAuthenticationKeyString;
        }

        private string GetStandardAuthenticationKeysString()
        {
            log.LogMethodEntry();
            string standardAuthenticationKeysString = SystemOptionContainerList.GetSystemOption(siteId, "Parafait Keys", "MifareAuthorization");
            ProductKeyListBL productKeyListBL = new ProductKeyListBL();
            List<KeyValuePair<ProductKeyDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<ProductKeyDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<ProductKeyDTO.SearchByParameters, string>(ProductKeyDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<ProductKeyDTO> productKeyDTOList = productKeyListBL.GetProductKeyDTOList(searchParameter);
            if (productKeyDTOList.Any() && productKeyDTOList[0].AuthKey != null)
            {
                standardAuthenticationKeysString += "|" + Encoding.Default.GetString(productKeyDTOList[0].AuthKey);
            }
            log.LogMethodExit("standardAuthenticationKeysString");
            return standardAuthenticationKeysString;
        }

        private List<MifareKeyContainerDTO> GetCustomerMifareUlcKeys()
        {
            log.LogMethodEntry();
            string pipeSeparetedCustomerMifareUlcKeyString = SystemOptionContainerList.GetSystemOption(siteId, "Parafait Keys", "CustomerMifareUltraLightCKey");
            List<MifareKeyContainerDTO> result = GetUltraLightCKeysFromPipeSeparetedString(pipeSeparetedCustomerMifareUlcKeyString);
            log.LogMethodExit("result");
            return result;
        }

        private List<MifareKeyContainerDTO> GetDefaultMifareUlcKeys()
        {
            log.LogMethodEntry();
            string pipeSeparetedDefaultMifareUlcKeyString = SystemOptionContainerList.GetSystemOption(siteId, "Parafait Keys", "DefaultMifareUltraLightCKeys");
            List<MifareKeyContainerDTO> result = GetUltraLightCKeysFromPipeSeparetedString(pipeSeparetedDefaultMifareUlcKeyString);
            log.LogMethodExit("result");
            return result;
        }

        private List<MifareKeyContainerDTO> GetUltraLightCKeysFromPipeSeparetedString(string pipeSeparatedUltralightCKeys)
        {
            log.LogMethodEntry("pipeSeparatedUltralightCKeys");
            List<MifareKeyContainerDTO> result = new List<MifareKeyContainerDTO>();
            if (string.IsNullOrWhiteSpace(pipeSeparatedUltralightCKeys) == false)
            {
                log.LogMethodExit(result, "pipeSeparatedUltralightCKeys is empty");
            }
            try
            {
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
                    MifareKeyContainerDTO mifareKeyContainerDTO = new MifareKeyContainerDTO(MifareKeyContainerDTO.MifareKeyType.ULTRA_LIGHT_C.ToString(), ulcKey.ToString(), false);
                    result.Add(mifareKeyContainerDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving default mifare ultralight c keys.", ex);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the mifare keys
        /// </summary>
        /// <returns></returns>
        public MifareKeyContainerDTOCollection GetMifareKeyContainerDTOCollection()
        {
            log.LogMethodEntry();
            MifareKeyContainerDTOCollection result = mifareKeyContainerDTOCollection;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Refreshes the container
        /// </summary>
        /// <returns></returns>
        public MifareKeyContainer Refresh()
        {
            log.LogMethodEntry();
            MifareKeyContainerDataHandler mifareKeyContainerDataHandler = new MifareKeyContainerDataHandler();
            DateTime? updateTime = mifareKeyContainerDataHandler.GetMifareKeyModuleLastUpdateTime(siteId);
            if (mifareKeyModuleLastUpdateTime.HasValue
                && mifareKeyModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in discount module since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            MifareKeyContainer result = new MifareKeyContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
