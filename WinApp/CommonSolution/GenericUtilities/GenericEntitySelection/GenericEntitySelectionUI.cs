/********************************************************************************************
 * Project Name - GenericEntitySelectionUI
 * Description  - Data object of GenericEntitySelectionUI
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 *2.70.2        26-Nov-2019      Lakshminarayana     Virtual store enhancement
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    public partial class GenericEntitySelectionUI<T> : Form where T : class
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Utilities.Utilities utilities;
        private readonly string formName;
        private readonly List<EntityPropertyDefintion> entityPropertyDefintionList;
        private readonly List<T> entityList;
        private VirtualKeyboardController virtualKeyboardController;
        private T selectedValue;
        public GenericEntitySelectionUI(Utilities.Utilities utilities, string formName, List<EntityPropertyDefintion> entityPropertyDefintionList, List<T> entityList)
        {
            log.LogMethodEntry(utilities, formName, entityPropertyDefintionList, entityList);
            InitializeComponent();
            this.formName = formName;
            this.entityPropertyDefintionList = entityPropertyDefintionList;
            this.entityList = entityList;
            this.utilities = utilities;
            ValidateInput();
            Text = formName;
            SetSelectionButtonColumnProperties();
            CreateDataGridColumns();
            SetFilterComboBoxProperties();
            SetDataGridViewDataSource(entityList);
            utilities.setupDataGridProperties(ref dgvEntityList);
            SetDataGridViewFont(dgvEntityList);
            dgvEntityList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            ThemeUtils.SetupVisuals(groupBox1);
            ThemeUtils.SetupVisuals(dgvEntityList);
            ThemeUtils.SetupVisuals(btnClose);
            ThemeUtils.SetupVisuals(btnSearch);
            ThemeUtils.SetupVisuals(cmbEntityProperty);
            ThemeUtils.SetupVisuals(txtValue);
            this.btnSearch.Size = new System.Drawing.Size(100, 40);
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnSearch.Font = new Font("Arial", 15F, FontStyle.Regular);
            this.btnClose.Font = new Font("Arial", 15F, FontStyle.Regular);
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>() {  }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"), new List<Control>() { dgvEntityList });
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should only be called from POS Application.
        /// This method is added for the New UI changes in the Customer Registration Form in POS.
        /// This method changes the styles and size , Fonts of this form.
        /// </summary>
        /// <param name="posBackGroundColor">posBackGroundColor</param>
        public void SetPOSBackGroundColor(Color posBackGroundColor)
        {
            dgvEntityList.BackgroundColor = posBackGroundColor;
            this.BackColor = posBackGroundColor;
            groupBox1.BackColor = posBackGroundColor;

            //
            //btnSearch
            //
            this.btnSearch.BackgroundImage = Properties.Resources.customer_button_normal;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.ForeColor = Color.White;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = Properties.Resources.customer_button_normal;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.ForeColor = Color.White;
        }

        private void SetDataGridViewFont(DataGridView dgvInput)
        {
            log.LogMethodEntry();
            Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font(@"Tahoma", 15, FontStyle.Regular);
            }
            dgvInput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvInput.ColumnHeadersHeight = 40;
            dgvInput.ColumnHeadersDefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            dgvInput.RowTemplate.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            dgvInput.RowTemplate.Height = 40;
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }
        private void SetDataGridViewDataSource(List<T> entityList)
        {
            log.LogMethodEntry(entityList);
            dgvEntityList.DataSource = new SortableBindingList<T>(entityList);
            log.LogMethodExit();
        }

        private void SetFilterComboBoxProperties()
        {
            log.LogMethodEntry();
            cmbEntityProperty.ValueMember = "PropertyName";
            cmbEntityProperty.DisplayMember = "DisplayName";
            List<EntityPropertyDefintion> filteredEntityPropertyDefintionList = entityPropertyDefintionList.Where(x => x.IsFilter).ToList();
            filteredEntityPropertyDefintionList.Insert(0, new EntityPropertyDefintion(MessageContainerList.GetMessage(utilities.ExecutionContext, "<SELECT>")));
            cmbEntityProperty.DataSource = filteredEntityPropertyDefintionList;
            log.LogMethodExit();
        }

        private void ValidateInput()
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(formName))
            {
                throw new Exception("formName should not be empty");
            }
            if (entityPropertyDefintionList == null || entityPropertyDefintionList.Count == 0)
            {
                throw new Exception("entityPropertyDefintionList should not be empty");
            }
            if (entityList == null || entityList.Count == 0)
            {
                throw new Exception("entityList should not be empty");
            }
            log.LogMethodExit();
        }

        private void CreateDataGridColumns()
        {
            log.LogMethodEntry();
            dgvEntityList.AutoGenerateColumns = false;
            List<DataGridViewColumn> dataGridViewColumns = GetDataGridColumns();
            dgvEntityList.Columns.AddRange(dataGridViewColumns.ToArray());
            log.LogMethodExit();
        }

        private List<DataGridViewColumn> GetDataGridColumns()
        {
            log.LogMethodEntry();
            List<DataGridViewColumn> dataGridViewColumns = new List<DataGridViewColumn>();
            foreach (var entityPropertyDefintion in entityPropertyDefintionList)
            {
                DataGridViewColumn dataGridViewColumn = GetDataGridViewColumn(entityPropertyDefintion);
                if (entityPropertyDefintion.DataGridViewCellStyle != null)
                {
                    dataGridViewColumn.DefaultCellStyle = entityPropertyDefintion.DataGridViewCellStyle;
                }
                dataGridViewColumn.HeaderText = entityPropertyDefintion.DisplayName;
                dataGridViewColumn.DataPropertyName = entityPropertyDefintion.PropertyName;
                dataGridViewColumns.Add(dataGridViewColumn);
            }
            log.LogMethodExit(dataGridViewColumns);
            return dataGridViewColumns;
        }

        private DataGridViewColumn GetDataGridViewColumn(EntityPropertyDefintion entityPropertyDefintion)
        {
            log.LogMethodEntry(entityPropertyDefintion);
            if (entityPropertyDefintion.IsFlag)
            {
                DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                dataGridViewCheckBoxColumn.TrueValue = "Y";
                dataGridViewCheckBoxColumn.FalseValue = "N";
                dataGridViewCheckBoxColumn.IndeterminateValue = "N";
                log.LogMethodExit(dataGridViewCheckBoxColumn);
                return dataGridViewCheckBoxColumn;
            }
            if (GetPropertyType(entityPropertyDefintion) == typeof(bool))
            {
                DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                log.LogMethodExit(dataGridViewCheckBoxColumn);
                return dataGridViewCheckBoxColumn;
            }
            DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            log.LogMethodExit(dataGridViewTextBoxColumn);
            return dataGridViewTextBoxColumn;
        }

        private Type GetPropertyType(EntityPropertyDefintion entityPropertyDefintion)
        {
            log.LogMethodEntry(entityPropertyDefintion);
            PropertyInfo entityPropertyInfo = GetPropertyInfo(entityPropertyDefintion);
            log.LogMethodExit(entityPropertyInfo.PropertyType);
            return entityPropertyInfo.PropertyType;
        }

        private PropertyInfo GetPropertyInfo(EntityPropertyDefintion entityPropertyDefintion)
        {
            log.LogMethodEntry(entityPropertyDefintion);
            if (entityList == null || entityList.Count == 0)
            {
                throw new Exception("Entity list is empty.");
            }
            Type entityType = entityList[0].GetType();
            PropertyInfo entityPropertyInfo = entityType.GetProperty(entityPropertyDefintion.PropertyName);
            log.LogMethodExit(entityPropertyInfo);
            return entityPropertyInfo;
        }

        private void SetSelectionButtonColumnProperties()
        {
            log.LogMethodEntry();
            dgvSelectButtonColumn.Text = "...";
            dgvSelectButtonColumn.UseColumnTextForButtonValue = true;
            dgvSelectButtonColumn.Width = 50;
            log.LogMethodExit();
        }

        public T SelectedValue
        {
            get
            {
                return selectedValue;
            }
        }

        private void dgvEntityList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataGridCellClick(e.RowIndex, e.ColumnIndex);
            log.LogMethodExit();
        }

        private void OnDataGridCellClick(int rowIndex, int columnIndex)
        {
            log.LogMethodEntry(rowIndex, columnIndex);
            SelectValue(rowIndex, columnIndex);
            if (IsValueSelected())
            {
                CloseOnValueSelection();
            }
            log.LogMethodExit();
        }

        private bool IsValueSelected()
        {
            log.LogMethodEntry();
            log.LogMethodExit(selectedValue);
            return selectedValue != null;
        }

        private void CloseOnValueSelection()
        {
            log.LogMethodEntry();
            DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void SelectValue(int rowIndex, int columnIndex)
        {
            log.LogMethodEntry(rowIndex, columnIndex);
            if (rowIndex < 0 || columnIndex != dgvSelectButtonColumn.Index)
            {
                log.LogMethodExit("Invalid row or column");
                return;
            }
            if (dgvEntityList.Rows[rowIndex].DataBoundItem != null)
            {
                selectedValue = dgvEntityList.Rows[rowIndex].DataBoundItem as T;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CloseOnCancelSelection();
            log.LogMethodExit();
        }

        private void CloseOnCancelSelection()
        {
            log.LogMethodEntry();
            DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisplayFilterEntityList();
            log.LogMethodExit();
        }

        private void DisplayFilterEntityList()
        {
            log.LogMethodEntry();
            List<T> list = null;
            if (NoFilterConditionExists())
            {
                list = entityList;
            }
            else
            {
                list = GetFilteredEntityList();
            }
            SetDataGridViewDataSource(list);
            log.LogMethodExit();
        }

        private List<T> GetFilteredEntityList()
        {
            log.LogMethodEntry();
            List<T> filteredEntityList = null;
            EntityPropertyDefintion filterEntityPropertyDefintion = GetFilterPropertyDefinition();
            PropertyInfo filterPropertyInfo = GetPropertyInfo(filterEntityPropertyDefintion);
            object filterValue = GetFilterValue(); ;
            filteredEntityList = entityList.FindAll(delegate (T entity)
            {
                return IsMatch(filterValue, filterPropertyInfo.GetValue(entity), filterPropertyInfo.PropertyType);
            });
            log.LogMethodExit(filteredEntityList);
            return filteredEntityList;
        }

        private bool IsMatch(object filterValue, object propertyValue, Type propertyType)
        {
            log.LogMethodEntry(filterValue, propertyValue, propertyType);
            if (IsEmpty(filterValue))
            {
                log.LogMethodExit(true, "Filter value is empty.");
                return true;
            }
            else if (IsEmpty(propertyValue))
            {
                log.LogMethodExit(false, "Property value is empty.");
                return false;
            }
            bool result = false;
            try
            {
                if (propertyType == typeof(string))
                {
                    result = CultureInfo.InvariantCulture.CompareInfo.IndexOf(propertyValue.ToString(), filterValue.ToString(), CompareOptions.IgnoreCase) >= 0;
                }
                else
                {
                    result = GetConvertedValue(filterValue, propertyType).Equals(GetConvertedValue(propertyValue, propertyType));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while checking filter value matches property value", ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        private object GetConvertedValue(object value, Type type)
        {
            log.LogMethodEntry(value, type);
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }
            log.LogMethodExit(value);
            return Convert.ChangeType(value, type);
        }

        private static bool IsEmpty(object filterValue)
        {
            log.LogMethodEntry(filterValue);
            bool returnValue = filterValue == null || string.IsNullOrWhiteSpace(Convert.ToString(filterValue));
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool IsFilterConditionExists()
        {
            log.LogMethodEntry();
            bool returnValue = IsFilterPropertySelected() && IsFilterValueExists();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool IsFilterValueExists()
        {
            log.LogMethodEntry();
            bool result = false;
            EntityPropertyDefintion filterEntityPropertyDefintion = GetFilterPropertyDefinition();
            if (IsBooleanFilterProperty())
            {
                result = true;
            }
            else
            {
                result = IsFilterTextValueExists();
            }
            log.LogMethodExit(result);
            return result;
        }

        private EntityPropertyDefintion GetFilterPropertyDefinition()
        {
            log.LogMethodEntry();
            return cmbEntityProperty.SelectedItem as EntityPropertyDefintion;

        }

        private bool IsFilterTextValueExists()
        {
            log.LogMethodEntry();
            bool returnValue = string.IsNullOrWhiteSpace(txtValue.Text) == false;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool NoFilterConditionExists()
        {
            log.LogMethodEntry();
            bool returnValue = !IsFilterConditionExists();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private object GetFilterValue()
        {
            log.LogMethodEntry();
            EntityPropertyDefintion filterPropertyDefinition = GetFilterPropertyDefinition();
            if (filterPropertyDefinition.IsFlag)
            {
                log.LogMethodExit(chbValue.Checked ? "Y" : "N", "Flag property");
                return chbValue.Checked ? "Y" : "N";
            }
            if (GetPropertyType(filterPropertyDefinition) == typeof(bool))
            {
                log.LogMethodExit(chbValue.Checked, "boolean property");
                return chbValue.Checked;
            }
            log.LogMethodExit(txtValue.Text);
            return txtValue.Text;
        }

        private bool IsBooleanFilterProperty()
        {
            log.LogMethodEntry();
            EntityPropertyDefintion filterEntityPropertyDefintion = GetFilterPropertyDefinition();
            return filterEntityPropertyDefintion.IsFlag ||
                            GetPropertyType(filterEntityPropertyDefintion) == typeof(bool);
        }

        private bool IsFilterPropertySelected()
        {
            log.LogMethodEntry();
            return cmbEntityProperty.SelectedValue != null && string.IsNullOrWhiteSpace(cmbEntityProperty.SelectedValue as string) == false;
        }

        private void cmbEntityProperty_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            OnFilterPropertyChanged();
            log.LogMethodExit();
        }

        private void OnFilterPropertyChanged()
        {
            log.LogMethodEntry();
            chbValue.Visible = false;
            txtValue.Visible = false;
            if (IsFilterPropertySelected())
            {
                if (IsBooleanFilterProperty())
                {
                    chbValue.Visible = true;
                }
                else
                {
                    txtValue.Visible = true;
                }
            }
            log.LogMethodExit();
        }

        private void dgvEntityList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Cancel = true;
            log.Error(string.Format(@"Datagridview DataError in col: {0} row: {1}", e.ColumnIndex + 1, e.RowIndex + 1));
            log.LogMethodExit();
        }

        private void dgvEntityList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataGridCellClick(e.RowIndex, dgvSelectButtonColumn.Index);
            log.LogMethodExit();
        }

        private void GenericEntitySelectionUI_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnClose.Left = (Width - btnClose.Width) / 2;
            log.LogMethodExit();
        }
    }
}
