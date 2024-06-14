/********************************************************************************************
 * Project Name - HR Module
 * Description  - BL Class for Holiday
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.0        20-Sep-2019  Indrajeet Kumar          Created 
 *2.90        20-May-2020     Vikas Dwivedi          Modified as per the Standard CheckList
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business Logic for HolidayBL
    /// </summary>
    public class HolidayBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HolidayDTO holidayDTO;
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Parametrized Constructor of HolidayBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private HolidayBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates HolidayBL object using the HolidayDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="holidayDTO">holidayDTO</param>
        public HolidayBL(ExecutionContext executionContext, HolidayDTO holidayDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, holidayDTO);
            this.holidayDTO = holidayDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the holiday id as the parameter
        /// Would fetch the holiday object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="holidayId">id of Holiday Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public HolidayBL(ExecutionContext executionContext, int holidayId, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, holidayId, sqlTransaction);
            HolidayDataHandler holidayDataHandler = new HolidayDataHandler(sqlTransaction);
            this.holidayDTO = holidayDataHandler.GetHolidayDTO(holidayId);
            if (holidayDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "HolidayDTO", holidayId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the HolidayDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// Gets HolidayDTO Object
        /// </summary>
        public HolidayDTO GetHolidayDTO
        {
            get { return holidayDTO; }
        }

        /// <summary>
        /// Saves the HolidayBL
        /// Checks if the HolidayBL id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);            
            HolidayDataHandler holidayDataHandler = new HolidayDataHandler(sqlTransaction);
            if (holidayDTO.HolidayId < 0)
            {
                holidayDTO = holidayDataHandler.Insert(holidayDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                holidayDTO.AcceptChanges();
            }
            else
            {
                if (holidayDTO.IsChanged)
                {
                    holidayDataHandler.Update(holidayDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    holidayDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete a Holiday record 
        /// </summary>
        /// <param name="SQLTrx"></param>
        public void Delete(SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(SQLTrx);
            try
            {
                HolidayDataHandler holidayDataHandler = new HolidayDataHandler(SQLTrx);
                holidayDataHandler.DeleteHoliday(holidayDTO.HolidayId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Manages the list of HolidayBL
    /// </summary>
    public class HolidayListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<HolidayDTO> holidayDTOList = new List<HolidayDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public HolidayListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="holidayDTOList">holidayDTOList</param>
        public HolidayListBL(ExecutionContext executionContext, List<HolidayDTO> holidayDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, holidayDTOList);
            this.holidayDTOList = holidayDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the HolidayBL list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        public List<HolidayDTO> GetAllHolidayList(List<KeyValuePair<HolidayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            HolidayDataHandler holidayDataHandler = new HolidayDataHandler(sqlTransaction);
            List<HolidayDTO> holidayDTOList = holidayDataHandler.GetHolidayDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(holidayDTOList);
            return holidayDTOList;
        }

        /// <summary>
        /// Will return list of Year based on the siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<int> GetYearList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            HolidayDataHandler holidayDataHandler = new HolidayDataHandler(sqlTransaction);
            List<int> getYearList = holidayDataHandler.GetYear(sqlTransaction);
            log.LogMethodExit();
            return getYearList;
        }

        /// <summary>
        /// Save the HolidayBL List
        /// </summary>
        public List<HolidayDTO> SaveUpdateHoliday(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<HolidayDTO> holidayDTOLists = new List<HolidayDTO>();
            if (holidayDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (HolidayDTO holidayDTO in holidayDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            HolidayBL holidayBL = new HolidayBL(executionContext, holidayDTO);
                            holidayBL.Save(sqlTransaction);
                            holidayDTOLists.Add(holidayBL.GetHolidayDTO);
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
            return holidayDTOLists;
        }

        /// <summary>
        /// Delete DeleteHolidayList
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (holidayDTOList != null && holidayDTOList.Any())
            {
                foreach (HolidayDTO holidayDTO in holidayDTOList)
                {
                    if (holidayDTO.IsChanged)
                    {
                        try
                        {
                            HolidayBL holidayBL = new HolidayBL(executionContext, holidayDTO);
                            holidayBL.Delete(sqlTransaction);
                        }
                        catch (ValidationException valEx)
                        {
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing validation Exception : " + valEx.Message);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
        }
    }
}
