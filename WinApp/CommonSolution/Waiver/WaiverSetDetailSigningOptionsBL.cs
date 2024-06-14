
/********************************************************************************************
 * Project Name - Waiver
 * Description  - Business logic of WaiverSetDetailSigningOption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       1-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 ********************************************************************************************///using Semnox.Parafait.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    /// Business logic for WaiverSetDetailSigningOptionsBL class.
    /// </summary>
    public class WaiverSetDetailSigningOptionsBL
    {
        private WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Constructor with the  Id as the parameter
        /// Would fetch the WaiverSetDetailSigningOptions object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public WaiverSetDetailSigningOptionsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id);
            this.executionContext = executionContext;
            WaiverSetDetailSigningOptionsDataHandler waiverSetDetailSigningOptionsDataHandler = new WaiverSetDetailSigningOptionsDataHandler(sqlTransaction);
            waiverSetDetailSigningOptionsDTO = waiverSetDetailSigningOptionsDataHandler.GetWaiverSetDetailSigningOptionsDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates WaiverSetDetailSigningOptionsBL object using the WaiverSetDetailSigningOptionsDTO
        /// </summary>
        /// <param name="waiverSetDetailSigningOptionsDTO">WaiverSetDetailSigningOptionsDTO object</param>
        public WaiverSetDetailSigningOptionsBL(ExecutionContext executionContext, WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO)
        {
            log.LogMethodEntry(executionContext, waiverSetDetailSigningOptionsDTO);
            this.executionContext = executionContext;
            this.waiverSetDetailSigningOptionsDTO = waiverSetDetailSigningOptionsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the WaiverSetDetailSigningOptions
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            WaiverSetDetailSigningOptionsDataHandler waiverSetDetailSigningOptionsDataHandler = new WaiverSetDetailSigningOptionsDataHandler(sqlTransaction);
            if (waiverSetDetailSigningOptionsDTO.Id < 0)
            {
                waiverSetDetailSigningOptionsDTO = waiverSetDetailSigningOptionsDataHandler.InsertWaiverSetDetailSigningOptions(waiverSetDetailSigningOptionsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                waiverSetDetailSigningOptionsDTO.AcceptChanges();
            }
            else
            {
                if (waiverSetDetailSigningOptionsDTO.IsChanged)
                {
                    waiverSetDetailSigningOptionsDTO = waiverSetDetailSigningOptionsDataHandler.UpdateWaiverSetDetailSigningOptions(waiverSetDetailSigningOptionsDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    waiverSetDetailSigningOptionsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Delete the PaymentChannelsDTO based on Id
        /// </summary>
        /// <param name="customerId">paymentChannelId</param>
        /// <returns>returns int status</returns>
        public int Delete(int waiverSetDetailSigningOptionsId)
        {
            log.LogMethodEntry(waiverSetDetailSigningOptionsId);
            try
            {
                SqlTransaction sqlTransaction = null;
                WaiverSetDetailSigningOptionsDataHandler waiverSetDetailSigningOptionsDataHandler = new WaiverSetDetailSigningOptionsDataHandler(sqlTransaction);
                int deletedId = waiverSetDetailSigningOptionsDataHandler.DeleteWaiverSetDetailSigningOptions(waiverSetDetailSigningOptionsId);
                log.LogMethodExit(deletedId);
                return deletedId;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }


    /// <summary>
    /// Manages the list of WaiverSetDetailSigningOptions
    /// </summary>
    public class WaiverSetDetailSigningOptionsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the WaiverSetDetailSigningOptions list
        /// </summary>
        public List<WaiverSetDetailSigningOptionsDTO> GetWaiverSetDetailSigningOptionsList(List<KeyValuePair<WaiverSetDetailSigningOptionsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            WaiverSetDetailSigningOptionsDataHandler waiverSetDetailSigningOptionsDataHandler = new WaiverSetDetailSigningOptionsDataHandler(sqlTransaction);
            List<WaiverSetDetailSigningOptionsDTO> returnValue = waiverSetDetailSigningOptionsDataHandler.GetWaiverSetDetailSigningOptionsList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }

}
