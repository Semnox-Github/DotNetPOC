/********************************************************************************************
* Project Name - User
* Description  - LocalFacilityPOSAssignmentUseCases  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
   public class LocalFacilityPOSAssignmentUseCases:IFacilityPOSAssignmentUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalFacilityPOSAssignmentUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<FacilityPOSAssignmentDTO>> GetFacilityPOSAssignments(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>>
                         searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<FacilityPOSAssignmentDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                FacilityPOSAssignmentList facilityPOSAssignmentBL = new FacilityPOSAssignmentList(executionContext);
                List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList = facilityPOSAssignmentBL.GetFacilityPOSAssignmentDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(facilityPOSAssignmentDTOList);
                return facilityPOSAssignmentDTOList;
            });
        }
        public async Task<string> SaveFacilityPOSAssignments(List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(facilityPOSAssignmentDTOList);
                if (facilityPOSAssignmentDTOList == null)
                {
                    throw new ValidationException("facilityPOSAssignmentDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (FacilityPOSAssignmentDTO facilityPOSAssignmentDTO in facilityPOSAssignmentDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            FacilityPOSAssignmentBL facilityPOSAssignmentBL = new FacilityPOSAssignmentBL(executionContext, facilityPOSAssignmentDTO);
                            facilityPOSAssignmentBL.Save(parafaitDBTrx.SQLTrx);
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
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> Delete(List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(facilityPOSAssignmentDTOList);
                    FacilityPOSAssignmentList facilityPOSAssignmentList = new FacilityPOSAssignmentList(executionContext, facilityPOSAssignmentDTOList);
                    facilityPOSAssignmentList.DeleteFacilityPOSAssignmentDTOList();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
