/********************************************************************************************
 * Project Name - Lockers
 * Description  - The bussiness logic for locker
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera         Created
 *2.60        02-May-2019   Jagan Mohana Rao   Created SaveUpdateLockerPanels() and changes log method entry and exit
 *2.70.2        19-Jul-2019   Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90        27-May-2020   Mushahid Faizan    Modified : 3 tier changes for Rest API
  *3.0         05-Nov-2020   Mushahid Faizan   WMS issue fixes, Added CreateLocker method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Bussiness logic class for Lockers operations
    /// </summary>
    public class LockerPanel
    {
        private LockerPanelDTO lockerPanelDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of LockerPanels class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private LockerPanel(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            lockerPanelDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor will fetch the LockerPanel DTO based on the locker id passed 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="panelId">panelId</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public LockerPanel(ExecutionContext executionContext, int panelId, SqlTransaction sqltransaction = null)
            : this(executionContext)
        {
            List<LockerPanelDTO> lockerPanelDTOList = new List<LockerPanelDTO>();
            log.LogMethodEntry(panelId, executionContext, sqltransaction);
            LockerPanelDataHandler lockerPanelsDataHandler = new LockerPanelDataHandler(sqltransaction);
            lockerPanelDTO = lockerPanelsDataHandler.GetLockerPanel(panelId);
            if (lockerPanelDTO != null)
            {
                lockerPanelDTOList.Add(lockerPanelDTO);
                LockerPanelsList lockerPanelsList = new LockerPanelsList(executionContext);
                lockerPanelsList.BuildLockerPanelList(lockerPanelDTOList, false);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates locker object using the LockerPanelsDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="lockerPanelDTO">lockerPanelDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public LockerPanel(ExecutionContext executionContext, LockerPanelDTO lockerPanelDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(lockerPanelDTO, executionContext, sqlTransaction);
            this.lockerPanelDTO = lockerPanelDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Returns the Locker allocation record matches with allocation id
        ///// </summary>
        //public LockerPanelDTO GetLockerPanel(int allocationId)
        //{
        //    log.Debug("Starts-GetLockerPanel(allocationId) method.");
        //    LockerPanelDataHandler lockerPanelsDataHandler = new LockerPanelDataHandler();
        //    log.Debug("Ends-GetLockerPanel(allocationId) method by returning the result of lockerPanelsDataHandler.GetLockerPanel(allocationId) call.");
        //    return lockerPanelsDataHandler.GetLockerPanel(allocationId);
        //}

        /// <summary>
        ///  Saves the locker record
        /// Checks if the schedule id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (lockerPanelDTO.IsChangedRecursive == false
                           && lockerPanelDTO.PanelId > -1)
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
            LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler(sqlTransaction);
            if (lockerPanelDTO.PanelId < 0)
            {
                lockerPanelDTO = lockerPanelDataHandler.InsertLockerPanel(lockerPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                lockerPanelDTO.AcceptChanges();
                //CreateLockers(lockerPanelDTO, sqlTransaction);
            }
            else
            {
                if (lockerPanelDTO.IsChanged)
                {
                    lockerPanelDTO = lockerPanelDataHandler.UpdateLocker(lockerPanelDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    lockerPanelDTO.AcceptChanges();
                }
            }
            CreateLockers(lockerPanelDTO, sqlTransaction);
            SaveLockerPanelsChild(sqlTransaction);
            log.LogMethodExit();
        }


        private void CreateLockers(LockerPanelDTO lockerPanelDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<LockerDTO> lockerFilterDTO = null;
            List<LockerDTO> lockerDTOList = new List<LockerDTO>();
            LockersList lockerList = new LockersList();
            List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchLockerParameters = new List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>>();
            searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            lockerDTOList = lockerList.GetAllLocker(searchLockerParameters, false, sqlTransaction);

            Semnox.Parafait.Device.Lockers.Locker locker = null;
            LockerDTO lockerDTO;
            int identifier;
            if (lockerDTOList != null && lockerDTOList.Count > 0)
            {
                identifier = lockerDTOList.Max(x => x.Identifier) + 1;
            }
            else
            {
                identifier = 1;
            }
            if (lockerPanelDTO.NumRows == -1
                || lockerPanelDTO.NumCols == -1
                || lockerPanelDTO.NumRows == 0
                || lockerPanelDTO.NumCols == 0)
            {
                if (locker == null)
                {
                    locker = new Semnox.Parafait.Device.Lockers.Locker(executionContext);
                }
                locker.RemoveLockers(lockerPanelDTO.PanelId, -1, -1);
            }

            int rows = lockerPanelDTO.NumRows;
            int cols = lockerPanelDTO.NumCols;
            string prefix = lockerPanelDTO.SequencePrefix;
            int index = 1;
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= cols; j++)
                {
                    if (lockerDTOList != null)
                    {
                        lockerFilterDTO = lockerDTOList.Where(x => (bool)(x.PanelId == lockerPanelDTO.PanelId && x.RowIndex == i && x.ColumnIndex == j)).ToList<LockerDTO>();
                    }
                    if (lockerFilterDTO == null || (lockerFilterDTO != null && lockerFilterDTO.Count == 0))
                    {
                        lockerDTO = new LockerDTO();
                        lockerDTO.LockerName = prefix + index.ToString();
                        lockerDTO.PanelId = lockerPanelDTO.PanelId;
                        lockerDTO.RowIndex = i;
                        lockerDTO.ColumnIndex = j;
                        lockerDTO.Identifier = identifier;
                        locker = new Semnox.Parafait.Device.Lockers.Locker(executionContext, lockerDTO);
                        locker.Save();
                        index++;
                        if (lockerDTO.LockerId > -1)
                            identifier++;
                    }
                    else
                    {
                        if ((string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(new String(lockerFilterDTO[0].LockerName.Where(x => ((bool)Char.IsLetter(x) || Char.IsPunctuation(x) || Char.IsSymbol(x))).ToArray())))
                            || (!string.IsNullOrEmpty(prefix) && new String(prefix.Where(x => ((bool)Char.IsLetter(x) || Char.IsPunctuation(x) || Char.IsSymbol(x))).ToArray()) != new String(lockerFilterDTO[0].LockerName.Where(x => ((bool)Char.IsLetter(x) || Char.IsPunctuation(x) || Char.IsSymbol(x))).ToArray())))
                        {
                            lockerFilterDTO[0].LockerName = prefix + new String(lockerFilterDTO[0].LockerName.Where(Char.IsNumber).ToArray());
                        }
                        locker = new Semnox.Parafait.Device.Lockers.Locker(executionContext, lockerFilterDTO[0]);
                        locker.Save();
                        index++;
                    }
                }
            }
            if (locker == null)
            {
                locker = new Semnox.Parafait.Device.Lockers.Locker();
            }
            locker.RemoveLockers(lockerPanelDTO.PanelId, lockerPanelDTO.NumRows, -1);
            locker.RemoveLockers(lockerPanelDTO.PanelId, -1, lockerPanelDTO.NumCols);

            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveLockerPanelsChild(SqlTransaction sqlTransaction)
        {

            // for Child Records : :LockerDTO
            if (lockerPanelDTO.LockerDTOList != null && lockerPanelDTO.LockerDTOList.Any())
            {
                List<LockerDTO> updatedLockerDTOList = new List<LockerDTO>();
                foreach (LockerDTO lockerDTO in lockerPanelDTO.LockerDTOList)
                {
                    if (lockerDTO.PanelId != lockerPanelDTO.PanelId)
                    {
                        lockerDTO.PanelId = lockerPanelDTO.PanelId;
                    }
                    if (lockerDTO.IsChanged)
                    {
                        updatedLockerDTOList.Add(lockerDTO);
                    }
                }
                if (updatedLockerDTOList.Any())
                {
                    LockersList lockersList = new LockersList(executionContext, updatedLockerDTOList);
                    lockersList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the lockerPanelDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LockerPanelDTO getLockerPanelDTO { get { return lockerPanelDTO; } }
    }

    /// <summary>
    /// Manages the list of lockers
    /// </summary>
    public class LockerPanelsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<LockerPanelDTO> lockerPanelDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public LockerPanelsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.lockerPanelDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="lockerPanelDTOList">lockerPanelDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public LockerPanelsList(ExecutionContext executionContext, List<LockerPanelDTO> lockerPanelDTOList)
        {
            log.LogMethodEntry(lockerPanelDTOList, executionContext);
            this.lockerPanelDTOList = lockerPanelDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the locker list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecord">loadChildRecord</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerPanelDTO> GetAllLockerPanels(List<KeyValuePair<LockerPanelDTO.SearchByLockerPanelsParameters, string>> searchParameters, bool loadChildRecord, SqlTransaction sqlTransaction = null)
        {
            List<LockerPanelDTO> lockerPanelDTOList;
            log.LogMethodEntry(searchParameters, loadChildRecord, sqlTransaction);
            LockerPanelDataHandler lockerPanelDataHandler = new LockerPanelDataHandler(sqlTransaction);
            lockerPanelDTOList = lockerPanelDataHandler.GetLockerPanelsList(searchParameters, false);
            BuildLockerPanelList(lockerPanelDTOList, false);
            log.LogMethodExit(lockerPanelDTOList);
            return lockerPanelDTOList;
        }

        /// <summary>
        /// loads the child records and
        /// </summary>
        /// <param name="lockerPanelDTOList">LockerpanelDTO list</param>
        /// <param name="loadInactiveChildRecord">Load active child record or not</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void BuildLockerPanelList(List<LockerPanelDTO> lockerPanelDTOList, bool loadInactiveChildRecord, SqlTransaction sqlTransaction = null)
        {
            string zoneIdList = "-1";
            LockersList lockerList = new LockersList();
            List<LockerDTO> lockerDTOList;
            log.LogMethodEntry(lockerPanelDTOList, loadInactiveChildRecord, sqlTransaction);
            if (lockerPanelDTOList == null)
            {
                log.LogMethodExit(null);
                return;
            }
            foreach (LockerPanelDTO lockerPanelDTO in lockerPanelDTOList)
            {
                zoneIdList += ", " + lockerPanelDTO.ZoneId;
            }
            zoneIdList += "";

            List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>> searchLockerParameters = new List<KeyValuePair<LockerDTO.SearchByLockersParameters, string>>();
            if (!loadInactiveChildRecord)
                searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.IS_ACTIVE, "1"));
            searchLockerParameters.Add(new KeyValuePair<LockerDTO.SearchByLockersParameters, string>(LockerDTO.SearchByLockersParameters.Zone_ID_LIST, zoneIdList));
            lockerDTOList = lockerList.GetAllLocker(searchLockerParameters, false, sqlTransaction);
            if (lockerDTOList != null)
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

            if (lockerPanelDTOList != null && lockerDTOList != null)
            {
                foreach (LockerPanelDTO lockerPanelDTO in lockerPanelDTOList)
                {
                    lockerPanelDTO.LockerDTOList = lockerDTOList.Where(x => (bool)(x.PanelId == lockerPanelDTO.PanelId)).ToList();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update locker panels for Web Management Studio
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (lockerPanelDTOList == null ||
               lockerPanelDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < lockerPanelDTOList.Count; i++)
            {
                var lockerPanelDTO = lockerPanelDTOList[i];
                if (lockerPanelDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    LockerPanel lockerPanel = new LockerPanel(executionContext, lockerPanelDTO);
                    lockerPanel.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving lockerPanelDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("lockerPanelDTO", lockerPanelDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
