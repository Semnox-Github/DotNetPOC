/********************************************************************************************
 * Project Name - DataGridViewVerticalScrollHandler
 * Description  - Class of DataGridViewVerticalScrollHandler
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's. 
 *2.150.1       22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 **********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public class DataGridViewVerticalScrollHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VerticalScrollBarView verticalScrollBarView;
        private DataGridView dataGridView;
        public DataGridViewVerticalScrollHandler(VerticalScrollBarView verticalScrollBarView, DataGridView dataGridView)
        {
            log.LogMethodEntry(verticalScrollBarView, dataGridView);
            this.dataGridView = dataGridView;
            this.verticalScrollBarView = verticalScrollBarView;

            if (dataGridView.ScrollBars == ScrollBars.Both)
            {
                dataGridView.ScrollBars = ScrollBars.Horizontal;
            }
            else if (dataGridView.ScrollBars == ScrollBars.Vertical)
            {
                dataGridView.ScrollBars = ScrollBars.None;
            }
            dataGridView.DataBindingComplete += DataGridView_DataBindingComplete;
            dataGridView.Resize += DataGridView_Resize;
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private void DataGridView_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private void DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        internal void btnUp_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dataGridView != null)
            {
                verticalScrollBarView.GenerateUpButtonClick();
                if (dataGridView.FirstDisplayedScrollingRowIndex < dataGridView.DisplayedRowCount(false))
                {
                    dataGridView.FirstDisplayedScrollingRowIndex = GetNextVisibleRow(0);
                }
                else
                {
                    dataGridView.FirstDisplayedScrollingRowIndex = GetPreviousVisibleRow(dataGridView.FirstDisplayedScrollingRowIndex - dataGridView.DisplayedRowCount(false));
                }
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        private int GetNextVisibleRow(int rowNumber, int level = 0)
        {
            log.LogMethodEntry(rowNumber, level);
            if (level >= 3)
            {
                return rowNumber;
            }
            int result = rowNumber;
            for (int i = rowNumber; i < dataGridView.Rows.Count; i++)
            {
                if(dataGridView.Rows[i].Visible)
                {
                    result = i;
                    break;
                }
            }
            if(dataGridView.Rows[result].Visible == false)
            {
                result = GetPreviousVisibleRow(rowNumber, level + 1);
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetPreviousVisibleRow(int rowNumber, int level = 0)
        {
            log.LogMethodEntry(rowNumber, level);
            if (level >= 3)
            {
                return rowNumber;
            }
            int result = rowNumber;
            for (int i = rowNumber; i >= 0; i--)
            {
                if (dataGridView.Rows[i].Visible)
                {
                    result = i;
                    break;
                }
            }
            if (dataGridView.Rows[result].Visible == false)
            {
                result = GetNextVisibleRow(rowNumber, level + 1);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void btnDown_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e); 
            if (dataGridView != null)
            {
                verticalScrollBarView.GenerateDownButtonClick();
                if (dataGridView.RowCount > (dataGridView.FirstDisplayedScrollingRowIndex + dataGridView.DisplayedRowCount(false)))
                {
                    dataGridView.FirstDisplayedScrollingRowIndex = GetNextVisibleRow(dataGridView.FirstDisplayedScrollingRowIndex + dataGridView.DisplayedRowCount(false));
                }
                else
                {
                    dataGridView.FirstDisplayedScrollingRowIndex = GetPreviousVisibleRow(dataGridView.RowCount - 1);
                }
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            if (dataGridView != null)
            {
                if(dataGridView.Rows.Count == 0)
                {
                    verticalScrollBarView.UpdateUpButtonStatus(false);
                    verticalScrollBarView.UpdateDownButtonStatus(false);
                    return;
                }
                if (dataGridView.FirstDisplayedScrollingRowIndex == 0)
                {
                    verticalScrollBarView.UpdateUpButtonStatus(false);
                }
                else
                {
                    verticalScrollBarView.UpdateUpButtonStatus(true);
                }

                if (dataGridView.FirstDisplayedScrollingRowIndex == (dataGridView.RowCount - dataGridView.DisplayedRowCount(false)))
                {
                    verticalScrollBarView.UpdateDownButtonStatus(false);
                }
                else
                {
                    verticalScrollBarView.UpdateDownButtonStatus(true);
                }
            }
            log.LogMethodExit();
        }
    }
}
