using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    public interface IThemeUseCases
    {
        Task<ThemeContainerDTOCollection> GetThemeContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
