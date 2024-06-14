/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteProductUserGroupsUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00      17-Nov-2020        Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteProductUserGroupsUseCases : RemoteUseCases, IProductUserGroupsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCT_USER_GROUPS_URL = "api/Products/ProductUserGroups";      

        public RemoteProductUserGroupsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductUserGroupsDTO>> GetProductUserGroups(List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            List<ProductUserGroupsDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                result = await Get<List<ProductUserGroupsDTO>>(PRODUCT_USER_GROUPS_URL, searchParameterList);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productUserGroupsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productUserGroupsName".ToString(), searchParameter.Value));
                        }
                        break;                  
                    case ProductUserGroupsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;                  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveProductUserGroups(List<ProductUserGroupsDTO> productUserGroupsDTOList)
        {
            log.LogMethodEntry(productUserGroupsDTOList);
            try
            {
                string responseString = await Post<string>(PRODUCT_USER_GROUPS_URL, productUserGroupsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}
