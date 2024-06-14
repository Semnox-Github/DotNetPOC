/********************************************************************************************
 * Project Name - Monitor types
 * Description  - Bussiness logic of monitors types
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60.2      13-June-2019   Jagan Mohana Rao    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Monitor types
    /// </summary>
    public class MonitorType
    {
        private MonitorTypeDTO monitorTypeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorType(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.monitorTypeDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorTypeDTO"></param>
        public MonitorType(ExecutionContext executionContext, MonitorTypeDTO monitorTypeDTO)
        {
            log.LogMethodEntry(executionContext, monitorTypeDTO);
            this.monitorTypeDTO = monitorTypeDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the monitor application module
        /// Monitor TypeId will be inserted if typeId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            MonitorTypeDataHandler monitorTypeDataHandler = new MonitorTypeDataHandler();
            if (monitorTypeDTO.MonitorTypeId <= 0)
            {
                monitorTypeDTO = monitorTypeDataHandler.Insert(monitorTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            else
            {
                if (monitorTypeDTO.IsChanged == true)
                {
                    monitorTypeDTO = monitorTypeDataHandler.Update(monitorTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    monitorTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of monitor application module 
    /// </summary>
    public class MonitorTypeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorTypeDTO> monitorTypeDTOList;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorTypeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.monitorTypeDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitor type list
        /// </summary>
        public List<MonitorTypeDTO> GetAllMonitorTypeDTO(List<KeyValuePair<MonitorTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MonitorTypeDataHandler monitorTypeDataHandler = new MonitorTypeDataHandler();
            monitorTypeDTOList = monitorTypeDataHandler.GetAllMonitorTypeDTO(searchParameters);
            log.LogMethodExit(monitorTypeDTOList);
            return monitorTypeDTOList;
        }
    }
}
