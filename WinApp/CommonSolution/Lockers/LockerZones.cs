/********************************************************************************************
* Project Name - LockerZones Programs 
* Description  - Data object of the LockerZones 
* 
**************
**Version Log
**************
*Version     Date           Modified By       Remarks          
*********************************************************************************************
*1.00        6-Nov-2017     Archana           Created 
*2.60        02-May-2019    Jagan Mohana Rao  Created SaveUpdateLockerZones() and changes log method entry and exit
*2.70.2        19-Jul-2019    Dakshakh raj      Modified : Save() method Insert/Update method returns DTO.
*2.90        27-May-2020   Mushahid Faizan Modified : 3 tier changes for Rest API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the LockerZones  data object class. This acts as data holder for the LockerZonesBL  business object
    /// </summary>
    public class LockerZones
    {
        private LockerZonesDTO lockerZonesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        ///  Default constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LockerZones(ExecutionContext executionContext)  // used in File
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  LockerZonesDTO constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="lockerZonesDTO">lockerZonesDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerZones(ExecutionContext executionContext, LockerZonesDTO lockerZonesDTO, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, lockerZonesDTO, sqlTransaction);
            this.lockerZonesDTO = lockerZonesDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the Locker Zones DTO based on the lockerZonesDTO ZoneId passed 
        /// </summary>
        /// <param name="ZoneId">Id</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerZones(ExecutionContext executionContext, int ZoneId, SqlTransaction sqlTransaction = null)
                : this(executionContext)
        {
            List<LockerZonesDTO> lockerZonesDTOList = new List<LockerZonesDTO>();
            log.LogMethodEntry(ZoneId, executionContext, sqlTransaction);
            LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
            lockerZonesDTO = lockerZonesDataHandler.GetLockerZonesDTO(ZoneId);
            if (lockerZonesDTO != null)
            {
                lockerZonesDTOList.Add(lockerZonesDTO);
                LockerZonesList lockerZonesList = new LockerZonesList(executionContext);
                lockerZonesList.BuildLockerZoneList(lockerZonesDTOList, false);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates if any reference exists
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public bool ValidateZoneMapping(SqlTransaction sqlTransaction = null)
        {
            bool status = false;
            log.LogMethodEntry(sqlTransaction);
            if (lockerZonesDTO != null)
            {
                LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
                status = lockerZonesDataHandler.ValidateProductZoneMapping(lockerZonesDTO.ZoneId);
            }
            log.LogMethodExit(status);
            return status;
        }

        /// <summary>
        /// Used For Save 
        /// It may by Insert Or Update
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
            if (lockerZonesDTO.IsChangedRecursive == false)
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
            try
            {
                if (lockerZonesDTO.ZoneId < 0)
                {
                    lockerZonesDTO = lockerZonesDataHandler.InsertLockerZones(lockerZonesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    lockerZonesDTO.AcceptChanges();
                }
                else
                {
                    if (lockerZonesDTO.IsChanged)
                    {
                        lockerZonesDTO = lockerZonesDataHandler.UpdateLockerZones(lockerZonesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        lockerZonesDTO.AcceptChanges();
                    }
                    SaveLockerZonesChild(sqlTransaction);
                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveLockerZonesChild(SqlTransaction sqlTransaction)
        {

            // for Child Records : :LockerPanelDTO
            if (lockerZonesDTO.LockerPanelDTOList != null && lockerZonesDTO.LockerPanelDTOList.Any())
            {
                List<LockerPanelDTO> updatedLockerPanelDTOList = new List<LockerPanelDTO>();
                foreach (LockerPanelDTO lockerPanelDTO in lockerZonesDTO.LockerPanelDTOList)
                {
                    if (lockerPanelDTO.ZoneId != lockerZonesDTO.ZoneId)
                    {
                        lockerPanelDTO.ZoneId = lockerZonesDTO.ZoneId;
                    }
                    if (lockerPanelDTO.IsChanged)
                    {
                        updatedLockerPanelDTOList.Add(lockerPanelDTO);
                    }
                }
                if (updatedLockerPanelDTOList.Any())
                {
                    LockerPanelsList lockerPanelsList = new LockerPanelsList(executionContext, updatedLockerPanelDTOList);
                    lockerPanelsList.Save();
                }
            }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (lockerZonesDTO == null)
            {
                //Validation to be implemented.
            }

            if (lockerZonesDTO.LockerPanelDTOList != null && lockerZonesDTO.LockerPanelDTOList.Any())
            {
                foreach (var lockerPanelDTO in lockerZonesDTO.LockerPanelDTOList)
                {
                    if (lockerPanelDTO.IsChanged)
                    {
                        LockerPanel lockerPanel = new LockerPanel(executionContext, lockerPanelDTO);
                        validationErrorList.AddRange(lockerPanel.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Load Locker Zoneby LockerId
        /// </summary>
        /// <param name="lockerId">lockerId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void LoadLockerZonebyLockerId(int lockerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerId, sqlTransaction);
            try
            {
                LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
                lockerZonesDTO = lockerZonesDataHandler.LoadLockerZonebyLockerId(lockerId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// gets the GetLockerZonesDTO
        /// </summary>
        public LockerZonesDTO GetLockerZonesDTO
        {
            get { return lockerZonesDTO; }
        }

    }

    /// <summary>
    /// Manages the list of LockerZones
    /// </summary>
    public class LockerZonesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LockerZonesDTO> lockerZonesDTOList = new List<LockerZonesDTO>();
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LockerZonesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.lockerZonesDTOList = new List<LockerZonesDTO>();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="lockerZonesDTOList">lockerZonesDTOList</param>
        public LockerZonesList(ExecutionContext executionContext, List<LockerZonesDTO> lockerZonesDTOList)
        {
            log.LogMethodEntry(executionContext, lockerZonesDTOList);
            this.lockerZonesDTOList = lockerZonesDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetParentZones method
        /// </summary>
        /// <param name="loadChildEntity">loadChildEntity</param>
        /// <param name="loadInactiveChildRecord">loadInactiveChildRecord</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerZonesDTO> GetParentZones(bool loadChildEntity, bool loadInactiveChildRecord, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loadChildEntity, loadInactiveChildRecord, sqlTransaction);
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            int site_id = executionContext.GetSiteId();
            LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
            List<LockerZonesDTO> lockerZonesDTOList = lockerZonesDataHandler.GetParentZones(site_id, loadChildEntity, loadInactiveChildRecord);
            log.LogMethodExit(lockerZonesDTOList);
            return lockerZonesDTOList;
        }

        /// <summary>
        /// GetZones method
        /// </summary>
        /// <param name="loadChildEntity">loadChildEntity</param>
        /// <param name="loadInactiveChildRecord">loadInactiveChildRecord</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerZonesDTO> GetZones(bool loadChildEntity, bool loadInactiveChildRecord, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loadChildEntity, loadInactiveChildRecord, sqlTransaction);
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            int site_id = executionContext.GetSiteId();
            LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
            List<LockerZonesDTO> lockerZonesDTOList = lockerZonesDataHandler.GetZonesList(site_id, loadChildEntity, loadInactiveChildRecord);
            log.LogMethodExit(lockerZonesDTOList);
            return lockerZonesDTOList;
        }

        /// <summary>
        /// Gets the lockerZoneslist detail which matches with the passed Zone id 
        /// </summary>
        /// <param name="ZoneId">integer type parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns LockerZonesDTO</returns>
        public DataTable GetLockerZonesList(int ZoneId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ZoneId, sqlTransaction);
            LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
            DataTable dataTable = lockerZonesDataHandler.GetLockerZonesList(ZoneId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns Search Request And returns List Of LockerZonesDTO Class  
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadInactiveChildRecord">loadInactiveChildRecord</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerZonesDTO> GetLockerZonesList(List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> searchParameters, bool loadInactiveChildRecord = false, SqlTransaction sqlTransaction = null)
        {
            List<LockerZonesDTO> lockerZonesDTOList;
            log.LogMethodEntry(searchParameters, loadInactiveChildRecord, sqlTransaction);
            try
            {
                LockerZonesDataHandler lockerZonesDataHandler = new LockerZonesDataHandler(sqlTransaction);
                lockerZonesDTOList = lockerZonesDataHandler.GetLockerZonesList(searchParameters, false, loadInactiveChildRecord);
                if (lockerZonesDTOList != null && lockerZonesDTOList.Any())
                {
                    BuildLockerZoneList(lockerZonesDTOList, loadInactiveChildRecord);
                }
                log.LogMethodExit(lockerZonesDTOList);
                return lockerZonesDTOList;
            }
            catch (Exception expn)
            {
                log.Error("Error while loading zone.", expn);
                log.LogMethodExit(expn.Message);
                throw new Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// BuildLockerZoneList
        /// </summary>
        /// <param name="lockerZonesDTOList">lockerZonesDTOList</param>
        /// <param name="loadInactiveChildRecord">loadInactiveChildRecord</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void BuildLockerZoneList(List<LockerZonesDTO> lockerZonesDTOList, bool loadInactiveChildRecord, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lockerZonesDTOList, loadInactiveChildRecord, sqlTransaction);
            string zoneIdList = "-1";
            LockerPanelsList lockerPanelsList = new LockerPanelsList(executionContext);
            LockersList lockerList = new LockersList();
            List<LockerPanelDTO> lockerPanelDTOList;
            List<LockerDTO> lockerDTOList;
            log.LogMethodEntry();
            if (lockerZonesDTOList == null)
            {
                log.LogMethodExit(null);
                return;
            }
            foreach (LockerZonesDTO lockerZoneDTO in lockerZonesDTOList)
            {
                zoneIdList += ", " + lockerZoneDTO.ZoneId;
            }
            zoneIdList += "";
            List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>> searchParameters = new List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>>();
            if (!loadInactiveChildRecord)
                searchParameters.Add(new KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>(LockerPanelDTO.SearchByLockerPanelsParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>(LockerPanelDTO.SearchByLockerPanelsParameters.Zone_ID_LIST, zoneIdList));
            lockerPanelDTOList = lockerPanelsList.GetAllLockerPanels(searchParameters, false);

            List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchLockerParameters = new List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>>();
            if (!loadInactiveChildRecord)
                searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.IS_ACTIVE, "1"));
            searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.Zone_ID_LIST, zoneIdList));
            lockerDTOList = lockerList.GetAllLocker(searchLockerParameters, false, sqlTransaction);
            if (lockerDTOList != null && lockerDTOList.Any())
            {
                LockerAllocationList lockerAllocationList = new LockerAllocationList();
                List<LockerAllocationDTO> lockerAllocationDTOList = lockerAllocationList.GetLockerAllocationOnZoneList(zoneIdList);
                if (lockerAllocationDTOList != null && lockerAllocationDTOList.Any())
                {
                    foreach (LockerAllocationDTO lockerAllocationDTO in lockerAllocationDTOList)
                    {
                        if (lockerDTOList.Exists(x => x.LockerId == lockerAllocationDTO.LockerId))
                        {
                            lockerDTOList.Where(x => (x.LockerId == lockerAllocationDTO.LockerId)).ToList()[0].LockerAllocated = lockerAllocationDTO;
                        }
                    }
                }
            }
            if (lockerPanelDTOList != null && lockerPanelDTOList.Any())
            {
                foreach (LockerZonesDTO lockerZoneDTO in lockerZonesDTOList)
                {
                    lockerZoneDTO.LockerPanelDTOList = lockerPanelDTOList.Where(x => (bool)(x.ZoneId == lockerZoneDTO.ZoneId)).ToList();
                    if (lockerZoneDTO.LockerPanelDTOList != null && lockerDTOList != null)
                    {
                        foreach (LockerPanelDTO lockerPanelDTO in lockerPanelDTOList)
                        {
                            lockerPanelDTO.LockerDTOList = lockerDTOList.Where(x => (bool)(x.PanelId == lockerPanelDTO.PanelId)).ToList();
                        }
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update locker zones for Web Management Studio
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (lockerZonesDTOList == null ||
               lockerZonesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < lockerZonesDTOList.Count; i++)
            {
                var lockerZonesDTO = lockerZonesDTOList[i];
                if (lockerZonesDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    LockerZones lockerZones = new LockerZones(executionContext, lockerZonesDTO);
                    lockerZones.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving lockerZonesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("lockerZonesDTO", lockerZonesDTO);
                    throw;
                }
            }
        }
    }
}



