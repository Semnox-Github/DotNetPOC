/********************************************************************************************
 * Project Name - Site
 * Description  - RemoteSiteTimeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      08-07-2021     Prajwal S               Created : F&B web design
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    public class RemoteSiteTimeUseCases : RemoteUseCases, ISiteTimeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// RemoteTransactionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteSiteTimeUseCases(ExecutionContext executionContext)
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
        public async Task<DateTime> GetSiteTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string SiteTimeUrl = "api/organization/site/" + siteId + "/Time";
            try
            {
                DateTime result = await Get<DateTime>(SiteTimeUrl, null);
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