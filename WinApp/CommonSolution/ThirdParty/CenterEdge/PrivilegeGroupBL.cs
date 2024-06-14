/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - PrivilegeGroupBL class - This is business layer class for PrivilegeGroup
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class Gets the products under the Privilege display group
    /// </summary>
    public class PrivilegeGroupBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of PrivilegeGroupBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public PrivilegeGroupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// This method Gets the PrivilegeGroups response object
        /// </summary>
        /// <returns></returns>
        public PrivilegeGroups GetPrivilegeGroups(int skip ,int take)
        {
            log.LogMethodEntry(skip, take);
            string displayGroupName = string.Empty;
            LookupsList lookupsList = new LookupsList(executionContext);
            List<KeyValuePair<LookupsDTO.SearchByParameters, string>> parameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
            parameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            parameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "CENTEREDGE_DISPLAY_GROUPS"));
            List<LookupsDTO> lookups = lookupsList.GetAllLookups(parameters, true);
            log.Debug("lookups Count : " + lookups.Count);
            if (lookups != null && lookups.Any())
            {
                if (lookups[0].LookupValuesDTOList != null && lookups[0].LookupValuesDTOList.Any())
                {
                    LookupValuesDTO lookupValuesDTO = lookups[0].LookupValuesDTOList.Where(x => x.LookupValue == "PRIVILEGE_GROUP").FirstOrDefault();
                    if (lookupValuesDTO != null)
                    {
                        displayGroupName = lookupValuesDTO.Description;
                    }
                    else
                    {
                        log.Debug("look up value is not created : PRIVILEGE_GROUP");
                        throw new ValidationException("look up value is not created : PRIVILEGE_GROUP");
                    }
                }
            }
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, displayGroupName));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);
            PrivilegeGroups cePrivilegeGroupResponse = new PrivilegeGroups();
            if (productsDTOList != null && productsDTOList.Any())
            {
                foreach (ProductsDTO productsDTO in productsDTOList)
                {
                    PrivilegeGroupDTO privilegeGroupDTO = new PrivilegeGroupDTO(productsDTO.ProductId, productsDTO.ProductName);
                    cePrivilegeGroupResponse.privilegeGroups.Add(privilegeGroupDTO);
                }
                cePrivilegeGroupResponse.skipped = skip;
                if (take > 0)
                {
                    take = take > cePrivilegeGroupResponse.privilegeGroups.Count ? cePrivilegeGroupResponse.privilegeGroups.Count : take;
                }
                else
                {
                    take = cePrivilegeGroupResponse.privilegeGroups.Count;
                }
                if( take > cePrivilegeGroupResponse.privilegeGroups.Count - skip)
                {
                    take = cePrivilegeGroupResponse.privilegeGroups.Count - skip;
                }
                List<PrivilegeGroupDTO> result = cePrivilegeGroupResponse.privilegeGroups.GetRange(skip, take);
                cePrivilegeGroupResponse.privilegeGroups.Clear();
                cePrivilegeGroupResponse.privilegeGroups.AddRange(result);
                cePrivilegeGroupResponse.totalCount = cePrivilegeGroupResponse.privilegeGroups.Count;
            }
            log.LogMethodExit(cePrivilegeGroupResponse);
            return cePrivilegeGroupResponse;
        }
    }
}
