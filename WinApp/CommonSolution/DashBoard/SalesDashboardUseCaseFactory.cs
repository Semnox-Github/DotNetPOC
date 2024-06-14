/********************************************************************************************
 * Project Name - Dashboard
 * Description  - SalesDashboardUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         23-Apr-2022       Nitin Pai                 Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.DashBoard
{
    public class SalesDashboardUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ISalesDashboardUseCases GetSalesDashboardUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISalesDashboardUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteSalesDashboardUseCases(executionContext);
            }
            else
            {
                result = new LocalSalesDashboardUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
