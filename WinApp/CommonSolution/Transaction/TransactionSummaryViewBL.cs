/*************************************************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Business logic class of transaction
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 ***************************************************************************************************************************
 *2.150.01    16-Feb-2023       Yashodhara C H     Created
 ************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionSummaryViewBL class
    /// </summary>
    public class TransactionSummaryViewBL
    {
        private TransactionSummaryViewDTO transactionSummaryViewDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;


        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public TransactionSummaryViewBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having transactionId
        /// </summary>
        public TransactionSummaryViewBL(ExecutionContext executionContext, int transactionId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionId);
            TransactionSummaryViewDataHandler transactionSummaryViewDataHandler = new TransactionSummaryViewDataHandler();
            transactionSummaryViewDTO = transactionSummaryViewDataHandler.GetTransactionSummaryViewDTO(transactionId);
            if (transactionSummaryViewDTO == null)
            {
                string errorMessage = "Object not found";
                log.Error(errorMessage + " " + transactionId);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// TransactionSummaryViewListBL class
    /// </summary>
    public class TransactionSummaryViewListBL
    {
        /// <summary>
        /// TransactionSummaryViewListBL business logic 
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransactionSummaryViewListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public TransactionSummaryViewListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TransactionSummaryView list
        /// </summary>
        public List<TransactionSummaryViewDTO> GetTransactionSummaryViewDTOList(List<KeyValuePair<TransactionSummaryViewDTO.SearchByParameters, string>> searchParameter, int pageNumber = 0, int numberOfRecords = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameter);
            TransactionSummaryViewDataHandler transactionSummaryViewDataHandler = new TransactionSummaryViewDataHandler(sqlTransaction);
            List<TransactionSummaryViewDTO> transactionSummaryViewDTOsList = transactionSummaryViewDataHandler.GetTransactionSummaryViewDTOList(searchParameter, executionContext, pageNumber, numberOfRecords);
            log.LogMethodExit(transactionSummaryViewDTOsList);
            return transactionSummaryViewDTOsList;
        }

    }
}


