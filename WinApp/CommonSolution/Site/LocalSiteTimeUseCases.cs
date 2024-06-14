/********************************************************************************************
 * Project Name - Site
 * Description  - LocalSiteTimeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      08-07-2021     Prajwal S               Created : F&B web design
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Site
{
    class LocalSiteTimeUseCases : LocalUseCases, ISiteTimeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public LocalSiteTimeUseCases(ExecutionContext executionContext)
        : base(executionContext)
    {
        log.LogMethodEntry(executionContext);
        log.LogMethodExit();
    }
        public async Task<DateTime> GetSiteTime(int siteId)
        {
            return await Task<DateTime>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId);
                DateTime siteTime = SiteContainerList.FromSiteDateTime(siteId, ServerDateTime.Now);
                log.LogMethodExit(siteTime);
                return siteTime;
            });
        }
    }
}