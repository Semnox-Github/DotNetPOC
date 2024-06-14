/********************************************************************************************
 * Project Name - Site
 * Description  - RemoteDefaultAddressUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      09-07-2021     Prajwal S               Created : F&B web design
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class RemoteDefaultAddressUseCases : RemoteUseCases, IDefaultAddressUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// RemoteTransactionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteDefaultAddressUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetWaiverLinks
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<CustomerDTO> SetDefaultAddress(int addressId)
        {
            log.LogMethodEntry(addressId);
            string DefaultAddressUrl = "api/customer/" + addressId + "/Address/Default";
            try
            {
                CustomerDTO result = await Post<CustomerDTO>(DefaultAddressUrl, null);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
    }
}
