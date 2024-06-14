/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteProductActivityUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     31-Dec-2020       Abhishek         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class RemoteProductActivityUseCases : RemoteUseCases, IProductActivityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCT_ACTIVITY_URL = "api/Inventory/ProductActivities";
        private const string PRODUCT_ACTIVITY_COUNT_URL = "api/Inventory/ProductActivityCounts";

        public RemoteProductActivityUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductActivityViewDTO>> GetProductActivities(int locationId, int productId, int lotId, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, lotId, currentPage, pageSize);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("locationId", locationId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("productId", productId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("lotId", lotId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage", currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));
            try
            {
                List<ProductActivityViewDTO> result = await Get<List<ProductActivityViewDTO>>(PRODUCT_ACTIVITY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetProductActivityCount(int locationId, int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("locationId", locationId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("productId", productId.ToString()));
            try
            {
                int result = await Get<int>(PRODUCT_ACTIVITY_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
