/********************************************************************************************
*Project Name -  SegmentCategorizationValueUI                                                                         
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80.0      25-Jun-2020            Deeksha                     Modified to Make Product module read only in Windows Management Studio.
*2.110.0     07-Oct-2020   Mushahid Faizan    Modified as per inventory changes,
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Semnox.Core.Utilities;
using System.Windows.Forms;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Category
{
    public partial class SegmentCategorizationValueUI : Form
    {
        string Applicability;
        int PrimaryKey, SegmentCategoryId = -1;
        Utilities utilities;
        ParafaitEnv parafaitEnv;
        List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOLoadList;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext segmentCategorizationValueContext = ExecutionContext.GetExecutionContext();
        bool loadSearch = true;
        private ManagementStudioSwitch managementStudioSwitch;
        public SegmentCategorizationValueUI(Utilities _Utilities, ParafaitEnv _ParafaitEnv, string pApplicability, int pPrimaryKey, int segmentCategoryId, string _productName)
        {
            log.Debug("Starts-SegmentCategorizationValueUI() parameterized constructor.");
            InitializeComponent();
            utilities = _Utilities;
            parafaitEnv = _ParafaitEnv;
            Applicability = pApplicability;
            PrimaryKey = pPrimaryKey;

            SegmentCategoryId = segmentCategoryId;
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            if (parafaitEnv.IsCorporate)
            {
                segmentCategorizationValueContext.SetSiteId(parafaitEnv.SiteId);
            }
            else
            {
                segmentCategorizationValueContext.SetSiteId(-1);
            }
            segmentCategorizationValueContext.SetUserId(parafaitEnv.LoginID);
            if (Applicability.Equals("POS Products"))
            {
                this.Text = "Segments";
                lblHeading.Text = utilities.MessageUtils.getMessage("Segments for ");
                lblSegmentCategoryValueLable.Text = utilities.MessageUtils.getMessage("Segment Values") + ":";
                lblSegmentIdLabel.Text = utilities.MessageUtils.getMessage("Segment Id") + ":";
                managementStudioSwitch = new ManagementStudioSwitch(segmentCategorizationValueContext);
                UpdateUIElements();
            }
            else if (Applicability.Equals("Product"))
            {
                lblHeading.Text = "SKU Data for: ";
            }
            lblHeading.Text += (string.IsNullOrEmpty(_productName) ? Applicability : _productName);
            lblHeading.Text = textInfo.ToTitleCase(lblHeading.Text.ToLower());
            RegisterKeyDownHandlers(this);//Modification on 29-Apr-2016 to hide the search grid control
            log.Debug("Ends-SegmentCategorizationValueUI() parameterized constructor.");
        }

        private void SegmentCategorizationValueUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-SegmentCategorizationValueUI_Load() event.");
            createUI();
            PopulateData();
            log.Debug("Ends-SegmentCategorizationValueUI_Load() event.");
        }
        private void createUI()
        {
            log.Debug("Starts-createUI() method.");
            List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
            SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(segmentCategorizationValueContext);
            List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionDTOSearchParams;

            List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList;
            SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(segmentCategorizationValueContext);
            List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>> segmentDefinitionSourceMapDTOSearchParams;

            segmentDefinitionDTOSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, segmentCategorizationValueContext.GetSiteId().ToString()));
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
            segmentDefinitionDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, Applicability));
            //retriveing all the active segment definitions which are belongs to the current site id based on the applicability
            segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionDTOSearchParams);
            if (segmentDefinitionDTOList != null)
            {
                foreach (SegmentDefinitionDTO segmentDefinitionDTO in segmentDefinitionDTOList)
                {
                    //Retriving source maping data for the perticular segment definiton
                    segmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
                    segmentDefinitionSourceMapDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>>();
                    segmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SITE_ID, segmentCategorizationValueContext.GetSiteId().ToString()));
                    segmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.IS_ACTIVE, "Y"));
                    segmentDefinitionSourceMapDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters, string>(SegmentDefinitionSourceMapDTO.SearchBySegmentDefinitionSourceMapParameters.SEGMENT_DEFINITION_ID, segmentDefinitionDTO.SegmentDefinitionId.ToString()));
                    segmentDefinitionSourceMapDTOList = segmentDefinitionSourceMapList.GetAllSegmentDefinitionSourceMaps(segmentDefinitionSourceMapDTOSearchParams);
                    if (segmentDefinitionSourceMapDTOList != null)
                    {
                        foreach (SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO in segmentDefinitionSourceMapDTOList)
                        {
                            Label lbl = new Label();
                            lbl.AutoSize = true;
                            lbl.Text = segmentDefinitionDTO.SegmentName;

                            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

                            lbl.Text = lbl.Text.Replace('_', ' ');
                            lbl.Text += ":";

                            FlowLayoutPanel fpLabel = new FlowLayoutPanel();
                            fpLabel.FlowDirection = FlowDirection.RightToLeft;
                            fpLabel.Width = 230;
                            fpLabel.Height = 30;
                            if (segmentDefinitionDTO.IsMandatory.Equals("Y"))
                            {
                                lbl.Text += "*";
                            }
                            flpCategorizationValues.Controls.Add(fpLabel);
                            fpLabel.Controls.Add(lbl);
                            switch (segmentDefinitionSourceMapDTO.DataSourceType)
                            {
                                case "DATE":
                                    {
                                        TextBox txt = new TextBox();
                                        txt.Width = 120;
                                        txt.Name = segmentDefinitionDTO.SegmentName;
                                        if (segmentDefinitionDTO.IsMandatory.Equals("Y"))
                                        {
                                            txt.Name += "*";
                                        }
                                        txt.Tag = segmentDefinitionSourceMapDTO;
                                        flpCategorizationValues.Controls.Add(txt);

                                        DateTimePicker dtp = new DateTimePicker();
                                        dtp.Width = 25;
                                        dtp.Tag = txt.Tag;
                                        dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                                        flpCategorizationValues.Controls.Add(dtp);
                                        break;
                                    }
                                case "TEXT":
                                    {
                                        TextBox txt = new TextBox();
                                        txt.Width = 120;
                                        txt.Name = segmentDefinitionDTO.SegmentName;
                                        txt.MaxLength = 250;
                                        if (segmentDefinitionDTO.IsMandatory.Equals("Y"))
                                        {
                                            txt.Name += "*";
                                        }
                                        txt.Tag = segmentDefinitionSourceMapDTO;
                                        flpCategorizationValues.Controls.Add(txt);
                                    }
                                    break;
                                case "STATIC LIST":
                                    {
                                        Button btnsearch = new Button();
                                        btnsearch.Width = 25;
                                        btnsearch.BackgroundImage = Semnox.Parafait.Category.Properties.Resources.Search;
                                        btnsearch.BackgroundImageLayout = ImageLayout.Stretch;
                                        ComboBox cmbCustom = new ComboBox();
                                        cmbCustom.Width = 150;
                                        List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList;
                                        SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(segmentCategorizationValueContext);
                                        List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> segmentDefinitionSourceValueDTOSearchParams;
                                        segmentDefinitionSourceValueDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, segmentCategorizationValueContext.GetSiteId().ToString()));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, "Y"));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId.ToString()));
                                        segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(segmentDefinitionSourceValueDTOSearchParams);
                                        if (segmentDefinitionSourceValueDTOList == null)
                                        {
                                            segmentDefinitionSourceValueDTOList = new List<SegmentDefinitionSourceValueDTO>();
                                        }
                                        segmentDefinitionSourceValueDTOList.Insert(0, new SegmentDefinitionSourceValueDTO());
                                        segmentDefinitionSourceValueDTOList[0].ListValue = "";
                                        cmbCustom.Name = segmentDefinitionDTO.SegmentName;
                                        btnsearch.Name = "btn" + segmentDefinitionDTO.SegmentName;
                                        if (segmentDefinitionDTO.IsMandatory.Equals("Y"))
                                        {
                                            cmbCustom.Name += "*";
                                        }
                                        cmbCustom.Tag = segmentDefinitionSourceMapDTO;
                                        var ValueList = new Dictionary<int, string>();
                                        foreach (SegmentDefinitionSourceValueDTO segmentDefinitionSourceValueDTO in segmentDefinitionSourceValueDTOList)
                                        {
                                            ValueList[segmentDefinitionSourceValueDTO.SegmentDefinitionSourceValueId] = segmentDefinitionSourceValueDTO.ListValue + ((string.IsNullOrEmpty(segmentDefinitionSourceValueDTO.Description)) ? "" : " - " + segmentDefinitionSourceValueDTO.Description);
                                        }
                                        cmbCustom.DataSource = new BindingSource(ValueList, null); ;
                                        cmbCustom.ValueMember = "Key";
                                        cmbCustom.DisplayMember = "Value";
                                        cmbCustom.DropDownStyle = ComboBoxStyle.DropDownList;
                                        btnsearch.Tag = ValueList;
                                        btnsearch.Click += new EventHandler(btnDynamicFilter_Click);
                                        flpCategorizationValues.Controls.Add(cmbCustom);
                                        flpCategorizationValues.Controls.Add(btnsearch);
                                    }
                                    break;
                                case "DYNAMIC LIST":
                                    {
                                        string sql = "";
                                        List<SegmentDefinitionSourceValueDTO> segmentDefinitionSourceValueDTOList;
                                        SegmentDefinitionSourceValueList segmentDefinitionSourceValueList = new SegmentDefinitionSourceValueList(segmentCategorizationValueContext);
                                        List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>> segmentDefinitionSourceValueDTOSearchParams;
                                        segmentDefinitionSourceValueDTOSearchParams = new List<KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>>();
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SITE_ID, segmentCategorizationValueContext.GetSiteId().ToString()));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.IS_ACTIVE, "Y"));
                                        segmentDefinitionSourceValueDTOSearchParams.Add(new KeyValuePair<SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters, string>(SegmentDefinitionSourceValueDTO.SearchBySegmentDefinitionSourceValueParameters.SEGMENT_DEFINITION_SOURCE_ID, segmentDefinitionSourceMapDTO.SegmentDefinitionSourceId.ToString()));
                                        segmentDefinitionSourceValueDTOList = segmentDefinitionSourceValueList.GetAllSegmentDefinitionSourceValues(segmentDefinitionSourceValueDTOSearchParams);
                                        //generating the dynamic query bassed on the value given
                                        if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceEntity))
                                        {
                                            MessageBox.Show("Please select the data source entity for the segment " + segmentDefinitionDTO.SegmentName + " in segment source maping.");
                                            return;
                                        }
                                        if (string.IsNullOrEmpty(segmentDefinitionSourceMapDTO.DataSourceColumn))
                                        {
                                            MessageBox.Show("Please select the data source column for the segment " + segmentDefinitionDTO.SegmentName + " in segment source maping.");
                                            return;
                                        }
                                        sql = "Select * from " + segmentDefinitionSourceMapDTO.DataSourceEntity + " " + segmentDefinitionSourceMapDTO.DataSourceEntity.Substring(0, 1);
                                        if (segmentDefinitionSourceValueDTOList != null && segmentDefinitionSourceValueDTOList.Count > 0)
                                        {
                                            sql += " Where " + segmentDefinitionSourceValueDTOList[0].DBQuery;
                                        }
                                        switch (segmentDefinitionSourceMapDTO.DataSourceEntity)
                                        {
                                            //setting the combobox values based on the table
                                            case "VENDOR":
                                                {
                                                    ComboBox cmbCustom = new ComboBox();
                                                    cmbCustom.Width = 150;
                                                    List<VendorDTO> vendorDTOList;// = new List<Core.Vendor.VendorDTO>();
                                                    VendorList vendorList = new VendorList(segmentCategorizationValueContext);
                                                    vendorDTOList = vendorList.GetVendorList(sql);
                                                    if (vendorDTOList == null)
                                                    {
                                                        vendorDTOList = new List<VendorDTO>();
                                                    }
                                                    cmbCustom.Name = segmentDefinitionDTO.SegmentName;
                                                    if (segmentDefinitionDTO.IsMandatory.Equals("Y"))
                                                    {
                                                        cmbCustom.Name += "*";
                                                    }
                                                    cmbCustom.Tag = segmentDefinitionSourceMapDTO;
                                                    cmbCustom.DataSource = vendorDTOList;
                                                    cmbCustom.ValueMember = segmentDefinitionSourceMapDTO.DataSourceColumn;
                                                    cmbCustom.DisplayMember = segmentDefinitionSourceMapDTO.DataSourceColumn;
                                                    cmbCustom.DropDownStyle = ComboBoxStyle.DropDownList;
                                                    flpCategorizationValues.Controls.Add(cmbCustom);
                                                }
                                                break;
                                            case "CATEGORY":
                                                {
                                                    ComboBox cmbCustom = new ComboBox();
                                                    cmbCustom.Width = 150;
                                                    List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
                                                    CategoryList categoryList = new CategoryList(segmentCategorizationValueContext);
                                                    categoryDTOList = categoryList.GetCategoryList(sql);
                                                    if (categoryDTOList == null)
                                                    {
                                                        categoryDTOList = new List<CategoryDTO>();
                                                    }
                                                    cmbCustom.Name = segmentDefinitionDTO.SegmentName;
                                                    if (segmentDefinitionDTO.IsMandatory.Equals("Y"))
                                                    {
                                                        cmbCustom.Name += "*";
                                                    }
                                                    cmbCustom.Tag = segmentDefinitionSourceMapDTO;
                                                    cmbCustom.DataSource = categoryDTOList;
                                                    cmbCustom.ValueMember = segmentDefinitionSourceMapDTO.DataSourceColumn;
                                                    cmbCustom.DisplayMember = segmentDefinitionSourceMapDTO.DataSourceColumn;
                                                    cmbCustom.DropDownStyle = ComboBoxStyle.DropDownList;
                                                    flpCategorizationValues.Controls.Add(cmbCustom);
                                                }
                                                break;
                                        }
                                        break;
                                    }
                            }
                            break;//Because one segment definition can have only one source maping for one applicability
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1000, Applicability));
                Close();
            }
            log.Debug("Ends-createUI() method.");
        }


        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtp_ValueChanged() Event.");
            DateTimePicker dtp = sender as DateTimePicker;
            foreach (Control c in flpCategorizationValues.Controls)
            {
                if (c.Tag != null && c.Tag == dtp.Tag && c.GetType().ToString().Contains("TextBox"))
                {
                    c.Text = dtp.Value.ToString(utilities.ParafaitEnv.DATE_FORMAT);
                    break;
                }
            }
            log.Debug("Ends-dtp_ValueChanged() Event.");
        }
        private void PopulateData()
        {
            log.Debug("Starts-PopulateData() Method.");
            segmentCategorizationValueDTOLoadList = new List<SegmentCategorizationValueDTO>();
            List<SegmentDefinitionSourceMapDTO> segmentDefinitionSourceMapDTOList = new List<SegmentDefinitionSourceMapDTO>();
            SegmentDefinitionSourceMapList segmentDefinitionSourceMapList = new SegmentDefinitionSourceMapList(segmentCategorizationValueContext);
            List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOList = new List<SegmentCategorizationValueDTO>();
            SegmentCategorizationValueList segmentCategorizationValueList = new SegmentCategorizationValueList(segmentCategorizationValueContext);
            List<KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>> segmentCategorizationValueDTOSearchParams;

            segmentCategorizationValueDTOSearchParams = new List<KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>>();
            segmentCategorizationValueDTOSearchParams.Add(new KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SITE_ID, segmentCategorizationValueContext.GetSiteId().ToString()));
            segmentCategorizationValueDTOSearchParams.Add(new KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.IS_ACTIVE, "Y"));
            segmentCategorizationValueDTOSearchParams.Add(new KeyValuePair<SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters, string>(SegmentCategorizationValueDTO.SearchBySegmentCategorizationValueParameters.SEGMENT_CATEGORY_ID, SegmentCategoryId.ToString()));
            segmentCategorizationValueDTOList = segmentCategorizationValueList.GetAllSegmentCategorizationValues(segmentCategorizationValueDTOSearchParams);

            if (SegmentCategoryId != -1)
            {
                lblSegmentId.Text = SegmentCategoryId.ToString();
            }
            else
            {
                lblSegmentId.Text = "";
            }
            lblSegmentCategoryValue.Text = "";
            if (segmentCategorizationValueDTOList != null)
            {
                foreach (SegmentCategorizationValueDTO segmentCategorizationValueDTO in segmentCategorizationValueDTOList)
                {
                    segmentCategorizationValueDTOLoadList.Add(segmentCategorizationValueDTO);
                    foreach (Control c in flpCategorizationValues.Controls)
                    {
                        SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO;
                        if (c.Tag != null)
                        {
                            if (c.GetType().ToString().Contains("Button"))
                            {
                                continue;
                            }
                            segmentDefinitionSourceMapDTO = (SegmentDefinitionSourceMapDTO)c.Tag;
                        }
                        else
                        {
                            continue;
                        }
                        if (segmentDefinitionSourceMapDTO.SegmentDefinitionId == segmentCategorizationValueDTO.SegmentDefinitionId)
                        {
                            switch (segmentDefinitionSourceMapDTO.DataSourceType)
                            {
                                case "DATE":
                                    if (!segmentCategorizationValueDTO.SegmentValueDate.Equals(DateTime.MinValue))
                                    {
                                        if (c.GetType().ToString().Contains("TextBox"))
                                        {
                                            c.Text = segmentCategorizationValueDTO.SegmentValueDate.ToString(utilities.ParafaitEnv.DATE_FORMAT);
                                            if (string.IsNullOrEmpty(lblSegmentCategoryValue.Text))
                                            {
                                                lblSegmentCategoryValue.Text = segmentCategorizationValueDTO.SegmentValueDate.ToString(utilities.ParafaitEnv.DATE_FORMAT);
                                            }
                                            else
                                            {
                                                lblSegmentCategoryValue.Text += "." + segmentCategorizationValueDTO.SegmentValueDate.ToString(utilities.ParafaitEnv.DATE_FORMAT);
                                            }
                                        }
                                        else
                                            (c as DateTimePicker).Value = segmentCategorizationValueDTO.SegmentValueDate;

                                    }
                                    break;
                                case "TEXT":
                                    {
                                        c.Text = segmentCategorizationValueDTO.SegmentValueText;
                                        if (!string.IsNullOrEmpty(segmentCategorizationValueDTO.SegmentValueText))
                                        {
                                            if (string.IsNullOrEmpty(lblSegmentCategoryValue.Text))
                                            {
                                                lblSegmentCategoryValue.Text = segmentCategorizationValueDTO.SegmentValueText;
                                            }
                                            else
                                            {
                                                lblSegmentCategoryValue.Text += "." + segmentCategorizationValueDTO.SegmentValueText;
                                            }
                                        }
                                        break;
                                    }
                                case "STATIC LIST":
                                    (c as ComboBox).SelectedValue = segmentCategorizationValueDTO.SegmentStaticValueId;
                                    if (segmentCategorizationValueDTO.SegmentStaticValueId != -1)
                                    {
                                        if (string.IsNullOrEmpty(lblSegmentCategoryValue.Text))
                                        {
                                            lblSegmentCategoryValue.Text = (c as ComboBox).Text;
                                        }
                                        else
                                        {
                                            lblSegmentCategoryValue.Text += "." + (c as ComboBox).Text;
                                        }
                                    }
                                    break;
                                case "DYNAMIC LIST":
                                    (c as ComboBox).SelectedValue = segmentCategorizationValueDTO.SegmentDynamicValueId;
                                    if (!string.IsNullOrEmpty(segmentCategorizationValueDTO.SegmentDynamicValueId))
                                    {
                                        if (string.IsNullOrEmpty(lblSegmentCategoryValue.Text))
                                        {
                                            lblSegmentCategoryValue.Text = (c as ComboBox).Text;
                                        }
                                        else
                                        {
                                            lblSegmentCategoryValue.Text += "." + (c as ComboBox).Text;
                                        }
                                    }
                                    break;
                            }

                            if (!segmentDefinitionSourceMapDTO.DataSourceType.Equals("DATE"))
                            {
                                break;
                            }
                        }
                    }
                }
            }
            log.Debug("Ends-PopulateData() Method.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click() Event.");
            this.Close();
            log.Debug("Ends-btnCancel_Click() Event.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefresh_Click() Event.");
            lblSaveSuccessful.Visible = false;
            PopulateData();
            log.Debug("Ends-btnRefresh_Click() Event.");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Event.");
            try
            {
                foreach (Control c in flpCategorizationValues.Controls)
                {
                    if ((c.GetType().ToString().Contains("TextBox") || c.GetType().ToString().Contains("ComboBox")) && c.Name.Contains("*") && string.IsNullOrEmpty(c.Text))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(165, c.Name.Replace("*", "")));
                        return;
                    }
                }
                List<SegmentCategorizationValueDTO> segmentCategorizationValueDTOFinalList = new List<SegmentCategorizationValueDTO>();
                if (segmentCategorizationValueDTOLoadList == null)
                {
                    segmentCategorizationValueDTOLoadList = new List<SegmentCategorizationValueDTO>();
                }
                foreach (Control c in flpCategorizationValues.Controls)
                {
                    SegmentCategorizationValueDTO segmentCategorizationValueDTO = new SegmentCategorizationValueDTO();
                    List<SegmentCategorizationValueDTO> segmentCategorizationValueFilteredDTOList = new List<SegmentCategorizationValueDTO>();
                    SegmentDefinitionSourceMapDTO segmentDefinitionSourceMapDTO;
                    if (c.Tag != null && !c.GetType().ToString().Contains("Button"))
                    {
                        segmentDefinitionSourceMapDTO = (SegmentDefinitionSourceMapDTO)c.Tag;
                        segmentCategorizationValueFilteredDTOList = segmentCategorizationValueDTOLoadList.Where(x => (bool)(x.SegmentDefinitionId == segmentDefinitionSourceMapDTO.SegmentDefinitionId)).ToList<SegmentCategorizationValueDTO>();//identifing whether the matching record is there or not
                        if (segmentCategorizationValueFilteredDTOList != null && segmentCategorizationValueFilteredDTOList.Count > 0)
                        {
                            segmentCategorizationValueDTO = segmentCategorizationValueFilteredDTOList[0];//asigning existing segment details to the current dto
                        }
                        segmentCategorizationValueDTO.SegmentDefinitionId = segmentDefinitionSourceMapDTO.SegmentDefinitionId;
                    }
                    else
                    {
                        continue;
                    }
                    switch (segmentDefinitionSourceMapDTO.DataSourceType)
                    {
                        case "DATE":
                            if (!string.IsNullOrEmpty(c.Text))
                            {
                                if (c.GetType().ToString().Contains("TextBox"))
                                {
                                    try
                                    {
                                        segmentCategorizationValueDTO.SegmentValueDate = Convert.ToDateTime(c.Text);
                                    }
                                    catch
                                    {
                                        segmentCategorizationValueDTO.SegmentValueDate = DateTime.MinValue;
                                    }
                                    segmentCategorizationValueDTOFinalList.Add(segmentCategorizationValueDTO);
                                }
                            }
                            break;
                        case "TEXT":
                            {
                                segmentCategorizationValueDTO.SegmentValueText = c.Text;
                                segmentCategorizationValueDTOFinalList.Add(segmentCategorizationValueDTO);
                                break;
                            }
                        case "STATIC LIST":
                            if ((c as ComboBox).SelectedValue != null && (int)(c as ComboBox).SelectedValue != -1)
                            {
                                segmentCategorizationValueDTO.SegmentStaticValueId = (int)(c as ComboBox).SelectedValue;//segmentCategorizationValueDTO.SegmentStaticValueId;
                                segmentCategorizationValueDTOFinalList.Add(segmentCategorizationValueDTO);
                            }
                            else
                            {
                                segmentCategorizationValueDTO.SegmentStaticValueId = -1;//segmentCategorizationValueDTO.SegmentStaticValueId;
                                segmentCategorizationValueDTOFinalList.Add(segmentCategorizationValueDTO);
                            }
                            break;
                        case "DYNAMIC LIST":
                            if ((c as ComboBox).SelectedValue != null)
                            {
                                segmentCategorizationValueDTO.SegmentDynamicValueId = (c as ComboBox).SelectedValue.ToString();//segmentCategorizationValueDTO.SegmentDynamicValueId;
                                segmentCategorizationValueDTOFinalList.Add(segmentCategorizationValueDTO);
                            }
                            break;
                    }
                }
                SegmentCategorizationValue segmentCategorizationValue = new SegmentCategorizationValue(segmentCategorizationValueContext);
                segmentCategorizationValue.SaveSegmentCategorizationValues(segmentCategorizationValueDTOFinalList, PrimaryKey, Applicability);
                if (segmentCategorizationValueDTOFinalList != null)
                {
                    if (segmentCategorizationValueDTOFinalList.Count > 0)
                        SegmentCategoryId = segmentCategorizationValueDTOFinalList[0].SegmentCategoryId;
                }
                PopulateData();
                lblSaveSuccessful.Text = utilities.MessageUtils.getMessage(122);
                lblSaveSuccessful.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Segment Categorization UI");
            }
            log.Debug("Ends-btnSave_Click() Event.");
        }

        private void btnDynamicFilter_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDynamicFilter_Click() Event.");
            txtFilter.Text = "";
            if (!txtFilter.Visible)
            {
                Button btn = (Button)sender;
                var ValueList = new Dictionary<int, string>();
                txtFilter.Width = 150;
                dgvFilter.Width = 150;
                ValueList = (Dictionary<int, string>)btn.Tag;
                dgvFilter.DataSource = new BindingSource(ValueList, null);
                dgvFilter.Columns["Key"].Visible = false;
                dgvFilter.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                txtFilter.Location = new Point((grpBoxSegments.Location.X + flpCategorizationValues.Location.X + btn.Location.X - 155), (grpBoxSegments.Location.Y + flpCategorizationValues.Location.Y + btn.Location.Y + 1));
                dgvFilter.Location = new Point(txtFilter.Left, txtFilter.Bottom);
                txtFilter.Visible = true;
                txtFilter.Tag = ValueList;
                Control cPrevious = null;
                foreach (Control c in flpCategorizationValues.Controls)
                {
                    if (c.Name == btn.Name)
                    {
                        if (cPrevious != null)
                        {
                            dgvFilter.Tag = cPrevious;
                        }
                    }
                    cPrevious = c;
                }
                txtFilter.BringToFront();
            }
            else
            {
                dgvFilter.Visible = txtFilter.Visible = false;
            }
            log.Debug("Ends-btnDynamicFilter_Click() Event.");
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-txtFilter_TextChanged() Event");
            if (loadSearch)
            {
                var ValueList = new Dictionary<int, string>();
                var ValueFilterdList = new Dictionary<int, string>();
                ValueList = (Dictionary<int, string>)txtFilter.Tag;
                if (txtFilter.Text.Length > 0)
                {
                    if (ValueList != null)
                    {
                        ValueFilterdList = (ValueList.Where(x => (bool)((string.IsNullOrEmpty(x.Value) ? "" : x.Value.ToLower()).Contains(txtFilter.Text.ToLower())))).ToDictionary(x => x.Key, x => x.Value);

                        if (ValueFilterdList.Count > 0)
                        {
                            dgvFilter.Visible = true;
                            dgvFilter.DataSource = new BindingSource(ValueFilterdList, null);
                        }
                        else
                        {
                            dgvFilter.Visible = false;
                        }
                    }
                }
                else
                {
                    //cmbAssignedTo.Text = "";
                    dgvFilter.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.Debug("Ends-txtFilter_TextChanged() Event");
        }

        private void dgvFilter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvFilter_CellClick() Event");
            try
            {
                ComboBox cmb = (ComboBox)dgvFilter.Tag;
                txtFilter.Text = dgvFilter.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmb.Text = txtFilter.Text;
                txtFilter.Text = "";
                txtFilter.Visible = false;
                dgvFilter.Visible = false;
            }
            catch { }
            log.Debug("Ends-dgvFilter_CellClick() Event");
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            log.Debug("Starts-txtFilter_KeyDown() event ");
            if (e.KeyCode == Keys.Down)
            {
                if (dgvFilter.Rows.Count > 0)
                {
                    dgvFilter.Focus();
                }
            }
            log.Debug("Ends-txtFilter_KeyDown() event ");
        }

        private void dgvFilter_KeyDown(object sender, KeyEventArgs e)
        {
            log.Debug("Starts-dgvFilter_KeyDown() event ");
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    ComboBox cmb = (ComboBox)dgvFilter.Tag;
                    txtFilter.Text = dgvFilter.SelectedCells[0].Value.ToString();
                    cmb.Text = txtFilter.Text;
                    txtFilter.Text = "";
                    txtFilter.Visible = false;
                    dgvFilter.Visible = false;
                    cmb.Focus();
                }
                catch { }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtFilter.Focus();
            }
            log.Debug("Ends-dgvFilter_KeyDown() event ");
        }
        /// <summary>
        /// To fix the issue hiding the List on click of any controls in the form
        /// </summary>
        /// <param name="control"></param>
        private void RegisterKeyDownHandlers(Control control)//Modification on 29-Apr-2016 to hide the search grid control
        {
            log.Debug("Starts-RegisterKeyDownHandlers() method ");
            foreach (Control ctl in control.Controls)
            {
                if (ctl != txtFilter && loadSearch == false)
                {
                    ctl.Click += MyKeyPressEventHandler;
                    RegisterKeyDownHandlers(ctl);
                }
            }
            log.Debug("Ends-RegisterKeyDownHandlers() method ");
        }//Ends modification on 29-Apr-2016 to hide the search grid control

        public void MyKeyPressEventHandler(Object sender, EventArgs e)//Modification on 29-Apr-2016 to hide the search grid control
        {
            log.Debug("Starts-MyKeyPressEventHandler() Event ");
            dgvFilter.Visible = txtFilter.Visible = false;
            loadSearch = false;
            log.Debug("Ends-MyKeyPressEventHandler() Event ");
        }//Ends modification on 29-Apr-2016 to hide the search grid control

        private void SegmentCategorizationValueUI_MouseClick(object sender, MouseEventArgs e)
        {
            log.Debug("Starts-GenericAssetUI_MouseClick() Event ");
            dgvFilter.Visible = txtFilter.Visible = false;
            loadSearch = false;
            log.Debug("Ends-GenericAssetUI_MouseClick() Event ");
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
            log.LogMethodExit();
        }

    }
}
