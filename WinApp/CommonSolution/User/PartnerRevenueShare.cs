/********************************************************************************************
 * Project Name - PartnerRevenueShare
 * Description  - Bussiness logic of Partner Revenue Share
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        13-May-2019   Jagan Mohana Rao    Created
  *2.80       21-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    public class PartnerRevenueShare
    {
        private PartnerRevenueShareDTO partnerRevenueShareDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private PartnerRevenueShare(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="partnerRevenueShareDTO"></param>
        public PartnerRevenueShare(ExecutionContext executionContext, PartnerRevenueShareDTO partnerRevenueShareDTO)
        {
            log.LogMethodEntry(executionContext, partnerRevenueShareDTO);            
            this.executionContext = executionContext;
            this.partnerRevenueShareDTO = partnerRevenueShareDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="partnerRevenueShareId">Parameter of the type partnerRevenueShareId</param>
        public PartnerRevenueShare(int partnerRevenueShareId)
        {
            log.LogMethodEntry(partnerRevenueShareId);
            PartnerRevenueShareDataHandler partnerRevenueShareDataHandler = new PartnerRevenueShareDataHandler();
            this.partnerRevenueShareDTO = partnerRevenueShareDataHandler.GetPartnerRevenueShare(partnerRevenueShareId);
            log.LogMethodExit(partnerRevenueShareDTO);
        }

        /// <summary>
        /// Saves the Partner Revenue Share
        /// PartnerRevenueShare will be inserted if machine group is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            PartnerRevenueShareDataHandler partnerRevenueShareDataHandler = new PartnerRevenueShareDataHandler();
            if (partnerRevenueShareDTO.IsChanged == false &&
               partnerRevenueShareDTO.PartnerRevenueShareId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (partnerRevenueShareDTO.IsActive)
            {
                if (partnerRevenueShareDTO.PartnerRevenueShareId <= 0)
                {
                    int partnerRevenueShareId = partnerRevenueShareDataHandler.InsertPartnerRevenueShare(partnerRevenueShareDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    partnerRevenueShareDTO.PartnerRevenueShareId = partnerRevenueShareId;
                }
                else

                {
                    if (partnerRevenueShareDTO.IsChanged)
                    {
                        partnerRevenueShareDataHandler.UpdatePartnerRevenueShare(partnerRevenueShareDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        partnerRevenueShareDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (partnerRevenueShareDTO.PartnerRevenueShareId >= 0)
                {
                    partnerRevenueShareDataHandler.DeletePartnerRevenueShare(partnerRevenueShareDTO.PartnerRevenueShareId);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the partnerRevenueShareDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// get PartnerRevenueShareDTO Object
        /// </summary>
        public PartnerRevenueShareDTO GetPartnerRevenueShareDTO
        {
            get { return partnerRevenueShareDTO; }
        }
        /// <summary>
        /// set PartnerRevenueShareDTO Object        
        /// </summary>
        public PartnerRevenueShareDTO SetPartnerRevenueShareDTO
        {
            set { partnerRevenueShareDTO = value; }
        }

    }
    /// <summary>
    /// Manages the list of partner revenue share
    /// </summary>
    public class PartnerRevenueShareList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PartnerRevenueShareDTO> partnerRevenueShareDTOList = new List<PartnerRevenueShareDTO>();
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public PartnerRevenueShareList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public PartnerRevenueShareList(ExecutionContext executionContext, List<PartnerRevenueShareDTO> partnerRevenueShareDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, partnerRevenueShareDTOList);
            this.partnerRevenueShareDTOList = partnerRevenueShareDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Partner Revenue Share list
        /// </summary>
        public List<PartnerRevenueShareDTO> GetAllPartnerRevenueShare(List<KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            PartnerRevenueShareDataHandler partnerRevenueShareDataHandler = new PartnerRevenueShareDataHandler();
            log.LogMethodExit();
            return partnerRevenueShareDataHandler.GetPartnerRevenueShareList(searchParameters);            
        }

        /// <summary>
        /// This method is will return Sheet object for PartnerRevenueShare.
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="transactions"></param>
        /// <param name="gamePlays"></param>
        /// <returns></returns>
        public Sheet GetPartnerRevenueSheet(DateTime fromdate, DateTime todate, string transactions, List<string> gamePlays)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();
            PartnerRevenueShareDataHandler partnerRevenueShareDataHandler = new PartnerRevenueShareDataHandler();
            List<PartnerRevenueDTODefination> partnerRevenueDTODefinationList = partnerRevenueShareDataHandler.GetPartnerRevenueTable(fromdate, todate, transactions, gamePlays);

            PartnerRevenueDTODefination partnerRevenueDTODefination = new PartnerRevenueDTODefination(executionContext, "");
            ///Building headers from PartnerRevenueDTODefination
            partnerRevenueDTODefination.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (partnerRevenueDTODefinationList != null && partnerRevenueDTODefinationList.Any())
            {
                foreach (PartnerRevenueDTODefination partnerRevenueDefinationDTO in partnerRevenueDTODefinationList)
                {
                    partnerRevenueDTODefination.Configure(partnerRevenueDefinationDTO);

                    Row row = new Row();
                    partnerRevenueDTODefination.Serialize(row, partnerRevenueDefinationDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        /// <summary>
        /// This method is will return Sheet object for PartnerRevenueShare.
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="transactions"></param>
        /// <param name="gamePlays"></param>
        /// <returns></returns>
        public List<PartnerRevenueShareDTO> GetPartnerRevenuesTable(DateTime fromdate, DateTime todate, string transactions, List<string> gamePlays)
        {
            log.LogMethodEntry();
            PartnerRevenueShareDataHandler partnerRevenueShareDataHandler = new PartnerRevenueShareDataHandler();
            List<PartnerRevenueDTODefination> partnerRevenueDTODefinationList = partnerRevenueShareDataHandler.GetPartnerRevenueTable(fromdate, todate, transactions, gamePlays);
            PartnerRevenueDTODefination partnerRevenueDTODefination = new PartnerRevenueDTODefination(executionContext, "");
            foreach (PartnerRevenueDTODefination partnerRevenueDTO in partnerRevenueDTODefinationList)
            {
                if (string.IsNullOrWhiteSpace(partnerRevenueDTO.RevenueSharePercentage))
                {
                    partnerRevenueDTO.RevenueSharePercentage = "0.0";
                }
                if (string.IsNullOrWhiteSpace(partnerRevenueDTO.MinimumGuarantee))
                {
                    partnerRevenueDTO.MinimumGuarantee = "0.0";
                }

                PartnerRevenueShareDTO partnerRevenueShareDTO = new PartnerRevenueShareDTO(partnerRevenueDTO.Month, partnerRevenueDTO.Partner, partnerRevenueDTO.MachineGroup,
                                                    partnerRevenueDTO.AgentGroupName, partnerRevenueDTO.TotalAmount, Convert.ToDouble(partnerRevenueDTO.RevenueSharePercentage),
                                                    partnerRevenueDTO.ShareAmount, Convert.ToDouble(partnerRevenueDTO.MinimumGuarantee), partnerRevenueDTO.FinalAmount);
                partnerRevenueShareDTOList.Add(partnerRevenueShareDTO);

            }
            return partnerRevenueShareDTOList;
        }

        /// <summary>
        /// Saves the  list of partnerRevenueShareDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (partnerRevenueShareDTOList == null ||
                partnerRevenueShareDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < partnerRevenueShareDTOList.Count; i++)
            {
                var partnerRevenueShareDTO = partnerRevenueShareDTOList[i];
                if (partnerRevenueShareDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PartnerRevenueShare partnerRevenueShare = new PartnerRevenueShare(executionContext, partnerRevenueShareDTO);
                    partnerRevenueShare.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving partnerRevenueShareDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("partnerRevenueShareDTO", partnerRevenueShareDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}