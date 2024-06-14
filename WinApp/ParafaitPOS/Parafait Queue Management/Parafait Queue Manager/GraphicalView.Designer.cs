namespace ParafaitQueueManagement
{
    partial class GraphicalView
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
            this.components = new System.ComponentModel.Container();
            this.lblMachineStatistics = new System.Windows.Forms.Label();
            this.lblFromdate = new System.Windows.Forms.Label();
            this.dtfrom = new System.Windows.Forms.DateTimePicker();
            this.lblTodate = new System.Windows.Forms.Label();
            this.dtend = new System.Windows.Forms.DateTimePicker();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.lblNoOfDays = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.chkIncludeTechCard = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblMachineStatistics
            // 
            this.lblMachineStatistics.AutoSize = true;
            this.lblMachineStatistics.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMachineStatistics.Location = new System.Drawing.Point(34, 9);
            this.lblMachineStatistics.Name = "lblMachineStatistics";
            this.lblMachineStatistics.Size = new System.Drawing.Size(76, 13);
            this.lblMachineStatistics.TabIndex = 0;
            this.lblMachineStatistics.Text = "Lane Statistics";
            // 
            // lblFromdate
            // 
            this.lblFromdate.AutoSize = true;
            this.lblFromdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromdate.Location = new System.Drawing.Point(34, 46);
            this.lblFromdate.Name = "lblFromdate";
            this.lblFromdate.Size = new System.Drawing.Size(57, 13);
            this.lblFromdate.TabIndex = 1;
            this.lblFromdate.Text = "From Date";
            // 
            // dtfrom
            // 
            this.dtfrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtfrom.Location = new System.Drawing.Point(123, 39);
            this.dtfrom.Name = "dtfrom";
            this.dtfrom.Size = new System.Drawing.Size(200, 21);
            this.dtfrom.TabIndex = 2;
            // 
            // lblTodate
            // 
            this.lblTodate.AutoSize = true;
            this.lblTodate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodate.Location = new System.Drawing.Point(380, 48);
            this.lblTodate.Name = "lblTodate";
            this.lblTodate.Size = new System.Drawing.Size(45, 13);
            this.lblTodate.TabIndex = 3;
            this.lblTodate.Text = "To Date";
            // 
            // dtend
            // 
            this.dtend.CalendarFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtend.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtend.Location = new System.Drawing.Point(455, 42);
            this.dtend.Name = "dtend";
            this.dtend.Size = new System.Drawing.Size(200, 21);
            this.dtend.TabIndex = 4;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(697, 41);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // zedGraph
            // 
            this.zedGraph.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zedGraph.Location = new System.Drawing.Point(37, 89);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(1056, 498);
            this.zedGraph.TabIndex = 6;
            // 
            // lblNoOfDays
            // 
            this.lblNoOfDays.AutoSize = true;
            this.lblNoOfDays.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoOfDays.Location = new System.Drawing.Point(834, 53);
            this.lblNoOfDays.Name = "lblNoOfDays";
            this.lblNoOfDays.Size = new System.Drawing.Size(41, 13);
            this.lblNoOfDays.TabIndex = 7;
            this.lblNoOfDays.Text = "label1";
            this.lblNoOfDays.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 591);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1122, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // chkIncludeTechCard
            // 
            this.chkIncludeTechCard.AutoSize = true;
            this.chkIncludeTechCard.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIncludeTechCard.Location = new System.Drawing.Point(948, 46);
            this.chkIncludeTechCard.Name = "chkIncludeTechCard";
            this.chkIncludeTechCard.Size = new System.Drawing.Size(111, 17);
            this.chkIncludeTechCard.TabIndex = 9;
            this.chkIncludeTechCard.Text = "Include tech Card";
            this.chkIncludeTechCard.UseVisualStyleBackColor = true;
            // 
            // GraphicalView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 613);
            this.Controls.Add(this.chkIncludeTechCard);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblNoOfDays);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.dtend);
            this.Controls.Add(this.lblTodate);
            this.Controls.Add(this.dtfrom);
            this.Controls.Add(this.lblFromdate);
            this.Controls.Add(this.lblMachineStatistics);
            this.Name = "GraphicalView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GraphicalView";
            this.Load += new System.EventHandler(this.GraphicalView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMachineStatistics;
        private System.Windows.Forms.Label lblFromdate;
        private System.Windows.Forms.DateTimePicker dtfrom;
        private System.Windows.Forms.Label lblTodate;
        private System.Windows.Forms.DateTimePicker dtend;
        private System.Windows.Forms.Button btnSubmit;
        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.Label lblNoOfDays;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.CheckBox chkIncludeTechCard;
    }
}