/********************************************************************************************
 * Project Name - Reservation
 * Description  - Product TrxLine Details form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Created for Booking phase 2 enhancement changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Reservation
{
    public partial class frmProductTrxLineDetails : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ComboProductDTO comboProductDTO;
        private List<Transaction.TransactionLine> trxLineList;
        private string bookingStatus;
        private Utilities utilities;
        private decimal pageNo = 1;
        private int pageSize = 15;
        private int trxLineCount = 0;
        private int noOfPages = 1;
        private bool userAction = true;

        internal delegate int GetTrxLineIndexDelegate(Transaction.TransactionLine trxLine);
        internal GetTrxLineIndexDelegate GetTrxLineIndex;
        internal delegate void EditModifiersDelegate(List<Transaction.TransactionLine> selectedTrxLineWithModifiers);
        internal EditModifiersDelegate EditModifiers;
        internal delegate List<Transaction.TransactionLine> RefreshTransactionLinesDelegate(int productId, int comboProductId);
        internal RefreshTransactionLinesDelegate RefreshTransactionLines;

        internal delegate void RescheduleAttractionDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal RescheduleAttractionDelegate RescheduleAttraction;
        internal delegate void RescheduleAttractionGroupDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal RescheduleAttractionGroupDelegate RescheduleAttractionGroup;
        internal delegate void CancelProductLineDelegate(Transaction.TransactionLine trxLineItem);
        internal CancelProductLineDelegate CancelProductLine;
        internal delegate void ChangePriceDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal ChangePriceDelegate ChangePrice;
        internal delegate void ResetPriceDelegate(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId);
        internal ResetPriceDelegate ResetPrice;
        public frmProductTrxLineDetails()
        {
            log.LogMethodEntry();
            utilities = POSStatic.Utilities;
            InitializeComponent();
            pageNo = 1;
            trxLineCount = 0;
            noOfPages = 1;
            userAction = true;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public frmProductTrxLineDetails(ComboProductDTO comboProductDTO, List<Transaction.TransactionLine> trxLineList, string bookingStatus)
        {
            log.LogMethodEntry(comboProductDTO, trxLineList);
            utilities = POSStatic.Utilities;
            this.comboProductDTO = comboProductDTO;
            this.trxLineList = trxLineList;
            this.bookingStatus = bookingStatus;
            InitializeComponent();
            utilities.setLanguage();
            pageNo = 1;
            trxLineCount = 0;
            userAction = true;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadTrxLineDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            if (comboProductDTO != null && trxLineList != null && trxLineList.Count > 0)
            {
                txtPageSize.Text = pageSize.ToString();
                decimal totalPage = GetTotalPageCOunt();
                noOfPages = (int)totalPage;
                lblTotalCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2119, noOfPages.ToString());//"OF " + noOfPages.ToString())
                if (pageNo >= noOfPages)
                {
                    pageNo = noOfPages;
                }
                txtPageNo.Text = pageNo.ToString();
                ClearPnlProducts();
                trxLineCount = trxLineList.Count;
                bool inEditMode = false;
                if (bookingStatus == ReservationDTO.ReservationStatus.BLOCKED.ToString() || bookingStatus == ReservationDTO.ReservationStatus.WIP.ToString()
                    || bookingStatus == ReservationDTO.ReservationStatus.NEW.ToString())
                {
                    inEditMode = true;
                }

                int locationX = this.usrCtrlProductTrxLineDetails1.Location.X;
                int locationY = this.usrCtrlProductTrxLineDetails1.Location.Y;
                this.usrCtrlProductTrxLineDetails1.Visible = false;
                int trxLineCounter = 0;
                List<Transaction.TransactionLine> pageLineList = trxLineList.Skip(((int)pageNo - 1) * pageSize).Take(pageSize).ToList();
                foreach (Transaction.TransactionLine trxLineItem in pageLineList)
                {
                    POSUtils.SetLastActivityDateTime();
                    int lineIndex = GetTrxLineIndex(trxLineItem);
                    int parentLineIndex = -1;
                    if (trxLineItem.ParentLine != null)
                    {
                        parentLineIndex = GetTrxLineIndex(trxLineItem.ParentLine);
                    }
                    usrCtrlProductTrxLineDetails usrCtrlProductTrxLineDetail = new usrCtrlProductTrxLineDetails(comboProductDTO, trxLineItem, parentLineIndex, lineIndex);
                    usrCtrlProductTrxLineDetail.Name = "usrCtrlProductTrxLineDetails0" + trxLineCounter.ToString();
                    usrCtrlProductTrxLineDetail.Location = new Point(locationX, locationY + trxLineCounter * 36);
                    pnlTrxLineDetails.Controls.Add(usrCtrlProductTrxLineDetail);
                    usrCtrlProductTrxLineDetail.Enabled = (inEditMode == true);
                    usrCtrlProductTrxLineDetail.RescheduleAttraction += new usrCtrlProductTrxLineDetails.RescheduleAttractionDelegate(RescheduleAttractionUsrCtl);
                    usrCtrlProductTrxLineDetail.RescheduleAttractionGroup += new usrCtrlProductTrxLineDetails.RescheduleAttractionGroupDelegate(RescheduleAttractionGroupUsrCtl);
                    usrCtrlProductTrxLineDetail.CancelProductLine += new usrCtrlProductTrxLineDetails.CancelProductLineDelegate(CancelProductLineUsrCtl);
                    usrCtrlProductTrxLineDetail.ChangePrice += new usrCtrlProductTrxLineDetails.ChangePriceDelegate(ChangePriceUsrCtl);
                    usrCtrlProductTrxLineDetail.ResetPrice += new usrCtrlProductTrxLineDetails.ResetPriceDelegate(ResetPriceUsrCtl);
                    trxLineCounter++;
                }
                if (inEditMode)
                {
                    btnEditModifiers.Enabled = true;
                }
                else
                {
                    btnEditModifiers.Enabled = false;
                }
            }
            else
            {
                txtPageSize.Text = pageSize.ToString();
                decimal totalPage = GetTotalPageCOunt();
                noOfPages = (int)totalPage;
                lblTotalCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2119, noOfPages.ToString());//"OF " + noOfPages.ToString())
                if (pageNo >= noOfPages)
                {
                    pageNo = noOfPages;
                }
                txtPageNo.Text = pageNo.ToString();
                ClearPnlProducts();
            }
            EnablePageButtons();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnEditModifiers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            txtMessage.Clear();
            try
            {
                POSUtils.SetLastActivityDateTime();
                List<Transaction.TransactionLine> selectedTrxLineWithModifiers = new List<Transaction.TransactionLine>();
                foreach (Control panelControl in pnlTrxLineDetails.Controls)
                {
                    if (panelControl.Name.StartsWith("usrCtrlProductTrxLineDetails0"))
                    {
                        usrCtrlProductTrxLineDetails pnlCtrlObj = (usrCtrlProductTrxLineDetails)panelControl;
                        if (pnlCtrlObj.CbxSelectForEdit)
                        {
                            selectedTrxLineWithModifiers.Add(pnlCtrlObj.ProductTrxLine);
                        }
                    }
                }
                if (selectedTrxLineWithModifiers.Count > 0)
                {
                    EditModifiers(selectedTrxLineWithModifiers);
                    this.trxLineList = RefreshTransactionLines(comboProductDTO.ChildProductId, comboProductDTO.ComboProductId);
                    decimal totalPage = GetTotalPageCOunt();
                    if (pageNo != totalPage)
                    {
                        pageNo = totalPage;
                    }
                    LoadTrxLineDetails();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ClearPnlProducts()
        {
            log.LogMethodEntry();
            for (int i = 0; i < pnlTrxLineDetails.Controls.Count; i++)
            {
                Control controlItem = pnlTrxLineDetails.Controls[i];
                if (controlItem.Name.StartsWith("usrCtrlProductTrxLineDetails0"))
                {
                    pnlTrxLineDetails.Controls.RemoveAt(i);
                    i = 0;
                }
            }
            log.LogMethodExit();
        }

        private void frmProductTrxLineDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadTrxLineDetails();
            //InitializeFlowLayoutHorizontalScroll(pnlTrxLineDetails, 1, trxLineCount);
            lblPageNo.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "PAGE");
            utilities.setLanguage(this);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmProductTrxLineDetails_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Parafait_POS.POSUtils.AttachFormEvents();
            log.LogMethodExit();
        }

        private void btnEditModifiers_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnEditModifiers_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        //private void DisplayScrollButtons()
        //{
        //    log.LogMethodEntry();
        //    trxLineScrollIndex = 1;

        //    pnlTrxLineDetails.VerticalScroll.Value = trxLineScrollIndex;
        //    pnlTrxLineDetails.Refresh();
        //   // btnLeft.Visible = btnRight.Visible = false;
        //    //if (pnlTrxLineDetails.Controls.Count != 0)
        //    if (trxLineCount != 0)
        //    {
        //        //if (pnlTrxLineDetails.Controls.Count <= 9) 
        //        if (trxLineCount <= 9)
        //        {
        //            btnRight.Visible = false;
        //        }
        //        else
        //        {
        //            btnRight.Visible = true;
        //        }
        //    }
        //    log.LogMethodExit();
        //}


        private void BtnLeft_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            userAction = false;
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (pageNo > 1)
                {
                    pageNo -= 1;
                }
                else
                {
                    pageNo = 1;
                }
                txtPageNo.Text = pageNo.ToString();
                this.Cursor = Cursors.WaitCursor;
                LoadTrxLineDetails();
                btnClose.Focus();
                btnLeft.Focus();
                //trxLineScrollIndex = FlowLayoutScrollLeft(pnlTrxLineDetails, trxLineScrollIndex); 
                this.Cursor = Cursors.Default;
            }
            finally
            {
                userAction = true;
            }
            log.LogMethodExit();
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            userAction = false;
            try
            {
                POSUtils.SetLastActivityDateTime();
                pageNo += 1;
                if (pageNo >= noOfPages)
                {
                    pageNo = noOfPages;
                }
                this.Cursor = Cursors.WaitCursor;
                LoadTrxLineDetails();
                btnClose.Focus();
                btnRight.Focus();
                //trxLineScrollIndex = FlowLayoutScrollRight(pnlTrxLineDetails, trxLineScrollIndex); 
                this.Cursor = Cursors.Default;
            }
            finally
            {
                userAction = true;
            }
            log.LogMethodExit();
        }

        void EnablePageButtons()
        {
            log.LogMethodEntry();
            //if (trxLineCount > (pnlTrxLineDetails.Height / (pnlTrxLineDetails.Controls[0].Height + pnlTrxLineDetails.Controls[0].Margin.Top + pnlTrxLineDetails.Controls[0].Margin.Bottom)))
            //{
            //    if (trxLineScrollIndex == 1)
            //    {
            //       btnLeft.Visible = false;
            //    }
            //    else
            //    {
            //       btnLeft.Visible = true;
            //    }
            //    if (((pnlTrxLineDetails.VerticalScroll.Maximum - pnlTrxLineDetails.VerticalScroll.SmallChange) <= trxLineScrollIndex)
            //        || ((double)trxLineCount / (double)pageNo <= 9)
            //        )
            //    {
            //       btnRight.Visible = false;
            //    }
            //    else
            //    {
            //       btnRight.Visible = true;
            //    }
            //}
            if (pageNo == noOfPages)
            {
                btnRight.Enabled = false;
            }
            if (pageNo == 1)
            {
                btnLeft.Enabled = false;
            }
            if (pageNo < noOfPages)
            {
                btnRight.Enabled = true;
            }
            if (pageNo > 1)
            {
                btnLeft.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void txtPageSize_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (userAction)
            {
                POSUtils.SetLastActivityDateTime();
                int pageSizeEntered = (int)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(utilities.ExecutionContext, 2120), txtPageSize.Text, utilities);// "Please enter page size [10 to 100]"
                if (pageSizeEntered >= 10 && pageSizeEntered <= 100 && pageSizeEntered != pageSize)
                {
                    pageSize = pageSizeEntered;
                    LoadTrxLineDetails();
                }
                btnClose.Focus();
                btnRight.Focus();
            }
            log.LogMethodExit();
        }

        private void txtPageNo_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (userAction)
            {
                POSUtils.SetLastActivityDateTime();
                int pageNoEntered = (int)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(utilities.ExecutionContext, 2121), txtPageNo.Text, utilities);//"Please enter page number"
                if (pageNoEntered > 0 && pageNoEntered <= noOfPages && pageNoEntered != pageNo)
                {
                    pageNo = pageNoEntered;
                    LoadTrxLineDetails();
                }
                btnClose.Focus();
                btnRight.Focus();
            }
            log.LogMethodExit();
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_Forward_Btn_Hover;
            log.LogMethodExit();
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_Forward_Btn;
            log.LogMethodExit();
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_Backward_Btn_Hover;
            log.LogMethodExit();
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.R_Backward_Btn;
            log.LogMethodExit();
        }

        private void RescheduleAttractionGroupUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                RescheduleAttractionGroup(productTrxLine, lineIndex, comboProductId); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            RefreshAndLoadTrxLines();
            log.LogMethodExit();
        }

        private void RescheduleAttractionUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                RescheduleAttraction(productTrxLine, lineIndex, comboProductId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            RefreshAndLoadTrxLines();
            log.LogMethodExit();
        }
        

        private void CancelProductLineUsrCtl(Transaction.TransactionLine productTrxLine)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                CancelProductLine(productTrxLine); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            RefreshAndLoadTrxLines();
            log.LogMethodExit();
        }

        private void RefreshAndLoadTrxLines()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.trxLineList = RefreshTransactionLines(comboProductDTO.ChildProductId, comboProductDTO.ComboProductId);
                decimal totalPage = GetTotalPageCOunt();
                if (pageNo != totalPage)
                {
                    pageNo = totalPage;
                }
                LoadTrxLineDetails();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private decimal GetTotalPageCOunt()
        {
            log.LogMethodEntry();
            int lineCount = 1;
            if (trxLineList != null && trxLineList.Count > 1)
            {
                lineCount = trxLineList.Count;
            }
            decimal totalPage = Math.Ceiling((decimal)lineCount / pageSize);
            log.LogMethodExit(totalPage);
            return totalPage;
        }

        private void ChangePriceUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                ChangePrice(productTrxLine, lineIndex, comboProductId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            RefreshAndLoadTrxLines();
            log.LogMethodExit();
        }


        private void ResetPriceUsrCtl(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                ResetPrice(productTrxLine, lineIndex, comboProductId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            RefreshAndLoadTrxLines();
            log.LogMethodExit();
        }

        private void Scroll_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        //private static void InitializeFlowLayoutHorizontalScroll(Panel flp, int Number_Of_Rows, int controlCount)
        //{
        //    log.LogMethodEntry(flp, Number_Of_Rows, controlCount);
        //    if (flp.Controls.Count > 0)
        //    {
        //        flp.VerticalScroll.Maximum = (Math.Max(controlCount, Number_Of_Rows) * (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom) + 1);
        //        flp.VerticalScroll.SmallChange = flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom;
        //        flp.VerticalScroll.LargeChange = (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom) * 3;
        //    }
        //    log.LogMethodExit();
        //}


        //private static int FlowLayoutScrollRight(Panel flp, int index)
        //{
        //    log.LogMethodEntry(flp, index);
        //    if (flp.Controls.Count != 0 && flp.Controls.Count > (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))
        //    {
        //        if (index <= 0)
        //        {
        //            index = 1;
        //        }
        //        else
        //        {
        //            index += flp.VerticalScroll.LargeChange;
        //        }
        //        if (index > (flp.VerticalScroll.Maximum - flp.VerticalScroll.LargeChange))
        //        {
        //            index = flp.VerticalScroll.Maximum - flp.VerticalScroll.Minimum;
        //        }

        //        flp.VerticalScroll.Value = index;
        //        flp.Refresh();

        //    }
        //    log.LogMethodExit(index);
        //    return index;
        //}
        //private static int FlowLayoutScrollLeft(Panel flp, int index)
        //{
        //    log.LogMethodEntry(flp, index);
        //    if (flp.Controls.Count > (flp.Height / (flp.Controls[0].Height + flp.Controls[0].Margin.Top + flp.Controls[0].Margin.Bottom)))
        //    {
        //        index -= flp.VerticalScroll.LargeChange;
        //        if (index <= (flp.VerticalScroll.SmallChange + 1))
        //        {
        //            index = 1;
        //        }

        //        flp.VerticalScroll.Value = index;
        //        flp.Refresh();
        //    }
        //    log.LogMethodExit(index);
        //    return index;
        //}
    }
}
