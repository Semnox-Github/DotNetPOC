/********************************************************************************************
 * Project Name - EntityOverrideDate
 * Description  - Bussiness logic of EntityOverrideDate
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60       24-Jan-2019   Jagan Mohana     Created constructor WaiverSetSigningOptionsListBL and
 *                                          added new method SaveUpdateWaiverSetSigningOptionList
 *2.60       17-Mar-2019   Manoj Durgam     Added ExecutionContext to the constructor, 
 *                                          Deleted : this() from WaiverSetSigningOptionsBL(WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO, ExecutionContext executionContext)
 *2.60       25-Mar-2019   Nagesh Badiger   Added log method entry and method exit and log.Error                                         
 ********************************************************************************************/

using Semnox.Core;
//using Semnox.Parafait.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Waiver;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    /// Business logic for WaiverSetSigningOptionsBL class.
    /// </summary>
    public class WaiverSetSigningOptionsBL
    {
        private WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Constructor with the  Id as the parameter
        /// Would fetch the WaiverSetSigningOptions object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public WaiverSetSigningOptionsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            this.executionContext = executionContext;
            WaiverSetSigningOptionsDataHandler waiverSetSigningOptionsDataHandler = new WaiverSetSigningOptionsDataHandler(sqlTransaction);
            waiverSetSigningOptionsDTO = waiverSetSigningOptionsDataHandler.GetWaiverSetSigningOptionsDTO(id);
            if (waiverSetSigningOptionsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "WaiverSetSigningOptions", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates WaiverSetSigningOptionsBL object using the WaiverSetSigningOptionsDTO
        /// </summary>
        /// <param name="waiverSetSigningOptionsDTO">WaiverSetSigningOptionsDTO object</param>
        public WaiverSetSigningOptionsBL(ExecutionContext executionContext, WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO)
        {
            log.LogMethodEntry(executionContext, waiverSetSigningOptionsDTO);
            this.executionContext = executionContext;
            this.waiverSetSigningOptionsDTO = waiverSetSigningOptionsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the WaiverSetSigningOptions
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            WaiverSetSigningOptionsDataHandler waiverSetSigningOptionsDataHandler = new WaiverSetSigningOptionsDataHandler(sqlTransaction);
            if (waiverSetSigningOptionsDTO.Id < 0)
            {
                waiverSetSigningOptionsDataHandler.InsertWaiverSetSigningOptions(waiverSetSigningOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                waiverSetSigningOptionsDTO.AcceptChanges();
            }
            else
            {
                if (waiverSetSigningOptionsDTO.IsChanged)
                {
                    waiverSetSigningOptionsDataHandler.UpdateWaiverSetSigningOptions(waiverSetSigningOptionsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    waiverSetSigningOptionsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the WaiverSetSigningOptionsDTO based on Id
        /// </summary>
        /// <param name="waiverSetSigningOptionsId">waiverSetSigningOptionsId</param>
        /// <returns>returs int status</returns>
        public int Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                WaiverSetSigningOptionsDataHandler waiverSetSigningOptionsDataHandler = new WaiverSetSigningOptionsDataHandler();
                int deletedId = waiverSetSigningOptionsDataHandler.DeleteWaiverSetSigningOptions(waiverSetSigningOptionsDTO.Id);
                log.LogMethodExit(deletedId);
                return deletedId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting WaiverSetSigningOptionsDTO.", ex);
                log.Error("Throwing exception At Delete() :  " + ex.Message);
                log.LogVariableState("WaiverSetSigningOptionsId", waiverSetSigningOptionsDTO.Id);
                throw;
            }
        }
        public WaiverSetSigningOptionsDTO WaiverSetSigningOptionsDTO
        {
            get
            {
                return waiverSetSigningOptionsDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of WaiverSetSigningOptions
    /// </summary>
    public class WaiverSetSigningOptionsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public WaiverSetSigningOptionsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.waiverSetSigningOptionsList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="waiverSetSigningOptionsList"></param>
        /// <param name="executionContext"></param>
        public WaiverSetSigningOptionsListBL(ExecutionContext executionContext, List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsList)
        {
            log.LogMethodEntry(waiverSetSigningOptionsList, executionContext);
            this.waiverSetSigningOptionsList = waiverSetSigningOptionsList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the WaiverSetDetailSigningOptions list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<WaiverSetSigningOptionsDTO> GetWaiverSetSigningOptionsList(List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            WaiverSetSigningOptionsDataHandler waiverSetDetailSigningOptionsDataHandler = new WaiverSetSigningOptionsDataHandler(sqlTransaction);
            List<WaiverSetSigningOptionsDTO> returnValue = waiverSetDetailSigningOptionsDataHandler.GetWaiverSetSigningOptionsList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Saves or update the WaiverSetSigningOption
        /// </summary>
        public void SaveUpdateWaiverSetSigningOptionList(SqlTransaction sqlTransaction =null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (waiverSetSigningOptionsList != null)
                {
                    WaiverSetSigningOptionsDataHandler waiverSetSigningOptionsDataHandler = new WaiverSetSigningOptionsDataHandler(null);

                    foreach (WaiverSetSigningOptionsDTO waiverSetSigningOptionDto in waiverSetSigningOptionsList)
                    {
                        WaiverSetSigningOptionsBL waiverSetSigningOptionObj = new WaiverSetSigningOptionsBL(executionContext, waiverSetSigningOptionDto);
                        if (waiverSetSigningOptionDto.Id < 0)
                        {
                            waiverSetSigningOptionObj.Save(sqlTransaction);

                        }
                        else
                        {
                            if (waiverSetSigningOptionDto.Id >= 0 && waiverSetSigningOptionDto.IsChanged)
                            {
                                int status = waiverSetSigningOptionObj.Delete(sqlTransaction);
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
