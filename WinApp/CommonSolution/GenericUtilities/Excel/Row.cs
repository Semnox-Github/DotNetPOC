/********************************************************************************************
 * Project Name - Row
 * Description  - Data object of Row
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Represents a row in the excel sheet
    /// </summary>
    public class Row
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<Cell> cells;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="cells"></param>
        public Row(List<Cell> cells)
        {
            log.LogMethodEntry(cells);
            this.cells = cells;
            log.LogMethodExit();
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public Row() : this(new List<Cell>())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// cell in the current row
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public IList<Cell> Cells
        {
            get
            {
                return cells.AsReadOnly();
            }
        }
        /// <summary>
        /// add new cell to the row
        /// </summary>
        /// <param name="cell"></param>
        public void AddCell(Cell cell)
        {
            cells.Add(cell);
        }

        /// <summary>
        /// adds new cell list to the row
        /// </summary>
        /// <param name="cellsToBeAdded"></param>
        public void AddCells(IList<Cell> cellsToBeAdded)
        {
            cells.AddRange(cellsToBeAdded);
        }
        /// <summary>
        /// indexing retirs the cell for a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Cell this[int index]
        {
            get
            {
                return cells[index];
            }
        }

        /// <summary>
        /// Get/Set Method for cells. added for serialization
        /// </summary>
        public List<Cell> CellList
        {
            get
            {
                return cells;
            }
            set
            {
                cells = value;
            }
        }
    }
}
