/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalPOSMachineDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 0.1         10-Nov-2020       Vikas Dwivedi             Modified as per new Standards
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.POS
{
    public class LocalPOSMachineUseCases : LocalUseCases, IPOSMachineUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalPOSMachineUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<POSMachineContainerDTOCollection> GetPOSMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<POSMachineContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    POSMachineContainerList.Rebuild(siteId);
                }
                POSMachineContainerDTOCollection result = POSMachineContainerList.GetPOSMachineContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<List<POSMachineDTO>> GetPOSMachines(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<POSMachineDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                POSMachineList pOSMachineList = new POSMachineList(executionContext);
                List<POSMachineDTO> pOSMachineDTOList = pOSMachineList.GetAllPOSMachines(parameters, loadChildRecords, activeChildRecords, sqlTransaction);
                log.LogMethodExit(pOSMachineDTOList);
                return pOSMachineDTOList;
            });
        }

        //public async Task<List<ShiftDTO>> GetAllOpenShifts(int posMachineId)
        //{
        //    return await Task<List<ShiftDTO>>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(posMachineId);
        //        POSMachines pOSMachines = new POSMachines(executionContext, posMachineId);
        //        List <ShiftDTO> shiftDTOList = pOSMachines.GetAllOpenShifts();
        //        log.LogMethodExit(shiftDTOList);
        //        return shiftDTOList;
        //    });
        //}

        public async Task<string> SavePOSMachines(List<POSMachineDTO> posMachineDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(posMachineDTOList);
                    if (posMachineDTOList == null)
                    {
                        throw new ValidationException("posMachineDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (POSMachineDTO posMachineDTO in posMachineDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                POSMachines pOSMachines = new POSMachines(executionContext, posMachineDTO);
                                pOSMachines.Save(parafaitDBTrx.SQLTrx);
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

        
    }
}
