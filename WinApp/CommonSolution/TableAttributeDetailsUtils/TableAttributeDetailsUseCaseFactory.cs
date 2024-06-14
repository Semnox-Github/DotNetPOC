/********************************************************************************************
 * Project Name - TableAttributeDetailsUtils
 * Description  - TableAttributeDetailsUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      13-Sep-2021      Fiona                    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeDetailsUtils
{
    public class TableAttributeDetailsUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ITableAttributeDetailsUseCases GetTableAttributeDetailsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITableAttributeDetailsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteTableAttributeDetailsUseCases(executionContext);
            }
            else
            {
                result = new LocalTableAttributeDetailsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
