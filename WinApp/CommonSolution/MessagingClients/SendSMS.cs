using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using System.Reflection;

namespace Semnox.Parafait.SMSGateway
{
    public enum SMSGateways
    {
        /// <summary>
        /// No SMS Gateway.
        /// </summary>
        None,
        /// <summary>
        /// Generic SMS Gateway
        /// </summary>
        Generic,
        /// <summary>
        /// Aliyun SMS Gateway
        /// </summary>
        Aliyun,

    }

    public class SendSMS
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities _utilities;
        public BaseSMS baseSMSGateway;
        private static SendSMS sendSMS;
        string _SMS_GATEWAY = string.Empty;
        protected bool initialized = false;

        /// <summary>
        /// Initialize method
        /// </summary>
        /// <param name="inUtilities"></param>
        public virtual void Initialize(Semnox.Core.Utilities.Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            if (!initialized)
            {
                this._utilities = inUtilities;
                initialized = true;
            }
            log.LogMethodExit(null);
        }


        /// <summary>
        /// GetInstance Method to create Instance of SendSMS
        /// </summary>
        /// <returns></returns>
        public static SendSMS GetInstance()
        {
            log.LogMethodEntry();
            if (sendSMS == null)
            {
                sendSMS = new SendSMS();
            }
            log.LogMethodExit(sendSMS);
            return sendSMS;
        }


        /// <summary>
        /// sendSMSSynchronous method to select the SMS Gateway and send the SMS request.
        /// </summary>
        /// <param name="PhoneNos"></param>
        /// <param name="Template"></param>
        /// <param name="ParamList"></param>
        /// <returns></returns>
        public string sendSMSSynchronous(string PhoneNos, string Template, List<KeyValuePair<string, string>> ParamList)
        {
            log.LogMethodEntry(PhoneNos, Template, ParamList);
          
            string result = string.Empty;
            string resultCode = string.Empty;
            SendSMS.GetInstance();
            //get Selected SMS Gateway
            _SMS_GATEWAY = _utilities.getParafaitDefaults("SMS_GATEWAY");            
            SendSMS.GetInstance().Initialize(_utilities);
            baseSMSGateway = SendSMS.GetInstance().GetSMSGateway(_SMS_GATEWAY);
            try
            {
              
                    baseSMSGateway.Initialize(_utilities);
                    Template = baseSMSGateway.PopulateTemplate(Template, ParamList);
                    resultCode = baseSMSGateway.SendRequest(PhoneNos, Template);

                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SMS_GATEWAY_RESPONSE_CODE"));
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (_utilities.ParafaitEnv.IsCorporate ? _utilities.ParafaitEnv.SiteId : -1).ToString()));
                    lookupValuesDTOList = new LookupValuesList(_utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
                    if (lookupValuesDTOList != null)
                    {
                        foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                        {
                            if (resultCode.Contains(lookup.LookupValue))
                            {
                                result = "success";
                            }
                        }
                    }
                    else
                    {
                        log.Error("SMS_GATEWAY_RESPONSE_CODE lookup values are not set to check the message send status");
                    }

                    //Checks for Alphanumeric response or success
                    if (Regex.IsMatch(resultCode, @"^[a-zA-Z0-9]+$") || result == "success")
                    {
                        log.LogMethodExit(result);
                        return result;
                    }

                    else
                    {
                        result = "Error - " + resultCode;
                        log.LogMethodExit(null, "Throwing ApplicationException- " + result);
                        throw new ApplicationException(result);
                    }

            }
            catch (Exception ex)
            {
                log.Error("Error occurred in sendSMSSynchronous", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + PhoneNos + ": " + ex.Message);
                throw new ApplicationException(PhoneNos + ": " + ex.Message);
            }
          
        }

 
        /// <summary>
        /// Get SMS Gateway Instance
        /// </summary>
        /// <param name="smsGatewayString"></param>
        /// <returns></returns>
        public BaseSMS GetSMSGateway(string smsGatewayString)
        {
            log.LogMethodEntry(smsGatewayString);
            SMSGateways SMSGatewayName;
            BaseSMS baseSMSGateway = null;
            if (Enum.TryParse<SMSGateways>(smsGatewayString, out SMSGatewayName))
            {
                if (_utilities == null)
                {
                    log.LogMethodExit(null, "Throwing ArgumentNullException - utilities is null.");
                    throw new ArgumentNullException("utilities");
                }
                baseSMSGateway = CreateSMSGatewayInstance(SMSGatewayName, _utilities);
            }
            else
            {
                log.LogMethodExit(null, "");
            }
            log.LogMethodExit(baseSMSGateway);
            return baseSMSGateway;
        }


        /// <summary>
        /// Create SMS Gateway Instance
        /// </summary>
        /// <param name="SMSGatewayName"></param>
        /// <param name="utilities"></param>
        /// <returns></returns>
        private BaseSMS CreateSMSGatewayInstance(SMSGateways SMSGatewayName, Utilities utilities)
        {
            log.LogMethodEntry(SMSGatewayName, utilities);

            BaseSMS baseSMSGateway = null;
            string baseSMSGatewayClassName = GetSMSGatewayClassName(SMSGatewayName);
            object SMSGatewayInstance = null;
            if (string.IsNullOrWhiteSpace(baseSMSGatewayClassName) == false)
            {
                try
                {
                    Type type = Type.GetType(baseSMSGatewayClassName);
                    if (type != null)
                    {
                        ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Utilities) });
                        SMSGatewayInstance = constructor.Invoke(new object[] { utilities });
                    }
                    else
                    {
                        log.LogMethodExit(null, "SMS Gateway Configuration Exception - Cannot create instance of type: " + baseSMSGatewayClassName + ". Please check whether the dll exist in application folder.");
                    }
                }
                catch (Exception e)
                {
                    log.Error("Error occurred while creating SMSGatewayInstance", e);
                    log.LogMethodExit(null, "SMS Gateway Configuration Exception - Error occurred while creating the SMSGateway. type: " + SMSGatewayName.ToString());
                }
            }
            else
            {
                log.LogMethodExit(null, "SMS Gateway Configuration Exception - SMSGateway not configured for : " + SMSGatewayName.ToString() + ". Please configure the same.");
            }
            if (SMSGatewayInstance != null && SMSGatewayInstance is BaseSMS)
            {
                baseSMSGateway = SMSGatewayInstance as BaseSMS;
            }
            else
            {
                StringBuilder message = new StringBuilder("Error occurred while creating the SMSGateway.type: " + SMSGatewayName.ToString());
                if (SMSGatewayInstance == null)
                {
                    message.Append(". SMSGatewayInstance is null. please check the baseSMSGatewayClassName");
                }
                else if ((SMSGatewayInstance is BaseSMS) == false)
                {
                    message.Append(". SMSGatewayInstance is not of valid type. Type of SMSGatewayInstance is :" + SMSGatewayInstance.GetType().ToString());
                }
                log.LogMethodExit(null, "SMS Gateway Configuration Exception - " + message.ToString());
            }
            log.LogMethodExit(baseSMSGateway);
            return baseSMSGateway;
        }


        /// <summary>
        /// Get SMSGateway class name 
        /// </summary>
        /// <param name="smsGateway"></param>
        /// <returns></returns>
        private string GetSMSGatewayClassName(SMSGateways smsGateway)
        {
            log.LogMethodEntry(smsGateway);
            string returnValue = string.Empty;
            switch (smsGateway)
            {
                case SMSGateways.None:
                    {
                        returnValue = "Semnox.Parafait.Communication.BaseSMS,Communication";
                        break;
                    }
                case SMSGateways.Generic:
                    {
                        returnValue = "Semnox.Parafait.Communication.GenericSMS,Communication";
                        break;
                    }
                case SMSGateways.Aliyun:
                    {
                        returnValue = "Semnox.Parafait.Communication.AliyunSMS,Communication";
                        break;
                    }               
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }



    }

}
