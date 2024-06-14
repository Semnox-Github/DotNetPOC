/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - HorizontalScrollBarView class for custom scroll bar
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.70        17-Jul-2019      Guru S A             MOdifications as per standards
 *2.70.2        10-Aug-2019      Girish kundar        Modified :Added Logger methods and Removed Unused namespace's. 
 *2.70.2        24-Sep-2019      Lakshminarayana      Added methods to scroll the container
 *2.150.1       22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public partial class HorizontalScrollBarView : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected DataGridView dataGridView;
        protected bool autoHide;
        protected bool leftButtonVisible = true;
        protected bool rightButtonVisible = true;
        protected ScrollableControl scrollableControl;
        protected System.Windows.Controls.ScrollViewer scrollViewer;
        protected Image leftButtonDisabledBackgroundImage;
        protected Image rightButtonDisabledBackgroundImage;
        protected Image leftButtonBackgroundImage;
        protected Image rightButtonBackgroundImage;
        protected ScrollableControlHorizontalScrollHandler scrollableControlHorizontalScrollHandler;
        protected DataGridViewHorizontalScrollHandler dataGridViewHorizontalScrollHandler;
        protected ScrollViewerHorizontalScrollHandler scrollViewerHorizontalScrollHandler;

        public event EventHandler LeftButtonClick;
        public event EventHandler RightButtonClick;
        public HorizontalScrollBarView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.Margin = new Padding(0, 0, 0, 0);
            leftButtonDisabledBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Left_Button_Disabled;
            rightButtonDisabledBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Right_Button_Disabled;
            leftButtonBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Left_Button;
            rightButtonBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Right_Button;
            autoHide = false;
            log.LogMethodExit();
        }
        public bool AutoHide
        {
            get
            {
                return autoHide;
            }
            set
            {
                autoHide = value;
            }
        }

        public Image LeftButtonBackgroundImage
        {
            get
            {
                return leftButtonBackgroundImage;//btnLeft.BackgroundImage;
            }

            set
            {
                leftButtonBackgroundImage = value;
            }
        }
        public Image LeftButtonDisabledBackgroundImage
        {
            get
            {
                return leftButtonDisabledBackgroundImage;
            }

            set
            {
                leftButtonDisabledBackgroundImage = value;
            }
        }


        public Image RightButtonBackgroundImage
        {
            get
            {
                return rightButtonBackgroundImage;
            }

            set
            {
                rightButtonBackgroundImage = value;
            }
        }
        public Image RightButtonDisabledBackgroundImage
        {
            get
            {
                return rightButtonDisabledBackgroundImage;
            }

            set
            {
                rightButtonDisabledBackgroundImage = value;
            }
        }

        public DataGridView DataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
                if (dataGridView != null)
                {
                    dataGridViewHorizontalScrollHandler = new DataGridViewHorizontalScrollHandler(this, dataGridView);
                    btnLeft.Click += dataGridViewHorizontalScrollHandler.btnLeft_Click;
                    btnRight.Click += dataGridViewHorizontalScrollHandler.btnRight_Click;
                }
            }
        }

        public ScrollableControl ScrollableControl
        {
            get
            {
                return scrollableControl;
            }
            set
            {
                scrollableControl = value;
                if (scrollableControl != null)
                {
                    scrollableControlHorizontalScrollHandler = new ScrollableControlHorizontalScrollHandler(this, scrollableControl);
                    btnLeft.Click += scrollableControlHorizontalScrollHandler.btnLeft_Click;
                    btnRight.Click += scrollableControlHorizontalScrollHandler.btnRight_Click;
                }
            }
        }

        public System.Windows.Controls.ScrollViewer ScrollViewer
        {
            get
            {
                return scrollViewer;
            }
            set
            {
                scrollViewer = value;
                if (scrollViewer != null)
                {
                    scrollViewerHorizontalScrollHandler = new ScrollViewerHorizontalScrollHandler(this, scrollViewer);
                    btnLeft.Click += scrollViewerHorizontalScrollHandler.btnLeft_Click;
                    btnRight.Click += scrollViewerHorizontalScrollHandler.btnRight_Click;
                }
            }
        }

        public void UpdateLeftButtonStatus(bool visible)
        {
            log.LogMethodEntry(visible);
            leftButtonVisible = visible;
            btnLeft.Enabled = visible;
            if (leftButtonVisible)
            {
                btnLeft.BackgroundImage = leftButtonBackgroundImage;
            }
            else
            {
                btnLeft.BackgroundImage = leftButtonDisabledBackgroundImage;
            }
            AutoHideScrollBar();
            log.LogMethodExit();
        }

        public void UpdateRightButtonStatus(bool visible)
        {
            log.LogMethodEntry(visible);
            rightButtonVisible = visible;
            btnRight.Enabled = visible;
            if (rightButtonVisible)
            {
                btnRight.BackgroundImage = rightButtonBackgroundImage;
            }
            else
            {
                btnRight.BackgroundImage = rightButtonDisabledBackgroundImage;
            }
            AutoHideScrollBar();
            log.LogMethodExit();
        }

        private void AutoHideScrollBar()
        {
            log.LogMethodEntry();
            if (autoHide)
            {
                this.Visible = rightButtonVisible || leftButtonVisible;
            }
            log.LogMethodExit();
        }

        public void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            if (scrollableControlHorizontalScrollHandler != null)
            {
                scrollableControlHorizontalScrollHandler.UpdateButtonStatus();
            }
            if (dataGridViewHorizontalScrollHandler != null)
            {
                dataGridViewHorizontalScrollHandler.UpdateButtonStatus();
            }
            if (scrollViewerHorizontalScrollHandler != null)
            {
                scrollViewerHorizontalScrollHandler.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }


        public void ScrollLeft()
        {
            btnLeft.PerformClick();
        }


        public void ScrollRight()
        {
            btnRight.PerformClick();
        }

        public bool CanScrollLeft
        {
            get { return btnLeft.Enabled; }
        }

        public bool CanScrollRight
        {
            get { return btnRight.Enabled; }
        }

        public void ScrollToRight()
        {
            while (CanScrollRight)
            {
                ScrollRight();
            }
        }

        public void ScrollToLeft()
        {
            while (CanScrollLeft)
            {
                ScrollLeft();
            }
        }
        public void GenerateLeftButtonClick()
        {
            if (LeftButtonClick != null)
            {
                LeftButtonClick(this, EventArgs.Empty);
            }
        }
        public void GenerateRightButtonClick()
        {
            if (RightButtonClick != null)
            {
                RightButtonClick(this, EventArgs.Empty);
            }
        }

    }
}
