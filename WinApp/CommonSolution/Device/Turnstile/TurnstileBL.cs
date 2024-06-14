/********************************************************************************************
 * Project Name - Turnstile BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       09-Jul-2019       Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                            LogMethodEntry() and LogMethodExit(). 
 *2.90         09-Jun-2020       Mushahid Faizan     Modified : 3 Tier changes for Rest API.                                                            
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TurnstileBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private TurnstileDTO turnstileDTO;


        /// <summary>
        /// Default constructor
        /// </summary>
        private TurnstileBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the turnstileId parameter
        /// </summary>
        /// <param name="turnstileId">turnstileId</param>
        public TurnstileBL(ExecutionContext executionContext, int turnstileId, SqlTransaction sqlTransaction = null) : this(executionContext)
        {
            log.LogMethodEntry(turnstileId);
            TurnstileDataHandler turnstileDataHandler = new TurnstileDataHandler(sqlTransaction);
            this.turnstileDTO = turnstileDataHandler.GetTurnstileDTO(turnstileId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the TurnstileDTO parameter
        /// </summary>
        /// <param name="turnstileDTO">TurnstileDTO</param>
        public TurnstileBL(ExecutionContext executionContext, TurnstileDTO turnstileDTO) : this(executionContext)
        {
            log.LogMethodEntry(turnstileDTO);
            this.turnstileDTO = turnstileDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get TurnstileDTO Object
        /// </summary>
        public TurnstileDTO GetTurnstileDTO
        {
            get { return turnstileDTO; }
        }

        /// <summary>
        /// Saves the TurnstileDTO
        /// Checks if the turnstileId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TurnstileDataHandler turnstileDataHandler = new TurnstileDataHandler(sqlTransaction);
            if (turnstileDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (turnstileDTO.TurnstileId < 0)
            {
                turnstileDTO = turnstileDataHandler.InsertTurnstile(turnstileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                turnstileDTO.AcceptChanges();
            }
            else if (turnstileDTO.IsChanged)
            {
                turnstileDTO = turnstileDataHandler.UpdateTurnstile(turnstileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                turnstileDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (turnstileDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }

    /// <summary>
    /// Manages the list of TurnstileDTO
    /// </summary>
    public class TurnstilesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<TurnstileDTO> turnstileDTOList = new List<TurnstileDTO>();
        List<TurnstileUI.TurnstileObjectClass> lstTurnstiles = new List<TurnstileUI.TurnstileObjectClass>();

        private TurnstileClass CurTurnstileDevice;
        private TurnstileUI.TurnstileObjectClass selectedTurnstile;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public TurnstilesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="turnstileDTOList"></param>
        public TurnstilesList(ExecutionContext executionContext, List<TurnstileDTO> turnstileDTOList)
        {
            log.LogMethodEntry(executionContext, turnstileDTOList);
            this.executionContext = executionContext;
            this.turnstileDTOList = turnstileDTOList;
            log.LogMethodExit();
        }


        // <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> by converting turnstileDTO</returns>
        private List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> BuildTurnstileDTOSearchParametersList(TurnstileSearchParams turnstileSearchParams)
        {
            log.LogMethodEntry(turnstileSearchParams);
            List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> turnstileDTOSearchParams = new List<KeyValuePair<TurnstileDTO.SearchByParameters, string>>();
            if (turnstileSearchParams != null)
            {
                if (turnstileSearchParams.TurnstileId > 0)
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.TURNSTILE_ID, turnstileSearchParams.TurnstileId.ToString()));

                if (turnstileSearchParams.Active)
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.ACTIVE, "1"));

                if (!string.IsNullOrEmpty(turnstileSearchParams.TurnstileName))
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.TURNSTILE_NAME, turnstileSearchParams.TurnstileName));

                if (!string.IsNullOrEmpty(turnstileSearchParams.Type))
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.TYPE, turnstileSearchParams.Type));

                if (!string.IsNullOrEmpty(turnstileSearchParams.Make))
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.MAKE, turnstileSearchParams.Make));

                if (!string.IsNullOrEmpty(turnstileSearchParams.Model))
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.MODEL, turnstileSearchParams.Model));

                if (turnstileSearchParams.GameProfileId > 0)
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.GAME_PROFILE_ID, turnstileSearchParams.GameProfileId.ToString()));

                if (turnstileSearchParams.SiteId > 0)
                    turnstileDTOSearchParams.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.SITE_ID, turnstileSearchParams.SiteId.ToString()));
            }
            log.LogMethodExit(turnstileDTOSearchParams);

            return turnstileDTOSearchParams;
        }

        public string TurnstileSetup(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();

            TurnstileDataHandler turnstileDataHandler = new TurnstileDataHandler(sqlTransaction);

            foreach (TurnstileDTO tDto in turnstileDTOList)
            {
                TurnstileUI.TurnstileObjectClass item = new TurnstileUI.TurnstileObjectClass();

                item.Type = turnstileDataHandler.GetTurnstileValues("TURNSTILE_TYPE", Convert.ToInt32(tDto.Type));
                item.Make = turnstileDataHandler.GetTurnstileValues("TURNSTILE_MAKE", Convert.ToInt32(tDto.Make));
                item.Model = turnstileDataHandler.GetTurnstileValues("TURNSTILE_MODEL", Convert.ToInt32(tDto.Model));
                item.GameProfileName = turnstileDataHandler.GetProfileName(tDto.GameProfileId);

                TurnstileClass device = null;
                if (TurnstileClass.TunstileMake.TISO.ToString().Equals(item.Make))
                {
                    device = new TISO();
                    if (tDto.PortNumber <= 0)
                        tDto.PortNumber = 9761;
                }

                item.DTO = tDto;
                item.Device = device;

              
                lstTurnstiles.Add(item);

                if (device != null)
                    device.SetParameters(tDto.IPAddress, (tDto.PortNumber == null ? -1 : Convert.ToInt32(tDto.PortNumber)));
            }
            string msg = DisplayTurnstileDetails(lstTurnstiles);
            log.LogMethodExit();
            return msg;

        }
        public string DisplayTurnstileDetails(List<TurnstileUI.TurnstileObjectClass> lstTurnstiles)
        {
            log.LogMethodEntry();
            string message = "";
            selectedTurnstile = null;
            if (CurTurnstileDevice != null)
                CurTurnstileDevice.Disconnect();

            CurTurnstileDevice = null;

            if (lstTurnstiles.Count == 1)
            {
                selectedTurnstile = lstTurnstiles[0];
                TurnstileDTO turnDetail = selectedTurnstile.DTO;
                TurnstileClass device = selectedTurnstile.Device;
              
                if (device != null && turnDetail.Active)
                {
                    CurTurnstileDevice = device;
                    if (CurTurnstileDevice != null)
                    {
                        ActivationCommand(CurTurnstileDevice.TurnstileData);
                    }
                    if (device.Connect())
                    {
                        message = turnDetail.TurnstileName + ":" + "Connected";
                        try
                        {
                            device.GetStatus();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred at displayTurnstileDetails() method", ex);
                            log.LogMethodExit(null, " Exception : " + ex.Message);
                            //throw new Exception(turnDetail.TurnstileName + ex.Message);
                            message = turnDetail.TurnstileName + ex.Message;
                        }
                    }
                    else
                    {
                        //message = turnDetail.TurnstileName + ":" + "Failed" + Environment.NewLine;
                        //message += turnDetail.TurnstileName + ":" + "Error: Invalid IP Address" + turnDetail.IPAddress + Environment.NewLine;
                        message += turnDetail.TurnstileName + ":" + "Unable to connect";
                    }
                }

                //if (device.TurnstileData != null)
                //{
                //    ActivationCommand(device.TurnstileData);
                //}
            }
            log.LogMethodExit(message);
            return message;
        }
        public void ActivationCommand(TurnstileClass.turnstileData turnstileData)
        {

            if (CurTurnstileDevice.TurnstileData.Panic)
                CurTurnstileDevice.PanicOff();
            else
                CurTurnstileDevice.Panic();
            if (CurTurnstileDevice.TurnstileData.SingleA)
            {
                CurTurnstileDevice.SingleA();
            }
            if (CurTurnstileDevice.TurnstileData.SingleB)
            {
                CurTurnstileDevice.SingleB();
            }
            if (CurTurnstileDevice.TurnstileData.FreeA)
            {
                CurTurnstileDevice.CancelFreeA();
            }
            else
            {
                CurTurnstileDevice.FreeA();
            }
            if (CurTurnstileDevice.TurnstileData.FreeB)
            {
                CurTurnstileDevice.CancelFreeB();
            }
            else
            {
                CurTurnstileDevice.FreeB();
            }
            if (CurTurnstileDevice.TurnstileData.LockA)
            {
                CurTurnstileDevice.UnlockA();
            }
            else
            {
                CurTurnstileDevice.LockA();
            }
            if (CurTurnstileDevice.TurnstileData.LockB)
            {
                CurTurnstileDevice.UnlockB();
            }
            else
            {
                CurTurnstileDevice.LockB();
            }
        }
        /// <summary>
        /// GetAllTurnstilesList method search based on turnstileSearchParams
        /// </summary>
        /// <param name="turnstileSearchParams"></param>
        /// <returns></returns>
        public List<TurnstileDTO> GetAllTurnstilesList(TurnstileSearchParams turnstileSearchParams, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(turnstileSearchParams);
                List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> searchParameters = BuildTurnstileDTOSearchParametersList(turnstileSearchParams);
                TurnstileDataHandler turnstileDataHandler = new TurnstileDataHandler(sqlTransaction);
                List<TurnstileDTO> turnstileDTOList = turnstileDataHandler.GetTurnstileDTOsList(searchParameters);
                log.LogMethodExit(turnstileDTOList);
                return turnstileDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetAllTurnstilesList(TurnstileSearchParams turnstileSearchParams) ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        public List<TurnstileDTO> GetAllTurnstilesList(List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> searchByParameters, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchByParameters, sqlTransaction);
                TurnstileDataHandler turnstileDataHandler = new TurnstileDataHandler(sqlTransaction);
                List<TurnstileDTO> turnstileDTOList = turnstileDataHandler.GetTurnstileDTOsList(searchByParameters);
                if (turnstileDTOList != null && turnstileDTOList.Any())
                {
                    foreach (TurnstileDTO tDto in turnstileDTOList)
                    {
                        TurnstileUI.TurnstileObjectClass item = new TurnstileUI.TurnstileObjectClass();
                        item.Make = turnstileDataHandler.GetTurnstileValues("TURNSTILE_MAKE", Convert.ToInt32(tDto.Make));
                        TurnstileClass device = null;
                        if (TurnstileClass.TunstileMake.TISO.ToString().Equals(item.Make))
                        {
                            device = new TISO();
                            if (tDto.PortNumber <= 0)
                                tDto.PortNumber = 9761;
                        }
                        item.Device = device;
                        tDto.TurnstileClass = device;
                    }
                }
                    log.LogMethodExit(turnstileDTOList);
                return turnstileDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetAllTurnstilesList(searchParameters) ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Saves the  list of turnstileDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (turnstileDTOList == null ||
                turnstileDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < turnstileDTOList.Count; i++)
            {
                var turnstileDTO = turnstileDTOList[i];
                if (turnstileDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    TurnstileBL turnstileBL = new TurnstileBL(executionContext, turnstileDTO);
                    turnstileBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving turnstileDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("turnstileDTO", turnstileDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
