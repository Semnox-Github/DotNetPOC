/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Left Pane View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     25-Nov-2020   Raja Uthanda            Created for parent-child leftpane menu UI.
 ********************************************************************************************/
using System.Reflection;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public class ExpanderMenuItem : ViewModelBase
    {
        #region Members
        private bool isExpanded;
        private string parentHeader;

        private ObservableCollection<string> subMenus;

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                log.LogMethodEntry(isExpanded, value);
                SetProperty(ref isExpanded, value);
                log.LogMethodExit(isExpanded);
            }
        }
        public string ParentHeader
        {
            get { return parentHeader; }
            set
            {
                log.LogMethodEntry(parentHeader, value);
                SetProperty(ref parentHeader, value);
                log.LogMethodExit();
            }
        }
        public ObservableCollection<string> SubMenus
        {
            get { return subMenus; }
            set
            {
                log.LogMethodEntry(subMenus, value);
                SetProperty(ref subMenus, value);
                log.LogMethodExit();
            }
        }
        #endregion

        #region Constructor
        public ExpanderMenuItem(string parentHeader, ObservableCollection<string> subMenus, bool isExpanded = false)
        {
            log.LogMethodEntry(parentHeader, subMenus, isExpanded);
            this.isExpanded = isExpanded;
            this.parentHeader = parentHeader;
            this.subMenus = subMenus;
            log.LogMethodExit();
        }
        #endregion        
    }
}
