/********************************************************************************************
 * Project Name - RemoteParafaitFunctionsUseCases
 * Description  - RemoteParafaitFunctionsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.110.0      14-Dec-2020       Deeksha            Created :Inventory UI/POS UI re-design with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// RemoteParafaitFunctionsUseCases
    /// </summary>
    public class RemoteParafaitFunctionsUseCases : RemoteUseCases, IParafaitFunctionsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PARAFAIT_FUNCTION_URL = "api/Communication/ParafaitFunctionss";
        /// <summary>
        /// RemoteParafaitFunctionsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteParafaitFunctionsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetParafaitFunctions
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<ParafaitFunctionsDTO>> GetParafaitFunctions(List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> parameters, bool loadChildren, bool loadActiveChildren)
        {
            log.LogMethodEntry(parameters, loadChildren, loadActiveChildren);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters, loadChildren, loadActiveChildren));
            }
            try
            {
                List<ParafaitFunctionsDTO> result = await Get<List<ParafaitFunctionsDTO>>(PARAFAIT_FUNCTION_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }         

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string>> productsSearchParams, bool loadChildren, bool loadActiveChildren)
        {
            log.LogMethodEntry(productsSearchParams, loadChildren, loadActiveChildren);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ParafaitFunctionsDTO.SearchByParameters, string> searchParameter in productsSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case ParafaitFunctionsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break; 
                    case ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitFunctionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitFunctionsDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitFunctionName".ToString(), searchParameter.Value));
                        }
                        break; 
                }
            }
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildren".ToString(), loadChildren.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildren".ToString(), loadActiveChildren.ToString()));
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        ///// <summary>
        ///// SaveParafaitFunctions
        ///// </summary>
        ///// <param name="parafaitFunctionsDTOList"></param>
        ///// <returns></returns>
        //public async Task<string> SaveParafaitFunctions(List<ParafaitFunctionsDTO> parafaitFunctionsDTOList)
        //{
        //    log.LogMethodEntry(parafaitFunctionsDTOList);
        //    try
        //    {
        //        string result = await Post<string>(PARAFAIT_FUNCTION_URL, parafaitFunctionsDTOList);
        //        log.LogMethodExit(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw ex;
        //    }
        //}
         
    }
}
