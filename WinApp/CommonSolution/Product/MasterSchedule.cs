/* Project Name - MasterSchedule 
* Description  - Business call object of the MasterSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.70        18-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.70        27-Jun-2019    Akshay Gulaganji     Added DeleteAttractionMasterSchedule() and DeleteAttractionMasterScheduleList() methods
*2.70        10-Jul-2019    Akshay Gulaganji     modified DeleteAttractionMasterScheduleList() method
*2.70.2        18-Oct-2019    Guru S A            Sort order change 
*2.80.0      21-02-2020     Girish Kundar        Modified : 3 tier Changes for REST API
*2.130.1     01-Dec-2021    Nitin Pai            Create a separate Facility Map for each schedule to avoid 
*                                                reference issue when promo and other schedule details are calculated.
*2.130.4     17-Feb-2022    Nitin Pai            Creating Attraction Schedule Container - Adding a default constructor and last update time method   
*2.130.10    13-Sep-2022    Nitin Pai            Cloning the product before adding it to the schedule. 
*                                                Doing this to avoid the same object being modified during promotional price calculation.
*********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Product
{

    /// <summary>
    /// Attraction Schedules
    /// </summary>
    public class MasterScheduleBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MasterScheduleDTO masterScheduleDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of MasterScheduleBL class
        /// </summary>
        public MasterScheduleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            masterScheduleDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the masterSchedule id as the parameter
        /// Would fetch the masterSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public MasterScheduleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null, bool buildChildDetails = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            masterScheduleDTO = masterScheduleDataHandler.GetMasterScheduleDTO(id);
            if (buildChildDetails && masterScheduleDTO != null && masterScheduleDTO.MasterScheduleId != -1)
            {
                LoadFacilityMapDTOList();
                LoadScheduleDetails();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates masterScheduleBL object using the masterScheduleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="masterScheduleDTO">masterScheduleDTO object</param>
        public MasterScheduleBL(ExecutionContext executionContext, MasterScheduleDTO masterScheduleDTO, bool buildChildDetails = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, masterScheduleDTO, buildChildDetails);
            this.masterScheduleDTO = masterScheduleDTO;
            if (buildChildDetails && masterScheduleDTO != null && masterScheduleDTO.MasterScheduleId != -1)
            {
                LoadFacilityMapDTOList();
                LoadScheduleDetails();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// loads facilityMapDTO
        /// </summary>
        private void LoadFacilityMapDTOList()
        {
            log.LogMethodEntry();
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.MASTER_SCHEDULE_ID, masterScheduleDTO.MasterScheduleId.ToString()));
            searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParameters, true);
            this.masterScheduleDTO.FacilityMapDTOList = facilityMapDTOList;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Loads Schedule Details
        /// </summary>
        private void LoadScheduleDetails()
        {
            log.LogMethodEntry();
            SchedulesListBL schedulesListBL = new SchedulesListBL(executionContext);
            List<KeyValuePair<SchedulesDTO.SearchByParameters, string>> searchPara = new List<KeyValuePair<SchedulesDTO.SearchByParameters, string>>();
            searchPara.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.MASTER_SCHEDULE_ID, masterScheduleDTO.MasterScheduleId.ToString()));
            searchPara.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchPara.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            List<SchedulesDTO> schedulesDTOList = schedulesListBL.GetScheduleDTOList(searchPara, true);
            this.masterScheduleDTO.SchedulesDTOList = schedulesDTOList;
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(masterScheduleDTO.MasterScheduleName))
            {
                validationErrorList.Add(new ValidationError("MasterSchedule", "MasterScheduleName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Master Schedule Name"))));
            }

            if (!string.IsNullOrWhiteSpace(masterScheduleDTO.MasterScheduleName) && masterScheduleDTO.MasterScheduleName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("MasterSchedule", "MasterScheduleName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Master Schedule Name"), 50)));
            }

            if (masterScheduleDTO.SchedulesDTOList != null && masterScheduleDTO.SchedulesDTOList.Count > 0)
            {
                foreach (var schedulesDTO in masterScheduleDTO.SchedulesDTOList)
                {
                    if (schedulesDTO.IsChanged)
                    {
                        SchedulesBL schedulesBL = new SchedulesBL(executionContext, schedulesDTO);
                        validationErrorList.AddRange(schedulesBL.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the masterSchedule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (masterScheduleDTO.IsChangedRecursive == false
                && masterScheduleDTO.MasterScheduleId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (masterScheduleDTO.MasterScheduleId < 0)
            {
                masterScheduleDTO = masterScheduleDataHandler.InsertMasterSchedule(masterScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(masterScheduleDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("AttractionMasterSchedule", masterScheduleDTO.Guid, sqlTransaction);
                }
                masterScheduleDTO.AcceptChanges();
            }
            else
            {
                if (masterScheduleDTO.IsChanged)
                {
                    masterScheduleDTO = masterScheduleDataHandler.UpdateMasterSchedule(masterScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(masterScheduleDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("AttractionMasterSchedule", masterScheduleDTO.Guid, sqlTransaction);
                    }
                    masterScheduleDTO.AcceptChanges();
                }
            }
            SaveSchedules(sqlTransaction);
            log.LogMethodExit();
        }

        private void SaveSchedules(SqlTransaction sqlTransaction)
        {
            if (masterScheduleDTO.SchedulesDTOList != null &&
                masterScheduleDTO.SchedulesDTOList.Any())
            {
                List<SchedulesDTO> updatedSchedulesDTOList = new List<SchedulesDTO>();
                foreach (var schedulesDTO in masterScheduleDTO.SchedulesDTOList)
                {
                    if (schedulesDTO.MasterScheduleId != masterScheduleDTO.MasterScheduleId)
                    {
                        schedulesDTO.MasterScheduleId = masterScheduleDTO.MasterScheduleId;
                    }
                    if (schedulesDTO.IsChanged)
                    {
                        updatedSchedulesDTOList.Add(schedulesDTO);
                    }
                }
                if (updatedSchedulesDTOList.Any())
                {
                    SchedulesListBL schedulesListBL = new SchedulesListBL(executionContext, updatedSchedulesDTOList);
                    schedulesListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Deletes the AttractionMasterSchedule based on masterScheduleId
        /// </summary>   
        /// <param name="masterScheduleId">masterScheduleId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void DeleteAttractionMasterSchedule(int masterScheduleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(masterScheduleId, sqlTransaction);
            try
            {
                MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
                masterScheduleDataHandler.DeleteMasterSchedule(masterScheduleId);
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

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MasterScheduleDTO MasterScheduleDTO
        {
            get
            {
                return masterScheduleDTO;
            }
        }

        /// <summary>
        /// Gets the Eligible Schedules based on parameters
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="facilityMapId"></param>
        /// <returns></returns>
        public List<ScheduleDetailsDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, string productType)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId);
            scheduleDate = scheduleDate.Date;
            log.LogVariableState("ScheduleDate: After date conv", scheduleDate);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            if (this.masterScheduleDTO != null && this.masterScheduleDTO.SchedulesDTOList != null && this.masterScheduleDTO.SchedulesDTOList.Count > 0)
            {
                List<SchedulesDTO> eligibleFixedSchedules = this.masterScheduleDTO.SchedulesDTOList.Where(sch => sch.FixedSchedule == true && sch.ScheduleTime >= fromTime && sch.ScheduleTime <= toTime).ToList();
                List<SchedulesDTO> eligibleFlexSchedules = this.masterScheduleDTO.SchedulesDTOList.Where(sch => sch.FixedSchedule == false && ((sch.ScheduleTime >= fromTime && sch.ScheduleTime <= toTime)
                                                                                                                                               || (fromTime >= sch.ScheduleTime && fromTime < sch.ScheduleToTime))).ToList();
                List<SchedulesDTO> eligibleSchedules = eligibleFixedSchedules;
                if (eligibleFlexSchedules != null && eligibleFlexSchedules.Count > 0)
                {
                    if (eligibleSchedules != null && eligibleSchedules.Count > 0)
                    {
                        eligibleSchedules.AddRange(eligibleFlexSchedules);
                    }
                    else
                        eligibleSchedules = eligibleFlexSchedules;
                }
                if (eligibleSchedules != null && eligibleSchedules.Count > 0)
                {
                    //eligibleSchedules = eligibleSchedules.OrderBy(sch => sch.ScheduleFromDate.)
                    if (facilityMapId > -1)
                    {
                        if (this.masterScheduleDTO.FacilityMapDTOList != null)
                        {
                            FacilityMapDTO facDTO = this.masterScheduleDTO.FacilityMapDTOList.Find(fac => fac.FacilityMapId == facilityMapId
                                                                                                 && (fac.SiteId == executionContext.GetSiteId() || executionContext.GetSiteId() == -1));
                            if (facDTO != null)
                            {
                                FacilityMapDTO facilityMapDTO = new FacilityMapDTO(
                                facDTO.FacilityMapId,
                                facDTO.FacilityMapName,
                                facDTO.MasterScheduleId,
                                facDTO.CancellationProductId,
                                facDTO.GraceTime,
                                facDTO.IsActive,
                                facDTO.Guid,
                                facDTO.CreatedBy,
                                facDTO.CreationDate,
                                facDTO.LastUpdatedBy,
                                facDTO.LastUpdatedDate,
                                facDTO.SiteId,
                                facDTO.SynchStatus,
                                facDTO.MasterEntityId,
                                facDTO.FacilityCapacity
                                );

                                facilityMapDTO.FacilityMapDetailsDTOList = facDTO.FacilityMapDetailsDTOList;
                                List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO = new List<ProductsAllowedInFacilityMapDTO>();
                                productsAllowedInFacilityMapDTO.AddRange(facDTO.ProductsAllowedInFacilityDTOList.ToList());
                                facilityMapDTO.ProductsAllowedInFacilityDTOList = productsAllowedInFacilityMapDTO;

                                //FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facDTO.FacilityMapId, true);
                                //scheduleDetailsDTOList = new List<ScheduleDetailsDTO>(); 
                                foreach (SchedulesDTO schedulesDTO in eligibleSchedules)
                                {
                                    String facilityMapDTOJSON = JsonConvert.SerializeObject(facilityMapDTO);
                                    FacilityMapDTO cloneFacilityMapDTO = JsonConvert.DeserializeObject<FacilityMapDTO>(facilityMapDTOJSON);

                                    SchedulesBL schedulesBL = new SchedulesBL(executionContext, schedulesDTO);
                                    scheduleDetailsDTOList.Add(schedulesBL.BuildScheduleDetails(cloneFacilityMapDTO, scheduleDate));
                                }
                            }
                        }
                    }
                    else
                    {
                        // scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
                        foreach (SchedulesDTO schedulesDTO in eligibleSchedules)
                        {
                            if (this.masterScheduleDTO.FacilityMapDTOList != null)
                            {
                                SchedulesBL schedulesBL = new SchedulesBL(executionContext, schedulesDTO);
                                foreach (FacilityMapDTO facDTO in this.masterScheduleDTO.FacilityMapDTOList)
                                {
                                    FacilityMapDTO facilityMapDTO = new FacilityMapDTO(
                                        facDTO.FacilityMapId,
                                        facDTO.FacilityMapName,
                                        facDTO.MasterScheduleId,
                                        facDTO.CancellationProductId,
                                        facDTO.GraceTime,
                                        facDTO.IsActive,
                                        facDTO.Guid,
                                        facDTO.CreatedBy,
                                        facDTO.CreationDate,
                                        facDTO.LastUpdatedBy,
                                        facDTO.LastUpdatedDate,
                                        facDTO.SiteId,
                                        facDTO.SynchStatus,
                                        facDTO.MasterEntityId,
                                        facDTO.FacilityCapacity
                                        );

                                    facilityMapDTO.FacilityMapDetailsDTOList = facDTO.FacilityMapDetailsDTOList;
                                    List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO = new List<ProductsAllowedInFacilityMapDTO>();
                                    productsAllowedInFacilityMapDTO.AddRange(facDTO.ProductsAllowedInFacilityDTOList.ToList());
                                    facilityMapDTO.ProductsAllowedInFacilityDTOList = productsAllowedInFacilityMapDTO;

                                    //FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facDTO.FacilityMapId, true);
                                    // select only the facility maps which have the selected product type
                                    if (!String.IsNullOrEmpty(productType) &&
                                        (facDTO.ProductsAllowedInFacilityDTOList == null ||
                                        facDTO.ProductsAllowedInFacilityDTOList.FirstOrDefault(x => x.ProductsDTO.ProductType == productType) == null))
                                    {
                                        continue;
                                    }

                                    String facilityMapDTOJSON = JsonConvert.SerializeObject(facilityMapDTO);
                                    FacilityMapDTO cloneFacilityMapDTO = JsonConvert.DeserializeObject<FacilityMapDTO>(facilityMapDTOJSON);

                                    scheduleDetailsDTOList.Add(schedulesBL.BuildScheduleDetails(cloneFacilityMapDTO, scheduleDate));
                                }
                            }
                        }

                    }
                }
                if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
                {
                    scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleFromDate).ToList();
                }
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2219, this.MasterScheduleDTO.MasterScheduleName));
            }
            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Any())
            {
                scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleFromDate).ThenBy(sch => sch.FacilityMapName).ToList();
            }
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        /// <summary>
        /// Gets the Eligible Schedules based on parameters
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="facilityId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<ScheduleDetailsDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, int productId, String productType)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId);
            scheduleDate = scheduleDate.Date;
            log.LogVariableState("ScheduleDate: After date conv", scheduleDate);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            if (this.masterScheduleDTO != null && this.masterScheduleDTO.SchedulesDTOList != null && this.masterScheduleDTO.SchedulesDTOList.Count > 0)
            {
                if (productId > -1)
                {
                    List<FacilityMapDTO> facilitiesAllowingTheProduct = null;
                    Products products = new Products(productId);
                    //int productFacilityMapId = products.GetProductsDTO.FacilityMapId; //For Attractions
                    if (facilityMapId > -1)
                    {
                        if (this.masterScheduleDTO.FacilityMapDTOList != null)
                        {
                            facilitiesAllowingTheProduct = this.masterScheduleDTO.FacilityMapDTOList.Where(fac => fac.IsActive == true
                                                                                                               &&
                                                                                                               ((fac.ProductsAllowedInFacilityDTOList != null
                                                                                                                  && fac.ProductsAllowedInFacilityDTOList.Exists(prod => prod.ProductsId == productId && prod.IsActive == true))
                                                                                                               // || (productFacilityMapId > -1 && fac.FacilityMapId == productFacilityMapId)
                                                                                                               )
                                                                                                               && fac.FacilityMapId == facilityMapId
                                                                                                        ).ToList();
                        }
                    }
                    else
                    {
                        if (this.masterScheduleDTO.FacilityMapDTOList != null)
                        {
                            facilitiesAllowingTheProduct = this.masterScheduleDTO.FacilityMapDTOList.Where(fac => fac.IsActive == true
                                                                                                               && ((fac.ProductsAllowedInFacilityDTOList != null && fac.ProductsAllowedInFacilityDTOList.Exists(prod => prod.ProductsId == productId && prod.IsActive == true))
                                                                                                                    //|| (productFacilityMapId > -1 && fac.FacilityMapId == productFacilityMapId)
                                                                                                                    )).ToList();
                        }
                    }
                    if (facilitiesAllowingTheProduct != null && facilitiesAllowingTheProduct.Count > 0)
                    {
                        foreach (FacilityMapDTO facDTO in facilitiesAllowingTheProduct)
                        {
                            List<ScheduleDetailsDTO> facScheduleDetailsDTOList = GetEligibleSchedules(scheduleDate, fromTime, toTime, facDTO.FacilityMapId, productType);
                            if (facScheduleDetailsDTOList != null && facScheduleDetailsDTOList.Count > 0)
                            {
                                //if (scheduleDetailsDTOList == null)
                                //{
                                //    scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
                                //}
                                foreach (ScheduleDetailsDTO scheduleDetailsDTO in facScheduleDetailsDTOList)
                                {
                                    scheduleDetailsDTO.Price = (double)products.GetProductsDTO.Price;
                                    scheduleDetailsDTO.ProductId = products.GetProductsDTO.ProductId;
                                    scheduleDetailsDTO.ProductName = products.GetProductsDTO.ProductName;
                                    //scheduleDetailsDTO.ProductLevelUnits = products.GetProductsDTO.AvailableUnits;
                                    //if (scheduleDetailsDTO.ProductLevelUnits > 0)
                                    //{
                                    //    if (scheduleDetailsDTO.RuleUnits == null)
                                    //    {
                                    //        if (scheduleDetailsDTO.AvailableUnits != null && scheduleDetailsDTO.AvailableUnits > scheduleDetailsDTO.ProductLevelUnits)
                                    //        {//set product level unit as availavle unit
                                    //            scheduleDetailsDTO.TotalUnits = (int)scheduleDetailsDTO.ProductLevelUnits;
                                    //        }
                                    //    }
                                    //}
                                }
                                scheduleDetailsDTOList.AddRange(facScheduleDetailsDTOList);
                            }
                        }
                    }
                }
                else
                {
                    List<ScheduleDetailsDTO> facScheduleDetailsDTOList = GetEligibleSchedules(scheduleDate, fromTime, toTime, facilityMapId, productType);
                    if (facScheduleDetailsDTOList != null && facScheduleDetailsDTOList.Count > 0)
                    {
                        //if (scheduleDetailsDTOList == null)
                        //{
                        //    scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
                        //}
                        scheduleDetailsDTOList.AddRange(facScheduleDetailsDTOList);
                    }
                }
            }
            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
            {
                scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleFromDate).ToList();
            }
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        //public List<ScheduleDetailsDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityId)
        //{
        //    log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityId);
        //    scheduleDate = scheduleDate.Date;
        //    log.LogVariableState("ScheduleDate: After date conv", scheduleDate);
        //    List<ScheduleDetailsDTO> scheduleDetailsDTOList = null;
        //    if (this.masterScheduleDTO != null && this.masterScheduleDTO.SchedulesDTOList != null && this.masterScheduleDTO.SchedulesDTOList.Count > 0)
        //    {
        //        List<SchedulesDTO> eligibleFixedSchedules = this.masterScheduleDTO.SchedulesDTOList.Where(sch => sch.FixedSchedule == true && sch.ScheduleTime >= fromTime && sch.ScheduleTime <= toTime).ToList();
        //        List<SchedulesDTO> eligibleFlexSchedules = this.masterScheduleDTO.SchedulesDTOList.Where(sch => sch.FixedSchedule == false && ((sch.ScheduleTime >= fromTime && sch.ScheduleTime <= toTime)
        //                                                                                                                                       || (fromTime >= sch.ScheduleTime  && fromTime <= sch.ScheduleToTime))).ToList();
        //        List<SchedulesDTO> eligibleSchedules = eligibleFixedSchedules;
        //        if (eligibleFlexSchedules != null && eligibleFlexSchedules.Count > 0)
        //        {
        //            if (eligibleSchedules != null && eligibleSchedules.Count > 0)
        //            {
        //                eligibleSchedules.AddRange(eligibleFlexSchedules);
        //            }
        //            else
        //                eligibleSchedules = eligibleFlexSchedules;
        //        }
        //        if (eligibleSchedules != null && eligibleSchedules.Count > 0)
        //        {
        //            //eligibleSchedules = eligibleSchedules.OrderBy(sch => sch.ScheduleFromDate.)
        //            if (facilityId > -1)
        //            {
        //                if (this.masterScheduleDTO.FacilityDTOList != null)
        //                {
        //                    FacilityDTO facDTO = this.masterScheduleDTO.FacilityDTOList.Find(fac => fac.FacilityId == facilityId
        //                                                                                         && (fac.SiteId == executionContext.GetSiteId() || executionContext.GetSiteId() == -1));
        //                    if (facDTO != null)
        //                    {
        //                        scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
        //                        foreach (SchedulesDTO schedulesDTO in eligibleSchedules)
        //                        {
        //                            scheduleDetailsDTOList.Add(BuildScheduleDetails(schedulesDTO, facDTO, scheduleDate));
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
        //                foreach (SchedulesDTO schedulesDTO in eligibleSchedules)
        //                {
        //                    if (this.masterScheduleDTO.FacilityDTOList != null)
        //                    {
        //                        foreach (FacilityDTO facDTO in this.masterScheduleDTO.FacilityDTOList)
        //                        {
        //                            scheduleDetailsDTOList.Add(BuildScheduleDetails(schedulesDTO, facDTO, scheduleDate));
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        if(scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
        //        {
        //            scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleTime).ToList();
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(MessageContainerList.GetMessage(executionContext, "Schedule details are not loaded for the master schedule"));
        //    }
        //    log.LogMethodExit(scheduleDetailsDTOList);
        //    return scheduleDetailsDTOList;
        //}

        //public List<ScheduleDetailsDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityId, int productId)
        //{
        //    log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityId);
        //    scheduleDate = scheduleDate.Date;
        //    log.LogVariableState("ScheduleDate: After date conv", scheduleDate);
        //    List<ScheduleDetailsDTO> scheduleDetailsDTOList = null;
        //    if (this.masterScheduleDTO != null && this.masterScheduleDTO.SchedulesDTOList != null && this.masterScheduleDTO.SchedulesDTOList.Count > 0)
        //    {
        //        if (productId > -1)
        //        {
        //            List<FacilityDTO> facilitiesAllowingTheProduct = null;
        //            Products products = new Products(productId);
        //            if (facilityId > -1)
        //            {
        //                if (this.masterScheduleDTO.FacilityDTOList != null)
        //                {
        //                    facilitiesAllowingTheProduct = this.masterScheduleDTO.FacilityDTOList.Where(fac => fac.ActiveFlag == true
        //                                                                                                       && fac.AllowedProductsDTOList != null
        //                                                                                                       && fac.AllowedProductsDTOList.Exists(prod => prod.ProductsId == productId)
        //                                                                                                       && fac.FacilityId == facilityId
        //                                                                                                ).ToList();
        //                }
        //            }
        //            else
        //            {
        //                if (this.masterScheduleDTO.FacilityDTOList != null)
        //                {
        //                    facilitiesAllowingTheProduct = this.masterScheduleDTO.FacilityDTOList.Where(fac => fac.ActiveFlag == true
        //                                                                                                       && fac.AllowedProductsDTOList != null
        //                                                                                                       && fac.AllowedProductsDTOList.Exists(prod => prod.ProductsId == productId)
        //                                                                                                ).ToList();
        //                }
        //            }
        //            if (facilitiesAllowingTheProduct != null && facilitiesAllowingTheProduct.Count > 0)
        //            {
        //                foreach (FacilityDTO facDTO in facilitiesAllowingTheProduct)
        //                {
        //                    List<ScheduleDetailsDTO> facScheduleDetailsDTOList = GetEligibleSchedules(scheduleDate, fromTime, toTime, facDTO.FacilityId);
        //                    if (facScheduleDetailsDTOList != null && facScheduleDetailsDTOList.Count > 0)
        //                    {
        //                        if (scheduleDetailsDTOList == null)
        //                        {
        //                            scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
        //                        }
        //                        foreach (ScheduleDetailsDTO scheduleDetailsDTO in facScheduleDetailsDTOList)
        //                        {
        //                            scheduleDetailsDTO.Price = (double)products.GetProductsDTO.Price;
        //                            scheduleDetailsDTO.ProductId = products.GetProductsDTO.ProductId;
        //                            scheduleDetailsDTO.ProductName = products.GetProductsDTO.ProductName;
        //                            scheduleDetailsDTO.ProductLevelUnits = products.GetProductsDTO.AvailableUnits;
        //                            if(scheduleDetailsDTO.ProductLevelUnits > 0)
        //                            {
        //                                if(scheduleDetailsDTO.RuleUnits == null)
        //                                {
        //                                    if(scheduleDetailsDTO.AvailableUnits != null && scheduleDetailsDTO.AvailableUnits > scheduleDetailsDTO.ProductLevelUnits)
        //                                    {//set product level unit as availavle unit
        //                                        scheduleDetailsDTO.TotalUnits = (int)scheduleDetailsDTO.ProductLevelUnits;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        scheduleDetailsDTOList.AddRange(facScheduleDetailsDTOList);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            List<ScheduleDetailsDTO> facScheduleDetailsDTOList = GetEligibleSchedules(scheduleDate, fromTime, toTime, facilityId);
        //            if (facScheduleDetailsDTOList != null && facScheduleDetailsDTOList.Count > 0)
        //            {
        //                if (scheduleDetailsDTOList == null)
        //                {
        //                    scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
        //                } 
        //                scheduleDetailsDTOList.AddRange(facScheduleDetailsDTOList);
        //            }
        //        } 
        //    }
        //    if(scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
        //    {
        //        scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleTime).ToList();
        //    }
        //    log.LogMethodExit(scheduleDetailsDTOList);
        //    return scheduleDetailsDTOList;
        //}
        //private ScheduleDetailsDTO BuildScheduleDetails(SchedulesDTO schedulesDTO, VirtualFacilityDTO facDTO, DateTime scheduleDate)
        //{
        //    log.LogMethodEntry();
        //    ScheduleDetailsDTO scheduleDetailsDTO = null;
        //    if (schedulesDTO.ScheduleRulesDTOList != null && schedulesDTO.ScheduleRulesDTOList.Count > 0) //Facility has rules
        //    {
        //        List<ScheduleRulesDTO> facilityScheduleRulesDTOList = schedulesDTO.ScheduleRulesDTOList.Where(sRule => sRule.VirtualFacilityId == facDTO.VirtualFacilityId
        //                                                                                                            && (
        //                                                                                                                (sRule.FromDate != null && sRule.ToDate != null && scheduleDate >= sRule.FromDate && scheduleDate <= ((DateTime)sRule.ToDate).AddDays(1))
        //                                                                                                                || sRule.Day != null && sRule.Day == (int)scheduleDate.DayOfWeek
        //                                                                                                                || sRule.Day != null && sRule.Day == scheduleDate.Day
        //                                                                                                                )
        //                                                                                                            ).ToList();
        //        if (facilityScheduleRulesDTOList != null && facilityScheduleRulesDTOList.Count > 0) //Foiund applicable rule info. User first entry
        //        {

        //            scheduleDetailsDTO = new ScheduleDetailsDTO(facDTO.VirtualFacilityId, facDTO.VirtualFacilityName, masterScheduleDTO.MasterScheduleId, masterScheduleDTO.MasterScheduleName,
        //                                                        schedulesDTO.ScheduleId, schedulesDTO.ScheduleName, scheduleDate.AddHours((double)schedulesDTO.ScheduleTime), schedulesDTO.ScheduleTime,
        //                                                        schedulesDTO.ScheduleToTime, schedulesDTO.FixedSchedule, schedulesDTO.AttractionPlayId,
        //                                                        schedulesDTO.AttractionPlayName, -1, "", null, facDTO.FacilityCapacity, 
        //                                                        facilityScheduleRulesDTOList[0].Units, (facilityScheduleRulesDTOList[0].Units != null ? (int)facilityScheduleRulesDTOList[0].Units : 0),
        //                                                        null, facilityScheduleRulesDTOList[0].Units, null, schedulesDTO.AttractionPlayExpiryDate, -1, -1, null, facilityScheduleRulesDTOList[0].SiteId, schedulesDTO.AttractionPlayPrice, facDTO);
        //            log.LogMethodExit(scheduleDetailsDTO);
        //            return scheduleDetailsDTO;
        //        }
        //    }
        //    /*int virtualFacilityId, string virtualFacilityName, int masterScheduleId, string masterScheduleName, int scheduleId, string scheduleName,
        //    DateTime scheduleTime, decimal scheduleFromTime, decimal scheduleToTime, bool fixedSchedule, int attractionPlayId, string attractionPlayName, 
        //    int productId, string productName, 
        //    double? price, int? facilityCapacity, int? ruleUnits, 
        //    //int? productLevelUnits, 
        //    int totalUnits, int? bookedUnits, int? availableUnits, int? desiredUnits, DateTime? expiryDate, int categoryId,
        //    int promotionId, int? seats, int siteId, double? attractionPlayPrice, VirtualFacilityDTO virtualFacilityDTO*/
        //    //No rules for the facility 
        //    scheduleDetailsDTO = new ScheduleDetailsDTO(facDTO.VirtualFacilityId, facDTO.VirtualFacilityName, masterScheduleDTO.MasterScheduleId, masterScheduleDTO.MasterScheduleName,
        //                                                schedulesDTO.ScheduleId, schedulesDTO.ScheduleName, scheduleDate.AddHours((double)schedulesDTO.ScheduleTime), schedulesDTO.ScheduleTime,
        //                                                schedulesDTO.ScheduleToTime, schedulesDTO.FixedSchedule, schedulesDTO.AttractionPlayId,
        //                                                schedulesDTO.AttractionPlayName, -1, "", null, facDTO.FacilityCapacity, null, (facDTO.FacilityCapacity != null ? (int)facDTO.FacilityCapacity : 0),
        //                                                null, facDTO.FacilityCapacity, null, schedulesDTO.AttractionPlayExpiryDate, -1, -1, null, schedulesDTO.SiteId, schedulesDTO.AttractionPlayPrice, facDTO);


        //    log.LogMethodExit(scheduleDetailsDTO);
        //    return scheduleDetailsDTO;
        //}
    }


    /// <summary>
    /// Manages the list of Attraction Schedules
    /// </summary>
    public class MasterScheduleList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<MasterScheduleDTO> masterScheduleDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public MasterScheduleList()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public MasterScheduleList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor to initialize masterScheduleDTO and executionContext
        /// </summary>
        /// <param name="masterScheduleDTO"></param>
        /// <param name="executionContext"></param>
        public MasterScheduleList(ExecutionContext executionContext, List<MasterScheduleDTO> masterScheduleDTO)
        {
            log.LogMethodEntry(masterScheduleDTO, executionContext);
            this.executionContext = executionContext;
            this.masterScheduleDTOList = masterScheduleDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetMasterScheduleDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<MasterScheduleDTO></returns>
        public List<MasterScheduleDTO> GetMasterScheduleDTOList(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters,
                                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            List<MasterScheduleDTO> returnValue = masterScheduleDataHandler.GetMasterScheduleDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets MasterScheduleDTOList along with its child SchedulesDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="productId"></param>
        /// <returns>masterScheduleDTOList</returns>
        public List<MasterScheduleDTO> GetMasterScheduleDTOsList(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters,
               bool loadChildActiveRecords = false, bool loadChildRecord = false, int facilityMapId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTransaction);
            List<MasterScheduleDTO> masterScheduleDTOList = masterScheduleDataHandler.GetMasterScheduleDTOList(searchParameters);
            if (masterScheduleDTOList != null && masterScheduleDTOList.Count != 0)
            {
                if (loadChildRecord)
                {
                    foreach (MasterScheduleDTO masterScheduleDTO in masterScheduleDTOList)
                    {
                        List<KeyValuePair<SchedulesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<SchedulesDTO.SearchByParameters, string>>();
                        searchByParameters.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.MASTER_SCHEDULE_ID, masterScheduleDTO.MasterScheduleId.ToString()));

                        if (loadChildActiveRecords)
                        {
                            searchByParameters.Add(new KeyValuePair<SchedulesDTO.SearchByParameters, string>(SchedulesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                        }
                        SchedulesListBL schedulesListBL = new SchedulesListBL(executionContext);
                        masterScheduleDTO.SchedulesDTOList = schedulesListBL.GetScheduleDTOList(searchByParameters, loadChildRecord, facilityMapId);
                        masterScheduleDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodEntry(masterScheduleDTOList);
            return masterScheduleDTOList;
        }

        //public void HasValidSchedule(int facilityId, int productId, SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry(facilityId, productId, sqlTrx);
        //    MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTrx);
        //    bool hasValidSchedule = false;
        //    hasValidSchedule = masterScheduleDataHandler.HasValidSchedule(facilityId, productId);
        //    if (hasValidSchedule == false)
        //    {
        //        string msg = "";
        //        if (facilityId > -1 && productId > -1)
        //        {
        //            msg = MessageContainerList.GetMessage(executionContext, "No valid schedules found for the facility and product combination");
        //        }
        //        else if (facilityId > -1 && productId == -1)
        //        {
        //            msg = MessageContainerList.GetMessage(executionContext, "No valid schedules found for the facility");
        //        }

        //        else if (facilityId == -1 && productId > -1)
        //        {
        //            msg = MessageContainerList.GetMessage(executionContext, "No valid schedules found for the product");
        //        }
        //        else
        //        {
        //            msg = MessageContainerList.GetMessage(executionContext, "Need facility or product as input");
        //        }
        //        throw new ValidationException(msg);
        //    }
        //    log.LogMethodExit();
        //}


        public void HasValidSchedule(int facilityMapId, int productId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(facilityMapId, productId, sqlTrx);
            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(sqlTrx);
            bool hasValidSchedule = false;
            hasValidSchedule = masterScheduleDataHandler.HasValidSchedule(facilityMapId, productId);
            if (hasValidSchedule == false)
            {
                string msg = "";
                if (facilityMapId > -1 && productId > -1)
                {
                    msg = MessageContainerList.GetMessage(executionContext, "No valid schedules found for the facility map and product combination");
                }
                else if (facilityMapId > -1 && productId == -1)
                {
                    msg = MessageContainerList.GetMessage(executionContext, "No valid schedules found for the facility map");
                }

                else if (facilityMapId == -1 && productId > -1)
                {
                    msg = MessageContainerList.GetMessage(executionContext, "No valid schedules found for the product");
                }
                else
                {
                    msg = MessageContainerList.GetMessage(executionContext, "Need facility or product as input");
                }
                throw new ValidationException(msg);
            }
            log.LogMethodExit();
        }

     


        /// <summary>
        /// Gets All Master Schedule List
        /// </summary>
        /// <returns></returns>
        public List<MasterScheduleBL> GetAllMasterScheduleBLList()
        {
            log.LogMethodEntry();
            List<MasterScheduleBL> masterScheduleBLList = null;
            List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<MasterScheduleDTO> masterScheduleDTOList = GetMasterScheduleDTOList(searchParameters);
            if (masterScheduleDTOList != null && masterScheduleDTOList.Count > 0)
            {
                masterScheduleBLList = new List<MasterScheduleBL>();
                bool buildChildDetails = true;
                foreach (MasterScheduleDTO masterScheduleDTO in masterScheduleDTOList)
                {
                    MasterScheduleBL masterScheduleBL = new MasterScheduleBL(executionContext, masterScheduleDTO, buildChildDetails);
                    if (masterScheduleBL.MasterScheduleDTO.SchedulesDTOList != null && masterScheduleBL.MasterScheduleDTO.SchedulesDTOList.Any())
                        masterScheduleBLList.Add(masterScheduleBL);
                    else
                        log.LogVariableState("No Schedule found for ", masterScheduleBL.MasterScheduleDTO.MasterScheduleName);
                }
            }

            log.LogMethodExit(masterScheduleBLList);
            return masterScheduleBLList;
        }

        /// <summary>
        /// Gets the Eligible Schedules
        /// </summary>
        /// <param name="masterScheduleBLList"></param>
        /// <param name="scheduleDate"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="facilityMapId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<ScheduleDetailsDTO> GetEligibleSchedules(List<MasterScheduleBL> masterScheduleBLList, DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, int productId = -1,
            string productType = "")
        {
            log.LogMethodEntry(masterScheduleBLList, scheduleDate, fromTime, toTime, facilityMapId, productId);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            if (masterScheduleBLList != null)
            {
                if (productId == -1)
                {
                    foreach (MasterScheduleBL masterScheduleBL in masterScheduleBLList)
                    {
                        List<ScheduleDetailsDTO> scheduleDetailsListFromMaster = masterScheduleBL.GetEligibleSchedules(scheduleDate, fromTime, toTime, facilityMapId, productType);
                        if (scheduleDetailsListFromMaster != null && scheduleDetailsListFromMaster.Count > 0)
                        {
                            scheduleDetailsDTOList.AddRange(scheduleDetailsListFromMaster);
                        }
                    }
                }
                else
                {
                    foreach (MasterScheduleBL masterScheduleBL in masterScheduleBLList)
                    {
                        List<ScheduleDetailsDTO> scheduleDetailsListFromMaster = masterScheduleBL.GetEligibleSchedules(scheduleDate, fromTime, toTime, facilityMapId, productId, productType);
                        if (scheduleDetailsListFromMaster != null && scheduleDetailsListFromMaster.Count > 0)
                        {
                            scheduleDetailsDTOList.AddRange(scheduleDetailsListFromMaster);
                        }
                    }
                }
            }
            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Any())
            {
                scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleFromDate).ThenBy(sch => sch.FacilityMapName).ToList();
            }
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        /// <summary>
        /// Saves Attraction Master Schedule List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SaveAttractionMasterScheduleList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                if (masterScheduleDTOList == null ||
                        masterScheduleDTOList.Any() == false)
                {
                    log.LogMethodExit(null, "List is empty");
                    return;
                }
                if (masterScheduleDTOList != null)
                {
                    foreach (MasterScheduleDTO masterScheduleDTO in masterScheduleDTOList)
                    {
                        MasterScheduleBL masterScheduleBL = new MasterScheduleBL(executionContext, masterScheduleDTO);
                        masterScheduleBL.Save(sqlTransaction);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1896));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Error occurred while saving masterScheduleDTO.", ex);
                log.LogVariableState("masterScheduleDTOList", masterScheduleDTOList);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete AttractionMasterSchedule List
        /// </summary>
        public void DeleteAttractionMasterScheduleList()
        {
            log.LogMethodEntry();
            if (masterScheduleDTOList != null && masterScheduleDTOList.Count > 0)
            {
                foreach (MasterScheduleDTO masterScheduleDTO in masterScheduleDTOList)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (masterScheduleDTO.SchedulesDTOList != null && masterScheduleDTO.SchedulesDTOList.Count != 0)
                            {
                                foreach (SchedulesDTO schedulesDTO in masterScheduleDTO.SchedulesDTOList)
                                {
                                    if (schedulesDTO.ScheduleRulesDTOList != null && schedulesDTO.ScheduleRulesDTOList.Count > 0)
                                    {
                                        foreach (ScheduleRulesDTO scheduleRulesDTO in schedulesDTO.ScheduleRulesDTOList)
                                        {
                                            if (scheduleRulesDTO.IsActive == false && scheduleRulesDTO.IsChanged)
                                            {
                                                ScheduleRulesBL scheduleRulesBL = new ScheduleRulesBL(executionContext);
                                                scheduleRulesBL.DeleteScheduleRules(scheduleRulesDTO.ScheduleRulesId, parafaitDBTrx.SQLTrx);
                                            }
                                        }

                                    }
                                    if (schedulesDTO.ActiveFlag == false && schedulesDTO.IsChanged)
                                    {
                                        SchedulesBL schedulesBL = new SchedulesBL(executionContext);
                                        schedulesBL.DeleteSchedules(schedulesDTO, parafaitDBTrx.SQLTrx);

                                    }
                                }
                            }
                            if (masterScheduleDTO.ActiveFlag == false && masterScheduleDTO.IsChanged)
                            {
                                MasterScheduleBL masterScheduleBL = new MasterScheduleBL(executionContext);
                                masterScheduleBL.DeleteAttractionMasterSchedule(masterScheduleDTO.MasterScheduleId, parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1896));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetAttractionSchedulesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? lastUpdateTime = null;

            MasterScheduleDataHandler masterScheduleDataHandler = new MasterScheduleDataHandler(null);
            lastUpdateTime = masterScheduleDataHandler.GetAttractionSchedulesLastUpdateTime(siteId);

            log.LogMethodExit(lastUpdateTime);
            return lastUpdateTime;
        }
    }
}


