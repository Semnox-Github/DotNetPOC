/* Project Name - AttractionSeatBooking
* Description  - AttractionSeatBooking form
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70.0      17-Apr-2019    Guru S A             Modified for Booking phase 2 enhancement changes 
*            16-Aug-2019    Mathew Ninan         Externalized seat images
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;

namespace Parafait_POS
{
    public partial class AttractionSeatBooking : Form
    {
        Utilities Utilities = POSStatic.Utilities;
        int AttractionScheduleId, SeatCount;
        private int facilityId;
        private int facilityMapId;
        public DateTime scheduleFromDate;
        public DateTime scheduleToDate;
        private List<FacilityDTO> facilityDTOList;
        private Image seatAvailableImage = null;
        private Image seatRedImage = null;
        private Image seatGreenImage = null;
        private Image seatAccessibilityImage = null;
        private Image seatBlankImage = null;
        private Image seatDisabledImage = null;


        internal class selectedSeats
        {
            internal int facilityId;
            internal List<int> SelectedSeats = new List<int>();
            internal List<string> SelectedSeatNames = new List<string>();
        }
        List<selectedSeats> seletedSeatsFacilityWise = new List<selectedSeats>();
        List<int> paramSelectedSeats = new List<int>();
        //List<KeyValuePair<int, List<object>>> facilitywiseSelectedSeats = new List<KeyValuePair<int, List<object>>>();
        //List<KeyValuePair<int, List<string>>> facilitywiseSelectedSeatNames = new List<KeyValuePair<int, List<string>>>();
        public List<int>  GetSelectedSeatIds
            { get { return GetSelectedSeatIDsFromFacilities(); }
            }

        public List<string> GetSelectedSeatNames
        {
            get { return GetSelectedSeatNamesFromFacilities(); }
        }

        private List<int> inMemorySeats = new List<int>();

        private List<int> GetSelectedSeatIDsFromFacilities()
        {
            log.LogMethodEntry();
            List<int> selectedSeatIds = null;
            for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
            {
                if (seletedSeatsFacilityWise[i].SelectedSeats != null && seletedSeatsFacilityWise[i].SelectedSeats.Any())
                {
                    if (selectedSeatIds != null)
                    {
                        selectedSeatIds.AddRange(seletedSeatsFacilityWise[i].SelectedSeats);
                    }
                    else
                    {
                        selectedSeatIds = new List<int>();
                        selectedSeatIds.AddRange(seletedSeatsFacilityWise[i].SelectedSeats);
                    }
                }
            }
            log.LogMethodExit(selectedSeatIds);
            return selectedSeatIds;
        }

        private List<string> GetSelectedSeatNamesFromFacilities()
        {
            log.LogMethodEntry();
            List<string> selectedSeatNames = null;
            for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
            {
                if (seletedSeatsFacilityWise[i].SelectedSeatNames != null && seletedSeatsFacilityWise[i].SelectedSeatNames.Any())
                {
                    if (selectedSeatNames != null)
                    {
                        selectedSeatNames.AddRange(seletedSeatsFacilityWise[i].SelectedSeatNames);
                    }
                    else
                    {
                        selectedSeatNames = new List<string>();
                        selectedSeatNames.AddRange(seletedSeatsFacilityWise[i].SelectedSeatNames);
                    }
                }
            }
            log.LogMethodExit(selectedSeatNames);
            return selectedSeatNames;
        }

        private bool userAction = true;
            
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        //public AttractionSeatBooking(int pAttractionScheduleId, int pFacilityId, DateTime pScheduleTime, int pSeatCount, object pSelectedSeats)
        public AttractionSeatBooking(int pAttractionScheduleId, int pFacilityMapId, DateTime pScheduleFromDate, DateTime pScheduleToDate, int pSeatCount, object pSelectedSeats, object pOtherSeats = null)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(pAttractionScheduleId, pFacilityMapId, pScheduleFromDate, pScheduleToDate, pSeatCount, pSelectedSeats);
            Utilities.setLanguage();
            InitializeComponent();
            AttractionScheduleId = pAttractionScheduleId;
            //this.facilityId = pFacilityId;
            this.facilityMapId = pFacilityMapId;
            SeatCount = pSeatCount;
            scheduleFromDate = pScheduleFromDate;
            scheduleToDate = pScheduleToDate;
            if (pSelectedSeats != null)
            {
                foreach(int seat in (pSelectedSeats as List<int>))
                    paramSelectedSeats.Add(seat);
            }
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - 200;
            bool found = false;
            GenericUtils genericUtils = new GenericUtils();
            try
            {
                seatAvailableImage = null;
                byte[] bytes = genericUtils.GetFileFromServer(Utilities, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\AvailableSeatImage.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    seatAvailableImage = System.Drawing.Image.FromStream(ms, false, true);
                    if (seatAvailableImage != null)
                        found = true;
                }

                if (!found)
                {
                    seatAvailableImage = Properties.Resources.AvailableSeat;
                }
            }
            catch
            {
                seatAvailableImage = Properties.Resources.AvailableSeat;
            }
            try
            {
                found = false;
                seatAccessibilityImage = null;
                byte[] bytes = genericUtils.GetFileFromServer(Utilities, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\AccessibilitySeatImage.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    seatAccessibilityImage = System.Drawing.Image.FromStream(ms, false, true);
                    if (seatAccessibilityImage != null)
                        found = true;
                }

                if (!found)
                {
                    seatAccessibilityImage = Properties.Resources.accessibility;
                }
            }
            catch
            {
                seatAccessibilityImage = Properties.Resources.accessibility;
            }
            try
            {
                found = false;
                seatBlankImage = null;
                byte[] bytes = genericUtils.GetFileFromServer(Utilities, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\BlankSeatImage.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    seatBlankImage = System.Drawing.Image.FromStream(ms, false, true);
                    if (seatBlankImage != null)
                        found = true;
                }

                if (!found)
                {
                    seatBlankImage = Properties.Resources.Blank;
                }
            }
            catch
            {
                seatBlankImage = Properties.Resources.Blank;
            }
            try
            {
                found = false;
                seatDisabledImage = null;
                byte[] bytes = genericUtils.GetFileFromServer(Utilities, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\DisabledSeatImage.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    seatDisabledImage = System.Drawing.Image.FromStream(ms, false, true);
                    if (seatDisabledImage != null)
                        found = true;
                }

                if (!found)
                {
                    seatDisabledImage = Properties.Resources.DisabledSeat;
                }
            }
            catch
            {
                seatDisabledImage = Properties.Resources.DisabledSeat;
            }
            try
            {
                found = false;
                seatGreenImage = null;
                byte[] bytes = genericUtils.GetFileFromServer(Utilities, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\GreenSeatImage.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    seatGreenImage = System.Drawing.Image.FromStream(ms, false, true);
                    if (seatGreenImage != null)
                        found = true;
                }

                if (!found)
                {
                    seatGreenImage = Properties.Resources.GreenSeat;
                }
            }
            catch
            {
                seatGreenImage = Properties.Resources.GreenSeat;
            }
            try
            {
                found = false;
                seatRedImage = null;
                byte[] bytes = genericUtils.GetFileFromServer(Utilities, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\RedSeatImage.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    seatRedImage = System.Drawing.Image.FromStream(ms, false, true);
                    if (seatRedImage != null)
                        found = true;
                }

                if (!found)
                {
                    seatRedImage = Properties.Resources.RedSeat;
                }
            }
            catch
            {
                seatRedImage = Properties.Resources.RedSeat;
            }

            if (pOtherSeats != null)
            {
                foreach (int seat in (pOtherSeats as List<int>))
                    inMemorySeats.Add(seat);
            }
            log.LogMethodExit();
        }

        private void AttractionSeatBooking_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblSeatsSelected.Text = "";
            userAction = false;
            //object o = Utilities.executeScalar(@"select isnull(ScreenPosition, 'Bottom') 
            //                                                         from CheckInFacility 
            //                                                        where FacilityId = (select FacilityId 
            //                                                                             from AttractionSchedules 
            //                                                                            where AttractionScheduleId = @AttractionScheduleId)",
            //                                           new SqlParameter[] { new SqlParameter("AttractionScheduleId", AttractionScheduleId) });

            //int? totalFACSeats = 0;
            if (facilityMapId > -1)
            {
                FacilityMapBL facilityMapBL = new FacilityMapBL(Utilities.ExecutionContext, facilityMapId);
                int totalBookedSeats = facilityMapBL.GetTotalBookedUnitsForAttraction(scheduleFromDate, scheduleToDate);
                lblBookedSeats.Text = totalBookedSeats.ToString();
                facilityDTOList = facilityMapBL.GetMappedFacilityDTOList();
                cmbFacility.DisplayMember = "FacilityName";
                cmbFacility.ValueMember = "FacilityId";
                cmbFacility.DataSource = facilityDTOList;
                if (facilityDTOList != null && facilityDTOList.Any())
                {
                   
                    for (int i = 0; i < facilityDTOList.Count; i++)
                    {
                        selectedSeats selectedSeats = new selectedSeats();
                        selectedSeats.facilityId = facilityDTOList[i].FacilityId;
                        seletedSeatsFacilityWise.Add(selectedSeats);
                    }
                    facilityId = facilityDTOList[0].FacilityId;
                    SetScreenPoistion(facilityDTOList[0]);
                    SetTotalFACSeats(facilityDTOList[0]);
                }
            }
            //if (o != null)
            //{
            //    string position = o.ToString();
            //    screenPostion(position);
            //}
            RefreshLayout();
            
            userAction = true;
            //lblTotalSeats.Text = Utilities.executeScalar("select Capacity from CheckInFacility where FacilityId = (select FacilityId from AttractionSchedules where AttractionScheduleId = @AttractionScheduleId)",
            //                                                           new SqlParameter[] { new SqlParameter("@AttractionScheduleId", AttractionScheduleId) }).ToString();
            //lblTotalSeats.Text = totalFACSeats.ToString();

            //lblBookedSeats.Text = Utilities.executeScalar(@"select count(1)
            //                                                  from FacilitySeats fs,
            //                                                        AttractionBookingSeats abs, AttractionBookings atb, AttractionSchedules ats
            //                                                  where atb.BookingId = abs.BookingId
            //                                                    and atb.ScheduleTime = @Time
            //                                                    and ats.AttractionScheduleId = atb.AttractionScheduleId
            //                                                    and ats.AttractionScheduleId = @AttractionScheduleId
            //                                                    and abs.SeatId = fs.SeatId
            //                                                    and fs.FacilityId = (select FacilityId from AttractionSchedules where AttractionScheduleId = @AttractionScheduleId)",
            //                                                       new SqlParameter[] { new SqlParameter("@AttractionScheduleId", AttractionScheduleId), new SqlParameter("@Time", ScheduleTime) }).ToString();
            Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void MarkSelectedSeats()
        {
            log.LogMethodEntry();
            //userAction = false;
            //dgvLayout.ClearSelection();
            //userAction = true;
            if (paramSelectedSeats != null && paramSelectedSeats.Any())
            {
                foreach (DataGridViewRow dr in dgvLayout.Rows)
                {
                    foreach (DataGridViewCell dc in dr.Cells)
                    {
                        if (dc.Tag != null)
                        {
                            if (paramSelectedSeats.Contains(Convert.ToInt32(dc.Tag)))
                            {
                                dc.Selected = true;
                            }

                            if (inMemorySeats.Contains(Convert.ToInt32(dc.Tag)))
                            {
                                dc.Style.Format = "R";
                                dc.Selected = true;
                            }
                        }
                    }
                }
            }
            if (seletedSeatsFacilityWise != null && seletedSeatsFacilityWise.Any())
            {
                for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
                {
                    //KeyValuePair<int, List<object>> keyValuePair = facilitywiseSelectedSeats[i];
                    if (seletedSeatsFacilityWise[i].facilityId == this.facilityId && seletedSeatsFacilityWise[i].SelectedSeats != null && seletedSeatsFacilityWise[i].SelectedSeats.Any())
                    {
                        foreach (DataGridViewRow dr in dgvLayout.Rows)
                        {
                            foreach (DataGridViewCell dc in dr.Cells)
                            {
                                if (dc.Tag != null)
                                {
                                    if (seletedSeatsFacilityWise[i].SelectedSeats.Contains(Convert.ToInt32(dc.Tag)))
                                    {
                                        dc.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                } 
                
            }
            lblSeatsSelected.Focus(); 
            UpdateLayOutWithSeatSelectionColorCode();
            log.LogMethodExit();
        }

        private void SetScreenPoistion(FacilityDTO facilityDTO)
        {
            log.LogMethodEntry(facilityDTO); 
            string position = (string.IsNullOrEmpty(facilityDTO.ScreenPosition) == true ? "Bottom" : facilityDTO.ScreenPosition);
            screenPostion(position); 
            log.LogMethodExit(); 
        }

        private void SetTotalFACSeats(FacilityDTO facilityDTO)
        {
            log.LogMethodEntry(facilityDTO);
            int? totalFACSeats; 
            totalFACSeats = (facilityDTO.Capacity == null ? 0 : facilityDTO.Capacity);
            lblTotalSeats.Text = totalFACSeats.ToString();
            log.LogMethodExit(totalFACSeats); 
        }

        void screenPostion(string postion)
        {
            log.LogMethodEntry(postion);
            if (postion == "Bottom")
            {
                panelScreenPosition.Location = new Point(panelScreenPosition.Location.X, this.Height - panelScreenPosition.Height - 30);
                dgvLayout.Location = new Point(dgvLayout.Location.X, 10);
                lblScreenPosition.Location = new Point(0, 15);
                panelScreenPosition.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else
            {
                panelScreenPosition.Location = new Point(panelScreenPosition.Location.X, 0);
                dgvLayout.Location = new Point(dgvLayout.Location.X, panelScreenPosition.Height + 10);
                lblScreenPosition.Location = new Point(0, 0);
                panelScreenPosition.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
            log.LogMethodExit();
        }

        void RefreshLayout()
        {
            log.LogMethodEntry();
            userAction = false;
            dgvLayout.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvLayout.Rows.Clear();
            dgvLayout.Columns.Clear();
            dgvLayout.Refresh();
            Application.DoEvents();
            FacilitySeatsBL facilitySeatsBL = new FacilitySeatsBL(Utilities.ExecutionContext);
            List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList = facilitySeatsBL.GetFacilitySeatLayout(facilityId);
            //DataTable dt = Utilities.executeDataTable(@"select * 
            //                                                        from FacilitySeatLayout fsl
            //                                                        where FacilityId = @facilityId
            //                                                        and (Type in ('A', 'P')
	           //                                                         or exists (select 1
			         //                                                           from FacilitySeats fs
			         //                                                           where fs.RowIndex = fsl.RowColumnIndex
			         //                                                             and fsl.Type = 'R'
			         //                                                             and fsl.FacilityId = fs.FacilityId)
	           //                                                         or exists (select 1
			         //                                                           from FacilitySeats fs
			         //                                                           where fs.ColumnIndex = fsl.RowColumnIndex
			         //                                                             and fsl.Type = 'C'
			         //                                                             and fsl.FacilityId = fs.FacilityId))
            //                                                        order by RowColumnIndex, Type desc", new SqlParameter[] { new SqlParameter("@facilityId", facilityId) });
            dgvLayout.Columns.Add("Dummy", "Dummy");
            dgvLayout.Columns["Dummy"].Visible = false;
            dgvLayout.Columns["Dummy"].SortMode = DataGridViewColumnSortMode.NotSortable;
            if (facilitySeatLayoutDTOList != null && facilitySeatLayoutDTOList.Count > 0)
            {
                foreach (FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilitySeatLayoutDTOList)
                {
                    string type = facilitySeatLayoutDTO.Type.ToString();
                    if (type == "C" || type == "A")
                    {
                        if (type == "C")
                        {
                            DataGridViewImageColumn dv = new DataGridViewImageColumn();
                            dv.ReadOnly = true;
                            dv.SortMode = DataGridViewColumnSortMode.NotSortable;
                            dv.Name = facilitySeatLayoutDTO.RowColumnName;//dr["RowColumnName"].ToString();
                            dv.Image = seatAvailableImage;
                            dv.ImageLayout = DataGridViewImageCellLayout.Normal;
                            dv.Width = 30;
                            dgvLayout.Columns.Add(dv);
                        }
                        else
                        {
                            DataGridViewTextBoxColumn dv = new DataGridViewTextBoxColumn();
                            dv.ReadOnly = true;
                            dv.SortMode = DataGridViewColumnSortMode.NotSortable;
                            dv.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dv.Name = facilitySeatLayoutDTO.RowColumnName;//dr["RowColumnName"].ToString();
                            dv.Width = 50;
                            dgvLayout.Columns.Add(dv);
                        }
                    }
                }
                foreach (FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilitySeatLayoutDTOList)
                {
                    string type = facilitySeatLayoutDTO.Type.ToString();
                    if (type == "R" || type == "P")
                    {
                        dgvLayout.Rows.Add(facilitySeatLayoutDTO.RowColumnName);// dr["RowColumnName"].ToString());
                        dgvLayout.Rows[dgvLayout.Rows.Count - 1].Tag = facilitySeatLayoutDTO.LayoutId;// dr["LayoutId"];
                        dgvLayout.Rows[dgvLayout.Rows.Count - 1].HeaderCell.Value = facilitySeatLayoutDTO.RowColumnName; // dr["RowColumnName"].ToString();

                        if (type == "P")
                            foreach (DataGridViewCell dc in dgvLayout.Rows[dgvLayout.Rows.Count - 1].Cells)
                            {
                                if (dc.OwningColumn.HeaderText != "Aisle")
                                    dc.Value = seatBlankImage;
                            }
                    }
                }
            }
            List<FacilitySeatsDTO> facilitySeatsDTOList = facilitySeatsBL.GetFacilitySeats(facilityId, AttractionScheduleId, scheduleFromDate);
            //DataTable dtSeat = Utilities.executeDataTable(@"select fs.SeatName, fs.Active, fs.IsAccessible, fs.SeatId, abs.SeatId bookedSeat
            //                                                                    from FacilitySeats fs left outer join 
            //                                                                    (select SeatId
            //                                                                    from AttractionBookingSeats abs, AttractionBookings atb
            //                                                                      where atb.BookingId = abs.BookingId
            //                                                                  and atb.ScheduleTime = @Time
            //                                                                  and atb.AttractionScheduleId = @AttractionScheduleId) abs
            //                                                                  on abs.SeatId = fs.SeatId
            //                                                                    where fs.FacilityId = @facilityId",
            //                                                        new SqlParameter[] { new SqlParameter("@AttractionScheduleId", AttractionScheduleId),
            //                                                                             new SqlParameter("@Time", ScheduleTime),
            //                                                                              new SqlParameter("@facilityId", facilityId) });
            if (facilitySeatsDTOList != null && facilitySeatsDTOList.Count > 0)
            {
                foreach (DataGridViewRow dr in dgvLayout.Rows)
                {
                    string row = dr.HeaderCell.Value.ToString();
                    if (row == "Passage")
                        continue;
                    foreach (DataGridViewCell dc in dr.Cells)
                    {
                        string col = dc.OwningColumn.HeaderText;
                        if (col == "Aisle")
                            continue;

                        dc.ToolTipText = row + col;
                        FacilitySeatsDTO foundRow = null;
                        bool found = false;
                        foreach (FacilitySeatsDTO facilitySeatsDTO in facilitySeatsDTOList)
                        {
                            if (facilitySeatsDTO.SeatName == dc.ToolTipText)
                            {
                                found = true;
                                foundRow = facilitySeatsDTO;
                                break;
                            }
                        }

                        dc.Style.SelectionBackColor = Color.White;
                        if (!found)
                        {
                            dc.Tag = -1;
                            dc.Value = seatBlankImage;
                            dc.ToolTipText = "";
                        }
                        else if (foundRow.Active.ToString() == "D")
                        {
                            dc.Tag = -1;
                            dc.Value = seatBlankImage;
                            dc.ToolTipText = "";
                        }
                        else if (foundRow.Active.ToString() == "N")
                        {
                            dc.Tag = -1;
                            dc.Value = seatDisabledImage;
                            dc.ToolTipText = "Not Available";
                        }
                        else if (foundRow.BookedSeat > -1)
                        {
                            dc.Tag = -1;
                            dc.Value = seatRedImage;
                        }
                        else if (foundRow.IsAccessible.ToString() == "Y")
                        {
                            dc.Tag = foundRow.SeatId;
                            dc.Style.Tag = dc.Value = seatAccessibilityImage;
                        }
                        else
                        {
                            dc.Tag = foundRow.SeatId;
                            dc.Style.Tag = dc.Value = seatAvailableImage;
                        }
                    }
                }
            }
            dgvLayout.Refresh();
            dgvLayout.ClearSelection();
            MarkSelectedSeats();
            userAction = true;
            log.LogMethodExit();
        }

        private void dgvLayout_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (userAction)
            {
                UpdateLayOutWithSeatSelectionColorCode();
            }
            log.LogMethodExit();
        }

        private void UpdateLayOutWithSeatSelectionColorCode()
        {
            log.LogMethodEntry();
            lblSeatsSelected.Text = "";
            int selectedFacilityIndex = seletedSeatsFacilityWise.IndexOf(seletedSeatsFacilityWise.Find(ssf => ssf.facilityId == this.facilityId));
            seletedSeatsFacilityWise[selectedFacilityIndex].SelectedSeats.Clear();
            seletedSeatsFacilityWise[selectedFacilityIndex].SelectedSeatNames.Clear(); 

            foreach (DataGridViewCell dc in dgvLayout.SelectedCells)
            {
                int selectedSeatsFromOtherFacilities = GetSelectedSeatCountFromOtherFacilities();

                if (dgvLayout.SelectedCells.Count + selectedSeatsFromOtherFacilities - inMemorySeats.Count > SeatCount)
                {
                    dc.Selected = false;
                    log.Info("dgvLayout.SelectedCells.Count > SeatCount");
                    log.LogMethodExit();
                    return;
                }
                else
                    break;
            }

            foreach (DataGridViewCell dc in dgvLayout.SelectedCells)
            {
                if (dc.Tag != null && dc.Tag.ToString() != "-1")
                {
                    if (dc.Style.Format.Equals("R"))
                    {
                        dc.Value = Properties.Resources.RedSeat;
                    }
                    else
                    {
                        dc.Style.Format = "G";
                        dc.Value = Properties.Resources.GreenSeat;
                        try
                        {
                            seletedSeatsFacilityWise[selectedFacilityIndex].SelectedSeats.Add(Convert.ToInt32(dc.Tag));
                        }
                        catch (Exception ex) { log.Error(ex); };
                        seletedSeatsFacilityWise[selectedFacilityIndex].SelectedSeatNames.Add(dc.ToolTipText);
                    }
                }
            }

            seletedSeatsFacilityWise[selectedFacilityIndex].SelectedSeats.Sort(delegate (int a, int b)
            {
                if (a == b)
                    return 0;
                else if (a > b)
                    return 1;
                else
                    return -1;
            });

            seletedSeatsFacilityWise[selectedFacilityIndex].SelectedSeatNames.Sort(delegate (string a, string b)
            {
                if (a == null)
                    if (b == null)
                        return 0;
                    else
                        return -1;
                else
                    if (b == null)
                    return 1;
                else
                    return a.CompareTo(b);
            });

            foreach (DataGridViewRow dr in dgvLayout.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    if (dc.Tag != null && dc.Tag.ToString() != "-1" && dc.Selected == false)
                    {
                        if (dc.Style.Format == "G")
                        {
                            dc.Value = dc.Style.Tag;
                            dc.Style.Format = "";
                        }
                    }
                }
            }

            SetlblSeatsSelected();
            //foreach (string seat in SelectedSeatNames)
            //    lblSeatsSelected.Text += seat + ", ";
            //lblSeatsSelected.Text = lblSeatsSelected.Text.TrimEnd(' ').TrimEnd(',');
            //List<object> selectedSeatsFinal = new List<object>();
            //List<string> selectedSeatNamesFinal = new List<string>();
            //if (SelectedSeats != null && SelectedSeats.Any())
            //{
            //    for (int i = 0; i < SelectedSeats.Count; i++)
            //    {
            //        selectedSeatsFinal.Add(SelectedSeats[i]);
            //    }

            //    for (int i = 0; i < SelectedSeatNames.Count; i++)
            //    {
            //        selectedSeatNamesFinal.Add(SelectedSeatNames[i]);
            //    }

            //}

            //if (facilitywiseSelectedSeats == null || facilitywiseSelectedSeats.Count == 0)
            //{
            //    facilitywiseSelectedSeats.Add(new KeyValuePair<int, List<object>>(this.facilityId, selectedSeatsFinal));
            //    facilitywiseSelectedSeatNames.Add(new KeyValuePair<int, List<string>>(this.facilityId, selectedSeatNamesFinal));
            //}
            //else
            //{
            //    KeyValuePair<int, List<object>> currentFacilitySeatEntry = facilitywiseSelectedSeats.Find(seatList => seatList.Key == this.facilityId);
            //    KeyValuePair<int, List<string>> currentFacilitySeatNameEntry = facilitywiseSelectedSeatNames.Find(seatList => seatList.Key == this.facilityId);

            //    if (currentFacilitySeatEntry.Value != null)
            //    {
            //        facilitywiseSelectedSeats.Remove(currentFacilitySeatEntry);
            //        facilitywiseSelectedSeats.Add(new KeyValuePair<int, List<object>>(this.facilityId, selectedSeatsFinal));
            //        facilitywiseSelectedSeatNames.Remove(currentFacilitySeatNameEntry);
            //        facilitywiseSelectedSeatNames.Add(new KeyValuePair<int, List<string>>(this.facilityId, selectedSeatNamesFinal));
            //    }
            //    else
            //    {
            //        facilitywiseSelectedSeats.Add(new KeyValuePair<int, List<object>>(this.facilityId, selectedSeatsFinal));
            //        facilitywiseSelectedSeatNames.Add(new KeyValuePair<int, List<string>>(this.facilityId, selectedSeatNamesFinal));
            //    }
            //}
            log.LogMethodExit();
        }

        private void SetlblSeatsSelected()
        {
            log.LogMethodEntry();
            lblSeatsSelected.Text = "";
            string selectedSeats = string.Empty;
            for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
            {
                if(seletedSeatsFacilityWise[i].SelectedSeatNames != null && seletedSeatsFacilityWise[i].SelectedSeatNames.Any())
                {
                    string selectedSeatName = string.Empty;
                    for (int j = 0; j < seletedSeatsFacilityWise[i].SelectedSeatNames.Count; j++)
                    {
                        selectedSeatName = selectedSeatName + seletedSeatsFacilityWise[i].SelectedSeatNames[j]+", ";
                    }
                    if (string.IsNullOrEmpty(selectedSeats) == false)
                    {
                        selectedSeats += "   ["+ facilityDTOList.Find(f => f.FacilityId == seletedSeatsFacilityWise[i].facilityId).FacilityName +"] "+ selectedSeatName;
                    }
                    else
                    {
                        selectedSeats += "[" + facilityDTOList.Find(f => f.FacilityId == seletedSeatsFacilityWise[i].facilityId).FacilityName + "] " + selectedSeatName;
                    }
                }
            }
            lblSeatsSelected.Text = selectedSeats;
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int seatsSelectedInAllfacilities = GetSelectedSeatCountFromAllFacilities();
            if (seatsSelectedInAllfacilities != SeatCount)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(4, SeatCount), "Seat Count Error");
                log.Info("SelectedSeats.Count != SeatCount");
                log.LogMethodExit();
                return;
            }             
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        //private void UpdateSeatDetails()
        //{
        //    log.LogMethodEntry();
        //    if (seletedSeatsFacilityWise != null && seletedSeatsFacilityWise.Any())
        //    {
        //        for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
        //        {
        //            KeyValuePair<int, List<object>> keyValuePair = facilitywiseSelectedSeats[i];
        //            if (keyValuePair.Key != facilityId)
        //            {
        //                foreach (var item in keyValuePair.Value)
        //                {
        //                    SelectedSeats.Add(Convert.ToInt32(item));
        //                }
        //            }
        //        }

        //        for (int i = 0; i < facilitywiseSelectedSeatNames.Count; i++)
        //        {
        //            KeyValuePair<int, List<string>> keyValuePair = facilitywiseSelectedSeatNames[i];
        //            if (keyValuePair.Key != facilityId)
        //            {
        //                foreach (string item in keyValuePair.Value)
        //                {
        //                    SelectedSeatNames.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e); 
            dgvLayout.ClearSelection();
            log.LogMethodExit();
        }

        private void cmbFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (userAction)
            { 
                int selectedFacilityId = Convert.ToInt32((cmbFacility.SelectedValue == null ? -1 : cmbFacility.SelectedValue));
                if (selectedFacilityId != this.facilityId)
                {
                    this.facilityId = selectedFacilityId;
                    FacilityDTO facilityDTO = facilityDTOList.Find(fac => fac.FacilityId == selectedFacilityId);
                    SetTotalFACSeats(facilityDTO);
                    SetScreenPoistion(facilityDTO);
                    RefreshLayout(); 
                } 
            }
            log.LogMethodExit();
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ClearAllSelectedSeats();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void ClearAllSelectedSeats()
        {
            log.LogMethodEntry();
            for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
            {
                seletedSeatsFacilityWise[i].SelectedSeats = null;
                seletedSeatsFacilityWise[i].SelectedSeatNames = null;
            }
            log.LogMethodExit();
        }

        private int GetSelectedSeatCountFromOtherFacilities()
        {
            log.LogMethodEntry(this.facilityId);
            int seatCount = 0;
            if(seletedSeatsFacilityWise != null && seletedSeatsFacilityWise.Any())
            {
                for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
                {
                    //KeyValuePair<int, List<object>> keyValuePair = facilitywiseSelectedSeats[i];
                    //if (keyValuePair.Key != facilityId)
                    //{
                    //    seatCount = seatCount + keyValuePair.Value.Count;
                    //}
                    if (seletedSeatsFacilityWise[i].facilityId != facilityId  
                        && seletedSeatsFacilityWise[i].SelectedSeats != null && seletedSeatsFacilityWise[i].SelectedSeats.Any())
                    {
                        seatCount += seletedSeatsFacilityWise[i].SelectedSeats.Count;
                    }
                }
            }
            log.LogMethodExit(seatCount);
            return seatCount;
        }

        private int GetSelectedSeatCountFromAllFacilities()
        {
            log.LogMethodEntry(this.facilityId);
            int seatCount = 0;
            if (seletedSeatsFacilityWise != null && seletedSeatsFacilityWise.Any())
            {
                for (int i = 0; i < seletedSeatsFacilityWise.Count; i++)
                { 
                    if (seletedSeatsFacilityWise[i].SelectedSeats != null && seletedSeatsFacilityWise[i].SelectedSeats.Any())
                    {
                        seatCount += seletedSeatsFacilityWise[i].SelectedSeats.Count;
                    }
                }
            }
            log.LogMethodExit(seatCount);
            return seatCount;
        }
    }
}
