/********************************************************************************************
 * Project Name -Communication
 * Description  -RemoteMessagingTriggerUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-May-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    class RemoteMessagingTriggerUseCases:RemoteUseCases,IMessagingTriggerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MESSAGING_TRIGGER_URL = "api/Communication/MessagingTriggers";
        public RemoteMessagingTriggerUseCases(ExecutionContext executionContext)
          : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<MessagingTriggerDTO>> GetMessagingTrigges(List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> searchParameters,
                                           bool loadChildRecords = false, bool activeChildRecords = true,
                                           SqlTransaction sqlTransaction = null)
        {

            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
                if (searchParameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(searchParameters));
                }
                try
                {
                    List<MessagingTriggerDTO> result = await Get<List<MessagingTriggerDTO>>(MESSAGING_TRIGGER_URL, searchParameterList);
                    log.LogMethodExit(result);
                    return result;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MessagingTriggerDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MessagingTriggerDTO.SearchByParameters.TRIGGER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("triggerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.TRIGGER_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("triggerName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.TYPE_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("typeCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.RECEIPT_TEMPLATE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("receiptTemplateId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.MESSAGE_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("messageType".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.SEND_RECEIPT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("sendReceipt".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.EMAIL_SUBJECT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("emailSubject".ToString(), searchParameter.Value));
                        }
                        break;
                    case MessagingTriggerDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMessagingTrigges(List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            log.LogMethodEntry(messagingTriggerDTOList);
            try
            {
                string responseString = await Post<string>(MESSAGING_TRIGGER_URL, messagingTriggerDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            try
            {
                log.LogMethodEntry(messagingTriggerDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(messagingTriggerDTOList);
                string responseString = await Delete(MESSAGING_TRIGGER_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw ;
            }
        }

    }
}
