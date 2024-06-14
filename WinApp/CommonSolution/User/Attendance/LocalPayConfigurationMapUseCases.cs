/********************************************************************************************
 * Project Name - User
 * Description  - LocalPayConfigurationMapUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      01-Apr-2021      Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class LocalPayConfigurationMapUseCases : IPayConfigurationMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalPayConfigurationMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<PayConfigurationMapDTO>> GetPayConfigurationMap(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<PayConfigurationMapDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                PayConfigurationMapListBL payConfigurationMapListBL = new PayConfigurationMapListBL(executionContext);
                List<PayConfigurationMapDTO> payConfigurationMapDTOList = payConfigurationMapListBL.GetPayConfigurationMapDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(payConfigurationMapDTOList);
                return payConfigurationMapDTOList;
            });
        }

        //public async Task<int> GetPayConfigurationMapCount(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        PayConfigurationMapListBL PayConfigurationMapsListBL = new PayConfigurationMapListBL(executionContext);
        //        int count = PayConfigurationMapsListBL.GetPayConfigurationMapCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<PayConfigurationMapDTO>> SavePayConfigurationMap(List<PayConfigurationMapDTO> payConfigurationMapDTOList)
        {
            return await Task<List<PayConfigurationMapDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    PayConfigurationMapListBL payConfigurationMapList = new PayConfigurationMapListBL(executionContext, payConfigurationMapDTOList);
                    List<PayConfigurationMapDTO> result = payConfigurationMapList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
