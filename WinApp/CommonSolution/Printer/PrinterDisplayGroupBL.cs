/********************************************************************************************
 * Project Name - Printer DisplayGroup BL
 * Description  - Business Logic to manage Printer Display groups
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2018      Mathew Ninan   Created 
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;

using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// BL class for PrinterDisplayGroup
    /// </summary>
    public class PrinterDisplayGroupBL
    {
        private PrinterDisplayGroupDTO printerDisplayGroupDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PrinterDisplayGroupBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public PrinterDisplayGroupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            printerDisplayGroupDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Printer Display Group Id as the parameter
        /// Would fetch the PrinterDisplayGroup object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public PrinterDisplayGroupBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PrinterDisplayGroupDataHandler printerDisplayGroupDataHandler = new PrinterDisplayGroupDataHandler(sqlTransaction);
            printerDisplayGroupDTO = printerDisplayGroupDataHandler.GetPrinterDisplayGroup(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PrinterDisplayGroupBL object using the printerDisplayGroupDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="printerDisplayGroupDTO">printerDisplayGroupDTO object</param>
        public PrinterDisplayGroupBL(ExecutionContext executionContext, PrinterDisplayGroupDTO printerDisplayGroupDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, printerDisplayGroupDTO);
            this.printerDisplayGroupDTO = printerDisplayGroupDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PrinterDisplayGroup
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PrinterDisplayGroupDataHandler printerDisplayGroupDataHandler = new PrinterDisplayGroupDataHandler(sqlTransaction);
            if (printerDisplayGroupDTO.PrinterDisplayGroupId < 0)
            {
                printerDisplayGroupDTO = printerDisplayGroupDataHandler.InsertPrinterDisplayGroup(printerDisplayGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());      
                printerDisplayGroupDTO.AcceptChanges();
            }
            else
            {
                if (printerDisplayGroupDTO.IsChanged)
                {
                    printerDisplayGroupDTO = printerDisplayGroupDataHandler.UpdatePrinterDisplayGroup(printerDisplayGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    printerDisplayGroupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PrinterDisplayGroupDTO PrinterDisplayGroupDTO
        {
            get
            {
                return printerDisplayGroupDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of PrinterDisplayGroup
    /// </summary>
    public class PrinterDisplayGroupListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public PrinterDisplayGroupListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PrinterDisplayGroup list
        /// </summary>
        /// <param name="searchParameters">SearchParameters</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <returns>Returs list</returns>
        public List<PrinterDisplayGroupDTO> GetPrinterDisplayGroupDTOList(List<KeyValuePair<PrinterDisplayGroupDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PrinterDisplayGroupDataHandler printerDisplayGroupDataHandler = new PrinterDisplayGroupDataHandler(sqlTransaction);
            List<PrinterDisplayGroupDTO> returnValue = printerDisplayGroupDataHandler.GetPrinterDisplayGroupList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
