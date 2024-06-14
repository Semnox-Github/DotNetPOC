/********************************************************************************************
 * Project Name - Product
 * Description  - LocalMasterScheduleUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    10-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    // <summary>
    /// Implementation of masterSchedule use-cases
    /// </summary>
    public class LocalMasterScheduleUseCases:IMasterScheduleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalMasterScheduleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MasterScheduleDTO>> GetMasterSchedules(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters,
               bool loadChildActiveRecords = false, bool loadChildRecord = false, int facilityMapId = -1, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<MasterScheduleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters,loadChildActiveRecords,loadChildRecord,facilityMapId,sqlTransaction);

                MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext);
                List<MasterScheduleDTO> masterScheduleDTOList = masterScheduleList.GetMasterScheduleDTOsList(searchParameters,loadChildActiveRecords,loadChildRecord,facilityMapId ,sqlTransaction);

                log.LogMethodExit(masterScheduleDTOList);
                return masterScheduleDTOList;
            });
        }
        public async Task<string> SaveMasterSchedules(List<MasterScheduleDTO> masterScheduleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(masterScheduleDTOList);
                    if (masterScheduleDTOList == null)
                    {
                        throw new ValidationException("masterScheduleDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext, masterScheduleDTOList);
                            masterScheduleList.SaveAttractionMasterScheduleList();
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
       
            public async Task<string> Delete(List<MasterScheduleDTO> masterScheduleDTOList)
            {
                return await Task<string>.Factory.StartNew(() =>
                {
                    string result = string.Empty;
                    try
                    {
                        log.LogMethodEntry(masterScheduleDTOList);
                        MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext, masterScheduleDTOList);
                        masterScheduleList.DeleteAttractionMasterScheduleList();
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
