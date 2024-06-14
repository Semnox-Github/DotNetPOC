/********************************************************************************************
 * Project Name - Sheet
 * Description  - Data object of Sheet
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Represents a excel sheet
    /// </summary>
    public class Sheet
    {
        private List<Row> rows;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="rows"></param>
        public Sheet(List<Row> rows)
        {
            log.LogMethodEntry(rows);
            this.rows = rows;
            log.LogMethodExit();
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public Sheet() : this(new List<Row>())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Retursn the rows in the sheet
        /// </summary>

        [IgnoreDataMember]
        [JsonIgnore]
        public IList<Row> Rows
        {
            get
            {
                return rows.AsReadOnly();
            }
        }
        /// <summary>
        /// add new row to the sheet
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(Row row)
        {
            rows.Add(row);
        }

        /// <summary>
        /// indexing returns the row for a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Row this[int index]
        {
            get
            {
                return rows[index];
            }
        }

        /// <summary>
        /// Get/Set Method for rows. added for serialization
        /// </summary>
        public List<Row> RowList
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }
    }
}