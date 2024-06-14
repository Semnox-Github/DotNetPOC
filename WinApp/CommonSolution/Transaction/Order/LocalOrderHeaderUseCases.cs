/********************************************************************************************
 * Project Name - LocalOrderHeaderUseCases
 * Description  - LocalOrderHeaderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      8-Mar-2021       Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// LocalOrderHeaderUseCases
    /// </summary>
    public class LocalOrderHeaderUseCases :IOrderHeaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalOrderHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalOrderHeaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetOrderHeader
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// /// <param name="loadChildRecords"></param>
        /// /// <param name="activeChildRecords"></param>
        /// <returns></returns>
        public async Task<List<OrderHeaderDTO>> GetOrderHeader(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>>
                          searchParameters, bool loadChildRecords = false, bool activeChildRecords = false,  SqlTransaction sqlTransaction = null )
        {
            return await Task<List<OrderHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                OrderHeaderList orderHeaderListBL = new OrderHeaderList(executionContext);
                List<OrderHeaderDTO> orderHeaderDTOList = orderHeaderListBL.GetOrderHeaderDTOList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
                log.LogMethodExit(orderHeaderDTOList);
                return orderHeaderDTOList;
            });
        }

        /// <summary>
        /// SaveOrderHeader
        /// </summary>
        /// <param name="orderHeaderDTOList"></param>
        /// <returns></returns>
        public async Task<List<OrderHeaderDTO>> SaveOrderHeader(List<OrderHeaderDTO> orderHeaderDTOList)
        {
            return await Task<List<OrderHeaderDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    List<OrderHeaderDTO> result = new List<OrderHeaderDTO>();
                    foreach (OrderHeaderDTO orderHeaderDTO in orderHeaderDTOList)
                    {
                        OrderHeaderBL orderHeaderBL = new OrderHeaderBL(executionContext, orderHeaderDTO);
                        orderHeaderBL.Save();
                        result.Add(orderHeaderBL.OrderHeaderDTO);
                    }
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
