/********************************************************************************************
* Project Name - Report DTO
* Description  - Data object of user
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        14-Apr-2017   Amaresh          Created 
*2.70.2      12-Jul-2019   Dakshakh raj     Modified : Added Parameterized costrustor, 
                                            Added WHO fields.
*2.80        15-June-2019  Laster menezes   Added new RepeatBreakColumns column to ReportsDTO
*2.100       28-Sep-2020   Laster Menezes   Added new columns HeaderBackgroundColor, HeaderTextColor to ReportsDTO
*2.100       15-Oct-2020   Laster Menezes   Added new column RowCountPerPage to ReportsDTO
*2.110       20-Dec-2020   Nitin Pai        Dashboard Changes - Added new variable IsDashboard
*2.120       26-Apr-2021   Laster Menezes   Added new variables PageSize, PageWidth, PageHeight, IsPortrait, IsReceipt
*2.130       01-Jul-2021   Laster Menezes   Added new columns DashboardType, IsActive
*2.140       09-Sep-2021   Laster Menezes   added DashboardReportType Enum
********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    ///  This is the Reports data object class. This acts as data holder for the Reports business object
    /// </summary>  
    public class ReportsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByReportsParameters
        {
            /// <summary>
            /// Search by REPORT ID field
            /// </summary>
            REPORT_ID,

            /// <summary>
            /// Search by REPORT NAME field
            /// </summary>
            REPORT_NAME,

            /// <summary>
            /// Search by REPORT KEY field
            /// </summary>
            REPORT_KEY ,

            /// <summary>
            /// Search by SITE ID field
            /// </summary> 
            SITE_ID,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID,

            /// <summary>
            /// Search by CUSTOMFLAG field
            /// </summary>
            CUSTOMFLAG,

            /// <summary>
            /// Search by REPORT_GROUP field
            /// </summary>
            REPORT_GROUP,

            /// <summary>
            /// Search by IS_DASHBOARD field
            /// </summary>
            IS_DASHBOARD,

            /// <summary>
            /// Search by IS_RECEIPT field
            /// </summary>
            IS_RECEIPT,

            /// <summary>
            /// Search by DASHBOARD_TYPE field
            /// </summary>
            DASHBOARD_TYPE,

            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

        public enum DashboardReportType
        {
            ///<summary>
            ///RESERVATION
            ///</summary>
            [Description("TABLEAU")] T,

            ///<summary>
            ///WALK-IN
            ///</summary>
            [Description("TELERIK REPORT")] R
        }

        private int reportId;
        private string reportName;
        private string reportKey;
        private string customFlag;
        private string outputFormat;
        private string displayOutputFormat;
        private string dbQuery;
        private string breakColumn;
        private string hideBreakColumn;
        private string reportGroup;
        private string aggregateColumns;
        private string guid;
        private int siteId;
        private string hideGridLines;
        private bool synchStatus;
        private int masterEntityId;
        private string showGrandTotal;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string printContinuous;
        private string repeatBreakColumns;
        private int maxDateRange;
        private string headerBackgroundColor;
        private string headerTextColor;
        private int rowCountPerPage;
        private bool isDashboard;
        private int pageSize;
        private double pageWidth;
        private double pageHeight;
        private bool isPortrait;
        private bool isReceipt;
        private string dashboardType;
        private bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportsDTO()
        {
            log.LogMethodEntry();
            reportId = -1;
            breakColumn = "";
            siteId = -1;
            masterEntityId = -1;
            customFlag = "Y";
            dbQuery = "";
            showGrandTotal = "N";
            printContinuous = "N";
            repeatBreakColumns = "N";
            maxDateRange = -1;
            headerBackgroundColor = string.Empty;
            rowCountPerPage = -1;
            headerTextColor = string.Empty;
            isDashboard = false;
            pageSize =-1;
            pageWidth =0;
            pageHeight =0;
            isPortrait = false;
            isReceipt = false;
            dashboardType = string.Empty;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReportsDTO(int reportId, string reportName, string reportKey, string customFlag, string outputFormat, string displayOutputFormat, string dbQuery,
                            string breakColumn, string hideBreakColumn, string reportGroup, string aggregateColumns, string hideGridLines, string showGrandTotal, 
                            string printContinuous, string repeatBreakColumns, int maxDateRange, string headerBackgroundColor, string headerTextColor, int rowCountPerPage,
                            bool isDashboard, int pageSize, double pageWidth, double pageHeight, bool isPortrait, bool isReceipt, string dashboardType, bool isActive)
            : this()
        {
            log.LogMethodEntry( reportId,  reportName,  reportKey,  customFlag,  outputFormat,  displayOutputFormat,  dbQuery,
                             breakColumn,  hideBreakColumn,  reportGroup,  aggregateColumns,  hideGridLines,  showGrandTotal,  
                             printContinuous, repeatBreakColumns, maxDateRange, headerBackgroundColor, headerTextColor, rowCountPerPage, isDashboard, pageSize, 
                             pageWidth, pageHeight, isPortrait, isReceipt, dashboardType, isActive);
            this.reportId = reportId;
            this.reportName = reportName;
            this.reportKey = reportKey;
            this.customFlag = customFlag;
            this.outputFormat = outputFormat;
            this.displayOutputFormat = displayOutputFormat;
            this.dbQuery = dbQuery;
            this.breakColumn = breakColumn;
            this.hideBreakColumn = hideBreakColumn;
            this.reportGroup = reportGroup;
            this.aggregateColumns = aggregateColumns;
            this.hideGridLines = hideGridLines;
            this.showGrandTotal = showGrandTotal;
            this.printContinuous = printContinuous;
            this.repeatBreakColumns = repeatBreakColumns;
            this.maxDateRange = maxDateRange;
            this.headerBackgroundColor = headerBackgroundColor;
            this.headerTextColor = headerTextColor;
            this.rowCountPerPage = rowCountPerPage;
            this.isDashboard = isDashboard;
            this.pageSize = pageSize;
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
            this.isPortrait = isPortrait;
            this.isReceipt = isReceipt;
            this.dashboardType = dashboardType;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        ///Parameterized constructor with all the data fields
        /// </summary>
        public ReportsDTO(int reportId, string reportName, string reportKey, string customFlag, string outputFormat, string displayOutputFormat, string dbQuery,
                            string breakColumn, string hideBreakColumn, string reportGroup, string aggregateColumns, string guid, int siteId,
                            string hideGridLines, bool synchStatus, int masterEntityId, string showGrandTotal, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                            DateTime lastUpdateDate, string printContinuous, string repeatBreakColumns, int maxDateRange, string headerBackgroundColor, string headerTextColor, int rowCountPerPage,
                            bool isDashboard, int pageSize, double pageWidth, double pageHeight, bool isPortrait, bool isReceipt, string dashboardType, bool isActive)
            :this(reportId, reportName, reportKey, customFlag, outputFormat, displayOutputFormat, dbQuery, breakColumn, hideBreakColumn, reportGroup, aggregateColumns, hideGridLines, showGrandTotal, 
                 printContinuous, repeatBreakColumns, maxDateRange, headerBackgroundColor, headerTextColor, rowCountPerPage, isDashboard, pageSize,
                             pageWidth, pageHeight, isPortrait, isReceipt, dashboardType, isActive)
        {
            log.LogMethodEntry();
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get/Set method of the reportId field
        /// </summary>
        [DisplayName("reportId")]
        [ReadOnly(true)]
        public int ReportId { get { return reportId; } set { reportId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportName field
        /// </summary>
        [DisplayName("ReportName")]
        public string ReportName { get { return reportName; } set { reportName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportKey field
        /// </summary>
        [DisplayName("ReportKey")]
        public string ReportKey { get { return reportKey; } set { reportKey = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustomFlag field
        /// </summary>
        [DisplayName("CustomFlag")]
        public string CustomFlag { get { return customFlag; } set { customFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OutputFormat field
        /// </summary>
        [DisplayName("OutputFormat")]
        [Browsable(false)]
        public string OutputFormat { get { return outputFormat; } set { outputFormat = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayOutputFormat field
        /// </summary>
        [DisplayName("OutputFormat")]
        public string DisplayOutputFormat { get { return displayOutputFormat; } set { displayOutputFormat = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DBQuery field
        /// </summary>
        [DisplayName("DBQuery")]
        public string DBQuery { get { return dbQuery; } set { dbQuery = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BreakColumn field
        /// </summary>
        [DisplayName("BreakColumn")]
        public string BreakColumn { get { return breakColumn; } set { breakColumn = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the HideBreakColumn field
        /// </summary>
        [DisplayName("HideBreakColumn")]
        public string HideBreakColumn { get { return hideBreakColumn; } set { hideBreakColumn = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportGroup field
        /// </summary>
        [DisplayName("ReportGroup")]
        public string ReportGroup { get { return reportGroup; } set { reportGroup = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AggregateColumns field
        /// </summary>
        [DisplayName("AggregateColumns")]
        public string AggregateColumns { get { return aggregateColumns; } set { aggregateColumns = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the HideGridLines field
        /// </summary>
        [DisplayName("HideGridLines")]
        public string HideGridLines { get { return hideGridLines; } set { hideGridLines = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId

        /// <summary>
        /// Get/Set method of the ShowGrandTotal field
        /// </summary>
        [DisplayName("ShowGrandTotal")]
        public string ShowGrandTotal { get { return showGrandTotal; } set { showGrandTotal = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the PrintContinuous field
        /// </summary>
        [DisplayName("PrintContinuous")]
        public string PrintContinuous { get { return printContinuous; } set { printContinuous = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RepeatBreakColumns field
        /// </summary>
        [DisplayName("RepeatBreakColumns")]
        public string RepeatBreakColumns { get { return repeatBreakColumns; } set { repeatBreakColumns = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MaxDateRange field
        /// </summary>
        [DisplayName("MaxDateRange")]
        public int MaxDateRange { get { return maxDateRange; } set { maxDateRange = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the HeaderBackgroundColor field
        /// </summary>
        [DisplayName("Header Background Color")]
        public string HeaderBackgroundColor { get { return headerBackgroundColor; } set { headerBackgroundColor = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the HeaderTextColor field
        /// </summary>
        [DisplayName("Header Text Color")]
        public string HeaderTextColor { get { return headerTextColor; } set { headerTextColor = value; this.IsChanged = true; } }
    
        [DisplayName("Row Count Per Page")]
        public int RowCountPerPage { get { return rowCountPerPage; } set { rowCountPerPage = value; this.IsChanged = true; } }

        [DisplayName("IsDashboard")]
        public bool IsDashboard { get { return isDashboard; } set { isDashboard = value; this.IsChanged = true; } }

        [DisplayName("Page Size")]
        public int PageSize { get { return pageSize; } set { pageSize = value; this.IsChanged = true; } }

        [DisplayName("Page Width")]
        public double PageWidth { get { return pageWidth; } set { pageWidth = value; this.IsChanged = true; } }

        [DisplayName("Page Height")]
        public double PageHeight { get { return pageHeight; } set { pageHeight = value; this.IsChanged = true; } }

        [DisplayName("Is Portrait")]
        public bool IsPortrait { get { return isPortrait; } set { isPortrait = value; this.IsChanged = true; } }

        [DisplayName("Is Receipt")]
        public bool IsReceipt { get { return isReceipt; } set { isReceipt = value; this.IsChanged = true; } }

        [DisplayName("Dashboard Type")]
        public string DashboardType { get { return dashboardType; } set { dashboardType = value; this.IsChanged = true; } }

        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || reportId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            {
                log.LogMethodEntry();
                this.IsChanged = false;
                log.LogMethodExit();
            }
        }
    }
}
