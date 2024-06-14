
/********************************************************************************************
 * Project Name - POSPrintDisplayGroup
 * Description  - Data object of the POSPrintDisplayGroupBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        14-Nov-2016    Amaresh          Created 
 *2.90        12-Jul-2020    Girish Kundar    Modified : 3 tier changes 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.DisplayGroup
{
    /// <summary>
    /// POSPrintDisplayGroup will creates and modifies the POSPrintDisplayGroup
    /// </summary>
    public class POSPrintDisplayGroup
    {
        private POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO;
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        private  POSPrintDisplayGroup(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
    
        /// <summary>
        /// Constructor with the POSPrintDisplayGroup DTO parameter
        /// </summary>
        /// <param name="pOSPrintDisplayGroupDTO">Parameter of the type POSPrintDisplayGroupDTO</param>
        public POSPrintDisplayGroup(ExecutionContext executionContext , POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, pOSPrintDisplayGroupDTO);
            this.pOSPrintDisplayGroupDTO = pOSPrintDisplayGroupDTO;
            log.LogMethodExit();
        }

        public POSPrintDisplayGroup(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            POSPrintDisplayGroupDataHandler POSPrintDisplayGroupDataHandler = new POSPrintDisplayGroupDataHandler(sqlTransaction);
            pOSPrintDisplayGroupDTO = POSPrintDisplayGroupDataHandler.GetPOSPrintDisplayGroup(id);
            if (pOSPrintDisplayGroupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSPrintDisplayGroup", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(pOSPrintDisplayGroupDTO);
        }

        /// <summary>
        /// Saves the POSPrintDisplayGroup  
        /// POSPrintDisplayGroup will be inserted if id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            POSPrintDisplayGroupDataHandler POSPrintDisplayGroupDataHandler = new POSPrintDisplayGroupDataHandler(sqlTransaction);
            if (pOSPrintDisplayGroupDTO.Id < 0)
            {
                pOSPrintDisplayGroupDTO = POSPrintDisplayGroupDataHandler.InsertPOSPrintDisplayGroup(pOSPrintDisplayGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                pOSPrintDisplayGroupDTO.AcceptChanges();
            }
            else
            {
                if (pOSPrintDisplayGroupDTO.IsChanged == true)
                {
                    pOSPrintDisplayGroupDTO = POSPrintDisplayGroupDataHandler.UpdatePOSPrintDisplayGroup(pOSPrintDisplayGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    pOSPrintDisplayGroupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of POSPrintDisplayGroup List
    /// </summary>
    public class POSPrintDisplayGroupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the POSPrintDisplayGroup DTO
        /// </summary>
        public POSPrintDisplayGroupDTO GetPOSPrintDisplayGroup(int id)
        {
            log.LogMethodEntry(id);
            POSPrintDisplayGroupDataHandler POSPrintDisplayGroupDataHandler = new POSPrintDisplayGroupDataHandler();
            POSPrintDisplayGroupDTO pOSPrintDisplayGroupDTO=  POSPrintDisplayGroupDataHandler.GetPOSPrintDisplayGroup(id);
            log.LogMethodExit(pOSPrintDisplayGroupDTO);
            return pOSPrintDisplayGroupDTO;
        }
      
        /// <summary>
        /// Returns the POSPrintDisplayGroupDTO List
        /// </summary>
        public List<POSPrintDisplayGroupDTO> GetAllPOSPrintDisplayGroup(List<KeyValuePair<POSPrintDisplayGroupDTO.SearchByPosPrintDisplayGroupParameters, string>> searchParameters,SqlTransaction sqlTransaction = null )
        {
            log.LogMethodEntry(sqlTransaction);
            POSPrintDisplayGroupDataHandler pOSPrintDisplayGroupDataHandler = new POSPrintDisplayGroupDataHandler(sqlTransaction);
            List<POSPrintDisplayGroupDTO> pOSPrintDisplayGroupDTOs = pOSPrintDisplayGroupDataHandler.GetPOSPrintDisplayGroupList(searchParameters);
            log.LogMethodExit(pOSPrintDisplayGroupDTOs);
            return pOSPrintDisplayGroupDTOs;
        }
    }
}
