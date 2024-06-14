/********************************************************************************************
 * Project Name - Printer Products BL
 * Description  - Business logic to handle Printer Products
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2018      Mathew Ninan   Created
 *2.70.2        18-Jul-2019      Deeksha       Modifications as per 3 tier standard.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// BL class for Printerproducts
    /// </summary>
    public class PrinterProductsBL
    {
        private PrinterProductsDTO printerProductsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PrinterProductsBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public PrinterProductsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            printerProductsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Printer Product Id as the parameter
        /// Would fetch the PrinterProducts object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public PrinterProductsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PrinterProductsDataHandler printerProductsDataHandler = new PrinterProductsDataHandler(sqlTransaction);
            printerProductsDTO = printerProductsDataHandler.GetPrinterProducts(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates PrinterProductsBL object using the printerProductsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="printerProductsDTO">printerProductsDTO object</param>
        public PrinterProductsBL(ExecutionContext executionContext, PrinterProductsDTO printerProductsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, printerProductsDTO);
            this.printerProductsDTO = printerProductsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the PrinterProducts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="sqlTransaction">SQL Transaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            PrinterProductsDataHandler printerProductsDataHandler = new PrinterProductsDataHandler(sqlTransaction);
            if (printerProductsDTO.PrinterProductId < 0)
            {
                printerProductsDTO = printerProductsDataHandler.InsertPrinterProducts(printerProductsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                printerProductsDTO.AcceptChanges();
            }
            else
            {
                if (printerProductsDTO.IsChanged)
                {
                    printerProductsDTO = printerProductsDataHandler.UpdatePrinterProducts(printerProductsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    printerProductsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PrinterProductsDTO PrinterProductsDTO
        {
            get
            {
                return printerProductsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of PrinterProducts
    /// </summary>
    public class PrinterProductsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PrinterProductsDTO> printerProductsList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public PrinterProductsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="printerProductsList">printerProductsList</param>
        public PrinterProductsListBL(ExecutionContext executionContext, List<PrinterProductsDTO> printerProductsList)
        {
            log.LogMethodEntry(executionContext, printerProductsList);
            this.executionContext = executionContext;
            this.printerProductsList = printerProductsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PrinterProducts list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returnValue</returns>
        public List<PrinterProductsDTO> GetPrinterProductsDTOList(List<KeyValuePair<PrinterProductsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PrinterProductsDataHandler printerProductsDataHandler = new PrinterProductsDataHandler(sqlTransaction);
            List<PrinterProductsDTO> returnValue = printerProductsDataHandler.GetPrinterProductsList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
