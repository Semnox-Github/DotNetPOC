/********************************************************************************************
* Project Name -  Jobs
* Description  - JobUseCaseFactory class
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    25-Apr-2021      B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
  public  class JobUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// ConcurrentProgramJobStatusUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        //public static IConcurrentProgramJobStatusUseCases GetConcurrentProgramJobStatuses(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IConcurrentProgramJobStatusUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteConcurrentProgramJobStatusUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalConcurrentProgramJobStatusUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}
        public static IProgramParameterValueUseCases GetProgramParameterValues(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProgramParameterValueUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteProgramParameterValueUseCases(executionContext);
            }
            else
            {
                result = new LocalProgramParameterValueUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;

        }

        //}
        //public static IConcurrentProgramParametersUseCases GetConcurrentProgramParameters(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IConcurrentProgramParametersUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteConcurrentProgramParametersUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalConcurrentProgramParametersUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;

        //}

        /// <summary>
        /// ConcurrentProgram
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IConcurrentProgramsUseCases GetConcurrentPrograms(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IConcurrentProgramsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteConcurrentProgramsUseCases(executionContext);
            }
            else
            {
                result = new LocalConcurrentProgramsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
