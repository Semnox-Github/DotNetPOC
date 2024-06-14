/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - Factory Class for Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *********************************************************************************************/
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentProgramfactory
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PROCEDURE_PROGRAM = "P";
        private const string LIBRARY_PROGRAM = "L";
        private const string EXE_PROGRAM = "E";

        public static ConcurrentPrograms GetConcurrentPrograms(Utilities utilities, ConcurrentProgramsDTO concurrentProgramsDTO,ConcurrentRequestsDTO concurrentRequestsDTO,
                                                                string logFileName, DatabaseConnectorDTO dbConnectorDTO)
        {
            log.LogMethodEntry(concurrentProgramsDTO, utilities);
            ConcurrentPrograms concurrentPrograms = null;
            if (concurrentProgramsDTO.ExecutionMethod == PROCEDURE_PROGRAM)
            {
                concurrentPrograms = new ProcConcurrentPrograms(utilities, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO);
            }
            else if (concurrentProgramsDTO.ExecutionMethod == EXE_PROGRAM)
            {
                concurrentPrograms = new ExeConcurrentProgram(utilities, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO);
            }
            else if (concurrentProgramsDTO.ExecutionMethod == LIBRARY_PROGRAM)
            {
                concurrentPrograms = new LibConcurrentProgram(utilities, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO);
            }
            log.LogMethodExit(concurrentPrograms);
            return concurrentPrograms;
        }

    }
}
