/********************************************************************************************
 * Project Name - Utilities  
 * Description  - IParafaitDefaultUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public interface IParafaitDefaultUseCases
    {
        Task<ParafaitDefaultContainerDTOCollection> GetParafaitDefaultContainerDTOCollection(int siteId, int userPkId, int machineId, string hash, bool rebuildCache);
    }
}
