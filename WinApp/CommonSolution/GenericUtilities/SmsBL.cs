/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Business Logic to send SMS.
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        17-Sept-2019  Mushahid Faizan     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Semnox.Core.GenericUtilities
{
    public class SmsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SmsDTO smsDTO;
       
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public SmsBL(ExecutionContext executionContext, SmsDTO smsDTO)
        {
            log.LogMethodEntry(executionContext, smsDTO);
            this.executionContext = executionContext;
            this.smsDTO = smsDTO;
            log.LogMethodExit();
        }

        public string SendSMS()
        {
            string result = string.Empty;
            log.LogMethodEntry();
            if (smsDTO != null)
            {
                String smsUserId = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PROMO_SMS_USERNAME");
                String smsPassword = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PROMO_SMS_PASSWORD");//Common.Utilities.getParafaitDefaults("PROMO_SMS_PASSWORD");
                String smsURL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PROMO_SMS_PROVIDER_URL");

                //SmsDTO smsDTOObj = new SmsDTO(smsUserId, smsPassword, smsDTO.Message, smsDTO.MobileNumber);
                string resultCode = string.Empty;

                WebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    string message = string.Empty;

                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);
                    //if (Regex.IsMatch(result, @"^[a-zA-Z0-9]+$") || result.ToLower().StartsWith("success") || result.ToLower().Contains("successful"))
                    LookupValuesDTO charLimitLookupDTO = lookupValuesDTOList.First(x => x.LookupValue == "CharLimit");
                    int charLimit = 160;
                    if (charLimitLookupDTO != null)
                    {
                        int limitResult;
                        if (int.TryParse(charLimitLookupDTO.Description, out limitResult))
                            charLimit = limitResult;
                    }
                    if (message.Length > charLimit)
                        message = message.Substring(0, charLimit);

                    message = System.Net.WebUtility.UrlEncode(message);

                    string smsapi = smsURL.Replace("@UserId", smsDTO.UserId).Replace
                                                    ("@Password", smsDTO.Password).Replace
                                                    ("@Message", smsDTO.Message).Replace
                                                    ("@PhoneNumber", smsDTO.MobileNumber);

                    request = WebRequest.Create(smsapi);
                    // Send the 'HttpWebRequest' and wait for response.
                    response = (HttpWebResponse)request.GetResponse();

                    Stream stream = response.GetResponseStream();
                    Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader reader = new System.IO.StreamReader(stream, ec);
                    resultCode = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();
                    //List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    //List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    //searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
                    //searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    //lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);
                    foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                    {
                        if (resultCode.Contains(lookup.LookupValue))
                        {
                            result = "success";
                        }
                    }

                    if (Regex.IsMatch(resultCode, @"^[a-zA-Z0-9]+$") || result == "success")
                    {
                        return result;
                    }

                    else
                    {
                        result = "Error";
                        throw new ApplicationException(result);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed to send SMS", ex);
                    log.LogMethodExit(null, "Throwing Application Exception-" + ex.Message);
                    throw new ApplicationException(ex.Message);

                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
        public class SmsListBL
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private ExecutionContext executionContext;
            private List<SmsDTO> smsDTOList;
            /// <summary>
            /// Parametrized Constructor
            /// </summary>
            /// <param name="executionContext"></param>
            public SmsListBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                log.LogMethodExit();
            }
            /// <summary>
            /// Parametrized Constructor
            /// </summary>
            /// <param name="executionContext"></param>
            /// <param name="smsDTOList"></param>
            public SmsListBL(ExecutionContext executionContext, List<SmsDTO> smsDTOList)
            {
                log.LogMethodEntry(executionContext, smsDTOList);
                this.executionContext = executionContext;
                this.smsDTOList = smsDTOList;
                log.LogMethodExit();
            }
            public List<SmsDTO> SendSMSList(string activityType)
            {
                List<SmsDTO> returnSMSDTOList = new List<SmsDTO>();
                log.LogMethodEntry();

                if (smsDTOList != null && smsDTOList.Any())
                {
                    foreach (SmsDTO smsDTO in smsDTOList)
                    {
                        try
                        {
                            SmsBL smsBL = new SmsBL(executionContext, smsDTO);
                            smsDTO.Status = smsBL.SendSMS();
                        }
                        catch (ApplicationException ex)
                        {
                            smsDTO.Status = ex.Message;
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            smsDTO.Status = ex.Message;
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        }
                        returnSMSDTOList.Add(smsDTO);
                    }
                }
                log.LogMethodExit(returnSMSDTOList);
                return returnSMSDTOList;
            }
        }
}
