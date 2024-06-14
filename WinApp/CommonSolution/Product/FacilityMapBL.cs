/********************************************************************************************
 * Project Name - FacilityMapBL
 * Description  - Business logic file for  FacilityMap
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        14-jun-2019   Guru S A                Created 
 *2.71        24-Jul-2019   Nitin Pai               Attraction enhancement for combo products
 *2.70.3      07-Jan-2020   Nitin Pai               Day Attraction and Reschedule Slot changes
 *2.80.0      02-Mar-2020   Girish Kundar           Modified: 3 Tier changes for RESt API
 *2.90        03-Jun-2020   Guru S A                reservation enhancements for commando release     
 *2.120.0    04-Mar-2021    Sathyavathi             Enabling option nto decide ''Multiple-Booking at Facility level  
  ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for FacilityMap class.
    /// </summary>
    public class FacilityMapBL
    {
        private FacilityMapDTO facilityMapDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of FacilityMapBL class
        /// </summary>
        public FacilityMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates FacilityMapBL object using the FacilityMapDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="facilityMapDTO">FacilityMapDTO object</param>
        public FacilityMapBL(ExecutionContext executionContext, FacilityMapDTO facilityMapDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityMapDTO);
            this.facilityMapDTO = facilityMapDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the facilityMap id as the parameter
        /// Would fetch the facilityMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public FacilityMapBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
            facilityMapDTO = facilityMapDataHandler.GetFacilityMap(id);
            if (facilityMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "FacilityMap", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction, true);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null, bool buildChildRecords = false)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            BuildFacilityMapDetails(activeChildRecords, sqlTransaction, buildChildRecords);
            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParm = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>();
            searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapDTO.FacilityMapId.ToString()));
            searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            facilityMapDTO.ProductsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParm, true, sqlTransaction);
            log.LogMethodExit();
        }

        private void BuildFacilityMapDetails(bool activeChildRecords = false, SqlTransaction sqlTransaction = null, bool buildChildRecords = false)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            FacilityMapDetailsListBL facilityMapDetailsListBL = new FacilityMapDetailsListBL(executionContext);
            List<KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>(FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapDTO.FacilityMapId.ToString()));
            searchParameters.Add(new KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>(FacilityMapDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>(FacilityMapDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            facilityMapDTO.FacilityMapDetailsDTOList = facilityMapDetailsListBL.GetFacilityMapDetailsDTOList(searchParameters, buildChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the FacilityMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityMapDTO.IsChangedRecursive == false
                && facilityMapDTO.FacilityMapId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (facilityMapDTO.FacilityMapId < 0)
            {
                log.LogVariableState("facilityMapDTO", facilityMapDTO);
                facilityMapDTO = facilityMapDataHandler.Insert(facilityMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(facilityMapDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("FacilityMap", facilityMapDTO.Guid, sqlTransaction);
                }
                facilityMapDTO.AcceptChanges();
            }
            else if (facilityMapDTO.IsChanged)
            {
                facilityMapDTO = facilityMapDataHandler.Update(facilityMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(facilityMapDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("FacilityMap", facilityMapDTO.Guid, sqlTransaction);
                }
                facilityMapDTO.AcceptChanges();
            }
            SaveChildDetails(sqlTransaction);
            log.LogMethodExit();
        }

        private void SaveChildDetails(SqlTransaction sqlTransaction)
        {
            if (facilityMapDTO.FacilityMapDetailsDTOList != null &&
                facilityMapDTO.FacilityMapDetailsDTOList.Any())
            {
                List<FacilityMapDetailsDTO> updatedFacilityMapDetailsDTOList = new List<FacilityMapDetailsDTO>();
                foreach (var facilityMapDetailsDTO in facilityMapDTO.FacilityMapDetailsDTOList)
                {
                    if (facilityMapDetailsDTO.FacilityMapId != facilityMapDTO.FacilityMapId)
                    {
                        facilityMapDetailsDTO.FacilityMapId = facilityMapDTO.FacilityMapId;
                    }
                    if (facilityMapDetailsDTO.IsChanged)
                    {
                        updatedFacilityMapDetailsDTOList.Add(facilityMapDetailsDTO);
                    }
                }
                if (updatedFacilityMapDetailsDTOList.Any())
                {
                    FacilityMapDetailsListBL facilityMapDetailsListBL = new FacilityMapDetailsListBL(executionContext, updatedFacilityMapDetailsDTOList);
                    facilityMapDetailsListBL.Save(sqlTransaction);
                }
            }
            if (facilityMapDTO.ProductsAllowedInFacilityDTOList != null &&
               facilityMapDTO.ProductsAllowedInFacilityDTOList.Any())
            {
                List<ProductsAllowedInFacilityMapDTO> updatedProductsAllowedInFacilityMapDTOList = new List<ProductsAllowedInFacilityMapDTO>();
                foreach (var productsAllowedInFacilityDTO in facilityMapDTO.ProductsAllowedInFacilityDTOList)
                {
                    if (productsAllowedInFacilityDTO.FacilityMapId != facilityMapDTO.FacilityMapId)
                    {
                        productsAllowedInFacilityDTO.FacilityMapId = facilityMapDTO.FacilityMapId;
                    }
                    if (productsAllowedInFacilityDTO.IsChanged)
                    {
                        updatedProductsAllowedInFacilityMapDTOList.Add(productsAllowedInFacilityDTO);
                    }
                }
                if (updatedProductsAllowedInFacilityMapDTOList.Any())
                {
                    ProductsAllowedInFacilityMapListBL productsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext, updatedProductsAllowedInFacilityMapDTOList);
                    productsAllowedInFacilityMapListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validates the FacilityMapDTO  ,FacilityMapDetailsDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (facilityMapDTO.ProductsAllowedInFacilityDTOList != null && facilityMapDTO.ProductsAllowedInFacilityDTOList.Any(x => x.IsChanged))
            {
                string productIdList = string.Join(",", facilityMapDTO.ProductsAllowedInFacilityDTOList.Where(x => x.IsChanged == true).Select(x => x.ProductsId));
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID_LIST, productIdList));
                searchParameters.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                ProductsList productList = new ProductsList(executionContext);
                List<ProductsDTO> productsDTOList = new List<ProductsDTO>();
                productsDTOList = productList.GetProductsDTOList(searchParameters);
                if (productsDTOList.Any(x => x.ActiveFlag == false))
                {
                    validationErrorList.Add(new ValidationError("FacilityMap", "AllowedProducts", MessageContainerList.GetMessage(executionContext, 4806)));
                }
            }
            if (string.IsNullOrWhiteSpace(facilityMapDTO.FacilityMapName))
            {
                validationErrorList.Add(new ValidationError("FacilityMap", "FacilityMapName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Facility Map Name"))));
            }

            if (!string.IsNullOrWhiteSpace(facilityMapDTO.FacilityMapName) && facilityMapDTO.FacilityMapName.Length > 100)
            {
                validationErrorList.Add(new ValidationError("FacilityMap", "FacilityMapName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Facility Map Name"), 100)));
            }
            if (facilityMapDTO.FacilityMapDetailsDTOList != null)
            {
                foreach (var facilityMapDetailsDTO in facilityMapDTO.FacilityMapDetailsDTOList)
                {
                    if (facilityMapDetailsDTO.IsChanged)
                    {
                        FacilityMapDetailsBL facilityMapDetailsBL = new FacilityMapDetailsBL(executionContext, facilityMapDetailsDTO);
                        validationErrorList.AddRange(facilityMapDetailsBL.Validate(sqlTransaction));
                    }
                }
            }
            if (facilityMapDTO.ProductsAllowedInFacilityDTOList != null)
            {
                foreach (var productsAllowedInFacilityDTO in facilityMapDTO.ProductsAllowedInFacilityDTOList)
                {
                    if (productsAllowedInFacilityDTO.IsChanged)
                    {
                        ProductsAllowedInFacilityMapBL productsAllowedInFacilityMapBL = new ProductsAllowedInFacilityMapBL(executionContext, productsAllowedInFacilityDTO);
                        validationErrorList.AddRange(productsAllowedInFacilityMapBL.Validate(sqlTransaction));
                    }
                }
            }
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameter1 = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            searchParameter1.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.FACILITY_MAP_NAME, facilityMapDTO.FacilityMapName));
            searchParameter1.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
            List<FacilityMapDTO> facilityMapDTOs = facilityMapDataHandler.GetAllFacilityMap(searchParameter1);

            if (facilityMapDTOs != null && facilityMapDTOs.Any())
            {
                if (facilityMapDTO.FacilityMapId < 0)
                {
                    validationErrorList.Add(new ValidationError("FacilityMap", "FacilityMapName", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Facility Map"))));
                }
                if (facilityMapDTO.FacilityMapId != facilityMapDTOs.FirstOrDefault().FacilityMapId)
                {
                    validationErrorList.Add(new ValidationError("FacilityMap", "FacilityMapName", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Facility Map"))));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public FacilityMapDTO FacilityMapDTO
        {
            get
            {
                return facilityMapDTO;
            }
        }

        /// <summary>
        /// Get the booked unit details
        /// </summary> 
        /// <param name="ScheduleFromDate"></param>
        /// <param name="scheduleToDate"></param>
        /// <param name="productId"></param>
        /// <param name="bookingId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public int GetTotalBookedUnitsForAttraction(DateTime ScheduleFromDate, DateTime scheduleToDate, int bookingId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleFromDate, scheduleToDate, bookingId, sqlTransaction);
            int bookedUnits = 0;
            if (this.facilityMapDTO != null)
            {
                FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
                bookedUnits = facilityMapDataHandler.GetTotalBookedUnitsForAttractions(this.facilityMapDTO.FacilityMapId, ScheduleFromDate, scheduleToDate, bookingId);
            }
            else
            {
                log.LogMethodExit("this.facilityMapDTO == null");
                throw new ValidationException(MessageContainerList.GetMessage(this.executionContext, "Virtual Facility details are not loaded"));
            }
            log.LogMethodExit(bookedUnits);
            return bookedUnits;
        }

        /// <summary>
        /// Get the booked unit details
        /// </summary>
        /// <param name="scheduleFromDate"></param>
        /// <param name="scheduleToDate"></param>
        /// <param name="bookingTrxId"></param>
        /// <param name="trxReservationScheduleId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public int GetTotalBookedUnitsForReservation(DateTime scheduleFromDate, DateTime scheduleToDate, int bookingTrxId = -1, int trxReservationScheduleId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleFromDate, scheduleToDate, bookingTrxId, trxReservationScheduleId, sqlTransaction);
            int bookedUnits = 0;
            if (this.facilityMapDTO != null)
            {
                FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
                bookedUnits = facilityMapDataHandler.GetTotalBookedUnitsForReservation(this.facilityMapDTO.FacilityMapId, scheduleFromDate, scheduleToDate, -1, bookingTrxId, trxReservationScheduleId);
            }
            else
            {
                log.LogMethodExit("this.facilityMapDTO == null");
                throw new ValidationException(MessageContainerList.GetMessage(this.executionContext, "Virtual Facility details are not loaded"));
            }
            log.LogMethodExit(bookedUnits);
            return bookedUnits;
        }
        /// <summary>
        /// GetMappedFacilityNames
        /// </summary>
        /// <returns></returns>
        public string GetMappedFacilityNames()
        {
            log.LogMethodEntry();
            string facilityNames = string.Empty;
            if (this.facilityMapDTO != null)
            {
                if (this.facilityMapDTO.FacilityMapDetailsDTOList == null || this.facilityMapDTO.FacilityMapDetailsDTOList.Count == 0)
                {
                    BuildFacilityMapDetails(true);
                }
                if (this.facilityMapDTO.FacilityMapDetailsDTOList != null)
                {
                    int totalMappedFacilities = this.facilityMapDTO.FacilityMapDetailsDTOList.Count;
                    for (int i = 0; i < totalMappedFacilities; i++)
                    {
                        if (i == totalMappedFacilities - 1)
                        {
                            facilityNames = facilityNames + this.facilityMapDTO.FacilityMapDetailsDTOList[i].FacilityName;
                        }
                        else
                        {
                            facilityNames = facilityNames + this.facilityMapDTO.FacilityMapDetailsDTOList[i].FacilityName + ", ";
                        }
                    }
                }
            }
            log.LogMethodExit(facilityNames);
            return facilityNames;
        }

        /// <summary>
        /// GetMappedFacilityDTOList
        /// </summary>
        /// <returns></returns>
        public List<FacilityDTO> GetMappedFacilityDTOList()
        {
            log.LogMethodEntry();
            List<FacilityDTO> facilityDTOList = new List<FacilityDTO>();
            if (this.facilityMapDTO != null)
            {

                FacilityList facilityList = new FacilityList(executionContext);
                List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.FACILITY_MAP_ID, this.facilityMapDTO.FacilityMapId.ToString()));
                searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                facilityDTOList = facilityList.GetFacilityDTOList(searchParameters);
            }
            log.LogMethodExit(facilityDTOList);
            return facilityDTOList;
        }
        /// <summary>
        /// Can Accomodate Reservation Qty
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="ruleUnits"></param>
        /// <param name="scheduleFromDate"></param>
        /// <param name="scheduleToDate"></param>
        /// <param name="bookingTrxId"></param>
        /// <param name="trxReservationScheduleId"></param>
        /// <param name="sqlTransaction"></param>
        public void CanAccomodateReservationQty(int qty, int? ruleUnits, DateTime scheduleFromDate, DateTime scheduleToDate, int bookingTrxId = -1, int trxReservationScheduleId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(qty, ruleUnits, scheduleFromDate, scheduleToDate, bookingTrxId, trxReservationScheduleId,sqlTransaction);
            int bookedQty = GetTotalBookedUnitsForReservation(scheduleFromDate, scheduleToDate, bookingTrxId, trxReservationScheduleId);
            string message = string.Empty;
            int maxQty = (int)(ruleUnits != null ? ruleUnits : (this.FacilityMapDTO.FacilityCapacity != null ? this.FacilityMapDTO.FacilityCapacity : 0));
            if (bookedQty + qty > maxQty)
            {
                message = MessageContainerList.GetMessage(executionContext, 326, qty, (maxQty - bookedQty));
                log.Error(message);
                log.LogMethodExit(message);
                throw new ValidationException(message);
            }
            if (bookedQty > 0)
            {
                CanAllowMulitpleBookingsForTheSlot(sqlTransaction);
                if (this.FacilityMapDTO.FacilityCapacity >= 0)
                {
                    if (bookedQty + qty > this.FacilityMapDTO.FacilityCapacity)
                    {
                        message = MessageContainerList.GetMessage(executionContext, 1253, qty, (this.FacilityMapDTO.FacilityCapacity - bookedQty), this.FacilityMapDTO.FacilityCapacity);
                        log.Error(message);
                        log.LogMethodExit(message);
                        throw new ValidationException(message);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// CanAllowMulitpleBookingsForTheSlot
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void CanAllowMulitpleBookingsForTheSlot(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityMapDTO != null
                                && (facilityMapDTO.FacilityMapDetailsDTOList == null || facilityMapDTO.FacilityMapDetailsDTOList.Any() == false))
            {
                Build(true, sqlTransaction);
            }
            if (facilityMapDTO != null && facilityMapDTO.FacilityMapDetailsDTOList != null &&
                                                                                    facilityMapDTO.FacilityMapDetailsDTOList.Any())
            {
                for (int i = 0; i < this.facilityMapDTO.FacilityMapDetailsDTOList.Count; i++)
                {
                    if (facilityMapDTO.FacilityMapDetailsDTOList[i].FacilityDTOList != null &&
                        facilityMapDTO.FacilityMapDetailsDTOList[i].FacilityDTOList.Any())
                    {
                        for (int j = 0; j < this.facilityMapDTO.FacilityMapDetailsDTOList[i].FacilityDTOList.Count; j++)
                        {
                            if (this.facilityMapDTO.FacilityMapDetailsDTOList[i].FacilityDTOList[j].AllowMultipleBookings == false)
                            {
                                string message = MessageContainerList.GetMessage(executionContext, 1254);
                                log.Error(message);
                                log.LogMethodExit(message);
                                throw new ValidationException(message);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of FacilityMap
    /// </summary>
    public class FacilityMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<FacilityMapDTO> facilityMapDTOList = new List<FacilityMapDTO>(); // To be initialized
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public FacilityMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="facilityMapDTOList"></param>
        public FacilityMapListBL(ExecutionContext executionContext, List<FacilityMapDTO> facilityMapDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, facilityMapDTOList);
            this.facilityMapDTOList = facilityMapDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Returns the Get the FacilityMapDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="loadChildForOnlyProductType"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>facilityMapDTOList</returns>
        public List<FacilityMapDTO> GetFacilityMapDTOList(List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters,
                                                        bool loadChildRecords = false, bool activeChildRecords = true,
                                                        bool loadChildForOnlyProductType = false, SqlTransaction sqlTransaction = null)
        {
            //child records needs to  build
            log.LogMethodEntry(searchParameters, sqlTransaction, loadChildRecords, activeChildRecords, loadChildForOnlyProductType);
            FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
            List<FacilityMapDTO> facilityMapDTOList = facilityMapDataHandler.GetAllFacilityMap(searchParameters);
            if (facilityMapDTOList != null && facilityMapDTOList.Any() && loadChildRecords)
            {
                string productTypesIn = string.Empty;
                if (loadChildForOnlyProductType && searchParameters.Any(m => m.Key.Equals(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN)))
                {
                    productTypesIn = searchParameters.Where(m => m.Key.Equals(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN)).FirstOrDefault().Value;
                }
                Build(facilityMapDTOList, activeChildRecords, productTypesIn, sqlTransaction, loadChildRecords);
            }
            log.LogMethodExit(facilityMapDTOList);
            return facilityMapDTOList;
        }


        /// <summary>
        ///  Returns the Get the FacilityMapDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="loadChildForOnlyProductType"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>facilityMapDTOList</returns>
        public List<int> GetFacilityMapsForSameFacility(int facilityMapId, SqlTransaction sqlTransaction = null)
        {
            //child records needs to  build
            log.LogMethodEntry(sqlTransaction);

            FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facilityMapId, true, true, sqlTransaction);
            if (facilityMapBL == null || facilityMapBL.FacilityMapDTO == null || facilityMapBL.FacilityMapDTO.FacilityMapDetailsDTOList == null)
                return null;

            String facilityIdList = "";
            foreach (FacilityMapDetailsDTO facilityMapDetailsDTO in facilityMapBL.FacilityMapDTO.FacilityMapDetailsDTOList)
            {
                foreach(FacilityDTO facilityDTO in facilityMapDetailsDTO.FacilityDTOList)
                {
                    if(facilityIdList.Length > 0)
                    {
                        facilityIdList += ",";
                    }
                    facilityIdList += facilityDTO.FacilityId;
                }
            }

            List<int> facilityMapIdList = new List<int>();
            if (!String.IsNullOrEmpty(facilityIdList))
            {
                FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
                List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.FACILITY_IDS_IN, facilityIdList));
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                List<FacilityMapDTO> facilityMapDTOList = facilityMapDataHandler.GetAllFacilityMap(searchParameters);
                if (facilityMapDTOList != null && facilityMapDTOList.Any())
                {
                    foreach (FacilityMapDTO facilityMapDTO in facilityMapDTOList)
                        facilityMapIdList.Add(facilityMapDTO.FacilityMapId);
                }
            }
            log.LogMethodExit(facilityMapIdList);
            return facilityMapIdList;
        }

        private void Build(List<FacilityMapDTO> facilityMapDTOList, bool activeChildRecords = true, string productTypesIn = null, SqlTransaction sqlTransaction = null, bool buildChildRecords = false)
        {
            log.LogMethodEntry(facilityMapDTOList, activeChildRecords, sqlTransaction, productTypesIn, buildChildRecords);

            Dictionary<int, FacilityMapDTO> facilityMapIdFacilityMapDictionary = new Dictionary<int, FacilityMapDTO>();
            StringBuilder sb = new StringBuilder("");
            string facilityMapIdSet;
            for (int i = 0; i < facilityMapDTOList.Count; i++)
            {
                if (facilityMapDTOList[i].FacilityMapId == -1 ||
                    facilityMapIdFacilityMapDictionary.ContainsKey(facilityMapDTOList[i].FacilityMapId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(facilityMapDTOList[i].FacilityMapId);
                facilityMapIdFacilityMapDictionary.Add(facilityMapDTOList[i].FacilityMapId, facilityMapDTOList[i]);
            }
            facilityMapIdSet = sb.ToString();
            FacilityMapDetailsListBL facilityMapDetailsListBL = new FacilityMapDetailsListBL(executionContext);
            List<KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>(FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_ID_LIST, facilityMapIdSet.ToString()));
            searchParameters.Add(new KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>(FacilityMapDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>(FacilityMapDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            List<FacilityMapDetailsDTO> facilityMapDetailsDTOList = facilityMapDetailsListBL.GetFacilityMapDetailsDTOList(searchParameters, buildChildRecords, activeChildRecords, sqlTransaction);
            if (facilityMapDetailsDTOList != null && facilityMapDetailsDTOList.Any())
            {
                log.LogVariableState("FacilityMapDetailsDTOList", facilityMapDetailsDTOList);
                foreach (var facilityMapDetailsDTO in facilityMapDetailsDTOList)
                {
                    if (facilityMapIdFacilityMapDictionary.ContainsKey(facilityMapDetailsDTO.FacilityMapId))
                    {
                        if (facilityMapIdFacilityMapDictionary[facilityMapDetailsDTO.FacilityMapId].FacilityMapDetailsDTOList == null)
                        {
                            facilityMapIdFacilityMapDictionary[facilityMapDetailsDTO.FacilityMapId].FacilityMapDetailsDTOList = new List<FacilityMapDetailsDTO>();
                        }
                        facilityMapIdFacilityMapDictionary[facilityMapDetailsDTO.FacilityMapId].FacilityMapDetailsDTOList.Add(facilityMapDetailsDTO);
                    }
                }
            }

            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParm = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>();
            searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID_LIST, facilityMapIdSet.ToString()));
            searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            if (!string.IsNullOrEmpty(productTypesIn))
            {
                searchParm.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, productTypesIn));
            }
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParm, true, sqlTransaction);
            if (productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Any())
            {
                log.LogVariableState("productsAllowedInFacilityDTOList", productsAllowedInFacilityDTOList);
                foreach (var productsAllowedInFacilityDTO in productsAllowedInFacilityDTOList)
                {
                    if (facilityMapIdFacilityMapDictionary.ContainsKey(productsAllowedInFacilityDTO.FacilityMapId))
                    {
                        if (facilityMapIdFacilityMapDictionary[productsAllowedInFacilityDTO.FacilityMapId].FacilityMapDetailsDTOList == null)
                        {
                            facilityMapIdFacilityMapDictionary[productsAllowedInFacilityDTO.FacilityMapId].ProductsAllowedInFacilityDTOList = new List<ProductsAllowedInFacilityMapDTO>();
                        }
                        facilityMapIdFacilityMapDictionary[productsAllowedInFacilityDTO.FacilityMapId].ProductsAllowedInFacilityDTOList.Add(productsAllowedInFacilityDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the FacilityMap List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (facilityMapDTOList == null ||
                facilityMapDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < facilityMapDTOList.Count; i++)
            {
                var facilityMapDTO = facilityMapDTOList[i];
                if (facilityMapDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facilityMapDTO);
                    facilityMapBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving FacilityMapDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("FacilityMapDTO", facilityMapDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public List<FacilityMapDTO> GetFacilityMapDTOList(List<int> facilityMapIdList,
                                                        bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        { 
            log.LogMethodEntry(facilityMapIdList, sqlTransaction, loadChildRecords, activeChildRecords);
            FacilityMapDataHandler facilityMapDataHandler = new FacilityMapDataHandler(sqlTransaction);
            List<FacilityMapDTO> facilityMapDTOList = facilityMapDataHandler.GetFacilityMapByIdList(facilityMapIdList);
            if (facilityMapDTOList != null && facilityMapDTOList.Any() && loadChildRecords)
            {                
                Build(facilityMapDTOList, activeChildRecords, null, sqlTransaction, loadChildRecords);
            }
            log.LogMethodExit(facilityMapDTOList);
            return facilityMapDTOList;
        }
    }
}
