/********************************************************************************************
 * Project Name - Product
 * Description  - FacilityMapUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    10-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public class RemoteFacilityMapUseCases:RemoteUseCases,IFacilityMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string FACILITYMAP_URL = "api/Product/FacilityMaps";

        public RemoteFacilityMapUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<FacilityMapDTO>> GetFacilityMaps(List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters,
                                                        bool loadChildRecords = false, bool activeChildRecords = true,
                                                        bool loadChildForOnlyProductType = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters,loadChildRecords, activeChildRecords, loadChildForOnlyProductType, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildForOnlyProductType".ToString(), loadChildForOnlyProductType.ToString()));

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<FacilityMapDTO> result = await Get<List<FacilityMapDTO>>(FACILITYMAP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<FacilityMapDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case FacilityMapDTO.SearchByParameters.FACILITY_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityMapId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.FACILITY_MAP_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityMapIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.CANCELLATION_PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cancellationProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("havingProductTypesIn".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.FACILITY_MAP_IDS_IN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityMapIdsIn".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("allowedProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_IDS_IN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("allowedProductIdsIn".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.FACILITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityId".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.FACILITY_IDS_IN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityIdsIn".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("allowedProductsHasExternalSystemReference".ToString(), searchParameter.Value));
                        }
                        break;
                    case FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCTS_IS_COMBO_CHILD:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("allowedProductsIsComboChild".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveFacilityMaps(List<FacilityMapDTO> facilityMapDTOList)
        {
            log.LogMethodEntry(facilityMapDTOList);
            try
            {
                string responseString = await Post<string>(FACILITYMAP_URL, facilityMapDTOList);
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
