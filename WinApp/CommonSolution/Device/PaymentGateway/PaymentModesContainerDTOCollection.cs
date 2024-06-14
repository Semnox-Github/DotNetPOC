/********************************************************************************************
 * Project Name - Device 
 * Description  - Data object of PaymentModesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.110.0      17-Aug-2021   Fiona                   Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentModesContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PaymentModesContainerDTO> paymentModesContainerDTOList;
        private string hash;

        public PaymentModesContainerDTOCollection()
        {
            log.LogMethodEntry();
            this.paymentModesContainerDTOList = new List<PaymentModesContainerDTO>();
            log.LogMethodExit();
        }

        public PaymentModesContainerDTOCollection(List<PaymentModesContainerDTO> paymentModesContainerDTOList) 
        {
            log.LogMethodEntry();
            this.paymentModesContainerDTOList = paymentModesContainerDTOList;
            if (this.paymentModesContainerDTOList == null)
            {
                this.paymentModesContainerDTOList = new List<PaymentModesContainerDTO>();
            }
            this.hash = new DtoListHash(GetDTOList(paymentModesContainerDTOList));
            log.LogMethodExit();
        }
        private IEnumerable<object> GetDTOList(List<PaymentModesContainerDTO> paymentModesContainerDTOList)
        {
            log.LogMethodEntry(paymentModesContainerDTOList);
            foreach (PaymentModesContainerDTO paymentModesContainerDTO in paymentModesContainerDTOList.OrderBy(x => x.PaymentMode))
            {
                yield return paymentModesContainerDTO;
            }
            log.LogMethodExit();
        }
        public List<PaymentModesContainerDTO> PaymentModesContainerDTOList
        {
            get
            {
                return paymentModesContainerDTOList;
            }

            set
            {
                paymentModesContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
