/********************************************************************************************
 * Project Name - ParentChildCardsBL
 * Description  - Business Logic for ParentChildCards Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
*2.100.0      10-Oct-2020     Mathew Ninan      Modified: Support for Daily Limit Percentage for child cards
**********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    /// Business logic for ParentChildCards class.
    /// </summary>
    public class ParentChildCardsBL
    {
        ParentChildCardsDTO parentChildCardsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ParentChildCardsBL class
        /// </summary>
        public ParentChildCardsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            parentChildCardsDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the parentchildcards id as the parameter
        /// Would fetch the parentchildcards object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ParentChildCardsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ParentChildCardsDataHandler parentChildCardsDataHandler = new ParentChildCardsDataHandler(sqlTransaction);
            parentChildCardsDTO = parentChildCardsDataHandler.GetParentChildCardsDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ParentChildCardsBL object using the ParentChildCardsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="parentChildCardsDTO">ParentChildCardsDTO object</param>
        public ParentChildCardsBL(ExecutionContext executionContext, ParentChildCardsDTO parentChildCardsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parentChildCardsDTO);
            this.parentChildCardsDTO = parentChildCardsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ParentChildCards
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ParentChildCardsDataHandler parentChildCardsDataHandler = new ParentChildCardsDataHandler(sqlTransaction);
            if (parentChildCardsDTO.Id < 0)
            {
                int id = parentChildCardsDataHandler.InsertParentChildCards(parentChildCardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parentChildCardsDTO.Id = id;
                parentChildCardsDTO.AcceptChanges();
            }
            else
            {
                if (parentChildCardsDTO.IsChanged)
                {
                    parentChildCardsDataHandler.UpdateParentChildCards(parentChildCardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    parentChildCardsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ParentChildCardsDTO ParentChildCardsDTO
        {
            get
            {
                return parentChildCardsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ParentChildCards
    /// </summary>
    public class ParentChildCardsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ParentChildCardsDTO> parentChildCardsDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ParentChildCardsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="parentChildCardsDTO">Parent Child Cards DTO</param>
        public ParentChildCardsListBL(ExecutionContext executionContext, List<ParentChildCardsDTO> parentChildCardsDTO)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.parentChildCardsDTOList = parentChildCardsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParentChildCards list
        /// </summary>
        public List<ParentChildCardsDTO> GetParentChildCardsDTOList(List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParentChildCardsDataHandler parentChildCardsDataHandler = new ParentChildCardsDataHandler(sqlTransaction);
            List<ParentChildCardsDTO> returnValue = parentChildCardsDataHandler.GetParentChildCardsDTOList(searchParameters);
            this.parentChildCardsDTOList = returnValue;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the ParentChildCards list
        /// </summary>
        public List<ParentChildCardsDTO> GetActiveParentCardsListByCustomer(int customerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerId, sqlTransaction);
            ParentChildCardsDataHandler parentChildCardsDataHandler = new ParentChildCardsDataHandler(sqlTransaction);
            List<ParentChildCardsDTO> returnValue = parentChildCardsDataHandler.GetActiveParentCardsListByCustomer(customerId);
            this.parentChildCardsDTOList = returnValue;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Performing Split percentage calculation by equal split
        /// </summary>
        public void SplitDailyLimitPercentage()
        {
            log.LogMethodEntry();
            int childCardsCount = 0;
            if (parentChildCardsDTOList == null)
                return;
            childCardsCount = parentChildCardsDTOList.Where(x => x.ActiveFlag == true).Count();
            int totalToSplit = 100;
            int each = 0;
            int first = 0;
            each = totalToSplit / childCardsCount;
            first = each + totalToSplit - (each * childCardsCount);

            parentChildCardsDTOList[0].DailyLimitPercentage = first;
            foreach (ParentChildCardsDTO parentChildCardDTO in parentChildCardsDTOList)
            {
                if (parentChildCardDTO.ActiveFlag)
                {
                    parentChildCardDTO.DailyLimitPercentage = each;
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void SaveParentChildCardsList(SqlTransaction sqlTrx = null)
        {
            try
            {
                log.LogMethodEntry(sqlTrx);
                if (parentChildCardsDTOList != null)
                {
                    foreach (ParentChildCardsDTO parentChildCardsDTO in parentChildCardsDTOList)
                    {
                        ParentChildCardsBL parentChildCardsBL = new ParentChildCardsBL(executionContext, parentChildCardsDTO);
                        parentChildCardsBL.Save(sqlTrx);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the List DTO
        /// </summary>
        public List<ParentChildCardsDTO> ParentChildCardsDTOList
        {
            get
            {
                return parentChildCardsDTOList;
            }
        }
    }
}
