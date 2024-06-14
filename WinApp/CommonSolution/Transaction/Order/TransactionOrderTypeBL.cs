/****************************************************************************************************************
 * Project Name - TransactionOrderTypeBL
 * Description  - Transaction Order Type
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *****************************************************************************************************************
 *2.80        26-Jun-2020      Raghuveera     Created 
 *****************************************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionOrderTypeBL
    {
        private TransactionOrderTypeDTO transactionOrderTypeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private TransactionOrderTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionOrderTypeDTO"></param>
        public TransactionOrderTypeBL(ExecutionContext executionContext, TransactionOrderTypeDTO transactionOrderTypeDTO)
        {
            log.LogMethodEntry(executionContext, transactionOrderTypeDTO);
            this.executionContext = executionContext;
            this.transactionOrderTypeDTO = transactionOrderTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public TransactionOrderTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionOrderTypeDataHandler transactionOrderTypeDataHandler = new TransactionOrderTypeDataHandler(sqlTransaction);
            transactionOrderTypeDTO = transactionOrderTypeDataHandler.GetTransactionOrderType(id);
            if (transactionOrderTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Transaction Order Type", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }             

        /// <summary>
        /// Saves the transaction order type
        /// Checks if the assetgroup id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (transactionOrderTypeDTO.IsChanged == false
                    && transactionOrderTypeDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            TransactionOrderTypeDataHandler transactionOrderTypeDataHandler = new TransactionOrderTypeDataHandler(sqlTransaction);
            if (transactionOrderTypeDTO.Id < 0)
            {
                transactionOrderTypeDTO = transactionOrderTypeDataHandler.InsertTransactionOrderType(transactionOrderTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionOrderTypeDTO.AcceptChanges();
            }
            else
            {
                if (transactionOrderTypeDTO.IsChanged == true)
                {
                    transactionOrderTypeDTO = transactionOrderTypeDataHandler.UpdateTransactionOrderType(transactionOrderTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionOrderTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionOrderTypeDTO GetTransactionOrderTypeDTO { get { return transactionOrderTypeDTO; } }
    }

    /// <summary>
    /// Manages the list of transaction order types
    /// </summary>
    public class TransactionOrderTypeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<TransactionOrderTypeDTO> transactionOrderTypeList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public TransactionOrderTypeList(ExecutionContext executionContext)
        {
            log.LogMethodExit(executionContext);
            this.executionContext = executionContext;
            this.transactionOrderTypeList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized  Constructor
        /// </summary>
        /// <param name="transactionOrderTypeList"></param>
        /// <param name="executionContext"></param>
        public TransactionOrderTypeList(List<TransactionOrderTypeDTO> transactionOrderTypeList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(transactionOrderTypeList, executionContext);
            this.transactionOrderTypeList = transactionOrderTypeList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the transaction order type list
        /// </summary>
        public List<TransactionOrderTypeDTO> GetAllTransactionOrderTypes(List<KeyValuePair<TransactionOrderTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            TransactionOrderTypeDataHandler transactionOrderTypeDataHandler = new TransactionOrderTypeDataHandler(sqlTransaction);
            List<TransactionOrderTypeDTO> transactionOrderTypeDTOList = transactionOrderTypeDataHandler.GetTransactionOrderTypeDTOList(searchParameters);
            log.LogMethodExit(transactionOrderTypeDTOList);
            return transactionOrderTypeDTOList;
        }

        /// <summary>
        /// Saves the TransactionOrderTypes collection
        /// </summary>
        public void Save()
        {
            using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTransaction.BeginTransaction();
                    log.LogMethodEntry();

                    if (transactionOrderTypeList != null && transactionOrderTypeList.Any())
                    {
                        foreach (TransactionOrderTypeDTO transactionOrderTypeDTO in transactionOrderTypeList)
                        {
                            TransactionOrderTypeBL transactionOrderType = new TransactionOrderTypeBL(executionContext, transactionOrderTypeDTO);
                            transactionOrderType.Save(parafaitDBTransaction.SQLTrx);                            
                        }
                        parafaitDBTransaction.EndTransaction();
                    }
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    parafaitDBTransaction.RollBack();
                    log.Error(ex.Message, ex);
                    log.LogMethodExit(ex, ex.Message);
                    throw;
                }
            }
        }
    }
}
