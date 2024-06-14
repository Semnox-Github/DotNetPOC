namespace Semnox.Core.GenericUtilities
{
    partial class HtmlEditorUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bToolbar = new System.Windows.Forms.Button();
            this.bEditHTML = new System.Windows.Forms.Button();
            this.bBackground = new System.Windows.Forms.Button();
            this.bForeground = new System.Windows.Forms.Button();
            this.bViewHtml = new System.Windows.Forms.Button();
            this.bStyle = new System.Windows.Forms.Button();
            this.readonlyCheck = new System.Windows.Forms.CheckBox();
            this.bOverWrite = new System.Windows.Forms.Button();
            this.bOpenHtml = new System.Windows.Forms.Button();
            this.bSaveHtml = new System.Windows.Forms.Button();
            this.listHeadings = new System.Windows.Forms.ComboBox();
            this.bHeading = new System.Windows.Forms.Button();
            this.bInsertHtml = new System.Windows.Forms.Button();
            this.bImage = new System.Windows.Forms.Button();
            this.bBasrHref = new System.Windows.Forms.Button();
            this.bPaste = new System.Windows.Forms.Button();
            this.bFormatted = new System.Windows.Forms.Button();
            this.bNormal = new System.Windows.Forms.Button();
            this.bScript = new System.Windows.Forms.Button();
            this.bMicrosoft = new System.Windows.Forms.Button();
            this.bLoadFile = new System.Windows.Forms.Button();
            this.bUrl = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbLanguages = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.htmlEditorControl = new Semnox.Core.GenericUtilities.HtmlEditorControl();
            this.SuspendLayout();
            // 
            // bToolbar
            // 
            this.bToolbar.Location = new System.Drawing.Point(460, 534);
            this.bToolbar.Name = "bToolbar";
            this.bToolbar.Size = new System.Drawing.Size(75, 23);
            this.bToolbar.TabIndex = 2;
            this.bToolbar.Text = "Tool Bar";
            this.bToolbar.Visible = false;
            this.bToolbar.Click += new System.EventHandler(this.bToolbar_Click);
            // 
            // bEditHTML
            // 
            this.bEditHTML.Location = new System.Drawing.Point(723, 534);
            this.bEditHTML.Name = "bEditHTML";
            this.bEditHTML.Size = new System.Drawing.Size(75, 23);
            this.bEditHTML.TabIndex = 3;
            this.bEditHTML.Text = "Edit HTML";
            this.bEditHTML.Click += new System.EventHandler(this.bEditHTML_Click);
            // 
            // bBackground
            // 
            this.bBackground.Location = new System.Drawing.Point(1, 380);
            this.bBackground.Name = "bBackground";
            this.bBackground.Size = new System.Drawing.Size(75, 23);
            this.bBackground.TabIndex = 4;
            this.bBackground.Text = "Background";
            this.bBackground.Visible = false;
            this.bBackground.Click += new System.EventHandler(this.bBackground_Click);
            // 
            // bForeground
            // 
            this.bForeground.Location = new System.Drawing.Point(1, 408);
            this.bForeground.Name = "bForeground";
            this.bForeground.Size = new System.Drawing.Size(75, 23);
            this.bForeground.TabIndex = 5;
            this.bForeground.Text = "Foreground";
            this.bForeground.Visible = false;
            this.bForeground.Click += new System.EventHandler(this.bForeground_Click);
            // 
            // bViewHtml
            // 
            this.bViewHtml.Location = new System.Drawing.Point(561, 534);
            this.bViewHtml.Name = "bViewHtml";
            this.bViewHtml.Size = new System.Drawing.Size(75, 23);
            this.bViewHtml.TabIndex = 7;
            this.bViewHtml.Text = "View Html";
            this.bViewHtml.Visible = false;
            this.bViewHtml.Click += new System.EventHandler(this.bViewHtml_Click);
            // 
            // bStyle
            // 
            this.bStyle.Location = new System.Drawing.Point(374, 534);
            this.bStyle.Name = "bStyle";
            this.bStyle.Size = new System.Drawing.Size(75, 23);
            this.bStyle.TabIndex = 8;
            this.bStyle.Text = "StyleSheet";
            this.bStyle.Visible = false;
            this.bStyle.Click += new System.EventHandler(this.bStyle_Click);
            // 
            // readonlyCheck
            // 
            this.readonlyCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.readonlyCheck.Location = new System.Drawing.Point(173, 533);
            this.readonlyCheck.Name = "readonlyCheck";
            this.readonlyCheck.Size = new System.Drawing.Size(104, 23);
            this.readonlyCheck.TabIndex = 9;
            this.readonlyCheck.Text = "Read Only";
            this.readonlyCheck.Visible = false;
            this.readonlyCheck.CheckedChanged += new System.EventHandler(this.readonlyCheck_CheckedChanged);
            // 
            // bOverWrite
            // 
            this.bOverWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOverWrite.Location = new System.Drawing.Point(87, 534);
            this.bOverWrite.Name = "bOverWrite";
            this.bOverWrite.Size = new System.Drawing.Size(75, 23);
            this.bOverWrite.TabIndex = 10;
            this.bOverWrite.Text = "OverWrite";
            this.bOverWrite.Visible = false;
            this.bOverWrite.Click += new System.EventHandler(this.bOverWrite_Click);
            // 
            // bOpenHtml
            // 
            this.bOpenHtml.Location = new System.Drawing.Point(1, 242);
            this.bOpenHtml.Name = "bOpenHtml";
            this.bOpenHtml.Size = new System.Drawing.Size(75, 23);
            this.bOpenHtml.TabIndex = 12;
            this.bOpenHtml.Text = "Open Html";
            this.bOpenHtml.Visible = false;
            this.bOpenHtml.Click += new System.EventHandler(this.bOpenHtml_Click);
            // 
            // bSaveHtml
            // 
            this.bSaveHtml.Location = new System.Drawing.Point(1, 270);
            this.bSaveHtml.Name = "bSaveHtml";
            this.bSaveHtml.Size = new System.Drawing.Size(75, 23);
            this.bSaveHtml.TabIndex = 13;
            this.bSaveHtml.Text = "Save Html";
            this.bSaveHtml.Visible = false;
            this.bSaveHtml.Click += new System.EventHandler(this.bSaveHtml_Click);
            // 
            // listHeadings
            // 
            this.listHeadings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listHeadings.FormattingEnabled = true;
            this.listHeadings.Items.AddRange(new object[] {
            "H1",
            "H2",
            "H3",
            "H4",
            "H5"});
            this.listHeadings.Location = new System.Drawing.Point(4, 354);
            this.listHeadings.MaxDropDownItems = 5;
            this.listHeadings.Name = "listHeadings";
            this.listHeadings.Size = new System.Drawing.Size(72, 21);
            this.listHeadings.TabIndex = 14;
            this.listHeadings.Visible = false;
            // 
            // bHeading
            // 
            this.bHeading.Location = new System.Drawing.Point(1, 186);
            this.bHeading.Name = "bHeading";
            this.bHeading.Size = new System.Drawing.Size(75, 23);
            this.bHeading.TabIndex = 15;
            this.bHeading.Text = "Set Heading";
            this.bHeading.Visible = false;
            this.bHeading.Click += new System.EventHandler(this.bHeading_Click);
            // 
            // bInsertHtml
            // 
            this.bInsertHtml.Location = new System.Drawing.Point(1, 298);
            this.bInsertHtml.Name = "bInsertHtml";
            this.bInsertHtml.Size = new System.Drawing.Size(75, 23);
            this.bInsertHtml.TabIndex = 16;
            this.bInsertHtml.Text = "Insert Html";
            this.bInsertHtml.Visible = false;
            this.bInsertHtml.Click += new System.EventHandler(this.bInsertHtml_Click);
            // 
            // bImage
            // 
            this.bImage.Location = new System.Drawing.Point(642, 534);
            this.bImage.Name = "bImage";
            this.bImage.Size = new System.Drawing.Size(75, 23);
            this.bImage.TabIndex = 17;
            this.bImage.Text = "Local Image";
            this.bImage.Visible = false;
            this.bImage.Click += new System.EventHandler(this.bImage_Click);
            // 
            // bBasrHref
            // 
            this.bBasrHref.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bBasrHref.Location = new System.Drawing.Point(1, 450);
            this.bBasrHref.Name = "bBasrHref";
            this.bBasrHref.Size = new System.Drawing.Size(75, 23);
            this.bBasrHref.TabIndex = 18;
            this.bBasrHref.Text = "Word Wrap";
            this.bBasrHref.Visible = false;
            this.bBasrHref.Click += new System.EventHandler(this.bBasrHref_Click);
            // 
            // bPaste
            // 
            this.bPaste.Location = new System.Drawing.Point(1, 326);
            this.bPaste.Name = "bPaste";
            this.bPaste.Size = new System.Drawing.Size(75, 23);
            this.bPaste.TabIndex = 19;
            this.bPaste.Text = "Insert Text";
            this.bPaste.Visible = false;
            this.bPaste.Click += new System.EventHandler(this.bPaste_Click);
            // 
            // bFormatted
            // 
            this.bFormatted.Location = new System.Drawing.Point(1, 158);
            this.bFormatted.Name = "bFormatted";
            this.bFormatted.Size = new System.Drawing.Size(75, 23);
            this.bFormatted.TabIndex = 20;
            this.bFormatted.Text = "Formatted";
            this.bFormatted.Visible = false;
            this.bFormatted.Click += new System.EventHandler(this.bFormatted_Click);
            // 
            // bNormal
            // 
            this.bNormal.Location = new System.Drawing.Point(1, 130);
            this.bNormal.Name = "bNormal";
            this.bNormal.Size = new System.Drawing.Size(75, 23);
            this.bNormal.TabIndex = 21;
            this.bNormal.Text = "Normal";
            this.bNormal.Visible = false;
            this.bNormal.Click += new System.EventHandler(this.bNormal_Click);
            // 
            // bScript
            // 
            this.bScript.Location = new System.Drawing.Point(1, 531);
            this.bScript.Name = "bScript";
            this.bScript.Size = new System.Drawing.Size(75, 23);
            this.bScript.TabIndex = 22;
            this.bScript.Text = "ScriptBlock";
            this.bScript.Visible = false;
            this.bScript.Click += new System.EventHandler(this.bScript_Click);
            // 
            // bMicrosoft
            // 
            this.bMicrosoft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bMicrosoft.Location = new System.Drawing.Point(288, 534);
            this.bMicrosoft.Name = "bMicrosoft";
            this.bMicrosoft.Size = new System.Drawing.Size(75, 23);
            this.bMicrosoft.TabIndex = 25;
            this.bMicrosoft.Text = "Microsoft";
            this.bMicrosoft.Visible = false;
            this.bMicrosoft.Click += new System.EventHandler(this.bMicrosoft_Click);
            // 
            // bLoadFile
            // 
            this.bLoadFile.Location = new System.Drawing.Point(1, 214);
            this.bLoadFile.Name = "bLoadFile";
            this.bLoadFile.Size = new System.Drawing.Size(75, 23);
            this.bLoadFile.TabIndex = 27;
            this.bLoadFile.Text = "Local File";
            this.bLoadFile.Visible = false;
            this.bLoadFile.Click += new System.EventHandler(this.bLoadFile_Click);
            // 
            // bUrl
            // 
            this.bUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bUrl.Location = new System.Drawing.Point(1, 479);
            this.bUrl.Name = "bUrl";
            this.bUrl.Size = new System.Drawing.Size(75, 23);
            this.bUrl.TabIndex = 28;
            this.bUrl.Text = "Enter Href";
            this.bUrl.Visible = false;
            this.bUrl.Click += new System.EventHandler(this.bUrl_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(890, 534);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(804, 534);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbLanguages
            // 
            this.cmbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguages.FormattingEnabled = true;
            this.cmbLanguages.Location = new System.Drawing.Point(833, 11);
            this.cmbLanguages.Name = "cmbLanguages";
            this.cmbLanguages.Size = new System.Drawing.Size(121, 21);
            this.cmbLanguages.TabIndex = 31;
            this.cmbLanguages.SelectedIndexChanged += new System.EventHandler(this.cmbLanguages_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(720, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Choose Language";
            // 
            // htmlEditorControl
            // 
            this.htmlEditorControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlEditorControl.InnerText = "Carl Nolan";
            this.htmlEditorControl.Location = new System.Drawing.Point(4, 38);
            this.htmlEditorControl.Name = "htmlEditorControl";
            this.htmlEditorControl.Size = new System.Drawing.Size(967, 490);
            this.htmlEditorControl.TabIndex = 26;
            this.htmlEditorControl.HtmlNavigation += new Semnox.Core.GenericUtilities.HtmlNavigationEventHandler(this.htmlEditorControl_HtmlNavigation);
            // 
            // HtmlEditorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 566);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLanguages);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.bUrl);
            this.Controls.Add(this.bLoadFile);
            this.Controls.Add(this.htmlEditorControl);
            this.Controls.Add(this.bMicrosoft);
            this.Controls.Add(this.bScript);
            this.Controls.Add(this.bNormal);
            this.Controls.Add(this.bFormatted);
            this.Controls.Add(this.bPaste);
            this.Controls.Add(this.bImage);
            this.Controls.Add(this.bInsertHtml);
            this.Controls.Add(this.bHeading);
            this.Controls.Add(this.listHeadings);
            this.Controls.Add(this.bSaveHtml);
            this.Controls.Add(this.bOpenHtml);
            this.Controls.Add(this.readonlyCheck);
            this.Controls.Add(this.bStyle);
            this.Controls.Add(this.bViewHtml);
            this.Controls.Add(this.bForeground);
            this.Controls.Add(this.bBackground);
            this.Controls.Add(this.bEditHTML);
            this.Controls.Add(this.bToolbar);
            this.Controls.Add(this.bBasrHref);
            this.Controls.Add(this.bOverWrite);
            this.MaximizeBox = false;
            this.Name = "HtmlEditorUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Html Editor";
            this.Load += new System.EventHandler(this.EditorTestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button bToolbar;
        private System.Windows.Forms.Button bBackground;
        private System.Windows.Forms.Button bForeground;
        private System.Windows.Forms.Button bEditHTML;
        private System.Windows.Forms.Button bViewHtml;
        private System.Windows.Forms.Button bStyle;
        private System.Windows.Forms.CheckBox readonlyCheck;
        private System.Windows.Forms.Button bOverWrite;
        private System.Windows.Forms.Button bOpenHtml;
        private System.Windows.Forms.Button bSaveHtml;
        private System.Windows.Forms.ComboBox listHeadings;
        private System.Windows.Forms.Button bHeading;
        private System.Windows.Forms.Button bInsertHtml;
        private System.Windows.Forms.Button bImage;
        private System.Windows.Forms.Button bBasrHref;
        private System.Windows.Forms.Button bPaste;
        private System.Windows.Forms.Button bFormatted;
        private System.Windows.Forms.Button bNormal;
        private System.Windows.Forms.Button bScript;
        private System.Windows.Forms.Button bMicrosoft;
        private System.Windows.Forms.Button bLoadFile;
        private System.Windows.Forms.Button bUrl;
        private Semnox.Core.GenericUtilities.HtmlEditorControl htmlEditorControl;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbLanguages;
        private System.Windows.Forms.Label label1;
    }
}

