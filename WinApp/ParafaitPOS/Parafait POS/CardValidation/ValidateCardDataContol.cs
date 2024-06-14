using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Parafait_POS.CardValidation
{
    public partial class ValidateCardDataContol : Panel
    {
        Utilities Utilities;//Added for Image Retrieval
        MessageUtils MessageUtils;

        private PictureBox pbCustomerPhoto = null;
        private PictureBox pbValidationIcon = null;
        private Label lblCardNumberLabel = null;
        private Label lblCardNumber = null;
        private Label lblCustomerNameLabel = null;
        private Label lblCustomerName = null;
        private Label lblEntryValidationLabel = null;
        private Label lblMembershipValidityLabel = null;
        private Label lblMembershipDate = null;
        private Label lblNumberofEntrieslabel = null;
        private Label lblNumberofEntries = null;
        private Label lblTransactionNumberLabel = null;
        private Label lblTransactionNumber = null;
        private Button btnClosePanel = null;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ValidateCardDataContol()
            : base()
        {
            InitializeComponent();
        }

        public ValidateCardDataContol(int panelno, String imgPath, string cardNumber, string customerName, string membervaliditydate, int entriesRemaining, bool validationFlag, int transactionNumber)
            : base()
        {
            Utilities = new Utilities();//Added for Image Retrieval
            MessageUtils = Utilities.MessageUtils;//Showing label messages

            Image customerImg = null;
            Image validImg = null;
            string exeDir = System.IO.Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""));

            //Panel 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Name = "panel" + panelno.ToString();
            this.Size = new System.Drawing.Size(882, 257);
            this.Tag = panelno;//Add panel no to Panel Tag

            //CustomerPhoto PictureBox 
            this.pbCustomerPhoto = new PictureBox();
            this.pbCustomerPhoto.Name = "pbCustomerPhoto" + panelno.ToString();
            this.pbCustomerPhoto.BorderStyle = BorderStyle.FixedSingle;
            try
            {
                //Begin: Added for Image Retrieval 
                if (!string.IsNullOrWhiteSpace(imgPath))
                {
                    SqlCommand cmdImage = Utilities.getCommand();
                    cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";
                    cmdImage.Parameters.AddWithValue("@FileName", imgPath);
                    object o = cmdImage.ExecuteScalar();
                    if (o != null && o != DBNull.Value)
                    {
                        byte[] b = o as byte[];
                        if (b != null)
                        {
                            try
                            {
                                b = Encryption.Decrypt(b);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Failed to decrypt the customer photo image", ex);
                                b = o as byte[];
                            }
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(b))
                            {
                                customerImg = System.Drawing.Image.FromStream(ms);
                            }
                        }
                        //customerImg = Utilities.ConvertToImage(o);
                    }
                }
                //End: Added for Image Retrieval 
            }
            catch (Exception ex)
            {
                log.Error("Exception while retrieving customer photo from server - ", ex);
                //customerImg = Properties.Resources.Semnox_Medium;//if exception use the default image
            }
            this.pbCustomerPhoto.Image = customerImg;
            this.pbCustomerPhoto.Location = new System.Drawing.Point(13, 18);
            this.pbCustomerPhoto.Size = new System.Drawing.Size(276, 218);//(183, 139);
            this.pbCustomerPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            //Card Number label
            this.lblCardNumberLabel = new Label();
            this.lblCardNumberLabel.Name = "lblCardNumberLabel" + panelno.ToString();
            this.lblCardNumberLabel.AutoSize = true;
            this.lblCardNumberLabel.Location = new System.Drawing.Point(447, 18);
            this.lblCardNumberLabel.Size = new System.Drawing.Size(174, 22);
            this.lblCardNumberLabel.Text = MessageUtils.getMessage("Tag Number") + ":";

            //Card Number 
            this.lblCardNumber = new Label();
            this.lblCardNumber.Name = "lblCardNumber" + panelno.ToString();
            this.lblCardNumber.AutoSize = true;
            this.lblCardNumber.Location = new System.Drawing.Point(585, 18);
            this.lblCardNumber.Size = new System.Drawing.Size(131, 22);
            this.lblCardNumber.Text = cardNumber;

            //Entry validation label
            this.lblEntryValidationLabel = new Label();
            this.lblEntryValidationLabel.Name = "lblEntryValidationLabel" + panelno.ToString();
            this.lblEntryValidationLabel.AutoSize = true;
            this.lblEntryValidationLabel.Location = new System.Drawing.Point(462, 56);
            this.lblEntryValidationLabel.Size = new System.Drawing.Size(131, 22);
            this.lblEntryValidationLabel.Text = MessageUtils.getMessage("Entry Valid") + ":";

            //Validation Icon pictureBox 
            this.pbValidationIcon = new PictureBox();
            this.pbValidationIcon.Name = "pbValidationIcon" + panelno.ToString();
            if (validationFlag)
            {
                if (File.Exists(exeDir + "\\Resources\\Green.png"))
                {
                    validImg = Image.FromFile(exeDir + "\\Resources\\Green.png");
                }
                else
                {
                    validImg = Properties.Resources.Green;
                }
            }
            else
            {
                if (File.Exists(exeDir + "\\Resources\\Red.png"))
                {
                    validImg = Image.FromFile(exeDir + "\\Resources\\Red.png");
                }
                else
                {
                    validImg = Properties.Resources.Red;
                }
            }
            this.pbValidationIcon.Image = validImg;
            this.pbValidationIcon.Location = new System.Drawing.Point(586, 56);
            this.pbValidationIcon.Size = new System.Drawing.Size(35, 30);
            this.pbValidationIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbValidationIcon.Tag = validationFlag;

            //Name label
            this.lblCustomerNameLabel = new Label();
            this.lblCustomerNameLabel.Name = "lblCustomerNameLabel" + panelno.ToString();
            this.lblCustomerNameLabel.AutoSize = true;
            this.lblCustomerNameLabel.Location = new System.Drawing.Point(500, 95);
            this.lblCustomerNameLabel.Size = new System.Drawing.Size(131, 22);
            this.lblCustomerNameLabel.Text = MessageUtils.getMessage("Name") + ":";

            //Customer Name
            this.lblCustomerName = new Label();
            this.lblCustomerName.Name = "lblCustomerName" + panelno.ToString();
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(585, 95);
            this.lblCustomerName.Size = new System.Drawing.Size(150, 22);
            this.lblCustomerName.Text = customerName;

            //Membership Validity label
            this.lblMembershipValidityLabel = new Label();
            this.lblMembershipValidityLabel.Name = "lblMembershipValidityLabel" + panelno.ToString();
            this.lblMembershipValidityLabel.AutoSize = true;
            this.lblMembershipValidityLabel.Location = new System.Drawing.Point(490, 135);
            this.lblMembershipValidityLabel.Size = new System.Drawing.Size(194, 22);
            this.lblMembershipValidityLabel.Text = MessageUtils.getMessage("Validity") + ":";

            //Membership Date
            this.lblMembershipDate = new Label();
            this.lblMembershipDate.Name = "lblMembershipDate" + panelno.ToString();
            this.lblMembershipDate.AutoSize = true;
            this.lblMembershipDate.Location = new System.Drawing.Point(585, 135);
            this.lblMembershipDate.Size = new System.Drawing.Size(135, 22);
            this.lblMembershipDate.Text = DateTime.Parse(membervaliditydate) == DateTime.MinValue ? string.Empty : membervaliditydate;

            //Number of entries remaining label
            this.lblNumberofEntrieslabel = new Label();
            this.lblNumberofEntrieslabel.Name = "lblNumberofEntrieslabel" + panelno.ToString();
            this.lblNumberofEntrieslabel.AutoSize = true;
            this.lblNumberofEntrieslabel.Location = new System.Drawing.Point(312, 174);
            this.lblNumberofEntrieslabel.Size = new System.Drawing.Size(253, 22);
            if (entriesRemaining != -1)
            {
                this.lblNumberofEntrieslabel.Location = new System.Drawing.Point(312, 174);
                this.lblNumberofEntrieslabel.Text = MessageUtils.getMessage("Number of entries remaining") + ":";
            }
            else
            {
                this.lblNumberofEntrieslabel.Location = new System.Drawing.Point(400, 174);
                this.lblNumberofEntrieslabel.Text = MessageUtils.getMessage("Balance/Overdue") + ":";
            }

            //Number of entries
            this.lblNumberofEntries = new Label();
            this.lblNumberofEntries.Name = "lblNumberofEntries" + panelno.ToString();
            this.lblNumberofEntries.AutoSize = true;
            this.lblNumberofEntries.Location = new System.Drawing.Point(585, 174);
            this.lblNumberofEntries.Size = new System.Drawing.Size(40, 22);
            if (entriesRemaining != -1)
                this.lblNumberofEntries.Text = entriesRemaining.ToString();
            else
                this.lblNumberofEntries.Text = ((int)(DateTime.Parse(membervaliditydate) - ServerDateTime.Now).TotalMinutes).ToString() + " mins.";
            //Transaction Number label
            this.lblTransactionNumberLabel = new Label();
            this.lblTransactionNumberLabel.Name = "lblTransactionNumberLabel" + panelno.ToString();
            this.lblTransactionNumberLabel.AutoSize = true;
            this.lblTransactionNumberLabel.Location = new System.Drawing.Point(381, 214);
            this.lblTransactionNumberLabel.Size = new System.Drawing.Size(184, 22);
            this.lblTransactionNumberLabel.Text = MessageUtils.getMessage("Transaction Number") + ":";

            //Transaction Number
            this.lblTransactionNumber = new Label();
            this.lblTransactionNumber.Name = "lblTransactionNumber" + panelno.ToString();
            this.lblTransactionNumber.AutoSize = true;
            this.lblTransactionNumber.Location = new System.Drawing.Point(585, 214);
            this.lblTransactionNumber.Size = new System.Drawing.Size(76, 22);
            this.lblTransactionNumber.Text = transactionNumber.ToString();

            //Close Panel button
            this.btnClosePanel = new Button();
            this.btnClosePanel.Name = "btnClosePanel" + panelno.ToString();
            this.btnClosePanel.AutoSize = true;
            this.btnClosePanel.Location = new System.Drawing.Point(825, 5);
            this.btnClosePanel.Size = new System.Drawing.Size(50, 50);
            this.btnClosePanel.Text = "x";
            this.btnClosePanel.FlatStyle = FlatStyle.Flat;
            this.btnClosePanel.BackColor = Color.Transparent;
            this.btnClosePanel.FlatAppearance.BorderColor = Color.White;
            //this.btnClosePanel.Font = new Font(btnClosePanel.Font, FontStyle.Bold);          
            this.btnClosePanel.Font = new Font(btnClosePanel.Font.FontFamily, 30, btnClosePanel.Font.Style | FontStyle.Bold);     
            this.btnClosePanel.Click += new EventHandler(btnClosePanel_Click);

            this.Controls.Add(this.pbCustomerPhoto);
            this.Controls.Add(this.lblCardNumberLabel);
            this.Controls.Add(this.lblCardNumber);
            this.Controls.Add(this.lblEntryValidationLabel);
            this.Controls.Add(this.pbValidationIcon);
            this.Controls.Add(this.lblCustomerNameLabel);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblMembershipValidityLabel);
            this.Controls.Add(this.lblMembershipDate);
            this.Controls.Add(this.lblNumberofEntrieslabel);
            this.Controls.Add(this.lblNumberofEntries);
            this.Controls.Add(this.lblTransactionNumberLabel);
            this.Controls.Add(this.lblTransactionNumber);
            this.Controls.Add(this.btnClosePanel);

            InitializeComponent();
        }

        private void btnClosePanel_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

        public string cardNumber
        {
            get { return this.lblCardNumber.Text.ToString(); }
            set { this.lblCardNumber.Text = value; }
        }

        public string pbValidationTag
        {
            get { return this.pbValidationIcon.Tag.ToString(); }
            set { this.pbValidationIcon.Tag = value; }
        }

        public string transactionNumber
        {
            get { return this.lblTransactionNumber.Text.ToString(); }
            set { this.lblTransactionNumber.Text = value; }
        }
               
    }
}
