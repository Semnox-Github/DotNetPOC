/********************************************************************************************
 * Project Name - Job Utils
 * Description  - Parser
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Data;
using System.Reflection;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// to handle parsing given string expected output format
    /// </summary>
    public class Parser
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parse the given json string and return to datatable
        /// </summary>
        /// <param name="array">input json string</param>
        /// <returns>return datatable if parse success</returns>
        public DataTable ConvertToDataTable(Object array)
        {
            log.LogMethodEntry(array);
            PropertyInfo[] properties =  array.GetType().GetProperties();
            DataTable dt = CreateDataTable(properties);
            if (array != null)
            {
                FillData(properties, dt, array);
            }
            log.LogMethodExit(dt);
            return dt;
        }

        /// <summary>
        /// Creating datatable
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(PropertyInfo[] properties)

        {
            log.LogMethodEntry(properties);
            DataTable dt = new DataTable();
            DataColumn dc = null;
            foreach (PropertyInfo pi in properties)
            {
                dc = new DataColumn();
                dc.ColumnName = pi.Name;
                dc.DataType = pi.PropertyType;
                dt.Columns.Add(dc);
            }
            log.LogMethodExit(dt);
            return dt;
        }

        /// <summary>
        /// create datatable rows
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="dt"></param>
        /// <param name="o"></param>
        public void FillData(PropertyInfo[] properties, DataTable dt, Object o)
        {
            log.LogMethodEntry(properties, dt, o);
            DataRow dr = dt.NewRow();
            foreach (PropertyInfo pi in properties)
            {
                dr[pi.Name] = pi.GetValue(o, null);
            }
            dt.Rows.Add(dr);
            log.LogMethodExit();
        }
       
    }
}
