/********************************************************************************************
 * Project Name - Device  
 * Description  - RemotePaymentModesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      19-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class RemotePaymentModesUseCases : RemoteUseCases, IPaymentModesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PAYMENT_MODE_CONTAINER_URL = "api/Transaction/PaymentModesContainer";
        private const string PAYMENT_MODE_URL = "api/Transaction/PaymentModes";
        /// <summary>
        /// RemotePaymentModesUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemotePaymentModesUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPaymentModesContainerDTOCollection
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public async Task<PaymentModesContainerDTOCollection> GetPaymentModesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            PaymentModesContainerDTOCollection result = await Get<PaymentModesContainerDTOCollection>(PAYMENT_MODE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetPaymentModes
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <returns></returns>
        public async Task<List<PaymentModeDTO>> GetPaymentModes(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> parameters,bool loadChildRecords, bool loadActiveChild=false)
        {
            log.LogMethodEntry(parameters, loadChildRecords);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>(); 
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            try
            {
                List<PaymentModeDTO> result = await Get<List<PaymentModeDTO>>(PAYMENT_MODE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private IEnumerable<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PaymentModeDTO.SearchByParameters, string> searchParameter in parameters)
            {
                switch (searchParameter.Key)
                {
                    case PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("paymentModeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PaymentModeDTO.SearchByParameters.PAYMENT_MODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("paymentMode".ToString(), searchParameter.Value));
                        }
                        break;
                    case PaymentModeDTO.SearchByParameters.PAYMENT_CHANNEL_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("paymentChannel".ToString(), searchParameter.Value));
                        }
                        break;
                    case PaymentModeDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case PaymentModeDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="paymentModeDTOList"></param>
        /// <returns></returns>
        public async Task<string> SavePaymentModes(List<PaymentModeDTO> paymentModeDTOList)
        {
            log.LogMethodEntry(paymentModeDTOList);
            try
            {
                string responseString = await Post<string>(PAYMENT_MODE_URL, paymentModeDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
    }
}
