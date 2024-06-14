/********************************************************************************************
 * Class Name - RedemptionBarcodeUtils                                                                         
 * Description - Frm Barcode
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Semnox.Parafait.BarcodeUtilities
{
    public class frm_barcode : System.Windows.Forms.Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string barcode = "";
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button cmdMakeBarcode;
        private System.Windows.Forms.PictureBox pictBarcode;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label2;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button cmdPrint;
        private Button cb_exit;
        private Button cb_OK;
        Utilities utilities;

        private System.ComponentModel.Container components = null;

        public frm_barcode(string inputString, Utilities _Utilities)
        {
            log.LogMethodEntry(inputString, _Utilities);
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            txtInput.Text = inputString;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_barcode));
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cmdMakeBarcode = new System.Windows.Forms.Button();
            this.pictBarcode = new System.Windows.Forms.PictureBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.cb_exit = new System.Windows.Forms.Button();
            this.cb_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to encode:";
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(98, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(312, 20);
            this.txtInput.TabIndex = 1;
            // 
            // cmdMakeBarcode
            // 
            this.cmdMakeBarcode.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdMakeBarcode.Location = new System.Drawing.Point(148, 38);
            this.cmdMakeBarcode.Name = "cmdMakeBarcode";
            this.cmdMakeBarcode.Size = new System.Drawing.Size(92, 23);
            this.cmdMakeBarcode.TabIndex = 2;
            this.cmdMakeBarcode.Text = "Make barcode";
            this.cmdMakeBarcode.UseVisualStyleBackColor = false;
            this.cmdMakeBarcode.Click += new System.EventHandler(this.cmdMakeBarcode_Click);
            // 
            // pictBarcode
            // 
            this.pictBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBarcode.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictBarcode.Location = new System.Drawing.Point(11, 69);
            this.pictBarcode.Name = "pictBarcode";
            this.pictBarcode.Size = new System.Drawing.Size(399, 84);
            this.pictBarcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictBarcode.TabIndex = 3;
            this.pictBarcode.TabStop = false;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(98, 39);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(44, 20);
            this.txtWeight.TabIndex = 4;
            this.txtWeight.Text = "2";
            this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 42);
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
            this.cmdPrint.Image = global::Semnox.Parafait.BarcodeUtilities.Properties.Resources.printer;
            this.cmdPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdPrint.Location = new System.Drawing.Point(263, 162);
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
            this.cb_exit.Image = global::Semnox.Parafait.BarcodeUtilities.Properties.Resources.cancel;
            this.cb_exit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_exit.Location = new System.Drawing.Point(191, 162);
            this.cb_exit.Name = "cb_exit";
            this.cb_exit.Size = new System.Drawing.Size(66, 23);
            this.cb_exit.TabIndex = 6;
            this.cb_exit.Text = "Exit";
            this.cb_exit.UseVisualStyleBackColor = true;
            this.cb_exit.Click += new System.EventHandler(this.cb_exit_Click);
            // 
            // cb_OK
            // 
            this.cb_OK.Image = global::Semnox.Parafait.BarcodeUtilities.Properties.Resources.status_ok;
            this.cb_OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_OK.Location = new System.Drawing.Point(344, 162);
            this.cb_OK.Name = "cb_OK";
            this.cb_OK.Size = new System.Drawing.Size(66, 23);
            this.cb_OK.TabIndex = 8;
            this.cb_OK.Text = "OK";
            this.cb_OK.UseVisualStyleBackColor = true;
            this.cb_OK.Click += new System.EventHandler(this.cb_OK_Click);
            // 
            // frm_barcode
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.cb_exit;
            this.ClientSize = new System.Drawing.Size(422, 194);
            this.Controls.Add(this.cb_OK);
            this.Controls.Add(this.cb_exit);
            this.Controls.Add(this.cmdPrint);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictBarcode);
            this.Controls.Add(this.cmdMakeBarcode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_barcode";
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
                PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext);
                //Image myimg = Code128Rendering.MakeBarcodeImage(txtInput.Text, int.Parse(txtWeight.Text), true);
                Image myimg = printerBL.MakeBarcodeLibImage(int.Parse(txtWeight.Text), 40, BarcodeLib.TYPE.CODE128.ToString(), txtInput.Text);
                pictBarcode.Image = myimg;
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
            using (Graphics g = e.Graphics)
            {
                if (Convert.ToInt32(txtWeight.Text) == 1)
                {
                    using (Font fnt = new Font("Arial", 8))
                    {
                        g.DrawImage(pictBarcode.Image, 50, 110);
                        g.DrawString(txtInput.Text, fnt, Brushes.Black, 57, 97);
                    }
                }
                else if (Convert.ToInt32(txtWeight.Text) == 2)
                {
                    using (Font fnt = new Font("Arial", 9))
                    {
                        g.DrawImage(pictBarcode.Image, 60, 130);
                        g.DrawString(txtInput.Text, fnt, Brushes.Black, 76, 115);
                    }
                }
                else if (Convert.ToInt32(txtWeight.Text) == 3)
                {
                    using (Font fnt = new Font("Arial", 10))
                    {
                        g.DrawImage(pictBarcode.Image, 70, 150);
                        g.DrawString(txtInput.Text, fnt, Brushes.Black, 98, 133);
                    }
                }
                else if (Convert.ToInt32(txtWeight.Text) >= 4)
                {
                    using (Font fnt = new Font("Arial", 12))
                    {
                        g.DrawImage(pictBarcode.Image, 80, 170);
                        g.DrawString(txtInput.Text, fnt, Brushes.Black, 120, 153);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void cmdPrint_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
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
            //MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = true;

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
