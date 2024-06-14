using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Devices;
using Semnox.Core.Customer;

namespace Semnox.Parafait.GenericUI
{
    public partial class GenericDataEntry : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Core.Logger log = new Semnox.Core.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public class DataTypes
        {
            public const string String = "String";
            public const string Integer = "Integer";
            public const string Number = "Number";
            public const string DateTime = "DateTime";
            public const string Hexadecimal = "Hexadecimal";
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
            ParafaitUtils.Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-GenericDataEntry(" + ElementCount + ")");//Added for logger function on 08-Mar-2016

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
            log.Debug("Ends-GenericDataEntry(" + ElementCount + ")");//Added for logger function on 08-Mar-2016
        }

        //Added on 17-Feb-2017 for coupons list show for customer
        public GenericDataEntry(CustomerDTO customerDTO , bool couponShow, int elementCount): this(elementCount)
        {
            this.customerDTO = customerDTO;
            this.gridControlRequired = true;
            this.couponShow = couponShow;           
        }//end

        //Added on 30-jan-2016 for register the devices and read the barcode number
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-BarCodeScanCompleteEventHandle()");//Added for logger function on 08-Mar-2016
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string scannedBarcode = POSStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, POSStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, POSStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);

                this.Invoke((MethodInvoker)delegate
                {
                    //check the textbox is for coupon number
                    if (DataEntryObjects.Length == 1 && DataEntryObjects[0].label == POSStatic.Utilities.MessageUtils.getMessage("Coupon Number") && flpLeftRight.Controls[1] != null)
                    {
                        flpLeftRight.Controls[1].Text = scannedBarcode;
                        btnOK_Click(null, null);
                    }
                });
            }
            log.Debug("Ends-BarCodeScanCompleteEventHandle()");//Added for logger function on 08-Mar-2016
        }//end

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnOK_Click()");//Added for logger function on 08-Mar-2016
            int i = 1;
            foreach (DataEntryElement de in DataEntryObjects)
            {
                de.data = flpLeftRight.Controls[i].Text.Trim();
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
            log.Debug("Ends-btnOK_Click()");//Added for logger function on 08-Mar-2016
        }

        private void GenericDataEntry_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-GenericDataEntry_Load()");//Added for logger function on 08-Mar-2016
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
                flpLeftRight.Controls.Add(txt);
                txt.Enter += new EventHandler(txt_Enter);

                switch (de.dataType)
                {
                    case DataTypes.Hexadecimal: txt.TextAlign = HorizontalAlignment.Left; txt.KeyPress += new KeyPressEventHandler(txt_KeyPressHexa); break;
                    case DataTypes.Number: txt.TextAlign = HorizontalAlignment.Right; txt.KeyPress += new KeyPressEventHandler(txt_KeyPress); break;
                    case DataTypes.Integer: txt.TextAlign = HorizontalAlignment.Right; txt.KeyPress += new KeyPressEventHandler(txt_KeyPressInt); break;
                    default: break;
                }
            }

            //Added for Merkle Integarion
            if (gridControlRequired)
            {
                UpdateGridProperties();

                if (couponShow)
                {
                    btnShowNumPad.Enabled = false;
                    btnCancel.Visible = false;
                    btnOK.Visible = false;
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
            log.Debug("Ends-GenericDataEntry_Load()");//Added for logger function on 08-Mar-2016
        }

        //Start Modification for merkle Integartion 
        void UpdateGridProperties()
        {
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
        }

        int previousRowIndex = -1;
        private void DgvCouponNumbers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
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
            }
            catch { }
        }

        void AssignCouponNumber(string couponNumber)
        {
            if (DataEntryObjects.Length == 1 && DataEntryObjects[0].label == POSStatic.Utilities.MessageUtils.getMessage("Coupon Number") && flpLeftRight.Controls[1] != null)
            {
                flpLeftRight.Controls[1].Text = couponNumber;
            }
        }

        void UnCheckSelectedCoupons()
        {
            for (int rwIndex = 0; rwIndex < DgvCouponNumbers.Rows.Count; rwIndex++)
            {
                DgvCouponNumbers.Rows[rwIndex].Cells["Select"].Value = false;
            }
        }
        //End Modification for merkle Integartion 

        TextBox currentTextBox;
        void txt_Enter(object sender, EventArgs e)
        {
            currentTextBox = sender as TextBox;
        }

        void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != POSStatic.decimalChar && e.KeyChar != '-' && !char.IsControl(e.KeyChar))
                e.Handled = true;

            if (((sender as TextBox).Tag as DataEntryElement).allowMinusSign == false && e.KeyChar == '-')
                e.Handled = true;
        }

        void txt_KeyPressHexa(object sender, KeyPressEventArgs e)
        {
            if (((!char.IsControl(e.KeyChar))
                    && !(e.KeyChar >= '0' && e.KeyChar <= '9')
                    && !(e.KeyChar >= 'a' && e.KeyChar <= 'f')
                    && !(e.KeyChar >= 'A' && e.KeyChar <= 'F')))
                e.Handled = true;
        }

        void txt_KeyPressInt(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-')
                e.Handled = true;

            if (((sender as TextBox).Tag as DataEntryElement).allowMinusSign == false && e.KeyChar == '-')
                e.Handled = true;
        }

        ParafaitUtils.AlphaNumericKeyPad keypad;
        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnShowNumPad_Click()");//Added for logger function on 08-Mar-2016

            //Start Modification Merkle Integration
            //Added for Resizing the form location when keypad is opened or closed
            if(gridControlRequired && !couponShow)
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

            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new ParafaitUtils.AlphaNumericKeyPad(this, currentTextBox);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width)/ 2, this.Location.Y + this.Height - 10);
                keypad.Show();
            }
            else if (keypad.Visible)
                keypad.Hide();
            else
            {
                keypad.Show();
            }
            log.Debug("Ends-btnShowNumPad_Click()");//Added for logger function on 08-Mar-2016
        }

        private void GenericDataEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-GenericDataEntry_FormClosing()");//Added for logger function on 08-Mar-2016
            if (keypad != null)
                keypad.Close();

            if (Common.Devices.PrimaryBarcodeScanner != null)
                Common.Devices.PrimaryBarcodeScanner.UnRegister();

            log.Debug("Ends-GenericDataEntry_FormClosing()");//Added for logger function on 08-Mar-2016
        }
    }
}
