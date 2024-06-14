/********************************************************************************************
 * Project Name - User
 * Description  - RemoteWorkShiftUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.120.0       31-Mar-2021       Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class RemoteWorkShiftUseCases : RemoteUseCases, IWorkShiftUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string WorkShift_URL = "api/HR/WorksShifts";

        public RemoteWorkShiftUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<WorkShiftDTO>> GetWorkShift(List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>>
                          parameters,bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<WorkShiftDTO> result = await Get<List<WorkShiftDTO>>(WorkShift_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case WorkShiftDTO.SearchByWorkShiftParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case WorkShiftDTO.SearchByWorkShiftParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("name".ToString(), searchParameter.Value));
                        }
                        break;
                    case WorkShiftDTO.SearchByWorkShiftParameters.FREQUENCY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("frequency".ToString(), searchParameter.Value));
                        }
                        break;
                    case WorkShiftDTO.SearchByWorkShiftParameters.STARTDATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("startdate".ToString(), searchParameter.Value));
                        }
                        break;
                    case WorkShiftDTO.SearchByWorkShiftParameters.ENDDATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("endDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case WorkShiftDTO.SearchByWorkShiftParameters.WORK_SHIFT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("workShitId".ToString(), searchParameter.Value));
                        }
                        break;
                    case WorkShiftDTO.SearchByWorkShiftParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<WorkShiftDTO>> SaveWorkShift(List<WorkShiftDTO> workShiftDTOList)
        {
            log.LogMethodEntry(workShiftDTOList);
            try
            {
                List<WorkShiftDTO> responseString = await Post<List<WorkShiftDTO>>(WorkShift_URL, workShiftDTOList);
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