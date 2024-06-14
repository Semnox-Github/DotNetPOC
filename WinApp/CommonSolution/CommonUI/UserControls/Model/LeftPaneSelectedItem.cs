/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Left pane selected item model  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
namespace Semnox.Parafait.CommonUI
{
    internal class LeftPaneSelectedItem
    {
        #region Members
        private string _menuItem;
        private LeftPaneUserControl _leftPaneUserControl;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string MenuItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_menuItem);
                return _menuItem;
            }
            set
            {
                log.LogMethodEntry(value);
                _menuItem = value;
            }
        }

        public LeftPaneUserControl LeftPaneUserControl
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_leftPaneUserControl);
                return _leftPaneUserControl;
            }
            set
            {
                log.LogMethodEntry(value);
                _leftPaneUserControl = value;
            }
        }
        #endregion
    }
}
