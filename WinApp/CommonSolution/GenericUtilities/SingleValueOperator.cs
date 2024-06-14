/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - SingleValueOperator 
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
    internal class SingleValueOperator : IOperator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string operatorString;
        protected string displayName;

        public SingleValueOperator(string operatorString, string displayName)
        {
            log.LogMethodEntry(operatorString, displayName);
            this.operatorString = operatorString;
            this.displayName = displayName;
            log.LogMethodExit();
        }
        public virtual string GetQueryString(Column column, List<CriteriaParameter> criteriaParameters)
        {
            log.LogMethodEntry(criteriaParameters);
            string queryString = string.Empty;
            if (column != null && (operatorString == "=" || operatorString == "!=") && (column is EncryptedTextColumn || column is EncryptedDateTimeColumn || column is EncryptedNumberColumn))
            {
                if (column is EncryptedTextColumn || column is EncryptedNumberColumn)
                {
                    if (criteriaParameters != null && criteriaParameters.Count == 1)
                    {
                        queryString = " " + operatorString + " hashbytes('SHA2_256',convert(nvarchar(max), upper( " + criteriaParameters[0].Name + "))) ";
                      
                    }
                    else
                    {
                        throw new Exception("Invalid no of values passed to the " + operatorString + " operator");
                    }
                }
                else if (column is EncryptedDateTimeColumn)
                {
                    if (criteriaParameters != null && criteriaParameters.Count == 1)
                    {
                        queryString = " " + operatorString + " hashbytes('SHA2_256', upper(Convert(nvarchar(100), convert(datetime, " + criteriaParameters[0].Name + " ,120), 120))) ";
                    }
                    else
                    {
                        throw new Exception("Invalid no of values passed to the " + operatorString + " operator");
                    }
                }
            }
            else
            {
                if (criteriaParameters != null && criteriaParameters.Count == 1)
                {
                    queryString = " " + operatorString + " " + criteriaParameters[0].Name + " ";
                }
                else
                {
                    throw new Exception("Invalid no of values passed to the " + operatorString + " operator");
                }
            }           
            log.LogMethodExit(queryString);
            return queryString;
        }

        public string GetDisplayString(params string[] values)
        {
            log.LogMethodEntry(values);
            string queryString;
            if (values != null && values.Length == 1)
            {
                queryString = " " + displayName + " " + values[0] + " ";
            }
            else
            {
                throw new Exception("Invalid no of values passed to the " + operatorString + " operator");
            }
            log.LogMethodExit(queryString);
            return queryString;
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
