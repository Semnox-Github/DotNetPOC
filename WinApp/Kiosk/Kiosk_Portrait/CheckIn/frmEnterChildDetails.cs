/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - Handles Playground Entry menu
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.0.0    20-Sep-2021      Sathyavathi        Created : Check-In feature Phase-2
*2.150.1      22-Feb-2023      Sathyavathi        Kiosk Cart Enhancements
*2.155.0      22-Jun-2023      Sathyavathi        Attraction Sale in Kiosk - Calendar changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using System.Globalization;
using System.Threading;

namespace Parafait_Kiosk
{
    public partial class frmEnterChildDetails : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private SortableBindingList<CustomCheckInDetailDTO> customCheckInDetailDTOList = new SortableBindingList<CustomCheckInDetailDTO>();
        private Font savTimeOutFont;
        private Font TimeOutFont;
        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private int childQtySelectedInQtyScreen;
        private List<CustomCustomerDTO> selectedChildList;
        private const decimal UPPER_AGE_LIMIT = 999;
        private ProductsContainerDTO productsContainerDTO;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> childTrxLines;
        private PurchaseProductDTO purchaseProductDTO;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        public TextBox CurrentActiveTextBox;
        private int dOBFirstColumnIndex; 

        internal List<Semnox.Parafait.Transaction.Transaction.TransactionLine> TrxLines { get { return childTrxLines; } }
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        internal List<CustomCustomerDTO> SelectedChildList { get { return selectedChildList; } }

        public frmEnterChildDetails(KioskTransaction kioskTransaction, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> inTrxLines, PurchaseProductDTO purchaseProd, List<CustomCustomerDTO> inChildList)
        {
            log.LogMethodEntry("kioskTransaction", inTrxLines, purchaseProd, inChildList);
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.selectedChildList = inChildList;
            this.childTrxLines = inTrxLines;
            this.purchaseProductDTO = purchaseProd; 
            btnProceed.Enabled = false;
            KioskStatic.setDefaultFont(this);

            try
            {
                ProductQtyMappingDTO productQtyMapping = purchaseProductDTO.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductId == childTrxLines[0].ProductID).FirstOrDefault();
                if (productQtyMapping == null)
                {
                    string errMsg = "Error: Could not retrieve product details from Purchase Product in Select Child screen";
                    log.Error(errMsg);
                    log.Debug(productQtyMapping);
                    KioskStatic.logToFile(errMsg);
                    return;
                }
                this.productsContainerDTO = productQtyMapping.ProductsContainerDTO;
                childQtySelectedInQtyScreen = childTrxLines.Count;

                lblSiteName.Text = KioskStatic.SiteHeading;
                savTimeOutFont = lblTimeRemaining.Font;
                TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
                SetKioskTimerTickValue(60);
                lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");

                InitializeKeyboard();

                //get facility name
                List<FacilityContainerDTO> facilityContainerDTOList = FacilityContainerList.GetFacilityContainerDTOCollection(utilities.ExecutionContext.SiteId).FacilitysContainerDTOList;
                FacilityContainerDTO facilityContainerDTO = facilityContainerDTOList.Where(f => f.FacilityId == productsContainerDTO.CheckInFacilityId).FirstOrDefault();
                string checkInFacilityName = facilityContainerDTO == null ? "" : facilityContainerDTO.FacilityName;

                lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4349);
                lblPackageHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Package") + ":";//Package Name Header
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                lblPackage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, productName);//Product Name for which child is being selected
                lblFacilityHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Playground") + ":";//facility Name Header
                lblFacility.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, checkInFacilityName);//facility name
                panelAgeCriteria.Visible = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                lblAgeCriteriaHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4812) + ":";//Age Criteria
                if (productsContainerDTO.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
                {
                    lblAgeCriteria.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4777, productsContainerDTO.AgeLowerLimit, productsContainerDTO.AgeUpperLimit);//1 to 13 yrs
                }
                else if (productsContainerDTO.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit == KioskStatic.AGE_UPPER_LIMIT)
                {
                    //lblAgeCriteria.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4779, productsContainerDTO.AgeLowerLimit);//above 1 yrs

                    lblAgeCriteria.Text = (productsContainerDTO.AgeLowerLimit == 1) ?
                        MessageContainerList.GetMessage(utilities.ExecutionContext, 5174, productsContainerDTO.AgeLowerLimit) //Above 1 yr
                        : MessageContainerList.GetMessage(utilities.ExecutionContext, 4779, productsContainerDTO.AgeLowerLimit); //Above 2 yrs
                }
                else if (productsContainerDTO.AgeLowerLimit == KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
                {
                    lblAgeCriteria.Text = (productsContainerDTO.AgeLowerLimit > 1) ?
                        MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4778, productsContainerDTO.AgeUpperLimit) //below 18 yrs
                        : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5172, productsContainerDTO.AgeUpperLimit); //below 1 yr
                }
                lblGuestCountHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4351); //"Quantity:&1"
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, (selectedChildList == null) ? 0 : selectedChildList.Count, childQtySelectedInQtyScreen); //"Quantity:&1"
                lblScreenDetails.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4787);

                dgvEnterChildDetails.Columns["LinkRelationdgvBtn"].HeaderText = "";
                dgvEnterChildDetails.Columns["LinkRelationdgvBtn"].DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4117); // "Click to link"
                lblChildAdded.Visible = false;
                lblGridFooterMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4350);//(*) Marks field are mandatory to fill up

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
                KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
                txtMessage.Text = "";
                SetCustomImages();
                SetFont();
                utilities.setLanguage(this);
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmEnterChildDetails(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmEnterChildDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                GetColumnsToDisplay();

                nameDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);//Enter Child Name
                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;

                dgvEnterChildDetails.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(CustomCheckInDetailDTOBindingSource_DataError);// Attach an event handler for the DataError event.
                customCheckInDetailDTOBindingSource.DataSource = new SortableBindingList<CustomCheckInDetailDTO>(customCheckInDetailDTOList);  // Bind the BindingSource to the DataGridView
                dgvEnterChildDetails.DataSource = customCheckInDetailDTOBindingSource;  // control's DataSource.
                //dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                dgvEnterChildDetails.AllowUserToAddRows = false;
                ClearDGV();
                if (selectedChildList == null)
                {
                    selectedChildList = new List<CustomCustomerDTO>(purchaseProductDTO.ParentCustomerId);
                }
                int diffCount = childQtySelectedInQtyScreen- ((selectedChildList == null) ? 0 : selectedChildList.Count);
                if (selectedChildList != null && selectedChildList.Any())
                {
                    AutoPopulateChildDetails();
                }
                for (int i = 0; i < diffCount; i++)
                {
                    InsertNewCustomCheckInDetailDTO(null);
                }
                UpdateBindDataSource();
                UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                displayMessageLine(ex.Message, ERROR);
                StopKioskTimer();
                frmOKMsg.ShowUserMessage(ex.Message);
                StartKioskTimer();
                KioskStatic.logToFile("Error while dgvEnterChildDetails_Load(): " + ex);
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();

            try
            {
                if (!ValidateAgeCreteriaOfAllProducts())
                    return;

                AttachCheckInDetailDTOToTrxLines();
                DisposeKeyboardObject();
                using (frmChildSummary frm = new frmChildSummary(kioskTransaction, childTrxLines, purchaseProductDTO))
                {
                    DialogResult dresult = frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    if (dresult != System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        DialogResult = dresult;
                        Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while processing frmEnterChildDetails btnProceed_Click:" + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void dgvEnterChildDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            dgvEnterChildDetails.EndEdit();
            try
            {
                StopKioskTimer();
                if (dgvEnterChildDetails.Rows[e.RowIndex].ReadOnly)
                    return;

                if (dgvEnterChildDetails.Columns[e.ColumnIndex].Name == "LinkRelationdgvBtn")
                {
                    //if customer is already linked, ignore the click to link relation button
                    if (customCheckInDetailDTOList[e.RowIndex].CustomerId > -1)
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4811); //Relation is already linked
                        displayMessageLine(msg, WARNING);
                        return;
                    }

                    if (string.IsNullOrEmpty(customCheckInDetailDTOList[e.RowIndex].Name)
                        || customCheckInDetailDTOList[e.RowIndex].DateOfBirth == null
                        || customCheckInDetailDTOList[e.RowIndex].DateOfBirth == DateTime.MinValue)
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4473); //Required fields are not entered
                        frmOKMsg.ShowUserMessage(msg);
                        displayMessageLine(msg, WARNING);
                        return;
                    }

                    if (!ValidateDateOfBirth(e.RowIndex))
                        return;

                    txtMessage.Text = "";
                    CustomCustomerDTO linkedRelatedCustomerDTO = LinkRelationToParentCustomer(customCheckInDetailDTOList[e.RowIndex]);
                    if (linkedRelatedCustomerDTO != null)
                    {
                        if (selectedChildList == null)
                        {
                            selectedChildList = new List<CustomCustomerDTO>(purchaseProductDTO.ParentCustomerId);
                        }
                        selectedChildList.Add(linkedRelatedCustomerDTO);
                        customCheckInDetailDTOList.RemoveAt(e.RowIndex);
                        CustomCheckInDetailDTO newCheckInDetailDTO = new CustomCheckInDetailDTO();
                        newCheckInDetailDTO.Name = linkedRelatedCustomerDTO.RelatedCustomerName;
                        newCheckInDetailDTO.DateOfBirth = linkedRelatedCustomerDTO.DOB;
                        newCheckInDetailDTO.Age = linkedRelatedCustomerDTO.Age;
                        newCheckInDetailDTO.CustomerId = linkedRelatedCustomerDTO.RelatedCustomerId;
                        customCheckInDetailDTOList.Insert(e.RowIndex, newCheckInDetailDTO);
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Value = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4474); //Linked
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.SelectionBackColor = Color.Green;
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.SelectionForeColor = Color.White;
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.BackColor = Color.Green;
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.ForeColor = Color.White;
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = newCheckInDetailDTO.DateOfBirth;
                        UpdateBindDataSource();
                        dgvEnterChildDetails.EndEdit();
                        dgvEnterChildDetails.Refresh();
                    }
                }
                else if (dgvEnterChildDetails.Columns[e.ColumnIndex].Name == "nameDataGridViewTextBoxColumn")
                {
                    if (dgvEnterChildDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value != null
                        && dgvEnterChildDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString() == MessageContainerList.GetMessage(utilities.ExecutionContext, 4135))
                    {
                        dgvEnterChildDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value = string.Empty;
                    }
                }
                else if (dgvEnterChildDetails.Columns[e.ColumnIndex].Name == "dateOfBirthDataGridViewTextBoxColumn")
                {
                    if (customCheckInDetailDTOList[e.RowIndex].CustomerId > 0)
                    {
                        dgvEnterChildDetails.CurrentCell.ReadOnly = true;
                    }
                    else
                    {
                        HideKeyboardObject();
                        DateTime doB = customCheckInDetailDTOList[e.RowIndex].DateOfBirth == null ? 
                            DateTime.MinValue : Convert.ToDateTime(customCheckInDetailDTOList[e.RowIndex].DateOfBirth);
                        DateTime newDOB = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: KioskStatic.enableDaySelection, enableMonthSelection: KioskStatic.enableMonthSelection,
                            enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                        if (newDOB != null && newDOB != DateTime.MinValue)
                        {
                            dgvEnterChildDetails.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = newDOB.Date.ToString();
                        }
                        string dateMonthformat = KioskStatic.DateMonthFormat;
                        DateTime dateofBirthValue = DateTime.MinValue;
                        try
                        {
                            dateofBirthValue = KioskHelper.GetFormatedDateValue(newDOB);
                            if (dateofBirthValue != null && dateofBirthValue != Convert.ToDateTime(customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].DateOfBirth))
                            {
                                dgvEnterChildDetails.Rows[dgvEnterChildDetails.CurrentRow.Index].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = dateofBirthValue.Date.ToString(KioskStatic.DateMonthFormat);
                                customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].DateOfBirth = dateofBirthValue;
                                customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].Age = KioskHelper.GetAge(dateofBirthValue.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                                dateofBirthValue = Convert.ToDateTime(newDOB, provider);
                                if (dateofBirthValue != null && dateofBirthValue != Convert.ToDateTime(customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].DateOfBirth))
                                {
                                    dgvEnterChildDetails.Rows[dgvEnterChildDetails.CurrentRow.Index].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = dateofBirthValue.Date.ToString(KioskStatic.DateMonthFormat);
                                    customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].DateOfBirth = dateofBirthValue;
                                    customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].Age = KioskHelper.GetAge(dateofBirthValue.ToString());
                                }
                            }
                            catch (Exception exp)
                            {
                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)), ERROR);
                                log.Error(exp);
                                log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)));
                            }
                            log.Error(ex);
                        }
                        dgvEnterChildDetails.EndEdit();
                        dgvEnterChildDetails.Refresh(); 
                    }
                }
                else if (dgvEnterChildDetails.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn")
                {
                    if (!string.IsNullOrEmpty(dgvEnterChildDetails.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                    {
                        if (customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].CustomerId != -1)
                        {
                            int index = selectedChildList.IndexOf(selectedChildList.Where(l => l.RelatedCustomerId == customCheckInDetailDTOList[dgvEnterChildDetails.CurrentRow.Index].CustomerId).FirstOrDefault());
                            selectedChildList.RemoveAt(index);
                        }

                        CustomCheckInDetailDTO customCheckInDetailDTO = new CustomCheckInDetailDTO();
                        customCheckInDetailDTO.Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                        customCheckInDetailDTO.DateOfBirth = null;
                        customCheckInDetailDTOList.RemoveAt(dgvEnterChildDetails.CurrentRow.Index);
                        customCheckInDetailDTOList.Add(customCheckInDetailDTO);
                        UpdateBindDataSource();
                    }
                }
                UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while processing deleteDataGridViewImageColumn of dgvEnterChildDetails: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void CustomCheckInDetailDTOBindingSource_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            e.Cancel = true;
            log.LogMethodExit(null);
        }

        private void dgvEnterChildDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.Control is TextBox)
                {
                    CurrentActiveTextBox = e.Control as TextBox;

                    Font font = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size);
                    e.CellStyle = dgvEnterChildDetails.DefaultCellStyle;
                    e.CellStyle.Font = font;
                }
                else if (e.Control is ComboBox)
                {
                    ComboBox cb = e.Control as ComboBox;
                    if (cb != null)
                    {
                        cb.IntegralHeight = false;
                        cb.MaxDropDownItems = 10;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while setting cell style in dgvCustomCustomerDTOList_EditingControlShowing() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvEnterChildDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            try
            {
                if(customCheckInDetailDTOBindingSource.DataSource is SortableBindingList<CustomCheckInDetailDTO>)
                {
                    customCheckInDetailDTOList = customCheckInDetailDTOBindingSource.DataSource as SortableBindingList<CustomCheckInDetailDTO>;
                    if (customCheckInDetailDTOList.Count > 0)
                    {
                        dgvEnterChildDetails.Rows[customCheckInDetailDTOList.Count - 1].Selected = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvEnterChildDetails_DataBindingComplete() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvEnterChildDetails_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                var addImage = Properties.Resources.Add;
                var deleteImage = Properties.Resources.Delete;

                if (dgvEnterChildDetails.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn")
                {
                    if ((dgvEnterChildDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value != null)
                         && (dgvEnterChildDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString() != MessageContainerList.GetMessage(utilities.ExecutionContext, 4135))
                         && !string.IsNullOrWhiteSpace(dgvEnterChildDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                    {
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        var w = deleteImage.Width;
                        var h = deleteImage.Height;
                        var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                        var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                        e.Graphics.DrawImage(deleteImage, new Rectangle(x, y, w, h));
                    }
                    else
                    {
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        var w = addImage.Width;
                        var h = addImage.Height;
                        var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                        var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                        e.Graphics.DrawImage(addImage, new Rectangle(x, y, w, h));

                    }
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvEnterAdultDetails_CellPainting(): " + ex.Message);
            }
            log.LogMethodExit();
        }


        private void dgvEnterChildDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.RowIndex < 0)
                return;

            dgvEnterChildDetails.EndEdit();
            try
            {
                if (dgvEnterChildDetails.Columns[e.ColumnIndex].Name == "nameDataGridViewTextBoxColumn")
                {
                    if (string.IsNullOrWhiteSpace(dgvEnterChildDetails.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                    {
                        customCheckInDetailDTOList[e.RowIndex].Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                    }
                    dgvEnterChildDetails.CurrentCell.Style.BackColor = Color.White;
                    UpdateCheckInGuestsCount();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("dgvEnterChildDetails_CellLeave: Error while handling wrong Date Of Birth : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void HighlightErrorField(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            ResetKioskTimer();

            try
            {
                HideKeyboardObject();
                dgvEnterChildDetails.Rows[rowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Style.SelectionBackColor = Color.Red;
                string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4288); //Date of birth doesn't meet the age requirements of the chosen product
                log.Error(msg);
                displayMessageLine(msg, ERROR);
                frmOKMsg.ShowUserMessage(msg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("HighlightErrorField(): Unexpected error while handling wrong Date Of Birth : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private bool ValidateDateOfBirth(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            ResetKioskTimer();
            bool status = false;
            try
            {
                //string dateOfBirth = customCheckInDetailDTOList[rowIndex].DateOfBirth;
                DateTime? dobValue = customCheckInDetailDTOList[rowIndex].DateOfBirth;
                if (dobValue != null)
                {
                    decimal age = KioskHelper.GetAge(((DateTime)dobValue).ToString());
                    if (age >= productsContainerDTO.AgeLowerLimit && age <= productsContainerDTO.AgeUpperLimit)
                    {
                        dgvEnterChildDetails.Rows[rowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Style.SelectionBackColor = this.dgvEnterChildDetails.DefaultCellStyle.SelectionBackColor;
                        customCheckInDetailDTOList[rowIndex].DateOfBirth = dobValue;
                        status = true;
                        UpdateBindDataSource();
                    }
                }

                if(status == false)
                    HighlightErrorField(rowIndex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ValidateDateOfBirth(): Unexpected error while handling wrong Date Of Birth : " + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private void AutoPopulateChildDetails()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                for (int i = 0; i < selectedChildList.Count; i++)
                {
                    CustomCheckInDetailDTO childCheckInDetailDTO = new CustomCheckInDetailDTO();
                    childCheckInDetailDTO.Name = selectedChildList[i].RelatedCustomerName;
                    childCheckInDetailDTO.DateOfBirth = selectedChildList[i].DOB;
                    childCheckInDetailDTO.Age = selectedChildList[i].Age;
                    childCheckInDetailDTO.CustomerId = selectedChildList[i].RelatedCustomerId;
                    InsertNewCustomCheckInDetailDTO(childCheckInDetailDTO);

                    //populate Day, Month and Year columns
                    dgvEnterChildDetails.Rows[i].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = selectedChildList[i].DOB;

                    //since disable option is not there for dgv button, just remove text on the button and handle in cell click action
                    dgvEnterChildDetails.Rows[i].Cells["LinkRelationdgvBtn"].Value = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4474); //Linked
                    dgvEnterChildDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.SelectionBackColor = Color.Green;
                    dgvEnterChildDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.SelectionForeColor = Color.White;
                    dgvEnterChildDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.BackColor = Color.Green;
                    dgvEnterChildDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.ForeColor = Color.White;

                    dgvEnterChildDetails.Rows[i].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while AutoPopulateChildDetails():" + ex);
            }
            log.LogMethodExit();
        }

        private bool ValidateAgeCreteriaOfAllProducts()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            bool status = true;
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                {
                    //When Ignore birth year is set, check-in feature is expected to work like phase-1.
                    //Date of Birth validation in that case is to be skipped
                    log.LogMethodExit(status);
                    return status;
                }

                if (productsContainerDTO.CustomerProfilingGroupId == -1)
                {
                    //If there is no customer profile set, Date of Birth validation is to be skipped
                    log.LogMethodExit(status);
                    return status;
                }

                for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
                {
                    status = ValidateDateOfBirth(i);

                    if (!status)
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while ValidateAgeCreteriaOfAllProducts():" + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private void UpdateBindDataSource()
        {
            log.LogMethodEntry();

            if (dgvEnterChildDetails.Rows.Count <= 0)
                return;

            try
            {
                ResetKioskTimer();
                //dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                dgvEnterChildDetails.Rows[customCheckInDetailDTOList.Count - 1].Selected = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateBindDataSource() - Enter Child Details Screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void AttachCheckInDetailDTOToTrxLines()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
                {
                    childTrxLines[i].LineCheckInDetailDTO = customCheckInDetailDTOList[i].CheckInDetailDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while AttachCheckInDetailDTOToTrxLines() in enter child details: " + ex.Message);
            }

            log.LogMethodExit();
        }

        private void InsertNewCustomCheckInDetailDTO(CustomCheckInDetailDTO inChildDTO = null)
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                ResetKioskTimer();
                CustomCheckInDetailDTO customCheckInDetailDTO = new CustomCheckInDetailDTO();
                if (inChildDTO != null)
                {
                    customCheckInDetailDTO = inChildDTO;
                }
                else
                {
                    customCheckInDetailDTO.Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                }
                customCheckInDetailDTOList.Add(customCheckInDetailDTO);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while InsertNewCustomCheckInDetailDTO() in frmEnterChildDetails: " + ex);
            }
            log.LogMethodExit();
        }

        private void UpdateCheckInGuestsCount()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();

                int selectedGuestCount = customCheckInDetailDTOList.Count(k => !string.IsNullOrWhiteSpace(k.Name)
                                        && k.Name != MessageContainerList.GetMessage(utilities.ExecutionContext, 4135));
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, selectedGuestCount, childQtySelectedInQtyScreen); //"Quantity:&1"
                btnProceed.Enabled = (selectedGuestCount < childQtySelectedInQtyScreen) ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateCheckInGuestsCount() in child screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void LeftButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void RightButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private CustomCustomerDTO LinkRelationToParentCustomer(CustomCheckInDetailDTO customCheckInDetailDTO)
        {
            log.LogMethodEntry();
            StopKioskTimer();

            CustomCustomerDTO linkedRelatedCustomerDTO = null;
            try
            {
                CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(purchaseProductDTO.ParentCustomerId);
                CustomerDTO parentCustomer = customCustomerDTO.GetCustomerDTO(purchaseProductDTO.ParentCustomerId);
                //virtualKeyboardController.Dispose();
                HideKeyboardObject();
                using (frmAddCustomerRelation frm = new frmAddCustomerRelation(parentCustomer.Id
                         , (parentCustomer.FirstName + " " + parentCustomer.LastName), parentCustomer.CardNumber
                         , customCheckInDetailDTO.Name, customCheckInDetailDTO.DateOfBirth.ToString()))
                {
                    DialogResult dr = frm.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        linkedRelatedCustomerDTO = frm.LinkedCustomerDTO;
                        frm.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateCheckInGuestsCount() in child screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit(linkedRelatedCustomerDTO);
            return linkedRelatedCustomerDTO;
        }

        private void ClearDGV()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                customCheckInDetailDTOBindingSource.DataSource = new SortableBindingList<CustomCheckInDetailDTO>();
                dgvEnterChildDetails.DataSource = customCheckInDetailDTOBindingSource;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvEnterChildDetails_ClearData() : " + ex);
            }
            log.LogMethodExit();
        }

        private void SortDOBFields()
        {

        }

        private void GetColumnsToDisplay()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                LookupsContainerDTO CheckinFieldsLookupValuesList = LookupsContainerList.GetLookupsContainerDTO(-1, "KIOSK_CHECKIN_DISPLAY_FIELDS_CONFIG");
                if (CheckinFieldsLookupValuesList != null)
                {
                    for (int i = 0; i < CheckinFieldsLookupValuesList.LookupValuesContainerDTOList.Count; i++)
                    {
                        switch (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].LookupValue)
                        {
                            case "Name":
                                nameDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                nameDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "VehicleNumber":
                                vehicleNumberDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //vehicleNumberDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "VehicleModel":
                                vehicleModelDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //vehicleModelDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "VehicleColour":
                                vehicleColorDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //vehicleColorDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "DateOfBirth":
                                bool value = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                dateOfBirthDataGridViewTextBoxColumn.Visible = value;
                                dateOfBirthDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "Age":
                                ageDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                break;

                            case "SpecialNeeds":
                                specialNeedsDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //specialNeedsDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "Allergies":
                                allergiesDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //allergiesDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Fetching Lookup values for dgvEnterChildDetails column display fields: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(panelGuestEntry.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeypad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(panelGuestEntry.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeypad }, showKeyboardOnTextboxEntry, null, lblScreenDetails.Font.FontFamily.Name);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    SetVirtualKeyboard();
                }
                else
                {
                    SetCustomKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.Dispose();
                }
                else
                {
                    customKeyboardController.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void HideKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.HideKeyboard();
                }
                else
                {
                    customKeyboardController.HideKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnKeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void FormOnKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void FormOnKeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void FormOnMouseClick(Object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(); try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            ResetKioskTimer();
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 60)
            {
                lblTimeRemaining.Font = TimeOutFont;
                lblTimeRemaining.Text = tickSecondsRemaining.ToString("#0");
            }
            else
            {
                lblTimeRemaining.Font = savTimeOutFont;
                lblTimeRemaining.Text = (tickSecondsRemaining / 60).ToString() + ":" + (tickSecondsRemaining % 60).ToString().PadLeft(2, '0');
            }

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 457), WARNING);
                Application.DoEvents();
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for enter Child details screen");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.EnterChildBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.panelGuestEntry.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.btnPrev.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmEnterChildDetails", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmEnterChildDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                dgvEnterChildDetails.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvEnterChildDetails.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                dgvEnterChildDetails.DefaultCellStyle.Font =
                    dgvEnterChildDetails.RowHeadersDefaultCellStyle.Font =
                    dgvEnterChildDetails.RowsDefaultCellStyle.Font =
                    dgvEnterChildDetails.RowTemplate.DefaultCellStyle.Font =
                    dgvEnterChildDetails.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                lblGreeting.Font = new Font(lblGreeting.Font.FontFamily, 38F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGuestCountHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblAgeCriteriaHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblPackageHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblFacilityHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGuestCount.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblAgeCriteria.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblPackage.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblFacility.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblScreenDetails.Font = new Font(lblGreeting.Font.FontFamily, 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGridFooterMsg.Font = new Font(lblGreeting.Font.FontFamily, 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                //this.dgvEnterChildDetails.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
                //this.dgvEnterChildDetails.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.HighlightText;
                this.dgvEnterChildDetails.DefaultCellStyle.SelectionBackColor = this.dgvEnterChildDetails.DefaultCellStyle.BackColor;
                this.dgvEnterChildDetails.DefaultCellStyle.SelectionForeColor = this.dgvEnterChildDetails.DefaultCellStyle.ForeColor;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while SetFont() of enter child details screen: " + ex);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements for frmEnterChildDetails");
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGreetingLblTextForeColor;
                lblGridFooterMsg.ForeColor = KioskStatic.CurrentTheme.LblGridFooterMsgTextForeColor; 
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsFooterTextMsgTextForeColor;//Footer text message
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsProceedButtonTextForeColor;
                //lblChildAdded.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsChildAddedLabelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsHomeButtonTextForeColor;//Footer text message
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsTimeRemainingLblTextForeColor;
                this.lblGuestCountHeader.ForeColor =
                    this.lblPackageHeader.ForeColor =
                    this.lblFacilityHeader.ForeColor =
                    this.lblAgeCriteriaHeader.ForeColor =
                    this.lblGuestCountHeader.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblHeaderTextForeColor;

                this.lblPackage.ForeColor =
                    this.lblFacility.ForeColor =
                    this.lblAgeCriteria.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblTextForeColor;

                this.lblGuestCount.ForeColor = KioskStatic.CurrentTheme.GuestCountLblTextForeColor;
                this.lblScreenDetails.ForeColor = KioskStatic.CurrentTheme.ScreenDetailsLblTextForeColor;
                this.bigVerticalScrollChildDetails.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigHorizontalScrollChildDetails.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollLeftEnabled, ThemeManager.CurrentThemeImages.ScrollLeftDisabled, ThemeManager.CurrentThemeImages.ScrollRightEnabled, ThemeManager.CurrentThemeImages.ScrollRightDisabled);

                this.dgvEnterChildDetails.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGridHeaderTextForeColor;


                this.dgvEnterChildDetails.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor;
                this.dgvEnterChildDetails.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor;
                this.dgvEnterChildDetails.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor;
                this.dgvEnterChildDetails.RowsDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor;
                this.dgvEnterChildDetails.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.EnterChildDetailsGridTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements for frmEnterChildDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void frmEnterChildDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                DisposeKeyboardObject();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmEnterChildDetails_FormClosed()", ex);
            }
            //Cursor.Hide();

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

    }
}


























