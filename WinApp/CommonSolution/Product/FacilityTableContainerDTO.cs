/********************************************************************************************
 * Project Name - Products
 * Description  - Data object of FacilityTableContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.130.00   16-Aug-2021    Prajwal S          Created                                                       
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class FacilityTableContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int tableId;
        private string tableName;
        private int rowIndex;
        private int columnIndex;
        private int facilityId;
        private string tableType;
        private string interfaceInfo1;
        private string interfaceInfo2;
        private string interfaceInfo3;
        private string remarks;
        private string guid;
        private int? maxCheckIns;
        private string tableStatus;
        private int orderId;
        private string customerName;
        private int trxId;
        private int userId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FacilityTableContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the  Facility Tables fields
        /// </summary>
        public FacilityTableContainerDTO(int tableId, string tableName, int rowIndex, int columnIndex, int facilityId, string tableType, string interfaceInfo1, string interfaceInfo2,
                                string interfaceInfo3, string remarks, string guid, int? maxCheckIns)
            : this()
        {
            log.LogMethodEntry(tableId, tableName, rowIndex, columnIndex, facilityId, tableType, interfaceInfo1, interfaceInfo2, interfaceInfo3, remarks, guid,
                                  maxCheckIns);
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
            this.maxCheckIns = maxCheckIns;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set TableId
        /// </summary>
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
        public string TableType { get { return tableType; } set { tableType = value; } }

        /// <summary>
        /// Get/Set InterfaceInfo1
        /// </summary>
        public string InterfaceInfo1 { get { return interfaceInfo1; } set { interfaceInfo1 = value; } }

        /// <summary>
        /// Get/Set InterfaceInfo2
        /// </summary>
        public string InterfaceInfo2 { get { return interfaceInfo2; } set { interfaceInfo2 = value; } }

        /// <summary>
        /// Get/Set InterfaceInfo3
        /// </summary>
        public string InterfaceInfo3 { get { return interfaceInfo3; } set { interfaceInfo3 = value; } }

        /// <summary>
        /// Get/Set Guid
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// Get/Set Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set MaxCheckIns
        /// </summary>
        public int? MaxCheckIns { get { return maxCheckIns; } set { maxCheckIns = value; } }

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
    }
}
