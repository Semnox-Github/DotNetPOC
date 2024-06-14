/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - InOperator 
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
    internal class InOperator : IOperator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetQueryString(Column column, List<CriteriaParameter> criteriaParameters)
        {
            log.LogMethodEntry(criteriaParameters);
            string queryString = string.Empty;
            if (criteriaParameters != null && criteriaParameters.Count > 0)
            {
                queryString = " IN ( ";
                string appender = " ";
                foreach (var value in criteriaParameters)
                {
                    queryString += appender + value.Name;
                    appender = ", ";
                }
                queryString += ")";
            }
            else
            {
                throw new Exception("Invalid no of values passed to the in operator");
            }
            log.LogMethodExit(queryString);
            return queryString;
        }

        public string GetDisplayString(params string[] values)
        {
            log.LogMethodEntry(values);
            string queryString = string.Empty;
            if (values != null && values.Length > 0)
            {
                queryString = " In ( ";
                string appender = " ";
                foreach (var value in values)
                {
                    queryString += appender + value;
                    appender = ", ";
                }
                queryString += ")";
            }
            else
            {
                throw new Exception("Invalid no of values passed to the in operator");
            }
            log.LogMethodExit(queryString);
            return queryString;
        }

        public string DisplayName
        {
            get { return "In"; }
        }

        public bool Browsable
        {
            get
            {
                return false;
            }
        }
    }
}
