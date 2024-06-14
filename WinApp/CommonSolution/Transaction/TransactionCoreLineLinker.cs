/********************************************************************************************
 * Project Name - TransactionDetails Programs
 * Description  - TransactionDetails object of TransactionDetails
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        15-June-2016   Rakshith          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionLineLinker Class
    /// </summary>
    public class TransactionCoreLineLinker
    {
        int transactionLineIdx;
        Transaction.TransactionLine transactionLine;


        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionCoreLineLinker()
        {
            transactionLineIdx = -1;
            transactionLine = null;
        }
        /// <summary>
        ///  Constructor with  parameter
        /// </summary>
        public TransactionCoreLineLinker(int trxLineIdx, Transaction.TransactionLine trxLine)
        {
            transactionLineIdx = trxLineIdx;
            transactionLine = trxLine;
        }

        /// <summary>
        /// Get/Set method of the TransactionLineIdx field
        /// </summary>
        public int TransactionLineIdx { get { return transactionLineIdx; } }

        /// <summary>
        /// Get/Set method of the TransactionLine field
        /// </summary>
        public Transaction.TransactionLine TransactionLine { get { return transactionLine; } }
    }
}

