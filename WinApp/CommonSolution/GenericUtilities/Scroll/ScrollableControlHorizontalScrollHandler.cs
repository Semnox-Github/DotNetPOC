/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - ScrollableControlHorizontalScrollHandler
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
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public class ScrollableControlHorizontalScrollHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HorizontalScrollBarView horizontalScrollBarView;
        private ScrollableControl scrollableControl;


        public ScrollableControlHorizontalScrollHandler(HorizontalScrollBarView horizontalScrollBarView, ScrollableControl scrollableControl)
        {
            log.LogMethodEntry(horizontalScrollBarView, scrollableControl);
            this.scrollableControl = scrollableControl;
            this.horizontalScrollBarView = horizontalScrollBarView;
            scrollableControl.AutoScroll = true;

            scrollableControl.Resize += FlowLayoutPanel_Resize;
            scrollableControl.Scroll += ScrollableControl_Scroll;
            log.LogMethodExit();
        }

        private void ScrollableControl_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private void FlowLayoutPanel_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        internal void btnLeft_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollableControl != null)
            {
                horizontalScrollBarView.GenerateLeftButtonClick();
                int largeChange = (int)(scrollableControl.HorizontalScroll.LargeChange * 0.75);

                if ((scrollableControl.HorizontalScroll.Value - largeChange) > scrollableControl.HorizontalScroll.Minimum)
                {
                    scrollableControl.HorizontalScroll.Value -= largeChange;
                }
                else
                {
                    scrollableControl.HorizontalScroll.Value = scrollableControl.HorizontalScroll.Minimum;
                }
                scrollableControl.PerformLayout();
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void btnRight_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollableControl != null)
            {
                horizontalScrollBarView.GenerateRightButtonClick();
                int largeChange = (int)(scrollableControl.HorizontalScroll.LargeChange * 0.75);
                if ((scrollableControl.HorizontalScroll.Value + largeChange) < scrollableControl.HorizontalScroll.Maximum)
                {
                    scrollableControl.HorizontalScroll.Value = scrollableControl.HorizontalScroll.Value + largeChange;
                }
                else
                {
                    scrollableControl.HorizontalScroll.Value = scrollableControl.HorizontalScroll.Maximum;
                }
                scrollableControl.PerformLayout();
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            if (scrollableControl != null)
            {
                if (scrollableControl.HorizontalScroll.Visible)
                {
                    if (scrollableControl.HorizontalScroll.Value == scrollableControl.HorizontalScroll.Minimum)
                    {
                        horizontalScrollBarView.UpdateLeftButtonStatus(false);
                    }
                    else
                    {
                        horizontalScrollBarView.UpdateLeftButtonStatus(true);
                    }

                    if (scrollableControl.HorizontalScroll.Value >= (scrollableControl.HorizontalScroll.Maximum - scrollableControl.HorizontalScroll.LargeChange))
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
