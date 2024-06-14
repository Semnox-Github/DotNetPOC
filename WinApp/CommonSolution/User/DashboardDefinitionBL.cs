using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class DashboardDefinitionBL
    {
        private DashboardDefinitionDTO dashboardDefinitionDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of DashboardDefinitionBL class
        /// </summary>
        private DashboardDefinitionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DashboardDefinitionBL object using the DashboardDefinitionDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="dashboardDefinitionDTO">DashboardDefinitionDTO object</param>
        public DashboardDefinitionBL(ExecutionContext executionContext, DashboardDefinitionDTO dashboardDefinitionDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dashboardDefinitionDTO);
            this.dashboardDefinitionDTO = dashboardDefinitionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DashboardDefinitionBL object using the 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public DashboardDefinitionBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            DashboardDefinitionDataHandler dashboardDefinitionDataHandler = new DashboardDefinitionDataHandler(sqlTransaction);
            dashboardDefinitionDTO = dashboardDefinitionDataHandler.GetDashboardDefinitionDTO(id);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the DashboardDefinition
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DashboardDefinitionDataHandler dashboardDefinitionDataHandler = new DashboardDefinitionDataHandler(sqlTransaction);
            if (dashboardDefinitionDTO.DashboardDefId < 0)
            {
                dashboardDefinitionDTO = dashboardDefinitionDataHandler.Insert(dashboardDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dashboardDefinitionDTO.AcceptChanges();
            }
            else if (dashboardDefinitionDTO.IsChanged)
            {
                dashboardDefinitionDTO = dashboardDefinitionDataHandler.Update(dashboardDefinitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                dashboardDefinitionDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DashboardDefinitionDTO DashboardDefinitionDTO
        {
            get
            {
                return dashboardDefinitionDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of DashboardDefinition
    /// </summary>
    public class DashboardDefinitionListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<DashboardDefinitionDTO> dashboardDefinitionDTOList;
        /// <summary>
        /// Parameterized constructor having ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public DashboardDefinitionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.dashboardDefinitionDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="dashboardDefinitionDTOList"></param>
        public DashboardDefinitionListBL(ExecutionContext executionContext, List<DashboardDefinitionDTO> dashboardDefinitionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dashboardDefinitionDTOList);
            this.dashboardDefinitionDTOList = dashboardDefinitionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the DashboardDefinitionDTO List
        /// </summary>
        public List<DashboardDefinitionDTO> GetDashboardDefinitionDTOList(List<KeyValuePair<DashboardDefinitionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DashboardDefinitionDataHandler dashboardDefinitionDataHandler = new DashboardDefinitionDataHandler(sqlTransaction);
            List<DashboardDefinitionDTO> dashboardDefinitionDTOList = dashboardDefinitionDataHandler.GetAllDashboardDefinitionDTOList(searchParameters);
            log.LogMethodExit(dashboardDefinitionDTOList);
            return dashboardDefinitionDTOList;
        }
    }
}