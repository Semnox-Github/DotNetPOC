/********************************************************************************************
 * Project Name - Utilities
 * Description  - This class helps in building GET url with parameters
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Jan-2020       Lakshminarayana             Created : POS UI Redesign with REST API
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Semnox.Core.Utilities
{
    public class WebApiGetRequestParameterCollection : List<KeyValuePair<string, string>>
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public WebApiGetRequestParameterCollection(params object[] parameters)
        {
            log.LogMethodEntry(parameters);
            if (parameters == null || parameters.Length % 2 != 0)
            {
                string errorMessage = "No of parameters doesn't match check whether all the parameters are passed.";
                log.LogMethodExit(null, "Throwing Exception -" + errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            for (int i = 0; i < parameters.Length / 2; i++)
            {
                string parameterName = parameters[i * 2] as string;
                object parameterValue = parameters[(i * 2) + 1];
                
                if (string.IsNullOrWhiteSpace(parameterName))
                {
                    string errorMessage = "No of parameters doesn't match check whether all the parameters are passed.";
                    log.LogMethodExit(null, "Throwing Exception -" + errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
                if (parameterValue == null)
                {
                    continue;
                }
                Add(new KeyValuePair<string, string>(parameterName, GetValue(parameterValue)));
            }
            log.LogMethodExit();
        }

        private string GetValue(object parameter)
        {
            string result;
            if (parameter is DateTime)
            {
                result = ((DateTime)parameter).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            else if (parameter is DateTime?)
            {
                result = parameter == null ? string.Empty : ((DateTime?)parameter).Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            else
            {
                result = parameter.ToString();
            }
            return result;
        }

    }
}
