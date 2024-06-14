/********************************************************************************************
 * Project Name - POS
 * Description  - Business Logic File for ShiftLog 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        4-June-2019   Divya A                 Created
 *2.90        26-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.POS
{
    public class ShiftLogBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ShiftLogDTO shiftLogDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of ShiftLogBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ShiftLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ShiftLogBL object using the shiftLogDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="shifLogtDTO">ShiftLogDTO object is passed as parameter</param>
        public ShiftLogBL(ExecutionContext executionContext,ShiftLogDTO shiftLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftLogDTO);
            this.shiftLogDTO = shiftLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ShiftLogBL id as the parameter
        /// Would fetch the ShiftLogBL object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of ShiftLogBL Object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ShiftLogBL(ExecutionContext executionContext, int shiftLogId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftLogId, sqlTransaction);
            ShiftLogDataHandler shiftLogDataHandler = new ShiftLogDataHandler(sqlTransaction);
            this.shiftLogDTO = shiftLogDataHandler.GetShiftLogDTO(shiftLogId);
            if (shiftLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ShiftLogDTO", shiftLogId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ShiftLog
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ShiftLogDataHandler ShiftLogDataHandler = new ShiftLogDataHandler(sqlTransaction);
            if (shiftLogDTO.ShiftLogId < 0)
            {
                shiftLogDTO = ShiftLogDataHandler.Insert(shiftLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                shiftLogDTO.AcceptChanges();
            }
            else if (shiftLogDTO.IsChanged)
            {
                shiftLogDTO = ShiftLogDataHandler.Update(shiftLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                shiftLogDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ShiftLogDTO
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
        /// Gets the DTO
        /// </summary>
        public ShiftLogDTO ShiftLogDTO { get { return shiftLogDTO; } }
    }

    /// <summary>
    /// Manages the list of ShiftLogBL
    /// </summary>
    public class ShiftLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ShiftLogDTO> shiftLogDTOList = new List<ShiftLogDTO>();

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ShiftLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ExecutionContext and DTO Parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="shiftLogDTOList">shiftLogDTOList</param>
        public ShiftLogListBL(ExecutionContext executionContext, List<ShiftLogDTO> shiftLogDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, shiftLogDTOList);
            this.shiftLogDTOList = shiftLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ShiftLog list
        /// </summary>
        public List<ShiftLogDTO> GetShiftLogDTOList(List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ShiftLogDataHandler shiftDataHandler = new ShiftLogDataHandler(sqlTransaction);
            List<ShiftLogDTO> shiftLogDTOList = shiftDataHandler.GetShiftLogDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(shiftLogDTOList);
            return shiftLogDTOList;
        }

        /// <summary>
        /// Saves the ShiftLog List
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (shiftLogDTOList == null ||
               shiftLogDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < shiftLogDTOList.Count; i++)
            {
                var shiftLogDTO = shiftLogDTOList[i];
                if (shiftLogDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ShiftLogBL shiftLogBL = new ShiftLogBL(executionContext, shiftLogDTO);
                    shiftLogBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving ShiftLogDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("ShiftLogDTOList", shiftLogDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }

}
