/********************************************************************************************
 * Project Name - LocalParafaitMessageQueueUseCases
 * Description  - LocalParafaitMessageQueueUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.120.0      8-Mar-2021       Prajwal S                  Created : urban Pipers changes
*2.130.0     08-Feb-2022       Fiona Lishal               Added GetParafaitMessageQueueDTOList
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.GenericUtilities
{
    public class LocalParafaitMessageQueueUseCases : IParafaitMessageQueueUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalParafaitMessageQueueUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ParafaitMessageQueueDTO>> GetParafaitMessageQueue(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null )
        {
            return await Task<List<ParafaitMessageQueueDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ParafaitMessageQueueListBL parafaitMessageQueueListBL = new ParafaitMessageQueueListBL(executionContext);
                List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueListBL.GetParafaitMessageQueues(searchParameters, sqlTransaction);

                log.LogMethodExit(parafaitMessageQueueDTOList);
                return parafaitMessageQueueDTOList;
            });
        }

        public async Task<List<ParafaitMessageQueueDTO>> SaveParafaitMessageQueue(List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList)
        {
            return await Task<List<ParafaitMessageQueueDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ParafaitMessageQueueListBL parafaitMessageQueueList = new ParafaitMessageQueueListBL(executionContext, parafaitMessageQueueDTOList);
                    List<ParafaitMessageQueueDTO> result = parafaitMessageQueueList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
        public async Task<List<ParafaitMessageQueueDTO>> GetParafaitMessageQueueDTOList(List<string> entityGuids, List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<ParafaitMessageQueueDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ParafaitMessageQueueListBL parafaitMessageQueueListBL = new ParafaitMessageQueueListBL(executionContext);
                    List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueListBL.GetParafaitMessageQueues(entityGuids, searchParameters, transaction.SQLTrx);
                    transaction.EndTransaction();
                    return parafaitMessageQueueDTOList;
                }
            });
        }
    }
}
