/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of StateValueConverter      s
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    class StateValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Dictionary<int, StateDTO> stateIdStateDTODictionary;
        private Dictionary<string, StateDTO> stateCodeStateDTODictionary;
        public StateValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            stateCodeStateDTODictionary = new Dictionary<string, StateDTO>();
            stateIdStateDTODictionary = new Dictionary<int, StateDTO>();
            List<StateDTO> stateList = null;
            StateDTOList stateDTOList = new StateDTOList(executionContext);
            List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
            searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            stateList = stateDTOList.GetStateDTOList(searchStateParams);
            if(stateList != null && stateList.Count > 0)
            {
                foreach (var state in stateList)
                {
                     
                    stateIdStateDTODictionary.Add(state.StateId, state);
                    if (stateCodeStateDTODictionary.ContainsKey(state.State.ToUpper()) == false)
                    {
                        stateCodeStateDTODictionary.Add(state.State.ToUpper(), state);
                    }
                        
                }
            }
            log.LogMethodExit();

        }
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int stateId = -1;
            if(stateCodeStateDTODictionary != null && stateCodeStateDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                stateId = stateCodeStateDTODictionary[stringValue.ToUpper()].StateId;
            }
            log.LogMethodExit(stateId);
            return stateId;
        }

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string stateCode = string.Empty;
            if(stateIdStateDTODictionary != null && stateIdStateDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                stateCode = stateIdStateDTODictionary[Convert.ToInt32(value)].State;
            }
            log.LogMethodExit(stateCode);
            return stateCode;
        }
    }
}
