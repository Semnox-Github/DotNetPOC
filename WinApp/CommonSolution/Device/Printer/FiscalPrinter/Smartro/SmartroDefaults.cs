
/********************************************************************************************
 * Project Name - Device
 * Description  - SmartroDefaults  Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By            Remarks          
 *********************************************************************************************

*2.130.4       01-Dec-2021      Vidita Solution        Created: Korean Fiscalization
*2.130.4       13-Apr-2021      Girish Kundar          Modified: Issue Fixes
*2.130.10      22-May-2023      Sathyavathi            Modified: Externalized SMARTRO_TIME_OUT value
***********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Printer.FiscalPrinter.Smartro
{
    public static class SmartroDefaults
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<string, string> defaultDictionary = new Dictionary<string, string>
        {
             //Intialization
            {"INIT_ITEM_SERVICE_TYPE", "9901" },

            //CUSTOMER
			{"CASH_PAY_CUSTOMER_ITEM_INSTALLMENT_PERIOD_USED_CHECK","03"},
			//BUSINESSPERSON
			{"CASH_PAY_BUSINESSPERSON_ITEM_INSTALLMENT_PERIOD_USED_CHECK","13"},
			//VOLUNTEER_ISSUANCE
			{"CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SERVICE_TYPE","0111" },
            {"CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_TRADE_SEPARATE_CODE","02" },
            {"CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_INSTALLMENT_PERIOD_USED_CHECK","03" },
            {"CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SIGN_SET","3" },
            {"CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SIGN_IMAGE_DATA","0100001234" },
            //card payment
            {"CARD_PAY_ITEM_SERVICE_TYPE","0101" },
            {"CARD_PAY_ITEM_TRADE_SEPARATE_CODE","01" },
            {"CARD_PAY_ITEM_SIGN_SET","03" },
            //card cancel
            {"CARD_CANCEL_ITEM_SERVICE_TYPE","2101" },
            {"CARD_CANCEL_ITEM_TRADE_SEPARATE_CODE","01" },
            {"CARD_CANCEL_ITEM_SIGN_SET","3" },
            //Cash approval
            {"CASH_PAY_ITEM_SERVICE_TYPE","0101" },
            {"CASH_PAY_ITEM_TRADE_SEPARATE_CODE","02" },
            {"CASH_PAY_ITEM_INSTALLMENT_PERIOD_USED_CHECK","13" },
            {"CASH_PAY_ITEM_SIGN_SET","0" },
            //cash cancel
            {"CASH_CANCEL_ITEM_SERVICE_TYPE","2101" },
            {"CANCEL_CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SERVICE_TYPE","2111" },
            {"CASH_CANCEL_ITEM_TRADE_SEPARATE_CODE","02" },
            {"CASH_CANCEL_ITEM_INSTALLMENT_PERIOD_USED_CHECK","13" },
            {"CASH_CANCEL_ITEM_SIGN_SET","0" },
            {"CASH_CANCEL_ITEM_CASH_CANCEL_REASON","1" },
            {"CANCEL_CASH_PAY_CUSTOMER_ITEM_INSTALLMENT_PERIOD_USED_CHECK","00" },
            {"SMARTRO_TIME_OUT", "60" }

        };

        static SmartroDefaults()
        {
            log.LogMethodEntry();
            int smartroTimeout = GetSmartroTimeoutValue();
            if(smartroTimeout < 0)
            {
                log.Error("smartroTimeout value is invalid smartroTimeout:" + smartroTimeout);
                smartroTimeout = 60;
            }
            defaultDictionary["SMARTRO_TIME_OUT"] = smartroTimeout.ToString();
            log.LogMethodExit();
        }

        public static string GetSmartroDefault(string defultName)
        {
            log.LogMethodEntry(defultName);
            string result = string.Empty;
            if (defaultDictionary.ContainsKey(defultName))
            {
                result = defaultDictionary[defultName].ToString().Trim();
            }
            log.LogMethodExit(result);
            return result;
        }

        private static int GetSmartroTimeoutValue()
        {
            log.LogMethodEntry();
            int retValue = 60;
            try
            {
                List<LookupValuesDTO> kioskScreenLookupSetupList = GetLookupSetupForKiosk();
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    LookupValuesDTO lookupValuesDTO = kioskScreenLookupSetupList.FirstOrDefault(lv => lv.IsActive && lv.LookupValue == "StatusUITimeoutValue");
                    if (lookupValuesDTO != null)
                    {
                        if(int.TryParse(lookupValuesDTO.Description, out retValue) == false)
                        {
                            retValue = 60;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        private static List<LookupValuesDTO> GetLookupSetupForKiosk()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(ExecutionContext.GetExecutionContext());
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParms = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "KIOSK_SCREEN_IMAGE"));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParms);
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }
    }
}
