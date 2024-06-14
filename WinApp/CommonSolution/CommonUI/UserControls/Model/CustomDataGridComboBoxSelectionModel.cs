/******************************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - custom data grid selection model
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
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public class CustomDataGridComboBoxSelectionModel : ViewModelBase
    {
        #region Members
        private int columnIndex;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomComboBox customComboBox;
        #endregion

        #region Properties
        public int ColumnIndex
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(columnIndex);
                return columnIndex;
            }
            set
            {
                log.LogMethodEntry(columnIndex, value);
                SetProperty(ref columnIndex, value);
                log.LogMethodExit(columnIndex);
            }
        }
        public CustomComboBox CustomComboBox
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customComboBox);
                return customComboBox;
            }
            set
            {
                log.LogMethodEntry(customComboBox, value);
                SetProperty(ref customComboBox, value);
                log.LogMethodExit(customComboBox);
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        public CustomDataGridComboBoxSelectionModel()
        {
            log.LogMethodEntry();
            columnIndex = -1;
            customComboBox = null;
            log.LogMethodExit();
        }
        public CustomDataGridComboBoxSelectionModel(int columnIndex, CustomComboBox customComboBox)
        {
            log.LogMethodEntry();
            this.columnIndex = columnIndex;
            this.customComboBox = customComboBox;
            log.LogMethodExit();
        }
        #endregion
    }
}
