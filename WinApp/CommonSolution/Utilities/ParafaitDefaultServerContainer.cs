/********************************************************************************************
 * Project Name - Utilities
 * Description  - ParafaitDefaultContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Core.Utilities.Sample
{
    /// <summary>
    /// Class holds the parafait default values.
    /// </summary>
    public class ParafaitDefaultContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<string, string> constantDefaultValueDictionary = new Dictionary<string, string>
        {
            {"DATE_FORMAT ", "dd-MMM-yyyy"},
            {"CARD_FACE_VALUE", "0" },
            {"DATETIME_FORMAT", "dd-MMM-yyyy h:mm tt" },
            {"NUMBER_FORMAT", "N0" },
            {"AMOUNT_FORMAT", "N2" },
            {"INVENTORY_QUANTITY_FORMAT", "N3" },
            {"INVENTORY_COST_FORMAT", "N3" },
            {"CURRENCY_CODE", "INR" },
            {"ALLOW_ROAMING_CARDS", "N" },
            {"ENABLE_ON_DEMAND_ROAMING", "N" },
            {"AUTOMATIC_ON_DEMAND_ROAMING", "N" },
            {"ALLOW_REFUND_OF_CARD_DEPOSIT", "Y" },
            {"ALLOW_PARTIAL_REFUND", "N" },
            {"ALLOW_REFUND_OF_CARD_CREDITS", "Y" },
            {"ALLOW_REFUND_OF_CREDITPLUS", "Y" },
            {"CURRENCY_SYMBOL", "Rs" },
            {"TOKEN_PRICE", "1" },
            {"DEFAULT_FONT", "Arial" },
            {"DEFAULT_FONT_SIZE", "10" },
            {"DEFAULT_GRID_FONT", "Tahoma" },
            {"DEFAULT_GRID_FONT_SIZE", "8" },
            {"POS_SKIN_COLOR", "Gray" },
            {"REAL_TICKET_MODE", "N" },
            {"CARD_VALIDITY", "12" },
            {"CARD_NUMBER_LENGTH", "10" },
            {"DEFAULT_PAY_MODE", "0" },
            {"POS_QUANTITY_DECIMALS", "0" },
            {"PREFERRED_NON-CASH_PAYMENT_MODE", "3" },
            {"MINIMUM_SPEND_FOR_VIP_STATUS", "0" },
            {"MINIMUM_RECHARGE_FOR_VIP_STATUS", "0" },
            {"VIP_POS_ALERT_RECHARGE_THRESHOLD", "40000" },
            {"VIP_POS_ALERT_SPEND_THRESHOLD", "40000" },
            {"ALLOW_TRANSACTION_ON_ZERO_STOCK", "N" },
            {"HIDE_SHIFT_OPEN_CLOSE", "N" },
            {"REGISTRATION_MANDATORY_FOR_VIP", "N" },
            {"UNIQUE_ID_MANDATORY_FOR_VIP", "N" },
            {"REGISTRATION_MANDATORY_FOR_MEMBERSHIP", "N" },
            {"ALLOW_DUPLICATE_UNIQUE_ID", "N" },
            {"CREDITCARD_DETAILS_MANDATORY", "N" },
            {"LEFT_TRIM_BARCODE", "0" },
            {"RIGHT_TRIM_BARCODE", "0" },
            {"LEFT_TRIM_CARD_NUMBER", "0" },
            {"RIGHT_TRIM_CARD_NUMBER", "0" },
            {"PRINTER_PAGE_LEFT_MARGIN", "10" },
            {"PRINTER_PAGE_RIGHT_MARGIN", "10" },
            {"LOAD_BONUS_EXPIRY_DAYS", "0" },
            {"AUTO_EXTEND_BONUS_ON_RELOAD", "N" },
            {"ALLOW_MANUAL_CARD_IN_REDEMPTION", "N" },
            {"CARD_MANDATORY_FOR_TRANSACTION", "N" },
            {"USE_ORIGINAL_TRXNO_FOR_REFUND", "N" },
            {"ALLOW_REDEMPTION_WITHOUT_CARD", "N" },
            {"LOAD_BONUS_LIMIT", "10" },
            {"LOAD_TICKETS_LIMIT", "50" },
            {"TRANSACTION_AMOUNT_LIMIT", "50" },
            {"REVERSE_DESKTOP_CARD_NUMBER", "N" },
            {"ROUNDOFF_PAYMENTMODE_ID", "-1" },
            {"MAX_TOKEN_NUMBER", "1000" },
            {"AUTO_CHECK_IN_POS", "N" },
            {"DEFAULT_LANGUAGE", "-9" }
        };

        private readonly Dictionary<string, string> siteParafaitDefaults = new Dictionary<string, string>();
        private readonly Dictionary<string, string> overridenParafaitDefaults = new Dictionary<string, string>(); //[Key = DefaultValueName + "USERID" + userId || Key = DefaultValueName + "POSMACHINEID" + machineId]
        private readonly Dictionary<string, ParafaitDefaultsDTO> parafaitDefaultValueNameParafaitDefaultsDTODictionary = new Dictionary<string, ParafaitDefaultsDTO>();
        private readonly List<ParafaitDefaultsDTO> parafaitDefaultsDTOList;
        private readonly DateTime? parafaitDefaultModuleLastUpdateTime;
        private readonly int encryptedPasswordDataTypeId;
        private readonly int siteId;

        public ParafaitDefaultContainer(int siteId) : this(siteId, GetParafaitDefaultsDTOList(siteId), GetParafaitDefaultsModuleLastUpdateTime(siteId), GetEncryptedPasswordDataTypeId(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        public ParafaitDefaultContainer(int siteId, List<ParafaitDefaultsDTO> parafaitDefaultsDTOList, DateTime? parafaitDefaultsModuleLastUpdateTime, int encryptedPasswordDataTypeId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.parafaitDefaultsDTOList = parafaitDefaultsDTOList;
            this.parafaitDefaultModuleLastUpdateTime = parafaitDefaultsModuleLastUpdateTime;
            this.encryptedPasswordDataTypeId = encryptedPasswordDataTypeId;
            if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Any())
            {
                foreach (ParafaitDefaultsDTO parafaitDefaultsDTO in parafaitDefaultsDTOList)
                {
                    if (parafaitDefaultValueNameParafaitDefaultsDTODictionary.ContainsKey(parafaitDefaultsDTO.DefaultValueName) == false)
                    {
                        parafaitDefaultValueNameParafaitDefaultsDTODictionary.Add(parafaitDefaultsDTO.DefaultValueName, parafaitDefaultsDTO);
                    }
                    AddToSiteDictionary(parafaitDefaultsDTO);
                    if (parafaitDefaultsDTO.ParafaitOptionValuesDTOList != null && // checks for whether default is overridden
                        parafaitDefaultsDTO.ParafaitOptionValuesDTOList.Any())
                    {
                        AddToOverridenDictionary(parafaitDefaultsDTO);
                    }
                }
                AssignComputedValues();
            }
            log.LogMethodExit();
        }

        

        private void AddToSiteDictionary(ParafaitDefaultsDTO parafaitDefaultsDTO)
        {
            log.LogMethodEntry(parafaitDefaultsDTO);
            string defaultValue = string.Empty;
            string defaultKey = parafaitDefaultsDTO.DefaultValueName;
            if (parafaitDefaultsDTO.DataTypeId == encryptedPasswordDataTypeId)
            {
                defaultValue = string.IsNullOrWhiteSpace(parafaitDefaultsDTO.DefaultValue) ? string.Empty: Encryption.Decrypt(parafaitDefaultsDTO.DefaultValue);
            }
            else
            {
                defaultValue = parafaitDefaultsDTO.DefaultValue;
            }
            if (siteParafaitDefaults.ContainsKey(defaultKey) == false)
            {
                siteParafaitDefaults.Add(defaultKey, defaultValue);
            }
            log.LogMethodExit();
        }
        private static DateTime? GetParafaitDefaultsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ParafaitDefaultsListBL parafaitDefaultsList = new ParafaitDefaultsListBL();
                result = parafaitDefaultsList.GetParafaitDefaultModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static List<ParafaitDefaultsDTO> GetParafaitDefaultsDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = null;
            try
            {
                ParafaitDefaultsListBL parafaitDefaultsList = new ParafaitDefaultsListBL();
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                parafaitDefaultsDTOList = parafaitDefaultsList.GetParafaitDefaults(searchParameters, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system options.", ex);
            }

            if (parafaitDefaultsDTOList == null)
            {
                parafaitDefaultsDTOList = new List<ParafaitDefaultsDTO>();
            }
            log.LogMethodExit(parafaitDefaultsDTOList);
            return parafaitDefaultsDTOList;
        }
        private static int GetEncryptedPasswordDataTypeId(int siteId)
        {
            int result = -1;
            try
            {
                ParafaitDefaultsListBL parafaitDefaultsList = new ParafaitDefaultsListBL();
                result = parafaitDefaultsList.GetEncryptedPasswordDataTypeId(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Encrypted Password DataTypeId.", ex);
                result = -1;
            }
            log.LogMethodExit(result);
            return result;
        }
        private void AddToOverridenDictionary(ParafaitDefaultsDTO parafaitDefaultsDTO)
        {
            string defaultValue = string.Empty;
            string defaultKey = string.Empty;

            foreach (ParafaitOptionValuesDTO parafaitOptionValuesDTO in parafaitDefaultsDTO.ParafaitOptionValuesDTOList)
            {
                if (parafaitOptionValuesDTO.UserId == -1 && parafaitOptionValuesDTO.PosMachineId == -1)
                {
                    continue;
                }
                if (parafaitDefaultsDTO.DataTypeId == encryptedPasswordDataTypeId)
                {
                    defaultValue = Encryption.Decrypt(parafaitOptionValuesDTO.OptionValue);
                }
                else
                {
                    defaultValue = parafaitOptionValuesDTO.OptionValue;
                }

                if (parafaitOptionValuesDTO.UserId > -1)
                {
                    defaultKey = parafaitDefaultsDTO.DefaultValueName + "USERID" + parafaitOptionValuesDTO.UserId;
                }
                else
                {
                    defaultKey = parafaitDefaultsDTO.DefaultValueName + "POSMACHINEID" + parafaitOptionValuesDTO.PosMachineId;
                }
                if (overridenParafaitDefaults.ContainsKey(defaultKey) == false)
                {
                    overridenParafaitDefaults.Add(defaultKey, defaultValue);
                }
            }
        }

        public string GetParafaitDefault(string defaultValueName, int userId, int posMachineId)
        {
            string result = GetValue(defaultValueName, userId, posMachineId);
            if (string.IsNullOrWhiteSpace(result) && constantDefaultValueDictionary.ContainsKey(defaultValueName))
            {
                result = constantDefaultValueDictionary[defaultValueName];
            }
            log.LogMethodExit("result");
            return result;
        }
        public ParafaitDefaultsDTO GetParafaitDefault(string defaultValueName)
        {
            ParafaitDefaultsDTO result = null;
            if (parafaitDefaultValueNameParafaitDefaultsDTODictionary != null && parafaitDefaultValueNameParafaitDefaultsDTODictionary.ContainsKey(defaultValueName))
            {
                result = parafaitDefaultValueNameParafaitDefaultsDTODictionary[defaultValueName];
            }
            log.LogMethodExit("result");
            return result;
        }

        private string GetValue(string defaultValueName, int userId, int posMachineId)
        {
            log.LogMethodEntry(defaultValueName, userId, posMachineId);
            string result = string.Empty;
            if (siteParafaitDefaults.ContainsKey(defaultValueName) == false)
            {
                string errorMessage = "Unable to find the default value: " + defaultValueName + " for site:user:pos " + siteId + ":" + userId + ":" + posMachineId;
                log.Error(errorMessage);
                log.LogMethodExit(result, errorMessage);
                return result;
            }
            string userLevelDictionaryKey = GetUserLevelDictionaryKey(defaultValueName, userId);
            if (overridenParafaitDefaults.ContainsKey(userLevelDictionaryKey))
            {
                result = overridenParafaitDefaults[userLevelDictionaryKey];
                log.LogMethodExit("result", "User level");
                return result;
            }
            string posMachineLevelDictionaryKey = GetPosMachineLevelDictionaryKey(defaultValueName, posMachineId);
            if (overridenParafaitDefaults.ContainsKey(posMachineLevelDictionaryKey))
            {
                result = overridenParafaitDefaults[posMachineLevelDictionaryKey];
                log.LogMethodExit("result", "Pos Machine Level");
                return result;
            }
            result = siteParafaitDefaults[defaultValueName];
            log.LogMethodExit("result");
            return result;
        }

        private string GetUserLevelDictionaryKey(string defaultValueName, int userId)
        {
            log.LogMethodEntry(defaultValueName, userId);
            string result = defaultValueName + "USERID" + userId;
            log.LogMethodExit(result);
            return result;

        }

        private string GetPosMachineLevelDictionaryKey(string defaultValueName, int posMachineId)
        {
            log.LogMethodEntry(defaultValueName, posMachineId);
            string result = defaultValueName + "POSMACHINEID" + posMachineId;
            log.LogMethodExit(result);
            return result;
        }

        public ParafaitDefaultContainerDTOCollection GetParafaitDefaultContainerDTOCollection(int userId, int posMachineId)
        {
            log.LogMethodEntry(userId, posMachineId);
            List<ParafaitDefaultContainerDTO> parafaitDefaultContainerDTOList = new List<ParafaitDefaultContainerDTO>();
            var defaultValueNameList = siteParafaitDefaults.Keys;
            foreach (string defaultValueName in defaultValueNameList)
            {
                string defultValue = GetParafaitDefault(defaultValueName, userId, posMachineId);
                ParafaitDefaultContainerDTO defaultViewDTO = new ParafaitDefaultContainerDTO(defaultValueName, defultValue);
                parafaitDefaultContainerDTOList.Add(defaultViewDTO);
            }
            ParafaitDefaultContainerDTOCollection parafaitDefaultContainerDTOCollection = new ParafaitDefaultContainerDTOCollection(parafaitDefaultContainerDTOList);
            log.LogMethodExit(parafaitDefaultContainerDTOCollection);
            return parafaitDefaultContainerDTOCollection;
        }

        private void AssignComputedValues()
        {
            log.LogMethodEntry();
            if(parafaitDefaultValueNameParafaitDefaultsDTODictionary.ContainsKey("ROUND_OFF_AMOUNT_TO"))
            {
                int roundOffAmountTo;
                double roundOffAmountValue = 0;
                if (siteParafaitDefaults.ContainsKey("ROUND_OFF_AMOUNT_TO"))
                {
                    roundOffAmountValue = double.Parse(siteParafaitDefaults["ROUND_OFF_AMOUNT_TO"]);
                }
                try
                {
                    string roundingPrecision = GetRoundingPrecision(siteParafaitDefaults["AMOUNT_FORMAT"]);
                    roundOffAmountTo = (int)(Math.Pow(10, Convert.ToInt32(roundingPrecision)) * roundOffAmountValue);
                    if (roundOffAmountTo <= 0)
                        roundOffAmountTo = 100;
                }
                catch
                {
                    roundOffAmountTo = 100;
                }
                siteParafaitDefaults["ROUND_OFF_AMOUNT_TO"] = roundOffAmountTo.ToString();
            }
            if (parafaitDefaultValueNameParafaitDefaultsDTODictionary.ContainsKey("AMOUNT_FORMAT") &&
               parafaitDefaultValueNameParafaitDefaultsDTODictionary.ContainsKey("CURRENCY_SYMBOL") &&
               parafaitDefaultValueNameParafaitDefaultsDTODictionary.ContainsKey("CURRENCY_CODE"))
            {
                string roundingPrecision = GetRoundingPrecision(siteParafaitDefaults["AMOUNT_FORMAT"]);
                string amountWithCurrencySymbol = GetAmountWithCurrencySymbol(siteParafaitDefaults["AMOUNT_FORMAT"], roundingPrecision, siteParafaitDefaults["CURRENCY_SYMBOL"]);
                string amountWithCurrencyCode = GetAmountWithCurrencySymbol(siteParafaitDefaults["AMOUNT_FORMAT"], roundingPrecision, siteParafaitDefaults["CURRENCY_CODE"]);

                if (siteParafaitDefaults.ContainsKey("ROUNDING_PRECISION") == false)
                {
                    siteParafaitDefaults.Add("ROUNDING_PRECISION", roundingPrecision);
                }
                if (siteParafaitDefaults.ContainsKey("AMOUNT_WITH_CURRENCY_SYMBOL") == false)
                {
                    siteParafaitDefaults.Add("AMOUNT_WITH_CURRENCY_SYMBOL", amountWithCurrencySymbol);
                }
                if (siteParafaitDefaults.ContainsKey("AMOUNT_WITH_CURRENCY_CODE") == false)
                {
                    siteParafaitDefaults.Add("AMOUNT_WITH_CURRENCY_CODE", amountWithCurrencyCode);
                }

                ParafaitDefaultsDTO amountFormatParafaitDefaultDTO = parafaitDefaultValueNameParafaitDefaultsDTODictionary["AMOUNT_FORMAT"];
                if (amountFormatParafaitDefaultDTO.ParafaitOptionValuesDTOList != null &&
                    amountFormatParafaitDefaultDTO.ParafaitOptionValuesDTOList.Any())
                {
                    foreach (ParafaitOptionValuesDTO parafaitOptionValuesDTO in amountFormatParafaitDefaultDTO.ParafaitOptionValuesDTOList)
                    {
                        if (parafaitOptionValuesDTO.UserId != -1)
                        {
                            roundingPrecision = GetRoundingPrecision(GetValue("AMOUNT_FORMAT", parafaitOptionValuesDTO.UserId, -1));
                            amountWithCurrencySymbol = GetAmountWithCurrencySymbol(GetValue("AMOUNT_FORMAT", parafaitOptionValuesDTO.UserId, -1), roundingPrecision, GetValue("CURRENCY_SYMBOL", parafaitOptionValuesDTO.UserId, -1));
                            amountWithCurrencyCode = GetAmountWithCurrencySymbol(GetValue("AMOUNT_FORMAT", parafaitOptionValuesDTO.UserId, -1), roundingPrecision, GetValue("CURRENCY_CODE", parafaitOptionValuesDTO.UserId, -1));
                            string key = "ROUNDING_PRECISION" + "USERID" + parafaitOptionValuesDTO.UserId;
                            if (overridenParafaitDefaults.ContainsKey(key) == false)
                            {
                                overridenParafaitDefaults.Add(key, roundingPrecision);
                            }
                            key = "AMOUNT_WITH_CURRENCY_SYMBOL" + "USERID" + parafaitOptionValuesDTO.UserId;
                            if (overridenParafaitDefaults.ContainsKey(key) == false)
                            {
                                overridenParafaitDefaults.Add(key, amountWithCurrencySymbol);
                            }
                            key = "AMOUNT_WITH_CURRENCY_CODE" + "USERID" + parafaitOptionValuesDTO.UserId;
                            if (overridenParafaitDefaults.ContainsKey(key) == false)
                            {
                                overridenParafaitDefaults.Add(key, amountWithCurrencyCode);
                            }
                        }
                        else if (parafaitOptionValuesDTO.PosMachineId != -1)
                        {
                            roundingPrecision = GetRoundingPrecision(GetValue("AMOUNT_FORMAT", -1, parafaitOptionValuesDTO.PosMachineId));
                            amountWithCurrencySymbol = GetAmountWithCurrencySymbol(GetValue("AMOUNT_FORMAT", -1, parafaitOptionValuesDTO.PosMachineId), roundingPrecision, GetValue("CURRENCY_SYMBOL", -1, parafaitOptionValuesDTO.PosMachineId));
                            amountWithCurrencyCode = GetAmountWithCurrencySymbol(GetValue("AMOUNT_FORMAT", -1, parafaitOptionValuesDTO.PosMachineId), roundingPrecision, GetValue("CURRENCY_CODE", -1, parafaitOptionValuesDTO.PosMachineId));
                            string key = "ROUNDING_PRECISION" + "POSMACHINEID" + parafaitOptionValuesDTO.PosMachineId;
                            if (overridenParafaitDefaults.ContainsKey(key) == false)
                            {
                                overridenParafaitDefaults.Add(key, roundingPrecision);
                            }
                            key = "AMOUNT_WITH_CURRENCY_SYMBOL" + "POSMACHINEID" + parafaitOptionValuesDTO.PosMachineId;
                            if (overridenParafaitDefaults.ContainsKey(key) == false)
                            {
                                overridenParafaitDefaults.Add(key, amountWithCurrencySymbol);
                            }
                            key = "AMOUNT_WITH_CURRENCY_CODE" + "POSMACHINEID" + parafaitOptionValuesDTO.PosMachineId;
                            if (overridenParafaitDefaults.ContainsKey(key) == false)
                            {
                                overridenParafaitDefaults.Add(key, amountWithCurrencyCode);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private string GetRoundingPrecision(string amountFormat)
        {
            log.LogMethodEntry();
            int RoundingPrecision = 0;
            try
            {
                if (amountFormat.Contains("#"))
                {
                    int pos = amountFormat.IndexOf(".");
                    if (pos >= 0)
                    {
                        RoundingPrecision = amountFormat.Length - pos - 1;
                    }
                    else
                    {
                        RoundingPrecision = 0;
                    }
                }
                else
                {
                    if (amountFormat.Length > 1)
                        RoundingPrecision = Convert.ToInt32(amountFormat.Substring(1));
                    else
                        RoundingPrecision = 0;
                }
            }
            catch { }
            log.LogMethodExit(RoundingPrecision);
            return RoundingPrecision.ToString();
        }

        private string GetAmountWithCurrencySymbol(string amountFormat, string roundingPrecesion, string currencySymbol)
        {
            log.LogMethodExit();
            string amountWithCurrencySymbol;
            if (amountFormat.Contains("#"))
            {
                amountWithCurrencySymbol = currencySymbol + " " + amountFormat;
            }
            else
            {
                amountWithCurrencySymbol = "C" + roundingPrecesion;
            }
            log.LogMethodExit(amountWithCurrencySymbol);
            return amountWithCurrencySymbol;
        }

        public ParafaitDefaultContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime =GetParafaitDefaultModuleLastUpdateTime(siteId);
            if (parafaitDefaultModuleLastUpdateTime.HasValue
                && parafaitDefaultModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in parafait defaults since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            ParafaitDefaultContainer result = new ParafaitDefaultContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
        private static DateTime? GetParafaitDefaultModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL();
                result = parafaitDefaultsListBL.GetParafaitDefaultModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the parfait default module last update time.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
