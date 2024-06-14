/********************************************************************************************
 * Project Name - Monitor Asset Type
 * Description  - Bussiness logic of monitor asset type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        23-Feb-2016   Raghuveera          Created
 *2.70        16-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method
 *                                                         returns DTO.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Monitor asset type will creates and modifies the asset type
    /// </summary>
    public class MonitorAssetType
    {
        private MonitorAssetTypeDTO monitorAssetTypeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorAssetType()
        {
            log.LogMethodEntry();
            monitorAssetTypeDTO = null;
            log.LogMethodExit();

        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="monitorAssetTypeDTO">Parameter of the type MonitorAssetTypeDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MonitorAssetType(MonitorAssetTypeDTO monitorAssetTypeDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.monitorAssetTypeDTO = monitorAssetTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the monitor asset type
        /// asset type will be inserted if assetTypeid is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExecutionContext monitorAssetTypeUserContext = ExecutionContext.GetExecutionContext();
            MonitorAssetTypeDataHandler monitorAssetTypeDataHandler = new MonitorAssetTypeDataHandler(sqlTransaction);
            if (monitorAssetTypeDTO.AssetTypeId <= 0)
            {
                monitorAssetTypeDTO = monitorAssetTypeDataHandler.InsertMonitorAssetType(monitorAssetTypeDTO, monitorAssetTypeUserContext.GetUserId(), monitorAssetTypeUserContext.GetSiteId());
                //monitorAssetTypeDTO.AssetTypeId = monitorAssetTypeId;
                monitorAssetTypeDTO.AcceptChanges();
            }
            else
            {
                if (monitorAssetTypeDTO.IsChanged)
                {
                    monitorAssetTypeDTO = monitorAssetTypeDataHandler.UpdateMonitorAssetType(monitorAssetTypeDTO, monitorAssetTypeUserContext.GetUserId(), monitorAssetTypeUserContext.GetSiteId());
                    monitorAssetTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of monitor asset type
    /// </summary>
    public class MonitorAssetTypeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public MonitorAssetTypeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the monitor asset type list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<MonitorAssetTypeDTO> GetAllMonitorAssetTypes(List<KeyValuePair<MonitorAssetTypeDTO.SearchByMonitorAssetTypeParameters, string>> searchParameters,SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry();
            MonitorAssetTypeDataHandler monitorAssetTypeDataHandler = new MonitorAssetTypeDataHandler(sqlTransaction);
            List<MonitorAssetTypeDTO> monitorAssetTypeDTOList = monitorAssetTypeDataHandler.GetMonitorAssetTypeList(searchParameters);
            log.LogMethodExit(monitorAssetTypeDTOList);
            return monitorAssetTypeDTOList;
        }
    }
}
