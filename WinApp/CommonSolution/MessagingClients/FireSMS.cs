using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.MessagingClients
{
    class FireSMS : MessagingClientBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string countryCode;
        string userName;
        string password;
        string url;
        string gatewayFrom;
        string gatewayTo;
        string gatewayText;
        string phoneNumberLength;
        int phoneLength;
        private MessagingClientDTO messagingClientDTO;
        private ExecutionContext executionContext;
        private static Dictionary<string, string> countryCodeList = new Dictionary<string, string>();
        public FireSMS(ExecutionContext executionContext, MessagingClientDTO messagingClientDTO) : base(executionContext, messagingClientDTO)
        {
            log.LogMethodEntry(executionContext, messagingClientDTO);
            this.executionContext = executionContext;
            this.messagingClientDTO = messagingClientDTO;
            Initialize();
            if (countryCodeList.Count == 0)
            {
                setCountryCodes();
            }
            phoneNumberLength = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_PHONE_NUMBER_WIDTH");
            phoneLength = string.IsNullOrEmpty(phoneNumberLength) ? 0 : Convert.ToInt32(phoneNumberLength);
            log.LogMethodExit(null);
        }

        public void Initialize()
        {
            log.LogMethodEntry();
            userName = messagingClientDTO.UserName;
            password = messagingClientDTO.Password;
            url = messagingClientDTO.HostUrl;
            gatewayFrom = messagingClientDTO.Sender;
            log.LogMethodExit();
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
        public bool InitializeCountry()
        {
            log.LogMethodEntry();
            string countryName = string.Empty;

            countryName = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE_LOOKUP_FOR_COUNTRY");
            if (!string.IsNullOrEmpty(countryName) && countryName != "-1")
            {
                List<CountryDTO> countryList = GetCountryList(countryName);

                if (countryList != null && countryList.Any())
                {
                    log.Debug("Country Name: " + countryList[0].CountryName.ToUpper());
                    if (countryCodeList.ContainsKey(countryList[0].CountryName.ToUpper()))
                    {
                        countryCode = countryCodeList[countryList[0].CountryName.ToUpper()];
                    }
                }
            }
            else
            {
                //string siteName = string.Empty;
                Site.Site site = new Site.Site(executionContext.GetSiteId());

                if (site.getSitedTO != null)
                {
                    log.Debug("Country Name: " + site.getSitedTO.Country.ToUpper());
                    if (countryCodeList.ContainsKey(site.getSitedTO.Country.ToUpper()))
                    {
                        countryCode = countryCodeList[site.getSitedTO.Country.ToUpper()];
                    }
                }
            }
            //SiteDTO siteDTO = new SiteDTO();
            //string siteName = string.Empty;

            //SiteList siteList = new SiteList(executionContext);
            //List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
            //searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters);
            //if (siteDTOList != null && siteDTOList.Any())
            //{
            //    log.Debug("Country Name: " + siteDTOList[0].Country.ToUpper());
            //    if (countryCodeList.ContainsKey(siteDTOList[0].Country.ToUpper()))
            //    {
            //        countryCode = countryCodeList[siteDTOList[0].Country.ToUpper()];
            //    }
            //}
            log.Debug("countryCode: " + countryCode);
            log.LogMethodExit(true);
            return true;
        }

        public override MessagingRequestDTO Send(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry(messagingRequestDTO);

            string response = string.Empty;
            gatewayTo = messagingRequestDTO.ToMobile;

            UTF8Encoding encoder = new UTF8Encoding();
            try
            {
                //Validate Inputs
                if (string.IsNullOrWhiteSpace(gatewayTo))
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext, "Phone Number"));//&1 details are missing
                    throw new ArgumentNullException(msg);
                }
                if (gatewayTo.StartsWith("0") || gatewayTo.StartsWith("+"))
                {
                    gatewayTo = gatewayTo.Substring(1);
                }
                if (phoneLength > 0 && gatewayTo.Length > phoneLength)
                {
                    gatewayTo = gatewayTo.Substring(gatewayTo.Length - phoneLength);
                }
                if (!string.IsNullOrWhiteSpace(messagingRequestDTO.CountryCode))
                {
                    countryCode = messagingRequestDTO.CountryCode;
                }
                else
                {
                    InitializeCountry();
                }
                if (!string.IsNullOrEmpty(countryCode))
                {
                    countryCode = countryCode.StartsWith("+") ? countryCode.Substring(1) : countryCode;
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext, "Country Code"));//&1 details are missing
                    throw new ArgumentNullException(msg);
                }


                if (!gatewayTo.StartsWith(countryCode))
                {
                    gatewayTo = countryCode + gatewayTo;
                }

                string param = HttpUtility.UrlEncode("gw-username") + "=" + HttpUtility.UrlEncode(userName);
                param += "&" + HttpUtility.UrlEncode("gw-password") + "=" + HttpUtility.UrlEncode(password);
                param += "&" + HttpUtility.UrlEncode("gw-from") + "=" + HttpUtility.UrlEncode(gatewayFrom);
                param += "&" + HttpUtility.UrlEncode("gw-to") + "=" + HttpUtility.UrlEncode(gatewayTo);
                param += "&" + HttpUtility.UrlEncode("gw-text") + "=" + HttpUtility.UrlEncode(messagingRequestDTO.Body);

                WebRequest wr = WebRequest.Create(url);
                wr.Method = "POST";
                wr.ContentType = "application/x-www-form-urlencoded";
                byte[] data = encoder.GetBytes(param);
                wr.ContentLength = data.Length;
                wr.GetRequestStream().Write(data, 0, data.Length);

                using (HttpWebResponse resp = (HttpWebResponse)wr.GetResponse())
                {
                    using (Stream resp_stream = resp.GetResponseStream())
                    {
                        using (StreamReader read_stream = new StreamReader(resp_stream, Encoding.UTF8))
                        {
                            response = read_stream.ReadToEnd();
                        }
                    }
                }
                string[] segments = response.Split('&');
                NameValueCollection Params = new NameValueCollection();
                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        Params.Add(Key, Value);
                    }
                }
                if (Params["status"] == "0")
                {
                    messagingRequestDTO.SendDate = ServerDateTime.Now;
                    base.UpdateResults(messagingRequestDTO, "Success", Params["msgid"]);
                }
                else
                {
                    base.UpdateResults(messagingRequestDTO, "Error", Params["err_msg"]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in sendSMSSynchronous", ex);
                log.LogMethodExit(null, "Throwing ApplicationException- " + messagingRequestDTO.ToMobile + ": " + ex.Message);
                throw new ApplicationException(messagingRequestDTO.ToMobile + ": " + ex.Message);
            }


            log.LogMethodExit(messagingRequestDTO);
            return messagingRequestDTO;
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
