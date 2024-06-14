/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - BigHorizontalScrollBarView class for custom scroll bar
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.150.1      02-Feb-20232      Guru S A       Created for Kiosk Cart Project
 ********************************************************************************************/
using System.Drawing; 
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// BigHorizontalScrollBarView
    /// </summary>
    public partial class BigHorizontalScrollBarView : HorizontalScrollBarView
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Constructor
        /// </summary>
        public BigHorizontalScrollBarView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// InitializeScrollBar
        /// </summary> 
        public void InitializeScrollBar(Image imgLeftButton, Image imgLeftButtonDisabled, Image imgRightButton, Image imgRightButtonDisabled)
        {
            log.LogMethodEntry();
            this.SuspendLayout();
            leftButtonDisabledBackgroundImage = imgLeftButtonDisabled;
            rightButtonDisabledBackgroundImage = imgRightButtonDisabled;
            leftButtonBackgroundImage = imgLeftButton;
            rightButtonBackgroundImage = imgRightButton;
            this.ResumeLayout(true);
            UpdateButtonStatus();
            log.LogMethodExit();
        }
    }
}
