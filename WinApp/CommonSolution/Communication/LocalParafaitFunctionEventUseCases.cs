/********************************************************************************************
 * Project Name - ParafaitFunctionEventUseCases
 * Description  - ParafaitFunctionEventUseCases class 
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
    /// LocalParafaitFunctionEventUseCases
    /// </summary>
    public class LocalParafaitFunctionEventUseCases : IParafaitFunctionEventUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalParafaitFunctionEventUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalParafaitFunctionEventUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetParafaitFunctionEvent
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildren"></param>
        /// <param name="loadActiveChildren"></param>
        /// <returns></returns>
        public async Task<List<ParafaitFunctionEventDTO>> GetParafaitFunctionEvent(List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<ParafaitFunctionEventDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ParafaitFunctionEventListBL parafaitFunctionsListBL = new ParafaitFunctionEventListBL(executionContext);
                List<ParafaitFunctionEventDTO> parafaitFunctionsDTOList = parafaitFunctionsListBL.GetAllParafaitFunctionEventDTOList(searchParameters);
                log.LogMethodExit(parafaitFunctionsDTOList);
                return parafaitFunctionsDTOList;
            });
        }
        ///// <summary>
        ///// SaveParafaitFunctionEvent
        ///// </summary>
        ///// <param name="parafaitFunctionsDTOList"></param>
        ///// <returns></returns>
        //public async Task<string> SaveParafaitFunctionEvent(List<ParafaitFunctionEventDTO> parafaitFunctionsDTOList)
        //{
        //    log.LogMethodEntry();
        //    return await Task<string>.Factory.StartNew(() =>
        //    {
        //        string result = string.Empty;
        //        try
        //        {
        //            if (parafaitFunctionsDTOList == null)
        //            {
        //                throw new ValidationException("ParafaitFunctionEventDTOList is empty");
        //            }

        //            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //            { 
        //                try
        //                {
        //                    parafaitDBTrx.BeginTransaction();
        //                    ParafaitFunctionEventListBL parafaitFunctionsListBL = new ParafaitFunctionEventListBL(executionContext, parafaitFunctionsDTOList);
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
