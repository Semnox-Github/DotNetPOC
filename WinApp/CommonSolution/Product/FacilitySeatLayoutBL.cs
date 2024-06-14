/********************************************************************************************
 * Project Name - Facility Seat Layout Business Layer
 * Description  - Data object of Facility Seat Layout Business Layer class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        22-Feb-2019   Akshay Gulaganji          Created FacilitySeatLayoutBL Business Layer class, FacilitySeatLayoutListBL Class
 *2.70        29-Jun-2019   Akshay Gulaganji          Modified Save() method
 *2.80.0      26-Feb-2020   Girish Kundar           Modified : 3 Tier Changes for API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for FacilitySeatLayout class.
    /// </summary>
    public class FacilitySeatLayoutBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private FacilitySeatLayoutDTO facilitySeatLayoutDTO;

        ///<summary>
        /// Parameterized constructor with executionContext
        ///</summary>
        public FacilitySeatLayoutBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.facilitySeatLayoutDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the parameters
        /// </summary>
        /// <param name="facilitySeatLayoutDTO"></param>
        /// <param name="executionContext"></param>
        public FacilitySeatLayoutBL(ExecutionContext executionContext, FacilitySeatLayoutDTO facilitySeatLayoutDTO)
        {
            log.LogMethodEntry(facilitySeatLayoutDTO, executionContext);
            this.executionContext = executionContext;
            this.facilitySeatLayoutDTO = facilitySeatLayoutDTO;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Saves the FacilitySeatLayout
        /// Checks if the  LayoutId is not less than 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            FacilitySeatLayoutDataHandler facilitySeatLayoutDataHandler = new FacilitySeatLayoutDataHandler(sqlTransaction);
            if (facilitySeatLayoutDTO.IsActive)
            {
            if (facilitySeatLayoutDTO.LayoutId < 0)
            {
                facilitySeatLayoutDTO = facilitySeatLayoutDataHandler.InsertFacilitySeatLayout(facilitySeatLayoutDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                facilitySeatLayoutDTO.AcceptChanges();
                log.Debug(string.Format("LayoutId {0} has been stored from Save() ", facilitySeatLayoutDTO.LayoutId));
            }
            else
            {
                if (facilitySeatLayoutDTO.IsChanged)
                {
                    facilitySeatLayoutDTO = facilitySeatLayoutDataHandler.UpdateFacilitySeatLayout(facilitySeatLayoutDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilitySeatLayoutDTO.AcceptChanges();
                    log.Debug(string.Format("{0} Rows has been updated from Save() ", facilitySeatLayoutDTO.LayoutId));
                }
            }
            }
            else
            {
                facilitySeatLayoutDataHandler.DeleteFacilitySeatLayout(facilitySeatLayoutDTO.LayoutId);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the facilitySeatLayoutDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (facilitySeatLayoutDTO.FacilityId < 0)
            {
                validationErrorList.Add(new ValidationError("Facility", "FacilityId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Facility Id"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }
    /// <summary>
    /// Manages the list of FacilitySeatLayout
    /// </summary>
    public class FacilitySeatLayoutListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList;
        /// <summary>
        /// Parameterized constructor of FacilitySeatLayoutListBL class
        /// </summary>
        /// <param name="executionContext"></param>
        public FacilitySeatLayoutListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.facilitySeatLayoutDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and FacilitySeatLayoutDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="facilitySeatLayoutDTOList"></param>
        public FacilitySeatLayoutListBL(ExecutionContext executionContext, List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList)
        {
            log.LogMethodEntry(executionContext, facilitySeatLayoutDTOList);
            this.executionContext = executionContext;
            this.facilitySeatLayoutDTOList = facilitySeatLayoutDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the FacilitySeatLayoutDTO List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public List<FacilitySeatLayoutDTO> GetFacilitySeatLayoutDTOList(List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList;
            FacilitySeatLayoutDataHandler facilitySeatLayoutDataHandler = new FacilitySeatLayoutDataHandler(sqlTransaction);
            facilitySeatLayoutDTOList = facilitySeatLayoutDataHandler.GetFacilitySeatLayoutDTOList(searchParameters);
            log.LogMethodEntry(facilitySeatLayoutDTOList);
            return facilitySeatLayoutDTOList;
        }
        /// <summary>
        /// Saves the FacilitySeatLayoutList
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (facilitySeatLayoutDTOList != null)
                {
                    foreach (FacilitySeatLayoutDTO facilitySeatLayoutDTO in facilitySeatLayoutDTOList)
                    {
                        FacilitySeatLayoutBL facilitySeatLayoutBL = new FacilitySeatLayoutBL(executionContext, facilitySeatLayoutDTO);
                        facilitySeatLayoutBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
