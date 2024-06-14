/********************************************************************************************
 * Project Name - MachineGroups
 * Description  - LocalMachineGroupsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 2.110.0      18-Dec-2020      Prajwal S            Created : POS UI Redesign with REST API
 2.110.0      05-Feb-2021      Fiona                Modified for pagination and for geting MachineGroups Count
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    class LocalMachineGroupsUseCases : IMachineGroupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMachineGroupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        

        public async Task<List<MachineGroupsDTO>> GetMachineGroups(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, bool loadActiveChild = false, bool buildChildRecords = false,int currentPage=0,int pageSize=0, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<MachineGroupsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MachineGroupsList machineGroupsListBL = new MachineGroupsList(executionContext);
                List<MachineGroupsDTO> machineGroupsDTOList = machineGroupsListBL.GetAllMachineGroupsDTOList(searchParameters, buildChildRecords, loadActiveChild, sqlTransaction, currentPage, pageSize);
                log.LogMethodExit(machineGroupsDTOList);
                return machineGroupsDTOList;


            });
        }

        public async Task<int> GetMachineGroupsCount(List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {

            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                MachineGroupsList machineGroupsListBL = new MachineGroupsList(executionContext);
                int count = machineGroupsListBL.GetMachineGroupsCount(searchParameters, sqlTransaction);
                log.LogMethodExit(count);
                return count;
            });
        }

        

        public async Task<string> SaveMachineGroups(List<MachineGroupsDTO> machineGroupsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(machineGroupsDTOList);
                    if (machineGroupsDTOList == null)
                    {
                        throw new ValidationException("MachineGroupsDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {

                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MachineGroupsList machineGroupsList = new MachineGroupsList(executionContext, machineGroupsDTOList);
                            machineGroupsList.Save(parafaitDBTrx.SQLTrx);
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
                            throw new Exception(ex.Message, ex);
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


