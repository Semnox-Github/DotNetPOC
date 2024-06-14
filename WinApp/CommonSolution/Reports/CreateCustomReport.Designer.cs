namespace Semnox.Parafait.Reports
{
    partial class CreateCustomReport
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
            // CreateCustomReport
            // 
            this.Name = "CreateCustomReport";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.7D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;

        /// <summary>
        /// table1
        ///   </summary>
        public Telerik.Reporting.Table table1;
        public Telerik.Reporting.Table table2;
        /// <summary>
        /// txtExecutionTime
        /// </summary>
        public Telerik.Reporting.TextBox txtExecutionTime;
        /// <summary>
        /// txtPrintedBy
        /// </summary>
        public Telerik.Reporting.TextBox txtPrintedBy;
        /// <summary>
        /// txtPageNumber
        /// </summary>
        public Telerik.Reporting.TextBox txtPageNumber;
        private Telerik.Reporting.TextBox txtReportName;
        private Telerik.Reporting.TextBox txtDateRange;
        private Telerik.Reporting.TextBox txtSelectedSites;
        private Telerik.Reporting.TextBox txtSelectedCategory;
        private Telerik.Reporting.TextBox txtOtherParams;
        private Telerik.Reporting.TextBox txtgroupIndexer;
    }
}