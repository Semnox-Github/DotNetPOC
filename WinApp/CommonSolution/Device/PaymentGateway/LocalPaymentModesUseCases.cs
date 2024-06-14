/********************************************************************************************
 * Project Name - Device  
 * Description  - LocalPaymentModesUseCases class to get the data  from API by doing remote call  
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
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class LocalPaymentModesUseCases : LocalUseCases, IPaymentModesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalPaymentModesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPaymentModes
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <returns></returns>
        public async Task<List<PaymentModeDTO>> GetPaymentModes(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords, bool loadActiveChild=false)
        {

            return await Task<List<PaymentModeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords);

                PaymentModeList paymentModesListBL = new PaymentModeList(executionContext);
                //List<int> paymentModeIdList = new List<int>();
                //if (string.IsNullOrWhiteSpace(paymentChannel) == false)
                //{
                //    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                //    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                //    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                //    lookupSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, paymentChannel));
                //    lookupSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                //    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupSearchParameters);
                //    if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                //    {
                //        PaymentChannelList paymentChannelList = new PaymentChannelList(executionContext);
                //        List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchChannelParameters = new List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>>();
                //        searchChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.LOOKUP_VALUE_ID, lookupValuesDTOList[0].LookupValueId.ToString()));
                //        searchChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                //        List<PaymentModeChannelsDTO> paymentChannelsDTOList = paymentChannelList.GetAllPaymentChannels(searchChannelParameters);
                //        if (paymentChannelsDTOList != null && paymentChannelsDTOList.Any())
                //        {
                //            paymentModeIdList = paymentChannelsDTOList.Select(x => x.PaymentModeId).ToList();
                //        }
                //        if (paymentModeIdList.Any())
                //        {
                //            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID_LIST, string.Join(",", paymentModeIdList).ToString()));
                //        }
                //    }
                //    else
                //    {
                //        log.Error("Invalid payment channel");
                //        throw new Exception("Invalid payment channel");
                //    }
                //}
                List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetAllPaymentModeList(searchParameters, loadChildRecords, loadActiveChild);
                log.LogMethodExit(paymentModesDTOList);
                return paymentModesDTOList;
            });
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
            return await Task<PaymentModesContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    PaymentModesContainerList.Rebuild(siteId);
                }
                PaymentModesContainerDTOCollection result = PaymentModesContainerList.GetPaymentModeContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="paymentModeDTOList"></param>
        /// <returns></returns>
        public async Task<string> SavePaymentModes(List<PaymentModeDTO> paymentModeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(paymentModeDTOList);
                if (paymentModeDTOList == null)
                {
                    throw new ValidationException("paymentModeDTOList is Empty");
                }

                try
                {
                    PaymentModeList paymentModeList = new PaymentModeList(executionContext, paymentModeDTOList);
                    paymentModeList.SaveUpdatePaymentModesList();
                }

                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    throw valEx;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw ex;
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
