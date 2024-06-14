/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - TimePlayGroupBL class - This is business layer class for TimePlayGroup
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      24-Sep-2020      Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class Gets the products under the TimePlayGroup display group
    /// </summary>
    public class TimePlayGroupBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of TimePlayGroupBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public TimePlayGroupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method Gets the TimePlayGroups response object
        /// </summary>
        /// <returns></returns>
        public TimePlayGroups GetTimePlayGroups(int skip, int take)   
        {
            log.LogMethodEntry( skip, take);
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
                    LookupValuesDTO lookupValuesDTO = lookups[0].LookupValuesDTOList.Where(x => x.LookupValue == "TIME_PLAY_GROUP").FirstOrDefault();
                    if (lookupValuesDTO != null)
                    {
                        displayGroupName = lookupValuesDTO.Description;
                    }
                    else
                    {
                        log.Debug("look up value is not created : TIME_PLAY_GROUP");
                        throw new ValidationException("look up value is not created : TIME_PLAY_GROUP");
                    }
                }
            }
            ProductsList productsList = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, displayGroupName));
            searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));
            List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParameters);
            TimePlayGroups ceTimePlayGroupResponse = new TimePlayGroups();
            if (productsDTOList != null && productsDTOList.Any())
            {
                foreach (ProductsDTO productsDTO in productsDTOList)
                {
                    TimePlayGroupDTO timePlayGroupDTO = new TimePlayGroupDTO(productsDTO.ProductId, productsDTO.ProductName);
                    ceTimePlayGroupResponse.timePlayGroups.Add(timePlayGroupDTO);
                }

                ceTimePlayGroupResponse.skipped = skip;
                if (take > 0)
                {
                    take = take > ceTimePlayGroupResponse.timePlayGroups.Count ? ceTimePlayGroupResponse.timePlayGroups.Count : take;
                }
                else
                {
                    take = ceTimePlayGroupResponse.timePlayGroups.Count;
                }
                if (take > ceTimePlayGroupResponse.timePlayGroups.Count - skip)
                {
                    take = ceTimePlayGroupResponse.timePlayGroups.Count - skip;
                }
                List<TimePlayGroupDTO> result = ceTimePlayGroupResponse.timePlayGroups.GetRange(skip, take);
                ceTimePlayGroupResponse.timePlayGroups.Clear();
                ceTimePlayGroupResponse.timePlayGroups.AddRange(result);
                ceTimePlayGroupResponse.totalCount = ceTimePlayGroupResponse.timePlayGroups.Count;
                log.LogMethodExit(ceTimePlayGroupResponse);
            }
            log.LogMethodExit(ceTimePlayGroupResponse);
            return ceTimePlayGroupResponse;
        }
 
    }
}
