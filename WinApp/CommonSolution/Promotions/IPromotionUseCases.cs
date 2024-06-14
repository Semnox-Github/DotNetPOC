/********************************************************************************************
* Project Name - Promotions
* Description  - Specification of the IPromotionUseCases use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   26-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
   public interface IPromotionUseCases
    {
        Task<List<PromotionDTO>> GetPromotions(List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters,
                                                                       bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SavePromotions(List<PromotionDTO> promotionDTOList);
        Task<string> Delete(List<PromotionDTO> promotionDTOList);
    }
}
