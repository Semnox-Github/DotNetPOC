/********************************************************************************************
 * Project Name - Product
 * Description  - ProductTypeUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.1      24-Jun-2021      Abhishek           Created : POS UI Redesign with REST API
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
    public class ProductTypeUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IProductTypeUseCases GetProductTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductTypeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProductTypeUseCases(executionContext);
            }
            else
            {
                result = new LocalProductTypeUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
