/********************************************************************************************
* Project Name - User
* Description  - Use case for Shift Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.140.0     14-Sep-2021     Deeksha               Modified : Provisional Shift changes
*2.140.0     16-Aug-2021     Girish                Modified : Multicash drawer changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class RemoteShiftsUseCases : RemoteUseCases, IShiftUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Shifts_URL = "api/HR/Shifts";
        private const string Shifts_COUNT_URL = "api/HR/ShiftsCount";
        private  string Cashdrawer_Assignment_URL = "api/HR/Shift/{shiftId}/Assign";
        private  string Cashdrawer_UnAssignment_URL = "api/HR/Shift/{shiftId}/UnAssign";

        public RemoteShiftsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ShiftDTO>> GetShift(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>
                          parameters, bool loadChildrecords = false, bool buildReceipt = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildrecords".ToString(), loadChildrecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("buildReceipt".ToString(), buildReceipt.ToString()));
            if (parameters != null)
                if (parameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(parameters));
                }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<ShiftDTO> result = await Get<List<ShiftDTO>>(Shifts_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ShiftDTO.SearchByShiftParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ShiftDTO.SearchByShiftParameters.POS_MACHINE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachine".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_FROM_TIME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftFromTime".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_TO_TIME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftToTime".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftLoginId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftUsername".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_USERTYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftUserType".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_KEY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftKey".ToString(), searchParameter.Value));
                        }
                        break;
                    case ShiftDTO.SearchByShiftParameters.SHIFT_ACTION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("shiftAction".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<ShiftDTO>> SaveShift(List<ShiftDTO> shiftDTODTOList)
        {
            log.LogMethodEntry(shiftDTODTOList);
            try
            {
                List<ShiftDTO> responseString = await Post<List<ShiftDTO>>(Shifts_URL, shiftDTODTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<ShiftDTO> AssignCashdrawer(int shiftId, CashdrawerActivityDTO assignCashdrawerDTO)
        {
            log.LogMethodEntry(shiftId, assignCashdrawerDTO);
            try
            {
                Cashdrawer_Assignment_URL = "api/HR/Shift/{" + shiftId + "}/Assign";
                ShiftDTO responseString = await Post<ShiftDTO>(Cashdrawer_Assignment_URL, assignCashdrawerDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<ShiftDTO> UnAssignCashdrawer(int shiftId, CashdrawerActivityDTO assignCashdrawerDTO)
        {
            log.LogMethodEntry(shiftId, assignCashdrawerDTO);
            try
            {
                Cashdrawer_UnAssignment_URL = "api/HR/Shift/{" + shiftId + "}/UnAssign";
                ShiftDTO responseString = await Post<ShiftDTO>(Cashdrawer_UnAssignment_URL, assignCashdrawerDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetShiftsCount(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(Shifts_COUNT_URL, searchParameterList);
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