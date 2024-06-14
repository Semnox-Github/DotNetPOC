/********************************************************************************************
 * Project Name - DataGridViewHorizontalScrollHandler
 * Description  - Class of DataGridViewHorizontalScrollHandler
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
    public class DataGridViewHorizontalScrollHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataGridView dataGridView;
        private HorizontalScrollBarView horizontalScrollBarView;
        public DataGridViewHorizontalScrollHandler(HorizontalScrollBarView horizontalScrollBarView, DataGridView dataGridView)
        {
            log.LogMethodEntry(horizontalScrollBarView,  dataGridView);
            this.dataGridView = dataGridView;
            this.horizontalScrollBarView = horizontalScrollBarView;

            if (dataGridView.ScrollBars == ScrollBars.Both)
            {
                dataGridView.ScrollBars = ScrollBars.Horizontal;
            }
            else if (dataGridView.ScrollBars == ScrollBars.Horizontal)
            {
                dataGridView.ScrollBars = ScrollBars.None;
            }
            dataGridView.DataBindingComplete += DataGridView_DataBindingComplete;
            dataGridView.Resize += DataGridView_Resize;
            dataGridView.Scroll += DataGridView_Scroll;
            UpdateButtonStatus();
            log.LogMethodExit();
        }

        private void DataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender,e);
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

        private int GetFirstVisibleNonFrozenColumnIndex()
        {
            log.LogMethodEntry();
            int result = 0;
            if (dataGridView != null)
            {
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    if (dataGridView.Columns[i].Frozen == false &&
                       dataGridView.Columns[i].Visible == true)
                    {
                        result = i;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetLastVisibleNonFrozenColumnIndex()
        {
            log.LogMethodEntry();
            int result = 0;
            if (dataGridView != null)
            {
                for (int i = dataGridView.ColumnCount - 1; i >= 0; i--)
                {
                    if (dataGridView.Columns[i].Frozen == false &&
                       dataGridView.Columns[i].Visible == true)
                    {
                        result = i;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        internal void btnLeft_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
            if (dataGridView != null)
            {
                horizontalScrollBarView.GenerateLeftButtonClick();
                if (dataGridView.FirstDisplayedScrollingColumnIndex <= dataGridView.DisplayedColumnCount(false))
                {
                    SetFirstDisplayedScrollingColumnIndex(GetFirstVisibleNonFrozenColumnIndex(), true);
                    dataGridView.HorizontalScrollingOffset = 0;
                }
                else
                {
                    SetFirstDisplayedScrollingColumnIndex(dataGridView.FirstDisplayedScrollingColumnIndex - dataGridView.DisplayedColumnCount(false), false);
                }
                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        internal void btnRight_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dataGridView != null)
            {
                horizontalScrollBarView.GenerateRightButtonClick();
                int noOfColumnsToMove = dataGridView.DisplayedColumnCount(false);
                if(noOfColumnsToMove == 0)
                {
                    noOfColumnsToMove = 1;
                }
                if (dataGridView.ColumnCount > (dataGridView.FirstDisplayedScrollingColumnIndex + noOfColumnsToMove))
                {
                    SetFirstDisplayedScrollingColumnIndex(dataGridView.FirstDisplayedScrollingColumnIndex + noOfColumnsToMove, true);
                }
                else
                {
                    SetFirstDisplayedScrollingColumnIndex(GetLastVisibleNonFrozenColumnIndex(), false);
                }

                UpdateButtonStatus();
                log.LogMethodExit();
            }
        }

        private void SetFirstDisplayedScrollingColumnIndex(int index, bool increamentColumns)
        {
            log.LogMethodEntry(index, increamentColumns);
            if (dataGridView.Columns.Count <= index || index < 0)
            {
                return;
            }
            if (dataGridView.FirstDisplayedScrollingColumnIndex >= 0)
            {
                if (dataGridView.Columns[index].Visible == false)
                {
                    if (increamentColumns)
                    {
                        SetFirstDisplayedScrollingColumnIndex(index + 1, increamentColumns);
                    }
                    else
                    {
                        SetFirstDisplayedScrollingColumnIndex(index - 1, increamentColumns);
                    }
                    return;
                }
                int firstVisibleNonFrozenColumn = GetFirstVisibleNonFrozenColumnIndex();
                if (index < firstVisibleNonFrozenColumn)
                {
                    dataGridView.FirstDisplayedScrollingColumnIndex = firstVisibleNonFrozenColumn;
                }
                else if (index > GetLastVisibleNonFrozenColumnIndex())
                {
                    dataGridView.FirstDisplayedScrollingColumnIndex = GetLastVisibleNonFrozenColumnIndex();
                }
                else
                {
                    dataGridView.FirstDisplayedScrollingColumnIndex = index;
                }
            }
            log.LogMethodExit();
        }

        internal void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            if (dataGridView != null)
            {
                int firstVisibleNonFrozenColumn = GetFirstVisibleNonFrozenColumnIndex();
                if (IsColumnVisibleFully(firstVisibleNonFrozenColumn) && dataGridView.HorizontalScrollingOffset == 0)
                {
                    horizontalScrollBarView.UpdateLeftButtonStatus(false);
                }
                else
                {
                    for (int i = firstVisibleNonFrozenColumn + 1; i <= GetLastVisibleNonFrozenColumnIndex(); i++)
                    {
                        horizontalScrollBarView.UpdateLeftButtonStatus(false);
                        if (dataGridView.Columns[i].Displayed)
                        {
                            horizontalScrollBarView.UpdateLeftButtonStatus(true);
                            break;
                        }
                    }
                }

                if (IsColumnVisibleFully(GetLastVisibleNonFrozenColumnIndex()))
                {
                    horizontalScrollBarView.UpdateRightButtonStatus(false);
                }
                else
                {
                    horizontalScrollBarView.UpdateRightButtonStatus(true);
                }
            }
            log.LogMethodExit();
        }

        private bool IsColumnVisibleFully(int columnIndex)
        {
            log.LogMethodEntry(columnIndex);
            bool result = false;
            if(dataGridView.Columns.Count > columnIndex)
            {
                if (dataGridView.Columns[columnIndex].Displayed)
                {
                    result = dataGridView.GetColumnDisplayRectangle(columnIndex, false).Width == dataGridView.GetColumnDisplayRectangle(columnIndex, true).Width;
                }
            }
            log.LogMethodExit(result); 
            return result;
        }
    }
}
