/********************************************************************************************
 * Project Name - Product
 * Description  - LocalFacilityUseCases class 
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
    // <summary>
    /// Implementation of facility use-cases
    /// </summary>
    public class LocalFacilityUseCases:IFacilityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalFacilityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<FacilityDTO>> GetFacilitys(List<KeyValuePair<FacilityDTO.SearchByParameters, string>>
                         searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<FacilityDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters,loadChildRecords,activeChildRecords,sqlTransaction);

                FacilityList facilityListBLList = new FacilityList(executionContext);
                List<FacilityDTO> facilityDTOList = facilityListBLList.GetFacilityDTOList(searchParameters,loadChildRecords, activeChildRecords,
                                             sqlTransaction );

                log.LogMethodExit(facilityDTOList);
                return facilityDTOList;
            });
        }
        public async Task<string> SaveFacilitys(List<FacilityDTO> facilityDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(facilityDTOList);
                if (facilityDTOList == null)
                {
                    throw new ValidationException("facilityDTOList is Empty");
                }
                FacilityList facility = new FacilityList(executionContext, facilityDTOList);
                facility.SaveUpdateFacilityList();
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<string> Delete(List<FacilityDTO> facilityDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(facilityDTOList);
                    FacilityList facilityList = new FacilityList(executionContext, facilityDTOList);
                    facilityList.Delete();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
