/********************************************************************************************
 * Project Name - Cell
 * Description  - Data object of Cell
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Represents a cell in the excel
    /// </summary>
    public class Cell
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string value;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Cell() : this(string.Empty)
        {

        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="value"></param>
        public Cell(string value)
        {
            log.LogMethodEntry(value);
            this.value = value;
            log.LogMethodExit();
        }

        

        /// <summary>
        /// Get method for value field
        /// </summary>
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
