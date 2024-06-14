/********************************************************************************************
 * Project Name - Object Translations
 * Description  - A high level structure created to classify the object translations 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        29-Mar-2019   Mushahid Faizan     Removed unused namespaces & Added LogMethodEntry/LogMethodExit method.
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *            29-Jul-2019   Mushahid Faizan     Added Delete in Save() Method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class StateBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private StateDTO stateDTO;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public StateBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            stateDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="stateDTO"></param>
        /// <param name="executionContext"></param>
        public StateBL(ExecutionContext executionContext, StateDTO stateDTO)
        {
            log.LogMethodEntry(executionContext, stateDTO);
            this.stateDTO = stateDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the stateId parameter
        /// </summary>
        /// <param name="stateId">stateId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public StateBL(int stateId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(stateId, sqlTransaction);
            StateDataHandler stateDataHandler = new StateDataHandler(sqlTransaction);
            this.stateDTO = stateDataHandler.GetStateDTO(stateId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the States based on stateId
        /// </summary>
        /// <param name="stateId"></param>        
        public void DeleteState(int stateId)
        {
            log.LogMethodEntry(stateId);
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            try
            {
                parafaitDBTrx.BeginTransaction();
                sqlTransaction = parafaitDBTrx.SQLTrx;
                if (stateDTO.IsChanged)
                {
                    StateDataHandler stateDataHandler = new StateDataHandler(sqlTransaction);
                    stateDataHandler.Delete(stateId);
                }
                parafaitDBTrx.EndTransaction();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                parafaitDBTrx.RollBack();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Saves the StateDTO
        /// Checks if the stateId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            StateDataHandler stateDataHandler = new StateDataHandler(sqlTransaction);
            if (stateDTO.IsActive)
            {
                if (stateDTO.StateId < 0)
                {
                    stateDTO = stateDataHandler.InsertStateDTO(stateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    stateDTO.AcceptChanges();
                }
                else
                {
                    if (stateDTO.IsChanged)
                    {
                        stateDTO = stateDataHandler.UpdateStateDTO(stateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        stateDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (stateDTO.StateId >= 0)
                {
                    stateDataHandler.Delete(stateDTO.StateId);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// get StateDTO Object
        /// </summary>
        public StateDTO GetStateDTO
        {
            get { return stateDTO; }
        }
    }

    /// <summary>
    /// Manages the list of StateDTO
    /// </summary>
    public class StateDTOList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public StateDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the GetStateDTOList list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<StateDTO> GetStateDTOList(List<KeyValuePair<StateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            StateDataHandler stateDataHandler = new StateDataHandler(sqlTransaction);
            List<StateDTO> stateDTOList = stateDataHandler.GetStateDTOList(searchParameters);
            log.LogMethodExit(stateDTOList);
            return stateDTOList;
        }

        /// <summary>
        ///Takes LookupParams as parameter.
        /// </summary>
        /// <param name="stateDTO">stateDTO</param>
        /// <returns>Returns KeyValuePair List of StateDTO.SearchByParameters by converting StateDTO</returns>
        public List<KeyValuePair<StateDTO.SearchByParameters, string>> BuildStateDTOSearchParametersList(StateDTO stateDTO)
        {
            log.LogMethodEntry(stateDTO);
            List<KeyValuePair<StateDTO.SearchByParameters, string>> stateDTOSearchParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
            if (stateDTO != null)
            {
                if (stateDTO.StateId >= 0)
                    stateDTOSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.STATE_ID, stateDTO.StateId.ToString()));

                if (!(string.IsNullOrEmpty(stateDTO.State)))
                    stateDTOSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.STATE, stateDTO.State.ToString()));

                if (!(string.IsNullOrEmpty(stateDTO.Description)))
                    stateDTOSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.STATE_DESCRIPTION, stateDTO.Description.ToString()));

                if (stateDTO.CountryId >= 0)
                stateDTOSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.COUNTRY_ID, stateDTO.CountryId.ToString()));


                if (stateDTO.MasterEntityId >= 0)
                stateDTOSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.MASTER_ENTITY_ID, stateDTO.MasterEntityId.ToString()));

                //if (stateDTO.SiteId > 0)
                stateDTOSearchParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, stateDTO.SiteId.ToString()));

            }
            log.LogMethodExit(stateDTOSearchParams);

            return stateDTOSearchParams;
        }


        /// <summary>
        /// GetStateDTOsList(StateDTO stateDTO) method search based on stateDTO
        /// </summary>
        /// <param name="stateDTO">StateDTO turnstileDTO</param>
        /// <returns>List of StateDTO object</returns>
        public List<StateDTO> GetStateDTOList(StateDTO stateDTO, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(stateDTO, sqlTransaction);
                List<KeyValuePair<StateDTO.SearchByParameters, string>> searchParameters = BuildStateDTOSearchParametersList(stateDTO);
                StateDataHandler stateDataHandler = new StateDataHandler(sqlTransaction);
                List<StateDTO> stateDTOList = stateDataHandler.GetStateDTOList(searchParameters);
                log.LogMethodExit(stateDTOList);
                return stateDTOList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
