/********************************************************************************************
* Project Name - Product
* Description  - Specification of the OrderType use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   08-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalOrderTypeGroupUseCases : IOrderTypeGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalOrderTypeGroupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<OrderTypeGroupDTO>> GetOrderTypeGroups(List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<OrderTypeGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                OrderTypeGroupListBL orderTypeListBL = new OrderTypeGroupListBL(executionContext);
                List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeListBL.GetOrderTypeGroupDTOList(searchParameters);

                log.LogMethodExit(orderTypeGroupDTOList);
                return orderTypeGroupDTOList;
            });
        }
        public async Task<string> SaveOrderTypeGroups(List<OrderTypeGroupDTO> orderTypeGroupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(orderTypeGroupDTOList);
                if (orderTypeGroupDTOList == null)
                {
                    throw new ValidationException("orderTypeGroupDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (OrderTypeGroupDTO orderTypeGroupDTO in orderTypeGroupDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            OrderTypeGroupBL orderTypeGroupBL = new OrderTypeGroupBL(executionContext, orderTypeGroupDTO);
                            orderTypeGroupBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
