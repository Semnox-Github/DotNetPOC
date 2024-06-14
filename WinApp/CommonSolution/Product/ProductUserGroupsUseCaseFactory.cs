/********************************************************************************************
 * Project Name - Product
 * Description  - ProductUserGroupsUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00     16-Nov-2020         Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductUserGroupsUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IProductUserGroupsUseCases GetProductUserGroupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductUserGroupsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductUserGroupsUseCases(executionContext);
            }
            else
            {
                result = new LocalProductUserGroupsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
