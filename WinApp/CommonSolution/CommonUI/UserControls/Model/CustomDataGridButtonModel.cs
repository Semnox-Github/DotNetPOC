/******************************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - custom data grid button model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *******************************************************************************************************
 *2.130.0     07-Jul-2021   Raja Uthanda            Created for POS UI Redesign 
 ******************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.CommonUI
{
    public class CustomDataGridButtonModel
    {

        #region Members
        private int columnIndex;
        private object item;
        private logging.Logger Log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public int ColumnIndex
        {
            get
            {
                Log.LogMethodEntry();
                Log.LogMethodExit(columnIndex);
                return columnIndex;
            }
            set
            {
                Log.LogMethodEntry(columnIndex, value);
                columnIndex = value;
                Log.LogMethodEntry(columnIndex);
            }
        }
        public object Item
        {
            get
            {
                Log.LogMethodEntry();
                Log.LogMethodExit(item);
                return item;
            }
            set
            {
                Log.LogMethodEntry(item, value);
                item = value;
                Log.LogMethodEntry(item);
            }
        }
        #endregion

        #region Constructor
        public CustomDataGridButtonModel(int columnIndex, object item)
        {
            this.columnIndex = columnIndex;
            this.item = item;
        }
        #endregion

    }
}
