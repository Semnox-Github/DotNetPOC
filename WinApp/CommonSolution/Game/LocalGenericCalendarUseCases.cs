/********************************************************************************************
 * Project Name - Game
 * Description  - LocalGenericCalendarUseCases class.
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 2.110.0      15-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class LocalGenericCalendarUseCases : IGenericCalendarUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGenericCalendarUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<int> GetGenericCalendarCount(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GenericCalendarList genericCalendarListBL = new GenericCalendarList(executionContext);
                int count = 1;//genericCalendarListBL.getGenericCalenderCount(searchParameters, sqlTransaction);
                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<List<GenericCalendarDTO>> GetGenericCalendars(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>>
                          searchParameters, string genericColId = "", string moduleName = "", int entityId = 0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<GenericCalendarDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                GenericCalendarList genericCalendarListBL = new GenericCalendarList(executionContext);
                List<GenericCalendarDTO> genericCalendarDTOList = genericCalendarListBL.GetGenericCalendarDTOList(searchParameters, genericColId, moduleName, entityId, sqlTransaction);
                log.LogMethodExit(genericCalendarDTOList);
                return genericCalendarDTOList;
            });
        }

        public async Task<string> SaveGenericCalendars(List<GenericCalendarDTO> genericCalendarDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(genericCalendarDTOList);
                    if (genericCalendarDTOList == null)
                    {
                        throw new ValidationException("GenericCalendarDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (GenericCalendarDTO genericCalendarDTO in genericCalendarDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                GenericCalendar genericCalendar = new GenericCalendar(executionContext, genericCalendarDTO);
                                genericCalendar.SaveCalendarThemes(parafaitDBTrx.SQLTrx);
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



    

