using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.MessagingClients
{
    public class SMS : MessagingClientBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string SMSuserid;
        string SMSpasswd;
        string SMSurl;

        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;
        public SMS(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO) : base(executionContext, messagingClientDTO)
        {
            log.LogMethodEntry(executionContext, messagingClientDTO);
            this.executionContext = executionContext;
            this.messagingClientDTO = messagingClientDTO;
            SMSuserid = messagingClientDTO.UserName;
            SMSpasswd = messagingClientDTO.Password;//Utilities.getParafaitDefaults("CRM_SMS_PASSWORD");
            SMSurl = messagingClientDTO.HostUrl;
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Initialize method to initialize Gateway Configuration
        /// </summary>
        /// <param name="inUtilities"></param>
        /// <returns></returns>
        public void Initialize()
        {
            log.LogMethodEntry();
            // Utilities = inUtilities;

            //smtpHost = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_HOST");
            //smtpPort = 587;
            //smtpUsername = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_NETWORK_USERNAME");
            //smtpPassword = ParafaitDefaultContainerList.GetDecryptedParafaitDefault(executionContext, "CRM_SMTP_NETWORK_PASSWORD"); //Utilities.getParafaitDefaults("CRM_SMTP_NETWORK_PASSWORD");
            //smtpDisplayName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_SMTP_FROM_NAME");
            //EnableSsl = (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CRM_ENABLE_SMTP_SSL") == "Y");

            



            log.LogMethodExit();
           // return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingRequestDTO"></param>
        public override MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry(messagingClientDTO);

            base.Send(messagingRequestDTO);
            string result = string.Empty;
            try
            {
                string responseCode = string.Empty;
                List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
                searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);
                LookupValuesDTO charLimitLookupDTO = lookupValuesDTOList.First(x => x.LookupValue == "CharLimit");

                int charLimit = 160;
                if (charLimitLookupDTO != null)
                {
                    int limitResult;
                    if (int.TryParse(charLimitLookupDTO.Description, out limitResult))
                        charLimit = limitResult;
                }
                if (messagingRequestDTO.Body.Length > charLimit)
                    messagingRequestDTO.Body = messagingRequestDTO.Body.Substring(0, charLimit);

                messagingRequestDTO.Body = System.Net.WebUtility.UrlEncode(messagingRequestDTO.Body);

                string smsapi = SMSurl.Replace("@UserId", SMSuserid).Replace
                                                ("@Password", SMSpasswd).Replace
                                                ("@Message", messagingRequestDTO.Body).Replace
                                                ("@PhoneNumber", messagingRequestDTO.ToMobile);
                result = sendHttpRequest(smsapi);

                if (result == "Success")
                {
                    messagingRequestDTO.SendDate = ServerDateTime.Now;
                    base.UpdateResults(messagingRequestDTO, "Success", result);

                }
                else
                {
                    base.UpdateResults(messagingRequestDTO, "Error", result);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured in sendSMSSynchronous", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + messagingRequestDTO.ToMobile + ": " + ex.Message);
                throw new ApplicationException(messagingRequestDTO.ToMobile + ": " + ex.Message);
            }

            log.LogMethodExit(result);
            return messagingRequestDTO;
        }

        /// <summary>
        /// Populate Template method to populate the Template parameters 
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="paramsList"></param>
        ///// <returns></returns>
        //public override string PopulateTemplate(string Template, List<KeyValuePair<string, string>> paramsList)
        //{
        //    log.LogMethodEntry(Template, paramsList);

        //    foreach(var parameter in paramsList)
        //    {
        //        Template = Template.Replace(parameter.Key.ToLower(), parameter.Value);
        //    }

        //    log.LogMethodExit(Template);
        //    return Template;
        //}


        /// <summary>
        /// SendRequest Method to send the SMS Request
        /// </summary>
        /// <param name="PhoneNos"></param>
        /// <param name="Template"></param>
        /// <returns></returns>
        //public override string SendRequest(string PhoneNos, string Template)
        //{
        //    log.LogMethodEntry(PhoneNos, Template);
        //    string result = string.Empty;
        //    try
        //    {
        //        string responseCode = string.Empty;
        //        if (Template.Length > 160)
        //            Template = Template.Substring(0, 160);

        //        Template = System.Net.WebUtility.UrlEncode(Template);

        //        string smsapi = SMSurl.Replace("@UserId", SMSuserid).Replace
        //                                        ("@Password", SMSpasswd).Replace
        //                                        ("@Message", Template).Replace
        //                                        ("@PhoneNumber", PhoneNos);
        //        result = sendHttpRequest(smsapi);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured in sendSMSSynchronous", ex);
        //        log.LogMethodExit(null, "Throwing ApplicationException- " + PhoneNos + ": " + ex.Message);
        //        throw new ApplicationException(PhoneNos + ": " + ex.Message);
        //    }
        //    log.LogMethodExit(result);
        //    return result;
        //}


        /// <summary>
        /// sendHttpRequest Method to send Http Request to API
        /// </summary>
        /// <param name="smsapi"></param>
        /// <returns></returns>
        protected string sendHttpRequest(string smsapi)
        {
            log.LogMethodEntry(smsapi);
            string resultCode = string.Empty;
            string result = string.Empty;
            WebRequest request = null;
            HttpWebResponse response = null;

            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);

            try
            {               
                request = WebRequest.Create(smsapi);
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                resultCode = reader.ReadToEnd();
                reader.Close();
                stream.Close();

                if (lookupValuesDTOList != null)
                {
                    foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                    {
                        if (resultCode.Contains(lookup.LookupValue))
                        {
                            result = "Success";
                        }
                    }
                }
                else
                {
                    log.Error("SMS_GATEWAY_RESPONSE_CODE lookup values are not set to check the message send status");
                }

                if (Regex.IsMatch(resultCode, @"^[a-zA-Z0-9]+$") || result == "Success")
                {
                    result = "Success";
                    log.LogMethodExit(result);
                    return result;
                }

                else
                {
                    result = "Error";
                    log.LogMethodExit(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred in sendHttpRequest", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + resultCode);
                return resultCode;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            
        }

    }
}
