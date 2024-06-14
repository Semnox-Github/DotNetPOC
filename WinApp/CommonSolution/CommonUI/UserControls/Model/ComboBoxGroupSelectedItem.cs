/******************************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Selected item model for combo box group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *******************************************************************************************************
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ******************************************************************************************************/
namespace Semnox.Parafait.CommonUI
{
    internal class ComboBoxGroupSelectedItem
    {
        private ComboGroupUserControl _comboBoxGroupUserControl;

        private ComboBoxField _comboBoxField;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ComboGroupUserControl ComboBoxGroupUserControl
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_comboBoxGroupUserControl);
                return _comboBoxGroupUserControl;
            }
            set
            {
                log.LogMethodEntry(value);
                _comboBoxGroupUserControl = value;
            }
        }

        public ComboBoxField ComboBoxField
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_comboBoxField);
                return _comboBoxField;
            }
            set
            {
                log.LogMethodEntry(value);
                _comboBoxField = value;
            }
        }
    }
}
