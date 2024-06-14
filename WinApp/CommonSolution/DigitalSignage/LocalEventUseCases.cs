/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - LocalEventUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    21-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Implementation of Event use-cases
    /// </summary>
    public class LocalEventUseCases:IEventUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalEventUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<EventDTO>> GetEvents(List<KeyValuePair<EventDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<EventDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                EventListBL eventListBL = new EventListBL(executionContext);
                List<EventDTO> eventDTODTOList = eventListBL.GetEventDTOList(searchParameters, sqlTransaction);
                log.LogMethodExit(eventDTODTOList);
                return eventDTODTOList;
            });
        }
        public async Task<string> SaveEvents(List<EventDTO> eventDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(eventDTOList);
                    if (eventDTOList == null)
                    {
                        throw new ValidationException("eventDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            EventListBL eventList = new EventListBL(executionContext, eventDTOList);
                            eventList.Save();
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
    }
}
