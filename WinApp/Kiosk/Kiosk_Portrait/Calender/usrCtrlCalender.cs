/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for Playground Quantity Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.150.0.0   21-Oct-2021   Sathyavathi             Created for Check-In feature Phase 2
 *2.150.0.0   02-Dec-2022   Sathyavathi             Check-In feature Phase-2 Additional features
 *2.150.3.0   12-Apr-2023      Sathyavathi        modified to make it generic
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Collections.Generic;

namespace Parafait_Kiosk
{
    public partial class usrCtrlCalender : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal delegate void ValueSelected(CalenderElement ele, string value);
        internal ValueSelected valueSelected;
        private CalenderElement calElement;

        private string selectedValue;

        public enum CalenderElement
        {
            DAY = 0,
            MONTH = 1,
            YEAR = 2,
            HOUR = 3,
            MINUTE = 4,
            AM_PM = 5
        };

        public usrCtrlCalender(CalenderElement element)
        {
            log.LogMethodEntry(element);
            InitializeComponent();
            this.calElement = element;
            try
            {
                SetDisplayElements();
                switch (element)
                {
                    case CalenderElement.DAY:
                        LoadDayOfCalender();
                        break;

                    case CalenderElement.MONTH:
                        LoadMonthOfCalender();
                        break;

                    case CalenderElement.YEAR:
                        LoadYearOfCalender();
                        break;

                    case CalenderElement.HOUR:
                        LoadHour();
                        break;

                    case CalenderElement.MINUTE:
                        LoadMinutes();
                        break;

                    case CalenderElement.AM_PM:
                        LoadAMPM();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in usrCtrlCalender" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {

                SetBackgroundImage();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in SetDisplayElements in usrCtrlCalender" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadDayOfCalender()
        {
            log.LogMethodEntry();
            try
            {
                int rowId;
                int rowIndex = 0;
                int colIndex = 0;
                for (int i = 1; i <= 31; i++)
                {
                    if (colIndex % 3 == 0)
                    {
                        rowId = dgvCalender.Rows.Add();
                        DataGridViewRow newRow = dgvCalender.Rows[rowId];
                        rowIndex = rowId;
                        colIndex = 0;
                    }

                    dgvCalender.Rows[rowIndex].Cells[colIndex].Value = i.ToString();
                    colIndex++;
                }
                dgvCalender.AllowUserToAddRows = false;
                dgvCalender.Refresh();
                vScrollBarCalender.UpdateButtonStatus();
                vScrollBarCalender.AutoScroll = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadDayOfCalender in usrCtrlCalender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadMonthOfCalender()
        {
            log.LogMethodEntry();
            try
            {
                int rowId;
                int rowIndex = 0;
                int colIndex = 0;
                string[] arrOfMonthNames = System.Globalization.DateTimeFormatInfo.InvariantInfo.AbbreviatedMonthGenitiveNames;
                List<string> monthNames = new List<string>(arrOfMonthNames);
                for (int i = 0; i < monthNames.Count; i++)
                {
                    if (colIndex % 3 == 0)
                    {
                        rowId = dgvCalender.Rows.Add();
                        DataGridViewRow newRow = dgvCalender.Rows[rowId];
                        rowIndex = rowId;
                        colIndex = 0;
                    }
                    if (!string.IsNullOrWhiteSpace(monthNames[i]))
                    {
                        dgvCalender.Rows[rowIndex].Cells[colIndex].Value = monthNames[i];
                        colIndex++;
                    }
                }
                dgvCalender.AllowUserToAddRows = false;
                flpCalender.Controls.Remove(vScrollBarCalender);
                flpCalender.Size = this.Size = dgvCalender.Size;
                vScrollBarCalender.UpdateButtonStatus();
                vScrollBarCalender.AutoScroll = true;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadMonthOfCalender in usrCtrlCalender form: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadYearOfCalender()
        {
            log.LogMethodEntry();
            try
            {
                int rowId;
                int rowIndex = 0;
                int colIndex = 0;
                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 120; i--)
                {
                    if (colIndex % 3 == 0)
                    {
                        rowId = dgvCalender.Rows.Add();
                        DataGridViewRow newRow = dgvCalender.Rows[rowIndex];
                        rowIndex = rowId;
                        colIndex = 0;
                    }

                    dgvCalender.Rows[rowIndex].Cells[colIndex].Value = i.ToString();
                    colIndex++;
                }
                dgvCalender.AllowUserToAddRows = false;
                vScrollBarCalender.UpdateButtonStatus();
                vScrollBarCalender.AutoScroll = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadYearOfCalender in usrCtrlCalender screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadHour()
        {
            log.LogMethodEntry();
            try
            {
                LoadGrid(1, 12);//Hour 1 to 12.

                CalenderCol1.Width = CalenderCol2.Width = CalenderCol3.Width = 85;
                flpCalender.Size = this.Size = dgvCalender.Size = new Size(251, 240);
                dgvCalender.AllowUserToAddRows = false;
                flpCalender.Controls.Remove(vScrollBarCalender);
                vScrollBarCalender.UpdateButtonStatus();
                vScrollBarCalender.AutoScroll = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadHour in usrCtrlCalender screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadMinutes()
        {
            log.LogMethodEntry();
            try
            {
                LoadGrid(0, 59);//Minutes 0 to 59.

                CalenderCol1.Width = CalenderCol2.Width = CalenderCol3.Width = 102;
                flpCalender.Size = this.Size = dgvCalender.Size = new Size(304, 240);
                dgvCalender.AllowUserToAddRows = false;
                vScrollBarCalender.UpdateButtonStatus();
                vScrollBarCalender.AutoScroll = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadMinutes in usrCtrlCalender screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadAMPM()
        {
            log.LogMethodEntry();
            try
            {
                int rowId;
                rowId = dgvCalender.Rows.Add();
                dgvCalender.Rows[rowId].Cells[0].Value = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "AM");
                

                rowId = dgvCalender.Rows.Add();
                dgvCalender.Rows[rowId].Cells[0].Value = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "PM");

                CalenderCol2.Visible = CalenderCol3.Visible = false;
                flpCalender.MinimumSize = this.MinimumSize = dgvCalender.MinimumSize = new Size(105, 120);
                flpCalender.Size = this.Size = dgvCalender.Size = new Size(110, 120);
                dgvCalender.AllowUserToAddRows = false;
                flpCalender.Controls.Remove(vScrollBarCalender);
                flpCalender.Size = this.Size = dgvCalender.Size;
                vScrollBarCalender.Size = new Size(vScrollBarCalender.Width, dgvCalender.Height);
                vScrollBarCalender.UpdateButtonStatus();
                vScrollBarCalender.AutoScroll = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in LoadHour in usrCtrlcalender screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadGrid(int startIndex, int endIndex)
        {
            log.LogMethodEntry();

            int rowId;
            int rowIndex = 0;
            int colIndex = 0;

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (colIndex % 3 == 0)
                {
                    rowId = dgvCalender.Rows.Add();
                    rowIndex = rowId;
                    colIndex = 0;
                }

                dgvCalender.Rows[rowIndex].Cells[colIndex].Value = i.ToString("00");
                colIndex++;
            }

            log.LogMethodExit();
        }

        private void SetBackgroundImage()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                dgvCalender.RowTemplate.DefaultCellStyle.ForeColor =
                    dgvCalender.RowTemplate.DefaultCellStyle.ForeColor =
                    dgvCalender.RowTemplate.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.CalenderGridTextForeColor;

                this.vScrollBarCalender.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in setting customized font colors for the UI elements of usrCtrlCalender: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvCalender_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                selectedValue = dgvCalender.CurrentCell.Value.ToString();
                FireDelegateMethod();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ERROR: Unhandled exception in dgvCalender_CellClick of usrCtrlCalender: " + ex.Message);
            }
        }

        private void FireDelegateMethod()
        {
            log.LogMethodEntry();
            try
            {
                if (valueSelected != null)
                {
                    valueSelected(calElement, selectedValue);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception while Firing usrCtrlCalender Delegate Method: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public void UpdateButtonStatus()
        {
            log.LogMethodEntry();
            try
            {
                vScrollBarCalender.UpdateButtonStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        internal void HighlightSelectedVaue(string searchValue)
        {
            //TODO: Create a Dictionary so that performance is better.
            log.LogMethodEntry();
            try
            {
                int rowIndex = -1;
                int colIndex = -1;
                bool found = false;

                foreach (DataGridViewRow row in dgvCalender.Rows)
                {
                    foreach (DataGridViewColumn col in dgvCalender.Columns)
                    {
                        if (row.Cells[col.Index].Value != null && row.Cells[col.Index].Value.ToString().Equals(searchValue))
                        {
                            colIndex = col.Index;
                            rowIndex = row.Index;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        break;
                }

                dgvCalender.Rows[rowIndex].Cells[colIndex].Selected = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in SetSelectedVaue() in usrCtrlCalender: " + ex.Message);
            }
            log.LogMethodExit();
        }

        internal void DisableCell(string cellValue)
        {
            //TODO: Create a Dictionary so that performance is better.
            log.LogMethodEntry();
            try
            {
                int rowIndex = -1;
                int colIndex = -1;
                bool found = false;

                foreach (DataGridViewRow row in dgvCalender.Rows)
                {
                    foreach (DataGridViewColumn col in dgvCalender.Columns)
                    {
                        if (row.Cells[col.Index].Value != null && row.Cells[col.Index].Value.ToString().Equals(cellValue))
                        {
                            colIndex = col.Index;
                            rowIndex = row.Index;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        break;
                }

                dgvCalender.Rows[rowIndex].Cells[colIndex].ReadOnly = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("ERROR: Unhandled exception in SetSelectedVaue() in usrCtrlCalender: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
