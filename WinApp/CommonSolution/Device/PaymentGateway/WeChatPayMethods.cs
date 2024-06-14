using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Semnox.Core.Utilities;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Security.Cryptography;

using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    public class WeChatPayMethods
    {
     
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        WeChatConfiguration wechatConfiguration = new WeChatConfiguration();

        Utilities utilities;

        public WeChatPayMethods(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            utilities = _utilities;
            Initailize();            
            log.LogMethodExit(null);
        }
        
        public void Initailize()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WECHAT_PAY_CONFIGURATION"));
                List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                if (LookupValuesDTOList != null)
                {
                    foreach (LookupValuesDTO lookupValueDTO in LookupValuesDTOList)
                    {

                        switch (lookupValueDTO.LookupValue)
                        {
                            case "WECHAT_APP_ID":
                                wechatConfiguration.WECHAT_APP_ID = lookupValueDTO.Description;
                                break;
                            case "WECHAT_VENDOR_ID":
                                wechatConfiguration.WECHAT_VENDOR_ID = lookupValueDTO.Description;
                                break;
                            case "WECHAT_APP_KEY":
                                wechatConfiguration.WECHAT_APP_KEY = lookupValueDTO.Description;
                                break;
                            case "MICRO_PAY_URL":
                                wechatConfiguration.MICRO_PAY_URL = lookupValueDTO.Description;
                                break;
                            case "QUERY_ORDER_URL":
                                wechatConfiguration.QUERY_ORDER_URL = lookupValueDTO.Description;
                                break;
                            case "REVOKE_ORDER_URL":
                                wechatConfiguration.REVOKE_ORDER_URL = lookupValueDTO.Description;
                                break;
                            case "REFUND_URL":
                                wechatConfiguration.REFUND_URL = lookupValueDTO.Description;
                                break;
                            case "SSLCERT_PASSWORD":
                                wechatConfiguration.SSLCERT_PASSWORD = lookupValueDTO.Description;
                                break;

                        }
                    }
                }




                //wechatConfiguration.WECHAT_APP_ID = GetConfigValue("WECHAT_APP_ID");
                //wechatConfiguration.WECHAT_VENDOR_ID = GetConfigValue("WECHAT_VENDOR_ID");
                //wechatConfiguration.WECHAT_APP_KEY = GetConfigValue("WECHAT_APP_KEY");
                //wechatConfiguration.MICRO_PAY_URL = GetConfigValue("MICRO_PAY_URL");
                //wechatConfiguration.QUERY_ORDER_URL = GetConfigValue("QUERY_ORDER_URL");
                //wechatConfiguration.REVOKE_ORDER_URL = GetConfigValue("REVOKE_ORDER_URL");
                //wechatConfiguration.REFUND_URL = GetConfigValue("REFUND_URL");
                //wechatConfiguration.SSLCERT_PATH = "cert/apiclient_cert.p12";
                //wechatConfiguration.SSLCERT_PASSWORD = GetConfigValue("SSLCERT_PASSWORD");
                //wechatConfiguration.NOTIFY_URL = GetConfigValue("NOTIFY_URL");
                //wechatConfiguration.IP = GetConfigValue("IP");

            }
            catch (Exception ex)
            {
                log.Error("Error occured during initialization", ex);
                //log exception
                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }
        }

        public TransactionPaymentsDTO DoPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            WeChatPayDTO weChatPayDTO = BuildWeChatPayDTO(transactionPaymentsDTO);
            weChatPayDTO.AuthorizationCode = transactionPaymentsDTO.CreditCardNumber;

            //SortedDictionary<string, object> responseList = new SortedDictionary<string, object>();

            weChatPayDTO.PaymentTypeSelected = WeChatPayDTO.PaymentType.PAY;

            List<KeyValuePair<string, object>> keyList = GenerateParams(weChatPayDTO);
            validate(keyList);

            WeChatPayResponseDTO WechatPayResponseDTO = new WeChatPayResponseDTO();
            int timeCost = 0;
            int code = SendOrder(ref WechatPayResponseDTO, weChatPayDTO, wechatConfiguration.MICRO_PAY_URL, out timeCost);

            int status = code;

            if (code == 2)
            {
                for (int i = 0; i < timeCost % 10 && i < 3; i++)
                {
                    System.Threading.Thread.Sleep(5000);

                    weChatPayDTO.PaymentTypeSelected = WeChatPayDTO.PaymentType.QUERY_ORDER;

                    status = SendOrder(ref WechatPayResponseDTO, weChatPayDTO, wechatConfiguration.QUERY_ORDER_URL, out timeCost);
                    if (code == 1 && code == 0)
                    {

                        status = SendRevoke(ref WechatPayResponseDTO, weChatPayDTO, wechatConfiguration.REVOKE_ORDER_URL);
                        //validate for revoke.....!!!!
                        i = 3;

                    }
                }
            }

            if (status == 3)
            {
                string message = WechatPayResponseDTO.ErrorCodeDescription;//  GetValue("message", responseList).ToString();
                log.LogMethodExit(null, "Throwing Exception"+message);
                throw new Exception(message);

            }
            transactionPaymentsDTO = BuildWeChatResponse(WechatPayResponseDTO, transactionPaymentsDTO);
            if (status == 0)
            {
                string message = string.IsNullOrEmpty(WechatPayResponseDTO.ErrorCodeDescription)? "Payment Failed." : WechatPayResponseDTO.ErrorCodeDescription;//  GetValue("message", responseList).ToString();
                log.LogMethodExit(null, "Throwing Exception" + message);
                throw new Exception(message);

            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }


        public TransactionPaymentsDTO DoRefund(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            WeChatPayDTO weChatPayDTO = BuildWeChatPayDTO(transactionPaymentsDTO);
            weChatPayDTO.RefundAmount = transactionPaymentsDTO.Amount*100;
            weChatPayDTO.RefundCurrencyType = transactionPaymentsDTO.CurrencyCode;
            weChatPayDTO.RefundVendorId = transactionPaymentsDTO.TransactionId.ToString();
            weChatPayDTO.PaymentTypeSelected = WeChatPayDTO.PaymentType.REFUND;

            WeChatPayResponseDTO WechatPayResponseDTO = new WeChatPayResponseDTO();

            int timeCost = 0;
            int code = RefundOrder(ref WechatPayResponseDTO, weChatPayDTO, wechatConfiguration.REFUND_URL, out timeCost);

            if (code == 3)
            {
                string message = WechatPayResponseDTO.ErrorCodeDescription;
                log.LogMethodExit(null,"Throwing Exception"+message);
                throw new Exception(message);

            }
         

            transactionPaymentsDTO = BuildWeChatResponse(WechatPayResponseDTO, transactionPaymentsDTO);
            if (code == 0)
            {
                string message = string.IsNullOrEmpty(WechatPayResponseDTO.ErrorCodeDescription) ? "Payment Failed." : WechatPayResponseDTO.ErrorCodeDescription;//  GetValue("message", responseList).ToString();
                log.LogMethodExit(null, "Throwing Exception" + message);
                throw new Exception(message);

            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }


        #region APICALLS

        /**********************************************API -- CALLS*********************************************/
        public int SendRevoke(ref WeChatPayResponseDTO wechatPayResponseDTO, WeChatPayDTO weChatPayDTO, string addressRevoke)
        {
            log.LogMethodEntry(wechatPayResponseDTO, weChatPayDTO, addressRevoke);
            weChatPayDTO.PaymentTypeSelected = WeChatPayDTO.PaymentType.CANCEL;
            List<KeyValuePair<string, object>> keyList = GenerateParams(weChatPayDTO);
            string urlstring = ToUrl(keyList);

            weChatPayDTO.Signature = GenerateHash(urlstring, wechatConfiguration.WECHAT_APP_SECRET_KEY);
            keyList.Add(new KeyValuePair<string, object>("sign", weChatPayDTO.Signature));
            string xml = ToXml(keyList);
            string response = HttpService.Send(xml, addressRevoke, false, 10, wechatConfiguration);
            //WechatPayResponseDTO wechatPayResponseDTO = new WechatPayResponseDTO();
            int revokecode = GetStatusCode(ref wechatPayResponseDTO, weChatPayDTO, response);
            log.LogVariableState("wechatpayresponsedto", wechatPayResponseDTO);
            log.LogMethodExit(revokecode);
            return revokecode;
        }


        public int SendOrder(ref WeChatPayResponseDTO wechatPayResponseDTO, WeChatPayDTO weChatPayDTO, string address, out int timeCost)
        {
            log.LogMethodEntry(wechatPayResponseDTO, weChatPayDTO, address);

            List<KeyValuePair<string, object>> keyList = GenerateParams(weChatPayDTO);
            string urlstring = ToUrl(keyList);
            // string sandKey = GetSandBoxKey(weChatPayDTO);


            weChatPayDTO.Signature = GenerateHash(urlstring, weChatPayDTO.AppKey);
            keyList.Add(new KeyValuePair<string, object>("sign", weChatPayDTO.Signature));
            string xml = ToXml(keyList);

            var start = ServerDateTime.Now;
            string response = HttpService.Send(xml, address, false, 10, wechatConfiguration);

            var end = ServerDateTime.Now;
            timeCost = (int)((end - start).TotalMilliseconds);// 

            int code = GetStatusCode(ref wechatPayResponseDTO, weChatPayDTO, response);
            log.LogVariableState("wechatPayResponseDTO", wechatPayResponseDTO);
            log.LogMethodExit(code);
            return code;
        }

        public string GetSandBoxKey(WeChatPayDTO weChatPayIntergrationDTO)
        {
            log.LogMethodEntry(weChatPayIntergrationDTO);
            string address = "https://api.mch.weixin.qq.com/sandboxnew/pay/getsignkey";
            List<KeyValuePair<string, object>> keyList = new List<KeyValuePair<string, object>>();
            keyList.Add(new KeyValuePair<string, object>("appid", weChatPayIntergrationDTO.AppId));
            keyList.Add(new KeyValuePair<string, object>("nonce_str", Guid.NewGuid().ToString().Replace("-", "")));

            string urlstring = ToUrl(keyList);

            weChatPayIntergrationDTO.Signature = GenerateHash(urlstring, wechatConfiguration.WECHAT_APP_KEY);

            string xml = ToXml(keyList);
            string response = HttpService.Send(xml, address, false, 10, wechatConfiguration);

            if (string.IsNullOrEmpty(xml))
            {
                log.LogMethodExit(null, "Throwing Exception Empty xml");
                throw new Exception("Empty xml");
            }
            WeChatPayResponseDTO wechatPayResponseDTO = new WeChatPayResponseDTO();
            string sandbox_signkey = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                // m_values[xe.Name] = (object)xe.InnerText;// 

                switch (xe.Name)
                {
                    case "sandbox_signkey":
                        sandbox_signkey = xe.InnerText;
                        break;

                    default: break;

                }
            }
            log.LogMethodExit(sandbox_signkey);
            return sandbox_signkey;
        }
        public int RefundOrder(ref WeChatPayResponseDTO wechatPayResponseDTO, WeChatPayDTO weChatPayDTO, string address, out int timeCost)
        {
            log.LogMethodEntry(wechatPayResponseDTO, weChatPayDTO, address);
            List<KeyValuePair<string, object>> keyList = GenerateParams(weChatPayDTO);
            string urlstring = ToUrl(keyList);

            weChatPayDTO.Signature = GenerateHash(urlstring, wechatConfiguration.WECHAT_APP_KEY);
            keyList.Add(new KeyValuePair<string, object>("sign", weChatPayDTO.Signature));
            string xml = ToXml(keyList);

            var start = ServerDateTime.Now;
            string response = HttpService.Send(xml, address, true, 10, wechatConfiguration);

            var end = ServerDateTime.Now;
            timeCost = (int)((end - start).TotalMilliseconds);// 

            int code = GetStatusCode(ref wechatPayResponseDTO, weChatPayDTO, response);
            log.LogVariableState("wechatPayResponseDTO", wechatPayResponseDTO);
            log.LogMethodExit(code);
            return code;
        }



        #endregion



        #region COMMON_METHODS

        /**********************************************GENERAL METHODS*********************************************/

        public int GetStatusCode(ref WeChatPayResponseDTO wechatPayResponseDTO, WeChatPayDTO weChatPayDTO, string response)
        {

            log.LogMethodEntry(wechatPayResponseDTO, weChatPayDTO, response);
            wechatPayResponseDTO = GenerateResponseParams(weChatPayDTO, response);
            wechatPayResponseDTO.XmlData = response;
            //     responseList = new SortedDictionary<string, object>();
            string returnCode = wechatPayResponseDTO.ReturnCode;
            string resultCode = wechatPayResponseDTO.ResultCode; ;

            if (!string.IsNullOrEmpty(returnCode) && returnCode.ToLower() == "success" &&
                !string.IsNullOrEmpty(resultCode) && resultCode.ToLower() == "success")
            {
                log.LogVariableState("wechatPayResponsedto", wechatPayResponseDTO);
                log.LogMethodExit(1);
                return 1;
            }
            else
            {
                string tradeState = wechatPayResponseDTO.TradeState;

                if (!string.IsNullOrEmpty(tradeState) && tradeState.ToLower() == "userpaying")
                {
                    if (CheckSign(wechatPayResponseDTO, weChatPayDTO))
                    {
                        log.LogMethodExit(null, "Throwing Exception-Signature doesnot match");
                        throw new Exception("Signature doesnot match");
                        //NOTE :  CHECK !!!
                    }
                    log.LogVariableState("wechatPayResponsedto", wechatPayResponseDTO);
                    log.LogMethodExit(2);
                    return 2;
                }
                log.LogVariableState("wechatPayResponsedto", wechatPayResponseDTO);
                log.LogMethodExit(0);
                return 0;
            }
           
        }

        private List<KeyValuePair<string, object>> GenerateParams(WeChatPayDTO weChatPayIntergrationDTO)
        {
            log.LogMethodEntry(weChatPayIntergrationDTO);
            // IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            List<KeyValuePair<string, object>> postparamslist = new List<KeyValuePair<string, object>>();
            Random rnd = new Random(10);
            postparamslist.Add(new KeyValuePair<string, object>("appid", weChatPayIntergrationDTO.AppId));
            postparamslist.Add(new KeyValuePair<string, object>("mch_id", weChatPayIntergrationDTO.VendorId));
            postparamslist.Add(new KeyValuePair<string, object>("nonce_str", weChatPayIntergrationDTO.RandomString));

            if (!string.IsNullOrEmpty(weChatPayIntergrationDTO.SubVendorId))
                postparamslist.Add(new KeyValuePair<string, object>("sub_mch_id", weChatPayIntergrationDTO.SubVendorId));

            //postparamslist.Add(new KeyValuePair<string, object>("sign_type", weChatPayIntergrationDTO.SignatureType));
            //postparamslist.Add(new KeyValuePair<string, object>("key", weChatPayIntergrationDTO.AppKey));
            if (weChatPayIntergrationDTO.PaymentTypeSelected == WeChatPayDTO.PaymentType.PAY)
            {

                postparamslist.Add(new KeyValuePair<string, object>("out_trade_no", weChatPayIntergrationDTO.WechatTransactionId));
                postparamslist.Add(new KeyValuePair<string, object>("body", weChatPayIntergrationDTO.ItemDescription));
                postparamslist.Add(new KeyValuePair<string, object>("total_fee", (Convert.ToDouble(weChatPayIntergrationDTO.Amount))));
                //postparamslist.Add(new KeyValuePair<string, object>("trade_type", weChatPayIntergrationDTO.TradeType));
                //postparamslist.Add(new KeyValuePair<string, string>("order_description", this.OrderDescription));
                // postparamslist.Add(new KeyValuePair<string, object>("notify_url", weChatPayIntergrationDTO.NotifyUrl));
                postparamslist.Add(new KeyValuePair<string, object>("auth_code", weChatPayIntergrationDTO.AuthorizationCode));
                // postparamslist.Add(new KeyValuePair<string, object>("spbill_create_ip", weChatPayIntergrationDTO.TerminalIP));
            }

            if (weChatPayIntergrationDTO.PaymentTypeSelected == WeChatPayDTO.PaymentType.CANCEL ||
                weChatPayIntergrationDTO.PaymentTypeSelected == WeChatPayDTO.PaymentType.QUERY_ORDER)
            {

                postparamslist.Add(new KeyValuePair<string, object>("out_trade_no",weChatPayIntergrationDTO.WechatTransactionId));
            }
            if (weChatPayIntergrationDTO.PaymentTypeSelected == WeChatPayDTO.PaymentType.REFUND)
            {
                postparamslist.Add(new KeyValuePair<string, object>("total_fee", (Convert.ToDouble(weChatPayIntergrationDTO.Amount))));
                postparamslist.Add(new KeyValuePair<string, object>("refund_fee", weChatPayIntergrationDTO.RefundAmount));
                //  postparamslist.Add(new KeyValuePair<string, object>("refund_fee_type", weChatPayIntergrationDTO.CurrencyType));
                postparamslist.Add(new KeyValuePair<string, object>("op_user_id", weChatPayIntergrationDTO.VendorId));

            
                if (!string.IsNullOrEmpty(weChatPayIntergrationDTO.WechatTransactionId))
                    postparamslist.Add(new KeyValuePair<string, object>("out_trade_no", weChatPayIntergrationDTO.WechatTransactionId));  //Pos transaction
                if (!string.IsNullOrEmpty(weChatPayIntergrationDTO.VendorOrderNumber))
                    postparamslist.Add(new KeyValuePair<string, object>("transaction_id", weChatPayIntergrationDTO.VendorOrderNumber));  //Wechat TransactionId

              
                if (!string.IsNullOrEmpty(weChatPayIntergrationDTO.WechatRefundNumber))
                    postparamslist.Add(new KeyValuePair<string, object>("out_refund_no", weChatPayIntergrationDTO.WechatRefundNumber));
                else
                {
                    postparamslist.Add(new KeyValuePair<string, object>("out_refund_no", GenerateOutTradeNo()));
                }
                
                

            }
            else
            {
                postparamslist.Add(new KeyValuePair<string, object>("fee_type", weChatPayIntergrationDTO.CurrencyType));
            }
            log.LogMethodExit(postparamslist);
            return postparamslist;
        }



        private string GetMandatoryConfigValue(string key)
        {
            log.LogMethodEntry(key);
            string value = "";// utilities.getParafaitDefaults(key);
            if (string.IsNullOrEmpty(value))
            {
                log.LogMethodExit(null,"Throwing Exception"+ key + " is mandatory field");
                throw new Exception(key + " is mandatory field");
            }
            log.LogMethodExit(value);
            return value;
        }

        /// <summary>
        /// validate Method
        /// </summary>
        /// <param name="postparamslist"> postparamslist</param>
        private void validate(List<KeyValuePair<string, object>> postparamslist)
        {
            log.LogMethodEntry(postparamslist);
            List<string> mandatoryFields = new List<string>();
            mandatoryFields.Add("command");
            mandatoryFields.Add("appid");
            mandatoryFields.Add("mch_id");
            //  mandatoryFields.Add("sub_mch_id");
            mandatoryFields.Add("nonce_str");
            //mandatoryFields.Add("sign_type");
            //mandatoryFields.Add("key");
            //mandatoryFields.Add("body");
            //mandatoryFields.Add("total_fee");
            //mandatoryFields.Add("fee_type ");
            //mandatoryFields.Add("out_trade_no");
            //mandatoryFields.Add("trade_type");
            //mandatoryFields.Add("notify_url");
            //mandatoryFields.Add("out_trade_no");
            //mandatoryFields.Add("auth_code");
            //mandatoryFields.Add("spbill_create_ip");
            //mandatoryFields.Add("trade_type");

            foreach (string mandatoryField in mandatoryFields)
            {
                foreach (KeyValuePair<string, object> keyValue in postparamslist)
                {
                    if (keyValue.Key == mandatoryField)
                    {
                        if (keyValue.Value == null && string.IsNullOrEmpty(keyValue.Value.ToString()))
                        {
                            log.LogMethodExit(null,"Throwing Exception"+ keyValue.Key + " is mandatory field"); 
                            throw new Exception(keyValue.Key + " is mandatory field");
                        }
                    }
                }
            }
            log.LogMethodExit(null);
        }

        static int Compare1(KeyValuePair<string, object> a, KeyValuePair<string, object> b)
        {
            log.LogMethodEntry(a, b);
            int returnvalue = a.Key.CompareTo(b.Key);
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }



        private string ToUrl(List<KeyValuePair<string, object>> m_values)
        {
            log.LogMethodEntry(m_values);
            string buff = "";
            m_values.Sort(Compare1);
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    log.LogMethodExit(null, "Throwing exception -Invalid Data");
                    throw new Exception("Invalid data");
                }
                // if (pair.Key != "sign" && pair.Value.ToString() != "" && pair.Key.ToString() != "sub_mch_id")
                if (pair.Key != "sign" &&
                    //pair.Key != "auth_code" &&
                    // pair.Key != "fee_type" &&

                    //   pair.Key != "out_trade_no" &&
                    //    pair.Key != "sub_mch_id" &&
                    //     pair.Key != "total_fee" && 
                    pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            log.LogMethodExit(buff);
            return buff;
        }

        private string ToXml(List<KeyValuePair<string, object>> m_values)
        {
            log.LogMethodEntry(m_values);
            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(double))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                    //Log.Error(this.GetType().ToString(), " !");
                    //throw new WxPayException(" !");
                }
            }
            xml += "</xml>";
            log.LogMethodExit(xml);
            return xml;
        }

        private string GenerateHash(string url, string key)
        {
            log.LogMethodEntry(url, key);
            //MD5
            url += "&key=" + key;
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            string returnvalue= sb.ToString().ToUpper();
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }



        private WeChatPayResponseDTO GenerateResponseParams(WeChatPayDTO weChatPayIntergrationDTO, string xml)
        {
            log.LogMethodEntry(weChatPayIntergrationDTO, xml);
            //            SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
            if (string.IsNullOrEmpty(xml))
            {
                log.LogMethodExit(null, "Throwing exception empty xml");
                throw new Exception("Empty xml");
            }
            WeChatPayResponseDTO wechatPayResponseDTO = new WeChatPayResponseDTO();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                // m_values[xe.Name] = (object)xe.InnerText;// 

                switch (xe.Name)
                {
                    case "appid":
                        wechatPayResponseDTO.AppId = xe.InnerText;
                        break;
                    case "mch_id":
                        wechatPayResponseDTO.VendorId = xe.InnerText;
                        break;
                    case "nonce_str":
                        wechatPayResponseDTO.RandomString = xe.InnerText;
                        break;
                    case "out_trade_no":
                        wechatPayResponseDTO.WechatTransactionId = xe.InnerText;
                        break;
                    case "total_fee":
                        wechatPayResponseDTO.Amount = Convert.ToDouble(xe.InnerText);
                        break;
                    case "trade_type":
                        wechatPayResponseDTO.TradeType = xe.InnerText;
                        break;
                    case "result_code":
                        wechatPayResponseDTO.ResultCode = xe.InnerText;
                        break;
                    case "return_code":
                        wechatPayResponseDTO.ReturnCode = xe.InnerText;
                        break;
                    case "sign":
                        wechatPayResponseDTO.Signature = xe.InnerText;
                        break;
                    case "err_code":
                        wechatPayResponseDTO.ErrorCode = xe.InnerText;
                        break;
                    case "err_code_des":
                        wechatPayResponseDTO.ErrorCodeDescription = xe.InnerText;
                        break;
                    case "openid":
                        wechatPayResponseDTO.UserTag = xe.InnerText;
                        break;
                    case "recall":
                        wechatPayResponseDTO.Recall = xe.InnerText;
                        break;
                    case "return_msg":
                        wechatPayResponseDTO.ResultCodeDescription = xe.InnerText;
                        break;
                    case "trade_state":
                        wechatPayResponseDTO.TradeState = xe.InnerText;
                        break;
                    case "transaction_id":
                        wechatPayResponseDTO.VendorOrderNumber = xe.InnerText;
                        break;
                    case "out_refund_no":
                        wechatPayResponseDTO.WechatRefundNumber = xe.InnerText;
                        break;
                    case "refund_id":
                       // wechatPayResponseDTO.WechatRefundNumber = xe.InnerText;
                        break;
                    case "refund_fee":
                        wechatPayResponseDTO.RefundAmount = Convert.ToDouble(xe.InnerText);
                        break;
                    case "refund_fee_type":
                        wechatPayResponseDTO.RefundCurrencyType = xe.InnerText;
                        break;
                    default: break;

                }
            }
            log.LogMethodExit(wechatPayResponseDTO);
            return wechatPayResponseDTO;
        }

        public bool CheckSign(WeChatPayResponseDTO wechatPayResponseDTO, WeChatPayDTO weChatPayIntergrationDTO)
        {
            log.LogMethodEntry(wechatPayResponseDTO, weChatPayIntergrationDTO);
            object ob = wechatPayResponseDTO.Signature;
            string return_sign = "";

            if (null != ob)
            {
                return_sign = ob.ToString();
            }
            if (!string.IsNullOrEmpty(return_sign) && weChatPayIntergrationDTO.Signature == return_sign)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
           

        }

        public static string GenerateTransactionId(string number)
        {
            log.LogMethodEntry(number);
            var ran = new Random();
            string returnvalue= string.Format("{0}{1}{2}", number, ServerDateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }


        private TransactionPaymentsDTO BuildWeChatResponse(WeChatPayResponseDTO wechatPayResponseDTO, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(wechatPayResponseDTO, transactionPaymentsDTO);

            transactionPaymentsDTO.Amount = wechatPayResponseDTO.Amount/100;
            //  transactionPaymentsDTO.CurrencyCode = wechatPayResponseDTO.CurrencyType;
            try
            {
                transactionPaymentsDTO.OrderId = Convert.ToInt32(wechatPayResponseDTO.VendorOrderNumber);
            }
            catch(Exception ex)
            {
                log.Error("Build We Chat Response at Vendor order id failed", ex);
            }
            if (wechatPayResponseDTO.ResultCode.ToLower() == "fail")
            {
                log.LogMethodExit(null, "Throwing Exception-Failed .Please retry");
                throw new Exception("Failed .Please retry");
            }
          
            if (wechatPayResponseDTO.PaymentTypeSelected == WeChatPayDTO.PaymentType.PAY)
            {
                transactionPaymentsDTO.Reference = string.IsNullOrEmpty(wechatPayResponseDTO.WechatTransactionId) ? null : wechatPayResponseDTO.WechatTransactionId;
            }
            else if (wechatPayResponseDTO.PaymentTypeSelected == WeChatPayDTO.PaymentType.REFUND)
            {
                transactionPaymentsDTO.Reference = string.IsNullOrEmpty(wechatPayResponseDTO.WechatRefundNumber) ? null : wechatPayResponseDTO.WechatRefundNumber;
            }


            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }



        private WeChatPayDTO BuildWeChatPayDTO(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            WeChatPayDTO weChatPayDTO = new WeChatPayDTO();
            weChatPayDTO.Amount = transactionPaymentsDTO.Amount*100;
            weChatPayDTO.CurrencyType = transactionPaymentsDTO.CurrencyCode;
            //weChatPayDTO.VendorOrderNumber = ""  //Pass reference number recieved by wechat
            weChatPayDTO.AppId = wechatConfiguration.WECHAT_APP_ID;
            weChatPayDTO.AppKey = wechatConfiguration.WECHAT_APP_KEY;
            weChatPayDTO.VendorId = wechatConfiguration.WECHAT_VENDOR_ID;
            weChatPayDTO.SubVendorId = wechatConfiguration.WECHAT_SUB_VENDOR_ID;
            weChatPayDTO.TerminalIP = wechatConfiguration.IP;
            weChatPayDTO.AppSecrekKey = wechatConfiguration.WECHAT_APP_SECRET_KEY;
            weChatPayDTO.RandomString = Guid.NewGuid().ToString().Replace("-", "");
            weChatPayDTO.CurrencyType = transactionPaymentsDTO.CurrencyCode;
            weChatPayDTO.RefundCurrencyType = transactionPaymentsDTO.CurrencyCode;
            weChatPayDTO.ItemDescription = string.IsNullOrEmpty(transactionPaymentsDTO.Memo) ? "Default" : transactionPaymentsDTO.CurrencyCode;
            weChatPayDTO.WechatTransactionId = transactionPaymentsDTO.TransactionId.ToString() ;

            log.LogMethodExit(weChatPayDTO);
            return weChatPayDTO;
        }

        #endregion


        //public string GetConfigValue(string key)
        //{
        //    string value = ConfigurationManager.AppSettings[key];

        //    return value;
        //}

        public string GenerateOutTradeNo()
        {
            log.LogMethodEntry();
            var ran = new Random();
            string returnvalue = string.Format("{0}{1}{2}", 1486649472, ServerDateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

    }
}
