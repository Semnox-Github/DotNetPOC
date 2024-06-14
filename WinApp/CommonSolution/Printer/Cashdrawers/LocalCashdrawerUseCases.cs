/********************************************************************************************
 * Project Name - Device
 * Description  - LocalCashdrawerUseCases
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    /// <summary>
    /// LocalCashdrawerUseCases
    /// </summary>
    public class LocalCashdrawerUseCases : ICashdrawerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalCashdrawerUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalCashdrawerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetCashdrawers
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<CashdrawerDTO>> GetCashdrawers(List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null  )
        {
            return await Task<List<CashdrawerDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                CashdrawerListBL cashdrawerListBL = new CashdrawerListBL(executionContext);
                List<CashdrawerDTO> cashdrawerDTOList = cashdrawerListBL.GetCashdrawers(searchParameters, sqlTransaction);
                log.LogMethodExit(cashdrawerDTOList);
                return cashdrawerDTOList;
            });
        }

        /// <summary>
        /// SaveCashdrawers
        /// </summary>
        /// <param name="cashdrawerDTOList"></param>
        /// <returns></returns>
        public async Task<List<CashdrawerDTO>> SaveCashdrawers(List<CashdrawerDTO> cashdrawerDTOList)
        {
            return await Task<List<CashdrawerDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    CashdrawerListBL cashdrawerListBL = new CashdrawerListBL(executionContext, cashdrawerDTOList);
                    List<CashdrawerDTO> result = cashdrawerListBL.Save(transaction.SQLTrx);
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<CashdrawerContainerDTOCollection> GetCashdrawerContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<CashdrawerContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    CashdrawerContainerList.Rebuild(siteId);
                }
                CashdrawerContainerDTOCollection result = CashdrawerContainerList.GetCashdrawerContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
