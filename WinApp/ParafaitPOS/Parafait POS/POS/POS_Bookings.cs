/******************************************************************************************************
 * Project Name - POS
 * Description  - POS Booking
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ****************************************************************************************************** 
*2.70        26-Mar-2019      Guru S A       Booking phase 2 enhancement changes 
*2.70.0      27-Jul-2019      Nitin Pai      Attraction Enhancement
*2.70.2      04-Feb-2019      Nitin Pai      Reschedule Attraction changes
*2.90        19-Aug-2019      Nitin Pai      Fix: Reschedule Attraction of a booking transaction should not pop up cards selection screen
*2.110       05-Feb-2021      Nitin Pai      Do not print transaction at end of reschedule. Date calculation adjustments to use beginning of day.
*2.110       24-Jan-2022      Girish Kundar  Modified : Do not consider the dates by default while searching attranction booking details
*2.140.0     27-Jun-2021      Fiona Lishal   Modified for Delivery Order enhancements for F&B
********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Product;
using Semnox.Parafait.Customer;
using System.Globalization;
using Semnox.Parafait.Languages;
using System.Windows.Controls;

namespace Parafait_POS
{
    public partial class POS
    {
        internal DataTable dtAttractionBookings;
        internal DataTable dtAttractionBookingsLoaded;
        private int dgvBookingsPageNo = 2;
        private int dgvBookingsPageSize = 100;
        void getAttractionFacilities()
        {
            log.LogMethodEntry();
            string productTypeList = ProductTypeValues.ATTRACTION;// + "," + ProductTypeValues.BOOKINGS;
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(Utilities.ExecutionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, productTypeList));
            searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParameters);
            if (facilityMapDTOList == null)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            else
            {
                facilityMapDTOList = facilityMapDTOList.OrderBy(fac => fac.FacilityMapName).ToList();
            }
            FacilityMapDTO facilityMapDTO = new FacilityMapDTO();
            facilityMapDTO.FacilityMapName = "- All -";
            facilityMapDTOList.Insert(0, facilityMapDTO);
            //cmbAttractionFacility.DataSource = Utilities.executeDataTable("select facilityId, FacilityName from CheckInFacility where active_Flag = 'Y' union all select -1, '- All -' order by 2");
            cmbAttractionFacility.DataSource = facilityMapDTOList;
            cmbAttractionFacility.DisplayMember = "FacilityMapName";
            cmbAttractionFacility.ValueMember = "FacilityMapId";
            cmbAttractionFacility.SelectedIndex = facilityMapDTOList.Count > 1 ? 1 : 0;
            this.cmbAttractionFacility.SelectedIndexChanged += new System.EventHandler(this.cmbAttractionFacility_SelectedIndexChanged);
            log.LogMethodExit();
        }

        System.Windows.Forms.ContextMenuStrip ctxBookingContextMenu;
        System.Windows.Forms.ContextMenuStrip ctxAttractionScheduleContextMenu;
        void displayBookings()
        {
            log.LogMethodEntry();
            if (tcAttractionBookings.SelectedTab == tpAttractionBooking)
            {
                displayAttractions();
            }
            else if (tcAttractionBookings.SelectedTab == tpAttractionScedules)
            {
                RefreshScheduleGrid();
            }
            else if (tcAttractionBookings.SelectedTab == tpReservations)
            {
                displayReservations();
            }
            log.LogMethodExit();
        }

        private DataTable LoadNextPageForDGVBookings()
        {
            log.LogMethodEntry();
            if (dtAttractionBookingsLoaded.Rows.Count < dtAttractionBookings.Rows.Count)
            {
                IEnumerable<DataRow> nextSetOfRows = dtAttractionBookings.AsEnumerable().Skip((dgvBookingsPageNo - 1) * dgvBookingsPageSize).Take(dgvBookingsPageSize);
                if (nextSetOfRows.Any())
                {
                    log.LogVariableState("Do  dtAttractionBookingsLoaded.Merge", nextSetOfRows.Count());
                    dtAttractionBookingsLoaded.Merge(nextSetOfRows.CopyToDataTable());
                    IncrementDGVBookingsPageNo();
                }
            }
            log.LogMethodExit(dtAttractionBookingsLoaded);
            return dtAttractionBookingsLoaded;
        }

        private void IncrementDGVBookingsPageNo()
        {
            log.LogMethodEntry(dgvBookingsPageNo);
            dgvBookingsPageNo++;
            log.LogMethodExit(dgvBookingsPageNo);
        }

        private int GetDGVBookingsDisplayedRowsCount(DataGridView dvgObject)
        {
            log.LogMethodEntry(dvgObject);
            int displayedRowsCount = dvgObject.Rows[dvgObject.FirstDisplayedScrollingRowIndex].Height;
            displayedRowsCount = dvgObject.Height / displayedRowsCount;
            log.LogMethodExit(displayedRowsCount);
            return displayedRowsCount;
        }
        private void dataGridViewBookings_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int display = dgvBookings.Rows.Count - dgvBookings.DisplayedRowCount(false);
            int dgvBookingsRowCount = dgvBookings.Rows.Count;
            if (e.ScrollOrientation.Equals(System.Windows.Forms.ScrollOrientation.VerticalScroll) &&
                (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.LargeIncrement))
            {
                if (e.NewValue >= dgvBookings.Rows.Count - GetDGVBookingsDisplayedRowsCount(dgvBookings))
                {
                    log.LogVariableState("e.NewValue", e.NewValue);
                    dgvBookings.DataSource = LoadNextPageForDGVBookings();
                    //POSUtils.ResetDGVPurchaseRowPrinterImage(dataGridViewPurchases, dgvPurchasesRowCount);
                    dgvBookings.ClearSelection();
                    dgvBookings.FirstDisplayedScrollingRowIndex = display;
                    lastTrxActivityTime = DateTime.Now;
                }
            }
            log.LogMethodExit();
        }

        private DataTable FullLoadForDGVBookings()
        {
            log.LogMethodEntry();
            if (dtAttractionBookingsLoaded.Rows.Count < dtAttractionBookings.Rows.Count)
            {
                dtAttractionBookingsLoaded = dtAttractionBookings;
                dgvBookingsPageNo = Convert.ToInt32(dtAttractionBookings.Rows.Count / dgvBookingsPageSize);
            }
            log.LogMethodExit(dtAttractionBookingsLoaded);
            return dtAttractionBookingsLoaded;
        }

        private void dgvBookingsColumnHeaderClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            dgvBookings.DataSource = FullLoadForDGVBookings();
            ListSortDirection direction = ListSortDirection.Ascending;
            if (dgvBookings.SortOrder == System.Windows.Forms.SortOrder.Ascending)
            {
                direction = ListSortDirection.Ascending;
            }
            else if (dgvBookings.SortOrder == System.Windows.Forms.SortOrder.Descending)
            {
                direction = ListSortDirection.Descending;
            }
            dgvBookings.Sort(dgvBookings.Columns[e.ColumnIndex], direction);
            log.LogMethodExit();
        }

        private List<ScheduleDetailsDTO> GetScheduleData()
        {
            log.LogMethodEntry();
            int facilityMapId = -1;
            int productId = -1;
            if (cmbAttractionFacility.SelectedValue != null)
            {
                facilityMapId = Convert.ToInt32(cmbAttractionFacility.SelectedValue);
            }

            DateTime scheduleDateTime = dtpAttractionDate.Value == null ? Utilities.getServerTime() : dtpAttractionDate.Value.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 0));
            if (scheduleDateTime.Hour < ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 0))
            {
                scheduleDateTime = scheduleDateTime.AddDays(-1).AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 0));
            }

            List<ScheduleDetailsDTO> scheduleDetailsDTOList = GetEligibleAttractionSchedules(scheduleDateTime, 0, 24, facilityMapId, productId);
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        private void dtpAttractionDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dtpAttractionDate.Value.Date < Utilities.getServerTime().Date)
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1070), "Reschedule Date", MessageBoxButtons.OK);
                dtpAttractionDate.Value = Utilities.getServerTime().Date;
                return;
            }

            RefreshScheduleGrid();
            log.LogMethodExit();
        }

        private Card attractionSearchParamInputCard = null;
        private CustomerDTO attractionSearchParamCustomerDTO = null;
        private FacilityMapDTO attractionSearchParamFacilityMapDTO = null;
        private int attractionSearchParamTxId = -1;
        private int attractionSearchParamAttractionScheduleId = -1;
        private string attractionSearchParamFromDate = "";
        private string attractionSearchParamToDate = "";
        void displayAttractions(Card inputCard = null, CustomerDTO inputCustomerDTO = null, int intputTrxId = -1, FacilityMapDTO inputFacilityMapDTO = null, String fromDate = "", String toDate = "",
            int attractionScheduleId = -1, bool isDateTimeValueChanged = true)
        {
            log.LogMethodEntry(inputCard, inputCustomerDTO, intputTrxId, inputFacilityMapDTO, fromDate, toDate,
           attractionScheduleId, isDateTimeValueChanged);

            attractionSearchParamInputCard = inputCard;
            attractionSearchParamCustomerDTO = inputCustomerDTO;
            attractionSearchParamFacilityMapDTO = inputFacilityMapDTO;
            attractionSearchParamTxId = intputTrxId;
            attractionSearchParamAttractionScheduleId = attractionScheduleId;
            attractionSearchParamFromDate = fromDate;
            attractionSearchParamToDate = toDate;
            // When date is not selected from the UI. 
            string dateSearchQuery = string.Empty;// 
            if (isDateTimeValueChanged)
            {
                dateSearchQuery = @"  and da.ScheduleDateTime >= @FromDate and da.ScheduleDateTime <= @ToDate ";
            }
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string query = @"select th.TrxId, th.trx_no, c.card_number card, CASE ISNULL(CustomerName, '')
                                                            when '' then 
		                                                        case ISNULL(cu.customer_name, '')
		                                                        when '' then cu1.customer_name +  isnull(' ' + cu1.last_name, '')
		                                                        else cu.customer_name +  isnull(' ' + cu.last_name, '')
		                                                        END 
                                                            Else  CustomerName
                                                            END Customer, 
                                                            fm.FacilityMapName as FacilityMap, atp.playname Play, da.ScheduleDateTime Time, BookedUnits Units, tl.Remarks, tl.product_id, tl.Card_Id CardId,
                                                            tl.lineId, atb.BookingId 
                                                        from AttractionBookings atb, DayAttractionSchedule da, AttractionPlays atp, FacilityMap fm,
	                                                         trx_header th
                                                             left outer join CustomerView(@PassPhrase) cu1 
	                                                         on cu1.customer_id = th.customerid,
	                                                         trx_lines tl
	                                                         left outer join cards c 
	                                                         on c.card_id = tl.card_id
                                                             left outer join (select TrxId, LineId, p.FirstName + ' '+ isnull(p.LastName,'') CustomerName
							                                            from customers c,
									                                            Profile p,
									                                            CustomerSignedWaiver csw,
									                                            WaiversSigned ws
							                                            where ws.IsActive = 1
								                                            and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
								                                            and csw.SignedFor = c.customer_id
								                                            and p.id = c.profileId) AS WC 
                                                        on WC.TrxId = tl.trxId and WC.LineId = tl.LineId
	                                                         left outer join CustomerView(@PassPhrase) cu 
	                                                         on cu.customer_id = c.customer_id
                                                        where atb.DayAttractionScheduleId = da.DayAttractionScheduleId 
                                                        and da.FacilityMapId = fm.FacilityMapId
                                                        and da.AttractionPlayId = atp.AttractionPlayId
                                                        and th.TrxId = atb.TrxId
                                                        and tl.TrxId = th.TrxId
                                                        and tl.LineId = atb.LineId
                                                        and (atb.expiryDate is null or atb.ExpiryDate > getdate())
                                                        and BookedUnits > 0 
                                                        " + dateSearchQuery + @"
                                                        and (c.card_id = @CardId Or @CardId = -1)
                                                        and (cu.customer_id = @CustomerId Or @CustomerId = -1)
                                                        and (th.TrxId = @TrxId Or @TrxId = -1)
                                                        and (da.AttractionScheduleId = @attractionScheduleId Or @attractionScheduleId = -1)
                                                        and (da.FacilityMapId = @FacilityMapId Or @FacilityMapId = -1)
                                                        order by ScheduleTime, th.TrxId desc";
                //Begin: Booking related Modified the query to get the booking details//
                sqlParameters.Add(new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
                DateTime Now = Utilities.getServerTime();
                int StartHour = String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(this.machineUserContext, "BUSINESS_DAY_START_TIME")) ? 6 : Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(this.machineUserContext, "BUSINESS_DAY_START_TIME"));
                if (rdBookingPast.Checked)
                {
                    DateTime from = Now;
                    if (Now.Hour < StartHour)
                        from = from.AddDays(-1);
                    sqlParameters.Add(new SqlParameter("@FromDate", from.AddDays(-3).Date.AddHours(StartHour)));
                    sqlParameters.Add(new SqlParameter("@ToDate", from.Date.AddHours(StartHour)));
                }
                else if (rdBookingFuture3.Checked)
                {
                    sqlParameters.Add(new SqlParameter("@FromDate", Now));
                    sqlParameters.Add(new SqlParameter("@ToDate", Now.Date.AddDays(4).AddHours(StartHour)));
                }
                else if (rdBookingFutureAll.Checked)
                {
                    sqlParameters.Add(new SqlParameter("@FromDate", Now));
                    sqlParameters.Add(new SqlParameter("@ToDate", DateTime.MaxValue));
                }
                else if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(toDate))
                {
                    DateTime fromDateObj = Convert.ToDateTime(fromDate);
                    sqlParameters.Add(new SqlParameter("@FromDate", fromDateObj.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                    DateTime toDateObj = Convert.ToDateTime(toDate);
                    sqlParameters.Add(new SqlParameter("@ToDate", toDateObj.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                }

                else if(isDateTimeValueChanged)
                {
                    sqlParameters.Add(new SqlParameter("@FromDate", (Now.Hour < StartHour ? Now.AddDays(-1).Date.AddHours(StartHour) : Now.Date.AddHours(StartHour))));
                    sqlParameters.Add(new SqlParameter("@ToDate", (Now.Hour > StartHour ? Now.AddDays(1).Date.AddHours(StartHour) : Now.Date.AddHours(StartHour))));
                }
                sqlParameters.Add(new SqlParameter("@CardId", inputCard == null ? -1 : inputCard.card_id));
                sqlParameters.Add(new SqlParameter("@CustomerId", inputCustomerDTO == null ? -1 : inputCustomerDTO.Id));
                sqlParameters.Add(new SqlParameter("@TrxId", intputTrxId));
                sqlParameters.Add(new SqlParameter("@attractionScheduleId", attractionScheduleId));
                sqlParameters.Add(new SqlParameter("@FacilityMapId", inputFacilityMapDTO == null ? -1 : inputFacilityMapDTO.FacilityMapId));

                DataTable dt = Utilities.executeDataTable(query, sqlParameters.ToArray());
                //End: Booking related //
                dtAttractionBookings = new DataTable();
                dtAttractionBookingsLoaded = new DataTable();
                dgvBookingsPageNo = 2;
                dtAttractionBookings = dt;
                IEnumerable<DataRow> firstSetOfRows = dt.AsEnumerable().Skip(0).Take(dgvBookingsPageSize);
                if (firstSetOfRows.Any())
                {
                    dtAttractionBookingsLoaded = firstSetOfRows.CopyToDataTable();
                }
                else
                {
                    dtAttractionBookingsLoaded = dtAttractionBookings;
                }
                dgvBookings.DataSource = dtAttractionBookingsLoaded;
                foreach (DataGridViewColumn dc in dgvBookings.Columns)
                {
                    if (dc.Index > dgvBookings.Columns["Remarks"].Index)
                        dc.Visible = false;
                }
                Utilities.setupDataGridProperties(ref dgvBookings);
                dgvBookings.Columns["Units"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                dgvBookings.Columns["Time"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                dgvBookings.Columns["Remarks"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                Utilities.setLanguage(dgvBookings);

                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.BackColor = Color.LightCyan;
                dgvBookings.RowTemplate.DefaultCellStyle = style;
                dgvBookings.RowsDefaultCellStyle.SelectionBackColor = Color.YellowGreen;
                dgvBookings.RowTemplate.Height = 32;

                dgvBookings.CellMouseClick -= dgvBookings_CellMouseClick;
                dgvBookings.CellMouseClick += dgvBookings_CellMouseClick;
                //dgvBookings.BorderStyle = BorderStyle.FixedSingle;
                dgvBookings.Columns["Time"].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;

                if (ctxBookingContextMenu == null)
                {
                    ctxBookingContextMenu = new System.Windows.Forms.ContextMenuStrip();

                    System.Windows.Forms.ToolStripMenuItem RescheduleBooking = new System.Windows.Forms.ToolStripMenuItem();

                    RescheduleBooking.Name = "Reschedule";
                    RescheduleBooking.Size = new System.Drawing.Size(125, 22);
                    RescheduleBooking.Text = "Reschedule";

                    System.Windows.Forms.ToolStripMenuItem RescheduleBookingGroup = new System.Windows.Forms.ToolStripMenuItem();

                    RescheduleBookingGroup.Name = "RescheduleGroup";
                    RescheduleBookingGroup.Size = new System.Drawing.Size(125, 22);
                    RescheduleBookingGroup.Text = "Reschedule Group";

                    System.Windows.Forms.ToolStripMenuItem ReduceBookingUnits = new System.Windows.Forms.ToolStripMenuItem();

                    ReduceBookingUnits.Name = "DecreaseUnits";
                    ReduceBookingUnits.Size = new System.Drawing.Size(125, 22);
                    ReduceBookingUnits.Text = "Decrease Units";

                    ctxBookingContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                     RescheduleBooking,
                                                     RescheduleBookingGroup,
                                                     ReduceBookingUnits
                });
                    ctxBookingContextMenu.Name = "ctxBookingContextMenu";
                    ctxBookingContextMenu.Size = new System.Drawing.Size(126, 70);
                    ctxBookingContextMenu.ItemClicked += ctxBookingContextMenu_ItemClicked;
                }

                //    SqlCommand cmd = Utilities.getCommand();
                //    //Begin: Booking related Modified the query to get the  attraction schedules//
                //    cmd.CommandText = @"SELECT scheduleName [Schedule Name],
                //                                       dateadd(MINUTE, convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) [Schedule Time],
                //                                       PlayName [Play Name],
                //                                       CASE
                //                                           WHEN isnull(RulePrice, 0) = 0 THEN CASE
                //                                                                                  WHEN isnull(p.Price, 0) = 0 THEN ap.price
                //                                                                                  ELSE p.price
                //                                                                              END
                //                                           ELSE RulePrice
                //                                       END Price,
                //                                       CASE
                //                                           WHEN isnull(RuleUnits, 0) = 0 THEN CASE
                //                                                                                  WHEN isnull(p.AvailableUnits, 0) = 0 THEN ats.Capacity
                //                                                                                  ELSE p.AvailableUnits
                //                                                                              END
                //                                           ELSE RuleUnits
                //                                       END [Total Units],
                //                                       BookedUnits [Booked Units],
                //                                       NULL [Avl. Units]
                //                                FROM products p,

                //                                  (SELECT p.product_Id,
                //                                          scheduleName,
                //                                          ScheduleTime,
                //                                          AttractionPlayId,
                //      cf.Capacity,
                //                                          ats.AttractionMasterScheduleId,
                //                                          ats.attractionScheduleId,
                //                                          cf.facilityId,

                //                                     (SELECT top 1 Price
                //                                      FROM AttractionScheduleRules,

                //                                        (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
                //                                      WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
                //                                             OR DATEPART(weekday, v.schDate) = DAY + 1
                //                                             OR DATEPART(DAY, v.schDate) = DAY - 1000)
                //                                        AND AttractionScheduleId = ats.AttractionScheduleId) RulePrice,

                //                                     (SELECT top 1 Units
                //                                      FROM AttractionScheduleRules,

                //                                        (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
                //                                      WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
                //                                             OR DATEPART(weekday, v.schDate) = Day + 1
                //                                             OR DATEPART(DAY, v.schDate) = Day - 1000)
                //                                        AND AttractionScheduleId = ats.AttractionScheduleId) RuleUnits
                //                                   FROM attractionschedules ats, products p, CheckInFacility cf
                //                                  where ats.AttractionMasterScheduleId=p.AttractionMasterScheduleId
                //                                    and ats.ActiveFlag='Y'
                //and ats.FacilityId = cf.FacilityId
                //and cf.active_flag = 'Y') ats
                //                                LEFT OUTER JOIN
                //                                  (SELECT p.product_Id,
                //                                          atb.attractionScheduleId,
                //                                          sum((CASE WHEN atb.ExpiryDate IS NULL THEN BookedUnits WHEN atb.ExpiryDate< getdate() THEN 0 ELSE BookedUnits END)) bookedUnits
                //                                   FROM attractionschedules ats,
                //                                        attractionBookings atb,
                //                                        products p
                //                                   WHERE p.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
                //                                     AND atb.attractionScheduleId = ats.attractionScheduleId
                //                                     AND atb.ScheduleTime >= @date
                //                                     AND atb.ScheduleTime < @date + 1
                //                                   GROUP BY atb.attractionScheduleId,
                //                                            p.product_Id) bookings ON bookings.attractionScheduleId = ats.attractionScheduleId,
                //                                                                       attractionPlays ap
                //                                WHERE p.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
                //                                  AND p.product_id = ats.Product_Id
                //                                  AND (ats.FacilityId = @facilityId
                //                                       OR @facilityId = -1)
                //                                  AND ap.AttractionPlayId = ats.AttractionPlayId
                //                                  AND (ap.ExpiryDate >= @date
                //                                       OR ap.ExpiryDate IS NULL)
                //                                ORDER BY ats.scheduleTime";
                //    //End: Booking related //
                //    cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
                //    cmd.Parameters.AddWithValue("@facilityId", cmbAttractionFacility.SelectedValue);
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    DataTable dts = new DataTable();
                //    da.Fill(dts);
                //if (ctxAttractionScheduleContextMenu == null)
                //{
                //    ctxAttractionScheduleContextMenu = new System.Windows.Forms.ContextMenuStrip();

                //    System.Windows.Forms.ToolStripMenuItem RescheduleAttractionSlot = new System.Windows.Forms.ToolStripMenuItem();

                //    RescheduleAttractionSlot.Name = "RescheduleSlot";
                //    RescheduleAttractionSlot.Size = new System.Drawing.Size(125, 22);
                //    RescheduleAttractionSlot.Text = "Reschedule Slot";

                //    ctxAttractionScheduleContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                //                                     RescheduleAttractionSlot
                //});
                //    ctxAttractionScheduleContextMenu.Name = "ctxAttractionScheduleContextMenu";
                //    ctxAttractionScheduleContextMenu.Size = new System.Drawing.Size(126, 22);
                //    ctxAttractionScheduleContextMenu.ItemClicked += ctxAttractionScheduleContextMenu_ItemClicked;
                //}

                //dtpAttractionDate.Value = Utilities.getServerTime().Date;
                //RefreshScheduleGrid();

            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void RefreshScheduleGrid()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_BOOKINGS_IN_POS"))
            {
                this.Cursor = Cursors.WaitCursor;
                log.Info("ENABLE_BOOKINGS_IN_POS is set as true");
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.BackColor = Color.LightCyan;
                List<ScheduleDetailsDTO> ScheduleDetailsDTOList = GetScheduleData();

                if (chkShowPast.Checked == false)
                {
                    if (ScheduleDetailsDTOList != null && ScheduleDetailsDTOList.Count > 0)
                    {
                        for (int i = 0; i < ScheduleDetailsDTOList.Count; i++)
                        {
                            if (ScheduleDetailsDTOList[i].ScheduleFromDate < Utilities.getServerTime().AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD))
                            {
                                ScheduleDetailsDTOList.RemoveAt(i);
                                i = -1;
                            }
                        }
                    }
                }

                if (ScheduleDetailsDTOList != null && ScheduleDetailsDTOList.Count > 0)
                {
                    for (int i = 0; i < ScheduleDetailsDTOList.Count; i++)
                    {
                        double Price = (ScheduleDetailsDTOList[i].Price == null ? 0 : (double)ScheduleDetailsDTOList[i].Price);
                        if (Price == 0)
                        {
                            Price = (ScheduleDetailsDTOList[i].AttractionPlayPrice == null ? 0 : (double)ScheduleDetailsDTOList[i].AttractionPlayPrice);
                        }

                        ScheduleDetailsDTOList[i].Price = Price;

                        int? totalUnits = 0;
                        totalUnits = ScheduleDetailsDTOList[i].TotalUnits;

                        int bookedUnits = 0;
                        //bookedUnits = (scheduleDetailsDTOList[i].BookedUnits == null ? 0: (int) scheduleDetailsDTOList[i].BookedUnits);
                        FacilityMapBL facilityMapBL = new FacilityMapBL(Utilities.ExecutionContext, ScheduleDetailsDTOList[i].FacilityMapDTO);
                        bookedUnits = facilityMapBL.GetTotalBookedUnitsForAttraction(ScheduleDetailsDTOList[i].ScheduleFromDate, ScheduleDetailsDTOList[i].ScheduleToDate);
                        if (bookedUnits > 0)
                        {
                            ScheduleDetailsDTOList[i].BookedUnits = bookedUnits;
                        }
                        if (totalUnits < 0)
                        {
                            ScheduleDetailsDTOList[i].AvailableUnits = 0;
                        }
                        else
                        {
                            ScheduleDetailsDTOList[i].AvailableUnits = totalUnits - bookedUnits;
                        }
                    }
                }

                dgvAttractionSchedules.MultiSelect = false;
                dgvAttractionSchedules.DataSource = ScheduleDetailsDTOList;
                Utilities.setupDataGridProperties(ref dgvAttractionSchedules);
                Utilities.setLanguage(dgvAttractionSchedules);
                dgvAttractionSchedules.RowTemplate.DefaultCellStyle = style;
                dgvAttractionSchedules.RowTemplate.Height = 32;
                dgvAttractionSchedules.RowsDefaultCellStyle.SelectionBackColor = Color.YellowGreen;
                //dgvAttractionSchedules.BorderStyle = BorderStyle.FixedSingle;
                dgvAttractionSchedules.Columns["FacilityMapId"].Visible = false;
                dgvAttractionSchedules.Columns["FacilityMapName"].Visible = false;
                dgvAttractionSchedules.Columns["masterScheduleId"].Visible = false;
                dgvAttractionSchedules.Columns["masterScheduleName"].Visible = false;
                dgvAttractionSchedules.Columns["scheduleId"].Visible = false;
                dgvAttractionSchedules.Columns["ScheduleToDate"].Visible = false;
                dgvAttractionSchedules.Columns["scheduleFromTime"].Visible = false;
                dgvAttractionSchedules.Columns["scheduleToTime"].Visible = false;
                dgvAttractionSchedules.Columns["fixedSchedule"].Visible = false;
                dgvAttractionSchedules.Columns["attractionPlayId"].Visible = false;
                dgvAttractionSchedules.Columns["productId"].Visible = false;
                dgvAttractionSchedules.Columns["productName"].Visible = false;
                dgvAttractionSchedules.Columns["facilityCapacity"].Visible = false;
                dgvAttractionSchedules.Columns["ruleUnits"].Visible = false;
                // dgvAttractionSchedules.Columns["productLevelUnits"].Visible = false;
                dgvAttractionSchedules.Columns["desiredUnits"].Visible = false;
                dgvAttractionSchedules.Columns["expiryDate"].Visible = false;
                dgvAttractionSchedules.Columns["categoryId"].Visible = false;
                dgvAttractionSchedules.Columns["promotionId"].Visible = false;
                dgvAttractionSchedules.Columns["seats"].Visible = false;
                dgvAttractionSchedules.Columns["attractionPlayPrice"].Visible = false;
                dgvAttractionSchedules.Columns["FacilityMapDTO"].Visible = false;
                dgvAttractionSchedules.Columns["DayAttractionScheduleId"].Visible = false;

                dgvAttractionSchedules.Columns["Price"].DefaultCellStyle.Alignment =
                dgvAttractionSchedules.Columns["TotalUnits"].DefaultCellStyle.Alignment =
                dgvAttractionSchedules.Columns["BookedUnits"].DefaultCellStyle.Alignment =
                dgvAttractionSchedules.Columns["AvailableUnits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvAttractionSchedules.Columns["Price"].AutoSizeMode =
               dgvAttractionSchedules.Columns["TotalUnits"].AutoSizeMode =
               dgvAttractionSchedules.Columns["BookedUnits"].AutoSizeMode =
               dgvAttractionSchedules.Columns["AvailableUnits"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgvAttractionSchedules.Columns["Price"].Width =
               dgvAttractionSchedules.Columns["TotalUnits"].Width =
               dgvAttractionSchedules.Columns["BookedUnits"].Width =
               dgvAttractionSchedules.Columns["AvailableUnits"].Width = 60;

                dgvAttractionSchedules.Columns["ScheduleName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvAttractionSchedules.CellMouseClick -= dgvAttractionSchedules_CellMouseClick;
                dgvAttractionSchedules.CellMouseClick += dgvAttractionSchedules_CellMouseClick;

                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        void displayReservations()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.RefreshReservationsList(dgvAllReservations);
                dgvAllReservations.RowTemplate.Height = 32;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Debug("Ends-displayReservations() due to exception" + ex.Message);
            }
            log.LogMethodExit();
        }

        void ctxBookingContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvBookings.SelectedRows.Count > 0)
            {
                if (e.ClickedItem.Name.Equals("Reschedule"))
                {
                    Card card = null;
                    if (dgvBookings.SelectedRows[0].Cells["CardId"].Value != DBNull.Value)
                        card = new Card((int)dgvBookings.SelectedRows[0].Cells["CardId"].Value, ParafaitEnv.LoginID, Utilities);

                    // build the transaction from db
                    if (dgvBookings.SelectedRows[0].Cells["trxId"].Value != DBNull.Value)
                    {
                        Transaction selectedTransaction = TransactionUtils.CreateTransactionFromDB((int)dgvBookings.SelectedRows[0].Cells["trxId"].Value, Utilities);
                        int lineId = (int)dgvBookings.SelectedRows[0].Cells["lineId"].Value;
                        Transaction.TransactionLine selectedLine = selectedTransaction.TrxLines.Where(x => x.DBLineId == lineId).ToList<Transaction.TransactionLine>()[0];

                        ReservationDTO reservationDTO = POSUtils.GetReservationDTO(selectedTransaction.Utilities.ExecutionContext, selectedTransaction.Trx_id);
                        bool isBooking = reservationDTO != null ? true : false;

                        List<AttractionBooking> existingBookings = new List<AttractionBooking>();
                        Dictionary<int, int> quantityMap = new Dictionary<int, int>();

                        quantityMap.Add(selectedLine.ProductID, Decimal.ToInt32(selectedLine.quantity));
                        if (isBooking)
                        {
                            selectedLine.LineAtb.AttractionBookingDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;
                        }
                        existingBookings.Add(selectedLine.LineAtb);

                        DateTime fromDateTime = selectedLine.LineAtb.AttractionBookingDTO.ScheduleFromDate;
                        DateTime? selectedToDateTime = null;

                        if (fromDateTime.Hour < ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"))
                        {
                            fromDateTime = fromDateTime.AddDays(-1).AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));
                        }

                        if (isBooking)
                        {
                            fromDateTime = reservationDTO.FromDate;
                            selectedToDateTime = reservationDTO.ToDate;
                        }

                        // do not sent existing booking details for single line change, to enable fast reschedule
                        String message = "";
                        List<AttractionBooking> attractionBooking = GetAttractionBookingSchedule(quantityMap, existingBookings, fromDateTime,
                                                                        -1, isBooking, selectedTransaction, ref message, selectedToDateTime: selectedToDateTime);

                        this.Cursor = Cursors.WaitCursor;
                        if (attractionBooking != null && attractionBooking.Any())
                        {
                            int bookingTrxId = -1;
                            using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    dBTransaction.BeginTransaction();

                                    List<AttractionBookingDTO> attractionBookingDTOList = new List<AttractionBookingDTO>();
                                    attractionBookingDTOList.Add(existingBookings[0].AttractionBookingDTO);

                                    List<AttractionBookingDTO> targetBookingDTOList = new List<AttractionBookingDTO>();
                                    attractionBooking[0].AttractionBookingDTO.TrxId = existingBookings[0].AttractionBookingDTO.TrxId;
                                    attractionBooking[0].AttractionBookingDTO.LineId = existingBookings[0].AttractionBookingDTO.LineId;
                                    targetBookingDTOList.Add(attractionBooking[0].AttractionBookingDTO);

                                    RescheduleAttractionBookingsBL rescheduleAttractionBookingBL = new RescheduleAttractionBookingsBL(selectedTransaction.Utilities.ExecutionContext,
                                        attractionBookingDTOList, targetBookingDTOList);
                                    rescheduleAttractionBookingBL.RescheduleAttractionBookings(dBTransaction.SQLTrx);

                                    dBTransaction.EndTransaction();
                                    //PrintSpecificTransaction(selectedTransaction.Trx_id, true);
                                }
                                catch (Exception ex)
                                //if (!atb.Save(card == null ? -1 : card.card_id, sqlTrx, ref message))
                                {
                                    message = ex.Message;
                                    log.Error(ex);
                                    dBTransaction.RollBack();
                                    displayMessageLine(message, ERROR);
                                    log.LogMethodExit("Ends-ctxBookingContextMenu_ItemClicked() as unable to Save AttractionBooking error:)" + message);
                                    return;
                                }
                            }

                            dgvBookings.SelectedRows[0].Cells["Time"].Value = existingBookings[0].AttractionBookingDTO.ScheduleFromDate;
                            this.Cursor = Cursors.Default;
                            displayAttractions(attractionSearchParamInputCard, attractionSearchParamCustomerDTO, attractionSearchParamTxId, attractionSearchParamFacilityMapDTO,
                                attractionSearchParamFromDate, attractionSearchParamToDate, attractionSearchParamAttractionScheduleId);
                        }
                    }
                }
                else if (e.ClickedItem.Name.Equals("RescheduleGroup"))
                {
                    Card card = null;
                    if (dgvBookings.SelectedRows[0].Cells["CardId"].Value != DBNull.Value)
                        card = new Card((int)dgvBookings.SelectedRows[0].Cells["CardId"].Value, ParafaitEnv.LoginID, Utilities);

                    // build the transaction from db
                    if (dgvBookings.SelectedRows[0].Cells["trxId"].Value != DBNull.Value)
                    {
                        Transaction selectedTransaction = TransactionUtils.CreateTransactionFromDB((int)dgvBookings.SelectedRows[0].Cells["trxId"].Value, Utilities);
                        int lineId = (int)dgvBookings.SelectedRows[0].Cells["lineId"].Value;
                        Transaction.TransactionLine selectedLine = selectedTransaction.TrxLines.Where(x => x.DBLineId == lineId).ToList<Transaction.TransactionLine>()[0];
                        ReservationDTO reservationDTO = POSUtils.GetReservationDTO(selectedTransaction.Utilities.ExecutionContext, selectedTransaction.Trx_id);
                        bool isBooking = reservationDTO != null ? true : false;
                        DateTime fromDateTime = selectedLine.LineAtb.AttractionBookingDTO.ScheduleFromDate;
                        DateTime? selectedToDateTime = null;
                        if (isBooking)
                        {
                            fromDateTime = reservationDTO.FromDate;
                            selectedToDateTime = reservationDTO.ToDate;
                        }

                        List<AttractionBooking> existingBookings = new List<AttractionBooking>();
                        Dictionary<int, int> quantityMap = new Dictionary<int, int>();
                        List<Products> productsList = new List<Products>();
                        Products parentProduct = null;
                        int parentProductId = -1;
                        // If this line has a parent line, then it is a combo product
                        if (selectedLine.ParentLine != null)
                        {
                            parentProduct = new Products(selectedLine.ParentLine.ProductID);
                            parentProductId = selectedLine.ParentLine.ProductID;
                            foreach (Transaction.TransactionLine trxLine in selectedTransaction.TrxLines)
                            {
                                if (trxLine.ParentLine != null && trxLine.ParentLine.ProductID == selectedLine.ParentLine.ProductID
                                    && trxLine.ProductID != selectedLine.ParentLine.ProductID)
                                {
                                    if (quantityMap.ContainsKey(trxLine.ProductID))
                                    {
                                        quantityMap[trxLine.ProductID] = quantityMap[trxLine.ProductID] + Decimal.ToInt32(trxLine.quantity);
                                    }
                                    else
                                    {
                                        quantityMap.Add(trxLine.ProductID, Decimal.ToInt32(trxLine.quantity));
                                        productsList.Add(new Products(trxLine.ProductID));
                                    }

                                    //List<AttractionBookingDTO> attractionBookingDTOListTrxline = attractionBookingDTOList.Where(x => x.LineId == trxLine.DBLineId).ToList();
                                    //if (attractionBookingDTOListTrxline != null && attractionBookingDTOListTrxline.Count > 0)
                                    if (trxLine.LineAtb != null)
                                    {
                                        if (isBooking)
                                        {
                                            trxLine.LineAtb.AttractionBookingDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;
                                        }
                                        existingBookings.Add(trxLine.LineAtb);
                                    }

                                }
                            }

                        }
                        else
                        {// this is a non combo attraction product, traverse through the trx grid and get all the matching trxlines with this product if
                            foreach (Transaction.TransactionLine trxLine in selectedTransaction.TrxLines)
                            {
                                if (trxLine.ProductID == selectedLine.ProductID)
                                {
                                    if (quantityMap.ContainsKey(trxLine.ProductID))
                                    {
                                        quantityMap[trxLine.ProductID] = quantityMap[trxLine.ProductID] + Decimal.ToInt32(trxLine.quantity);
                                    }
                                    else
                                    {
                                        quantityMap.Add(trxLine.ProductID, Decimal.ToInt32(trxLine.quantity));
                                        productsList.Add(new Products(trxLine.ProductID));
                                    }

                                    if (trxLine.LineAtb != null)
                                    {
                                        if (isBooking)
                                        {
                                            trxLine.LineAtb.AttractionBookingDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;
                                        }
                                        existingBookings.Add(trxLine.LineAtb);
                                    }

                                }
                            }

                        }

                        String message = "";
                        List<AttractionBooking> attractionBooking = GetAttractionBookingSchedule(quantityMap, existingBookings, fromDateTime,
                                                                        -1, isBooking, selectedTransaction, ref message, selectedToDateTime: selectedToDateTime);

                        this.NewTrx = selectedTransaction;
                        if (attractionBooking != null && attractionBooking.Any())
                        {
                            foreach (KeyValuePair<int, int> productMap in quantityMap)
                            {
                                Products tempProd = productsList.FirstOrDefault(x => x.GetProductsDTO.ProductId == productMap.Key);
                                bool loadToSingleCard = false;
                                //success = GetAttractionBookingSchedule(lclChildProduct, seats, out atbList, ref message);

                                if ((parentProduct != null && parentProduct.GetProductsDTO.LoadToSingleCard) || tempProd.GetProductsDTO.LoadToSingleCard)
                                {
                                    loadToSingleCard = true;
                                }

                                if (tempProd.GetProductsDTO.CardSale.Equals("Y") && !tempProd.GetProductsDTO.AutoGenerateCardNumber.Equals("Y") && !isBooking)
                                {
                                    List<AttractionBooking> atbList = attractionBooking.Where(x => x.AttractionBookingDTO.AttractionProductId == tempProd.GetProductsDTO.ProductId).ToList();
                                    int seats = productMap.Value;
                                    if (atbList.Any())
                                    {
                                        List<Card> cardList;
                                        if (getAttractionCards(tempProd.GetProductsDTO.ProductId, tempProd.GetProductsDTO.ProductName, tempProd.GetProductsDTO.CardSale.Equals("Y"), tempProd.GetProductsDTO.AutoGenerateCardNumber.Equals("Y"), seats, loadToSingleCard, atbList, out cardList, parentProductId) == false)
                                        {
                                            return;
                                        }
                                    }
                                }
                            }

                            this.Cursor = Cursors.WaitCursor;
                            int bookingTrxId = -1;
                            using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    dBTransaction.BeginTransaction();

                                    List<AttractionBookingDTO> finalTargetATBDTOList = new List<AttractionBookingDTO>();
                                    List<AttractionBookingDTO> finalSourceATBDTOList = new List<AttractionBookingDTO>();

                                    foreach (Transaction.TransactionLine trxLine in selectedTransaction.TrxLines)
                                    {
                                        Products tempProd = productsList.FirstOrDefault(x => x.GetProductsDTO.ProductId == trxLine.ProductID);

                                        DateTime originalDateTime = DateTime.Now;
                                        List<AttractionBooking> tempExistingList = existingBookings.Where(x => (x.AttractionBookingDTO.LineId == trxLine.DBLineId)).ToList();

                                        List<AttractionBooking> tempList = null;
                                        tempList = attractionBooking.Where(x => (x.AttractionBookingDTO.AttractionProductId == trxLine.ProductID && x.AttractionBookingDTO.BookedUnits > 0)).ToList();

                                        if (tempList.Any() && trxLine.card != null && !tempProd.GetProductsDTO.AutoGenerateCardNumber.Equals("Y") && !isBooking)
                                        {
                                            tempList = tempList.Where(y => y.cardList.Any(z => z.CardNumber == trxLine.card.CardNumber)).ToList();
                                        }

                                        if (tempExistingList.Any())
                                        {
                                            AttractionBooking originalATB = tempExistingList[0];
                                            AttractionBooking atb = tempList[0];

                                            AttractionBooking clone = new AttractionBooking(machineUserContext);
                                            clone.CloneObject(atb, 1);
                                            clone.AttractionBookingDTO.TrxId = originalATB.AttractionBookingDTO.TrxId;
                                            clone.AttractionBookingDTO.LineId = originalATB.AttractionBookingDTO.LineId;
                                            atb.AttractionBookingDTO.BookedUnits -= 1;
                                            finalTargetATBDTOList.Add(clone.AttractionBookingDTO);
                                            finalSourceATBDTOList.Add(originalATB.AttractionBookingDTO);
                                        }
                                    }

                                    RescheduleAttractionBookingsBL rescheduleAttractionBookingBL = new RescheduleAttractionBookingsBL(selectedTransaction.Utilities.ExecutionContext,
                                        finalSourceATBDTOList, finalTargetATBDTOList);
                                    rescheduleAttractionBookingBL.RescheduleAttractionBookings(dBTransaction.SQLTrx);

                                    dBTransaction.EndTransaction();
                                    this.NewTrx = null;
                                    //PrintSpecificTransaction(selectedTransaction.Trx_id, true);
                                }
                                catch (Exception ex)
                                {
                                    this.Cursor = Cursors.Default;
                                    log.Error(ex);
                                    dBTransaction.RollBack();
                                    displayMessageLine(message, ERROR);
                                    log.LogMethodExit("Ends-ctxBookingContextMenu_ItemClicked() as unable to Save AttractionBooking error:)" + message);
                                    return;
                                }
                            }

                            this.Cursor = Cursors.Default;
                            displayAttractions(attractionSearchParamInputCard, attractionSearchParamCustomerDTO, attractionSearchParamTxId, attractionSearchParamFacilityMapDTO,
                                attractionSearchParamFromDate, attractionSearchParamToDate, attractionSearchParamAttractionScheduleId);
                        }
                    }
                }
                else
                {
                    int quantity = (int)dgvBookings.SelectedRows[0].Cells["Units"].Value;
                    Attraction.frmChangeBookingQuantity f = new Attraction.frmChangeBookingQuantity(quantity, "Enter Number of Units to Reduce");
                    if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK && f._Quantity > 0)
                        quantity = f._Quantity;
                    else
                    {
                        log.LogMethodExit("End as Reschedule f._Quantity < 0)");
                        return;
                    }

                    using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
                    {
                        try
                        {
                            dBTransaction.BeginTransaction();

                            AttractionBooking originalATB = new AttractionBooking(Utilities.ExecutionContext, (int)dgvBookings.SelectedRows[0].Cells["BookingId"].Value, true, dBTransaction.SQLTrx);
                            originalATB.ReduceBookedUnits(quantity, dBTransaction.SQLTrx);

                            dBTransaction.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            dBTransaction.RollBack();
                            displayMessageLine("Unable to reduce Booking quantity, Errro: " + ex.Message, ERROR);
                            log.LogMethodExit("Unable to reduce Booking quantity error");
                            return;
                        }
                    }
                    displayBookings();
                }
            }
            ctxBookingContextMenu.Hide();
            log.LogMethodExit();
        }

        void dgvBookings_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit("Ends-displayAttractions() as e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dgvBookings.Rows[e.RowIndex].Selected = true;
                ctxBookingContextMenu.Show(MousePosition.X, MousePosition.Y);
            }
            log.LogMethodExit();
        }

        private void chkShowPast_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //displayAttractions();
            RefreshScheduleGrid();
            log.LogMethodExit();
        }

        private void btnRescheduleAttraction_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int facilityMapId = -1;
            if (cmbAttractionFacility.SelectedValue != null)
            {
                facilityMapId = Convert.ToInt32(cmbAttractionFacility.SelectedValue);
            }
            if (facilityMapId == -1)
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(2776), "Attraction Facility", MessageBoxButtons.OK);
            }
            else
            {
                btnRescheduleAttraction.BackgroundImage = global::Parafait_POS.Properties.Resources.Reschedule_Icon_Normal;
                String message = "";

                int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 0);
                LookupValuesList serverTimeObject = new LookupValuesList(Utilities.ExecutionContext);
                DateTime selectedDate = dtpAttractionDate.Value;
                if (selectedDate.Hour < businessEndHour)
                {
                    selectedDate = selectedDate.AddDays(-1).Date.AddHours(businessEndHour);
                }

                List<AttractionBooking> atbList = new List<AttractionBooking>();
                List<AttractionBooking> attractionBooking = RescheduleAttractionBookingSlot(atbList, selectedDate, facilityMapId, ref message);
                RefreshScheduleGrid();
            }

            log.LogMethodExit();
        }

        private void btnRescheduleAttraction_MouseDown(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnRescheduleAttraction.BackgroundImage = global::Parafait_POS.Properties.Resources.Reschedule_Icon_Hover;
            log.LogMethodExit();
        }

        private void btnRescheduleAttraction_MouseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnRescheduleAttraction.BackgroundImage = global::Parafait_POS.Properties.Resources.Reschedule_Icon_Normal;
            log.LogMethodExit();
        }

        private void cmbAttractionFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //displayAttractions();
            RefreshScheduleGrid();
            log.LogMethodExit();
        }

        private void dgvAllReservations_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.LogMethodExit("Ends-dgvAllReservations_CellClick() as e.RowIndex < 0 || e.ColumnIndex < 0");
                return;
            }

            if (dgvAllReservations.Columns[e.ColumnIndex].Name.Equals("dcSelectBooking"))
            {
                editBooking();
            }
            log.LogMethodExit(); ;
        }

        void editBooking()
        {
            log.LogMethodEntry();
            try
            {
                if (dgvAllReservations.CurrentRow.Cells["Status"].Value.ToString() == "COMPLETE" 
                    && (dgvAllReservations.CurrentRow.Cells["TrxStatus"].Value.ToString() == "OPEN" 
                        || dgvAllReservations.CurrentRow.Cells["TrxStatus"].Value.ToString() == "INITIATED"
                        || dgvAllReservations.CurrentRow.Cells["TrxStatus"].Value.ToString() == "PREPARED"
                        || dgvAllReservations.CurrentRow.Cells["TrxStatus"].Value.ToString() == "ORDERED"))
                {
                    tabControlCardAction.SelectedTab = tabPageTrx;
                    tcOrderView.SelectedTab = tpOrderOrderView;
                    Application.DoEvents();
                    displayOpenOrders((int)dgvAllReservations.CurrentRow.Cells["TrxId"].Value);
                }
                else
                {
                    using (Reservation.frmReservationUI f = new Reservation.frmReservationUI(Convert.ToInt32(dgvAllReservations.CurrentRow.Cells["BookingId"].Value)))
                    {
                        f.PerformActivityTimeOutChecks += new Reservation.frmReservationUI.PerformActivityTimeOutChecksdelegate(PerformActivityTimeOutChecks);
                        f.ShowDialog(); 
                        f.Dispose();
                    }
                    lnkRefreshReservations_LinkClicked(null, null);
                }
            }
            catch
            {
                log.Fatal("Ends-editBooking() due to exception in dgvAllReservations");
            }

            log.LogMethodExit();
        }

        //Begin: Modified for Booking View Added on 21-Mar-2016
        private void dgvBookingDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0)
            {
                log.LogMethodExit("Ends-dgvBookingDetails_CellClick() as e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                return;
            }
            int trxId = (int)dgvBookingDetails.Rows[e.RowIndex].Cells["TrxId"].Value;
            refreshOrder(trxId);

            log.LogMethodExit();
        }

        //Begin: Modified for Changing Query for Booking View Display (Booking Product and Status) on 22-Mar-2016
        public void displayBookingDetails()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable(@"select isnull(b.CustomerName, c.customer_name) [Customer Name],
                                                                b.ReservationCode [Reservation Code], 
                                                                p.product_name [Booking Product],
                                                                th.status [Booked Status],
                                                                th.TrxDate Date ,th.TrxId ,
                                                                th.pos_machine POS , th.Remarks
                                                        from Bookings b
                                                        Left outer join Customers c on c.customer_id = b.CustomerId
                                                        inner join Products p on p.product_id = b.BookingProductId      
                                                        inner join trx_header th on b.trxid = th.trxid 
                                                        and th.Status in ('OPEN','INITIATED','ORDERED','PREPARED', 'RESERVED')
                                                        and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                                        and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                                        and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
                                                        and 1 = CASE WHEN (th.status = 'OPEN' OR th.status = 'INITIATED' OR th.status = 'ORDERED'  OR th.status = 'PREPARED' )
                                                                     THEN 1
														             WHEN TH.STATUS = 'RESERVED' 
																	  AND TH.TrxDate > case WHEN datepart(HH, getdate()) between 00 and 06
							                                                            then dateadd(hour,6,convert(datetime,convert(date,getdate())))
							                                                            else dateadd(hour,6,convert(datetime,convert(date,getdate() + 1)))
							                                                            end
																	 THEN 0 
																	 ELSE 1
															     END
                                                        order by trxdate", 
                                                        new SqlParameter[] { new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")),
                                                        new SqlParameter("@enableOrderShareAcrossPOSCounters", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS),
                                                        new SqlParameter("@enableOrderShareAcrossPOS", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS),
                                                        new SqlParameter("@enableOrderShareAcrossUsers", POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS),
                                                        new SqlParameter("@POSTypeId", ParafaitEnv.POSTypeId),
                                                        new SqlParameter("@userId", ParafaitEnv.User_Id),
                                                        new SqlParameter("@POSMachineId", ParafaitEnv.POSMachineId),
                                                        new SqlParameter("@POSMachineName", ParafaitEnv.POSMachine)
                                                                           });
            //End: Modified for Changing Query for Booking View Display (Booking class and Status) on 22-Mar-2016

            dgvBookingDetails.DataSource = dt;
            dgvBookingDetails.Refresh();
            Application.DoEvents();
            dgvBookingDetails.EndEdit();
            dgvBookingDetails.ClearSelection();
            //dgvBookingDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvBookingDetails.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvBookingDetails.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBookingDetails.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();

            //Begin Modification-Jan-14-2016-Added to display Booking Details. When all orders are completed panel is made visible false
            if (dt.Rows.Count > 0)
            {
                if (!panelOrders.Visible)
                {
                    // dataGridViewTransaction.Height = tabPageTrx.Height - panelOrders.Height - dgvTrxSummary.Height - panelTrxButtons.Height - 13;
                    // dataGridViewTransaction.Location = new Point(4, 196);
                    // panelOrders.Visible = true;
                    if (this.Controls["TableView"] != null)
                        this.Controls["TableView"].Visible = true;
                    //tcOrderView.SelectedTab = tpOrderBookingView;
                }
            }
            log.LogMethodExit();
        }

        void dgvAttractionSchedules_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit("Ends-displayAttractions() as e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dgvAttractionSchedules.Rows[e.RowIndex].Selected = true;
                //ctxAttractionScheduleContextMenu.Show(MousePosition.X, MousePosition.Y);
            }
            log.LogMethodExit();
        }

        void tpAttractionBooking_OnEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            displayAttractions();
            log.LogMethodExit();
        }
    }
}
