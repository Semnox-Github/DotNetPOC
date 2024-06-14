/********************************************************************************************
 * Project Name - Device
 * Description  - CashdrawerUseCaseFactory.cs
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.0      10-Aug-2021     Girish Kundar              Created : Multi cashdrawer for POS changes
 ********************************************************************************************/
using System;
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    public class CashdrawerUseCaseFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ICashdrawerUseCases GetCashdrawerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICashdrawerUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCashdrawerUseCases(executionContext);
            }
            else
            {
                result = new LocalCashdrawerUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
