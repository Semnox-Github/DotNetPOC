using System.Reflection;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    /// <summary>
    /// Interaction logic for FileExplorerUserControl.xaml
    /// </summary>
    public partial class FileExplorerUserControl : UserControl
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor        
        public FileExplorerUserControl()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        #endregion
    }
}
