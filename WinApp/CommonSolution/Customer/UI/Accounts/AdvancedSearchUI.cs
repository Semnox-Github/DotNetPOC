using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Customer Advanced Search UI
    /// </summary>
    public partial class AdvancedSearchUI : Form
    {
        private Utilities utilities;
        private Dictionary<Enum, Column> columnDictionary;
        private SearchCriteria searchCriteria;
        private CriteriaUI criteriaUI;
        private List<KeyValuePair<Enum, string>> operatorDictionary;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VirtualKeyboardController virtualKeyboardController;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="searchCriteria"></param>
        public AdvancedSearchUI(Utilities utilities, SearchCriteria searchCriteria)
        {
            log.LogMethodEntry(utilities, searchCriteria);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setLanguage(this);
            this.searchCriteria = searchCriteria;
            cmbColumns.ValueMember = "Key";
            cmbColumns.DisplayMember = "Value";
            columnDictionary = searchCriteria.GetColumnProvider().GetAllColumns();
            List<KeyValuePair<Enum, string>> columnDataSource = new List<KeyValuePair<Enum, string>>();
            foreach (var columnItem in columnDictionary)
            {
                if (columnItem.Value.Browsable)
                {
                    columnDataSource.Add(new KeyValuePair<Enum, string>(columnItem.Key, MessageContainerList.GetMessage(utilities.ExecutionContext, columnItem.Value.DisplayName)));
                }
            }
            columnDataSource = columnDataSource.OrderBy((x) => x.Value).ToList();
            cmbColumns.DataSource = columnDataSource;
            operatorDictionary = new List<KeyValuePair<Enum, string>>();
            foreach (Enum operatorEnum in Enum.GetValues(typeof(Operator)))
            {
                IOperator @operator = OperatorFactory.GetOperator((Operator)operatorEnum);
                if (@operator.Browsable)
                {
                    operatorDictionary.Add(new KeyValuePair<Enum, string>(operatorEnum, MessageContainerList.GetMessage(utilities.ExecutionContext, @operator.DisplayName)));
                }
            }
            cmbOperator.ValueMember = "Key";
            cmbOperator.DisplayMember = "Value";
            List<KeyValuePair<bool, string>> boolDataSource = new List<KeyValuePair<bool, string>>();
            boolDataSource.Add(new KeyValuePair<bool, string>(true, MessageContainerList.GetMessage(utilities.ExecutionContext, "True")));
            boolDataSource.Add(new KeyValuePair<bool, string>(false, MessageContainerList.GetMessage(utilities.ExecutionContext, "False")));
            cmbValue.ValueMember = "Key";
            cmbValue.DisplayMember = "Value";
            cmbValue.DataSource = boolDataSource;
            SetOperatorDataSource();
            DisplayValueControl();
            RefreshCriteiraUI();
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, false);
            btnShowKeyPad.Visible = false;
            ThemeUtils.SetupVisuals(this);
            log.LogMethodExit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(rdbNot.Checked || ValidateValue())
            {
                Criteria selectedCriteria = criteriaUI != null? criteriaUI.GetSelectedCriteria() : null;
                if (selectedCriteria == null)
                {
                    selectedCriteria = searchCriteria;
                }
                Criteria newCriteria = null;
                if (rdbAnd.Checked)
                {
                    newCriteria = selectedCriteria.And((Enum)cmbColumns.SelectedValue, (Operator)cmbOperator.SelectedValue, GetValue());
                }
                else if (rdbOr.Checked)
                {
                    newCriteria = selectedCriteria.Or((Enum)cmbColumns.SelectedValue, (Operator)cmbOperator.SelectedValue, GetValue());
                }
                else if (rdbNot.Checked)
                {
                    newCriteria = selectedCriteria.Not();
                }
                searchCriteria.ReplaceCriteria(selectedCriteria, newCriteria);
                RefreshCriteiraUI();
            }
            else
            {

                Column column = columnDictionary[(Enum)cmbColumns.SelectedValue];
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, column.DisplayName)));
            }
            log.LogMethodExit();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(criteriaUI != null)
            {
                Criteria selectedCriteria = criteriaUI.GetSelectedCriteria();
                if (selectedCriteria != null && searchCriteria.ContainsCondition)
                {
                    searchCriteria.RemoveCriteria(selectedCriteria);
                    RefreshCriteiraUI();
                }
            }
            log.LogMethodExit();
        }

        private void RefreshCriteiraUI()
        {
            log.LogMethodEntry();
            if (criteriaUI != null)
            {
                flpCriteria.Controls.Remove(criteriaUI);
                criteriaUI = null;
            }
            if (searchCriteria.ContainsCondition)
            {
                criteriaUI = new CriteriaUI(searchCriteria.GetCriteria());
                flpCriteria.Controls.Add(criteriaUI);
            }
            log.LogMethodExit();
        }

        private void SetOperatorDataSource()
        {
            log.LogMethodEntry();
            if (columnDictionary != null && operatorDictionary != null)
            {
                Column column = columnDictionary[(Enum)cmbColumns.SelectedValue];
                List<Operator> supportedOperators = column.GetApplicableOperators();
                List<KeyValuePair<Enum, string>> operatorDataSource = new List<KeyValuePair<Enum, string>>();
                foreach (var operatorItem in operatorDictionary)
                {
                    if (supportedOperators.Contains((Operator)operatorItem.Key))
                    {
                        operatorDataSource.Add(operatorItem);
                    }
                }
                cmbOperator.DataSource = operatorDataSource;
            }
            log.LogMethodExit();
        }

        private void cmbColumns_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetOperatorDataSource();
            DisplayValueControl();
            log.LogMethodExit();
        }

        private void CustomerAdvancedSearchUI_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyCode == Keys.Q && e.Modifiers == Keys.Control)
            {
                if(searchCriteria.ContainsCondition)
                {
                    MessageBox.Show(searchCriteria.GetWhereClause());
                }
            }
            log.LogMethodExit();
        }

        private void DisplayValueControl()
        {
            log.LogMethodEntry();
            if (columnDictionary != null && 
                operatorDictionary != null && 
                cmbOperator.SelectedValue != null &&
                cmbColumns.SelectedValue != null)
            {
                cmbValue.Visible = false;
                dtpValue.Visible = false;
                txtValue.Visible = false;
                Operator @operator = (Operator)cmbOperator.SelectedValue;
                if (@operator != Operator.IS_NOT_NULL && @operator != Operator.IS_NULL)
                {
                    if (columnDictionary != null && operatorDictionary != null)
                    {
                        Column column = columnDictionary[(Enum)cmbColumns.SelectedValue];
                        ColumnType columnType = column.GetColumnType();
                        if (columnType == ColumnType.NUMBER || columnType == ColumnType.TEXT || columnType == ColumnType.UNIQUE_IDENTIFIER)
                        {
                            txtValue.Visible = true;
                        }
                        else if (columnType == ColumnType.DATE_TIME)
                        {
                            dtpValue.Visible = true;
                        }
                        else if (columnType == ColumnType.BIT)
                        {
                            cmbValue.Visible = true;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private bool ValidateValue()
        {
            log.LogMethodEntry();
            bool valid = false;
            Operator @operator = (Operator)cmbOperator.SelectedValue;
            Column column = columnDictionary[(Enum)cmbColumns.SelectedValue];
            ColumnType columnType = column.GetColumnType();
            if (@operator != Operator.IS_NOT_NULL && 
                @operator != Operator.IS_NULL &&
                columnType != ColumnType.BIT && columnType != ColumnType.DATE_TIME)
            {
                
                if(columnType == ColumnType.NUMBER)
                {
                    decimal value;
                    valid = decimal.TryParse(txtValue.Text, out value);
                }
                else if(columnType == ColumnType.TEXT)
                {
                    valid = (txtValue.Text != null);
                }
                else if(columnType == ColumnType.UNIQUE_IDENTIFIER)
                {
                    Guid value;
                    valid = Guid.TryParse(txtValue.Text, out value);
                }
            }
            else
            {
                valid = true;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private object GetValue()
        {
            log.LogMethodEntry();
            object returnValue = null;
            Column column = columnDictionary[(Enum)cmbColumns.SelectedValue];
            ColumnType columnType = column.GetColumnType();
            Operator @operator = (Operator)cmbOperator.SelectedValue;
            if (@operator != Operator.IS_NOT_NULL &&
                @operator != Operator.IS_NULL)
            {
                if (columnType == ColumnType.DATE_TIME)
                {
                    returnValue = dtpValue.Value;
                }
                else if (columnType == ColumnType.NUMBER)
                {
                    decimal value;
                    decimal.TryParse(txtValue.Text, out value);
                    returnValue = value;
                }
                else if (columnType == ColumnType.TEXT)
                {
                    returnValue = txtValue.Text;
                }
                else if (columnType == ColumnType.BIT)
                {
                    returnValue = (bool)cmbValue.SelectedValue;
                }
                else if (columnType == ColumnType.UNIQUE_IDENTIFIER)
                {
                    Guid value;
                    Guid.TryParse(txtValue.Text, out value);
                    returnValue = value;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private void cmbOperator_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisplayValueControl();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
    }
}
