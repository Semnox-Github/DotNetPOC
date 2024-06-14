namespace Semnox.Parafait.Customer
{
    partial class CustomerFeedBackLaunchUI
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCustomerFBQuestion = new System.Windows.Forms.Button();
            this.btnSurvey = new System.Windows.Forms.Button();
            this.btnsurveydetail = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnResponseValue = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCustomerFBQuestion
            // 
            this.btnCustomerFBQuestion.Location = new System.Drawing.Point(56, 206);
            this.btnCustomerFBQuestion.Name = "btnCustomerFBQuestion";
            this.btnCustomerFBQuestion.Size = new System.Drawing.Size(234, 23);
            this.btnCustomerFBQuestion.TabIndex = 1;
            this.btnCustomerFBQuestion.Text = "Customer Feed Back Survey Questionnair";
            this.btnCustomerFBQuestion.UseVisualStyleBackColor = true;
            this.btnCustomerFBQuestion.Click += new System.EventHandler(this.btnCustomerFBQuestion_Click);
            // 
            // btnSurvey
            // 
            this.btnSurvey.Location = new System.Drawing.Point(56, 20);
            this.btnSurvey.Name = "btnSurvey";
            this.btnSurvey.Size = new System.Drawing.Size(234, 23);
            this.btnSurvey.TabIndex = 2;
            this.btnSurvey.Text = "Survey";
            this.btnSurvey.UseVisualStyleBackColor = true;
            this.btnSurvey.Click += new System.EventHandler(this.btnSurvey_Click);
            // 
            // btnsurveydetail
            // 
            this.btnsurveydetail.Location = new System.Drawing.Point(56, 61);
            this.btnsurveydetail.Name = "btnsurveydetail";
            this.btnsurveydetail.Size = new System.Drawing.Size(234, 23);
            this.btnsurveydetail.TabIndex = 3;
            this.btnsurveydetail.Text = "Survey Details";
            this.btnsurveydetail.UseVisualStyleBackColor = true;
            this.btnsurveydetail.Click += new System.EventHandler(this.btnsurveydetail_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(56, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(234, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Survey Response";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnResponseValue
            // 
            this.btnResponseValue.Location = new System.Drawing.Point(56, 135);
            this.btnResponseValue.Name = "btnResponseValue";
            this.btnResponseValue.Size = new System.Drawing.Size(234, 23);
            this.btnResponseValue.TabIndex = 5;
            this.btnResponseValue.Text = "Survey Response Value";
            this.btnResponseValue.UseVisualStyleBackColor = true;
            this.btnResponseValue.Click += new System.EventHandler(this.btnResponseValue_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(56, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(234, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Survey Feedback Questions";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CustomerFeedBackLaunchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 241);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnResponseValue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnsurveydetail);
            this.Controls.Add(this.btnSurvey);
            this.Controls.Add(this.btnCustomerFBQuestion);
            this.Name = "CustomerFeedBackLaunchUI";
            this.Text = "CustomerFeedBackLaunchUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCustomerFBQuestion;
        private System.Windows.Forms.Button btnSurvey;
        private System.Windows.Forms.Button btnsurveydetail;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnResponseValue;
        private System.Windows.Forms.Button button2;
    }
}