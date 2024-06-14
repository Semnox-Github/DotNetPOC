/********************************************************************************************
 * Project Name - User
 * Description  - LocalDepartmentUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    25-Feb-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Implementation of department use-cases
    /// </summary>
    public class LocalDepartmentUseCases : IDepartmentUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalDepartmentUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<DepartmentDTO>> GetDepartments(List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>
                         searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<DepartmentDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                DepartmentList departmentListBL = new DepartmentList(executionContext);
                List<DepartmentDTO> departmentDTOList = departmentListBL.GetDepartmentDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(departmentDTOList);
                return departmentDTOList;
            });
        }
        public async Task<string> SaveDepartments(List<DepartmentDTO> departmentDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(departmentDTOList);
                if (departmentDTOList == null)
                {
                    throw new ValidationException("departmentDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (DepartmentDTO departmentDTO in departmentDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DepartmentBL departmentBL = new DepartmentBL(executionContext, departmentDTO);
                            departmentBL.Save(parafaitDBTrx.SQLTrx);
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
                log.LogMethodExit(result);
                return result;
            });
        }
        //public async Task<DepartmentContainerDTOCollection> GetDepartmentContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        //{
        //    return await Task<DepartmentContainerDTOCollection>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(siteId, hash, rebuildCache);
        //        if (rebuildCache)
        //        {
        //            DepartmentContainerList.Rebuild(siteId);
        //        }
        //        List<DepartmentContainerDTO> currencyRuleContainerList = DepartmentContainerList.GetDepartmentContainerDTOList(siteId);
        //        DepartmentContainerDTOCollection result = new DepartmentContainerDTOCollection(currencyRuleContainerList);
        //        if (hash == result.Hash)
        //        {
        //            log.LogMethodExit(null, "No changes to the cache");
        //            return null;
        //        }
        //        log.LogMethodExit(result);
        //        return result;
        //    });
        //}
        public async Task<string> Delete(List<DepartmentDTO> departmentDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(departmentDTOList);
                    DepartmentList departmentList = new DepartmentList(executionContext, departmentDTOList);
                    departmentList.Delete();
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
