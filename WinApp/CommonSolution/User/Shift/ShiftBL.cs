/********************************************************************************************
 * Project Name - ShiftBL
 * Description  - Bussiness logic of Shift
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        04-Mar-2019   Indhu          Created 
 *2.90        26-May-2020   Vikas Dwivedi  Modified as per the Standard CheckList
 *2.140.0     16-Aug-2021   Deeksha        Modified : Provisional Shift changes
 *2.140.0     16-Aug-2021   Girish         Modified : Multicash drawer changes, Added cashdrawerId column
 *2.140.0     16-Feb-2022   Girish         Modified : Multicash drawer changes- Update the approver Id and time for assign/unassign operation
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for ShiftBL
    /// </summary>
    public class ShiftBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ShiftDTO shiftDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of ShiftBL
        /// </summary>
        /// <param name="executionContext"></param>
        private ShiftBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ShiftBL object using the shiftDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="shiftDTO">ShiftDTO object is passed as parameter</param>
        public ShiftBL(ExecutionContext executionContext, ShiftDTO shiftDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftDTO);
            this.shiftDTO = shiftDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ShiftBL id as the parameter
        /// Would fetch the ShiftBL object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="shiftKey">id of ShiftBL Object </param>
        /// <param name="sqlTransaction"></param>
        /// <param name="buildReceipt">buildReceipt</param> // added for WMS API to print shift report 
        public ShiftBL(ExecutionContext executionContext, int shiftKey, bool loadChildRecords = true, bool buildSystemNumbers = false, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftKey, loadChildRecords, sqlTransaction);
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            this.shiftDTO = shiftDataHandler.GetShiftDTO(shiftKey);
            Utilities utilities = new Utilities();
            if (shiftDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Shift", shiftKey);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(sqlTransaction);
            }
            if (buildSystemNumbers && shiftDTO.ShiftAction == ShiftDTO.ShiftActionType.Open.ToString() || shiftDTO.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString())
            {
                DateTime shiftEndTime = shiftDTO.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString() ? shiftDTO.ShiftLogDTOList.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()).ShiftTime : ServerDateTime.Now;
                shiftDTO = shiftDataHandler.GetSystemNumbers(shiftDTO, shiftEndTime);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the child records for ShiftBL object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ShiftLogListBL shiftLogListBL = new ShiftLogListBL(executionContext);
            List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>> searchParameters = new List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>>();
            searchParameters.Add(new KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>(ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY, shiftDTO.ShiftKey.ToString()));
            shiftDTO.ShiftLogDTOList = shiftLogListBL.GetShiftLogDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ShiftDTO, ShiftLogDTOList - child 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            // List of values to be validated for each DTO .
            // Like if Balance== -1 or Id = null etc.

            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            // Validation Logic here 
            if (string.IsNullOrWhiteSpace(shiftDTO.ShiftKey.ToString()))
            {
                validationErrorList.Add(new ValidationError("ShiftBL", "ShiftKey", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Shift Key"))));
            }

            if (!string.IsNullOrWhiteSpace(shiftDTO.ShiftKey.ToString()) && shiftDTO.ShiftKey.ToString().Length > 50)
            {
                validationErrorList.Add(new ValidationError("ShiftBL", "ShiftKey", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Shift Key"), 50)));
            }

            // Validate Child List 
            if (shiftDTO.ShiftLogDTOList != null &&
               shiftDTO.ShiftLogDTOList.Count > 0)
            {
                foreach (ShiftLogDTO shiftLogDTO in shiftDTO.ShiftLogDTOList)
                {
                    ShiftLogBL shiftLogBL = new ShiftLogBL(executionContext, shiftLogDTO);
                    validationErrorList.AddRange(shiftLogBL.Validate(sqlTransaction));
                }
            }
            return validationErrorList;
        }

        /// <summary>
        /// Saves the Shift
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (shiftDTO.IsChangedRecursive == false && shiftDTO.ShiftKey > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            ShiftDataHandler ShiftDataHandler = new ShiftDataHandler(sqlTransaction);
            if (shiftDTO.ShiftKey < 0)
            {
                shiftDTO = ShiftDataHandler.InsertShiftDTO(shiftDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                shiftDTO.AcceptChanges();
            }
            else
            {
                if (shiftDTO.IsChanged)
                {
                    shiftDTO = ShiftDataHandler.UpdateShiftDTO(shiftDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    shiftDTO.AcceptChanges();
                }
            }
            SaveShiftLog(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records : ShiftLogDTOList
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        private void SaveShiftLog(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            // For child records : ShiftLog
            if (shiftDTO.ShiftLogDTOList != null &&
                shiftDTO.ShiftLogDTOList.Any())
            {
                List<ShiftLogDTO> updatedShiftLogDTOList = new List<ShiftLogDTO>();
                foreach (var shiftLogDTO in shiftDTO.ShiftLogDTOList)
                {
                    if (shiftLogDTO.ShiftKey != shiftDTO.ShiftKey)
                    {
                        shiftLogDTO.ShiftKey = shiftDTO.ShiftKey;
                        shiftLogDTO.ShiftId = shiftDTO.ShiftKey;
                    }
                    if (shiftLogDTO.IsChanged)
                    {
                        updatedShiftLogDTOList.Add(shiftLogDTO);
                    }
                }
                if (updatedShiftLogDTOList.Any())
                {
                    log.LogVariableState("UpdatedShiftLogDTOList", updatedShiftLogDTOList);
                    ShiftLogListBL shiftLogListBL = new ShiftLogListBL(executionContext, updatedShiftLogDTOList);
                    shiftLogListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        public ShiftCollections[] GetNetAmountAndPaymentMode(DateTime shiftEndTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            ShiftCollections[] paymentModes = shiftDataHandler.GetNetAmountAndPaymentMode(shiftDTO.ShiftTime, shiftDTO.POSMachine, shiftEndTime);
            log.LogMethodExit(paymentModes);
            return paymentModes;
        }

        public ShiftCollections[] GetShiftAmounts(DateTime shiftEndTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            ShiftCollections[] shiftAmounts = shiftDataHandler.GetShiftAmounts(shiftDTO.ShiftTime, shiftDTO.POSMachine, shiftEndTime);
            log.LogMethodExit(shiftAmounts);
            return shiftAmounts;
        }

        public ShiftCollections[] GetShiftDiscountedAmounts(DateTime shiftEndTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(shiftEndTime);
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            ShiftCollections[] shiftDisountedAmounts = shiftDataHandler.GetShiftDiscountedAmounts(shiftDTO.ShiftTime, shiftDTO.POSMachine, shiftEndTime);
            log.LogMethodExit(shiftDisountedAmounts);
            return shiftDisountedAmounts;
        }

        public ShiftCollections[] GetShiftPaymentModes(DateTime shiftEndTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(shiftEndTime);
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            ShiftCollections[] shiftPaymentModes = shiftDataHandler.GetShiftPaymentModes(shiftDTO.ShiftTime, shiftDTO.POSMachine, shiftEndTime);
            log.LogMethodExit(shiftPaymentModes);
            return shiftPaymentModes;
        }
        internal void ProvisionalClose(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            ShiftLogDTO shiftLogDTO = new ShiftLogDTO(-1, shiftDTO.ShiftKey, ServerDateTime.Now, ShiftDTO.ShiftActionType.PClose.ToString(), 0, 0, 0, "Blind Closed",
                                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, null, "Provisional Close Shift", null, shiftDTO.ShiftKey);
            shiftDTO.ShiftAction = ShiftDTO.ShiftActionType.PClose.ToString();
            shiftDTO.ShiftLogDTOList.Add(shiftLogDTO);
            Save(sqlTransaction);
            log.LogMethodExit();
        }
        internal void OpenShift(SqlTransaction sqlTransaction, string shiftReason)
        {
            log.LogMethodEntry(sqlTransaction, shiftReason);
            ShiftLogDTO shiftLogDTO = null;
            if (shiftDTO.ShiftKey != -1)
            {
                //shiftLogDTO = shiftDTO.ShiftLogDTOList.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.Open.ToString());
                //if(shiftLogDTO != null)
                //{
                    // to unlock the shift 
                    //shiftLogDTO.ShiftAmount = shiftDTO.ShiftAmount;
                    //shiftLogDTO.CardCount = Convert.ToInt32(shiftDTO.CardCount);
                    //shiftLogDTO.ShiftTicketNumber = Convert.ToInt32(shiftDTO.ShiftTicketNumber);
                    //shiftLogDTO.ShiftReason = shiftDTO.ShiftRemarks;
                    //shiftLogDTO.ActualAmount = Convert.ToDecimal(shiftDTO.ActualAmount);
                    //shiftLogDTO.ActualCards = Convert.ToInt32(shiftDTO.ActualCards);
                    //shiftLogDTO.ActualTickets = Convert.ToInt32(shiftDTO.ActualTickets);
                    //shiftLogDTO.GameCardAmount = Convert.ToDecimal(shiftDTO.GameCardamount);
                    //shiftLogDTO.CreditCardamount = Convert.ToDecimal(shiftDTO.CreditCardamount);
                    //shiftLogDTO.ChequeAmount = Convert.ToDecimal(shiftDTO.ChequeAmount);
                    //shiftLogDTO.CouponAmount = Convert.ToDecimal(shiftDTO.CouponAmount);
                    //shiftLogDTO.ActualGameCardamount = Convert.ToDecimal(shiftDTO.ActualGameCardamount);
                    //shiftLogDTO.ActualCreditCardAmount = Convert.ToDecimal(shiftDTO.ActualCreditCardamount);
                    //shiftLogDTO.ActualChequeAmount = Convert.ToDecimal(shiftDTO.ActualChequeAmount);
                    //shiftLogDTO.ActualCouponAmount = Convert.ToDecimal(shiftDTO.ActualCouponAmount);
                    //shiftLogDTO.ShiftReason = shiftReason;
                //}
            }
            else
            {
                shiftLogDTO = new ShiftLogDTO(-1, -1, ServerDateTime.Now, shiftDTO.ShiftAction, shiftDTO.ShiftAmount, Convert.ToInt32(shiftDTO.CardCount),
                                Convert.ToInt32(shiftDTO.ShiftTicketNumber), shiftDTO.ShiftRemarks, Convert.ToDecimal(shiftDTO.ActualAmount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.ActualAmount),
                                Convert.ToInt32(shiftDTO.ActualCards) == -1 ? (int?)null : Convert.ToInt32(shiftDTO.ActualCards), Convert.ToInt32(shiftDTO.ActualTickets) == -1 ? (int?)null : Convert.ToInt32(shiftDTO.ActualTickets),
                                 Convert.ToDecimal(shiftDTO.GameCardamount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.GameCardamount), Convert.ToDecimal(shiftDTO.CreditCardamount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.CreditCardamount),
                                 Convert.ToDecimal(shiftDTO.ChequeAmount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.ChequeAmount), Convert.ToDecimal(shiftDTO.CouponAmount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.CouponAmount),
                                 Convert.ToDecimal(shiftDTO.ActualGameCardamount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.ActualGameCardamount), Convert.ToDecimal(shiftDTO.ActualCreditCardamount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.ActualCreditCardamount),
                                 Convert.ToDecimal(shiftDTO.ActualChequeAmount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.ActualChequeAmount), Convert.ToDecimal(shiftDTO.ActualCouponAmount) == -1 ? (decimal?)null : Convert.ToDecimal(shiftDTO.ActualCouponAmount),null, null, shiftReason, "",-1);
            }
            if (shiftLogDTO != null)
            {
                shiftDTO.ShiftLogDTOList.Add(shiftLogDTO);
                Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        internal void CloseShift(decimal shiftAmount, int cardCount, double actualTickets, string remarks, double actualAmount, double actualCards, decimal gamecardAmount,
                                decimal creditCardAmount, decimal chequeAmount, decimal couponAmount, decimal actualGameCardAmount, decimal actualcreditCardAmount,
                                decimal actualChequeAmount, decimal actualCouponAmount, string shiftApplication, double shiftTicketnumber, SqlTransaction sqlTransaction)
        {
            shiftDTO.ShiftAction = ShiftDTO.ShiftActionType.Close.ToString();
            shiftDTO.ShiftAmount = Convert.ToDouble(shiftAmount);
            shiftDTO.CardCount = cardCount;
            shiftDTO.ActualTickets = actualTickets;
            shiftDTO.ShiftRemarks = remarks;
            shiftDTO.ActualAmount = actualAmount;
            shiftDTO.ActualCards = actualCards;
            shiftDTO.GameCardamount = Convert.ToDouble(gamecardAmount);
            shiftDTO.CreditCardamount = Convert.ToDouble(creditCardAmount);
            shiftDTO.ChequeAmount = Convert.ToDouble(chequeAmount);
            shiftDTO.CouponAmount = Convert.ToDouble(couponAmount);
            shiftDTO.ActualGameCardamount = Convert.ToDouble(actualGameCardAmount);
            shiftDTO.ActualCreditCardamount = Convert.ToDouble(actualcreditCardAmount);
            shiftDTO.ActualChequeAmount = Convert.ToDouble(actualChequeAmount);
            shiftDTO.ActualCouponAmount = Convert.ToDouble(actualCouponAmount);
            if (shiftApplication == "POS")
            {
                ShiftLogDTO shiftLogDTO = new ShiftLogDTO(-1, -1, ServerDateTime.Now, ShiftDTO.ShiftActionType.Close.ToString(), Convert.ToDouble(shiftAmount), cardCount, Convert.ToInt32(shiftTicketnumber), remarks,
                                                          Convert.ToDecimal(actualAmount), Convert.ToInt32(actualCards), Convert.ToInt32(actualTickets), gamecardAmount, creditCardAmount, chequeAmount, couponAmount, actualGameCardAmount, actualcreditCardAmount, actualChequeAmount
                                                           , actualCouponAmount, null, null, "Close Shift", null,-1);
                shiftDTO.ShiftLogDTOList.Add(shiftLogDTO);
            }
            else
            {
                ShiftLogDTO shiftLogDTO = new ShiftLogDTO(-1, -1, ServerDateTime.Now, ShiftDTO.ShiftActionType.Close.ToString(), 0, 0, Convert.ToInt32(shiftTicketnumber), remarks, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty,
                                                           null, "Close Shift Redemption", "",-1);
                shiftDTO.ShiftLogDTOList.Add(shiftLogDTO);
            }
            Save(sqlTransaction);
        }

        public void OpenRemoteShift()
        {
            log.LogMethodEntry();
            try
            {
                ShiftLogDTO shiftLogDTO;
                try
                {
                    shiftLogDTO = shiftDTO.ShiftLogDTOList.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.ROpen.ToString());
                    shiftDTO.ShiftAction = ShiftDTO.ShiftActionType.Open.ToString();
                    if (shiftLogDTO != null)
                    {
                        ShiftLogDTO logDTO = new ShiftLogDTO(-1, shiftDTO.ShiftKey, ServerDateTime.Now, ShiftDTO.ShiftActionType.Open.ToString(), shiftLogDTO.ShiftAmount, shiftLogDTO.CardCount, shiftLogDTO.ShiftTicketNumber,
                                                                shiftLogDTO.ShiftRemarks, shiftLogDTO.ActualAmount, shiftLogDTO.ActualCards, shiftLogDTO.ActualTickets, shiftLogDTO.GameCardAmount, shiftLogDTO.CreditCardamount,
                                                                shiftLogDTO.ChequeAmount, shiftLogDTO.CouponAmount, shiftLogDTO.ActualGameCardamount, shiftLogDTO.ActualCreditCardAmount, shiftLogDTO.ActualChequeAmount, shiftLogDTO.ActualCouponAmount,
                                                                shiftLogDTO.ApproverID, shiftLogDTO.ApprovalTime, shiftLogDTO.ShiftReason, shiftLogDTO.ExternalReference, shiftDTO.ShiftKey);
                        shiftDTO.ShiftLogDTOList.Add(logDTO);
                    }
                }
                catch { }
                Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        public ShiftDTO AssignCashdrawer(CashdrawerActivityDTO cashdrawerActivityDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cashdrawerActivityDTO, sqlTransaction);
            string approverId = string.Empty;
            if (string.IsNullOrWhiteSpace(cashdrawerActivityDTO.ManagerId) == false)
            {
                Users users = new Users(executionContext, Convert.ToInt32(cashdrawerActivityDTO.ManagerId));
                if(users.UserDTO != null)
                {
                    approverId = users.UserDTO.LoginId;
                }
            }
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            shiftDTO = shiftDataHandler.AssignCashdrawer(shiftDTO.ShiftKey, cashdrawerActivityDTO.CashdrawerId);
            UpdateApproverDetails(approverId, sqlTransaction);
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }

        private void UpdateApproverDetails(string approverId, SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry(approverId, sqlTransaction);
                ShiftLogDataHandler shiftLogDataHandler = new ShiftLogDataHandler(sqlTransaction);
                List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>>();
                searchParams.Add(new KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>(ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY, shiftDTO.ShiftKey.ToString()));
                searchParams.Add(new KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>(ShiftLogDTO.SearchByShiftParameters.SHIFT_ACTION, ShiftDTO.ShiftActionType.Open.ToString()));
                List<ShiftLogDTO> shiftLogDTOList = shiftLogDataHandler.GetShiftLogDTOList(searchParams, sqlTransaction);
                log.LogVariableState("shiftLogDTOList", shiftLogDTOList);
                if (shiftLogDTOList != null && shiftLogDTOList.Any())
                {
                    shiftLogDataHandler.UpdateCashdrawerApproverDetails(shiftLogDTOList.First().ShiftLogId, approverId);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        public ShiftDTO UnAssignCashdrawer(CashdrawerActivityDTO cashdrawerActivityDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(cashdrawerActivityDTO, sqlTransaction);
            string approverId = string.Empty;
            if (string.IsNullOrWhiteSpace(cashdrawerActivityDTO.ManagerId) == false)
            {
                Users users = new Users(executionContext, Convert.ToInt32(cashdrawerActivityDTO.ManagerId));
                if (users.UserDTO != null)
                {
                    approverId = users.UserDTO.LoginId;
                }
            }
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            shiftDTO = shiftDataHandler.UnAssignCashdrawer(shiftDTO.ShiftKey);
            UpdateApproverDetails(approverId, sqlTransaction);
            log.LogMethodExit(shiftDTO);
            return shiftDTO;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ShiftDTO ShiftDTO
        {
            get
            {
                return shiftDTO;
            }
        }


    }

    public class ShiftCollections
    {
        public string Mode;
        public bool isCreditCard;
        public double Amount;
        public double TaxableAmount;
        public double DiscountOnTaxableAmount;
        public double NonTaxableAmount;
        public double DiscountOnNonTaxableAmount;
        public double TaxAmount;
        public double DiscountOnTaxAmount;
        public string discountName;
        public double discountAmount;
        public double DiscountedTaxAmount;
        public double TrxCount;
        public string SortType;
    }



    /// <summary>
    /// Manages the list of Shift
    /// </summary>
    public class ShiftListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ShiftDTO> shiftDTOList = new List<ShiftDTO>();

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ShiftListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="shiftDTOList">shiftDTOList</param>
        public ShiftListBL(ExecutionContext executionContext, List<ShiftDTO> shiftDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftDTOList);
            this.shiftDTOList = shiftDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ShiftDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ShiftDTO</returns>
        public List<ShiftDTO> GetShiftDTOList(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParameters,
          bool loadChildRecords = false, bool buildReceipt = false, SqlTransaction sqlTransaction = null , bool buildSystemNumbers = false)
        {
            // child records needs to build
            log.LogMethodEntry(searchParameters, sqlTransaction);
            Utilities utilities = new Utilities();
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            List<ShiftDTO> shiftDTOList = shiftDataHandler.GetShiftDTOList(searchParameters, sqlTransaction);
            if (shiftDTOList != null && shiftDTOList.Any() && loadChildRecords)
            {
                Build(shiftDTOList, sqlTransaction);
                if (buildSystemNumbers)
                {
                    for (int i = 0; i < shiftDTOList.Count; i++)
                    {
                        if (shiftDTOList[i].ShiftAction == ShiftDTO.ShiftActionType.Open.ToString() || shiftDTOList[i].ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString())
                        {
                            DateTime shiftEndTime = shiftDTOList[i].ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString() ? shiftDTOList[i].ShiftLogDTOList.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()).ShiftTime : ServerDateTime.Now;
                            shiftDTOList[i] = shiftDataHandler.GetSystemNumbers(shiftDTOList[i], shiftEndTime);
                        }
                    }
                }
            }

            log.LogMethodExit(shiftDTOList);
            return shiftDTOList;
        }


        /// <summary>
        /// Builds the List of ShiftBL object based on the list of ShiftBL id.
        /// </summary>
        /// <param name="shiftDTOList"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<ShiftDTO> shiftDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(shiftDTOList, sqlTransaction);
            Dictionary<int, ShiftDTO> shiftKeyShiftDictionary = new Dictionary<int, ShiftDTO>();
            string shiftKeyIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < shiftDTOList.Count; i++)
            {
                if (shiftDTOList[i].ShiftKey == -1 ||
                    shiftKeyShiftDictionary.ContainsKey(shiftDTOList[i].ShiftKey))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(shiftDTOList[i].ShiftKey);
                shiftKeyShiftDictionary.Add(shiftDTOList[i].ShiftKey, shiftDTOList[i]);
            }
            shiftKeyIdSet = sb.ToString();
            ShiftLogListBL shiftLogListBL = new ShiftLogListBL(executionContext);
            List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>> searchParameters = new List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>>();
            searchParameters.Add(new KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>(ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY_LIST, shiftKeyIdSet.ToString()));
            List<ShiftLogDTO> shiftLogDTOList = shiftLogListBL.GetShiftLogDTOList(searchParameters, sqlTransaction);
            if (shiftLogDTOList != null && shiftLogDTOList.Any())
            {
                log.LogVariableState("ShiftLogDTOList", shiftLogDTOList);
                foreach (var shiftLogDTO in shiftLogDTOList)
                {
                    if (shiftKeyShiftDictionary.ContainsKey(shiftLogDTO.ShiftKey))
                    {
                        if (shiftKeyShiftDictionary[shiftLogDTO.ShiftKey].ShiftLogDTOList == null)
                        {
                            shiftKeyShiftDictionary[shiftLogDTO.ShiftKey].ShiftLogDTOList = new List<ShiftLogDTO>();
                        }
                        shiftKeyShiftDictionary[shiftLogDTO.ShiftKey].ShiftLogDTOList.Add(shiftLogDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

     

        /// <summary>
        /// This method should be called from the Parent Class BL method Save().
        /// Saves the ShiftBL List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public List<ShiftDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ShiftDTO> shiftDTOLists = new List<ShiftDTO>();
            if (shiftDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ShiftDTO shiftDTO in shiftDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ShiftBL shiftBL = new ShiftBL(executionContext, shiftDTO);
                            shiftBL.Save(sqlTransaction);
                            shiftDTOLists.Add(shiftBL.ShiftDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return shiftDTOLists;
        }

        public DateTime? GetShiftModuleLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            ShiftDataHandler shiftDataHandler = new ShiftDataHandler(sqlTransaction);
            DateTime? result = shiftDataHandler.GetShiftModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
        
    }

    public class RemoteShiftAssignment
    {
        bool isSelected;
        int machineId;
        int userId;
        int shiftId;
        DateTime shiftDate;
        DateTime shiftEndTime;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
        public int UserId { get { return userId; } set { userId = value; } }
        public int MachineId { get { return machineId; } set { machineId = value; } }
        public int ShiftId { get { return shiftId; } set { shiftId = value; } }
        public DateTime ShiftDate { get { return shiftDate; } set { shiftDate = value; } }
        public DateTime ShiftEndTime { get { return shiftEndTime; } set { shiftEndTime = value; } }
        public RemoteShiftAssignment()
        {
            isSelected = false;
            machineId = -1;
            userId = -1;
            shiftId = -1;
        }
    }
}
