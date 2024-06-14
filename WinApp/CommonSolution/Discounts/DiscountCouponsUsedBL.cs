/********************************************************************************************
 * Project Name - DiscountCouponsUsed BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017      Lakshminarayana     Created 
 *2.60        18-Mar-2019      Akshay Gulagnji     Added default constructor and parameterized constructor for ExecutionContext 
 *2.70.2      31-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        31-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 *2.140       14-Sep-2021      Fiona               Added GetDiscountCouponsUsedDTOfDiscountCoupons
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Business logic for DiscountCouponsUsed class.
    /// </summary>
    public class DiscountCouponsUsedBL
    {
        private DiscountCouponsUsedDTO discountCouponsUsedDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of DiscountCouponsUsedBL class
        /// </summary>
        private DiscountCouponsUsedBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="discountCouponsUsedDTO"></param>
        /// <param name="executionContext"></param>
        public DiscountCouponsUsedBL(ExecutionContext executionContext, DiscountCouponsUsedDTO discountCouponsUsedDTO)
                   : this(executionContext)
        {
            log.LogMethodEntry(discountCouponsUsedDTO, executionContext);
            this.discountCouponsUsedDTO = discountCouponsUsedDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the discountCouponsUsed id as the parameter
        /// Would fetch the discountCouponsUsed object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public DiscountCouponsUsedBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
            discountCouponsUsedDTO = discountCouponsUsedDataHandler.GetDiscountCouponsUsedDTO(id);
            if (discountCouponsUsedDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "DiscountCouponsUsed", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(discountCouponsUsedDTO);
        }

        /// <summary>
        /// Saves the DiscountCouponsUsed
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
            if (discountCouponsUsedDTO.Id < 0)
            {
                discountCouponsUsedDTO = discountCouponsUsedDataHandler.InsertDiscountCouponsUsed(discountCouponsUsedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                discountCouponsUsedDTO.AcceptChanges();
            }
            else
            {
                if (discountCouponsUsedDTO.IsChanged)
                {
                    discountCouponsUsedDTO = discountCouponsUsedDataHandler.UpdateDiscountCouponsUsed(discountCouponsUsedDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    discountCouponsUsedDTO.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Removes the DiscountCouponsUsed
        /// Checks if the id is not equal to -1
        /// If it is not equal to -1, then deletes
        /// else ignores
        /// </summary>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            int noOfRowsDeleted = 0;
            DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
            if (discountCouponsUsedDTO.Id != -1)
            {
                noOfRowsDeleted = discountCouponsUsedDataHandler.Delete(discountCouponsUsedDTO.Id);
            }
            log.LogMethodExit(noOfRowsDeleted);
            return noOfRowsDeleted;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DiscountCouponsUsedDTO DiscountCouponsUsedDTO
        {
            get
            {
                return discountCouponsUsedDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of DiscountCouponsUsed
    /// </summary>
    public class DiscountCouponsUsedListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public DiscountCouponsUsedListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        internal List<DiscountCouponsUsedDTO> GetDiscountCouponsUsedDTOfDiscountCoupons(List<int> couponSetIdList, bool activeRecords, SqlTransaction sqlTransaction)//added this method
        {
            log.LogMethodEntry(couponSetIdList, activeRecords, sqlTransaction);
            DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
            List<DiscountCouponsUsedDTO> discountCouponsUsedList = discountCouponsUsedDataHandler.GetDiscountCouponsDTOListOfDiscountCoupons(couponSetIdList, activeRecords);
            log.LogMethodExit(discountCouponsUsedList);
            return discountCouponsUsedList;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="discountCouponsUsedDTOList">discountCouponsUsedDTOList</param>
        public DiscountCouponsUsedListBL(ExecutionContext executionContext, List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList)
                : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.discountCouponsUsedDTOList = discountCouponsUsedDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the DiscountCouponsUsed list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of DiscountCouponsUsedDTO </returns>
        public List<DiscountCouponsUsedDTO> GetDiscountCouponsUsedDTOList(List<KeyValuePair<DiscountCouponsUsedDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
            List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList = discountCouponsUsedDataHandler.GetDiscountCouponsUsedDTOList(searchParameters);
            log.LogMethodExit(discountCouponsUsedDTOList);
            return discountCouponsUsedDTOList;
        }

       
        /// <summary>
        /// Validates and saves the discountCouponsUsedDTOList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (discountCouponsUsedDTOList == null ||
                discountCouponsUsedDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }

            DiscountCouponsUsedDataHandler discountCouponsUsedDataHandler = new DiscountCouponsUsedDataHandler(sqlTransaction);
            discountCouponsUsedDataHandler.Save(discountCouponsUsedDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }
    }
}
