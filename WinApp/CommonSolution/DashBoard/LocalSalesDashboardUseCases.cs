/********************************************************************************************
 * Project Name - Dashboard
 * Description  - LocalSalesDashboardUseCases class 
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DashBoard
{
    public class LocalSalesDashboardUseCases : LocalUseCases, ISalesDashboardUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalSalesDashboardUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<WeeklyCollectionDTO> GetWeeklyCollectionList(int roleId)
        {
            log.LogMethodEntry(roleId);
            CollectionDashBoardList weeklyCollectionList = new CollectionDashBoardList();
            var result = weeklyCollectionList.GetWeeklyCollectionList(roleId);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<WeeklyCollectionPOSReportDTO>> GetWeeklyCollectionPOSList(int siteId, string posMachine)
        {
            log.LogMethodEntry();
            CollectionDashBoardList weeklyCollectionList = new CollectionDashBoardList();
            var result = weeklyCollectionList.GetWeeklyCollectionPOSList(siteId, posMachine);
            log.LogMethodExit(result);
            return result;
        }
    }
}
