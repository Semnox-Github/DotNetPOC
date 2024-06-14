/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - EnabledAttributesUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      Fiona                    Created 
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
    public class EnabledAttributesUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IEnabledAttributesUseCases GetEnabledAttributesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IEnabledAttributesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteEnabledAttributesUseCases(executionContext);
            }
            else
            {
                result = new LocalEnabledAttributesUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
