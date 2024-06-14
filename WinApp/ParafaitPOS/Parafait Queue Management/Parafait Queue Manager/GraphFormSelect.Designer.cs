namespace ParafaitQueueManagement
{
    partial class GraphFormSelect
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
            this.graphrb1 = new System.Windows.Forms.RadioButton();
            this.graphrb2 = new System.Windows.Forms.RadioButton();
            this.lblgraph1 = new System.Windows.Forms.Label();
            this.lblGraph2 = new System.Windows.Forms.Label();
            this.btngetGraph = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // graphrb1
            // 
            this.graphrb1.AutoSize = true;
            this.graphrb1.Location = new System.Drawing.Point(38, 85);
            this.graphrb1.Name = "graphrb1";
            this.graphrb1.Size = new System.Drawing.Size(14, 13);
            this.graphrb1.TabIndex = 0;
            this.graphrb1.TabStop = true;
            this.graphrb1.UseVisualStyleBackColor = true;
            // 
            // graphrb2
            // 
            this.graphrb2.AutoSize = true;
            this.graphrb2.Location = new System.Drawing.Point(38, 125);
            this.graphrb2.Name = "graphrb2";
            this.graphrb2.Size = new System.Drawing.Size(14, 13);
            this.graphrb2.TabIndex = 1;
            this.graphrb2.TabStop = true;
            this.graphrb2.UseVisualStyleBackColor = true;
            // 
            // lblgraph1
            // 
            this.lblgraph1.AutoSize = true;
            this.lblgraph1.Location = new System.Drawing.Point(95, 85);
            this.lblgraph1.Name = "lblgraph1";
            this.lblgraph1.Size = new System.Drawing.Size(103, 13);
            this.lblgraph1.TabIndex = 2;
            this.lblgraph1.Text = "Game Play Statistics";
            // 
            // lblGraph2
            // 
            this.lblGraph2.AutoSize = true;
            this.lblGraph2.Location = new System.Drawing.Point(95, 125);
            this.lblGraph2.Name = "lblGraph2";
            this.lblGraph2.Size = new System.Drawing.Size(85, 13);
            this.lblGraph2.TabIndex = 3;
            this.lblGraph2.Text = "Bowler Statistics";
            // 
            // btngetGraph
            // 
            this.btngetGraph.Location = new System.Drawing.Point(76, 163);
            this.btngetGraph.Name = "btngetGraph";
            this.btngetGraph.Size = new System.Drawing.Size(122, 23);
            this.btngetGraph.TabIndex = 4;
            this.btngetGraph.Text = "Get Graph/Statistics";
            this.btngetGraph.UseVisualStyleBackColor = true;
            this.btngetGraph.Click += new System.EventHandler(this.btngetGraph_Click);
            // 
            // GraphFormSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 221);
            this.Controls.Add(this.btngetGraph);
            this.Controls.Add(this.lblGraph2);
            this.Controls.Add(this.lblgraph1);
            this.Controls.Add(this.graphrb2);
            this.Controls.Add(this.graphrb1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GraphFormSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Play Info";
            this.Load += new System.EventHandler(this.GraphFormSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton graphrb1;
        private System.Windows.Forms.RadioButton graphrb2;
        private System.Windows.Forms.Label lblgraph1;
        private System.Windows.Forms.Label lblGraph2;
        private System.Windows.Forms.Button btngetGraph;
    }
}