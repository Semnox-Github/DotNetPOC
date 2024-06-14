/********************************************************************************************
 * Project Name - User
 * Description  - LocalShiftConfigurationsUseCases class 
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
    class LocalShiftConfigurationsUseCases : IShiftConfigurationsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalShiftConfigurationsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ShiftConfigurationsDTO>> GetShiftConfigurations(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<ShiftConfigurationsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ShiftConfigurationsListBL shiftConfigurationsListBL = new ShiftConfigurationsListBL(executionContext);
                List<ShiftConfigurationsDTO> shiftConfigurationsDTOList = shiftConfigurationsListBL.GetShiftConfigurationsDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(shiftConfigurationsDTOList);
                return shiftConfigurationsDTOList;
            });
        }

        //public async Task<int> GetShiftConfigurationsCount(List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        ShiftConfigurationsListBL ShiftConfigurationssListBL = new ShiftConfigurationsListBL(executionContext);
        //        int count = ShiftConfigurationssListBL.GetShiftConfigurationsCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<ShiftConfigurationsDTO>> SaveShiftConfigurations(List<ShiftConfigurationsDTO> shiftConfigurationsDTOList)
        {
            return await Task<List<ShiftConfigurationsDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ShiftConfigurationsListBL shiftConfigurationsList = new ShiftConfigurationsListBL(executionContext, shiftConfigurationsDTOList);
                    List<ShiftConfigurationsDTO> result = shiftConfigurationsList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
