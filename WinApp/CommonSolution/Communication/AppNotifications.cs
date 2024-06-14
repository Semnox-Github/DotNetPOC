/********************************************************************************************
 * Project Name - Communications
 * Description  - BL class to send Firebase App Notifications.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.100.0     15-Sep-2020     Nitin Pai         Push Notification: Created
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Communication
{
    internal class FirebaseResponse
    {
        public class FirebaseResponseResults
        {
            public string message_id;
            public string registration_id;
            public string error;
        }
        public string multicast_id;
        public string success;
        public string failure;
        public List<FirebaseResponseResults> results;
    }
    public class AppNotifications
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private static ConcurrentDictionary<int, MessagingClientDTO> messagingClientDictionary = new ConcurrentDictionary<int, MessagingClientDTO>();

        public AppNotifications(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public void Initialize()
        {

        }

        public string SendMessage(MessagingRequestDTO messagingRequestDTO)
        {
            log.LogMethodEntry();
            String returnValue = "";
            try
            {
                MessagingClientDTO messagingClientDTO = null;
                if (messagingRequestDTO.MessagingClientId != -1)
                {
                    int messagingClientId = messagingRequestDTO.MessagingClientId;
                    if (messagingClientDictionary.ContainsKey(messagingClientId))
                    {
                        messagingClientDTO = messagingClientDictionary[messagingClientId];
                    }
                    else
                    {
                        MessagingClientBL messagingClientBL = new MessagingClientBL(this.executionContext, messagingClientId);
                        if (messagingClientBL.GetMessagingClientDTO != null && messagingClientBL.GetMessagingClientDTO.MessagingChannelCode == "A")
                        {
                            messagingClientDictionary.TryAdd(messagingClientId, messagingClientBL.GetMessagingClientDTO);
                            messagingClientDTO = messagingClientBL.GetMessagingClientDTO;
                        }
                        else
                        {
                            throw new ValidationException("Messaging client " + messagingClientId + " not setup.");
                        }
                    }
                }
                else
                {
                    // get the default app notification client. This will be the first client set up for app notifications
                    MessagingClientListBL messagingClientListBL = new MessagingClientListBL(executionContext);
                    List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.IS_ACTIVE, "1"));
                    searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.MESSAGING_CHANNEL_CODE, "A"));

                    List<MessagingClientDTO> messagingClientDTOList = messagingClientListBL.GetMessagingClientDTOList(searchParameters);
                    if (messagingClientDTOList != null && messagingClientDTOList.Any())
                    {
                        messagingClientDTO = messagingClientDTOList[0];
                    }
                    else
                    {
                        throw new ValidationException("Notification Messaging client not setup.");
                    }
                }

                PushNotificationDeviceListBL pushNotificationDeviceListBL = new PushNotificationDeviceListBL(this.executionContext);
                List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> pndSearchParameters = new List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>>();
                pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, messagingRequestDTO.CustomerId.ToString()));
                pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE, "1"));
                if(messagingRequestDTO.SignedInCustomersOnly)
                    pndSearchParameters.Add(new KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>(PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_SIGNED_IN, "1"));
                List<PushNotificationDeviceDTO> pushNotificationDeviceDTOList = pushNotificationDeviceListBL.GetPushNotificationDeviceDTOList(pndSearchParameters);

                if (pushNotificationDeviceDTOList != null && pushNotificationDeviceDTOList.Any())
                {
                    bool noActiveDeviceFound = true;
                    foreach (PushNotificationDeviceDTO pushDevice in pushNotificationDeviceDTOList)
                    {
                        returnValue += " Sending to:" + pushDevice.Id;
                        String messageBody = messagingRequestDTO.Body.Replace("@ToDevice", pushDevice.PushNotificationToken);
                        messageBody = messageBody.Replace("@MessageId", messagingRequestDTO.Id.ToString());
                        log.Error(messageBody);
                        Dictionary<string, string> responseDictionary = Post(messageBody, messagingClientDTO.HostUrl, messagingClientDTO.UserName, messagingClientDTO.Password, 10);

                        FirebaseResponse response = null;
                        if (responseDictionary != null && responseDictionary.Any())
                        {
                            response = JsonConvert.DeserializeObject<FirebaseResponse>(responseDictionary.FirstOrDefault().Value);
                        }
                        if (responseDictionary.ContainsKey("OK"))
                        {
                            if (!String.IsNullOrEmpty(response.failure) && Convert.ToInt32(response.failure) > 0)
                            {
                                // Send message has failed. Log the error
                                returnValue += " Error";
                                log.Error("ERROR ... in publishInitialReceipt ..." + response.ToString());
                                if (response.results != null && response.results.Any())
                                {
                                    foreach (FirebaseResponse.FirebaseResponseResults result in response.results)
                                    {
                                        log.Error("ERROR ... in publishInitialReceipt ..." + result.error);

                                        // Check if the token is invaid, if yes, Inactivate the devices 
                                        if (result.error.IndexOf("InvalidRegistration", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                           result.error.IndexOf("unregistered", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                                           result.error.IndexOf("notregistered", StringComparison.InvariantCultureIgnoreCase) >= 0)
                                        {
                                            try
                                            {
                                                log.Error("ERROR ... invalid device. update PND to inactive ..." + pushDevice.Id);
                                                PushNotificationDeviceBL pushNotificationDeviceBL = new PushNotificationDeviceBL(executionContext, pushDevice);
                                                pushNotificationDeviceBL.PushNotificationDeviceDTO.IsActive = false;
                                                pushNotificationDeviceBL.Save();
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Debug("Failed while updating PND device " + ex.Message);
                                            }
                                        }
                                        returnValue += ":" + result.error;
                                    }
                                }
                            }
                            else
                            {
                                noActiveDeviceFound = false;
                                returnValue += "success " + response.multicast_id;
                                log.Debug("Successful invocation of Initial Receipt.");
                            }
                        }
                        else
                        {
                            returnValue += " Error";
                            log.Error("ERROR ... in publishInitialReceipt ...");
                            foreach (KeyValuePair<string, string> responseMessage in responseDictionary)
                            {
                                returnValue += " : " + responseMessage.Value;
                                log.Error("\tKey {responseMessage.Key}: Value={responseMessage.Value}");
                            }
                        }
                    }

                    if(noActiveDeviceFound)
                        throw new Exception(returnValue);
                }
                else
                {
                    String error = "Customer is not signed in to any active device";
                    log.Error(error);
                    throw new Exception(error);
                }
            }
            catch (System.Net.WebException webex)
            {
                log.Error("Error occured during publish to fiskaltrust", webex);
            }

            log.LogMethodExit();
            return returnValue;
        }

        /// <summary>
        /// This method is for inserting a new record over a Web API using POST method
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="requestUri"></param>
        /// <param name="sessionToken"></param>
        /// <param name="host"></param>
        /// <param name="readWriteTimeout"></param>
        /// <returns></returns>
        private Dictionary<string, string> Post(string jsonString, string requestUri, string sender, string password, int readWriteTimeout)
        {
            log.LogMethodEntry(jsonString, requestUri, sender, password, readWriteTimeout);
            string jsonResponse = string.Empty;
            Dictionary<string, string> responseDictionary = new Dictionary<string, string>();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            byte[] byteArray = utf8Encoding.GetBytes(jsonString);
            httpWebRequest.Method = "POST";
            if (!string.IsNullOrEmpty(password))
            {
                log.LogVariableState("Session Token for Authorization", password);
                httpWebRequest.Headers.Add("Authorization", "key=" + password);
            }
            if (!string.IsNullOrEmpty(sender))
            {
                log.LogVariableState("Sender", sender);
                httpWebRequest.Headers.Add("Sender", sender);
            }
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = byteArray.Length;
            httpWebRequest.ReadWriteTimeout = readWriteTimeout * 1000; // Covert readWriteTimeout FROM seconds to milliseconds
            using (Stream requestedStreamData = httpWebRequest.GetRequestStream())
            {
                requestedStreamData.Write(byteArray, 0, byteArray.Length);
            }
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    if (HttpStatusCode.OK == httpWebResponse.StatusCode)
                    {
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            log.LogVariableState("Json Response ", jsonResponse);
                            // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                            string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "POST", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                            responseDictionary.Add(httpWebResponse.StatusCode.ToString(), jsonResponse);
                            log.LogVariableState("Response Message - ", responseMessage);
                            log.Debug(responseMessage);
                        }
                    }
                    else
                    {
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            log.LogVariableState("Json Response ", jsonResponse);
                            // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                            string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "POST", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                            log.Error(responseMessage);
                            log.LogVariableState("Forbidden Response - ", responseMessage);
                            responseDictionary.Add(httpWebResponse.StatusCode.ToString(), responseMessage);
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            log.Error(webException);
                            log.LogMethodExit(streamReader.ReadToEnd(), "Throwing Exception : " + webException.Message);
                            throw new Exception(streamReader.ReadToEnd() + "Throwing Exception : " + webException.Message);
                        }
                    }
                }
                else
                {
                    log.Error(webException.Message, webException);
                    log.LogMethodExit(responseDictionary, "Throwing Exception : " + webException.Message);
                    throw webException;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(responseDictionary, "Throwing Exception : " + ex.Message);
                throw ex;
            }
            log.LogMethodExit(responseDictionary);
            return responseDictionary;
        }

    }
}
