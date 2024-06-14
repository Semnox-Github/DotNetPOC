/********************************************************************************************
 * Project Name - TransactionSplitPayments BL
 * Description  - Business logict
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jun-2017      Lakshminarayana     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for TransactionSplitPayments class.
    /// </summary>
    public class TransactionSplitPaymentsBL
    {
        TransactionSplitPaymentsDTO transactionSplitPaymentsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of TransactionSplitPaymentsBL class
        /// </summary>
        public TransactionSplitPaymentsBL()
        {
            log.Debug("Starts-TransactionSplitPaymentsBL() default constructor.");
            transactionSplitPaymentsDTO = null;
            log.Debug("Ends-TransactionSplitPaymentsBL() default constructor.");
        }

        /// <summary>
        /// Constructor with the transactionSplitPayments id as the parameter
        /// Would fetch the transactionSplitPayments object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public TransactionSplitPaymentsBL(int id)
            : this()
        {
            log.Debug("Starts-TransactionSplitPaymentsBL(id) parameterized constructor.");
            TransactionSplitPaymentsDataHandler transactionSplitPaymentsDataHandler = new TransactionSplitPaymentsDataHandler();
            transactionSplitPaymentsDTO = transactionSplitPaymentsDataHandler.GetTransactionSplitPaymentsDTO(id);
            log.Debug("Ends-TransactionSplitPaymentsBL(id) parameterized constructor.");
        }

        /// <summary>
        /// Creates TransactionSplitPaymentsBL object using the TransactionSplitPaymentsDTO
        /// </summary>
        /// <param name="transactionSplitPaymentsDTO">TransactionSplitPaymentsDTO object</param>
        public TransactionSplitPaymentsBL(TransactionSplitPaymentsDTO transactionSplitPaymentsDTO)
            : this()
        {
            log.Debug("Starts-TransactionSplitPaymentsBL(transactionSplitPaymentsDTO) Parameterized constructor.");
            this.transactionSplitPaymentsDTO = transactionSplitPaymentsDTO;
            log.Debug("Ends-TransactionSplitPaymentsBL(transactionSplitPaymentsDTO) Parameterized constructor.");
        }

        /// <summary>
        /// Saves the TransactionSplitPayments
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            TransactionSplitPaymentsDataHandler transactionSplitPaymentsDataHandler = new TransactionSplitPaymentsDataHandler();
            if(transactionSplitPaymentsDTO.SplitId < 0)
            {
                int id = transactionSplitPaymentsDataHandler.InsertTransactionSplitPayments(transactionSplitPaymentsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                transactionSplitPaymentsDTO.SplitId = id;
                transactionSplitPaymentsDTO.AcceptChanges();
            }
            else
            {
                if(transactionSplitPaymentsDTO.IsChanged)
                {
                    transactionSplitPaymentsDataHandler.UpdateTransactionSplitPayments(transactionSplitPaymentsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    transactionSplitPaymentsDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method.");
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionSplitPaymentsDTO TransactionSplitPaymentsDTO
        {
            get
            {
                return transactionSplitPaymentsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of TransactionSplitPayments
    /// </summary>
    public class TransactionSplitPaymentsListBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the TransactionSplitPayments list
        /// </summary>
        public List<TransactionSplitPaymentsDTO> GetTransactionSplitPaymentsDTOList(List<KeyValuePair<TransactionSplitPaymentsDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetTransactionSplitPaymentsDTOList(searchParameters) method");
            TransactionSplitPaymentsDataHandler transactionSplitPaymentsDataHandler = new TransactionSplitPaymentsDataHandler();
            log.Debug("Ends-GetTransactionSplitPaymentsDTOList(searchParameters) method by returning the result of transactionSplitPaymentsDataHandler.GetTransactionSplitPaymentsDTOList(searchParameters) call");
            return transactionSplitPaymentsDataHandler.GetTransactionSplitPaymentsDTOList(searchParameters);
        }

    }
}
