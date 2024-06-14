using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Semnox.Parafait.Communication;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.MessagingClients
{
    public class TwilioWhatsApp : MessagingClientBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string phoneNumberLength;
        string countryCode;
        string fromPhoneNumber;
        string authToken;
        string accountSid;
        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;
        private static Dictionary<string, string> countryCodeList = new Dictionary<string, string>();
        public TwilioWhatsApp(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO) : base(executionContext, messagingClientDTO)
        {
            log.LogMethodEntry(executionContext, messagingClientDTO);
            this.executionContext = executionContext;
            this.messagingClientDTO = messagingClientDTO;
            accountSid = messagingClientDTO.HostUrl;
            authToken = messagingClientDTO.Password;
            fromPhoneNumber = messagingClientDTO.UserName;
            if (countryCodeList.Count == 0)
            {
                setCountryCodes();
            }
            phoneNumberLength = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_PHONE_NUMBER_WIDTH");
            //InitializeCountry();

            log.LogMethodExit(null);
        }

        public List<CountryDTO> GetCountryList(string countryName = "")
        {
            log.LogMethodEntry();

            CountryDTOList countryDTOList = new CountryDTOList(executionContext);
            List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
            searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (!string.IsNullOrEmpty(countryName))
            {
                searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.COUNTRY_NAME, countryName));
            }
            List<CountryDTO> countryList = countryDTOList.GetCountryDTOList(searchCountryParams);

            log.LogMethodExit(countryList);
            return countryList;
        }

        /// <summary>
        /// InitializeCountry method to initialize phone number length and country code 
        /// </summary>
        /// <param name="country">country param has Country name</param>
        /// <returns>Return true</returns>
        public bool InitializeCountry()
        {
            log.LogMethodEntry();
            List<CountryDTO> countryList = null;
            CountryDTOList countryDTOList = new CountryDTOList(executionContext);
            List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
            searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            countryList = countryDTOList.GetCountryDTOList(searchCountryParams);
            if (countryList != null)
            {
                if (countryCodeList.ContainsKey(countryList[0].CountryName))
                {
                    countryCode = countryCodeList[countryList[0].CountryName];
                }
            }
            //countryCode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_COUNTRY_PHONE_CODE");

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Initialize method to initialize Twilio Gateway Configuration
        /// </summary>
        /// <param name="inUtilities">intilization utilities </param>
        /// <returns>Return true</returns>
        public void Initialize()
        {
            log.LogMethodEntry();

            setCountryCodes();
            //InitializeCountry();
            accountSid = messagingClientDTO.HostUrl;
            authToken = messagingClientDTO.Password;
            fromPhoneNumber = messagingClientDTO.UserName;

            log.LogMethodExit();

        }

        /// <summary>
        /// SendRequest Method to send the WhatsApp message using Twilio.
        /// </summary>
        /// <param name="phoneNos">To phone number to send WhatsApp message</param>
        /// <param name="message">Body of the WhatsApp message</param>
        /// <returns>Return WhatsApp Message Response</returns>
        public override MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry(messagingRequestDTO);
            base.Send(messagingRequestDTO);
            string PhoneNos;
            string messageResponse;
            PhoneNos = messagingRequestDTO.ToMobile;
            try
            {
                //InitializeCountry();
                //Validate Inputs
                if (string.IsNullOrWhiteSpace(PhoneNos))
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext, "Mobile Number"));//&1 details are missing
                    throw new ArgumentNullException(msg);
                }
                if (!string.IsNullOrWhiteSpace(messagingRequestDTO.CountryCode))
                {
                    countryCode = messagingRequestDTO.CountryCode;
                }
                else
                {
                    InitializeCountry();
                }
                if (string.IsNullOrEmpty(countryCode))
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext, "Country Code"));//&1 details are missing
                    throw new ArgumentNullException(msg);
                }
                //Add country code to phone number
                //if (Regex.IsMatch(PhoneNos, @"^([0]|\" + countryCode + @")?[0-9]\d{" + (Convert.ToInt32(phoneNumberLength) - 1) + @"}$"))
                //{
                //    if (PhoneNos.Length == Convert.ToInt32(phoneNumberLength))
                //    {
                //        PhoneNos = countryCode + PhoneNos;
                //    }
                //}
                //else
                //{
                //    throw new Exception("Invalid Phone Number");
                //}

                if (!PhoneNos.StartsWith(countryCode))
                {
                    PhoneNos = countryCode + PhoneNos;
                }

                if (string.IsNullOrWhiteSpace(messagingRequestDTO.Body))
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext, "Body"));//&1 details are missing
                    throw new ArgumentNullException(msg);
                }
                //Must be less than or equal 1600 characters
                if (messagingRequestDTO.Body.Length >= 1600)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2908, "1600");//Message cannot be longer than &1 characters
                    throw new ArgumentNullException(msg);
                }
                //throw new ArgumentOutOfRangeException(nameof(messagingRequestDTO.Body), "Message cannot be longer than 1600 characters");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                TwilioClient.Init(accountSid, authToken);

                //Type type = Type.GetType("Twilio.TwilioClient,Twilio");
                //type.GetMethod("Init", new[] { typeof(string), typeof(string) }).Invoke(null, new object[] { accountSid, authToken });

                var whatsAppMessage = MessageResource.Create(
                         body: messagingRequestDTO.Body,
                         from: new Twilio.Types.PhoneNumber("whatsapp:" + fromPhoneNumber),
                         provideFeedback: true,
                         to: new Twilio.Types.PhoneNumber("whatsapp:" + PhoneNos)
                     );

                log.Info(whatsAppMessage);
                messageResponse = ValidateResponse(whatsAppMessage.Sid);
                if (messageResponse == "Success")
                {
                    messagingRequestDTO.SendDate = ServerDateTime.Now;
                    base.UpdateResults(messagingRequestDTO, "Success", messageResponse);

                }
                else
                {
                    base.UpdateResults(messagingRequestDTO, "Error", messageResponse);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in SendRequest", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + PhoneNos + ": " + ex.Message);
                throw new ApplicationException(PhoneNos + ": " + ex.Message);
            }
            log.LogMethodExit(messageResponse);
            return messagingRequestDTO;
        }


        /// <summary>
        /// validateResponse Method to check WhatsApp message status
        /// </summary>
        /// <param name="messageSid">Message sid to find the status of the WhatsApp message</param>
        /// <returns>WhatsApp Message Response</returns>
        private string ValidateResponse(string messageSid)
        {
            log.LogMethodEntry(messageSid);

            var message = MessageResource.Fetch(pathSid: messageSid);
            if (message.ErrorMessage == null)
            {
                log.LogMethodExit("Success");
                return "Success";
            }
            else
            {
                log.LogMethodExit(message.ErrorMessage);
                return message.ErrorMessage;
            }
        }

        /// <summary>
        /// isValidNumber method is to check number is active on WhatsApp or not
        /// </summary>
        /// <param name="sid">Message sid to find the To number is active on WhatsApp or not</param>
        /// <returns>Return true</returns>
        public override bool isValidWhatsAppNumber(string phoneNumber)
        {
            log.LogMethodEntry(phoneNumber);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            TwilioClient.Init(accountSid, authToken);

            var whatsAppRequestMessage = MessageResource.Create(
                     body: "",
                     from: new Twilio.Types.PhoneNumber("whatsapp:" + fromPhoneNumber),
                     provideFeedback: true,
                     to: new Twilio.Types.PhoneNumber("whatsapp:" + phoneNumber)
                 );
            var whatsAppMessage = MessageResource.Fetch(pathSid: whatsAppRequestMessage.Sid);

            if(whatsAppMessage.ErrorCode.ToString()== "63003")
            {
                log.Info("Phone number "+ whatsAppMessage.To.ToString().Split(':').Last()+ " is not active on WhatsApp");
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }
        public void setCountryCodes()
        {
            log.LogMethodEntry();

            countryCodeList.Add("AFGHANISTAN", "+93");
            countryCodeList.Add("ARGENTINA", "+54");
            countryCodeList.Add("AUSTRALIA", "+61");
            countryCodeList.Add("AUSTRIA", "+43");
            countryCodeList.Add("BAHRAIN", "+973");
            countryCodeList.Add("BANGLADESH", "+880");
            countryCodeList.Add("BELGIUM", "+32");
            countryCodeList.Add("BHUTAN", "+975");
            countryCodeList.Add("BRAZIL", "+55");
            countryCodeList.Add("CANADA", "+1");
            countryCodeList.Add("CHILE", "+56");
            countryCodeList.Add("CHINA", "+86");
            countryCodeList.Add("COLOMBIA", "+57");
            countryCodeList.Add("CROATIA", "+383");
            countryCodeList.Add("DENMARK", "+45");
            countryCodeList.Add("FINLAND", "+358");
            countryCodeList.Add("FRANCE", "+33");
            countryCodeList.Add("GERMANY", "+49");
            countryCodeList.Add("GREECE", "+30");
            countryCodeList.Add("HONG KONG", "+852");
            countryCodeList.Add("HUNGARY", "+36");
            countryCodeList.Add("ICELAND", "+354");
            countryCodeList.Add("INDIA", "+91");
            countryCodeList.Add("INDONESIA", "+62");
            countryCodeList.Add("IRAN", "+98");
            countryCodeList.Add("IRAQ", "+964");
            countryCodeList.Add("IRELAND", "+353");
            countryCodeList.Add("ITALY", "+39");
            countryCodeList.Add("JAPAN", "+81");
            countryCodeList.Add("KUWAIT", "+965");
            countryCodeList.Add("MALAYSIA", "+60");
            countryCodeList.Add("MALI", "+223");
            countryCodeList.Add("MAURITIUS", "+230");
            countryCodeList.Add("MEXICO", "+52");
            countryCodeList.Add("MYANMAR", "+95");
            countryCodeList.Add("NEPAL", "+977");
            countryCodeList.Add("NETHERLANDS", "+31");
            countryCodeList.Add("NEW ZEALAND", "+64");
            countryCodeList.Add("NORTH KOREA", "+850");
            countryCodeList.Add("NORWAY", "+47");
            countryCodeList.Add("OMAN", "+968");
            countryCodeList.Add("PERU", "+51");
            countryCodeList.Add("PHILIPPINES", "+63");
            countryCodeList.Add("POLAND", "+48");
            countryCodeList.Add("PORTUGAL", "+353");
            countryCodeList.Add("QATAR", "+974");
            countryCodeList.Add("ROMANIA", "+40");
            countryCodeList.Add("RUSSIA", "+7");
            countryCodeList.Add("SAUDI ARABIA", "+966");
            countryCodeList.Add("SINGAPORE", "+65");
            countryCodeList.Add("SOUTH AFRICA", "+27");
            countryCodeList.Add("SOUTH KOREA", "+82");
            countryCodeList.Add("SPAIN", "+34");
            countryCodeList.Add("SRILANKA", "+94");
            countryCodeList.Add("SWEDEN", "+46");
            countryCodeList.Add("THAILAND", "+66");
            countryCodeList.Add("TURKEY", "+90");
            countryCodeList.Add("UKARAIN", "+380");
            countryCodeList.Add("UAE", "+971");
            countryCodeList.Add("UK", "+44");
            countryCodeList.Add("USA", "+1");


            log.LogMethodExit();
        }

    }
}
