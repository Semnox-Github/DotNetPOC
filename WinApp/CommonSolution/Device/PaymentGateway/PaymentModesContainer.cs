/********************************************************************************************
 * Project Name - Device
 * Description  - PaymentModesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     03-Sep-2021    Fiona               Created
 *2.150.1     22-Feb-2023    Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModesContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<int, PaymentModesContainerDTO> paymentModesContainerDTODictionary = new Dictionary<int, PaymentModesContainerDTO>();
        private readonly List<PaymentModeDTO> paymentModeDTOList;
        private readonly PaymentModesContainerDTOCollection paymentModesContainerDTOCollection;
        private readonly DateTime? paymentModesModuleLastUpdateTime;
        private readonly int siteId;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public PaymentModesContainer(int siteId): this(siteId, GetPaymentModeDTOList(siteId), GetPaymentModeModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="paymentModeDTOList"></param>
        /// <param name="paymentModesModuleLastUpdateTime"></param>
        public PaymentModesContainer(int siteId, List<PaymentModeDTO> paymentModeDTOList, DateTime? paymentModesModuleLastUpdateTime)
        {
            log.LogMethodEntry();
            this.siteId = siteId;
            this.paymentModeDTOList = paymentModeDTOList;
            this.paymentModesModuleLastUpdateTime = paymentModesModuleLastUpdateTime;
            List<PaymentModesContainerDTO> paymentModeContainerDTOList = new List<PaymentModesContainerDTO>();
           
            if (paymentModeDTOList!=null && paymentModeDTOList.Any())
            {
                foreach (PaymentModeDTO paymentModeDTO in paymentModeDTOList)
                {
                    PaymentModesContainerDTO paymentModesContainerDTO = new PaymentModesContainerDTO(paymentModeDTO.PaymentModeId, paymentModeDTO.PaymentMode, paymentModeDTO.IsCreditCard, paymentModeDTO.CreditCardSurchargePercentage,
                        paymentModeDTO.IsCash, paymentModeDTO.IsDebitCard, paymentModeDTO.Gateway, paymentModeDTO.ManagerApprovalRequired, paymentModeDTO.IsRoundOff, paymentModeDTO.POSAvailable, paymentModeDTO.DisplayOrder,
                        paymentModeDTO.ImageFileName, paymentModeDTO.IsQRCode, paymentModeDTO.PaymentReferenceMandatory, paymentModeDTO.Attribute1, paymentModeDTO.Attribute2, paymentModeDTO.Attribute3, paymentModeDTO.Attribute4,
                        paymentModeDTO.Attribute5, paymentModeDTO.Guid);

                    if (paymentModeDTO.PaymentModeChannelsDTOList != null && paymentModeDTO.PaymentModeChannelsDTOList.Any())
                    {
                        foreach (PaymentModeChannelsDTO paymentModeChannelsDTO in paymentModeDTO.PaymentModeChannelsDTOList)
                        {
                            PaymentModeChannelsContainerDTO paymentModeChannelsContainerDTO = new PaymentModeChannelsContainerDTO(paymentModeChannelsDTO.PaymentModeChannelId, paymentModeChannelsDTO.PaymentModeId, paymentModeChannelsDTO.LookupValueId, paymentModeChannelsDTO.Guid);
                            if (paymentModesContainerDTO.PaymentModeChannelsContainerDTOList == null)
                            {
                                paymentModesContainerDTO.PaymentModeChannelsContainerDTOList = new List<PaymentModeChannelsContainerDTO>();
                            }
                            paymentModesContainerDTO.PaymentModeChannelsContainerDTOList.Add(paymentModeChannelsContainerDTO);
                        }
                    }

                    if (paymentModeDTO.CompatiablePaymentModesDTOList != null && paymentModeDTO.CompatiablePaymentModesDTOList.Any())
                    {
                        foreach(CompatiablePaymentModesDTO compatiablePaymentModesDTO in paymentModeDTO.CompatiablePaymentModesDTOList)
                        {
                            CompatiablePaymentModesContainerDTO compatiablePaymentModesContainerDTO = new CompatiablePaymentModesContainerDTO(compatiablePaymentModesDTO.Id, compatiablePaymentModesDTO.PaymentModeId, compatiablePaymentModesDTO.CompatiablePaymentModeId, compatiablePaymentModesDTO.Guid);
                            if(paymentModesContainerDTO.CompatiablePaymentModesContainerDTOList == null)
                            {
                                paymentModesContainerDTO.CompatiablePaymentModesContainerDTOList = new List<CompatiablePaymentModesContainerDTO>();
                            }
                            paymentModesContainerDTO.CompatiablePaymentModesContainerDTOList.Add(compatiablePaymentModesContainerDTO);
                        }
                    }

                    if (paymentModeDTO.PaymentModeDisplayGroupsDTOList != null && paymentModeDTO.PaymentModeDisplayGroupsDTOList.Any())
                    {
                        foreach (PaymentModeDisplayGroupsDTO paymentModeDisplayGroupsDTO in paymentModeDTO.PaymentModeDisplayGroupsDTOList)
                        {
                            PaymentModeDisplayGroupsContainerDTO paymentModeDisplayGroupsContainerDTO = new PaymentModeDisplayGroupsContainerDTO(paymentModeDisplayGroupsDTO.PaymentModeDisplayGroupId, paymentModeDisplayGroupsDTO.PaymentModeId, paymentModeDisplayGroupsDTO.ProductDisplayGroupId, paymentModeDisplayGroupsDTO.Guid);
                            if (paymentModesContainerDTO.PaymentModeDisplayGroupsContainerDTOList == null)
                            {
                                paymentModesContainerDTO.PaymentModeDisplayGroupsContainerDTOList = new List<PaymentModeDisplayGroupsContainerDTO>();
                            }
                            paymentModesContainerDTO.PaymentModeDisplayGroupsContainerDTOList.Add(paymentModeDisplayGroupsContainerDTO);
                        }
                    }

                    paymentModeContainerDTOList.Add(paymentModesContainerDTO);
                    if(paymentModesContainerDTODictionary.ContainsKey(paymentModesContainerDTO.PaymentModeId)==false)
                    {
                        paymentModesContainerDTODictionary.Add(paymentModesContainerDTO.PaymentModeId, paymentModesContainerDTO);
                    }
                }
            }
            paymentModesContainerDTOCollection = new PaymentModesContainerDTOCollection(paymentModeContainerDTOList);
            log.LogMethodExit();
        }
        private static List<PaymentModeDTO> GetPaymentModeDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<PaymentModeDTO> paymentModeDTOList = null;
            try
            {
                PaymentModeList PaymentModesList = new PaymentModeList();
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISACTIVE, "1"));
                paymentModeDTOList = PaymentModesList.GetPaymentModeDTOList(searchParameters,true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the payment modes.", ex);
            }

            if (paymentModeDTOList == null)
            {
                paymentModeDTOList = new List<PaymentModeDTO>();
            }
            log.LogMethodExit(paymentModeDTOList);
            return paymentModeDTOList;
        }
        private static DateTime? GetPaymentModeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                PaymentModeList paymentModesList = new PaymentModeList();
                result = paymentModesList.GetPaymentModeModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the payment modes max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Gets the paymentModesContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        public PaymentModesContainerDTOCollection GetPaymentModeContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(paymentModesContainerDTOCollection);
            return paymentModesContainerDTOCollection;
        }
        /// <summary>
        /// GetPaymentModesContainerDTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PaymentModesContainerDTO GetPaymentModesContainerDTO(int id)
        {
            log.LogMethodEntry(id);
            if (paymentModesContainerDTODictionary.ContainsKey(id) == false)
            {
                string errorMessage = "PaymentModes with Id : " + id + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = paymentModesContainerDTODictionary[id];
            return result;
        }
        /// <summary>
        /// Refresh
        /// </summary>
        /// <returns></returns>
        public PaymentModesContainer Refresh() 
        {
            log.LogMethodEntry();
            PaymentModeList paymentModesList = new PaymentModeList();
            DateTime? updateTime = paymentModesList.GetPaymentModeModuleLastUpdateTime(siteId);
            if (paymentModesModuleLastUpdateTime.HasValue
                && paymentModesModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Payment modes since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            PaymentModesContainer result = new PaymentModesContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
