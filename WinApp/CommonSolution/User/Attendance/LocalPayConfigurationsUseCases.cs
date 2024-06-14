/********************************************************************************************
 * Project Name - User
 * Description  - LocalPayConfigurationsUseCases class 
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
    class LocalPayConfigurationsUseCases : IPayConfigurationsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalPayConfigurationsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<PayConfigurationsDTO>> GetPayConfigurations(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>>
                          searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<PayConfigurationsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                PayConfigurationsListBL payConfigurationsListBL = new PayConfigurationsListBL(executionContext);
                List<PayConfigurationsDTO> payConfigurationsDTOList = payConfigurationsListBL.GetPayConfigurationsDTOList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(payConfigurationsDTOList);
                return payConfigurationsDTOList;
            });
        }

        //public async Task<int> GetPayConfigurationsCount(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        PayConfigurationsListBL PayConfigurationssListBL = new PayConfigurationsListBL(executionContext);
        //        int count = PayConfigurationssListBL.GetPayConfigurationsCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<PayConfigurationsDTO>> SavePayConfigurations(List<PayConfigurationsDTO> payConfigurationsDTOList)
        {
            return await Task<List<PayConfigurationsDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    PayConfigurationsListBL payConfigurationsList = new PayConfigurationsListBL(executionContext, payConfigurationsDTOList);
                    List<PayConfigurationsDTO> result = payConfigurationsList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
