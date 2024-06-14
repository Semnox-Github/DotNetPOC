/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - Handles Playground Entry menu
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
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
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using System.Runtime.Serialization;
using System.Globalization;
using System.Data.SqlClient;
using System.Threading;

namespace Parafait_Kiosk
{
    public partial class frmSelectChild : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private int childQtySelectedInQtyScreen;
        private List<CustomCustomerDTO> customCustomerDTOList = new List<CustomCustomerDTO>();
        private List<CustomCustomerDTO> selectedChildList = new List<CustomCustomerDTO>(); //needed to auto populate the selected list in Child Entry screen
        private ProductsContainerDTO productsContainerDTO;
        private CustomerDTO parentCustomerDTO;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> childTrxLines;
        private PurchaseProductDTO purchaseProductDTO;

        internal List<Semnox.Parafait.Transaction.Transaction.TransactionLine> TrxLines { get { return childTrxLines; } }
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmSelectChild(KioskTransaction kioskTransaction, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> inTrxLines, PurchaseProductDTO purchaseProd)
        {
            log.LogMethodEntry("kioskTransaction", inTrxLines, purchaseProd);
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.childTrxLines = inTrxLines;
            this.purchaseProductDTO = purchaseProd;
            KioskStatic.setDefaultFont(this);

            try
            {
                if (childTrxLines != null)
                {
                    childQtySelectedInQtyScreen = childTrxLines.Count;
                }

                //checkin checkout feature currently does not work without parent customer details
                if (purchaseProductDTO.ParentCustomerId == -1)
                {
                    string errMsg = "ERROR: ParentCustomerId is null";
                    log.Error(errMsg);
                    KioskStatic.logToFile(errMsg);
                    log.LogMethodExit();
                    return;
                }

                ProductQtyMappingDTO productQtyMapping = purchaseProd.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductId == childTrxLines[0].ProductID).FirstOrDefault();
                if (productQtyMapping == null)
                {
                    string errMsg = "Error : Could not retrieve product details from Purchase Product in Select Child screen";
                    log.Error(errMsg);
                    log.Debug(productQtyMapping);
                    KioskStatic.logToFile(errMsg);
                    return;
                }
                this.productsContainerDTO = productQtyMapping.ProductsContainerDTO;

                //get facility name
                List<FacilityContainerDTO> facilityContainerDTOList = FacilityContainerList.GetFacilityContainerDTOCollection(utilities.ExecutionContext.SiteId).FacilitysContainerDTOList;
                FacilityContainerDTO facilityContainerDTO = facilityContainerDTOList.Where(f => f.FacilityId == productsContainerDTO.CheckInFacilityId).FirstOrDefault();
                string checkInFacilityName = facilityContainerDTO == null ? "" : facilityContainerDTO.FacilityName;

                lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4347);
                btnSkip.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4469); //"Skip"
                lblMemberDetails.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4346);
                lblYourFamily.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4285);
                lblPackageHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Package") + ":";//Package Name Header
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                lblPackage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, productName);//Product Name for which child is being selected
                lblFacilityHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Playground") + ":";//facility Name Header
                lblFacility.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, checkInFacilityName);//facility name
                panelAgeCriteria.Visible = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                lblAgeCriteriaHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4812) + " :";//Age Criteria
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
                    lblAgeCriteria.Text = (productsContainerDTO.AgeUpperLimit > 1) ? 
                        MessageContainerList.GetMessage(utilities.ExecutionContext, 4778, productsContainerDTO.AgeUpperLimit) //below 18 yrs
                        : MessageContainerList.GetMessage(utilities.ExecutionContext, 5172, productsContainerDTO.AgeUpperLimit); //below 1 yr
                }
                lblGuestCountHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4351); //"Quantity:&1"
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, (selectedChildList == null) ? 0 : selectedChildList.Count, childQtySelectedInQtyScreen); //"Quantity:&1"
                lblScreenDetails.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4786);

                btnProceed.Enabled = false;

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
                btnSkip.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Skip");
                btnPrev.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Back");
                btnProceed.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Proceed");
                txtMessage.Text = "";
                SetCustomImages();
                SetFont();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while frmSelectChild constructor: " + ex.Message);
            }

            GetLinkedRelationsMatchingAgeCriteria();
            if (customCustomerDTOList == null || customCustomerDTOList.Count == 0)
            {
                StopKioskTimer();
                throw new ChildRelationsNotExistException();
            }
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmSelectChild_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StartKioskTimer();
            try
            {
                int rowId = dgvMemberDetails.Rows.Add();
                DataGridViewRow newRow = dgvMemberDetails.Rows[rowId];

                CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(purchaseProductDTO.ParentCustomerId);
                parentCustomerDTO = customCustomerDTO.GetCustomerDTO(purchaseProductDTO.ParentCustomerId);
                if (parentCustomerDTO == null)
                {
                    string errMsg = "ERROR: parentCustomerDTO can not be null";
                    log.Error(errMsg);
                    KioskStatic.logToFile(errMsg);
                    log.LogMethodExit();
                    return;
                }
                newRow.Cells["MemberNameDataGridViewTextBoxColumn"].Value = (parentCustomerDTO.FirstName + parentCustomerDTO.LastName);
                newRow.Cells["CardNumberdataGridViewTextBoxColumn"].Value = purchaseProductDTO.ParentCardNumber ?? "";
                newRow.Cells["memberDateofBirth"].Value = (parentCustomerDTO.DateOfBirth == null) ? "" : parentCustomerDTO.DateOfBirth.Value.ToString(KioskStatic.DateMonthFormat);
                newRow.Cells["editMemberBirthYear"].Value = MessageContainerList.GetMessage(utilities.ExecutionContext, 4134); //"Edit DOB"

                dgvYourFamily.DataSource = customCustomerDTOBindingSource;  // control's DataSource.
                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;  // Bind the BindingSource to the DataGridView                
                dgvYourFamily.Columns["editRelativeBirthYear"].DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4134); // "Edit DOB"
                dOBDataGridViewTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                dgvMemberDetails.ReadOnly = true;
                this.dgvMemberDetails.ClearSelection();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error while frmSelectChild_Load(): " + ex.Message);
            }

            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Back pressed");
            StopKioskTimer();
            DialogResult = DialogResult.None;
            try
            {
                if (selectedChildList.Any())
                {
                    string screen = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "quantity");
                    bool isShowCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_CART_IN_KIOSK", false);
                    string msg = (isShowCartInKiosk == false) ? MessageContainerList.GetMessage(utilities.ExecutionContext, 4287, screen) //"This will abondon the transaction and take you back to the quantity screen"
                                    : MessageContainerList.GetMessage(utilities.ExecutionContext, 4850, screen) //"This action will remove the product from the Cart and take you back to the quantity screen"
                                    + Environment.NewLine
                                    + MessageContainerList.GetMessage(utilities.ExecutionContext, 4291); //"Are you sure you want to go back?"

                    using (frmYesNo frm = new frmYesNo(msg))
                    {
                        DialogResult dr = frm.ShowDialog();
                        if (dr != DialogResult.Yes)
                        {
                            log.LogMethodExit();
                            return;
                        }
                    }
                }

                DialogResult = DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error while dgvSelectAdult_Load(): " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            try
            {
                using (frmEnterChildDetails frm = new frmEnterChildDetails(kioskTransaction, childTrxLines, purchaseProductDTO, selectedChildList))
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
                    if (dresult == System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        if (frm.SelectedChildList != null && frm.SelectedChildList.Any())
                        {
                            this.selectedChildList = frm.SelectedChildList;
                            GetLinkedRelationsMatchingAgeCriteria();
                            customCustomerDTOBindingSource.DataSource = null;
                            customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
                            dgvYourFamily.SuspendLayout();
                            dgvYourFamily.Refresh();
                            dgvYourFamily.ResumeLayout();
                            EnableCheckboxForSelectedChild(selectedChildList);
                            UpdateCheckInGuestsCount();
                        }
                        else
                        {
                            DisableCheckboxForSelectedChild();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error while frmSelectChild_Load(): " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();

            try
            {
                if (selectedChildList.Count < childQtySelectedInQtyScreen)
                {
                    using (frmEnterChildDetails frm = new frmEnterChildDetails(kioskTransaction, childTrxLines, purchaseProductDTO, selectedChildList))
                    {
                        DialogResult dresult = frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                        if (dresult == System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            if (frm.SelectedChildList != null && frm.SelectedChildList.Any())
                            {
                                this.selectedChildList = frm.SelectedChildList;
                                GetLinkedRelationsMatchingAgeCriteria();
                                customCustomerDTOBindingSource.DataSource = null;
                                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
                                dgvYourFamily.SuspendLayout();
                                dgvYourFamily.Refresh();
                                dgvYourFamily.ResumeLayout();
                                EnableCheckboxForSelectedChild(selectedChildList);
                            }
                            else
                            {
                                DisableCheckboxForSelectedChild();
                            }
                        }
                        else
                        {
                            DialogResult = dresult;
                            Close();
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                else
                {
                    bool status = AttachCheckInDetailDTOToTrxLines();
                    if (status == false)
                    {
                        string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4468); //"Error: Failed to attach check-in details dto to the lines"
                        frmOKMsg.ShowUserMessage(errMsg);
                        return;
                    }

                    using (frmChildSummary frmcs = new frmChildSummary(kioskTransaction, childTrxLines, purchaseProductDTO))
                    {
                        DialogResult dresult = frmcs.ShowDialog();
                        kioskTransaction = frmcs.GetKioskTransaction;
                        if (dresult != System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            DialogResult = dresult;
                            Close();
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnProceed_Click() of select child screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void dgvMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                if (dgvMemberDetails.Columns[e.ColumnIndex].Name == "editMemberBirthYear")
                {
                    string text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Edit")
                        + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Member")
                        + " " + dgvMemberDetails.Columns["memberDateofBirth"].HeaderText;
                    bool status = EditBirthYear(parentCustomerDTO, text);
                    if (status == false)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    dgvMemberDetails.Rows[0].Cells["memberDateofBirth"].Value = parentCustomerDTO.DateOfBirth.Value.ToString(KioskStatic.DateMonthFormat);
                    UpdateCheckInGuestsCount();
                    this.Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error in dgvMember_CellContentClick() in child select screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvYourFamily_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                int index = dgvYourFamily.CurrentRow.Index;

                if (dgvYourFamily.Columns[e.ColumnIndex].Name == "editRelativeBirthYear")
                {
                    string text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Edit")
                        + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Family")
                        + " " + dgvYourFamily.Columns["dOBDataGridViewTextBoxColumn"].HeaderText;
                    bool status = EditBirthYear(customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO, text);
                    if (status == false)
                    {
                        log.LogMethodExit();
                        return;
                    }

                    dgvYourFamily.EndEdit();
                    dgvYourFamily.Refresh();
                    if (!IsAgeCriteriaMatches(customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO.DateOfBirth.ToString()))
                    {
                        (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        int removeIndex = selectedChildList.IndexOf(selectedChildList.Where(k => k.RelatedCustomerId == customCustomerDTOList[index].RelatedCustomerId).FirstOrDefault());
                        if (removeIndex > -1)
                        {
                            selectedChildList.RemoveAt(removeIndex);
                        }
                        dgvYourFamily.Rows[index].DefaultCellStyle.ForeColor = SystemColors.InactiveCaption;
                        dgvYourFamily.Rows[index].DefaultCellStyle.SelectionForeColor = SystemColors.InactiveCaption;
                    }
                    else
                    {
                        this.dgvYourFamily.Rows[index].DefaultCellStyle.SelectionBackColor = this.dgvYourFamily.DefaultCellStyle.BackColor;
                        this.dgvYourFamily.Rows[index].DefaultCellStyle.SelectionForeColor = this.dgvYourFamily.DefaultCellStyle.ForeColor;
                        txtMessage.Text = "";
                    }
                    UpdateCheckInGuestsCount();
                    log.LogMethodExit();
                    return;
                }

                Bitmap x = (System.Drawing.Bitmap)(dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value;
                Bitmap y = (System.Drawing.Bitmap)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                int editIndex = -1;
                int age = customCustomerDTOList[index].Age;

                if (CompareImages(x, y))
                {
                    (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                    editIndex = selectedChildList.IndexOf(selectedChildList.Where(k => k.RelatedCustomerId == customCustomerDTOList[index].RelatedCustomerId).FirstOrDefault());
                    if (editIndex > -1)
                    {
                        selectedChildList.RemoveAt(editIndex);
                        txtMessage.Text = "";
                    }
                }
                else
                {
                    if (selectedChildList.Count < childQtySelectedInQtyScreen)
                    {
                        if (IsAgeCriteriaMatches(customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO.DateOfBirth.ToString()))
                        {
                            (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                            selectedChildList.Add(customCustomerDTOList[index]);
                            txtMessage.Text = "";
                        }
                        else
                        {
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4288);
                            frmOKMsg.ShowUserMessage(errMsg);
                            txtMessage.Text = errMsg;
                            return;
                        }
                    }
                    else
                    {
                        (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        string warningMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4286, selectedChildList.Count, childQtySelectedInQtyScreen); //'Warning: You have already selected &1/&2.'
                        txtMessage.Text = warningMsg;
                        frmOKMsg.ShowUserMessage(warningMsg);
                    }
                }
                UpdateCheckInGuestsCount();
                this.Refresh();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while processing checkboxDataGridViewImageColumn of dgvSelectChild: " + ex.Message);
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

        private void dgvMemberDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvMemberDetails.Columns[e.ColumnIndex].Name == "CardNumberdataGridViewTextBoxColumn")
                {
                    if (e.Value != null)
                    {
                        e.Value = KioskHelper.GetMaskedCardNumber(e.Value.ToString());
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvLinkedRelations_CellFormatting : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private bool IsAgeCriteriaMatches(string doB)
        {
            log.LogMethodEntry(doB);
            ResetKioskTimer();
            bool status = false;
            try
            {
                decimal age = KioskHelper.GetAge(doB);
                if (age >= productsContainerDTO.AgeLowerLimit && age <= productsContainerDTO.AgeUpperLimit)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error in IsAgeCriteriaMatches() while getting linked relations: " + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private void ControlSkipAndEnableButtons()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                btnProceed.Enabled = selectedChildList.Any() ? true : false;
                btnSkip.Enabled = !btnProceed.Enabled;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in ControlSkipAndEnableeButtons of select adult screen:: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void EnableCheckboxForSelectedChild(List<CustomCustomerDTO> selectedList)
        {
            log.LogMethodEntry(selectedList);
            StopKioskTimer();
            try
            {
                foreach (CustomCustomerDTO child in selectedList)
                {
                    int index = -1;
                    index = customCustomerDTOList.IndexOf(customCustomerDTOList.Where(k => k.RelatedCustomerId == child.RelatedCustomerId).FirstOrDefault());
                    if (index > -1)
                    {
                        (dgvYourFamily.Rows[index].Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error while EnableCheckboxForNewlyLinkedChild() in select child screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void DisableCheckboxForSelectedChild()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            try
            {
                for (int i = 0; i < customCustomerDTOList.Count; i++)
                {
                    (dgvYourFamily.Rows[i].Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error while EnableCheckboxForNewlyLinkedChild() in select child screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private bool AttachCheckInDetailDTOToTrxLines()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            bool status = true;
            try
            {
                for (int i = 0; i < selectedChildList.Count; i++)
                {
                    CustomCheckInDetailDTO childCheckInDetailDTO = new CustomCheckInDetailDTO();
                    childCheckInDetailDTO.Name = selectedChildList[i].RelatedCustomerName;
                    childCheckInDetailDTO.DateOfBirth = selectedChildList[i].DOB;
                    childCheckInDetailDTO.Age = selectedChildList[i].Age;
                    childCheckInDetailDTO.CustomerId = selectedChildList[i].RelatedCustomerId;
                    childTrxLines[i].LineCheckInDetailDTO = childCheckInDetailDTO.CheckInDetailDTO;
                }
            }
            catch (Exception ex)
            {
                status = false;
                log.Error(ex);
                KioskStatic.logToFile("Error in AttachCheckInDetailDTOToTrxLines() of select child screen: " + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private List<CustomerRelationshipDTO> GetLinkedRelationsMatchingAgeCriteria()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            List<CustomerRelationshipDTO> childDTOList = null;
            try
            {
                customCustomerDTOList.Clear(); ;
                childDTOList = KioskHelper.GetLinkedRelationsFilteredByAge(purchaseProductDTO.ParentCustomerId, productsContainerDTO.AgeLowerLimit, productsContainerDTO.AgeUpperLimit);
                if (childDTOList != null && childDTOList.Any())
                {
                    foreach (CustomerRelationshipDTO customerRelationshipDTO in childDTOList)
                    {
                        CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(customerRelationshipDTO);
                        customCustomerDTOList.Add(customCustomerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error in GetLinkedRelationsMatchingAgeCriteria(): " + ex.Message);
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit(childDTOList);
            return childDTOList;
        }

        private bool EditBirthYear(CustomerDTO customerDTO, string controlText)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            bool status = false;
            try
            {
                bool allowEditBirthDayAndMonth = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ALLOW_EDIT_BIRTH_DAY_AND_MONTH", true);
                if (allowEditBirthDayAndMonth == false)
                {
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(utilities.ExecutionContext, 5159)); //'Please note, you are allowed to edit year only'
                }
                DateTime doB = (customerDTO.DateOfBirth == null) ? DateTime.MinValue : Convert.ToDateTime(customerDTO.DateOfBirth);
                DateTime newDOB = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: (allowEditBirthDayAndMonth && KioskStatic.enableDaySelection),
                    enableMonthSelection: (allowEditBirthDayAndMonth && KioskStatic.enableMonthSelection), enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                if (newDOB != null && newDOB != DateTime.MinValue)
                {
                    customerDTO.DateOfBirth = newDOB;
                }

                //Save Customer
                SqlConnection sqlConnection = KioskStatic.Utilities.createConnection();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, customerDTO);
                    customerBL.Save(sqlTransaction);
                    sqlTransaction.Commit();
                    status = true;
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    log.Error(ex);
                    string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4800); //Unable to change date of birth
                    frmOKMsg.ShowUserMessage(errMsg);
                    KioskStatic.logToFile("Error in EditDateOfBirth() of Select child: " + errMsg + ex.Message);
                    customerDTO.DateOfBirth = doB;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile("Error in EditDateOfBirth() of Select child: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
            return status;
        }

        private void UpdateCheckInGuestsCount()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, selectedChildList.Count, childQtySelectedInQtyScreen); //"Quantity:&1"
                btnProceed.Enabled = selectedChildList.Any() ? true : false;
                btnSkip.Enabled = !btnProceed.Enabled;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateCheckInGuestsCount() in child screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private static bool CompareImages(Bitmap image1, Bitmap image2)
        {
            log.LogMethodEntry(image1, image2);

            try
            {
                if (image1.Width == image2.Width && image1.Height == image2.Height)
                {
                    for (int i = 0; i < image1.Width; i++)
                    {
                        for (int j = 0; j < image1.Height; j++)
                        {
                            if (image1.GetPixel(i, j) != image2.GetPixel(i, j))
                            {
                                log.LogMethodExit("false");
                                return false;
                            }
                        }
                    }
                    log.LogMethodExit("true");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in CompareImages() of select child screen: " + ex);
            }
            log.LogMethodExit("false");
            return false;
        }

        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
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
                DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 457));
                Application.DoEvents();
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                dgvMemberDetails.ColumnHeadersDefaultCellStyle.Font =
                    dgvYourFamily.ColumnHeadersDefaultCellStyle.Font = new Font(lblGreeting.Font.FontFamily, 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                dgvMemberDetails.DefaultCellStyle.Font =
                    dgvYourFamily.DefaultCellStyle.Font =
                    dgvMemberDetails.RowHeadersDefaultCellStyle.Font =
                    dgvYourFamily.RowHeadersDefaultCellStyle.Font =
                    dgvMemberDetails.RowsDefaultCellStyle.Font =
                    dgvYourFamily.RowsDefaultCellStyle.Font =
                    dgvMemberDetails.RowTemplate.DefaultCellStyle.Font =
                    dgvYourFamily.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

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
                lblMemberDetails.Font = new Font(lblGreeting.Font.FontFamily, 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblYourFamily.Font = new Font(lblGreeting.Font.FontFamily, 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                this.dgvMemberDetails.DefaultCellStyle.SelectionBackColor = SystemColors.Window;
                this.dgvMemberDetails.DefaultCellStyle.SelectionForeColor = SystemColors.WindowText;
                this.dgvYourFamily.DefaultCellStyle.SelectionBackColor = this.dgvYourFamily.DefaultCellStyle.BackColor;
                this.dgvYourFamily.DefaultCellStyle.SelectionForeColor = this.dgvYourFamily.DefaultCellStyle.ForeColor;

                memberDateofBirth.DefaultCellStyle = KioskHelper.gridViewDateOfBirthCellStyle();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFont() of select child screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for frmSelectChild");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.SelectChildBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.panelMemberDetails.BackgroundImage = this.panelRelations.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.btnPrev.BackgroundImage = btnSkip.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmSelectChild", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmSelectChild: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.SelectChildGreetingLblTxtForeColor;
                this.lblMemberDetails.ForeColor = KioskStatic.CurrentTheme.SelectChildMemberDetailsLblTextForeColor;
                this.lblYourFamily.ForeColor = KioskStatic.CurrentTheme.SelectChildYourfamilyLblTextForeColor;
                this.dgvYourFamily.ForeColor = KioskStatic.CurrentTheme.SelectChildYourFamilyGridTextForeColor;
                this.dgvMemberDetails.ForeColor = KioskStatic.CurrentTheme.SelectChildMemberDetailsGridTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.SelectChildFooterTxtMsgTextForeColor;//Footer text message
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.SelectChildProceedButtonTextForeColor;
                this.btnSkip.ForeColor = KioskStatic.CurrentTheme.SelectChildSkipButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.SelectChildBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.SelectChildHomeButtonTextForeColor;
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
                //this.verticalScrollBarViewMemberDetails.InitializeScrollBar(KioskStatic.CurrentTheme.ScrollDownEnabled, KioskStatic.CurrentTheme.ScrollDownDisabled, KioskStatic.CurrentTheme.ScrollUpEnabled, KioskStatic.CurrentTheme.ScrollUpDisabled);
                this.bigVerticalScrollViewFamily.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);

                this.dgvYourFamily.ColumnHeadersDefaultCellStyle.ForeColor =
                    this.dgvMemberDetails.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.SelectChildGridHeaderTextForeColor;

                this.dgvYourFamily.RowsDefaultCellStyle.ForeColor =
                    this.dgvYourFamily.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectChildYourFamilyGridInfoTextForeColor;

                this.dgvMemberDetails.RowsDefaultCellStyle.ForeColor =
                    this.dgvMemberDetails.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectChildMemberDetailsGridInfoTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSelectChild: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public class ChildRelationsNotExistException : Exception
        {
            public ChildRelationsNotExistException()
            {
            }

            public ChildRelationsNotExistException(string message) : base(message)
            {
            }

            public ChildRelationsNotExistException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ChildRelationsNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        private void frmSelectChild_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //currentTrx = null;
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmSelectChild_FormClosed()", ex);
            }
            //Cursor.Hide();

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}


























