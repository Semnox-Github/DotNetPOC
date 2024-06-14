/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Factory class to instantiate MIFARE use cases based on the execution mode configuration.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class MifareKeyUseCaseFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IMifareKeyUseCases GetIMifareKeyUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMifareKeyUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMifareKeyUseCases(executionContext);
            }
            else
            {
                result = new LocalMifareKeyUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
