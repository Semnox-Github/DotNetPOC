/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - Page 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Sql pagination condition
    /// </summary>
    public class Page
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int pageNumber;
        private int pageSize;
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public Page(int pageNumber, int pageSize)
        {
            log.LogMethodEntry(pageNumber, pageSize);
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            log.LogMethodExit();
        }
        /// <summary>
        /// string represention of pagination clause
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            string returnvalue = (" OFFSET " + (pageNumber * pageSize).ToString() + " ROWS FETCH NEXT " + pageSize.ToString() + " ROWS ONLY ");
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }
    }
}
