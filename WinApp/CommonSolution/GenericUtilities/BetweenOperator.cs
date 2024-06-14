/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - BetweenOperator 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    internal class BetweenOperator : IOperator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetQueryString(Column column, List<CriteriaParameter> criteriaParameters)
        {
            log.LogMethodEntry(criteriaParameters);
            string queryString = string.Empty;
            if (criteriaParameters != null && criteriaParameters.Count == 2)
            {
                queryString = " BETWEEN " + criteriaParameters[0].Name + " AND " + criteriaParameters[1].Name;
            }
            else
            {
                throw new Exception("Invalid no of values passed to the equal to operator");
            }
            log.LogMethodExit(queryString);
            return queryString;
        }

        public string GetDisplayString(params string[] values)
        {
            log.LogMethodEntry(values);
            string queryString = string.Empty;
            if (values != null && values.Length == 2)
            {
                queryString = " Between " + values[0] + " AND " + values[1];
            }
            else
            {
                throw new Exception("Invalid no of values passed to the equal to operator");
            }
            log.LogMethodExit(queryString);
            return queryString;
        }

        public string DisplayName
        {
            get { return "Between"; }
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
