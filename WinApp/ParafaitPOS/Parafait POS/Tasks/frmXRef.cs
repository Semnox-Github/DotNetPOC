using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmXRef : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utils;
        private LegacyCardDTO legacyCardDTO;
        public frmXRef(Utilities utils)
        {
            log.LogMethodEntry(utils);
            this.utils = utils;
            InitializeComponent();
            CurrentActiveTextBox = txtMCASHCardNumber;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LegacyCardListBL legacyCardListBL = new LegacyCardListBL(POSStatic.Utilities.ExecutionContext);
            DataTable dt = legacyCardListBL.GetLegacyCardsDTOList(txtMCASHCardNumber.Text, txtParafaitCardNumber.Text);
            dgvXRef.DataSource = dt;
            dgvXRef.Refresh();
            log.LogMethodExit();
        }

        private void XRef_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvXRef.BackgroundColor = this.BackColor;
            btnSearch.PerformClick();
            txtMCASHCardNumber.Focus();
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMCASHCardNumber.Text = "%";
            txtParafaitCardNumber.Text = "";
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CurrentActiveTextBox = sender as TextBox;
            if (keypad != null && !keypad.IsDisposed)
            {
                keypad.currentTextBox = CurrentActiveTextBox;
            }
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox);
                keypad.currentTextBox = CurrentActiveTextBox;
                keypad.Location = new Point((this.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                keypad.Show();
            }
            //else if (keypad.Visible)
            //    keypad.Hide();
            else
            {
                keypad.Show();
            }
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        public TextBox CurrentActiveTextBox;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if ((sender as TextBox).Enabled)
            //    CurrentActiveTextBox = sender as TextBox;
            if (keypad == null)
            {
                keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox);
            }
            if (keypad != null && !keypad.IsDisposed)
            {
                keypad.currentTextBox = CurrentActiveTextBox;
                keypad.Location = new Point((this.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                //if (keypad.Visible)
                //{
                //    keypad.Hide();
                //}
                //else
                //{
                keypad.Show();
            }
            //}
            log.LogMethodExit();
        }

        private void btnShowCredits_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.dgvXRef.SelectedRows.Count > 0)
                {
                    DataGridViewRow legacyCardDataGridViewRow = this.dgvXRef.SelectedRows[0];
                    if (!string.IsNullOrWhiteSpace(legacyCardDataGridViewRow.Cells[0].Value.ToString()))
                    {
                        //LegacyCardDTO legacyCardDTO = GetLegacyCardDTO(legacyCardDataGridViewRow.Cells[0].Value.ToString());
                        if (this.legacyCardDTO.LegacyCardCreditPlusDTOList != null && this.legacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                        {
                            frmLegacyEntitlements frmLegacyEntitlements = new frmLegacyEntitlements(POSStatic.Utilities.ExecutionContext, true, legacyCardDTO, "CREDITS");
                            frmLegacyEntitlements.ShowDialog();
                            legacyCardDTO = (LegacyCardDTO)frmLegacyEntitlements.Tag;
                        }
                        else
                        {
                            MessageBox.Show(utils.MessageUtils.getMessage(2945), "Credits");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utils.MessageUtils.getMessage(2945), "Credits");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }



        private void btnShowGames_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.dgvXRef.SelectedRows.Count > 0)
                {
                    DataGridViewRow legacyCardDataGridViewRow = this.dgvXRef.SelectedRows[0];
                    if (!string.IsNullOrWhiteSpace(legacyCardDataGridViewRow.Cells[0].Value.ToString()))
                    {
                        //LegacyCardDTO legacyCardDTO = GetLegacyCardDTO(legacyCardDataGridViewRow.Cells[0].Value.ToString());
                        if (this.legacyCardDTO.LegacyCardGamesDTOsList != null && this.legacyCardDTO.LegacyCardGamesDTOsList.Any())
                        {
                            frmLegacyEntitlements frmLegacyEntitlements = new frmLegacyEntitlements(POSStatic.Utilities.ExecutionContext, true, legacyCardDTO, "GAMES");
                            frmLegacyEntitlements.ShowDialog();
                            legacyCardDTO = (LegacyCardDTO)frmLegacyEntitlements.Tag;
                        }
                        else
                        {
                            MessageBox.Show(utils.MessageUtils.getMessage(2945), "Games");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utils.MessageUtils.getMessage(2945), "Games");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnShowDiscounts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.dgvXRef.SelectedRows.Count > 0)
                {
                    DataGridViewRow legacyCardDataGridViewRow = this.dgvXRef.SelectedRows[0];
                    if (!string.IsNullOrWhiteSpace(legacyCardDataGridViewRow.Cells[0].Value.ToString()))
                    {
                        //LegacyCardDTO legacyCardDTO = GetLegacyCardDTO(legacyCardDataGridViewRow.Cells[0].Value.ToString());
                        if (this.legacyCardDTO.LegacyCardDiscountsDTOList != null && this.legacyCardDTO.LegacyCardDiscountsDTOList.Any())
                        {
                            frmLegacyEntitlements frmLegacyEntitlements = new frmLegacyEntitlements(POSStatic.Utilities.ExecutionContext, true, legacyCardDTO, "DISCOUNT");
                            frmLegacyEntitlements.ShowDialog();
                            legacyCardDTO = (LegacyCardDTO)frmLegacyEntitlements.Tag;
                        }
                        else
                        {
                            MessageBox.Show(utils.MessageUtils.getMessage(2945), "Discount");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utils.MessageUtils.getMessage(2945), "Discount");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private LegacyCardDTO GetLegacyCardDTO(string cardNumber)
        {
            log.LogMethodEntry();
            LegacyCardDTO legacyCardDTO = null;
            List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>> SearchByParameters = new List<KeyValuePair<LegacyCardDTO.SearchByParameters, string>>();
            SearchByParameters.Add(new KeyValuePair<LegacyCardDTO.SearchByParameters, string>(LegacyCardDTO.SearchByParameters.CARD_NUMBER, cardNumber));
            List<LegacyCardDTO> LegacyCardDTOList = new List<LegacyCardDTO>();
            LegacyCardListBL legacyCardListBL = new LegacyCardListBL(POSStatic.Utilities.ExecutionContext, LegacyCardDTOList, null);
            LegacyCardDTOList = legacyCardListBL.GetLegacyCardDTOList(SearchByParameters);
            if (LegacyCardDTOList != null && LegacyCardDTOList.Count > 0)
            {
                legacyCardDTO = LegacyCardDTOList.FirstOrDefault();
            }
            log.LogMethodExit();
            return legacyCardDTO;
        }

        private void dgvXRef_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnShowCredits.Enabled = false;
            btnShowGames.Enabled = false;
            btnShowDiscounts.Enabled = false;
            if (this.dgvXRef.SelectedRows.Count > 0)
            {
                DataGridViewRow legacyCardDataGridViewRow = this.dgvXRef.SelectedRows[0];
                if (!String.IsNullOrEmpty(legacyCardDataGridViewRow.Cells[0].Value.ToString()))
                {
                    this.legacyCardDTO = GetLegacyCardDTO(legacyCardDataGridViewRow.Cells[0].Value.ToString());
                    if (legacyCardDTO.LegacyCardCreditPlusDTOList != null && legacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                    {
                        btnShowCredits.Enabled = true;
                    }
                    else
                    {
                        btnShowCredits.Enabled = false;
                    }
                    if (legacyCardDTO.LegacyCardGamesDTOsList != null && legacyCardDTO.LegacyCardGamesDTOsList.Any())
                    {
                        btnShowGames.Enabled = true;
                    }
                    else
                    {
                        btnShowGames.Enabled = false;
                    }
                    if (legacyCardDTO.LegacyCardDiscountsDTOList != null && legacyCardDTO.LegacyCardDiscountsDTOList.Any())
                    {
                        btnShowDiscounts.Enabled = true;
                    }
                    else
                    {
                        btnShowDiscounts.Enabled = false;
                    }
                }
                log.LogMethodExit();
            }
        }
    }
}

