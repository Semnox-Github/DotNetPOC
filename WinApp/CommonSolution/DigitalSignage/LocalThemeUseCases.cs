using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public class LocalThemeUseCases : IThemeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalThemeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<ThemeContainerDTOCollection> GetThemeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<ThemeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    ThemeContainerList.Rebuild(siteId);
                }
                List<ThemeContainerDTO> themeContainerDTOList = ThemeContainerList.GetThemeContainerDTOList(siteId, executionContext);
                ThemeContainerDTOCollection result = new ThemeContainerDTOCollection(themeContainerDTOList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
