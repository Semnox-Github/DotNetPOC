/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - LocalRemoteConnectionCheckService used to get latest ping time of the server connectivity from local DB
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Amitha Joy                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Data.SqlClient;

namespace Semnox.Core.Utilities
{
    public class LocalRemoteConnectionCheckDataService : IRemoteConnectionCheck
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;

        public LocalRemoteConnectionCheckDataService()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public DateTime? Get()
        {
            log.LogMethodEntry();
            DateTime? result = null;
            string result1 = null;
            dataAccessHandler = new DataAccessHandler();
            try
            {
                result1 = dataAccessHandler.executeScalar(@"SELECT getdate() ", null, sqlTransaction).ToString();

                if (result1 == null)
                    result = DateTime.MinValue;
                else
                    result = Convert.ToDateTime(result1).ToUniversalTime();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = DateTime.MinValue;
            }

            log.LogMethodExit(result);
            return result;
        }

    }
}
