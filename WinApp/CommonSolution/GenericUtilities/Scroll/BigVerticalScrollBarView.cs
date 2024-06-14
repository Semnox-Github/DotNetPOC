/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - BigVerticalScrollBarView class for custom scroll bar
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
    /// BigVerticalScrollBarView
    /// </summary>
    public partial class BigVerticalScrollBarView : VerticalScrollBarView
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        /// <summary>
        /// Constructor
        /// </summary>
        public BigVerticalScrollBarView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }
        /// <summary>
        /// InitializeScrollBar
        /// </summary> 
        public void InitializeScrollBar(Image imgDownButton, Image imgDownButtonDisabled, Image imgUpButton, Image imgUpButtonDisabled )
        {
            log.LogMethodEntry();
            this.SuspendLayout();
            upButtonDisabledBackgroundImage = imgUpButtonDisabled;
            downButtonDisabledBackgroundImage = imgDownButtonDisabled;
            upButtonBackgroundImage = imgUpButton;
            downButtonBackgroundImage = imgDownButton; 
            this.ResumeLayout(true);
            UpdateButtonStatus();
            log.LogMethodExit();
        }
    }
}
