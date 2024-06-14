/********************************************************************************************
 * Project Name - ParafaitFunctionsUseCases
 * Description  - ParafaitFunctionsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 2.110.0      10-Feb-2021      Guru S A              For Subscription changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// LocalParafaitFunctionsUseCases
    /// </summary>
    public class LocalParafaitFunctionsUseCases : IParafaitFunctionsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalParafaitFunctionsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalParafaitFunctionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetParafaitFunctions
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildren"></param>
        /// <param name="loadActiveChildren"></param>
        /// <returns></returns>
        public async Task<List<ParafaitFunctionsDTO>> GetParafaitFunctions(List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> searchParameters, bool loadChildren, bool loadActiveChildren)
        {
            return await Task<List<ParafaitFunctionsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ParafaitFunctionsListBL parafaitFunctionsListBL = new ParafaitFunctionsListBL(executionContext);
                List<ParafaitFunctionsDTO> parafaitFunctionsDTOList = parafaitFunctionsListBL.GetAllParafaitFunctionsDTOList(searchParameters, loadChildren, loadActiveChildren);
                log.LogMethodExit(parafaitFunctionsDTOList);
                return parafaitFunctionsDTOList;
            });
        }
        ///// <summary>
        ///// SaveParafaitFunctions
        ///// </summary>
        ///// <param name="parafaitFunctionsDTOList"></param>
        ///// <returns></returns>
        //public async Task<string> SaveParafaitFunctions(List<ParafaitFunctionsDTO> parafaitFunctionsDTOList)
        //{
        //    log.LogMethodEntry();
        //    return await Task<string>.Factory.StartNew(() =>
        //    {
        //        string result = string.Empty;
        //        try
        //        {
        //            if (parafaitFunctionsDTOList == null)
        //            {
        //                throw new ValidationException("ParafaitFunctionsDTOList is empty");
        //            }

        //            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //            { 
        //                try
        //                {
        //                    parafaitDBTrx.BeginTransaction();
        //                    ParafaitFunctionsListBL parafaitFunctionsListBL = new ParafaitFunctionsListBL(executionContext, parafaitFunctionsDTOList);
        //                    parafaitFunctionsListBL.Save(parafaitDBTrx.SQLTrx);
        //                    parafaitDBTrx.EndTransaction();
        //                }
        //                catch (ValidationException valEx)
        //                {
        //                    parafaitDBTrx.RollBack();
        //                    log.Error(valEx);
        //                    throw valEx;
        //                }
        //                catch (Exception ex)
        //                {
        //                    parafaitDBTrx.RollBack();
        //                    log.Error(ex);
        //                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //                    throw new Exception(ex.Message, ex);
        //                }
        //            }
        //            result = "Success";
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex);
        //            throw ex;
        //        }
        //        log.LogMethodExit(result);
        //        return result;
        //    });
        //}
         
    }
}
