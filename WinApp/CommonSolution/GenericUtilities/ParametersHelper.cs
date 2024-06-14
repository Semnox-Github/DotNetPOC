/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - Parametershelper 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    public class ParametersHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool IsNullParameterValue( object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(value, negetiveValueNull);
            if (value is int)
            {
                if (negetiveValueNull && ((int)value) < 0)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                if (value == null)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }

            log.LogMethodExit(false);
            return false;
        }

        public static void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);
            if (parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if (IsNullParameterValue(value, negetiveValueNull))
                {
                    parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                }
                else
                {
                    parameters.Add(new SqlParameter(parameterName, value));
                }
            }
            log.LogMethodExit();
        }

    }

   
}
