namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a criteria parameter
    /// </summary>
    public class CriteriaParameter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string name;
        private readonly object value;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public CriteriaParameter(string name, object value)
        {
            log.LogMethodEntry(name, value);
            this.name = name;
            this.value = value;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Method of name field
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Get Method of value field
        /// </summary>
        public object Value
        {
            get { return value; }
        }
    }
}
