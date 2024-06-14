
/********************************************************************************************
 * Project Name - POS
 * Description  - Business Logic class for POSCashdrawer
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer.Cashdrawers;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// POSCashdrawer BL class
    /// </summary>
    public class POSCashdrawerBL
    {
        private readonly ExecutionContext executionContext;
        private POSCashdrawerDTO posCashdrawerDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor Of POSCashdrawerBL class.
        /// </summary>
        private POSCashdrawerBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        public POSCashdrawerBL(ExecutionContext executionContext, POSCashdrawerDTO posCashdrawerDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(posCashdrawerDTO, executionContext);
            this.posCashdrawerDTO = posCashdrawerDTO;
            log.LogMethodExit();
        }

        public POSCashdrawerBL(ExecutionContext executionContext, int posCashdrawerId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posCashdrawerId, sqlTransaction);
            POSCashdrawerDataHandler posCashdrawerDataHandler = new POSCashdrawerDataHandler(sqlTransaction);
            LoadPOSCashdrawerDTO(posCashdrawerId, sqlTransaction);
            log.LogMethodExit();
        }

        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (posCashdrawerDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSCashdrawer", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadPOSCashdrawerDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            POSCashdrawerDataHandler posCashdrawerDataHandler = new POSCashdrawerDataHandler(sqlTransaction);
            posCashdrawerDTO = posCashdrawerDataHandler.GetPOSCashdrawer(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        public POSCashdrawerBL(ExecutionContext executionContext, POSCashdrawerDTO parameterPOSCashdrawerDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterPOSCashdrawerDTO, sqlTransaction);

            if (parameterPOSCashdrawerDTO.POSCashdrawerId > -1)
            {
                LoadPOSCashdrawerDTO(parameterPOSCashdrawerDTO.POSCashdrawerId, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterPOSCashdrawerDTO.POSCashdrawerId);
                Update(parameterPOSCashdrawerDTO);
            }
            else
            {
                Validate(sqlTransaction);
                posCashdrawerDTO = new POSCashdrawerDTO(-1, parameterPOSCashdrawerDTO.CashdrawerId, parameterPOSCashdrawerDTO.POSMachineId, parameterPOSCashdrawerDTO.IsActive);
            }
            log.LogMethodExit();
        }
        private void Update(POSCashdrawerDTO parameterPOSCashdrawerDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterPOSCashdrawerDTO);
            posCashdrawerDTO.POSCashdrawerId = parameterPOSCashdrawerDTO.POSCashdrawerId;
            posCashdrawerDTO.POSMachineId = parameterPOSCashdrawerDTO.POSMachineId;
            posCashdrawerDTO.IsActive = parameterPOSCashdrawerDTO.IsActive;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CASHDRAWER_INTERFACE_MODE");
            log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
            POSCashdrawerListBL pOSCashdrawerListBL = new POSCashdrawerListBL(executionContext);
            List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>(POSCashdrawerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>(POSCashdrawerDTO.SearchByParameters.POS_MACHINE_ID, posCashdrawerDTO.POSMachineId.ToString()));
            searchParams.Add(new KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>(POSCashdrawerDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<POSCashdrawerDTO> posCashdrawerDTOList = pOSCashdrawerListBL.GetPOSCashdrawerDTOList(searchParams);
            if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
            {
                //Get all cashdrawers for POS machine
                 // Already cashdrawer is assigned and it is active
                if (posCashdrawerDTOList != null && posCashdrawerDTOList.Count == 1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4079)); // Cashdrawer interface mode set as Single. Cannot add new cashdrawer
                }
            }
            if (posCashdrawerDTOList != null && posCashdrawerDTOList.Any())
            {
                if (posCashdrawerDTOList.Exists(x => x.CashdrawerId == posCashdrawerDTO.CashdrawerId && x.POSMachineId == posCashdrawerDTO.POSMachineId) && posCashdrawerDTO.POSCashdrawerId == -1)
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " Cashdrawer"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (posCashdrawerDTOList.Exists(x => x.CashdrawerId == posCashdrawerDTO.CashdrawerId && x.POSMachineId == posCashdrawerDTO.POSMachineId && x.POSCashdrawerId != posCashdrawerDTO.POSCashdrawerId))
                {
                    log.Debug("Duplicate update entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " Cashdrawer"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            log.LogMethodExit();
        }
       
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            POSCashdrawerDataHandler posCashdrawerDataHandler = new POSCashdrawerDataHandler(sqlTransaction);
            if (posCashdrawerDTO.POSCashdrawerId < 0)
            {
                posCashdrawerDTO = posCashdrawerDataHandler.Insert(posCashdrawerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                posCashdrawerDTO.AcceptChanges();
            }
            else if (posCashdrawerDTO.IsChanged)
            {
                posCashdrawerDTO = posCashdrawerDataHandler.Update(posCashdrawerDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                posCashdrawerDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public POSCashdrawerDTO POSCashdrawerDTO
        {
            get
            {
                POSCashdrawerDTO result = new POSCashdrawerDTO(posCashdrawerDTO);
                return result;
            }
        }


    }

    /// <summary>
    /// Manages the list of POSCashdrawerListBL
    /// </summary>
    public class POSCashdrawerListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSCashdrawerDTO> posCashdrawerDTOList = new List<POSCashdrawerDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSCashdrawerListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSCashdrawerListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="posCashdrawerDTOList"></param>
        public POSCashdrawerListBL(ExecutionContext executionContext, List<POSCashdrawerDTO> posCashdrawerDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posCashdrawerDTOList);
            this.posCashdrawerDTOList = posCashdrawerDTOList;
            log.LogMethodExit();
        }

        public List<POSCashdrawerDTO> GetPOSCashdrawerDTOList(List<KeyValuePair<POSCashdrawerDTO.SearchByParameters, string>>
                                                                   searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            POSCashdrawerDataHandler posCashdrawerDataHandler = new POSCashdrawerDataHandler(sqlTransaction);
            List<POSCashdrawerDTO> posCashdrawerDTOList = posCashdrawerDataHandler.GetPOSCashdrawerList(searchParameters);
            log.LogMethodExit(posCashdrawerDTOList);
            return posCashdrawerDTOList;
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posCashdrawerDTOList == null ||
                posCashdrawerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < posCashdrawerDTOList.Count; i++)
            {
                var posCashdrawerDTO = posCashdrawerDTOList[i];
                try
                {
                    POSCashdrawerBL posCashdrawerBL = new POSCashdrawerBL(executionContext, posCashdrawerDTO);
                    posCashdrawerBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PeripheralsDTO List for POSMachineIdList
        /// </summary>
        /// <param name="pOSMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of PeripheralsDTO</returns>
        public List<POSCashdrawerDTO> GetPOSCashdrawerDTOList(List<int> pOSMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineIdList, activeRecords, sqlTransaction);
            POSCashdrawerDataHandler posCashdrawerDataHandler = new POSCashdrawerDataHandler(sqlTransaction);
            List<POSCashdrawerDTO> posCashdrawerDTOList = posCashdrawerDataHandler.GetPOSCashdrawerDTOList(pOSMachineIdList, activeRecords);
            log.LogMethodExit(posCashdrawerDTOList);
            return posCashdrawerDTOList;
        }

    }
}
