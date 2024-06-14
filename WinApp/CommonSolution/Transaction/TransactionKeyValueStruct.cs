/********************************************************************************************
 * Project Name - TransactionKeyValueStruct Programs
 * Description  - TransactionKeyValueStruct object of TransactionKeyValueStruct used to retturn type for transaction 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        25-May-2016   Rakshith          Created 
 *2.70.2        12-Aug-2019   Deeksha           Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Summary description for TransactionKeyValueStruct
    /// </summary>
    ///
    public class TransactionKeyValueStruct
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _key;
        private string _val;

        /// <summary>
        /// Get/Set method of the Key field
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        /// Get/Set method of the Value field
        /// </summary>
        public string Value
        {
            get { return _val; }
            set { _val = value; }
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionKeyValueStruct()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        ///  Constructor with  parameter
        /// </summary>
        public TransactionKeyValueStruct(string key, string value)
        {
            log.LogMethodEntry("key", value);
            _key = key;
            _val = value;
            log.LogMethodExit();
        }
    }
}
