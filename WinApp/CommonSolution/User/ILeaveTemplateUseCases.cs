/********************************************************************************************
* Project Name - User
* Description  - Interface for LeaveTemplate Controller.
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface ILeaveTemplateUseCases
    {
        Task<List<LeaveTemplateDTO>> GetLeaveTemplate(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        //  Task<int> GetLeaveTemplateCount(List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<LeaveTemplateDTO>> SaveLeaveTemplate(List<LeaveTemplateDTO> leaveTemplateDTOList);
        Task<string> DeleteLeaveTemplate(List<LeaveTemplateDTO> leaveTemplateDTOList);
    }
}
