/********************************************************************************************
 * Project Name - Dashboard
 * Description  - ISalesDashboardUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DashBoard
{
    public interface ISalesDashboardUseCases
    {
        Task<WeeklyCollectionDTO> GetWeeklyCollectionList(int roleId = -1);

        Task<List<WeeklyCollectionPOSReportDTO>> GetWeeklyCollectionPOSList(int siteId, string posMachine);

    }
}
