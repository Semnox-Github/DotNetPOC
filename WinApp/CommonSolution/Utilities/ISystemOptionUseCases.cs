/********************************************************************************************
 * Project Name - Utilities  
 * Description  - ISystemOptionUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public interface ISystemOptionUseCases
    {
        Task<SystemOptionContainerDTOCollection> GetSystemOptionContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
