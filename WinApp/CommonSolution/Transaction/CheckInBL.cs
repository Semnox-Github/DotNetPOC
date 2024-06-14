/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  CheckIn
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.0      16-Jun-2019   Girish Kundar           Created 
 *2.70.0      27-Jun-2019   Mathew Ninan            Added methods from clsCheckIn and clsCheckOut
 *                                                  to move to 3 tier
 *2.80.0      07-May-2020   Mathew Ninan            Added logic to ignore pause in case pause is done
 *                                                  after check-in allocated minutes are completed
 *2.80        10-May-2020   Girish Kundar           Modified: REST API Changes merge from WMS     
 *2.140.0     24-Sept-2021  Girish Kundar           Modified: Check In Check Out related changes  
 ********************************************************************************************/
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for CheckIn class.
    /// </summary>
    public class CheckInBL
    {
        private CheckInDTO checkInDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CheckInBL class
        /// </summary>
        public CheckInBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates CheckInBL object using the checkInDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="checkInDTO">CheckInDTO object</param>
        public CheckInBL(ExecutionContext executionContext, CheckInDTO checkInDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInDTO);
            this.checkInDTO = checkInDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CheckIns id as the parameter
        /// Would fetch the CheckIns object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public CheckInBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CheckInDataHandler checkInDataHandler = new CheckInDataHandler(sqlTransaction);
            checkInDTO = checkInDataHandler.GetCheckIn(id);
            if (checkInDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CheckIn", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                // throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate CheckInDetail list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(executionContext);
            List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CHECK_IN_ID, checkInDTO.CheckInId.ToString()));
            searchParameters.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.IS_ACTIVE, "1"));
            checkInDTO.CheckInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(searchParameters, sqlTransaction);
            if (checkInDTO.CheckInDetailDTOList != null)
            {
                foreach (CheckInDetailDTO checkInDetailDTO in checkInDTO.CheckInDetailDTOList)
                {
                    if (checkInDetailDTO.CardId > -1)
                    {
                        checkInDetailDTO.AccountNumber = new AccountBL(executionContext, checkInDetailDTO.CardId, false, false, sqlTransaction).AccountDTO.TagNumber;
                    }
                }
            }
            if (checkInDTO.CustomerDTO == null && checkInDTO.CustomerId > -1)
            {
                CustomerBL customerBL = new CustomerBL(executionContext, checkInDTO.CustomerId);
                checkInDTO.CustomerDTO = customerBL.CustomerDTO;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CheckInDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (checkInDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            CheckInDataHandler checkInDataHandler = new CheckInDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (checkInDTO.CustomerDTO != null && checkInDTO.CustomerDTO.Id == -1)
            {
                CustomerBL customerBL = new CustomerBL(executionContext, checkInDTO.CustomerDTO);
                customerBL.Save(sqlTransaction);
                checkInDTO.CustomerId = checkInDTO.CustomerDTO.Id;
            }
            if (checkInDTO.CustomerDTO != null && checkInDTO.CustomerDTO.Id >= 0 && checkInDTO.CustomerId == -1)
            {
                checkInDTO.CustomerId = checkInDTO.CustomerDTO.Id;
            }
            if (checkInDTO.CheckInId < 0)
            {
                log.LogVariableState("CheckInDTO", checkInDTO);
                checkInDTO = checkInDataHandler.Insert(checkInDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInDTO.AcceptChanges();
            }
            else if (checkInDTO.IsChanged)
            {
                log.LogVariableState("CheckInDTO", checkInDTO);
                checkInDTO = checkInDataHandler.Update(checkInDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInDTO.AcceptChanges();
            }
            SaveCheckInDetails(sqlTransaction);
            try
            {
                ExternalInterfaces(true);
            }
            catch (Exception ex)
            {
                log.Error("Unable to call ExternalInterfaces()", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save Check in details
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveCheckInDetails(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (checkInDTO.CheckInDetailDTOList != null &&
                checkInDTO.CheckInDetailDTOList.Any())
            {
                List<CheckInDetailDTO> updatedCheckInDetailDTOList = new List<CheckInDetailDTO>();
                foreach (var checkInDetailDTO in checkInDTO.CheckInDetailDTOList)
                {
                    if (checkInDetailDTO.CheckInId != checkInDTO.CheckInId)
                    {
                        checkInDetailDTO.CheckInId = checkInDTO.CheckInId;
                    }
                    if (checkInDetailDTO.IsChanged)
                    {
                        updatedCheckInDetailDTOList.Add(checkInDetailDTO);
                    }
                }
                if (updatedCheckInDetailDTOList.Any())
                {
                    CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(executionContext, updatedCheckInDetailDTOList);
                    checkInDetailListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the CheckInDTO  ,CheckInDetailDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns>ValidationError List</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            FacilityDTO facilityDTO = new FacilityBL(executionContext, checkInDTO.CheckInFacilityId, false, false, sqlTransaction).FacilityDTO;
            int capacity = facilityDTO.Capacity == null ? 0 : Convert.ToInt32(facilityDTO.Capacity);
            if (capacity > 0)
            {
                if (capacity < GetTotalCheckedInForFacility(checkInDTO.CheckInFacilityId, sqlTransaction))
                {
                    validationErrorList.Add(new ValidationError("CheckIn", "Capacity", MessageContainerList.GetMessage(executionContext, 11)));
                }
            }
            if (checkInDTO != null && checkInDTO.CustomerDTO == null)
                validationErrorList.Add(new ValidationError("CheckIn", "Customer", MessageContainerList.GetMessage(executionContext, 2157)));

            if (checkInDTO.CheckInDetailDTOList != null)
            {
                foreach (var checkInDetailDTO in checkInDTO.CheckInDetailDTOList)
                {
                    if (checkInDetailDTO.IsChanged)
                    {
                        CheckInDetailBL checkInDetailBL = new CheckInDetailBL(executionContext, checkInDetailDTO);
                        checkInDetailBL.Validate(sqlTransaction); //calls child validation method.
                    }
                    if (checkInDetailDTO.CheckInDetailId > -1)
                    {
                        CheckInDetailBL checkInDetailBL = new CheckInDetailBL(executionContext, checkInDetailDTO.CheckInDetailId,false,false,sqlTransaction);
                        CheckInStatus currentStatus = checkInDetailBL.CheckInDetailDTO.Status;
                        CheckInStatus newStatus = checkInDetailDTO.Status;
                        if(IsValidCheckInStatus(currentStatus, newStatus) == false)
                        {
                            log.Error("Invalid CheckInStatus");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4082)); // "Invalid CheckInStatus"));
                        }
                        log.Debug("currentStatus: " + currentStatus.ToString());
                    }
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        internal bool IsValidCheckInStatus(CheckInStatus currentStatus, CheckInStatus newstatus)
        {
            log.LogMethodEntry(currentStatus, newstatus);
            bool result = false;
            // Possible status matrix
            Dictionary<string, List<string>> statusMatrix = new Dictionary<string, List<string>>()
            {
                {CheckInStatusConverter.ToString(CheckInStatus.PENDING), new List<string>{ CheckInStatusConverter.ToString(CheckInStatus.PENDING ), CheckInStatusConverter.ToString(CheckInStatus.ORDERED )} },
                {CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN ), new List<string>{ CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN), CheckInStatusConverter.ToString(CheckInStatus.PAUSED ) , CheckInStatusConverter.ToString(CheckInStatus.CHECKEDOUT ) } },
                {CheckInStatusConverter.ToString(CheckInStatus.PAUSED ), new List<string>{  CheckInStatusConverter.ToString(CheckInStatus.PAUSED) ,CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN ) , CheckInStatusConverter.ToString(CheckInStatus.CHECKEDOUT ) } },
                {CheckInStatusConverter.ToString(CheckInStatus.CHECKEDOUT ), new List<string>{ CheckInStatusConverter.ToString(CheckInStatus.CHECKEDOUT) } },
                {CheckInStatusConverter.ToString(CheckInStatus.ORDERED ), new List<string>{  CheckInStatusConverter.ToString(CheckInStatus.ORDERED),CheckInStatusConverter.ToString(CheckInStatus.CHECKEDIN) , CheckInStatusConverter.ToString(CheckInStatus.CHECKEDOUT) } },
            };

            if (statusMatrix.ContainsKey(CheckInStatusConverter.ToString(currentStatus)))
            {
                List<string> validStatuses = statusMatrix[CheckInStatusConverter.ToString(currentStatus)];
                if (validStatuses.Contains(CheckInStatusConverter.ToString(newstatus)))
                {
                    log.LogMethodExit(true);
                    result = true;
                }
            }

            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Method to compute check out pricing based on check out product
        /// </summary>
        /// <param name="productId">Check out product id</param>
        /// <param name="inCheckInDetailDTOList">List of CheckInDetailDTO, which is being checked out</param>
        /// <returns>effective final price for checkout</returns>
        public decimal GetCheckOutPrice(int productId, List<CheckInDetailDTO> inCheckInDetailDTOList)
        {
            log.LogMethodEntry(productId, inCheckInDetailDTOList);
            decimal effectivePrice = 0;
            ProductsDTO productsDTO = new Products(executionContext, productId, false, false).GetProductsDTO;
            if (checkInDTO != null)
            {
                //TimeSpan checkInDuration = (new LookupValuesList(executionContext)).GetServerDateTime() - checkInDTO.CheckInTime;
                foreach (CheckInDetailDTO checkInDetailDTO in inCheckInDetailDTOList)
                {
                    TimeSpan checkInDuration = new TimeSpan(0, 0, 0);
                    if (checkInDetailDTO.CheckInTime.HasValue)
                    {
                        checkInDuration = (ServerDateTime.Now - Convert.ToDateTime(checkInDetailDTO.CheckInTime));
                    }
                    int duration = (int)checkInDuration.TotalMinutes;
                    int overdue = Math.Max((duration - (checkInDTO.AllowedTimeInMinutes == -1 ? 0 : checkInDTO.AllowedTimeInMinutes)), 0);
                    log.LogVariableState("duration", duration);
                    log.LogVariableState("overdue", overdue);
                    int totalPauseTime = 0;
                    if (checkInDetailDTO.CheckOutTime == null)
                    {
                        CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(executionContext);
                        List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParameters = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, checkInDetailDTO.CheckInDetailId.ToString()));
                        List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParameters);
                        if (checkInPauseLogDTOList != null)
                        {
                            foreach (CheckInPauseLogDTO checkInPauseLogDTO in checkInPauseLogDTOList)
                            {
                                DateTime pauseEndTime = new LookupValuesList(executionContext).GetServerDateTime();
                                //Get total allowed time based on product set up
                                //DateTime checkInTimeAllowed = checkInDTO.CheckInTime.AddMinutes((checkInDTO.AllowedTimeInMinutes == -1 ? 0 : checkInDTO.AllowedTimeInMinutes));
                                DateTime checkInTimeAllowed = Convert.ToDateTime(checkInDetailDTO.CheckInTime).AddMinutes((checkInDTO.AllowedTimeInMinutes == -1 ? 0 : checkInDTO.AllowedTimeInMinutes));
                                if (overdue > 0 && checkInPauseLogDTO.PauseStartTime > checkInTimeAllowed)
                                {
                                    pauseEndTime = checkInPauseLogDTO.PauseStartTime;
                                }
                                totalPauseTime += checkInPauseLogDTO.PauseEndTime != null ? checkInPauseLogDTO.TotalPauseTime : Convert.ToInt32(pauseEndTime.Subtract(checkInPauseLogDTO.PauseStartTime).TotalMinutes);
                                log.LogVariableState("Total Pause Time for CheckIndetailId " + checkInDetailDTO.CheckInDetailId, totalPauseTime);
                            }
                        }
                        overdue = Math.Max((overdue - totalPauseTime),0);
                        effectivePrice += GetTotalPrice(productsDTO, overdue);
                    }
                    if (overdue > 0)
                        checkInDetailDTO.Detail = (string.IsNullOrEmpty(checkInDetailDTO.Detail) ? "" : checkInDetailDTO.Detail + ": ") + overdue.ToString() + " Min overdue";
                }
            }
            log.LogMethodExit(effectivePrice);
            return effectivePrice;
        }

        /// <summary>
        /// Computes price per overdue for each check in detail DTO
        /// </summary>
        /// <param name="productsDTO">Check out products DTO</param>
        /// <param name="overdue">Overdue time for each check in detail</param>
        /// <returns>total price based on check out product set up</returns>
        private decimal GetTotalPrice(ProductsDTO productsDTO, int overdue)
        {
            log.LogMethodEntry(productsDTO, overdue);
            decimal effectivePrice = 0;
            int baseTimeForPrice;
            if (productsDTO != null && productsDTO.Time >= 0)
            {
                baseTimeForPrice = Convert.ToInt32(productsDTO.Time);
            }
            else
            {
                baseTimeForPrice = 0;
            }
            if (overdue > 0)
            {
                if (checkInDTO.CheckInDetailDTOList.Count == 0)
                {
                    CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(executionContext);
                    List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CheckInDetailDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CheckInDetailDTO.SearchByParameters, string>(CheckInDetailDTO.SearchByParameters.CHECK_IN_ID, checkInDTO.CheckInId.ToString()));
                    List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(searchParameters);
                    if (checkInDetailDTOList != null && checkInDetailDTOList.Count > 0)
                    {
                        checkInDTO.CheckInDetailDTOList.AddRange(checkInDetailDTOList);
                        checkInDTO.AcceptChanges();
                    }
                }
                decimal units = productsDTO.AvailableUnits == -1 ? 1 : (1 / checkInDTO.CheckInDetailDTOList.Count);
                decimal maxCheckOutAmount = productsDTO.MaxCheckOutAmount == -1 ? 0 : productsDTO.MaxCheckOutAmount;
                decimal prodPrice = productsDTO.Price;
                Products productsBL = new Products(executionContext, productsDTO);
                decimal? slabPricing = productsBL.GetCheckInSlabPrice(overdue);
                if (slabPricing != null)
                {
                    prodPrice = Convert.ToDecimal(slabPricing);
                }
                effectivePrice = units * prodPrice;
                if (baseTimeForPrice != 0)
                {
                    int multiple = Convert.ToInt32(Math.Ceiling(overdue / Convert.ToDouble(baseTimeForPrice)));
                    effectivePrice = effectivePrice * multiple;

                    if (maxCheckOutAmount > 0 && effectivePrice > maxCheckOutAmount)
                        effectivePrice = maxCheckOutAmount;
                }
                log.LogVariableState("Product_id", productsDTO.ProductId);
                log.LogVariableState("Overdue", overdue);
                log.LogVariableState("Product Price", prodPrice);
                log.LogVariableState("MaxCheckOutAmount", maxCheckOutAmount);
                log.LogVariableState("Base Time for Pricing", baseTimeForPrice);
            }
            log.LogMethodExit(effectivePrice);
            return effectivePrice;
        }

        /// <summary>
        /// PerformCheckOut
        /// Perform check out which includes validation 
        /// and switching off any external device
        /// </summary>
        /// <param name="checkInDetailDTOList"></param>
        /// <param name="SQLTrx"></param>
        /// <returns></returns>
        public bool PerformCheckOut(List<CheckInDetailDTO> checkInDetailDTOList, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(checkInDetailDTOList, SQLTrx);

            try
            {
                foreach (CheckInDetailDTO checkInDetailDTO in checkInDetailDTOList)
                {
                    if (checkInDetailDTO.CheckInDetailId == -1)
                        throw new ValidationException("Cannot Check-out unsaved detail.");
                    if (checkInDetailDTO.CheckOutTrxId == -1)
                        throw new ValidationException("Cannot save as Trx Id is not updated for checkout.");
                    if (checkInDetailDTO.TrxLineId == -1)
                        throw new ValidationException("Cannot save as Line Id is not updated for checkout.");
                    checkInDetailDTO.CheckOutTime = ServerDateTime.Now; //set check out time
                    checkInDetailDTO.Status = CheckInStatus.CHECKEDOUT;
                    CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(executionContext);
                    List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
                    searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, checkInDetailDTO.CheckInDetailId.ToString()));
                    searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.PAUSE_END_TIME_IS_NULL, "null"));
                    List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParams);
                    if (checkInPauseLogDTOList != null && checkInPauseLogDTOList.Count > 0)
                    {
                        foreach (CheckInPauseLogDTO checkInPauseLogDTO in checkInPauseLogDTOList)
                        {
                            checkInPauseLogDTO.PauseEndTime = ServerDateTime.Now; ;
                            TimeSpan totalPauseTime = (ServerDateTime.Now - checkInPauseLogDTO.PauseStartTime);
                            checkInPauseLogDTO.TotalPauseTime = (int)totalPauseTime.TotalMinutes;
                            CheckInPauseLogBL checkInPauseLogBL = new CheckInPauseLogBL(executionContext, checkInPauseLogDTO);
                            checkInPauseLogBL.Save(SQLTrx);
                        }
                    }
                    CheckInDetailBL checkIndetailBL = new CheckInDetailBL(executionContext, checkInDetailDTO);
                    //to update the status during the check out Process
                    if (checkInDetailDTO.CheckInDetailId > -1)
                    {
                        CheckInStatus currentStatus = checkIndetailBL.CheckInDetailDTO.Status;
                        log.Debug("currentStatus: " + currentStatus.ToString());
                        if (IsValidCheckInStatus(currentStatus, CheckInStatus.CHECKEDOUT) == false)
                        {
                            log.Error("Invalid CheckInStatus");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4082)); // "Invalid CheckInStatus"));
                        }
                    }
                    checkIndetailBL = new CheckInDetailBL(executionContext, checkInDetailDTO);
                    checkIndetailBL.Save(SQLTrx);
                }
                ExternalInterfaces(false);
            }
            catch (ValidationException validateExcep)
            {
                log.Error(validateExcep.Message, validateExcep);
                throw;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating check-in details", ex);
                log.LogVariableState("Message ", ex.Message);
                log.LogMethodExit(false);
                throw new ValidationException("CheckOut Save: " + ex.Message);
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Switch On/off External Interfaces during Save process
        /// </summary>
        public void ExternalInterfaces(bool switchON)
        {
            log.LogMethodEntry(switchON);

            if (checkInDTO != null && checkInDTO.TableId >= 0)
            {
                FacilityTableDTO facilityTableDTO = new FacilityTables(executionContext, checkInDTO.TableId).FacilityTableDTO;
                log.LogVariableState("Table Id: ", checkInDTO.TableId);
                if (switchON)
                    Semnox.Parafait.Transaction.ExternalInterfaces.SwitchOn(facilityTableDTO);
                else
                    Semnox.Parafait.Transaction.ExternalInterfaces.SwitchOff(facilityTableDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Total Checked in based on Facility
        /// </summary>
        /// <param name="pCheckInFacilityId">Facility</param>
        /// <param name="pSQLTrx">SQL Transaction</param>
        /// <returns>Total Checked in count returned</returns>
        public int GetTotalCheckedInForFacility(int pCheckInFacilityId, SqlTransaction pSQLTrx)
        {
            log.LogMethodEntry(pCheckInFacilityId, pSQLTrx);
            CheckInDataHandler checkInDataHandler = new CheckInDataHandler(pSQLTrx);
            int totalCheckedIn = checkInDataHandler.GetTotalCheckedInForFacility(pCheckInFacilityId);
            log.LogMethodExit(totalCheckedIn);
            return totalCheckedIn;
        }

        /// <summary>
        /// Get Total Checked in based on Table
        /// </summary>
        /// <param name="pCheckInFacilityId">Facility</param>
        /// <param name="pSQLTrx">SQL Transaction</param>
        /// <returns>Total Checked in count returned</returns>
        public int GetTotalCheckedInForTable(int pTableId, SqlTransaction pSQLTrx = null)
        {
            log.LogMethodEntry(pTableId, pSQLTrx);
            CheckInDataHandler checkInDataHandler = new CheckInDataHandler(pSQLTrx);
            int totalCheckedIn = checkInDataHandler.GetTotalCheckedInForTable(pTableId);
            log.LogMethodExit(totalCheckedIn);
            return totalCheckedIn;
        }

        /// <summary>
        /// Compute check in price
        /// </summary>
        /// <param name="productsDTO">Products DTO</param>
        /// <param name="availableUnits">available Units passed</param>
        /// <param name="userPrice">Variable price</param>
        /// <returns>Effective price is returned</returns>
        public decimal GetCheckInPrice(ProductsDTO productsDTO, int availableUnits, decimal userPrice)
        {
            log.LogMethodEntry(productsDTO, availableUnits, userPrice);
            decimal effectivePrice;
            decimal Price = productsDTO.Price;

            log.LogVariableState("Price", Price);

            if (userPrice > 0)
            {
                effectivePrice = userPrice;
                if (Price != 0)
                    checkInDTO.AllowedTimeInMinutes = Convert.ToInt32(userPrice / Price);
                checkInDTO.UserTime = true;
            }
            else
                effectivePrice = Price;
            log.LogMethodExit(effectivePrice);
            return (effectivePrice);
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CheckInDTO CheckInDTO
        {
            get
            {
                return checkInDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of CheckIns
    /// </summary>
    public class CheckInListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<CheckInDTO> checkInDTOList = new List<CheckInDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public CheckInListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="checkInDTOList"></param>
        public CheckInListBL(ExecutionContext executionContext,
                                                List<CheckInDTO> checkInDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInDTOList);
            this.checkInDTOList = checkInDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the CheckInDTO list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>checkInDTOList</returns>
        public List<CheckInDTO> GetCheckInDTOList(List<KeyValuePair<CheckInDTO.SearchByParameters, string>> searchParameters,
                                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CheckInDataHandler checkInDataHandler = new CheckInDataHandler(sqlTransaction);
            List<CheckInDTO> checkInDTOList = checkInDataHandler.GetAllCheckInDTOList(searchParameters);
            if (checkInDTOList.Any() && loadChildRecords)
            {
                Build(checkInDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(checkInDTOList);
            return checkInDTOList;
        }

        private void Build(List<CheckInDTO> checkInDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(checkInDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, CheckInDTO> checkInIdIdCheckInDdetailDictionary = new Dictionary<int, CheckInDTO>();
            List<int> checkInIdSet = new List<int>();
            for (int i = 0; i < checkInDTOList.Count; i++)
            {
                if (checkInDTOList[i].CheckInId == -1 ||
                    checkInIdIdCheckInDdetailDictionary.ContainsKey(checkInDTOList[i].CheckInId))
                {
                    continue;
                }
                checkInIdSet.Add(checkInDTOList[i].CheckInId);
                checkInIdIdCheckInDdetailDictionary.Add(checkInDTOList[i].CheckInId, checkInDTOList[i]);
            }
            CheckInDetailListBL checkInDetailListBL = new CheckInDetailListBL(executionContext);
            List<CheckInDetailDTO> checkInDetailDTOList = checkInDetailListBL.GetCheckInDetailDTOList(checkInIdSet);
            if (checkInDetailDTOList.Any())
            {
                log.LogVariableState("CheckInDetailDTOList", checkInDetailDTOList);
                foreach (var checkInDetailDTO in checkInDetailDTOList)
                {
                    if (checkInIdIdCheckInDdetailDictionary.ContainsKey(checkInDetailDTO.CheckInId))
                    {
                        if (checkInIdIdCheckInDdetailDictionary[checkInDetailDTO.CheckInId].CheckInDetailDTOList == null)
                        {
                            checkInIdIdCheckInDdetailDictionary[checkInDetailDTO.CheckInId].CheckInDetailDTOList = new List<CheckInDetailDTO>();
                        }
                        checkInIdIdCheckInDdetailDictionary[checkInDetailDTO.CheckInId].CheckInDetailDTOList.Add(checkInDetailDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  List of CheckInDTO objects
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (checkInDTOList == null ||
                checkInDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < checkInDTOList.Count; i++)
            {
                var checkInDTO = checkInDTOList[i];
                if (checkInDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    CheckInBL checkInBL = new CheckInBL(executionContext, checkInDTO);
                    checkInBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving CheckInDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("CheckInDTO", checkInDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }

}
