/********************************************************************************************
 * Project Name - ActiveCampaignCustomerInfoListBL
 * Description  - BL of the Active Campaign Info data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.3      01-Feb-2020   Nitin Pai           Created, new BL for active campaign object 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    class ActiveCampaignCustomerInfoListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ActiveCampaignCustomerInfoListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Customer list
        /// </summary>
        public List<ActiveCampaignCustomerInfoDTO> GetCustomerActivityDTOList(List<KeyValuePair<ActiveCampaignCustomerInfoDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ActiveCampaignCustomerInfoDataHandler customerActivityDataHandler = new ActiveCampaignCustomerInfoDataHandler(sqlTransaction);
            List<ActiveCampaignCustomerInfoDTO> customerActivityDTOList = customerActivityDataHandler.GetCustomerActivityDTOList(searchParameters);
            log.LogMethodExit(customerActivityDTOList);
            return customerActivityDTOList;
        }

        public List<ActiveCampaignCustomerInfoDTO> BuildCustomerActivity(int customerId, string accountList, DateTime fromdate, DateTime toDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerId, accountList, fromdate, toDate, sqlTransaction);
            ActiveCampaignCustomerInfoDataHandler customerActivityDataHandler = new ActiveCampaignCustomerInfoDataHandler(sqlTransaction);
            List<ActiveCampaignCustomerInfoDTO> customerActivityDTOList = customerActivityDataHandler.BuildCustomerActivity(customerId, accountList, fromdate, toDate, sqlTransaction);
            log.LogMethodExit(customerActivityDTOList);
            return customerActivityDTOList;
        }
    }
}
