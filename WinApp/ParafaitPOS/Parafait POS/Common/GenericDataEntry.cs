/********************************************************************************************
 * Project Name - Common
 * Description  - UI Class for GenericDataEntry
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019      Girish Kundar        Modified : Added Logger methods and Removed unused namespace's 
 *2.130.7      13-Apr-2022      Guru S A             Payment mode OTP validation changes
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Parafait_POS
{
    public partial class GenericDataEntry : Form
    { 
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        private Utilities Utilities = POSStatic.Utilities;
        private Semnox.Core.GenericUtilities.TagNumberParser tagNumberParser;

        public class DataTypes
        {
            public const string String = "String";
            public const string Integer = "Integer";
            public const string Number = "Number";
            public const string DateTime = "DateTime";
            public const string Hexadecimal = "Hexadecimal";
            public const string StringList = "StringList";
        }

        public class DataEntryElement
        {
            public string label = "default";
            public string data = "";
            public bool mandatory = false;
            public string dataType;
            public bool allowMinusSign = false;
            public int width = 300;
            public int height = 22;
            public bool multiline = false;
            public bool unique = false;
            public string uniqueInTable;
            public string uniqueColumn;
            public bool uppercase;
            public int maxlength = 100; 
            public List<KeyValuePair<string, string>> listDataSource = null;
            public bool readOnly = false;
            public object tagValue = null;
        }

        //Added for merkle Integartion
        public bool gridControlRequired = false;
        public bool couponShow = false;
        DataGridView DgvCouponNumbers = new DataGridView();
        FlowLayoutPanel fpCouponGrid = new FlowLayoutPanel();
        DataTable customerCouponsDT = new DataTable();
        CustomerDTO customerDTO;
       //end

        public DataEntryElement[] DataEntryObjects;
        public GenericDataEntry(int ElementCount)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(ElementCount);//Added for logger function on 08-Mar-2016

            POSStatic.Utilities.setLanguage();
            InitializeComponent();
            DataEntryObjects = new DataEntryElement[ElementCount];
            for (int i = 0; i < ElementCount; i++)
            {
                DataEntryObjects[i] = new DataEntryElement();
            }
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            log.LogMethodExit();
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                tagNumberParser = new Semnox.Core.GenericUtilities.TagNumberParser(Utilities.ExecutionContext);
                Semnox.Core.GenericUtilities.TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    MessageBox.Show(message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                string CardNumber = tagNumber.Value;
                log.LogVariableState("CardNumber", CardNumber);
                btnShowNumPad.Enabled = false;
                cardSwiped(CardNumber, sender as DeviceClass);
            }
            log.LogMethodExit();
        }
        private void cardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(CardNumber, readerDevice);
            if (CardNumber != null)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (DataEntryObjects.Length == 1 && flpLeftRight.Controls[1] != null)
                    {
                        flpLeftRight.Controls[1].Text = CardNumber;
                        if (!btnShowNumPad.Enabled)
                        {
                            btnOK_Click(null, null);
                        }
                    }
                    else if (DataEntryObjects.Length == 2 && flpLeftRight.Controls[1] != null)
                    {
                        flpLeftRight.Controls[1].Text = CardNumber;
                    }
                });
            }
            log.LogMethodExit();
        }
        //Added on 17-Feb-2017 for coupons list show for customer
        public GenericDataEntry(CustomerDTO customerDTO , bool couponShow, int elementCount): this(elementCount)
        {
            log.LogMethodEntry();
            if (customerDTO != null)
            {
                this.customerDTO = customerDTO;
                this.gridControlRequired = true;
            }
            this.couponShow = couponShow;
            log.LogMethodExit();
        }//end

        //public GenericDataEntry(bool btnShowNumPad, int elementCount) : this(elementCount)
        //{
        //    log.LogMethodEntry();

        //    this.btnShowNumPad.Visible = btnShowNumPad;
           
        //    //this.flpLeftRight.Controls[1] = 
            
        //    log.LogMethodExit();
        //}//end

        //Added on 30-jan-2016 for register the devices and read the barcode number
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string scannedBarcode = POSStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, POSStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, POSStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                log.LogVariableState("scannedBarcode" , scannedBarcode);
                this.Invoke((MethodInvoker)delegate
                {
                    //check the textbox is for coupon number
                    if (DataEntryObjects.Length == 1 && flpLeftRight.Controls[1] != null)
                    {
                        flpLeftRight.Controls[1].Text = scannedBarcode;
                        if (!btnShowNumPad.Enabled)
                            btnOK_Click(null, null);
                    }
                    else if (DataEntryObjects.Length == 2 && flpLeftRight.Controls[1] != null)
                    {
                        flpLeftRight.Controls[1].Text = scannedBarcode;
                    }
                });
            }
            log.LogMethodExit();
        }//end

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int i = 1;

            foreach (DataEntryElement de in DataEntryObjects)
            {
                if (de.dataType == DataTypes.StringList)
                {
                    Semnox.Core.GenericUtilities.AutoCompleteComboBox comboBox = (Semnox.Core.GenericUtilities.AutoCompleteComboBox)flpLeftRight.Controls[i];
                    if (comboBox.SelectedIndex > -1 && comboBox.SelectedValue != null)
                    {
                        de.data = comboBox.SelectedValue.ToString();
                    }
                    else { de.data = string.Empty; }
                }
                else
                {
                    de.data = flpLeftRight.Controls[i].Text.Trim();
                }
                if (de.mandatory && String.Empty.Equals(de.data))
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(165, de.label));
                    log.Info("Ends-btnOK_Click() as " + de.label + " is mandatory");//Added for logger function on 08-Mar-2016
                    this.ActiveControl = flpLeftRight.Controls[i];

                    return;
                }



                if (de.unique && !string.IsNullOrEmpty(de.data) && !string.IsNullOrEmpty(de.uniqueInTable) && !string.IsNullOrEmpty(de.uniqueColumn))
                {
                    if (POSStatic.Utilities.executeScalar(@"select top 1 1
                                                                from " + de.uniqueInTable +
                                                                " where " + de.uniqueColumn + " = '" + de.data + "'") != null)
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(166, de.label, de.data));
                        log.Info("Ends-btnOK_Click() as " + de.label + " must be unique. Value " + de.data + " already exists.");//Added for logger function on 08-Mar-2016
                        this.ActiveControl = flpLeftRight.Controls[i];
                        (flpLeftRight.Controls[i] as TextBox).SelectAll();
                        return;
                    }
                }

                i += 2;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }



        private void GenericDataEntry_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach(DataEntryElement de in DataEntryObjects)
            {
                this.Height += de.height;
                FlowLayoutPanel fplbl = new FlowLayoutPanel();
                fplbl.Width = (int)(flpLeftRight.Width * .33);
                fplbl.Height = 30;

                fplbl.FlowDirection = FlowDirection.RightToLeft;
                Label lbl = new Label();
                lbl.Text = de.label + ":";
                lbl.AutoSize = true;
                fplbl.Controls.Add(lbl);
                flpLeftRight.Controls.Add(fplbl);
                if (de.dataType == DataTypes.StringList)
                {
                    AddDropDown(de);
                }
                else
                {
                    AddTextBox(de);
                }
            }

            if (couponShow)
            {
                btnShowNumPad.Enabled = false;
                btnCancel.Visible = false;
                btnOK.Visible = false;
            }
            //Added for Merkle Integarion
            if (gridControlRequired)
            {
                UpdateGridProperties();

                if (couponShow)
                {
                    //btnShowNumPad.Enabled = false;
                    //btnCancel.Visible = false;
                    //btnOK.Visible = false;
                    btnContinue.Visible = true;
                    btnContinue.Location = new Point(btnContinue.Location.X + 70, btnContinue.Location.Y);
                    DgvCouponNumbers.Columns[0].ReadOnly = true;
                    DgvCouponNumbers.Columns["SL No"].Visible = true;
                    DgvCouponNumbers.Columns["Select"].Visible = false;
                    btnShowNumPad.Visible = false;
                }
                else
                {
                    //DgvCouponNumbers.Columns["SL No"].ReadOnly = false;
                    btnShowNumPad.Enabled = true;
                    btnCancel.Visible = true;
                    btnCancel.Location = new Point(btnCancel.Location.X + 70, btnCancel.Location.Y);
                    btnOK.Visible = true;
                    btnOK.Location = new Point(btnOK.Location.X + 70, btnOK.Location.Y);
                    btnContinue.Visible = false;
                    btnShowNumPad.Visible = true;
                    DgvCouponNumbers.Columns["Select"].Visible = true;
                    DgvCouponNumbers.Columns["SL No"].Visible = false;
                }
                this.Height = this.Height + DgvCouponNumbers.Height - 25;
                this.Width += DgvCouponNumbers.Width / 4;
                this.Location = new Point(this.Location.X, this.Location.Y - 150);
                fpCouponGrid.Height = DgvCouponNumbers.Height;
                fpCouponGrid.Width = DgvCouponNumbers.Width;
            }

            if (DataEntryObjects.Length == 1)
            {
                this.Height -= 20;
            }
            //end
            POSStatic.Utilities.setLanguage(this);
            this.Location = new Point(this.Location.X, this.Location.Y - 60);
            log.LogMethodExit();
        }        

        //Start Modification for merkle Integartion 
        void UpdateGridProperties()
        {
            log.LogMethodEntry();
            lblCustomer.Text = "Customer Name : " + customerDTO.FirstName + "       Phone No : " + customerDTO.PhoneNumber;
            lblCustomer.Visible = true;

            DgvCouponNumbers.AllowUserToAddRows = false;
            DgvCouponNumbers.AllowUserToResizeRows = false;
            DgvCouponNumbers.MultiSelect = false;
            DgvCouponNumbers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // DgvCouponNumbers.CellContentClick += new DataGridViewCellEventHandler(DgvCouponNumbers_CellContentClick);
            DgvCouponNumbers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(DgvCouponNumbers_CellClick);
            DgvCouponNumbers.RowTemplate.Height = 30;

            DgvCouponNumbers.Height = 200;
            DgvCouponNumbers.Width = 630;
            FlowLayoutPanel fplbl = new FlowLayoutPanel();
            fplbl.Width = (int)(flpLeftRight.Width * .65);
            fplbl.Height = 20;

            fplbl.FlowDirection = FlowDirection.RightToLeft;
            Label lbl = new Label();
            lbl.Text = "You have following coupons available to redeem.";
            Font lblFont = new Font(new FontFamily("Microsoft Sans Serif"), 9.75F);
            lbl.Font = lblFont;

            lbl.AutoSize = true;
            fplbl.Controls.Add(lbl);
            flpLeftRight.Controls.Add(fplbl);
            fpCouponGrid.Width = 700;
            fpCouponGrid.Controls.Add(DgvCouponNumbers);
            flpLeftRight.Controls.Add(fpCouponGrid);
            this.Height += flpLeftRight.Height;

            DgvCouponNumbers.DataSource = customerDTO.CustomerCuponsDT;

            DgvCouponNumbers.Columns["code"].HeaderText = "Coupon Number";
            DgvCouponNumbers.Columns["expires_at"].HeaderText = "Expiry Date";
            DgvCouponNumbers.Columns["description"].HeaderText = "Description";

            DgvCouponNumbers.Columns["SL No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["SL No"].Width = 50;
            DgvCouponNumbers.Columns["Select"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["Select"].Width = 60;

            DgvCouponNumbers.Columns["code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["code"].Width = 120;
            DgvCouponNumbers.Columns["Discount Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["Discount Name"].Width = 130;
            DgvCouponNumbers.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["Value"].Width = 80;
            DgvCouponNumbers.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["description"].Width = 120;
            DgvCouponNumbers.Columns["expires_at"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            DgvCouponNumbers.Columns["expires_at"].Width = 100;

            for (int colindex = 0; colindex < DgvCouponNumbers.Columns.Count; colindex++)
            {
                DgvCouponNumbers.Columns[colindex].ReadOnly = true;
            }

            POSStatic.Utilities.setupDataGridProperties(ref DgvCouponNumbers);

            DgvCouponNumbers.ScrollBars = ScrollBars.Both;
            fpCouponGrid.AutoScroll = false;
            DgvCouponNumbers.RowHeadersVisible = false;
            foreach (DataGridViewColumn c in DgvCouponNumbers.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13, GraphicsUnit.Pixel);
            }
            log.LogMethodExit();
        }

        int previousRowIndex = -1;
        private void DgvCouponNumbers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //When Previous row and current row is same
                if (previousRowIndex == e.RowIndex)
                {
                    if (!couponShow && DgvCouponNumbers.Columns[e.ColumnIndex].Name == "Select")
                    {
                        if (Convert.ToBoolean(DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value) == false)
                        {
                            AssignCouponNumber(DgvCouponNumbers.Rows[e.RowIndex].Cells["code"].Value.ToString());
                            DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value = true;
                        }
                        else
                        {
                            AssignCouponNumber("");
                            DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value = false;
                        }
                    }
                    return;
                }
                previousRowIndex = e.RowIndex;
                UnCheckSelectedCoupons();
                if (!couponShow)
                {
                    if (Convert.ToBoolean(DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value) == false)
                    {
                        // assign the coupon number to textbox when check the coupon rows
                        AssignCouponNumber(DgvCouponNumbers.Rows[e.RowIndex].Cells["code"].Value.ToString());
                        DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value = true;
                    }
                    else
                    {
                        // clear the textbox when uncheck the coupon rows
                        AssignCouponNumber("");
                        DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value = false;
                    }
                }
                else
                {
                    DgvCouponNumbers.Rows[e.RowIndex].Cells["Select"].Value = false;
                    AssignCouponNumber("");
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error("Exception : ", ex);
            }
        }

        void AssignCouponNumber(string couponNumber)
        {
            log.LogMethodEntry(couponNumber);
            if (DataEntryObjects.Length == 1 && DataEntryObjects[0].label == POSStatic.Utilities.MessageUtils.getMessage("Coupon Number") && flpLeftRight.Controls[1] != null)
            {
                flpLeftRight.Controls[1].Text = couponNumber;
            }
            log.LogMethodExit();
        }

        void UnCheckSelectedCoupons()
        {
            log.LogMethodEntry();
            for (int rwIndex = 0; rwIndex < DgvCouponNumbers.Rows.Count; rwIndex++)
            {
                DgvCouponNumbers.Rows[rwIndex].Cells["Select"].Value = false;
            }
            log.LogMethodExit();
        }
        //End Modification for merkle Integartion 

        Control currentTextBox;
        void txt_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            currentTextBox = sender as Control;
            log.LogMethodExit();
        }
        void cmb_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            currentTextBox = sender as Control;
            log.LogMethodExit();
        }

        void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != POSStatic.decimalChar && e.KeyChar != '-' && !char.IsControl(e.KeyChar))
                e.Handled = true;

            if (((sender as TextBox).Tag as DataEntryElement).allowMinusSign == false && e.KeyChar == '-')
                e.Handled = true;

            log.LogMethodExit();
        }

        void txt_KeyPressHexa(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (((!char.IsControl(e.KeyChar))
                    && !(e.KeyChar >= '0' && e.KeyChar <= '9')
                    && !(e.KeyChar >= 'a' && e.KeyChar <= 'f')
                    && !(e.KeyChar >= 'A' && e.KeyChar <= 'F')))
                e.Handled = true;

            log.LogMethodExit();
        }

        void txt_KeyPressInt(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-')
                e.Handled = true;

            if (((sender as TextBox).Tag as DataEntryElement).allowMinusSign == false && e.KeyChar == '-')
                e.Handled = true;
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();//Added for logger function on 08-Mar-2016

            //Start Modification Merkle Integration
            //Added for Resizing the form location when keypad is opened or closed
            if (gridControlRequired && !couponShow)
            {
                if (keypad == null || !keypad.Visible || keypad.IsDisposed)
                {
                    //On Opening keypad resize the form location to top
                    this.Location = new Point(this.Location.X, 0);
                }
                else if (keypad.Visible)
                {
                    //On closing keypad resize the form to orginal location
                    this.Location = new Point(this.Location.X, 110);
                }
            }//End Modification Merkle Integration
            //skip AutoCompleteComboBox as keyboard does not support the same
            if (currentTextBox != null && currentTextBox.GetType().Name == "AutoCompleteComboBox")
            {
                if (keypad != null)
                {
                    keypad.Hide();
                }
            }
            else
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, currentTextBox);
                    keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                    keypad.Show();
                }
                else if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Show();
                }
            }
            log.LogMethodExit();
        }

        private void GenericDataEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (keypad != null)
                keypad.Close();

            if (Common.Devices.PrimaryBarcodeScanner != null)
                Common.Devices.PrimaryBarcodeScanner.UnRegister();

            log.LogMethodExit();
        }

        private void AddTextBox(DataEntryElement de)
        {
            log.LogMethodEntry(de);
            TextBox txt = new TextBox();
            txt.Tag = de;
            txt.Width = de.width;
            txt.MaxLength = de.maxlength;
            if (de.multiline)
            {
                txt.Multiline = de.multiline;
                txt.Height = de.height;
                txt.AcceptsReturn = true;
            }
            if (de.uppercase)
                txt.CharacterCasing = CharacterCasing.Upper;
            txt.Text = de.data;
            if (couponShow || de.readOnly)
            {
                txt.ReadOnly = true;
            }
            flpLeftRight.Controls.Add(txt);
            txt.Enter += new EventHandler(txt_Enter);

            switch (de.dataType)
            {
                case DataTypes.Hexadecimal: txt.TextAlign = HorizontalAlignment.Left; txt.KeyPress += new KeyPressEventHandler(txt_KeyPressHexa); break;
                case DataTypes.Number: txt.TextAlign = HorizontalAlignment.Right; txt.KeyPress += new KeyPressEventHandler(txt_KeyPress); break;
                case DataTypes.Integer: txt.TextAlign = HorizontalAlignment.Right; txt.KeyPress += new KeyPressEventHandler(txt_KeyPressInt); break;
                default: break;
            }
            log.LogMethodExit();
        }

        private void AddDropDown(DataEntryElement de)
        {
            log.LogMethodEntry(de);
            Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbBox = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            cmbBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cmbBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            //cmbBox.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            cmbBox.FormattingEnabled = true;
            cmbBox.Tag = de;
            cmbBox.Width = de.width;
            cmbBox.MaxLength = de.maxlength;
            cmbBox.DataSource = de.listDataSource;
            cmbBox.ValueMember = "Key";
            cmbBox.DisplayMember = "Value";
            cmbBox.Enter += new EventHandler(cmb_Enter);
            if (de.readOnly)
            {
                cmbBox.Enabled = false;
            } 
            flpLeftRight.Controls.Add(cmbBox);

            if (string.IsNullOrWhiteSpace(de.data) == false && de.listDataSource != null)
            {
                List<int> indices = new List<int>();
                int index = -1;
                de.listDataSource.ForEach((pair) => { if (pair.Value.Equals(de.data)) indices.Add(index); index++; });
                if (index > -1)
                {
                    cmbBox.SelectedIndex = index;
                }
            } 
            log.LogMethodExit();
        }
        /// <summary>
        /// GenerateListDataSource
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GenerateListDataSource(List<string> stringList)
        {
            log.LogMethodEntry(stringList);
            List<KeyValuePair<string, string>> resultList = new List<KeyValuePair<string, string>>();
            if (stringList !=null && stringList.Any())
            {
                for (int i = 0; i < stringList.Count; i++)
                {
                    resultList.Add(new KeyValuePair<string, string>(stringList[i], stringList[i]));
                }
            }
            log.LogMethodExit(resultList);
            return resultList;
        }         
    }
}
