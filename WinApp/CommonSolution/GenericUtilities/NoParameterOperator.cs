/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - NoParameterOperator 
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
    internal class NoParameterOperator : IOperator
    {
        private string operatorString;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string displayName;
        public NoParameterOperator(string operatorString, string displayName)
        {
            log.LogMethodEntry(operatorString, displayName);
            this.operatorString = operatorString;
            this.displayName = displayName;
            log.LogMethodExit();
        }

        public string GetQueryString(Column column, List<CriteriaParameter> criteriaParameters)
        {
            log.LogMethodEntry(criteriaParameters);
            string queryString = string.Empty;
            if (criteriaParameters != null && criteriaParameters.Count > 1)
            {
                throw new Exception("Invalid no of values passed to the " + operatorString + "operator");
            }
            string returnvalue = " " + operatorString + " ";
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        public string GetDisplayString(params string[] values)
        {
            log.LogMethodEntry(values);
            string queryString = string.Empty;
            if (values != null && values.Length > 1)
            {
                throw new Exception("Invalid no of values passed to the " + operatorString + "operator");
            }
            string returnvalue = " " + displayName + " ";
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public bool Browsable
        {
            get
            {
                return true;
            }
        }
    }
}
