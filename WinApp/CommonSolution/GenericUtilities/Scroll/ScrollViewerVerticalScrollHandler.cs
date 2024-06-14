/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - ScrollViewerVerticalScrollHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's. 
 *2.150.1       22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities
{
    public class ScrollViewerVerticalScrollHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VerticalScrollBarView verticalScrollBarView;
        private System.Windows.Controls.ScrollViewer scrollViewer;


        public ScrollViewerVerticalScrollHandler(VerticalScrollBarView verticalScrollBarView, System.Windows.Controls.ScrollViewer scrollViewer)
        {
            log.LogMethodEntry(verticalScrollBarView, scrollViewer);
            this.scrollViewer = scrollViewer;
            this.verticalScrollBarView = verticalScrollBarView;
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            log.LogMethodExit();
        }

        private void ScrollViewer_ScrollChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private double LargeChange
        {
            get
            {
                return scrollViewer.ViewportHeight;
            }
        }

        private double Minimum
        {
            get
            {
                return 0;
            }
        }

        private double Maximum
        {
            get
            {
                return scrollViewer.ScrollableHeight;
            }
        }

        internal void btnUp_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollViewer != null)
            {
                verticalScrollBarView.GenerateUpButtonClick();
                if ((scrollViewer.VerticalOffset - LargeChange) > Minimum)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - LargeChange);
                }
                else
                {
                    scrollViewer.ScrollToTop();
                }
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void btnDown_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollViewer != null)
            {
                verticalScrollBarView.GenerateDownButtonClick();
                if ((scrollViewer.VerticalOffset + LargeChange) < Maximum)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + LargeChange);
                }
                else
                {
                    scrollViewer.ScrollToBottom();
                }
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            if (scrollViewer != null)
            {
                if (scrollViewer.ViewportHeight < scrollViewer.ScrollableHeight)
                {
                    if (scrollViewer.VerticalOffset == Minimum)
                    {
                        verticalScrollBarView.UpdateUpButtonStatus(false);
                    }
                    else
                    {
                        verticalScrollBarView.UpdateUpButtonStatus(true);
                    }

                    if (scrollViewer.VerticalOffset == Maximum)
                    {
                        verticalScrollBarView.UpdateDownButtonStatus(false);
                    }
                    else
                    {
                        verticalScrollBarView.UpdateDownButtonStatus(true);
                    }
                }
                else
                {
                    verticalScrollBarView.UpdateUpButtonStatus(false);
                    verticalScrollBarView.UpdateDownButtonStatus(false);
                }
            }
            log.LogMethodExit();
        }
    }
}
