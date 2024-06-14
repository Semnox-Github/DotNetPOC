/********************************************************************************************
 * Project Name - Country Params Programs 
 * Description  - Criteria
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        28-Nov-2019     Lakshminarayana    Created.
 ********************************************************************************************/

using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Generates sql parameter names for search criteria
    /// </summary>
    public class SqlParameterNameProvider
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<string, int> parameterNameCountMap;

        public SqlParameterNameProvider()
        {
            log.LogMethodEntry();
            parameterNameCountMap = new Dictionary<string, int>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Sql Parameter Name
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string GetSqlParameterName(string columnName)
        {
            log.LogMethodEntry(columnName);
            if (parameterNameCountMap.ContainsKey(columnName) == false)
            {
                parameterNameCountMap.Add(columnName, 0);
            }

            parameterNameCountMap[columnName]++;
            string result = "@" + columnName + parameterNameCountMap[columnName];
            log.LogMethodExit(result);
            return result;
        }
    }
}
