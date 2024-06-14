using System;
/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - IRemoteConnectionCheck : An interface having method to get ping time
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Amitha Joy                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public interface IRemoteConnectionCheck
    {
        DateTime? Get();
    }
}
