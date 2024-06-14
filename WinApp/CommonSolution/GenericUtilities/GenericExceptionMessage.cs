/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - GenereicExceptionmessagge 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 *2.150.2       25-Jul-2023            Nitin Pai      SISA Fixes - Do not propagate SQL exceptions
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;

namespace Semnox.Core.GenericUtilities
{
    public class GenericExceptionMessage
    {        
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GenericExceptionMessage(string exceptionMessage)
        {
            log.LogMethodEntry(exceptionMessage);
            log.LogMethodExit();
        }
        public static string GetValidCustomExeptionMessage(Exception ex, ExecutionContext executionContext)
        {
            log.LogMethodEntry(ex, executionContext);
            string retMessage = "";
            int messgeNo = 0;
            Type exType = ex.GetType();       

            if (exType == typeof(ValidationException))
            {
                retMessage = (ex as ValidationException).GetAllValidationErrorMessages();
            }
            else if (exType == typeof(System.InvalidOperationException))
            {
                messgeNo = 1781;
                //retMessage = "InvalidOperationException";
            }
            else if (exType == typeof(System.Data.SqlClient.SqlException))
            {
                messgeNo = 1782;
                //retMessage = "SqlException";
            }
            else if (exType == typeof(System.OverflowException))
            {
                messgeNo = 1783;
                //retMessage = "OverflowException";
            }
            else if (exType == typeof(System.NullReferenceException))
            {
                messgeNo = 1784;
                //retMessage = "NullReferenceException";
            }
            else if (exType == typeof(System.DivideByZeroException))
            {
                messgeNo = 1785;
                //retMessage = "DividedByZeroException";
            }
            else if (exType == typeof(System.OutOfMemoryException))
            {
                messgeNo = 1786;
                //retMessage = "OutOfMemoryException";
            }
            else if (ex.InnerException != null)
            {
                ex = ex.InnerException;
                retMessage = ex.Message;
            }
            else
            {
                retMessage = ex.Message;
            }

            if (messgeNo > 0)
            {
                retMessage = MessageContainerList.GetMessage(executionContext, messgeNo);
            }
            if (ex.Message != null && exType != typeof(System.Data.SqlClient.SqlException))
            {
                retMessage = ex.Message;
            }
            else if (exType == typeof(System.Data.SqlClient.SqlException))
            {
                retMessage = "SQL exception encountered. Check log files to get more details.";
            }
            log.LogMethodExit(retMessage);
            return retMessage;
        }
    }
}
