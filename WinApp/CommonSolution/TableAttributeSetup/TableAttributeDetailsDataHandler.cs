/********************************************************************************************
 * Project Name - TableAttributeSetup
 * Description  - TableAttributeDetailsDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.140.0      16-Aug-2021    Fiona         Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Semnox.Parafait.TableAttributeSetup
{
    /// <summary>
    /// TableAttributeDetailsDataHandler
    /// </summary>
    public class TableAttributeDetailsDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of TableAttributeDetailsDataHandler class
        /// </summary>
        public TableAttributeDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetSQLDataList
        /// </summary>
        /// <param name="sqlSource"></param>
        /// <param name="sqlDisplayMember"></param>
        /// <param name="sqlValueMember"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetSQLDataList(string sqlSource, string sqlDisplayMember, string sqlValueMember)
        {
            log.LogMethodEntry(sqlSource, sqlDisplayMember, sqlValueMember);
            List<KeyValuePair<string, string>> sourceDataList = new List<KeyValuePair<string, string>>();
            string selectQuery = @"SELECT " + sqlValueMember + ", " + sqlDisplayMember + " from " + sqlSource;
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (data.Rows.Count > 0)
            { 
                foreach (DataRow dataRow in data.Rows)
                {
                    KeyValuePair<string, string> stringData = GetSQLDataToString(dataRow);
                    sourceDataList.Add(stringData);
                }
            }
            log.LogMethodExit(sourceDataList);
            return sourceDataList;
        }
        private KeyValuePair<string, string> GetSQLDataToString(DataRow dataRow)
        {
            log.LogMethodEntry();
            KeyValuePair<string, string> stringData = new KeyValuePair<string, string>(ConvertToStringValue(dataRow[0]), ConvertToStringValue(dataRow[1]));
            log.LogMethodExit(stringData);
            return stringData;
        }
        private string ConvertToStringValue(object inputValue)
        {
            log.LogMethodEntry(inputValue);
            string outputValue = string.Empty;
            if (inputValue != null)
            {
                if (inputValue.GetType().ToString() == "DateTime")
                {
                    outputValue = ((DateTime)inputValue).ToString("MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else if (inputValue.GetType().ToString() == "Date")
                {
                    outputValue = ((DateTime)inputValue).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    outputValue = inputValue.ToString();
                }
            }
            log.LogMethodExit(outputValue);
            return outputValue;
        }
    }
}
