/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*1.00        03-Feb-2017            Soumya                      New form used by product screens
 *                                                              to generate barcode
*2.0         26-Sep-2018            Mathew Ninan                Changed to refer to Printer BL
*2.70.2        12-Aug-2019            Deeksha                     Added logger methods.
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Semnox.Parafait.BarcodeUtilities
{
    public class frm_ProductBarcode : System.Windows.Forms.Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string barcode = "";
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button cmdMakeBarcode;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label2;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button cmdPrint;
        private Button cb_exit;
        private Button cb_OK;
        Utilities utilities;
        private Label label4;
        private Label label3;
        private CheckBox chkShowDescription;
        private CheckBox chkShowPrice;
        double productPrice;
        string productDescription;
        private PictureBox pictBarcode;
        private System.ComponentModel.Container components = null;

        public frm_ProductBarcode(string inputString, string description, double price, Utilities _Utilities)
        {
            log.LogMethodEntry(inputString, description, price, _Utilities);
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            txtInput.Text = inputString;
            productPrice = price;
            productDescription = description;
            utilities = _Utilities;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            log.LogMethodExit();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            log.LogMethodEntry(disposing);
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
            log.LogMethodExit();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            log.LogMethodEntry();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ProductBarcode));
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cmdMakeBarcode = new System.Windows.Forms.Button();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.cb_exit = new System.Windows.Forms.Button();
            this.cb_OK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShowDescription = new System.Windows.Forms.CheckBox();
            this.chkShowPrice = new System.Windows.Forms.CheckBox();
            this.pictBarcode = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to encode:";
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(133, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(277, 20);
            this.txtInput.TabIndex = 1;
            // 
            // cmdMakeBarcode
            // 
            this.cmdMakeBarcode.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdMakeBarcode.Location = new System.Drawing.Point(186, 87);
            this.cmdMakeBarcode.Name = "cmdMakeBarcode";
            this.cmdMakeBarcode.Size = new System.Drawing.Size(92, 23);
            this.cmdMakeBarcode.TabIndex = 2;
            this.cmdMakeBarcode.Text = "Make barcode";
            this.cmdMakeBarcode.UseVisualStyleBackColor = false;
            this.cmdMakeBarcode.Click += new System.EventHandler(this.cmdMakeBarcode_Click);
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(136, 88);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(44, 20);
            this.txtWeight.TabIndex = 4;
            this.txtWeight.Text = "2";
            this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(46, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bar weight:";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // cmdPrint
            // 
            this.cmdPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrint.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdPrint.Location = new System.Drawing.Point(254, 213);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(75, 23);
            this.cmdPrint.TabIndex = 7;
            this.cmdPrint.Text = "Print";
            this.cmdPrint.UseVisualStyleBackColor = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cb_exit
            // 
            this.cb_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cb_exit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_exit.Location = new System.Drawing.Point(170, 213);
            this.cb_exit.Name = "cb_exit";
            this.cb_exit.Size = new System.Drawing.Size(66, 23);
            this.cb_exit.TabIndex = 6;
            this.cb_exit.Text = "Exit";
            this.cb_exit.UseVisualStyleBackColor = true;
            this.cb_exit.Click += new System.EventHandler(this.cb_exit_Click);
            // 
            // cb_OK
            // 
            this.cb_OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_OK.Location = new System.Drawing.Point(346, 213);
            this.cb_OK.Name = "cb_OK";
            this.cb_OK.Size = new System.Drawing.Size(66, 23);
            this.cb_OK.TabIndex = 8;
            this.cb_OK.Text = "OK";
            this.cb_OK.UseVisualStyleBackColor = true;
            this.cb_OK.Click += new System.EventHandler(this.cb_OK_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(13, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 20);
            this.label4.TabIndex = 31;
            this.label4.Text = "Show Description:";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(39, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = "Show Price:";
            // 
            // chkShowDescription
            // 
            this.chkShowDescription.AutoSize = true;
            this.chkShowDescription.Checked = true;
            this.chkShowDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowDescription.Location = new System.Drawing.Point(133, 65);
            this.chkShowDescription.Name = "chkShowDescription";
            this.chkShowDescription.Size = new System.Drawing.Size(15, 14);
            this.chkShowDescription.TabIndex = 29;
            this.chkShowDescription.UseVisualStyleBackColor = true;
            // 
            // chkShowPrice
            // 
            this.chkShowPrice.AutoSize = true;
            this.chkShowPrice.Checked = true;
            this.chkShowPrice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPrice.Location = new System.Drawing.Point(134, 42);
            this.chkShowPrice.Name = "chkShowPrice";
            this.chkShowPrice.Size = new System.Drawing.Size(15, 14);
            this.chkShowPrice.TabIndex = 28;
            this.chkShowPrice.UseVisualStyleBackColor = true;
            // 
            // pictBarcode
            // 
            this.pictBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBarcode.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictBarcode.Location = new System.Drawing.Point(11, 116);
            this.pictBarcode.Name = "pictBarcode";
            this.pictBarcode.Size = new System.Drawing.Size(399, 84);
            this.pictBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictBarcode.TabIndex = 33;
            this.pictBarcode.TabStop = false;
            // 
            // frm_ProductBarcode
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.cb_exit;
            this.ClientSize = new System.Drawing.Size(422, 248);
            this.Controls.Add(this.pictBarcode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkShowDescription);
            this.Controls.Add(this.chkShowPrice);
            this.Controls.Add(this.cb_OK);
            this.Controls.Add(this.cb_exit);
            this.Controls.Add(this.cmdPrint);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdMakeBarcode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_ProductBarcode";
            this.Text = "Generate BarCode";
            ((System.ComponentModel.ISupportInitialize)(this.pictBarcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            log.LogMethodExit();
        }
        #endregion

        private void cmdMakeBarcode_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                pictBarcode.Refresh();
                PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext);
                //Image myimg = Code128Rendering.MakeBarcodeImage(txtInput.Text, int.Parse(txtWeight.Text), true);
                Image myimg = printerBL.MakeBarcodeLibImage(int.Parse(txtWeight.Text), 40, BarcodeLib.TYPE.CODE128.ToString(), txtInput.Text);
                int width = pictBarcode.Width;
                int height = pictBarcode.Height;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                int yLocation = 0;
                int fontSize = 8;
                if (Convert.ToInt32(txtWeight.Text) == 1)
                    fontSize = 8;
                else if (Convert.ToInt32(txtWeight.Text) == 2)
                    fontSize = 9;
                else if (Convert.ToInt32(txtWeight.Text) == 3)
                    fontSize = 10;
                else if (Convert.ToInt32(txtWeight.Text) >= 4)
                    fontSize = 12;

                Bitmap bmp = new Bitmap(pictBarcode.Width, pictBarcode.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    using (Font myFont = new Font("Arial", fontSize, FontStyle.Regular))
                    {
                        string str = productPrice == -1 ? "" : productPrice.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                        SizeF stringSize = new SizeF();
                        stringSize = g.MeasureString(str, myFont);
                        g.DrawString(txtInput.Text, myFont, Brushes.Black, 0, yLocation);
                        if (chkShowPrice.Checked)
                            g.DrawString(str, myFont, Brushes.Black, myimg.Width - stringSize.Width, yLocation);
                        yLocation += Convert.ToInt32(stringSize.Height);
                        g.DrawImage(
                                    //Code128Rendering.MakeBarcodeImage(txtInput.Text, int.Parse(txtWeight.Text), true),
                                    printerBL.MakeBarcodeLibImage(int.Parse(txtWeight.Text), 40, BarcodeLib.TYPE.CODE128.ToString(), txtInput.Text),
                                    new Rectangle(0, yLocation, width, height),  // destination rectangle  
                                    0,
                                    0,           // upper-left corner of source rectangle
                                    width,       // width of source rectangle
                                    height,      // height of source rectangle
                                    GraphicsUnit.Pixel,
                                    null);
                        yLocation += myimg.Height;
                        if (chkShowDescription.Checked)
                            g.DrawString(productDescription, myFont, Brushes.Black, 0, yLocation);
                    }
                }
                pictBarcode.Image = bmp;
            }
            catch (Exception ex)
            {
                log.Error("Error while executing cmdMakeBarcode_Click()" + ex.Message);
                MessageBox.Show(this, ex.Message, this.Text);
            }
            log.LogMethodExit();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                using (Graphics g = e.Graphics)
                {
                    if (Convert.ToInt32(txtWeight.Text) == 1)
                    {
                        g.DrawImage(pictBarcode.Image, 50, 110);
                    }
                    else if (Convert.ToInt32(txtWeight.Text) == 2)
                    {
                        g.DrawImage(pictBarcode.Image, 60, 130);
                    }
                    else if (Convert.ToInt32(txtWeight.Text) == 3)
                    {
                        g.DrawImage(pictBarcode.Image, 70, 150);
                    }
                    else if (Convert.ToInt32(txtWeight.Text) >= 4)
                    {
                        g.DrawImage(pictBarcode.Image, 80, 170);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error while executing printDocument1_PrintPage()" + ex.Message);
                MessageBox.Show(this, ex.Message, this.Text);
            }
            log.LogMethodExit();
        }

        private void cmdPrint_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (pictBarcode.Image == null)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(145));
                    log.LogMethodExit();
                    return;
                }

                PrintDialog MyPrintDialog = new PrintDialog();
                MyPrintDialog.AllowCurrentPage = false;
                MyPrintDialog.AllowPrintToFile = false;
                MyPrintDialog.AllowSelection = false;
                MyPrintDialog.AllowSomePages = false;
                MyPrintDialog.PrintToFile = false;
                MyPrintDialog.ShowHelp = false;
                MyPrintDialog.ShowNetwork = false;
                MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = true;

                if (MyPrintDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.DefaultPageSettings =
                    MyPrintDialog.PrinterSettings.DefaultPageSettings;
                    printDocument1.DefaultPageSettings.Margins =
                                     new Margins(20, 20, 20, 20);
                    printDocument1.PrinterSettings =
                                        MyPrintDialog.PrinterSettings;
                    printDocument1.Print();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing cmdPrint_Click()" + ex.Message);
                MessageBox.Show(this, ex.Message, this.Text);
            }
            log.LogMethodExit();
        }

        private void cb_exit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void cb_OK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            barcode = txtInput.Text.Trim();
            if (barcode == "")
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(755), utilities.MessageUtils.getMessage("Generate Barcode"));
            }
            else
            {
                BarcodeReader.Barcode = barcode;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            log.LogMethodExit();
        }
    }
}
