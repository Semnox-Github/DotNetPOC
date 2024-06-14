/********************************************************************************************
 * Project Name - Reports
 * Description  - ReportFunctions class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.120.1     31-May-2021      Laster Menezes    Created new class.
 *2.130       22-Sep-2021      Laster Menezes    Created new method GetWebMetaTitleByVertical to get web meta title based on Vertical Type
 *2.140.0     16-Nov-2021      Laster Menezes    Added Telerik user function DecryptLicenceKeyDetails
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Telerik.Reporting.Expressions;

namespace Semnox.Parafait.Reports
{
    public class ReportFunctions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public ReportFunctions(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        [Function(Category = "Reports", Namespace = "ReportFunctions", Description = "Returns the decrypted value")]
        public static string Decrypt(string encryptedValue)
        {
            log.LogMethodEntry(encryptedValue);
            string decryptedValue = string.Empty;
            try
            {                
                decryptedValue = Encryption.Decrypt(encryptedValue);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                decryptedValue = encryptedValue;
            }
            log.LogMethodExit();
            return decryptedValue;
        }


        /// <summary>
        /// GetWebMetaTitleByVertical
        /// </summary>
        /// <returns>Meta title for website based on Vertical type</returns>
        public string GetWebMetaTitleByVertical()
        {
            log.LogMethodEntry();
            string reportTitle = MessageContainerList.GetMessage(executionContext, "Reports");
            string metaTitle = "SEMNOX | " + reportTitle;
            try
            {
                LookupValuesList lookUpList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SELECTED_VERTICAL_TYPE"));
                List<LookupValuesDTO> lookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);

                if (lookUpValuesList != null && lookUpValuesList.Count > 0)
                {
                    foreach (LookupValuesDTO valueDTO in lookUpValuesList)
                    {
                        if (valueDTO.LookupValue == "SELECTED_VERTICAL_TYPE")
                        {
                            if (!string.IsNullOrEmpty(valueDTO.Description))
                            {
                                metaTitle = valueDTO.Description + " | " + reportTitle;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
            return metaTitle;
        }


        [Function(Category = "Reports", Namespace = "LicenceKeyFunction", Description = "Returns the decrypted Licence Expiry Date")]
        public static string DecryptLicenceKeyDetails(string encryptedLicenceKey, bool getSiteKey = true)
        {
            log.LogMethodEntry(encryptedLicenceKey);
            string decryptedLicenceData = string.Empty;
            string siteKey = string.Empty;
            DateTime expiryDate = DateTime.MinValue;
            string decryptedLicenceKey = Encryption.Decrypt(encryptedLicenceKey.Replace("\0", string.Empty));
            KeyManagement km = new KeyManagement(Common.Utilities.DBUtilities, Common.Utilities.ParafaitEnv);
            km.DecodeKey(decryptedLicenceKey, ref siteKey, ref expiryDate);
            if (getSiteKey)
            {
                decryptedLicenceData = siteKey;
            }
            else
            {
                decryptedLicenceData = expiryDate.ToString();
            }
            log.LogMethodExit(decryptedLicenceData);
            return decryptedLicenceData;
        }
    }
}
