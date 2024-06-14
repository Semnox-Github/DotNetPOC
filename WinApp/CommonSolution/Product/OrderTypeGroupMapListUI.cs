
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
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// OrderTypeGroupMap UI
    /// </summary>
    public partial class OrderTypeGroupMapListUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private OrderTypeDTO orderTypeDTO;

        /// <summary>
        /// Constructor of OrderTypeGroupMapListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="orderTypeDTO">Order Type</param>
        public OrderTypeGroupMapListUI(Utilities utilities, OrderTypeDTO orderTypeDTO)
        {
            log.LogMethodEntry(utilities, orderTypeDTO);
            InitializeComponent();
            this.utilities = utilities;
            this.orderTypeDTO = orderTypeDTO;
            
            utilities.setupDataGridProperties(ref dgvOrderTypeGroupMapDTOList);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            this.Text = this.Text + " : " + orderTypeDTO.Name;
            log.LogMethodExit();
        }

        private void OrderTypeGroupMapListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            orderTypeGroupMapDTOListBS.DataSource = null;
            LoadOrderTypeGroupDTOList();
            LoadOrderTypeGroupMapDTOList();
            log.LogMethodExit();
        }

        private void LoadOrderTypeGroupDTOList()
        {
            log.LogMethodEntry();
            try
            {
                OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(machineUserContext);
                List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                }
                searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParameters);
                if(orderTypeGroupDTOList == null)
                {
                    orderTypeGroupDTOList = new List<OrderTypeGroupDTO>();
                }
                orderTypeGroupDTOList.Insert(0, new OrderTypeGroupDTO());
                orderTypeGroupDTOList[0].Name = "SELECT";
                BindingSource bs = new BindingSource();
                bs.DataSource = orderTypeGroupDTOList;
                cmbOrderTypeGroup.DataSource = bs;
                cmbOrderTypeGroup.DisplayMember = "Name";
                cmbOrderTypeGroup.ValueMember = "Id";

                bs = new BindingSource();
                bs.DataSource = orderTypeGroupDTOList;
                orderTypeGroupIdDataGridViewComboBoxColumn.DataSource = bs;
                orderTypeGroupIdDataGridViewComboBoxColumn.DisplayMember = "Name";
                orderTypeGroupIdDataGridViewComboBoxColumn.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occured while loading OrderTypeGroup", ex);
            }
            log.LogMethodExit();
        }

        private void LoadOrderTypeGroupMapDTOList()
        {
            log.LogMethodEntry();
            try
            {
                OrderTypeGroupMapListBL orderTypeGroupMapListBL = new OrderTypeGroupMapListBL(machineUserContext);
                List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                }
                if(cmbOrderTypeGroup.SelectedValue != null && Convert.ToInt32(cmbOrderTypeGroup.SelectedValue) != -1)
                {
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_GROUP_ID, Convert.ToString(cmbOrderTypeGroup.SelectedValue)));
                }
                searchParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_ID, orderTypeDTO.Id.ToString()));
                searchParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList = orderTypeGroupMapListBL.GetOrderTypeGroupMapDTOList(searchParameters);
                SortableBindingList<OrderTypeGroupMapDTO> orderTypeGroupMapDTOSortableList;
                if (orderTypeGroupMapDTOList != null)
                {
                    orderTypeGroupMapDTOSortableList = new SortableBindingList<OrderTypeGroupMapDTO>(orderTypeGroupMapDTOList);
                }
                else
                {
                    orderTypeGroupMapDTOSortableList = new SortableBindingList<OrderTypeGroupMapDTO>();
                }
                orderTypeGroupMapDTOListBS.DataSource = orderTypeGroupMapDTOSortableList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occured while loading OrderTypeGroupMap",ex);
            }
            log.LogMethodExit();
        }

        private void dgvOrderTypeGroupMapDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvOrderTypeGroupMapDTOList.Columns[e.ColumnIndex].HeaderText +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            cmbOrderTypeGroup.SelectedValue = -1;
            RefreshData();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadOrderTypeGroupMapDTOList();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvOrderTypeGroupMapDTOList.EndEdit();
            SortableBindingList<OrderTypeGroupMapDTO> orderTypeGroupMapDTOSortableList = orderTypeGroupMapDTOListBS.DataSource as SortableBindingList<OrderTypeGroupMapDTO>;
            string message;
            OrderTypeGroupMapBL orderTypeGroupMapBL;
            bool error = false;
            if (orderTypeGroupMapDTOSortableList != null && orderTypeGroupMapDTOSortableList.Count > 0)
            {
                for (int i = 0; i < orderTypeGroupMapDTOSortableList.Count; i++)
                {
                    if (orderTypeGroupMapDTOSortableList[i].IsChanged)
                    {
                        message = ValidateOrderTypeGroupMapDTO(orderTypeGroupMapDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                orderTypeGroupMapBL = new OrderTypeGroupMapBL(machineUserContext,orderTypeGroupMapDTOSortableList[i]);
                                orderTypeGroupMapBL.Save();
                            }
                            catch(InvalidOrderTypeGroupException ex)
                            {
                                error = true;
                                log.Error("Error occured while saving the record",ex);
                                dgvOrderTypeGroupMapDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", orderTypeGroupIdDataGridViewComboBoxColumn.HeaderText));
                                break;
                            }
                            catch (DuplicateOrderTypeGroupMapException ex)
                            {
                                error = true;
                                log.Error("Error occured while saving the record", ex);
                                dgvOrderTypeGroupMapDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", orderTypeGroupIdDataGridViewComboBoxColumn.HeaderText));
                                break;
                            }
                            catch (Exception ex)
                            {
                                error = true;
                                log.Error("Error occured while saving the record", ex);
                                dgvOrderTypeGroupMapDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                        else
                        {
                            error = true;
                            dgvOrderTypeGroupMapDTOList.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            if (!error)
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvOrderTypeGroupMapDTOList.Update();
                dgvOrderTypeGroupMapDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private string ValidateOrderTypeGroupMapDTO(OrderTypeGroupMapDTO orderTypeGroupMapDTO)
        {
            log.LogMethodEntry(orderTypeGroupMapDTO);
            string message = string.Empty;
            if (orderTypeGroupMapDTO.OrderTypeGroupId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", orderTypeGroupIdDataGridViewComboBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvOrderTypeGroupMapDTOList.SelectedRows.Count <= 0 && dgvOrderTypeGroupMapDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit(null, "No rows selected. Please select the rows you want to delete and press delete");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvOrderTypeGroupMapDTOList.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvOrderTypeGroupMapDTOList.SelectedCells)
                {
                    dgvOrderTypeGroupMapDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvOrderTypeGroupMapDTOList.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvOrderTypeGroupMapDTOList.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        SortableBindingList<OrderTypeGroupMapDTO> orderTypeGroupMapDTOSortableList = (SortableBindingList<OrderTypeGroupMapDTO>)orderTypeGroupMapDTOListBS.DataSource;
                        OrderTypeGroupMapDTO orderTypeGroupMapDTO = orderTypeGroupMapDTOSortableList[row.Index];
                        orderTypeGroupMapDTO.IsActive = false;
                        OrderTypeGroupMapBL orderTypeGroupMapBL = new OrderTypeGroupMapBL(machineUserContext,orderTypeGroupMapDTO);
                        confirmDelete = true;
                        refreshFromDB = true;
                        try
                        {
                            orderTypeGroupMapBL.Save();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while inactivating the record",ex);
                            MessageBox.Show(ex.Message);
                            break;
                        }
                    }
                }
            }
            if (rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            if (refreshFromDB == true)
            {
                btnSearch.PerformClick();
            }
            log.LogMethodExit();
        }

        private void orderTypeGroupMapDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OrderTypeGroupMapDTO orderTypeGroupMapDTO = new OrderTypeGroupMapDTO();
            orderTypeGroupMapDTO.OrderTypeId = orderTypeDTO.Id;
            e.NewObject = orderTypeGroupMapDTO;
            log.LogVariableState("orderTypeGroupMapDTO", orderTypeGroupMapDTO);
            log.LogMethodExit();
        }
    }
}
