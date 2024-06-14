/********************************************************************************************
 * Project Name - Redemption 
 * Description  - LocalTicketStationUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class LocalTicketStationUseCases : LocalUseCases, ITicketStationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalTicketStationUseCases(ExecutionContext executionContext) 
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<TicketStationContainerDTOCollection> GetTicketStationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<TicketStationContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    TicketStationContainerList.Rebuild(siteId);
                }
                TicketStationContainerDTOCollection result = TicketStationContainerList.GetTicketStationContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<List<TicketStationDTO>> GetTicketStations(List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> parameters,
                                                                    SqlTransaction sqlTransaction = null)
        {
            return await Task<List<TicketStationDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);

                TicketStationListBL ticketStationListBL = new TicketStationListBL(executionContext);
                int siteId = GetSiteId();
                List<TicketStationDTO> ticketStationDTOList = ticketStationListBL.GetTicketStationDTOList(parameters, sqlTransaction);
                log.LogMethodExit(ticketStationDTOList);
                return ticketStationDTOList;
            });
        }

        public async Task<string> SaveTicketStations(List<TicketStationDTO> ticketStationDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(ticketStationDTOList);
                    if (ticketStationDTOList == null)
                    {
                        throw new ValidationException("TicketStationDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (TicketStationDTO TicketStationDTO in ticketStationDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                TicketStationBL ticketStationBL = new TicketStationBL(executionContext, TicketStationDTO);
                                ticketStationBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw ;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
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

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }

    }
}
