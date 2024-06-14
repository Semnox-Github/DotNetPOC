/********************************************************************************************
 * Project Name - KDSUtils
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.40.0       05-Sep-2018   Archana                 Modified for RDS changes3
 *2.50.0      03-Dec-2018   Mathew Ninan            deprecated staticdataexchange
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using System.Reflection;
using Semnox.Parafait.Transaction.KDS;

namespace Semnox.Parafait.Transaction
{
    public class KDSUtilsDecommissioned
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        ParafaitEnv ParafaitEnv;
        TransactionUtils trxUtils;


        public string KDSTerminalMode;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="ParafaitUtilities">Utilities</param>
        public KDSUtils1(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;
            ParafaitEnv = ParafaitUtilities.ParafaitEnv;

            trxUtils = new TransactionUtils(Utilities);

            log.LogMethodExit(null);
        }
    }
}
