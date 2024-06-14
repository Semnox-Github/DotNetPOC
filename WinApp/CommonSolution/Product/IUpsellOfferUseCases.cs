/********************************************************************************************
* Project Name - Product
* Description  - Specification of the UpsellOffer use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   06-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IUpsellOfferUseCases
    {
        Task<List<UpsellOffersDTO>> GetUpsellOffers(List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> parameters);
        Task<string> SaveUpsellOffers(List<UpsellOffersDTO> upsellOffersList); 
        Task<string> Delete(List<UpsellOffersDTO> upsellOffersDTOList);
    }
}
