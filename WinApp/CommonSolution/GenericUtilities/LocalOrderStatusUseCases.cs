/********************************************************************************************
 * Project Name - LocalOrderStatusUseCases
 * Description  - LocalOrderStatusUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      8-Mar-2021       Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    public class LocalOrderStatusUseCases : IOrderStatusUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalOrderStatusUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<OrderStatusDTO>> GetOrderStatus(List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>>
                          searchParameters,  SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<OrderStatusDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                OrderStatusListBL orderStatusListBL = new OrderStatusListBL(executionContext);
                List<OrderStatusDTO> orderStatusDTOList = orderStatusListBL.GetOrderStatuses(searchParameters, sqlTransaction);

                log.LogMethodExit(orderStatusDTOList);
                return orderStatusDTOList;
            });
        }

        public async Task<List<OrderStatusDTO>> SaveOrderStatus(List<OrderStatusDTO> orderStatusDTOList)
        {
            return await Task<List<OrderStatusDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    OrderStatusListBL orderStatusList = new OrderStatusListBL(executionContext, orderStatusDTOList);
                    List<OrderStatusDTO> result = orderStatusList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
