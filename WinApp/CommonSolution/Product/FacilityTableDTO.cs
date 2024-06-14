/********************************************************************************************
 * Project Name - FacilityTableDTO
 * Description  - FacilityTable DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
  *2.60        14-May-2019  Mushahid Faizan          Modified Active datatype from char to bool and 
  *                                                  Added ISACTIVE SearchByParameters, IsChanged property,AcceptChanges method and Parameterized constructor.
  *2.70        25-Jun-2019   Mathew Ninan            Adding remaining columns
  *2.70        25-Feb-2020   Girish Kundar           Modified : 3 Tier changes for API
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// FacilityTableDTO class
    /// </summary>
    public class FacilityTableDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public const string VACANT = "Vacant";
        public const string OCCUPIED = "Occupied";

        ///// <summary>
        ///// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by facilityId
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by facilityId
            /// </summary>
            FACILITY_ID_LIST,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            ///  Search by Active
            /// </summary>
            ISACTIVE
        }

        private int tableId;
        private string tableName;
        private int rowIndex;
        private int columnIndex;
        private int facilityId;
        private string tableType;
        private string interfaceInfo1;
        private string interfaceInfo2;
        private string interfaceInfo3;
        private bool isActive;
        private string remarks;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int? maxCheckIns;
        private int masterEntityId;
        private string tableStatus;
        private int orderId;
        private string customerName;
        private int trxId;
        private int userId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FacilityTableDTO()
        {
            log.LogMethodEntry();
            TableId = -1;
            RowIndex = -1;
            ColumnIndex = -1;
            FacilityId = -1;
            SiteId = -1;
            customerName = "";
            orderId = -1;
            trxId = -1;
            MasterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default Parameterized Constructor
        /// </summary>
        public FacilityTableDTO(int tableId, string tableName, int rowIndex, int columnIndex,
                        string tableStatus, int orderId, string customerName, int trxId,
                        string remarks, int userId)
        {
            log.LogMethodEntry(tableId, tableName, rowIndex, columnIndex, tableStatus, orderId,
                         customerName, trxId, remarks, userId);
            this.tableId = tableId;
            this.tableName = tableName;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.tableStatus = tableStatus;
            this.orderId = orderId;
            this.customerName = customerName;
            this.trxId = trxId;
            this.remarks = remarks;
            this.userId = userId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default Parameterized Constructor
        /// </summary>
        public FacilityTableDTO(int tableId, string tableName, int rowIndex, int columnIndex,
                                            string tableStatus, int orderId, string customerName, int trxId, string remarks,
                                            int userId, int facilityId, string tableType, string interfaceInfo1, string interfaceInfo2, string interfaceInfo3)
        : this()
        {
            log.LogMethodEntry(tableId, tableName, rowIndex, columnIndex, tableStatus, orderId,
                customerName, trxId, remarks, userId, facilityId, tableType, interfaceInfo1,
                interfaceInfo2, interfaceInfo3);
            this.facilityId = facilityId;
            this.interfaceInfo1 = interfaceInfo1;
            this.interfaceInfo2 = interfaceInfo2;
            this.interfaceInfo3 = interfaceInfo3;
            this.tableType = tableType;
            this.tableId = tableId;
            this.tableName = tableName;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.tableStatus = tableStatus;
            this.orderId = orderId;
            this.customerName = customerName;
            this.trxId = trxId;
            this.remarks = remarks;
            this.userId = userId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the  Facility Tables fields
        /// </summary>
        public FacilityTableDTO(int tableId, string tableName, int rowIndex, int columnIndex, int facilityId, string tableType, string interfaceInfo1, string interfaceInfo2,
                                string interfaceInfo3, bool active, string remarks, string guid, int siteId, bool synchStatus, int? maxCheckIns, int masterEntityId,
                                 string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this()
        {
            log.LogMethodEntry(tableId, tableName, rowIndex, columnIndex, facilityId, tableType, interfaceInfo1, interfaceInfo2, interfaceInfo3, active, remarks, guid, siteId, synchStatus,
                                  maxCheckIns, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.facilityId = facilityId;
            this.interfaceInfo1 = interfaceInfo1;
            this.interfaceInfo2 = interfaceInfo2;
            this.interfaceInfo3 = interfaceInfo3;
            this.tableType = tableType;
            this.tableId = tableId;
            this.tableName = tableName;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.remarks = remarks;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.maxCheckIns = maxCheckIns;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.Active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set TableId
        /// </summary>
        [DisplayName("TableId")]
        public int TableId { get { return tableId; } set { tableId = value; } }

        /// <summary>
        /// Get/Set TableName
        /// </summary>

        public string TableName { get { return tableName; } set { tableName = value; } }

        /// <summary>
        /// Get/Set RowIndex
        /// </summary>
        public int RowIndex { get { return rowIndex; } set { rowIndex = value; } }

        /// <summary>
        /// Get/Set ColumnIndex
        /// </summary>
        public int ColumnIndex { get { return columnIndex; } set { columnIndex = value; } }

        /// <summary>
        /// Get/Set FacilityId
        /// </summary>
        public int FacilityId { get { return facilityId; } set { facilityId = value; } }

        /// <summary>
        /// Get/Set TableType
        /// </summary>
        [DefaultValue("")]
        public string TableType { get { return tableType; } set { tableType = value; } }

        /// <summary>
        /// Get/Set InterfaceInfo1
        /// </summary>
        [DefaultValue("")]
        public string InterfaceInfo1 { get { return interfaceInfo1; } set { interfaceInfo1 = value; } }

        /// <summary>
        /// Get/Set InterfaceInfo2
        /// </summary>
        [DefaultValue("")]
        public string InterfaceInfo2 { get { return interfaceInfo2; } set { interfaceInfo2 = value; } }

        /// <summary>
        /// Get/Set InterfaceInfo3
        /// </summary>
        public string InterfaceInfo3 { get { return interfaceInfo3; } set { interfaceInfo3 = value; } }

        /// <summary>
        /// Get/Set Guid
        /// </summary>
        public bool Active { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set Guid
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// Get/Set Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set site_id
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set SynchStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set MaxCheckIns
        /// </summary>
        public int? MaxCheckIns { get { return maxCheckIns; } set { maxCheckIns = value; } }

        /// <summary>
        /// Get/Set MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set userId
        /// </summary>
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Get/Set tableStatus
        /// </summary>
        public string TableStatus { get { return tableStatus; } set { tableStatus = value; } }

        /// <summary>
        /// Get/Set trxId
        /// </summary>
        public int TrxId { get { return trxId; } set { trxId = value; } }

        /// <summary>
        /// Get/Set orderId
        /// </summary>
        public int OrderId { get { return orderId; } set { orderId = value; } }

        /// <summary>
        /// Get/Set CustomerName
        /// </summary>
        public string CustomerName { get { return customerName; } set { customerName = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
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
                    return notifyingObjectIsChanged || tableId < 0  ;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
