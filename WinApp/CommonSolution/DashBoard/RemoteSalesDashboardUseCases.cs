/********************************************************************************************
 * Project Name - Dashboard
 * Description  - RemoteSalesDashboardUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DashBoard
{
    public class RemoteSalesDashboardUseCases : RemoteUseCases, ISalesDashboardUseCases 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SALESDASHBOARD_URL = "api/Report/SalesDashboard";

        public RemoteSalesDashboardUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<WeeklyCollectionDTO> GetWeeklyCollectionList(int roleId = -1)
        {
            log.LogMethodEntry(roleId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("roleId", roleId.ToString()));
            WeeklyCollectionDTO result = await Get<WeeklyCollectionDTO>(SALESDASHBOARD_URL, searchParameterList, string.Empty);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<WeeklyCollectionPOSReportDTO>> GetWeeklyCollectionPOSList(int siteId, string posMachine)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("posMachine", posMachine.ToString()));
            List<WeeklyCollectionPOSReportDTO> result = await Get<List<WeeklyCollectionPOSReportDTO>>(SALESDASHBOARD_URL, searchParameterList, string.Empty);
            log.LogMethodExit(result);
            return result;
        }
    }
}
