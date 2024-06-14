/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - AttributeEnabledTablesUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      24-Aug-2021      Fiona                    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IAttributeEnabledTablesUseCases GetAttributeEnabledTablesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAttributeEnabledTablesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAttributeEnabledTablesUseCases(executionContext);
            }
            else
            {
                result = new LocalAttributeEnabledTablesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
