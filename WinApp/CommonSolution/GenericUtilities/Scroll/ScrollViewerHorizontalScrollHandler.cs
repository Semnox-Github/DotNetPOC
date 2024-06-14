/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - ScrollViewerHorizontalScrollHandler
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
    public class ScrollViewerHorizontalScrollHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HorizontalScrollBarView horizontalScrollBarView;
        private System.Windows.Controls.ScrollViewer scrollViewer;


        public ScrollViewerHorizontalScrollHandler(HorizontalScrollBarView horizontalScrollBarView, System.Windows.Controls.ScrollViewer scrollViewer)
        {
            log.LogMethodEntry(horizontalScrollBarView, scrollViewer);
            this.scrollViewer = scrollViewer;
            this.horizontalScrollBarView = horizontalScrollBarView;
            scrollViewer.ScrollChanged += FlowLayoutPanel_Resize;
            log.LogMethodExit();
        }

        private void FlowLayoutPanel_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private double LargeChange
        {
            get
            {
                return scrollViewer.ViewportWidth;
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
                return scrollViewer.ScrollableWidth;
            }
        }

        internal void btnLeft_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollViewer != null)
            {
                horizontalScrollBarView.GenerateLeftButtonClick();
                if ((scrollViewer.HorizontalOffset - LargeChange) > Minimum)
                {
                    scrollViewer.ScrollToHorizontalOffset((scrollViewer.HorizontalOffset - LargeChange));
                }
                else
                {
                    scrollViewer.ScrollToLeftEnd();
                }
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void btnRight_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollViewer != null)
            {
                horizontalScrollBarView.GenerateRightButtonClick();
                if ((scrollViewer.HorizontalOffset + LargeChange) < Maximum)
                {
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + LargeChange);
                }
                else
                {
                    scrollViewer.ScrollToRightEnd();
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
                if (scrollViewer.ViewportWidth < scrollViewer.ScrollableWidth)
                {
                    if (scrollViewer.HorizontalOffset == Minimum)
                    {
                        horizontalScrollBarView.UpdateLeftButtonStatus(false);
                    }
                    else
                    {
                        horizontalScrollBarView.UpdateLeftButtonStatus(true);
                    }

                    if (scrollViewer.HorizontalOffset == Maximum)
                    {
                        horizontalScrollBarView.UpdateRightButtonStatus(false);
                    }
                    else
                    {
                        horizontalScrollBarView.UpdateRightButtonStatus(true);
                    }
                }
                else
                {
                    horizontalScrollBarView.UpdateLeftButtonStatus(false);
                    horizontalScrollBarView.UpdateRightButtonStatus(false);
                }
            }
            log.LogMethodExit();
        }
    }
}
