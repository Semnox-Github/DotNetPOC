namespace Semnox.Parafait.Reports
{
    partial class CreatePOSCustomReport
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // CreatePOSCustomReport
            // 
            this.Name = "CreatePOSCustomReport";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Pixel(40D), Telerik.Reporting.Drawing.Unit.Pixel(40D), Telerik.Reporting.Drawing.Unit.Pixel(20D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.PageSettings.Landscape = false;
            this.PageSettings.ContinuousPaper = true;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();


        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;


        /// <summary>
        /// table1
        ///   </summary>
        public Telerik.Reporting.Table table1;
        /// <summary>
        /// txtExecutionTime
        /// </summary>
        public Telerik.Reporting.TextBox txtExecutionTime;


        private Telerik.Reporting.TextBox txtReportName;
        private Telerik.Reporting.TextBox txtDateRange;
        private Telerik.Reporting.TextBox txtSelectedSites;
        private Telerik.Reporting.TextBox txtSelectedCategory;
        private Telerik.Reporting.TextBox txtOtherParams;
    }
}