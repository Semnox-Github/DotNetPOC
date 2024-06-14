using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public interface IMifareKeyUseCases
    {
        Task<MifareKeyContainerDTOCollection> GetMifareKeyContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
