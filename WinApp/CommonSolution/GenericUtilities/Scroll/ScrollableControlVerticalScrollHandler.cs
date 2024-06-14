/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - ScrollableControlVerticalScrollHandler
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
    public class ScrollableControlVerticalScrollHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VerticalScrollBarView verticalScrollBarView;
        private ScrollableControl scrollableControl;


        public ScrollableControlVerticalScrollHandler(VerticalScrollBarView verticalScrollBarView, ScrollableControl scrollableControl)
        {
            log.LogMethodEntry(verticalScrollBarView , scrollableControl);
            this.scrollableControl = scrollableControl;
            this.verticalScrollBarView = verticalScrollBarView;
            scrollableControl.AutoScroll = true;

            scrollableControl.Resize += FlowLayoutPanel_Resize;
            scrollableControl.Scroll += ScrollableControl_Scroll;
            scrollableControl.ControlAdded += ScrollableControl_ControlAdded;
            scrollableControl.ControlRemoved += ScrollableControl_ControlRemoved;
            log.LogMethodExit();
        }

        private void ScrollableControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private void ScrollableControl_ControlAdded(object sender, ControlEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
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

        internal void btnUp_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollableControl != null)
            {
                verticalScrollBarView.GenerateUpButtonClick();
                int largeChange = (int)(scrollableControl.VerticalScroll.LargeChange * 0.75);
                if ((scrollableControl.VerticalScroll.Value - largeChange) > scrollableControl.VerticalScroll.Minimum)
                {
                    scrollableControl.VerticalScroll.Value -= largeChange;
                }
                else
                {
                    scrollableControl.VerticalScroll.Value = scrollableControl.VerticalScroll.Minimum;
                }
                scrollableControl.PerformLayout();
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void btnDown_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (scrollableControl != null)
            {
                verticalScrollBarView.GenerateDownButtonClick();
                int largeChange = (int)(scrollableControl.VerticalScroll.LargeChange * 0.75);
                if ((scrollableControl.VerticalScroll.Value + largeChange) < scrollableControl.VerticalScroll.Maximum)
                {
                    scrollableControl.VerticalScroll.Value = scrollableControl.VerticalScroll.Value + largeChange;
                }
                else
                {
                    scrollableControl.VerticalScroll.Value = scrollableControl.VerticalScroll.Maximum;
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
                if(scrollableControl.VerticalScroll.Visible)
                {
                    if (scrollableControl.VerticalScroll.Value == scrollableControl.VerticalScroll.Minimum)
                    {
                        verticalScrollBarView.UpdateUpButtonStatus(false);
                    }
                    else
                    {
                        verticalScrollBarView.UpdateUpButtonStatus(true);
                    } 

                    if (scrollableControl.VerticalScroll.Value >= (scrollableControl.VerticalScroll.Maximum - scrollableControl.VerticalScroll.LargeChange))
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
