/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteSegmentDefinitionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class RemoteSegmentDefinitionUseCases : RemoteUseCases, ISegmentDefinitionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SEGMENT_URL = "api/Inventory/Segments";
        private const string SEGMENT_COUNT_URL = "api/Inventory/SegmentCounts";

        public RemoteSegmentDefinitionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<SegmentDefinitionDTO>> GetSegmentDefinitions(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>
                          parameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<SegmentDefinitionDTO> result = await Get<List<SegmentDefinitionDTO>>(SEGMENT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetSegmentCount(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>
                          parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(SEGMENT_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("segmentName".ToString(), searchParameter.Value));
                        }
                        break;
                    case SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEGMENT_DEFINITION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("segmentDefinitionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("applicableEntity".ToString(), searchParameter.Value));
                        }
                        break;
                    case SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SEQUENCE_ORDER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("sequenceOrder".ToString(), searchParameter.Value));
                        }
                        break;
                    case SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_MANDATORY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isMandatory".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveSegmentDefinitions(List<SegmentDefinitionDTO> segmentDefinitionDTOList)
        {
            log.LogMethodEntry(segmentDefinitionDTOList);
            try
            {
                string result = await Post<string>(SEGMENT_URL, segmentDefinitionDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}
