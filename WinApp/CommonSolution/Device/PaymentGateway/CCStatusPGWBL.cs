/********************************************************************************************
 * Project Name - CCStatusPGW BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jun-2017      Lakshminarayana     Created 
 *2.70.2        09-Jul-2019      Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                        LogMethodEntry() and LogMethodExit(). 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Business logic for CCStatusPGW class.
    /// </summary>
    public class CCStatusPGWBL
    {
        private CCStatusPGWDTO cCStatusPGWDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor of CCStatusPGWBL class
        /// </summary>
        public CCStatusPGWBL()
        {
            log.LogMethodEntry();
            cCStatusPGWDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the cCStatusPGW id as the parameter
        /// Would fetch the cCStatusPGW object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public CCStatusPGWBL(int id, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(id);
            CCStatusPGWDataHandler cCStatusPGWDataHandler = new CCStatusPGWDataHandler(sqlTransaction);
            cCStatusPGWDTO = cCStatusPGWDataHandler.GetCCStatusPGWDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CCStatusPGWBL object using the CCStatusPGWDTO
        /// </summary>
        /// <param name="cCStatusPGWDTO">CCStatusPGWDTO object</param>
        public CCStatusPGWBL(CCStatusPGWDTO cCStatusPGWDTO)
            : this()
        {
            log.LogMethodEntry(cCStatusPGWDTO);
            this.cCStatusPGWDTO = cCStatusPGWDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CCStatusPGW
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CCStatusPGWDataHandler cCStatusPGWDataHandler = new CCStatusPGWDataHandler(sqlTransaction);
            if (cCStatusPGWDTO.StatusId < 0)
            {
                cCStatusPGWDTO = cCStatusPGWDataHandler.InsertCCStatusPGW(cCStatusPGWDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                cCStatusPGWDTO.AcceptChanges();
            }
            else
            {
                if (cCStatusPGWDTO.IsChanged)
                {
                    cCStatusPGWDTO = cCStatusPGWDataHandler.UpdateCCStatusPGW(cCStatusPGWDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cCStatusPGWDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CCStatusPGWDTO CCStatusPGWDTO
        {
            get
            {
                return cCStatusPGWDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CCStatusPGW
    /// </summary>
    public class CCStatusPGWListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the CCStatusPGW list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>List of CCStatusPGWDTO</returns>
        public List<CCStatusPGWDTO> GetCCStatusPGWDTOList(List<KeyValuePair<CCStatusPGWDTO.SearchByParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CCStatusPGWDataHandler cCStatusPGWDataHandler = new CCStatusPGWDataHandler(sqlTransaction);
            List<CCStatusPGWDTO> cCStatusPGWDTOList = cCStatusPGWDataHandler.GetCCStatusPGWDTOList(searchParameters);
            log.LogMethodExit(cCStatusPGWDTOList);
            return cCStatusPGWDTOList;
        }

    }
}
