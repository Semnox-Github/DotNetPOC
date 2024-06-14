/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - AndCriteria
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/


namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// a complex criteria with logical and operator
    /// </summary>
    public class AndCriteria : ComplexCriteria
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="sqlParameterNameProvider"></param>
        /// <param name="leftHandCriteria"></param>
        /// <param name="rightHandCriteria"></param>
        public AndCriteria(ColumnProvider columnProvider, SqlParameterNameProvider sqlParameterNameProvider, Criteria leftHandCriteria, Criteria rightHandCriteria) : base(columnProvider, sqlParameterNameProvider)
        {
            log.LogMethodEntry(columnProvider, sqlParameterNameProvider, leftHandCriteria, rightHandCriteria);
            criteriaList.Add(leftHandCriteria);
            criteriaList.Add(rightHandCriteria);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates a new complex criteria with logical and operator
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public override Criteria And(Criteria criteria)
        {
            log.LogMethodEntry();
            criteriaList.Add(criteria);
            log.LogMethodExit();
            return this;
        }

        /// <summary>
        /// return logical operators
        /// </summary>
        /// <returns></returns>
        public override string GetLogialOperator()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return "AND";
        }
    }
}
