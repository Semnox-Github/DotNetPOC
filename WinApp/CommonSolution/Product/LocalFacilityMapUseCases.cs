/********************************************************************************************
 * Project Name - Product
 * Description  - LocalFacilityMapUseCases class 
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
    /// Implementation of facilityMap use-cases
    /// </summary>
    public class LocalFacilityMapUseCases:IFacilityMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalFacilityMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<FacilityMapDTO>> GetFacilityMaps(List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters,
                                                        bool loadChildRecords = false, bool activeChildRecords = true,
                                                        bool loadChildForOnlyProductType = false, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<FacilityMapDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters,loadChildRecords,activeChildRecords,loadChildForOnlyProductType, sqlTransaction);

                FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParameters, loadChildRecords, activeChildRecords, loadChildForOnlyProductType, sqlTransaction);

                log.LogMethodExit(facilityMapDTOList);
                return facilityMapDTOList;
            });
        }

        public async Task<string> SaveFacilityMaps(List<FacilityMapDTO> facilityMapDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(facilityMapDTOList);
                    if (facilityMapDTOList == null)
                    {
                        throw new ValidationException("facilityMapDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext, facilityMapDTOList);
                            facilityMapListBL.Save();
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
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
       
    }
}
