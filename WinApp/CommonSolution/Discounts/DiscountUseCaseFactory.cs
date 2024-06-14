/********************************************************************************************
 * Project Name - Discounts
 * Description  - DiscountUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.00     14-Apr-2021       Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    public class DiscountUseCaseFactory
    {
        public static IDiscountUseCases GetDiscountUseCases(ExecutionContext executionContext, string requestGuid)
        {
            Semnox.Parafait.logging.Logger log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry(executionContext, requestGuid);
            IDiscountUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteDiscountUseCases(executionContext, requestGuid);
            }
            else
            {
                result = new LocalDiscountUseCases(executionContext, requestGuid);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
