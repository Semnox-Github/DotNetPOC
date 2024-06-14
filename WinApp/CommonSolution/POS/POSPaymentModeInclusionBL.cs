/********************************************************************************************
 * Project Name - POS
 * Description  - BL object of POSPaymentModeInclusion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.90.0        13-Jun-2020   Girish kundar       Created
 *********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class POSPaymentModeInclusionBL
    {
        private POSPaymentModeInclusionDTO posPaymentModeInclusionDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of POSPaymentModeInclusionBL class
        /// </summary>
        private POSPaymentModeInclusionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with th  POSPaymentModeInclusion id as the parameter
        /// Would fetch the Customer Feedback Response object from the database based on the id passed. 
        /// </summary>
        /// <param name="CustFbResponseId">Customer Feedback Response id</param>
        public POSPaymentModeInclusionBL(ExecutionContext executionContext, int posPaymentModeInclusionId, bool activeRecords = true, bool loadPaymentModes = false, 
                                           SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posPaymentModeInclusionId);
            POSPaymentModeInclusionDataHandler posPaymentModeInclusionDataHandler = new POSPaymentModeInclusionDataHandler(sqlTransaction);
            posPaymentModeInclusionDTO = posPaymentModeInclusionDataHandler.GetPOSPaymentModeInclusionDTO(posPaymentModeInclusionId);
            if (posPaymentModeInclusionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSPaymentModeInclusion", posPaymentModeInclusionId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadPaymentModes)
            {
                Build(activeRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeRecords, sqlTransaction);
            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, posPaymentModeInclusionDTO.PaymentModeId.ToString()));
            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
            if (paymentModeDTOList != null && paymentModeDTOList.Any())
            {
                posPaymentModeInclusionDTO.PaymentModeDTO = paymentModeDTOList[0];
            }
            log.LogMethodExit(posPaymentModeInclusionDTO);
        }
        public POSPaymentModeInclusionBL(ExecutionContext executionContext, POSPaymentModeInclusionDTO posPaymentModeInclusionDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posPaymentModeInclusionDTO);
            this.posPaymentModeInclusionDTO = posPaymentModeInclusionDTO;
            log.LogMethodExit();
        }

        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            POSPaymentModeInclusionDataHandler posPaymentModeInclusionDataHandler = new POSPaymentModeInclusionDataHandler(sqlTransaction);
            if (posPaymentModeInclusionDTO.IsChanged == false
                 && posPaymentModeInclusionDTO.POSPaymentModeInclusionId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            if (posPaymentModeInclusionDTO.IsActive)
            {
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (posPaymentModeInclusionDTO.POSPaymentModeInclusionId < 0)
                {
                    posPaymentModeInclusionDTO = posPaymentModeInclusionDataHandler.Insert(posPaymentModeInclusionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    posPaymentModeInclusionDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(posPaymentModeInclusionDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("POSPaymentModeInclusions", posPaymentModeInclusionDTO.Guid);
                    }
                }
                else
                {
                    if (posPaymentModeInclusionDTO.IsChanged)
                    {
                        posPaymentModeInclusionDTO = posPaymentModeInclusionDataHandler.Update(posPaymentModeInclusionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        posPaymentModeInclusionDTO.AcceptChanges();
                        if (!string.IsNullOrEmpty(posPaymentModeInclusionDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("POSPaymentModeInclusions", posPaymentModeInclusionDTO.Guid);
                        }
                    }
                }
            }
            else
            {
                if (posPaymentModeInclusionDTO.POSPaymentModeInclusionId >= 0)
                {
                    posPaymentModeInclusionDataHandler.Delete(posPaymentModeInclusionDTO.POSPaymentModeInclusionId);
                }
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public POSPaymentModeInclusionDTO POSPaymentModeInclusionDTO
        {
            get
            {
                return posPaymentModeInclusionDTO;
            }
        }
    }

    public class POSPaymentModeInclusionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSPaymentModeInclusionDTO> posPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSPaymentModeInclusionListBL( )
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public POSPaymentModeInclusionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public POSPaymentModeInclusionListBL(ExecutionContext executionContext,
                                             List<POSPaymentModeInclusionDTO> posPaymentModeInclusionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.posPaymentModeInclusionDTOList = posPaymentModeInclusionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the POSPaymentModeInclusionDTO list
        /// </summary>
        public List<POSPaymentModeInclusionDTO> GetPOSPaymentModeInclusionDTOList(List<KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string>> searchParameters, bool loadPaymentMode = false,
                                               SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadPaymentMode, sqlTransaction);
            POSPaymentModeInclusionDataHandler posPaymentModeInclusionDataHandler = new POSPaymentModeInclusionDataHandler(sqlTransaction);
            List<POSPaymentModeInclusionDTO> posPaymentModeInclusionDTOList = posPaymentModeInclusionDataHandler.GetPOSPaymentModeInclusionDTOList(searchParameters);
            if (posPaymentModeInclusionDTOList != null && posPaymentModeInclusionDTOList.Any() && loadPaymentMode)
            {
                foreach (POSPaymentModeInclusionDTO posPaymentModeInclusionDTO in posPaymentModeInclusionDTOList)
                {
                    PaymentMode paymentModeBL = new PaymentMode(executionContext, posPaymentModeInclusionDTO.PaymentModeId, sqlTransaction);
                    if (paymentModeBL.GetPaymentModeDTO != null)
                    {
                        posPaymentModeInclusionDTO.PaymentModeDTO = paymentModeBL.GetPaymentModeDTO;
                    }
                }
            }

            log.LogMethodExit(posPaymentModeInclusionDTOList);
            return posPaymentModeInclusionDTOList;
        }

        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (posPaymentModeInclusionDTOList == null ||
                posPaymentModeInclusionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < posPaymentModeInclusionDTOList.Count; i++)
            {
                POSPaymentModeInclusionDTO posPaymentModeInclusionDTO = posPaymentModeInclusionDTOList[i];
                //posPaymentModeInclusionDTO.IsChanged = true;
                if (posPaymentModeInclusionDTO.IsChanged== false)
                {
                    continue;
                }
                try
                {
                    POSPaymentModeInclusionBL posPaymentModeInclusionBL = new POSPaymentModeInclusionBL(executionContext, posPaymentModeInclusionDTO);
                    posPaymentModeInclusionBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving posPaymentModeInclusion DTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("posPaymentModeInclusion", posPaymentModeInclusionDTO);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the POSPaymentModeInclusionDTO List for POSMachineIdList
        /// </summary>
        /// <param name="pOSMachineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of POSPaymentModeInclusionDTO</returns>
        public List<POSPaymentModeInclusionDTO> GetPOSPaymentModeInclusionDTOList(List<int> pOSMachineIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(pOSMachineIdList, activeRecords, sqlTransaction);
            POSPaymentModeInclusionDataHandler pOSPaymentModeInclusionDataHandler = new POSPaymentModeInclusionDataHandler(sqlTransaction);
            List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = pOSPaymentModeInclusionDataHandler.GetPOSPaymentModeInclusionDTOList(pOSMachineIdList, activeRecords);
            log.LogMethodExit(pOSPaymentModeInclusionDTOList);
            return pOSPaymentModeInclusionDTOList;
        }
    }
}

