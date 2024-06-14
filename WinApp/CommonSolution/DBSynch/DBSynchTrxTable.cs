/********************************************************************************************
 * Project Name - DBSynchTrxTable
 * Description  - Bussiness logic of Db synchTrx
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        21-Oct-2019   Rakesh Kumar   Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DBSynch
{
    public class DBSynchTrxTable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal string TableName;
        internal string TrxDateColumn;

        public List<DBSynchTrxTable> GetDBSynchTrxTables()
        {
            log.LogMethodEntry();
            DataTable dt = new Utilities().executeDataTable("exec DBSynchTrxTables");
            List<DBSynchTrxTable> list = new List<DBSynchTrxTable>();
            list = (from DataRow row in dt.Rows
                    select new DBSynchTrxTable
                    {
                        TableName = row["TableName"].ToString(),
                        TrxDateColumn = row["TrxDateColumn"].ToString()
                    }).ToList();
            log.LogMethodExit(list);
            return list;
        }
    }
}
