/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - LikeOperator 
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
    internal class LikeOperator : SingleValueOperator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LikeOperator(string operatorString, string displayName) :
            base(operatorString, displayName)
        {
            log.LogMethodEntry(operatorString, displayName);
            log.LogMethodExit();
        }

        public override string GetQueryString(Column column, List<CriteriaParameter> criteriaParameters)
        {
            log.LogMethodEntry(criteriaParameters);
            string queryString;
            if (criteriaParameters != null && criteriaParameters.Count == 1)
            {
                string parameterValue = criteriaParameters[0].Value.ToString();
                if (parameterValue.Contains("%"))
                {
                    bool startsWith = parameterValue.StartsWith("%");
                    bool endsWith =  parameterValue.EndsWith("%");
                    queryString = " " + operatorString + (startsWith ?" '%' + " : " ")+ criteriaParameters[0].Name + (endsWith? " + '%' " : " ");
                }
                else
                {
                    queryString = " " + operatorString + " '%' + " + criteriaParameters[0].Name + " + '%' ";
                }

            }
            else
            {
                throw new Exception("Invalid no of values passed to the " + operatorString + " operator");
            }
            log.LogMethodExit(queryString);
            return queryString;
        }
    }
}
