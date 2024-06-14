namespace Semnox.Parafait.Customer
{
    partial class SingleValueContact
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleValueContact));
            this.pnlSingleValueContact = new System.Windows.Forms.Panel();
            this.cmbCountryCode = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.pnlSingleValueContact.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSingleValueContact
            // 
            this.pnlSingleValueContact.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlSingleValueContact.Controls.Add(this.cmbCountryCode);
            this.pnlSingleValueContact.Controls.Add(this.btnDelete);
            this.pnlSingleValueContact.Controls.Add(this.txtContact);
            this.pnlSingleValueContact.Location = new System.Drawing.Point(3, 3);
            this.pnlSingleValueContact.Name = "pnlSingleValueContact";
            this.pnlSingleValueContact.Size = new System.Drawing.Size(368, 47);
            this.pnlSingleValueContact.TabIndex = 6;
            // 
            // cmbCountryCode
            // 
            this.cmbCountryCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCountryCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCountryCode.DropDownWidth = 51;
            this.cmbCountryCode.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.cmbCountryCode.FormattingEnabled = true;
            this.cmbCountryCode.Items.AddRange(new object[] {
            "Facility",
            "Game"});
            this.cmbCountryCode.Location = new System.Drawing.Point(5, 6);
            this.cmbCountryCode.Name = "cmbCountryCode";
            this.cmbCountryCode.Size = new System.Drawing.Size(59, 27);
            this.cmbCountryCode.TabIndex = 110;
            this.cmbCountryCode.Visible = false;
            this.cmbCountryCode.SelectedIndexChanged += new System.EventHandler(this.cmbCountryCode_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(308, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(34, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtContact
            // 
            this.txtContact.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtContact.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContact.Location = new System.Drawing.Point(70, 7);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(223, 26);
            this.txtContact.TabIndex = 2;
            this.txtContact.Leave += new System.EventHandler(this.txtContact_Leave);
            // 
            // SingleValueContact
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlSingleValueContact);
            this.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Name = "SingleValueContact";
            this.Size = new System.Drawing.Size(349, 40);
            this.pnlSingleValueContact.ResumeLayout(false);
            this.pnlSingleValueContact.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSingleValueContact;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtContact;
        protected internal Core.GenericUtilities.AutoCompleteComboBox cmbCountryCode;
    }
}
