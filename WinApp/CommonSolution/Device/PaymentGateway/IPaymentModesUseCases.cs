/********************************************************************************************
 * Project Name - PaymentGateway  
 * Description  - IPaymentModesUseCases class to get the data  from API 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.0      19-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public interface IPaymentModesUseCases
    {
        Task<PaymentModesContainerDTOCollection> GetPaymentModesContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<List<PaymentModeDTO>> GetPaymentModes(List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> parameters, bool loadChildRecords, bool loadActiveChild=false);
        Task<string> SavePaymentModes(List<PaymentModeDTO> paymentModeDTOList);
    }
}
