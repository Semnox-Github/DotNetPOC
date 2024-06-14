/********************************************************************************************
 * Project Name - Vendor Return
 * Description  - Business logic of Vendor Return
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Aug-2016   Suneetha.S          Created 
 *2.70.2        15-Jul-2019   Girish Kundar       Modified : Added LogMethdEntry() and LogMethodExit()
 ********************************************************************************************/
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// VendorReturnsBL class 
    /// </summary>
    public class VendorReturnsBL
    {
        private VendorReturnsDTO vendorReturn;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of VendorReturn Receipts  class
        /// </summary>
        public VendorReturnsBL()
        {
            log.LogMethodEntry();
            vendorReturn = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Receipt Id id as the parameter
        /// Would fetch the Templates object from the database based on the id passed. 
        /// </summary>
        /// <param name="receiptId">receipts id</param>
        public VendorReturnsBL(int receiptId,SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(receiptId,sqlTransaction);
            VendorReturnsDataHandler vendorReturnReceiptsDataHandler = new VendorReturnsDataHandler(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates receipts type object using the vendorReturnDTO
        /// </summary>
        /// <param name="vendorReturnDTO">VendorReturnsDTO object</param>
        public VendorReturnsBL(VendorReturnsDTO vendorReturnDTO)
            : this()
        {
            log.LogMethodEntry(vendorReturnDTO);
            this.vendorReturn = vendorReturnDTO;
            log.LogMethodExit();
        }

        
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public VendorReturnsDTO GetRequisitions { get { return vendorReturn; } }
    }

    /// <summary>
    /// VendorReturnList class
    /// </summary>
    public class VendorReturnList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///// <summary>
        ///// Returns the Receipt records list
        ///// </summary>
        //public List<VendorReturnsDTO> GetAllReceiptsData(List<KeyValuePair<VendorReturnsDTO.SearchByVendorReturnParameters, string>> searchParameters)
        //{
        //    log.Debug("Starts-GetAllReceiptsData(searchParameters) method.");
        //    VendorReturnsDataHandler vendorReturnDataHandler = new VendorReturnsDataHandler();
        //    log.Debug("Ends-GetAllReceiptsData(searchParameters) method by returning the result of RequisitionDataHandler.GetAllRequisitions() call.");
        //    return vendorReturnDataHandler.GetReceiptsList(searchParameters);
        //}

        /// <summary>
        /// Returns the Receipt record
        /// </summary>
        public VendorReturnsDTO GetReceiptsData(int poID, int receiptId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(poID, receiptId);
            VendorReturnsDataHandler vendorReturnDataHandler = new VendorReturnsDataHandler(sqlTransaction);
            VendorReturnsDTO vendorReturnsDTO =  vendorReturnDataHandler.GetReceiptsList(poID, receiptId);
            log.LogMethodExit(vendorReturnsDTO);
            return vendorReturnsDTO;
        }
        /// <summary>
        /// Returns the Receipt ids type of string
        /// </summary>
        public DataTable GetReceiptIdsForFilter(string vendorName, string orderNo, string vendorBillNo, string grn , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(vendorName,  orderNo,  vendorBillNo,  grn);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            VendorReturnsDataHandler vendorReturnDataHandler = new VendorReturnsDataHandler(sqlTransaction);
            DataTable dataTable = vendorReturnDataHandler.GetReceiptsIdsOnSearchKeys(vendorName, orderNo, vendorBillNo, grn, machineUserContext.GetSiteId());
            log.LogMethodExit(dataTable);
            return dataTable;
        }
    }
}
