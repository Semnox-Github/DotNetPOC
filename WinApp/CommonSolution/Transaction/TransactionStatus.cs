/********************************************************************************************
 * Class Name - Transaction                                                                       
 * Description - TransactionStatus 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Transaction
{
      /// <summary>
    ///  TransactionStruct Class
    /// </summary>
    public class TransactionStatus
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool isExecuted;
        private string message;
        private string status;
      
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionStatus()
        {
            log.LogMethodEntry();
            this.message = "";
            this.status = "";
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ExecutionStatus field
        /// </summary>
        public bool IsExecuted { get { return isExecuted; } set { isExecuted = value; } }

        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>
        public string Message { get { return message; } set { message = value; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status { get { return status; } set { status = value; } }

    }
}
