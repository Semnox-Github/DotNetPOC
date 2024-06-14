/********************************************************************************************
* Project Name - Customer
* Description  - BL for CustomerPasswordHistory
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.80        26-June-2020  Indrajeet Kumar         Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    public class CustomerPasswordHistoryBL
    {
        private CustomerPasswordHistoryDTO customerPasswordHistoryDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private CustomerPasswordHistoryBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);            
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerPasswordHistoryDTO"></param>
        public CustomerPasswordHistoryBL(ExecutionContext executionContext, CustomerPasswordHistoryDTO customerPasswordHistoryDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerPasswordHistoryDTO);
            this.customerPasswordHistoryDTO = customerPasswordHistoryDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public CustomerPasswordHistoryBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomerPasswordHistoryDataHandler customerPasswordHistoryDataHandler = new CustomerPasswordHistoryDataHandler(sqlTransaction);
            customerPasswordHistoryDTO = customerPasswordHistoryDataHandler.GetCustomerPasswordHistoryDTO(id);            
            if (customerPasswordHistoryDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "customerPasswordHistory", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerPasswordHistoryDataHandler customerPasswordHistoryDataHandler = new CustomerPasswordHistoryDataHandler(sqlTransaction);
            if (customerPasswordHistoryDTO.CustomerPasswordHistoryId <= 0)
            {
                customerPasswordHistoryDTO = customerPasswordHistoryDataHandler.Insert(customerPasswordHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerPasswordHistoryDTO.AcceptChanges();
            }
            else
            {
                if (customerPasswordHistoryDTO.IsChanged)
                {
                    customerPasswordHistoryDTO = customerPasswordHistoryDataHandler.Update(customerPasswordHistoryDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerPasswordHistoryDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    public class CustomerPasswordHistoryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public CustomerPasswordHistoryListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public List<CustomerPasswordHistoryDTO> GetCustomerPasswordHistoryDTOList(List<KeyValuePair<CustomerPasswordHistoryDTO.SearchByCustomerPasswordHistoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerPasswordHistoryDataHandler customerPasswordHistoryDataHandler = new CustomerPasswordHistoryDataHandler(sqlTransaction);
            List<CustomerPasswordHistoryDTO> customerPasswordHistoryDTOList = customerPasswordHistoryDataHandler.GetCustomerPasswordHistoryDTOList(searchParameters);
            log.LogMethodExit(customerPasswordHistoryDTOList);
            return customerPasswordHistoryDTOList;
        }
    }
}
