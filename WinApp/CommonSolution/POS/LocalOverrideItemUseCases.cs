/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalOverrideItemUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class LocalOverrideItemUseCases : LocalUseCases, IOverrideItemUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalOverrideItemUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<OverrideOptionItemDTO>> GetOverrideOptionItems(List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>> parameters,
                                                                                       SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            return await Task<List<OverrideOptionItemDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                OverrideOptionItemListBL overrideOptionItemListBL = new OverrideOptionItemListBL(executionContext);
                List<OverrideOptionItemDTO> overrideOptionItemDTOList = overrideOptionItemListBL.GetOverrideOptionItemDTOList(parameters, sqlTransaction);
                log.LogMethodExit(overrideOptionItemDTOList);
                return overrideOptionItemDTOList;
            });
        }
    }
}
