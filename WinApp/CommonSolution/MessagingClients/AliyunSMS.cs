using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.MessagingClients
{
    public class AliyunSMS : MessagingClientBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string product = string.Empty;
        string domain = string.Empty;
        string accessKeyId = string.Empty;
        string Template = string.Empty;
        string accessKeySecret = string.Empty;
        string signName = string.Empty;
        string templateCode = string.Empty;
        string templateParams = string.Empty;
        Utilities Utilities;
        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="messagingClientDTO"></param>
        public AliyunSMS(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO): base(executionContext,  messagingClientDTO)
        {
            log.LogMethodEntry(messagingClientDTO);
            this.executionContext = executionContext;
            this.messagingClientDTO = messagingClientDTO;
            Initialize();
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagingRequestDTO"></param>
        public override MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry(messagingRequestDTO);

            string result = string.Empty;
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            try
            {
                request.PhoneNumbers = messagingRequestDTO.ToMobile;
                request.SignName = signName;
                request.TemplateCode = templateCode;
                request.TemplateParam = templateParams;
                response = acsClient.GetAcsResponse(request);
                result = validateResponse(response);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            log.LogMethodEntry();
            //Utilities = inutilities;
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ALIYUN_SMS_GATEWAY_CONFIGURATION"));
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
            lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);
            if (lookupValuesDTOList != null)
            {
                foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                {
                    if (lookup.LookupValue == "ALIYUN_SMS_PRODUCT_NAME")
                        product = lookup.Description;
                    else if (lookup.LookupValue == "ALIYUN_GATEWAY_DOMAIN")
                        domain = lookup.Description;
                    else if (lookup.LookupValue == "ALIYUN_SMS_GATEWAY_ACCESSKEYID")
                        accessKeyId = lookup.Description;
                    else if (lookup.LookupValue == "ALIYUN_SMS_GATEWAY_ACCESSKEYSECRET")
                        accessKeySecret = lookup.Description;
                    else if (lookup.LookupValue == "ALIYUN_SMS_GATEWAY_SIGNATURE_NAME")
                        signName = lookup.Description;
                    else if (lookup.LookupValue == "ALIYUN_SMS_GATEWAY_VERIFICATION_TEMPLATE_CODE")
                        templateCode = lookup.Description;
                }
            }

            log.LogMethodExit();
           
        }


        /// <summary>
        /// Populate Template method to populate the Template parameters
        /// </summary>
        /// <param name="Template"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        //public override string PopulateTemplate(string Template, List<KeyValuePair<string, string>> paramsList)
        //{
        //    log.LogMethodEntry(Template, paramsList);
        //    string jsonParams = string.Empty;
        //    dynamic paramFields = new JObject();

        //    foreach (KeyValuePair<string, string> parameter in paramsList)
        //    {
        //        paramFields.Add(parameter.Key.Replace("@",""),parameter.Value);
        //    }

        //    jsonParams = JsonConvert.SerializeObject(paramFields);
        //    templateParams = jsonParams;
        //    log.LogMethodExit(Template);
        //    return Template;
        //}


        /// <summary>
        /// SendRequest Method to send the Request to API
        /// </summary>
        /// <param name="PhoneNos"></param>
        /// <param name="Template"></param>
        /// <returns></returns>
        //public override string SendRequest(string PhoneNos, string Template)
        //{
        //    log.LogMethodEntry(PhoneNos, Template);
        //    string result = string.Empty;
        //    IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
        //    DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
        //    IAcsClient acsClient = new DefaultAcsClient(profile);
        //    SendSmsRequest request = new SendSmsRequest();
        //    SendSmsResponse response = null;
        //    try
        //    {
        //        request.PhoneNumbers = PhoneNos;
        //        request.SignName = signName;
        //        request.TemplateCode = templateCode;
        //        request.TemplateParam = templateParams;
        //        response = acsClient.GetAcsResponse(request);
        //        result = validateResponse(response);
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
        /// ValidateResponse method to check for the result based on Response Code
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string validateResponse(SendSmsResponse response)
        {
            log.LogMethodEntry(response);
            string resultCode = string.Empty;
            if (response.Code != null && response.Code =="OK")
            {
                resultCode = response.Code.ToString();
            }
            else
            {
                resultCode = response.Code.ToString() + " - " + response.Message;
            }
            log.LogMethodExit(resultCode);
            return resultCode;
        }
    }
}

