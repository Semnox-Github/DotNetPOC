/********************************************************************************************
 * Project Name - frmLegacyEntitlements
 * Description  - frmLegacyEntitlements class
 * 
 **************
 **Version Log
 **************
 *Version          Date             Modified By         Remarks          
 *********************************************************************************************
 * 2.130.4        22-02-2022       Dakshakh raj        frm Legacy Entitlements
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmLegacyEntitlements : Form
    {
        private bool readOnly;
        LegacyCardDTO legacyCardDTO;
        private string entitlementType;
        private ExecutionContext executionContext;
        DataGridView dgvLegacyCardGames;
        DataGridView dgvLegacyCardDiscounts;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmLegacyEntitlements(ExecutionContext executionContext, bool readOnly, LegacyCardDTO legacyCardDTO, string entitlementType)
        {
            log.LogMethodEntry();
            this.readOnly = readOnly;
            this.legacyCardDTO = legacyCardDTO;
            this.executionContext = executionContext;
            this.entitlementType = entitlementType;
            this.Tag = legacyCardDTO;
            InitializeComponent();
            createDGVLegacyCardGames();
            createDGVLegacyCardDiscounts();
            log.LogMethodExit();
        }

        private void createDGVLegacyCardGames()
        {
            log.LogMethodEntry();
            dgvLegacyCardGames = new DataGridView();
            dgvLegacyCardGames.SelectionChanged += new System.EventHandler(this.dgvLegacyCardGames_SelectionChanged);
            this.Controls.Add(dgvLegacyCardGames);
            dgvLegacyCardGames.Location = dgvLegacyEntitlements.Location;
            dgvLegacyCardGames.Size = dgvLegacyEntitlements.Size;
            this.dgvLegacyCardGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            log.LogMethodExit();
        }
        private void createDGVLegacyCardDiscounts()
        {
            log.LogMethodEntry();
            dgvLegacyCardDiscounts = new DataGridView();
            this.Controls.Add(dgvLegacyCardDiscounts);
            dgvLegacyCardDiscounts.Location = dgvLegacyEntitlements.Location;
            dgvLegacyCardDiscounts.Size = dgvLegacyEntitlements.Size;

            log.LogMethodExit();
        }

        private void frmLegacyEntitlements_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadGridData();
            log.LogMethodExit();
        }

        public void LoadGridData()
        {
            log.LogMethodEntry();
            List<LegacyCardCreditPlusDTO> legacyCardDTOList;
            btnOk.Visible = !readOnly;
            if (readOnly == true && entitlementType.Equals("CREDITS"))
            {
                AttachListToDataSource(legacyCardDTO.LegacyCardCreditPlusDTOList);
                AttachCreditPlusConsumption(legacyCardDTO.LegacyCardCreditPlusDTOList[0].LegacyCardCreditPlusConsumptionDTOList);
                DGVChildLines.Visible = true;
                DGVChildLines.ReadOnly = true;
                lblChildHeader.Visible = true;
            }
            if (readOnly == true && entitlementType.Equals("GAMES"))
            {
                AttachListToGameDataSource(legacyCardDTO.LegacyCardGamesDTOsList);
                AttachListToGameExtendedDataSource(legacyCardDTO.LegacyCardGamesDTOsList[0].LegacyCardGameExtendedDTOList);
                lblChildHeader.Visible = true;
                DGVChildLines.Visible = true;
                DGVChildLines.ReadOnly = true;
            }
            if (readOnly == true && entitlementType.Equals("DISCOUNT"))
            {
                AttachListToDiscountDataSource(legacyCardDTO.LegacyCardDiscountsDTOList);
            }
            if (readOnly == false && entitlementType.Equals("CREDITS"))
            {
                legacyCardDTOList = legacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.CARD_BALANCE) || l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.GAME_PLAY_CREDIT) || l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.COUNTER_ITEM));
                AttachListToDataSource(legacyCardDTOList);
                DGVChildLines.Visible = false;
                btnClose.Visible = false;
                lblChildHeader.Visible = false;
            }
            else if (readOnly == false && entitlementType.Equals("BONUS"))
            {
                legacyCardDTOList = legacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.GAME_PLAY_BONUS));
                AttachListToDataSource(legacyCardDTOList);
                DGVChildLines.Visible = false;
                btnClose.Visible = false;
                lblChildHeader.Visible = false;
            }
            else if (readOnly == false && entitlementType.Equals("TICKETS"))
            {
                legacyCardDTOList = legacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.TICKET));
                AttachListToDataSource(legacyCardDTOList);
                DGVChildLines.Visible = false;
                btnClose.Visible = false;
                lblChildHeader.Visible = false;
            }
            else if (readOnly == false && entitlementType.Equals("TIME"))
            {
                legacyCardDTOList = legacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.TIME));
                AttachListToDataSource(legacyCardDTOList);
                DGVChildLines.Visible = false;
                btnClose.Visible = false;
                lblChildHeader.Visible = false;
            }
            else if (readOnly == false && entitlementType.Equals("LOYALTYPOINTS"))
            {
                //dgvLegacyEntitlements.ReadOnly = false;
                legacyCardDTOList = legacyCardDTO.LegacyCardCreditPlusDTOList.FindAll(l => l.CreditPlusType.Equals(Semnox.Parafait.Customer.Accounts.CreditPlusType.LOYALTY_POINT));
                AttachListToDataSource(legacyCardDTOList);
                DGVChildLines.Visible = false;
                btnClose.Visible = false;
                lblChildHeader.Visible = false;
            }
            else if (readOnly == false && entitlementType.Equals("GAMES"))
            {
                AttachListToGameDataSource(legacyCardDTO.LegacyCardGamesDTOsList);
                dgvLegacyCardGames.ReadOnly = readOnly;
                DGVChildLines.Visible = false;
                btnClose.Visible = false;
            }
            log.LogMethodExit();
        }

        private void AttachCreditPlusConsumption(List<LegacyCardCreditPlusConsumptionDTO> legacyCardCreditPlusConsumptionDTOList)
        {
            log.LogMethodEntry(legacyCardCreditPlusConsumptionDTOList);
            BindingSource legacyCardCreditPlusConsumptionDTOListBS = new BindingSource();
            if (legacyCardCreditPlusConsumptionDTOList != null && legacyCardCreditPlusConsumptionDTOList.Any())
            {
                SortableBindingList<LegacyCardCreditPlusConsumptionDTO> SortableLegacyCardCreditPlusDTOList = new SortableBindingList<LegacyCardCreditPlusConsumptionDTO>(legacyCardCreditPlusConsumptionDTOList);
                legacyCardCreditPlusConsumptionDTOListBS.DataSource = SortableLegacyCardCreditPlusDTOList;
            }
            else
            {
                legacyCardCreditPlusConsumptionDTOListBS.DataSource = new SortableBindingList<LegacyCardCreditPlusDTO>();
            }
            lblChildHeader.Text = "Legacy Card CreditPlus Consumption";
            DGVChildLines.DataSource = legacyCardCreditPlusConsumptionDTOListBS;
            DGVChildLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DGVChildLines.Refresh();// = true;
            DGVChildLines.Columns["LastUpdateDate"].Visible = false;
            DGVChildLines.Columns["LastUpdatedBy"].Visible = false;
            DGVChildLines.Columns["Guid"].Visible = false;
            DGVChildLines.Columns["SynchStatus"].Visible = false;
            DGVChildLines.Columns["MasterEntityId"].Visible = false;
            DGVChildLines.Columns["CreationDate"].Visible = false;
            DGVChildLines.Columns["CreatedBy"].Visible = false;
            
            log.LogMethodExit();
        }

        private void AttachListToGameExtendedDataSource(List<LegacyCardGameExtendedDTO> legacyCardGameExtendedDTOList)
        {
            log.LogMethodEntry(legacyCardGameExtendedDTOList);
            BindingSource legacyCardGameExtendedDTOListBS = new BindingSource();
            if (legacyCardGameExtendedDTOList != null && legacyCardGameExtendedDTOList.Any())
            {
                SortableBindingList<LegacyCardGameExtendedDTO> SortableLegacyCardCreditPlusDTOList = new SortableBindingList<LegacyCardGameExtendedDTO>(legacyCardGameExtendedDTOList);
                legacyCardGameExtendedDTOListBS.DataSource = SortableLegacyCardCreditPlusDTOList;
            }
            else
            {
                legacyCardGameExtendedDTOListBS.DataSource = new SortableBindingList<LegacyCardCreditPlusDTO>();
            }
            DGVChildLines.DataSource = legacyCardGameExtendedDTOListBS;
            DGVChildLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DGVChildLines.Refresh();// = true;
            DGVChildLines.Columns["LastUpdateDate"].Visible = false;
            DGVChildLines.Columns["LastUpdatedBy"].Visible = false;
            DGVChildLines.Columns["Guid"].Visible = false;
            DGVChildLines.Columns["SynchStatus"].Visible = false;
            DGVChildLines.Columns["MasterEntityId"].Visible = false;
            DGVChildLines.Columns["CreationDate"].Visible = false;
            DGVChildLines.Columns["CreatedBy"].Visible = false;
            lblChildHeader.Text = "Legacy Card Game Extended";
            log.LogMethodExit();
        }

        private void AttachListToDataSource(List<LegacyCardCreditPlusDTO> legacyCardDTOList)
        {
            log.LogMethodEntry(legacyCardDTOList);
            BindingSource legacyCardCreditPlusDTOListBS = new BindingSource();
            if (legacyCardDTOList != null && legacyCardDTOList.Any())
            {
                SortableBindingList<LegacyCardCreditPlusDTO> SortableLegacyCardCreditPlusDTOList = new SortableBindingList<LegacyCardCreditPlusDTO>(legacyCardDTOList);
                legacyCardCreditPlusDTOListBS.DataSource = SortableLegacyCardCreditPlusDTOList;
            }
            else
            {
                legacyCardCreditPlusDTOListBS.DataSource = new SortableBindingList<LegacyCardCreditPlusDTO>();
            }
            dgvLegacyEntitlements.DataSource = legacyCardCreditPlusDTOListBS;
            dgvLegacyEntitlements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLegacyEntitlements.Refresh();// = true;
            dgvLegacyEntitlements.Visible = true;
            dgvLegacyEntitlements.ReadOnly = false;
            //dgvLegacyEntitlements.ReadOnly = !readOnly;
            foreach (DataGridViewRow row in dgvLegacyEntitlements.Rows)
            {
                row.Cells["RevisedLegacyCreditPlus"].ReadOnly = readOnly;
            }
            dgvLegacyEntitlements.Columns["RevisedLegacyCreditPlus"].ReadOnly = false;
            dgvLegacyEntitlements.Refresh();
            //dgvLegacyEntitlements.ReadOnly = readOnly;
            lblParentHeader.Text = "Legacy Card CreditPlus";
            lblParentHeader.Visible = true;
            log.LogMethodExit();
        }
        private void AttachListToGameDataSource(List<LegacyCardGamesDTO> legacyCardGamesDTOList)
        {
            log.LogMethodEntry(legacyCardGamesDTOList);
            BindingSource legacyCardGameDTOListBS = new BindingSource();
            
            dgvLegacyEntitlements.Visible = false;
            if (legacyCardGamesDTOList != null && legacyCardGamesDTOList.Any())
            {
                SortableBindingList<LegacyCardGamesDTO> SortableLegacyCardGamesDTOList = new SortableBindingList<LegacyCardGamesDTO>(legacyCardGamesDTOList);
                legacyCardGameDTOListBS.DataSource = SortableLegacyCardGamesDTOList;
            }
            else
            {
                legacyCardGameDTOListBS.DataSource = new SortableBindingList<LegacyCardGamesDTO>();
            }
            dgvLegacyCardGames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLegacyCardGames.AllowUserToAddRows = false;
            dgvLegacyCardGames.DataSource = legacyCardGameDTOListBS;
            dgvLegacyCardGames.Columns["LastUpdateDate"].Visible = false;
            dgvLegacyCardGames.Columns["LastUpdatedBy"].Visible = false;
            dgvLegacyCardGames.Columns["Guid"].Visible = false;
            dgvLegacyCardGames.Columns["SynchStatus"].Visible = false;
            dgvLegacyCardGames.Columns["MasterEntityId"].Visible = false;
            dgvLegacyCardGames.Columns["CreationDate"].Visible = false;
            dgvLegacyCardGames.Columns["CreatedBy"].Visible = false;
            lblParentHeader.Text = "Legacy Card Games";
            lblParentHeader.Visible = true;
            dgvLegacyCardGames.Refresh();
            dgvLegacyCardGames.ReadOnly = true;
            dgvLegacyCardGames.Visible = true;
            dgvLegacyEntitlements.Visible = false;
            dgvLegacyCardDiscounts.Visible = false;
            log.LogMethodExit();

        }
        private void AttachListToDiscountDataSource(List<LegacyCardDiscountsDTO> legacyCardDiscountsDTOList)
        {
            log.LogMethodEntry(legacyCardDiscountsDTOList);
            BindingSource legacyCardDiscountsDTOListBS = new BindingSource();
            
            dgvLegacyEntitlements.Visible = false;
            if (legacyCardDiscountsDTOList != null && legacyCardDiscountsDTOList.Any())
            {
                SortableBindingList<LegacyCardDiscountsDTO> SortableLegacyCardDiscountsDTOList = new SortableBindingList<LegacyCardDiscountsDTO>(legacyCardDiscountsDTOList);
                legacyCardDiscountsDTOListBS.DataSource = SortableLegacyCardDiscountsDTOList;
            }
            else
            {
                legacyCardDiscountsDTOListBS.DataSource = new SortableBindingList<LegacyCardCreditPlusDTO>();
            }
            dgvLegacyCardDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLegacyCardDiscounts.AllowUserToAddRows = false;
            dgvLegacyCardDiscounts.DataSource = legacyCardDiscountsDTOListBS;
            dgvLegacyCardDiscounts.Refresh();
            dgvLegacyCardDiscounts.ReadOnly = true;
            dgvLegacyCardDiscounts.Columns["LastUpdateDate"].Visible = false;
            dgvLegacyCardDiscounts.Columns["Guid"].Visible = false;
            dgvLegacyCardDiscounts.Columns["SynchStatus"].Visible = false;
            dgvLegacyCardDiscounts.Columns["MasterEntityId"].Visible = false;
            dgvLegacyCardDiscounts.Columns["CreationDate"].Visible = false;
            dgvLegacyCardDiscounts.Columns["CreatedBy"].Visible = false;
            dgvLegacyCardGames.Visible = false;
            dgvLegacyEntitlements.Visible = false;
            dgvLegacyCardDiscounts.Visible = true;
            dgvLegacyCardDiscounts.Visible = true;
            lblParentHeader.Text = "Legacy Card Discounts";
            lblParentHeader.Visible = true;

            this.btnClose.Location = this.btnOk.Location;
            DGVChildLines.Visible = false;
            lblChildHeader.Visible = false;
            log.LogMethodExit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //dgvLegacyEntitlements.EndEdit();
                //if (dgvLegacyEntitlements.DataSource != null)
                //{
                //    BindingSource legacyCardCreditPlusDTOListBS = (BindingSource)dgvLegacyEntitlements.DataSource;
                //    if (legacyCardCreditPlusDTOListBS.Count > 0)
                //    {
                //        var legacyCardCreditPlusDTOList = (SortableBindingList<LegacyCardCreditPlusDTO>)legacyCardCreditPlusDTOListBS.DataSource;

                //        if (legacyCardCreditPlusDTOList.Count > 0)
                //        {
                //            foreach (LegacyCardCreditPlusDTO legacyCardCreditPlusDTO in legacyCardCreditPlusDTOList)
                //            {
                //                if (legacyCardDTO.LegacyCardCreditPlusDTOList != null && legacyCardDTO.LegacyCardCreditPlusDTOList.Any())
                //                {
                //                    legacyCardDTO.LegacyCardCreditPlusDTOList.RemoveAll(lc => lc.LegacyCardCreditPlusId == legacyCardCreditPlusDTO.LegacyCardCreditPlusId);
                //                    if (legacyCardCreditPlusDTO.IsChanged == true)
                //                    {
                //                        legacyCardCreditPlusDTO.RevisedLegacyCreditPlus = legacyCardCreditPlusDTO.LegacyCreditPlus;
                //                    }
                //                    legacyCardDTO.LegacyCardCreditPlusDTOList.Add(legacyCardCreditPlusDTO);
                //                }
                //            }

                //        }
                //    }
                //}
                //dgvLegacyCardGames.EndEdit();
                //if (dgvLegacyCardGames.DataSource != null)
                //{
                //    BindingSource legacyCardGamesDTOListBS = (BindingSource)dgvLegacyCardGames.DataSource;
                //    if (legacyCardGamesDTOListBS.Count > 0)
                //    {
                //        foreach (LegacyCardGamesDTO legacyCardGamesDTO in legacyCardGamesDTOListBS)
                //        {
                //            if (legacyCardDTO.LegacyCardGamesDTOsList != null && legacyCardDTO.LegacyCardGamesDTOsList.Any())
                //            {
                //                legacyCardDTO.LegacyCardGamesDTOsList.RemoveAll(lc => lc.LegacyCardGameId == legacyCardGamesDTO.LegacyCardGameId);
                //                if (legacyCardGamesDTO.IsChanged == true)
                //                {
                //                    legacyCardGamesDTO.RevisedQuantity = legacyCardGamesDTO.Quantity;
                //                }
                //                legacyCardDTO.LegacyCardGamesDTOsList.Add(legacyCardGamesDTO);
                //            }
                //        }
                //    }
                //}
                LegacyCardBL legacyCardBL = new LegacyCardBL(executionContext, legacyCardDTO, null);
                legacyCardBL.Save();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

       
        private void dgvLegacyEntitlements_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show("Error in Data Grid at Row " + e.RowIndex + 1 + "and Column " + dgvLegacyEntitlements.Columns[e.ColumnIndex].DataPropertyName + Environment.NewLine + e.Exception.Message);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing dgvLegacyEntitlements_DataError()" + ex.Message);
            }

            log.LogMethodExit();
        }

        private void dgvLegacyEntitlements_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.dgvLegacyEntitlements.SelectedRows.Count <= 0)
            {

            }
            else
            {
                string cardCreditPlusId = this.dgvLegacyEntitlements.SelectedRows[0].Cells["LegacyCardCreditPlusId"].Value.ToString();
                LegacyCardCreditPlusDTO legacyCardCreditPlusDTO = legacyCardDTO.LegacyCardCreditPlusDTOList.Find(lcc => lcc.LegacyCardCreditPlusId.ToString().Equals(cardCreditPlusId));
                if (legacyCardCreditPlusDTO != null)
                {
                    AttachCreditPlusConsumption(legacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList);
                }
                else
                {
                    DGVChildLines.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void dgvLegacyCardGames_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.dgvLegacyCardGames.SelectedRows.Count <= 0)
            {

            }
            else
            {
                string legacyCardGameId = this.dgvLegacyCardGames.SelectedRows[0].Cells["LegacyCardGameId"].Value.ToString();
                LegacyCardGamesDTO legacyCardGamesDTO = legacyCardDTO.LegacyCardGamesDTOsList.Find(lcc => lcc.LegacyCardGameId.ToString().Equals(legacyCardGameId));
                if (legacyCardGamesDTO != null)
                {
                    AttachListToGameExtendedDataSource(legacyCardGamesDTO.LegacyCardGameExtendedDTOList);
                }
                else
                {
                    DGVChildLines.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void dgvLegacyCardDiscounts_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.dgvLegacyEntitlements.SelectedRows.Count <= 0)
            {

            }
            else
            {
                string cardCreditPlusId = this.dgvLegacyEntitlements.SelectedRows[0].Cells["LegacyCardCreditPlusId"].Value.ToString();
                LegacyCardCreditPlusDTO legacyCardCreditPlusDTO = legacyCardDTO.LegacyCardCreditPlusDTOList.Find(lcc => lcc.LegacyCardCreditPlusId.ToString().Equals(cardCreditPlusId));
                if (legacyCardCreditPlusDTO != null)
                {
                    AttachCreditPlusConsumption(legacyCardCreditPlusDTO.LegacyCardCreditPlusConsumptionDTOList);
                }
                else
                {
                    DGVChildLines.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
    }
}
