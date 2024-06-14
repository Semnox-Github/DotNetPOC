/********************************************************************************************
 * Project Name - Games                                                                         
 * Description  - Machine Attribute BL
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80       05-Apr-2020   Girish Kundar        Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Semnox.Parafait.Game.MachineConfigurationClass;

namespace Semnox.Parafait.Game
{
    public class MachineAttributeBL
    {
        private MachineAttributeDTO machineAttributeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MachineAttributeDTO.AttributeContext attributeContext;

        /// <summary>
        /// Sets the AttributeContext
        /// </summary>
        public MachineAttributeDTO.AttributeContext AttributeContext
        {
            get { return attributeContext; }
            set { attributeContext = value; }
        }

        /// <summary>
        /// Parameterized constructor of MachineAttributeBL class
        /// </summary>
        private MachineAttributeBL(ExecutionContext executionContext)
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
        public MachineAttributeBL(ExecutionContext executionContext, MachineAttributeDTO machineAttributeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineAttributeDTO);
            this.machineAttributeDTO = machineAttributeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Address
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        private void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
             if (machineAttributeDTO.AttributeId < 0 && machineAttributeDTO.ContextOfAttribute == AttributeContext)
            {
                machineAttributeDTO = machineAttributeDataHandler.InsertMachineAttribute(machineAttributeDTO, AttributeContext, -1, executionContext.GetUserId(), executionContext.GetSiteId());
                machineAttributeDTO.AcceptChanges();

            }
            else
            {
                if (machineAttributeDTO.IsChanged  && machineAttributeDTO.ContextOfAttribute == AttributeContext)
                {
                    machineAttributeDTO = machineAttributeDataHandler.UpdateMachineAttribute(machineAttributeDTO, AttributeContext, -1, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineAttributeDTO.AcceptChanges();
                }
            }
            /// Below code is to save the Audit log details into DBAuditLog
            if (!string.IsNullOrEmpty(machineAttributeDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("GameProfileattributevalues", machineAttributeDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MachineAttributeDTO MachineAttributeDTO
        {
            get
            {
                return machineAttributeDTO;
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
    public class MachineAttributeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<MachineAttributeDTO> machineAttributeDTOList = new List<MachineAttributeDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public MachineAttributeListBL(ExecutionContext executionContext)
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
        public MachineAttributeListBL(ExecutionContext executionContext, List<MachineAttributeDTO> machineAttributeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineAttributeDTOList);
            this.machineAttributeDTOList = machineAttributeDTOList;
            log.LogMethodExit();
        }
        public List<MachineAttributeDTO> GetMachineAttributes(List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters,
                                          MachineAttributeDTO.AttributeContext attributeContext)
        {
            log.LogMethodEntry(searchByParameters, attributeContext);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler();
            List<MachineAttributeDTO> machineAttributeDTOList = machineAttributeDataHandler.GetMachineAttributeList(searchByParameters, attributeContext);
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
        }

        /// <summary>
        /// Populate configuration value considering Promotion, PromotionDetail
        /// GameProfile, Game, Machine
        /// </summary>
        /// <param name="MachineId">Machineid</param>
        /// <param name="PromotionDetailId">Promotion Detail Id if applicable else -1</param>
        public List<clsConfig> Populate(int MachineId, int PromotionDetailId)
        {
            log.LogMethodEntry(MachineId, PromotionDetailId);
            MachineAttributeListBL machineAttributeListBL = new MachineAttributeListBL(executionContext);
            List<MachineAttributeDTO> machineAttributeDTOList = machineAttributeListBL.GetMachinePromotionAttributes(MachineId, PromotionDetailId);
            log.LogVariableState("machine_id", MachineId);
            log.LogVariableState("promotionDetailId", PromotionDetailId);
            MachineConfigurationClass machineConfigurationClass = new MachineConfigurationClass(executionContext);
            List<clsConfig> configuration = new List<clsConfig>();
            if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
            {
                foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                {
                    machineConfigurationClass.addValue(machineAttributeDTO.AttributeName.ToString(), machineAttributeDTO.AttributeValue.ToString(), machineAttributeDTO.EnableForPromotion);
                }
                configuration = machineConfigurationClass.Configuration;
            }
            log.LogMethodExit(configuration);
            return configuration;
        }

        public List<MachineAttributeDTO> GetMachinePromotionAttributes(int machineId = -1, int promotionDetailId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            List<MachineAttributeDTO> machineAttributeDTOList = machineAttributeDataHandler.GetMachinePromotionAttributes(machineId, promotionDetailId);
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
        }

        private void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                foreach (MachineAttributeDTO machineAttributeDTO in machineAttributeDTOList)
                {
                    MachineAttributeBL machineAttributeBL = new MachineAttributeBL(executionContext, machineAttributeDTO);
                    //machineAttributeBL.Save(sqlTransaction);
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

        public DateTime? GetAttributeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler();
            DateTime? result = machineAttributeDataHandler.GetAttributeModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
