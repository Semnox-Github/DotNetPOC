/********************************************************************************************
* Project Name - User
* Description  - Interface for LeaveActivity Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface ILeaveActivityUseCases
    {
        Task<LeaveActivityDTO> GetLeaveActivity(int userId);
    }
}
