/********************************************************************************************
 * Project Name - Utilities
 * Description  - Interface for all the entities data which can be cached in the system.
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.140.0     27-Oct-2021      Lakshminarayana     Created 
 ********************************************************************************************/

namespace Semnox.Core.Utilities
{
    public interface IEntityCacheData
    {
        IEntityCacheData Clone();
    }
}
