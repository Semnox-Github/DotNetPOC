/********************************************************************************************
* Project Name - Communication
* Description  - LocalMessagingTriggerUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    05-May-2021       Roshan Devadiga            Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    public class LocalMessagingTriggerUseCases:IMessagingTriggerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMessagingTriggerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MessagingTriggerDTO>> GetMessagingTrigges(List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> searchParameters,
                                           bool loadChildRecords = false, bool activeChildRecords = true,
                                           SqlTransaction sqlTransaction = null)
        {
            return await Task<List<MessagingTriggerDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                MessagingTriggerListBL messagingTriggerList = new MessagingTriggerListBL(executionContext);
                List<MessagingTriggerDTO> messagingTriggerDTOList = messagingTriggerList.GetAllMessagingTriggerList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(messagingTriggerDTOList);
                return messagingTriggerDTOList;
            });
        }
        public async Task<string> SaveMessagingTrigges(List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(messagingTriggerDTOList);
                    if (messagingTriggerDTOList == null)
                    {
                        throw new ValidationException("messagingTriggerDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MessagingTriggerListBL messagingTriggerList = new MessagingTriggerListBL(executionContext, messagingTriggerDTOList);
                            messagingTriggerList.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> Delete(List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(messagingTriggerDTOList);
                    MessagingTriggerListBL messagingTriggerList = new MessagingTriggerListBL(executionContext, messagingTriggerDTOList);
                    messagingTriggerList.Delete();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
