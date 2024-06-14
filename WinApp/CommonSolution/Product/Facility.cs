/* Project Name - Semnox.Parafait.Booking.Facility 
* Description  - Business call object of the Facility
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.70        26-Nov-2018    Guru S A             Booking phase 2 enhancement changes 
*2.70        22-Feb-2019    Akshay G             Added IsExistFacilityName(), GetFacilitySeatsLayoutList(searchParameter) in FacilityList Class 
*                                                modified Save() in FacilityBL class
*2.70        29-Jun-2019    Akshay G             Added DeleteFacility() and DeleteFacilityList() methods
*2.70        10-Jul-2019    Akshay G             modified DeleteFacilityList() method
*2.70.2      29-Oct-2019    Akshay G             ClubSpeed enhancement changes - added GetFacilities()
*2.70.3      26-Feb-2020    Girish Kundar        Modified : Added Build() method to build the child list  
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{

    /// <summary>
    /// Facility
    /// </summary>
    public class FacilityBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FacilityDTO facilityDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of FacilityBL class
        /// </summary>
        private FacilityBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.facilityDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Facility id as the parameter Would fetch the facility object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        ///// <param name="buildAllowedProductList"></param>
        public FacilityBL(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                          bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
            facilityDTO = facilityDataHandler.GetFacilityDTO(id);
            if (facilityDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Facility", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate Facility DTO
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            //build First child list FacilitySeatDTO
            FacilitySeatsListBL facilitySeatsListBL = new FacilitySeatsListBL(executionContext);
            List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>> searchParameters = new List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>>();
            searchParameters.Add(new KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>(FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID, facilityDTO.FacilityId.ToString()));
            facilityDTO.FacilitySeatsDTOList = facilitySeatsListBL.GetFacilitySeatsDTOList(searchParameters, sqlTransaction);

            //build second child list FacilitySeatLayoutDTO
            FacilitySeatLayoutListBL facilitySeatLayoutListBL = new FacilitySeatLayoutListBL(executionContext);
            List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>> psearchParameters = new List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>>();
            psearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID, facilityDTO.FacilityId.ToString()));
            facilityDTO.FacilitySeatLayoutDTOList = facilitySeatLayoutListBL.GetFacilitySeatLayoutDTOList(psearchParameters, sqlTransaction);

            // build third childFacilityWaiver
            FacilityWaiverListBL facilityWaiverListBL = new FacilityWaiverListBL(executionContext);
            List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>> wsearchParameters = new List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>>();
            wsearchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.FACILITY_ID, facilityDTO.FacilityId.ToString()));
            facilityDTO.FacilityWaiverDTOList = facilityWaiverListBL.GetAllFacilityWaiverList(wsearchParameters, sqlTransaction);

            //build Fourth child List FacilityTableDTOList
            FacilityTablesList facilityTablesListBL = new FacilityTablesList(executionContext);
            List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> tsearchParameters = new List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>>();
            tsearchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.FACILITY_ID, facilityDTO.FacilityId.ToString()));
            facilityDTO.FacilityTableDTOList = facilityTablesListBL.GetAllFacilityTableList(tsearchParameters, sqlTransaction);

            log.LogMethodExit();
        }

        /// <summary>
        /// Creates FacilityBL object using the FacilityDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="FacilityDTO">facilityDTO object</param>
        public FacilityBL(ExecutionContext executionContext, FacilityDTO facilityDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityDTO);
            this.facilityDTO = facilityDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the facilityDTO  ,FacilitySeatLayoutDTOList - child ,FacilitySeatsDTOList -Child
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<FacilityMapDTO> facilityMapDTOList = new List<FacilityMapDTO>();
            List<FacilityWaiverDTO> facilityWaiverDTOList = new List<FacilityWaiverDTO>();
            if (facilityDTO != null && facilityDTO.FacilityId > -1 && facilityDTO.ActiveFlag == false)
            {
                List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.FACILITY_ID, facilityDTO.FacilityId.ToString()));
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParameters);

                List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>> searchWaiverParameters = new List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>>();
                searchWaiverParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchWaiverParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.FACILITY_ID, facilityDTO.FacilityId.ToString()));
                searchWaiverParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                FacilityWaiverListBL facilityWaiverListBL = new FacilityWaiverListBL(executionContext);
                facilityWaiverDTOList = facilityWaiverListBL.GetAllFacilityWaiverList(searchWaiverParameters);

                if (facilityMapDTOList != null && facilityMapDTOList.Any() && facilityDTO.ActiveFlag == false)
                {
                    validationErrorList.Add(new ValidationError("Facility", "FacilityMap", MessageContainerList.GetMessage(executionContext, 1869)));
                }

                if (facilityWaiverDTOList != null && facilityWaiverDTOList.Any() && facilityDTO.ActiveFlag == false)
                {
                    validationErrorList.Add(new ValidationError("Facility", "FacilityWaiver", MessageContainerList.GetMessage(executionContext, 1869)));
                }

            }

            if (string.IsNullOrWhiteSpace(facilityDTO.FacilityName))
            {
                validationErrorList.Add(new ValidationError("Facility", "FacilityName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Facility Name"))));
            }

            if (!string.IsNullOrWhiteSpace(facilityDTO.Description) && facilityDTO.Description.Length > 50)
            {
                validationErrorList.Add(new ValidationError("Facility", "Description", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Description"), 50)));
            }
            if (!string.IsNullOrWhiteSpace(facilityDTO.FacilityName) && facilityDTO.FacilityName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("Facility", "FacilityName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Facility Name"), 50)));
            }

            if (facilityDTO.FacilitySeatLayoutDTOList != null)
            {
                foreach (var facilitySeatLayoutDTO in facilityDTO.FacilitySeatLayoutDTOList)
                {
                    if (facilitySeatLayoutDTO.IsChanged)
                    {
                        FacilitySeatLayoutBL facilitySeatLayoutBL = new FacilitySeatLayoutBL(executionContext, facilitySeatLayoutDTO);
                        validationErrorList.AddRange(facilitySeatLayoutBL.Validate(sqlTransaction));
                    }
                }
                if (facilityDTO.FacilitySeatsDTOList != null)
                {
                    foreach (var facilitySeatsDTO in facilityDTO.FacilitySeatsDTOList)
                    {
                        if (facilitySeatsDTO.IsChanged)
                        {
                            FacilitySeatsBL facilitySeatsBL = new FacilitySeatsBL(executionContext, facilitySeatsDTO);
                            validationErrorList.AddRange(facilitySeatsBL.Validate(sqlTransaction));
                        }
                    }
                }
                if (facilityDTO.FacilityWaiverDTOList != null)
                {
                    foreach (var facilityWaiverDTO in facilityDTO.FacilityWaiverDTOList)
                    {
                        if (facilityWaiverDTO.IsChanged)
                        {
                            FacilityWaiverBL facilityWaiverBL = new FacilityWaiverBL(executionContext, facilityWaiverDTO);
                            validationErrorList.AddRange(facilityWaiverBL.Validate());
                        }
                    }
                }
                if (facilityDTO.FacilityTableDTOList != null)//This snippet is for saving FacilityTableDTOList
                {
                    foreach (FacilityTableDTO facilityTableDTO in facilityDTO.FacilityTableDTOList)
                    {
                        if (facilityTableDTO.IsChanged)
                        {
                            FacilityTables facilityTables = new FacilityTables(executionContext, facilityTableDTO);
                            validationErrorList.AddRange(facilityTables.Validate());
                        }
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the Facility
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityDTO.IsChangedRecursive == false
                  && facilityDTO.FacilityId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            //if (facilityDTO.ActiveFlag)
            //{
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
            if (facilityDTO.FacilityId < 0)
            {
                facilityDTO = facilityDataHandler.InsertFacility(facilityDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                facilityDTO.AcceptChanges();
            }
            else
            {
                if (facilityDTO.IsChanged)
                {
                    facilityDTO = facilityDataHandler.UpdateFacility(facilityDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilityDTO.AcceptChanges();
                }
            }
            SaveChildList(sqlTransaction);
            //}
            //else
            //{
            //    if (facilityDTO.FacilityId >= 0)
            //    {
            //        DeleteFacility(sqlTransaction);
            //    }
            //    facilityDTO.AcceptChanges();
            //}
            log.LogMethodExit();
        }

        private void SaveChildList(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityDTO.FacilitySeatLayoutDTOList != null && facilityDTO.FacilitySeatLayoutDTOList.Count > 0) //This snippet is for saving FacilitySeatLayoutDTO
            {
                foreach (FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilityDTO.FacilitySeatLayoutDTOList)
                {
                    facilitySeatLayoutDTO.FacilityId = facilityDTO.FacilityId;
                    FacilitySeatLayoutBL facilitySeatLayoutBL = new FacilitySeatLayoutBL(executionContext, facilitySeatLayoutDTO);
                    facilitySeatLayoutBL.Save(sqlTransaction);
                }
            }
            if (facilityDTO.FacilitySeatsDTOList != null && facilityDTO.FacilitySeatsDTOList.Count > 0)//This snippet is for saving FacilitySeatsDTO
            {
                foreach (FacilitySeatsDTO facilitySeatsDTO in facilityDTO.FacilitySeatsDTOList)
                {
                    facilitySeatsDTO.FacilityId = facilityDTO.FacilityId;
                    FacilitySeatsBL facilitySeatsBL = new FacilitySeatsBL(executionContext, facilitySeatsDTO);
                    facilitySeatsBL.Save(sqlTransaction);
                }
            }
            if (facilityDTO.FacilityWaiverDTOList != null && facilityDTO.FacilityWaiverDTOList.Count > 0)//This snippet is for saving FacilityWaiverDTOList
            {
                foreach (FacilityWaiverDTO facilityWaiverDTO in facilityDTO.FacilityWaiverDTOList)
                {
                    facilityWaiverDTO.FacilityId = facilityDTO.FacilityId;
                    FacilityWaiverBL facilityWaiverBL = new FacilityWaiverBL(executionContext, facilityWaiverDTO);
                    facilityWaiverBL.Save(sqlTransaction);
                }
            }
            if (facilityDTO.FacilityTableDTOList != null && facilityDTO.FacilityTableDTOList.Count > 0)//This snippet is for saving FacilityTableDTOList
            {
                foreach (FacilityTableDTO facilityTableDTO in facilityDTO.FacilityTableDTOList)
                {
                    facilityTableDTO.FacilityId = facilityDTO.FacilityId;
                    FacilityTables facilityTables = new FacilityTables(executionContext, facilityTableDTO);
                    facilityTables.Save(sqlTransaction);
                }
            }
            facilityDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the DTO
        /// </summary>
        public FacilityDTO FacilityDTO
        {
            get
            {
                return facilityDTO;
            }
        }

        /// <summary>
        /// Deletes the Facility based on facilityId
        /// </summary>   
        /// <param name="facilityId">facilityId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(facilityDTO, sqlTransaction);
            try
            {
                FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
                if ((facilityDTO.FacilitySeatLayoutDTOList != null &&
                                    facilityDTO.FacilitySeatsDTOList.Any(x => x.Active == 'Y'))
               || facilityDTO.FacilityWaiverDTOList != null &&
                                    facilityDTO.FacilityWaiverDTOList.Any(x => x.IsActive == true)
               || facilityDTO.FacilitySeatsDTOList != null &&
                                    facilityDTO.FacilitySeatsDTOList.Any(x => x.Active == 'Y')
               || facilityDTO.FacilityTableDTOList != null &&
                                    facilityDTO.FacilityTableDTOList.Any(x => x.Active == true))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                if (facilityDTO.FacilityTableDTOList != null && facilityDTO.FacilityTableDTOList.Any())//This snippet is for saving FacilityTableDTOList
                {
                    FacilityTablesList facilityTablesList = new FacilityTablesList(executionContext, facilityDTO.FacilityTableDTOList);
                    facilityTablesList.Save(sqlTransaction);
                }
                if (facilityDTO.FacilityWaiverDTOList != null && facilityDTO.FacilityWaiverDTOList.Any())//This snippet is for saving FacilityWaiverDTOList
                {
                    FacilityWaiverListBL facilityWaiverBL = new FacilityWaiverListBL(executionContext, facilityDTO.FacilityWaiverDTOList);
                    facilityWaiverBL.Delete(sqlTransaction);
                }
                if (facilityDTO.FacilitySeatLayoutDTOList != null && facilityDTO.FacilitySeatLayoutDTOList.Any())//This snippet is for saving FacilitySeatsDTO
                {
                    FacilitySeatLayoutListBL facilitySeatLayoutListBL = new FacilitySeatLayoutListBL(executionContext, facilityDTO.FacilitySeatLayoutDTOList);
                    facilitySeatLayoutListBL.Save(sqlTransaction);
                }
                if (facilityDTO.FacilitySeatsDTOList != null && facilityDTO.FacilitySeatsDTOList.Any())//This snippet is for saving FacilitySeatsDTO
                {
                    FacilitySeatsListBL facilitySeatsListBL = new FacilitySeatsListBL(executionContext, facilityDTO.FacilitySeatsDTOList);
                    facilitySeatsListBL.Delete(sqlTransaction);
                }
                SaveChildList(sqlTransaction);
                log.LogVariableState("facilityDTO", facilityDTO);
                facilityDataHandler.DeleteFacility(facilityDTO.FacilityId);
                log.LogMethodExit();
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Facilities
    /// </summary>
    public class FacilityList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<FacilityDTO> facilityDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public FacilityList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with executionContext and facilityList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="facilityDTOList"></param>
        public FacilityList(ExecutionContext executionContext, List<FacilityDTO> facilityDTOList)
        {
            log.LogMethodEntry(executionContext, facilityDTOList);
            this.executionContext = executionContext;
            this.facilityDTOList = facilityDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetFacilityDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<FacilityDTO></returns>
        public List<FacilityDTO> GetFacilityDTOList(List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters,
                                              bool loadChildRecords = false, bool activeChildRecords = false,
                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTransaction);
            List<FacilityDTO> facilityDTOList = facilityDataHandler.GetFacilityDTOList(searchParameters);
            if (facilityDTOList != null && facilityDTOList.Any() && loadChildRecords)
            {
                Build(facilityDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(facilityDTOList);
            return facilityDTOList;
        }

        private void Build(List<FacilityDTO> facilityDTOList, bool activeChildRecords = true,
                           SqlTransaction sqlTransaction = null)
        {
            Dictionary<int, FacilityDTO> facilityldictionary = new Dictionary<int, FacilityDTO>();
            string facilityIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < facilityDTOList.Count; i++)
            {
                if (facilityDTOList[i].FacilityId == -1 ||
                    facilityldictionary.ContainsKey(facilityDTOList[i].FacilityId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(facilityDTOList[i].FacilityId);
                facilityldictionary.Add(facilityDTOList[i].FacilityId, facilityDTOList[i]);
            }
            facilityIdSet = sb.ToString();
            FacilitySeatLayoutListBL facilitySeatLayoutListBL = new FacilitySeatLayoutListBL(executionContext);
            List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>> facilitySeatLayoutDTOSearchParameters = new List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>>();
            facilitySeatLayoutDTOSearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID_LIST, facilityIdSet.ToString()));
            facilitySeatLayoutDTOSearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                facilitySeatLayoutDTOSearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.IS_ACTIVE, "1"));
            }
            List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList = facilitySeatLayoutListBL.GetFacilitySeatLayoutDTOList(facilitySeatLayoutDTOSearchParameters, sqlTransaction);
            if (facilitySeatLayoutDTOList != null && facilitySeatLayoutDTOList.Any())
            {
                log.LogVariableState("facilitySeatLayoutDTOList", facilitySeatLayoutDTOList);
                foreach (var facilitySeatLayoutDTO in facilitySeatLayoutDTOList)
                {
                    if (facilityldictionary.ContainsKey(facilitySeatLayoutDTO.FacilityId))
                    {
                        if (facilityldictionary[facilitySeatLayoutDTO.FacilityId].FacilitySeatLayoutDTOList == null)
                        {
                            facilityldictionary[facilitySeatLayoutDTO.FacilityId].FacilitySeatLayoutDTOList = new List<FacilitySeatLayoutDTO>();
                        }
                        facilityldictionary[facilitySeatLayoutDTO.FacilityId].FacilitySeatLayoutDTOList.Add(facilitySeatLayoutDTO);
                    }
                }
            }

            //This below snippet will fetches the FacilitySeats based on Facility Id and SiteId
            FacilitySeatsListBL facilitySeatsListBL = new FacilitySeatsListBL(executionContext);
            List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>> facilitySeatsDTOSearchParameters = new List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>>();
            facilitySeatsDTOSearchParameters.Add(new KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>(FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID_LIST, facilityIdSet.ToString()));
            facilitySeatsDTOSearchParameters.Add(new KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>(FacilitySeatsDTO.SearchByFacilitySeatsParameter.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                facilitySeatsDTOSearchParameters.Add(new KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>(FacilitySeatsDTO.SearchByFacilitySeatsParameter.ACTIVE, "Y"));
            }
            List<FacilitySeatsDTO> facilitySeatsDTOList = facilitySeatsListBL.GetFacilitySeatsDTOList(facilitySeatsDTOSearchParameters);
            if (facilitySeatsDTOList != null && facilitySeatsDTOList.Any())
            {
                log.LogVariableState("facilitySeatsDTOList", facilitySeatsDTOList);
                foreach (var facilitySeatsDTO in facilitySeatsDTOList)
                {
                    if (facilityldictionary.ContainsKey(facilitySeatsDTO.FacilityId))
                    {
                        if (facilityldictionary[facilitySeatsDTO.FacilityId].FacilitySeatsDTOList == null)
                        {
                            facilityldictionary[facilitySeatsDTO.FacilityId].FacilitySeatsDTOList = new List<FacilitySeatsDTO>();
                        }
                        facilityldictionary[facilitySeatsDTO.FacilityId].FacilitySeatsDTOList.Add(facilitySeatsDTO);
                    }
                }
            }

            FacilityWaiverListBL facilityWaiverListBL = new FacilityWaiverListBL(executionContext);
            List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>> wsearchParameters = new List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>>();
            wsearchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.FACILITY_ID_LIST, facilityIdSet.ToString()));
            wsearchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                wsearchParameters.Add(new KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>(FacilityWaiverDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<FacilityWaiverDTO> facilityWaiverDTOList = facilityWaiverListBL.GetAllFacilityWaiverList(wsearchParameters, sqlTransaction);
            if (facilityWaiverDTOList != null && facilityWaiverDTOList.Any())
            {
                log.LogVariableState("facilityWaiverDTOList", facilityWaiverDTOList);
                foreach (var facilityWaiverDTO in facilityWaiverDTOList)
                {
                    if (facilityldictionary.ContainsKey(facilityWaiverDTO.FacilityId))
                    {
                        if (facilityldictionary[facilityWaiverDTO.FacilityId].FacilityWaiverDTOList == null)
                        {
                            facilityldictionary[facilityWaiverDTO.FacilityId].FacilityWaiverDTOList = new List<FacilityWaiverDTO>();
                        }
                        facilityldictionary[facilityWaiverDTO.FacilityId].FacilityWaiverDTOList.Add(facilityWaiverDTO);
                    }
                }
            }

            FacilityTablesList facilityTablesListBL = new FacilityTablesList(executionContext);
            List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>> tsearchParameters = new List<KeyValuePair<FacilityTableDTO.SearchByParameters, string>>();
            tsearchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.FACILITY_ID_LIST, facilityIdSet.ToString()));
            tsearchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                tsearchParameters.Add(new KeyValuePair<FacilityTableDTO.SearchByParameters, string>(FacilityTableDTO.SearchByParameters.ISACTIVE, "1"));
            }
            List<FacilityTableDTO> FacilityTableDTOList = facilityTablesListBL.GetAllFacilityTableList(tsearchParameters, sqlTransaction);
            if (FacilityTableDTOList != null && FacilityTableDTOList.Any())
            {
                log.LogVariableState("FacilityTableDTOList", FacilityTableDTOList);
                foreach (var facilityTableDTO in FacilityTableDTOList)
                {
                    if (facilityldictionary.ContainsKey(facilityTableDTO.FacilityId))
                    {
                        if (facilityldictionary[facilityTableDTO.FacilityId].FacilityTableDTOList == null)
                        {
                            facilityldictionary[facilityTableDTO.FacilityId].FacilityTableDTOList = new List<FacilityTableDTO>();
                        }
                        facilityldictionary[facilityTableDTO.FacilityId].FacilityTableDTOList.Add(facilityTableDTO);
                    }
                }
            }

        }

        ///// <summary>
        ///// Gets Facility along with FacilitySeats and FacilitySeatsLayout
        ///// </summary>
        ///// <param name="searchParameters"></param>
        ///// <returns>facilityDTOList</returns>
        //public List<FacilityDTO> GetFacilitySeatsLayoutList(List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters)
        //{
        //    log.LogMethodEntry(searchParameters);
        //    FacilityDataHandler facilityDataHandler = new FacilityDataHandler(null);
        //    List<FacilityDTO> facilityDTOList = facilityDataHandler.GetFacilityDTOList(searchParameters);
        //    if (facilityDTOList != null && facilityDTOList.Count > 0)
        //    {
        //        //This below snippet will fetches the FacilitySeatLayout based on Facility Id and SiteId
        //        FacilitySeatLayoutListBL facilitySeatLayoutListBL = new FacilitySeatLayoutListBL(executionContext);
        //        foreach (FacilityDTO facilityDTO in facilityDTOList)
        //        {
        //            List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>> facilitySeatLayoutDTOSearchParameters = new List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>>();
        //            facilitySeatLayoutDTOSearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID, facilityDTO.FacilityId.ToString()));
        //            facilitySeatLayoutDTOSearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.SITE_ID, executionContext.GetSiteId().ToString()));
        //            //Always the Layout will fetch the Active records - Akshay G
        //            facilitySeatLayoutDTOSearchParameters.Add(new KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>(FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.IS_ACTIVE, "1"));
        //            List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList = facilitySeatLayoutListBL.GetFacilitySeatLayoutDTOList(facilitySeatLayoutDTOSearchParameters);
        //            if (facilitySeatLayoutDTOList != null)
        //            {
        //                facilityDTO.FacilitySeatLayoutDTOList = new List<FacilitySeatLayoutDTO>(facilitySeatLayoutDTOList);
        //            }
        //        }
        //        //This below snippet will fetches the FacilitySeats based on Facility Id and SiteId
        //        FacilitySeatsListBL facilitySeatsListBL = new FacilitySeatsListBL(executionContext);
        //        foreach (FacilityDTO facilityDTO in facilityDTOList)
        //        {
        //            List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>> facilitySeatsDTOSearchParameters = new List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>>();
        //            facilitySeatsDTOSearchParameters.Add(new KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>(FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID, facilityDTO.FacilityId.ToString()));
        //            facilitySeatsDTOSearchParameters.Add(new KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>(FacilitySeatsDTO.SearchByFacilitySeatsParameter.SITE_ID, executionContext.GetSiteId().ToString()));
        //            List<FacilitySeatsDTO> facilitySeatsDTOList = facilitySeatsListBL.GetFacilitySeatsDTOList(facilitySeatsDTOSearchParameters);
        //            if (facilitySeatsDTOList != null)
        //            {
        //                facilityDTO.FacilitySeatsDTOList = new List<FacilitySeatsDTO>(facilitySeatsDTOList);
        //            }
        //        }
        //    }
        //    log.LogMethodExit(facilityDTOList);
        //    return facilityDTOList;
        //}

        /// <summary>
        /// Get facility list with max row and column info
        /// </summary>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public List<FacilityDTO> GetFacilityListWithMaxRowNColumnInfo(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTrx);
            List<FacilityDTO> facilityDTOList = facilityDataHandler.GetFacilityListWithMaxRowNColumnInfo();
            log.LogMethodExit(facilityDTOList);
            return facilityDTOList;
        }

        /// <summary>
        /// This method checks for duplicate Facility Name in CheckInFacility
        /// </summary>
        /// <param name="facilityDTO"></param>
        /// <returns></returns>
        private bool GetFacilityName(FacilityDTO facilityDTO)
        {
            log.LogMethodEntry(facilityDTO);
            List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.FACILITY_NAME, facilityDTO.FacilityName));

            List<FacilityDTO> facilityDTOList = GetFacilityDTOList(searchParameters);
            if (facilityDTOList != null && facilityDTOList.Count > 0)
            {
                string message = MessageContainerList.GetMessage(executionContext, 166, "FacilityName", "\"" + facilityDTO.FacilityName + "\"");
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
            return false;
        }
        /// <summary>
        /// Save and Updated the Facility details
        /// </summary>
        public void SaveUpdateFacilityList()
        {
            log.LogMethodEntry();
            if (facilityDTOList == null ||
                facilityDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            if (facilityDTOList != null && facilityDTOList.Any())
            {
                foreach (FacilityDTO facilityDTO in facilityDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            bool checkDuplicateFacilityName = false;
                            if (facilityDTO.FacilityId < 0)
                            {
                                checkDuplicateFacilityName = GetFacilityName(facilityDTO); //Added this for checking duplicate facility Names on 22 Feb 2019 by Akshay Gulaganji
                            }
                            if (!checkDuplicateFacilityName)
                            {
                                parafaitDBTrx.BeginTransaction();
                                FacilityBL facilityObj = new FacilityBL(executionContext, facilityDTO);
                                facilityObj.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            else
                            {
                                log.Debug("Duplicate Facility Name :" + facilityDTO.FacilityName);
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        public void Delete()
        {
            log.LogMethodEntry();
            if (facilityDTOList == null ||
                facilityDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            if (facilityDTOList != null && facilityDTOList.Any())
            {
                foreach (FacilityDTO facilityDTO in facilityDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            FacilityBL facilityObj = new FacilityBL(executionContext, facilityDTO);
                            facilityObj.Delete(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GetConfiguredFacility
        /// </summary>
        /// <returns> list of FacilityDTO</returns>
        public List<FacilityDTO> GetConfiguredFacility(string macAddress, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(macAddress);
            FacilityDataHandler facilityDataHandler = new FacilityDataHandler(sqlTrx);
            List<FacilityDTO> facilityDTOList = facilityDataHandler.GetConfiguredFacility(macAddress);
            log.LogMethodExit(facilityDTOList);
            return facilityDTOList;
        }

        /// <summary>
        /// Gets a list of Facilities based on input parameters such as InterfaceTypeValue, and InterfaceNameValue
        /// This function written w.r.t ClubSpeed integration. There is already a function exposed to get a list of facilities. However, the below function takes input params which will retrive values from the tables Lookups and lookupValues
        /// Once the lookup values are retrieved, it will call the existing GetFacilityDTOList().
        /// </summary>
        /// <param name="interfaceTypeValue"></param>
        /// <param name="interfaceNameValue"></param>
        /// <returns></returns>
        public List<FacilityDTO> GetFacilities(string interfaceTypeValue, string interfaceNameValue)
        {
            log.LogMethodEntry();
            int interfaceTypeId;
            int interfaceNameId;
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INTERFACE_TYPE")); // lookupName 
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, interfaceTypeValue));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    interfaceTypeId = lookupValuesDTOList.First().LookupValueId;// This will be lookupValueId for interfaceType
                    lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INTERFACE_NAME"));// lookupName 
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, interfaceNameValue));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        interfaceNameId = lookupValuesDTOList.First().LookupValueId;// This will be lookupValueId for interfaceName
                        FacilityList facilityListBL = new FacilityList(executionContext);
                        List<KeyValuePair<FacilityDTO.SearchByParameters, string>> facilitySearchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                        facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.INTERFACE_TYPE, interfaceTypeId.ToString()));
                        facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.INTERFACE_NAME, interfaceNameId.ToString()));
                        facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        facilitySearchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                        facilityDTOList = facilityListBL.GetFacilityDTOList(facilitySearchParameters);
                    }
                    else
                    {
                        log.Debug(string.Format(" InterfaceName - {0} - does not exists", interfaceNameValue));
                    }
                }
                else
                {
                    log.Debug(string.Format(" IntefaceType - {0} - does not exists", interfaceTypeValue));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityDTOList);
            return facilityDTOList;
        }
    }
}


