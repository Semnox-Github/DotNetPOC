/********************************************************************************************
 * Project Name - TransactionPayments BL
 * Description  - Business logict
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        22-Jun-2017      Lakshminarayana     Created 
 *2.50.0      12-Dec-2018      Mathew Ninan        Updated to add execution context and other features of TrxPayments
 *2.60.2      06-Jun-2019      Akshay Gulaganji    ExecutionContext changes
 *2.70.2.0      11-Jul-2019      Girish Kundar       Modified : Save() method ,Insert/Update methods returns DTO.
 *                                                            Added LogMethodEntry() and LogMethodExit().
 *2.90        31-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 *2.130.7     14-APR-2022      Girish Kundar           Modified : Aloha BSP integration changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Business logic for TransactionPayments class.
    /// </summary>
    public class TransactionPaymentsBL
    {
        private TransactionPaymentsDTO transactionPaymentsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor of TransactionPaymentsBL class
        /// </summary>
        /// <param name="executionContext">Execution Context</param>
        private TransactionPaymentsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            transactionPaymentsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the transactionPayments id as the parameter
        /// Would fetch the transactionPayments object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">Execution Context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public TransactionPaymentsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
            transactionPaymentsDTO = transactionPaymentsDataHandler.GetTransactionPaymentsDTO(id);
            if (transactionPaymentsDTO != null)
            {
                transactionPaymentsDTO.PaymentModeDTO = (new PaymentMode(executionContext, transactionPaymentsDTO.PaymentModeId)).GetPaymentModeDTO;
            }
            if (transactionPaymentsDTO.PaymentModeDTO != null && transactionPaymentsDTO.PaymentModeDTO.IsCreditCard)
            {
                string gateway = (new PaymentMode(executionContext, transactionPaymentsDTO.PaymentModeDTO)).Gateway;
                if (!string.IsNullOrEmpty(gateway) && Enum.IsDefined(typeof(PaymentGateways), gateway))
                    transactionPaymentsDTO.PaymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), gateway);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TransactionPaymentsBL object using the TransactionPaymentsDTO
        /// </summary>
        /// <param name="executionContext">Execution Context</param>
        /// <param name="transactionPaymentsDTO">TransactionPaymentsDTO object</param>
        public TransactionPaymentsBL(ExecutionContext executionContext, TransactionPaymentsDTO transactionPaymentsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionPaymentsDTO);
            this.transactionPaymentsDTO = transactionPaymentsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor for backward compatability
        /// </summary>
        /// <param name="transactionPaymentsDTO">TransactionPaymentsDTO object</param>
        public TransactionPaymentsBL(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            this.transactionPaymentsDTO = transactionPaymentsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TransactionPayments
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">Sql Transaction passed if transaction managed outside</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            ValidateEnabledAttributes();
            TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
            if (transactionPaymentsDTO.PaymentId < 0)
            {
                transactionPaymentsDTO = transactionPaymentsDataHandler.InsertTransactionPayments(transactionPaymentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionPaymentsDTO.AcceptChanges();
            }
            else
            {
                if (transactionPaymentsDTO.IsChanged)
                {
                    transactionPaymentsDTO = transactionPaymentsDataHandler.UpdateTransactionPayments(transactionPaymentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionPaymentsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionPaymentsDTO TransactionPaymentsDTO
        {
            get
            {
                return transactionPaymentsDTO;
            }
        }
        /// <summary>
        /// SettleTransactionPayment
        /// </summary>
        public void SettleTransactionPayment(TransactionPaymentsDTO inputTrxPaymentDTO)
        {
            log.LogMethodEntry(inputTrxPaymentDTO);
            ValidatePaymentDetailsForSettlement(inputTrxPaymentDTO);
            Update(inputTrxPaymentDTO);
            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionPaymentsDTO.PaymentModeDTO.GatewayLookUp.ToString());
            // bool settlementPending = paymentGateway.IsSettlementPending(transactionPaymentsDTO);
            this.transactionPaymentsDTO = paymentGateway.SettleTransactionPayment(this.transactionPaymentsDTO);
            Save();
            log.LogMethodExit();
        }

        private void ValidatePaymentDetailsForSettlement(TransactionPaymentsDTO inputTrxPaymentDTO)
        {
            log.LogMethodEntry();
            if (this.transactionPaymentsDTO == null || inputTrxPaymentDTO == null)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1130));
                //Please enter data and save
            }
            if (this.transactionPaymentsDTO.PaymentId == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 989));
                //Please save the entry first.
            }
            if (this.transactionPaymentsDTO.PaymentId != inputTrxPaymentDTO.PaymentId)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "PaymentId") + "-" + MessageContainerList.GetMessage(executionContext, 1775));
                //Please enter valid value for parameter
            }
            if (this.transactionPaymentsDTO.PaymentModeId != inputTrxPaymentDTO.PaymentModeId)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "PaymentModeId") + "-" + MessageContainerList.GetMessage(executionContext, 665));
                //Please save changes before this operation
            }
            if (transactionPaymentsDTO.TipAmount != inputTrxPaymentDTO.TipAmount
                && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "MANAGER_APPROVAL_REQUIRED_FOR_TIP_ADJUSTMENT")
                && string.IsNullOrWhiteSpace(inputTrxPaymentDTO.ApprovedBy))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268) + " - " + MessageContainerList.GetMessage(executionContext, "Tip Adjustment"));
                //Manager Approval Required for this Task
            }
            if (this.transactionPaymentsDTO.PaymentModeDTO == null)
            {
                LoadPaymentModeDTO();
            }
            if (this.transactionPaymentsDTO.PaymentModeDTO.IsCreditCard == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4093));
                //Cannot perform settle payment task for non credit card payments 
            }
            else if (string.IsNullOrEmpty(this.transactionPaymentsDTO.PaymentModeDTO.GatewayLookUp.ToString()) || this.transactionPaymentsDTO.PaymentModeDTO.GatewayLookUp == PaymentGateways.None)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4094));
                //Gateway details are missing, cannot perform settle payment task
            }
            if (transactionPaymentsDTO.TipAmount != inputTrxPaymentDTO.TipAmount && transactionPaymentsDTO.TipAmount != 0)
            {
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionPaymentsDTO.PaymentModeDTO.GatewayLookUp.ToString());
                if (paymentGateway.IsTipAllowed(inputTrxPaymentDTO) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4096));
                    //Sorry, tip adjustment is not allowed 
                }
            }
            log.LogMethodExit();
        }

        private void LoadPaymentModeDTO()
        {
            log.LogMethodEntry();
            this.transactionPaymentsDTO.PaymentModeDTO = (new PaymentMode(executionContext, this.transactionPaymentsDTO.PaymentModeId)).GetPaymentModeDTO;
            if (this.transactionPaymentsDTO.PaymentModeDTO != null && this.transactionPaymentsDTO.PaymentModeDTO.IsCreditCard)
            {
                string gateway = (new PaymentMode(executionContext, this.transactionPaymentsDTO.PaymentModeDTO)).Gateway;
                if (!string.IsNullOrEmpty(gateway) && Enum.IsDefined(typeof(PaymentGateways), gateway))
                    this.transactionPaymentsDTO.PaymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), gateway);
            }
            log.LogMethodExit();
        }

        private void ValidateEnabledAttributes()
        {
            log.LogMethodEntry();
            if (transactionPaymentsDTO.IsChanged)
            {
                if (this.transactionPaymentsDTO.PaymentModeDTO == null)
                {
                    LoadPaymentModeDTO();
                }
                List<TableAttributeDetailsDTO> attributeDTOList = GetTableAttributeDTOList();
                if (attributeDTOList != null && attributeDTOList.Any())
                {
                    SetDefaultValueIfMandatoryValueIsNotProvided(attributeDTOList);
                    try
                    {
                        Type tableAttributeUIHelper = Type.GetType("Semnox.Parafait.TableAttributeSetupUI.TableAttributesUIHelper, TableAttributeSetupUI");

                        MethodInfo setAttributeValuesInfo = tableAttributeUIHelper.GetMethod("SetAttributeValues",
                                                                        new[] { executionContext.GetType(), transactionPaymentsDTO.GetType(), attributeDTOList.GetType() });
                        attributeDTOList = (List<TableAttributeDetailsDTO>)setAttributeValuesInfo.Invoke(tableAttributeUIHelper,
                                                                       new object[] { executionContext, transactionPaymentsDTO, attributeDTOList });
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        if (ex.InnerException != null && string.IsNullOrWhiteSpace(ex.InnerException.Message) == false)
                        {
                            throw new ValidationException(ex.InnerException.Message);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    for (int i = 0; i < attributeDTOList.Count; i++)
                    {
                        ValidateTableAttributeDTO(attributeDTOList[i]);
                    }
                }
            }
            log.LogMethodExit();
        }

        private List<TableAttributeDetailsDTO> GetTableAttributeDTOList()
        {
            log.LogMethodEntry();
            Type tableAttributeDetailsListBLType = Type.GetType("Semnox.Parafait.TableAttributeDetailsUtils.TableAttributeDetailsListBL, TableAttributeDetailsUtils");
            object tableAttributeDetailsListBL = null;
            if (tableAttributeDetailsListBLType != null)
            {
                ConstructorInfo constructorN = tableAttributeDetailsListBLType.GetConstructor(new Type[] { executionContext.GetType() });
                tableAttributeDetailsListBL = constructorN.Invoke(new object[] { executionContext });
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "TableAttributeDetailsListBL"));
                //Unable to retrive &1 class from assembly
            }
            List<TableAttributeDetailsDTO> attributeDTOList = new List<TableAttributeDetailsDTO>();
            try
            {
                string paymentModeguid = this.transactionPaymentsDTO.PaymentModeDTO.Guid;
                MethodInfo tableAttributeDetailsListBLInfo = tableAttributeDetailsListBLType.GetMethod("GetTableAttributeDetailsDTOList",
                                                                new[] { EnabledAttributesDTO.TableWithEnabledAttributes.NONE.GetType(), paymentModeguid.GetType() });

                attributeDTOList = (List<TableAttributeDetailsDTO>)tableAttributeDetailsListBLInfo.Invoke(tableAttributeDetailsListBL,
                                                               new object[] { EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode, paymentModeguid });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (ex.InnerException != null && string.IsNullOrWhiteSpace(ex.InnerException.Message) == false)
                {
                    throw new ValidationException(ex.InnerException.Message);
                }
                else
                {
                    throw;
                }
            }
            log.LogMethodExit(attributeDTOList);
            return attributeDTOList;
        }

        private void ValidateTableAttributeDTO(TableAttributeDetailsDTO tableAttributeDetailsDTO)
        {
            log.LogMethodEntry(tableAttributeDetailsDTO);

            Type tableAttributeDetailBLType = Type.GetType("Semnox.Parafait.TableAttributeDetailsUtils.TableAttributeDetailsBL, TableAttributeDetailsUtils");
            object tableAttributeDetailBL = null;
            if (tableAttributeDetailBLType != null)
            {
                ConstructorInfo constructorN = tableAttributeDetailBLType.GetConstructor(new Type[] { executionContext.GetType(), tableAttributeDetailsDTO.GetType() });
                tableAttributeDetailBL = constructorN.Invoke(new object[] { executionContext, tableAttributeDetailsDTO });
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "TableAttributeDetailsBL"));
                //Unable to retrive &1 class from assembly
            }
            try
            {
                MethodInfo validateAttributeValueInfo = tableAttributeDetailBLType.GetMethod("ValidateAttributeValue");
                validateAttributeValueInfo.Invoke(tableAttributeDetailBL, new object[] { });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (ex.InnerException != null && string.IsNullOrWhiteSpace(ex.InnerException.Message) == false)
                {
                    throw new ValidationException(ex.InnerException.Message);
                }
                else
                {
                    throw;
                }
            }
            log.LogMethodExit();
        }
        public void SetDefaultValueIfMandatoryValueIsNotProvided(List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList)
        {
            log.LogMethodEntry(tableAttributeDetailsDTOList);
            if (tableAttributeDetailsDTOList != null && tableAttributeDetailsDTOList.Any())
            {
                for (int i = 0; i < tableAttributeDetailsDTOList.Count; i++)
                {
                    if (tableAttributeDetailsDTOList[i].MandatoryOrOptional == EnabledAttributesDTO.IsMandatoryOrOptional.Mandatory)
                    {
                        if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute1" && string.IsNullOrWhiteSpace(transactionPaymentsDTO.Attribute1))
                        {
                            transactionPaymentsDTO.Attribute1 = tableAttributeDetailsDTOList[i].DefaultAttributeValue;
                        }
                        else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute2" && string.IsNullOrWhiteSpace(transactionPaymentsDTO.Attribute2))
                        {
                            transactionPaymentsDTO.Attribute2 = tableAttributeDetailsDTOList[i].DefaultAttributeValue;
                        }
                        else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute3" && string.IsNullOrWhiteSpace(transactionPaymentsDTO.Attribute3))
                        {
                            transactionPaymentsDTO.Attribute3 = tableAttributeDetailsDTOList[i].DefaultAttributeValue;
                        }
                        else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute4" && string.IsNullOrWhiteSpace(transactionPaymentsDTO.Attribute4))
                        {
                            transactionPaymentsDTO.Attribute4 = tableAttributeDetailsDTOList[i].DefaultAttributeValue;
                        }
                        else if (tableAttributeDetailsDTOList[i].EnabledAttributeName == "Attribute5" && string.IsNullOrWhiteSpace(transactionPaymentsDTO.Attribute5))
                        {
                            transactionPaymentsDTO.Attribute5 = tableAttributeDetailsDTOList[i].DefaultAttributeValue;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        private void Update(TransactionPaymentsDTO parameterTransactionPaymentsDTO)
        {
            log.LogMethodEntry(parameterTransactionPaymentsDTO);
            ChangeId(parameterTransactionPaymentsDTO.PaymentId);
            ChangeTransactionId(parameterTransactionPaymentsDTO.TransactionId);
            ChangePaymentModeId(parameterTransactionPaymentsDTO.PaymentModeId, parameterTransactionPaymentsDTO.PaymentModeDTO);
            ChangeAmount(parameterTransactionPaymentsDTO.Amount);
            ChangeCreditCardNumber(parameterTransactionPaymentsDTO.CreditCardNumber);
            ChangeNameOnCreditCard(parameterTransactionPaymentsDTO.NameOnCreditCard);
            ChangeCreditCardName(parameterTransactionPaymentsDTO.CreditCardName);
            ChangeCreditCardExpiry(parameterTransactionPaymentsDTO.CreditCardExpiry);
            ChangeCreditCardAuthorization(parameterTransactionPaymentsDTO.CreditCardAuthorization);
            ChangeCardId(parameterTransactionPaymentsDTO.CardId);
            ChangeCardEntitlementType(parameterTransactionPaymentsDTO.CardEntitlementType);
            ChangeCardCreditPlusId(parameterTransactionPaymentsDTO.CardCreditPlusId);
            ChangePaymentUsedCreditPlus(parameterTransactionPaymentsDTO.PaymentUsedCreditPlus);
            ChangeOrderId(parameterTransactionPaymentsDTO.OrderId);
            ChangeReference(parameterTransactionPaymentsDTO.Reference);
            ChangeCCResponseId(parameterTransactionPaymentsDTO.CCResponseId);
            ChangeMemo(parameterTransactionPaymentsDTO.Memo);
            ChangeParentPaymentId(parameterTransactionPaymentsDTO.ParentPaymentId);
            ChangeTenderedAmount(parameterTransactionPaymentsDTO.TenderedAmount);
            ChangeTipAmount(parameterTransactionPaymentsDTO.TipAmount);
            ChangeSplitId(parameterTransactionPaymentsDTO.SplitId);
            ChangeCurrencyCode(parameterTransactionPaymentsDTO.CurrencyCode);
            ChangeCurrencyRate(parameterTransactionPaymentsDTO.CurrencyRate);
            ChangeCouponValue(parameterTransactionPaymentsDTO.CouponValue);
            ChangeCurrencyMcc(parameterTransactionPaymentsDTO.MCC);
            ChangeCouponSetId(parameterTransactionPaymentsDTO.CouponSetId);
            ChangePaymentCardNumber(parameterTransactionPaymentsDTO.PaymentCardNumber);
            ChangeisTaxable(parameterTransactionPaymentsDTO.IsTaxable);
            ChangeApprovedBy(parameterTransactionPaymentsDTO.ApprovedBy);
            ChangeSubscriptionAuthorizationMode(parameterTransactionPaymentsDTO.SubscriptionAuthorizationMode);
            ChangeCustomerCardProfileId(parameterTransactionPaymentsDTO.CustomerCardProfileId);
            ChangeExternalSourceReference(parameterTransactionPaymentsDTO.ExternalSourceReference);
            ChangeGatewayPaymentProcessed(parameterTransactionPaymentsDTO.GatewayPaymentProcessed);
            ChangeAttribute1(parameterTransactionPaymentsDTO.Attribute1);
            ChangeAttribute2(parameterTransactionPaymentsDTO.Attribute2);
            ChangeAttribute3(parameterTransactionPaymentsDTO.Attribute3);
            ChangeAttribute4(parameterTransactionPaymentsDTO.Attribute4);
            ChangeAttribute5(parameterTransactionPaymentsDTO.Attribute5);
            log.LogMethodExit();
        }
        private void ChangeId(int id)
        {
            log.LogMethodEntry(id);
            if (transactionPaymentsDTO.PaymentId != id)
            {
                transactionPaymentsDTO.PaymentId = id;
            }
            log.LogMethodExit();
        }
        private void ChangeTransactionId(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            if (transactionPaymentsDTO.TransactionId != transactionId)
            {
                transactionPaymentsDTO.TransactionId = transactionId;
            }
            log.LogMethodExit();
        }
        private void ChangePaymentModeId(int paymentModeId, PaymentModeDTO paymentModeDTO)
        {
            log.LogMethodEntry(paymentModeId, paymentModeDTO);
            if (transactionPaymentsDTO.PaymentModeId != paymentModeId)
            {
                transactionPaymentsDTO.PaymentModeId = paymentModeId;
                if (paymentModeDTO != null && paymentModeDTO.PaymentModeId == paymentModeId)
                {
                    transactionPaymentsDTO.PaymentModeDTO = paymentModeDTO;
                }
            }
            log.LogMethodExit();
        }
        private void ChangeAmount(double amount)
        {

            log.LogMethodEntry(amount);
            if (transactionPaymentsDTO.Amount != amount)
            {
                transactionPaymentsDTO.Amount = amount;
            }
            log.LogMethodExit();
        }
        private void ChangeCreditCardNumber(string creditCardNumber)
        {
            log.LogMethodEntry(creditCardNumber);
            if (transactionPaymentsDTO.CreditCardNumber != creditCardNumber)
            {
                transactionPaymentsDTO.CreditCardNumber = creditCardNumber;
            }
            log.LogMethodExit();
        }
        private void ChangeNameOnCreditCard(string nameOnCreditCard)
        {
            log.LogMethodEntry(nameOnCreditCard);
            if (transactionPaymentsDTO.NameOnCreditCard != nameOnCreditCard)
            {
                transactionPaymentsDTO.NameOnCreditCard = nameOnCreditCard;
            }
            log.LogMethodExit();
        }
        private void ChangeCreditCardName(string creditCardName)
        {
            log.LogMethodEntry(creditCardName);
            if (transactionPaymentsDTO.CreditCardName != creditCardName)
            {
                transactionPaymentsDTO.CreditCardName = creditCardName;
            }
            log.LogMethodExit();
        }
        private void ChangeCreditCardExpiry(string creditCardExpiry)
        {
            log.LogMethodEntry(creditCardExpiry);
            if (transactionPaymentsDTO.CreditCardExpiry != creditCardExpiry)
            {
                transactionPaymentsDTO.CreditCardExpiry = creditCardExpiry;
            }
            log.LogMethodExit();
        }
        private void ChangeCreditCardAuthorization(string creditCardAuthorization)
        {
            log.LogMethodEntry(creditCardAuthorization);
            if (transactionPaymentsDTO.CreditCardAuthorization != creditCardAuthorization)
            {
                transactionPaymentsDTO.CreditCardAuthorization = creditCardAuthorization;
            }
            log.LogMethodExit();
        }
        private void ChangeCardId(int cardId)
        {
            log.LogMethodEntry(cardId);
            if (transactionPaymentsDTO.CardId != cardId)
            {
                transactionPaymentsDTO.CardId = cardId;
            }
            log.LogMethodExit();
        }
        private void ChangeCardEntitlementType(string cardEntitlementType)
        {
            log.LogMethodEntry(cardEntitlementType);
            if (transactionPaymentsDTO.CardEntitlementType != cardEntitlementType)
            {
                transactionPaymentsDTO.CardEntitlementType = cardEntitlementType;
            }
            log.LogMethodExit();
        }
        private void ChangeCardCreditPlusId(int cardCreditPlusId)
        {
            log.LogMethodEntry(cardCreditPlusId);
            if (transactionPaymentsDTO.CardCreditPlusId != cardCreditPlusId)
            {
                transactionPaymentsDTO.CardCreditPlusId = cardCreditPlusId;
            }
            log.LogMethodExit();
        }
        private void ChangePaymentUsedCreditPlus(double paymentUsedCreditPlus)
        {
            log.LogMethodEntry(paymentUsedCreditPlus);
            if (transactionPaymentsDTO.PaymentUsedCreditPlus != paymentUsedCreditPlus)
            {
                transactionPaymentsDTO.PaymentUsedCreditPlus = paymentUsedCreditPlus;
            }
            log.LogMethodExit();
        }
        private void ChangeOrderId(int orderId)
        {
            log.LogMethodEntry(orderId);
            if (transactionPaymentsDTO.OrderId != orderId)
            {
                transactionPaymentsDTO.OrderId = orderId;
            }
            log.LogMethodExit();
        }
        private void ChangeReference(string reference)
        {
            log.LogMethodEntry(reference);
            if (transactionPaymentsDTO.Reference != reference)
            {
                transactionPaymentsDTO.Reference = reference;
            }
            log.LogMethodExit();
        }
        private void ChangeCCResponseId(int cCResponseId)
        {
            log.LogMethodEntry(cCResponseId);
            if (transactionPaymentsDTO.CCResponseId != cCResponseId)
            {
                transactionPaymentsDTO.CCResponseId = cCResponseId;
            }
            log.LogMethodExit();
        }
        private void ChangeMemo(string memo)
        {
            log.LogMethodEntry(memo);
            if (transactionPaymentsDTO.Memo != memo)
            {
                transactionPaymentsDTO.Memo = memo;
            }
            log.LogMethodExit();
        }
        private void ChangeParentPaymentId(int parentPaymentId)
        {
            log.LogMethodEntry(parentPaymentId);
            if (transactionPaymentsDTO.ParentPaymentId != parentPaymentId)
            {
                transactionPaymentsDTO.ParentPaymentId = parentPaymentId;
            }
            log.LogMethodExit();
        }
        private void ChangeTenderedAmount(double? tenderedAmount)
        {
            log.LogMethodEntry(tenderedAmount);
            if (transactionPaymentsDTO.TenderedAmount != tenderedAmount)
            {
                transactionPaymentsDTO.TenderedAmount = tenderedAmount;
            }
            log.LogMethodExit();
        }
        private void ChangeTipAmount(double tipAmount)
        {
            log.LogMethodEntry(tipAmount);
            if (transactionPaymentsDTO.TipAmount != tipAmount)
            {
                transactionPaymentsDTO.TipAmount = tipAmount;
            }
            log.LogMethodExit();
        }
        private void ChangeSplitId(int splitId)
        {
            log.LogMethodEntry(splitId);
            if (transactionPaymentsDTO.SplitId != splitId)
            {
                transactionPaymentsDTO.SplitId = splitId;
            }
            log.LogMethodExit();
        }
        private void ChangeCurrencyCode(string currencyCode)
        {
            log.LogMethodEntry(currencyCode);
            if (transactionPaymentsDTO.CurrencyCode != currencyCode)
            {
                transactionPaymentsDTO.CurrencyCode = currencyCode;
            }
            log.LogMethodExit();
        }
        private void ChangeCurrencyRate(double? currencyRate)
        {
            log.LogMethodEntry(currencyRate);
            if (transactionPaymentsDTO.CurrencyRate != currencyRate)
            {
                transactionPaymentsDTO.CurrencyRate = currencyRate;
            }
            log.LogMethodExit();
        }
        private void ChangeCouponValue(double? couponValue)
        {
            log.LogMethodEntry(couponValue);
            if (transactionPaymentsDTO.CouponValue != couponValue)
            {
                transactionPaymentsDTO.CouponValue = couponValue;
            }
            log.LogMethodExit();
        }
        private void ChangeCurrencyMcc(string mcc)
        {
            log.LogMethodEntry(mcc);
            if (transactionPaymentsDTO.MCC != mcc)
            {
                transactionPaymentsDTO.MCC = mcc;
            }
            log.LogMethodExit();
        }
        private void ChangeCouponSetId(int couponSetId)
        {
            log.LogMethodEntry(couponSetId);
            if (transactionPaymentsDTO.CouponSetId != couponSetId)
            {
                transactionPaymentsDTO.CouponSetId = couponSetId;
            }
            log.LogMethodExit();
        }
        private void ChangePaymentCardNumber(string paymentCardNumber)
        {
            log.LogMethodEntry(paymentCardNumber);
            if (transactionPaymentsDTO.PaymentCardNumber != paymentCardNumber)
            {
                transactionPaymentsDTO.PaymentCardNumber = paymentCardNumber;
            }
            log.LogMethodExit();
        }
        private void ChangeisTaxable(bool? isTaxable)
        {
            log.LogMethodEntry(isTaxable);
            if (transactionPaymentsDTO.IsTaxable != isTaxable)
            {
                transactionPaymentsDTO.IsTaxable = isTaxable;
            }
            log.LogMethodExit();
        }
        private void ChangeApprovedBy(string approvedBy)
        {
            log.LogMethodEntry(approvedBy);
            if (transactionPaymentsDTO.ApprovedBy != approvedBy)
            {
                transactionPaymentsDTO.ApprovedBy = approvedBy;
            }
            log.LogMethodExit();
        }
        private void ChangeSubscriptionAuthorizationMode(SubscriptionAuthorizationMode subscriptionAuthorizationMode)
        {
            log.LogMethodEntry(subscriptionAuthorizationMode);
            if (transactionPaymentsDTO.SubscriptionAuthorizationMode != subscriptionAuthorizationMode)
            {
                transactionPaymentsDTO.SubscriptionAuthorizationMode = subscriptionAuthorizationMode;
            }
            log.LogMethodExit();
        }
        private void ChangeCustomerCardProfileId(string customerCardProfileId)
        {
            log.LogMethodEntry(customerCardProfileId);
            if (transactionPaymentsDTO.CustomerCardProfileId != customerCardProfileId)
            {
                transactionPaymentsDTO.CustomerCardProfileId = customerCardProfileId;
            }
            log.LogMethodExit();
        }
        private void ChangeExternalSourceReference(string externalSourceReference)
        {
            log.LogMethodEntry(externalSourceReference);
            if (transactionPaymentsDTO.ExternalSourceReference != externalSourceReference)
            {
                transactionPaymentsDTO.ExternalSourceReference = externalSourceReference;
            }
            log.LogMethodExit();
        }
        private void ChangeGatewayPaymentProcessed(bool gatewayPaymentProcessed)
        {
            log.LogMethodEntry(gatewayPaymentProcessed);
            if (transactionPaymentsDTO.GatewayPaymentProcessed != gatewayPaymentProcessed)
            {
                transactionPaymentsDTO.GatewayPaymentProcessed = gatewayPaymentProcessed;
            }
            log.LogMethodExit();
        }
        private void ChangeAttribute1(string attribute1)
        {
            log.LogMethodEntry(attribute1);
            if (transactionPaymentsDTO.Attribute1 != attribute1)
            {
                transactionPaymentsDTO.Attribute1 = attribute1;
            }
            log.LogMethodExit();
        }
        private void ChangeAttribute2(string attribute2)
        {
            log.LogMethodEntry(attribute2);
            if (transactionPaymentsDTO.Attribute2 != attribute2)
            {
                transactionPaymentsDTO.Attribute2 = attribute2;
            }
            log.LogMethodExit();
        }
        private void ChangeAttribute3(string attribute3)
        {
            log.LogMethodEntry(attribute3);
            if (transactionPaymentsDTO.Attribute3 != attribute3)
            {
                transactionPaymentsDTO.Attribute3 = attribute3;
            }
            log.LogMethodExit();
        }
        private void ChangeAttribute4(string attribute4)
        {
            log.LogMethodEntry(attribute4);
            if (transactionPaymentsDTO.Attribute4 != attribute4)
            {
                transactionPaymentsDTO.Attribute4 = attribute4;
            }
            log.LogMethodExit();
        }
        private void ChangeAttribute5(string attribute5)
        {
            log.LogMethodEntry(attribute5);
            if (transactionPaymentsDTO.Attribute5 != attribute5)
            {
                transactionPaymentsDTO.Attribute5 = attribute5;
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of TransactionPayments
    /// </summary>
    public class TransactionPaymentsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TransactionPaymentsDTO> transactionPaymentsDTOList;
        private ExecutionContext executionContext;

        /// <summary>
        /// TransactionPaymentsDTOList Constructor
        /// </summary>
        public TransactionPaymentsListBL()
        {
            log.LogMethodEntry();
            this.transactionPaymentsDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor to set Execution context
        /// </summary>
        /// <param name="executioncontext"></param>
        public TransactionPaymentsListBL(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executionContext);
            this.transactionPaymentsDTOList = null;
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor to set TransactionPaymentsDTOList and ExecutionContext
        /// </summary>
        /// <param name="transactionPaymentsDTOList"></param>
        /// <param name="executioncontext"></param>
        public TransactionPaymentsListBL(ExecutionContext executioncontext, List<TransactionPaymentsDTO> transactionPaymentsDTOList)
            : this(executioncontext)
        {
            log.LogMethodEntry(executioncontext, transactionPaymentsDTOList);
            this.executionContext = executioncontext;
            this.transactionPaymentsDTOList = transactionPaymentsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TransactionPayments list
        /// </summary>
        /// <param name="searchParameters">Search Parameters</param>
        /// <param name="sqlTransaction">Sql Transaction if transaction managed outside</param>
        public List<TransactionPaymentsDTO> GetTransactionPaymentsDTOList(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsDataHandler.GetTransactionPaymentsDTOList(searchParameters);
            if (transactionPaymentsDTOList != null)
            {
                foreach (TransactionPaymentsDTO trxPaymentsDTO in transactionPaymentsDTOList)
                {
                    trxPaymentsDTO.PaymentModeDTO = (new PaymentMode(executionContext, trxPaymentsDTO.PaymentModeId)).GetPaymentModeDTO;
                }
            }
            log.LogMethodExit(transactionPaymentsDTOList);
            return transactionPaymentsDTOList;
        }

        /// <summary>
        /// Returns the non reversed TransactionPayments list
        /// </summary>
        /// <param name="cCResponseIdList">Specific Response list passed</param>
        /// <param name="searchParameters">Search Parameters</param>
        /// <param name="sqlTransaction">SQL Transaction if managed outside</param>
        public List<TransactionPaymentsDTO> GetNonReversedTransactionPaymentsDTOList(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters, List<int> cCResponseIdList = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, cCResponseIdList, sqlTransaction);
            TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsDataHandler.GetNonReversedTransactionPaymentsDTOList(searchParameters, cCResponseIdList);
            if (transactionPaymentsDTOList != null)
            {
                foreach (TransactionPaymentsDTO trxPaymentsDTO in transactionPaymentsDTOList)
                {
                    trxPaymentsDTO.PaymentModeDTO = (new PaymentMode(executionContext, trxPaymentsDTO.PaymentModeId)).GetPaymentModeDTO;
                    if (trxPaymentsDTO.PaymentModeDTO != null && trxPaymentsDTO.PaymentModeDTO.IsCreditCard)
                    {
                        string gateway = (new PaymentMode(executionContext, trxPaymentsDTO.PaymentModeDTO)).Gateway;
                        if (!string.IsNullOrEmpty(gateway) && Enum.IsDefined(typeof(PaymentGateways), gateway))
                            trxPaymentsDTO.PaymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), gateway);
                    }
                }
            }
            log.LogMethodExit(transactionPaymentsDTOList);
            return transactionPaymentsDTOList;
        }

        /// <summary>
        /// Returns the non reversed TransactionPayments list for Reversal entry
        /// </summary>
        /// <param name="cCResponseIdList">Specific Response list passed</param>
        /// <param name="searchParameters">Search Parameters</param>
        /// <param name="sqlTransaction">SQL Transaction if managed outside</param>
        public List<TransactionPaymentsDTO> GetNonReversedTransactionPaymentsDTOListForReversal(List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters, List<int> cCResponseIdList = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, cCResponseIdList, sqlTransaction);
            TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsDataHandler.GetNonReversedTransactionPaymentsDTOListForReversal(searchParameters, cCResponseIdList);
            if (transactionPaymentsDTOList != null)
            {
                foreach (TransactionPaymentsDTO trxPaymentsDTO in transactionPaymentsDTOList)
                {
                    trxPaymentsDTO.PaymentModeDTO = (new PaymentMode(executionContext, trxPaymentsDTO.PaymentModeId)).GetPaymentModeDTO;
                    if (trxPaymentsDTO.PaymentModeDTO != null && trxPaymentsDTO.PaymentModeDTO.IsCreditCard)
                    {
                        string gateway = (new PaymentMode(executionContext, trxPaymentsDTO.PaymentModeDTO)).Gateway;
                        if (!string.IsNullOrEmpty(gateway) && Enum.IsDefined(typeof(PaymentGateways), gateway))
                            trxPaymentsDTO.PaymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), gateway);
                    }
                }
            }
            log.LogMethodExit(transactionPaymentsDTOList);
            return transactionPaymentsDTOList;
        }

        /// <summary>
        /// Method to clear PaymentId in case payment does not exist in DB. 
        /// This can happen if after DTO update, roll back happens for the transaction
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void CleanupPaymentList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
            {
                List<TransactionPaymentsDTO> savedTrxPaymentDTOList = transactionPaymentsDTOList.Where(tp => tp.PaymentId > -1).ToList();
                List<int> trxPaymentIdList = new List<int>();
                if (savedTrxPaymentDTOList != null && savedTrxPaymentDTOList.Any())
                {
                    trxPaymentIdList = savedTrxPaymentDTOList.Select(tp => tp.PaymentId).Distinct().ToList();
                }
                List<TransactionPaymentsDTO> dbTransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                if (trxPaymentIdList != null && trxPaymentIdList.Any())
                {

                    TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
                    dbTransactionPaymentsDTOList = transactionPaymentsDataHandler.GetTransactionPaymentsDTOList(trxPaymentIdList);
                }
                foreach (TransactionPaymentsDTO trxPaymentsDTO in transactionPaymentsDTOList)
                {
                    //TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
                    //List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    //trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_ID, trxPaymentsDTO.PaymentId.ToString()));
                    //List<TransactionPaymentsDTO> dbTransactionPaymentsDTOList = transactionPaymentsDataHandler.GetTransactionPaymentsDTOList(trxPaymentSearchParameters);
                    if (trxPaymentsDTO.PaymentId > -1 &&
                        ((dbTransactionPaymentsDTOList == null || dbTransactionPaymentsDTOList.Any() == false)
                         || (dbTransactionPaymentsDTOList.Any() && dbTransactionPaymentsDTOList.Exists(tp => tp.PaymentId == trxPaymentsDTO.PaymentId) == false)))
                    {
                        trxPaymentsDTO.PaymentId = -1;
                    }
                }
            }
            log.LogMethodExit();
        }
        public void UpdateTransactionPaymentModeDetails()
        {
            log.LogMethodEntry();
            ValidatePaymentDetails();
            foreach (TransactionPaymentsDTO modifiedTrxPaymentsDTO in transactionPaymentsDTOList)
            {
                TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(executionContext, modifiedTrxPaymentsDTO.PaymentId);
                TransactionPaymentsDTO originalTransactionPaymentsDTO = transactionPaymentsBL.TransactionPaymentsDTO;
                if (originalTransactionPaymentsDTO != null)
                {
                    if (originalTransactionPaymentsDTO.PaymentModeId != modifiedTrxPaymentsDTO.PaymentModeId)
                    {
                        TransactionPaymentsDTO existingTransactionPaymentsDTO = new TransactionPaymentsDTO(-1, originalTransactionPaymentsDTO.TransactionId,
                        originalTransactionPaymentsDTO.PaymentModeId, -originalTransactionPaymentsDTO.Amount, originalTransactionPaymentsDTO.CreditCardNumber, originalTransactionPaymentsDTO.NameOnCreditCard, originalTransactionPaymentsDTO.CreditCardName,
                        originalTransactionPaymentsDTO.CreditCardExpiry, originalTransactionPaymentsDTO.CreditCardAuthorization, originalTransactionPaymentsDTO.CardId, originalTransactionPaymentsDTO.CardEntitlementType,
                        originalTransactionPaymentsDTO.CardCreditPlusId, originalTransactionPaymentsDTO.OrderId, originalTransactionPaymentsDTO.Reference, null, originalTransactionPaymentsDTO.SynchStatus,
                        executionContext.GetSiteId(), originalTransactionPaymentsDTO.CCResponseId, originalTransactionPaymentsDTO.Memo, originalTransactionPaymentsDTO.PaymentDate, executionContext.GetUserId(),
                        originalTransactionPaymentsDTO.PaymentId, originalTransactionPaymentsDTO.TenderedAmount, originalTransactionPaymentsDTO.TipAmount, originalTransactionPaymentsDTO.SplitId, originalTransactionPaymentsDTO.PosMachine,
                        originalTransactionPaymentsDTO.MasterEntityId, originalTransactionPaymentsDTO.CurrencyCode, originalTransactionPaymentsDTO.CurrencyRate, originalTransactionPaymentsDTO.IsTaxable, originalTransactionPaymentsDTO.CouponValue,
                        executionContext.GetUserId(), DateTime.Now, DateTime.Now, originalTransactionPaymentsDTO.GatewayPaymentProcessed, originalTransactionPaymentsDTO.ApprovedBy, originalTransactionPaymentsDTO.CustomerCardProfileId,
                        originalTransactionPaymentsDTO.ExternalSourceReference, originalTransactionPaymentsDTO.Attribute1, originalTransactionPaymentsDTO.Attribute2, originalTransactionPaymentsDTO.Attribute3, originalTransactionPaymentsDTO.Attribute4,
                        originalTransactionPaymentsDTO.Attribute5, originalTransactionPaymentsDTO.PaymentModeOTP);

                        TransactionPaymentsBL newtransactionPaymentsBL = new TransactionPaymentsBL(executionContext, existingTransactionPaymentsDTO);
                        newtransactionPaymentsBL.Save();

                       TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, modifiedTrxPaymentsDTO.TransactionId,
                       modifiedTrxPaymentsDTO.PaymentModeId, modifiedTrxPaymentsDTO.Amount, modifiedTrxPaymentsDTO.CreditCardNumber, modifiedTrxPaymentsDTO.NameOnCreditCard, modifiedTrxPaymentsDTO.CreditCardName,
                       modifiedTrxPaymentsDTO.CreditCardExpiry, modifiedTrxPaymentsDTO.CreditCardAuthorization, modifiedTrxPaymentsDTO.CardId, modifiedTrxPaymentsDTO.CardEntitlementType,
                       modifiedTrxPaymentsDTO.CardCreditPlusId, modifiedTrxPaymentsDTO.OrderId, modifiedTrxPaymentsDTO.Reference, null, modifiedTrxPaymentsDTO.SynchStatus,
                       executionContext.GetSiteId(), modifiedTrxPaymentsDTO.CCResponseId, modifiedTrxPaymentsDTO.Memo, modifiedTrxPaymentsDTO.PaymentDate, executionContext.GetUserId(),
                       modifiedTrxPaymentsDTO.ParentPaymentId, modifiedTrxPaymentsDTO.TenderedAmount, modifiedTrxPaymentsDTO.TipAmount, modifiedTrxPaymentsDTO.SplitId, modifiedTrxPaymentsDTO.PosMachine,
                       modifiedTrxPaymentsDTO.MasterEntityId, modifiedTrxPaymentsDTO.CurrencyCode, modifiedTrxPaymentsDTO.CurrencyRate, modifiedTrxPaymentsDTO.IsTaxable, modifiedTrxPaymentsDTO.CouponValue,
                       executionContext.GetUserId(), DateTime.Now, DateTime.Now, modifiedTrxPaymentsDTO.GatewayPaymentProcessed, modifiedTrxPaymentsDTO.ApprovedBy, modifiedTrxPaymentsDTO.CustomerCardProfileId,
                       modifiedTrxPaymentsDTO.ExternalSourceReference, modifiedTrxPaymentsDTO.Attribute1, modifiedTrxPaymentsDTO.Attribute2, modifiedTrxPaymentsDTO.Attribute3, modifiedTrxPaymentsDTO.Attribute4,
                       modifiedTrxPaymentsDTO.Attribute5, modifiedTrxPaymentsDTO.PaymentModeOTP);
                        TransactionPaymentsBL newtrxPaymentsBL = new TransactionPaymentsBL(executionContext, transactionPaymentsDTO);
                        newtrxPaymentsBL.Save();
                    }
                    else
                    {
                       transactionPaymentsBL = new TransactionPaymentsBL(executionContext, modifiedTrxPaymentsDTO);
                       transactionPaymentsBL.Save();
                    }
                }
                else
                {
                    log.Info("Payment mode is not modified, skipping update");
                }
            }
            log.LogMethodExit();
        }
        private void ValidatePaymentDetails()
        {
            log.LogMethodEntry();
            if (transactionPaymentsDTOList == null || transactionPaymentsDTOList.Any() == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "No records to update"));
            }
            foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOList)
            {
                PaymentMode paymentMode = new PaymentMode(executionContext, transactionPaymentsDTO.PaymentModeId);
                PaymentModeDTO paymentModeDTO = paymentMode.GetPaymentModeDTO;
                if (paymentModeDTO.IsCreditCard)
                {
                    if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName) ||
                        string.IsNullOrEmpty(transactionPaymentsDTO.NameOnCreditCard) ||
                        string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber) ||
                        string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardAuthorization))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4110));
                        //"Credit Card and Authorization fields are not provided for the Credit card payment details"
                    }
                    if (!string.IsNullOrEmpty(transactionPaymentsDTO.Reference))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4111));
                        //Reference field not required for the Credit card payment details
                    }
                }
                else if (paymentModeDTO.IsCash)
                {
                    if (!string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName) ||
                        !string.IsNullOrEmpty(transactionPaymentsDTO.NameOnCreditCard) ||
                        !string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber) ||
                        !string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardAuthorization) ||
                        !string.IsNullOrEmpty(transactionPaymentsDTO.Reference))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4112));
                        //Card related , authorization and  Reference fields are not required for the Cash payment details
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName) ||
                       !string.IsNullOrEmpty(transactionPaymentsDTO.NameOnCreditCard) ||
                       !string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber) ||
                       !string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardAuthorization))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4113));
                        //Card related, authorization are not required for the payment details
                    }
                    if (string.IsNullOrEmpty(transactionPaymentsDTO.Reference))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4114));
                        //Reference field is required for the payment details
                    }
                }
                if (paymentModeDTO.IsCreditCard && transactionPaymentsDTO.CCResponseId > -1)
                {
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    if (cCTransactionsPGWBL.CCTransactionsPGWDTO.TranCode == "Authorization")
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4115));
                        //Payment is pending for settlement                        
                    }
                }

            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TransactionPaymentsDTO list
        /// </summary>
        public List<TransactionPaymentsDTO> TransactionPaymentsDTOList { get { return transactionPaymentsDTOList; } }
        public List<TransactionPaymentsDTO> GetTransactionPayments(List<int> transactionIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionIdList, sqlTransaction);
            TransactionPaymentsDataHandler transactionPaymentsDataHandler = new TransactionPaymentsDataHandler(sqlTransaction);
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsDataHandler.GetTransactionPaymentLines(transactionIdList);
            log.LogMethodExit(transactionPaymentsDTOList);
            if (transactionPaymentsDTOList != null)
            {
                foreach (TransactionPaymentsDTO trxPaymentsDTO in transactionPaymentsDTOList)
                {
                    trxPaymentsDTO.paymentModeDTO = (new PaymentMode(executionContext, trxPaymentsDTO.PaymentModeId)).GetPaymentModeDTO;
                }
            }
            return transactionPaymentsDTOList;
        }
    }
}
