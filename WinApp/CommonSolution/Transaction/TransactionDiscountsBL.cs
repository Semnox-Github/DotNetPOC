/********************************************************************************************
 * Project Name - TransactionDiscounts BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017      Lakshminarayana     Created 
 *1.01        30-Oct-2017      Lakshminarayana     Modified   Option to choose generated coupons to sequential or random, Allow multiple coupons in one transaction 
 *2.80        31-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
//using Semnox.Parafait.DiscountCouponsUsed;

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for TransactionDiscounts class.
    /// </summary>
    public class TransactionDiscountsBL
    {
        private TransactionDiscountsDTO transactionDiscountsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of TransactionDiscountsBL class
        /// </summary>
        private TransactionDiscountsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            transactionDiscountsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        

        /// <summary>
        /// Creates TransactionDiscountsBL object using the TransactionDiscountsDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transactionDiscountsDTO">TransactionDiscountsDTO object</param>
        public TransactionDiscountsBL(ExecutionContext executionContext, TransactionDiscountsDTO transactionDiscountsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(transactionDiscountsDTO);
            this.transactionDiscountsDTO = transactionDiscountsDTO;
            log.LogMethodExit();
        }

        

        /// <summary>
        /// Saves the TransactionDiscounts
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            TransactionDiscountsDataHandler transactionDiscountsDataHandler = new TransactionDiscountsDataHandler(sqlTransaction);
            if (transactionDiscountsDTO.TransactionDiscountId < 0)
            {
                log.LogVariableState("KDSOrderLineDTO", transactionDiscountsDTO);
                transactionDiscountsDTO = transactionDiscountsDataHandler.InsertTransactionDiscounts(transactionDiscountsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                transactionDiscountsDTO.AcceptChanges();
            }
            else
            {
                if (transactionDiscountsDTO.IsChanged)
                {
                    transactionDiscountsDTO = transactionDiscountsDataHandler.UpdateTransactionDiscounts(transactionDiscountsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    transactionDiscountsDTO.AcceptChanges();
                }
            }
            if (transactionDiscountsDTO.DiscountCouponsUsedDTO != null &&
               transactionDiscountsDTO.DiscountCouponsUsedDTO.IsChanged)
            {
                DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(executionContext, transactionDiscountsDTO.DiscountCouponsUsedDTO);
                discountCouponsUsedBL.Save(sqlTransaction);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the TransactionDiscounts
        /// Checks if the id is not equal to -1
        /// If it is not equal to -1, then deletes
        /// else ignores
        /// </summary>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            int noOfRowsDeleted = 0;
            TransactionDiscountsDataHandler transactionDiscountsDataHandler = new TransactionDiscountsDataHandler(sqlTransaction);
            if (transactionDiscountsDTO.TransactionDiscountId != -1)
            {
                noOfRowsDeleted = transactionDiscountsDataHandler.Delete(transactionDiscountsDTO.TransactionDiscountId);
            }
            if (transactionDiscountsDTO.DiscountCouponsUsedDTO != null)
            {
                DiscountCouponsUsedBL discountCouponsUsedBL = new DiscountCouponsUsedBL(executionContext, transactionDiscountsDTO.DiscountCouponsUsedDTO);
                discountCouponsUsedBL.Delete(sqlTransaction);
            }
            log.LogMethodExit(noOfRowsDeleted);
            return noOfRowsDeleted;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionDiscountsDTO TransactionDiscountsDTO
        {
            get
            {
                return transactionDiscountsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of TransactionDiscounts
    /// </summary>
    public class TransactionDiscountsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<TransactionDiscountsDTO> transactionDiscountsDTOList = new List<TransactionDiscountsDTO>();

        /// <summary>
        /// Parameterized constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public TransactionDiscountsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public TransactionDiscountsListBL(ExecutionContext executionContext, List<TransactionDiscountsDTO> transactionDiscountsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionDiscountsDTOList);
            this.transactionDiscountsDTOList = transactionDiscountsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TransactionDiscounts list
        /// </summary>
        public List<TransactionDiscountsDTO> GetTransactionDiscountsDTOList(List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool includechildRecords = true)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction, includechildRecords);
            TransactionDiscountsDataHandler transactionDiscountsDataHandler = new TransactionDiscountsDataHandler(sqlTransaction);
            List<TransactionDiscountsDTO> transactionDiscountsDTOList = transactionDiscountsDataHandler.GetTransactionDiscountsDTOList(searchParameters);
            if (includechildRecords && transactionDiscountsDTOList != null && transactionDiscountsDTOList.Count > 0)
            {
                Build(transactionDiscountsDTOList);
            }
            log.LogMethodExit(transactionDiscountsDTOList);
            return transactionDiscountsDTOList;
        }

        /// <summary>
        /// builds the complex transactionDiscountsDTO
        /// </summary>
        private void Build(List<TransactionDiscountsDTO> transactionDiscountsDTOList)
        {
            log.LogMethodEntry(transactionDiscountsDTOList);
            if (transactionDiscountsDTOList != null && transactionDiscountsDTOList.Count > 0)
            {
                Dictionary<int, List<TransactionDiscountsDTO>> transactionDiscountsDTODictionary = new Dictionary<int, List<TransactionDiscountsDTO>>();
                foreach (var transactionDiscountsDTO in transactionDiscountsDTOList)
                {
                    if (transactionDiscountsDTODictionary.ContainsKey(transactionDiscountsDTO.LineId) == false)
                    {
                        transactionDiscountsDTODictionary.Add(transactionDiscountsDTO.LineId, new List<TransactionDiscountsDTO>());
                    }
                    transactionDiscountsDTODictionary[transactionDiscountsDTO.LineId].Add(transactionDiscountsDTO);
                }
                DiscountCouponsUsedListBL discountCouponsUsedListBL = new DiscountCouponsUsedListBL(executionContext);
                List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchDiscountCouponsUsedParams = new List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>>();
                searchDiscountCouponsUsedParams.Add(new KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>(DiscountCouponsUsedDTO.SearchByParameters.TRANSACTION_ID, transactionDiscountsDTOList[0].TransactionId.ToString()));
                List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedListBL.GetDiscountCouponsUsedDTOList(searchDiscountCouponsUsedParams);
                if (discountCouponsUsedDTOList != null && discountCouponsUsedDTOList.Count > 0)
                {
                    foreach (var discountCouponsUsedDTO in discountCouponsUsedDTOList)
                    {
                        if (transactionDiscountsDTODictionary.ContainsKey(discountCouponsUsedDTO.LineId))
                        {
                            List<TransactionDiscountsDTO> list = transactionDiscountsDTODictionary[discountCouponsUsedDTO.LineId];
                            if (list.Count == 1)
                            {
                                list[0].DiscountCouponsUsedDTO = discountCouponsUsedDTO;
                            }
                            else
                            {
                                TransactionDiscountsDTO transactionDiscountsDTO = list.FirstOrDefault(x => x.DiscountId == discountCouponsUsedDTO.DiscountId && x.DiscountCouponsUsedDTO == null);
                                if (transactionDiscountsDTO != null)
                                {
                                    transactionDiscountsDTO.DiscountCouponsUsedDTO = discountCouponsUsedDTO;
                                }
                            }
                        }
                    }
                }
            }

            log.LogMethodExit();
        }

    }
}
