/********************************************************************************************
 * Project Name - Logger
 * Description  - IEventLogUseCases Class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0    11-Apr-2022       Roshan Devadiga           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
   public  interface IEventLogUseCases
   {
        Task<List<EventLogDTO>> GetEventLogs(int eventLogId = -1, string source = null, DateTime? timestamp = null, string type = null, string username = null, string computer = null, string category = null,int currentPage=0,int pageSize=0);
        Task<List<EventLogDTO>> SaveEventLogs(List<EventLogDTO> eventLogDTOList);
   }
}
