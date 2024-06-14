/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - VerticalScrollBarView class for custom scroll bar
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.70.2        24-Sep-2019      Lakshminarayana      Added methods to scroll the container
 *2.150.1       22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public partial class VerticalScrollBarView : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected DataGridView dataGridView;
        protected ScrollableControl scrollableControl;
        protected System.Windows.Controls.ScrollViewer scrollViewer;

        protected bool upButtonVisible = true;
        protected Image upButtonDisabledBackgroundImage;
        protected Image downButtonDisabledBackgroundImage;
        protected Image upButtonBackgroundImage;
        protected Image downButtonBackgroundImage;
        protected bool downButtonVisible = true;
        protected bool autoHide;
        protected DataGridViewVerticalScrollHandler dataGridViewVerticalScrollHandler;
        protected ScrollableControlVerticalScrollHandler scrollableControlVerticalScrollHandler;
        protected ScrollViewerVerticalScrollHandler scrollViewerVerticalScrollHandler;

        public event EventHandler UpButtonClick;
        public event EventHandler DownButtonClick; 

        public VerticalScrollBarView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.Margin = new Padding(0, 0, 0, 0);
            autoHide = false;

            upButtonDisabledBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Up_Button_Disabled;
            downButtonDisabledBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Down_Button_Disabled;
            upButtonBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Up_Button;
            downButtonBackgroundImage = Semnox.Core.GenericUtilities.Properties.Resources.Down_Button;
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

        public Image UpButtonBackgroundImage
        {
            get
            {
                return upButtonBackgroundImage;
            }

            set
            {
                upButtonBackgroundImage = value;
            }
        }
        public Image UpButtonDisabledBackgroundImage
        {
            get
            {
                return upButtonDisabledBackgroundImage;
            }

            set
            {
                upButtonDisabledBackgroundImage = value;
            }
        }

        public Image DownButtonBackgroundImage
        {
            get
            {
                return downButtonBackgroundImage;
            }

            set
            {
                downButtonBackgroundImage = value;
            }
        }
        public Image DownButtonDisabledBackgroundImage
        {
            get
            {
                return downButtonDisabledBackgroundImage;
            }

            set
            {
                downButtonDisabledBackgroundImage = value;
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
                    dataGridViewVerticalScrollHandler = new DataGridViewVerticalScrollHandler(this, dataGridView);
                    btnUp.Click += dataGridViewVerticalScrollHandler.btnUp_Click;
                    btnDown.Click += dataGridViewVerticalScrollHandler.btnDown_Click;
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
                    scrollableControlVerticalScrollHandler = new ScrollableControlVerticalScrollHandler(this, scrollableControl);
                    btnUp.Click += scrollableControlVerticalScrollHandler.btnUp_Click;
                    btnDown.Click += scrollableControlVerticalScrollHandler.btnDown_Click;
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
                    scrollViewerVerticalScrollHandler = new ScrollViewerVerticalScrollHandler(this, scrollViewer);
                    btnUp.Click += scrollViewerVerticalScrollHandler.btnUp_Click;
                    btnDown.Click += scrollViewerVerticalScrollHandler.btnDown_Click;
                }
            }
        }

        public void UpdateUpButtonStatus(bool visible)
        {
            log.LogMethodEntry(visible);
            upButtonVisible = visible;
            btnUp.Enabled = visible;
            if (upButtonVisible)
            {
                btnUp.BackgroundImage = upButtonBackgroundImage;
            }
            else
            {
                btnUp.BackgroundImage = upButtonDisabledBackgroundImage;
            }
            AutoHideScrollBar();
            log.LogMethodExit();
        }

        public void UpdateDownButtonStatus(bool visible)
        {
            log.LogMethodEntry(visible);
            downButtonVisible = visible;
            btnDown.Enabled = visible;
            if (downButtonVisible)
            {
                btnDown.BackgroundImage = downButtonBackgroundImage;
            }
            else
            {
                btnDown.BackgroundImage = downButtonDisabledBackgroundImage;
            }
            AutoHideScrollBar();
            log.LogMethodExit();
        }

        private void AutoHideScrollBar()
        {
            log.LogMethodEntry();
            if (autoHide)
            {
                this.Visible = upButtonVisible || downButtonVisible;
            }
            log.LogMethodExit();
        }

        public void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            if (dataGridViewVerticalScrollHandler != null)
            {
                dataGridViewVerticalScrollHandler.UpdateButtonStatus();
            }
            if(scrollableControlVerticalScrollHandler != null)
            {
                scrollableControlVerticalScrollHandler.UpdateButtonStatus();
            }
            if(scrollViewerVerticalScrollHandler != null)
            {
                scrollViewerVerticalScrollHandler.UpdateButtonStatus();
            }
            log.LogMethodExit();
        }

        public void ScrollDown()
        {
            btnDown.PerformClick();
        }


        public void ScrollUp()
        {
            btnUp.PerformClick();
        }

        public bool CanScrollUp
        {
            get { return btnUp.Enabled; }
        }

        public bool CanScrollDown
        {
            get { return btnDown.Enabled; }
        }

        public void ScrollToBottom()
        {
            while (CanScrollDown)
            {
                ScrollDown();
            }
        }

        public void ScrollToTop()
        {
            while (CanScrollUp)
            {
                ScrollUp();
            }
        }

        public void GenerateUpButtonClick()
        {
            if(UpButtonClick != null)
            {
                UpButtonClick(this, EventArgs.Empty);
            }
        }
        public void GenerateDownButtonClick()
        {
            if (DownButtonClick != null)
            {
                DownButtonClick(this, EventArgs.Empty);
            }
        }
    }
}
