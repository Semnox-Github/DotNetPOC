/********************************************************************************************
 * Project Name - Job Utils
 * Description  - Inventory details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;
using System.Data;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// the class implements Iquery interface, this will be handle all Members Template File related queries
    /// </summary>
    public class InventoryQuery : IQuery
    {
        /// <summary>
        /// Property to hold filename
        /// </summary>
        public string FileName { get; set; }

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;

        /// <summary>
        /// Parameterised constructor to set class properties
        /// </summary>
        /// <param name="_Utilities">parafait utility object</param>
        public InventoryQuery(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            FileName = ConfigurationManager.AppSettings["InventoryFileName"];
            log.LogMethodExit();
        }

        /// <summary>
        /// To get Inventory list for the curent month
        /// </summary>
        /// <returns>Execute Query and load into datatable</returns>
        public DataTable GetQueryResult()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable("exec usp_Inventory ");
            log.LogMethodExit(dt);
            return dt;
        }
    }
}
