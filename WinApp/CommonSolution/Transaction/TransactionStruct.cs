/********************************************************************************************
 * Project Name - TransactionStruct Programs
 * Description  - Data object of Transaction Struct
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-june-2016   Rakshith          Created 
 *2.70.2        12-Aug-2019    Deeksha           Added logger methods
 ********************************************************************************************/


namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  TransactionStruct Class
    /// </summary>
    public class TransactionStruct
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int transactionId;
        private double transactionPreTaxAmount;
        private double transactionTaxAmount;
        private double transactionFinalAmount;
        private double transactionRoundOffAmount;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionStruct()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public TransactionStruct(int transactionId, double transactionPreTaxAmount, double transactionTaxAmount, double transactionFinalAmount, double transactionRoundOffAmount)
        {
            log.LogMethodEntry(transactionId, transactionPreTaxAmount, transactionTaxAmount, transactionFinalAmount, transactionRoundOffAmount);
            this.transactionId = transactionId;
            this.transactionPreTaxAmount = transactionPreTaxAmount;
            this.transactionTaxAmount = transactionTaxAmount;
            this.transactionRoundOffAmount = transactionRoundOffAmount;
            this.transactionFinalAmount = transactionFinalAmount;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int TransactionId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set method of the TransactionPreTaxAmount field
        /// </summary>
        public double TransactionPreTaxAmount { get { return transactionPreTaxAmount; } set { transactionPreTaxAmount = value; } }

        /// <summary>
        /// Get/Set method of the TransactionTaxAmount field
        /// </summary>
        public double TransactionTaxAmount { get { return transactionTaxAmount; } set { transactionTaxAmount = value; } }

        /// <summary>
        /// Get/Set method of the TransactionFinalAmount field
        /// </summary>
        public double TransactionFinalAmount { get { return transactionFinalAmount; } set { transactionFinalAmount = value; } }

        /// <summary>
        /// Get/Set method of the TransactionRoundOffAmount field
        /// </summary>
        public double TransactionRoundOffAmount { get { return transactionRoundOffAmount; } set { transactionRoundOffAmount = value; } }

    }
}
