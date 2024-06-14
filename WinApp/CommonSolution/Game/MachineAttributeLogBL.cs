/********************************************************************************************
 * Project Name - Games                                                                         
 * Description  - MachineAttributeLogDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100       05-Sep-2020   Girish Kundar        Created
 *2.130.0     06-Aug-2021   Abhishek             Handle WHO column updates
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Game
{
    public class MachineAttributeLogBL
    {
        private MachineAttributeLogDTO machineAttributeLogDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Parameterized constructor of MachineAttributeLogBL class
        /// </summary>
        private MachineAttributeLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates MachineAttributeLogBL object using the MachineAttributeLogDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="MachineAttributeLogDTO">MachineAttributeLogDTO object</param>
        public MachineAttributeLogBL(ExecutionContext executionContext, MachineAttributeLogDTO machineAttributeLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineAttributeLogDTO);
            this.machineAttributeLogDTO = machineAttributeLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Address
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updatess
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MachineAttributeLogDataHandler machineAttributeLogDataHandler = new MachineAttributeLogDataHandler(sqlTransaction);
            ValidateUserRemarks(machineAttributeLogDTO.UserRemarks);
            if (!string.IsNullOrWhiteSpace(machineAttributeLogDTO.UserRemarks))
            {
                machineAttributeLogDTO.UserRemarks = Regex.Replace(machineAttributeLogDTO.UserRemarks, @"[^\x20-\x7E]", " ");
            }
            if (machineAttributeLogDTO.Id < 0 )
            {
                machineAttributeLogDTO = machineAttributeLogDataHandler.Insert(machineAttributeLogDTO, executionContext.GetUserPKId(), executionContext.GetSiteId());
                machineAttributeLogDTO.AcceptChanges();

            }
            else
            {
                if (machineAttributeLogDTO.IsChanged )
                {
                    machineAttributeLogDTO = machineAttributeLogDataHandler.Update(machineAttributeLogDTO, executionContext.GetUserPKId(), executionContext.GetSiteId());
                    machineAttributeLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        private void ValidateUserRemarks(string remarks)
        {
            log.LogMethodEntry(remarks);
            if (string.IsNullOrWhiteSpace(remarks))
            {
                log.LogMethodExit(null, "remarks empty");
                return;
            }
            if (remarks.Length > 500)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "User Remarks"), 500);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MachineAttributeLogDTO MachineAttributeLogDTO
        {
            get
            {
                return machineAttributeLogDTO;
            }
        }

        /// <summary>
        /// Validates the Address, throws ValidationException if any fields are not valid
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of Address
    /// </summary>
    public class MachineAttributeLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<MachineAttributeLogDTO> machineAttributeLogDTOList = new List<MachineAttributeLogDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public MachineAttributeLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates MachineAttributeBL object using the machineAttributeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="machineAttributeDTO">machineAttributeDTO object</param>
        public MachineAttributeLogListBL(ExecutionContext executionContext, List<MachineAttributeLogDTO> machineAttributeLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineAttributeLogDTOList);
            this.machineAttributeLogDTOList = machineAttributeLogDTOList;
            log.LogMethodExit();
        }
        public List<MachineAttributeLogDTO> GetMachineAttributeLogs(List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> searchByParameters, SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(searchByParameters, sqlTransaction);
            MachineAttributeLogDataHandler machineAttributeLogDataHandler = new MachineAttributeLogDataHandler(sqlTransaction);
            List<MachineAttributeLogDTO> machineAttributeLogDTOList = machineAttributeLogDataHandler.GetMachineAttributeLogs(searchByParameters, sqlTransaction);
            log.LogMethodExit(machineAttributeLogDTOList);
            return machineAttributeLogDTOList;
        }


        internal void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (machineAttributeLogDTOList != null && machineAttributeLogDTOList.Any())
                {
                    foreach (MachineAttributeLogDTO machineAttributeLogDTO in machineAttributeLogDTOList)
                    {
                        MachineAttributeLogBL machineAttributeLogBL = new MachineAttributeLogBL(executionContext, machineAttributeLogDTO);
                        machineAttributeLogBL.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 654));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 545));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

    }
}
